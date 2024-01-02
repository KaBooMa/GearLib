namespace GearLib.Data.Fields;

class IntData : IFieldData
{
    public int value { get; set; }

    public IntData(int value)
    {
        this.value = value;
    }
}