using UnityEngine;
using FrostheimSaga.Characters;

namespace FrostheimSaga.Characters
{
    public class CharacterDebugSpawner : MonoBehaviour
    {
        [SerializeField] private string race = "Människa";
        [SerializeField] private string characterClass = "Krigare";

        private async void Start()
        {
            if (CharacterManager.Instance == null)
            {
                Debug.LogError("[CHARACTER DEBUG] Ingen CharacterManager i scenen.");
                return;
            }

            Debug.Log("[CHARACTER DEBUG] Genererar karaktär...");
            var character = await CharacterManager.Instance.GenerateAndAddCharacterAsync(race, characterClass);

            if (character == null)
            {
                Debug.LogError("[CHARACTER DEBUG] Misslyckades generera karaktär.");
                return;
            }

            // Display character info
            Debug.Log(
                $"[CHARACTER DEBUG]\n" +
                $"Namn: {character.Name}\n" +
                $"Ålder: {character.Age}\n" +
                $"Ras: {character.Race}\n" +
                $"Klass: {character.CharacterClass}\n" +
                $"Level: {character.Level}\n\n" +
                $"Stats:\n" +
                $"  STR: {character.Attributes?.Strength ?? 0}\n" +
                $"  DEX: {character.Attributes?.Dexterity ?? 0}\n" +
                $"  CON: {character.Attributes?.Constitution ?? 0}\n" +
                $"  INT: {character.Attributes?.Intelligence ?? 0}\n" +
                $"  WIS: {character.Attributes?.Wisdom ?? 0}\n" +
                $"  CHA: {character.Attributes?.Charisma ?? 0}\n\n" +
                $"Bakgrund: {character.Background}\n\n" +
                $"Traits: {string.Join(", ", character.Traits ?? new System.Collections.Generic.List<string>())}\n" +
                $"Skills: {string.Join(", ", character.Skills ?? new System.Collections.Generic.List<string>())}\n" +
                $"Utrustning: {string.Join(", ", character.Equipment ?? new System.Collections.Generic.List<string>())}"
            );
        }
    }
}
