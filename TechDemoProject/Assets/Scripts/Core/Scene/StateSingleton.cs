using System.Threading;
using Cysharp.Threading.Tasks;
using Pixelplacement;
using Tech.UI.Panel;
using UnityEngine.SceneManagement;

namespace Tech.Core
{
    //TODO got to refactor and in depend free.

    public class StateSingleton : Singleton<StateSingleton>
    {
        private readonly SceneSystem _sceneSystem = new SceneSystem(CancellationToken.None);
        protected override void OnRegistration()
        {
            BaseDocument.OnLoadNext += Load;
        }

        private void Load(string scene)
        {
            //Got to make the audio fade down to zero then load to next scene.
            //Wait for the main audio source to fade out
            //Find a way to lerp audio source
            _sceneSystem.LoadSceneAsync(scene, LoadSceneMode.Single, () => { }).Forget();
        }
    }
}