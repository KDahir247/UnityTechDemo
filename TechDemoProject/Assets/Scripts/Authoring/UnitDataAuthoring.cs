using System;
using Cysharp.Threading.Tasks;
using MasterData;
using Tech.Data;
using Tech.DB;
using UniRx;
using Unity.Entities;
using UnityEngine;
using Unit = Tech.DB.Unit;

namespace Tech.Authoring
{

    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class UnitDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private readonly TechDynamicDBBuilder _dynamicDb = new TechDynamicDBBuilder();
        
        private static bool _isLoading;

        [SerializeField] private UnitData unitData;

        private async UniTaskVoid Awake()
        {
            //Create the Universally Unique Lexicographically Sortable Identifier
            unitData.id  = Ulid.NewUlid(DateTimeOffset.Now);
            
            //Mutating database goes here
            
            await UniTask.WaitUntil(() => _isLoading == false);
            
            _isLoading = true;

            if (_dynamicDb.TryLoadDatabase(FileDestination.UnitPath, out ImmutableBuilder immutableBuilder))
            {
              
              immutableBuilder.Diff(new[]
                {
                    new Unit
                    {
                        Id = _dynamicDb.RegisterUlid(unitData.id),
                        Name = unitData.name,

                        Skills = new[]
                        {
                            new Skill {ImageBytes = unitData.skillDatas[0].image.GetRawTextureData()},
                            new Skill {ImageBytes = unitData.skillDatas[1].image.GetRawTextureData()},
                            new Skill {ImageBytes = unitData.skillDatas[2].image.GetRawTextureData()},
                        }
                    },
                });
              
                await _dynamicDb
                    .Build(immutableBuilder)
                    .ContinueWith(() => _isLoading = false);
            }
            
            
            MessageBroker
                .Default
                .Receive<Unit>().Subscribe(val => transform
                    .GetChild(1)
                    .gameObject
                    .SetActive(_dynamicDb.UnRegisterUlid(val.Id).CompareTo(unitData.id) == 0));

        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {

            //ECS/DOTS goes here

        }
    }
}