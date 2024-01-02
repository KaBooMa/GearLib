using System;

namespace GearLib.Behaviours.Fields;

public class StringField : Attribute, IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public string initial_value { get; set; } = "";
    public bool multiple_lines { get; set; } = false;
}