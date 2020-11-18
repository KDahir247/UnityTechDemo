using Pixelplacement;
using Tech.Data;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Creation_Document : VisualElement
    {

        private StateMachine _characterStateMachine;
        
        private readonly Button[] _skills = new Button[3];
        private VisualElement _coreElement;

        private string _headScene = string.Empty;

        private RotationDirection _modelRotateDirection;
        private string _tailScene = string.Empty;

        public Creation_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            //Skill Button
            //TODO might use 3 Visual Element for the skill
            _skills[0] = this.Q<Button>("Skill1_Button");
            _skills[1] = this.Q<Button>("Skill2_Button");
            _skills[2] = this.Q<Button>("Skill3_Button");

            //Rotating Model Button
            //TODO trying to get Click Event this isn't click event but pointerEnter event
            this.Q<Button>("RotationArrowR_Button").RegisterCallback<ClickEvent>(RotateModelRight);
            this.Q<Button>("RotationArrowL_Button").RegisterCallback<ClickEvent>(RotateModelLeft);
            this.Q<Button>("RotationArrowR_Button").RegisterCallback<PointerLeaveEvent>(RotateModelNeutral);
            this.Q<Button>("RotationArrowL_Button").RegisterCallback<PointerLeaveEvent>(RotateModelNeutral);

            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public void SetSkillTexture(Texture2D texture2D, int index, int stateIndex)
        {
            _characterStateMachine.ChangeState(stateIndex);
            _skills[index].style.backgroundImage = texture2D;
        }


        //Send RotationDirection to MessageBroker so anybody can subscribe to the event.
        //The event get propagated to other classes 
        private void RotateModelRight<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            MessageBroker.Default.Publish(RotationDirection.Right);
        }


        private void RotateModelLeft<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            MessageBroker.Default.Publish(RotationDirection.Left);
        }

        private void RotateModelNeutral<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            MessageBroker.Default.Publish(RotationDirection.None);
        }

        private void Init(in string scene, in string nextScene)
        {
            _headScene = scene;
            _tailScene = nextScene;
        }


        public void RetrieveStateMachine(StateMachine stateMachine)
        {
            _characterStateMachine = stateMachine;
        }
        
        public new class UxmlFactory : UxmlFactory<Creation_Document, UxmlTraits>
        {
            public override VisualElement Create(IUxmlAttributes bag, CreationContext cc)
            {
                return base.Create(bag, cc);
            }

            public override bool AcceptsAttributeBag(IUxmlAttributes bag, CreationContext cc)
            {
                return base.AcceptsAttributeBag(bag, cc);
            }
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "start-scene", defaultValue = "Assets/Scenes/Creation.unity"};

            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Assets/Scenes/Game.unity"};

            //set the _headScene 
            //and set the _tailScene
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nextSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((Creation_Document) ve).Init(sceneName, nextSceneName);
            }
        }
    }
}