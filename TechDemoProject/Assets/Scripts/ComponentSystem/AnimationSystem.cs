using Tech.Animation;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Kinematica;
using UnityEngine;

namespace Tech.ECS
{
    //TODO clean
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct KinematicaSoloJob : IJob
    {
        public MemoryRef<MotionSynthesizer> motionSynthesizer;
        private MotionSynthesizer MotionSynthesizer => motionSynthesizer.Ref;
        
        public TaskReference IdleTaskReference;
        public TaskReference LocomotionTaskReference;

        public int skillIndex;
        
        public void Execute()
        {
            //Add logic here
            switch (skillIndex)
            {
                case 1:
                    MotionSynthesizer
                        .Root
                        .Action()
                        .PlayFirstSequence(MotionSynthesizer
                            .Query
                            .Where(Locomotion.Default)
                            .And(Skill1.Default));
                    break;
                
                case 2:
                    MotionSynthesizer
                        .Root
                        .Action()
                        .PlayFirstSequence(MotionSynthesizer
                            .Query
                            .Where(Locomotion.Default)
                            .And(Skill2.Default));
                    break;
                
                case 3:
                    MotionSynthesizer
                        .Root
                        .Action()
                        .PlayFirstSequence(MotionSynthesizer
                            .Query
                            .Where(Locomotion.Default)
                            .And(Skill3.Default));
                    break;
            }
        }
    }
    
    
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    public sealed class AnimationSystem : ComponentSystem
    {
        protected override void OnStartRunning()
        {
            Entities
                .WithAll<Kinematica>()
                .ForEach((Entity entity, 
                    Kinematica kinematica,
                    ref UnitRuntime unitRuntime) =>
                {
                    ref MotionSynthesizer motionSynthesizer = ref kinematica.Synthesizer.Ref;

                    motionSynthesizer
                        .Root
                        .Action()
                        .PlayFirstSequence(motionSynthesizer
                            .Query
                            .Where(Locomotion.Default)
                            .And(Idle.Default));
                });
        }
        
        protected override void OnUpdate()
        {
            Entities
                .WithAll<Kinematica>()
                .ForEach((Entity entity,
                    Kinematica kinematica,
                    ref UnitRuntime unitRuntime) =>
                {
                    //Something happens
                    if (unitRuntime.enabled && unitRuntime.skillIndex > 0)
                    {
                        KinematicaSoloJob job = new KinematicaSoloJob
                        {
                            motionSynthesizer = kinematica.Synthesizer,
                            skillIndex = unitRuntime.skillIndex
                        };

                        kinematica.AddJobDependency(job.Schedule());

                        unitRuntime.skillIndex = 0;
                    }
                });
        }
    }
}

