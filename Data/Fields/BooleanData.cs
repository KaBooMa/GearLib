namespace GearLib.Data.Fields;

class BooleanData : IFieldData
{
    public bool value { get; set; }

    public BooleanData(bool value)
    {
        this.value = value;
    }
}