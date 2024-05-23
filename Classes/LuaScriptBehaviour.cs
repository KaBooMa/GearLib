using System;
using System.Collections.Generic;
using GearLib.API.Lua;
using GearLib.Utils;
using Il2CppInterop.Runtime.Injection;
using MoonSharp.Interpreter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;

namespace GearLib.Classes;

class LuaScriptBehaviour : API.Behaviour
{    
    public override string Name { get; }
    public Script script = new Script();
    LuaVariables lua_vars;
    bool instanced = false;
    bool is_frozen = false;

    static LuaScriptBehaviour() {
        ClassInjector.RegisterTypeInIl2Cpp<LuaScriptBehaviour>();
    }
    
    public LuaScriptBehaviour() : base()
    {
    }

    new void Awake()
    {
        base.Awake();
        SetupTweakables();
    }

    void FixedUpdate() 
    {
        if (!instanced)
        {
            if (descriptor.isInstantiated && gameObject.scene.name != "DontDestroyOnLoad")
            {
                instanced = true;

                // Run our lua to init all declarations, etc.
                if (GearthonLoader.scripts[descriptor.AssetGUID] != null)
                {
                    // Give lua access to part data and modder-defined tweakables
                    lua_vars = new LuaVariables(this);
                    lua_vars._UpdateVars();

                    DynValue script_string = script.LoadString(GearthonLoader.scripts[descriptor.AssetGUID]);
                    script.Call(script_string);
                }
            }
        }
        else
        {
            if (!descriptor.ParentConstruction)
                return;
                
            if (!descriptor.ParentConstruction.IsFrozen)
            {
                if (is_frozen)
                {
                    is_frozen = false;

                    // Call the unfrozen function if exists
                    DynValue unfrozen_function = script.Globals.Get("Unfrozen");
                    if (unfrozen_function.IsNotNil())
                    {
                        // Give lua access to updated part data
                        lua_vars._UpdateVars();
                        script.Call(unfrozen_function);

                    }
                }
                else
                {
                    // Call the update function if exists
                    DynValue update_function = script.Globals.Get("Update");
                    if (update_function.IsNotNil())
                    {
                        // Give lua access to updated part data
                        lua_vars._UpdateTweakables();
                        script.Call(update_function);
                    }
                }
            }
            else
            {
                if (!is_frozen)
                {
                    is_frozen = true;

                    // Call the frozen function if exists
                    DynValue frozen_function = script.Globals.Get("Frozen");
                    if (frozen_function.IsNotNil())
                    {
                        // Give lua access to updated part data
                        lua_vars._UpdateVars();
                        script.Call(frozen_function);
                    }
                }
            }
        }
    }

    public void SetupTweakables() {
        ulong uid = ulong.Parse(name.Replace("(Clone)", ""));
        JArray tweakables_array;
        GearthonLoader.tweakables.TryGetValue(uid, out tweakables_array);
        if (GearthonLoader.tweakables == null)
            return;

        for (int i2 = 0; i2 < tweakables_array.Count; i2++)
        {
            JToken tweakable_data = tweakables_array[i2].Cast<JToken>();
            string field_name = (string)tweakable_data["name"];
            string type = (string)tweakable_data["type"];
            TweakableBase tweakable;
            if (type == "int")
            {
                tweakable = CreateIntTweakable(
                    label: (string)tweakable_data["label"],
                    description: (string)tweakable_data["description"],
                    min: (int)tweakable_data["minimum"],
                    max: (int)tweakable_data["maximum"],
                    initVal: (int)tweakable_data["initial_value"]
                );
            } else if (type == "float")
            {
                tweakable = CreateFloatTweakable(
                    label: (string)tweakable_data["label"],
                    description: (string)tweakable_data["description"],
                    min: (float)tweakable_data["minimum"],
                    max: (float)tweakable_data["maximum"],
                    initVal: (float)tweakable_data["initial_value"]
                );
            } else if (type == "string")
            {
                tweakable = CreateStringTweakable(
                    label: (string)tweakable_data["label"],
                    description: (string)tweakable_data["description"],
                    multiLine: (bool)tweakable_data["multiline"],
                    initVal: (string)tweakable_data["initial_value"]
                );
            } else if (type == "boolean")
            {
                tweakable = CreateBooleanTweakable(
                    label: (string)tweakable_data["label"],
                    description: (string)tweakable_data["description"],
                    initVal: (bool)tweakable_data["initial_value"]
                );
            } 
            // TODO: FIX JOYSTICKS. CURRENTLY CCTOR ERROR WITH new JoystickAxis()
            // else if (type == "joystick")
            // {
            //     tweakable = CreateJoystickAxisTweakable(
            //         label: (string)tweakable_data["label"],
            //         description: (string)tweakable_data["description"],
            //         initVal: new JoystickAxis()
            //     );
            // }
            else if (type == "inputaction")
            {
                tweakable = CreateInputActionTweakable(
                    label: (string)tweakable_data["label"],
                    description: (string)tweakable_data["description"],
                    initVal: new InputAction()
                );
            }
            else
            {
                Plugin.Log.LogWarning($"Tried to load invalid tweakable!!! [{field_name}]");
                continue;
            }
            
            tweakables_dict.Add(field_name, tweakable);
        }
    }

