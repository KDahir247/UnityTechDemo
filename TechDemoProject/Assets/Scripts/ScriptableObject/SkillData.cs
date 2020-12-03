using UnityEngine;
using UnityEngine.Serialization;

namespace Tech.Data.Scriptable
{
    [CreateAssetMenu(fileName = "defaultSkill", menuName = "Character/Data/Skill", order = 32)]
    [PreferBinarySerialization]
    public class SkillData : ScriptableObject
    {
        [FormerlySerializedAs("Imge")] public Texture2D Image;

        //Other information
    }
}