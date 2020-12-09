using JetBrains.Annotations;
using Tech.Core;
using Tech.UI.Linq;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class TitleScreen_Document : Base_Document
    {
        private string _headSceneName = string.Empty;
        private bool _isTransitioning;
        private Button _mailButton;
        private VisualElement _mainMenuVisualElement;
        private VisualElement _newsCoreVisualElement;

        private VisualElement _newsVisualElement;

        private Button _optionButton;

        private VisualElement _optionCoreVisualElement;
        private VisualElement _optionVisualElement;

        private Button _supportButton;
        private VisualElement _supportCoreVisualElement;
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


            _optionCoreVisualElement = _optionVisualElement.Q<VisualElement>("Core_Panel");
            _supportCoreVisualElement = _supportVisualElement.Q<VisualElement>("Core_Panel");
            _newsCoreVisualElement = _newsVisualElement.Q<VisualElement>("Core_Panel");
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
            _optionCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _optionVisualElement));

            _supportCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _supportVisualElement));

            _newsCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _newsVisualElement));

            _mainMenuVisualElement.RegisterCallback(LoadScene());
        }

        protected override void OnDestroy()
        {
            _optionButton
                .UnregisterCallback(FadeToNewScreen<ClickEvent>(_optionVisualElement, _mainMenuVisualElement));
            _supportButton
                .UnregisterCallback(FadeToNewScreen<ClickEvent>(_supportVisualElement, _mainMenuVisualElement));
            _mailButton
                .UnregisterCallback(FadeToNewScreen<ClickEvent>(_newsVisualElement, _mainMenuVisualElement));

            _optionCoreVisualElement
                .UnregisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _optionVisualElement));

            _supportCoreVisualElement
                .UnregisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _supportVisualElement));

            _newsCoreVisualElement
                .UnregisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _newsVisualElement));

            _mainMenuVisualElement.UnregisterCallback(LoadScene());
        }


        [NotNull]
        private EventCallback<T> FadeToNewScreen<T>(VisualElement fadeTo, VisualElement fadeFrom)
            where T : EventBase //maybe the issue lies here
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


        public new class UxmlFactory : UxmlFactory<TitleScreen_Document, UxmlTraits>
        {
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