using System;
using System.IO;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

//Script Happens Separately from ECS System 

namespace Tech.Core
{
#if UNITY_EDITOR
    public static class LogHelper
    {
        public static event VII<int, string, string> OnDelete;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            string[] directory = Directory.GetFiles($@"{Environment.CurrentDirectory}\Assets\Log\");

            foreach (string file in directory)
            {
                DateTime date = File.GetLastWriteTimeUtc(file);
                
                if (Mathf.Abs(date.Date.ToLocalTime().Day - DateTime.Today.Day) > 10)
                {
                    OnDelete?.Invoke(date.Day, $@"{Environment.CurrentDirectory}\Assets\Log\", file);
                    File.Delete(file);
                }
            }
            
            Application.quitting += () =>
            {
                if (OnDelete != null)
                    foreach (var @delegate in OnDelete.GetInvocationList())
                    {
                        OnDelete -= @delegate as VII<int,string,string>;
                    }

                OnDelete = null;
            };
        }
    }
#endif
}