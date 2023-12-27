using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GearLib.Core.BehaviourFields;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib;

public class BehaviourBase : PartBehaviourBase
{
    public unsafe override string Name { get; }
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    
    public BehaviourBase() : base()
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(IntField))) Tweakables.Add((IntField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(StringField))) Tweakables.Add((StringField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(JoystickField))) Tweakables.Add((JoystickField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(InputField))) Tweakables.Add((InputField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(FloatField))) Tweakables.Add((FloatField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(BooleanField))) Tweakables.Add((BooleanField)field.GetValue(this));
    }

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