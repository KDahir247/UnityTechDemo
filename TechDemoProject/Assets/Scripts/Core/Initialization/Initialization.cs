using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tech.Core;
using Tech.Data.Scriptable;
using Tech.Report;
using Tech.Utility;
using UnityEngine;
using UnityEngine.Serialization;

//TODO remove for in memory database
namespace Tech.Initialization
{
    //Initialize all the character skill
    //TODO remove
    public class Initialization : MonoBehaviour
    {
        [FormerlySerializedAs("_characterData")] [SerializeField]
        private List<CharacterData> characterData = new List<CharacterData>();

        private bool pass;

        private async UniTaskVoid Awake()
        {
            foreach (var data in characterData)
            {
                pass = await DataAddress.LoadCharacterData(data, new DataProgress("Get Character Data"));
                GlobalSetting.StoredCharacter.Add(data.key, data);
            }

            if (pass)
                Destroy(gameObject, 1);
            else
                LogManager.Logger.LogError("Failed to initialize Character Asset");
        }
    }
}