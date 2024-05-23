using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;
using SmashHammer.Input;

namespace GearLib.API.Lua.Tweakables;

class LuaInputAction : Il2CppSystem.Object
{
    public Il2CppReferenceField<InputAction> input_action;
    
    static LuaInputAction()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaInputAction>();
        UserData.RegisterType<LuaInputAction>();
    }

    public LuaInputAction(IntPtr ptr) : base(ptr) { }

    public LuaInputAction(InputAction input_action) : base(ClassInjector.DerivedConstructorPointer<LuaInputAction>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.input_action.Set(input_action);
    }

    /// <summary>
    /// Returns whether or not the input action button is currently held (constantly triggers)
    /// </summary>
    /// <returns></returns>
    public bool Held()
    {
        return input_action.Get().IsHeld;
    }

    /// <summary>
    /// Returns whether or not the input action button was pressed (only triggers on key down)
    /// </summary>
    /// <returns></returns>
    public bool Pressed()
    {
        return input_action.Get().IsTriggered;
    }
}