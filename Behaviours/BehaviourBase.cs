using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GearLib.Behaviours.Fields;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.Behaviours;

public class BehaviourBase : PartBehaviourBase
{
    public override string Name { get; }
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    
    private GCHandle tweakables_gc;
    private GCHandle dataChanels_gc;

    new void Awake()
    {
        tweakables_gc = GCHandle.Alloc(Tweakables, GCHandleType.Normal);
        dataChanels_gc = GCHandle.Alloc(dataChannels, GCHandleType.Normal);
        base.Awake();
    }

    public BehaviourBase() : base()
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(BooleanField))) Tweakables.Add((BooleanField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(FloatField))) Tweakables.Add((FloatField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(InputField))) Tweakables.Add((InputField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(IntField))) Tweakables.Add((IntField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(JoystickField))) Tweakables.Add((JoystickField)field.GetValue(this));
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.FieldType == typeof(StringField))) Tweakables.Add((StringField)field.GetValue(this));
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