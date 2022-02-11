using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Companions
{
    public class GreenBase : GuardianBase
    {
        public GreenBase()
        {
            Name = "Green"; //Jochen Green
            Description = "Treated many TerraGuardians in the Ether Realm,\nhis newest challenge now is on the Terra Realm.";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 86;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 89f / 86;
            CompanionSlotWeight = 1.15f;
            Age = 31;
            SetBirthday(SEASON_SPRING, 4);
            Male = true;
            InitialMHP = 170; //895
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 25;
            Accuracy = 0.6f;
            Mass = 0.36f;
            MaxSpeed = 5.15f;
            Acceleration = 0.21f;
            SlowDown = 0.39f;
            //MaxJumpHeight = 15;
            //JumpSpeed = 7.08f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            DontUseHeavyWeapons = true;
            SetTerraGuardian();
            CallUnlockLevel = 3;
            MountUnlockLevel = 6;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.FlintlockPistol, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);
            AddInitialItem(Terraria.ID.ItemID.MeteorShot, 50);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 15;
            ChairSittingFrame = 14;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 17;
            BedSleepingFrame = 16;
            SleepingOffset.X = 16;
            ReviveFrame = 18;
            DownedFrame = 19;

            BackwardStanding = 20;
            BackwardRevive = 21;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(15, 0);

            RightArmFrontFrameSwap.Add(14, 0);
            RightArmFrontFrameSwap.Add(15, 1);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(21, 40); //19

            //Mount
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(18, 15);
            MountShoulderPoints.AddFramePoint2x(14, 17, 20);
            MountShoulderPoints.AddFramePoint2x(17, 17, 15);
            MountShoulderPoints.AddFramePoint2x(18, 29, 21);
            MountShoulderPoints.AddFramePoint2x(21, 29, 21);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 3);
            LeftHandPoints.AddFramePoint2x(11, 33, 11);
            LeftHandPoints.AddFramePoint2x(12, 37, 21);
            LeftHandPoints.AddFramePoint2x(13, 30, 29);

            LeftHandPoints.AddFramePoint2x(15, 29, 31);

            LeftHandPoints.AddFramePoint2x(18, 37, 41);

            LeftHandPoints.AddFramePoint2x(18, 41, 41);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 25, 3);
            RightHandPoints.AddFramePoint2x(11, 37, 11);
            RightHandPoints.AddFramePoint2x(12, 39, 21);
            RightHandPoints.AddFramePoint2x(13, 33, 29);

            RightHandPoints.AddFramePoint2x(18, 41, 41);

            RightHandPoints.AddFramePoint2x(18, 37, 41);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(23, 12);
            HeadVanityPosition.AddFramePoint2x(14, 23, 17);
            HeadVanityPosition.AddFramePoint2x(15, 23, 17);
            HeadVanityPosition.AddFramePoint2x(18, 36, 30);
            HeadVanityPosition.AddFramePoint2x(21, 36, 30);
        }

        public override string CallUnlockMessage => "*I think I had enough of studying terrarian anatomy day and night. I would like to know more about your world too, if you're willing to take me with you.*";

        public override string MountUnlockMessage => "*If you want, I can carry you on my shoulder. That is, if you don't mind touching cold scale.*";

        public override string ControlUnlockMessage => "*I think I know you enough to entrust you with myself. I can take on whatever adventures you can't go, if you want.*";

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Need a doctor? You found one.*";
                case 1:
                    return "*It's always good to meet someone new.*";
                case 2:
                    return "*Are you injured? I can take care of that.*";
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Add("(He's sleeping? I think he's sleeping. His eyes... Doesn't seems like his regular eyes.)");
                Mes.Add("(You try waving your hand in front of his eye, but see no reaction. He must be sleeping.)");
                Mes.Add("(You shiver while watching him sleeping.)");
            }
            else if(player.statLife < player.statLifeMax2 * 0.33f)
            {
                Mes.Add("*[nickname]! Hang on! You need medical help, right now.*");
                Mes.Add("*What happened to you? Hang on, sit down a bit.*");
                Mes.Add("*You're bleeding too much. Let's see if I can help you with those wounds.*");
            }
            else
            {
                Mes.Add("*I have treated many people in the Ether Realm. Let's see if I can do the same here.*");
                Mes.Add("*Why people seems scared of me? Is it because of my profession?*");
                Mes.Add("*Whatever wound you show me, probably isn't the gruesomest thing I've ever seen.*");
                Mes.Add("*Are you in need of a check up?*");
                Mes.Add("*In case your back is aching, I can solve that with ease.*");
                Mes.Add("*Do I look angry, or scary? Sorry, my face is just like that.*");

                if (Main.dayTime)
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("*Please direct the injured to my room.*");
                        Mes.Add("*I wont rest this eclipse, right?*");
                        Mes.Add("*Are you hurt? Those monsters look tough.*");
                    }
                    else
                    {
                        Mes.Add("*Enjoying the time, [nickname]?*");
                        Mes.Add("*You visited me, that means either you are injured, sick, or wanted to check me out.*");
                        Mes.Add("*I'm not treating anyone right now, so I can spend some time talking.*");
                    }
                }
                else
                {
                    if (Main.bloodMoon)
                    {
                        Mes.Add("*What is wrong with the women here? They all look furious this night.*");
                        Mes.Add("*I don't know what's scarier this night: The monsters or the women.*");
                        Mes.Add("*I wont have a good night of sleep today, right?*");
                    }
                    else
                    {
                        Mes.Add("*I was about to get some sleep. Need something before I do so?*");
                        Mes.Add("*Feeling tired? Me too.*");
                        Mes.Add("*You would not believe your eyes, if ten million fireflies, lit up the world as I fall asleep.*");
                    }
                }

                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("*There is enough space on my house for both of us, so I can share it with you.*");
                    Mes.Add("*I really hope I don't make you sick whenever I change my skin.*");
                }
                if (guardian.IsPlayerBuddy(player))
                {
                    Mes.Add("*Feeling fine, [nickname]? Just checking you up.*");
                }
                if (NpcMod.HasGuardianNPC(Sardine))
                {
                    Mes.Add("*It has been many times I've had [gn:" + Sardine + "] be brought by someone to my house, unconscious.*");
                    Mes.Add("*Everytime is the same: [gn:" + Sardine + "] is brought to me unconscious to me, I treat him, the he gets jump scared when he wakes up. He should have been used to my face.*");
                }
                if (NpcMod.HasGuardianNPC(Bree))
                {
                    Mes.Add("*Why [gn:" + Bree + "] takes so many vitamins?*");
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    Mes.Add("*For someone who charges into fights, [gn:" + Brutus + "] only visits me with light wounds.*");
                    Mes.Add("*I can't help but notice that [gn:" + Brutus + "] has his left arm stronger than the right. Is it because he practices swinging his sword?*");
                }
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    Mes.Add("*Ever since [gn:" + Mabel + "] moved in, I had to get a new batch of tissues.*");
                    Mes.Add("*I tried recommending some diet for [gn:" + Mabel + "]. She didn't took that well.*");
                }
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    Mes.Add("*I'm glad that my old skins are being useful for [gn:" + Malisha + "]. I only wonder what she uses it for.*");
                }
                if (NpcMod.HasGuardianNPC(Cinnamon))
                {
                    Mes.Add("*Why does [gn:" + Cinnamon + "] thinks I will eat her?*");
                    Mes.Add("*In some extreme cases, I have to make [gn:" + Cinnamon + "] sleep in order to treat her disease or wounds.*");
                }
                if (NpcMod.HasGuardianNPC(Miguel))
                {
                    Mes.Add("*It seems like I managed to get into some synergy with [gn:" + Miguel + "]'s work. He gives exercises to people, while I help with their nutrition.*");
                }
                if (NpcMod.HasGuardianNPC(Cille))
                {
                    Mes.Add("*Ah, good that you came. Your friend [gn:" + Cille + "] has visited me earlier. She said that had something wrong with her, but I did a checkup, and I didn't found anything wrong.*");
                }
                if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
                {
                    Mes.Clear();
                    Mes.Add("*Uh, [nickname]... There is a ghost on your shoulder.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Initially, I didn't moved to this world to treat people, but was more for curiosity. I wanted to know how the Terra Realm look like.*");
            Mes.Add("*I would be grateful if people were less scared of me.*");
            Mes.Add("*I don't recommend you to make treatment harder to a medic. It can actually end up making the treatment worser and painful.*");
            Mes.Add("*Don't underestimate diseases. Even little coughs could be something serious if you don't be careful, so watch yourself.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I have no necessities right now.*");
            Mes.Add("*There isn't much I need help with right now. Maybe another time.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Perfect timing! I need you to do something for me. Can you [objective]?*");
            Mes.Add("*Yes... As a matter of fact, I have something that I need your help with. Can you help me out with my problem, [objective]?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Perfect job. You're reliable, [nickname].*");
            Mes.Add("*I shouldn't expect less of you, [nickname].*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm really getting older.. I barelly can believe.*");
            Mes.Add("*I think I can take the day off and just enjoy my birthday.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian))
                Mes.Add("*People says that snakes hypnotizes people, so... Tell me, [nickname]. What have you brought to me?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need a safe place to stay. I can't simply stay on the wilderness like this.*");
            Mes.Add("*If you want me to be able to treat people, I need a place to keep my equipments, and the patients safe.*");
            Mes.Add("*This isn't a ideal place for me to stay. I'd like to have a roof above my head.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Don't worry, you'll be fixed in a moment.*");
            Mes.Add("*Stitching some wounds.*");
            Mes.Add("*Bleeding stopped.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*I'm really happy that you picked me as your buddy, but that doesn't mean I wont charge for my services.*";
                case MessageIDs.RescueMessage:
                    return "*Don't worry, you'll be waking up soon.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    if(Main.rand.NextDouble() < 0.5)
                        return "*[nickname], many things I wont mind, but waking me up in the middle of my slumber isn't one of them.*";
                    return "*You woke me up. I hope it is important.*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    if(Main.rand.NextDouble() < 0.5)
                        return "*I really hope you woke me up because you completed my request.*";
                    return "*You got me off my bed. You must have done my request, right?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Alright. I can be your field medic, then.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There is too many people.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Right now, I don't think it's good to join your travels.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*This place is somewhat dangerous. Do you want to leave me here?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Visit me whenever you're wounded.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Well, I guess I'll have to fight my way home, then. And probably gather some snacks on the way.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*If you want to remove me from your group, find a safe place with someone close.*";
                case MessageIDs.RequestAccepted:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I'll await your return.*";
                    return "*Don't disappoint me, [nickname].*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*If I gave you my request, I would only help stressing you out. Only try doing what you can manage to do at a time.*";
                case MessageIDs.RequestRejected:
                    return "*Hm... Disappointing.*";
                case MessageIDs.RequestPostpone:
                    return "*It can wait, sure.*";
                case MessageIDs.RequestFailed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I shouldn't have entrusted that to you.*";
                    return "*You disappointed me, [nickname]. Don't do that again.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Bring me good news, [nickname]. Did you do what I asked?*";
                case MessageIDs.RequestRemindObjective:
                    return "*You forgot my request? I asked you to [objective].*";
                case MessageIDs.RestAskForHowLong:
                    return "*Getting drowzy? We can rest then. How long until we return to your adventures?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't the best moment for that.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*If you want, use my body as pillow. If you don't mind a cold skin.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*What [npc] is offering looks interesting. Let's check it out.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Just a minute...*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you.*";
                case MessageIDs.ChatAboutSomething:
                    return "*Do you want to know about something?*";
                case MessageIDs.NevermindTheChatting:
                    return "*I hope I answered all your questions.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*Are you sure you want to cancel my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Why did you accept it in first place...?*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*You nearly disappointed me, [nickname].*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Tell me more about yourself, doctor.*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... Seems like you've been treating many people...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*...What have you been eating? It sounds like...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*This... This guy is scary... I can now see why there's less critters around here.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Y-you don't plan on squeezing me, right?*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "*I'm really glad to see you people managed to heal my wounds.*";
                case MessageIDs.RevivedByRecovery:
                    return "*I can fight again, Thanks.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Argh... I could use an antidote now...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Ahh!! AHH!! This doesn't go well on my skin!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes!!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head is spinning...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*Why my arms wont obey me?!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I'm feeling... Sluggish...*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I don't feel well...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Argh... My skin...*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What a gruesome creature is that?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Why are you all looking at me like that?*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*I-I'm f-f-freezing...*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I'm stuck here!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*I'll break your bones!!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Try to hurt me now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*More.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Time to open up something.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Faster!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I feel vigorous now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*I shall hit where it hurts.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Better I be careful of how I use this blade.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Down it goes.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I could drink this all day.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*I think you may want to see that.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Watch your step!*";
                case MessageIDs.FoundMineTile:
                    return "*A mine!*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Detonator ahead. Try not to fall on it.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*That plant smells like trouble.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I wonder how etherians tastes.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Something good must be in that chest.*";
                case MessageIDs.FoundGemTile:
                    return "*Shiny gems.*";
                case MessageIDs.FoundRareOreTile:
                    return "*You may want to see this.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*That ore looks rare.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*I hope I don't feel sick.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Feeling home sick? Let's go, then.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*I'm glad to see you join us.*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*One more person.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*I wonder if I can train them to help me on surgeries.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Healing his loneliness, [nickname]?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Our leader needs medical attention!*";
                case MessageIDs.LeaderDiesMessage:
                    if(Main.rand.NextDouble() < 0.5)
                        return "*[nickname]...*";
                    {
                        float Time = (float)WorldMod.LastTime / 3600;
                        return "*Time of death: " + (int)Time + ":" + (int)((Time - (int)Time) * 60) + "*";
                    }
                case MessageIDs.AllyFallsMessage:
                    return "*Someone needs medical attention!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*That must be good.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*Try not to get yourself killed while I'm away.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*[nickname], watch your health, you don't look good.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*I need to take it easy...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Trouble. I'm running out of potions.*";
                case MessageIDs.UsesLastPotion:
                    return "*My potions! They're gone!*";
                case MessageIDs.SpottedABoss:
                    return "*I spot trouble.*";
                case MessageIDs.DefeatedABoss:
                    return "*Trouble down.*";
                case MessageIDs.InvasionBegins:
                    return "*I think we will have a long fight.*";
                case MessageIDs.RepelledInvasion:
                    return "*Their attack ceased. Let's tend the wounded.*";
                case MessageIDs.EventBegins:
                    return "*What a horrible "+(Main.dayTime ? "day" : "night")+" for this to happen.*";
                case MessageIDs.EventEnds:
                    return "*Anyone injured?*";
                case MessageIDs.RescueComingMessage:
                    return "*Hold on, I'm coming!*";
                case MessageIDs.RescueGotMessage:
                    return "*I'll hold you until it's possible to treat your wounds on peace.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*Due to my profession, I keep in contact with many people. One of them is a terrarian named [player]. They've been helping me latelly.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I've been hearing about [player] latelly. Seems like they defeated [subject] recently.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*Looks like [player] found a [subject]. What is it? I don't know.*";
                case MessageIDs.FeatEventFinished:
                    return "*[player] managed to take care of a [subject] in their world.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*Looks like [player] met [subject] on their travels.*";
                case MessageIDs.FeatPlayerDied:
                    return "*If you know [player], I'm sorry to inform you that they died recently. You can find their tombstone in [subject].*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] managed to open a temple door at [subject]. If you're interessed, go visit that world.*";
                case MessageIDs.FeatCoinPortal:
                    return "*A coin portal surprised [player] on their travels.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I have met another Terrarian recently, their name was [player], if I remember well.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] has been catching many weird fish recently. None of them look tasty for me.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*It seems like [player] made their world safe to live. They killed some creature called [subject] and now people are saying that.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*I don't recommend you to visit [subject], things have... Changed... There.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*I'm really happy with [subject] and [player]. They seem like the perfect buddies.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I'm actually also [player]'s buddy, but I may still join your travels when they're not around.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> Options = base.GetGuardianExtraDialogueActions(guardian);
            if (GuardianGlobalInfos.UnlockedGreensHealing)
            {
                Options.Add(new DialogueOption("I'm hurt.", HealDialogue, true));
            }
            return Options;
        }

        private void HealDialogue()
        {
            int HealValue = 0;
            PlayerMod pl = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            HealValue = pl.player.statLifeMax2 - pl.player.statLife;
            int WoundDebuffID = Terraria.ModLoader.ModContent.BuffType<Buffs.LightWound>(),
                HeavyWoundDebuffID = Terraria.ModLoader.ModContent.BuffType<Buffs.VeryWounded>();
            for (int b = 0; b < pl.player.buffType.Length; b++)
            {
                int buffid = pl.player.buffType[b];
                if(Main.debuff[buffid] && pl.player.buffType[b] > 5 && Terraria.ModLoader.BuffLoader.CanBeCleared(buffid))
                {
                    HealValue += 800;
                }
                if (buffid == WoundDebuffID)
                    HealValue += 200;
                if (buffid == HeavyWoundDebuffID)
                    HealValue += 8000;
            }
            bool NearDeath = false;
            if(pl.player.statLife < pl.player.statLifeMax2 * 0.33f)
            {
                HealValue = (int)(Math.Max(HealValue * 0.35f, 1f));
                NearDeath = true;
            }
            foreach(TerraGuardian tg in pl.GetAllGuardianFollowers)
            {
                if (tg.Active && !tg.KnockedOut)
                {
                    HealValue += tg.MHP - tg.HP;
                    foreach(BuffData b in tg.Buffs)
                    {
                        if(Main.debuff[b.ID] && b.Time > 5 && Terraria.ModLoader.BuffLoader.CanBeCleared(b.ID))
                        {
                            HealValue += 800;
                        }
                        if (b.ID == WoundDebuffID)
                            HealValue += 200;
                        if (b.ID == HeavyWoundDebuffID)
                            HealValue += 8000;
                    }
                }
            }
            if (HealValue <= 0)
            {
                Dialogue.ShowEndDialogueMessage("*Nobody seems to need medication or wounds treated.\nDon't spend my time.*");
            }
            else
            {
                HealValue = (int)(Math.Max(HealValue * 0.35f, 1f)); //Nurse charges 75%
                int c = HealValue, s = 0, g = 0, p = 0;
                if (c >= 100)
                {
                    s += c / 100;
                    c -= s * 100;
                }
                if (s >= 100)
                {
                    g += s / 100;
                    s -= g * 100;
                }
                if (g >= 100)
                {
                    p += g / 100;
                    g -= p * 100;
                }
                string PriceText = "";
                if (c > 0)
                    PriceText = " " + c + " Coppers";
                if (s > 0)
                {
                    if (PriceText != "")
                        PriceText = "," + PriceText;
                    PriceText = " " + s + " Silvers";
                }
                if (g > 0)
                {
                    if (PriceText != "")
                        PriceText = "," + PriceText;
                    PriceText = " " + g + " Golds";
                }
                if (p > 0)
                {
                    if (PriceText != "")
                        PriceText = "," + PriceText;
                    PriceText = " " + p + " Platinum";
                }
                if (Dialogue.ShowDialogueWithOptions(NearDeath ? "Due to the state you encounter yourself into, I'll only charge you\n"+PriceText+" for the treatment.*" :
                    "*I see that you're in need of treatment.\nI can take care of your wounds and ailments if you give me\n" + PriceText + ". Want me to begin treatment?*", new string[] { "Fix me", "Nevermind" }) == 0)
                {
                    if (pl.player.SellItem(HealValue, 1))
                    {
                        string Message = "";
                        if (pl.player.statLife < pl.player.statLifeMax2 * 0.33f)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    Message = "*I recommend you to lay down for a while and let your wounds heal, before returning to action.*";
                                    break;
                                case 1:
                                    Message = "*I doubt you can jump of happiness, but at least you will live.*";
                                    break;
                                case 2:
                                    Message = "*I think I can give you clearance to go. You will still need some time to recover your wounds.*";
                                    break;
                            }
                        }
                        else if (pl.player.statLife < pl.player.statLifeMax2 * 0.67f)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    Message = "*Your wounds have been treated, and all your ailments were cured. You may feel a bit of pain when exercising the areas that were wounded, but they will go away soon.*";
                                    break;
                                case 1:
                                    Message = "*It's all done. Just be careful not to open the wounds again during your travels.*";
                                    break;
                                case 2:
                                    Message = "*Next time you get injured, don't hexitate to visit me.*";
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    Message = "*That was a minor wound. You'll be fine.*";
                                    break;
                                case 1:
                                    Message = "*It wasn't hard to take care of that issue. You can go now.*";
                                    break;
                                case 2:
                                    Message = "*All patched up.*";
                                    break;
                            }
                        }
                        pl.player.statLife = pl.player.statLifeMax2;
                        for (int b = 0; b < pl.player.buffType.Length; b++)
                        {
                            int buffid = pl.player.buffType[b];
                            if ((Main.debuff[buffid] || buffid == WoundDebuffID || buffid == HeavyWoundDebuffID) && pl.player.buffType[b] > 5 && Terraria.ModLoader.BuffLoader.CanBeCleared(buffid))
                            {
                                pl.player.DelBuff(b);
                                b -= 1;
                            }
                        }
                        foreach (TerraGuardian tg in pl.GetAllGuardianFollowers)
                        {
                            if (tg.Active)
                            {
                                if (tg.KnockedOut)
                                {
                                    tg.TeleportToPlayer();
                                    tg.KnockedOut = tg.KnockedOutCold = false;
                                }
                                tg.HP = tg.MHP;
                                for (int buff = tg.Buffs.Count - 1; buff >= 0; buff--)
                                {
                                    BuffData b = tg.Buffs[buff];
                                    if ((Main.debuff[b.ID] || b.ID == WoundDebuffID || b.ID == HeavyWoundDebuffID) && b.Time > 5 && Terraria.ModLoader.BuffLoader.CanBeCleared(b.ID))
                                    {
                                        tg.Buffs.RemoveAt(buff);
                                    }
                                }
                            }
                        }
                        Dialogue.ShowEndDialogueMessage(Message, false);
                    }
                    else
                    {
                        pl.player.statLife = pl.player.statLifeMax2;
                        foreach (TerraGuardian tg in pl.GetAllGuardianFollowers)
                        {
                            if (tg.Active)
                            {
                                if (tg.KnockedOut)
                                {
                                    tg.TeleportToPlayer();
                                    tg.KnockedOut = tg.KnockedOutCold = false;
                                }
                                tg.HP = tg.MHP;
                            }
                        }
                        Dialogue.ShowEndDialogueMessage("*If you cannot pay, at least I can treat your wounds, but I wont be able to take care of your health issues.*", false);
                    }
                }
                else
                {
                    if (NearDeath)
                    {
                        pl.player.statLife = pl.player.statLifeMax2;
                        foreach (TerraGuardian tg in pl.GetAllGuardianFollowers)
                        {
                            if (tg.Active)
                            {
                                if (tg.KnockedOut)
                                {
                                    tg.TeleportToPlayer();
                                    tg.KnockedOut = tg.KnockedOutCold = false;
                                }
                                tg.HP = tg.MHP;
                            }
                        }
                        Dialogue.ShowEndDialogueMessage("*No way, I can't simply let you leave like that.\nLet me at least take care of your wounds.*", false);
                        return;
                    }
                    Dialogue.ShowEndDialogueMessage("*Changed your mind? It's fine.*", false);
                }
            }
        }
    }
}
