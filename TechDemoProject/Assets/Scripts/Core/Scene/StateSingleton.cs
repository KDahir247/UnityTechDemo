using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Core
{
    //TODO look at
    public class StateSingleton : Singleton<StateSingleton>
    {
        private readonly List<SceneState> _states = new List<SceneState>(3);

        [FormerlySerializedAs("State")] public IReadOnlyList<SceneState> state;

        public StateMachine Machine { get; private set; }
        public int Index { get; private set; }

        protected override void OnRegistration()
        {
            Machine = gameObject.GetComponent<StateMachine>();

            foreach (Transform child in transform) _states.Add(child.GetComponent<SceneState>());
            state = new List<SceneState>(_states);
        }


        //Wrapper Method for StateMachine class
        public void Next()
        {
            _states[Index].gameObject.GetComponent<AudioSource>().DOFade(0, _states[Index].fadeOutTime)
                .SetEase(_states[Index].fadeOutEase)
                .onComplete += () =>
            {
                if (Index == _states.Count)
                {
                    Index = 0;
                    SceneAddress.SceneLoadByNameOrLabel(_states[Index].onNextScene).Forget();
                    Machine.ChangeState(Index);
                }
                else
                {
                    SceneAddress.SceneLoadByNameOrLabel(_states[Index++].onNextScene).Forget();
                    Machine.Next();
                }
            };
        }
    }
}