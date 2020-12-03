using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Idle", "#16c3c9")]
    public struct IdleTag : Payload<Idle>
    {
        public static IdleTag CreateDefaultTag()
        {
            return new IdleTag();
        }
        
        
        public Idle Build(PayloadBuilder builder)
        {
            return Idle.Default;
        }
    }
}