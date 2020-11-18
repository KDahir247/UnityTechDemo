using System.Collections;
using System.Collections.Generic;
using Tech.UI.Panel;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

//Handles Logic in the Option Document.
namespace Tech.UI.Panel
{
    public class Option_Document : VisualElement
    {
        private TitleScreen_Document _parentDocument; //reference to parent Document

        public Option_Document()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }

        private void OnGeometryChange(GeometryChangedEvent evt)
        {
                
                UnregisterCallback<GeometryChangedEvent>(OnGeometryChange);
        }


        public void OnInitialize(TitleScreen_Document elementParent) { }
        
        public new class UxmlFactory : UxmlFactory<Option_Document, UxmlTraits>
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