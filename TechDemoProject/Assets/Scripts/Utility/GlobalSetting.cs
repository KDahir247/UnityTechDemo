using System.Collections.Generic;
using Tech.Core;
using Tech.Data.Scriptable;
using UniRx;
using UnityEngine;

//Container for Global Parameter.
//Test File. File hasn't been finalized 

namespace Tech.Utility
{
    public static class GlobalSetting
    {
        public static readonly StringReactiveProperty ReactiveUnityVersion =
            new StringReactiveProperty(Application.unityVersion);

        public static readonly StringReactiveProperty ReactiveVersion = new StringReactiveProperty(Application.version);

        public static readonly Dictionary<string, CharacterData> StoredCharacter = new Dictionary<string, CharacterData>();
        
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