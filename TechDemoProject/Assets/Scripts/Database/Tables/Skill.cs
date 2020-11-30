using System;
using MasterMemory;
using MessagePack;
using UnityEngine;

namespace Tech.DB
{
    [MemoryTable("Image"), MessagePackObject(true)]
    public class Skill : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public int index { get; set; }

        [SecondaryKey(0)] public string name { get; set; }

        //attributes
        public byte[] Bytes;

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {

        }

    }
}