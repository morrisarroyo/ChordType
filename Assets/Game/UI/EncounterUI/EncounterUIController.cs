using Game.Systems.Encounter;
using Game.Systems.Input;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.EncounterUI
{
    [RequireComponent(typeof(UIDocument))]
    public class EncounterUIController : MonoBehaviour
    {
        private EncounterInstance _encounterInstance;
        private EncounterState _encounterState;
        
        private InputManager _inputManager;

        private VisualElement _root;
        private Button _restartButton;
        
        // UI Views
        private PromptUIView _promptUIView;
        private ScoreUIView _scoreUIView;
        private StatsUIView _statsUIView;
        private TypeWindowUIView _typeWindowUIView;
        
        
        void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _promptUIView  = new PromptUIView(_root);
            _scoreUIView  = new ScoreUIView(_root);
            _statsUIView  = new StatsUIView(_root);
            _typeWindowUIView =  new TypeWindowUIView(_root);
            
            // Add Class for disabling USS Transition Animations in Editor
            _root.AddToClassList("runtime");
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
            
            _encounterInstance.OnCharacterTyped -=  OnCharacterType;
            _encounterInstance.OnEncounterEnded -= OnEncounterEnd;
            _encounterInstance.OnEncounterPromptRefreshed -= ResetEncounterUI;
            _encounterInstance.OnPromptFinished -= _promptUIView.UpdateHistoryPromptWindow;
            
            _encounterState.OnTypedTextChanged -= _typeWindowUIView.SetTypeWindowText;
            _encounterState.OnCursorVisibilityChanged -= _typeWindowUIView.SetCursorVisibility;
            
            _encounterState.OnScoreToAddChanged -= _scoreUIView.UpdateAddScore;
            _encounterState.OnScoreToRemoveChanged -= _scoreUIView.UpdateRemoveScore;
            _encounterState.OnTotalScoreChanged -= _scoreUIView.UpdateScore;
            
            _encounterState.OnPromptTextChanged -= _promptUIView.UpdatePromptWindowText;
            
            _encounterState.OnCharactersPerMinuteChanged -= _statsUIView.UpdateCharactersPerMinute;
            _encounterState.OnWordsPerMinuteChanged -= _statsUIView.UpdateWordsPerMinute;
            _encounterState.OnMistakesInCurrentPromptChanged -= _statsUIView.UpdateMistakeCount;
        }
        
        public void Bind(EncounterInstance encounterInstance, EncounterState encounterState, InputManager inputManager)
        {
            _encounterInstance = encounterInstance;
            _encounterState = encounterState;
            
            // TODO: Figure out if this is the right place for this
            _inputManager = inputManager;
            _root.RegisterCallback<KeyUpEvent>(_inputManager.OnKeyUp, TrickleDown.TrickleDown);
            
            _promptUIView.Initialize(_encounterInstance.EncounterDefinition.characterWidth);
            _promptUIView.UpdatePromptWindowText(_encounterState.CurrentPrompt);
            
            _typeWindowUIView.Initialize(_encounterInstance.EncounterDefinition.characterWidth);
            _typeWindowUIView.ChangeFocusToTypeWindow(true);

            _encounterInstance.OnCharacterTyped += OnCharacterType;
            _encounterInstance.OnEncounterEnded += OnEncounterEnd;
            _encounterInstance.OnEncounterPromptRefreshed += ResetEncounterUI;
            _encounterInstance.OnPromptFinished += _promptUIView.UpdateHistoryPromptWindow;
            
            _encounterState.OnTypedTextChanged += _typeWindowUIView.SetTypeWindowText;
            _encounterState.OnCursorVisibilityChanged += _typeWindowUIView.SetCursorVisibility;
            
            _encounterState.OnScoreToAddChanged += _scoreUIView.UpdateAddScore;
            _encounterState.OnScoreToRemoveChanged += _scoreUIView.UpdateRemoveScore;
            _encounterState.OnTotalScoreChanged += _scoreUIView.UpdateScore;
            
            _encounterState.OnPromptTextChanged += _promptUIView.UpdatePromptWindowText;
            
            _encounterState.OnCharactersPerMinuteChanged += _statsUIView.UpdateCharactersPerMinute;
            _encounterState.OnWordsPerMinuteChanged += _statsUIView.UpdateWordsPerMinute;
            _encounterState.OnMistakesInCurrentPromptChanged += _statsUIView.UpdateMistakeCount;
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

        private void OnEncounterEnd()
        {
            _promptUIView.UpdateHistoryPromptWindow();
            _encounterState.SetCursorVisibility(false);
        }
    }
}
