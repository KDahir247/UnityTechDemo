using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using MasterData;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
using Tech.Data.Scriptable;
using Tech.DB;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class temp : MonoBehaviour
{
    readonly TechDBBuilder _dbBuilder = new TechDBBuilder();
    [FormerlySerializedAs("_skill")] [SerializeField] private SkillDataDB skill;
    readonly List<Skill> _skills = new List<Skill>();
    
    [SerializeField]
    public GameObject test;

    // Start is called before the first frame update
    async UniTaskVoid Start()
    {
        for (int i = 0; i < skill.Skills.Count; i++)
        {
            _skills.Add(new Skill
            {
                index = i, 
                Bytes = skill.Skills[i].image.GetRawTextureData(),
                Id = Ulid.NewUlid(),
                name = skill.Skills[i].name
            });
            break;
        }


        await _dbBuilder.Build(builder =>
        {
            builder.Append(_skills);
            return builder;
        }, "master-data");
        
        

    }
}
