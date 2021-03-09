using Tech.Animation;
using UniRx;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Kinematica;

namespace Tech.ECS
{
    //TODO clean up and find a better solution then using a switch statement
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, CompileSynchronously = false, Debug = false)]
    internal struct KinematicaSoloJob : IJob
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

    [BurstCompile(FloatPrecision.Low, FloatMode.Fast, CompileSynchronously = false, Debug = false)]
    public sealed class AnimationSystem : SystemBase
    {

        protected override void OnUpdate()
        {
            Entities
                .WithAll<Kinematica>()
                .ForEach((Entity entity,
                    Kinematica kinematica,
                    ref UnitRuntime unitRuntime) =>
                {
                    if (unitRuntime.enabled && unitRuntime.skillIndex > 0)
                    {
                        var job = new KinematicaSoloJob
                        {
                            motionSynthesizer = kinematica.Synthesizer,
                            skillIndex = unitRuntime.skillIndex
                        };

                        kinematica.AddJobDependency(job.Schedule());

                        unitRuntime.skillIndex = 0;
                    }
                })
                .WithoutBurst()
                .Run();
        }
    }
}