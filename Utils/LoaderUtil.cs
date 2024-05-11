using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace GearLib.Utils;

class LoaderUtil
{
    private static Dictionary<string, AssetBundle> loaded_bundles = new Dictionary<string, AssetBundle>();

    public static Mesh LoadMeshFromOBJ(string asset_path, string asset_name) 
    {
        string path = Path.Combine(asset_path, $"{asset_name}.obj");
        if (File.Exists(path))
        {
            string[] lines = File.ReadAllLines(path);
            return LoadMeshFromOBJData(lines);
        }
        else
        {
            Plugin.Log.LogError($"Failed to load OBJ {path}!");
            return null;
        }
    }
    public static Mesh LoadMeshFromOBJData(string[] data) 
    {
        return ObjParser.ParseObj(data);
    }

    public static AssetBundle GetAssetBundle(string bundle_path, string asset_name)
    {        
        AssetBundle bundle;

        if (!loaded_bundles.TryGetValue(bundle_path, out bundle)) 
        {
            bundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, bundle_path));
            loaded_bundles.Add(bundle_path, bundle);
        }

        return bundle;
    }
    
    public static GameObject LoadObject(string bundle_path, string object_name)
    {
        AssetBundle bundle = GetAssetBundle(bundle_path, object_name);

        try
        {
            GameObject asset = bundle.LoadAsset(object_name, Il2CppType.Of<GameObject>()).Cast<GameObject>();
            
            return asset;
        }
        catch
        {
            Plugin.Log.LogError($"Failed to load object {object_name}!!");
            return null;
        }
    }
    
    public static Material LoadMaterial(string bundle_path, string material_name)
    {
        AssetBundle bundle = GetAssetBundle(bundle_path, material_name);

        try
        {
            Material material = bundle.LoadAsset(material_name, Il2CppType.Of<Material>()).Cast<Material>();
            
            return material;
        }
        catch
        {
            Plugin.Log.LogError($"Failed to load material {material_name}!!");
            return null;
        }
    }
}