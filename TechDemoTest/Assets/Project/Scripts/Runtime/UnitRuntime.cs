using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct UnitRuntime : IComponentData
{
    public float startingAction;
    public float storedStartingAction;
    public bool actionTime;
}
