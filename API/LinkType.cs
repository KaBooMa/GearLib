using System.Collections.Generic;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.API;

/// <summary>
/// Base class for a new Link Type in the game. Allows your mod Parts to connect together.
/// </summary>
public class LinkType
{
    public static Dictionary<string, PartLinkTypeAsset> link_types = new Dictionary<string, PartLinkTypeAsset>();
    public PartLinkTypeAsset asset;

    /// <summary>
    /// Creates your new Link Type.
    /// </summary>
    /// <param name="link_type">Unique string ID for you to use in your Behaviours.</param>
    /// <param name="color">Color you want your link to display as in game.</param>
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