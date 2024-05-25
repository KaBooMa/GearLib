using System.Collections.Generic;
using System.Linq;
using GearLib.API;
using GearLib.Utils;
using HarmonyLib;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.UI;
using UnityEngine;

namespace GearLib.Patches;

[HarmonyPatch(typeof(LinkerToolGui), nameof(LinkerToolGui.Awake))]
class LinkTypeDatabasePatch : MonoBehaviour
{
    private static List<LinkType> new_link_types = new List<LinkType>();

    // Patches the database load to load modded parts AFTER game parts are loaded
    private static void Prefix(LinkerToolGui __instance)
    {
        // Queue up Gearthon link types
        GearthonLoader.LoadLinkTypes();

        // Pull in all our new link types
        foreach (LinkType link_type in new_link_types)
        {
            PartLinkTypeAsset asset = Instantiate(__instance.linkTypes[0]);
            asset.name = link_type.uid;
            asset.displayName = link_type.display_name;
            asset.colour = link_type.color;

            __instance.linkTypes = __instance.linkTypes.AddItem(asset).ToArray();
        }

    }
    
    public static void Queue(LinkType link_type)
    {
        new_link_types.Add(link_type);
    }
}