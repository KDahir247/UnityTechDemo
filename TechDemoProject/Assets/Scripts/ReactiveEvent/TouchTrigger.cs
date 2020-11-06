using System;
using System.Collections.Generic;
using Tech.Core;
using Tech.Data;
using Tech.Event.Variable;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZLogger;

namespace Tech.Event
{
//TODo might make something inherit this
    //TODO create a compositeDisposable to dispose of the Observable subscription
    [RequireComponent(typeof(Button))]
    public class TouchTrigger : ObservableTriggerBase, IPointerClickHandler, IPointerDownHandler
    {
        private static Subject<KeyValuePair<string, PointerEventData>> _touchSubject;

        private static readonly Dictionary<string, Button> SelectableGameObject = new Dictionary<string, Button>();
        public static IReadOnlyReactiveDictionary<string, Button> Selectable;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private IObservable<Unit> _conditional;

        [SerializeField] private List<string> buttonKeyRef = new List<string>();

        [SerializeField] [FormerlySerializedAs("Command")]
        protected ReactiveTouchProperty command;

        [FormerlySerializedAs("_disableTimer")] [SerializeField]
        private float disableTimer;

        [SerializeField] private string keyRef;


        public void OnPointerClick(PointerEventData eventData)
        {
            //going to add more logic here
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.selectedObject == null) return;

            var button = eventData.selectedObject.GetComponent<Button>();
            Debug.Log("touch");
            _touchSubject.OnNext(
                new KeyValuePair<string, PointerEventData>(eventData.selectedObject.GetComponent<TouchTrigger>().keyRef,
                    eventData));
            
            switch (command.Value)
            {
                case ButtonCommand.None:
                    _conditional = Observable.Empty<Unit>().AsUnitObservable();
                    break;

                case ButtonCommand.Undefined:
                    LogManager.Logger.ZLogWarning("ButtonCommand is Undefined falling back to ButtonCommand.None");
                    command.Value = ButtonCommand.None;
                    break;

                case ButtonCommand.DisableIndefinitely:
                    _conditional = Observable.Never<Unit>().AsUnitObservable();
                    break;

                case ButtonCommand.DisableTimer:
                    _conditional = Observable.Timer(TimeSpan.FromSeconds(disableTimer)).AsUnitObservable();
                    break;
                case ButtonCommand.DisableTillDistinct:
                    //Not Implemented yet
                    LogManager.Logger.ZLogWarning("Not implemented yet.");
                    break;
                case ButtonCommand.DisableOther:
                    _disposable.Dispose();
                    foreach (var key in buttonKeyRef)
                        if (SelectableGameObject.TryGetValue(key, out var refButton))
                            refButton.interactable = false;
                    break;
                case ButtonCommand.DisableAll:
                    foreach (var keyValuePair in SelectableGameObject)
                    {
                        keyValuePair.Value.interactable = false;
                    }
                    break;
                case ButtonCommand.EnableOtherExceptSelf:
                    foreach (var key in buttonKeyRef)
                        if (SelectableGameObject.TryGetValue(key, out var refButton))
                            refButton.interactable = true;

                    _conditional = Observable.Never<Unit>().AsUnitObservable();
                    break;
                case ButtonCommand.EnableAllExceptSelf:
                    foreach (var keyValuePair in SelectableGameObject) keyValuePair.Value.interactable = true;

                    _conditional = Observable.Never<Unit>().AsUnitObservable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            
            button.BindToOnClick(unit => _conditional).AddTo(_disposable);
        }

        private void Awake()
        {
            Selectable = new ReactiveDictionary<string, Button>(SelectableGameObject);
            SelectableGameObject.Add(keyRef, gameObject.GetComponent<Button>());
        }

        /// <summary>
        ///     Method Call to subscribe to event that are fired from this script
        /// </summary>
        /// <returns></returns>
        public static IObservable<KeyValuePair<string, PointerEventData>> TouchTriggerAsObservable()
        {
            return _touchSubject ?? (_touchSubject = new Subject<KeyValuePair<string, PointerEventData>>());
        }

        /// <summary>
        ///     Raises Complete when the Object is Destroyed
        /// </summary>
        protected override void RaiseOnCompletedOnDestroy()
        {
            // if (_touchSubject != null)
            //     _touchSubject.OnCompleted();

            if (!_disposable.IsDisposed)
                _disposable.Dispose();
            
            SelectableGameObject.Clear();
        }

        private void OnApplicationQuit()
        {
            if (_touchSubject != null)
            {
                _touchSubject.OnCompleted();
            }
        }
    }
}