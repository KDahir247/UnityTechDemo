using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Tech.Utility
{
    public class VersionValidation : MonoBehaviour
    {
        [SerializeField] private Text versionText;
        public void ValidateVersion()
        {
            if (versionText.text != GlobalSetting<VersionValidation>.ReactiveVersion.Value)
            {
                //version are different
            }
        }

        public void Awake()
        {
            GlobalSetting<VersionValidation>.ReactiveVersion.SubscribeToText(versionText);
        }
    }
}