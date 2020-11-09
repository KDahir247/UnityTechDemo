using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class MainMenu_Document : VisualElement
    {
        private VisualElement _coreElement;

        public MainMenu_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
        }

        public new class UxmlFactory : UxmlFactory<MainMenu_Document, UxmlTraits>
        {
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