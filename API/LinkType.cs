using System.Collections.Generic;
using GearLib.Patches;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.API;

/// <summary>
/// Base class for a new Link Type in the game. Allows your mod Parts to connect together.
/// </summary>
public class LinkType
{
    public string uid;
    public string display_name;
    public Color color;
    /// <summary>
    /// Creates your new Link Type.
    /// </summary>
    /// <param name="name">Unique string ID for you to use in your Behaviours.</param>
    /// <param name="color">Color you want your link to display as in game.</param>
    public LinkType(string uid, string display_name, Color color)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom link type [{display_name}]");
        this.uid = uid;
        this.display_name = display_name;
        this.color = color;
        LinkTypeDatabasePatch.Queue(this);
    }
}