using System.Collections;
using System.Collections.Generic;
using Tech.DB;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

namespace Tech.Core
{
    public static class AdaptivePerformanceConfig
    {
        private static readonly DatabaseStream DatabaseStream = new DatabaseStream();
        private static StaticDbBuilder _dbBuilder = new StaticDbBuilder(DatabaseStream);


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void SetUp()
        {
        }

    }
}
