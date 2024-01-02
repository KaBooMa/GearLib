using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GearLib.Behaviours.Fields;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;
using UnityEngine;

namespace GearLib.Behaviours;

public class BehaviourBase : PartBehaviourBase
{
    public override string Name { get; }
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    public Dictionary<string, TweakableBase> tweakables_dict = new Dictionary<string, TweakableBase>();
    private GCHandle tweakables_gc;
    private GCHandle dataChanels_gc;

    new void Awake()
    {
        base.Awake();
    }

    public BehaviourBase() : base()
    {
        new JoystickAxis();
        tweakables_gc = GCHandle.Alloc(Tweakables, GCHandleType.Normal);
        dataChanels_gc = GCHandle.Alloc(dataChannels, GCHandleType.Normal);
        ScanForTweakables();
    }

    void ScanForTweakables()
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(FloatField)))) 
        {
            FloatField field_data = field.GetCustomAttribute<FloatField>();
            FloatTweakable tweakable = CreateFloatTweakable(field_data.label, field_data.minimum_value, field_data.maximum_value, field_data.initial_value, field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(BooleanField)))) 
        {
            BooleanField field_data = field.GetCustomAttribute<BooleanField>();
            BooleanTweakable tweakable = CreateBooleanTweakable(field_data.label, field_data.initial_value, field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(InputField)))) 
        {
            InputField field_data = field.GetCustomAttribute<InputField>();
            InputActionTweakable tweakable = CreateInputActionTweakable(field_data.label, new InputAction(), description: field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(IntField)))) 
        {
            IntField field_data = field.GetCustomAttribute<IntField>();
            IntTweakable tweakable = CreateIntTweakable(field_data.label, field_data.minimum_value, field_data.maximum_value, field_data.initial_value, field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(JoystickField)))) 
        {
            JoystickField field_data = field.GetCustomAttribute<JoystickField>();
            JoystickAxisTweakable tweakable = CreateJoystickAxisTweakable(field_data.label, new JoystickAxis(), description: field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(StringField)))) 
        {
            StringField field_data = field.GetCustomAttribute<StringField>();
            StringTweakable tweakable = CreateStringTweakable(field_data.label, field_data.multiple_lines, field_data.initial_value, field_data.tooltip_text);
            tweakables_dict.Add(field.Name, tweakable);
        }
    }

    void Update()
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(FloatField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            FloatTweakable float_tweakable = (FloatTweakable)tweakable;
            if (Convert.ToSingle(float_tweakable) != (float)field.GetValue(this))
                field.SetValue(this, Convert.ToSingle(float_tweakable));
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(BooleanField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            BooleanTweakable boolean_tweakable = (BooleanTweakable)tweakable;
            if (Convert.ToBoolean(boolean_tweakable) != (bool)field.GetValue(this))
                field.SetValue(this, Convert.ToBoolean(boolean_tweakable));
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(InputField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            InputActionTweakable input_tweakable = (InputActionTweakable)tweakable;
            if (input_tweakable.value != (InputAction)field.GetValue(this))
                field.SetValue(this, input_tweakable.value);
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(IntField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            IntTweakable int_tweakable = (IntTweakable)tweakable;
            if (Convert.ToInt32(int_tweakable) != (int)field.GetValue(this))
                field.SetValue(this, Convert.ToInt32(int_tweakable));
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(JoystickField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            JoystickAxisTweakable joystick_tweakable = (JoystickAxisTweakable)tweakable;
            if (joystick_tweakable.value != ((JoystickAxis)field.GetValue(this)))
                {field.SetValue(this, joystick_tweakable.value); Plugin.Log.LogError("RAN");}
        }
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(StringField)))) 
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            StringTweakable string_tweakable = (StringTweakable)tweakable;
            if (Convert.ToString(string_tweakable) != (string)field.GetValue(this))
                field.SetValue(this, Convert.ToString(string_tweakable));
        }
    }

    // TODO: Implement or delete
    // Not working as of now, DNU! Planned to convert to generic for update copy/paste
    // private void VerifyTweakables<T1, T2>() where T1 : Attribute where T2 : TweakableBase
    // {
    //     foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(T1)))) 
    //     {
    //         TweakableBase tweakable;
    //         tweakables_dict.TryGetValue(field.Name, out tweakable);
    //         Plugin.Log.LogInfo(tweakable.Cast<T2>());
    //         if (Convert.ToSingle(tweakable.Cast<T2>()) != (float)field.GetValue(this))
    //         {
    //             field.SetValue(this, Convert.ToSingle(tweakable.Cast<T2>()));
    //         }
    //     }
    // }
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
        foreach (AttachmentBase attachment in descriptor.Attachments.associatedAttachments)
        {
            if (attachment.ownerPartPointGrid.name.Replace("PointGrid_", "") == attachment_name || 
                attachment.connectedPartPointGrid.name.Replace("PointGrid_", "") == attachment_name) 
                return attachment;
        }

        return null;
    }
}