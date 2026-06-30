using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data.Save
{
    [Serializable]
    public class SettingsData
    {
        // Audio
        [SerializeField] private int masterVolume = 100;
        public int MasterVolume
        {
            get => masterVolume;
            set
            {
                masterVolume = value;
                OnMasterVolumeChanged?.Invoke(masterVolume);
            }
        }
        
        [SerializeField] private int musicVolume = 100;
        public int MusicVolume
        {
            get => musicVolume;
            set
            {
                musicVolume = value;
                OnMusicVolumeChanged?.Invoke(musicVolume);
            }
        }
        
        [SerializeField] private int sfxVolume = 100;
        public int SfxVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = value;
                OnSfxVolumeChanged?.Invoke(sfxVolume);
            }
        }

        // Graphics
        [SerializeField] private bool isFullscreen = false;
        public bool IsFullscreen
        {
            get =>  isFullscreen;
            set
            {
                isFullscreen = value;
                OnFullscreenChanged?.Invoke(isFullscreen);
            }
        }
        
        // public int resolutionIndex = 0;
        // public int qualityLevel = 2;

        // Gameplay
        // public bool showCorrectKeyPressNumbers = true;
        // public bool screenShake = true;

        // Controls
        // public float mouseSensitivity = 1f;
        
        // Game
        [SerializeField] private int textSizePercent = 100;
        public int TextSizePercent
        {
            get => textSizePercent;
            set
            {
                textSizePercent = value;
                OnTextSizePercentChanged?.Invoke(textSizePercent);
            }
        }
        
        [NonSerialized] public Action<int> OnMasterVolumeChanged;
        [NonSerialized] public Action<int> OnMusicVolumeChanged;
        [NonSerialized] public Action<int> OnSfxVolumeChanged;
        
        [NonSerialized] public Action<bool> OnFullscreenChanged;
        
        [NonSerialized] public Action<int> OnTextSizePercentChanged;
    }
}