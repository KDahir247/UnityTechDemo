﻿using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [MemoryTable("enemy")]
    [MessagePackObject(true)]
    public class Enemy : IMessagePackSerializationCallbackReceiver
    {
        public byte[] Id { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        [PrimaryKey] public string Name { get; set; }

        [SecondaryKey(0)] public int Index { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        public string Address { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public EnemyInfo EnemyInfo { get; set; }

        //TODO might change
        public Weapon Weapon { get; set; }
        public Equipment[] Equipment { get; set; }
        public Ability Ability { get; set; } //final skill for character
        public Skill[] Skills { get; set; }

        public void OnBeforeSerialize()
        {
            //Called Before Serialization
        }

        public void OnAfterDeserialize()
        {
            //Called After Deserialization
        }
    }
}