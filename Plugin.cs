using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using GearLib.Patches;
using HarmonyLib;

namespace GearLib;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    
    public override void Load()
    {
        Log = base.Log;
        Harmony.CreateAndPatchAll(typeof(PartsDatabase));
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}