using _Settings.Scripts;
using UnityEngine;

namespace _SoundSystem.Scripts
{
    public class SoundPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private SoundType _type;
        private float _volume;
        private float _volumeScale = 1f;
        
        
        private void Awake()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();

            GameSettings.OnMusicVolumeChanged += OnMusicVolumeChanged;
            GameSettings.OnSfxVolumeChanged += OnSfxVolumeChanged;
        }

        private void OnDestroy()
        {
            GameSettings.OnMusicVolumeChanged -= OnMusicVolumeChanged;
            GameSettings.OnSfxVolumeChanged -= OnSfxVolumeChanged;
        }

        private void Update()
        {
            if (_audioSource.isPlaying) return;
            
            Destroy(gameObject);
        }


        private void OnMusicVolumeChanged(float value)
        {
            if (_type != SoundType.Music) return;

            _volumeScale = value;
            _audioSource.volume = _volume * _volumeScale;
        }

        private void OnSfxVolumeChanged(float value)
        {
            if (_type != SoundType.Sfx) return;

            _volumeScale = value;
            _audioSource.volume = _volume * _volumeScale;
        }
        

        public void Play()
        {
            _audioSource.Play();
        }
        
        public void SetClip(AudioClip clip)
        {
            _audioSource.clip = clip;
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
            _audioSource.volume = volume * _volumeScale;
        }

        public void SetType(SoundType type)
        {
            _type = type;
            _volumeScale = type switch
            {
                SoundType.Music => GameSettings.GetMusicVolume(),
                SoundType.Sfx => GameSettings.GetSfxVolume()
            };
        }


        public void SetLoop(bool isLoop)
        {
            _audioSource.loop = isLoop;
        }
    }
}