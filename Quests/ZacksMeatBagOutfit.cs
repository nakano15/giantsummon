using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class ZacksMeatBagOutfitQuest : QuestBase
    {
        /// <summary>
        /// Zacks Meatbag Outfit Quest Steps
        /// Step 0: Speak with Blue.
        /// Step 1: Dialogue with Blue.
        /// Step 2~3: Speak with the Nurse.
        /// Step 4~5: Speak with the Clothier.
        /// Step 6~7: Speak with Zacks.
        /// Step 8: Quest completed.
        /// Even = Zacks doesn't knows.
        /// Odd = Zack knows (was in the party when listening to her request).
        /// 
        /// ZacksKnowsOfShirt Steps
        /// Step 0: Doesn't know that Blue picked the shirt.
        /// Step 1: Knows Blue picked shirt.
        /// Step 2: After asking if player picked shirt.
        /// Step 3: After talking about Blue's choice of shirt.
        /// Step 4: After player lied, saying that they picked the shirt.
        /// </summary>

        public override string Name => "Let's patch it up!";

        public override string Description(QuestData data)
        {
            return "Blue is interessed in changing Zacks look.";
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)rawdata;
            switch (data.QuestStep)
            {
                case 1:
                    return "Speak with Blue about what she wants.";
                case 2:
                case 3:
                    return "Visit the Nurse while having Blue with you.";
                case 4:
                case 5:
                    return "Visit the Clothier while having Blue with you.";
                case 6:
                case 7:
                    return "Talk with Zacks while having Blue with you.";
            }
            return base.GetQuestCurrentObjective(data);
        }

        public override string QuestStory(QuestData rawdata)
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)rawdata;
            string Story = "";
            byte StoryStep = data.QuestStep;
            if(StoryStep == 0)
            {
                return "You must speak with Blue while having Zacks living in your world.";
            }
            if (StoryStep >= 1)
            {
                Story += "I was talking with Blue, until she told me that wanted to talk to me about something.";
                if (StoryStep >= 2)
                    Story += " As I asked, she told me that wants to do something about the open wounds on Zacks body, but needs my help to get a number of things for this.";
            }
            if (StoryStep > 2 && StoryStep % 2 == 1)
            {
                Story += "\n\nZacks ended up knowing about what Blue and I were planning for him, so he knows of what we were planning for him.";
            }
            if (StoryStep >= 2)
            {
                if (StoryStep < 4)
                {
                    Story += "\n\nI was asked to visit the Nurse while having Blue with me, to get some Bandages.";
                }
                else
                {
                    Story += "\n\nBlue told me that we should visit the Nurse and get some Bandages, and so we went to see her. As we spoke to the Nurse about why we needed the bandages, she gave some of the Bandages she had spare without charging anything.";
                }
            }
            if (StoryStep >= 4)
            {
                if (StoryStep < 6)
                {
                    Story += "\n\nNow we need to visit the Clothier to try getting a shirt.";
                }
                else
                {
                    Story += "\n\nWe then went to visit the Clothier to pick a shirt for Zacks chest. Blue seems to have found one, which she said that was perfect for Zacks.";
                }
            }
            if (StoryStep >= 6)
            {
                if (StoryStep < 8)
                {
                    Story += "\n\nBlue and I should now deliver the items to Zacks, and see his reaction.";
                }
                else
                {
                    Story += "\n\nWe delivered the Bandages and the Shirt to Zacks. Blue managed to patch his wounds with the bandages, and gave him a shirt to cover the hole on his chest.\nZacks didn't liked the fact that \"Meat Bag\" was written in his shirt, with the image of a piece of meat on it, and then Blue laughed at his reaction, while Zacks was clearly not happy about his gift, beside he liked that got his wounds all covered up.\n\nTHE END";
                }
            }
            return Story;
        }

        public override bool IsQuestStarted(QuestData data)
        {
            return ((ZacksOutfitQuestData)data).QuestStep > 0;
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return ((ZacksOutfitQuestData)data).QuestStep >= 8;
        }

        public static bool IsMeatbagQuestCompleted()
        {
            return PlayerMod.GetQuestData(Main.LocalPlayer, Quests.TgQuestContainer.ZacksMeatbagOutfitQuest).IsQuestCompleted;
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsChatDialogue, int GuardianID, string GuardianModID)
        {
            if (!IsChatDialogue && GuardianModID == MainMod.mod.Name && (GuardianID == GuardianBase.Blue || GuardianID == GuardianBase.Zacks))
            {
                ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
                List<DialogueOption> dialogues = base.AddDialogueOptions(IsChatDialogue, GuardianID, GuardianModID);
                if(GuardianID == GuardianBase.Blue) //If is Blue
                {
                    if (data.QuestStep == 1)
                    {
                        dialogues.Add(new DialogueOption("What do you need help with?", BlueWhenListeningToHerRequest, true));
                    }
                    else if (data.QuestStep > 1 && data.QuestStep < 8)
                    {
                        dialogues.Add(new DialogueOption("What should we be doing now?", UponAskingAboutRequest, true));
                    }
                }
                else //If is Zacks
                {
                    if (data.QuestStep == 6 || data.QuestStep == 7)
                    {
                        dialogues.Add(new DialogueOption("We have something to give you.", UponDeliveringToZacksDialogue, true));
                    }
                    else if (data.QuestStep >= 2 && data.QuestStep < 6)
                    {
                        dialogues.Add(new DialogueOption("About Blue's request related to you.", UponAskingAboutRequest, true));
                    }
                    else if(data.QuestStep >= 8)
                    {
                        dialogues.Add(new DialogueOption("About the gift you got.", AfterQuestZacks, true));
                    }
                }
                return dialogues;
            }
            return base.AddDialogueOptions(IsChatDialogue, GuardianID, GuardianModID);
        }

        public override Action ImportantDialogueMessage(QuestData data, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (GuardianModID == MainMod.mod.Name && GuardianID == GuardianBase.Blue)
            {
                if(NpcMod.HasGuardianNPC(GuardianBase.Zacks) && ((ZacksOutfitQuestData)data).QuestStep == 0)
                {
                    return new Action(BlueTellsYouAboutHerRequest);
                }
            }
            return base.ImportantDialogueMessage(data, tg, GuardianID, GuardianModID);
        }

        public override QuestData GetQuestData => new ZacksOutfitQuestData();

        public class ZacksOutfitQuestData : QuestData
        {
            public byte QuestStep = 0;
            public byte ZacksKnowsWhoPickedShirt = 0;
            private const short DataVersion = 1;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey + "_Version", DataVersion);
                Writer.Add(QuestKey + "_Step", QuestStep);
                Writer.Add(QuestKey + "_ZacksKnowOfShirt", ZacksKnowsWhoPickedShirt);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                short Version = Reader.GetShort(QuestKey + "_Version");
                QuestStep = Reader.GetByte(QuestKey + "_Step");
                if (Version > 0)
                {
                    ZacksKnowsWhoPickedShirt = Reader.GetByte(QuestKey + "_ZacksKnowOfShirt");
                }
            }
        }

        public override string QuestNpcDialogue(NPC npc)
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            if (PlayerMod.PlayerHasGuardianSummoned(Terraria.Main.LocalPlayer, GuardianBase.Blue))
            {
                if ((data.QuestStep == 4 || data.QuestStep == 5) && npc.type == Terraria.ID.NPCID.Clothier)
                {
                    data.QuestStep += 2;
                    PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue).
					SaySomethingCanSchedule("*This one! This one is perfect. I'll take It. Thank you. Let's give It to Zacks.*", true, 90);
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                    {
                        data.ZacksKnowsWhoPickedShirt = 1;
                        PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks).
						SaySomethingCanSchedule("*I hope you picked something cool for me.*", true, 90 + 150);
                    }
                    return "So, you want a shirt for " + NpcMod.GetGuardianNPCName(GuardianBase.Zacks) + "? I have quite a selection, feel free to browse.";
                }
                if ((data.QuestStep == 2 || data.QuestStep == 3) && npc.type == Terraria.ID.NPCID.Nurse)
                {
                    data.QuestStep += 2;
                    PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue).
					SaySomethingCanSchedule("*That's perfect! Thanks " + npc.GivenOrTypeName + ". Now we should visit the Clothier.*", true, 90);
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                    {
                        PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks).
						SaySomethingCanSchedule("*That's a lot of bandages! Just how wounded am I? I could even use some to wipe myself.*", true, 90 + 150);
                    }
                    return "You want some bandages for " + NpcMod.GetGuardianNPCName(GuardianBase.Zacks) + "? Gladly I have an extra number of them. Feel free to take them.";
                }
            }
            return base.QuestNpcDialogue(npc);
        }

        public static void BlueTellsYouAboutHerRequest()
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            data.QuestStep = 1;
            if(Dialogue.ShowDialogueWithOptions("*I have something that I will need your help with, could you talk to me later about that?*", new string[] { "Okay.", "We can speak about that now." }) == 1)
            {
                Dialogue.ShowDialogueWithContinue("*Thank you, I hope you don't mind listening me for a while.*");
                BlueWhenListeningToHerRequest();
                return;
            }
            Dialogue.ShowEndDialogueMessage("*Good. Now, do you want to speak about It right away, or want to speak about something else?*", false);
        }

        public static void BlueWhenListeningToHerRequest()
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            const int BlueSlot = 0, ZacksSlot = 1;
            bool ZacksInTheTeam = PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks);
            if (ZacksInTheTeam)
            {
                Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks));
            }
            Dialogue.GatherAroundGuardian(Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*I'm glad for Zacks back into my life, but I think we should do something about his wounds.*");
            //After this point, Dialogue.GetParticipant breaks.
            if (ZacksInTheTeam)
            {
                Dialogue.ShowDialogueWithContinue("*I'm fine, those wounds doesn't hurt or anything.*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*It may not be hurting, but isn't the greatest thing to look at, even more at that hole in your chest.*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*Oh, yeah... This is really a nasty thing to look at.*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*We need to patch up the wounds on your arms and feet, and wash the blood in your paws... Is that even your blood?*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*I... Don't think... It is?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*We definitelly have to wash that off.*", Dialogue.GetParticipant(BlueSlot));
            }
            Dialogue.ShowDialogueWithContinue("*We need something to patch up his wounds, and I think the Nurse may help us with that.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*Then we will need to visit the Clothier, and see if we can find something to cover the hole on his chest.*");
            if (Dialogue.ShowDialogueWithOptions("*Can you help me with this?*", new string[] { "Yes", "No" }) == 1)
            {
                if (ZacksInTheTeam)
                {
                    Dialogue.ShowDialogueWithContinue("*I guess [nickname] isn't feeling like helping you with that right now.*", Dialogue.GetParticipant(ZacksSlot));
                    Dialogue.ShowEndDialogueMessage("*It's fine. I still want to do this some time, so if you feel like helping me, just speak to me about that.*", false, Dialogue.GetParticipant(BlueSlot));
                }
                else
                {
                    Dialogue.ShowEndDialogueMessage("*Oh... I'll leave this open for when you decide to help me with this.*", false);
                }
                return;
            }
            Dialogue.ShowDialogueWithContinue("*Thank you, [nickname].*", Dialogue.GetParticipant(BlueSlot));
            if (ZacksInTheTeam)
            {
                Dialogue.ShowDialogueWithContinue("*That actually sounds like a good plan, I like it.*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ChangeSpeaker(BlueSlot);
                data.QuestStep = 3;
            }
            else
            {
                data.QuestStep = 2;
            }
            Dialogue.ShowDialogueWithContinue("*For this to work, we need to go see those people ourselves.*", Dialogue.GetParticipant(BlueSlot));
            if (!PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue))
            {
                Dialogue.ShowDialogueOnly("*And by \"we\", that means I need to come too.*");
                GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*So, shall we go?*", true);
            }
        }

        public static void UponAskingAboutRequest()
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            Dialogue.SetImportantDialogue();
            if (Dialogue.GetSpeaker.ID == GuardianBase.Zacks) //Zacks triggered the dialogue
            {
                bool Knows = data.QuestStep % 2 == 1;
                bool HasBlueInTheParty = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue);
                if (!Knows)
                {
                    Dialogue.ShowDialogueWithContinue("*Blue wants to patch up my wounds?*");
                    const int ZacksID = 0, BlueID = 1;
                    Dialogue.GatherAroundGuardian(Dialogue.GetParticipant(ZacksID));
                    if (HasBlueInTheParty)
                    {
                        Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue));
                        Dialogue.ShowDialogueWithContinue("*([nickname], we should have kept that a secret...)*", Dialogue.GetParticipant(BlueID));
                        Dialogue.ShowDialogueWithContinue("*Yes... Your wounds aren't a good thing to look at, and that hole in your chest needs to be covered. There is also that blood in your paws to remove, too.*", Dialogue.GetParticipant(BlueID));
                        Dialogue.ShowDialogueWithContinue("*You have a point. Okay, I guess I should wait to see what you two will bring me, then.*", Dialogue.GetParticipant(ZacksID));
                        Dialogue.ShowDialogueWithContinue("*Don't worry, you wont be disappointed. (She gives a slightly evil smile)*", Dialogue.GetParticipant(BlueID));
                        data.QuestStep++;
                        Dialogue.ShowEndDialogueMessage("*Uh oh.*", false, Dialogue.GetParticipant(ZacksID));
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("*Is she worried that I may be in pain or something? I already said that I can't feel anything in this state.*");
                        Dialogue.ShowDialogueWithContinue("*Maybe I'm worrying her again...?*");
                        Dialogue.ShowEndDialogueMessage("*I don't know... Maybe I should wait to see what is it about.*", false);
                    }
                }
                else
                {
                    const int ZacksID = 0, BlueID = 1;
                    if (!HasBlueInTheParty)
                    {
                        Dialogue.ShowEndDialogueMessage("*Ah, yes. I remember that you and [gn:"+GuardianBase.Blue+"] were planning something, right? Where she is, by the way?*", false);
                        return;
                    }
                    Dialogue.GatherAroundGuardian(Dialogue.GetParticipant(ZacksID));
                    Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue));
                    Dialogue.ShowDialogueWithContinue("*So, have you two managed to find something to take care of my wounds?*");
                    // Place here branching dialogues based on the current progress in the quest.
                    switch (data.QuestStep)
                    {
                        case 2:
                        case 3:
                            {
                                Dialogue.ShowDialogueWithContinue("*We still have to visit the Nurse to get the Bandages.*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*I see... You two came to see if I could lend some money?*", Dialogue.GetParticipant(ZacksID));
                                switch (Dialogue.ShowDialogueWithOptions("*No no no. Don't worry about that, It's all on [nickname]'s tab.*", new string[] { "My what?", "Wait, I never agreed with that!", "...if you say so..." }, Dialogue.GetParticipant(BlueID)))
                                {
                                    case 0:
                                        Dialogue.ShowDialogueWithContinue("*That's one good Terrarian, so reliable.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*Yes, they never disappoint me.*", Dialogue.GetParticipant(BlueID));
                                        break;
                                    case 1:
                                        switch (Dialogue.ShowDialogueWithOptions("*Come on, you plan on making me pay for that? It's for a good cause, you know*", new string[] { "Yes", "No..." }, Dialogue.GetParticipant(BlueID)))
                                        {
                                            case 0:
                                                Dialogue.ShowDialogueWithContinue("*...*", Dialogue.GetParticipant(BlueID));
                                                Dialogue.ShowDialogueWithContinue("*Now that's a complication. Hahaha.*", Dialogue.GetParticipant(ZacksID));
                                                Dialogue.ShowDialogueWithContinue("*Oh well, gladly I have my own money.*", Dialogue.GetParticipant(BlueID));
                                                break;
                                            case 1:
                                                Dialogue.ShowDialogueWithContinue("*Thank you, [nickname].*", Dialogue.GetParticipant(BlueID));
                                                Dialogue.ShowDialogueWithContinue("*At least now I know why you asked [nickname]'s help.*", Dialogue.GetParticipant(ZacksID));
                                                Dialogue.ShowDialogueWithContinue("*Wait a minute, It's not like that...*", Dialogue.GetParticipant(BlueID));
                                                break;
                                        }
                                        break;
                                    case 2:
                                        {
                                            Dialogue.ShowDialogueWithContinue("*Come on, don't sound so ungrateful. It's for a good cause, remember?*", Dialogue.GetParticipant(BlueID));
                                            Dialogue.ShowDialogueWithContinue("*But [gn:"+GuardianBase.Blue+"], I don't really mind my body being like... This...*", Dialogue.GetParticipant(ZacksID));
                                            Dialogue.ShowDialogueWithContinue("*Well, you should begin minding if you want me to keep sharing my bed with you.*", Dialogue.GetParticipant(BlueID));
                                            Dialogue.ShowDialogueWithContinue("*Uh... Good point... Help her, [nickname].*", Dialogue.GetParticipant(ZacksID));
                                        }
                                        break;
                                }
                                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                                {
                                    Dialogue.ShowEndDialogueMessage("*Better we get going then... I guess?*", true, Dialogue.GetParticipant(ZacksID));
                                }
                                else
                                {
                                    Dialogue.ShowEndDialogueMessage("*Better you two get going then... I guess?*", true, Dialogue.GetParticipant(ZacksID));
                                }
                            }
                            break;
                        case 4:
                        case 5:
                            {
                                Dialogue.ShowDialogueWithContinue("*We got the bandages, but we need to visit the clothier.*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*The clothier? Why?*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*You still need something to cover that hole in your chest, and I don't think the bandages will be helpful in this case.*", Dialogue.GetParticipant(BlueID));
                                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                                {
                                    Dialogue.ShowDialogueWithContinue("*This hole? Hm. I can only think of a shirt to solve this. Will you let me pick the shirt?*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*No.*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*Then... I guess I should trust your decision.*", Dialogue.GetParticipant(ZacksID));
                                }
                                else
                                {
                                    Dialogue.ShowDialogueWithContinue("*This hole? Hm. I can only think of a shirt to solve this. I hope you two pick a cool design one.*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*Ah, yes. Right...*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*Wait, what was that?*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*Nothing.*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*[gn:"+GuardianBase.Blue+"]...*", Dialogue.GetParticipant(ZacksID));
                                }
                                Dialogue.ShowEndDialogueMessage("*Don't worry, you wont be disappointed. (wink)*", true, Dialogue.GetParticipant(BlueID));
                            }
                            break;
                    }
                }
            }
            else //Blue triggered the dialogue.
            {
                bool HasZacksInTheParty = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks);
                bool Knows = false;
                if (HasZacksInTheParty)
                {
                    Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks));
                    Knows = data.QuestStep % 2 == 1;
                }
                const int BlueID = 0, ZacksID = 1;
                switch(data.QuestStep)
                {
                    case 2:
                    case 3:
                        {
                            Dialogue.ShowDialogueWithContinue("*You forgot already what we talked about? We need to visit the Nurse and get some bandages.*");
                            if (!NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                            {
                                if (HasZacksInTheParty)
                                {
                                    if (!Knows)
                                    {
                                        Dialogue.ShowDialogueWithContinue("*Nurse? I don't know why you need a Nurse, but this world doesn't seems to have one.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*Oh well, then either we wait for her to show up, or we visit a world that has a Nurse.*", Dialogue.GetParticipant(BlueID));
                                    }
                                    else
                                    {
                                        Dialogue.ShowDialogueWithContinue("*There is only one little problem about your plan: There is no Nurse in this world.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*That frustrates things a bit, but we could either see if one appears, or visit some place that has one.*", Dialogue.GetParticipant(BlueID));
                                    }
                                }
                                else
                                {
                                    Dialogue.ShowDialogueWithContinue("*Now that I remember... Here doesn't actually have a Nurse.*");
                                    Dialogue.ShowDialogueWithContinue("*What should we do, [nickname]? We should wait for one to appear, or visit another world to speak to one?*");
                                    Dialogue.ShowDialogueWithContinue("*I'll let you choose what you plan on doing.*");
                                }
                            }
                            else
                            {
                                if (HasZacksInTheParty)
                                {
                                    if (!Knows)
                                    {
                                        Dialogue.ShowDialogueWithContinue("*Bandages? What do you need bandages for?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*It's for you?*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*For me? Why? You plan on mummifying me or something?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*No no. It's to cover your wounds.*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*Ah, that's why. So... I shouldn't worry about being locked inside a sarcophagus, then?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*...[gn:" + GuardianBase.Zacks + "]...*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*Hey! That's a serious question!*", Dialogue.GetParticipant(ZacksID));
                                        data.QuestStep++;
                                    }
                                    else
                                    {
                                        Dialogue.ShowDialogueWithContinue("*You don't seems to have a very good memory, [nickname].\nMind if I analyze your head?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*That's not funny.*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*At least I can scare others using my current state.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*And sometimes you spook me out with those.*", Dialogue.GetParticipant(BlueID));
                                    }
                                }
                            }
                            Dialogue.ShowEndDialogueMessage("*Before we go, do you need to talk about something else, [nickname]?*", false, Dialogue.GetParticipant(BlueID));
                        }
                        break;
                    case 4:
                    case 5:
                        {
                            Dialogue.ShowDialogueWithContinue("*We already got the bandages, now we need to visit the Clothier.*");
                            if (!NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                            {
                                if (HasZacksInTheParty)
                                {
                                    Dialogue.ShowDialogueWithContinue("*A clothier? Here? I don't remember seeing any.*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*Hm, then I guess we should go look for one, then?*", Dialogue.GetParticipant(BlueID));
                                    if (!Knows)
                                    {
                                        Dialogue.ShowDialogueWithContinue("*And what is that of bandages? Why do you need them?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*You'll know soon.*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*I don't like much mystery, but I can wait to see why.*", Dialogue.GetParticipant(ZacksID));
                                    }
                                }
                                else
                                {
                                    Dialogue.ShowDialogueWithContinue("*Do you actually know any clothier? Your world doesn't seems to have one.*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*Either we could wait for one to appear, or visit a world with one. You pick.*", Dialogue.GetParticipant(BlueID));
                                }
                            }
                            else
                            {
                                if (HasZacksInTheParty)
                                {
                                    if (!Knows)
                                    {
                                        Dialogue.ShowDialogueWithContinue("*Bandages? Clothier? Those doesn't add up.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("*We're doing something for you, and you'll know what it is once I'm done.*", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*You should have lit the \"spoiler alert\" before speaking about that, then.*", Dialogue.GetParticipant(ZacksID));
                                        data.QuestStep++;
                                    }
                                    else
                                    {
                                        Dialogue.ShowDialogueWithContinue("*Come on, let's get going. I'm curious to see what you guys will pick for me.*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("(Gives an evil smile to Zacks.)", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*I don't like that look... What are you planning?*", Dialogue.GetParticipant(ZacksID));
                                        Dialogue.ShowDialogueWithContinue("Soon, Zackary... Soon.", Dialogue.GetParticipant(BlueID));
                                        Dialogue.ShowDialogueWithContinue("*She just said my first name. That can't be good.*", Dialogue.GetParticipant(ZacksID));
                                    }
                                }
                            }
                            Dialogue.ShowEndDialogueMessage("*Before we go, do you need to talk about something else, [nickname]?*", false, Dialogue.GetParticipant(BlueID));
                        }
                        break;
                    case 6:
                    case 7:
                        {
                            if (HasZacksInTheParty)
                            {
                                Dialogue.ShowDialogueWithContinue("*We have to give all those to Zacks.*");
                                Dialogue.ShowDialogueWithContinue("*I'm right here, you know.*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*See? Very convenient!*", Dialogue.GetParticipant(BlueID));
                                UponDeliveringToZacksDialogue();
                                return;
                            }
                            Dialogue.ShowEndDialogueMessage("*We should be seeing Zacks by now.*", false);
                            if (!NpcMod.HasGuardianNPC(GuardianBase.Zacks))
                            {
                                Dialogue.ShowDialogueWithContinue("*We now need to deliver those to Zacks, but he isn't here.*");
                                Dialogue.ShowDialogueWithContinue("*Could you call him so we deliver those?*");
                            }
                        }
                        break;
                }
            }
        }

        public static void AfterQuestZacks() //Needs work. Zacks dialogue looks rather bad.
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            Dialogue.SetImportantDialogue();
            TerraGuardian Blue = null, Zacks = Dialogue.GetSpeaker;
            if (PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Blue))
                Dialogue.AddParticipant(Blue = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Blue));
            switch (data.ZacksKnowsWhoPickedShirt)
            {
                case 0: //Don't know
                    Dialogue.ShowDialogueWithContinue("*I really appreciate that [gn:" + GuardianBase.Blue + "] and you managed to get things to cover my wounds.*");
                    Dialogue.ShowDialogueWithContinue("*But I really don't like this shirt calling me a \'Meat Bag\'.*");
                    switch (Dialogue.ShowDialogueWithOptions("*You didn't had anything to do with this shirt choice, right?*", new string[] {
                            "It was [gn:"+GuardianBase.Blue+"] who picked the shirt.",
                            "I didn't even had the chance.",
                            "Yes, I did."
                        }))
                    {
                        case 0:
                            if (Blue == null)
                            {
                                Dialogue.ShowDialogueWithContinue("*Ah, so it was a prank from her?*");
                                data.ZacksKnowsWhoPickedShirt += 2;
                                Dialogue.ShowEndDialogueMessage("*That means I'll need to think of a reply for that.*");
                            }
                            else
                            {
                                Dialogue.ShowDialogueWithContinue("*Ah, so you picked that shirt for me?*");
                                Dialogue.ShowDialogueWithContinue("*Looked like fitting *giggles*.*", Blue);
                                data.ZacksKnowsWhoPickedShirt += 2;
                                Dialogue.ShowDialogueWithContinue("*I guess I'll need to give a reply to this gift, right?*", Zacks);
                                Dialogue.ShowEndDialogueMessage("*I'll be waiting anxiously.*", true, Blue);
                            }
                            return;
                        case 1:
                            if (Blue == null)
                            {
                                Dialogue.ShowDialogueWithContinue("*Then It was [gn:" + GuardianBase.Blue + "] who picked the shirt?*");
                                Dialogue.ShowDialogueWithContinue("*Tell me, [nickname], were you going to give me something nice? I'm curious.*");
                                data.ZacksKnowsWhoPickedShirt += 2;
                                Dialogue.ShowEndDialogueMessage("*Hey, don't leave me curious. Oh well.. Better I plan out the reply to [gn:"+GuardianBase.Blue+"] then.*");
                            }
                            else
                            {
                                Dialogue.ShowDialogueWithContinue("*Then It was you.*");
                                Dialogue.ShowDialogueWithContinue("*Disappointed?*", Blue);
                                data.ZacksKnowsWhoPickedShirt += 2;
                                Dialogue.ShowDialogueWithContinue("*It's really unusual of you to prank on me, but I'll think of some reply to that.*", Zacks);
                                Dialogue.ShowEndDialogueMessage("*Surprise me, teehee.*", Speaker: Blue);
                            }
                            break;
                        case 2:
                            Dialogue.ShowDialogueWithContinue("*You did picked me that shirt, right? Hehe..*");
                            if (Blue != null)
                            {
                                Dialogue.ShowDialogueWithContinue("*([nickname], you shouldn't have said that.)*", Blue);
                            }
                            Dialogue.ShowDialogueWithContinue("*You don't really have any idea of who you pranked with, right?*");
                            data.ZacksKnowsWhoPickedShirt = 4;
                            if (Blue != null)
                            {
                                Dialogue.ShowDialogueWithContinue("*(Uh oh... It was nice knowing you, [nickname].)*", Blue);
                            }
                            Dialogue.ShowEndDialogueMessage("*Better you wait for my reply, [nickname]. It will be coming soon. I guarantee.*");
                            break;
                    }
                    break;
                case 1:
                    Dialogue.ShowDialogueWithContinue("*I really appreciate that [gn:" + GuardianBase.Blue + "] and you managed to get things to cover my wounds.*");
                    switch(Dialogue.ShowDialogueWithOptions("*But this shirt... I hate it.*", new string[] {
                        "But It was a gift.",
                        "It actually seems fitting.",
                        "Think of it as a gift from [gn:" +GuardianBase.Blue+"]." }))
                    {
                        case 0:
                            Dialogue.ShowDialogueWithContinue("*Yes, but the label really doesn't help.*");
                            if (Blue != null)
                            {
                                Dialogue.ShowDialogueWithContinue("*Rejecting my gift, are you?*", Blue);
                                Dialogue.ShowDialogueWithContinue("*No no no, that's not what I mea...*", Zacks);
                                Dialogue.ShowDialogueWithContinue("*Wait, you're joking, right?*", Zacks);
                                Dialogue.ShowDialogueWithContinue("(giggles)", Blue);
                            }
                            Dialogue.ShowDialogueWithContinue("*At least the patch up was really nice.*", Zacks);
                            data.ZacksKnowsWhoPickedShirt += 2;
                            Dialogue.ShowEndDialogueMessage("*But now I need to think of ways of showing my affection for this gift.*");
                            break;
                        case 1:
                            if (Blue != null)
                            {
                                Dialogue.ShowDialogueWithContinue("*Hahaha, I liked that reply, [nickname].*", Blue);
                            }
                            Dialogue.ShowDialogueWithContinue("*Very funny, [nickname].*", Zacks);
                            Dialogue.ShowDialogueWithContinue("*Beside the patch up really went well.*");
                            data.ZacksKnowsWhoPickedShirt += 2;
                            Dialogue.ShowEndDialogueMessage("*I need to think of my reply to this...*");
                            break;
                        case 2:
                            Dialogue.ShowDialogueWithContinue("*Yes, yes, I will.*");
                            Dialogue.ShowDialogueWithContinue("*A very heartfelt gift.*");
                            if (Blue != null)
                            {
                                Dialogue.ShowDialogueWithContinue("*I'm really glad that you think that way.*", Blue);
                            }
                            data.ZacksKnowsWhoPickedShirt += 2;
                            Dialogue.ShowEndDialogueMessage("*I'm grateful for this, but now I need to think of my reply for it.*", Speaker:Zacks);
                            break;
                    }
                    break;
            }
        }

        public static void UponDeliveringToZacksDialogue()
        {
            if(!PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue))
            {
                Dialogue.ShowEndDialogueMessage("*You say that Blue and You have something for me? But where is Blue? She's part of that too, right?*", true);
                return;
            }
            Dialogue.IsImportantDialogue();
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            TerraGuardian Blue = null, Zacks = null;
            for (int i = 0; i < Dialogue.DialogueParticipants.Length; i++)
            {
                if (Dialogue.DialogueParticipants[i].ModID == MainMod.mod.Name)
                {
                    if (Dialogue.DialogueParticipants[i].ID == GuardianBase.Blue)
                    {
                        Blue = Dialogue.DialogueParticipants[i];
                    }
                    if (Dialogue.DialogueParticipants[i].ID == GuardianBase.Zacks)
                    {
                        Zacks = Dialogue.DialogueParticipants[i];
                    }
                }
            }
            bool ZacksIsInTheTeam = PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Zacks);
            if (Blue == null)
            {
                Blue = NpcMod.GetGuardianNPCCharacter(GuardianBase.Blue);
            }
            if (Zacks == null)
            {
                Zacks = NpcMod.GetGuardianNPCCharacter(GuardianBase.Zacks);
            }
            if (!Dialogue.HasParticipant(Blue.ID, Blue.ModID))
                Dialogue.AddParticipant(Blue);
            if (!Dialogue.HasParticipant(Zacks.ID, Zacks.ModID))
                Dialogue.AddParticipant(Zacks);
            Dialogue.GatherAroundGuardian(Zacks);
            if (Zacks.UsingFurniture)
                Zacks.LeaveFurniture(false);
            bool ZacksKnow = data.QuestStep % 2 == 1;
            Dialogue.ShowDialogueWithContinue("*Zacks, we brought something for you.*", Blue);
            if (!ZacksKnow)
            {
                if (ZacksIsInTheTeam)
                {
                    Dialogue.ShowDialogueWithContinue("*What is it that you two brought me?*", Zacks);
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Something for me? What is It?*", Zacks);
                }
                Dialogue.ShowDialogueWithContinue("*We brought you bandages and a shirt.*", Blue);
                Dialogue.ShowDialogueWithContinue("*Bandages and a shirt? Why?*", Zacks);
                Dialogue.ShowDialogueWithContinue("*It's so we can cover those wounds of yours.*", Blue);
                Dialogue.ShowDialogueWithContinue("*Oh, that is actually... nice of you two.*", Zacks);
            }
            else
            {
                if (ZacksIsInTheTeam)
                {
                    Dialogue.ShowDialogueWithContinue("*Finally managed to get everything, right?*", Zacks);
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Finally managed to get everything you two were trying to get?*", Zacks);
                }
                Dialogue.ShowDialogueWithContinue("*Yes, everything is here.*", Blue);
            }
            Dialogue.ShowDialogueWithContinue("*Now let's patch up those wounds...*", Blue);
            Dialogue.ShowDialogueWithContinue("*Those bandages will surelly help covering the wounds.*", Zacks);
            Dialogue.ShowDialogueWithContinue("*...Blue... Why it's written \"Meat Bag\" in this shirt?*", Zacks);
            Dialogue.ShowDialogueWithContinue("*I don't know, it looked fitting.*", Blue);
            Dialogue.ShowDialogueWithContinue("*How fitting? Do I look like a meat bag?*", Zacks);
            Dialogue.ShowDialogueWithContinue("*A rotten one (giggles).*", Blue);
            Dialogue.ShowDialogueWithContinue("*I wont wear this.*", Zacks);
            Dialogue.ShowDialogueWithContinue("*Come on, don't be a child. I got all those things for you, just wear it, please.*", Blue);
            Dialogue.ShowDialogueWithContinue("*... Okay...*", Zacks);
            /*MainMod.ScreenColor = Microsoft.Xna.Framework.Color.Black;
            MainMod.ScreenColorAlpha = 1;
            Dialogue.ShowDialogueTimed("(Some washing, patching and cuff wearing later...)",null, 2500);
            //System.Threading.Thread.Sleep(2500);*/
            Zacks.OutfitID = Creatures.ZacksBase.MeatBagOutfitID;
            //MainMod.ScreenColorAlpha = 0;
            data.QuestStep = (byte)(ZacksKnow ? 9 : 8);
            Dialogue.ShowDialogueWithContinue("*... How do I look?*", Zacks);
            Dialogue.ShowDialogueWithContinue("*Perfect! Now you look less half eaten and gross.*", Blue);
            Blue.IncreaseFriendshipProgress(2);
            Zacks.IncreaseFriendshipProgress(2);
            Main.NewText("Zacks [Meat Bag] Outfit unlocked.");
            Dialogue.ShowEndDialogueMessage("*... I really hope you didn't helped her pick this shirt, [nickname].*", true, Zacks);
        }
    }
}
