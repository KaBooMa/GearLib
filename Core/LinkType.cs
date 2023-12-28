using System.Collections.Generic;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.Core;

public class LinkType
{
    public static Dictionary<string, PartLinkTypeAsset> link_types = new Dictionary<string, PartLinkTypeAsset>();
    public PartLinkTypeAsset asset;

    public LinkType(string link_name, Color color)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom link [{link_name}]");
        asset = ScriptableObject.CreateInstance<PartLinkTypeAsset>();
        asset.name = link_name;
        asset.displayName = link_name;
        asset.colour = color;
        link_types.Add(link_name, asset);
    }
}