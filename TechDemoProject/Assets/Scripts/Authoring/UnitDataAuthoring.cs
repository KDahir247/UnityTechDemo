using System;
using Tech.Animation;
using Tech.Data;
using Tech.Utility;
using UniRx;
using Unity.Entities;
using Unity.Kinematica;
using UnityEngine;

namespace Tech.ECS
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public sealed class UnitDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        //TODO might make UnitData a scriptableObject to make it more ergonomic
        [SerializeField] private UnitData unitData;
        internal UnitData UnitData => unitData;

        private Kinematica _kinematica;

        void Awake()
        {
            _kinematica = gameObject.GetComponent<Kinematica>();
            unitData.id = Ulid.NewUlid(DateTimeOffset.Now);

            PlayIdleAnimation();
        }

        private void PlayIdleAnimation()
        {
            ref var motionSynthesizer = ref _kinematica.Synthesizer.Ref;

            motionSynthesizer
                .Root
                .Action()
                .PlayFirstSequence(motionSynthesizer
                    .Query
                    .Where(Locomotion.Default)
                    .And(Idle.Default));
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity,
                new ComponentTypes(ComponentType.ReadWrite<UnitRuntime>()));

            MessageBroker
                .Default
                .Receive<(DB.Unit, DB.Skill)>() //current unit, desired skill
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