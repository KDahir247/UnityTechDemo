using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Skill2", "#ff5900")]
    public struct Skill2Tag : Payload<Skill2>
    {
        public static Skill2Tag CreateDefaultTag()
        {
            return new Skill2Tag();
        }

        public Skill2 Build(PayloadBuilder builder)
        {
            return Skill2.Default;
        }
    }
}