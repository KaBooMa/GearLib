using System;
using System.Collections.Generic;
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

namespace GearLib.API;

/// <summary>
/// Base class to use when creating a new Part.   <br/><br/>
/// These can be created using:  <br/>
/// Part new_part = new Part(args);  <br/><br/>
/// or by inheriting like so:  <br/>
/// class NewPart : Part  <br/>
/// {  <br/>
///   public NewPart() : base(args)  <br/>
///   {  <br/>
///       // Your code here  <br/>
///   }  <br/>
/// }  <br/>
/// </summary>
public class Part
{
    public GameObject game_object;
    public PartDescriptor descriptor;

    // This cube is loaded from a bundle and will be replaced as new parts are generated
    // This is a HACK used to avoid GC on the GameObject...
    public static GameObject loading_object = LoaderUtil.LoadObject("GearLib/gearlib", "Cube");

    /// <summary>
    /// Creates a new Part within the game. No attachment points will be available unless specified.
    /// </summary>
    /// <param name="bundle_path">Path to the bundle within your mod folder. For example "CombustionMotors/assets/my_asset_bundle"</param>
    /// <param name="asset_name">The name of your asset within the bundle.</param>
    /// <param name="part_uid">Unique ID for your part. This must be unique even across other mods, so randomize a big number!</param>
    /// <param name="display_name">The name that will be used when players interact with it in game.</param>
    /// <param name="category">String ID representation of the category you want it listed in. Default categories in game are: (Area, Blocks, Bodies, Brakes & Clutches, Checkpoints, Connectors, Control Wheels, Electronics, Gears, Lights, Linear Actuators, Motors, Power, Props, Pulleys, Seats, Suspension, Wheels)</param>
    /// <param name="mass">Weight of your part</param>
    /// <param name="is_paintable">Determines if the part is paintable. Default is false</param>
    /// <param name="is_swappable_material">Determines if the part material can be changed. Default is false</param>
    /// <returns>
    /// Returns your new Part object. Further methods can be called for adding attachments, links, etc.
    /// </returns>
    public Part(ulong part_uid, string display_name, string category, float mass = 1f, string bundle_path = null, string asset_name = null, string asset_path = null, string asset = null, bool is_paintable = false, bool is_swappable_material = false)
    {
        Plugin.Log.LogInfo($"{GetType().Name}: Adding custom part [{display_name}]");

        if (bundle_path != null) 
        {
            game_object = LoaderUtil.LoadObject(bundle_path, asset_name);
        } 
        else if (asset_path != null || asset != null) 
        {
            // Create our new game object w/ parent for hack
            game_object = new GameObject(display_name);
            game_object.transform.SetParent(loading_object.transform);

            // Generate a child for mesh data
            GameObject model = new GameObject("Model");
            model.transform.SetParent(game_object.transform);
            MeshFilter new_mesh_filter = model.AddComponent<MeshFilter>();
            if (asset != null)
            {
                new_mesh_filter.mesh = LoaderUtil.LoadMeshFromOBJData(asset.Split("\n"));
            }
            else 
            {
                new_mesh_filter.mesh = LoaderUtil.LoadMeshFromOBJ(asset_path, asset_name);
            }
            model.AddComponent<MeshRenderer>();
        } 
        else 
        {
            throw new Exception($"{display_name} doesn't have a asset_name or bundle_path specified!");
        }

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
        foreach (MeshCollider collider in game_object.GetComponentsInChildren<MeshCollider>()) GameObject.Destroy(collider);
        foreach (BoxCollider collider in game_object.GetComponentsInChildren<BoxCollider>()) GameObject.Destroy(collider);
        foreach (SphereCollider collider in game_object.GetComponentsInChildren<SphereCollider>()) GameObject.Destroy(collider);
        foreach (CapsuleCollider collider in game_object.GetComponentsInChildren<CapsuleCollider>()) GameObject.Destroy(collider);
        
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

        PartsDatabase.QueuePart(part_uid, game_object);
    }

    /// <summary>
    /// Adds a Behaviour to your Part. Behaviours are logic that runs on your part. A part can have multiple Behaviours.
    /// </summary>
    /// <param name="T">T Represents your Behaviour class with all it's logic.</param>
    /// <returns>
    /// Returns your instanced Behaviour object on the Part.
    /// </returns>
    public T AddBehaviour<T>() where T : Behaviour
    {
        ClassInjector.RegisterTypeInIl2Cpp<T>();
        T behaviour = game_object.AddComponent<T>();
        return behaviour;
    }

    /// <summary>
    /// Adds a Link Point to your Part. This allows players to connect Parts together for logic.
    /// </summary>
    /// <param name="name">The name of your Link Point. This is just a unique name for you to utilize in your Behaviours.</param>
    /// <param name="link_type">Unique string ID of the link type you'll be using.</param>
    /// <param name="position">The position in 3D space to place your link point.</param>
    /// <param name="can_send">Determines if you can send data from this link. Default is true</param>
    /// <param name="can_receive">Determines if you can receive data from this link. Default is true</param>
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

    /// <summary>
    /// Adds an Attachment Point to your Part. This allows players to attach it to things or things to it.
    /// </summary>
    /// <param name="attachment_name">The name of your Attachment Point. This is just a unique name for you to utilize in your Behaviours.</param>
    /// <param name="attachment_flags">Class from the base game. This determines what style attachment you'll have, for example, knuckle joints, spherical, fixed, etc.</param>
    /// <param name="alignment_flags">Class from the base game. This determines the alignment required for your attachment, for example, bi-directional, only 90 degrees, only 180, etc.</param>
    /// <param name="position">Unit position on your part this attachment will be added.</param>
    /// <param name="orientation">The unit direction your attachment will face, so to speak.</param>
    /// <param name="size">The size in units your attachment will be.</param>
    /// <param name="pivot">Determines if other parts can attach to your part, or only your part can attach to them. Default is false</param>
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

    /// <summary>
    /// Allows you to alter the unit size of your attachments on the Part.
    /// </summary>
    /// <param name="attachment_name">The name of your Attachment Point.</param>
    /// <param name="size">The size in units your attachment will be.</param>
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