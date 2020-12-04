using System;
using Cysharp.Threading.Tasks;
using MasterData;
using Tech.Data;
using Tech.DB;
using UniRx;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;
using Unit = Tech.DB.Unit;

namespace Tech.Authoring
{

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class UnitDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private readonly TechDynamicDBBuilder _dynamicDb = new TechDynamicDBBuilder();
        private readonly TechStaticDBBuilder _dbBuilder = new TechStaticDBBuilder();

        private static bool _isLoading = false;

        [SerializeField] private UnitData unitData;

        private async UniTaskVoid Awake()
        {
            
            //Mutating database goes here
            await UniTask.WaitUntil(() => _isLoading == false);
            _isLoading = true;

            if (_dynamicDb.TryLoadDatabase(FileDestination.UnitPath, out ImmutableBuilder immutableBuilder))
            {
                var ulid = Ulid.NewUlid();
                
                
                immutableBuilder.Diff(new[]
                {
                    new Unit
                    {
                        Id = ulid,
                        Name = unitData.name,

                        Skills = new[]
                        {
                            new Skill {ImageBytes = unitData.skillDatas[0].image.GetRawTextureData()},
                            new Skill {ImageBytes = unitData.skillDatas[1].image.GetRawTextureData()},
                            new Skill {ImageBytes = unitData.skillDatas[2].image.GetRawTextureData()},
                        }
                    },
                });
                await _dynamicDb.Build(immutableBuilder).ContinueWith(() => _isLoading = false);
            }
            
            
            MessageBroker
                .Default
                .Receive<string>().Subscribe(val =>
                {
                
                    if (unitData.name == val)
                    {
                        transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(1).gameObject.SetActive(false);
                    }
                });

        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            //ECS/DOTS goes here

        }
    }
}