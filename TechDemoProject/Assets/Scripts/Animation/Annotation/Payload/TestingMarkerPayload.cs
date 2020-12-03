using Unity.Burst;
using Unity.Entities;
using Unity.Kinematica;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Tech.Animation
{
    [Trait]
    [BurstCompile]
    public struct TestingMarkerPayload : Trait
    {
        public void Execute(ref MotionSynthesizer synthesizer)
        {
            Debug.Log("Called from testing marker");
        }

        [BurstCompile]
        public static void ExecuteSelf(ref TestingMarkerPayload p, ref MotionSynthesizer synthesizer)
        {
            p.Execute(ref synthesizer);
        }
    }
}