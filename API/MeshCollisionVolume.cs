using System;
using Il2CppInterop.Runtime.Injection;
using SmashHammer.Core;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.Physics;
using UnityEngine;

namespace GearLib.API;

class MeshCollisionVolume : CollisionVolumeBase
{
    public MeshCollider mesh_collider;
    PartDescriptor descriptor;
    MeshCollider mesh_collider_concave;

    public MeshCollisionVolume(IntPtr ptr) : base(ptr) { }
    public MeshCollisionVolume() : base(ClassInjector.DerivedConstructorPointer<MeshCollisionVolume>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public void Awake()
    {
        foreach (MeshCollider collider in gameObject.GetComponents<MeshCollider>())
        {
            PartColliderRegistry.Unregister(collider);
            Destroy(collider);
        }

        PhysicMaterial mat = new PhysicMaterial();
        material = mat;

        MeshFilter mesh_filter = gameObject.transform.parent.GetComponentInChildren<MeshFilter>();

        mesh_collider = gameObject.AddComponent<MeshCollider>();
        mesh_collider.convex = true;
        mesh_collider.sharedMesh = mesh_filter.sharedMesh;
        mesh_collider.sharedMaterial = mat;
        Collider = mesh_collider;

        mesh_collider_concave = gameObject.AddComponent<MeshCollider>();
        mesh_collider_concave.convex = false;
        mesh_collider_concave.sharedMesh = mesh_filter.sharedMesh;

        gameObject.name = "Convex";

        descriptor = gameObject.transform.parent.GetComponent<PartDescriptor>();

        PartColliderRegistry.Register(mesh_collider, descriptor);
        PartColliderRegistry.Register(mesh_collider_concave, descriptor);

    }

    public override float Volume { 
        get { 
            return 1f; 
        } 
    }

    // Probably need to implement this method...
    public override void Clone(CollisionVolumeBase otherCollisionVolume)
    {
        // Plugin.Log.LogWarning("Clone");
    }

    // Probably need to implement this method...
    public override bool Contains(Vector3 point, float bias = 0)
    {
        // Plugin.Log.LogWarning("Contains");
        return true;
    }

    // Probably need to implement this method...
    public override bool Intersects(IConditional<Collider> colliderConditional, int layerMask = -5, float bias = 0)
    {
        // Plugin.Log.LogWarning("Intersects1");
        return false;
    }

    // Probably need to implement this method...
    public override bool Intersects(int layerMask = -5, float bias = 0)
    {
        // Plugin.Log.LogWarning("Intersects2");
        return false;
    }

    // Probably need to implement this method...
    public override void Refresh()
    {
        // Plugin.Log.LogWarning("Refreshed");
    }

    public override Bounds GetBounds()
    {
        return mesh_collider.bounds;
    }

    public void FixedUpdate()
    {
        if (!descriptor.ParentConstruction)
            return;

        if ((gameObject.layer == 27 || gameObject.layer == 28) && gameObject.name == "Convex") // Frozen layer
        {
            mesh_collider.enabled = false;
            mesh_collider_concave.enabled = true;
            PartColliderRegistry.Unregister(mesh_collider);
            PartColliderRegistry.Register(mesh_collider_concave, descriptor);
            gameObject.name = "Concave";
        }
        else if (gameObject.layer != 27 && gameObject.layer != 28 && gameObject.name == "Concave")
        {
            mesh_collider_concave.enabled = false;
            mesh_collider.enabled = true;
            PartColliderRegistry.Unregister(mesh_collider_concave);
            PartColliderRegistry.Register(mesh_collider, descriptor);
            gameObject.name = "Convex";
        }
    }
}