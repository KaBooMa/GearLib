using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;
using SmashHammer.GearBlocks.Construction;

namespace GearLib.API.Lua;

class LuaAttachment : Il2CppSystem.Object
{
    public Il2CppReferenceField<AttachmentBase> attachment;

    static LuaAttachment()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaAttachment>();
        UserData.RegisterType<LuaAttachment>();
    }

    public LuaAttachment(IntPtr ptr) : base(ptr) { }
    public LuaAttachment(AttachmentBase attachment) : base(ClassInjector.DerivedConstructorPointer<LuaAttachment>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.attachment.Set(attachment);
    }

    /// <summary>
    /// Sets the angular velocity with torque of the attachment
    /// </summary>
    /// <param name="angular_velocity"></param>
    /// <param name="torque"></param>
    /// <returns>LuaPart</returns>
    public void SetAngularVelocity(float angular_velocity, float torque)
    {
        RotaryBearingAttachment rotary = attachment.Get().Cast<RotaryBearingAttachment>();
        if (rotary != null)
        {
            rotary.SetMotor(angular_velocity, torque);
        }
    }

    /// <summary>
    /// Gets the angular of the attachment
    /// </summary>
    /// <returns>LuaPart</returns>
    public float GetAngularSpeed()
    {
        RotaryBearingAttachment rotary = attachment.Get().Cast<RotaryBearingAttachment>();
        if (rotary != null)
        {
            return rotary.CurrentAngularSpeed;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// Returns the owner part
    /// </summary>
    /// <returns>LuaPart</returns>
    public DynValue GetOwnerPart()
    {
        return DynValue.FromObject(null, new LuaPart(attachment.Get().OwnerPart));
    }

    /// <summary>
    /// Returns the connected part
    /// </summary>
    /// <returns>LuaPart</returns>
    public DynValue GetConnectedPart()
    {
        return DynValue.FromObject(null, new LuaPart(attachment.Get().ConnectedPart));
    }

    /// <summary>
    /// Returns if the attachment is locked
    /// </summary>
    /// <param name="attachment"></param>
    /// <returns></returns>
    public bool GetLocked(AttachmentBase attachment)
    {
        return attachment.IsLocked;
    }

    /// <summary>
    /// Returns the attachment type(s)
    /// </summary>
    /// <param name="attachment"></param>
    /// <returns></returns>
    public string GetType(AttachmentBase attachment)
    {
        return attachment.Type.ToString();
    }

    /// <summary>
    /// Returns if the attachment is named string 'name'. This is based off how you name the attachment in the editor.
    /// </summary>
    /// <param name="attachment"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckIfNamed(AttachmentBase attachment, string name)
    {
        return attachment.ownerPartPointGrid.name.Replace("PointGrid_", "") == name || 
                attachment.connectedPartPointGrid.name.Replace("PointGrid_", "") == name;
    }
}