using System;
using Tech.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

//Passes
//TODO use utf8 string builder 
namespace Tech.UI.Panel
{
    public class TitleScreen_Document : VisualElement
    {
        private VisualElement _titleScreen;
        private VisualElement _optionScreen;
        private VisualElement _supportScreen;
        private VisualElement _newsScreen;

        private Label _id;
        private Label _version;
        
        private const int AnimationFadeInDuration = 1000;
        private const int AnimationFadeOutDuration = 1000;

        private bool _isTransitioning = false;

        public new class UxmlFactory : UxmlFactory<TitleScreen_Document, UxmlTraits>{ }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }

        public TitleScreen_Document()
        {
            this.RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        void OnGeometryChange(GeometryChangedEvent evt)
        {
            _titleScreen = this.Q<VisualElement>("MainMenu_Document");
            _optionScreen = this.Q<VisualElement>("Option_Document");
            _supportScreen = this.Q<VisualElement>("Support_Document");
            _newsScreen = this.Q<VisualElement>("News_Document");
            
            _titleScreen.Q<Button>("Option_Button").RegisterCallback<ClickEvent>(EnableOptionScreen);
            _titleScreen.Q<Button>("Support_Button").RegisterCallback<ClickEvent>(EnableSupportButton);
            _titleScreen.Q<Button>("Mail_Button").RegisterCallback<ClickEvent>(EnableNewsButton);

            
            _id = _titleScreen.Q<Label>("ID_Text");
            _version = _titleScreen.Q<Label>("Version_Text");
            
            _id.text = "ID." + "1444";
            _version.text = "VER."+ GlobalSetting<TitleScreen_Document>.ReactiveVersion.Value;
            
            
            this.UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }
        
        //TODO got to refactor the experimental UIELEMENT Animation to a method. When script passes test
        private void EnableOptionScreen<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            if(!_isTransitioning)
                FadeOut(evt, _titleScreen, () => FadeIn(evt, _optionScreen, () => {}));
        }

        private void EnableSupportButton<T>(T evt)
            where T : PointerEventBase<T>, new()
        {
            if(!_isTransitioning)
                FadeOut(evt, _titleScreen, () => FadeIn(evt, _supportScreen, () => {}));
        }

        private void EnableNewsButton<T>(T evt)
            where T : PointerEventBase<T> , new()
        {
            if(!_isTransitioning)
                FadeOut(evt, _titleScreen, () => FadeIn(evt, _newsScreen, () => {}));
        }
        

        private void FadeOut<T>(T evt, VisualElement visualElementToFadeOut, Action onComplete)
            where T : PointerEventBase<T>, new()
        {
            _isTransitioning = true;
            
            visualElementToFadeOut.experimental.animation
                .Start(new StyleValues {opacity = 1.0f}, new StyleValues {opacity = 0.0f}, AnimationFadeOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(onComplete);
        }


        private void FadeIn<T>(T evt, VisualElement visualElementToFadeIn, Action onComplete)
            where T : PointerEventBase<T>, new()
        {
            
            _titleScreen.style.display = DisplayStyle.None;
            visualElementToFadeIn.style.display = DisplayStyle.Flex;
            
            visualElementToFadeIn.experimental.animation
                .Start(new StyleValues {opacity = visualElementToFadeIn.style.opacity.value},
                    new StyleValues {opacity = 1.0f}, AnimationFadeInDuration)
                .Ease(Easing.Linear)
                .OnCompleted(onComplete);
        }
    }
}
