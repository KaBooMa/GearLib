using System;

namespace GearLib.Behaviours.Fields;

public class IntField : Attribute, IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public int initial_value { get; set; } = 0;
    public int minimum_value { get; set; } = 0;
    public int maximum_value { get; set; } = 10;
}