using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Il2CppSystem.Collections.Generic;
using MoonSharp.Interpreter;
using SmashHammer.GearBlocks.Construction;

namespace GearLib.API.Lua;

class LuaConstruction : Il2CppSystem.Object
{    
    public Il2CppReferenceField<Construction> construction;

    static LuaConstruction()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaConstruction>();
        UserData.RegisterType<LuaConstruction>();
    }

    public LuaConstruction(IntPtr ptr) : base(ptr) { }
    public LuaConstruction(Construction construction) : base(ClassInjector.DerivedConstructorPointer<LuaConstruction>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.construction.Set(construction);
    }

    /// <summary>
    /// Gets all parts in a construction
    /// </summary>
    /// <param name="construction"></param>
    /// <returns></returns>
    public List<LuaPart> GetParts()
    {
        List<LuaPart> parts = new List<LuaPart>();
        foreach (PartDescriptor part in construction.Get().Parts)
        {
            parts.Add(new LuaPart(part));
        }
        return parts;
    }

    /// <summary>
    /// Gets whether a construction is currently frozen or not
    /// </summary>
    /// <param name="construction"></param>
    /// <returns></returns>
    public bool GetFrozen()
    {
        return construction.Get().IsFrozen;
    }

    /// <summary>
    /// Gets the mass of a construction
    /// </summary>
    /// <param name="construction"></param>
    /// <returns></returns>
    public float GetMass()
    {
        return construction.Get().mass;
    }

    /// <summary>
    /// Sets the construction frozen/unfrozen
    /// </summary>
    /// <param name="construction"></param>
    /// <param name="frozen"></param>
    /// <returns></returns>
    public void SetFrozen(bool frozen)
    {
        construction.Get().SetFrozen(frozen);
    }
}