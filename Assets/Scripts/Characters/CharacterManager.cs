using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using FrostheimSaga.AI;

namespace FrostheimSaga.Characters
{
    /// Manages all characters (player and NPCs) in the game.
    /// Handles AI generation and storage.
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager Instance { get; private set; }

        [Header("Characters")]
        [SerializeField] private List<Character> characters = new List<Character>();

        private GeminiAPIClient aiClient;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                aiClient = GetComponent<GeminiAPIClient>();
                if (aiClient == null)
                {
                    Debug.LogError("[CHARACTER] Hittar inte GeminiAPIClient på samma GameObject.");
                }
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public IReadOnlyList<Character> Characters => characters;

        /// Generate a new character via AI and add to list.
        public async Task<Character> GenerateAndAddCharacterAsync(string race, string characterClass)
        {
            if (aiClient == null)
            {
                Debug.LogError("[CHARACTER] Ingen AI-klient tillgänglig.");
                return null;
            }

            Character character = await aiClient.GenerateCharacterAsync(race, characterClass);
            if (character == null)
            {
                return null;
            }

            characters.Add(character);
            Debug.Log($"[CHARACTER] Lagt till karaktär: {character.Name}");
            return character;
        }
    }
}
