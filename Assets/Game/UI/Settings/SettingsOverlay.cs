using System;
using Game.Data.Save;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Settings
{
    public class SettingsOverlay
    { 
        private readonly VisualElement _settingsOverlayElement;
        
        private readonly SliderInt _masterVolumeSlider;
        private readonly SliderInt _musicVolumeSlider;
        private readonly SliderInt _sfxVolumeSlider;
        private readonly SliderInt _textSizeSlider;
        
        private readonly Label _masterVolumeLabel;
        private readonly Label _musicVolumeLabel;
        private readonly Label _sfxVolumeLabel;
        private readonly Label _textSizeLabel;
        
        private readonly Toggle _fullscreenToggle;
        
        private readonly Button _settingsApplyButton;
        private readonly Button _settingsCloseButton;

        private readonly SettingsData _settingsData;
        
        public Action OnSettingsApplied;

        public SettingsOverlay(VisualElement root, SettingsData settingsData)
        {
            if (root == null)
            {
                Debug.LogError($"{nameof(root)} should not be null.");
            }

            if (settingsData == null)
            {
                Debug.LogError($"{nameof(settingsData)} should not be null.");
            }
            
            _settingsData = settingsData;
            
            _settingsOverlayElement = root.Q<VisualElement>("settings-overlay");
            _settingsApplyButton = root.Q<Button>("settings-apply-button");
            _settingsCloseButton = root.Q<Button>("settings-close-button");
            
            _masterVolumeSlider =  root.Q<SliderInt>("master-volume-slider");
            _musicVolumeSlider =  root.Q<SliderInt>("music-volume-slider");
            _sfxVolumeSlider =  root.Q<SliderInt>("sfx-volume-slider");
            _textSizeSlider =  root.Q<SliderInt>("text-size-slider");
            
            _masterVolumeLabel = root.Q<Label>("master-volume-label");
            _musicVolumeLabel = root.Q<Label>("music-volume-label");
            _sfxVolumeLabel = root.Q<Label>("sfx-volume-label");
            _textSizeLabel = root.Q<Label>("text-size-label");
            
            _fullscreenToggle =  root.Q<Toggle>("fullscreen-toggle");

            UpdateSettingsFromData(_settingsData);

            Bind();
        }

        private void Bind()
        {
            _masterVolumeSlider.RegisterValueChangedCallback(OnMasterVolumeChange);
            _musicVolumeSlider.RegisterValueChangedCallback(OnMusicVolumeChange);
            _sfxVolumeSlider.RegisterValueChangedCallback(OnSfxVolumeChange);
            _textSizeSlider.RegisterValueChangedCallback(OnTextSizeChange);
            
            _fullscreenToggle.RegisterValueChangedCallback(OnFullscreenToggleChange);
            
            _settingsApplyButton.clicked += OnSettingsApply;
            _settingsCloseButton.clicked += HideSettingsOverlay;
        }

        public void Unbind()
        {
            _masterVolumeSlider.UnregisterValueChangedCallback(OnMasterVolumeChange);
            _musicVolumeSlider.UnregisterValueChangedCallback(OnMusicVolumeChange);
            _sfxVolumeSlider.UnregisterValueChangedCallback(OnSfxVolumeChange);
            _textSizeSlider.UnregisterValueChangedCallback(OnTextSizeChange);
            
            _fullscreenToggle.UnregisterValueChangedCallback(OnFullscreenToggleChange);

            _settingsApplyButton.clicked -= OnSettingsApply;
            _settingsCloseButton.clicked -= HideSettingsOverlay;
        }
        
        public void ShowSettingsOverlay()
        {
            _settingsOverlayElement.style.display = DisplayStyle.Flex;
        }
        
        public void HideSettingsOverlay()
        {
            _settingsOverlayElement.style.display = DisplayStyle.None;
        }

        public void UpdateSettingsFromData(SettingsData settingsData)
        {
            _masterVolumeSlider.value = settingsData.MasterVolume;
            _musicVolumeSlider.value = settingsData.MusicVolume;
            _sfxVolumeSlider.value = settingsData.SfxVolume;
            _textSizeSlider.value = settingsData.TextSizePercent;
            
            _masterVolumeLabel.text = $"{settingsData.MasterVolume.ToString()}%";
            _musicVolumeLabel.text = $"{settingsData.MusicVolume.ToString()}%";
            _sfxVolumeLabel.text = $"{settingsData.SfxVolume.ToString()}%";
            _textSizeLabel.text = $"{settingsData.TextSizePercent.ToString()}%";
            
            _fullscreenToggle.SetValueWithoutNotify(settingsData.IsFullscreen);
        }

        private void OnMasterVolumeChange(ChangeEvent<int> changeEvent)
        {
            _settingsData.MasterVolume = changeEvent.newValue;
            _masterVolumeLabel.text = $"{_settingsData.MasterVolume}%";
        }

        private void OnMusicVolumeChange(ChangeEvent<int> changeEvent)
        {
            _settingsData.MusicVolume = changeEvent.newValue;
            _musicVolumeLabel.text = $"{_settingsData.MusicVolume}%";
        }
        
        private void OnSfxVolumeChange(ChangeEvent<int> changeEvent)
        {
            _settingsData.SfxVolume = changeEvent.newValue;
            _sfxVolumeLabel.text = $"{_settingsData.SfxVolume}%";
        }
        
        private void OnTextSizeChange(ChangeEvent<int> changeEvent)
        {
            _settingsData.TextSizePercent = changeEvent.newValue;
            _textSizeLabel.text = $"{_settingsData.TextSizePercent}%";
        }
        
        private void OnFullscreenToggleChange(ChangeEvent<bool> changeEvent)
        {
            _settingsData.IsFullscreen = changeEvent.newValue;
            Debug.Log($"Fullscreen toggle: {_settingsData.IsFullscreen}");
        }

        private void OnSettingsApply()
        {
            HideSettingsOverlay();
            OnSettingsApplied?.Invoke();
        }
    }
}
