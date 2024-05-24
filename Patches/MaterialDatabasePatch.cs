using System.Collections.Generic;
using GearLib.Utils;
using HarmonyLib;
using SmashHammer.GearBlocks.Construction;
using static SmashHammer.GearBlocks.Construction.PartDatabaseAsset;

namespace GearLib.Patches;

[HarmonyPatch(typeof(PartDatabaseAsset), nameof(PartDatabaseAsset.UpdateEntries))]
class MaterialDatabasePatch : UnityEngine.MonoBehaviour
{
    private static Dictionary<ulong, PartMaterialAsset> new_materials = new Dictionary<ulong, PartMaterialAsset>();

    // Patches the database load to load modded parts AFTER game parts are loaded
    private static void Postfix(PartDatabaseAsset __instance)
    {
        // Queue up Gearthon link types
        GearthonLoader.LoadMaterials();

        // Pull in all our new materials
        foreach (KeyValuePair<ulong, PartMaterialAsset> mat in new_materials)
        {
            MaterialEntry mat_entry = new MaterialEntry(mat.Value);
            Plugin.Log.LogWarning("BEFORE"+__instance.materials.Count);
            __instance.materials.TryAdd(mat.Key, mat_entry);
            Plugin.Log.LogWarning("AFTER"+__instance.materials.Count);
        }
    }
    
    public static void Queue(ulong uid, PartMaterialAsset material)
    {
        new_materials.Add(uid, material);
    }
}