using System;
using MessagePack;
using Tech.Data.DB;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Data
{
    [Serializable]
    [MessagePackObject(true)]
    public struct AbilityData
    {
        [IgnoreMember] public Ulid id;
        public string name;
        public int innocentCost;
        public string description;
        public string abilityDescription;

        public Texture2D image;
    }
}