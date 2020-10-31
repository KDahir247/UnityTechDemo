using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tech.Core;
using Tech.Runtime;
using Tech.Utility;
using Unity.Entities;
using UnityEngine;

//TODO find a better solution
public class AudioFadeSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Entities
        //         .WithAll<AudioFadeData>()
        //         .ForEach((Entity entity, ref AudioFadeData fadeData, AudioSource source) =>
        //         {
        //             source
        //                 .DOFade(0, fadeData.fadeDuration)
        //                 .Play()
        //                 .onComplete += (() => SceneAddress.SceneLoadByNameOrLabel("Assets/Scenes/Creation.unity")
        //                 .Forget());
        //         });
        // }
    }
}
