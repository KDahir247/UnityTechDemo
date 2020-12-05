using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tech.DB
{
    public class DatabaseInitialization : MonoBehaviour
    {
        private readonly TechStaticDBBuilder _dbBuilder = new TechStaticDBBuilder();


        //create all the files with a null value to start with
        private async UniTaskVoid Awake()
        {
            await _dbBuilder.Build(builder =>
            {
                builder.Append(new[] {new Unit {Name = "Nil"},});
                return builder;
            }, FileDestination.UnitPath);
        }
    }
}