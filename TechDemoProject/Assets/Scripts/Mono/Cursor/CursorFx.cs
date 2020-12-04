using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Pixelplacement;
using Tech.Core;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.Serialization;

//TODO cursor eat alot of performance initially it goes down to 5 fps from 500 fps 
namespace Tech.Mono
{
    public class CursorFx : Singleton<CursorFx>
    {
        private readonly ReactiveCollection<GameObject> _reactiveCollection = new ReactiveCollection<GameObject>();
        private Camera _camera;

        [FormerlySerializedAs("resourceLocations")] [FormerlySerializedAs("_resourceLocations")] [SerializeField]
        private List<AssetReference> assetRef = new List<AssetReference>(5);

        [FormerlySerializedAs("clickVfx")] [SerializeField]
        private float clickVfxDepth = 15f;

        // Start is called before the first frame update
        private void Awake()
        {
            MessageBroker.Default
                .Receive<Camera>()
                .Subscribe(cam => _camera = cam)
                .AddTo(this);

            _reactiveCollection
                .ObserveAdd()
                .Subscribe(clickVfx =>
                {
                    var timer = clickVfx
                        .Value
                        .GetComponent<ParticleSystem>()
                        .main
                        .duration;

                    AssetAddress.Release(clickVfx.Value, timer);
                }).AddTo(this);
        }

        private async UniTaskVoid Update()
        {
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Vector3 screenWorldTouch = Input.GetTouch(0).position;
            screenWorldTouch.z = clickVfxDepth;
            Vector3 worldPointTouch = _camera.ScreenToWorldPoint(screenWorldTouch);
            //
            //Create struct for loading addressable
            AssetAddress.CreateAssetList<GameObject>(assetRef[Random.Range(0, assetRef.Count - 1)],
                 _reactiveCollection, new InstantiationParameters(worldPointTouch, Quaternion.identity, null)).Forget();
        }
#else
            if (Input.GetMouseButtonDown(0) && _camera)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = clickVfxDepth;
                var worldMousePos = _camera.ScreenToWorldPoint(mousePos);

                await AssetAddress.CreateAssetList(assetRef[Random.Range(0, assetRef.Count - 1)],
                        _reactiveCollection, new InstantiationParameters(worldMousePos, Quaternion.identity, null));
            }
#endif
        }
    }
}