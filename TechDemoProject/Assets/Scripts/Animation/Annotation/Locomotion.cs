using System.Collections;
using System.Collections.Generic;
using Unity.Kinematica;
using UnityEngine;

namespace Tech.Animation.Annotation
{
    [Trait]
    public struct Locomotion
    {
        public static Locomotion Default => new Locomotion();
    }
}