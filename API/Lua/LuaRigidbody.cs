using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GearLib.API.Lua;

class LuaRigidbody : Il2CppSystem.Object
{
    public Il2CppReferenceField<Rigidbody> rigidbody;
    
    static LuaRigidbody()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaRigidbody>();
        UserData.RegisterType<LuaRigidbody>();
    }

    public LuaRigidbody(IntPtr ptr) : base(ptr) { }
    public LuaRigidbody(Rigidbody rigidbody) : base(ClassInjector.DerivedConstructorPointer<LuaRigidbody>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.rigidbody.Set(rigidbody);
    }

    /// <summary>
    /// Gets the angular velocity of the rigidbody
    /// </summary>
    /// <returns></returns>
    public LuaVector3 GetAngularVelocity()
    {
        return new LuaVector3(rigidbody.Get().angularVelocity);
    }

    /// <summary>
    /// Gets the angular drag of the rigidbody
    /// </summary>
    /// <returns></returns>
    public float GetAngularDrag()
    {
        return rigidbody.Get().angularDrag;
    }

    /// <summary>
    /// Sets the angular drag of the rigidbody
    /// </summary>
    /// <returns></returns>
    public void SetAngularDrag(float drag)
    {
        rigidbody.Get().angularDrag = drag;
    }

    /// <summary>
    /// Gets the torque of the rigidbody
    /// </summary>
    /// <returns></returns>
    public LuaVector3 GetTorque()
    {
        Rigidbody rb = rigidbody.Get();
        Vector3 angular_velocity = rb.angularVelocity;
        Vector3 inertia = rb.inertiaTensor;
        Vector3 torque = Vector3.Scale(inertia, angular_velocity);
        return new LuaVector3(torque);
    }

    /// <summary>
    /// Sets the angular velocity of the rigidbody
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public void SetAngularVelocity(float x, float y, float z)
    {
        rigidbody.Get().angularVelocity = new Vector3(x, y, z);
    }

    /// <summary>
    /// Adds to the angular velocity of the rigidbody
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public void AddAngularVelocity(float x, float y, float z)
    {
        rigidbody.Get().angularVelocity += new Vector3(x, y, z);
    }

    /// <summary>
    /// Gets the world position of the rigidbody
    /// </summary>
    /// <returns></returns>
    public LuaVector3 GetPosition()
    {
        return new LuaVector3(rigidbody.Get().position.x, rigidbody.Get().position.y, rigidbody.Get().position.z);
    }

    /// <summary>
    /// Gets the mass of the rigidbody
    /// </summary>
    /// <returns></returns>
    public float GetMass()
    {
        return rigidbody.Get().mass;
    }

    /// <summary>
    /// Enable/Disable gravity on a rigidbody
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    /// 
    public void SetGravity(bool enabled)
    {
        rigidbody.Get().useGravity = enabled;
    }

    /// <summary>
    /// Gets the velocity of the rigidbody
    /// </summary>
    /// <returns></returns>
    public LuaVector3 GetVelocity()
    {
        return new LuaVector3(rigidbody.Get().velocity.x, rigidbody.Get().velocity.y, rigidbody.Get().velocity.z);
    }

    /// <summary>
    /// Gets the rotation of the rigidbody
    /// </summary>
    /// <returns></returns>
    public LuaVector3 GetRotation()
    {

        return new LuaVector3(rigidbody.Get().rotation.eulerAngles);
    }

    /// <summary>
    /// Applies a force to the part in world coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void AddForce(float x, float y, float z)
    {
        AddForce(x, y, z, "force");
    }
    
    /// <summary>
    /// Applies a force to the part in local coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddRelativeForce(float x, float y, float z)
    {
        AddRelativeForce(x, y, z, "force");
    }
    
    /// <summary>
    /// Applies torque to the part in world coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddTorque(float x, float y, float z)
    {
        AddTorque(x, y, z, "force");
    }
    
    /// <summary>
    /// Applies torque to the part in local coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddRelativeTorque(float x, float y, float z)
    {
        AddRelativeTorque(x, y, z, "force");
    }

    /// <summary>
    /// Applies a force with a specific mode to the part in world coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddForce(float x, float y, float z, string mode)
    {
        ForceMode mode_enum = _GetForceMode(mode);
        rigidbody.Get().AddForce(x, y, z, mode_enum);
    }
    
    /// <summary>
    /// Applies a force with a specific mode to the part in local coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddRelativeForce(float x, float y, float z, string mode)
    {
        ForceMode mode_enum = _GetForceMode(mode);
        rigidbody.Get().AddRelativeForce(x, y, z, mode_enum);
    }
    
    /// <summary>
    /// Applies torque with a specific mode to the part in world coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddTorque(float x, float y, float z, string mode)
    {
        ForceMode mode_enum = _GetForceMode(mode);
        rigidbody.Get().AddTorque(x, y, z, mode_enum);
    }
    
    /// <summary>
    /// Applies torque with a specific mode to the part in local coordinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mode">Force, Acceleration, Impulse or VelocityChange</param>
    public void AddRelativeTorque(float x, float y, float z, string mode)
    {
        ForceMode mode_enum = _GetForceMode(mode);
        rigidbody.Get().AddRelativeTorque(x, y, z, mode_enum);
    }

    ForceMode _GetForceMode(string mode)
    {
        switch (mode.ToLower())
        {
            case "force":
                return ForceMode.Force;
            case "acceleration":
                return ForceMode.Acceleration;
            case "impsule":
                return ForceMode.Impulse;
            case "velocitychange":
                return ForceMode.VelocityChange;
            default:
                return ForceMode.Force;
        }
        
    }
}