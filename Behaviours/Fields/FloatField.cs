using SmashHammer.GearBlocks.Tweakables;

namespace GearLib.Behaviours.Fields;

public class FloatField : FloatTweakable
{
    public FloatField(string label, string tooltip_text = null, float value = 0, float min = 0, float max = 1) : base(label, min, max, value, tooltip_text, FieldConstants.bool_expr_asset) { }
}