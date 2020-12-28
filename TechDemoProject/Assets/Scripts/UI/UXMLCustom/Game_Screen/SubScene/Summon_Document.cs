using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Summon_Document : Base_Document
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

        public new class UxmlFactory : UxmlFactory<Summon_Document, UxmlTraits>
        {
        }
        
        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((Summon_Document)ve).Init();
            }
        }
    }
}