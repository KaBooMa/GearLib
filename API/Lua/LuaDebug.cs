using System;
using Il2CppInterop.Runtime.Injection;
using MoonSharp.Interpreter;

namespace GearLib.API.Lua;

class LuaDebug : Il2CppSystem.Object
{
    static LuaDebug()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaDebug>();
        UserData.RegisterType<LuaDebug>();
    }

    public LuaDebug(IntPtr ptr) : base(ptr) { }
    public LuaDebug() : base(ClassInjector.DerivedConstructorPointer<LuaDebug>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    /// <summary>
    /// Prints message(s) to the BepInEx console
    /// </summary>
    /// <returns></returns>
    public void Log(string message)
    {
        Plugin.Log.LogMessage(message);
    }

    /// <summary>
    /// Prints informational message(s) to the BepInEx console
    /// </summary>
    /// <returns></returns>
    public void LogInfo(string message)
    {
        Plugin.Log.LogInfo(message);
    }

    /// <summary>
    /// Prints warning message(s) to the BepInEx console
    /// </summary>
    /// <returns></returns>
    public void LogWarning(string message)
    {
        Plugin.Log.LogWarning(message);
    }

    /// <summary>
    /// Prints error message(s) to the BepInEx console
    /// </summary>
    /// <returns></returns>
    public void LogError(string message)
    {
        Plugin.Log.LogError(message);
    }
}