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
        [SerializeField] private UnitData unitData;
        internal UnitData UnitData => unitData;

        void Awake()
        {
            unitData.id = Ulid.NewUlid(DateTimeOffset.Now);
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