using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
[DisallowMultipleComponent]
[RequiresEntityConversion]
public sealed class UnitAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (dstManager.HasComponent<UnitRuntime>(entity)) return;

        dstManager.AddComponents(entity, new ComponentTypes(new ComponentType(typeof(UnitRuntime)), ComponentType.ReadOnly<CopyTransformToGameObject>()));
        dstManager.SetComponentData(entity, new UnitRuntime
        {
            storedStartingAction = 5
        });
    }
}
