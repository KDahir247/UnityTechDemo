using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [Serializable]
    [MemoryTable("Image")]
    [MessagePackObject(true)]
    public class Skill : IMessagePackSerializationCallbackReceiver
    {
        //attributes
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }

        [NonUnique] public string Description { get; set; }

        [NonUnique] public string SkillDescription { get; set; }

        [NonUnique] public byte[] ImageBytes { get; set; }

        [NonUnique] public SkillInfo SkillInfo { get; set; }

        [NonUnique] [SecondaryKey(0)] public int Index { get; set; }

        public void OnBeforeSerialize()
        {
            //Called Before Serialization
        }

        public void OnAfterDeserialize()
        {
            //Called After DeSerialization 
        }
    }
}