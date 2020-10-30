﻿using System.Collections.Generic;
using DG.Tweening;
using Tech.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CursorFx : MonoBehaviour
{
    private Camera _camera;
    private readonly ReactiveCollection<GameObject> _reactiveCollection = new ReactiveCollection<GameObject>();
    
    [FormerlySerializedAs("resourceLocations")]
    [FormerlySerializedAs("_resourceLocations")]
    [SerializeField] List<AssetReference> assetRef = new List<AssetReference>(5);
    
    [FormerlySerializedAs("clickVfx")] 
    [SerializeField] private float clickVfxDepth = 15f; 
    
    // Start is called before the first frame update
    void Awake()
    {
        DOTween.Init().SetCapacity(200, 25);
        MessageBroker.Default
            .Receive<Camera>()
            .Subscribe(cam => _camera = cam).AddTo(this);

        _reactiveCollection
            .ObserveAdd()
            .Subscribe(clickVfx =>
            {
                float timer = clickVfx.Value.GetComponent<ParticleSystem>().main.duration / 2.0f;
                AssetAddress.Release(clickVfx.Value, timer);
                
            }).AddTo(this);
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Vector3 screenWorldTouch = Input.GetTouch(0).position;
            screenWorldTouch.z = clickVfxDepth;
            Vector3 worldPointTouch =  _camera.ScreenToWorldPoint(screenWorldTouch);
            //
            //Create struct for loading addressable
            AssetAddress.CreateAssetList<GameObject>(assetRef[Random.Range(0, assetRef.Count - 1)],
                 _reactiveCollection, new InstantiationParameters(worldPointTouch, Quaternion.identity, null)).Forget();
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = clickVfxDepth;
            Vector3 worldMousePos = _camera.ScreenToWorldPoint(mousePos);

            AssetAddress.CreateAssetList<GameObject>(assetRef[Random.Range(0, assetRef.Count - 1)],
                _reactiveCollection, new InstantiationParameters(worldMousePos, Quaternion.identity, null)).Forget();
        }
#endif
    }
}