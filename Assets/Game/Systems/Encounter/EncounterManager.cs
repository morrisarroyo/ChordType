using System;
using Game.Core.Save;
using Game.Data;
using Game.Data._ScriptableObjectScripts;
using Game.Managers;
using Game.Systems.Input;
using Game.Systems.Score;
using Game.UI.Encounter;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Systems.Encounter
{
    // TODO: Improve finishing of game/round instance
    // --- Balatro has 3 rounds per boss battle (inclusive)
    
    public class EncounterManager : MonoBehaviour
    {
        [SerializeField] private EncounterUIController encounterUI;
        [SerializeField] private ScoreConfig scoreConfig;
        
        private GameManager _gameManager;
        private InputManager _inputManager;
        private SaveManager _saveManager;
        
        private ScoreController _scoreController;
        
        // TODO: Make private when InputController is fully refactored
        public EncounterInstance EncounterInstance;

        private void Awake()
        {
            Validate.Assigned(encounterUI, nameof(encounterUI));
            Validate.Assigned(scoreConfig, nameof(scoreConfig));
            
            _gameManager = GameManager.Instance;
            
            _inputManager = _gameManager.inputManager;
            if (_inputManager == null)
            {
                Debug.LogError($"Could not find {nameof(InputManager)} on {gameObject.name}");
                enabled = false;
                return;
            }
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDisable()
        {
            _inputManager.OnKeyUpped -= EncounterInstance.OnKeyUp;
            
            EncounterInstance.OnEncounterEnded -= OnEncounterEnd;
        }

        public void Initialize(GameManager gameManager, SaveManager saveManager)
        {
            // reference injected instead of global access
            _gameManager = gameManager;
            _saveManager = saveManager;
        }
        
        public void StartEncounter(EncounterDefinition encounterDefinition)
        {
            EncounterState encounterState = new EncounterState();
            _scoreController = new ScoreController(scoreConfig, encounterState);
            
            EncounterInstance = new EncounterInstance(encounterDefinition, encounterState, _scoreController);
            
            _inputManager.OnKeyUpped += EncounterInstance.OnKeyUp;

            EncounterInstance.OnEncounterEnded += OnEncounterEnd;
            
            encounterUI.Bind(EncounterInstance, encounterState, _inputManager, _saveManager.Data.settingsData);
            
            _gameManager.SetState(GameManager.GameState.InEncounter);
        }

        public void QuitEncounter()
        {
            OnEncounterEnd();
        }

        private void OnEncounterEnd()
        {
            _inputManager.OnKeyUpped -= EncounterInstance.OnKeyUp;
            
            // TODO: Change once SaveData has been improved with better encounter saving
            _saveManager.Data.ProcessNewHighScore("Mine", EncounterInstance.GetEncounterScore());
            _saveManager.Save();
            
            EncounterInstance.DeleteInstance();

            _gameManager.EndGame();
            Debug.Log("Encounter Ended");
        }
    }
}
