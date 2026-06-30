using System;
using System.Collections.Generic;
using Game.Systems.Score;
using UnityEngine;

namespace Game.Data.Save
{
    [System.Serializable]
    public class SaveData
    {
        private const int HIGH_SCORES_MAX_CAPACITY = 20; // Disconnected from the amount to be displayed in the UI on purpose
        public List<HighScoreEntry> highScores = new(HIGH_SCORES_MAX_CAPACITY);
        
        public SettingsData settingsData =  new SettingsData();

        // TODO: Change method to process a finished encounter instead, should not limited be limited to just high scores
        public void ProcessNewHighScore(string playerName = "Test", int score = 0, string difficulty = "N/A", string version = "0.0.0.0")
        {
            var highScoreEntry = new HighScoreEntry(DateTime.UtcNow, playerName, score, difficulty, version);
            Debug.Log(highScoreEntry);
            
            if (highScores.Count >= HIGH_SCORES_MAX_CAPACITY)
            {
                for (int i = 0; i < highScores.Count; i++)
                {
                    if (score > highScores[i].score)
                    {
                        highScores.RemoveAt(highScores.Count - 1);
                        highScores.Insert(i, highScoreEntry);
                        return;
                    }
                }
            }
            else
            {
                for (int i = 0; i < highScores.Count; i++)
                {
                    if (score > highScores[i].score)
                    {
                        highScores.Insert(i, highScoreEntry);
                        return;
                    }
                }
                
                highScores.Add(highScoreEntry);
            }
        }
        
              
#if UNITY_EDITOR
        public void PrintHighScores(IReadOnlyList<HighScoreEntry> highScoreEntries)
        {
            if (highScoreEntries == null || highScoreEntries.Count == 0)
            {
                Debug.Log("HighScores: EMPTY");
                return;
            }

            string output = "===== HIGH SCORES =====\n";

            for (int i = 0; i < highScoreEntries.Count; i++)
            {
                output += $"{i + 1}. {highScoreEntries[i].playerName} - {highScoreEntries[i].score}\n";
            }

            output += "=======================";

            Debug.Log(output);
        }
#endif
    }
}