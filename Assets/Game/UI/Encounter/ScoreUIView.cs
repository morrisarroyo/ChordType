using UnityEngine.UIElements;

namespace Game.UI.Encounter
{
    public class ScoreUIView
    {
        private Label _addScoreLabel;
        private Label _removeScoreLabel;
        private Label _promptScoreLabel;
        private Label _totalScoreLabel;
        
        public ScoreUIView(VisualElement root)
        {
            _addScoreLabel = root.Q<Label>("add-score-label");
            _removeScoreLabel = root.Q<Label>("remove-score-label");
            _promptScoreLabel = root.Q<Label>("prompt-score-label");
            _totalScoreLabel = root.Q<Label>("total-score-label");
        }
        
        public void UpdateAddScore(int addScore)
        {
            _addScoreLabel.text = $"+{addScore}";
            
            _addScoreLabel.RemoveFromClassList("correct-popup");
            _addScoreLabel.schedule.Execute(() => _addScoreLabel.AddToClassList("correct-popup")).StartingIn(1);
        }
        
        public void UpdateRemoveScore(int removeScore)
        {
            _removeScoreLabel.text = $"-{removeScore}";
            
            _removeScoreLabel.RemoveFromClassList("mistake-popdown");
            _removeScoreLabel.schedule.Execute(() => _removeScoreLabel.AddToClassList("mistake-popdown")).StartingIn(1);
        }
        
        public void UpdatePromptScore(int score)
        {
            _promptScoreLabel.text = $"{score}";
        }

        public void UpdateTotalScore(int score)
        {
            _totalScoreLabel.text = $"{score}";
        }
    }
}
