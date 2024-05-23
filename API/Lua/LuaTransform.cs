using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GearLib.API.Lua;

class LuaTransform : Il2CppSystem.Object
{
    public Il2CppReferenceField<Transform> transform;
    static LuaTransform()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaTransform>();
        UserData.RegisterType<LuaTransform>();
    }

    public LuaTransform(IntPtr ptr) : base(ptr) { }
    public LuaTransform(Transform transform) : base(ClassInjector.DerivedConstructorPointer<LuaTransform>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.transform.Set(transform);
    }

    /// <summary>
    /// Sets the local scale of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="child_name"></param>
    /// <returns></returns>
    public void SetScale(float x, float y, float z)
    {
        transform.Get().localScale = new Vector3(x, y, z);
    }

    /// <summary>
    /// Sets the local rotation of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public void SetLocalRotation(float x, float y, float z)
    {
        transform.Get().localRotation = Quaternion.EulerAngles(new Vector3(x, y, z));
    }

    /// <summary>
    /// Gets the local rotation of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public LuaVector3 GetLocalRotation()
    {
        Vector3 rotation = transform.Get().localRotation.eulerAngles;
        return new LuaVector3(rotation);
    }

    /// <summary>
    /// Gets a child transform of the transform by name
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="child_name"></param>
    /// <returns>LuaTransform</returns>
    public DynValue GetChild(string child_name)
    {
        return DynValue.FromObject(null, new LuaTransform(transform.Get().FindChild(child_name)));
    }

    /// <summary>
    /// Gets a parent transform of the transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns>LuaTransform</returns>
    public DynValue GetParent()
    {
        return DynValue.FromObject(null, new LuaTransform(transform.Get().parent));
    }

    /// <summary>
    /// Gets the up Vector3 of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public LuaVector3 GetUp()
    {
        Vector3 up = transform.Get().up;
        return new LuaVector3(up.x, up.y, up.z);
    }

    /// <summary>
    /// Gets the right Vector3 of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public LuaVector3 GetRight()
    {
        Vector3 up = transform.Get().right;
        return new LuaVector3(up.x, up.y, up.z);
    }

    /// <summary>
    /// Gets the forward Vector3 of a transform
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public LuaVector3 GetForward()
    {
        Vector3 up = transform.Get().forward;
        return new LuaVector3(up.x, up.y, up.z);
    }
}