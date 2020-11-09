using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Tech.Data.Scriptable
{
    [CreateAssetMenu(fileName = "DefaultCharacter", menuName = "Character/Data/CharacterData", order = 33)]
    public class CharacterData : ScriptableObject
    {
        public string key;

        public AssetLabelReference[] labelToInclude; //skillData
        public SkillData[] skills { get; private set; }

        //Create the equipment compatible 

        public void SkillRetrieved(IList<SkillData> skillDataDownloaded)
        {
            var skillDataCount = skillDataDownloaded.Count;
            skills = new SkillData[skillDataCount];

            for (var i = 0; i < skillDataCount; i++) skills[i] = skillDataDownloaded[i];
        }
    }
}