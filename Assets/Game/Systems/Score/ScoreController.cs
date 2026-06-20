using System;
using Game.Core.GameManager;
using Game.Data;
using Game.Data._ScriptableObjectScripts;
using Game.Systems.Encounter;
using UnityEngine;

namespace Game.Systems.Score
{
    public class ScoreController
    {
        private ScoreConfig _scoreConfig;
        
        private EncounterState _encounterState;

        public ScoreController(ScoreConfig scoreConfig, EncounterState encounterState)
        {
            _scoreConfig = scoreConfig;
            
            _encounterState = encounterState;
        }
        
        /// <summary>
        /// Adds points to the current score.
        /// </summary>
        /// <param name="amount">How many points to add</param>
        public void AddScore(int amount)
        {
            // TODO: Expose and Improve Scoring and Calculations logic
            // Use Score Curve as a Multiplier; DeltaTime between keypress as time evaluator
            int scoreToAdd = Mathf.RoundToInt(amount * _scoreConfig.scoreCurveData.curve.Evaluate(_encounterState.timeSinceLastKeyUp));
            
            _encounterState.AddScore(scoreToAdd);
        }
        
        
        /// <summary>
        /// Remove points from the current score.
        /// </summary>
        /// <param name="amount">How many points to remove</param>
        public void RemoveScore(int amount)
        {
            // TODO: Figure out if point removal requires scaling as well?
            _encounterState.RemoveScore(amount);
        }

        /// <summary>
        /// Resets score back to zero.
        /// </summary>
        public void ResetScore()
        {
            _encounterState.ResetScore();
        }
    }
}