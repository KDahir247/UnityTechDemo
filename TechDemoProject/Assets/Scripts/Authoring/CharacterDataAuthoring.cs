using System;
using MasterData;
using Tech.Authoring;
using Tech.Data;
using Tech.DB;
using Tech.Runtime;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class CharacterDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    
    private readonly TechDynamicDBBuilder _dynamicDb = new TechDynamicDBBuilder();
    
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
    
    public async void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (_dynamicDb.TryLoadDatabase(FileDestination.SkillPath, out ImmutableBuilder immutableBuilder))
        {
            immutableBuilder.Diff(new []
            {
                new Skill{Id = Ulid.NewUlid(), Index = 0, Name = "death"}, 
            });
            

            await _dynamicDb.Build(immutableBuilder);
        }
        Debug.Log("lol");
        
        // Call methods on 'dstManager' to create runtime components on 'entity' here. Remember that:
        //
        // * You can add more than one component to the entity. It's also OK to not add any at all.
        //
        // * If you want to create more than one entity from the data in this class, use the 'conversionSystem'
        //   to do it, instead of adding entities through 'dstManager' directly.
        //
        // For example,
        //   dstManager.AddComponentData(entity, new Unity.Transforms.Scale { Value = scale });


    }
}
