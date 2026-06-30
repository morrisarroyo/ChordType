using UnityEngine;

namespace Game.Utilities
{
    public class AudioUtility
    {
        // For Audio Mixer
        public static float PercentToDecibel(float percent)
        {
            if (percent <= 0f)
                return -80f; // or -160f depending on mixer range

            float volume = percent / 100f;
            return Mathf.Log10(volume) * 20f;
        }
    }
}
