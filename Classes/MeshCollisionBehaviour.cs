using Il2CppInterop.Runtime.Injection;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.Classes;

class MeshCollisionBehaviour : MonoBehaviour
{
    bool is_frozen = false;
    PartDescriptor descriptor;

    static MeshCollisionBehaviour()
    {
        ClassInjector.RegisterTypeInIl2Cpp<MeshCollisionBehaviour>();
    }
    
    public void Awake()
    {
        foreach(MonoBehaviour behaviour in GetComponentsInChildren<MonoBehaviour>())
            behaviour.enabled = true;

            
        descriptor = GetComponent<PartDescriptor>();
    }

    void FixedUpdate()
    {
        if (!descriptor.ParentConstruction)
            return;
            
        if (descriptor.ParentConstruction.IsFrozen)
        {
            is_frozen = true;
            return;
        }
        else if (is_frozen)
        {
            DisableCollisions(); // Disables collision when unfrozen
            is_frozen = false;
        }
    }


    // Disable collision with all parts
    void DisableCollisions()
    {
        foreach (PartDescriptor other_desc in descriptor.ParentConstruction.Parts)
        {
            Collider collider = descriptor.GetComponentInChildren<Collider>();
            foreach (Collider other_collider in other_desc.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(other_collider, collider, true);
            }
        }
    }
}