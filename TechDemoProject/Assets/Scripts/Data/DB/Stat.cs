using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct Stat
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Manapoint { get; set; }
        public int Magic { get; set; }
        public int Spirit { get; set; }
        public int Speed { get; set; }
    }
}