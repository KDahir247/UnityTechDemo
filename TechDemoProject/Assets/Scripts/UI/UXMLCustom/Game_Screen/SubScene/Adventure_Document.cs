﻿using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Adventure_Document : Base_Document
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
        
        public new class UxmlFactory : UxmlFactory<Adventure_Document, UxmlTraits>
        {
        }
        
        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                
                ((Adventure_Document)ve).Init();
            }
        }
        
    }
}