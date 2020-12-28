﻿using System;
using JetBrains.Annotations;
using MasterMemory;
using Tech.Data;
using Tech.UI.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tech.UI.Panel
{
    public class CoreGame_Document : Base_Document
    {
        private Button _hideAllButton;

        private Button _rechargeStamina;
        private Button _rechargeNote;
        private Button _rechargeCred;
        
        
        private VisualElement _gameElement;
        private VisualElement _shopElement;

        
        private bool _isHiding;
        private bool _isStarting;
        
        
        public CoreGame_Document() : base(500,500)
        {
            
        }
        
        protected override void Init(params string[] scenes)
        {
        }

        protected override void UIQuery()
        {
            _hideAllButton = this.Q<Button>("HideAll_Button");

            _gameElement = this.Q<VisualElement>("Game_Document");
            _shopElement = this.Q<VisualElement>("Shop_Document");
            
            _rechargeCred = this.Q<Button>("RechargeCred_Button");
            _rechargeNote = this.Q<Button>("RechargeNote_Button");
            _rechargeStamina = this.Q<Button>("RechargeStamina_Button");
        }

        protected override void Start()
        {
            _hideAllButton.RegisterCallback(HideAllShowAll<ClickEvent>());
            
            
            
            //Recharging
            _rechargeCred.RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Cred));
            _rechargeNote.RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Note));
            _rechargeStamina.RegisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Stamina));
        }

        protected override void OnDestroy()
        {
            _hideAllButton.UnregisterCallback(HideAllShowAll<ClickEvent>());
            
            //Recharging
            _rechargeCred.UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Cred));
            _rechargeNote.UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Note));
            _rechargeStamina.UnregisterCallback(ShowRechargePanel<ClickEvent>(RechargeType.Stamina));
        }


        [NotNull]
        private EventCallback<T> ShowRechargePanel<T>(RechargeType rechargeType)
            where T : EventBase<T>, new()
        {
            return evt =>
            {
                
                if (!_isHiding)
                    HideAllShowAll<T>()
                        .Invoke(evt);
                
                _shopElement.FadeInOrOut(FadeOutStyle, FadeInStyle, FadeInDuration);
                
                
            };
        }
        
        
        
        

        [NotNull]
        private EventCallback<T> HideAllShowAll<T>()
            where T : EventBase<T>, new()
        {
            return evt =>
            {
                if(_isStarting) return;
                _isStarting = true;
                _isHiding = !_isHiding;

                if (_isHiding)
                {
                    _gameElement.FadeInOrOut(FadeInStyle, FadeOutStyle, FadeOutDuration, () =>
                    {
                        _gameElement.style.display = DisplayStyle.None;
                        _hideAllButton.text = "Show";
                        _isStarting = false;
                    });
                }
                else
                {
                    _gameElement.style.display = DisplayStyle.Flex;
                    _gameElement.FadeInOrOut(FadeOutStyle, FadeInStyle, FadeInDuration, () =>
                    {
                        _hideAllButton.text = "Hide";
                        _isStarting = false;
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
                ((CoreGame_Document)ve).Init();
            }
        }
        
    }
}