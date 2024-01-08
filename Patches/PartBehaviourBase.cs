using HarmonyLib;
using Newtonsoft.Json;
using SmashHammer.GearBlocks.Construction;

namespace GearLib.Patches;

[HarmonyPatch(typeof(PartBehaviourBase), nameof(PartBehaviourBase.Deserialize))]
class PartBehaviourBase_Patch
{
    static bool Prefix(PartBehaviourBase __instance, JsonReader __0)
    {
        while (__0.Read() && __0.TokenType != JsonToken.EndObject)
        {
            if (__0.TokenType == JsonToken.PropertyName)
            {
                __instance.LoadJsonProperty(__0, __0.Value.ToString());
            }
        }

        return false;
    }
}