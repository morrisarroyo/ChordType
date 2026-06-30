using System.IO;
using Game.Data.Save;
using UnityEngine;

namespace Game.Core.Save
{
    public class SaveManager
    {
        private SaveData _cachedData;

        private static string SavePath =>
            Path.Combine(Application.persistentDataPath, "save.json");

        public SaveManager()
        {
            Load();
        }
        
        // TODO: Improve Data Setting/Modification
        public SaveData Data
        {
            get
            {
                if (_cachedData == null)
                    Load();

                return _cachedData;
            }
        }

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                _cachedData = new SaveData();
                Save();
                return;
            }

            string json = File.ReadAllText(SavePath);
            _cachedData = JsonUtility.FromJson<SaveData>(json);

            if (_cachedData == null)
                _cachedData = new SaveData();
            
#if UNITY_EDITOR
            _cachedData.PrintHighScores(_cachedData.highScores);
#endif
        }

        public void Save()
        {
            string json = JsonUtility.ToJson(_cachedData, true);
            File.WriteAllText(SavePath, json);
            
#if UNITY_EDITOR
            Debug.Log(json);
#endif
        }
  
    }
}