using System;
using UnityEngine;

namespace Game.Systems.Encounter
{
    public class EncounterState //, IScoreState
    {
        private bool _isStartingNewPrompt = true;
        private int _promptIndex = 0;
        public int PromptIndex => _promptIndex;

        public bool IsStartingNewPrompt
        {
            get => _isStartingNewPrompt;
            set
            {
                // Prompt only when starting new prompt
                if (_isStartingNewPrompt != value && !value)
                {
                    OnNewPromptStarted?.Invoke();
                }
                 
                _isStartingNewPrompt = value;
            }
        }
        
        public float timeSincePromptStart = 0.0f;
        public float timeAtLastKeyUp = 0.0f;
        public float timeSinceLastKeyUp = 0.0f;
        public float timeAtPromptStart = 0.0f;

        private string _currentPrompt;
        public string CurrentPrompt => _currentPrompt;
        
        private string _typedText;
        public string TypedText => _typedText;
        
        
        
        // TODO: Move to a ScoreInterface/Score State
        private int _scoreToAdd = 0;
        public int ScoreToAdd => _scoreToAdd;
        private int _scoreToRemove = 0;
        public int ScoreToRemove => _scoreToRemove;
    
        private int _promptScore = 0;
        public int PromptScore => _promptScore;
        
        private int _totalScore = 0;
        public int TotalScore => _totalScore;
    
        private bool _isCursorVisible = false;
        public bool IsCursorVisible => _isCursorVisible;
        
        // >>>>>>>> TODO: Move to separate Stats data struct/class? TypeStatState
        private float _charactersPerMinute = 0.0f;
        public float CharactersPerMinute
        {
            get => _charactersPerMinute;
            set
            {
                _charactersPerMinute = value;
                OnCharactersPerMinuteChanged?.Invoke(_charactersPerMinute);
            }
        }

        private float _wordsPerMinute = 0.0f;
        public float WordsPerMinute
        {
            get => _wordsPerMinute;
            set
            {
                _wordsPerMinute = value;
                OnWordsPerMinuteChanged?.Invoke(_wordsPerMinute);
            }
        }
        
        private int _currentPromptWordCount = 0;
        public int CurrentPromptWordCount => _currentPromptWordCount;
        private int _correctsInCurrentPrompt = 0;
        public int CorrectsInCurrentPrompt => _correctsInCurrentPrompt;
        private int _mistakesInCurrentPrompt = 0;
        public int MistakesInCurrentPrompt => _mistakesInCurrentPrompt;
        
        private int _encounterCharacterCount = 0;
        public int EncounterCharacterCount => _encounterCharacterCount;
        private int _encounterWordCount = 0;
        public int EncounterWordCount => _encounterWordCount;
        private int _encounterCorrectsCount = 0;
        public int EncounterCorrectsCount => _encounterCorrectsCount;
        private int _encounterMistakesCount = 0;
        public int EncounterMistakesCount => _encounterMistakesCount;
        // >>>>>>>>><<< End Refactor out
        
        public event Action OnNewPromptStarted;
        public event Action OnPromptTextRefreshed;
        
        public event Action<string> OnTypedTextChanged;
        public event Action<string> OnPromptTextChanged;
        
        public event Action<int> OnScoreToAddChanged;
        public event Action<int> OnScoreToRemoveChanged;
        public event Action<int> OnPromptScoreChanged;
        public event Action<int> OnTotalScoreChanged;
        
        public event Action<bool> OnCursorVisibilityChanged;

        public event Action<float> OnCharactersPerMinuteChanged;
        public event Action<float> OnWordsPerMinuteChanged;
        public event Action<int> OnCorrectsInCurrentPromptChanged;
        public event Action<int> OnMistakesInCurrentPromptChanged;
        
        
        public void AddScore(int value)
        {
            _scoreToAdd = value;
            OnScoreToAddChanged?.Invoke(value);
        
            _promptScore += _scoreToAdd;
            OnPromptScoreChanged?.Invoke(_promptScore);
        }
        
        public void RemoveScore(int value)
        {
            _scoreToRemove = value;
            OnScoreToRemoveChanged?.Invoke(value);
        
            _promptScore -= _scoreToRemove;
            OnPromptScoreChanged?.Invoke(_promptScore);
        }
        
        public void ResetScore()
        {
            _promptScore = 0;
            _totalScore = 0;
        }
        
        public void SetCursorVisibility(bool isCursorVisible)
        {
            _isCursorVisible = isCursorVisible;
            OnCursorVisibilityChanged?.Invoke(isCursorVisible);
        }
        
        public void IncrementCorrectsInCurrentPrompt()
        {
            _correctsInCurrentPrompt += 1;
            OnCorrectsInCurrentPromptChanged?.Invoke(_correctsInCurrentPrompt);
        }

        public void IncrementMistakesInCurrentPrompt()
        {
            _mistakesInCurrentPrompt += 1;
            OnMistakesInCurrentPromptChanged?.Invoke(_mistakesInCurrentPrompt);
        }

        public void ResetCorrectsInCurrentPrompt()
        {
            _correctsInCurrentPrompt = 0;
        }

        public void ResetMistakesInCurrentPrompt()
        {
            _mistakesInCurrentPrompt = 0;
        }

        public void IncrementCurrentPromptWordCount()
        {
            _currentPromptWordCount += 1;
        }

        public void ResetCurrentPromptWordCount()
        {
            _currentPromptWordCount = 0;
        }
        
        public void UpdateCurrentPrompt(string newPrompt)
        {
            if (newPrompt != _currentPrompt)
            {
                _currentPrompt = newPrompt;
                OnPromptTextChanged?.Invoke(_currentPrompt);
            }
            else
            {
                OnPromptTextRefreshed?.Invoke();
            }
        }
        
        public void AppendToTypedText(char characterToAppend)
        {
            _typedText += characterToAppend;
            OnTypedTextChanged?.Invoke(_typedText);
        }

        public void ResetTypedText()
        {
            _typedText = "";
        }

        public void FinishPrompt()
        {
            _encounterCharacterCount += _typedText.Length;
            _encounterWordCount += _currentPromptWordCount;
            _encounterCorrectsCount += _correctsInCurrentPrompt;
            _encounterMistakesCount += _mistakesInCurrentPrompt;
            ++_promptIndex;
            
            _totalScore += _promptScore;
            _promptScore = 0;
            OnTotalScoreChanged?.Invoke(_totalScore);
            
            ResetPrompt();
        }
        
        public void FinishEncounter()
        {
            
        }
        
        public void ResetPrompt()
        {
            ResetTypedText();
            
            _currentPromptWordCount = 0;
            _correctsInCurrentPrompt = 0;            
            _mistakesInCurrentPrompt = 0;
            
            _promptScore = 0;
        }
        
        public void UpdateTimersForNewPrompt()
        {
            timeSincePromptStart = 0.0f;
            timeAtPromptStart = Time.unscaledTime;
            timeAtLastKeyUp = timeAtPromptStart;
        }
        
        public void UpdateTimersForNewInput()
        {
            timeSinceLastKeyUp = Time.unscaledTime - timeAtLastKeyUp;
            timeAtLastKeyUp = Time.unscaledTime;
            timeSincePromptStart = timeAtLastKeyUp - timeAtPromptStart;
        }
    }
}
