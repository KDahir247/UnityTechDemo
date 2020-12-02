using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MasterData;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
using Tech.Data.Scriptable;
using Tech.DB;
using Tech.Utility;
using UnityEngine;
using UnityEngine.Serialization;

public class temp : MonoBehaviour
{
    private readonly TechDBBuilder _dbBuilder = new TechDBBuilder(
        GeneratedResolver.Instance,
        BuiltinResolver.Instance,
        PrimitiveObjectResolver.Instance,
        UnityResolver.Instance,
        UnityBlitResolver.Instance,
        MasterMemoryResolver.Instance,
        StandardResolver.Instance);

    private readonly List<Skill> _skills = new List<Skill>();

    [FormerlySerializedAs("_skill")] [SerializeField]
    private SkillDataDB skill;

    // Start is called before the first frame update
    private async UniTaskVoid Start()
    {
        //doesn't need to happen every start
        for (var i = 0; i < skill.Skills.Count; i++)
            _skills.Add(new Skill
            {
                Index = i,
                ImageBytes = skill.Skills[i].image.GetRawTextureData(),
                Id = Ulid.NewUlid(),
                Name = skill.Skills[i].name
            });

        await _dbBuilder.Build(builder =>
        {
            builder.Append(_skills);
            return builder;
        }, GlobalSetting.SkillDataPath);
    }
}