using SmashHammer.Variables;
using UnityEngine;

namespace GearLib.Behaviours.Fields;

public class FieldConstants
{
    public static BoolExprAsset bool_expr_asset;
    public static Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<BoolVariableAsset> bool_vars;

    static FieldConstants()
    {
        BoolVariableAsset bool_variable_asset = ScriptableObject.CreateInstance<BoolVariableAsset>();
        bool_variable_asset.name = "LimitedPartBehaviourTweaking";
        bool_variable_asset.reset = true;
        bool_variable_asset.syncToNetworkClients = true;

        bool_expr_asset = ScriptableObject.CreateInstance<BoolExprAsset>();
        bool_expr_asset.name = "TweakPartBehavioursExpr";
        bool_expr_asset.expr = "A !";
        bool_vars = new BoolVariableAsset[] { bool_variable_asset };

        bool_expr_asset.boolVars = bool_vars;
    }
}