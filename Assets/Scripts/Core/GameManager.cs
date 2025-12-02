using UnityEngine;

namespace FrostheimSaga.Core
{
    /// Main game manager - handles core game systems for Frostheim Saga
    /// Singleton pattern ensures only one instance exists
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Game State")]
        [SerializeField] private bool isGameInitialized = false;

        void Awake()
        {
            // Singleton pattern
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("[Frostheim Saga] GameManager initialized");
            }
            else
            {
                Debug.LogWarning("[Frostheim Saga] Duplicate GameManager destroyed");
                Destroy(gameObject);
            }
        }

        void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (isGameInitialized)
                return;

            Debug.Log("[Frostheim Saga] Initializing game systems...");

            // TODO: Initialize AI system
            // TODO: Initialize network system
            // TODO: Load player data

            isGameInitialized = true;
            Debug.Log("[Frostheim Saga] Game ready!");
        }

        public bool IsInitialized()
        {
            return isGameInitialized;
        }
    }
}
