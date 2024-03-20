using System.Collections.Generic;
using HarmonyLib;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;
using static SmashHammer.GearBlocks.Construction.PartDatabaseAsset;

namespace GearLib.Patches;

[HarmonyPatch(typeof(PartDatabaseAsset), nameof(PartDatabaseAsset.Load))]
class PartsDatabase : MonoBehaviour
{
    private static Dictionary<ulong, GameObject> new_parts = new Dictionary<ulong, GameObject>();
    private static Dictionary<ulong, GameObject> new_materials = new Dictionary<ulong, GameObject>();

    // Patches the database load to load modded parts AFTER game parts are loaded
    private static void Postfix(PartDatabaseAsset __instance)
    {
        GameObject donor_beam_part = __instance.parts[6532399048539789867].partPrefab;
        PartPropertiesSwappableMaterial donor_beam_swappable = donor_beam_part.GetComponent<PartPropertiesSwappableMaterial>();

        foreach (KeyValuePair<ulong, GameObject> part in new_parts)
        {
            PartPropertiesSwappableMaterial swappable_material = part.Value.GetComponent<PartPropertiesSwappableMaterial>();
            if (swappable_material)
            {
                swappable_material.partDatabase = __instance;
                swappable_material.objectMaterialisation = donor_beam_swappable.objectMaterialisation;
            }

            PartEntry new_entry = new PartEntry(part.Value);
            __instance.parts.TryAdd(part.Key, new_entry);
        }
    }

    // Adds mod items to the queue. Items is not added to the database until the database loads its assets
    public static void QueuePart(ulong part_uid, GameObject asset)
    {
        new_parts.Add(part_uid, asset);
    }
    
    public static void QueueMaterial(ulong material_uid, GameObject asset)
    {
        new_materials.Add(material_uid, asset);
    }
}