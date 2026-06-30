using System;
using Game.Systems.Encounter;
using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    public class EncounterStatsPopup
    {
        private readonly VisualElement _encounterStatsPopup;

        private readonly Label _promptCountLabel;
        private readonly Label _totalCharactersLabel;
        private readonly Label _correctCharactersLabel;
        private readonly Label _mistakesLabel;
        private readonly Label _totalScoreLabel;    
        
        private readonly Button _closeButton;

        public event Action OnEncounterStatsPopupCloseClicked;

        public EncounterStatsPopup(VisualElement root, EncounterState encounterState)
        {
            _encounterStatsPopup = root.Q<VisualElement>("encounter-stats-popup");
            
            _promptCountLabel = _encounterStatsPopup.Q<Label>("prompt-count-label");
            _totalCharactersLabel = _encounterStatsPopup.Q<Label>("total-characters-label");
            _correctCharactersLabel = _encounterStatsPopup.Q<Label>("correct-characters-label");
            _mistakesLabel = _encounterStatsPopup.Q<Label>("mistakes-label");
            _totalScoreLabel = _encounterStatsPopup.Q<Label>("encounter-stats-total-score-label");
            
            _closeButton = _encounterStatsPopup.Q<Button>("encounter-stats-close-button");

            BindButtons();
            
            UpdateStatsFromData(encounterState);
        }

        private void BindButtons()
        {
            _closeButton.clicked += OnCloseClick;
        }

        public void UnbindButtons()
        {
            _closeButton.clicked -= OnCloseClick;
        }

        public void UpdateStatsFromData(EncounterState encounterState)
        {
            _promptCountLabel.text = $"{encounterState.PromptIndex + 1}";
            _totalCharactersLabel.text = encounterState.EncounterCharacterCount.ToString();
            _correctCharactersLabel.text = encounterState.EncounterCorrectsCount.ToString();
            _mistakesLabel.text = encounterState.EncounterMistakesCount.ToString();
            _totalScoreLabel.text = encounterState.TotalScore.ToString();
        }
        
        public void ShowEncounterStatsPopup()
        {
            _encounterStatsPopup.style.display = DisplayStyle.Flex;
        }
        
        public void HideEncounterStatsPopup()
        {
            _encounterStatsPopup.style.display = DisplayStyle.None;
        }

        private void OnCloseClick()
        {
            OnEncounterStatsPopupCloseClicked?.Invoke();
        }
    }
}
