using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

[RequireComponent( typeof(UnitAnimation))]
[DisallowMultipleComponent]
[RequiresEntityConversion]
public sealed class UnitAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    private UnitAnimation unitAnimation;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (dstManager.HasComponent<UnitRuntime>(entity)) return;

        unitAnimation = gameObject.GetComponent<UnitAnimation>();
        unitAnimation.Self = entity;
        unitAnimation.DstManager = dstManager;

        dstManager.AddComponents(entity, new ComponentTypes(new ComponentType(typeof(UnitRuntime)), ComponentType.ReadOnly<CopyTransformToGameObject>()));
        dstManager.SetComponentData(entity, new UnitRuntime
        {
            storedStartingAction = Random.Range(3,8)
        });
    }
}
