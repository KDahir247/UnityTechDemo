using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public sealed class UnitQueueSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _simulationEntityCommandBufferSystem;

        protected override void OnStartRunning()
        {
            Entities.ForEach((Entity entity, ref UnitRuntime unitRuntime) =>
                unitRuntime.startingAction = unitRuntime.storedStartingAction)
                .Schedule();

            _simulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            float deltaTime = World.Time.DeltaTime;

            EntityArchetype executionArchetype = EntityManager
                .CreateArchetype(ComponentType.ReadWrite<UnitExecutionRuntime>());

           EntityCommandBuffer.ParallelWriter parallelWriter = _simulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();

           Entities
                .ForEach((Entity entity,int entityInQueryIndex, ref UnitRuntime unitRuntime, in Translation translation) =>
                {
                    if (!unitRuntime.actionTime && unitRuntime.startingAction <= 0)
                    {
                        Entity unitExec = parallelWriter.CreateEntity(entityInQueryIndex, executionArchetype);
                        parallelWriter.SetComponent(entityInQueryIndex, unitExec, new UnitExecutionRuntime
                        {
                            unitEntity = entity,
                            unitEntityPosition = translation.Value,
                            targetEntity = Entity.Null,
                            targetEntityPosition = float3.zero
                        });

                        unitRuntime.actionTime = true;
                    }else if (unitRuntime.startingAction > 0)
                    {
                        unitRuntime.startingAction = math.max(unitRuntime.startingAction - deltaTime, 0);
                    }
                }).ScheduleParallel();

            _simulationEntityCommandBufferSystem
                .AddJobHandleForProducer(Dependency);
        }
    }
