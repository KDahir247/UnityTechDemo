#define CameraEvent
//#undef CameraEvent

using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Object = UnityEngine.Object;

//Happens Separately from Unity ECS System

//Small Helper class to retrieve the camera and broadcast it to the listener.
//Retrieving the camera from addressable also works, but requires instantiating the asset while CameraRetrieval retrieve the camera 
//and doesn't instantiate it. Unless it is required.
#if CameraEvent
//TODO dont need remove this and change mouse FX script that depends on this
namespace Tech.Core
{
    public static class CameraRetrieval
    {
        private static readonly ILogger Logger = LogManager.GetLogger("CameraLogger");
        private static readonly List<Camera> Cameras = new List<Camera>(5);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            if (Camera.allCamerasCount > 1)
            {
                Cameras.Clear();

                try
                {
                    foreach (var camera in Camera.allCameras) Object.Destroy(camera.gameObject);

                    AssetAddress.LoadByNameOrLabel("Camera", Cameras,
                        new InstantiationParameters(Vector3.zero, Quaternion.identity, null)).Forget();
                    Cameras[0].gameObject.tag = "MainCamera";
                }
                catch (Exception e)
                {
                    Logger.ZLogError(e.Message);
                }
            }

            if (Camera.main == null)
                Logger.ZLogError(
                    "Failed to find Camera in the Loaded Scene. \n Either the camera isn't tagged MainCamera or there isn't a Camera in the loaded scene.");
            else
                MessageBroker
                    .Default
                    .Publish(Camera.main);
        }
    }
}
#endif