using Pixelplacement;
using UnityEngine;
using UnityEngine.Video;

namespace Tech.Mono
{
    [RequireComponent(typeof(VideoPlayer), typeof(AudioSource))]
    public class DisplayVideo : DisplayObject
    {
        private VideoPlayer _player;

        public void SetVideoDisplay()
        {
            SetActive(true);
        }


        private void Awake()
        {
            _player = gameObject.GetComponent<VideoPlayer>();
            _player.loopPointReached += PlayerOnloopPointReached;
        }

        private void OnEnable()
        {
            if (!_player.isPlaying)
                _player.Play();
        }

        private void OnDestroy()
        {
            _player.loopPointReached -= PlayerOnloopPointReached;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _player.Stop();
                SetActive(false);
            }
        }

        private void PlayerOnloopPointReached(VideoPlayer source)
        {
            //TODO Set The VideoPlayer as a Material or Texture so it can be faded
            SetActive(false);
        }
    }
}