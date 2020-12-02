using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct ItemInfo
    {
        public uint Level { get; set; }
        public Rarity Rarity { get; set; }
        public ItemType ItemType { get; set; }
        public Ailment CureAilment { get; set; }
        public Ailment InflictAilment { get; set; }
        public Target Target { get; set; }
    }
}