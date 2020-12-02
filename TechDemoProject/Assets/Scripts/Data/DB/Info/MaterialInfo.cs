using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct MaterialInfo
    {
        public uint Level { get; set; }
        public Rarity Rarity { get; set; }

        //Can add more properties for Material later when known.
    }
}