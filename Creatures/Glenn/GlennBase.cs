using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class GlennBase : GuardianBase
    {
        public GlennBase()
        {
            Name = "Glenn";
            Description = "";
            Size = GuardianSize.Small;
            Width = 14;
            Height = 38;
            SpriteWidth = 64;
            SpriteHeight = 56;
            Scale = 32f / 38;
            FramesInRows = 25;
            CompanionSlotWeight = 0.7f;
            //DuckingHeight = 54;
            Age = 11;
            SetBirthday(SEASON_WINTER, 25);
            Male = true;
            InitialMHP = 70; //280
            LifeCrystalHPBonus = 10;
            LifeFruitHPBonus = 3;
            Accuracy = 0.88f;
            Mass = 0.31f;
            MaxSpeed = 4.80f;
            Acceleration = 0.18f;
            SlowDown = 0.9f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.52f;
            DontUseHeavyWeapons = true;
            CanDuck = false;
            ReverseMount = true;
            SetTerraGuardian();
            GroupID = TerraGuardianCaitSithGroupID;
            DodgeRate = 60;
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit51);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath54);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.SilverBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.Shuriken, 250);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = PlayerMountedArmAnimation = 15;
            ChairSittingFrame = 14;
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            DownedFrame = 19;
            ReviveFrame = 18;
            //PetrifiedFrame = 23;

            BackwardStanding = 20;
            BackwardRevive = 21;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(17, 21);
            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(15, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 8);
            LeftHandPoints.AddFramePoint2x(11, 20, 13);
            LeftHandPoints.AddFramePoint2x(12, 22, 17);
            LeftHandPoints.AddFramePoint2x(13, 20, 22);

            LeftHandPoints.AddFramePoint2x(15, 18, 16);

            LeftHandPoints.AddFramePoint2x(18, 17, 22);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 18, 8);
            RightHandPoints.AddFramePoint2x(11, 22, 13);
            RightHandPoints.AddFramePoint2x(12, 24, 17);
            RightHandPoints.AddFramePoint2x(13, 22, 22);

            RightHandPoints.AddFramePoint2x(15, 21, 16);

            RightHandPoints.AddFramePoint2x(18, 20, 22);

            //Head Vanity
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16, 12);
            HeadVanityPosition.AddFramePoint2x(14, 16, 10);
            HeadVanityPosition.AddFramePoint2x(15, 16, 10);
            HeadVanityPosition.AddFramePoint2x(16, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(18, 16, 14);
            HeadVanityPosition.AddFramePoint2x(19, 22, 23);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            bool SardineFollowing = PlayerMod.HasGuardianSummoned(player, GuardianBase.Sardine),
                BreeFollowing = PlayerMod.HasGuardianSummoned(player, GuardianBase.Bree);
            if (SardineFollowing && BreeFollowing)
            {
                Mes.Add("Mom! Dad! I'm glad you two are okay, but why didn't you have returned home?");
            }
            else if (SardineFollowing)
            {
                Mes.Add("Hey Dad, glad to see you safe. Have you seen Mom?");
            }
            else if (BreeFollowing)
            {
                Mes.Add("Hey Mom, you were taking too long to return. Have you found Dad?");
            }
            else
            {
                Mes.Add("Hello, have you a Black and a White cat?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            bool SardineMet = PlayerMod.PlayerHasGuardian(player, GuardianBase.Sardine),
                BreeMet = PlayerMod.PlayerHasGuardian(player, GuardianBase.Bree);
            if (guardian.IsUsingToilet)
            {
                Mes.Add("Aaahh! Go away! This is my private time!");
                Mes.Add("I can't really concentrate on what I'm doing with you watching me.");
                Mes.Add("I know how to use toilet, [nickname].");
            }
            else
            {
                Mes.Add("I have read too many books, so now I'm having to use glasses to see far away places.");
                Mes.Add("What kind of books do you like, [nickname]?");
                Mes.Add("Do you like games?");
                Mes.Add("[nickname], want something?");
                Mes.Add("Yes, [nickname]?");
                if (SardineMet && BreeMet)
                {
                    Mes.Add("I'm so glad that Mom and Dad are around. I wonder when we'll go back home.");
                    Mes.Add("I can't really recall which world we lived on. I've been in many places, and It's too much for my head.");
                    Mes.Add("Has mom and dad been arguing? They generally cooldown after a while.");
                    Mes.Add("I don't get why people consider Mom and Dad like Good and Evil. I don't think either of them are evil.");
                }
                else if (SardineMet)
                {
                    Mes.Add("After I told my dad that mom is out there looking for him, he begun searching for her too.");
                    Mes.Add("I'm glad to have dad around, but I kind of miss mom...");
                    Mes.Add("I wonder where could mom be. Even dad is looking for her.");
                }
                else if (BreeMet)
                {
                    Mes.Add("Mom told me that had no success finding dad. I hope he's fine.");
                    Mes.Add("Mom always stares at the window during the night, with sad look in her eyes. I think she's expecting dad to show up.");
                    Mes.Add("Mom has been looking for dad all around latelly.");
                }
                if (NpcMod.HasGuardianNPC(Bree))
                {
                    Mes.Add("Mom is kind of grumpy, buy she's not angry at you.");
                    Mes.Add("Mom seems to expect us returning home any time soon, that's why she didn't unpacked her things.");
                }
                if (NpcMod.HasGuardianNPC(Sardine))
                {
                    if (GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
                    {
                        Mes.Add("Dad is really happy with his bounty business, but It seems quite dangerous.");
                    }
                    Mes.Add("My dad is the strongest warrior I know. Or was.");
                    Mes.Add("Dad promissed mom that he would bring lots of treasures home. I didn't see any trace of that.");
                }
                if (Main.dayTime)
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("(Eating popcorn, while watching the monsters running around.)");
                        Mes.Add("It feels like being in a horror movie!");
                        Mes.Add("Those monsters doesn't look that scary on the tv. Here, otherwise...");
                    }
                    else
                    {
                        if (!Main.raining)
                            Mes.Add("Enjoying the sun, [nickname]?");
                        else
                            Mes.Add("Playing in the rain, [nickname]?");
                        Mes.Add("I tried to stare at the sun once. It was a horrible idea. I don't recommend doing that though, even more if you use glasses.");
                        Mes.Add("Hey [nickname], wanna play a game?");
                    }
                }
                else
                {
                    if (Main.bloodMoon)
                    {
                        Mes.Add("Why are the women in this world so scary this night?");
                        Mes.Add("[nickname], I'm scared.");
                        Mes.Add("This night is waaaaay too scary for me!");
                        Mes.Add("Why is the moon red?");
                    }
                    else
                    {
                        Mes.Add("I'm enjoying the gentle wind of the night.");
                        Mes.Add("I could read a book during this night, or play some game.");
                        if (Main.moonPhase != 4) Mes.Add("The moon is soooooooooo brighty...");
                        else Mes.Add("Where did the moon go?");
                    }
                }
                if (Main.raining)
                {
                    Mes.Add("I can't stand rain. If I spend a few minutes in It, I start to sneeze.");
                    Mes.Add("I like the sounds of rain drops falling around.");
                    Mes.Add("The thunder sounds really scares me out. They make the fur of my entire body rise.");
                }
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("[gn:" + Rococo + "] and I like to play in the forest.");
                    Mes.Add("I understand that [gn:" + Rococo + "] is really bad at hide and seek...");
                }
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("[gn:" + Blue + "] is actually really nice to me. I wonder why she sometimes shows her teeth when talking to me.");
                    if (NpcMod.HasGuardianNPC(Sardine))
                    {
                        Mes.Add("I've seen my dad defeating many kinds of scary creatures when I was a kid, but why does he let [gn:" + Blue + "] chew him?");
                        Mes.Add("Why does [gn:" + Blue + "] always keeps chasing my dad? Did he do something to her?");
                        if (NpcMod.HasGuardianNPC(Bree))
                        {
                            Mes.Add("It's not unusual seeing mom getting into a discussion with [gn:" + Blue + "], because of her chewing dad.");
                            Mes.Add("Mom always keeps looking around, whenever dad is around, and [gn:" + Blue + "] is nearby.");
                        }
                    }
                }
                if (NpcMod.HasGuardianNPC(Zacks))
                {
                    Mes.Add("[gn:" + Zacks + "] is so scary... Even his smile when looking at me freaks me out!");
                    Mes.Add("I think [gn:" + Zacks + "] follows me around when I'm alone. I can hear him moving, and his groaning. I try locking myself at home when that happens.");
                    Mes.Add("The other day, [gn:" + Zacks + "] surged from the floor right in front of me. I ran away soooo fast after that happened!");
                    if (NpcMod.HasGuardianNPC(Sardine))
                    {
                        Mes.Add("Help! My dad! Is being chased by [gn:" + Zacks + "]!!!");
                        Mes.Add("I just saw [gn:" + Zacks + "] pull my dad using something he pulled from his chest!");
                        Mes.Add("My dad keeps getting chased by [gn:" + Zacks + "] about everyday. He always returns home fine, but smelly and a bit wounded.");
                        if (NpcMod.HasGuardianNPC(Bree))
                        {
                            Mes.Add("I told mom about dad being chased by [gn:" + Zacks + "], but she looked as spooked as me!");
                        }
                    }
                }
                if (NpcMod.HasGuardianNPC(Fluffles))
                {
                    Mes.Add("I sometimes like having [gn:" + Fluffles + "] company around.");
                    Mes.Add("The other day, I was collecting rocks in the forest, until a Demon Eye appeared. [gn:" + Fluffles + "] appeared and saved me.");
                    Mes.Add("I don't feel scared when I discover that [gn:" + Fluffles + "] is on my shoulder.");
                    Mes.Add("Sometimes I read books alongside [gn:" + Fluffles + "].");
                    if (NpcMod.HasGuardianNPC(Sardine))
                    {
                        if (NpcMod.HasGuardianNPC(Blue))
                        {
                            Mes.Add("Everytime [gn:" + Blue + "] begins chasing my dad, [gn:" + Fluffles + "] joins her. Dad is really out of luck.");
                            Mes.Add("It's really hard for dad to run away from [gn:" + Fluffles + "] during the day, since barelly can see her.");
                            Mes.Add("Sometimes dad arrives home after being chased, and discover that [gn:" + Fluffles + "] is on his shoulder when I tell him...");
                        }
                    }
                }
                if (NpcMod.HasGuardianNPC(Alex))
                {
                    Mes.Add("I like playing with [gn:" + Alex + "], but he sometimes asks if I want to play when I'm busy reading or playing...");
                    Mes.Add("I wonder if you play with [gn:" + Alex + "] too, sometimes.");
                }
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    Mes.Add("Why a lot of people stops whatever they are doing, to stare [gn:" + Mabel + "] when she passes through?");
                    if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                    {
                        Mes.Add("I don't like [nn:" + Terraria.ID.NPCID.Angler + "], but I only play with him because [gn:" + Mabel + "] asked.");
                    }
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    Mes.Add("Do you think some day I'll be as big as [gn:" + Brutus + "]?");
                    Mes.Add("[gn:" + Brutus + "] told me the other day that I don't need to worry, because he'll protect me. From what I heard, he also said that to many people.");
                }
                if (NpcMod.HasGuardianNPC(Vladimir))
                {
                    Mes.Add("At first, I thought [gn:" + Vladimir + "] was a big and scary guy, until he talked to me. Now we're best friends.");
                    Mes.Add("No matter what's happening around, [gn:" + Vladimir + "] is always smiling. I wonder what makes him smile so much.");
                    Mes.Add("Whenever I'm sad, I get a hug from [gn:" + Vladimir + "]. I always feel better later.");
                }
                if (NpcMod.HasGuardianNPC(Minerva))
                {
                    Mes.Add("[gn:" + Minerva + "] told me to eat lots so I can grow up strong like my parents.");
                    Mes.Add("I like the food [gn:" + Minerva + "] makes, but I really dislike when my food involves broccolis.");
                }
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    Mes.Add("Everyone keeps telling me to stay away from [gn:" + Malisha + "]'s place, but she doesn't seems like that bad of a person.");
                    Mes.Add("I saw [gn:" + Malisha + "] the other day by her house, she was offering me candies. I didn't accepted them, because I just lunched.");
                }
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Gah! Ghost!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            bool SardineMet = PlayerMod.PlayerHasGuardian(player, GuardianBase.Sardine),
                BreeMet = PlayerMod.PlayerHasGuardian(player, GuardianBase.Bree);
            List<string> Mes = new List<string>();
            Mes.Add("There's so many games I play. Which ones do you play?");
            Mes.Add("I like reading fantasy books: It feels like life.");
            Mes.Add("What kind of places have you visited? I discovered some as I travelled to here.");
            if (SardineMet && BreeMet)
            {
                Mes.Add("I fear when my parents argue, since I don't like thinking about them breaking up.");
            }
            if (SardineMet)
            {
                Mes.Add("Do you think I'll be a great adventurer, like my father?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Not yet.");
            Mes.Add("I have everything I need right now.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hey [nickname], could you do something for me?");
            Mes.Add("There is something I need done, but I can't really do It right now. Could you help me with It?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Thanks [nickname]! That was amazing!");
            Mes.Add("Very nice, [nickname]. You helped me a lot!");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("This isn't good... This isn't good...");
            Mes.Add("Too much blood...");
            Mes.Add("Is this how I do that?");
            Mes.Add("So many wounds.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "Don't worry! You're at my house right now, It's safe in here.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "[nickname].. It's too late for me to stay awaken.";
                    return "Yawn... Couldn't you wait until the morning?";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "..Oh... Did you do my request... Or what?";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "You're calling me to go on an adventure? Yay! Let's go!";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "There's way too many people in your group, I can't seem to fit in It.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "... My parents teached me not to follow strangers..";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "Awww... But It was so fun...";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "Here?! This place is dangerous! How can I get to home from here?";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "W-what?! Uh... I guess.. I should try to survive my way home, then.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "Oh, okay. Then let's continue the adventure.";
                case MessageIDs.RequestAccepted:
                    return "Amazing!";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "Mom and dad teached me not to get myself overloaded with many stuffs to do, maybe you shouldn't either.";
                case MessageIDs.RequestRejected:
                    return "Aww man....";
                case MessageIDs.RequestPostpone:
                    return "Not now? Oh... Fine...";
                case MessageIDs.RequestFailed:
                    return "You couldn't do It...?";
                case MessageIDs.RestAskForHowLong:
                    return "Rest? Okay. But for how long?";
                case MessageIDs.RestNotPossible:
                    return "Now?! Not now.";
                case MessageIDs.RestWhenGoingSleep:
                    return "I'll take the window!";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "Woah! That thing [shop] is selling looks interesting. Let's check It out.";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Ok, this... Then this...";
                case MessageIDs.GenericYes:
                    return "Yup!";
                case MessageIDs.GenericNo:
                    return "No.";
                case MessageIDs.GenericThankYou:
                    return "Yeah, Thank you!";
                case MessageIDs.ChatAboutSomething:
                    return "Huh? Okay, what do you want to know?";
                case MessageIDs.NevermindTheChatting:
                    return "Cool. Anything else?";
                case MessageIDs.CancelRequestAskIfSure:
                    return "You can't do what I asked? Are you serious?!";
                case MessageIDs.CancelRequestYesAnswered:
                    return "Aww... Now I will have to do It...";
                case MessageIDs.CancelRequestNoAnswered:
                    return "Phew.... Then try completting It.";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "Hm... What can I discover about you...";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "Many books... Video games.... Wait, does that even exists?";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "A portrait of his family...";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "Okay, I think that's enough information.";
                case MessageIDs.AlexanderSleuthingFail:
                    return "Ouch... Ow! You didn't needed to scratch my nose.";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "That was so horrible! I'm glad you guys helped me out.";
                case MessageIDs.RevivedByRecovery:
                    return "Ouch... Everything, from head to feet, hurts...";
                //
                case MessageIDs.LeopoldMessage1:
                    return "Uh... What is your problem?";
                case MessageIDs.LeopoldMessage2:
                    return "*My problem?! What do you mean by that? Why you're following that Terrarian?*";
                case MessageIDs.LeopoldMessage3:
                    return "They called me for an adventure, and I'm following them.";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
