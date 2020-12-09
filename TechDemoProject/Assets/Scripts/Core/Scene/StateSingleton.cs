using System.Collections.Generic;
using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Core
{
    public class StateSingleton : Singleton<StateSingleton>
    {
        private readonly List<SceneState> _states = new List<SceneState>(3);

        private StateMachine _machine;

        [FormerlySerializedAs("State")] public IReadOnlyList<SceneState> state;
        public int CurrentIndex { get; private set; }

        protected override void OnRegistration()
        {
            _machine = gameObject.GetComponent<StateMachine>();

            foreach (Transform child in transform) _states.Add(child.GetComponent<SceneState>());
            state = new List<SceneState>(_states);
        }

        internal void Next()
        {
            _states[CurrentIndex].gameObject.GetComponent<AudioSource>().DOFade(0, _states[CurrentIndex].FadeOutTime)
                .SetEase(_states[CurrentIndex].FadeOutEase)
                .onComplete += () =>
            {
                if (CurrentIndex == _states.Count)
                {
                    CurrentIndex = 0;
                    SceneAddress.SceneLoadByNameOrLabel(_states[CurrentIndex].OnNextScene,
                            onComplete: () => _machine.ChangeState(CurrentIndex))
                        .Forget();
                }
                else
                {
                    SceneAddress.SceneLoadByNameOrLabel(_states[CurrentIndex++].OnNextScene,
                            onComplete: () => _machine.Next())
                        .Forget();
                }
            };
        }
    }
}