using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "SpaceSeparatedData", menuName = "Data/Space Separated Data")]
    public class SpaceSeparatedData : ScriptableObject
    {
        [TextArea(5, 20)]
        public string ssvText;

        /// <summary>
        /// Parses the SSV text into rows of cells.
        /// Each line is a row, and cells are separated by spaces.
        /// </summary>
        public string[][] GetRows()
        {
            if (string.IsNullOrEmpty(ssvText))
                return new string[0][];

            // Split into lines, ignoring empty lines
            string[] lines = ssvText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            string[][] result = new string[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                // Split each line by spaces
                result[i] = lines[i].Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            }

            return result;
        }
    
        /// <summary>
        /// Parses a specific of the SSV text into a List of strings.
        /// </summary>
        public List<string> GetRowAtIndex(int index)
        {
            if (string.IsNullOrEmpty(ssvText))
            {
                Debug.LogWarning("ssvText is empty");
                return new List<string>();
            }
        
            // Split into lines, ignoring empty lines
            string[] lines = ssvText.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (index > lines.Length || string.IsNullOrEmpty(lines[index]))
            {
                Debug.LogWarning("index is out of range");
                return new List<string>();
            }
        
            return lines[index].Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
