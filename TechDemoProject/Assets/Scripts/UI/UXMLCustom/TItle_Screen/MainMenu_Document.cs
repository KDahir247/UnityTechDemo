using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;


//Send it self to Title Screen so Sub UI Panel can access the initial Main Menu Panel

//Pseudo code
/*
 *I Want to acquire the button to load the UI Panel that are apart of Title Screen Document
 *
 * 
 */

namespace Tech.UI.Panel
{
    public class MainMenu_Document : VisualElement
    {
        private TitleScreen_Document _parentDocument;

        private Label _idLabel;
        private Label _versionLabel;
        
        
        private const int AnimRecurInDuration = 2000;
        private const int AnimRecurOutDuration = 2000;
        private StyleValues _fadeInStyle;
        private StyleValues _fadeOutStyle;

        public MainMenu_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            _idLabel = this.Q<Label>("ID_Text");
            _versionLabel = this.Q<Label>("Version_Text");

            _idLabel.text = "ID. 1111";
            _versionLabel.text = $"Ver.{Application.version}.{Application.unityVersion}";

            
            //creating a single instance of Style Value since AnimationTouchScreen is recursive and it will recursively create instance
            _fadeInStyle = new StyleValues {opacity = 1};
            _fadeOutStyle = new StyleValues {opacity = 0};
            
            AnimationTouchScreen();

              


            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
            
        }

        public void OnInitialize(TitleScreen_Document elementParent)
        {
      
        }
        
        //TODO add a label as a parameter to make it more generic 
        //Uses Recursion since there is not looping currently in the UIElement animation. Animation is still in experimental 
        //going to redo when UIElement animation is capable of looping.
        private void AnimationTouchScreen()
        {
            this.Q<Label>("TouchScreen_Text").experimental.animation
                .Start(_fadeInStyle, _fadeOutStyle, AnimRecurOutDuration)
                .Ease(Easing.Linear)
                .OnCompleted(() =>
                {
                    this.Q<Label>("TouchScreen_Text").experimental.animation
                        .Start(_fadeOutStyle, _fadeInStyle, AnimRecurInDuration)
                        .Ease(Easing.Linear)
                        .OnCompleted(AnimationTouchScreen);
                });
        }

        
        
        public new class UxmlFactory : UxmlFactory<MainMenu_Document, UxmlTraits>
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
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
            }
        }
    }
}