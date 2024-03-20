using System;

namespace GearLib.API.Fields;

/// <summary>
/// A Float property for your Behaviour.
/// </summary>
public class FloatField : IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public float initial_value { get; set; } = 0f;
    public float minimum_value { get; set; } = 0f;
    public float maximum_value { get; set; } = 1f;
}