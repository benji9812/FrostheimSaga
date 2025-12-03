using FrostheimSaga.World;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostheimSaga.UI
{
    public class QuestUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI questText;

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (questText == null)
            {
                Debug.LogError("[QUEST UI] questText reference saknas.");
                return;
            }

            if (QuestManager.Instance == null)
            {
                questText.text = "Inga questsystem tillgängliga.";
                return;
            }

            var quests = QuestManager.Instance.ActiveQuests;
            if (quests == null || quests.Count == 0)
            {
                questText.text = "Inga aktiva quests ännu.";
                return;
            }

            // Visa bara senaste questen
            var q = quests[quests.Count - 1];

            var sb = new StringBuilder();
            sb.AppendLine($"<b>{q.Title}</b>");
            sb.AppendLine($"<size=12>{q.Biome}  |  Svårighet: {q.Difficulty}</size>\n");
            sb.AppendLine(q.Description);
            sb.AppendLine();

            if (q.Goals != null && q.Goals.Count > 0)
            {
                sb.AppendLine("<b>Mål:</b>");
                foreach (var goal in q.Goals)
                    sb.AppendLine($"• {goal}");
                sb.AppendLine();
            }

            if (q.Rewards != null && q.Rewards.Count > 0)
            {
                sb.AppendLine("<b>Belöningar:</b>");
                foreach (var reward in q.Rewards)
                    sb.AppendLine($"• {reward}");
            }

            questText.text = sb.ToString();

            // Force TMP to recalculate
            questText.ForceMeshUpdate();

            // Set Content height to text height
            RectTransform contentRect = questText.transform.parent.GetComponent<RectTransform>();
            if (contentRect != null)
            {
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, questText.preferredHeight + 20);
            }
        }
    }
}
