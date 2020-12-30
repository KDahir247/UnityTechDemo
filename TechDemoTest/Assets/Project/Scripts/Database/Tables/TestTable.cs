using MasterMemory;
using MessagePack;

[MemoryTable("Test")][MessagePackObject(true)]
public sealed class TestTable : IMessagePackSerializationCallbackReceiver
{
    [PrimaryKey]
    public string Name { get; set; }
    [SecondaryKey(1)]
    public int Index { get; set; }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}
