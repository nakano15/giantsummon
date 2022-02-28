using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Quests
{
    public class BlueTravellerOutfitQuest : QuestBase
    {
        public override string Name => ""; //Can't make a quest without a plot. I need to think how this will work.

        public override string Description(QuestData data)
        {
            return "Ignite the adventuring spirit of Blue.";
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            return base.GetQuestCurrentObjective(data);
        }

        public override string QuestStory(QuestData data)
        {
            string Story = "";
            return Story;
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return false;
        }

        public override bool IsQuestStarted(QuestData data)
        {
            return false;
        }

        public override void UpdatePlayer(Player player, QuestData data)
        {

        }

        public override QuestData GetQuestData => new TravellerQuestData();

        public class TravellerQuestData : QuestData
        {

        }
    }
}
