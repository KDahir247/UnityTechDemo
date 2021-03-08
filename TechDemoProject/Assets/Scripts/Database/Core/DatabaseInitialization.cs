using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tech.DB
{
    public sealed class DatabaseInitialization : MonoBehaviour
    {
        private readonly StaticDbBuilder _dbBuilder = new StaticDbBuilder(new DatabaseStream());

        //create all the files with a null value to start with
        private async UniTaskVoid Awake()
        {
            _dbBuilder.StaticallyMutateDatabase(FileDestination.UnitPath, builder =>
            {
                builder.Append(new[] {new Unit {Name = "Nil"}});
                return builder;
            });

            await _dbBuilder.BuildToDatabaseAsync();
        }
    }
}