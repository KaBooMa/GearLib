using System;

namespace GearLib.API.Fields;

/// <summary>
/// A Boolean property for your Behaviour.
/// </summary>
public class BooleanField : IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public bool initial_value { get; set; } = false;
}