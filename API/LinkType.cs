using System.Collections.Generic;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.API;

public class LinkType
{
    public static Dictionary<string, PartLinkTypeAsset> link_types = new Dictionary<string, PartLinkTypeAsset>();
    public PartLinkTypeAsset asset;

    public LinkType(string link_type, Color color)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom link [{link_type}]");
        asset = ScriptableObject.CreateInstance<PartLinkTypeAsset>();
        asset.name = link_type;
        asset.displayName = link_type;
        asset.colour = color;
        link_types.Add(link_type, asset);
    }
}