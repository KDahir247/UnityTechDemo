using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Tag("Greeting", "#2ade2d")]
    public struct GreetingTag : Payload<Greeting>
    {
        public static GreetingTag CreateDefaultTag()
        {
            return new GreetingTag();
        }
        
        
        public Greeting Build(PayloadBuilder builder)
        {
            return Greeting.Default;
        }
    }
}