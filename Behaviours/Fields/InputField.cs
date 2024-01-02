using System;
using SmashHammer.Input;

namespace GearLib.Behaviours.Fields;

public class InputField : Attribute, IField
{

    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public InputAction initial_value { get; set; } = new InputAction();
}