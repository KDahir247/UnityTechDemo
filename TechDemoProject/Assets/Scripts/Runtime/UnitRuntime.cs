﻿using System;
using Unity.Entities;
using Unity.Kinematica;
using UnityEngine.Serialization;

namespace Tech.ECS
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct UnitRuntime : IComponentData
    {
        //test
        public int skillIndex;
        public Ulid ulid;
        public bool enabled;
    }
}