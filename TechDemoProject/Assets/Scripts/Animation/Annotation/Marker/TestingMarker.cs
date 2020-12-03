using System;
using Unity.Kinematica.Editor;

namespace Tech.Animation
{
    [Serializable]
    [Marker("TestingMarker", "Purple")]
    public struct TestingMarker : Payload<TestingMarkerPayload>
    {
        public TestingMarkerPayload Build(PayloadBuilder builder)
        {
            return new TestingMarkerPayload();
        }
    }
}