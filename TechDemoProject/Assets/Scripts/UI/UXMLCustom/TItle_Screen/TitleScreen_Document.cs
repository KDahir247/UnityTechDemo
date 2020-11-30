using JetBrains.Annotations;
using MasterData;
using Tech.Core;
using Tech.UI.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

//Passes
//TODO make more Visual Element script for the different panel state in Title Screen so this script doesn't handle all responsibility for all the panel 
//TODO use utf8 string builder 
namespace Tech.UI.Panel
{
    public class TitleScreen_Document : Base_Document
    {
        private string _headSceneName = string.Empty;
        private bool _isTransitioning;
        private Button _mailButton;
        private VisualElement _mainMenuVisualElement;
        
        private VisualElement _newsVisualElement;

        private Button _optionButton;
        private VisualElement _optionVisualElement;
        
        private Button _supportButton;
        private VisualElement _supportVisualElement;
        
        private string _tailSceneName = string.Empty;
        
        protected override void Init(params string[] scenes)
        {
            if (scenes == null) return;
            _headSceneName = scenes[0];
            _tailSceneName = scenes[1];
        }

        protected override void UIQuery()
        {
            _mainMenuVisualElement = this.Q<VisualElement>("MainMenu_Document");
            _optionVisualElement = this.Q<VisualElement>("Option_Document");
            _supportVisualElement = this.Q<VisualElement>("Support_Document");
            _newsVisualElement = this.Q<VisualElement>("News_Document");

            _optionButton = this.Q<Button>("Option_Button");
            _supportButton = this.Q<Button>("Support_Button");
            _mailButton = this.Q<Button>("Mail_Button");
        }

        protected override void Start()
        {
            //TODO button only works once?
            _optionButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_optionVisualElement, _mainMenuVisualElement));
            _supportButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_supportVisualElement, _mainMenuVisualElement));
            _mailButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_newsVisualElement, _mainMenuVisualElement));


            //TODO issue with the event call
            _optionVisualElement.Q<VisualElement>("Core_Panel")
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _optionVisualElement));

            _supportVisualElement.Q<VisualElement>("Core_Panel")
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _supportVisualElement));

            _newsVisualElement.Q<VisualElement>("Core_Panel")
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _newsVisualElement));

            _mainMenuVisualElement.RegisterCallback(LoadScene());
        }

        [NotNull]
        private EventCallback<T> FadeToNewScreen<T>(VisualElement fadeTo, VisualElement fadeFrom)
            where T : EventBase
        {
            return evt =>
            {
                if (!_isTransitioning)
                {
                    _isTransitioning = true;
                    fadeFrom.FadeToNewScreen(fadeTo, FadeOutStyle, FadeInStyle, FadeOutDuration, FadeInDuration,
                        () => _isTransitioning = false);
                }
            };
        }

        [NotNull]
        private EventCallback<ClickEvent> LoadScene()
        {
            return evt =>
            {
                if (_isTransitioning) return;

                _mainMenuVisualElement
                    .FadeInOrOut(FadeInStyle,
                        FadeOutStyle,
                        FadeOutDuration,
                        () => _mainMenuVisualElement.style.display = DisplayStyle.None);

                StateSingleton.Instance.Next();

                _isTransitioning = true;
            };
        }

        protected override void OnDestroy()
        {
        }

        public new class UxmlFactory : UxmlFactory<TitleScreen_Document, UxmlTraits>
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

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            //Addressable Path for current scene
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "start-scene", defaultValue = "Assets/Scenes/MainMenu.unity"};

            //Addressable Path for next scene
            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Assets/Scenes/Creation.unity"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nexSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((TitleScreen_Document) ve).Init(nexSceneName, sceneName);
            }
        }
    }
}