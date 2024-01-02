using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using GearLib.Behaviours;
using GearLib.Behaviours.Fields;
using GearLib.Data.Fields;
using LibCpp2IL;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.GearBlocks.UI;
using SmashHammer.Input;
using UnityEngine;
using static SmashHammer.Input.InputUtils;

namespace GearLib.Data;

sealed class SaveData
{
    private static readonly Lazy<SaveData> lazy = new Lazy<SaveData>(() => new SaveData());
    public Dictionary<ushort, ConstructionData> constructions { get; set; } = new Dictionary<ushort, ConstructionData>();
    public static SaveData Instance { get { return lazy.Value; } }

    public void Save()
    {
        GameObject scene = GameObject.Find("/MainGui/Save Scene");
        SaveLoadScreen save_component = scene.GetComponent<SaveLoadScreen>();
        if (save_component.currentSaveData == null) return;
        Plugin.Log.LogInfo("Saving mod data...");

        string save_folder = save_component.currentSaveData.SaveFolder;

        GameObject construction_pool = construction_pool = GameObject.Find("/ConstructionsPool");
        foreach (Il2CppSystem.Object construction_object in construction_pool.transform)
        {
            Transform construction_transform = construction_object.Cast<Transform>();
            if (!construction_transform.gameObject.activeSelf) continue;
            ConstructionData construction_data = new ConstructionData();

            Construction construction = construction_transform.GetComponent<Construction>();

            foreach (PartDescriptor part_descriptor in construction.Parts)
            {
                PartData part_data = new PartData();
                BehaviourBase[] modded_behaviours = part_descriptor.GetComponents<BehaviourBase>();
                if (modded_behaviours.Length == 0) continue;

                foreach (BehaviourBase modded_behaviour in modded_behaviours)
                {
                    BehaviourData behaviour_data = new BehaviourData();
                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(FloatField)))) 
                        behaviour_data.AddTweakable(field.Name, new FloatData(Convert.ToSingle(field.GetValue(modded_behaviour))));

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(BooleanField)))) 
                        behaviour_data.AddTweakable(field.Name, new BooleanData((bool)field.GetValue(modded_behaviour)));

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(IntField)))) 
                        behaviour_data.AddTweakable(field.Name, new IntData(Convert.ToInt32(field.GetValue(modded_behaviour))));

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(StringField)))) 
                        behaviour_data.AddTweakable(field.Name, new StringData(Convert.ToString(field.GetValue(modded_behaviour))));

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(InputField)))) 
                        behaviour_data.AddTweakable(field.Name, new InputData((InputAction)field.GetValue(modded_behaviour)));

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance).Where(fi => fi.IsDefined(typeof(JoystickField))))
                        behaviour_data.AddTweakable(field.Name, new JoystickData((JoystickAxis)field.GetValue(modded_behaviour)));

                    // TODO: Need a iterator so multiple behaviours on a single modded part dont conflict
                    part_data.AddBehaviour(0, behaviour_data);
                }
                
                construction_data.AddPart(part_descriptor.ID, part_data);
            }

            AddConstruction(construction.ID, construction_data);
        }

        string json = JsonSerializer.Serialize(this);
        string file_path = $"{Application.persistentDataPath}/SavedScenes/{save_folder}/gearlib.json";
        File.WriteAllText(file_path, json);
        Plugin.Log.LogInfo("Saved mod data!");

        Reset();
    }

    public void Load()
    {        
        GameObject scene_obj = GameObject.Find("/MainGui/Load Scene");
        SaveLoadScreen load_component = scene_obj.GetComponent<SaveLoadScreen>();
        if (load_component.currentSaveData == null) return;
        Plugin.Log.LogInfo("Loading mod data...");

        string save_folder = load_component.currentSaveData.SaveFolder;
        string file_path = $"{Application.persistentDataPath}/SavedScenes/{save_folder}/gearlib.json";
        if (!File.Exists(file_path)) return;
        string json = File.ReadAllText(file_path);

        SaveData loaded_save = JsonSerializer.Deserialize<SaveData>(json);

        GameObject construction_pool = construction_pool = GameObject.Find("/ConstructionsPool");
        foreach (Il2CppSystem.Object construction_object in construction_pool.transform)
        {
            Transform construction_transform = construction_object.Cast<Transform>();
            if (!construction_transform.gameObject.activeSelf) continue;

            Construction construction = construction_transform.GetComponent<Construction>();

            foreach (PartDescriptor part_descriptor in construction.Parts)
            {
                BehaviourBase[] modded_behaviours = part_descriptor.GetComponents<BehaviourBase>();
                if (modded_behaviours.Length == 0) continue;

                foreach (BehaviourBase modded_behaviour in modded_behaviours)
                {
                    ConstructionData construction_data = loaded_save.constructions.GetOrDefault(construction.ID);
                    if (construction_data == null) { Plugin.Log.LogError("Saved construction not found for modded part!"); continue; }

                    PartData part_data = construction_data.parts.GetOrDefault(part_descriptor.ID);
                    if (part_data == null) { Plugin.Log.LogError("Saved part not found for modded part!"); continue; }
                    
                    BehaviourData behaviour_data = part_data.behaviours.GetOrDefault((ushort)0);
                    if (part_data == null) { Plugin.Log.LogError("Saved behaviour not found for modded part!"); continue; }

                    foreach (FieldInfo field in modded_behaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)) 
                    {
                        if (field.IsDefined(typeof(FloatField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            
                            ((FloatTweakable)modded_behaviour.tweakables_dict[field.Name]).value = ((JsonElement)tweakable).GetProperty("value").GetSingle();
                        }
                        else if (field.IsDefined(typeof(BooleanField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            
                            ((BooleanTweakable)modded_behaviour.tweakables_dict[field.Name]).value = ((JsonElement)tweakable).GetProperty("value").GetBoolean();
                        }
                        else if (field.IsDefined(typeof(InputField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            
                            InputData input_data = new InputData() {
                                primary_key = (KeyCode)((JsonElement)tweakable).GetProperty("primary_key").GetInt32(),
                                primary_key_modifier = (KeyCode)((JsonElement)tweakable).GetProperty("primary_key_modifier").GetInt32(),
                                secondary_key = (KeyCode)((JsonElement)tweakable).GetProperty("secondary_key").GetInt32(),
                                secondary_key_modifier = (KeyCode)((JsonElement)tweakable).GetProperty("secondary_key_modifier").GetInt32()
                            };
                            ((InputActionTweakable)modded_behaviour.tweakables_dict[field.Name]).value = input_data.GetInputAction();
                        }
                        else if (field.IsDefined(typeof(IntField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            
                            ((IntTweakable)modded_behaviour.tweakables_dict[field.Name]).value = ((JsonElement)tweakable).GetProperty("value").GetInt32();
                        }
                        else if (field.IsDefined(typeof(JoystickField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            {
                                JoystickAxis joystick_axis = ((JoystickAxisTweakable)modded_behaviour.tweakables_dict[field.Name]).value;
                                // TODO: Fix axis binding for Joystick
                                // Axis axis = new Axis(Enum.Parse<AxisCode>(((JsonElement)tweakable).GetProperty("axis_code").GetInt32().ToString()));
                                // joystick_axis.axis.axisCode = axis.axisCode;
                                // joystick_axis.axis.axisName = axis.axisName;
                                joystick_axis.deadzone = (float)((JsonElement)tweakable).GetProperty("deadzone").GetSingle();
                                joystick_axis.gamma = (float)((JsonElement)tweakable).GetProperty("gamma").GetSingle();
                                joystick_axis.centre = (float)((JsonElement)tweakable).GetProperty("centre").GetSingle();
                            }
                        }
                        else if (field.IsDefined(typeof(StringField)))
                        {
                            object tweakable;
                            behaviour_data.tweakables.TryGetValue(field.Name, out tweakable);
                            if (tweakable == null) { Plugin.Log.LogError("Saved field not found for modded part!"); continue; }
                            
                            ((StringTweakable)modded_behaviour.tweakables_dict[field.Name]).value = ((JsonElement)tweakable).GetProperty("value").GetString();
                        }
                    }
                }
            }
        }
        Plugin.Log.LogInfo("Loaded!");
    }

    
    void Reset()
    {
        constructions = new Dictionary<ushort, ConstructionData>();
    }

    void AddConstruction(ushort id, ConstructionData data) 
    {
        constructions.Add(id, data);
    }
}