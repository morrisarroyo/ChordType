using UnityEngine;

namespace Game.Data._ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Data/ScoreConfig")]
    public class ScoreConfig : ScriptableObject
    {
        public CurveData scoreCurveData;
        
    }
}
