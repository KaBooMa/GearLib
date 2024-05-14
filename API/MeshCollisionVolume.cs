using SmashHammer.Core;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.Physics;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GearLib.API;

class MeshCollisionVolume : BoxCollisionVolume
{
    MeshCollider mesh_collider;
    MeshCollider mesh_collider_concave;
    PartDescriptor descriptor;

    public new void Awake()
    {
        foreach (BoxCollider collider in gameObject.GetComponents<BoxCollider>())
            Destroy(collider);
        foreach (MeshCollider collider in gameObject.GetComponents<MeshCollider>())
            Destroy(collider);

        MeshFilter mesh_filter = gameObject.transform.parent.GetComponentInChildren<MeshFilter>();
        // mesh = mesh_filter.sharedMesh;

        mesh_collider = gameObject.AddComponent<MeshCollider>();
        mesh_collider.convex = true;
        mesh_collider.sharedMesh = mesh_filter.sharedMesh;

        mesh_collider_concave = gameObject.AddComponent<MeshCollider>();
        mesh_collider_concave.convex = false;
        mesh_collider_concave.sharedMesh = mesh_filter.sharedMesh;

        descriptor = gameObject.transform.parent.GetComponent<PartDescriptor>();
        gameObject.name = "Convex";

        PartColliderRegistry.Register(mesh_collider, descriptor);
        PartColliderRegistry.Register(mesh_collider_concave, descriptor);
    }

    public void FixedUpdate()
    {
        if (!descriptor.ParentConstruction)
            return;

        // Plugin.Log.LogWarning((gameObject.layer ) + " | " + mesh_collider.name);
        if ((gameObject.layer == 27 || gameObject.layer == 28) && gameObject.name == "Convex") // Frozen layer
        {
            mesh_collider.enabled = false;
            mesh_collider_concave.enabled = true;
            PartColliderRegistry.Unregister(mesh_collider);
            PartColliderRegistry.Register(mesh_collider_concave, descriptor);
            gameObject.name = "Concave";
            Collider = mesh_collider;

            descriptor.parentComposite.Rigidbody.mass = descriptor.ParentConstruction.mass;
            descriptor.parentComposite.Rigidbody.ResetInertiaTensor();
        }
        else if (gameObject.layer != 27 && gameObject.layer != 28 && gameObject.name == "Concave")
        {
            mesh_collider_concave.enabled = false;
            mesh_collider.enabled = true;
            PartColliderRegistry.Unregister(mesh_collider_concave);
            PartColliderRegistry.Register(mesh_collider, descriptor);
            gameObject.name = "Convex";
            Collider = mesh_collider;

            descriptor.parentComposite.Rigidbody.mass = descriptor.ParentConstruction.mass;
            descriptor.parentComposite.Rigidbody.ResetInertiaTensor();
        }
    }

    public override Bounds GetBounds()
    {
        return mesh_collider.bounds;
    }
}