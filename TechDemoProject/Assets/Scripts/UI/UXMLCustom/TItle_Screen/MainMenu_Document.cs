using Tech.UI.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class MainMenu_Document : Base_Document
    {
        private Label _idLabel;
        private Label _touchLabel;
        private Label _versionLabel;

        public MainMenu_Document() : base(2000, 2000)
        {
        }


        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _idLabel = this.Q<Label>("ID_Text");
            _versionLabel = this.Q<Label>("Version_Text");
            _touchLabel = this.Q<Label>("TouchScreen_Text");
        }

        protected override void Start()
        {
            _idLabel.text = "ID. 1111";
            _versionLabel.text = $"Ver.{Application.version}.{Application.unityVersion}";

            _touchLabel.RecursiveFadeOutIn(FadeInStyle, FadeOutStyle, FadeInDuration, FadeOutDuration);
        }

        protected override void OnDestroy()
        {
        }

        public new class UxmlFactory : UxmlFactory<MainMenu_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ((MainMenu_Document) ve).Init();
            }
        }
    }
}