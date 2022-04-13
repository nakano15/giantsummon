using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class BreeStayQuest : QuestBase
    {
        public override string Name => "Stay";

        public override QuestData GetQuestData => new BreeStayQuestData();
        public const short FishID = Terraria.ID.ItemID.VariegatedLardfish;
        public const byte FishCount = 15;
        public const string FishName = "Variegated Lardfish";

        public override string Description(QuestData data)
        {
            return "Let's try to convince Bree to stay.";
        }

        public override string QuestStory(QuestData rawdata)
        {
            string Story = "";
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            if(data.QuestStep == 0)
            {
                Story = "Bree shall talk to you about this when you speak to her, depending on your friendship level with her.";
            }
            else
            {
                Story = "Bree told me that many people in this world are asking her to stay. She seems to be really annoyed by that, and said that if I want her to stay in my world, I should bring her "+FishCount+" " + MainMod.PluralifyWord(FishName, FishCount) + ".";
            }
            if(data.QuestStep >= 2)
            {
                Story += "\n\nI brought Bree the fishs she asked for, and she looked really happy for that. ";
                switch (data.ConclusionState)
                {
                    case 0:
                        Story += "She then suddenly got saddened because she ate them alone, while her husband and son were nowhere to be found.";
                        break;
                    case 1:
                        Story += "She uses the fishs to dine alongside her husband, but she was partially unhappy because she couldn't have her son to dine with them, too.";
                        break;
                    case 2:
                        Story += "She uses the fishs to dine alongside her son, but her worry about wether her husband is safe or not, kept her from enjoying her dinner.";
                        break;
                    case 3:
                        Story += "She uses the fishs to dine alongside her husband and son, and they all enjoyed dinning together again.";
                        break;
                }
                if (data.QuestStep == 3)
                {
                    Story += "\n\nLater during that day, Bree untied the bag off her neck, and stored all her belongings on the house, she now calls hers.";
                    Story += "\n\nTHE END";
                }
            }
            return Story;
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            byte CurFishCount = (byte)(FishCount - data.FishGiven);
            return "Give " + CurFishCount + " " + MainMod.PluralifyWord(FishName, CurFishCount) + " to Bree, so you convince her to stay.";
        }

        public override bool IsQuestStarted(QuestData rawdata)
        {
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Bree) && data.QuestStep > 0;
        }

        public override bool IsQuestComplete(QuestData rawdata)
        {
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            return data.QuestStep == 3;
        }

        public static bool IsThisQuestCompleted()
        {
            return PlayerMod.GetQuestData(Main.LocalPlayer, Quests.TgQuestContainer.BreeStayQuest).IsQuestCompleted;
        }

        public override void UpdatePlayer(Player player, QuestData rawdata)
        {
            BreeStayQuestData data = (BreeStayQuestData)rawdata;
            if(data.QuestStep == 2)
            {
                if (!Main.dayTime && Main.time >= 1800 && NpcMod.HasGuardianNPC(GuardianBase.Bree) && !PlayerMod.HasGuardianSummoned(player, GuardianBase.Bree))
                {
                    TerraGuardian tg = NpcMod.GetGuardianNPCCharacter(GuardianBase.Bree);
                    if (tg.IsCompanionAtHome)
                    {
                        data.QuestStep = 3;
                        QuestCompletedNotification(data);
                    }
                }
            }
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (tg.ID == GuardianBase.Bree && tg.ModID == MainMod.mod.Name)
            {
                BreeStayQuestData data = (BreeStayQuestData)rawdata;
                if (tg.FriendshipLevel >= 5 && data.QuestStep == 0)
                {
                    return BreeDialogueQuestStart;
                }
            }
            return base.ImportantDialogueMessage(rawdata, tg, GuardianID, GuardianModID);
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            List<DialogueOption> dialogue = base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
            if (GuardianID == GuardianBase.Bree && GuardianModID == MainMod.mod.Name)
            {
                BreeStayQuestData data = (BreeStayQuestData)Data;
                if(data.QuestStep == 1)
                {
                    if (Main.LocalPlayer.HasItem(FishID))
                    {
                        dialogue.Add(new DialogueOption("I've got the fish.", BreeGiveFishQuestDialogue));
                    }
                    else
                    {
                        dialogue.Add(new DialogueOption("What kind of fish you want?.", AskWhichFishBreeWants));
                    }
                }
            }
            return dialogue;
        }

        public static void BreeGiveFishQuestDialogue()
        {
            BreeStayQuestData data = (BreeStayQuestData)Data;
            Dialogue.ShowDialogueWithContinue("You got the fish? Let me see them.");
            byte Count = 0;
            Player player = Main.LocalPlayer;
            for (int i = 0; i < 50; i++)
            {
                if (player.inventory[i].type == FishID)
                {
                    if (player.inventory[i].stack + Count > byte.MaxValue)
                    {
                        Count = 255;
                        break;
                    }
                    else
                    {
                        Count += (byte)player.inventory[i].stack;
                        if (Count >= FishCount)
                        {

                        }
                    }
                }
            }
            if (Count <= FishCount)
            {
                Dialogue.ShowDialogueWithContinue("Ah, so you've got " + Count + " of them.");
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("Wow, you've got quite a lot of them.");
            }
            byte Needed = (byte)(FishCount - data.FishGiven);
            if (Needed < Count)
            {
                Dialogue.ShowDialogueWithContinue("I only need " + Needed + " of them, so let's not get greedy.");
                Count = Needed;
            }
            else if (Needed > Count)
            {
                Dialogue.ShowDialogueWithContinue("It's not enough, but you can always bring me more.");
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("Ah, that's the necessary amount that I was needing.");
            }
            data.FishGiven += Count;
            for (int i = 0; i < 50; i++)
            {
                if (player.inventory[i].type == FishID)
                {
                    int StackToRemove = player.inventory[i].stack;
                    if(StackToRemove > Count)
                    {
                        StackToRemove = Count;
                    }
                    player.inventory[i].stack -= StackToRemove;
                    Count -= (byte)StackToRemove;
                    if (player.inventory[i].stack == 0)
                        player.inventory[i].SetDefaults(0);
                }
            }
            if (data.FishGiven >= FishCount)
            {
                data.QuestStep = 2;
                bool SardineAround = PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Sardine), 
                    GlennAround = PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Glenn);
                if (SardineAround && GlennAround)
                {
                    data.ConclusionState = 3;
                    Dialogue.ShowDialogueWithContinue("My family and I will enjoy eating those as dinner for two weeks.");
                }
                else if (!SardineAround && GlennAround)
                {
                    data.ConclusionState = 2;
                    Dialogue.ShowDialogueWithContinue("My son and I will enjoy eating those as dinner for two weeks, but I really wanted my husband to be here too.");
                }
                else if (SardineAround && !GlennAround)
                {
                    data.ConclusionState = 1;
                    Dialogue.ShowDialogueWithContinue("My husband and I will enjoy eating those as dinner for two weeks, but my son also should be here dinning with us...");
                }
                else
                {
                    data.ConclusionState = 0;
                    Dialogue.ShowDialogueWithContinue("This will give me a nice dinner for 2 weeks, but I really miss sharing those with my husband and son....");
                }
                Dialogue.ShowDialogueWithContinue("Yes, I know that I promissed to stay here if you brought me all those fishs. You don't need to remind me of that.");
                Dialogue.ShowDialogueWithContinue("I'll store away the things on my bag at my house, when I get back home.");
                Dialogue.ShowDialogueWithContinue("At least I will be able to relieve my back from this heavy bag.");
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("You still have to bring me " + (FishCount - data.FishGiven) + " " + MainMod.PluralifyWord(FishName, FishCount - data.FishGiven) + ", if you want me to stay.");
            }
            Dialogue.ShowEndDialogueMessage("Is there anything else you want to talk to me?", false);
        }

        public static void AskWhichFishBreeWants()
        {
            Dialogue.ShowDialogueWithContinue("You forgot already? I asked you to bring me " + FishCount + " " + MainMod.PluralifyWord(FishName, FishCount) + ".");
            BreeStayQuestData data = (BreeStayQuestData)Data;
            if(data.FishGiven > 0)
            {
                byte NewFishCount = (byte)(FishCount - data.FishGiven);
                Dialogue.ShowDialogueWithContinue("Actually, since you've already given me some, you only need to bring me " + NewFishCount + " " + MainMod.PluralifyWord(FishName, NewFishCount) + ".");
            }
            Dialogue.ShowEndDialogueMessage("Don't let that get out of your head, because I dislike repeating myself.", false);
        }

        public static void BreeDialogueQuestStart()
        {
            BreeStayQuestData data = (BreeStayQuestData)Data;
            Dialogue.ShowDialogueWithContinue("We have to talk now, [nickname], and I don't care if you're in a hurry or not.");
            Dialogue.ShowDialogueWithContinue("Everyone keeps nagging me to stay here. I already said that I still need to find my house!");
            Dialogue.ShowDialogueWithContinue("If you also think the same too, then bring me "+FishCount+" "+MainMod.PluralifyWord(FishName, FishCount)+" and I might consider staying.");
            Dialogue.ShowDialogueWithContinue("Otherwise, don't nag me with that anymore.");
            data.QuestStep = 1;
            QuestStartedNotification(data);
            Dialogue.ShowEndDialogueMessage("Now that's out of the way, is there something else you want to talk about?", false);
        }

        public class BreeStayQuestData : QuestData
        {
            public byte QuestStep = 0, FishGiven = 0;
            public byte ConclusionState = 0;
            private const byte QuestVersion = 0;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey+"questver", QuestVersion);
                Writer.Add(QuestKey + "qstep", QuestStep);
                Writer.Add(QuestKey + "qfish", FishGiven);
                Writer.Add(QuestKey + "qconc", ConclusionState);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                byte LastVer = Reader.GetByte(QuestKey + "questver");
                QuestStep = Reader.GetByte(QuestKey + "qstep");
                FishGiven = Reader.GetByte(QuestKey + "qfish");
                ConclusionState = Reader.GetByte(QuestKey + "qconc");

            }
        }
    }
}
