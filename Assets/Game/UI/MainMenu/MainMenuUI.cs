using System;
using System.Collections;
using System.Collections.Generic;
using Game.Core.Save;
using Game.Managers;
using Game.Systems.Audio;
using Game.Systems.Input;
using Game.Systems.Score;
using Game.UI.Encounter;
using Game.UI.HighScore;
using Game.UI.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        
        // Audio
        [SerializeField] private AudioClip playSfx;
        [SerializeField] private AudioClip highscoresSfx;
        [SerializeField] private AudioClip settingsSfx;
        [SerializeField] private AudioClip quitSfx;
        
        // Config
        // TODO: Move to a separate Config File/ Scriptable Object?
        [SerializeField] private int highScoreCountToDisplay = 10;
        
        // High Scores
        [SerializeField] private VisualTreeAsset highScoreEntryTemplate;
        private HighScoresOverlay _highScoresOverlay;
        
        // Prompt Count Selector Popup
        [SerializeField] private VisualTreeAsset promptCountEntryTemplate;
        private PromptCounterSelectorPopup _promptCounterSelectorPopup;
        
        // Managers
        private GameManager _gameManager;
        private AudioManager _audioManager;
        private InputManager _inputManager;
        private SaveManager _saveManager;
        
        // Settings
        private SettingsOverlay _settingsOverlay;

        // UI
        [SerializeField] private UIDocument uiDocument;
        private VisualElement _root;
        
        // VFX
        // TODO: Move to a separate UI class for MainMenu Effects and stuff
        [SerializeField] private ClickMarkerPool clickMarkerPool;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
            if (_gameManager == null)
            {
                Debug.LogError("GameManager not yet initialized");
            }
            
            _audioManager = AudioManager.Instance;
            if (_audioManager == null)
            {
                Debug.LogError("AudioManager not yet initialized");
            }
            
            Validate.Assigned(playSfx, nameof(playSfx));
            Validate.Assigned(highscoresSfx, nameof(highscoresSfx));
            Validate.Assigned(settingsSfx, nameof(settingsSfx));
            Validate.Assigned(quitSfx, nameof(quitSfx));
            
            Validate.Assigned(clickMarkerPool, nameof(clickMarkerPool));
            
            Validate.Assigned(highScoreEntryTemplate, nameof(highScoreEntryTemplate));
            Validate.Assigned(promptCountEntryTemplate, nameof(promptCountEntryTemplate));
            
            _root = uiDocument.rootVisualElement;
            if (_root == null)
            {
                Debug.LogError("UI root not yet initialized");
                enabled = false;
                return;
            }

            _root.Q<Button>("play-button").clicked += OnPlay;
            _root.Q<Button>("highscores-button").clicked += OnHighScores;
            _root.Q<Button>("settings-button").clicked += OnSettings;
            _root.Q<Button>("quit-button").clicked += OnQuit;
        }

        private void OnEnable()
        {
            _inputManager = _gameManager.inputManager;
            
            _root.RegisterCallback<PointerDownEvent>(_inputManager.OnUIPointerDown);
            _root.RegisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _inputManager.OnPointerDowned += clickMarkerPool.OnPointerDown;
        }

        private void Start()
        {
            _saveManager = _gameManager.SaveManager;
            
            _highScoresOverlay = new HighScoresOverlay(_root, highScoreEntryTemplate, highScoreCountToDisplay, _saveManager.Data.highScores.AsReadOnly());
            _highScoresOverlay.HideHighScoresOverlay();
            
            _settingsOverlay = new SettingsOverlay(_root, _saveManager.Data.settingsData);
            _settingsOverlay.HideSettingsOverlay();
            _settingsOverlay.OnSettingsApplied += _saveManager.Save;

            _promptCounterSelectorPopup = new PromptCounterSelectorPopup(_root, promptCountEntryTemplate);
            _promptCounterSelectorPopup.HidePromptCountSelectorPopup();
            _promptCounterSelectorPopup.OnPromptCountButtonClicked += _gameManager.StartGame;
        }


        private void OnDisable()
        {
            _root.UnregisterCallback<PointerDownEvent>(_inputManager.OnUIPointerDown);
            _root.UnregisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _inputManager.OnPointerDowned -= clickMarkerPool.OnPointerDown;
            
            _highScoresOverlay.UnbindButtons();
            _settingsOverlay.Unbind();
            
            _settingsOverlay.OnSettingsApplied -= _saveManager.Save;
        }

        private void OnDestroy()
        {
        }

        private void OnPlay()
        {
            Debug.Log("Play");
            // _gameManager.StartGame();
            _audioManager.PlaySoundOverride(playSfx);
            
            _promptCounterSelectorPopup.ShowPromptCountSelectorPopup();
            
            PlaceClickMarkerOnButton();
        }


        private void OnHighScores()
        {
            Debug.Log("High Scores");
            _audioManager.PlaySoundOverride(highscoresSfx);

            _highScoresOverlay.RepopulateHighScores(_saveManager.Data.highScores.AsReadOnly());
            _highScoresOverlay.ShowHighScoresOverlay();
            
            PlaceClickMarkerOnButton();
        }

        private void OnSettings()
        {
            Debug.Log("Settings");
            _audioManager.PlaySoundOverride(settingsSfx);
            
            _settingsOverlay.UpdateSettingsFromData(_saveManager.Data.settingsData);
            _settingsOverlay.ShowSettingsOverlay();
            
            PlaceClickMarkerOnButton();
        }

        private void OnQuit()
        {
            _audioManager.PlaySoundOverride(quitSfx);

            PlaceClickMarkerOnButton();
            
            StartCoroutine(QuitAfterDelay(1.5f));
        }
        
        private IEnumerator QuitAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        private void PlaceClickMarkerOnButton()
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            screenPos.y = Screen.height - screenPos.y;
            Vector2 panelPos = RuntimePanelUtils.ScreenToPanel(
                _root.panel,
                screenPos);
            clickMarkerPool.ShowMarker(panelPos);
        }
    }
}