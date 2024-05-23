using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;

namespace GearLib.API.Lua;

class LuaVector3 : Il2CppSystem.Object
{
    public Il2CppValueField<float> x, y, z;

    static LuaVector3()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaVector3>();
        UserData.RegisterType<LuaVector3>();
    }

    public LuaVector3(IntPtr ptr) : base(ptr) { }

    public LuaVector3() : base(ClassInjector.DerivedConstructorPointer<LuaVector3>())
    {
        ClassInjector.DerivedConstructorBody(this);
    }

    public LuaVector3(float x, float y, float z) : base(ClassInjector.DerivedConstructorPointer<LuaVector3>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.x.Set(x);
        this.y.Set(y);
        this.z.Set(z);
    }

    public LuaVector3(UnityEngine.Vector3 vector3) : base(ClassInjector.DerivedConstructorPointer<LuaVector3>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.x.Set(vector3.x);
        this.y.Set(vector3.y);
        this.z.Set(vector3.z);
    }

    public LuaVector3(System.Numerics.Vector3 vector3) : base(ClassInjector.DerivedConstructorPointer<LuaVector3>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.x.Set(vector3.X);
        this.y.Set(vector3.Y);
        this.z.Set(vector3.Z);
    }

    /// <summary>
    /// Adds this vector to another vector
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns>LuaVector3</returns>
    public DynValue Add(DynValue other_vector)
    {
        LuaVector3 other_lua_vector = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vector.x.Get(), other_lua_vector.y.Get(), other_lua_vector.z.Get());
        return DynValue.FromObject(null, new LuaVector3(this_vec + other_vec));
    }

    /// <summary>
    /// Subtracts this vector to another vector
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns>LuaVector3</returns>
    public DynValue Subtract(DynValue other_vector)
    {
        LuaVector3 other_lua_vector = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vector.x.Get(), other_lua_vector.y.Get(), other_lua_vector.z.Get());
        return DynValue.FromObject(null, new LuaVector3(this_vec - other_vec));
    }

    /// <summary>
    /// Divides this vector by a value
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns>LuaVector3</returns>
    public DynValue Divide(float value)
    {
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        return DynValue.FromObject(null, new LuaVector3(this_vec / value));
    }

    /// <summary>
    /// Multiplies this vector by a value
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns>LuaVector3</returns>
    public DynValue Multiply(float value)
    {
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        return DynValue.FromObject(null, new LuaVector3(this_vec * value));
    }

    /// <summary>
    /// Gets the magnitude of the vector
    /// </summary>
    /// <returns></returns>
    public float Magnitude()
    {
        UnityEngine.Vector3 vec3 = new UnityEngine.Vector3(x, y, z);
        return vec3.magnitude;
    }

    /// <summary>
    /// Gets the normalized LuaVector3 of the vector
    /// </summary>
    /// <returns></returns>
    public DynValue Normalized()
    {
        UnityEngine.Vector3 vec3 = new UnityEngine.Vector3(x, y, z);
        return DynValue.FromObject(null, new LuaVector3(vec3.normalized));
    }

    /// <summary>
    /// Gets the distance between two vectors
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns></returns>
    public float Distance(DynValue other_vector)
    {
        LuaVector3 other_lua_vec = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vec.x, other_lua_vec.y, other_lua_vec.z);
        return UnityEngine.Vector3.Distance(this_vec, other_vec);
    }

    /// <summary>
    /// Gets the distance between two vectors
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns></returns>
    public float Angle(DynValue other_vector)
    {
        LuaVector3 other_lua_vec = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vec.x, other_lua_vec.y, other_lua_vec.z);
        return UnityEngine.Vector3.Angle(this_vec, other_vec);
    }

    /// <summary>
    /// Gets the distance between two vectors
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <param name="t">0.0 - 1.0</param>
    /// <returns>LuaVector3</returns>
    public DynValue Lerp(DynValue other_vector, float value)
    {
        LuaVector3 other_lua_vec = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vec.x, other_lua_vec.y, other_lua_vec.z);
        UnityEngine.Vector3 lerped = UnityEngine.Vector3.Lerp(this_vec, other_vec, value);
        return DynValue.FromObject(null, new LuaVector3(lerped));
    }

    /// <summary>
    /// Gets the dot product of two vectors
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns></returns>
    public float Dot(DynValue other_vector)
    {
        LuaVector3 other_lua_vec = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vec.x, other_lua_vec.y, other_lua_vec.z);
        return UnityEngine.Vector3.Dot(this_vec, other_vec);
    }

    /// <summary>
    /// Gets the cross product of two vectors
    /// </summary>
    /// <param name="other_vector">LuaVector3</param>
    /// <returns>LuaVector</returns>
    public DynValue Cross(DynValue other_vector)
    {
        LuaVector3 other_lua_vec = other_vector.ToObject<LuaVector3>();
        UnityEngine.Vector3 this_vec = new UnityEngine.Vector3(x, y, z);
        UnityEngine.Vector3 other_vec = new UnityEngine.Vector3(other_lua_vec.x, other_lua_vec.y, other_lua_vec.z);
        UnityEngine.Vector3 cross_product = UnityEngine.Vector3.Cross(this_vec, other_vec);
        return DynValue.FromObject(null, new LuaVector3(cross_product));
    }

    public override string ToString()
    {
        return $"{{x: {x.Get()}, y: {y.Get()}, z: {z.Get()}}}";
    }
}