using System;
using System.Collections.Generic;
using Game.Systems.Score;
using UnityEngine.UIElements;

namespace Game.UI.HighScore
{
    public class HighScoresOverlay
    {
        private readonly VisualElement _highScoresOverlayElement;
        
        private readonly VisualTreeAsset _entryTemplate;
        private readonly VisualElement _entriesContainer;

        private readonly List<TemplateContainer> _rows = new();
        
        private readonly Button _highScoresCloseButton;
        
        public HighScoresOverlay(
            VisualElement root,
            VisualTreeAsset entryTemplate,
            int highScoreCount,
            IReadOnlyList<HighScoreEntry> scores)
        {
            _entryTemplate = entryTemplate;
            
            _highScoresOverlayElement = root.Q<VisualElement>("highscores-overlay");
            _entriesContainer = root.Q<VisualElement>("entries-container");
            _highScoresCloseButton = root.Q<Button>("highscores-close-button");
            
            BindButtons();
            
            _entriesContainer.Clear();
            for (int i = 0; i < highScoreCount; i++)
            {
                var row = _entryTemplate.Instantiate();
                _rows.Add(row);
                _entriesContainer.Add(row);
            }
            
            RepopulateHighScores(scores);
        }

        private void BindButtons()
        {
            _highScoresCloseButton.clicked += OnHighScoresClose;
        }

        public void UnbindButtons()
        {
            _highScoresCloseButton.clicked -= OnHighScoresClose;
        }

        public void ShowHighScoresOverlay()
        {
            _highScoresOverlayElement.style.display = DisplayStyle.Flex;
        }
        
        public void HideHighScoresOverlay()
        {
            _highScoresOverlayElement.style.display = DisplayStyle.None;
        }
        
        private void OnHighScoresClose()
        {
            HideHighScoresOverlay();
        }

        public void RepopulateHighScores(IReadOnlyList<HighScoreEntry> scores)
        {
            int i = 0;
            foreach (var row in _rows)
            {
                row.Q<Label>("entry-rank-label").text = (i + 1).ToString();

                if (i < scores.Count)
                {
                    PopulateEntry(row, scores[i]);
                }
                else
                {
                    HighScoreEntry invalidEntry = new HighScoreEntry(DateTime.MinValue, "Invalid", Int16.MinValue, "N/A", "Broken");
                    PopulateEntry(row, invalidEntry);
                    row.visible = false;
                }
                
                ++i;
            }
            
            /*
            for (; i < scores.Count; i++)
            {
                var row = _entryTemplate.Instantiate();
                row.Q<Label>("entry-rank-label").text = (i + 1).ToString();
                PopulateEntry(row, scores[i]);

                _entriesContainer.Add(row);
            }
            */
        }

        private static void PopulateEntry(TemplateContainer row, HighScoreEntry scoreEntry)
        {
            row.Q<Label>("entry-name-label").text = scoreEntry.playerName;
            row.Q<Label>("entry-date-label").text = new DateTime(scoreEntry.dateTimeStamp, DateTimeKind.Utc).ToString("yyyy-MM-dd");
            row.Q<Label>("entry-score-label").text = scoreEntry.score.ToString();
        }
    }
}