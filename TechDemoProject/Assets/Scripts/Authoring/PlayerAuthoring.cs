using System;
using Unity.Entities;
using Unity.Kinematica;
using Unity.Transforms;
using UnityEngine;

namespace Tech.Authoring
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        // Add fields to your component here. Remember that:
        //
        // * The purpose of this class is to store data for authoring purposes - it is not for use while the game is
        //   running.
        // 
        // * Traditional Unity serialization rules apply: fields must be public or marked with [SerializeField], and
        //   must be one of the supported types.
        //
        // For example,
        //    public float scale;

        
        private Kinematica _kinematica;
        private Entity _entity;
        private EntityManager _dstManager;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            _entity = entity;
            _dstManager = dstManager;
            
            InitializeDataStack(entity, ref dstManager);
            
        }

        private void InitializeDataStack(in Entity entity, ref EntityManager entityManager)
        {
            entityManager.AddComponents(entity, new ComponentTypes(
                ComponentType.ReadOnly<Kinematica>(),
                ComponentType.ReadOnly<Animator>(),
                ComponentType.ReadOnly<CopyTransformToGameObject>()));
        }
    }
}
