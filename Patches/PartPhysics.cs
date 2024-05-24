using GearLib.Classes;
using HarmonyLib;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.Physics;
using UnityEngine;

namespace GearLib.Patches;

[HarmonyPatch(typeof(PartPhysics), nameof(PartPhysics.InitMassProperties))]
class PartPhysicsPatch
{
    static void Postfix(PartPhysics __instance, float mass)
    {
        foreach (CollisionVolumeBase volume in __instance.collisionVolumes)
        {
            if (volume is MeshCollisionVolume)
            {
                MeshCollisionVolume mesh_volume = (MeshCollisionVolume)volume;
                __instance.massProperties.SetFromMeshCollider(mesh_volume.mesh_collider, Vector3.zero, Quaternion.identity, mass * 0.01f, true);
                return;
            }
        }
    }
}