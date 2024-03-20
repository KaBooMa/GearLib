using System;
using SmashHammer.Input;

namespace GearLib.API.Fields;

public class JoystickField : IField
{
    public string label { get; set; } = "MissingLabel";
    public string tooltip_text { get; set; } = "MissingTooltipText";
    public JoystickAxis initial_value { get; set; } = new JoystickAxis();
}