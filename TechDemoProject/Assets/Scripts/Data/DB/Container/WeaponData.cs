using System;
using MessagePack;
using UnityEngine;

namespace Tech.Data
{
    [Serializable]
    [MessagePackObject(true)]
    public struct WeaponData
    {
        public byte[] id;
        public string name;
        public string description;
        public Texture2D image;

    }
}