/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// StateMachine main class.
/// 
/// </summary>

//TODO redesign this class to fit project scope
// Used to disable the lack of usage of the exception in a try/catch:

#pragma warning disable 168

using Tech.Core;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Pixelplacement
{
    [RequireComponent(typeof(Initialization))]
    public class StateMachine : MonoBehaviour
    {
        [FormerlySerializedAs("OnStateChanged")]
        public bool _unityEventsFolded;

        /// <summary>
        ///     Can States within this StateMachine be reentered?
        /// </summary>
        [Tooltip("Can States within this StateMachine be reentered?")]
        public bool allowReentry;

        public ReactiveProperty<GameObject> currentState;


        //Public Variables:
        public GameObject defaultState;
        public UnityEvent OnFirstStateEntered;
        public UnityEvent OnFirstStateExited;
        public UnityEvent OnLastStateEntered;
        public UnityEvent OnLastStateExited;
        public GameObjectEvent OnStateEntered;

        //Publice Events:
        public GameObjectEvent OnStateExited;

        /// <summary>
        ///     Return to default state on disable?
        /// </summary>
        [Tooltip("Return to default state on disable?")]
        public bool returnToDefaultOnDisable = true;

        /// <summary>
        ///     Should log messages be thrown during usage?
        /// </summary>
        [Tooltip("Should log messages be thrown during usage?")]
        public bool verbose = true;

        private readonly ILogger Logger = LogManager.Logger;
        private bool _atFirst;
        private bool _atLast;

        //Private Variables:
        private bool _initialized;


        //Public Properties:
        /// <summary>
        ///     Internal flag used to determine if the StateMachine is set up properly.
        /// </summary>
        public bool CleanSetup { get; private set; }

        /// <summary>
        ///     Are we at the first state in this state machine.
        /// </summary>
        public bool AtFirst
        {
            get => _atFirst;

            private set
            {
                if (_atFirst)
                {
                    _atFirst = false;
                    if (OnFirstStateExited != null) OnFirstStateExited.Invoke();
                }
                else
                {
                    _atFirst = true;
                    if (OnFirstStateEntered != null) OnFirstStateEntered.Invoke();
                }
            }
        }

        /// <summary>
        ///     Are we at the last state in this state machine.
        /// </summary>
        public bool AtLast
        {
            get => _atLast;

            private set
            {
                if (_atLast)
                {
                    _atLast = false;
                    if (OnLastStateExited != null) OnLastStateExited.Invoke();
                }
                else
                {
                    _atLast = true;
                    if (OnLastStateEntered != null) OnLastStateEntered.Invoke();
                }
            }
        }

        //Public Methods:
        /// <summary>
        ///     Change to the next state if possible.
        /// </summary>
        public GameObject Next()
        {
            if (currentState.Value == null) return ChangeState(0);
            var currentIndex = currentState.Value.transform.GetSiblingIndex();

            if (currentIndex == transform.childCount - 1)
                return currentState.Value;
            return ChangeState(++currentIndex);
        }

        /// <summary>
        ///     Change to the previous state if possible.
        /// </summary>
        public GameObject Previous()
        {
            if (currentState.Value == null) return ChangeState(0);
            var currentIndex = currentState.Value.transform.GetSiblingIndex();
            if (currentIndex == 0)
                return currentState.Value;
            return ChangeState(--currentIndex);
        }

        /// <summary>
        ///     Exit the current state.
        /// </summary>
        public void Exit()
        {
            if (currentState.Value == null) return;
            Log("(-) " + name + " EXITED state: " + currentState.Value.name);
            var currentIndex = currentState.Value.transform.GetSiblingIndex();

            //no longer at first:
            if (currentIndex == 0) AtFirst = false;

            //no longer at last:
            if (currentIndex == transform.childCount - 1) AtLast = false;

            if (OnStateExited != null) OnStateExited.Invoke(currentState.Value);
            currentState.Value.SetActive(false);
            currentState.Value = null;
        }

        /// <summary>
        ///     Changes the state.
        /// </summary>
        public GameObject ChangeState(int childIndex)
        {
            if (childIndex > transform.childCount - 1)
            {
                Log("Index is greater than the amount of states in the StateMachine \"" + gameObject.name +
                    "\" please verify the index you are trying to change to.");
                return null;
            }

            return ChangeState(transform.GetChild(childIndex).gameObject);
        }

        /// <summary>
        ///     Changes the state.
        /// </summary>
        public GameObject ChangeState(GameObject state)
        {
            if (currentState.Value != null)
                if (!allowReentry && state == currentState.Value)
                {
                    Log("State change ignored. State machine \"" + name + "\" already in \"" + state.name +
                        "\" state.");
                    return null;
                }

            if (state.transform.parent != transform)
            {
                Log("State \"" + state.name + "\" is not a child of \"" + name +
                    "\" StateMachine state change canceled.");
                return null;
            }

            Exit();
            Enter(state);

            return currentState.Value;
        }


        public void ChangeStateNoReturn(int index)
        {
            ChangeState(index);
        }

        public void ChangeStateNoReturn(GameObject state)
        {
            ChangeState(state);
        }

        public void ChangeStateNoReturn(string state)
        {
            ChangeState(state);
        }


        /// <summary>
        ///     Changes the state.
        /// </summary>
        public GameObject ChangeState(string state)
        {
            var found = transform.Find(state);
            if (!found)
            {
                Log("\"" + name + "\" does not contain a state by the name of \"" + state +
                    "\" please verify the name of the state you are trying to reach.");
                return null;
            }

            return ChangeState(found.gameObject);
        }

        /// <summary>
        ///     Internally used within the framework to auto start the state machine.
        /// </summary>
        public void Initialize()
        {
            //turn off all states:
            for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
        }

        /// <summary>
        ///     Internally used within the framework to auto start the state machine.
        /// </summary>
        public void StartMachine()
        {
            //start the machine:
            if (Application.isPlaying && defaultState != null) ChangeState(defaultState.name);
        }

        //Private Methods:
        private void Enter(GameObject state)
        {
            currentState.Value = state;
            var index = currentState.Value.transform.GetSiblingIndex();

            //entering first:
            if (index == 0) AtFirst = true;

            //entering last:
            if (index == transform.childCount - 1) AtLast = true;

            Log("(+) " + name + " ENTERED state: " + state.name);
            if (OnStateEntered != null) OnStateEntered.Invoke(currentState.Value);
            currentState.Value.SetActive(true);
        }

        private void Log(string message)
        {
            if (true) return; //Hard coded no verbose
            Logger.ZLogInformation(message, gameObject);
        }
    }
}