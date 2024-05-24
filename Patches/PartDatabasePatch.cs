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
class PartDatabasePatch : MonoBehaviour
{
    private static Dictionary<ulong, GameObject> new_parts = new Dictionary<ulong, GameObject>();

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
            PartPropertiesSwappableMaterial swappable_material = part.Value.GetComponent<PartPropertiesSwappableMaterial>();
            if (swappable_material)
            {
                swappable_material.partDatabase = __instance;
                swappable_material.objectMaterialisation = donor_beam_swappable.objectMaterialisation;
            }

            PartEntry new_entry = new PartEntry(part.Value);
            part.Value.transform.SetParent(null, false);
            DontDestroyOnLoad(part.Value);
            // GameObject.Destroy(part.Value.GetComponentInChildren<BoxCollider>());
            __instance.parts.TryAdd(part.Key, new_entry);
        }
    }

    // Adds mod items to the queue. Items is not added to the database until the database loads its assets
    public static void QueuePart(ulong part_uid, GameObject asset)
    {
        new_parts.Add(part_uid, asset);
    }
}