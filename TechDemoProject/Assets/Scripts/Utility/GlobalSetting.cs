using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
//Container for Global Parameter.
//Test File. File hasn't been finalized 

namespace Tech.Utility
{
    public static class GlobalSetting<T>
    { 
        public static readonly ScheduledNotifier<T> ScheduledNotifier = new ScheduledNotifier<T>();
        public static readonly StringReactiveProperty ReactiveUnityVersion = new StringReactiveProperty(Application.unityVersion);
        public static readonly StringReactiveProperty ReactiveVersion = new StringReactiveProperty(Application.version);
    }
}
