using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.Core;

public class LinkType : PartLinkTypeAsset
{
    public LinkType(string display_name, Color color)
    {
        displayName = display_name;
        colour = color;
    }
}