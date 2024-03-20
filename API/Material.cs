using SmashHammer.GearBlocks.Construction;
using UnityEngine;
using GearLib.Patches;
using GearLib.Utils;

namespace GearLib.API;

public class Material
{
    public Material(ulong material_uid, string bundle_path, string asset_name, string display_name, float density, float strength, float bounciness, float dynamic_friction, float static_friction, bool is_paintable = true) 
    {
        UnityEngine.Material material = LoaderUtil.LoadMaterial(bundle_path, asset_name);

        PartMaterialAsset material_asset = new PartMaterialAsset();
        material_asset.displayName = display_name;
        material_asset.density = density;
        material_asset.isPaintable = is_paintable;
        material_asset.renderMaterial = material;

        PhysicMaterial physic_material = new PhysicMaterial();
        physic_material.bounciness = bounciness;
        physic_material.dynamicFriction = dynamic_friction;
        physic_material.staticFriction = static_friction;

        material_asset.physicsMaterial = physic_material;
        
        PartsDatabase.QueueMaterial(material_uid, material_asset);
    }
}