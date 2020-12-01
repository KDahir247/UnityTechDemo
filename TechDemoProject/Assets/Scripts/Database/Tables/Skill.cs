using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [MemoryTable("Image")]
    [MessagePackObject(true)]
    public class Skill : IMessagePackSerializationCallbackReceiver
    {
        //attributes
        [IgnoreMember] public Ulid Id { get; set; }
    
        [PrimaryKey] public string Name { get; set; }

        public string Description { get; set; }
        
        public string SkillDescription { get; set; }
        
        public byte[] ImageBytes { get; set; }

        public SkillInfo SkillInfo { get; set; }
        
        [SecondaryKey(0)] public int Index { get; set; }

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