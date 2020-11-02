using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pixelplacement;
using Tech.scenestate;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tech.Core
{
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

        private void Start()
        {
            EventSystem.current.ObserveEveryValueChanged(eventSystem => eventSystem.currentSelectedGameObject)
                .Where(nullableObj => nullableObj != null)
                .Subscribe(obj =>
                {
                    if (obj.TryGetComponent(out Button button) && obj.CompareTag(_states[Index].key))
                        _states[Index].gameObject.GetComponent<AudioSource>().DOFade(0, 2)
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
                }).AddTo(this);
        }
    }
}