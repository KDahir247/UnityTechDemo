using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Tech.Core;
using Tech.Utility;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using ZLogger;

public class AudioFade : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         _audioSource.DOFade(0, 1).Play().onComplete += stuff;
    //     }  
    // }

    async void stuff()
    {
        // await await SceneAddress
        //     .SceneLoadByNameOrLabel("Assets/Scenes/Sample.unity",GlobalSetting<float>.ScheduledNotifier);
    }
}
