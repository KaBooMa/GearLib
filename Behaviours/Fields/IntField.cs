using SmashHammer.GearBlocks.Tweakables;

namespace GearLib.Behaviours.Fields;

public class IntField : IntTweakable
{
    public IntField(string label, string tooltip_text = null, int value = 0, int min = 0, int max = 1) : base(label, min, max, value, tooltip_text, null) { }
}