using System;

namespace GearLib.API.Fields;

/// <summary>
/// A Integer property for your Behaviour.
/// </summary>
public class IntField : IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public int initial_value { get; set; } = 0;
    public int minimum_value { get; set; } = 0;
    public int maximum_value { get; set; } = 10;
}