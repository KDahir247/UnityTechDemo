using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Locomotion", "#27558c")]
    public struct LocomotionTag : Payload<Locomotion>
    {
        public static LocomotionTag CreateDefaultTag()
        {
            return new LocomotionTag();
        }

        public Locomotion Build(PayloadBuilder builder)
        {
            return Locomotion.Default;
        }
    }
}