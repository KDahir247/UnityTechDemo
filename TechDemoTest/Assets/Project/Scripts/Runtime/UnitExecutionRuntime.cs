using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct UnitExecutionRuntime : IComponentData
{
    public Entity unitEntity;
    public float3 unitEntityPosition;

    public Entity targetEntity;
    public float3 targetEntityPosition;
}
