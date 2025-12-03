using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FrostheimSaga.World;
using System.Collections.Generic;

namespace FrostheimSaga.AI
{
    /// Client for interacting with Google Gemini API.
    /// Handles generic text generation that vi sedan kan bygga quests, karaktärer osv på.
    public class GeminiAPIClient : MonoBehaviour
    {
        private string apiKey;
        private const string ApiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        private static readonly HttpClient httpClient = new HttpClient();

        private void Awake()
        {
            LoadApiKey();
        }
        
        /// Läser API-nyckeln från Resources/Config/apikey.txt
        private void LoadApiKey()
        {
            try
            {
                TextAsset apiKeyFile = Resources.Load<TextAsset>("Config/apikey");
                if (apiKeyFile == null)
                {
                    Debug.LogError("[AI] apikey.txt saknas i Assets/Resources/Config/");
                    return;
                }

                apiKey = apiKeyFile.text.Trim();
                if (string.IsNullOrEmpty(apiKey))
                {
                    Debug.LogError("[AI] API-nyckeln i apikey.txt är tom.");
                }
                else
                {
                    Debug.Log("[AI] API-nyckel inläst.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AI] Kunde inte läsa API-nyckel: {e.Message}");
            }
        }

        /// Genererar text från Gemini utifrån en prompt.
        public async Task<string> GenerateText(string prompt)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("[AI] Ingen API-nyckel tillgänglig.");
                return null;
            }

            try
            {
                var body = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = $"{ApiUrl}?key={apiKey}";
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                string responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"[AI] API-fel: {response.StatusCode} - {responseText}");
                    return null;
                }

                return ParseResponse(responseText);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AI] Undantag vid anrop: {e.Message}");
                return null;
            }
        }

        /// Genererar en quest som JSON och mappar till Quest-objekt.
        public async Task<Quest> GenerateQuestAsync(string biome, int difficulty)
        {
            string prompt =
                $"Du är en spel-designer. Skapa en quest i världen Frostheim Saga i biomet \"{biome}\" " +
                $"med svårighetsgrad {difficulty} (1=lätt, 5=svår). " +
                "Svara ENDAST som strikt JSON i följande format, utan extra text runt omkring:\n\n" +
                "{\n" +
                "  \"quest\": {\n" +
                "    \"Title\": \"...\",\n" +
                "    \"Description\": \"...\",\n" +
                "    \"Goals\": [\"...\", \"...\"],\n" +
                "    \"Rewards\": [\"...\", \"...\"],\n" +
                "    \"Biome\": \"...\",\n" +
                "    \"Difficulty\": 3\n" +
                "  }\n" +
                "}";

            string jsonText = await GenerateText(prompt);

            if (string.IsNullOrEmpty(jsonText))
            {
                Debug.LogError("[AI QUEST] Tomt svar från Gemini.");
                return null;
            }

            try
            {
                // Ta bort eventuellt skräp runt JSON, t.ex. ```
                jsonText = jsonText.Trim();

                // Om svaret börjar med ``` så försöker vi plocka ut innehållet mellan ```
                if (jsonText.StartsWith("```"))
                {
                    // Ta bort första raden (``````)
                    int firstNewLine = jsonText.IndexOf('\n');
                    if (firstNewLine >= 0)
                    {
                        jsonText = jsonText.Substring(firstNewLine + 1);
                    }

                    // Ta bort avslutande ```
                    int lastFence = jsonText.LastIndexOf("```", StringComparison.Ordinal);
                    if (lastFence >= 0)
                    {
                        jsonText = jsonText.Substring(0, lastFence);
                    }

                    jsonText = jsonText.Trim();
                }

                QuestResponse response = JsonConvert.DeserializeObject<QuestResponse>(jsonText);
                if (response?.quest == null)
                {
                    Debug.LogError("[AI QUEST] Kunde inte parsa quest från JSON.");
                    return null;
                }

                // Sätt biome/svårighet om modellen strulat
                response.quest.Biome ??= biome;
                if (response.quest.Difficulty <= 0) response.quest.Difficulty = difficulty;

                Debug.Log($"[AI QUEST] Genererad quest: {response.quest.Title}");
                return response.quest;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AI QUEST] JSON-fel: {e.Message}\nRått svar:\n{jsonText}");
                return null;
            }
        }
        /// Parserar svaret från Gemini för att extrahera texten.
        private string ParseResponse(string jsonResponse)
        {
            try
            {
                JObject obj = JObject.Parse(jsonResponse);
                string text = obj["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

                if (string.IsNullOrEmpty(text))
                {
                    Debug.LogWarning("[AI] Svar saknar text.");
                    return null;
                }

                return text;
            }
            catch (Exception e)
            {
                Debug.LogError($"[AI] Kunde inte tolka svar: {e.Message}");
                return null;
            }
        }

        /// Enkel testmetod som kollar att allt fungerar.
        public async Task TestConnection()
        {
            string prompt = "Skapa en kort quest-hook i världen Frostheim Saga med titel, beskrivning och mål i punktlista. Svara på svenska.";
            string result = await GenerateText(prompt);

            if (!string.IsNullOrEmpty(result))
            {
                Debug.Log($"[AI TEST] Svar från Gemini:\n{result}");
            }
            else
            {
                Debug.LogError("[AI TEST] Inget svar från Gemini.");
            }
        }
    }
}
