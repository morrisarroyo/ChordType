using Game.Managers;
using Game.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Systems.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxAudioSource;
        [SerializeField] private AudioSource musicAudioSource;

        [SerializeField] private AudioMixer audioMixer;
        
        private int _sfxVolume = 100;
        private int _musicVolume = 100;
        private int _masterVolume = 100;
        
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Validate.Assigned(musicAudioSource, nameof(musicAudioSource));
            Validate.Assigned(sfxAudioSource, nameof(sfxAudioSource));
            Validate.Assigned(audioMixer, nameof(audioMixer));
        }
        
        public void Start()
        {
            OnMasterVolumeChange(GameManager.Instance.SaveManager.Data.settingsData.MasterVolume);
            OnMusicVolumeChange(GameManager.Instance.SaveManager.Data.settingsData.MusicVolume);
            OnSfxVolumeChange(GameManager.Instance.SaveManager.Data.settingsData.SfxVolume);
            
            GameManager.Instance.SaveManager.Data.settingsData.OnMasterVolumeChanged += OnMasterVolumeChange;
            GameManager.Instance.SaveManager.Data.settingsData.OnMusicVolumeChanged += OnMusicVolumeChange;
            GameManager.Instance.SaveManager.Data.settingsData.OnSfxVolumeChanged += OnSfxVolumeChange;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.SaveManager.Data.settingsData.OnMasterVolumeChanged -= OnMasterVolumeChange;
            GameManager.Instance.SaveManager.Data.settingsData.OnMusicVolumeChanged -= OnMusicVolumeChange;
            GameManager.Instance.SaveManager.Data.settingsData.OnSfxVolumeChanged -= OnSfxVolumeChange;
        }
        
        public void PlaySoundOneShot(AudioClip clip)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        
        public void PlaySoundOverride(AudioClip clip)
        {
            sfxAudioSource.Stop();
            sfxAudioSource.clip = clip;
            sfxAudioSource.Play();
        }

        public void PlayMusic(AudioClip clip)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }
        
        private void OnMasterVolumeChange(int newMasterVolume)
        {
            _masterVolume = newMasterVolume;
            audioMixer.SetFloat("MasterVolume", AudioUtility.PercentToDecibel(_masterVolume));
        }
        
        private void OnMusicVolumeChange(int newMusicVolume)
        {
            _musicVolume = newMusicVolume;
            audioMixer.SetFloat("BGMVolume", AudioUtility.PercentToDecibel(_musicVolume));
        }

        private void OnSfxVolumeChange(int newSfxVolume)
        {
            _sfxVolume = newSfxVolume;
            audioMixer.SetFloat("SFXVolume", AudioUtility.PercentToDecibel(_sfxVolume));
        }
    }
}