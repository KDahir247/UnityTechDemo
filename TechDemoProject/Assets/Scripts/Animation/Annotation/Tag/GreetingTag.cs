using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation.Annotation
{
    [Serializable]
    [Tag("Greeting", "#2ade2d")]
    public struct GreetingTag : Payload<Greeting>
    {
        public Greeting Build(PayloadBuilder builder)
        {
            return Greeting.Default;
        }
    }
}