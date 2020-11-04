using Unity.Kinematica;

namespace Tech.Animation.Annotation
{
    [Trait]
    public struct Greeting
    {
        public static Greeting Default => new Greeting();
    }
}