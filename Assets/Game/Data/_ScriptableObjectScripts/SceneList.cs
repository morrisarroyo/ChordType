using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "Data/SceneList")]
    public class SceneList : ScriptableObject
    {
#if UNITY_EDITOR  
        // Need to wrap in UnityEditor because SceneAsset is Editor Only
        public SceneAsset MainMenu;
        public SceneAsset TestEncounter;
#endif
        [HideInInspector, SerializeField] public string mainMenuSceneName;
        [HideInInspector, SerializeField] public string testEncounterSceneName;
        
#if UNITY_EDITOR
        public void OnValidate()
        {
            FillScenes();

            if (string.IsNullOrEmpty(mainMenuSceneName))
            {
                Debug.LogWarning("Missing main menu scene name");
            }
            if (string.IsNullOrEmpty(testEncounterSceneName))
            {
                Debug.LogWarning("Missing test encounter scene name");
            }
        }

        private void FillScenes()
        {
            mainMenuSceneName = MainMenu != null ? MainMenu.name : "";
            testEncounterSceneName = TestEncounter != null ? TestEncounter.name : "";
        }

#endif

    }
}
