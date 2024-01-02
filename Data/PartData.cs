using System.Collections.Generic;

namespace GearLib.Data;

class PartData
{
    public Dictionary<ushort, BehaviourData> behaviours { get; set; } = new Dictionary<ushort, BehaviourData>();

    public void AddBehaviour(ushort id, BehaviourData data) 
    {
        behaviours.Add(id, data);
    }
}