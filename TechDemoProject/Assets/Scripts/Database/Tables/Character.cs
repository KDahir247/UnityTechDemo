using System;
using MasterMemory;
using MessagePack;

namespace Tech.DB
{
    [MemoryTable("character"), MessagePackObject(true)]
    public class Character : IMessagePackSerializationCallbackReceiver
    {

        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public int Index { get; set; }

        [SecondaryKey(0)] public string Name { get; set; }

        public string Description { get; set; }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }

        public Character(int Index, string Name, string Description)
        {
            this.Index = Index;
            this.Name = Name;
            this.Description = Description;
        }


    }
}