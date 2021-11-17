using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Quests
{
    public class TgQuestContainer : QuestContainer
    {
        public const int ZacksMeatbagOutfitQuest = 0, ShatteredQuest = 1;

        public override void CreateQuestDB()
        {
            AddQuest(ZacksMeatbagOutfitQuest, new ZacksMeatBagOutfitQuest());
            AddQuest(ShatteredQuest, new PigCompanionsQuest());
        }
    }
}
