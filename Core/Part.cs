using GearLib.Patches;
using GearLib.Utils;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using SmashHammer.Core;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.Physics;
using UnityEngine;
using static SmashHammer.GearBlocks.Construction.PartPointGrid;

namespace GearLib.Core;

public class Part : MonoBehaviour
{
    public GameObject game_object;
    public PartDescriptor descriptor;
    public PartPropertiesBase properties;

    public Part(string bundle_path, string asset_name, ulong part_uid, string display_name, string category, float mass = 1f)
    {
        game_object = LoaderUtil.LoadAsset(bundle_path, asset_name);

        // Create mandatory components for new asset
        descriptor = game_object.AddComponent<PartDescriptor>();
        PartPropertiesBasic basic_properties = game_object.AddComponent<PartPropertiesBasic>();
        basic_properties.mass = mass;
        properties = basic_properties;
        game_object.AddComponent<PartPoints>();
        

        // TODO: Clean up this old collider handling
        // Current handling converts mesh dimensions to a box collider
        // This means custom parts are squared, even circular ones

        // foreach (BoxCollider collider in game_object.GetComponentsInChildren<BoxCollider>())
        // {
        //     BoxCollisionVolume volume = collider.transform.parent.gameObject.AddComponent<BoxCollisionVolume>();
        //     volume.size = collider.transform.localScale;
        //     volume.center = collider.transform.position;
        //     Destroy(collider);
        // }

        // foreach (CapsuleCollider collider in game_object.GetComponentsInChildren<CapsuleCollider>())
        // {
        //     CapsuleCollisionVolume volume = collider.transform.parent.gameObject.AddComponent<CapsuleCollisionVolume>();
        //     volume.radius = collider.radius;
        //     volume.height = collider.height;
        //     volume.center = collider.center;
        //     Destroy(collider);
        // }
        foreach (MeshRenderer collider in game_object.GetComponentsInChildren<MeshRenderer>())
        {
            BoxCollisionVolume volume = collider.transform.parent.gameObject.AddComponent<BoxCollisionVolume>();
            volume.size = collider.bounds.size;
            volume.center = collider.bounds.center;
        }

        foreach (MeshCollider collider in game_object.GetComponentsInChildren<MeshCollider>())
        {
            Destroy(collider);
        }
        // END TODO

        // Setup layers
        LayerAsset default_layer = ScriptableObject.CreateInstance<LayerAsset>();
        default_layer.name = "Default";
        default_layer.index = 0;

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

        // Setup default variables for new components
        descriptor.displayName = display_name;
        descriptor.category = category;
        descriptor.defaultLayer = default_layer;
        descriptor.dematerialisingLayer = dematerialising_layer;
        descriptor.highlightingLayer = highlighting_layer;

        // TODO: This list creation might can be shortened. IDE says so but once compiled, it errors. 
        descriptor.intersectionLayerMask = intersection_layer_mask;
        // END TODO

        PartsDatabase.Add(part_uid, game_object);
        Plugin.Log.LogInfo($"Queued {display_name} to be added.");
    }

    public T AddBehaviour<T>() where T : BehaviourBase
    {
        ClassInjector.RegisterTypeInIl2Cpp<T>();
        T behaviour = game_object.AddComponent<T>();
        descriptor.Behaviours.AddItem(behaviour);
        return behaviour;
    }

    public void AddLinkPoint(string link_name, LinkType link_type, Vector3 position, bool can_send, bool can_receive)
    {
        GameObject link_node_object = new GameObject("LinkNode_"+link_name);
        link_node_object.transform.parent = game_object.transform;
        link_node_object.transform.position = position;
        link_node_object.layer = 14;

        PartLinkNode link_node = link_node_object.AddComponent<PartLinkNode>();
        link_node.type = link_type;
        link_node.maxNumOutgoingLinks = can_send ? (byte)255 : (byte)0;
        link_node.maxNumIncomingLinks = can_receive ? (byte)255 : (byte)0;
    }

    public void AddAttachmentPoint(string attachment_name, AttachmentTypeFlags attachment_flags, AlignmentFlags alignment_flags, Vector3 position, Vector3 orientation)
    {
        GameObject point_grid = new GameObject("PointGrid_"+attachment_name);
        point_grid.transform.parent = game_object.transform;
        
        PartPointGrid part_point_grid = point_grid.AddComponent<PartPointGrid>();
        part_point_grid.gridUnitSize = new Vector3Int(1, 1, 1);
        part_point_grid.Position = position;
        part_point_grid.Orientation = Quaternion.Euler(orientation);
        part_point_grid.isPivot = true;
        part_point_grid.alignmentFlags = alignment_flags;
        part_point_grid.attachmentTypes = attachment_flags;
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