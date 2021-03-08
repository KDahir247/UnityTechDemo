using System;
using Cysharp.Threading.Tasks;
using Tech.Core;
using Tech.Data;
using Tech.DB;
using Tech.Utility;
using UniRx;
using Unity.Entities;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Unit = Tech.DB.Unit;

namespace Tech.ECS
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public sealed class UnitDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private static readonly ILogger Logger = LogManager.GetLogger("AuthoringLogger");

        //TODO remove this for an IComponentData and once the IComponentData 
        [SerializeField] private UnitData unitData;

        private readonly DatabaseStream _dbStream = new DatabaseStream();
        private DynamicDbBuilder _dynamicDb;

        private async UniTaskVoid Awake()
        {
            _dynamicDb = new DynamicDbBuilder(_dbStream);
            //Create the Universally Unique Lexicographically Sortable Identifier
            unitData.id = Ulid.NewUlid(DateTimeOffset.Now);
                _dynamicDb.DynamicallyMutateDatabase(FileDestination.UnitPath, builder =>
                {
                    builder.Diff(new[]
                    {
                        new Unit
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
                        }
                    });

                    return builder;
                });

               await _dynamicDb.BuildToDatabaseAsync();
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //ECS/DOTS goes here
            dstManager.AddComponents(entity,
                new ComponentTypes(ComponentType.ReadWrite<UnitRuntime>()));

            MessageBroker
                .Default
                .Receive<(Unit, Skill)>() //current unit, desired skill
                .Subscribe(valueTuple =>
                {
                    var enable = TechUtility.UnRegisterUlid(valueTuple.Item1.Id)
                        .CompareTo(unitData.id) == 0;

                    transform
                        .GetChild(1)
                        .gameObject
                        .SetActive(enable);

                    dstManager.SetComponentData(entity, new UnitRuntime
                    {
                        skillIndex = valueTuple.Item2?.Index ?? 0,
                        ulid = unitData.id,
                        enabled = enable
                    });
                });
        }
    }
}