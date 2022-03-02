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

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            TravellerQuestData data = (TravellerQuestData)rawdata;
            switch (data.QuestStep)
            {
                case 0:
                    if (GuardianID == GuardianBase.Zacks && GuardianModID == MainMod.mod.Name && NpcMod.HasGuardianNPC(GuardianBase.Blue) && !PlayerMod.IsQuestActive(Main.LocalPlayer, 0))
                    {
                        return ZacksSpeakingAboutBlue;
                    }
                    break;
            }
            return base.ImportantDialogueMessage(data, tg, GuardianID, GuardianModID);
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            TravellerQuestData data = (TravellerQuestData)Data;
            switch (data.QuestStep)
            {
                case 1:
                    if (GuardianID == GuardianBase.Zacks && GuardianModID == MainMod.mod.Name)
                    {
                        List<DialogueOption> d = base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
                        d.Add(new DialogueOption("What you wanted to ask me about?", ZacksSpeakingAboutBluePlayerAskedLater, true));
                        return d;
                    }
                    break;
                case 2:
                    if (GuardianID == GuardianBase.Blue && GuardianModID == MainMod.mod.Name)
                    {
                        List<DialogueOption> d = base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
                        d.Add(new DialogueOption("I need to talk to you.", SpeakingToBlueAfterTalkingToZacks, true));
                        return d;
                    }
                    break;
            }
            return base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
        }

        private void ZacksSpeakingAboutBlue()
        {
            TravellerQuestData data = (TravellerQuestData)Data;
            if(Dialogue.ShowDialogueWithOptions("*I have something I have to ask of you, [nickname].*", new string[] { "What is it?", "I can't right now." }) == 1)
            {
                data.QuestStep = 1;
                Dialogue.ShowEndDialogueMessage("*So, speak to me when possible, It's about Blue.*", false);
                return;
            }
            ZacksRequestStep();
        }

        private void ZacksSpeakingAboutBluePlayerAskedLater()
        {
            Dialogue.ShowDialogueWithContinue("*Oh, you finally got time to hear my request. Better I not delay anymore, then.*");
            ZacksRequestStep();
        }

        private void ZacksRequestStep()
        {
            TravellerQuestData data = (TravellerQuestData)Data;
            Dialogue.ShowDialogueWithContinue("*I am worried about Blue. Ever since I returned, she's been taking care of me.*", ContinueText: "And what is the problem?");
            Dialogue.ShowDialogueWithContinue("*She always liked entering in new adventures, but it feels like I'm locking her at home.*", ContinueText: "You want me to take her on an adventure?");
            Dialogue.ShowDialogueWithContinue("*You're mostly always exploring the world, so I think that's going to help.*", ContinueText: "What about you?");
            Dialogue.ShowDialogueWithContinue("*Don't worry much about me. Unless a bloodmoon happens, I can deal with my hunger issue.*", ContinueText: "I'll see to that then.");
            data.QuestStep = 2;
            Dialogue.ShowEndDialogueMessage("*Thanks [nickname]. Is there something else you want to talk to me?*", false);
        }

        private void SpeakingToBlueAfterTalkingToZacks()
        {
            TravellerQuestData data = (TravellerQuestData)Data;

        }

        public override QuestData GetQuestData => new TravellerQuestData();

        public class TravellerQuestData : QuestData
        {
            /// <summary>
            /// 0 = Zacks trigger the quest.
            /// 1 = Zacks said he has a request, but player said they're busy.
            /// 2 = Zacks told his request, now you need to speak with Blue.
            /// 3 = Take blue on your quests, and look for a relic.
            /// </summary>
            public byte QuestStep = 0;
        }
    }
}
