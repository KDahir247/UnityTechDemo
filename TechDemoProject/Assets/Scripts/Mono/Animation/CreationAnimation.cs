using Tech.Event;
using UniRx;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tech.Mono
{
    //Uses Event to handle basic animation used in the creation scene
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class CreationAnimation : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Start()
        {
            TouchTrigger.TouchTriggerAsObservable()
                .Subscribe(keyRef =>
                {
                _animator.SetTrigger(keyRef.Key);
            }).AddTo(this);
        }
    }
}