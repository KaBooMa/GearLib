using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class InputField : InputActionTweakable
{

    public InputField(string label, string tooltip_text = null) : base(label, new InputAction(), tooltip_text, FieldConstants.bool_expr_asset)
    {
        Label = label;
        Description = tooltip_text;
        //BoolVariableAsset tweakable = new BoolVariableAsset() { };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}