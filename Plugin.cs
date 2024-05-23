using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using GearLib.Utils;
using HarmonyLib;

namespace GearLib;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;
    
    public override void Load()
    {
        Log = base.Log;
        Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
        harmony.PatchAll();

        GearthonLoader.LoadMods();

        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
}