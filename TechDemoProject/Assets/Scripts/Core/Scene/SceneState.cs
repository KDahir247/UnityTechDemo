using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;
using ZLogger;


//small wrapper for State from Pixelplacement
namespace Tech.Core
{
    //TODO might also include an audioSource
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class SceneState : State
    {
        private AudioSource _source;

        [FormerlySerializedAs("Clip")] [SerializeField]
        private AudioClip clip;

        public Ease fadeInEase;
        public float fadeInTime = 1;

        [FormerlySerializedAs("FadeOutEase")] public Ease fadeOutEase;

        [FormerlySerializedAs("FadeOutTime")] public float fadeOutTime = 2;

        [FormerlySerializedAs("Key")] [Tooltip("The Tag Key to load load to the next Scene")]
        public string key;

        [Tooltip("Next Scene Addressable Path")]
        public string onNextScene;

        private void Start()
        {
            if (!clip)
            {
                LogManager.Logger.ZLogError($"Missing AudioClip on {gameObject}");
                return;
            }

            _source = gameObject.GetComponent<AudioSource>();

            _source.loop = true;
            _source.clip = clip;

            _source.Play();

            _source
                .DOFade(1, fadeInTime)
                .SetEase(fadeInEase)
                .Play();
        }
    }
}