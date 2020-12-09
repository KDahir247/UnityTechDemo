using JetBrains.Annotations;
using Tech.UI.Linq;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Game_Document : Base_Document
    {
        private bool _fadingOut;

        private Button _hideAllButton;
        private VisualElement _mainBodyPanel;

        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _hideAllButton = this.Q<Button>("HideAll_Button");
            _mainBodyPanel = this.Q<VisualElement>("Body_Panel");
        }

        protected override void Start()
        {
            _hideAllButton.RegisterCallback(HideAllPanel<ClickEvent>());
        }

        protected override void OnDestroy()
        {
            _hideAllButton.UnregisterCallback(HideAllPanel<ClickEvent>());
        }


        [NotNull]
        private EventCallback<T> HideAllPanel<T>()
            where T : PointerEventBase<T>, new()
        {
            return evt =>
            {
                if (!(_mainBodyPanel.style.opacity.value <= 0) && !(_mainBodyPanel.style.opacity.value >= 1.0f)) return;

                _fadingOut = !_fadingOut;

                if (_fadingOut)
                {
                    _mainBodyPanel.FadeInOrOut(FadeInStyle, FadeOutStyle, FadeOutDuration,
                        () => _mainBodyPanel.style.display = DisplayStyle.None);
                }
                else
                {
                    _mainBodyPanel.style.display = DisplayStyle.Flex;
                    _mainBodyPanel.FadeInOrOut(FadeOutStyle, FadeInStyle, FadeInDuration);
                }
            };
        }


        public new class UxmlFactory : UxmlFactory<Game_Document, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                ((Game_Document) ve).Init();
            }
        }
    }
}