using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class PigCompanionsQuest : QuestBase
    {
        public const int WrathID = 0;

        public const int FriendshipLevelForSolidification = 5;

        public override string Name => "Shattered";

        public override QuestData GetQuestData => new PigQuestData();

        public override string Description(QuestData data)
        {
            return "You met a companion split into different emotions. Find them all and try fusing them together.";
        }

        public class PigQuestData : QuestData
        {
            public bool[] MetPigs = new bool[4];
            public bool[] SolidificationRequestGiven = new bool[4], SolidificationUnlocked = new bool[4];
            public bool UnlockedBland = false, SpokeToLeopoldAboutTheEmotionalPigs = false;
            private bool JustLoaded = true;
            private const byte Version = 0;

            public bool CheckIfJustLoaded()
            {
                if (JustLoaded)
                {
                    JustLoaded = false;
                    return true;
                }
                return false;
            }

            public byte MetPigCount
            {
                get
                {
                    byte c = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (MetPigs[i])
                            c++;
                    }
                    return c;
                }
            }

            public bool MetAnyPig
            {
                get
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (MetPigs[i])
                            return true;
                    }
                    return false;
                }
            }

            public bool AnyPigNeedingSolidification
            {
                get
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (MetPigs[i] && SolidificationRequestGiven[i] && !SolidificationUnlocked[i])
                            return true;
                    }
                    return false;
                }
            }

            public bool AnyPigCanBeSolidified
            {
                get
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (MetPigs[i] && SolidificationRequestGiven[i] && SolidificationUnlocked[i])
                            return true;
                    }
                    return false;
                }
            }

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey + "_Version", Version);
                for(int i = 0; i < 4; i++)
                {
                    Writer.Add(QuestKey + "_SolReq", SolidificationRequestGiven[i]);
                    Writer.Add(QuestKey + "_SolUnlock", SolidificationUnlocked[i]);
                }
                Writer.Add(QuestKey + "_LeopoldKnows", SpokeToLeopoldAboutTheEmotionalPigs);
                Writer.Add(QuestKey + "_BlandUnlocked", UnlockedBland);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                byte Version = Reader.GetByte(QuestKey + "_Version");
                for (int i = 0; i < 4; i++)
                {
                    SolidificationRequestGiven[i] = Reader.GetBool(QuestKey + "_SolReq");
                    SolidificationUnlocked[i] = Reader.GetBool(QuestKey + "_SolUnlock");
                }
                SpokeToLeopoldAboutTheEmotionalPigs = Reader.GetBool(QuestKey + "_LeopoldKnows");
                UnlockedBland = Reader.GetBool(QuestKey + "_BlandUnlocked");
            }
        }

        public override bool IsQuestStarted(QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            return data.MetAnyPig;
        }

        public override bool IsQuestComplete(QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            return data.UnlockedBland;
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            byte PigsFound = data.MetPigCount;
            if(PigsFound == 0)
            {
                return "I didn't found any of the emotional Pig TerraGuardian pieces.";
            }
            else if (PigsFound < 4)
            {
                if (!data.SpokeToLeopoldAboutTheEmotionalPigs)
                {
                    return "I've found a emotionally fragmented TerraGuardian. I should seek someone who could help with this.";
                }
                return "I've found " + PigsFound + " pieces of a TerraGuardian, but didn't found all of them.";
            }
            else
            {
                return "I need to find a way of fusing the pig TerraGuardians together.";
            }
        }

        public override string QuestStory(QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            byte PigsFound = data.MetPigCount;
            string Story = "";
            if(PigsFound == 0)
            {
                Story = "I didn't met any of the emotional Pig TerraGuardians yet. I may end up bumping into them during my travels.";
            }
            else
            {
                bool MetOnlyOne = true;
                string CompanionsNames = "";
                for (int i = 0; i < 4; i++)
                {
                    if (data.MetPigs[i])
                    {
                        if (CompanionsNames != "")
                        {
                            CompanionsNames += ", ";
                            MetOnlyOne = false;
                        }
                        switch (i)
                        {
                            case WrathID:
                                CompanionsNames += "Wrath";
                                break;
                        }
                    }
                }
                Story = "During my travels, I met ";
                if (MetOnlyOne)
                    Story += "a TerraGuardian named " + CompanionsNames + ", whose nickname is directly tied to their emotion they express.";
                else
                    Story += "some TerraGuardians. They were named " + CompanionsNames + ". Their nicknames were directly tied to the emotion they express.";

            }
            if (PigsFound > 0)
            {
                Story += "\n\nThey said that they don't remember what happened to them, and neither how they ended up emotionally divided";
                if (PigsFound < 4)
                {
                    Story += ", but it seems like there's more pieces of them to be found.";
                }
                else
                {
                    Story += ", but gladly it looks like I managed to find all of them.";
                }

                bool First = true;
                for(int i = 0; i < 4; i++)
                {
                    if (data.SolidificationRequestGiven[i])
                    {
                        if (First)
                        {
                            Story += "\n\n";
                            First = false;
                        }
                        bool Solidified = data.SolidificationUnlocked[i];
                        switch (i)
                        {
                            case WrathID:
                                Story += "The Pig of Wrath said that was tired of being a red cloud, and asked me to find a way of solidifying their body.";
                                if (Solidified)
                                    Story += " Gladly, Leopold helped giving a solution to allow solidifying their body, and also vaporize too.";
                                break;
                        }
                    }
                }
                if (data.UnlockedBland)
                {
                    Story += "\n\nI managed to get Leopold to help me fuse all pigs together into a TerraGuardian, and it resulted into a emotionless TerraGuardian with incredible powers.\n\nTHE END";
                }
            }

            return Story;
        }

        public override void UpdatePlayer(Player player, QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            if(data.CheckIfJustLoaded())
            {
                //Add here the scripts to get the companions player have already met
                data.MetPigs[WrathID] = PlayerMod.PlayerHasGuardian(player, GuardianBase.Wrath);
            }
        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            if(GuardianModID == MainMod.mod.Name)
            {
                switch (GuardianID)
                {
                    case GuardianBase.Leopold:
                        {
                            PigQuestData data = (PigQuestData)Data;
                            List<GuardianMouseOverAndDialogueInterface.DialogueOption> dialogues = new List<GuardianMouseOverAndDialogueInterface.DialogueOption>();
                            if (data.MetAnyPig)
                            {
                                dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("About the emotional pigs...", WhenTalkingToLeopoldAboutThePigs));
                            }
                            return dialogues;
                        }
                }
            }
            return base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            PigQuestData data = (PigQuestData)rawdata;
            if(GuardianModID == MainMod.mod.Name)
            {
                if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
                {
                    switch (GuardianID)
                    {
                        case GuardianBase.Wrath:
                            data.MetPigs[WrathID] = true;
                            if (tg.FriendshipLevel >= 5 && !data.SolidificationRequestGiven[WrathID])
                            {
                                data.SolidificationRequestGiven[WrathID] = true;
                                //Wrath speaks about trying to find a way of solidifying his body.
                                return new Action(WrathTellingYouAboutFormChanging);
                            }
                            break;
                    }
                }
            }
            return base.ImportantDialogueMessage(data, tg, GuardianID, GuardianModID);
        }

        public static void WrathTellingYouAboutFormChanging()
        {
            PigQuestData data = (PigQuestData)Data;
            if(Dialogue.ShowDialogueWithOptions("*This form is revolting, I'm boiling out of rage due to this. There must be a way of making me solid again, maybe that nerdy guy can help me in this, let's talk to him.*", new string[] { "Who? [gn:"+GuardianBase.Leopold+"]? Sure, Let's visit him.", "Not right now.." }) == 0)
            {
                Dialogue.ShowEndDialogueMessage("*Great, or else I would give you a beating.*", false);
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*Oh you...! (Insert several different insults here)*", false);
            }
        }

        public static void WhenTalkingToLeopoldAboutThePigs()
        {
            PigQuestData data = (PigQuestData)Data;
            if (!data.SpokeToLeopoldAboutTheEmotionalPigs)
            {
                bool HasAnyOfThePigs = PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Leopold);
                Dialogue.ShowDialogueWithContinue("*Huh? Emotional pigs? What are you talking about?*");
                if (HasAnyOfThePigs)
                {
                    Dialogue.ShowDialogueWithContinue("*Ah, I see what you mean now.*");
                    Dialogue.ShowDialogueWithContinue("*Hm... Actually, I do know about their condition. It seems like its body has vaporized at the moment its personality was split.*");
                    Dialogue.ShowDialogueWithContinue("*I can try doing something to make his personality solid, but I can only merge its personalities if you find them.*");
                    data.SpokeToLeopoldAboutTheEmotionalPigs = true;
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*I don't know of anything like that. Could you bring what you mean to me? Whatever that is, I need to see with my own eyes.*");
                    Dialogue.ShowEndDialogueMessage("*Anyways, is there something else you want to talk about?*");
                    return;
                }
            }
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> dialogues = new List<GuardianMouseOverAndDialogueInterface.DialogueOption>();
            if (data.MetAnyPig)
            {
                dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("What can you tell me about the emotional pigs?", LeopoldsCommentAboutThePigs));
            }
            if (data.AnyPigNeedingSolidification)
            {
                dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("One of the pigs wants to have their body solidified.", LeopoldsTalkAboutUnlockingSolidification));
            }
            if (data.AnyPigCanBeSolidified)
            {
                dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("Can you alter the body state of a pig?", LeopoldsTalkAboutUnlockingSolidification));
            }
            Dialogue.ShowDialogueWithOptions("*About them? What do you want to talk about them?*", dialogues.ToArray());
        }

        public static void LeopoldsCommentAboutThePigs()
        {
            PigQuestData data = (PigQuestData)Data;
            if (data.UnlockedBland)
            {
                Dialogue.ShowDialogueWithContinue("*The result caused by the fusion of each pig piece is really intriguing me.*");
                Dialogue.ShowDialogueWithContinue("*I wonder what caused their final form to nullify any trace of emotion.*");
                Dialogue.ShowEndDialogueMessage("*Maybe happened some kind of negation by the part of the resulting form to them, but that's only a theory.\nAnyways, do you want to speak about something else?*", false);
            }
            else
            {
                if(data.MetPigCount < 4)
                {
                    Dialogue.ShowDialogueWithContinue("*Oh yes, the case of the TerraGuardian shattered into different emotions.*");
                    Dialogue.ShowDialogueWithContinue("*I'm studying what could be the causer of that issue.*");
                    Dialogue.ShowDialogueWithContinue("*If you manage to know about something, or know of anything they may remember, do tell me. I'm curious to know what actually happened.*");
                    Dialogue.ShowEndDialogueMessage("*Meanwhile, all we can do is either research, or theorize what may have happened.*", false);
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Ah, them. It seems like you managed to find all of them. Funny, I thought there were more.*");
                    Dialogue.ShowDialogueWithContinue("*Oh well, when you find out that they're ready for the fusion, I'll take care of merging them together into their final form.*");
                    Dialogue.ShowEndDialogueMessage("*I'm curious about what that final form may look like, probably into someone really strong.*", false);
                }
            }
        }

        public static void LeopoldTalkAboutChangingPigForm()
        {
            PigQuestData data = (PigQuestData)Data;
            List<int> CompanionsCanChangeForm = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (data.MetPigs[i] && data.SolidificationRequestGiven[i] && data.SolidificationUnlocked[i])
                {
                    bool HasCompanionsSummoned = false;
                    switch (i)
                    {
                        case WrathID:
                            if (PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Wrath))
                                HasCompanionsSummoned = true;
                            break;
                    }
                    if (HasCompanionsSummoned)
                        CompanionsCanChangeForm.Add(i);
                }
            }
            Dialogue.ShowDialogueWithContinue("*You want to change the state of the body of one of the emotional pigs?*");
            if (CompanionsCanChangeForm.Count == 0)
            {
                Dialogue.ShowEndDialogueMessage("*You should have the companion you want to change their body state following you, or else I can't do anything.*", false);
            }
            else
            {
                string[] Options = new string[CompanionsCanChangeForm.Count + 1];
                for(int i = 0; i < CompanionsCanChangeForm.Count; i++)
                {
                    switch (CompanionsCanChangeForm[i])
                    {
                        case WrathID:
                            Options[i] = "Change Wrath's Form";
                            break;
                    }
                }
                Options[CompanionsCanChangeForm.Count] = "Nevermind";
                int PickedOption = Dialogue.ShowDialogueWithOptions("*Who do you want to change the body form?*", Options);
                if(PickedOption == CompanionsCanChangeForm.Count)
                {
                    Dialogue.ShowEndDialogueMessage("*Changed your mind? Then I will do nothing. Want to talk about something else?*", false);
                }
                else
                {
                    TerraGuardian tg = null;
                    PlayerMod player = Main.LocalPlayer.GetModPlayer<PlayerMod>();
                    string CloudFormDialogue = "", SolidFormDialogue = "";
                    switch (CompanionsCanChangeForm[PickedOption])
                    {
                        case WrathID:
                            {
                                tg = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Wrath);
                            }
                            break;
                    }
                    if(tg == null)
                    {
                        Dialogue.ShowEndDialogueMessage("*Oh well... Something unexpected happened. Want to talk about something else?*", false);
                    }
                    else
                    {
                        if(Dialogue.ShowDialogueWithOptions("*Do you really want to change " + tg.Name + "'s form to " + (player.PigGuardianCloudForm[PickedOption] ? "Cloud" : "Solid") + "?*", new string[] { "Yes", "No" }) == 0)
                        {
                            player.PigGuardianCloudForm[PickedOption] = !player.PigGuardianCloudForm[PickedOption];
                            TerraGuardian Speaker = Dialogue.GetSpeaker;
                            if (player.PigGuardianCloudForm[PickedOption])
                                Dialogue.ShowDialogueWithContinue(CloudFormDialogue, tg);
                            else
                                Dialogue.ShowDialogueWithContinue(SolidFormDialogue, tg);
                            Dialogue.ShowEndDialogueMessage("*Well, It's done. Do you want something else?*", false, Speaker);
                        }
                    }
                }
            }
        }

        public static void LeopoldsTalkAboutUnlockingSolidification()
        {
            PigQuestData data = (PigQuestData)Data;
            List<int> CompanionsNeedingSolidification = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (data.MetPigs[i] && data.SolidificationRequestGiven[i] && !data.SolidificationUnlocked[i])
                {
                    bool HasCompanionsSummoned = false;
                    switch (i)
                    {
                        case WrathID:
                            if (PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Wrath))
                                HasCompanionsSummoned = true;
                            break;
                    }
                    if(HasCompanionsSummoned)
                       CompanionsNeedingSolidification.Add(i);
                }
            }
            Dialogue.ShowDialogueWithContinue("*Some of the emotional pigs are wanting to solidify their body?*");
            if (CompanionsNeedingSolidification.Count == 0)
            {
                Dialogue.ShowDialogueWithContinue("*Wait! There is nobody needing solidification following you!*");
                Dialogue.ShowEndDialogueMessage("*I can't try solidifying the emotional pig wanting that without them around. Call them the next time.*", false);
            }
            else
            {
                while (CompanionsNeedingSolidification.Count > 0)
                {
                    int PickedPig = CompanionsNeedingSolidification[0];
                    CompanionsNeedingSolidification.RemoveAt(0);
                    TerraGuardian Leopold = Dialogue.GetSpeaker;
                    switch (PickedPig)
                    {
                        case WrathID:
                            {
                                int ParticipantID = Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Wrath));
                                TerraGuardian Wrath = Dialogue.GetParticipant(ParticipantID);
                                Dialogue.ShowDialogueWithContinue("*I am sick of this form! It even makes me boil into anger just thinking about it!*", Wrath);
                                Dialogue.ShowDialogueWithContinue("*Let me try doing something...*", Leopold);
                                Main.LocalPlayer.GetModPlayer<PlayerMod>().PigGuardianCloudForm[WrathID] = false;
                                Dialogue.ShowDialogueWithContinue("*It worked! Great!*", Wrath);
                            }
                            break;
                    }
                    data.SolidificationUnlocked[PickedPig] = true;
                    Dialogue.ShowDialogueWithContinue("*It's done. Someone else needs their body solidified?*", Leopold);
                }
                Dialogue.ShowEndDialogueMessage("*No? alright then. Do you want to speak about something else, [nickname]?*", false);
            }
        }
    }
}