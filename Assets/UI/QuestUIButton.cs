using UnityEngine;
using UnityEngine.UI;
using FrostheimSaga.World;

namespace FrostheimSaga.UI
{
    [RequireComponent(typeof(Button))]
    public class QuestUIButton : MonoBehaviour
    {
        [SerializeField] private string biome = "snöigt fjällbiom nära Ekohamn";
        [SerializeField][Range(1, 5)] private int difficulty = 3;
        [SerializeField] private QuestUIController questUI;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        private async void OnClicked()
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogError("[QUEST UI] Ingen QuestManager.");
                return;
            }

            Debug.Log("[QUEST UI] Genererar ny quest via knapp...");

            var quest = await QuestManager.Instance.GenerateAndAddQuestAsync(biome, difficulty);
            if (quest == null)
            {
                Debug.LogError("[QUEST UI] Misslyckades generera quest.");
                return;
            }

            if (questUI != null)
            {
                questUI.RefreshUI();
            }
        }
    }
}
