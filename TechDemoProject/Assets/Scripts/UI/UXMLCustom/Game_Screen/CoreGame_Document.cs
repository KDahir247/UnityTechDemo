using JetBrains.Annotations;
using Tech.Data;
using Tech.UI.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Tech.UI.Panel
{
    public class CoreGame_Document : BaseDocument
    {
        private VisualElement _gameElement;
        private Button _hideAllButton;

        private bool _isHiding;
        private VisualElement _shopElement;

        private readonly Label[] _currencyLabel = new Label[3];
        private readonly Button[] _rechargeButton = new Button[3];

        private GameWallet _wallet;
        public CoreGame_Document()
            : base(500, 500)
        {
        }

        protected override void Init(params string[] scenes)
        {
            GameFoundationSdk.initialized += () =>
            {
                _wallet = new GameWallet();

                _wallet.WalletValueChanged().Subscribe(currency =>
                {
                    if (currency.TryGetStaticProperty("Label-Index", out var property))
                    {
                        if (_currencyLabel[property.AsInt()] == null) return;

                        _currencyLabel[property.AsInt()].text = currency.quantity.ToString();
                    }
                });
            };
        }

        protected override void UIQuery()
        {
            _gameElement = this.Q<VisualElement>("Game_Document");
            _shopElement = this.Q<VisualElement>("Shop_Document");

            _hideAllButton = this.Q<Button>("HideAll_Button");

            _currencyLabel[0] = this.Q<Label>("CredAmount_Text");
            _currencyLabel[1] = this.Q<Label>("NoteAmount_Text");
            _currencyLabel[2] = this.Q<Label>("StaminaAmount_Text");

            _rechargeButton[0] = this.Q<Button>("RechargeCred_Button");
            _rechargeButton[1] = this.Q<Button>("RechargeNote_Button");
            _rechargeButton[2] = this.Q<Button>("RechargeStamina_Button");
        }

        protected override void RegisterCallback()
        {
            _hideAllButton.RegisterCallback(HideAllShowAll<ClickEvent>());

            //Recharging
            _rechargeButton[0].RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Cred));
            _rechargeButton[1].RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Note));
            _rechargeButton[2].RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Stamina));

            _currencyLabel[0].text = _wallet?.GetWalletBalance(_currencyLabel[0].viewDataKey).ToString();
            _currencyLabel[1].text = _wallet?.GetWalletBalance(_currencyLabel[1].viewDataKey).ToString();
            _currencyLabel[2].text = _wallet?.GetWalletBalance(_currencyLabel[2].viewDataKey).ToString();
        }

        protected override void UnregisterCallback()
        {
            _hideAllButton.UnregisterCallback(HideAllShowAll<ClickEvent>());

            //Recharging
            _rechargeButton[0].UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Cred));
            _rechargeButton[1].UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Note));
            _rechargeButton[2].UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Stamina));
        }

        protected override void OnDispose()
        {
            _wallet.Dispose();
        }

        [NotNull]
        private EventCallback<T> ShowRechargePanel<T>(RechargeType rechargeType)
            where T : EventBase<T>, new()
        {
            return evt =>
            {
                HideAllShowAll<T>()
                        .Invoke(evt); //??

                //_shopElement.FadeInOrOut(FadeOutStyle, FadeInStyle, Easing.Linear, FadeInDuration); TODO shop element is null currently
            };
        }


        [NotNull]
        private EventCallback<T> HideAllShowAll<T>()
            where T : EventBase<T>, new()
        {
            return evt =>
            {
                UnregisterCallback();
                _isHiding = !_isHiding;

                if (_isHiding)
                {
                    _gameElement.FadeInOrOut(FadeInStyle, FadeOutStyle, Easing.Linear, FadeOutDuration, () =>
                    {
                        _gameElement.style.display = DisplayStyle.None;
                        _hideAllButton.text = "Show";
                        RegisterCallback();

                    });
                }
                else
                {
                    _gameElement.style.display = DisplayStyle.Flex;
                    _gameElement.FadeInOrOut(FadeOutStyle, FadeInStyle, Easing.Linear, FadeInDuration, () =>
                    {
                        _hideAllButton.text = "Hide";
                        RegisterCallback();
                    });
                }
            };
        }

        public new class UxmlFactory : UxmlFactory<CoreGame_Document, UxmlTraits>
        {
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((CoreGame_Document) ve).Init();
            }
        }
    }
}