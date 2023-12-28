using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.Core;

public class LinkType : PartLinkTypeAsset
{
    public LinkType(string link_name, Color color)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom link [{link_name}]");
        name = link_name;
        displayName = link_name;
        colour = color;
    }
}