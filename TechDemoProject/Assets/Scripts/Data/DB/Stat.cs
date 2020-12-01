using System;
using MessagePack;
using UnityEngine.Serialization;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct Stat
    {
        public uint Health { get; set; }
        public uint Attack { get; set; }
        public uint Defence { get; set; }
        public uint Manapoint { get; set; }
        public uint Magic { get; set; }
        public uint Spirit { get; set; }
        public uint Speed { get; set; }
    }
}