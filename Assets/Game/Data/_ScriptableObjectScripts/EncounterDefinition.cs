using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "EncounterDefinition", menuName = "Data/EncounterDefinition")]
    public class EncounterDefinition : ScriptableObject
    {
        // TODO: Move to a new separate EncounterConfig Data SO?
        public float baseFontSize = 32.0f;
        public float characterWidthRatio = 0.7f;

        public int maxPromptsPerEncounter = 3;
        
        public int scorePerCharacter = 100;
        
        public SpaceSeparatedData promptPoolAsset;
    
        private List<string> _promptSourceList = new List<string>();

        public Color correctColor = Color.green;
        public Color incorrectColor = Color.red;

        public string GetRandomPromptString(int wordCount)
        {
            if (promptPoolAsset == null)
            {
                Debug.LogError("Prompt Pool Asset is null");
                return "";
            }
            
            if (_promptSourceList.Count == 0)
            {
                _promptSourceList = promptPoolAsset.GetRowAtIndex(0);
            }
            /*
            // Optimize when needed, LINQ is slow
            List<string> randomPrompt = _promptSourceList
                .OrderBy(x => Guid.NewGuid())
                .Take(wordCount)
                .ToList();
            var result = new StringBuilder();
            for (int i = 0; i < randomPrompt.Count - 1; ++i)
            {
                result.AppendFormat("{0} ", randomPrompt[i]);
            }
            result.Append(randomPrompt.Last());
            */
            
            List<string> availablePrompts = new List<string>(_promptSourceList);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < wordCount && availablePrompts.Count > 0; i++)
            {
                int index = UnityEngine.Random.Range(0, availablePrompts.Count);

                if (i > 0)
                {
                    result.Append(' ');
                }

                result.Append(availablePrompts[index]);
                availablePrompts.RemoveAt(index);
            }
            
            return result.ToString();
        }
    }
}
