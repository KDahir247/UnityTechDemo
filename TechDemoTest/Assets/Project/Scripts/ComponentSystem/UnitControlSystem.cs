using System.Threading;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public sealed class UnitControlSystem : JobComponentSystem
{
    private EntityQuery _entityQuery;
    private EndSimulationEntityCommandBufferSystem _entityCommandBuffer;
    public int Limit = 1;
    protected override void OnStartRunning()
    {
        _entityQuery = GetEntityQuery(ComponentType.ReadWrite<UnitRuntime>());
        _entityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    /*
    protected override void OnUpdate()
    {
         // var parallelWriter = _entityCommandBuffer.CreateCommandBuffer();
         var c = _entityQuery.ToEntityArray(Allocator.Temp);

       
         //NativeArray<Entity> entities = _entityQuery.ToEntityArray(Allocator.TempJob);

        /*parallelWriter.DestroyEntity(entities[0]);
        var gameObjectToDelete = EntityManager.GetComponentObject<Animator>(entities[0]).gameObject;
        _assetSystem.UnloadAsset(gameObjectToDelete);#1#
    }*/

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
       return Entities.ForEach((Entity entity, ref Translation translation, in UnitRuntime unitRuntime) =>
       {
       })
           .Schedule(inputDeps);
    }
}