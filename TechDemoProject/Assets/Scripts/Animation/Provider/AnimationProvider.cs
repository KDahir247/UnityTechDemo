using Unity.Burst;
using Unity.Jobs;
using Unity.Kinematica;
using Unity.SnapshotDebugger;
using UnityEngine;

namespace Tech.Animation.Provider
{
    [BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
    struct KinematicaSoloJob : IJob
    {
        public MemoryRef<MotionSynthesizer> motionSynthesizer;
        private MotionSynthesizer MotionSynthesizer => motionSynthesizer.Ref;
        
        public TaskReference IdleTaskReference;
        public TaskReference LocomotionTaskReference;
        
        public void Execute()
        {
            MotionSynthesizer
                .Root
                .Action()
                .PlayFirstSequence(MotionSynthesizer.Query.Where(Locomotion.Default)
                    .Except(Idle.Default));
        }
    }
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Kinematica))]
    public sealed class AnimationProvider : SnapshotProvider
    {
        private Kinematica _kinematica;
        private TaskReference _idleCandidate;
        private Trajectory _trajectory;
        public override void OnEnable()
        {
            base.OnEnable();
            
            _kinematica = gameObject.GetComponent<Kinematica>();
            ref MotionSynthesizer motionSynthesizer = ref _kinematica.Synthesizer.Ref;
            
          motionSynthesizer.Root.Action()
                .PlayFirstSequence(motionSynthesizer.Query.Where(Locomotion.Default).And(Idle.Default));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                KinematicaSoloJob job = new KinematicaSoloJob
                {
                    motionSynthesizer = _kinematica.Synthesizer
                };
            
                _kinematica.AddJobDependency(job.Schedule());
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }
    }
}