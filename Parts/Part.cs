using System.Collections.Generic;
using GearLib.Behaviours;
using GearLib.Links;
using GearLib.Patches;
using GearLib.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SmashHammer.Core;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.GearBlocks.Graphics;
using SmashHammer.Physics;
using UnityEngine;
using static SmashHammer.GearBlocks.Construction.PartPointGrid;

namespace GearLib.Parts;

public class Part : MonoBehaviour
{
    public GameObject game_object;
    public PartDescriptor descriptor;

    public Part(string bundle_path, string asset_name, ulong part_uid, string display_name, string category, float mass = 1f, bool is_paintable = false, bool is_swappable_material = false)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom part [{asset_name}]");
        game_object = LoaderUtil.LoadAsset(bundle_path, asset_name);

        // Create mandatory components for new asset
        descriptor = game_object.AddComponent<PartDescriptor>();
        if (is_paintable)
        {
            game_object.AddComponent<PartPaint>();
            foreach (MeshFilter mesh_filter in game_object.GetComponentsInChildren<MeshFilter>())
            {
                mesh_filter.tag = "PaintableMesh";
            }

            foreach (MeshRenderer mesh_renderer in game_object.GetComponentsInChildren<MeshRenderer>())
            {
                mesh_renderer.material.shader = Shader.Find("GearBlocks/Standard");
                List<string> shader_keywords = new List<string>() { "_PAINT" };
                mesh_renderer.material.shaderKeywords = shader_keywords.ToArray();
            }
        }

        if (!is_swappable_material)
        {
            PartPropertiesBasic properties = game_object.AddComponent<PartPropertiesBasic>();
            properties.mass = mass;
        }
        else
        {
            PartPropertiesSwappableMaterial properties = game_object.AddComponent<PartPropertiesSwappableMaterial>();
            properties.objectMaterialisation = new ObjectMaterialisationAsset();
        }

        game_object.AddComponent<PartPoints>();

        // Destroy any existing colliders that might have been imported
        foreach (MeshCollider collider in game_object.GetComponentsInChildren<MeshCollider>()) Destroy(collider);
        foreach (BoxCollider collider in game_object.GetComponentsInChildren<BoxCollider>()) Destroy(collider);
        foreach (SphereCollider collider in game_object.GetComponentsInChildren<SphereCollider>()) Destroy(collider);
        foreach (CapsuleCollider collider in game_object.GetComponentsInChildren<CapsuleCollider>()) Destroy(collider);
        
        // For each mesh we want to add a new box collider
        foreach (MeshRenderer mesh_renderer in game_object.GetComponentsInChildren<MeshRenderer>())
        {
            GameObject collider = new GameObject("Collider");
            collider.transform.SetParent(game_object.transform);
            BoxCollisionVolume volume = collider.AddComponent<BoxCollisionVolume>();
            volume.size = mesh_renderer.bounds.size;
            volume.center = mesh_renderer.bounds.center;
        }

        // Setup layers
        LayerAsset default_layer = ScriptableObject.CreateInstance<LayerAsset>();
        default_layer.name = "Default";
        default_layer.index = 0;

        LayerAsset no_collide_layer = ScriptableObject.CreateInstance<LayerAsset>();
        no_collide_layer.name = "NoDefaultCollide";
        no_collide_layer.index = 6;

        LayerAsset dematerialising_layer = ScriptableObject.CreateInstance<LayerAsset>();
        dematerialising_layer.name = "Dematerialising";
        dematerialising_layer.index = 29;

        LayerAsset highlighting_layer = ScriptableObject.CreateInstance<LayerAsset>();
        highlighting_layer.name = "Highlighting";
        highlighting_layer.index = 19;

        LayerAsset selected_layer = ScriptableObject.CreateInstance<LayerAsset>();
        selected_layer.name = "Selected";
        selected_layer.index = 27;

        LayerAsset frozen_layer = ScriptableObject.CreateInstance<LayerAsset>();
        frozen_layer.name = "Frozen";
        frozen_layer.index = 28;

        LayerMaskAsset intersection_layer_mask = ScriptableObject.CreateInstance<LayerMaskAsset>();
        intersection_layer_mask.layers.AddItem(selected_layer);
        intersection_layer_mask.layers.AddItem(frozen_layer);

