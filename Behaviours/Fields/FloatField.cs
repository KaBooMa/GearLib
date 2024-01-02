using System;

namespace GearLib.Behaviours.Fields;


public class FloatField : Attribute, IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public float initial_value { get; set; } = 0f;
    public float minimum_value { get; set; } = 0f;
    public float maximum_value { get; set; } = 1f;
}