using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Range(1, 20)]
        public int highScoreCountToDisplay = 10;
    }
}
