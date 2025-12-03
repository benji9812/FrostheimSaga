using System.Collections.Generic;

namespace FrostheimSaga.World
{
    [System.Serializable]
    public class Quest
    {
        public string Title;
        public string Description;
        public List<string> Goals;
        public List<string> Rewards;
        public string Biome;
        public int Difficulty;
    }

    // Hjälpklass för att parsa JSON-svar
    [System.Serializable]
    public class QuestResponse
    {
        public Quest quest;
    }
}
