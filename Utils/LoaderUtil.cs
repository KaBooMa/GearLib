using System.Collections.Generic;
using System.IO;
using BepInEx;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace GearLib.Utils;

class LoaderUtil
{
    private static Dictionary<string, AssetBundle> loaded_bundles = new Dictionary<string, AssetBundle>();
    
    public static GameObject LoadAsset(string bundle_path, string asset_name)
    {
        AssetBundle bundle;

        if (!loaded_bundles.TryGetValue(bundle_path, out bundle)) 
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, bundle_path));
            loaded_bundles.Add(bundle_path, bundle);
        }

        try
        {
            GameObject asset = bundle.LoadAsset(asset_name, Il2CppType.Of<GameObject>()).Cast<GameObject>();
            return asset;
        }
        catch
        {
            Plugin.Log.LogError($"Failed to load asset {asset_name}!!");
            return null;
        }
    }
}