using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;
using RaycastHit = Unity.Physics.RaycastHit;

public sealed class UnitRaycastSystem : SystemBase
{
    private BuildPhysicsWorld _buildPhysicWorld;
    private EndFramePhysicsSystem _endFramePhysics;
    private EndFixedStepSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem;
    private PlayerInput _input;
    private Vector3 _cursorPosition;
    private  Camera _camera;

    protected override void OnStartRunning()
    {
        _buildPhysicWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        _endFramePhysics = World.GetOrCreateSystem<EndFramePhysicsSystem>();
        _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndFixedStepSimulationEntityCommandBufferSystem>();

        //
        _input = new PlayerInput();
        _input.Enable();
        _input.Player.MousePos.performed += MovedCursor;
        _camera = Camera.main;
    }

    private void MovedCursor(InputAction.CallbackContext callbackContext)
    {
        _cursorPosition = callbackContext.ReadValue<Vector2>();
    }

    protected override void OnUpdate()
    {
        if (!_input.Player.Click.triggered) return;

        var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer();
        var entityQueue = new NativeQueue<RaycastHit>(Allocator.TempJob);
        var screenRay = _camera.ScreenPointToRay(_cursorPosition);

       Dependency = JobHandle.CombineDependencies(Dependency, _endFramePhysics.GetOutputDependency());

       Dependency = new PhysicsJob
        {
            RaycastInput = new RaycastInput
            {
                Start = screenRay.origin,
                End = screenRay.GetPoint(1000),
                Filter = new CollisionFilter
                {
                    BelongsTo = ~0u,
                    CollidesWith = ~0u,
                    GroupIndex = 0
                }
            },
            PhysicsWorld = _buildPhysicWorld.PhysicsWorld,
            EntityQueueParallelWriter = entityQueue.AsParallelWriter()

        }.Schedule();

        ComponentDataFromEntity<EnemyRuntime> targetEntity = GetComponentDataFromEntity<EnemyRuntime>();
        ComponentDataFromEntity<LocalToWorld> targetPosition = GetComponentDataFromEntity<LocalToWorld>(); //We want the actual entity position not hit position.

        //ComponentDataFromEntity<Local> myTypeFromEntity = GetComponentDataFromEntity<MyType>(true);
        Entities.ForEach((Entity e, in int entityInQueryIndex, in UnitExecutionRuntime unitRuntime) =>
        {
            if (!entityQueue.TryDequeue(out RaycastHit hit)) return;

            if (!targetEntity.HasComponent(hit.Entity)  || !targetPosition.HasComponent(hit.Entity)) return;

            entityCommandBuffer.AddComponent(unitRuntime.unitEntity, new UnitExecutionRuntime()
            {
                targetEntity = hit.Entity,
                targetEntityPosition = targetPosition[hit.Entity].Position,
                unitEntity = unitRuntime.unitEntity,
                unitEntityPosition = unitRuntime.unitEntityPosition
            });

            entityCommandBuffer.DestroyEntity(e);
        }).Run();
        //TODO find a solution to this.
        //Can't switch Run to Schedule with job dependency as Dependency since it will cause unity to close and throw an error from native queue collection.
        //Unity.Collections.LowLevel.Unsafe.UnsafeUtility.Free

        _endSimulationEntityCommandBufferSystem
            .AddJobHandleForProducer(Dependency);

        entityQueue.Dispose(Dependency);
    }

    protected override void OnStopRunning()
    {
        _input.Player.MousePos.performed -= MovedCursor;
        _input.Disable();
    }
}

[BurstCompile(FloatPrecision.Low, FloatMode.Fast)]
internal struct PhysicsJob : IJob
{
    [ReadOnly] public PhysicsWorld PhysicsWorld;
    [ReadOnly] public RaycastInput RaycastInput;
    [WriteOnly] public NativeQueue<RaycastHit>.ParallelWriter EntityQueueParallelWriter;

    public void Execute()
    {
        if (PhysicsWorld.CollisionWorld.CastRay(RaycastInput, out var raycastHit))
            EntityQueueParallelWriter.Enqueue(raycastHit);
    }
}
