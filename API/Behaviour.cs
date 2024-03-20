using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using GearLib.API.Fields;
using Newtonsoft.Json;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;
using UnityEngine;

namespace GearLib.API;

public class BehaviourBase : PartBehaviourActivatableBase
{
    public override string Name { get; }
    public PartDescriptor descriptor { get { return gameObject.GetComponent<PartDescriptor>(); }}
    public Composite composite { get { return transform.parent.GetComponent<Composite>(); }}
    public Rigidbody rigidBody { get { return composite.GetComponent<Rigidbody>(); }}
    public Construction construction { get { return descriptor.ParentConstruction; }}
    public Dictionary<string, TweakableBase> tweakables_dict = new Dictionary<string, TweakableBase>();
    private GCHandle tweakables_gc;
    private GCHandle dataChanels_gc;

    public BehaviourBase() : base()
    {
        tweakables_gc = GCHandle.Alloc(Tweakables, GCHandleType.Normal);
        dataChanels_gc = GCHandle.Alloc(dataChannels, GCHandleType.Normal);
        ScanForTweakables();
    }
    
    public override void LoadJsonProperty(JsonReader reader, string propertyName)
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(IField)))) 
        {
            if (field.Name != propertyName) continue;
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(field.Name, out tweakable);
            if (field.IsDefined(typeof(IntField)))
                ((IntTweakable)tweakable).Value = Il2CppSystem.Convert.ToInt32(reader.ReadAsInt32());
            else if (field.IsDefined(typeof(BooleanField)))
                ((BooleanTweakable)tweakable).Value = Convert.ToBoolean(reader.ReadAsString());
            else if (field.IsDefined(typeof(FloatField)))
                ((FloatTweakable)tweakable).Value = Convert.ToSingle(reader.ReadAsString());
            else if (field.IsDefined(typeof(StringField)))
                ((StringTweakable)tweakable).Value = Il2CppSystem.Convert.ToString(reader.ReadAsString());
            else if (field.IsDefined(typeof(JoystickField)))
            {
                JoystickAxis joystick_axis = new JoystickAxis();
                joystick_axis.Deserialize(reader);
                ((JoystickAxisTweakable)tweakable).Value = joystick_axis;
            }
            else if (field.IsDefined(typeof(InputField)))
            {
                InputAction input_action = new InputAction();
                input_action.Deserialize(reader);
                ((InputActionTweakable)tweakable).Value = input_action;
            }
        }
    }

    public override void SaveJsonProperties(JsonWriter writer)
    {
        foreach (FieldInfo field in GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(IField))))
        {
            if (field.IsDefined(typeof(IntField)) || 
                field.IsDefined(typeof(BooleanField)) || 
                field.IsDefined(typeof(FloatField)) || 
                field.IsDefined(typeof(StringField)) ||
                field.IsDefined(typeof(InputField)) ||
                field.IsDefined(typeof(JoystickField))) 
                    writer.WritePropertyName(field.Name);

            if (field.IsDefined(typeof(IntField))) writer.WriteValue((int)field.GetValue(this));
            if (field.IsDefined(typeof(BooleanField))) writer.WriteValue((bool)field.GetValue(this));
            if (field.IsDefined(typeof(FloatField))) writer.WriteValue((float)field.GetValue(this));
            if (field.IsDefined(typeof(StringField))) writer.WriteValue((string)field.GetValue(this));
            if (field.IsDefined(typeof(InputField))) 
            {
                InputAction field_data = (InputAction)field.GetValue(this);
                if (field_data != null)
                    field_data.Serialize(writer);
            }
            if (field.IsDefined(typeof(JoystickField))) 
            {
                JoystickAxis field_data = (JoystickAxis)field.GetValue(this);
                if (field_data != null)
                    field_data.Serialize(writer);
            }
        }
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
                field.SetValue(this, joystick_tweakable.value);
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