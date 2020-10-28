using Unity.Kinematica.Editor;

namespace Tech.Animation.Annotation
{ 
    [System.Serializable]
    [Tag("Idle", "#16c3c9")]
    public struct IdleTag : Payload<Idle>
    {
        public Idle Build(PayloadBuilder builder)
        {
            return Idle.Default;
        }
    }
}