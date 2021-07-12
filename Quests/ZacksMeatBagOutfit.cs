using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Quests
{
    public class ZacksMeatBagOutfit
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

        public static void BlueWhenListeningToHerRequest()
        {
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
                Dialogue.ShowDialogueWithContinue("*Oh, yeah... This is really nasty thing to look at.*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*We need to patch up the wounds on your arms and feet, and wash the blood in your paws... Is that even your blood?*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*I... Don't think... It is?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*We definitelly have to wash that off.*", Dialogue.GetParticipant(BlueSlot));
            }
            Dialogue.ShowDialogueWithContinue("*We need something to patch up his wounds, and I think the Nurse may help us with that.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowDialogueWithContinue("*Then we will need to visit the Clothier, and see if we can find something to cover the hole on his chest.*");
            if (ZacksInTheTeam)
            {
                Dialogue.ShowDialogueWithContinue("*That actually sounds like a good plan, I like it.*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ChangeSpeaker(BlueSlot);
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep = 3;
            }
            else
            {
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep = 2;
            }
            if (!PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue))
            {
                Dialogue.ShowDialogueOnly("*And by \"we\", that means I need to come too.*");
                GuardianMouseOverAndDialogueInterface.GetDefaultOptions(Dialogue.GetParticipant(BlueSlot));
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*So, shall we go?*", true);
            }
        }

        public static void UponAskingAboutRequest()
        {
            if(Dialogue.GetSpeaker().ID == GuardianBase.Zacks) //Zacks triggered the dialogue
            {
                bool Knows = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep % 2 == 1;
                if (!Knows)
                {
                    Dialogue.ShowDialogueWithContinue("*Blue wants to patch up my wounds?*");
                    bool HasBlueInTheParty = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue);
                    const int ZacksID = 0, BlueID = 1;
                    if (HasBlueInTheParty)
                    {
                        Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], GuardianBase.Blue));
                        Dialogue.ShowDialogueWithContinue("*Yes. Your wounds aren't good things to look at, and that hole in your chest needs to be covered.*", Dialogue.GetParticipant(BlueID));
                        Dialogue.ShowDialogueWithContinue("*You have a point. Okay, I guess I should wait to see what you two will bring me, then.*", Dialogue.GetParticipant(ZacksID));
                        Dialogue.ShowDialogueWithContinue("*Don't worry, you wont be disappointed. (She gives a discrete smile)*", Dialogue.GetParticipant(BlueID));
                        Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep++;
                        Dialogue.ShowEndDialogueMessage("*Uh oh.*", false, Dialogue.GetParticipant(ZacksID));
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("*Is she worried that I may be in pain or something? I already said that I can't feel anything in this state.*");
                        Dialogue.ShowDialogueWithContinue("*Maybe I'm worrying her again...?*");
                    }
                }
                else
                {
                    bool HasBlueInTheParty = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], GuardianBase.Blue);
                    const int ZacksID = 0, BlueID = 1;
                    Dialogue.ShowDialogueWithContinue("*So, have you two managed to find something to take care of my wounds?*");
                    // Place here branching dialogues based on the current progress in the quest.
                    switch (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep)
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
                                        break;
                                    case 1:
                                        switch (Dialogue.ShowDialogueWithOptions("*Come on, you plan on making me pay for that? It's for a good cause, you know*", new string[] { "Yes", "No" }, Dialogue.GetParticipant(ZacksID)))
                                        {
                                            case 0:
                                                Dialogue.ShowDialogueWithContinue("*...*", Dialogue.GetParticipant(BlueID));
                                                Dialogue.ShowDialogueWithContinue("*Now that's a complication.*", Dialogue.GetParticipant(ZacksID));
                                                break;
                                            case 1:
                                                Dialogue.ShowDialogueWithContinue("*Thank you, [nickname].*", Dialogue.GetParticipant(BlueID));
                                                break;
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
                                Dialogue.ShowDialogueWithContinue("*You need something to cover that hole in your chest*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*This hole? Hm. I can only think of a shirt to solve this. I hope you pick a cool designed one.*", Dialogue.GetParticipant(ZacksID));
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
                    Knows = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep % 2 == 1;
                }
                const int BlueID = 0, ZacksID = 1;
                switch(Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep)
                {
                    case 2:
                    case 3:
                        {
                            Dialogue.ShowDialogueWithContinue("*You forgot already what we talked about? We need to visit the Nurse and get some bandages.*");
                            if (HasZacksInTheParty && !Knows)
                            {
                                Dialogue.ShowDialogueWithContinue("*Bandages? What do you need bandages for?*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*It's for you.*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*For me? What? You plan on mummifying me or something?*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*No no. It's to take care of your wounds.*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*Ah, that's why. So... I shouldn't worry about being locked inside a sarcophagus, then?*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*...*", Dialogue.GetParticipant(BlueID));
                                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep++;
                            }
                            Dialogue.ShowEndDialogueMessage("*Before we go, do you need to talk about something else?*", false);
                        }
                        break;
                    case 4:
                    case 5:
                        {
                            Dialogue.ShowDialogueWithContinue("*We already got the bandages, now we need to visit the Clothier.*");
                            if (HasZacksInTheParty && !Knows)
                            {
                                Dialogue.ShowDialogueWithContinue("*Bandages? Clothier? Those doesn't add up*", Dialogue.GetParticipant(ZacksID));
                                Dialogue.ShowDialogueWithContinue("*We're doing something for you, and you'll know what it is once I'm done.*", Dialogue.GetParticipant(BlueID));
                                Dialogue.ShowDialogueWithContinue("*No spoiler, then...?*", Dialogue.GetParticipant(ZacksID));
                                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep++;
                            }
                            Dialogue.ShowEndDialogueMessage("*Before we go, do you need to talk about something else?*", false);
                        }
                        break;
                    case 6:
                    case 7:
                        {
                            if (HasZacksInTheParty)
                            {
                                Dialogue.ShowDialogueWithContinue("*We have to give all those to Zacks. Gladly, you brought him with us, so this should be easier.*");
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
                Dialogue.ShowEndDialogueMessage("*You say that Blue and You have something for me? Where is Blue? She's part of it too, right?*", true);
                return;
            }
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
            bool ZacksKnow = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep % 2 == 1;
            Dialogue.ShowDialogueWithContinue("*Zacks, we brought something for you.*", Dialogue.GetParticipant(BlueSlot));
            if (!ZacksKnow)
            {
                Dialogue.ShowDialogueWithContinue("*Something for me? What is It?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*We brought you bandages and a shirt.*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*Bandages and a shirt? Why?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*It's so we can cover those wounds of yours.*", Dialogue.GetParticipant(BlueSlot));
                Dialogue.ShowDialogueWithContinue("*Oh, that is actually nice of you two.*", Dialogue.GetParticipant(ZacksSlot));
            }
            else
            {
                Dialogue.ShowDialogueWithContinue("*Finally managed to get everything you two were trying to get?*", Dialogue.GetParticipant(ZacksSlot));
                Dialogue.ShowDialogueWithContinue("*Yes, everything is here.*", Dialogue.GetParticipant(BlueSlot));
            }
            Dialogue.ShowDialogueWithContinue("*Now let's patch up those wounds...*", Dialogue.GetParticipant(BlueSlot));
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
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep = 8;
            Dialogue.ShowDialogueWithContinue("*... How do I look?*", Dialogue.GetParticipant(ZacksSlot));
            Dialogue.ShowDialogueWithContinue("*Perfect! Now you look less half eaten and gross.*", Dialogue.GetParticipant(BlueSlot));
            Dialogue.ShowEndDialogueMessage("*... I really hope you didn't helped her pick this shirt, [nickname].*", true, Dialogue.GetParticipant(ZacksSlot));
            Main.NewText("Zacks [Meat Bag] Outfit unlocked.");
        }
    }
}
