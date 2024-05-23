using System;
using GearLib.Classes;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;

namespace GearLib.API.Lua;

class LuaScript : Il2CppSystem.Object
{
    public Il2CppReferenceField<LuaScriptBehaviour> script;
    
    static LuaScript()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaScript>();
        UserData.RegisterType<LuaScript>();
    }

    public LuaScript(IntPtr ptr) : base(ptr) { }
    public LuaScript(LuaScriptBehaviour script) : base(ClassInjector.DerivedConstructorPointer<LuaScript>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.script.Set(script);
    }

    /// <summary>
    /// Returns if the script is currently active (toggled by player in-game)
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return script.Get().IsActivated;
    }
}