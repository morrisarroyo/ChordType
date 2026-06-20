using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "CurveData", menuName = "Data/Curve")]
    public class CurveData : ScriptableObject
    {
        public AnimationCurve curve;
    }
}
