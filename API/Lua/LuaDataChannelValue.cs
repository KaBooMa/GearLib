using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using MoonSharp.Interpreter;
using static SmashHammer.GearBlocks.Construction.PartBehaviourBase;

namespace GearLib.API.Lua;

class LuaDataChannelValue : Il2CppSystem.Object
{
    Il2CppReferenceField<DataChannel> datachannel;
    
    static LuaDataChannelValue()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaDataChannelValue>();
        UserData.RegisterType<LuaDataChannelValue>();
    }

    public LuaDataChannelValue(IntPtr ptr) : base(ptr) { }
    public LuaDataChannelValue(DataChannel datachannel) : base(ClassInjector.DerivedConstructorPointer<LuaDataChannelValue>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.datachannel.Set(datachannel);
    }

    /// <summary>
    /// Sets the data channel as a text. Once this is done, a float value cannot be reassigned.
    /// </summary>
    /// <param name="value"></param>
    public void SetText(string value)
    {
        DataChannel channel = datachannel.Get();
        channel.FormatString = value;
    }

    /// <summary>
    /// Sets the data channel value
    /// </summary>
    /// <param name="value"></param>
    public void Set(float value)
    {
        DataChannel channel = datachannel.Get();
        channel.Value = value;
        channel.UpdateAverage();
    }

    /// <summary>
    /// Gets the data channel value
    /// </summary>
    /// <param name="value"></param>
    public float Get()
    {
        return datachannel.Get().value;
    }
}