using System.Collections.Generic;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib;

public class BehaviourBase : MonoBehaviour
{
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    
    public Dictionary<string, GameObject> linked_parts { get {
        Dictionary<string, GameObject> linked_parts = new Dictionary<string, GameObject>();
        foreach (PartLinkNode source_link in gameObject.transform.GetComponentsInChildren<PartLinkNode>()) 
        {
            foreach (PartLinkNode target_link in source_link.LinkedNodes)
            {
                if (target_link.Part) linked_parts.Add(source_link.transform.gameObject.name.Replace("LinkNode_", ""), target_link.Part.transform.gameObject);
            }
        }
        return linked_parts;
    }}

    public List<GameObject> GetLinkedParts(string link_name)
    {
        foreach (PartLinkNode source_node in gameObject.transform.GetComponentsInChildren<PartLinkNode>())
        {
            if (source_node.transform.gameObject.name.Replace("LinkNode_", "") == link_name) 
            {
                List<GameObject> linked_parts = new List<GameObject>();
                foreach (PartLinkNode target_node in source_node.LinkedNodes)
                {
                    linked_parts.Add(target_node.Part.transform.gameObject);
                }
                
                return linked_parts;
            }
        }

        return null;
    }

    public AttachmentBase GetAttachment(string attachment_name)
    {
        foreach (Il2CppSystem.Collections.Generic.KeyValuePair<IPart, AttachmentBase> attachment in descriptor.Attachments.ownedAttachments)
        {
            if (attachment.value.ownerPartPointGrid.gameObject.name.Replace("PointGrid_", "") == attachment_name) return attachment.value;
        }

        return null;
    }
}