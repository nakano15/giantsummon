﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class AlexBase : GuardianBase
    {
        public const int HealingLickAction = 0;
        /// <summary>
        /// -Very playful.
        /// -Blames himself for his old partner's demise.
        /// -Bad at protecting people.
        /// -Extremely sociable.
        /// </summary>

        public AlexBase()
        {
            Name = "Alex";
            Description = "Your new best friend - a very playful one.";
            Size = GuardianSize.Large;
            Width = 68;
            Height = 62;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 42; //Age, 42. Mental Age, Real Age / 9
            SetBirthday(SEASON_WINTER, 19);
            Male = true;
            InitialMHP = 175; //1125
            LifeCrystalHPBonus = 30;
            LifeFruitHPBonus = 25;
            Accuracy = 0.36f;
            Mass = 0.7f;
            MaxSpeed = 5.65f;
            Acceleration = 0.33f;
            SlowDown = 0.83f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.71f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = false;
            DontUseRightHand = false;
            ForceWeaponUseOnMainHand = true;
            SetTerraGuardian();
            GroupID = GiantDogGuardianGroupID;
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);
            CallUnlockLevel = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 4;

            AddInitialItem(Terraria.ID.ItemID.TungstenBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 14;
            ChairSittingFrame = 31;
            SittingItemUseFrames = new int[] { 15, 16, 17 };
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 15 });
            ThroneSittingFrame = 22;
            BedSleepingFrame = 23;
            SleepingOffset.X = 16;
            ReviveFrame = 20;
            DownedFrame = 29;
            PetrifiedFrame = 30;

            BackwardStanding = 32;
            BackwardRevive = 33;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);

            //for(int f = 0; f < 9; f++)
            //    RightArmFrontFrameSwap.Add(f, 0);
            //RightArmFrontFrameSwap.Add(99, 1);
            //RightArmFrontFrameSwap.Add(100, 2);

            //Mount Player Sit Position
            MountShoulderPoints.DefaultCoordinate2x = new Point(30, 25);
            MountShoulderPoints.AddFramePoint2x(14, 13, 20);
            SittingPoint = new Point(20 * 2, 41 * 2); //14, 40 > 20,41

            //Left Arm Item Position
            LeftHandPoints.AddFramePoint2x(10, 37, 28);
            LeftHandPoints.AddFramePoint2x(11, 41, 31);
            LeftHandPoints.AddFramePoint2x(12, 42, 37);
            LeftHandPoints.AddFramePoint2x(13, 39, 43);

            LeftHandPoints.AddFramePoint2x(15, 24 + 6, 14 + 1);
            LeftHandPoints.AddFramePoint2x(16, 29 + 6, 27 + 1);
            LeftHandPoints.AddFramePoint2x(17, 22 + 6, 36 + 1);

            LeftHandPoints.AddFramePoint2x(19, 38, 47);
            LeftHandPoints.AddFramePoint2x(20, 38, 47);
            LeftHandPoints.AddFramePoint2x(21, 38, 47);
            LeftHandPoints.AddFramePoint2x(24, 38, 47);
            LeftHandPoints.AddFramePoint2x(25, 38, 47);
            LeftHandPoints.AddFramePoint2x(26, 38, 47);
            LeftHandPoints.AddFramePoint2x(27, 38, 47);
            LeftHandPoints.AddFramePoint2x(28, 38, 47);

            //Mouth Item Position
            RightHandPoints.DefaultCoordinate2x = new Point(43, 29);
            RightHandPoints.AddFramePoint2x(2, 31, 24);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(30, 27);
            WingPosition.AddFramePoint2x(14, 17 + 6, 25 + 1);
            WingPosition.AddFramePoint2x(18, 17 + 6, 25 + 1);

            //Helmet Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(38, 23);
            HeadVanityPosition.AddFramePoint2x(14, 21 + 6, 17 + 1);
            HeadVanityPosition.AddFramePoint2x(15, 21 + 6, 17 + 1);
            HeadVanityPosition.AddFramePoint2x(16, 21 + 6, 17 + 1);
            HeadVanityPosition.AddFramePoint2x(17, 21 + 6, 17 + 1);
            HeadVanityPosition.AddFramePoint2x(18, 21 + 6, 17 + 1);
            HeadVanityPosition.AddFramePoint2x(22, 23, 17);

            HeadVanityPosition.AddFramePoint2x(19, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(20, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(21, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(19, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(24, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(25, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(26, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(27, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(28, -1000, -1000);
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "Hey, let's play Dragon Fighter? Mount on my back so we can be like Knight and Steed!";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "I care so much for you, I don't want to see any harm come to you. If you need to do something dangerous, let me go do it instead.";
            }
        }
        
        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if (AnimationID == 2)
            {
                if (guardian.BodyAnimationFrame == ThroneSittingFrame || guardian.BodyAnimationFrame == BedSleepingFrame)
                {
                    Frame = 0;
                }
                else if (guardian.SelectedOffhand > -1)
                {
                    if (guardian.BodyAnimationFrame == SittingFrame || guardian.BodyAnimationFrame == ChairSittingFrame)
                    {
                        Frame = 2;
                    }
                    else
                    {
                        Frame = 1;
                    }
                }
                else
                {
                    Frame = 0;
                }
            }
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            /*if (!guardian.DoAction.InUse)
            {
                if (trigger == TriggerTypes.PlayerHurt)
                {
                    GuardianActions action = guardian.StartNewGuardianAction(HealingLickAction);
                    action.Players.Add(Main.player[Value]);
                }
                if (trigger == TriggerTypes.GuardianHurt)
                {
                    GuardianActions action = guardian.StartNewGuardianAction(HealingLickAction);
                    action.Guardians.Add(MainMod.ActiveGuardians[Value]);
                }
            }*/
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "Hey, who are you? Are you my newest friend?";
                case 1:
                    return "More friends? I like that!";
                case 2:
                    return "Yay! I met more people!";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Yay! I met more people!");
            Mes.Add("Don't worry, I'll protect you from any danger.");
            Mes.Add("I'm sure you would like to meet " + AlexRecruitScripts.AlexOldPartner + ". Well, she was a bit closed off to other people, but she was my best pal. That's what matter, I guess?");
            Mes.Add("I wonder if " + AlexRecruitScripts.AlexOldPartner + "'s tombstone is alright. Should I check it up later?");
            if (Main.bloodMoon)
            {
                Mes.Add("Stay near me and you'll be safe.");
                Mes.Add("So many things to bite outside.");
            }
            if (Main.eclipse)
            {
                Mes.Add("I think I saw some of those guys in some movies I watched with " + AlexRecruitScripts.AlexOldPartner + ".");
                Mes.Add("I don't fear any of those monsters outside. The only thing I fear is the Legion, but It doesn't exist in this world.");
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    if (!Main.raining)
                        Mes.Add("This day seems good enough for playing outside!");
                    else
                        Mes.Add("The rain would spoil all the fun if it weren't for the puddles.");
                    Mes.Add("I still have two AA batteries to be depleted, so let's play a game!");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    if (Main.moonPhase == 2)
                        Mes.Add("This night reminds me of an adventure I had with " + AlexRecruitScripts.AlexOldPartner + ". That makes me miss her.");
                }
            }
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("When you are not around, I play some Hide and Seek with [gn:0]. He's really bad at hiding. His tail gives him away, but It's fun to always find him.");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                    Mes.Add("Do you know why [gn:0] eats [nn:" + Terraria.ID.NPCID.Merchant + "]'s trash? I'd join him but, " + AlexRecruitScripts.AlexOldPartner + " taught me that eating trash is bad.");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("I fertilized [gn:1]'s yard, and she thanked me by chasing me while swinging her broom at me. I guess we are besties now.");
                Mes.Add("[gn:1] looked very upset when I was playing with her red cloak. By the way, tell her that you didn't see me if she asks.");
                if (NpcMod.HasGuardianNPC(2))
                    Mes.Add("Why does [gn:1] watches [gn:2] and I play? Why don't she join us in the fun? That would be better than staring, right?");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("[gn:2] called me to go on an adventure, I wonder if you will mind that.");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("I asked earlier if [gn:3] was using one of his bones. His answer was very rude.");
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("Why do [gn:3] and [gn:1] look a bit sad when they meet each other? Aren't they dating?");
                Mes.Add("I tried to cheer [gn:3] on. He threw a frizbee for me to catch, but when I returned, he wasn't there. Where did he go?");
            }
            if (NpcMod.HasGuardianNPC(4))
            {
                Mes.Add("What's with [gn:4]? He never shows up any kind of emotion when I talk to him. Even when we play.");
                Mes.Add("I don't really have any fun when playing with [gn:4], because he doesn't seems to be having fun.");
            }
            if (NpcMod.HasGuardianNPC(0) && NpcMod.HasGuardianNPC(2))
                Mes.Add("I've got [gn:0] and [gn:2] to play with me. I guess my new dream will be for everyone in the village to play together.");
            if (NpcMod.HasGuardianNPC(7))
            {
                Mes.Add("I think sometimes [gn:7] feels lonely, so I stay nearby to make her feel less lonely.");
                Mes.Add("I smell a variety of things inside of [gn:7]'s bag, including food. Can you persuade her to open her bag and show us what is inside?");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("I love playing with [gn:8]. The other person that played as much with me was " + AlexRecruitScripts.AlexOldPartner + ".");
                Mes.Add("I'm up to playing some more. Do you know if [gn:8] is free?");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("(He's watching the horizon. Maybe he's thinking about something?)");
                Mes.Add("Have been talking with [gn:"+Vladimir+"] and... No... Forget it... Nevermind what I was saying.");
                Mes.Add("That [gn:" + Vladimir + "] is a real buddy. He accompanies me when I go visit " + AlexRecruitScripts.AlexOldPartner + "'s Tombstone. I don't feel alone when doing that anymore.");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Michelle))
            {
                Mes.Add("I've got a new friend, and the name is [gn:" + GuardianBase.Michelle + "]. What? I'm your buddy too.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Wrath))
            {
                Mes.Add("*Whine~whine* [gn:" + Wrath + "] is a mean guy, I try playing with him, and he's really rude to me.");
                Mes.Add("I try making [gn:"+Wrath+"] feel better, but he aways yells at me.");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("I don't know why some people are scared of [gn:" + Fluffles + "], she's a good person. I like her.");
                Mes.Add("I love playing with [gn:" + Fluffles + "]. She always knows my favorite petting spot.");
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("Sometimes [gn:"+Rococo+"] and [gn:"+Fluffles+"] play with me. It's like a dream came true. They could do that more often.");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("I'm trying hard to aim at the hole.");
                Mes.Add("Is this how you humans use a toilet? It's very hard for me to use it.");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("You'll sleep in my bedroom? That's awesome! I will keep you protected while you sleep.");
                Mes.Add("You'll share your bed with me? This is the best day ever!");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("(He seems to be in pain.)");
                Mes.Add("(His wounds aren't letting him rest.)");
                Mes.Add("I have... To protec.t... " + player.name +"...");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(He's moving his paws while sleeping, maybe he's dreaming that he's running?)");
                Mes.Add("No... Don't go... No... (He seems to be having a nightmare)");
                Mes.Add("(You can hear his silent snores.)");
            }
            if(FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Who's she? Is she friendly? Can she play with me?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Other than playing? I want nothing else.";
            return "I'm full of energy, if that's what you mean.";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I really need something to be done. What? Thought I was into playing 24/7?";
            return "There is a thing I need to be done, can you help me with it?";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Yay! You're the best!";
            return "You did it! Yay!";
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("There's a lot of space in this backyard, but... Where will I sleep?");
            if (Main.raining)
                Mes.Add("This is awful, you know. I could have a less wet place to stay.");
            if (!Main.dayTime)
                Mes.Add("I don't fear anything, but I'd like to sleep soundly at night.");
            if (Main.bloodMoon)
                Mes.Add("Looks like I wont be able to rest tonight. If I had a place to live, I could avoid that.");

            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("My old partner once said that I had C batteries in my body. What is that supposed to mean?");
            Mes.Add("You're the best, let's play a game?");
            Mes.Add(AlexRecruitScripts.AlexOldPartner + " and I were the best friends ever! I should've followed my instincts and stopped her.");
            if (PlayerMod.PlayerHasGuardian(player, 0))
                Mes.Add("[gn:0] and I are trying to guess who's your best friend. Can you tell me?");
            if(!Main.dayTime && Main.moonPhase == 2)
                Mes.Add("Maybe I should tell you why " + AlexRecruitScripts.AlexOldPartner + " isn't with us anymore... She tried to protect me from flying bugs in the purple place... I should be the one protecting her, not the inverse.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 5) && Main.rand.NextDouble() < 0.5)
                return "Woof! Woof! What you brought me? Is it edible? Is it a new toy?";
            return "A party? Just for me? I must be the luckiest dog ever!";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("No!! I wont lose you too!");
                Mes.Add("Hang on buddy, I'll lick your wounds! Please don't die!");
                Mes.Add("Don't die too! You are the only things for me in the world right now! I can't lose you like "+AlexRecruitScripts.AlexOldPartner+"!");
            }
            else
            {
                Mes.Add("I'll help you!");
                Mes.Add("I can take care of your wounds.");
                Mes.Add("When you wake up, I will be here, buddy.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "Hang on buddy, I'll help you.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "I was just taking a nap. Want to play?";
                        case 1:
                            return "Need something? I was having a nice rest.";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "You woke me up. Is It because you finished my request?";
                        case 1:
                            return "You want something? I have asked for something? Did you complete It?";
                    }
                    break;
                    //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "You're taking me out for a walk? Cool! I'll get the leash.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "Uh... There's too many people with you right now...";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "I don't want to go out for a walk right now...";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "You're going to leave me here? All alone?";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "Okay! I'll be waiting for you.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "Alright... I'll try going home...";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "Woof! Amazing!";
                case MessageIDs.RequestAccepted:
                    return "You do It? Woof!";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "Won't you get overloaded? You seems to have lots to do already.";
                case MessageIDs.RequestRejected:
                    return "Whine.. Whine.. It's okay.. It was a silly request anyway..";
                case MessageIDs.RequestPostpone:
                    return "Not now? Try helping me with this later. Woof!";
                case MessageIDs.RequestFailed:
                    return "You couldn't do it? Don't worry. *He seems to be trying to cheer you up now*";
                case MessageIDs.RestAskForHowLong:
                    return "Already? Aww. How long will we rest then?";
                case MessageIDs.RestNotPossible:
                    return "Doesn't look like the best moment to get a rest.";
                case MessageIDs.RestWhenGoingSleep:
                    return "I hope you have good dreams.";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Snif snif* Hey [nickname], there is something cool on\n[shop]'s shop! Let's check it out.";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Hey, what is this?";
                case MessageIDs.GenericYes:
                    return "Yes!";
                case MessageIDs.GenericNo:
                    return "No...";
                case MessageIDs.GenericThankYou:
                    return "Yay! Thank you!";
                case MessageIDs.ChatAboutSomething:
                    return "You may ask me anything.";
                case MessageIDs.NevermindTheChatting:
                    return "Huh? Oh! Fine!";
                case MessageIDs.CancelRequestAskIfSure:
                    return "Yipee! Wait, what? You're don't want to do that for me?";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Whine whine* Fine... I'm not sad or anything...";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Happily wags their tail*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "Thank you, Buddy-Buddy!";
                    return "I've got the best pack ever!";
                case MessageIDs.RevivedByRecovery:
                    return "*Whine whine whine* You guys could have helped me...";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
