using System;
using MessagePack;
using UnityEngine.Serialization;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct Stat
    {
        [FormerlySerializedAs("Health")] public uint health;
        [FormerlySerializedAs("Attack")] public uint attack;
        [FormerlySerializedAs("Defence")] public uint defence;

        [FormerlySerializedAs("Manapoint")] public uint manapoint;
        [FormerlySerializedAs("Magic")] public uint magic;
        [FormerlySerializedAs("Spirit")] public uint spirit;

        [FormerlySerializedAs("Speed")] public uint speed;
    }
}