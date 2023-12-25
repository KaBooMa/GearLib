using GearLib.Patches;
using GearLib.Utils;
using Il2CppInterop.Runtime.Injection;
using SmashHammer.Core;
using SmashHammer.GearBlocks.Construction;
using SmashHammer.Physics;
using UnityEngine;
using static SmashHammer.GearBlocks.Construction.PartPointGrid;

namespace GearLib.Parts;

public class Part : MonoBehaviour
{
    public GameObject game_object;

    public Part(string bundle_path, string asset_name, ulong part_uid, string display_name, string category)
    {
        game_object = LoaderUtil.LoadAsset(bundle_path, asset_name);

        // Create mandatory components for new asset
        PartDescriptor descriptor = game_object.AddComponent<PartDescriptor>();
        PartPropertiesBasic points = game_object.AddComponent<PartPropertiesBasic>();
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

        // Setup default variables for new components
        descriptor.displayName = display_name;
        descriptor.category = category;
        descriptor.defaultLayer = new LayerAsset() {name = "Default", index = 0};
        descriptor.dematerialisingLayer = new LayerAsset() {name = "Dematerialising", index = 29};
        descriptor.highlightingLayer = new LayerAsset() {name = "Hightlighting", index = 19};

        // TODO: This list creation might can be shortened. IDE says so but once compiled, it errors. 
        descriptor.intersectionLayerMask = new LayerMaskAsset() {
            layers = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<LayerAsset>(new LayerAsset[] {
                    new LayerAsset() {name = "Selected", index = 27},
                    new LayerAsset() {name = "Frozen", index = 28}
            })
        };
        // END TODO

        PartsDatabase.Add(part_uid, game_object);
        Plugin.Log.LogInfo($"Queued {display_name} to be added.");
    }

    public void AddBehaviour<T>() where T : BehaviourBase
    {
        ClassInjector.RegisterTypeInIl2Cpp<T>();
        game_object.AddComponent<T>();
    }

    public void AddAttachmentPoint(AttachmentTypeFlags attachment_flags, AlignmentFlags alignment_flags, Vector3 position)
    {
        GameObject point_grid = new GameObject("PointGrid");
        point_grid.transform.parent = game_object.transform;
        PartPointGrid part_point_grid = point_grid.AddComponent<PartPointGrid>();
        part_point_grid.gridUnitSize = new Vector3Int(1, 1, 1);
        part_point_grid.Position = position;
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