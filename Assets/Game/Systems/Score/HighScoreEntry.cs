using System;

namespace Game.Systems.Score
{
    [Serializable]
    public class HighScoreEntry
    {
        public string playerName; // Get from GameManager? Or Wherever Player Name/Session Owner is changed
        public int score;
        public long dateTimeStamp;
        public string difficulty; // TODO: Change to modifiers
        public string version;
        
        public HighScoreEntry(DateTime dateTime, string playerName, int score, string difficulty, string version)
        {
            this.dateTimeStamp = dateTime.Ticks;
            this.playerName = playerName;
            this.score = score;
            this.difficulty = difficulty;
            this.version = version;
        }
    }
}
