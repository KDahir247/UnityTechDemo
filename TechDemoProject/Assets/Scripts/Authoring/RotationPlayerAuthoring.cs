using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Tech.Authoring
{
    public class RotationPlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity,
                new ComponentTypes(ComponentType.ReadWrite<CopyTransformFromGameObject>(),
                    ComponentType.ReadWrite<LocalToWorld>()));
        }
    }
}