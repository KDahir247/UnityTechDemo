using MasterData;
using Tech.DB;
using Unity.Entities;
using UnityEngine;

namespace Tech.Authoring
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class EnemyDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        private readonly TechDynamicDBBuilder _dynamicDb = new TechDynamicDBBuilder();
        
        public async void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //Mutating data base goes here
            if (_dynamicDb.TryLoadDatabase(FileDestination.EnemyPath, out ImmutableBuilder immutableBuilder))
            {
                immutableBuilder.Diff(new []
                {
                    new Enemy(), 
                });

                 _dynamicDb.Build(immutableBuilder);
            }
            
            //ECS/DOTS goes here
            
            
        }
    }
}