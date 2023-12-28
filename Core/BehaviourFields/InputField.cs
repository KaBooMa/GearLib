using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;

namespace GearLib.Core.BehaviourFields;

public class InputField : InputActionTweakable
{

    public InputField(string label, string tooltip_text = null) : base(label, new InputAction(), tooltip_text, FieldConstants.bool_expr_asset) { }
}