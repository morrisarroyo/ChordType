using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    
        public void PlaySoundOneShot(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
        
        public void PlaySoundOverride(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}