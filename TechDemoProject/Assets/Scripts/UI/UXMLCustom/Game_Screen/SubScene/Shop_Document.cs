using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.GameFoundation;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class Shop_Document : BaseDocument
    {
        private GameTransaction _transaction;
        private GameStore _store;
        private ScrollView _currentShopView;

        private readonly string[] _shopButtonsNames = {
            "BestSeller_Button",
            "Pack_Button",
            "RegularSupply_Button",
            "CredRecharge_Button",
            "NoteRecharge_Button",
            "LimitedTime_Button",
            "Exchange_Button"
        };

        private readonly Button[] _shopButtons = new Button[7];
        private readonly ScrollView[] _shopViews = new ScrollView[7];
        private List<Button> _transactionButtons = new List<Button>();
        protected override void Init(params string[] scenes)
        {
            GameFoundationSdk.initialized += () =>
            {
                _transaction = new GameTransaction();
                _store = new GameStore();

                _transaction.OnTransactionCompleted()
                    .Subscribe(transactionResult =>
                {
                    Debug.Log("Successfully");
                    //When the transaction is successful it auto-magically add the payout to the correct stuff(money, inventory)
                    //We want a pop-up and display the result on the transaction. cost of payment, payout, and/or a thank you message.
                });

                _transaction.OnTransactionFailed()
                    .Subscribe(exception =>
                {
                    Debug.Log("Failed");
                    //We want a pop-up and display the error on the transaction. Maybe not enough money or something else
                });
            };

        }

        protected override void UIQuery()
        {
            for (byte index = 0; index < _shopButtonsNames.Length; index++)
            {
                _shopButtons[index] = this.Q<Button>(_shopButtonsNames[index]);
                _shopViews[index] = this.Q<ScrollView>(_shopButtons[index].viewDataKey);
            }
        }

        protected override void RegisterCallback()
        {
            for (byte index = 0; index < _shopButtons.Length; index++)
                _shopButtons[index].RegisterCallback(RefreshShop<ClickEvent>(index));
        }

        protected override void UnregisterCallback()
        {
        }

        protected override void OnDispose()
        {
            _transaction.Dispose();
            _store.Dispose();
        }

        public new class UxmlFactory : UxmlFactory<Shop_Document, UxmlTraits>
        {
        }

        [NotNull]
        private EventCallback<T> RefreshShop<T>(int shopIndex)
            where T : EventBase<T>, new()
        {
            return evt =>
            {
                if (_currentShopView == _shopViews[shopIndex]) return;

                if (_currentShopView != null)
                {
                    foreach (var button in _transactionButtons)
                        button.UnregisterCallback(OnPurchase<ClickEvent>(_currentShopView.viewDataKey, button.viewDataKey));

                    _transactionButtons.Clear();
                    _currentShopView.Clear();
                    _currentShopView.style.display = DisplayStyle.None;
                }

                _shopViews[shopIndex].style.display = DisplayStyle.Flex;
                _currentShopView = _shopViews[shopIndex];

                var baseTransactions = _store.RetrieveStoreTransactions(_currentShopView.viewDataKey);

                if (baseTransactions == null) return;

                //StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UI/USS/ShopStyle.uss");
                for (byte i = 0; i < baseTransactions.Count; i++)
                {
                    Button transactionBut = new Button
                    {
                        name = baseTransactions[i].displayName,
                        viewDataKey = baseTransactions[i].key,
                        style = {height = 200, width =200} //TODO currently style is hardcoded later it will read from a uss file
                    };

                    transactionBut.RegisterCallback(OnPurchase<ClickEvent>(_currentShopView.viewDataKey, transactionBut.viewDataKey));

                    _currentShopView.Add(transactionBut);
                    _transactionButtons.Add(transactionBut);
                    //transactionBut.styleSheets.Add(styleSheet);
                }
            };
        }

        [NotNull]
        private EventCallback<T> OnPurchase<T>([NotNull] string storeKey, [NotNull] string baseTransactionKey)
            where T : EventBase<T>, new()
        {
            return evt => _store.PurchaseFromStore(storeKey, baseTransactionKey);
        }

        public new sealed class UxmlTraits : VisualElement.UxmlTraits
        {
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((Shop_Document) ve).Init();
            }
        }
    }
}