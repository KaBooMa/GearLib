namespace GearLib.Data.Fields;

class StringData : IFieldData
{
    public string value { get; set; }

    public StringData(string value)
    {
        this.value = value;
    }
}