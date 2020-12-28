using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    //todo use string compare
    [Serializable]
    [MemoryTable("equipment")]
    [MessagePackObject(true)]
    public class Equipment : IMessagePackSerializationCallbackReceiver
    {
        public byte[] Id { get; set; }

        [PrimaryKey] public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public EquipmentInfo EquipmentInfo { get; set; }

        [SecondaryKey(2)] public int Index { get; set; }


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