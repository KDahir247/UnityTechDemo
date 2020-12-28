using System;
using Tech.Core;
using Tech.Data;
using Tech.DB;
using UniRx;
using Unity.Entities;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Unit = Tech.DB.Unit;

namespace Tech.ECS
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class UnitDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private static readonly ILogger Logger = LogManager.GetLogger("AuthoringLogger");

        private readonly TechDynamicDBBuilder _dynamicDb = new TechDynamicDBBuilder();

        //TODO remove this for an IComponentData and once the IComponentData 
        [SerializeField] private UnitData unitData;

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
                    var enable = _dynamicDb
                        .UnRegisterUlid(valueTuple.Item1.Id)
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

        private void Awake()
        {
            //Create the Universally Unique Lexicographically Sortable Identifier
            unitData.id = Ulid.NewUlid(DateTimeOffset.Now);

            if (_dynamicDb.TryLoadDatabase(FileDestination.UnitPath, out var immutableBuilder))
            {
                immutableBuilder.Diff(new[]
                {
                    new Unit
                    {
                        Id = _dynamicDb.RegisterUlid(unitData.id),
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

                _dynamicDb
                    .Build(immutableBuilder);
            }
            else
            {
                Logger.ZLogWarning($"File is Missing from unitPath {Application.dataPath}/Resources/user-data.bytes");
            }
        }
    }
}