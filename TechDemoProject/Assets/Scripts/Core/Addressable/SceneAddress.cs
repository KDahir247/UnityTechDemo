using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UnityEngine.AddressableAssets;
using ZLogger;

//Script Happens Separately from ECS System 
namespace Tech.Core
{
    internal static class SceneAddress
    {
        private static readonly ILogger Logger = LogManager.GetLogger("SceneLogger");

        public static async UniTaskVoid SceneLoadByNameOrLabel(string nameOrLabel,
            [CanBeNull] IProgress<float> progressSceneAddress = null,
            CancellationToken cancellationTokenSceneAddress = default,
            [CanBeNull] Action onComplete = null
        )
        {
            if (nameOrLabel == string.Empty)
            {
                Logger.ZLogError(
                    "Name or Label Empty Exception: Label or name passed in is equal to string.empty (\"\")");
                return;
            }

            cancellationTokenSceneAddress.Register(OperationCanceled);

            try
            {
                var resourceLocations =
                    await Addressables.LoadResourceLocationsAsync(nameOrLabel)
                        .ToUniTask(progressSceneAddress,
                            cancellationToken: cancellationTokenSceneAddress);

                progressSceneAddress?.Report(1.0f);

                if (resourceLocations.Count <= 0)
                {
                    Logger.ZLogError("Couldn't find Addressable Scene with the parameter passed through");
                    return;
                }

                await Addressables.LoadSceneAsync(resourceLocations[0])
                    .ToUniTask(progressSceneAddress, PlayerLoopTiming.Update, cancellationTokenSceneAddress);

                onComplete?.Invoke();

                progressSceneAddress?.Report(1.0f);
            }
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
            }
        }


        private static void OperationCanceled()
        {
            Logger.ZLogError("Loading Scene have been canceled mid way of loading");
        }
    }
}