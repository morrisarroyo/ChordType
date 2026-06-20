using UnityEditor;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "SceneList", menuName = "Data/SceneList")]
    public class SceneList : ScriptableObject
    {
        public SceneAsset MainMenu;
        public SceneAsset TestEncounter;
    }
}
