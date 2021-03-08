using JetBrains.Annotations;
using Tech.UI.Linq;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public class TitleScreen_Document : BaseDocument
    {
        private string _headSceneName = string.Empty;
        private string _tailSceneName = string.Empty;

        private VisualElement _mainMenuVisualElement;

        private Button _mailButton;
        private VisualElement _newsCoreVisualElement;
        private VisualElement _newsVisualElement;

        private Button _optionButton;
        private VisualElement _optionCoreVisualElement;
        private VisualElement _optionVisualElement;

        private Button _supportButton;
        private VisualElement _supportCoreVisualElement;
        private VisualElement _supportVisualElement;


        protected override void Init(params string[] scenes)
        {
            if (scenes == null || scenes.Length <= 1) return;

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

        protected override void RegisterCallback()
        {
            _optionButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_optionVisualElement, _mainMenuVisualElement));
            _supportButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_supportVisualElement, _mainMenuVisualElement));
            _mailButton
                .RegisterCallback(FadeToNewScreen<ClickEvent>(_newsVisualElement, _mainMenuVisualElement));

            _optionCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _optionVisualElement));
            _supportCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _supportVisualElement));
            _newsCoreVisualElement
                .RegisterCallback(FadeToNewScreen<MouseLeaveEvent>(_mainMenuVisualElement, _newsVisualElement));

            _mainMenuVisualElement.RegisterCallback(LoadScene());
        }

        protected override void UnregisterCallback()
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
            where T : EventBase
        {
            return evt =>
            {
                UnregisterCallback();

                fadeFrom.FadeToNewScreen(fadeTo, FadeOutStyle, FadeInStyle, Easing.Linear, FadeOutDuration, FadeInDuration,
                    RegisterCallback);
            };
        }

        [NotNull]
        private EventCallback<ClickEvent> LoadScene()
        {
            return evt =>
            {
                _mainMenuVisualElement
                    .FadeInOrOut(FadeInStyle,
                        FadeOutStyle,
                        Easing.Linear,
                        FadeOutDuration,
                        () => _mainMenuVisualElement.style.display = DisplayStyle.None);

                UnregisterCallback();
                OnLoadedNextScene(_tailSceneName);
            };
        }

        public new class UxmlFactory : UxmlFactory<TitleScreen_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "current-scene", defaultValue = "MainMenu"};

            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Creation"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nexSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((TitleScreen_Document) ve).Init(sceneName, nexSceneName);
            }
        }
    }
}