using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tech.Data;
using Tech.DB;
using UnityEngine;

//There will be 8 core scripts that will run async on seperate thread to save the bare minimum for the database.
//All unity, skill, equipment, item, material, and enemy will contain the script to append to the database. when needed.
//so Performance is kept minimum.

//Example Script of in-memory database in action
public class temp : MonoBehaviour
{
    private readonly TechStaticDBBuilder _dbBuilder = new TechStaticDBBuilder();
    private readonly TechDynamicDBBuilder _dynamicDbBuilder = new TechDynamicDBBuilder();

    private readonly List<Skill> _skills = new List<Skill>();

    //testing variables
    [SerializeField] private UnitData data;

    //Example Of how to use the database 
    private async UniTaskVoid Start()
    {
        await _dbBuilder.Build(builder =>
        {
            builder.Append(_skills);
            return builder;
        }, FileDestination.SkillPath);


        if (_dynamicDbBuilder.TryLoadDatabase(FileDestination.SkillPath, out var a))
        {
            a.Diff(new[]
            {
                new Skill {Name = "Help", Index = 11}
            });

             _dynamicDbBuilder.Build(a);
        }
    }
}