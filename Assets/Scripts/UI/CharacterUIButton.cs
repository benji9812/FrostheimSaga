using UnityEngine;
using UnityEngine.UI;
using FrostheimSaga.Characters;

namespace FrostheimSaga.UI
{
    [RequireComponent(typeof(Button))]
    public class CharacterUIButton : MonoBehaviour
    {
        [SerializeField] private string race = "Människa";
        [SerializeField] private string characterClass = "Krigare";
        [SerializeField] private CharacterUIController characterUI;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        private async void OnClicked()
        {
            if (CharacterManager.Instance == null)
            {
                Debug.LogError("[CHARACTER UI] Ingen CharacterManager.");
                return;
            }

            Debug.Log("[CHARACTER UI] Genererar ny karaktär via knapp...");

            var character = await CharacterManager.Instance.GenerateAndAddCharacterAsync(race, characterClass);
            if (character == null)
            {
                Debug.LogError("[CHARACTER UI] Misslyckades generera karaktär.");
                return;
            }

            if (characterUI != null)
            {
                characterUI.RefreshUI();
            }
        }
    }
}
