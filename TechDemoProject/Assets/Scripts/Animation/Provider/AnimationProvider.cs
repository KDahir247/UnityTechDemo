// using Tech.Animation.Annotation;
// using Unity.Kinematica;
// using Unity.SnapshotDebugger;
// using UnityEngine;
//
// namespace Tech.Animation.Provider
// {
//     [DisallowMultipleComponent]
//     [RequireComponent(typeof(Kinematica))]
//     public sealed class AnimationProvider : SnapshotProvider
//     {
//         
//         private Kinematica _kinematica;
//         
//         public override void OnEnable()
//         {
//             base.OnEnable();
//
//             _kinematica = this.gameObject.GetComponent<Kinematica>();
//             ref MotionSynthesizer motionSynthesizer = ref _kinematica.Synthesizer.Ref;
//             
//             var idleCad = motionSynthesizer
//                 .PlayFirstSequence(motionSynthesizer.Query.Where)
//         }
//     }
// }