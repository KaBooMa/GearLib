using System;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using Il2CppSystem.Collections.Generic;
using MoonSharp.Interpreter;
using SmashHammer.GearBlocks.Construction;
using UnityEngine;

namespace GearLib.API.Lua;

class LuaPart : Il2CppSystem.Object
{
    public Il2CppReferenceField<PartDescriptor> part;

    static LuaPart()
    {
        ClassInjector.RegisterTypeInIl2Cpp<LuaPart>();
        UserData.RegisterType<LuaPart>();

        // Register Unity classes that throw errors otherwise
        UserData.RegisterType<Rigidbody>();
        UserData.RegisterType<BoxCollider>();
    }

    public LuaPart(IntPtr ptr) : base(ptr) { }
    public LuaPart(PartDescriptor part) : base(ClassInjector.DerivedConstructorPointer<LuaPart>())
    {
        ClassInjector.DerivedConstructorBody(this);
        this.part.Set(part);
    }

    /// <summary>
    /// Gets the attachments associated with the part
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public LuaRigidbody GetRigidbody()
    {
        return new LuaRigidbody(part.Get().parentComposite.GetComponent<Rigidbody>());
    }

    /// <summary>
    /// Gets the attachments associated with the part
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public List<LuaAttachment> GetAttachments()
    {
        List<LuaAttachment> attachments = new List<LuaAttachment>();
        foreach (AttachmentBase attachment in part.Get().Attachments.associatedAttachments)
        {
            attachments.Add(new LuaAttachment(attachment));
        }
        return attachments;
    }

    /// <summary>
    /// Gets the attachments associated with the part
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public LuaTransform GetTransform()
    {
        return new LuaTransform(part.Get().transform);
    }

    /// <summary>
    /// Gets the parts linked with the part over a specific link
    /// </summary>
    /// <param name="part"></param>
    /// <param name="link_name">The name supplied in the editor for this link</param>
    /// <returns>List<LuaPart></returns>
    public DynValue GetLinkedParts(string link_name)
    {
        List<LuaPart> linked_parts = new List<LuaPart>();
        foreach (PartLinkNode link_node in part.Get().LinkNodes)
        {
            if (link_node.name == link_name)
            {
                foreach (PartLinkNode linked_node in link_node.LinkedNodes)
                {
                    linked_parts.Add(new LuaPart(linked_node.Part));
                }
            }
        }

        return DynValue.FromObject(null, linked_parts);
    }

    /// <summary>
    /// Gets a attachment by name associated with the part
    /// </summary>
    /// <param name="part"></param>
    /// <param name="name"></param>
    /// <returns>Returns the Lua___Attachment object equivalent</returns>
    public LuaAttachment GetAttachment(string name)
    {
        foreach (AttachmentBase attachment in part.Get().Attachments.associatedAttachments)
        {
            if (attachment.ownerPartPointGrid.name.Replace("PointGrid_", "") == name || 
                attachment.connectedPartPointGrid.name.Replace("PointGrid_", "") == name)
                    return new LuaAttachment(attachment);
        }

        return null;
    }

    /// <summary>
    /// Gets a part attached to another part by the attachment name
    /// </summary>
    /// <param name="attachment_name"></param>
    /// <returns>LuaPart</returns>
    public DynValue GetAttachedPart(string attachment_name)
    {
        foreach (AttachmentBase attachment in part.Get().Attachments.associatedAttachments)
        {
            if (attachment.ownerPartPointGrid.name.Replace("PointGrid_", "") == attachment_name)
                return DynValue.FromObject(null, new LuaPart(attachment.ConnectedPart));
            else if (attachment.connectedPartPointGrid.name.Replace("PointGrid_", "") == attachment_name)
                return DynValue.FromObject(null, new LuaPart(attachment.OwnerPart));
        }

        return null;
    }

    /// <summary>
    /// Gets the display name of a part
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public string GetDisplayName()
    {
        return part.Get().displayName;
    }

    /// <summary>
    /// Gets the strength of a part
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public float GetStrength()
    {
        return part.Get().Strength;
    }
    
    /// <summary>
    /// Returns if a part is paintable
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public bool GetPaintable()
    {
        return part.Get().IsPaintable;
    }

    /// <summary>
    /// Sets the part collision on/off with another part
    /// </summary>
    /// <param name="other_part">LuaPart</param>
    /// <param name="part2"></param>
    /// <param name="enabled"></param>
    /// 
    public void SetCollision(DynValue other_part, bool enabled)
    {
        LuaPart other_part_object = other_part.ToObject<LuaPart>();
        foreach (Collider collider1 in part.Get().GetComponentsInChildren<Collider>())
        {
            foreach (Collider collider2 in other_part_object.part.Get().GetComponentsInChildren<Collider>()) {
                Physics.IgnoreCollision(collider1, collider2, !enabled);
                Physics.IgnoreCollision(collider2, collider1, !enabled);
            }
        }
    }
}