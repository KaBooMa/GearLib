using SmashHammer.GearBlocks.Tweakables;

namespace GearLib.Behaviours.Fields;

public class StringField : StringTweakable
{

    public StringField(string label, string tooltip_text = null, string value = "", bool multiple_lines = false) : base(label, multiple_lines, value, tooltip_text, null) { }
}