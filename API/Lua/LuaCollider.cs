using System;
using Il2CppInterop.Runtime.Injection;
using MoonSharp.Interpreter;

namespace GearLib.API.Lua;

class LuaCollider : Il2CppSystem.Object
{
    
    static LuaCollider()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaCollider>();
        UserData.RegisterType<LuaCollider>();
    }

    public LuaCollider(IntPtr ptr) : base(ptr) { }
    public LuaCollider() : base(ClassInjector.DerivedConstructorPointer<LuaCollider>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }
}