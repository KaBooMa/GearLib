using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GearLib.Core.BehaviourFields;
using Il2CppInterop.Runtime.Injection;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.Tweakables;
using UnityEngine;

namespace GearLib;

public class BehaviourBase : PartBehaviourBase
{
    public override string Name { get; }
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    public new Il2CppSystem.Collections.Generic.List<TweakableBase> Tweakables = new Il2CppSystem.Collections.Generic.List<TweakableBase>();
    
    public BehaviourBase() : base()
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(IntField))) 
        {
            Tweakables.Add((IntField)field.GetValue(this));
            base.Tweakables.Add((IntField)field.GetValue(this));
        }

        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(StringField))) 
        {
            Tweakables.Add((StringField)field.GetValue(this));
            base.Tweakables.Add((StringField)field.GetValue(this));
        }

        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(JoystickField))) 
        {
            Tweakables.Add((JoystickField)field.GetValue(this));
            base.Tweakables.Add((JoystickField)field.GetValue(this));
        }

        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(InputField))) 
        {
            Tweakables.Add((InputField)field.GetValue(this));
            base.Tweakables.Add((InputField)field.GetValue(this));
        }

        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(FloatField))) 
        {
            Tweakables.Add((FloatField)field.GetValue(this));
            base.Tweakables.Add((FloatField)field.GetValue(this));
        }

        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(BooleanField))) 
        {
            Tweakables.Add((BooleanField)field.GetValue(this));
            base.Tweakables.Add((BooleanField)field.GetValue(this));
        }
    }

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