using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Data.Scriptable
{
    //TODO this will replace the skillData scriptableObject
    [CreateAssetMenu(fileName = "defaultSkillDB", menuName = "Tech/Database/Skill", order = 0)]
    public class SkillDataDB : ScriptableObject
    {
        [FormerlySerializedAs("_skills")] [SerializeField]
        List<Skill> skills = new List<Skill>();

        public List<Skill> Skills => skills;

        //List struct 
        //the struct will have a string name and an image 

    }
}