using System;
using Tech.Core;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

//Passes
//TODO make more Visual Element script for the different panel state in Title Screen so this script doesn't handle all responsibility for all the panel 
//TODO use utf8 string builder 
namespace Tech.UI.Panel
{
    public class TitleScreen_Document : VisualElement
    {
        private const int AnimationFadeInDuration = 1000;
        private const int AnimationFadeOutDuration = 1000;

        //variables for the recursive animation
        private bool _hasPressedSelectable;

        private string _headSceneName = string.Empty;
        private bool _isTransitioning;
        private string _tailSceneName = string.Empty;

        public bool OutOfBound = true;

        public TitleScreen_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public VisualElement NewsScreen { get; private set; }
        public VisualElement OptionScreen { get; private set; }
        public VisualElement SupportScreen { get; private set; }
        public VisualElement MainMenuVisualElement { get; private set; }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            //Visual Element Panel that MainMenu can Transition to 
            Initialize();
            ButtonInitialize();

            //TODO issue with the event call
            OptionScreen.Q<VisualElement>("Core_Panel")
                .RegisterCallback<MouseLeaveEvent>(evnt =>
                    FadeToNewScreen(MainMenuVisualElement, OptionScreen));

            SupportScreen.Q<VisualElement>("Core_Panel")
                .RegisterCallback<MouseLeaveEvent>(evnt => FadeToNewScreen(MainMenuVisualElement, SupportScreen));

            NewsScreen.Q<VisualElement>("Core_Panel")
                .RegisterCallback<MouseLeaveEvent>(evnt => FadeToNewScreen(MainMenuVisualElement, NewsScreen));


            MainMenuVisualElement.RegisterCallback<ClickEvent>(evnt =>
            {
                if (!_isTransitioning)
                    LoadScene(evnt);
            });

            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void ButtonInitialize()
        {
            this.Q<Button>("Option_Button")
                .RegisterCallback<ClickEvent>(
                    evnt => FadeToNewScreen(OptionScreen, MainMenuVisualElement)
                );

            this.Q<Button>("Support_Button")
                .RegisterCallback<ClickEvent>(evnt =>
                    FadeToNewScreen(SupportScreen, MainMenuVisualElement));

            this.Q<Button>("Mail_Button")
                .RegisterCallback<ClickEvent>(evnt =>
                    FadeToNewScreen(NewsScreen, MainMenuVisualElement));
        }

        private void Initialize()
        {
            MainMenuVisualElement = this.Q<VisualElement>("MainMenu_Document");
            OptionScreen = this.Q<VisualElement>("Option_Document");
            SupportScreen = this.Q<VisualElement>("Support_Document");
            NewsScreen = this.Q<VisualElement>("News_Document");
        }


        public void FadeToNewScreen(VisualElement fadeInTarget, VisualElement fadeOutTarget)
        {
            if (!_isTransitioning)
                FadeOut(fadeOutTarget,
                    () => FadeIn(fadeInTarget, fadeOutTarget, () => { _isTransitioning = false; }));
        }


        private void FadeOut(VisualElement visualElementToFadeOut, Action onComplete)
        {
            _isTransitioning = true;

            visualElementToFadeOut.experimental.animation
                .Start(new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f}, AnimationFadeOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(onComplete);
        }


        private void FadeIn(VisualElement visualElementToFadeIn, VisualElement visualElementThatFadeOut,
            Action onComplete)
        {
            visualElementToFadeIn.style.display = DisplayStyle.Flex;
            visualElementThatFadeOut.style.display = DisplayStyle.None;

            visualElementToFadeIn.experimental.animation
                .Start(new StyleValues {opacity = 0},
                    new StyleValues {opacity = 1.0f}, AnimationFadeInDuration)
                .Ease(Easing.Linear)
                .OnCompleted(onComplete);
        }

        private void Init(in string targetScene, in string currentScene)
        {
            _headSceneName = currentScene;
            _tailSceneName = targetScene;
        }

        private void LoadScene<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            if (!_isTransitioning)
            {
                if (Application.isPlaying)
                {
                    MainMenuVisualElement.experimental.animation
                        .Start(new StyleValues {opacity = 1.0f},
                            new StyleValues {opacity = 0.0f}, AnimationFadeOutDuration)
                        .Ease(Easing.Linear)
                        .OnCompleted(() => { MainMenuVisualElement.style.display = DisplayStyle.None; });

                    StateSingleton.Instance.Next();
                }
                else
                {
                    Debug.Log($"Loading scene: {_tailSceneName} from the addressable");
                }

                _isTransitioning = true;
            }
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

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //Addressable Path for current scene
            private readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription
                {name = "start-scene", defaultValue = "Assets/Scenes/MainMenu.unity"};

            //Addressable Path for next scene
            private readonly UxmlStringAttributeDescription _tailScene = new UxmlStringAttributeDescription
                {name = "next-scene", defaultValue = "Assets/Scenes/Creation.unity"};

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nexSceneName = _tailScene.GetValueFromBag(bag, cc);

                ((TitleScreen_Document) ve).Init(nexSceneName, sceneName);
            }
        }
    }
}