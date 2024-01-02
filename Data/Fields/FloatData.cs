namespace GearLib.Data.Fields;

class FloatData : IFieldData
{
    public float value { get; set; }

    public FloatData(float value)
    {
        this.value = value;
    }
}