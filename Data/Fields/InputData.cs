using SmashHammer.Input;
using UnityEngine;

namespace GearLib.Data.Fields;

class InputData : IFieldData
{
    public KeyCode primary_key { get; set; }
    public KeyCode primary_key_modifier { get; set; }
    public KeyCode secondary_key { get; set; }
    public KeyCode secondary_key_modifier { get; set; }

    public InputData() {}

    public InputData(InputAction value)
    {
        primary_key = value.primaryKey.keyCode;
        primary_key_modifier = value.primaryKey.modifierKeyCode;
        secondary_key = value.secondaryKey.keyCode;
        secondary_key_modifier = value.secondaryKey.modifierKeyCode;
    }

    public InputAction GetInputAction()
    {
        return new InputAction(primary_key, primary_key_modifier, secondary_key, secondary_key_modifier);
    }
}