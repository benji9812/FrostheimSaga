using FrostheimSaga.Characters;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostheimSaga.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI characterText;

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            if (characterText == null)
            {
                Debug.LogError("[CHARACTER UI] characterText reference saknas.");
                return;
            }

            if (CharacterManager.Instance == null)
            {
                characterText.text = "Inget karaktärssystem tillgängligt.";
                return;
            }

            var characters = CharacterManager.Instance.Characters;
            if (characters == null || characters.Count == 0)
            {
                characterText.text = "Ingen karaktär genererad ännu.";
                return;
            }

            // Show only the latest character
            var c = characters[characters.Count - 1];

            var sb = new StringBuilder();
            sb.AppendLine($"<b><size=20>{c.Name}</size></b>");
            sb.AppendLine($"<size=14>{c.Race} {c.CharacterClass} - Level {c.Level}</size>");
            sb.AppendLine($"<size=12>Ålder: {c.Age}</size>\n");

            // Stats
            if (c.Attributes != null)
            {
                sb.AppendLine("<b>Attribut:</b>");
                sb.AppendLine($"STR: {c.Attributes.Strength}  DEX: {c.Attributes.Dexterity}  CON: {c.Attributes.Constitution}");
                sb.AppendLine($"INT: {c.Attributes.Intelligence}  WIS: {c.Attributes.Wisdom}  CHA: {c.Attributes.Charisma}");
                sb.AppendLine();
            }

            // Background
            if (!string.IsNullOrEmpty(c.Background))
            {
                sb.AppendLine("<b>Bakgrund:</b>");
                sb.AppendLine(c.Background);
                sb.AppendLine();
            }

            // Traits
            if (c.Traits != null && c.Traits.Count > 0)
            {
                sb.AppendLine("<b>Karaktärsdrag:</b>");
                sb.AppendLine(string.Join(", ", c.Traits));
                sb.AppendLine();
            }

            // Skills
            if (c.Skills != null && c.Skills.Count > 0)
            {
                sb.AppendLine("<b>Färdigheter:</b>");
                sb.AppendLine(string.Join(", ", c.Skills));
                sb.AppendLine();
            }

            // Equipment
            if (c.Equipment != null && c.Equipment.Count > 0)
            {
                sb.AppendLine("<b>Utrustning:</b>");
                foreach (var item in c.Equipment)
                    sb.AppendLine($"• {item}");
            }

            characterText.text = sb.ToString();
            
            // Force TMP to recalculate
            characterText.ForceMeshUpdate();

            // Set Content height to text height
            RectTransform contentRect = characterText.transform.parent.GetComponent<RectTransform>();
            if (contentRect != null)
            {
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, characterText.preferredHeight + 20);
            }
        }
    }
}
