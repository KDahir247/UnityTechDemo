using Tech.Animation.Annotation;
using Unity.Burst;
using Unity.Entities;
using Unity.Kinematica;

namespace Tech.Component
{
    [BurstCompatible]
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, CompileSynchronously = true, Debug = false)]
    public class AnimationSystem : ComponentSystem
    {
        protected override void OnStartRunning()
        {
            Entities
                .WithAll<Kinematica>()
                .ForEach((Entity entity, Kinematica kinematica) =>
                {
                    ref MotionSynthesizer synthesizer = ref kinematica.Synthesizer.Ref;

                    synthesizer
                        .Root
                        .Action()
                        .PlayFirstSequence(synthesizer
                            .Query
                            .Where(Idle.Default));
                });
        }
        
        protected override void OnUpdate()
        {
        }
    }
}