using System;
using DG.Tweening;
using Tech.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZLogger;


//small wrapper for State from Pixelplacement
namespace Tech.scenestate
{
    //TODO might also include an audioSource
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class SceneState : Pixelplacement.State
    {
        [Tooltip("Next Scene Addressable Path")]
        public string onNextScene;

        [FormerlySerializedAs("Key")] 
        [Tooltip("The Tag Key to load load to the next Scene")]
        public string key;
        
        [FormerlySerializedAs("FadeOutEase")]
        public Ease fadeOutEase;
        [FormerlySerializedAs("FadeOutTime")]
        public float fadeOutTime = 2;
        
        public Ease fadeInEase;
        public float fadeInTime = 1;
        
        [FormerlySerializedAs("Clip")]
        [SerializeField]
        private AudioClip clip;

        private AudioSource _source;
        private void Start()
        {
            if (!clip)
            {
                LogManager.Logger.ZLogError($"Missing AudioClip on {this.gameObject}");
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