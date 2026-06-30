using System.Collections;
using Game.Core.Save;
using Game.Data;
using Game.Systems.Audio;
using Game.Systems.Encounter;
using Game.Systems.Input;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    [RequireComponent(typeof(InputManager))]
    public class GameManager : MonoBehaviour
    {
        // Singleton instance (easy global access)
        public static GameManager Instance { get; private set; }
        
        [SerializeField] public AudioManager audioManager; 
        [SerializeField] public InputManager inputManager;
        
        public DisplayManager DisplayManager;
        public SaveManager SaveManager;
        
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
            DontDestroyOnLoad(gameObject);
            
            Validate.Assigned(audioManager);
            Validate.Assigned(inputManager);

            SaveManager = new SaveManager();
            DisplayManager = new DisplayManager(SaveManager.Data.settingsData);
            
            Addressables.LoadAssetAsync<EncounterList>(encounterListAssetReference)
                .Completed += handle =>
            {
                _encounterListAssetHandle = handle;
                _encounterList = handle.Result;
            };
        }
        
        private void OnEnable()
        {
            // audioManager.Initialize();
        }


        private void Start()
        {
            SetState(GameState.MainMenu);
        }
        
        private void OnDisable()
        {
            
        }
        
        private void OnDestroy()
        {
            DisplayManager?.Unbind();

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
            
            // TODO: Move to somewhere better or improve to load a previous session
            SaveManager.Load();
            
            if (_encounterList == null)
            {
                Debug.LogError($"{nameof(_encounterList)} is null and is not loaded properly");
                return;
            }
            
            // TODO: Remove use of test encounter encounter list and improve logic of encounter selection
            LoadScene(sceneList.testEncounterSceneName, () =>
                {
                    Debug.Log($"{sceneList.testEncounterSceneName} loaded");
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
            SceneManager.sceneLoaded -= OnEncounterLoad;
            
            if (scene.name != sceneList.testEncounterSceneName)
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
                    
            encounterManager.Initialize(this, SaveManager);
            encounterManager.StartEncounter(_encounterList.testEncounterDefinition);
        }

        public void EndGame()
        {
            GoToMainMenu();
        }

        public void GoToMainMenu()
        {
            SetState(GameState.MainMenu);
            
            LoadScene(sceneList.mainMenuSceneName, () =>
                {
                    Debug.Log($"{sceneList.mainMenuSceneName} loaded");
                });
        }

        public void StartGame(EncounterPromptCount promptCount)
        {
            _encounterList.testEncounterDefinition.maxPromptsPerEncounter = (int)promptCount;
            StartGame();
        }
    }
}