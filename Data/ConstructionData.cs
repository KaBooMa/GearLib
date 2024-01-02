using System.Collections.Generic;

namespace GearLib.Data;

class ConstructionData
{
    public Dictionary<ushort, PartData> parts { get; set; } = new Dictionary<ushort, PartData>();

    public void AddPart(ushort id, PartData data) 
    {
        parts.Add(id, data);
    }
}