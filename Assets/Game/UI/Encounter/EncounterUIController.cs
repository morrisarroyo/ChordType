using System;
using Game.Data.Save;
using Game.Systems.Encounter;
using Game.Systems.Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    [RequireComponent(typeof(UIDocument))]
    public class EncounterUIController : MonoBehaviour
    {
        private EncounterInstance _encounterInstance;
        private EncounterState _encounterState;
        private SettingsData _settingsData;
        
        private InputManager _inputManager;

        private VisualElement _root;
        private Button _restartButton;
        
        // UI Views
        private PromptUIView _promptUIView;
        private ScoreUIView _scoreUIView;
        private StatsUIView _statsUIView;
        private TypeWindowUIView _typeWindowUIView;

        private EncounterStatsPopup _encounterStatsPopup;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            // Add Class for disabling USS Transition Animations in Editor
            _root.AddToClassList("runtime");
        }

        void OnEnable()
        {
            _promptUIView  = new PromptUIView(_root);
            _scoreUIView  = new ScoreUIView(_root);
            _statsUIView  = new StatsUIView(_root);
            _typeWindowUIView =  new TypeWindowUIView(_root);
            
            
            _typeWindowUIView.SetCursorVisibility(false);
            
            _restartButton = _root.Q<Button>("restart-button");
            _restartButton.clickable.clicked += OnRestartButtonClicked;
        }

        void Start()
        {
 
            
        }
        
        private void OnDisable()
        {
            _root.UnregisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _restartButton.clickable.clicked -= OnRestartButtonClicked;
            
            _settingsData.OnTextSizePercentChanged -= _promptUIView.UpdateTextSize;
            _settingsData.OnTextSizePercentChanged -= _typeWindowUIView.UpdateTextSize;

            _encounterInstance.OnCharacterTyped -=  OnCharacterType;
            _encounterInstance.OnEncounterPromptRefreshed -= ResetEncounterUI;
            _encounterInstance.OnPromptFinished -= _promptUIView.UpdateHistoryPromptWindow;
            _encounterInstance.OnFinalPromptFinished -= OnFinalPromptFinish;
            
            _encounterState.OnTypedTextChanged -= _typeWindowUIView.SetTypeWindowText;
            _encounterState.OnCursorVisibilityChanged -= _typeWindowUIView.SetCursorVisibility;
            
            _encounterState.OnScoreToAddChanged -= _scoreUIView.UpdateAddScore;
            _encounterState.OnScoreToRemoveChanged -= _scoreUIView.UpdateRemoveScore;
            _encounterState.OnPromptScoreChanged -= _scoreUIView.UpdatePromptScore;
            _encounterState.OnTotalScoreChanged -= _scoreUIView.UpdateTotalScore;
            
            _encounterState.OnPromptTextChanged -= _promptUIView.UpdatePromptWindowText;
            
            _encounterState.OnCharactersPerMinuteChanged -= _statsUIView.UpdateCharactersPerMinute;
            _encounterState.OnWordsPerMinuteChanged -= _statsUIView.UpdateWordsPerMinute;
            _encounterState.OnMistakesInCurrentPromptChanged -= _statsUIView.UpdateMistakeCount;
            
            _encounterStatsPopup.OnEncounterStatsPopupCloseClicked -= FinishEncounter;
            _encounterStatsPopup.UnbindButtons();
        }
        
        public void Bind(EncounterInstance encounterInstance, EncounterState encounterState, InputManager inputManager, SettingsData settingsData)
        {
            // Assign
            _encounterInstance = encounterInstance;
            _encounterState = encounterState;
            _settingsData = settingsData;

            _encounterStatsPopup = new EncounterStatsPopup(_root, _encounterState);
            _encounterStatsPopup.HideEncounterStatsPopup();
            
            // TODO: Figure out if this is the right place for this
            _inputManager = inputManager;
            _root.RegisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            // Initialize
            _promptUIView.Initialize(_encounterInstance.EncounterDefinition.baseFontSize, _encounterInstance.EncounterDefinition.characterWidthRatio);
            _typeWindowUIView.Initialize(_encounterInstance.EncounterDefinition.baseFontSize, _encounterInstance.EncounterDefinition.characterWidthRatio);
            
            // Update
            UpdateEncounterFromSettings(_settingsData);
            _promptUIView.UpdatePromptWindowText(_encounterState.CurrentPrompt);
            _typeWindowUIView.ChangeFocusToTypeWindow(true);

            // Register
            _settingsData.OnTextSizePercentChanged += _promptUIView.UpdateTextSize;
            _settingsData.OnTextSizePercentChanged += _typeWindowUIView.UpdateTextSize;
            
            _encounterInstance.OnCharacterTyped += OnCharacterType;
            _encounterInstance.OnEncounterPromptRefreshed += ResetEncounterUI;
            _encounterInstance.OnPromptFinished += _promptUIView.UpdateHistoryPromptWindow;
            _encounterInstance.OnFinalPromptFinished += OnFinalPromptFinish;
            
            _encounterState.OnTypedTextChanged += _typeWindowUIView.SetTypeWindowText;
            _encounterState.OnCursorVisibilityChanged += _typeWindowUIView.SetCursorVisibility;
            
            _encounterState.OnScoreToAddChanged += _scoreUIView.UpdateAddScore;
            _encounterState.OnScoreToRemoveChanged += _scoreUIView.UpdateRemoveScore;
            _encounterState.OnPromptScoreChanged += _scoreUIView.UpdatePromptScore;
            _encounterState.OnTotalScoreChanged += _scoreUIView.UpdateTotalScore;
            
            _encounterState.OnPromptTextChanged += _promptUIView.UpdatePromptWindowText;
            
            _encounterState.OnCharactersPerMinuteChanged += _statsUIView.UpdateCharactersPerMinute;
            _encounterState.OnWordsPerMinuteChanged += _statsUIView.UpdateWordsPerMinute;
            _encounterState.OnMistakesInCurrentPromptChanged += _statsUIView.UpdateMistakeCount;
            
            _encounterStatsPopup.OnEncounterStatsPopupCloseClicked += FinishEncounter;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
           _typeWindowUIView.ChangeFocusToTypeWindow(hasFocus);
        }

        private void OnRestartButtonClicked()
        {
            Debug.Log("Restart Button clicked");

            _encounterInstance.RestartEncounter();
            
            // ClearTypeWindowLabel(); - Create InputTypeView
            _typeWindowUIView.ClearTypeWindow();
            
            _promptUIView.UpdatePromptWindowText(_encounterState.CurrentPrompt);
            _encounterState.SetCursorVisibility(false);
        }
        
        private void OnCharacterType(char character, Color characterColour)
        {
            if (character == '\n')
            {
                ResetEncounterUI();
                return;
            }
            
            if (!_encounterState.IsCursorVisible)
            {
                _encounterState.SetCursorVisibility(true);
            }

            _typeWindowUIView.UpdateCursorPosition(_encounterState.TypedText.Length);
            _promptUIView.ColourCharacterInPrompt(_encounterState.TypedText.Length - 1, characterColour);
        }

        private void ResetEncounterUI()
        {
            _encounterState.SetCursorVisibility(false);
            
            _typeWindowUIView.ClearTypeWindow();
            _promptUIView.ResetCurrentPrompt();
        }
        
        private void OnFinalPromptFinish()
        {
            _encounterStatsPopup.UpdateStatsFromData(_encounterState);
            _encounterStatsPopup.ShowEncounterStatsPopup();
        }
        
        private void FinishEncounter()
        {
            _promptUIView.UpdateHistoryPromptWindow();
            _encounterState.SetCursorVisibility(false);
            
            _encounterStatsPopup.HideEncounterStatsPopup();
            
            _encounterInstance.FinishEncounter();
        }

        private void UpdateEncounterFromSettings(SettingsData settingsData)
        {
            _promptUIView.UpdateTextSize(settingsData.TextSizePercent);
            _typeWindowUIView.UpdateTextSize(settingsData.TextSizePercent);
        }
    }
}
