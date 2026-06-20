using UnityEngine;

public class SampleScript : MonoBehaviour
{
    /*
      FIELD ORDER (Unity Convention)

      1. Constants / Static Readonly
         - const / static readonly values shared across instances

      2. Serialized Fields (Inspector)
         - [SerializeField] private fields
         - Group with [Header] if needed

      3. Public API Fields / Properties
         - Public properties exposed to other classes
         - Keep logic minimal

      4. Events
         - public event Action / delegates

      5. Private Runtime State
         - Core gameplay/state variables

      6. Cached Components / References
         - GetComponent results, UI refs, transforms

      7. Temporary / Helper Fields (rare)
         - Only if needed for performance or reuse
   */
    
    /*
      METHOD ORDER (Unity Convention)

      1. Unity Lifecycle Methods
         - Awake
         - OnEnable
         - Start
         - Update / LateUpdate / FixedUpdate
         - OnDisable
         - OnDestroy

      2. Public API Methods
         - Methods called by other classes

      3. Interface Implementations
         - e.g. IDisposable, custom interfaces

      4. Event Handlers / Callbacks
         - Input events, subscriptions, Unity events

      5. Initialization / Setup Methods
         - Cache(), Bind(), Initialize()

      6. Core Logic Methods
         - Main gameplay / system logic

      7. Helper / Utility Methods
         - Private small reusable functions
   */
          
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
