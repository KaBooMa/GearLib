using System;
using SmashHammer.Input;

namespace GearLib.API.Fields;

/// <summary>
/// A Input property for your Behaviour. This is for Keyboard inputs.
/// </summary>
public class InputField : IField
{

    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public InputAction initial_value { get; set; } = new InputAction();
}