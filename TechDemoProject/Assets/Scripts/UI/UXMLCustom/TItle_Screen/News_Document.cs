using UnityEngine.UIElements;

//Handles logic in the New Panel
namespace Tech.UI.Panel
{
    public class News_Document : Base_Document
    {
        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
        }

        protected override void Start()
        {
        }

        protected override void OnDestroy()
        {
        }

        public new class UxmlFactory : UxmlFactory<News_Document, UxmlTraits>
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

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ((News_Document) ve).Init();
            }
        }
    }
}