using UnityEngine;
using FrostheimSaga.AI;
using FrostheimSaga.World;

namespace FrostheimSaga.AI
{
    public class AITestRunner : MonoBehaviour
    {
        private GeminiAPIClient client;

        private async void Start()
        {
            client = GetComponent<GeminiAPIClient>();
            if (client == null)
            {
                Debug.LogError("[AI TEST] Hittar inte GeminiAPIClient på samma GameObject.");
                return;
            }

            Debug.Log("[AI TEST] Genererar quest...");

            // Exempel: snö-biom, medelsvår quest
            Quest quest = await client.GenerateQuestAsync("snöigt fjällbiom nära Ekohamn", 3);

            if (quest == null)
            {
                Debug.LogError("[AI TEST] Kunde inte generera quest.");
                return;
            }

            // Visa snyggt i Console
            Debug.Log(
                $"[QUEST]\n" +
                $"Titel: {quest.Title}\n" +
                $"Biome: {quest.Biome} (Svårighet {quest.Difficulty})\n\n" +
                $"Beskrivning:\n{quest.Description}\n\n" +
                $"Mål:\n - {string.Join("\n - ", quest.Goals ?? new System.Collections.Generic.List<string>())}\n\n" +
                $"Belöningar:\n - {string.Join("\n - ", quest.Rewards ?? new System.Collections.Generic.List<string>())}"
            );
        }
    }
}
