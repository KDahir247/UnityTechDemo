using System;
using MessagePack;
using UnityEngine;

//Doesn't seem to work on SkillInfo
namespace Tech.Data
{
    [Serializable]
    [MessagePackObject(true)]
    public struct SkillData
    {
        [IgnoreMember] public Ulid id;
        public string name;
        public Texture2D image;
        public string description;

        public string skillDescription;
    }
}