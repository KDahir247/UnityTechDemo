﻿/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// 
/// Utilizes script execution order to run before anything else to avoid order of operation failures so accessing things like singletons at any stage of application startup will never fail.
/// 
/// </summary>

using System.Reflection;
using UnityEngine;

namespace Pixelplacement
{
    //Todo modify this to the scope of the project
    public class Initialization : MonoBehaviour
    {
        private DisplayObject _displayObject;

        //Private Variables:
        private StateMachine _stateMachine;

        //Init:
        private void Awake()
        {
            //singleton initialization:
            InitializeSingleton();

            //values:
            _stateMachine = GetComponent<StateMachine>();
            _displayObject = GetComponent<DisplayObject>();

            //display object initialization:
            if (_displayObject != null) _displayObject.Register();

            //state machine initialization:
            if (_stateMachine != null) _stateMachine.Initialize();
        }

        private void Start()
        {
            //state machine start:
            if (_stateMachine != null) _stateMachine.StartMachine();
        }

        //Deinit:
        private void OnDisable()
        {
            if (_stateMachine != null)
            {
                if (!_stateMachine.returnToDefaultOnDisable || _stateMachine.defaultState == null) return;

                if (_stateMachine.currentState == null)
                {
                    _stateMachine.ChangeState(_stateMachine.defaultState);
                    return;
                }

                if (_stateMachine.currentState.Value != _stateMachine.defaultState)
                    _stateMachine.ChangeState(_stateMachine.defaultState);
            }
        }

        //Private Methods:
        private void InitializeSingleton()
        {
            foreach (var item in GetComponents<Component>())
            {
                string baseType;

#if NETFX_CORE
                baseType = item.GetType ().GetTypeInfo ().BaseType.ToString ();
#else
                baseType = item.GetType().BaseType.ToString();
#endif

                if (baseType.Contains("Singleton") && baseType.Contains("Pixelplacement"))
                {
                    MethodInfo m;

#if NETFX_CORE
                    m =
 item.GetType ().GetTypeInfo ().BaseType.GetMethod ("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
#else
                    m = item.GetType().BaseType.GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
#endif

                    m.Invoke(item, new[] {item});
                }
            }
        }
    }
}