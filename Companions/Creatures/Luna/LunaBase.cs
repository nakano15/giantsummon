using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using giantsummon.Companions.Luna;

namespace giantsummon.Companions
{
    public class LunaBase : GuardianBase
    {
        public LunaBase()
        {
            Name = "Luna";
            Description = "She can tell you about almost everything\nrelated to TerraGuardians.";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 84;
            DuckingHeight = 70;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 101f / 84;
            CompanionSlotWeight = 1.05f;
            Age = 19;
            SetBirthday(SEASON_AUTUMN, 17);
            Male = false;
            InitialMHP = 200; //950
            LifeCrystalHPBonus = 30;
            LifeFruitHPBonus = 15;
            Accuracy = 0.63f;
            Mass = 0.5f;
            MaxSpeed = 5f;
            Acceleration = 0.2f;
            SlowDown = 0.53f;
            CanDuck = true;
            ReverseMount = false;
            SetTerraGuardian();
            CallUnlockLevel = 0;
			
            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.CopperBroadsword);
            AddInitialItem(Terraria.ID.ItemID.RichMahoganyBow);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 5);

            //Animation Frames
            StandingFrame = 2;
            WalkingFrames = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
            PlayerMountedArmAnimation = JumpFrame = 11;
            HeavySwingFrames = new int[] { 16, 17, 18 };
            ItemUseFrames = new int[] { 12, 13, 14, 15 };
            DuckingFrame = 19;
            DuckingSwingFrames = new int[] { 20, 21, 22 };
            SittingFrame = 24;
            ChairSittingFrame = 23;
            ThroneSittingFrame = 25;
            BedSleepingFrame = 26;
            SleepingOffset.X = 16;
            ReviveFrame = 27;
            DownedFrame = 28;

            BackwardStanding = 29;
            BackwardRevive = 30;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(20, 34);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(23, 0);
            BodyFrontFrameSwap.Add(24, 0);

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(1, 1);
            RightArmFrontFrameSwap.Add(23, 1);
            RightArmFrontFrameSwap.Add(24, 1);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(12, 13, 3);
            LeftHandPoints.AddFramePoint2x(13, 33, 12);
            LeftHandPoints.AddFramePoint2x(14, 36, 22);
            LeftHandPoints.AddFramePoint2x(15, 30, 31);

            LeftHandPoints.AddFramePoint2x(16, 30, 4);
            LeftHandPoints.AddFramePoint2x(17, 40, 18);
            LeftHandPoints.AddFramePoint2x(18, 36, 42);

            LeftHandPoints.AddFramePoint2x(20, 11, 17);
            LeftHandPoints.AddFramePoint2x(21, 34, 18);
            LeftHandPoints.AddFramePoint2x(22, 30, 33);

            LeftHandPoints.AddFramePoint2x(24, 29, 28);

            LeftHandPoints.AddFramePoint2x(27, 33, 43);
            LeftHandPoints.AddFramePoint2x(30, 33, 43);

            //Right Arm
            RightHandPoints.AddFramePoint2x(12, 16, 3);
            RightHandPoints.AddFramePoint2x(13, 35, 12);
            RightHandPoints.AddFramePoint2x(14, 38, 22);
            RightHandPoints.AddFramePoint2x(15, 32, 31);

            RightHandPoints.AddFramePoint2x(16, 32, 4);
            RightHandPoints.AddFramePoint2x(17, 42, 18);
            RightHandPoints.AddFramePoint2x(18, 38, 42);

            RightHandPoints.AddFramePoint2x(20, 13, 17);
            RightHandPoints.AddFramePoint2x(21, 36, 18);
            RightHandPoints.AddFramePoint2x(22, 32, 33);

            RightHandPoints.AddFramePoint2x(27, 41, 43);
            RightHandPoints.AddFramePoint2x(30, 41, 43);

            //Mount Sit Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16, 16);
            MountShoulderPoints.AddFramePoint2x(17, 22, 20);
            MountShoulderPoints.AddFramePoint2x(18, 24, 25);

            MountShoulderPoints.AddFramePoint2x(19, 16, 23);
            MountShoulderPoints.AddFramePoint2x(20, 16, 23);
            MountShoulderPoints.AddFramePoint2x(21, 16, 23);
            MountShoulderPoints.AddFramePoint2x(22, 16, 23);

