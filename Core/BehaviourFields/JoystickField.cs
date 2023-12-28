using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;

namespace GearLib.Core.BehaviourFields;

public class JoystickField : JoystickAxisTweakable
{

    public JoystickField(string label, string tooltip_text = null) : base(label, new JoystickAxis(), tooltip_text, FieldConstants.bool_expr_asset) { }
}