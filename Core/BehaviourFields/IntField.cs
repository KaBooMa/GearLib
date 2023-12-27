using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class IntField : IntTweakable
{

    public IntField(string label, string tooltip_text = null, int value = 0, int min = 0, int max = 1) : base(label, min, max, value, tooltip_text, FieldConstants.bool_expr_asset)
    {
        Label = label;
        Description = tooltip_text;
        Value = value;
        Min = min;
        Max = max;
        //BoolVariableAsset tweakable = new BoolVariableAsset() { };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}