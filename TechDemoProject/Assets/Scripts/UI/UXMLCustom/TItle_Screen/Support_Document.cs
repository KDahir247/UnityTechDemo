﻿using UnityEngine.UIElements;

//Handles logic in the support document
namespace Tech.UI.Panel
{
    public class Support_Document : BaseDocument
    {
        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
        }

        protected override void RegisterCallback()
        {
        }

        protected override void UnregisterCallback()
        {
        }

        public new class UxmlFactory : UxmlFactory<Support_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                ((Support_Document) ve).Init();
            }
        }
    }
}