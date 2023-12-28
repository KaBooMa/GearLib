using System;
using SmashHammer.GearBlocks.Tweakables;
using SmashHammer.Variables;
using UnityEngine;

namespace GearLib.Core.BehaviourFields;

public class IntField : IntTweakable
{
    public IntField(string label, string tooltip_text = null, int value = 0, int min = 0, int max = 1) : base(label, min, max, value, tooltip_text, FieldConstants.bool_expr_asset) { }
}