            MountShoulderPoints.AddFramePoint2x(25, 17, 19);

            MountShoulderPoints.AddFramePoint2x(27, 25, 27);
            MountShoulderPoints.AddFramePoint2x(30, 25, 27);

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(22, 12);

            HeadVanityPosition.AddFramePoint2x(17, 28, 18);
            HeadVanityPosition.AddFramePoint2x(18, 30, 24);

            HeadVanityPosition.AddFramePoint2x(19, 22, 19);
            HeadVanityPosition.AddFramePoint2x(20, 22, 19);
            HeadVanityPosition.AddFramePoint2x(21, 22, 19);
            HeadVanityPosition.AddFramePoint2x(22, 22, 19);

            HeadVanityPosition.AddFramePoint2x(27, 32, 27);
            HeadVanityPosition.AddFramePoint2x(30, 32, 27);
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            if(BodyPartID > 0 && !guardian.PlayerMounted && guardian.SelectedOffhand == -1 && guardian.ItemAnimationTime == 0 && Frame < 11)
            {
                if (Frame == 4 || Frame == 8)
                    Frame = 1;
                else
                    Frame = 0;
            }
        }

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> ExtraDialogues = new List<DialogueOption>();
            ExtraDialogues.Add(new DialogueOption("I have some questions.", TutoringDialogues.StartTutoringDialogue, true));
            ExtraDialogues.Add(new DialogueOption("Anything new recently?", GetRandomTip, true));
            return ExtraDialogues;
        }

        public void GetRandomTip()
        {
            Dialogue.ShowDialogueOnly(GuardianSpawnTip.GetRandomTip());
            GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
        }

        public override string MountUnlockMessage => "*Hey [nickname], if you want, I can carry you on my shoulder. My legs are bigger than yours, so may be faster if I carry you, while you tell me where to go.*";
        public override string ControlUnlockMessage => "*Please don't be shocked. If you ever need to do something dangerous that you need someone strong to do that, you can use me to do that. Don't worry, I trust you enough that my life is in your hands.*";

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Hello Terrarian. I'm "+guardian.Name+", and I can help you with questions related to TerraGuardians.*";
                case 1:
                    return "*Hi. I'm "+guardian.Name+". I know many things about TerraGuardians, so I can help you in case you have questions.*";
                case 2:
                    return "*Please don't be scared, I'm friendly. We TerraGuardians are generally friendly. If you ever have questions about us, you can ask me.*";
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (!player.GetModPlayer<PlayerMod>().TutorialDryadIntroduction)
            {
                Main.NewText(guardian.Name + " can help solve your questions. She also may know rummors about companions in your world.", Microsoft.Xna.Framework.Color.LightBlue);
                player.GetModPlayer<PlayerMod>().TutorialDryadIntroduction = true;
            }
            if (MainMod.IsPopularityContestRunning && !Main.bloodMoon)
            {
                Mes.Add("*The TerraGuardians Popularity Contest is running right now. Sorry, but I wont be hosting the event. Seek someone who is.*");
                string Hosts = "";
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Hosts += "Rococo";
                }
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    if (Hosts != "") Hosts += ", ";
                    Hosts += "Blue";
                }
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    if (Hosts != "") Hosts += ", ";
                    Hosts += "Mabel";
                }
                if(Hosts == "")
                {
                    Mes.Add("*If you're interessed in participating of the popularity contest, I'm saddened to inform you that no known hosts is present in this world.*\n(Check the mod thread or discord server for the voting link.)");
                }
                else
                {
                    Mes.Add("*If you're interessed in participating of the popularity contest, you can speak with those companions to access the voting: "+Hosts+"*");
                }
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Those creatures... They doesn't seem native from here. Where are they coming from?*");
                    Mes.Add("*Be careful [nickname], they look tough.*");
                }
                else if (!Main.raining)
                {
                    Mes.Add("*This weather is so perfect. Want some lecturing?*");
                    Mes.Add("*What a fine day. Need something, [nickname]?*");
                    Mes.Add("*This sun will do wonders to my fur.*");
                }
                else
                {
                    Mes.Add("*I guess I should stay home...*");
                    Mes.Add("*Hm... I wonder when the rain will go away...*");
                    Mes.Add("*Need some questions cleared? The rain may turn the explanation less tedious.*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*Grrr... Blood Moons makes female citizens angry. I don't know why, so don't ask!*");
                    Mes.Add("*I'm... Not in the mood of talking... I don't think I can answer questions right now...*");
                    Mes.Add("*Leave me be...*");
                }
                else if (!Main.raining)
                {
                    Mes.Add("*Just a few more time until I can get some sleep.*");
                    Mes.Add("*I think I have some time to answer some questions. What troubles your mind?*");
                    Mes.Add("*You came to visit me. Are you planning on doing a pillow fight?*");
                }
                else
                {
                    Mes.Add("*Aahhh... This will go nice with hours of sleep...*");
                    Mes.Add("*This is the perfect time of day to rain. Sleeping with the rain sound, and the cold air around is so great.*");
                    Mes.Add("*Need my aid in something?*");
                }
            }
            if (!Main.bloodMoon)
            {
                Mes.Add("*Any question you have, feel free to talk. I may have a answer to it.*");
                Mes.Add("*Sometimes I hear from people rumors of people around the world. They may be useful if you want to meet new people.*");
                Mes.Add("*Yes? Do you need something?*");
                Mes.Add("*I was expecting to see you!*");
                Mes.Add("*What can " + guardian.Name + " help you with?*");
                if (NpcMod.HasGuardianNPC(guardian.ID))
                {
                    Mes.Add("*I'm so happy that you let me live here. I love staying here with you Terrarians.*");
                }
                {
                    int TgNppCount = NpcMod.GetTerraGuardianNPCCount();
                    if (TgNppCount < 5)
                    {
                        Mes.Add("*I feel a little weird that there's not many people like me around. It's like as if I'm out of place.*");
                    }
                    else
                    {
                        if (TgNppCount >= 20)
                        {
                            Mes.Add("*Wow! There's a lot of TerraGuardians living here! It feel like I'm back home. I thank you for this.*");
                        }
                        else if (TgNppCount >= 10)
                        {
                            Mes.Add("*There's quite a number of TerraGuardians living in your world. I'm glad to see you Terrarians and TerraGuardians are living well together.*");
                        }
                        else
                        {
                            Mes.Add("*I like seeing that there's people like me living here. Don't get me wrong, I like being around Terrarians, but I don't really feel out of place when people like me are in the world.*");
                        }
                    }
                }

                if (guardian.IsUsingToilet)
                {
                    Mes.Clear();
                    Mes.Add("*Yes, I can answer your questions, at least as long as you don't stare at me doing my business.*");
                    Mes.Add("*It is really embarrassing having someone watch you when you're.... You know what... People will not like being talked to while... Doing that.*");
                    Mes.Add("*Sorry, but you're distracting me from what I'm doing right now. Can't we speak later?*");
                }
                else if (guardian.IsSleeping)
                {
                    Mes.Clear();
                    Mes.Add("(She seems to be sleeping comfortably, beside the way she sleeps doesn't look like it...)");
                    Mes.Add("(You can hear her snoring softly.)");
                    Mes.Add("(She's speaking about a number of things during her sleep. She seems to be dreaming about tutoring someone.)");
                }
                else
                {
                    if (guardian.IsPlayerRoomMate(player))
                    {
                        Mes.Add("*I'm really happy for having a room mate, but I don't know if we can share the same bed.*");
                        Mes.Add("*Don't worry, I can leave the beds tidy until sleep time.*");
                    }
                    if (NpcMod.HasGuardianNPC(Rococo))
                    {
                        Mes.Add("*I'm so happy that you let [gn:"+Rococo+"] stay around so many nice people, he deserves that.*");
                        Mes.Add("*I know [gn:"+Rococo+"], he's from my home town. People never cared about him, and he never managed to find a lair for him there. Only I gave him some affection some times.*");
                    }
                    if (NpcMod.HasGuardianNPC(Blue))
                    {
                        Mes.Add("*I really like talking with [gn:"+Blue+"], she never makes me bored when talking to her.*");
                        Mes.Add("*Just like [gn:"+Blue+"], I also care about my look. I just don't have long hair like she has.*");
                    }
                    if (NpcMod.HasGuardianNPC(Zacks))
                    {
                        Mes.Add("*Oh my, what happened to [gn:" + Zacks + "]?*");
                        Mes.Add("*I know [gn:" + Zacks + "] vaguelly, since I used to visit where he lived to collect some herbs.*");
                        Mes.Add("*I think... I think [gn:" + Zacks + "] has been stalking around my house during the night. I hope I'm wrong.*");
                    }
                    if (NpcMod.HasGuardianNPC(Alex))
                    {
                        Mes.Add("*What a cute doggie you managed to get to your world. I really love petting [gn:"+Alex+"]'s belly.*");
                    }
                    if (NpcMod.HasGuardianNPC(Mabel))
                    {
                        Mes.Add("*I don't mean to brag, but I wont one edition of Miss North Pole once. Maybe I can help [gn:"+Mabel+"] win an edition of it.*");
                        Mes.Add("*I didn't really needed to practice hard to participate of Miss North Pole. I think [gn:"+Mabel+"] is exagerating a bit.*");
                    }
                    if (NpcMod.HasGuardianNPC(Leopold))
                    {
                        Mes.Add("*Do you know about [gn:"+Leopold+"]? He's a really famous sage from the Ether Realm. Anyone knows him, but I only managed to talk to him when he moved to Terra Realm.*");
                        Mes.Add("*If you manage to get [gn:"+Leopold+"]'s mind focused on the discussion, I would be impressed.*");
                    }
                    if (NpcMod.HasGuardianNPC(Malisha))
                    {
                        Mes.Add("*If you ever wondered what a witch looks like, just look at [gn:"+Malisha+"]. Or at least she does look like one.*");
                        Mes.Add("*Why does [gn:"+Malisha+"] likes so much to wear nothing?*");
                    }
                    if (NpcMod.HasGuardianNPC(Fluffles))
                    {
                        Mes.Add("*At first, I was really scared about having [gn:" + Fluffles + "] around, but then I found out she's really nice.*");
                    }
                    if (NpcMod.HasGuardianNPC(Minerva))
                    {
                        Mes.Add("*I really love the food [gn:"+Minerva+"] makes, but I wish she cooked more things that don't have fat.*");
                    }
                    if (NpcMod.HasGuardianNPC(Liebre))
                    {
                        Mes.Add("*It seems like we got a \'grimm\' company on this world... I'm now expecting news of people death for some reason.*");
                        Mes.Add("*I know that [gn:"+Liebre+"] said that he didn't came here to kill us but... I don't feel very safe knowing he's around.*");
                    }
                    if (NpcMod.HasGuardianNPC(Alexander))
                    {
                        if (AlexanderBase.HasAlexanderSleuthedGuardian(player, guardian.ID))
                        {
                            Mes.Add("*How did [gn:" + Alexander + "] managed to know many things about me? I never spoke to anyone about most of it.*");
                        }
                        Mes.Add("*I really don't like people spying on me. If you can tell that to [gn:"+Alexander+"], I would be grateful.*");
                    }
                    if (NpcMod.HasGuardianNPC(Miguel))
                    {
                        Mes.Add("*It's really good to have [gn:"+Miguel+"] around. He's going to help me get fit.*");
                        Mes.Add("*Yes, I'm doing the exercises daily. Tell [gn:"+Miguel+"] to stop sending people to tell me that.*");
                    }
                    if (NpcMod.HasGuardianNPC(Cinnamon))
                    {
                        Mes.Add("*Oh my, [gn:" + Cinnamon + "] is so cute, that everytime I see her I want to hug.*");
                        Mes.Add("*I really wouldn't mind carrying [gn:" + Cinnamon + "] around. She's so cute that would look like I'm carrying a teddy.*");
                    }
                    if (NpcMod.HasGuardianNPC(Green))
                    {
                        Mes.Add("*Beside [gn:"+Green+"] has a menacing face, he's actually a good doctor. Visit him whenever you feel sick or hurt.*");
                    }
                    if (NpcMod.HasGuardianNPC(Cille))
                    {
                        Mes.Add("*I tried visitting [gn:"+Cille+"], but she always refuses my company. I even tried to make her cheer up. Beside she giggled a bit, she turned cold later, and told me to go away.*");
                        Mes.Add("*The other day I was lunching, until I noticed [gn:" + Cille + "] watching. I offered some to her, and she quickly gobbled up my food. I think she must have been really hungry.*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's not like as if I'm a nerd or anything, but I really like solving questions people have.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I don't have any request right now. Check me out later, and I may have something for you.*");
            Mes.Add("*Not right now. Do you want to talk about anything else?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Yes, I have something that I need your help with. Could you help me with.. [objective]?*");
            Mes.Add("*I'm so glad you asked, I was totally lost thinking about how I would solve that. Can you [objective] for me?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Thank you so much! You don't have any idea of how grateful I am right now. Thank you!*");
            Mes.Add("*I'm so happy that you managed to do what I asked. Thank you, [nickname].*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What is it, [nickname]? Want to call me to dance?*");
            Mes.Add("*I'm so happy that you all managed to organize this party.*");
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian))
            {
                Mes.Add("*Well... You know... It's usual that in birthday parties a gift is given by the guests. So... Did you brought me a gift?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*I... Ah... Thank you! You... Uh... You wont regret picking me. Thanks!*";
                case MessageIDs.RescueMessage:
                    return "*Don't worry, you're safe now. Just rest a bit.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Yawn.. Terrarian... It's late, do you need something?*";
                        case 1:
                            return "*I know that questions can rise any time, but couldn't you wait until tomorrow?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Huh, oh? So... Did you do my request?*";
                        case 1:
                            return "*I'm so sleepy... Tell me you completed my request...*";
                    }
                    break;
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I'd be happy to accompany you on your adventure. Let's go.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I'd feel uncomfortable in the middle of so many people...*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Sorry, but I can't join you right now...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I don't like being left here, can't I leave the group in a safe place?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Alright, I'll be heading home then. If you ever have any question, come see me.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I'll try fighting my way home, then.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Alright. When we get into a town, you can ask me to leave again, if you want.*";
                case MessageIDs.RequestAccepted:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I'll be cheering for you.*";
                    return "*Go, [nickname]. Go.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*[nickname], I recommend you not to overload yourself with several requests, that's detrimental to your health. My request can wait until you take care of one of them.*";
                case MessageIDs.RequestRejected:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*Aww.... Alright. So... Want to talk about something else?*";
                    return "*No? Oh... I'll do that later then.*";
                case MessageIDs.RequestPostpone:
                    return "*Well, there's no time limit, so It can wait.*";
                case MessageIDs.RequestFailed:
                    return "*You couldn't manage to complete my request... I'm... I'm sorry... So... Anything else you need...?*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*I'm so happy. You completed my request, right?*";
                case MessageIDs.RequestRemindObjective:
                    return "*Did you forget what I asked you to do? It's fine, I'll tell you again. I asked you to [objective]. Don't hesitate to come back to me if you forget again.*";
                case MessageIDs.RestAskForHowLong:
                    return "*Feeling sleepy too? Let's refill our energy for the next adventure then. How long should we sleep?*";
                case MessageIDs.RestNotPossible:
                    return "*I don't think this is a good moment to rest.*";
                case MessageIDs.RestWhenGoingSleep:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*Gladly I can keep you warm meanwhile. I'm used to cold, by the way.*";
                    return "*Let's hope nothing interrupt our rest then.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Ooooohhh!! Look at that! Let's check [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Let me see...*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank You!*";
                case MessageIDs.ChatAboutSomething:
                    return "*Want to know something else from me? Let's talk then.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Alright, need anything else?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*Are you sure that you want to cancel my request? You can't complete it or is it too hard?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Oh... I guess I should try doing it instead then... Alright, you no longer need to do my request.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Then you asked that by mistake? Alright. Do you need anything else?*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Let's see what can I learn about you...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... She has a sweet scent.*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*I'm getting a bit uncomfortable, better I end this sleuthing quickly.*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Alright, I think I know more about you than yourself, now.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... Not in the face! Not in the face!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() <= 0.5f)
                        return "*Thank you all, I really mean it.*";
                    return "*I'm so grateful to have you all to help me.*";
                case MessageIDs.RevivedByRecovery:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I'm still a bit hurt... But I could have got some help...*";
                    return "*I'm awake, this hurts a lot...*";
                //
                case MessageIDs.LeopoldMessage1:
                    return "*Leopold! That's an extremelly rude assumption!*";
                case MessageIDs.LeopoldMessage2:
                    return "*What do you mean? That means you're not that Terrarian's hostage?*";
                case MessageIDs.LeopoldMessage3:
                    return "*I joined their adventure because they asked me to, and I accepted. Where did that crazy story of yours came from?*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*I'm not feeling good..*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Aaahhh!! My fur!!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes!!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head is spinning...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't attack!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My feet feels sluggish.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I feel weak...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Argh! My chest!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What in the underworld is that?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hey! I'm not a toilet!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*C-cold... This c-cold...*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Someone help me!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Ack! Damn you!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*This will alleviate damages.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Yummy! Delicious.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*I think I got some more muscles from this.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Be prepared to watch my lovely behind!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I feel better now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*DEX UP!*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Better I be careful not to hurt myself with the sword.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Hic!*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I could lick this all day.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Life Crystal over there!*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Watch your step!*";
                case MessageIDs.FoundMineTile:
                    return "*Mine over there!*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Don't press that! Watch first if it's not connected to anything.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I can wonder something will be furious if you break that.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I'll do my best.*";
                case MessageIDs.FoundTreasureTile:
                    return "*I think there's something valuable in there.*";
                case MessageIDs.FoundGemTile:
                    return "*Beautiful gems over there.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I found some interesting ores here.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Check out, some rare ores here.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Look! Minecart rails. Want to ride on it?*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Back home we go, then.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Welcome, I hope we keep you for a while.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*That's nice, [nickname]. We just met someone new.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Rise, minion! Protect your mentor!*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Well... I'm glad he's friendly. Someone got a camera?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Terrarian fell!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*No!! [nickname]!!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*An ally is down!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Look! That looks rare!*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be back soon.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Watch your health, [nickname]!*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Vision getting faint....*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have few potions left.*";
                case MessageIDs.UsesLastPotion:
                    return "*My potions! Where did they go?*";
                case MessageIDs.SpottedABoss:
                    return "*Trouble incoming!*";
                case MessageIDs.DefeatedABoss:
                    return "*Easily dealt, right [nickname]?*";
                case MessageIDs.InvasionBegins:
                    return "*They don't have a friendly face. Ready yourself.*";
                case MessageIDs.RepelledInvasion:
                    return "*We're safe now...*";
                case MessageIDs.EventBegins:
                    return "*That doesn't look good...*";
                case MessageIDs.EventEnds:
                    return "*I'm glad it's over...*";
                case MessageIDs.RescueComingMessage:
                    return "*Hang on, I'll help you!*";
                case MessageIDs.RescueGotMessage:
                    return "*Got 'em. Now we need a safe place.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*Hey, have you heard of [player]? Well, they have heard a lot from me.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*It seems like [player] took care of [subject] recently.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*I heard that [player] found a [subject]. That must be quite rare, since people are speaking about It around the worlds.*";
                case MessageIDs.FeatEventFinished:
                    return "*Looks like [player] survived a [subject] that happened in their world.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*Did you hear? [player] met [subject] during their travels.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I heard about [player]'s demise... Everyone's heartbroken due to that, including me.*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] has opened a temple in [subject] world. Do you think they found treasures there?*";
                case MessageIDs.FeatCoinPortal:
                    return "*A coin portal suddenly appeared for [player]. That's really impressive.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I have met someone new recently. They are called [player]. Do you know them?*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*It seems like [player] is really addicted into doing angler kid's requests.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*The Legend of [player]. Sounds poetic, but that Terrarian saved their world from [subject]. And I help spreading the story to other people.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*Looks like things changed in [subject] world, [player] killed a grotesque creature and things... Changed...*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Hey, did you hear? [player] picked me as their buddy. I'm still surprised that happened.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I'm still shocked that you picked me, of everyone else, as your buddy. Yes! Yes that I want to be your buddy! You don't know how much meaning this has to me.*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*Wow! [subject] got a house on [world]. That's cool, right?*";
                case MessageIDs.DeliveryGiveItem:
                    return "*I know you need some [item], have them [target].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*Where did the item go? It was right here a while ago.*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*I can't give you an item until you clean your inventory, [target].*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
