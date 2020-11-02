using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Pixelplacement;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Button = UnityEngine.UI.Button;

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
        [FormerlySerializedAs("_rotationDirection")] 
        [SerializeField]
        private RotationDirection rotationDirection;

        [FormerlySerializedAs("RotationEase")]
        [SerializeField]
        private Ease rotationEase = Ease.Linear;
        
        [FormerlySerializedAs("_stateMachine")]
        [SerializeField]
        private StateMachine stateMachine;

        private bool _isHeld;

        [FormerlySerializedAs("rotationSpeed")]
        [FormerlySerializedAs("RotationSpeed")]
        [SerializeField]
        private  float rotationSensitivity = 3;

        [SerializeField]
        private float rotationDuration = 1;
        
        private TweenerCore<Quaternion, Vector3, QuaternionOptions> _rotationTween;
        private GameObject _currentPlayerState;
        private void Start()
        {
            _currentPlayerState = stateMachine.currentState;
            stateMachine.ObserveEveryValueChanged(machine => machine.currentState).Subscribe(state =>
            {
                _rotationTween?.Kill();
                _currentPlayerState.gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                _currentPlayerState = state;
            }).AddTo(this);
        }

        private void Update()
        {
            if (_isHeld)
            {
                _rotationTween = _currentPlayerState.transform
                    .DORotate((rotationDirection == RotationDirection.Right ? Vector3.down : Vector3.up) * rotationSensitivity, rotationDuration,
                        RotateMode.WorldAxisAdd).SetEase(rotationEase)
                    .Play();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isHeld = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isHeld = false;
        }
    }
}