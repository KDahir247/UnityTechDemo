using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Skill1", "#ff0000" )]
    public struct Skill1Tag : Payload<Skill1>
    {
        public static Skill1Tag CreateDefaultTag()
        {
            return new Skill1Tag();
        }

        public Skill1 Build(PayloadBuilder builder)
        {
            return Skill1.Default;
        }
    }
}