using DG.Tweening;
using UnityEngine;

namespace LFramework
{
    public class AudioScript : MonoCached
    {
        AudioConfig _config;

        AudioSource _audioSource;

        Tween _tween;

        public AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                    _audioSource = gameObjectCached.GetComponent<AudioSource>();

                return _audioSource;
            }
        }

        #region MonoBehaviour

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        private void Start()
        {
            transformCached.SetParent(AudioManager.Instance.TransformCached);

            AudioManager.volumeSound.eventValueChanged += VolumeSound_EventValueChanged;
            AudioManager.volumeMusic.eventValueChanged += VolumeMusic_EventValueChanged;
        }

        #endregion

        #region Function -> Public

        public void Play(AudioConfig config, bool loop = false)
        {
            Construct(config, loop);

            _tween?.Kill();

            if (!loop)
                _tween = DOVirtual.DelayedCall(config.clip.length, Stop, false);
        }

        public void Stop()
        {
            if (AudioManager.IsDestroyed)
                return;

            _tween?.Kill();

            audioSource.Stop();

            AudioManager.ReturnPool(this);
        }

        #endregion

        #region Function -> Private

        private float GetVolume()
        {
            return _config.volumeScale * (_config.type == AudioType.Music ? AudioManager.volumeMusic.value : AudioManager.volumeSound.value);
        }

        private void VolumeSound_EventValueChanged(float volume)
        {
            UpdateVolume();
        }

        private void VolumeMusic_EventValueChanged(float volume)
        {
            UpdateVolume();
        }

        private void Construct(AudioConfig config, bool loop = false)
        {
            _config = config;

            audioSource.clip = config.clip;
            audioSource.loop = loop;
            audioSource.minDistance = config.distance.x;
            audioSource.maxDistance = config.distance.y;
            audioSource.spatialBlend = config.is3D ? 1f : 0f;

            UpdateVolume();

            audioSource.Play();
        }

        private void UpdateVolume()
        {
            if (_config == null)
                return;

            float volumeFinal = GetVolume();

            audioSource.mute = volumeFinal <= 0;
            audioSource.volume = volumeFinal;
        }

        #endregion
    }
}