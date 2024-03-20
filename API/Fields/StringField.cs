using System;

namespace GearLib.API.Fields;

/// <summary>
/// A String property for your Behaviour.
/// </summary>
public class StringField : IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public string initial_value { get; set; } = "";
    public bool multiple_lines { get; set; } = false;
}