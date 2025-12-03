using UnityEngine;
using FrostheimSaga.World;

namespace FrostheimSaga.World
{
    public class QuestDebugSpawner : MonoBehaviour
    {
        [SerializeField] private string biome = "snöigt fjällbiom nära Ekohamn";
        [SerializeField][Range(1, 5)] private int difficulty = 3;

        private async void Start()
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogError("[QUEST DEBUG] Ingen QuestManager i scenen.");
                return;
            }

            Debug.Log("[QUEST DEBUG] Genererar quest via QuestManager...");
            var quest = await QuestManager.Instance.GenerateAndAddQuestAsync(biome, difficulty);

            if (quest == null)
            {
                Debug.LogError("[QUEST DEBUG] Misslyckades generera quest.");
                return;
            }

            Debug.Log(
                $"[QUEST DEBUG]\n" +
                $"Titel: {quest.Title}\n" +
                $"Biome: {quest.Biome} (Svårighet {quest.Difficulty})\n\n" +
                $"Beskrivning:\n{quest.Description}\n\n" +
                $"Mål:\n - {string.Join("\n - ", quest.Goals ?? new System.Collections.Generic.List<string>())}\n\n" +
                $"Belöningar:\n - {string.Join("\n - ", quest.Rewards ?? new System.Collections.Generic.List<string>())}"
            );
        }
    }
}
