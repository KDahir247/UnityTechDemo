using System;
using System.Reflection;
using System.IO;
using UnityEngine;

namespace Tech.Core
{
    public static class LogHelper
    {
        //TODO responsible for removing old log in the editor log folder or do action before removing old log

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            var directory = Directory.GetFiles($@"{Environment.CurrentDirectory}\Assets\Log\");

            // foreach (var s in directory)
            // {
            //     Debug.Log(s);
            // }
            
        }
        
    }
}