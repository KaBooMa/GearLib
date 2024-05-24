using SmashHammer.GearBlocks.Construction;
using UnityEngine;
using GearLib.Patches;
using GearLib.Utils;
using System.IO;
using System.Collections.Generic;

namespace GearLib.API;

public class Material
{
    public Material(ulong uid, string path, string texture_name, string display_name, float density, float strength, float bounciness, float dynamic_friction, float static_friction, bool is_paintable = true) 
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom material [{display_name}]");
        // UnityEngine.Material material = LoaderUtil.LoadMaterial(bundle_path, asset_name);
        Plugin.Log.LogWarning(path+"/"+texture_name);
        byte[] bytes = File.ReadAllBytes($"{path}/{texture_name}");
        UnityEngine.Material material = new UnityEngine.Material(Shader.Find("GearBlocks/Standard"));
        if (is_paintable)
        {
            List<string> shader_keywords = new List<string>() { "_PAINT" };
            material.shaderKeywords = shader_keywords.ToArray();
        }

        Texture2D texture = new Texture2D(2, 2);
        texture.filterMode = FilterMode.Trilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.anisoLevel = 16;
        texture.LoadImage(bytes);

        PartMaterialAsset material_asset = new PartMaterialAsset();
        material_asset.displayName = display_name;
        material_asset.density = density;
        material_asset.strength = strength;
        material_asset.isPaintable = is_paintable;
        material_asset.renderMaterial = material;
        material.mainTexture = texture;

        PhysicMaterial physic_material = new PhysicMaterial();
        physic_material.bounciness = bounciness;
        physic_material.dynamicFriction = dynamic_friction;
        physic_material.staticFriction = static_friction;
        material_asset.physicsMaterial = physic_material;
        
        MaterialDatabasePatch.Queue(uid, material_asset);
    }
}