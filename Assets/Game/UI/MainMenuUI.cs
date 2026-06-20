using System;
using System.Collections;
using Game.Audio;
using Game.Core.GameManager;
using Game.Systems.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        
        [SerializeField] private AudioClip playSfx;
        [SerializeField] private AudioClip highscoresSfx;
        [SerializeField] private AudioClip settingsSfx;
        [SerializeField] private AudioClip quitSfx;
        
        // TODO: Move to a separate UI class for MainMenu Effects and stuff
        [SerializeField] private ClickMarkerPool clickMarkerPool;
        
        private GameManager _gameManager;
        private AudioManager _audioManager;
        private InputManager _inputManager;
        
        private VisualElement _root;
        
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
            
            _inputManager = _gameManager.inputManager;
            
            Validate.Assigned(playSfx, nameof(playSfx));
            Validate.Assigned(highscoresSfx, nameof(highscoresSfx));
            Validate.Assigned(settingsSfx, nameof(settingsSfx));
            Validate.Assigned(quitSfx, nameof(quitSfx));
            
            Validate.Assigned(clickMarkerPool, nameof(clickMarkerPool));
            
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
            _root.RegisterCallback<PointerDownEvent>(_inputManager.OnUIPointerDown);
            _root.RegisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _inputManager.OnPointerDowned += clickMarkerPool.OnPointerDown;
        }


        private void OnDisable()
        {
            _root.UnregisterCallback<PointerDownEvent>(_inputManager.OnUIPointerDown);
            _root.UnregisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _inputManager.OnPointerDowned -= clickMarkerPool.OnPointerDown;
        }

        private void OnDestroy()
        {
        }

        private void OnPlay()
        {
            Debug.Log("Play");
            _gameManager.StartGame();
            _audioManager.PlaySoundOverride(playSfx);
            
            PlaceClickMarkerOnButton();
        }


        private void OnHighScores()
        {
            Debug.Log("High Scores");
            _audioManager.PlaySoundOverride(highscoresSfx);
            
            PlaceClickMarkerOnButton();
        }

        private void OnSettings()
        {
            Debug.Log("Settings");
            _audioManager.PlaySoundOverride(settingsSfx);
            
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