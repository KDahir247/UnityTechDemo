using System;
using MessagePack;
using UnityEngine;

namespace Tech.Data
{
    [Serializable]
    [MessagePackObject(true)]
    public struct EnemyData
    {
        [IgnoreMember] public Ulid id;
        public string name;
        public string description;
        public Texture2D image;

        public WeaponData weaponData;
        public EquipmentData[] equipmentDatas;
        public AbilityData abilityData;
        public SkillData[] skillDatas;

    }
}