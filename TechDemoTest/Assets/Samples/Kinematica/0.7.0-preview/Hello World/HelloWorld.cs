using System;
using Unity.Kinematica;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HelloWorld
{
    [RequireComponent(typeof(Kinematica))]
    public class HelloWorld : MonoBehaviour
    {
        bool idle;


        private void Update()
        {
            var currentMouse = Mouse.current;

            if (currentMouse.press.wasPressedThisFrame)
            {
                idle ^= true;

                var kinematica = GetComponent<Kinematica>();

                ref var synthesizer = ref kinematica.Synthesizer.Ref;

                if (idle)
                {
                    synthesizer.Root.Action().PlayFirstSequence(
                        synthesizer.Query.Where(
                            Locomotion.Default).And(Idle.Default));
                }
                else
                {
                    synthesizer.Root.Action().PlayFirstSequence(
                        synthesizer.Query.Where(
                            Locomotion.Default).Except(Idle.Default));
                }
            }
        }
    }
}