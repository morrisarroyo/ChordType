using System;
using Game.Data;
using Game.Systems.Score;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Systems.Encounter
{
    public class EncounterInstance
    {
        private EncounterDefinition _encounterDefinition;
        public EncounterDefinition EncounterDefinition => _encounterDefinition;
        
        private EncounterState _encounterState;

        private ScoreController _scoreController;
        
        // Event triggered whenever score changes
        // UI or other systems can subscribe to update themselves
        public event Action OnEncounterEnded;
        public event Action OnPromptFinished;
        public event Action OnFinalPromptFinished;
        public event Action OnEncounterPromptRefreshed;
        public event Action<char, Color> OnCharacterTyped;
        
        private const int SECONDS_IN_MINUTE = 60;

        public EncounterInstance(EncounterDefinition encounterDefinition, EncounterState encounterState, ScoreController scoreController)
        {
            _encounterDefinition = encounterDefinition;
            _encounterState = encounterState;
            _scoreController =  scoreController;
            
            Validate();

            UpdateToNewPrompt();
        }

        private void UpdateToNewPrompt()
        {
            _encounterState.ResetTypedText();
            
            _encounterState.UpdateCurrentPrompt(_encounterDefinition.GetRandomPromptString(10));
            EvokePromptRefresh();
        }

        private void EvokePromptRefresh()
        {
            _encounterState.IsStartingNewPrompt = true;
            
            _encounterState.ResetCurrentPromptWordCount();
            _encounterState.ResetCorrectsInCurrentPrompt();
            _encounterState.ResetMistakesInCurrentPrompt();
            
            OnEncounterPromptRefreshed?.Invoke();
            Debug.Log($"{nameof(EncounterInstance)}. Prompt: " + _encounterState.CurrentPrompt);
        }

        public void OnKeyUp(KeyUpEvent keyUpEvent)
        {
            // End early if typedText is the same lenght or longer
            if (_encounterState.CurrentPrompt.Length <= _encounterState.TypedText.Length)
            {
                FinishPrompt();
                return;
            }
            
            if (keyUpEvent.keyCode is KeyCode.Return or KeyCode.KeypadEnter)
            {
                OnCharacterTyped?.Invoke('\n', _encounterDefinition.incorrectColor);
                return;
            }
            
            if (!KeyCodeCharMap.TryGetChar(keyUpEvent.keyCode, keyUpEvent.shiftKey, out var character)) 
                return;
            
            UpdateEncounterTimers();

            _encounterState.AppendToTypedText(character);
            
            int currentIndexInPrompt = _encounterState.TypedText.Length - 1;

            // Compare Typed Character with Prompt Character at the same index
            bool isCorrect = _encounterState.TypedText[currentIndexInPrompt] == _encounterState.CurrentPrompt[currentIndexInPrompt];
            UpdateEncounterStats(isCorrect, currentIndexInPrompt);
            
            OnCharacterTyped?.Invoke(character, isCorrect ? _encounterDefinition.correctColor : _encounterDefinition.incorrectColor);
        }

        private void UpdateEncounterTimers()
        {
            if (_encounterState.IsStartingNewPrompt)
            {
                _encounterState.IsStartingNewPrompt = false;
                _encounterState.UpdateTimersForNewPrompt();
            }

            _encounterState.UpdateTimersForNewInput();
        }

        private void UpdateEncounterStats(bool isCorrect, int currentIndexInPrompt)
        {
            if (isCorrect)
            {
                _scoreController.AddScore(_encounterDefinition.scorePerCharacter);
                _encounterState.IncrementCorrectsInCurrentPrompt();
            }
            else
            {
                _encounterState.RemoveScore(_encounterDefinition.scorePerCharacter);
                _encounterState.IncrementMistakesInCurrentPrompt();
            }
            
            if (_encounterState.CurrentPrompt[currentIndexInPrompt] == ' ')
            {
                _encounterState.IncrementCurrentPromptWordCount();
            }
            
            CalculatePerMinStatsForPrompt();
        }

        private void CalculatePerMinStatsForEncounter()
        {
            _encounterState.CharactersPerMinute = ((_encounterState.TypedText.Length + _encounterState.EncounterCharacterCount)
                                                    - (_encounterState.MistakesInCurrentPrompt + _encounterState.EncounterMistakesCount)) 
                                                  / (_encounterState.timeSincePromptStart / SECONDS_IN_MINUTE);

            
            float approximateWordMistakeCount =  (_encounterState.MistakesInCurrentPrompt + _encounterState.EncounterMistakesCount)
                                                 / ((float)(_encounterState.TypedText.Length + _encounterState.EncounterCharacterCount)
                                                    / (_encounterState.CurrentPromptWordCount + _encounterState.EncounterWordCount));
            _encounterState.WordsPerMinute = ((_encounterState.CurrentPromptWordCount + _encounterState.EncounterWordCount) - approximateWordMistakeCount) 
                                             / (_encounterState.timeSincePromptStart / SECONDS_IN_MINUTE);
        }
        
        private void CalculatePerMinStatsForPrompt()
        {
            _encounterState.CharactersPerMinute = (_encounterState.TypedText.Length - _encounterState.MistakesInCurrentPrompt) 
                                                  / (_encounterState.timeSincePromptStart / SECONDS_IN_MINUTE);

            
            float approximateWordMistakeCount =  _encounterState.MistakesInCurrentPrompt 
                                                 / ((float)_encounterState.TypedText.Length / _encounterState.CurrentPromptWordCount);
            _encounterState.WordsPerMinute = (_encounterState.CurrentPromptWordCount - approximateWordMistakeCount) 
                                             / (_encounterState.timeSincePromptStart / SECONDS_IN_MINUTE);
        }

        public void StartEncounter()
        {
            UpdateToNewPrompt();
        }

        public void RestartPrompt()
        {
            _encounterState.ResetPrompt();
            
            EvokePromptRefresh();
        }
        
        public void RestartEncounter()
        {
           RestartPrompt();
            
            _scoreController.ResetScore();
        }
        
        public void FinishPrompt()
        {
            OnPromptFinished?.Invoke();
            
            _encounterState.FinishPrompt();

            if (_encounterState.PromptIndex + 1 < _encounterDefinition.maxPromptsPerEncounter)
            {
                UpdateToNewPrompt();
            }
            else
            {
                FinishLastPrompt();
            }
        }

        public void FinishLastPrompt()
        {
            OnFinalPromptFinished?.Invoke();
        }
        
        public void FinishEncounter()
        {
            OnEncounterEnded?.Invoke();
            
            _encounterState.FinishEncounter();
            _scoreController.ResetScore();
        }
        
        private void Validate()
        {
            Debug.AssertFormat(_encounterDefinition != null,
                $"{nameof(_encounterDefinition)} is required.",
                this);
            Debug.AssertFormat(_encounterState != null,
                $"{nameof(_encounterState)} is required.",
                this);
            Debug.AssertFormat(_scoreController != null,
                $"{nameof(_scoreController)} is required.",
                this);
        }

        public void DeleteInstance()
        {
            
        }
        
        public int GetEncounterScore()
        {
            return _encounterState.TotalScore;
        }
    }
}

