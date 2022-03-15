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
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            if (tg.ID == GuardianBase.Bree && tg.ModID == MainMod.mod.Name)
            {
                switch (data.QuestStep)
                {
                    case 0:
                        if(PlayerMod.GetPlayerGuardianFriendshipLevel(Main.LocalPlayer, GuardianBase.Bree) >= 2)
                            return BreeTellsAboutLeaving;
                        break;
                }
            }
            return base.ImportantDialogueMessage(data, tg, GuardianID, GuardianModID);
        }

        public static void BreeTellsAboutLeaving()
        {
            Dialogue.ShowDialogueWithContinue("[nickname], we have to talk.", ContinueText: "What is it?");
            if (!PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Sardine))
            {
                Dialogue.ShowDialogueWithContinue("I'm sorry for not hanging around here for much longer, but I need to look for my husband.");
                Dialogue.ShowDialogueWithContinue("I may actually leave some times to look for him.");
                Dialogue.ShowDialogueWithContinue("You seems to be trying your best to find him, but I wont be able to help if I idle around.");
                Dialogue.ShowDialogueWithContinue("I will be going to look for him too.");
            }
            else if (!PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Glenn))
            {
                Dialogue.ShowDialogueWithContinue("Look, I'm thankful that you found my husband, but I still need to find my house.");
                Dialogue.ShowDialogueWithContinue("I left my son all alone there taking care of the house while we're away.");
                Dialogue.ShowDialogueWithContinue("He's able to take care of himself for a while, but it has been really long since I left.");
                Dialogue.ShowDialogueWithContinue("That's why sometimes you might not find me, is because I will be looking for my house, and see my son.");
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("I'm really glad that both my husband and my son are safe, but we still have a house, which I should look for.");
                Dialogue.ShowDialogueWithContinue("Having a house here with people around is one thing, but we already have one.");
                Dialogue.ShowDialogueWithContinue("So, if you look for me and I'm not around, is because I left to look for my house.");
            }
            BreeStayQuestData data = (BreeStayQuestData)Data;
            data.QuestStep = 1;
            Dialogue.ShowEndDialogueMessage("Now that you know it, do you have anything else you want to talk to me?", false);
        } 

        public class BreeStayQuestData : QuestData
        {
            public byte QuestStep = 0;
        }
    }
}
