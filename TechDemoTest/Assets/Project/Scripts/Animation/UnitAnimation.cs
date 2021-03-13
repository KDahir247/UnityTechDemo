using System;
using Unity.Entities;
using Unity.Kinematica;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Kinematica))]
public sealed class UnitAnimation : MonoBehaviour
{
    private Kinematica _kinematica;
    private Identifier<SelectorTask> locomotion;

    [SerializeField] private float desiredSpeed = 1.5f;
    [SerializeField] private float accelerationDistance = 0.01f;
    [SerializeField] private float decelerationsDistance = 1.0f;
    
    internal EntityManager DstManager;
    internal Entity Self;

    void OnEnable()
    {
        _kinematica = gameObject.GetComponent<Kinematica>();

        ref MotionSynthesizer synthesizer = ref _kinematica.Synthesizer.Ref;

        synthesizer.PlayFirstSequence(
            synthesizer.Query.Where(
                Locomotion.Default).And(Idle.Default));

        var selector = synthesizer.Root.Selector();

        {
            var sequence = selector.Condition().Sequence();

            sequence.Action().MatchPose(
                synthesizer.Query.Where(
                    Locomotion.Default).And(Idle.Default), 0.01f);

            sequence.Action().Timer();
        }

        {
            var action = selector.Action();

            action.MatchPoseAndTrajectory(
                synthesizer.Query.Where(
                    Locomotion.Default).Except(Idle.Default),
                action.Navigation().GetAs<NavigationTask>().trajectory);
        }

        locomotion = selector.GetAs<SelectorTask>();
        //Start Debugging
        Unity.Kinematica.DebugDraw.Begin();
    }

    void Update()
    {
        ref var synthesizer = ref _kinematica.Synthesizer.Ref;

        synthesizer.Tick(locomotion);


        if (DstManager.HasComponent<UnitExecutionRuntime>(Self))
        {
            ref NavigationTask navigation = ref synthesizer.GetChildByType<NavigationTask>(locomotion).Ref;
            ref ConditionTask idle = ref synthesizer.GetChildByType<ConditionTask>(locomotion).Ref;

            //TODO make this only get called once later rather then OnUpdate()
            var data = DstManager.GetComponentData<UnitExecutionRuntime>(Self);

            Vector3 targetPosition = data.targetEntityPosition;
            NavMeshPath navMeshPath = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, navMeshPath)) //add a offset
            {
                NavigationParams navParams = new NavigationParams()
                {
                    desiredSpeed = desiredSpeed,
                    maxSpeedAtRightAngle = 0.0f,
                    maximumAcceleration = NavigationParams.ComputeAccelerationToReachSpeed(desiredSpeed, accelerationDistance),
                    maximumDeceleration = NavigationParams.ComputeAccelerationToReachSpeed(desiredSpeed, decelerationsDistance),
                    intermediateControlPointRadius = 1.0f,
                    finalControlPointRadius = 0.15f,
                    pathCurvature = 5.0f
                };

                float3[] points = Array.ConvertAll(navMeshPath.corners, pos => new float3(pos));
                navigation.FollowPath(points,navParams);
            }

            idle.value = math.distance(transform.position, targetPosition) <= 1.0f;

            //Debugging
            navigation.DrawPath();
        }
    }

    private void OnDisable()
    {
        //End Debugging
        Unity.Kinematica.DebugDraw.End();
    }
}
