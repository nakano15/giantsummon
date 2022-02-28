using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions
{
    public class FearBase : PigGuardianFragmentBase
    {
        public FearBase() : 
            base(FearPigGuardianID)
        {
            Name = "Fear";
            PossibleNames = new string[] { "Frighty", "Dread", "Anxie", "Pani" };
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very scaredy.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 64;
            FramesInRows = 31;
            //DuckingHeight = 54;
            //Each pig should definitelly have the same size, birthday age and time, so I moved those infos.
            Genderless = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            DrinksBeverage = false;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.PurplePhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.Handgun, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.MusketBall, 250));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(2, 0);
            RightArmFrontFrameSwap.Add(3, 1);
            RightArmFrontFrameSwap.Add(4, 1);
            RightArmFrontFrameSwap.Add(5, 0);
            RightArmFrontFrameSwap.Add(6, 0);
            RightArmFrontFrameSwap.Add(7, 1);
            RightArmFrontFrameSwap.Add(8, 1);

            RightArmFrontFrameSwap.Add(19, 0);

            //Animation Frames
            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 2);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 24, 17);
            LeftHandPoints.AddFramePoint2x(13, 20, 22);

            LeftHandPoints.AddFramePoint2x(17, 25, 27);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 16, 2);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 27, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 22);

            RightHandPoints.AddFramePoint2x(17, 27, 27);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);
        }

        public override string CallUnlockMessage => base.CallUnlockMessage;

        public override string ControlUnlockMessage => base.ControlUnlockMessage;

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Waaah!!! Who are you?! Are you friendly or not?!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Add("*Oh, hi [nickname]. What do you wa-AAAHHHHH!! A GHOST!!! BEHIND YOU!!*");
                Mes.Add("*Waah!! What is that!! On your back!!*");
            }
            else if (guardian.IsSleeping)
            {
                Mes.Add("*Aaahhh!! No!! Go away!! Yaaaa!!!* (He seems to be having nightmares.)");
                Mes.Add("*No! I don't want!! Don't push meeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee....* (He seems to be having nightmares)");
                Mes.Add("*Please... Stop... Go away...* (He's moving from a lot while sleeping.)");
            }
            else if (guardian.IsUsingToilet)
            {
                Mes.Add("*This... Is really... Uncomfortable... With you... Like.... Watching...*");
                Mes.Add("*Aaahhh!! (Loud noise) Uh... That helped... Go away... Please..?*");
            }
            else
            {
                Mes.Add("*Aaah!! Oh! It's you.*");
                Mes.Add("*H- Hi! You caught me off guard.*");
                Mes.Add("*Ahh!! Don't care me.*");
                Mes.Add("*Who approaches?! Ah, [nickname]. Phew...*");
                Mes.Add("*No!! Don't!! Oh.. It's just you.*");
                Mes.Add("*My heart is constantly racing. This is not good at all.*");
                Mes.Add("*Everything, anything, scares me. Includding you. You wont do something bad or scary, are you?*");

                Mes.Add("*I need my space, why are you so close!?*");
                Mes.Add("*Hey! Stay away from me!*");
                Mes.Add("*I have nothing to discuss. You should be on your way far from me.*");
                Mes.Add("*Hey! Stop moving around me, it's giving me anxiety.*");
                Mes.Add("*Just existing around here gives me the creeps.*");

                if (Main.dayTime)
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("*No more!! Please end this horrible day!!*");
                        Mes.Add("*It's way too scary!! I want to lock myself at home!*");
                        Mes.Add("*Get them away from me! Get them away from me!*");
                    }
                    else
                    {
                        Mes.Add("*I keep looking around, I never know when something may try to jump out on me.*");
                        Mes.Add("*At least I can spot far away things at this time.*");
                    }
                }
                else
                {
                    if (Main.bloodMoon)
                    {
                        Mes.Add("*This is a horrible night to be here!! HEEEEEEEEELP!!!*");
                        Mes.Add("*There is something wrong! Everything is red, and there's several monsters outside!*");
                    }
                    else
                    {
                        Mes.Add("*It's way too dark! Anything could come out of the dark.*");
                        Mes.Add("*I don't think I'll have a good night of sleep like this.*");
                    }
                }
                if (Main.raining)
                {
                    Mes.Add("*At least the rain sound isn't that sca- (Kabroom!!!) Yikes!!*");
                    Mes.Add("*I can't relax any moment, those thunders always scares me.*");
                }
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("*Please tell [gn:"+Rococo+"] to stop scaring me! He'll end up killing me like that.*");
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    Mes.Add("*[gn:"+Brutus+"] told me to stop seeing him everytime I get scared at something.*");
                    Mes.Add("*Even with [gn:" + Brutus + "] here, I can't feel entirelly safe.*");
                    Mes.Add("*I sometimes also fear the presence of [gn:" + Brutus + "] here. What if we end up in a discussion? He could easily beat me up to the ground.*");
                }
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    Mes.Add("*I dislike wandering outside at night. Beside the zombies and other horrible creatures, there's also [gn:" + Malisha + "] too. Just speaking to her makes me feel in danger.*");
                }
                if (NpcMod.HasGuardianNPC(Leopold))
                {
                    if (player.GetModPlayer<PlayerMod>().TalkedToLeopoldAboutThePigs)
                    {
                        Mes.Add("*I'm... Fragmented? Just this thought makes me really scared. Please help me find my other pieces.*");
                    }
                    else
                    {
                        Mes.Add("*It seems like [gn:" + Leopold + "] really wants to ask many things, but I can't concentrate well on the talking.*");
                    }
                }
                if (NpcMod.HasGuardianNPC(Fluffles))
                {
                    Mes.Add("*Please! Tell that ghost to stay very far away from me!*");
                }
                if (NpcMod.HasGuardianNPC(Wrath))
                {
                    Mes.Add("*Make sure [gn:" + Wrath + "] stay VERY, VERY far away from me! They are menacing and should be caged up.*");
                }
                if (NpcMod.HasGuardianNPC(Joy))
                {
                    Mes.Add("*I don't care how nice [gn:" + Joy + "] may seem, just don't get them close to me!*");
                }
                /*if (NpcMod.HasGuardianNPC(Sadness))
                {
                    Mes.Add("*[gn:" + Sadness + "]*");
                }*/
                if (NpcMod.HasGuardianNPC(Miguel))
                {
                    Mes.Add("*Those abs on [gn:" + Miguel + "] are menacing. They really scare me.*");
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("*You... You will protect me at night, right? You know... Under bed monsters? Closet monsters? And those things?*");
                    Mes.Add("*Sharing the house with you is the best idea ever!! Just please, don't do scary noises.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It doesn't work, I'm like 24/7 scared, "+GuardianGlobalInfos.DaysInAYear+" days in a year.*");
            Mes.Add("*I think my other pieces must think of me as a scaredy cat. But It can't be helped, I am a scaredy cat!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*No... There is nothing scaring me..*");
            Mes.Add("*Ah... No.. I fear I disappointed you.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.request.Base is TravelRequestBase)
            {
                Mes.Add("*There is no way I will explore the world by myself, so can you take me with you?*");
            }
            else
            {
                Mes.Add("*Yes... There is. It's too scary for me to do this, so I need your help with it. Can you [objective]?*");
                Mes.Add("*It's so scary outside, so I can't get this done by myself. Could you help me with [objective]?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Thank you! Thank you! You're a life saver!!*");
            Mes.Add("*I'm really grateful for you, I fear if the reward isn't enough.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I know I should be happy, but each birthday I'm a year older, and that means... Oh no... I should not think about that.*");
            Mes.Add("*Please! No surprise party!! I still want to live a few more years!*");
            if (PlayerMod.HasGuardianBeenGifted(player, guardian))
                Mes.Add("*Did you brought me a gift? I wonder if you brought me a gift. I really don't want to think about what it is, since I fear I may either like it or not.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's scary here! I need a place to live and lock myself in!*");
            Mes.Add("*Aahhh!! Sorry, I thought you was a slime.*");
            Mes.Add("*I need a place to lock myself in! This place is too dangerous and scary. Anything could try killing me.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Nobody coming near? I hope so...*");
            Mes.Add("*Come on... Faster....*");
            Mes.Add("*I hope I'm doing this right...*");
            Mes.Add("*Come on... Breath....*");
            Mes.Add("*Get up! Get up! You need to protect me!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*Y-You picked me?! I... Ah... Will ever be a good buddy? I mean... Thanks... Thanks really..*";
                case MessageIDs.RescueMessage:
                    return "*I managed to find you, gladly I could get back home too.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*Aahh!! Why did you do that? Phew...*";
                        case 1:
                            return "*Waah!! Don't do that!*";
                        case 2:
                            return "*No! Don't! Oh... You like scaring sleeping people?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Aahh!! Oh... Did you do my request?*";
                        case 1:
                            return "*Waahh!! Ah... Oh... Did... You completed what I asked for?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I really don't like this idea... But okay...*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*It's... It's late for me to say, that I'm enochlophobe? Sorry, I just wont join your group right now.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*No! I already have a lot to fear of right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*What?! You want to leave me here?! Alone? In the danger?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*If you don't mind, I'll have to run back home and never leave it.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Ah... I... I'll run back home! Waaahhh!!*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Don't scare me like that! Phew...*";
                case MessageIDs.RequestAccepted:
                    return "*I will anxiously wait for your return. Seriously, try doing that fast, It's a horrible feeling.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*You're overloaded of things to do. Why don't you take care of your other requests before taking mine?*";
                case MessageIDs.RequestRejected:
                    return "*I feared that would end up happening. Now I will need to do it myself.*";
                case MessageIDs.RequestPostpone:
                    return "*Later? Later?! Oh my...*";
                case MessageIDs.RequestFailed:
                    return "*What?! You couldn't do it?!*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Ahh! Oh, it's you.. Did... Did you do my request?*";
                case MessageIDs.RequestRemindObjective:
                    return "*Y-you forgot it?! It was to ... Uh... [objective]... Remember?*";
                case MessageIDs.RestAskForHowLong:
                    return "*Wait, you're proposing the idea of laying down somewhere, closing the eyes and not worrying about any threats or scary things?! How long we'll realize that beautiful idea for?*";
                case MessageIDs.RestNotPossible:
                    return "*Now?! Couldn't you have a worser moment to pick for taking a rest?! I'm sorry but... Look around.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*It will be good to keep my head off scary things for a while, if possible.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Look! [shop] has something I need to check.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Do I need this...?*";
                case MessageIDs.GenericYes:
                    return "*Yes!*";
                case MessageIDs.GenericNo:
                    return "*No...*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you!*";
                case MessageIDs.ChatAboutSomething:
                    return "*Do you need to know about something? What is it?*";
                case MessageIDs.NevermindTheChatting:
                    return "*Enough chatting then?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You want to cancel my request? Are you sure?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Aww... Now I need to do this myself...*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Don't scare me like that.*";
                case MessageIDs.LeopoldMessage1:
                    return "*What is wrong with him? Is he crazy?*";
                case MessageIDs.LeopoldMessage2:
                    return "*I'm not crazy! Why are you following that Terrarian?*";
                case MessageIDs.LeopoldMessage3:
                    return "*I don't know either, their adventure is scary for me. I just joined them because they asked me.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*The feeling of death terrifies me...*";
                    return "*Thank you for pulling me out of that tunnel.*";
                case MessageIDs.RevivedByRecovery:
                    return "*I'm alive?! I'm alive!!*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*I CAN FEEL IT CREEP THROUGH MY VEINS!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*IT BURNS! IT BURNS!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*Who turned off the lights?!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head isn't right right now...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't attack anymore! No, I really mean it, isn't because of fear!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I can't move faster!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I feel like dropping on the ground...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Agh!! Open wound!!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Ah!! No!!! Help!!!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Ewww!!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*S-someone got a b-blanket?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*No! Get away from me!! Help! I'm stuck!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*It bit me! It bit my neck!! I'm going to turn into a vampire?!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*This will be great for when I try escaping from monsters.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Hm... There's more?*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*I... Don't know if this will be very helpful.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*I can now outrun threats!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I will survive longer with this.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*I hope scary things die faster due to this.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Will this dispatch monsters with ease?*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*[nickname]! Life Crystal there!*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Stop! Pressure plate!*";
                case MessageIDs.FoundMineTile:
                    return "*Don't step on that!*";
                case MessageIDs.FoundDetonatorTile:
                    return "*No! Don't push that detonator!*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Based on previous experience, seems like a horrible idea breaking that.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Oh no! We're about to get surrounded, are we?*";
                case MessageIDs.FoundTreasureTile:
                    return "*What are you waiting for? Open it up!*";
                case MessageIDs.FoundGemTile:
                    return "*Shinies!!*";
                case MessageIDs.FoundRareOreTile:
                    return "*Look at that!*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Those are really shiny! Check it out.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*No! Don't! I'm affraid of rollercoasters!*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Don't leave me here!*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Hi... Please, don't do something scary.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*I'm glad they're friendly.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*They're not going to hurt me, right?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Ahhh!! What's it doing to the Terrarian?!*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Leader blacked out! Help them!!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*No! No!!! [nickname]!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone fell here!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*That seems rare.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You don't look well, [nickname].*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*I'm dying here!!*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Ahh!! My potions! I have few!!*";
                case MessageIDs.UsesLastPotion:
                    return "*I am out of potions!!*";
                case MessageIDs.SpottedABoss:
                    return "*Watch out! Big angry thing coming!!*";
                case MessageIDs.DefeatedABoss:
                    return "*Is it dead? It is dead!!*";
                case MessageIDs.InvasionBegins:
                    return "*Why are we being attacked?*";
                case MessageIDs.RepelledInvasion:
                    return "*They're gone, whew.*";
                case MessageIDs.EventBegins:
                    if (Main.bloodMoon)
                        return "*WE'RE ALL GONNA DIE!*";
                    return "*What is going on? Why things suddenly look grim?*";
                case MessageIDs.EventEnds:
                    return "*Is it over? Can I stop panicking now?*";
                case MessageIDs.RescueComingMessage:
                    return "*Ahhh!! NOO!!!*";
                case MessageIDs.RescueGotMessage:
                    return "*Alright! Now we run away!!*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*I feel a bit more safer latelly, [player] also checks if i'm fine frequently.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*It was really creepy when [subject] surged and caused chaos, but [player] managed to take it down.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*It seems like [player] found a [subject].*";
                case MessageIDs.FeatEventFinished:
                    return "*There was a [subject] happening in some world the other day. It was really scary, but [player] managed to end it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*I heard that [player] met [subject] recently. I hope they're not scary.*";
                case MessageIDs.FeatPlayerDied:
                    return "*You wont do that, right? End up dead like [player], right? Please don't.*";
                case MessageIDs.FeatOpenTemple:
                    return "*A temple door was open at [subject]. No, I didn't checked out what is inside, and I don't want to know.*";
                case MessageIDs.FeatCoinPortal:
                    return "*[player] was surprised by a coin portal recently.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I... I have met [player]. I thought they were scary, but they turned out to be nice.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*I heard that [player] has been catching many weird fish.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*Did you knew? [player] managed to kill [subject] and save everyone.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*It was really weird. I was at my house, until I felt something really weird. After that, scarier monsters begun showing on the world. What just happened on [subject]?*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*You could think of congratulating [subject] for being picked as [player]'s buddy. I fear I'll never manage to be picked as someone's buddy... Who'd pick a scaredy person like me as their buddy?*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I'm still... I still want to... I... Thank you... For picking me as buddy... I fear... No... I..*";
                case MessageIDs.DeliveryGiveItem:
                    return "*H-here some [item], [target].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*W-where did t-the item go?*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*Uh... Err... [target]... Your inventory... Is full..*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
