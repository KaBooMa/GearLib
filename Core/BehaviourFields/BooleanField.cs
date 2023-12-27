using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class BooleanField : BooleanTweakable
{
    public BooleanField(string label, string tooltip_text = null, bool value = false) : base(label, value, tooltip_text, FieldConstants.bool_expr_asset)
    {
        Value = value;
        Label = label;
        Description = tooltip_text;
        // BoolVariableAsset tweakable = new BoolVariableAsset() { reset = true, syncToNetworkClients = true };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}