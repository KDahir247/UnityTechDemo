using Tech.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public class Creation_Document : VisualElement
    {
        private VisualElement _coreElement;

        private string _headScene = string.Empty;
        private string _tailScene = string.Empty;

        private Button _skill1, _skill2, _skill3;

        private RotationDirection _modelRotateDirection;
        
        public new class UxmlFactory : UxmlFactory<Creation_Document, UxmlTraits>{}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            readonly  UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription{name = "start-scene", defaultValue = "Assets/Scenes/Creation.unity"};
            
            readonly  UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription{name = "next-scene", defaultValue = "Assets/Scenes/Game.unity"};
            //set the _headScene 
            //and set the _tailScene
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nextSceneName = _tailScene.GetValueFromBag(bag, cc);
                
                ((Creation_Document)ve).Init(sceneName, nextSceneName);
            }
        }
        
        public Creation_Document() => RegisterCallback<GeometryChangedEvent>(OnGeometryChange);

        void OnGeometryChange(GeometryChangedEvent evt)
        {
            //Class Choose
            this.Q<Button>("Assassin_Button")
                .RegisterCallback<ClickEvent>(ChooseAssassin);
            
            this.Q<Button>("Necromancer_Button")
                .RegisterCallback<ClickEvent>(ChooseNecromancer);
            
            this.Q<Button>("Oracle_Button")
                .RegisterCallback<ClickEvent>(ChooseOracle);

            //Skill Button
            //TODO might use 3 Visual Element for the skill
            _skill1 = this.Q<Button>("Skill1_Button");
            _skill2 = this.Q<Button>("Skill2_Button");
            _skill3 = this.Q<Button>("Skill3_Button");

            //Rotating Model Button
            this.Q<Button>("RotationArrowR_Button").RegisterCallback<PointerEnterEvent>(RotateModelRight);
            this.Q<Button>("RotationArrowL_Button").RegisterCallback<PointerEnterEvent>(RotateModelRight);
            RegisterCallback<PointerUpEvent>(RotateModelNeutral);
            
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void ChooseAssassin<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            Debug.Log("You have pressed Assassin");
        }

        private void ChooseNecromancer<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            Debug.Log("You have pressed Necromancer");
        }

        private void ChooseOracle<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            Debug.Log("You have pressed Oracle");
        }

        //Send RotationDirection to MessageBroker so anybody can subscribe to the event.
        //The event get propagated to other classes 
        private void RotateModelRight<T>(T evt)
            where T : PointerEventBase<T>, new() =>
            MessageBroker.Default.Publish<RotationDirection>(RotationDirection.Right);


        private void RotateModelLeft<T>(T evt)
            where T : PointerEventBase<T>, new() =>
            MessageBroker.Default.Publish<RotationDirection>(RotationDirection.Left);

        private void RotateModelNeutral<T>(T evt)
            where T : PointerEventBase<T>, new() =>
            MessageBroker.Default.Publish<RotationDirection>(RotationDirection.None);

        private void Init(in string scene,in string nextScene)
        {
            _headScene = scene;
            _tailScene = nextScene;
        }
        
    }
}