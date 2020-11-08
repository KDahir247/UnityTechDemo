using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tech.Core;
using Tech.Data.Scriptable;
using Tech.Report;
using Tech.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Initialization
{
    //Initialize all the character skill
    //TODO make the main screen stall until this script is done.
    public class Initialization : MonoBehaviour
    {
        [FormerlySerializedAs("_characterData")] [SerializeField]
        private List<CharacterData> characterData = new List<CharacterData>();

        private bool pass;
        private async UniTaskVoid Awake()
        {
            foreach (var data in characterData)
            {
                 pass = await DataAddress.LoadCharacterData(data, new DataProgress());
                 GlobalSetting.StoredCharacter.Add(data.key, data);
            }
            if (pass)
            {
                Destroy(this.gameObject, 1);
            }
            else
            {
                LogManager.Logger.LogError("Failed to initialize Character Asset");
            }
        }
    }
}