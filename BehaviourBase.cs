using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib;

public class BehaviourBase : MonoBehaviour
{
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}

    // TODO: Add linked parts to behaviour to allow logic across custom parts
}