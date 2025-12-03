using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using FrostheimSaga.AI;

namespace FrostheimSaga.World
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance { get; private set; }

        [Header("Active Quests")]
        [SerializeField] private List<Quest> activeQuests = new List<Quest>();

        private GeminiAPIClient aiClient;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                // Initiera AI-klienten direkt i Awake
                aiClient = GetComponent<GeminiAPIClient>();
                if (aiClient == null)
                {
                    Debug.LogError("[QUEST] Hittar inte GeminiAPIClient på samma GameObject.");
                }
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        public IReadOnlyList<Quest> ActiveQuests => activeQuests;

        public async Task<Quest> GenerateAndAddQuestAsync(string biome, int difficulty)
        {
            if (aiClient == null)
            {
                Debug.LogError("[QUEST] Ingen AI‑klient tillgänglig.");
                return null;
            }

            Quest quest = await aiClient.GenerateQuestAsync(biome, difficulty);
            if (quest == null)
            {
                return null;
            }

            activeQuests.Add(quest);
            Debug.Log($"[QUEST] Lagt till ny quest: {quest.Title}");
            return quest;
        }
    }
}
