using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data._ScriptableObjectScripts
{
    [CreateAssetMenu(fileName = "ScoreConfig", menuName = "Data/ScoreConfig")]
    public class ScoreConfig : ScriptableObject
    {
        [FormerlySerializedAs("scoreCurveData")] public CurveData addScoreCurveData;
        public CurveData removeScoreCurveData;
    }
}
