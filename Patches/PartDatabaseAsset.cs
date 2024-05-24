using System.Collections.Generic;
using System.Linq;
using GearLib.API;
using GearLib.Utils;
using HarmonyLib;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.UI;
using UnityEngine;
using static SmashHammer.GearBlocks.Construction.PartDatabaseAsset;

namespace GearLib.Patches;

[HarmonyPatch(typeof(PartDatabaseAsset), nameof(PartDatabaseAsset.UpdateEntries))]
class PartDatabaseAssetPatch
{
    private static List<PartLinkTypeAsset> new_link_types = new List<PartLinkTypeAsset>();
    private static Dictionary<ulong, GameObject> new_parts = new Dictionary<ulong, GameObject>();
    private static Dictionary<ulong, PartMaterialAsset> new_materials = new Dictionary<ulong, PartMaterialAsset>();

    // Patches the database load to load modded parts AFTER game parts are loaded
    private static void Postfix(PartDatabaseAsset __instance)
    {
        // Lets get Gearthon parts queued up
        GearthonLoader.LoadParts();

        // Pull in all our new parts
        GameObject donor_beam_part = __instance.parts[6532399048539789867].partPrefab;
        PartPropertiesSwappableMaterial donor_beam_swappable = donor_beam_part.GetComponent<PartPropertiesSwappableMaterial>();
    
        foreach (KeyValuePair<ulong, GameObject> part in new_parts)
        {
            Plugin.Log.LogWarning("FOUND++++"+part.Value.name);
            PartPropertiesSwappableMaterial swappable_material = part.Value.GetComponent<PartPropertiesSwappableMaterial>();
            if (swappable_material)
            {
                swappable_material.partDatabase = __instance;
                swappable_material.objectMaterialisation = donor_beam_swappable.objectMaterialisation;
            }

            PartEntry new_entry = new PartEntry(part.Value);
            part.Value.transform.SetParent(null, false);
            GameObject.DontDestroyOnLoad(part.Value);
            // GameObject.Destroy(part.Value.GetComponentInChildren<BoxCollider>());
            __instance.parts.TryAdd(part.Key, new_entry);
        }

        // Pull in all our new materials
        foreach (KeyValuePair<ulong, PartMaterialAsset> mat in new_materials)
        {
            MaterialEntry mat_entry = new MaterialEntry(mat.Value);
            __instance.materials.TryAdd(mat.Key, mat_entry);
        }
    }

    // Adds mod items to the queue. Items is not added to the database until the database loads its assets
    public static void QueuePart(ulong part_uid, GameObject asset)
    {
        new_parts.Add(part_uid, asset);
    }
    
    public static void QueueMaterial(ulong material_uid, PartMaterialAsset material)
    {
        new_materials.Add(material_uid, material);
    }
    
    public static void QueueLinkType(PartLinkTypeAsset link_type)
    {
        new_link_types.Add(link_type);
    }
}