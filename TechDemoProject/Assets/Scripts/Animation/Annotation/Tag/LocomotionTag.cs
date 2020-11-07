using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation.Annotation
{
    [Tag("Locomotion", "#27558c")]
    public struct LocomotionTag : Payload<Locomotion>
    {
        public Locomotion Build(PayloadBuilder builder)
        {
            return Locomotion.Default;
        }
    }
}