using DG.Tweening;
using Pixelplacement;
using UnityEngine;
using UnityEngine.Serialization;
using ZLogger;


//small wrapper for State from Pixelplacement State script
namespace Tech.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public sealed class SceneState : State
    {
        private AudioSource _source;

        [FormerlySerializedAs("Clip")] [SerializeField]
        private AudioClip clip;

        [SerializeField] private Ease fadeInEase;

        [SerializeField] private float fadeInTime = 1;


        [SerializeField] private Ease fadeOutEase;

        [SerializeField] private float fadeOutTime = 2;


        [Tooltip("Next Scene Addressable Path")] [SerializeField]
        private string onNextScene;

        internal Ease FadeOutEase => fadeOutEase;
        internal float FadeOutTime => fadeOutTime;
        internal string OnNextScene => onNextScene;


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