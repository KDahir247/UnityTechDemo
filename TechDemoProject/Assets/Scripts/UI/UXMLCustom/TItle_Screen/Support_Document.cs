using UnityEngine.UIElements;

//Handles logic in the support document
namespace Tech.UI.Panel
{
    public class Support_Document : VisualElement
    {
        private VisualElement _coreElement;


        public Support_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public TitleScreen_Document ParentDocument { get; set; } //reference to parent document

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        public void OnInitialize(TitleScreen_Document elementParent)
        {
        }

        public new class UxmlFactory : UxmlFactory<Support_Document, UxmlTraits>
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