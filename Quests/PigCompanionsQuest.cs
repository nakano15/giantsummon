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
        public const int WrathID = Creatures.PigGuardianFragmentBase.AngerPigGuardianID, FearID = Creatures.PigGuardianFragmentBase.FearPigGuardianID,
            SadnessID = Creatures.PigGuardianFragmentBase.SadnessPigGuardianID, HappinessID = Creatures.PigGuardianFragmentBase.HappinessPigGuardianID;

        public const int FriendshipLevelForSolidification = 5;

        public override string Name => "Shattered";

        public override QuestData GetQuestData => new PigQuestData();

        public override string Description(QuestData data)
        {
            return "You met a emotional pig fragment. Find them all to fuse back to their original embodiment.";
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
                    Writer.Add(QuestKey + "_SolReq_" + i, SolidificationRequestGiven[i]);
                    Writer.Add(QuestKey + "_SolUnlock_"+i, SolidificationUnlocked[i]);
                }
                Writer.Add(QuestKey + "_LeopoldKnows", SpokeToLeopoldAboutTheEmotionalPigs);
                Writer.Add(QuestKey + "_BlandUnlocked", UnlockedBland);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                byte Version = Reader.GetByte(QuestKey + "_Version");
                for (int i = 0; i < 4; i++)
                {
                    SolidificationRequestGiven[i] = Reader.GetBool(QuestKey + "_SolReq_"+i);
                    SolidificationUnlocked[i] = Reader.GetBool(QuestKey + "_SolUnlock_"+1);
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
            if (data.UnlockedBland)
            {
                return "I managed to be able to fuse all pigs guardians.";
            }
            if(PigsFound == 0)
            {
                return "I didn't found any of the emotional Pig TerraGuardian pieces.";
            }
            else if (PigsFound < 4)
            {
                if (!data.SpokeToLeopoldAboutTheEmotionalPigs)
                {
                    return "I've found a fragmented piece of a TerraGuardian. I should seek someone who could help with this.";
                }
                return "I've found " + PigsFound + " pieces of a TerraGuardian, but didn't find all of them yet.";
            }
            else
            {
                return "I need to find a way of fusing the pig guardians together.";
            }
        }

        public override string QuestStory(QuestData rawdata)
        {
            PigQuestData data = (PigQuestData)rawdata;
            byte PigsFound = data.MetPigCount;
            string Story = "";
            if(PigsFound == 0)
            {
                Story = "I haven't met any of the emotional Pig TerraGuardians yet. I may end up bumping into them during my travels.";
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
                                CompanionsNames += "Anger";
                                break;
                            case SadnessID:
                                CompanionsNames += "Sadness";
                                break;
                            case HappinessID:
                                CompanionsNames += "Happiness";
                                break;
                            case FearID:
                                CompanionsNames += "Fear";
                                break;
                        }
                    }
                }
                Story = "During my travels, I met ";
                if (MetOnlyOne)
                    Story += "a TerraGuardian. The only emotion they can express is " + CompanionsNames + ".";
                else
                    Story += "some TerraGuardians. Based on the emotions they express, they were " + CompanionsNames + ".";

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
                                Story += "The Pig of Wrath said that it was tired of being astral, and asked me to find a way of getting a physical form.";
                                if (Solidified)
                                    Story += " Gladly, Leopold helped giving a solution to allow mortalizing their body, and also re-astralizing if needed.";
                                break;
                            case SadnessID:
                                Story += "";
                                if (Solidified)
                                    Story += " ";
                                break;
                            case HappinessID:
                                Story += "";
                                if (Solidified)
                                    Story += " ";
                                break;
                            case FearID:
                                Story += "The Pig of Fear is fearing that people think of him as a freak, just because of his astral form, and asked me to find out how to turn his body into physical form.";
                                if (Solidified)
                                    Story += " Leopold knew a solution for this issue, so he's able to solidify his body, and turn it to astral form when needed too.";
                                break;
                        }
                    }
                }
                if (data.UnlockedBland)
                {
                    Story += "\n\nI managed to get Leopold to help me fuse all pigs together into a TerraGuardian, and it resulted into a emotionless TerraGuardian with impressive capabilities and a bland attitude.\n\nTHE END";
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

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            if(GuardianModID == MainMod.mod.Name)
            {
                switch (GuardianID)
                {
                    case GuardianBase.Leopold:
                        {
                            PigQuestData data = (PigQuestData)Data;
                            List<DialogueOption> dialogues = new List<DialogueOption>();
                            if (data.MetAnyPig)
                            {
                                dialogues.Add(new DialogueOption("About the emotional pigs...", WhenTalkingToLeopoldAboutThePigs, true));
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
                        case GuardianBase.Joy:
                            data.MetPigs[HappinessID] = true;
                            /*if (tg.FriendshipLevel >= 5 && !data.SolidificationRequestGiven[HappinessID])
                            {
                                data.SolidificationRequestGiven[HappinessID] = true;
                                //Solidifying quest
                                return new Action(WrathTellingYouAboutFormChanging);
                            }*/
                            break;
                        case GuardianBase.Sadness:
                            data.MetPigs[SadnessID] = true;
                            /*if (tg.FriendshipLevel >= 5 && !data.SolidificationRequestGiven[SadnessID])
                            {
                                data.SolidificationRequestGiven[SadnessID] = true;
                                //Solidifying quest
                                return new Action(WrathTellingYouAboutFormChanging);
                            }*/
                            break;
                        case GuardianBase.Fear:
                            data.MetPigs[FearID] = true;
                            if (tg.FriendshipLevel >= 5 && !data.SolidificationRequestGiven[FearID])
                            {
                                data.SolidificationRequestGiven[FearID] = true;
                                //Solidifying quest
                                return new Action(FearTellingYouAboutFormChanging);
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
            if (Dialogue.ShowDialogueWithOptions("*I need flesh and muscle to give more impact to my attacks. There must be a way of giving me a solid body,that white bunny needs to help me now, go talk to him and im not accepting no for a answer.*", new string[] { "Who? [gn:" + GuardianBase.Leopold + "]? Sure, Let's visit him.", "Not right now.." }) == 0)
            {
                Dialogue.ShowEndDialogueMessage("*Great, or else I would give you a pounding.*", false);
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*Oh you...! (Insert several different insults here)*", false);
            }
        }

        public static void FearTellingYouAboutFormChanging()
        {
            PigQuestData data = (PigQuestData)Data;
            if (Dialogue.ShowDialogueWithOptions("*I don't like this form. Everyone else is solid, like you, and I look like... This. Could you go talk with [gn:"+ GuardianBase.Leopold + "] to see if he has a solution for this?*", new string[] { "Yes, I can.", "Not now." }) == 0)
            {
                Dialogue.ShowEndDialogueMessage("*Okay... So... Let's go then?*", false);
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*I think I can stay in this form for some more time. But it is really scary sounding like a freak.*", false);
            }
        }

        public static void WhenTalkingToLeopoldAboutThePigs()
        {
            PigQuestData data = (PigQuestData)Data;
            if (!data.SpokeToLeopoldAboutTheEmotionalPigs)
            {
                bool HasAnyOfThePigs = PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Wrath) || PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Fear) 
                    || PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Joy) || PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Sadness);
                Dialogue.ShowDialogueWithContinue("*Huh? Emotional pigs? What are you talking about?*");
                if (HasAnyOfThePigs)
                {
                    Dialogue.ShowDialogueWithContinue("*Ah, I see what you mean now.*");
                    Dialogue.ShowDialogueWithContinue("*Hm... Actually, I do know about their condition. It seems like its body has vaporized at the moment its personality was split.*");
                    Dialogue.ShowDialogueWithContinue("*I can try doing something to make its personality solid, but I can only merge its personalities if you find them.**");
                    data.SpokeToLeopoldAboutTheEmotionalPigs = true;
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*I don't know of anything like that. Could you bring what you mean to me? Whatever that is, I need to see with my own eyes.*");
                    Dialogue.ShowEndDialogueMessage("*Anyways, is there something else you want to talk about?*");
                    return;
                }
            }
            List<DialogueOption> dialogues = new List<DialogueOption>();
            if (data.MetAnyPig)
            {
                dialogues.Add(new DialogueOption("What can you tell me about the emotional pigs?", LeopoldsCommentAboutThePigs));
            }
            if (data.AnyPigNeedingSolidification)
            {
                dialogues.Add(new DialogueOption("One of the pigs wants to have their body solidified.", LeopoldsTalkAboutUnlockingSolidification));
            }
            if (data.AnyPigCanBeSolidified)
            {
                dialogues.Add(new DialogueOption("Can you alter the body state of a pig?", LeopoldTalkAboutChangingPigForm));
            }
            dialogues.Add(new DialogueOption("Enough talking about that.", delegate ()
            {
                Dialogue.ShowEndDialogueMessage("*I'm still trying to understand too, but I can still try figuring out what is going on with the pigs.\n" +
                    "Feel free to speak to me again in case you have any other question.*", false);
            }));
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
                    Dialogue.ShowDialogueWithContinue("*Oh well, when you find out that they're ready for the fusion, I'll take care of merging them together into their original form.*");
                    Dialogue.ShowEndDialogueMessage("*I'm curious about what that original form may look like, probably would result in something fascinating.*", false);
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
                        case FearID:
                            if (PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Fear))
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
                        case FearID:
                            Options[i] = "Change Fear's Form";
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
                                CloudFormDialogue = "*Grrr. I hate this! I hate It!*";
                                SolidFormDialogue = "*Now I can really hurt things.*";
                            }
                            break;
                        case FearID:
                            {
                                tg = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Fear);
                                CloudFormDialogue = "*Why? Now people will look weird at me again.*";
                                SolidFormDialogue = "*It's good to be solid again, and I think people will no longer look weird at me.*";
                            }
                            break;
                    }
                    if(tg == null)
                    {
                        Dialogue.ShowEndDialogueMessage("*Oh well... Something unexpected happened. Want to talk about something else?*", false);
                    }
                    else
                    {
                        if(Dialogue.ShowDialogueWithOptions("*Do you really want to change " + tg.Name + "'s form to " + (player.PigGuardianCloudForm[PickedOption] ? "Astral" : "Solid") + "?*", new string[] { "Yes", "No" }) == 0)
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
                        case FearID:
                            if (PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Fear))
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
                        case FearID:
                            {
                                int ParticipantID = Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Fear));
                                TerraGuardian Fear = Dialogue.GetParticipant(ParticipantID);
                                Dialogue.ShowDialogueWithContinue("*This form... Can you change my body form?*", Fear);
                                Dialogue.ShowDialogueWithContinue("*I read something related to that, I'll try doing something I read in a book...*", Leopold);
                                Main.LocalPlayer.GetModPlayer<PlayerMod>().PigGuardianCloudForm[FearID] = false;
                                Dialogue.ShowDialogueWithContinue("*I- I'm solid again! Amazing!! Oh... Everyone is looking at me... It's quite scary...*", Fear);
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
