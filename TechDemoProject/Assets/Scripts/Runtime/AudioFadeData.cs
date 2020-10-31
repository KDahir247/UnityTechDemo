using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using System;
using Unity.Collections;
using Unity.Entities.UniversalDelegates;
using UnityEngine.Serialization;

namespace Tech.Runtime
{
    [Serializable]
   [GenerateAuthoringComponent]
    public struct AudioFadeData : IComponentData
    {
        [FormerlySerializedAs("FadeDuration")] 
        public float fadeDuration;
    }
}