using JetBrains.Annotations;
using Tech.DB;
using Tech.UI.Linq;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class CoreCreation_Document : BaseDocument
    {
        private Button _createButton;
        private VisualElement _creationElement;
        private VisualElement _dialogueElement;
        public Unit CurrentUnit { get; set; }

        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _createButton = this.Q<Button>("Create_Button");

            _creationElement = this.Q<VisualElement>("Creation_Document");
            _dialogueElement = this.Q<VisualElement>("Dialogue_Document");
        }

        protected override void RegisterCallback()
        {
            _createButton?.RegisterCallback(OnCreateUser<ClickEvent>(_creationElement, _dialogueElement));
        }

        protected override void UnregisterCallback()
        {
            _createButton?.UnregisterCallback(OnCreateUser<ClickEvent>(_creationElement, _dialogueElement));
        }

        [NotNull]
        private EventCallback<T> OnCreateUser<T>(VisualElement fadeOutTarget, VisualElement fadeInTarget)
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                UnregisterCallback();
                fadeOutTarget.SwitchDisplay(fadeInTarget);
            };
        }

        public new class UxmlFactory : UxmlFactory<CoreCreation_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ((CoreCreation_Document) ve).Init();
            }
        }
    }
}