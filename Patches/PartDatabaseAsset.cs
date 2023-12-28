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

    // Patches the database load to load modded parts AFTER game parts are loaded
    private static void Postfix(PartDatabaseAsset __instance)
    {
        foreach (KeyValuePair<ulong, GameObject> part in new_parts)
        {
            PartEntry new_entry = new PartEntry(part.Value);
            __instance.parts.TryAdd(part.Key, new_entry);
        }
    }

    // Adds a part to the queue. Part is not added to the database until the database loads its assets
    public static void Add(ulong part_uid, GameObject asset)
    {
        new_parts.Add(part_uid, asset);
    }
}