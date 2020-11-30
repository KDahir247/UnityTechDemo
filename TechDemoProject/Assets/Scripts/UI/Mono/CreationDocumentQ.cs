using Pixelplacement;
using Tech.UI.Panel;
using Tech.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

//TODO change badly
public class CreationDocumentQ : MonoBehaviour
{
    private static Creation_Document _coreVisualElement;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _coreVisualElement = gameObject
            .GetComponent<UIDocument>()
            .rootVisualElement?
            .Q<Creation_Document>();
    }
    
    public void OnStateChange(GameObject state)
    {
        if (!_stateMachine)
        {
            _stateMachine = state.GetComponentInParent<StateMachine>();

            _coreVisualElement
                .RetrieveStateMachine(_stateMachine);
        }
    }
}