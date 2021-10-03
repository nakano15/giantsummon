﻿using System;
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
                Story += "Blue told me that she wanted to talk to me about something.";
                if (StoryStep >= 2)
                    Story += " She told me that wants to do something about the open wounds on Zacks body, but needs my help to get a number of things for this.";
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
                    Story += "\n\nBlue told me that we should visit the Nurse and get some Bandages, she gave them some of the Bandages she had spare.";
                }
            }
            if (StoryStep >= 4)
            {
                if (StoryStep < 6)
                {
                    Story += " Now we need to visit the Clothier to try getting a shirt.";
                }
                else
                {
                    Story += " We then went to visit the Clothier to pick a shirt, Blue seems to have found one, which she said that was perfect for Zacks.";
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
                    Story += "\n\nWe delivered the Bandages and the Shirt to Zacks. Blue managed to patch his wounds, and gave him a shirt to cover the hole on his chest.\nZacks didn't liked the fact that \"Meat Bag\" was written in his shirt, but at least Blue laughed in the end, and he got the wounds all covered up.\n\nTHE END";
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
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)PlayerMod.GetQuestData(Main.LocalPlayer, 0);
            return data.QuestStep >= 8;
        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> AddDialogueOptions(bool IsChatDialogue, int GuardianID, string GuardianModID)
        {
            if (!IsChatDialogue && GuardianModID == MainMod.mod.Name && (GuardianID == GuardianBase.Blue || GuardianID == GuardianBase.Zacks))
            {
                ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
                List<GuardianMouseOverAndDialogueInterface.DialogueOption> dialogues = base.AddDialogueOptions(IsChatDialogue, GuardianID, GuardianModID);
                if(GuardianID == GuardianBase.Blue) //If is Blue
                {
                    if (data.QuestStep == 1)
                    {
                        dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("What do you need help with?", BlueWhenListeningToHerRequest));
                    }
                    else if (data.QuestStep > 1 && data.QuestStep < 8)
                    {
                        dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("What should we be doing now?", UponAskingAboutRequest));
                    }
                }
                else //If is Zacks
                {
                    if (data.QuestStep >= 2 && data.QuestStep < 8)
                    {
                        dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("About Blue's request related to you.", UponAskingAboutRequest));
                    }
                    else if (data.QuestStep == 6 || data.QuestStep == 7)
                    {
                        dialogues.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("We have something to give you.", UponDeliveringToZacksDialogue));
                    }
                }
                return dialogues;
            }
            return base.AddDialogueOptions(IsChatDialogue, GuardianID, GuardianModID);
        }

        public override Action ImportantDialogueMessage(QuestData data, int GuardianID, string GuardianModID)
        {
            if (GuardianModID == MainMod.mod.Name && GuardianID == GuardianBase.Blue)
            {
                if(NpcMod.HasGuardianNPC(GuardianBase.Zacks) && ((ZacksOutfitQuestData)data).QuestStep == 0)
                {
                    return new Action(BlueTellsYouAboutHerRequest);
                }
            }
            return base.ImportantDialogueMessage(data, GuardianID, GuardianModID);
        }

        public override QuestData GetQuestData => new ZacksOutfitQuestData();

        public class ZacksOutfitQuestData : QuestData
        {
            public byte QuestStep = 0;
            private const short DataVersion = 0;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey + "_Version", DataVersion);
                Writer.Add(QuestKey + "_Step", QuestStep);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                short Version = Reader.GetShort(QuestKey + "_Version");
                QuestStep = Reader.GetByte(QuestKey + "_Step");
            }
        }

        public override string QuestNpcDialogue(NPC npc)
        {
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            if (PlayerMod.PlayerHasGuardian(Terraria.Main.LocalPlayer, GuardianBase.Blue))
            {
                if ((data.QuestStep == 4 || data.QuestStep == 5) && npc.type == Terraria.ID.NPCID.Clothier)
                {
                    data.QuestStep += 2;
                    PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue).SaySomethingCanSchedule("*This one! This one is perfect. I'll take It. Thank you. Let's give It to Zacks.*", true, 90);
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                    {
                        PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks).SaySomethingCanSchedule("*I hope you picked something cool for me.*", true, 90 + 150);
                    }
                    return "So, you want a shirt for " + NpcMod.GetGuardianNPCName(GuardianBase.Zacks) + "? I have quite a selection, feel free to browse.";
                }
                if ((data.QuestStep == 2 || data.QuestStep == 3) && npc.type == Terraria.ID.NPCID.Nurse)
                {
                    data.QuestStep += 2;
                    PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue).SaySomethingCanSchedule("*That's perfect! Thanks " + npc.GivenOrTypeName + ". Now we should visit the Clothier.*", true, 90);
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Zacks))
                    {
                        PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks).SaySomethingCanSchedule("*That's a lot of bandages! Just how wounded am I? I could even use some to wipe myself.*", true, 90 + 150);
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
            Dialogue.ShowDialogueWithContinue("*I'm glad for Zacks back into my life, but I think we should do something about his wounds.*");
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
            if (Dialogue.GetSpeaker.ID == GuardianBase.Zacks) //Zacks triggered the dialogue
            {
                bool Knows = data.QuestStep % 2 == 1;
                bool HasBlueInTheParty = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue);
                if (!Knows)
                {
                    Dialogue.ShowDialogueWithContinue("*Blue wants to patch up my wounds?*");
                    const int ZacksID = 0, BlueID = 1;
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
                        Dialogue.ShowDialogueWithContinue("*I don't know... Maybe I should wait to see what is it about.*");
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
                            if (HasZacksInTheParty)
                            {
                                if (!Knows)
                                {
                                    Dialogue.ShowDialogueWithContinue("*Bandages? What do you need bandages for?*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*It's for you?*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*For me? Why? You plan on mummifying me or something?*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*No no. It's to cover your wounds.*", Dialogue.GetParticipant(BlueID));
                                    Dialogue.ShowDialogueWithContinue("*Ah, that's why. So... I shouldn't worry about being locked inside a sarcophagus, then?*", Dialogue.GetParticipant(ZacksID));
                                    Dialogue.ShowDialogueWithContinue("*...[gn:"+GuardianBase.Zacks+"]...*", Dialogue.GetParticipant(BlueID));
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
                            Dialogue.ShowEndDialogueMessage("*Before we go, do you need to talk about something else, [nickname]?*", false, Dialogue.GetParticipant(BlueID));
                        }
                        break;
                    case 4:
                    case 5:
                        {
                            Dialogue.ShowDialogueWithContinue("*We already got the bandages, now we need to visit the Clothier.*");
                            if (HasZacksInTheParty)
                            {
                                if (!Knows)
                                {
                                    Dialogue.ShowDialogueWithContinue("*Bandages? Clothier? Those doesn't add up*", Dialogue.GetParticipant(ZacksID));
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
                        }
                        break;
                }
            }
        }

        public static void UponDeliveringToZacksDialogue()
        {
            if(!PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue))
            {
                Dialogue.ShowEndDialogueMessage("*You say that Blue and You have something for me? But where is Blue? She's part of that too, right?*", true);
                return;
            }
            ZacksOutfitQuestData data = (ZacksOutfitQuestData)Data;
            int BlueSlot = -1, ZacksSlot = -1;
            for (int i = 0; i < Dialogue.DialogueParticipants.Length; i++)
            {
                if (Dialogue.DialogueParticipants[i].ID == GuardianBase.Blue)
                {
                    BlueSlot = i;
                }
                if (Dialogue.DialogueParticipants[i].ID == GuardianBase.Zacks)
                {
                    ZacksSlot = i;
                }
            }
            bool ZacksIsInTheTeam = PlayerMod.HasGuardianSummoned(Main.LocalPlayer, GuardianBase.Zacks);
            Dialogue.CenterType = Dialogue.GatherCenterType.GatherAroundGuardian;
            Dialogue.GatherGuardian = Dialogue.GetParticipant(ZacksSlot);
            if (!Dialogue.HasParticipant(GuardianBase.Blue))
            {
                BlueSlot = Dialogue.DialogueParticipants.Length;
                Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue));
            }
            if (!Dialogue.HasParticipant(GuardianBase.Zacks))
            {
                ZacksSlot = Dialogue.DialogueParticipants.Length;
                Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Zacks));
            }
            bool ZacksKnow = data.QuestStep % 2 == 1;
            Dialogue.ShowDialogueWithContinue("*Zacks, we brought something for you.*", Dialogue.GetParticipant(BlueSlot));
            if (!ZacksKnow)
            {
                if (ZacksIsInTheTeam)
                {
                    Dialogue.ShowDialogueWithContinue("*What is it that you two brought me?*", Dialogue.GetParticipant(ZacksSlot));
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Something for me? What is It?*", Dialogue.GetParticipant(ZacksSlot));
                }
                Dialogue.ShowDialogueWithContinue("*We brought you bandages and a shirt.*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*Bandages and a shirt? Why?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*It's so we can cover those wounds of yours.*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*Oh, that is actually... nice of you two.*", Dialogue.GetParticipant(ZacksSlot));
            }
            else
            {
                if (ZacksIsInTheTeam)
                {
                    Dialogue.ShowDialogueWithContinue("*Finally managed to get everything, right?*", Dialogue.GetParticipant(ZacksSlot));
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Finally managed to get everything you two were trying to get?*", Dialogue.GetParticipant(ZacksSlot));
                }
                Dialogue.ShowDialogueWithContinue("*Yes, everything is here.*", Dialogue.GetParticipant(BlueSlot));
            }
            Dialogue.ShowDialogueWithContinue("*Now let's patch up those wounds...*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*Those bandages will surelly help covering the wounds.*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*...Blue... Why it's written \"Meat Bag\" in this shirt?*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*I don't know, it looked fitting.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*How fitting? Do I look like a meat bag?*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*A rotten one (giggles).*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*I wont wear this.*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*Come on, don't be a child. I got all those things for you, just wear it, please.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*... Okay...*", Dialogue.GetParticipant(ZacksSlot));
            /*MainMod.ScreenColor = Microsoft.Xna.Framework.Color.Black;
            MainMod.ScreenColorAlpha = 1;
            Dialogue.ShowDialogueTimed("(Some washing, patching and cuff wearing later...)",null, 2500);
            //System.Threading.Thread.Sleep(2500);*/
            Dialogue.GetParticipant(ZacksSlot).OutfitID = Creatures.ZacksBase.MeatBagOutfitID;
            //MainMod.ScreenColorAlpha = 0;
            data.QuestStep = (byte)(ZacksKnow ? 9 : 8);
            Dialogue.ShowDialogueWithContinue("*... How do I look?*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*Perfect! Now you look less half eaten and gross.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowEndDialogueMessage("*... I really hope you didn't helped her pick this shirt, [nickname].*", true, Dialogue.GetParticipant(ZacksSlot));
            Main.NewText("Zacks [Meat Bag] Outfit unlocked.");
        }
    }
}
