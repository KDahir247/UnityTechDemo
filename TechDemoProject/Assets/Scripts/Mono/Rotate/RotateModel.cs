using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pixelplacement;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Tech.Mono
{
    public enum RotationDirection
    {
        Left,
        Right
    }

    [RequireComponent(typeof(Button))]
    public class RotateModel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private GameObject _currentPlayerState;

        private bool _isHeld;

        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotationTween;

        [FormerlySerializedAs("_rotationDirection")] [SerializeField]
        private RotationDirection rotationDirection;

        [SerializeField] private float rotationDuration = 1;

        [FormerlySerializedAs("RotationEase")] [SerializeField]
        private Ease rotationEase = Ease.Linear;

        [FormerlySerializedAs("rotationSpeed")] [FormerlySerializedAs("RotationSpeed")] [SerializeField]
        private float rotationSensitivity = 3;

        [FormerlySerializedAs("_stateMachine")] [SerializeField]
        private StateMachine stateMachine;

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("pressed");
            _isHeld = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isHeld = false;
        }

        private void Start()
        {
            stateMachine.ObserveEveryValueChanged(machine => machine.currentState.Value).Subscribe(state =>
            {
                _rotationTween?.Kill();
                _currentPlayerState = state;
            }).AddTo(this);
        }

        private void Update()
        {
            if (_isHeld)
                _rotationTween = _currentPlayerState.transform
                    .DORotate(
                        (rotationDirection == RotationDirection.Right ? Vector3.down : Vector3.up) *
                        rotationSensitivity, rotationDuration,
                        RotateMode.WorldAxisAdd).SetEase(rotationEase)
                    .Play();
        }
    }
}