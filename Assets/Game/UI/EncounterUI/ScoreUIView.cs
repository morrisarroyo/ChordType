using UnityEngine.UIElements;

namespace Game.UI.EncounterUI
{
    public class ScoreUIView
    {
        private Label _addScoreLabel;
        private Label _removeScoreLabel;
        private Label _scoreLabel;
        
        public ScoreUIView(VisualElement root)
        {
            _addScoreLabel = root.Q<Label>("add-score-label");
            _removeScoreLabel = root.Q<Label>("remove-score-label");
            _scoreLabel = root.Q<Label>("score-label");
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
        
        public void UpdateScore(int score)
        {
            _scoreLabel.text = $"{score}";
        }
    }
}
