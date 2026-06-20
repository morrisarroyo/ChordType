using UnityEngine.UIElements;

namespace Game.UI.EncounterUI
{
    public class StatsUIView
    {
        private Label _wordsPerMinuteLabel;
        private Label _charactersPerMinuteLabel;
        private Label _mistakeCountLabel;
        
        public StatsUIView(VisualElement root)
        {
            _wordsPerMinuteLabel = root.Q<Label>("words-per-minute-label");
            _charactersPerMinuteLabel = root.Q<Label>("characters-per-minute-label");
            _mistakeCountLabel = root.Q<Label>("mistake-count-label");
        }
        
        public void UpdateWordsPerMinute(float wordsPerMinute)
        {
            _wordsPerMinuteLabel.text = $"{wordsPerMinute}";
            // _wordsPerMinuteLabel.MarkDirtyText();
        }
        
        public void UpdateCharactersPerMinute(float charactersPerMinute)
        {
            _charactersPerMinuteLabel.text = $"{charactersPerMinute}";
            _charactersPerMinuteLabel.MarkDirtyText();
        }
        
        public void UpdateMistakeCount(int mistakeCount)
        {
            _mistakeCountLabel.text = $"{mistakeCount}";
        }
    }
}
