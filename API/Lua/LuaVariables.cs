using System;
using System.Collections.Generic;
using GearLib.API.Lua.Tweakables;
using GearLib.Classes;
using Il2CppInterop.Runtime.Injection;
using MoonSharp.Interpreter;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;
using UnityEngine;

namespace GearLib.API.Lua;

class LuaVariables : Il2CppSystem.Object
{
    public LuaPart PART;
    public LuaCollider COLLIDER;
    public LuaRigidbody RIGIDBODY;
    public LuaConstruction CONSTRUCTION;
    public LuaScript SCRIPT;
    

    LuaScriptBehaviour script_behaviour;

    static LuaVariables()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaVariables>();
    }

    public LuaVariables(IntPtr ptr) : base(ptr) { }
    public LuaVariables(LuaScriptBehaviour script_behaviour) : base(ClassInjector.DerivedConstructorPointer<LuaVariables>())
    {
        ClassInjector.DerivedConstructorBody(this);
        
        this.script_behaviour = script_behaviour;

        // COLLIDER = UserData.Create(new LuaCollider(script_behaviour.descriptor.GetComponentInChildren<Collider>()));
        PART = new LuaPart(script_behaviour.descriptor);
        CONSTRUCTION = new LuaConstruction(script_behaviour.descriptor.ParentConstruction);
        RIGIDBODY = new LuaRigidbody(script_behaviour.descriptor.parentComposite.GetComponent<Rigidbody>());
        SCRIPT = new LuaScript(script_behaviour);
        
        // Static non-updating
        script_behaviour.script.Globals.Set("Script", UserData.Create(SCRIPT));

        // Dynamic, will need to update
        script_behaviour.script.Globals.Set("Construction", UserData.Create(CONSTRUCTION));
        script_behaviour.script.Globals.Set("Rigidbody", UserData.Create(RIGIDBODY));
        script_behaviour.script.Globals.Set("Part", UserData.Create(PART));

        // Give lua access to our "helper" methods
        script_behaviour.script.Globals.Set("Debug", UserData.Create(new LuaDebug()));
        script_behaviour.script.Globals.Set("DataChannel", UserData.Create(new LuaDataChannel(script_behaviour)));
        
        
        // foreach (KeyValuePair<string, TweakableBase> pair in script_behaviour.tweakables_dict)
        // {
        //     if (pair.Value is IntTweakable)
        //         script_behaviour.script.Globals.Set(pair.Key, UserData.Create((IntTweakable)pair.Value));
        //     else if (pair.Value is BooleanTweakable)
        //         script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, (bool)(BooleanTweakable)pair.Value));
        //     else if (pair.Value is FloatTweakable)
        //         script_behaviour.script.Globals.Set(pair.Key, UserData.Create((Il2CppReferenceField<float>)(float)(FloatTweakable)pair.Value));
        //     else if (pair.Value is StringTweakable)
        //         script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, (string)(StringTweakable)pair.Value));
        //     // else if (pair.Value is JoystickAxisTweakable)
        //     //     script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, (int)(IntTweakable)pair.Value));
        //     else if (pair.Value is InputActionTweakable)
        //         script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, new LuaInputAction((InputAction)(InputActionTweakable)pair.Value)));
        // }
    }

    /// <summary>
    /// Internal Use Only.
    /// Updates our script with values from LuaVariables
    /// </summary>
    public void _UpdateVars()
    {
        // Update our variables
        RIGIDBODY.rigidbody.Set(script_behaviour.descriptor.parentComposite.GetComponent<Rigidbody>());
        CONSTRUCTION.construction.Set(script_behaviour.descriptor.ParentConstruction);
        PART.part.Set(script_behaviour.descriptor);
    }
    

    /// <summary>
    /// Internal Use Only.
    /// Updates our script with tweakables from LuaVariables
    /// </summary>
    public void _UpdateTweakables()
    {

        foreach (KeyValuePair<string, TweakableBase> pair in script_behaviour.tweakables_dict)
        {
            if (pair.Value is IntTweakable)
            {
                int value = (int)(IntTweakable)pair.Value;
                if (script_behaviour.script.Globals.Get(pair.Key).Number != value)
                    script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, value));
            }
            else if (pair.Value is BooleanTweakable)
            {
                bool value = (bool)(BooleanTweakable)pair.Value;
                if (script_behaviour.script.Globals.Get(pair.Key).Boolean != value)
                    script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, value));
            }
            else if (pair.Value is FloatTweakable)
            {
                float value = (float)(FloatTweakable)pair.Value;
                if (script_behaviour.script.Globals.Get(pair.Key).Number != value)
                    script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, value));
            }
            else if (pair.Value is StringTweakable)
            {
                string value = (string)(StringTweakable)pair.Value;
                if (script_behaviour.script.Globals.Get(pair.Key).String != value)
                    script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, value));
            }
            // else if (pair.Value is JoystickAxisTweakable) // NOT UPDATED JUST YET
            // {
            //     bool value = (bool)(BooleanTweakable)pair.Value;
            //     if (script_behaviour.script.Globals.Get(pair.Key).Boolean != value)
            //         script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, (bool)(BooleanTweakable)pair.Value));
            // }
            else if (pair.Value is InputActionTweakable)
            {
                InputAction value = (InputAction)(InputActionTweakable)pair.Value;
                if (script_behaviour.script.Globals.Get(pair.Key).ToObject<LuaInputAction>() == null || script_behaviour.script.Globals.Get(pair.Key).ToObject<LuaInputAction>().input_action.Get() != value)
                {
                    script_behaviour.script.Globals.Set(pair.Key, DynValue.FromObject(script_behaviour.script, new LuaInputAction(value)));
                }
            }
        }
    }
}