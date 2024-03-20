using System;

namespace GearLib.API.Fields;

/// <summary>
/// Interface for adding properties to their Part Behaviour in game for players. This uses the built-in existing properties available on Parts to customize them.<br/>
/// MODDERS: Do not use this. Instead, use the other fields inheriting from this. Example below on how to use these fields:<br/>
/// [IntField(label = "Timing", tooltip_text = "Offset degrees from TDC to fire piston", initial_value = 5, minimum_value = -30, maximum_value = 30)]
/// public int timing;
/// </summary>
public class IField : Attribute { }