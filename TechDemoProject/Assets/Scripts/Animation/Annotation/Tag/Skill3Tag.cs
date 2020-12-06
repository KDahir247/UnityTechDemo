using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Skill3", "#ffd500")]
    public struct Skill3Tag : Payload<Skill3>
    {
        public static Skill3Tag CreateDefaultTag()
        {
            return new Skill3Tag();
        }


        public Skill3 Build(PayloadBuilder builder)
        {
            return Skill3.Default;
        }
    }
}