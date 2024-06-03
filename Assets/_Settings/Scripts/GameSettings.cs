using System;
using UnityEngine;

namespace _Settings.Scripts
{
    public static class GameSettings
    {
        private const string MusicVolumeKey = "Music_Volume";
        private const string SfxVolumeKey = "Sfx_Volume";


        public static event Action<float> OnMusicVolumeChanged; 
        public static event Action<float> OnSfxVolumeChanged; 


        public static float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
        }

        public static void SetMusicVolume(float value)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, value);
            OnMusicVolumeChanged?.Invoke(value);
        }

        public static float GetSfxVolume()
        {
            return PlayerPrefs.GetFloat(SfxVolumeKey, 1f);
        }

        public static void SetSfxVolume(float value)
        {
            PlayerPrefs.SetFloat(SfxVolumeKey, value);
            OnSfxVolumeChanged?.Invoke(value);
        }
    }
}