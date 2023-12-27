using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class StringField : StringTweakable
{

    public StringField(string label, string tooltip_text = null, string value = "", bool multiple_lines = false) : base(label, multiple_lines, value, tooltip_text, FieldConstants.bool_expr_asset)
    {
        Label = label;
        Description = tooltip_text;
        Value = value;
        MultiLine = multiple_lines;
        //BoolVariableAsset tweakable = new BoolVariableAsset() { };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}