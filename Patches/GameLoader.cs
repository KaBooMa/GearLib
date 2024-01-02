using System.Threading.Tasks;
using GearLib.Data;
using HarmonyLib;
using SmashHammer.GearBlocks.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GearLib.Patches;

[HarmonyPatch(typeof(GameLoader))]
class GameLoader_Patch : MonoBehaviour
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameLoader.OnSaveScene))]
    private static void OnSaveScene_Postfix(GameLoader __instance, Il2CppSystem.Object obj)
    {
        Task.Run(async () => {
            while (!GameObject.Find("/ConstructionsPool"))
                await Task.Delay(100);

            SaveData.Instance.Save();
        });
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(GameLoader.OnSceneLoaded))]
    private static void OnLoadScene_Postfix(GameLoader __instance, Scene scene, LoadSceneMode mode)
    {
        Task.Run(async () => {
            while (!GameObject.Find("/ConstructionsPool"))
                await Task.Delay(100);

            SaveData.Instance.Load();
        });
    }
}