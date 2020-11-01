using UnityEngine;
using UnityEngine.UI;


//small wrapper for State from Pixelplacement
namespace Tech.scenestate
{
    //TODO might also include an audioSource
    public class SceneState : Pixelplacement.State
    {
        [Tooltip("Next Scene Addressable Path")]
        public string onNextScene;
        
    }
}