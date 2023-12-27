using Il2CppInterop.Runtime;
using SmashHammer.Variables;
using UnityEditor.Experimental;
using UnityEngine;

namespace GearLib.Core.BehaviourFields;

public class FieldConstants
{
    public static BoolVariableAsset bool_variable_asset; // = ScriptableObject.CreateInstance<BoolVariableAsset>(); //new BoolVariableAsset() { reset = true, syncToNetworkClients = true };
    public static BoolExprAsset bool_expr_asset; // = //(BoolExprAsset)ScriptableObject.CreateInstance(Il2CppType.Of<BoolExprAsset>()) { expr = "" };//new BoolExprAsset() { expr = "A !" };

    static FieldConstants()
    {
        bool_variable_asset = ScriptableObject.CreateInstance<BoolVariableAsset>();
        bool_expr_asset = ScriptableObject.CreateInstance<BoolExprAsset>();
        bool_expr_asset.expr = "A !";
    }
}