        // Setup part physics
        PartPhysics part_physics = game_object.GetComponent<PartPhysics>();
        part_physics.defaultLayer = default_layer;
        part_physics.noDefaultCollideLayer = no_collide_layer;

        // Setup default variables for new components
        descriptor.displayName = display_name;
        descriptor.category = category;
        descriptor.defaultLayer = default_layer;
        descriptor.dematerialisingLayer = dematerialising_layer;
        descriptor.highlightingLayer = highlighting_layer;
        descriptor.intersectionLayerMask = intersection_layer_mask;

        PartsDatabase.Add(part_uid, game_object);
    }

    public T AddBehaviour<T>() where T : BehaviourBase
    {
        ClassInjector.RegisterTypeInIl2Cpp<T>();
        T behaviour = game_object.AddComponent<T>();
        return behaviour;
    }

    public void AddLinkPoint(string name, string link_type, Vector3 position, bool can_send = true, bool can_receive = true)
    {
        PartLinkTypeAsset asset;
        LinkType.link_types.TryGetValue(link_type, out asset);
        if (!asset) Plugin.Log.LogError($"Missing link type: [{link_type}]!!");

        GameObject link_node_object = new GameObject("LinkNode_"+name);
        link_node_object.transform.parent = game_object.transform;
        link_node_object.transform.position = position;
        link_node_object.layer = 14;
        
        PartLinkNode link_node = link_node_object.AddComponent<PartLinkNode>();
        link_node.type = asset;
        link_node.maxNumOutgoingLinks = can_send ? (byte)255 : (byte)0;
        link_node.maxNumIncomingLinks = can_receive ? (byte)255 : (byte)0;
    }

    public void AddAttachmentPoint(string attachment_name, AttachmentTypeFlags attachment_flags, AlignmentFlags alignment_flags, Vector3 position, Vector3 orientation, Vector3Int size, bool pivot = false)
    {
        GameObject point_grid = new GameObject("PointGrid_"+attachment_name);
        point_grid.transform.parent = game_object.transform;
        
        PartPointGrid part_point_grid = point_grid.AddComponent<PartPointGrid>();
        part_point_grid.gridUnitSize = size;
        part_point_grid.Position = position;
        part_point_grid.Orientation = Quaternion.Euler(orientation);
        part_point_grid.isPivot = pivot;
        part_point_grid.alignmentFlags = alignment_flags;
        part_point_grid.attachmentTypes = attachment_flags;
    }

    public void SetAttachmentSize(string attachment_name, Vector3Int size)
    {
        GameObject point_grid = game_object.transform.Find("PointGrid_"+attachment_name).gameObject;
        PartPointGrid part_point_grid = point_grid.GetComponent<PartPointGrid>();
        part_point_grid.GridUnitSize = size;
    }

    // TODO: THIS FUNCTION IS BUSTED CURRENTLY. DNU
    // Expected result: PointGrid is generated automatically for bottom side of the a part. 
    // Good for simple parts that just need to be mounted to other parts.
    public void AddCalculatedPointGrid(AttachmentTypeFlags attachment_flags)
    {
        BoxCollisionVolume collision = game_object.GetComponent<BoxCollisionVolume>();

        // Add a PointGrid child and PartPointGrid component to allow snapping to other objects
        GameObject point_grid = new GameObject("PointGrid");
        point_grid.transform.parent = game_object.transform;
        PartPointGrid part_point_grid = point_grid.AddComponent<PartPointGrid>();
        Vector3Int grid_size = Vector3Int.CeilToInt(collision.size*10);
        //grid_size.y = 1;
        part_point_grid.gridUnitSize = grid_size;
        part_point_grid.alignmentFlags = AlignmentFlags.IsInterior;
        //part_point_grid.inverseOrientation = Quaternion.Euler(0, 180, 180);
        //part_point_grid.Orientation = Quaternion.Euler(0, 180, 180);
        part_point_grid.Position = new Vector3(0, collision.size.y/2, 0); // THIS CODE PLACED A BOTTOM GRID
        part_point_grid.isPivot = true;
        part_point_grid.attachmentTypes = attachment_flags;
    }
    // END TODO
}