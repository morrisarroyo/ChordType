using System.Collections;
using Game.Audio;
using Game.Data;
using Game.Systems.Encounter;
using Game.Systems.Input;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Game.Core.GameManager
{
    [RequireComponent(typeof(InputManager))]
    public class GameManager : MonoBehaviour
    {
        // Singleton instance (easy global access)
        public static GameManager Instance { get; private set; }
        
        [SerializeField] public InputManager inputManager; 
        
        // TODO: Create a separate SceneService/ Loader
        [SerializeField]
        private SceneList sceneList;
        
        // TODO: Fill encounter list and improve logic of encounter selection (save data?)
        [SerializeField] 
        public AssetReferenceT<EncounterList> encounterListAssetReference;
        // TODO: Improve and Move Asset Loading and Unloading
        private  AsyncOperationHandle<EncounterList> _encounterListAssetHandle;
        private EncounterList _encounterList;
        

        // Current state of the game
        public enum GameState
        {
            MainMenu,
            Playing,
            InEncounter,
            Paused,
            GameOver
        }

        public GameState CurrentState { get; private set; }

        private void Awake()
        {
            // Ensure only one GameManager exists
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Optional: persist across scenes
            DontDestroyOnLoad(gameObject);
            
            Validate.Assigned(inputManager);
            
            Addressables.LoadAssetAsync<EncounterList>(encounterListAssetReference)
                .Completed += handle =>
            {
                _encounterListAssetHandle = handle;
                _encounterList = handle.Result;
            };
        }
        
        private void OnEnable()
        {
//            _inputController.OnScoreButtonPressed += _scoreController.AddPoint;
        }


        private void Start()
        {
            SetState(GameState.MainMenu);

            // TODO: Move when Main Menu is implemented
            // StartGame();
        }
        
        private void OnDisable()
        {
            
        }
        
        private void OnDestroy()
        {
           //  Addressables.Release(_encounterListAssetHandle);  
        }

#if UNITY_EDITOR
            private void OnValidate()
            {
                
            }
#endif

        // Change game state safely
        public void SetState(GameState newState)
        {
            CurrentState = newState;

            switch (newState)
            {
                case GameState.Playing:
                    Time.timeScale = 1f;
                    break;

                case GameState.InEncounter:
                    Time.timeScale = 1f;
                    break;

                case GameState.Paused:
                    Time.timeScale = 0f;
                    break;

                case GameState.GameOver:
                    Time.timeScale = 0f;
                    break;

                case GameState.MainMenu:
                    Time.timeScale = 1f;
                    break;
            }
        }

        // Start game
        public void StartGame()
        {
            SetState(GameState.Playing);

            if (_encounterList == null)
            {
                Debug.LogError($"{nameof(_encounterList)} is null and is not loaded properly");
                return;
            }

            if (SceneManager.GetActiveScene().name != sceneList.MainMenu.name)
            {
                Debug.LogError($"Attempting to start game not from the Main Menu!");
            }
            
            // TODO: Remove use of test encounter encounter list and improve logic of encounter selection
            LoadScene(sceneList.TestEncounter.name, () =>
                {
                    Debug.Log($"{sceneList.TestEncounter.name} loaded");
                });

            SceneManager.sceneLoaded += OnEncounterLoad;
        }

        // Pause game toggle
        public void TogglePause()
        {
            if (CurrentState == GameState.Playing)
                SetState(GameState.Paused);
            else if (CurrentState == GameState.Paused)
                SetState(GameState.Playing);
        }

        // Restart current scene
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Game over logic
        public void GameOver()
        {
            SetState(GameState.GameOver);
        }
        
        public void LoadScene(string sceneName, System.Action onComplete)
        {
            StartCoroutine(LoadRoutine(sceneName, onComplete));
        }

        private IEnumerator LoadRoutine(string sceneName, System.Action onComplete)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                Debug.Log($"Loading: {progress * 100f}%");
                yield return null;
            }

            onComplete?.Invoke();
        }

        private void OnEncounterLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name != sceneList.TestEncounter.name)
            {
                Debug.LogError($"{scene.name} is not loaded properly");
                return;
            }

            EncounterManager encounterManager = FindFirstObjectByType<EncounterManager>();
            if (encounterManager == null)
            {
                Debug.LogError($"{nameof(encounterManager)} is null");
                return;
            }
                    
            // TODO: Improve Encounters
            encounterManager.Initialize(this);
            encounterManager.StartEncounter(_encounterList.testEncounterDefinition);
        }
    }
}