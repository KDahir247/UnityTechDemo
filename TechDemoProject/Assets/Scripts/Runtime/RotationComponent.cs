using System;
using DG.Tweening;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Tech.Component
{
    [GenerateAuthoringComponent]
    [Serializable]
    public struct RotationComponent : IComponentData
    {
        [FormerlySerializedAs("RotationSpeed")]
        public float rotationSpeed;

        public float3 direction;
        public float rotationDuration;
        [FormerlySerializedAs("RotateMode")] public RotateMode rotateMode;


        public Ease rotateEase;
        public float rotationAmplitude;
        public float rotationPeriod;
    }
}