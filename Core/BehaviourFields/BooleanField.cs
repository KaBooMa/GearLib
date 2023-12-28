using SmashHammer.GearBlocks.Tweakables;

namespace GearLib.Core.BehaviourFields;

public class BooleanField : BooleanTweakable
{
    public BooleanField(string label, string tooltip_text = null, bool value = false) : base(label, value, tooltip_text, FieldConstants.bool_expr_asset) { }
}