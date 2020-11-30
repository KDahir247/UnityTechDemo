using JetBrains.Annotations;
using MasterData;
using UnityEngine;

namespace Tech.DB
{
    public static class TechDB
    {
        [NotNull]
        public static MemoryDatabase LoadDataBase(string filName, bool internString = true)
        {
            return new MemoryDatabase(Resources.Load<TextAsset>(filName)?.bytes, internString);
        }
    }
}