    /// <summary>
    /// Used by GearBlocks for loading your Part properties.
    /// </summary>
    public override void LoadJsonProperty(JsonReader reader, string propertyName)
    {
        foreach (KeyValuePair<string, TweakableBase> pair in tweakables_dict)
        {
            if (pair.Key != propertyName) continue;
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(pair.Key, out tweakable);
            if (tweakable is IntTweakable)
            {
                ((IntTweakable)tweakable).Value = Il2CppSystem.Convert.ToInt32(reader.ReadAsInt32());
            }
            else if (tweakable is BooleanTweakable)
                ((BooleanTweakable)tweakable).Value = Convert.ToBoolean(reader.ReadAsString());
            else if (tweakable is FloatTweakable)
                ((FloatTweakable)tweakable).Value = Convert.ToSingle(reader.ReadAsString());
            else if (tweakable is StringTweakable)
                ((StringTweakable)tweakable).Value = Il2CppSystem.Convert.ToString(reader.ReadAsString());
            // else if (tweakable is JoystickAxisTweakable)// TODO: FIX JOYSTICKS. CURRENTLY CCTOR ERROR WITH new JoystickAxis()
            // {
            //     JoystickAxis joystick_axis = new JoystickAxis();
            //     joystick_axis.Deserialize(reader);
            //     ((JoystickAxisTweakable)tweakable).Value = joystick_axis;
            // }
            else if (tweakable is InputActionTweakable)
            {
                InputAction input_action = new InputAction();
                input_action.Deserialize(reader);
                ((InputActionTweakable)tweakable).Value = input_action;
            }
        }
    }

    /// <summary>
    /// Used by GearBlocks for saving your Part properties.
    /// </summary>
    public override void SaveJsonProperties(JsonWriter writer)
    {
        foreach (KeyValuePair<string, TweakableBase> pair in tweakables_dict)
        {
            TweakableBase tweakable;
            tweakables_dict.TryGetValue(pair.Key, out tweakable);
            // TODO: Else if you dingus
            if (tweakable is IntTweakable || 
                tweakable is BooleanTweakable || 
                tweakable is FloatTweakable || 
                tweakable is StringTweakable ||
                tweakable is JoystickAxisTweakable ||
                tweakable is InputActionTweakable) 
                    writer.WritePropertyName(pair.Key);

            if (tweakable is IntTweakable) 
            {
                writer.WriteValue((int)(IntTweakable)tweakable);
            }
            if (tweakable is BooleanTweakable) writer.WriteValue((bool)(BooleanTweakable)tweakable);
            if (tweakable is FloatTweakable) writer.WriteValue((float)(FloatTweakable)tweakable);
            if (tweakable is StringTweakable) writer.WriteValue((string)(StringTweakable)tweakable);
            if (tweakable is InputActionTweakable) 
            {
                InputAction field_data = ((InputActionTweakable)tweakable).value;
                if (field_data != null)
                    field_data.Serialize(writer);
            }
            // if (tweakable is JoystickAxisTweakable) // TODO: FIX JOYSTICKS. CURRENTLY CCTOR ERROR WITH new JoystickAxis()
            // {
            //     JoystickAxis field_data = ((JoystickAxisTweakable)tweakable).value;
            //     if (field_data != null)
            //         field_data.Serialize(writer);
            // }
        }
    }
}