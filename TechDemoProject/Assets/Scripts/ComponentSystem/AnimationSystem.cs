using Tech.Animation.Annotation;
using Tech.Event;
using UniRx;
using Unity.Burst;
using Unity.Entities;
using Unity.Kinematica;
using UnityEngine;

namespace Tech.Component
{
    [BurstCompatible]
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, CompileSynchronously = true, Debug = false)]
    public class AnimationSystem : ComponentSystem
    {
        protected override void OnStartRunning()
        {
           
            Entities.ForEach((Entity entity, Kinematica kinematica) =>
            {
                Debug.Log("works");
            });
            
        }
        
        protected override void OnUpdate()
        {
        }
    }
}

