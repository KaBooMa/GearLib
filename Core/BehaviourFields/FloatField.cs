using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class FloatField : FloatTweakable
{

    public FloatField(string label, string tooltip_text = null, int value = 0, int min = 0, int max = 1) : base(label, min, max, value, tooltip_text, FieldConstants.bool_expr_asset)
    {
        Value = value;
        Min = min;
        Max = max;
        Label = label;
        Description = tooltip_text;
        //BoolVariableAsset tweakable = new BoolVariableAsset() { };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}