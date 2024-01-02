using SmashHammer.Input;
using static SmashHammer.Input.InputUtils;

namespace GearLib.Data.Fields;

class JoystickData : IFieldData
{
    public AxisCode axis_code { get; set; }
    public float deadzone { get; set; }
    public float gamma { get; set; }
    public float centre { get; set; }

    public JoystickData() { }
    public JoystickData(JoystickAxis joystick) 
    { 
        axis_code = joystick.axis.axisCode;
        deadzone = joystick.deadzone;
        gamma = joystick.gamma;
        centre = joystick.centre;
    }

    public JoystickAxis GetJoystickAxis()
    {
        return new JoystickAxis(axis_code, deadzone, gamma, centre);
    }
}