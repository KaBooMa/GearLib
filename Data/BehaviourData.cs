using System.Collections.Generic;

namespace GearLib.Data;

class BehaviourData
{
    public Dictionary<string, object> tweakables { get; set; } = new Dictionary<string, object>();

    public void AddTweakable(string name, object value) 
    {
        tweakables.Add(name, value);
    }
}