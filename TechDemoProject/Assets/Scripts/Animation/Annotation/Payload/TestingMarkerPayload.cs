using Unity.Burst;
using Unity.Kinematica;
using UnityEngine;

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
        public static void ExecuteSelf(ref TestingMarkerPayload payload, ref MotionSynthesizer synthesizer)
        {
            payload.Execute(ref synthesizer);
        }
    }
}