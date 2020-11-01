using Tech.Core;
using UniRx;
using UnityEngine;

//Container for Global Parameter.
//Test File. File hasn't been finalized 

namespace Tech.Utility
{
    public static class GlobalSetting<T>
    {
        public static readonly ScheduledNotifier<T> ScheduledNotifier = new ScheduledNotifier<T>();

        public static readonly StringReactiveProperty ReactiveUnityVersion =
            new StringReactiveProperty(Application.unityVersion);

        public static readonly StringReactiveProperty ReactiveVersion = new StringReactiveProperty(Application.version);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void ValidateVersion()
        {
            if (Application.unityVersion != ReactiveUnityVersion.Value || Application.version != ReactiveVersion.Value)
                //Unity version is different return them back to title screen
                //Prepare for Update;

                //Go back to main menu
                StateSingleton.Instance.Machine.ChangeState(0);
        }
    }
}