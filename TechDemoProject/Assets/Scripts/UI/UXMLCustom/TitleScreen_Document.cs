﻿using System;
using Tech.Core;
using Tech.Utility;
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

        private bool _isTransitioning;

        private bool _hasPressedSelectable;

        private string _headSceneName = string.Empty;
        private string _tailSceneName = string.Empty;
        
        //variables for the recursive animation
        private StyleValues _fadeOutStyle;
        private StyleValues _fadeInStyle;

        private const int AnimRecurInDuration = 2000;
        private const int AnimRecurOutDuration = 2000;
        
        public new class UxmlFactory : UxmlFactory<TitleScreen_Document, UxmlTraits>{ }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //Addressable Path for current scene
            readonly UxmlStringAttributeDescription _headScene = new UxmlStringAttributeDescription{name = "start-scene", defaultValue = "Assets/Scenes/MainMenu.unity"};
            //Addressable Path for next scene
            readonly UxmlStringAttributeDescription  _tailScene = new UxmlStringAttributeDescription{name = "next-scene", defaultValue = "Assets/Scenes/Creation.unity"}; 
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var sceneName = _headScene.GetValueFromBag(bag, cc);
                var nexSceneName = _tailScene.GetValueFromBag(bag, cc);
          
                ((TitleScreen_Document)ve).Init(nexSceneName,sceneName);
            }
        }

        public TitleScreen_Document() => RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        

        void OnGeometryChange(GeometryChangedEvent evt)
        {
            _titleScreen = this.Q<VisualElement>("MainMenu_Document");
            _optionScreen = this.Q<VisualElement>("Option_Document");
            _supportScreen = this.Q<VisualElement>("Support_Document");
            _newsScreen = this.Q<VisualElement>("News_Document");
            
            _titleScreen.Q<Button>("Option_Button").RegisterCallback<ClickEvent>(EnableOptionScreen);
            _titleScreen.Q<Button>("Support_Button").RegisterCallback<ClickEvent>(EnableSupportButton);
            _titleScreen.Q<Button>("Mail_Button").RegisterCallback<ClickEvent>(EnableNewsButton);

            _titleScreen.Q<VisualElement>("Base_Panel").RegisterCallback<ClickEvent>(LoadScene);
            
            _id = _titleScreen.Q<Label>("ID_Text");
            _version = _titleScreen.Q<Label>("Version_Text");
            
            _id.text = "ID." + "1444";
            _version.text = "VER."+ Application.version;
            
            //creating a single instance of Style Value since AnimationTouchScreen is recursive and it will recursively create instance
            _fadeInStyle = new StyleValues{opacity = 1};
            _fadeOutStyle = new StyleValues{opacity = 0};
            
            AnimationTouchScreen();
            
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        //Uses Recursion since there is not looping currently in the UIElement animation. Animation is still in experimental 
        //going to redo when UIElement animation is capable of looping.
        private void AnimationTouchScreen()
        {
            _titleScreen.Q<Label>("TouchScreen_Text").experimental.animation
                .Start(_fadeInStyle, _fadeOutStyle, AnimRecurOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(() =>
                {
                    _titleScreen.Q<Label>("TouchScreen_Text").experimental.animation
                        .Start(_fadeOutStyle, _fadeInStyle, AnimRecurInDuration)
                        .Ease(Easing.Linear)
                        .OnCompleted(AnimationTouchScreen);
                });
        }

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

        private void Init(in string targetScene, in string currentScene)
        {
            _headSceneName = currentScene;
            _tailSceneName = targetScene;
        }
        
        //TODO Refactor
        private void LoadScene<T>(T evt)
            where T : PointerEventBase<T> , new()
        {
            if (!_isTransitioning)
            {
                if (Application.isPlaying)
                {
                    _titleScreen.experimental.animation
                        .Start(new StyleValues {opacity = 1.0f},
                            new StyleValues {opacity = 0.0f}, AnimationFadeOutDuration)
                        .Ease(Easing.Linear)
                        .OnCompleted(() =>
                        {
                            _titleScreen.style.display = DisplayStyle.None;
                            
                        });
                    
                    StateSingleton.Instance.Next();
                }
                else
                {
                    Debug.Log($"Loading scene: {_tailSceneName} from the addressable");
                }
                
                _isTransitioning = true;
            }
        }
    }
}
