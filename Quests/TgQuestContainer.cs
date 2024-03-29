﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Quests
{
    public class TgQuestContainer : QuestContainer
    {
        public const int ZacksMeatbagOutfitQuest = 0, ShatteredQuest = 1, MissingQuest = 2, KnowYouInsideQuest = 3, TutorialQuest = 4, BreeStayQuest = 5, SardineOutfitQuest = 6;

        public override void CreateQuestDB()
        {
            AddQuest(ZacksMeatbagOutfitQuest, new ZacksMeatBagOutfitQuest());
            AddQuest(ShatteredQuest, new PigCompanionsQuest());
            AddQuest(MissingQuest, new BlueSeekingZacksQuest());
            AddQuest(KnowYouInsideQuest, new GreenHealingUnlockQuest());
            AddQuest(TutorialQuest, new TutorialQuest());
            AddQuest(BreeStayQuest, new BreeStayQuest());
            AddQuest(SardineOutfitQuest, new SardineOutfitQuest());
        }
    }
}
