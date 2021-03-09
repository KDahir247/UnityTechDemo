using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Tech.Data;
using Tech.DB;
using Tech.ECS;
using Tech.Utility;
using UnityEngine;

//TODO currently hardcoded will refactor this later since it will only get called once through the whole game.
public sealed class StarterCharacterStore : MonoBehaviour
{
    private readonly DatabaseStream _dbStream = new DatabaseStream();
    private StaticDbBuilder _dynamicDb;

    [SerializeField]
    private List<UnitDataAuthoring> _unitData = new List<UnitDataAuthoring>();
    void Start()
    {
        _dynamicDb = new StaticDbBuilder(_dbStream);

        _dynamicDb.StaticallyMutateDatabase(FileDestination.UnitPath, builder =>
        {
            builder.Append(new []
            {
                CreateUnit(_unitData[0].UnitData),
                CreateUnit(_unitData[1].UnitData),
                CreateUnit(_unitData[2].UnitData)
            });

            return builder;
        });

        _dynamicDb.BuildToDatabaseAsync().Forget();
    }

    [NotNull]
    Unit CreateUnit(UnitData unitData)
    {
        return new Unit
        {
            Id = TechUtility.RegisterUlid(unitData.id),
            Name = unitData.name,
            Address = unitData.name,
            Skills = new[]
            {
                new Skill
                {
                    Index = 1,
                    ImageBytes = unitData.skillDatas[0].image.GetRawTextureData()
                },
                new Skill
                {
                    Index = 2,
                    ImageBytes = unitData.skillDatas[1].image.GetRawTextureData()
                },
                new Skill
                {
                    Index = 3,
                    ImageBytes = unitData.skillDatas[2].image.GetRawTextureData()
                }
            }
        };
    }
}
