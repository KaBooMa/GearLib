using SmashHammer.GearBlocks.Tweakables;

namespace GearLib.Core.BehaviourFields;

public class StringField : StringTweakable
{

    public StringField(string label, string tooltip_text = null, string value = "", bool multiple_lines = false) : base(label, multiple_lines, value, tooltip_text, FieldConstants.bool_expr_asset) { }
}