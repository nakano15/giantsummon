using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class GreenHealingUnlockQuest : QuestBase
    {
        public override string Name => "Knowing You Inside";

        public override string Description(QuestData data)
        {
            return "Green needs your help to acquire knowledge about \nTerrarians body.";
        }

        public override string QuestStory(QuestData rawdata)
        {
            GreenQuestData data = (GreenQuestData)rawdata;
            string Story = "";
            if (data.QuestStep > 0)
                Story += "Green told me that he need to read books about medicine and Terrarian anathomy, so he could begin treating Terrarians with reduced mistake chances.";
            if (data.QuestStep > 1)
                Story += "\n\nI've brought him quite a number of books, which a few he found out to be about what he wants to know.";
            if (data.QuestStep > 3)
                Story += "Once I brought all the books he asked for, he told me that I needed to wait some days until he finished reading them all.";
            if (GuardianGlobalInfos.UnlockedGreensHealing)
            {
                if (data.QuestStep < 5)
                {
                    if (Story.Length > 0)
                        Story += "\n\n";
                    Story += "Due to the help of another Terrarian, Green can now treat Terrarian patients anywhere he goes.\n\nTHE END";
                }
                else
                {
                    Story += "\n\nAfter about 8 days have passed, I spoke to Green again. He told me that now he has better knowledge of the Terrarians body, diseases and known treatments, and can now begin treating Terrarian patients wherever he goes, and for a fair fee.\n\nTHE END";
                }
            }
            return Story;
        }

        public override bool IsQuestStarted(QuestData rawdata)
        {
            GreenQuestData data = (GreenQuestData)rawdata;
            return GuardianGlobalInfos.UnlockedGreensHealing || data.QuestStep > 0;
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            GreenQuestData data = (GreenQuestData)rawdata;
            switch (data.QuestStep)
            {
                case 0:
                    return "Talk to Green about what he needs.";
                case 1:
                case 2:
                case 3:
                    return "Bring books to Green.";
                case 4:
                    return "Wait until Green finishes reading the books.";
            }
            return base.GetQuestCurrentObjective(data);
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (GuardianID == GuardianBase.Green && GuardianModID == MainMod.mod.Name && !GuardianGlobalInfos.UnlockedGreensHealing)
            {
                GreenQuestData data = (GreenQuestData)rawdata;
                if(data.QuestStep == 4 && data.TimePassed <= 0)
                {
                    return PostReadingStageDialogue;
                }
            }
            return base.ImportantDialogueMessage(rawdata, tg, GuardianID, GuardianModID);
        }

        public override void UpdatePlayer(Player player, QuestData rawdata)
        {
            if (IsQuestComplete(rawdata))
                return;
            GreenQuestData data = (GreenQuestData)rawdata;
            if(data.QuestStep == 4 && data.TimePassed > 0)
            {
                data.TimePassed -= Main.dayRate;
                if (data.TimePassed < 0)
                    data.TimePassed = 0;
            }
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            GreenQuestData data = (GreenQuestData)Data;
            List<DialogueOption> Options = new List<DialogueOption>();
            if (GuardianID == GuardianBase.Green && GuardianModID == MainMod.mod.Name && !GuardianGlobalInfos.UnlockedGreensHealing)
            {
                if (data.QuestStep == 0)
                {
                    Options.Add(new DialogueOption("What do you need to begin medicating Terrarians?", QuestBrief, true));
                }
                else if(data.QuestStep < 4)
                {
                    Options.Add(new DialogueOption("About the books you asked...", BookCollectionQuestStage, true));
                }
                else if (data.QuestStep == 4)
                {
                    Options.Add(new DialogueOption("Have you finished reading the books?", ReadingStageDialogue, true));
                }
            }
            return Options;
        }

        private void QuestBrief()
        {
            Dialogue.ShowDialogueWithContinue("*I need books about medicine and anathomy of Terrarians, so I can know how your bodies works. If you could bring me some of them, I can check if is what I need.*", ContinueText: "Medicine and Anathomy Books?");
            Dialogue.ShowDialogueWithContinue("*Yes. I need to know more about how your bodies works, diseases you can get and how to treat them, so I can ensure that my treatment will be efficient.*", ContinueText: "I think I saw some books on the Dungeon.");
            Dialogue.ShowDialogueWithContinue("*The creepy catacombs far in this world? You think the books there will be useful? Collect a number of them and bring them to me, so I can check them.*", ContinueText: "Okay");
            GreenQuestData data = (GreenQuestData)Data;
            data.QuestStep = 1;
            Dialogue.ShowEndDialogueMessage("*Do you want to talk about something else?*", false);
        }

        private void BookCollectionQuestStage()
        {
            const int BookID = Terraria.ID.ItemID.Book;
            string[] Options = new string[] { "", "Where can I find books?", "Alright" };
            string LobbyMessage = "*Yes? What do you need to know?*";
            while (true)
            {
                if (Main.LocalPlayer.HasItem(BookID))
                {
                    Options[0] = "Is this book useful?";
                }
                else
                {
                    Options[0] = "";
                }
                switch (Dialogue.ShowDialogueWithOptions(LobbyMessage, Options))
                {
                    case 0:
                        Dialogue.ShowDialogueWithContinue("*Let me check it.*");
                        for(int p = 0; p < 50; p++)
                        {
                            Item i = Main.LocalPlayer.inventory[p];
                            if (i.type == BookID)
                            {
                                i.stack--;
                                if (i.stack <= 0)
                                    i.SetDefaults(0);
                            }
                        }
                        if (Main.rand.NextDouble() < 1f / 20)
                        {
                            GreenQuestData data = (GreenQuestData)Data;
                            switch (data.QuestStep)
                            {
                                case 2:
                                    LobbyMessage = "*Perfect! This book talks about diseases and treatments. I still need some more books.*";
                                    break;
                                case 3:
                                    LobbyMessage = "*This is an anathomy book. Now I can know the location of your organs and more. Please look for more books.*";
                                    break;
                                case 4:
                                    data.QuestStep = 4;
                                    data.TimePassed = 8 * 3600 * 24;
                                    Dialogue.ShowDialogueWithContinue("*This is a medicine book. The information in it may help. I think that's all I need.*", ContinueText: "Are those books really enough?");
                                    Dialogue.ShowDialogueWithContinue("*I think they are. I will still need some time to read them, so check back after some days have passed.*", ContinueText: "So, I have to check you back in a number of days?");
                                    Dialogue.ShowEndDialogueMessage("*Yes. I will tell you when I finish reading the books. Until then, is there any other way I can help you with?*", false);
                                    return;
                            }
                            data.QuestStep++;
                        }
                        else
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    LobbyMessage = "*Beside Terrarian story books actually look interesting, this isn't what I'm looking for.*";
                                    break;
                                case 1:
                                    LobbyMessage = "*This book is filled with gibberish. Who writes a book that doesn't makes sense?*";
                                    break;
                                case 2:
                                    LobbyMessage = "*This book has several images of female Terrarians in underwear. That wont help me much related to Terrarian anathomy. Or at least for finding out internal organs placement.*";
                                    break;
                                case 3:
                                    LobbyMessage = "*This seems like a geography book. What was it doing in a dungeon?*";
                                    break;
                                case 4:
                                    LobbyMessage = "*There's a book talking about godly humanoid creatures. Why does this sounds so familiar?*";
                                    break;
                            }
                        }
                        break;

                    case 1:
                        Dialogue.ShowDialogueWithContinue("*I think I remember that you said the Dungeon contained books.*");
                        LobbyMessage = "*If what you said was right, then you should check the Dungeon.*";
                        break;

                    case 2:
                        Dialogue.ShowEndDialogueMessage("*Do you need something else?*", false);
                        return;
                }
            }
        }

        private void ReadingStageDialogue()
        {
            Dialogue.ShowEndDialogueMessage("*Not yet. I still have more reading to do. But I seems to be getting a clearer understanding of how Terrarian bodies works, and where the organs are in the system.*", false);
        }

        private void PostReadingStageDialogue()
        {
            GreenQuestData data = (GreenQuestData)Data;
            data.QuestStep = 5;
            GuardianGlobalInfos.UnlockedGreensHealing = true;
            Dialogue.ShowDialogueWithContinue("*[nickname], I have finished reading all those books.*", ContinueText: "And?");
            Dialogue.ShowDialogueWithContinue("*Beside not everything I should know about Terrarians is contained in them, I think it's enough knowledge for me to try medicating Terrarians.*", ContinueText: "That means you can medicate us?");
            Dialogue.ShowDialogueWithContinue("*Exactly. Before you arrived, I contacted some travelling merchant to get the supplies I need. Gladly the first batch arrived and I can begin right away. Of course, I will charge some fee out of this, but I'll try making it fair.*", ContinueText: "Alright.");
            Dialogue.ShowEndDialogueMessage("*That's all I had to say, do you want to speak about something else?*", false);
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return GuardianGlobalInfos.UnlockedGreensHealing;
        }

        public override QuestData GetQuestData => new GreenQuestData();

        public class GreenQuestData : QuestData
        {
            public byte QuestStep = 0;
            public float TimePassed = 0;
            private const int QuestVersion = 1;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add("Version", QuestVersion);
                Writer.Add("Step", QuestStep);
                Writer.Add("Time", TimePassed);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                int Version = Reader.GetInt("Version");
                if(Version > 0)
                {
                    QuestStep = Reader.GetByte("Step");
                    TimePassed = Reader.GetFloat("Time");
                }
            }
        }
    }
}
