using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    //todo use string compare
    [MemoryTable("equipment")]
    [MessagePackObject(true)]
    public class Equipment : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }

        [NonUnique] public Stat Stat { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        [SecondaryKey(0)] [NonUnique] public uint Level { get; set; }

        [SecondaryKey(1)] [NonUnique] public EquipmentType EquipmentType { get; set; }

        [SecondaryKey(2)] public int Index { get; set; }

        [NonUnique] public Rarity Rarity { get; set; }

        //stats // done
        //name //done
        //passive
        //description // done
        //image //done
        //level //done
        //restriction // done
        //rarity // done

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