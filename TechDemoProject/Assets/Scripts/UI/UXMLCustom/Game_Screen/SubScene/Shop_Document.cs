using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Shop_Document : Base_Document
    {
        private ListView _shopCategory;
        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
         //   _shopCategory = this.Q<ListView>("");
        }

        protected override void Start()
        {
        }

        protected override void OnDestroy()
        {
        }

        public new class UxmlFactory : UxmlFactory<Shop_Document, UxmlTraits>
        {
        }
        
        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((Shop_Document)ve).Init();
            }
        }
    }
}