using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Quests
{
    public class BreeStayQuest : QuestBase
    {
        public override string Name => "Stay";

        public override QuestData GetQuestData => new BreeStayQuestData();

        public override string Description(QuestData data)
        {
            return "Let's try to convince Bree to stay.";
        }

        public override string QuestStory(QuestData data)
        {
            string Story = "";
            return Story;
        }

        public override bool IsQuestStarted(QuestData data)
        {
            return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Bree);
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return base.IsQuestComplete(data);
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (tg.ID == GuardianBase.Bree && tg.ModID == MainMod.mod.Name)
            {
                BreeStayQuestData data = (BreeStayQuestData)rawdata;
                if (data.QuestStep == 0)
                {

                }
            }
            return base.ImportantDialogueMessage(rawdata, tg, GuardianID, GuardianModID);
        }

        public class BreeStayQuestData : QuestData
        {
            public byte QuestStep = 0;
        }
    }
}
