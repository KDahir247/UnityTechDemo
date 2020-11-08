using System.Collections;
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

        private SkillData[] _skills;
        public SkillData[] skills => _skills;
        
        //Create the equipment compatible 

        public void SkillRetrieved(IList<SkillData> skillDataDownloaded)
        {
            int skillDataCount = skillDataDownloaded.Count;
            _skills = new SkillData[skillDataCount];

            for (int i = 0; i < skillDataCount; i++)
            {
                _skills[i] = skillDataDownloaded[i];
            }
        }
    }
}