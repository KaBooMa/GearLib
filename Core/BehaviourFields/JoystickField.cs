using HarmonyLib;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Input;
using SmashHammer.UI;
using SmashHammer.Variables;

namespace GearLib.Core.BehaviourFields;

public class JoystickField : JoystickAxisTweakable
{

    public JoystickField(string label, string tooltip_text = null) : base(label, new JoystickAxis(), tooltip_text, FieldConstants.bool_expr_asset)
    {
        Value = value;
        Label = label;
        Description = tooltip_text;
        //BoolVariableAsset tweakable = new BoolVariableAsset() { };
        TweakAllowed.boolVars = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset>(
            new BoolVariableAsset[] { FieldConstants.bool_variable_asset }
        );
    }
}