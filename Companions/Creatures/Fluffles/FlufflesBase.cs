﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giantsummon.Trigger;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Companions
{
    public class FlufflesBase : GuardianBase
    {
        public const string FoxSoulTextureID = "foxsoul";

        public FlufflesBase()
        {
            Name = "Fluffles";
            PossibleNames = new string[] { "Fluffles", "Krümel" }; //Thank BentoFox for the names.
            Description = "She was an experienced adventurer, part of a\ngroup of Terrarians and TerraGuardians.\nA traumatic experience unallows her to speak.";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 92;
            FramesInRows = 23;
            CompanionSlotWeight = 1.2f;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 104;
            Scale = 101f / 92;
            Age = 31;
            SetBirthday(SEASON_SPRING, 11);
            Male = false;
            InitialMHP = 235; //955
            LifeCrystalHPBonus = 35;
            LifeFruitHPBonus = 10;
            Accuracy = 0.77f;
            Mass = 0.25f;
            MaxSpeed = 4.85f;
            Acceleration = 0.15f;
            SlowDown = 0.26f;
            MaxJumpHeight = 17;
            JumpSpeed = 6.18f;
            CanDuck = false;
            ReverseMount = true;
            DrinksBeverage = true;
            SetTerraGuardian();
            IsNocturnal = true;

            CallUnlockLevel = MountUnlockLevel = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.BoneSword, 1);
            AddInitialItem(Terraria.ID.ItemID.GoldBow, 1);
            AddInitialItem(Terraria.ID.ItemID.BoneArrow, 250);
            AddInitialItem(Terraria.ID.ItemID.LesserHealingPotion, 5);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 3, 4, 5, 4 };
            JumpFrame = 2;
            PlayerMountedArmAnimation = 10;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 6, 7, 8, 9 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 11;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            SleepingOffset.X = 16;
            ReviveFrame = 15;
            DownedFrame = 18;
            //PetrifiedFrame = 28;

            BackwardStanding = 19;
            BackwardRevive = 22;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(11, 0);

            //MountShoulderPoints.DefaultCoordinate2x = new Point(21, 44);
            SittingPoint2x = new Point(21, 36);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(6, 15, 3);
            LeftHandPoints.AddFramePoint2x(7, 31, 11);
            LeftHandPoints.AddFramePoint2x(8, 35, 20);
            LeftHandPoints.AddFramePoint2x(9, 31, 29);

            LeftHandPoints.AddFramePoint2x(10, 25, 31);

            LeftHandPoints.AddFramePoint2x(15, 39, 44);

            //Right Arm
            RightHandPoints.AddFramePoint2x(6, 27, 3);
            RightHandPoints.AddFramePoint2x(7, 34, 11);
            RightHandPoints.AddFramePoint2x(8, 38, 20);
            RightHandPoints.AddFramePoint2x(9, 34, 29);

            RightHandPoints.AddFramePoint2x(15, 42, 44);
            
            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Point(22, 13);
            HeadVanityPosition.AddFramePoint2x(15, 38, 39);
            HeadVanityPosition.AddFramePoint2x(17, 10, 38);
            HeadVanityPosition.AddFramePoint2x(18, 33, 33);

            //WingPosition
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new FlufflesData(ID, ModID);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(FoxSoulTextureID, "FoxSoul");
        }

        public static bool IsHauntedByFluffles(Player player)
        {
            return player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>());
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(It looks like she's greeting you.)");
            Mes.Add("(She smiled as she met you.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Add("(She's snoring gently.)");
                Mes.Add("(She seems to be having a peaceful rest.)");
                Mes.Add("(You wonder what she may be dreaming of, to make her sleep so calmly.)");
            }
            else if (Main.bloodMoon)
            {
                Mes.Add("(She looks really annoyed, growling at you.)");
                Mes.Add("(She seems to be wanting this night to end as soon as possible.)");
                Mes.Add("(Her face is full of anger, maybe I shouldn't try speaking to her.)");
            }
            else
            {
                Mes.Add("(She's asking if you're okay.)");
                Mes.Add("(She softly hits her paw on her chest a few times after she saw you.)");
                Mes.Add("(She looks relieved for seeing you.)");
                if (Main.raining)
                {
                    Mes.Add("(She looks at the sky, as the rain drops pass through her body.)");
                    Mes.Add("(Everytime It's raining, she has a sorrow look. Maybe she liked that?)");
                }
                if (Main.eclipse)
                {
                    Mes.Add("(She looks to be on alert.)");
                    Mes.Add("(She seems to be scared for the people around, more than herself.)");
                }
                if (Main.moonPhase == 0)
                {
                    Mes.Add("(She shows you a portrait she carries with her. In It you see a male and a female Terrarian, a TerraGuardian and her.)");
                }
                if (guardian.IsUsingToilet)
                {
                    Mes.Add("(She seems embarrassed. I think she wants you to leave the room.)");
                    Mes.Add("(As she notices you, she puts one of her paws on her hip, while with the other she signals for you to leave the room.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Rococo))
                {
                    Mes.Add("(She's looking at [gn:" + Rococo + "] with a puzzled face. It's like as if she has seen him before.)");
                    Mes.Add("(She seems to be trying to recall something, after looking at [gn:" + Rococo + "].)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Blue))
                {
                    if (PlayerMod.HasGuardianSummoned(player, Sardine))
                    {
                        Mes.Add("(She seems to be asking [gn:" + Blue + "] if they're going to play again.)");
                        Mes.Add("([gn:" + Sardine + "] begun panicking as [gn:" + Blue + "] and [gn:" + Fluffles + "] stares at him, with a grim smile.)");
                    }
                    else if (NpcMod.HasGuardianNPC(Sardine))
                    {
                        Mes.Add("([gn:" + Blue + "] and her must be scheming something. I wonder if that's related to [gn:"+Sardine+"].)");
                    }
                }
                if (PlayerMod.HasGuardianSummoned(player, Sardine))
                {
                    Mes.Add("(She's looking at [gn:" + Sardine + "] while pretending to be biting something. He starting backing off after noticing that.)");
                    Mes.Add("([gn:" + Sardine + "] seems to be wanting to run away.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Zacks))
                {
                    Mes.Add("(She's staring at [gn:" + Zacks + "]. She seems to be full of thoughts after looking at him. What could she be thinking about?)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Nemesis))
                {
                    Mes.Add("([gn:" + Nemesis + "] stares at her, while she stares at [gn:"+Nemesis+"]. I don't think anything will come out of this.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Alex))
                {
                    Mes.Add("(She's petting [gn:" + Alex + "] in the head. He seems to be enjoying It.)");
                    Mes.Add("(She threw a bone for [gn:" + Alex + "] to pick. After brought It back, she petted him.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Leopold))
                {
                    Mes.Add("(She gave a scare on [gn:" + Leopold + "]. Now I should be trying to find some leaves.)");
                    Mes.Add("([gn:" + Leopold + "] seems to be trying to avoid being surprised by her.)");
                    Mes.Add("(She seems to be blushing, and avoiding to stare directly at [gn:"+Leopold+"].)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Mabel))
                {
                    Mes.Add("(She looks at [gn:" + Mabel + "] up and down, then she looked at her own body. Is she trying to compare them?)");
                    Mes.Add("(She tries posing like [gn:" + Mabel + "], but by the way she shook her head, It seems like she didn't liked the idea.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Vladimir))
                {
                    Mes.Add("(As she looke at [gn:" + Vladimir + "], he opened his arms, which made her go hug him. She seems to be crying.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Michelle))
                {
                    Mes.Add("([gn:" + Michelle + "] is playing with her. They seems to be liking It.)");
                    Mes.Add("([gn:" + Michelle + "] tried petting her, but the hand passed through her body.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Glenn))
                {
                    Mes.Add("(She smiles and waves at [gn:" + Glenn + "]. He also waved back at her while smiling.)");
                    Mes.Add("(It seems like both her and [gn:" + Glenn + "] are great friends.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Cinnamon))
                {
                    Mes.Add("(She stares at [gn:" + Cinnamon + "], then showed a soft smile.)");
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("(She seems really happy for having you as her room mate.)");
                    Mes.Add("(She seems to be trying to apologize for the scare she gave you on the last time you slept.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.Downed:
                    {
                        if (Sender.TargetType == TriggerTarget.TargetTypes.TerraGuardian && Sender.TargetID == guardian.WhoAmID)
                        {
                            SpawnSoul(guardian);
                        }
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public void SpawnSoul(TerraGuardian guardian)
        {
            FlufflesData data = (FlufflesData)guardian.Data;
            data.SoulPosition = guardian.CenterPosition;
            data.SoulPosition.Y += guardian.Height * 0.25f;
            data.SoulPosition.X += guardian.Direction * guardian.Width * 0.25f;
            data.SoulVelocity = Vector2.Zero;
            data.SoulAttached = false;
            data.SoulOpacity = 0f;
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems to be telling that likes having your company.)");
            Mes.Add("(She shows you a portrait she carries with her, having a male and a female terrarian, a terraguardian, and her. She seems to be pointing at the terraguardian in the image. Maybe that was her boyfriend, or husband?)");
            Mes.Add("(She looks thoughtful.)");
            Mes.Add("(She touched your heart, then smiled at you, and then pulled the paw.)");
            Mes.Add("(She gave you a hug, which your felt, as she smiled while having her eyes closed.)");
            Mes.Add("(She kneels on the floor and stares at you for a while.)");
            Mes.Add("(She leaned on your shoulder for a while.)");
            Mes.Add("(She seems to be thanking you for breaking her haunt.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(After you asked, she shook her head.)");
            Mes.Add("(She waves the pointing finger left and right.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.request.Base is TravelRequestBase)
                Mes.Add("(She seems to be feeling lonelly, maybe she wants to [objective]?)");
            else
            {
                Mes.Add("(After you asked if she had a request, she pulled a list of things she wants you to do. The list contains this: [objective])");
                Mes.Add("(She seems to be happy after you asked that, then gave you a list of things she needs. She asks you to: [objective])");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(After you reported the progress, she looked very joyful.)");
            Mes.Add("(Upon receiving what she asked, she gave you a thankful hug.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("(She stares at you with a menacing look, doesn't looks happy for not having a house to stay.)");
            }
            else
            {
                Mes.Add("(She sketches a house in the air using her fingers, she seems to want a place to live.)");
                Mes.Add("(She seems to be wanting a place to stay.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems very happy because of the surprise party that was prepared for her.)");
            Mes.Add("(She seems eager to return to the party.)");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("(She seems to be expecting you to give her something, but is trying to pretend not.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("(Tear drops are falling on your body.)");
                Mes.Add("(She's trying to ease your pain.)");
                Mes.Add("(She's trying to make you comfortable on the floor.)");
            }
            else
            {
                if (!IsPlayer && Guardian.ID == Leopold && Guardian.ModID == MainMod.mod.Name)
                {
                    Mes.Add("(She seems to be trying her best to reanimate him.)");
                    Mes.Add("(She seems to be crying while attempting to revive him.)");
                    Mes.Add("(She seems to be trying to make him comfortable, while healing his wounds.)");
                }
                else
                {
                    Mes.Add("(She's trying to soothe the one she's tending.)");
                    Mes.Add("(She's trying to reduce the pain.)");
                    Mes.Add("(She seems to be tending the wounds.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Leopold)
                {
                    Weight = 1.5f;
                    return "(She gave a shy looking greeting to "+WhoJoined.Name+".)";
                }
            }
            Weight = 1f;
            return "(She looks happy for meeting someone new.)";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Leopold:
                        Weight = 1.5f;
                        return "(She greets "+WhoJoined.Name+", and ends up spooking him out.)";
                }
            }
            Weight = 1f;
            return "(She waves at the person, while smiling.)";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "(You feel a friendly presence tending you.)";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "(She woke up, then yawned, and is looking at you with a sleepy face.)";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "(She jumped out of the bed, and is now looking at you excited, wanting to know if you did what she asked for.)";
                case MessageIDs.BuddySelected:
                    return "(She doesn't knows how to react for you to choosing her as buddy, she looks happy for the choice.)";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "(She jumps out of excitement.)";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "(She looks at the size of your group, and then denies.)";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "(She doesn't seems very interessed in joining your group right now.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "(She seems to be worried about the place you want her to leave the group. Are you sure you want to do that?)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "(She nods and then gives you a farewell.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "(She leaves your team with a sad look in her face.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "(She smiled after you said no.)";
                case MessageIDs.RequestAccepted:
                    return "(She nods at you. She seems to be counting on you.)";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "(She seems to have noticed that you have many things to do, and decided not to give you her request.)";
                case MessageIDs.RequestRejected:
                    return "(She looked a bit sad after you rejected.)";
                case MessageIDs.RequestPostpone:
                    return "(She's looking at you with a question mark face.)";
                case MessageIDs.RequestFailed:
                    return "(Her face shows the disappointment your failure brought, but she seems to recognize that you did your best.)";
                case MessageIDs.RequestAsksIfCompleted:
                    return "(She looks a bit excited. I think she knows you completed her request.)";
                case MessageIDs.RequestRemindObjective:
                    return "(Just as precaution, she sketched on the ground what she asked you to do: [objective])";
                case MessageIDs.RestAskForHowLong:
                    return "(She seems to like the idea. She seems to be asking for how long will rest.)";
                case MessageIDs.RestNotPossible:
                    return "(She seems worried about the moment you asked that. Maybe It's better to try again another time.)";
                case MessageIDs.RestWhenGoingSleep:
                    return "(She yawned, then went to bed.)";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "(She's calling you attention, and pointing at [shop]'s shop.)";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "(She signals you to wait a moment.)";
                case MessageIDs.GenericYes:
                    return "(She nods)";
                case MessageIDs.GenericNo:
                    return "(She shook her head.)";
                case MessageIDs.GenericThankYou:
                    return "(She bows as a Thank You.)";
                case MessageIDs.GenericNevermind:
                    return "(She acknowledges that.)";
                case MessageIDs.ChatAboutSomething:
                    return "(She seems to be waiting for your questions.)";
                case MessageIDs.NevermindTheChatting:
                    return "(She nods to you.)";
                case MessageIDs.CancelRequestAskIfSure:
                    return "(She looks at you like as if didn't believed what you said. You're really wanting to cancel her request?)";
                case MessageIDs.CancelRequestYesAnswered:
                    return "(She lowers her head, then shook It slowly side ways.)";
                case MessageIDs.CancelRequestNoAnswered:
                    return "(She got a smile on her face after hearing that.)";
                case MessageIDs.LeopoldMessage1:
                    return "(Her face turned red while looking at the Rabbit Guardian. Her facial expression also changed, she seems to be shy.)";
                case MessageIDs.LeopoldMessage2:
                    return "*Is that... AAAAAHHH!! A ghost!!! Help!*";
                case MessageIDs.LeopoldMessage3:
                    return "(After he screamed, she looked at you with a sad face, and your character tried comforting her, while you looked at him with an angry face.)";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "(She calls your attention to the Life Crystal.)";
                case MessageIDs.FoundPressurePlateTile:
                    return "(She pulls your shoulder, then points at the Pressure Plate.)";
                case MessageIDs.FoundMineTile:
                    return "(She shows you the Mine in the floor.)";
                case MessageIDs.FoundDetonatorTile:
                    return "(She gestures you to not try pressing the Detonator.)";
                case MessageIDs.FoundPlanteraTile:
                    return "(As she notices the Bulb, she cowers behind you.)";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "(She nods to you, after seeing the Eternia Crystal Stand.)";
                case MessageIDs.FoundTreasureTile:
                    return "(She jumps happily while pointing at the Chest.)";
                case MessageIDs.FoundGemTile:
                    return "(She calls your attention to the Gems around.)";
                case MessageIDs.FoundRareOreTile:
                    return "(She tells you of ores around.)";
                case MessageIDs.FoundVeryRareOreTile:
                    return "(She shows you some rare ores she found.)";
                case MessageIDs.FoundMinecartRailTile:
                    return "(She points at the Minecart Rails like a kid wanting to go on a rollercoaster.)";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "(She gives a sigh of relief.)";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "(She waves at the person, while smiling.)";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "(She looks happy for meeting someone new.)";
                case MessageIDs.CompanionInvokesAMinion:
                    return "(She got a evil look in her eyes.)";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "(She seems to not be able to believe what is happening.)";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "(She panicks when she notices your fall.)";
                case MessageIDs.LeaderDiesMessage:
                    return "(Her face is filled with sorrow for your demise.)";
                case MessageIDs.AllyFallsMessage:
                    return "(She points at a fallen ally.)";
                case MessageIDs.SpotsRareTreasure:
                    return "(She looks excited to see what is that item that fell.)";
                case MessageIDs.LeavingToSellLoot:
                    return "(She points at the loot, telling that she'll be selling the loot.)";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "(She looks scared after noticing your health low.)";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "(She starts to fade a bit.)";
                case MessageIDs.RunningOutOfPotions:
                    return "(She points at the potions, telling that she's running out of them.)";
                case MessageIDs.UsesLastPotion:
                    return "(She looks shocked after noticing that's her last potion.)";
                case MessageIDs.SpottedABoss:
                    return "(She looks scared upon seeing that creature.)";
                case MessageIDs.DefeatedABoss:
                    return "(She sighs in relief at the creature's fall.)";
                case MessageIDs.InvasionBegins:
                    return "(She readies her weapon.)";
                case MessageIDs.RepelledInvasion:
                    return "(She celebrates the victory.)";
                case MessageIDs.EventBegins:
                    return "(She stares at the sky, wondering what is going on.)";
                case MessageIDs.EventEnds:
                    return "(She seems glad It's over.)";
                case MessageIDs.RescueComingMessage:
                    return "(She hastily tries to reach the fallen ally.)";
                case MessageIDs.RescueGotMessage:
                    return "(She got relieved after catching the fallen ally.)";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "(She shows you the portrait of someone that she was hugging. The name [player] is written in the bottom of it.)";
                case MessageIDs.FeatMentionBossDefeat:
                    return "(She makes a drawing telling that someone defeated [subject].)";
                case MessageIDs.FeatFoundSomethingGood:
                    return "(She draws on the floor that someone found what seems to be a [subject].)";
                case MessageIDs.FeatEventFinished:
                    return "(She does a drawing depicting a [subject] happening in a world, and someone facing it.)";
                case MessageIDs.FeatMetSomeoneNew:
                    return "(She drew someone meeting another person.)";
                case MessageIDs.FeatPlayerDied:
                    return "(She looks saddened while embracing the picture of someone. Once you asked about the picture, she showed you a grayed photo of a Terrarian. The name [player] is written at the bottom.)";
                case MessageIDs.FeatOpenTemple:
                    return "(She drew the image of someone opening the door of a kind of temple.)";
                case MessageIDs.FeatCoinPortal:
                    return "(She drew the image of someone finding a portal that rained coins.)";
                case MessageIDs.FeatPlayerMetMe:
                    return "(She seems to be happy about meeting someone new recently.)";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "(She looks impressed that someone managed to get many fish.)";
                case MessageIDs.FeatKilledMoonLord:
                    return "(She tells you of a Terrarian who saved their world from evil.)";
                case MessageIDs.FeatStartedHardMode:
                    return "(She seems to be telling you of something horrible that happened at someone's world.)";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "(She seems happy that someone Terrarian picked another person as their buddy.)";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "(She crouches to watch attentiously watch you're going to say, while smiling very happily.)";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "(She seems amazed that someone got a house somewhere else.)";
                case MessageIDs.DeliveryGiveItem:
                    return "(She gave [target] some [item].)";
                case MessageIDs.DeliveryItemMissing:
                    return "(She seems to be looking for something in her bag, but can't find.)";
                case MessageIDs.DeliveryInventoryFull:
                    return "(She tried to give some item to [target], but couldn't because they can't carry anymore.)";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "(She nods, and awaits for members.)";
                case MessageIDs.CommandingLeadGroupFail:
                    return "(She denied.)";
                case MessageIDs.CommandingDisbandGroup:
                    return "(She nods, and tells their members to disperse.)";
                case MessageIDs.CommandingChangeOrder:
                    return "(She nods as she acknowledges the order change.)";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            g.AddFlag(GuardianFlags.HideKOBar);
            g.AddFlag(GuardianFlags.FallDamageImmunity);
            g.AddFlag(GuardianFlags.CantDie);
            g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            g.AddFlag(GuardianFlags.CantTakeDamageWhenKod);
            g.AddFlag(GuardianFlags.BreathUnderwater);
            g.AddFlag(GuardianFlags.FireblocksImmunity);
            g.AddBuffImmunity(Terraria.ID.BuffID.Suffocation);
        }

        public static Color GhostfyColor(Color Original, float KoAlpha, float ColorMod)
        {
            Color color = Original;
            color.R -= (byte)(color.R * ColorMod);
            color.G += (byte)((255 - color.G) * ColorMod);
            color.B += (byte)((255 - color.B) * ColorMod);
            color *= KoAlpha;
            color *= 0.75f;
            //color.A -= (byte)(color.A * ColorMod);
            return color;
        }

        public static float GetColorMod { get { if (TerraGuardian.DrawingIgnoringLighting) return 1f; return (float)Math.Sin(Main.GlobalTime * 3) * 0.3f + 0.3f; } }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            float KoAlpha = 1, ColorMod = 0;
            FlufflesData data = (FlufflesData)guardian.Data;
            if (!TerraGuardian.DrawingIgnoringLighting)
            {
                KoAlpha = data.KnockoutAlpha;
                ColorMod = GetColorMod;
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
            {
                if(gdd.textureType == GuardianDrawData.TextureType.TGHeadAccessory)
                {
                    if (guardian.KnockedOut)
                    {
                        gdd.color = GhostfyColor(gdd.color, KoAlpha, 1f);
                    }
                }
                else if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
                }
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
            {
                if (gdd.textureType == GuardianDrawData.TextureType.TGHeadAccessory)
                {
                    if (guardian.KnockedOut)
                    {
                        gdd.color = GhostfyColor(gdd.color, KoAlpha, 1f);
                    }
                }
                else if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
                }
            }
            if(data.SoulOpacity > 0)
            {
                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.Effect, sprites.GetExtraTexture(FoxSoulTextureID), data.SoulPosition - Main.screenPosition, 
                    new Rectangle(16 * (int)(data.SoulFrame * 0.25f), 0, 16, 20), Color.White * data.SoulOpacity * 0.75f, 0f, new Vector2(8, 10), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                Main.playerDrawData.Add(gdd.GetDrawData());
                //TerraGuardian.DrawFront.Add(gdd);
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if (Frame == ReviveFrame || Frame == BackwardRevive)
                return;
            if (Frame == 0)
            {
                if (guardian.OffsetY >= 2)
                {
                    Frame = 2;
                }
                else if (guardian.OffsetY <= -2)
                {
                    Frame = 1;
                }
            }
            if (Frame == BackwardStanding)
            {
                if (guardian.OffsetY >= 2)
                {
                    Frame = BackwardStanding + 2;
                }
                else if (guardian.OffsetY <= -2)
                {
                    Frame = BackwardStanding + 1;
                }
            }
            if (guardian.Velocity.Y != 0 && Frame == JumpFrame)
            {
                if (Math.Abs(guardian.Velocity.Y) < 1)
                    Frame = 1;
                else if (guardian.Velocity.Y >= 1)
                {
                    Frame = 13;
                }
            }
            else if ((guardian.Velocity.X > 0 && guardian.Direction < 0) || (guardian.Velocity.X < 0 && guardian.Direction > 0))
            {
                if (Frame >= 3 && Frame <= 5)
                {
                    Frame += 9;
                }
            }
        }

        public static bool IsFriendlyHauntActive(TerraGuardian guardian)
        {
            return guardian.DoAction.InUse && guardian.DoAction.IsGuardianSpecificAction && guardian.DoAction is Creatures.Fluffles.FriendlyHauntAction;
        }

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> Dialogues = base.GetGuardianExtraDialogueActions(guardian);
            if(IsFriendlyHauntActive(guardian))
            {
                Creatures.Fluffles.FriendlyHauntAction action = (Creatures.Fluffles.FriendlyHauntAction)guardian.DoAction;
                if(action.TargetIsPlayer)
                {
                    if(action.GetPlayer.whoAmI == Main.myPlayer)
                    {
                        Dialogues.Add(new DialogueOption("Get off my shoulder.", delegate ()
                        {
                            action.InUse = false;
                            guardian.SaySomething("(She did as you asked.)");
                            GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
                        }));
                    }
                }
                else
                {
                    Dialogues.Add(new DialogueOption("Get off " + action.GetGuardian.Name + " shoulders.", delegate ()
                    {
                        action.InUse = false;
                        guardian.SaySomething("(She did as you asked.)");
                        GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
                    }));
                    Dialogues.Add(new DialogueOption("I wanted to speak with " + action.GetGuardian.Name + ".", delegate ()
                    {
                        GuardianMouseOverAndDialogueInterface.StartDialogue(action.GetGuardian);
                    }));
                }
            }
            else
            {
                if(guardian.OwnerPos == Main.myPlayer && HasMountableCompanions(Main.LocalPlayer))
                {
                    Dialogues.Add(new DialogueOption("Mount on someones shoulder", MountOnSomeonesShoulderDialogue));
                }
            }
            return Dialogues;
        }

        public TerraGuardian[] GetWhoSheCanMountOn(Player player)
        {
            List<TerraGuardian> Mountables = new List<TerraGuardian>();
            foreach(TerraGuardian tg in player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
            {
                if(tg.Active && (tg.ID != Fluffles || tg.ModID != MainMod.mod.Name))
                {
                    Mountables.Add(tg);
                }
            }
            return Mountables.ToArray();
        }

        public bool HasMountableCompanions(Player player)
        {
            return GetWhoSheCanMountOn(player).Length > 0;
        }

        public void MountOnSomeonesShoulderDialogue()
        {
            TerraGuardian Fluffles = Dialogue.GetSpeaker;
            TerraGuardian[] Guardians = GetWhoSheCanMountOn(Main.LocalPlayer);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            GuardianMouseOverAndDialogueInterface.SetDialogue("(She nods, and awaits you to tell who.)");
            for(int i = 0; i < Guardians.Length; i++)
            {
                TerraGuardian tg = Guardians[i];
                GuardianMouseOverAndDialogueInterface.Options.Add(new DialogueOption(tg.Name, delegate ()
                {
                    Fluffles.StartNewGuardianAction(new Creatures.Fluffles.FriendlyHauntAction(tg, true));
                    GuardianMouseOverAndDialogueInterface.SetDialogue("(She does as you asked.)");
                    GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
                }));
            }
            GuardianMouseOverAndDialogueInterface.Options.Add(new DialogueOption("Nevermind", delegate ()
            {
                GuardianMouseOverAndDialogueInterface.SetDialogue("(She nods, and wonders what else you want to talk about.)");
                GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
            }));
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            FlufflesData data = (FlufflesData)guardian.Data;
            if (!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0) //guardian.BodyAnimationFrame != ReviveFrame
                guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
            if (guardian.KnockedOut)
            {
                guardian.ReviveBoost += 2;
                if (data.KnockoutAlpha > 0)
                {
                    data.KnockoutAlpha -= 0.005f;
                    if (data.KnockoutAlpha <= 0)
                    {
                        data.KnockoutAlpha = 0;
                        if(guardian.OwnerPos == -1)
                        {
                            guardian.WofFood = false;
                            guardian.RemoveBuff(Terraria.ID.BuffID.Horrified);
                            guardian.Spawn();
                        }
                    }
                }
                else
                {
                    if (guardian.OwnerPos > -1)
                    {
                        Player player = Main.player[guardian.OwnerPos];
                        guardian.Position.X = player.Center.X;
                        guardian.Position.Y = player.position.Y + player.height - 1;
                        guardian.LookingLeft = player.direction < 0;
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                Tile t = Main.tile[(int)(guardian.Position.X * (1f / 16)), (int)(guardian.CenterY * (1f / 16))];
                bool ReduceOpacity = Main.dayTime && !Main.eclipse && guardian.Position.Y < Main.worldSurface * 16 && (t.wall == 0 || t.active() && !Main.tileSolid[t.type]);
                if (ReduceOpacity)
                {
                    float MinOpacity = 0.2f;
                    if(Math.Abs(guardian.Position.X - Main.player[Main.myPlayer].Center.X) > 120 || 
                        Math.Abs(guardian.CenterY - Main.player[Main.myPlayer].Center.Y) > 120)
                    {
                        if (guardian.OwnerPos == Main.myPlayer)
                            MinOpacity = 0.1f;
                        else
                            MinOpacity = 0;
                    }
                    if (data.KnockoutAlpha > MinOpacity)
                    {
                        data.KnockoutAlpha -= 0.005f;
                        if (data.KnockoutAlpha < MinOpacity)
                            data.KnockoutAlpha = MinOpacity;
                    }
                    else if (data.KnockoutAlpha < MinOpacity)
                    {
                        data.KnockoutAlpha += 0.005f;
                        if (data.KnockoutAlpha > MinOpacity)
                            data.KnockoutAlpha = MinOpacity;
                    }
                }
                else if (data.KnockoutAlpha < 0.8f)
                {
                    data.KnockoutAlpha += 0.005f;
                    if (data.KnockoutAlpha > 0.8f)
                        data.KnockoutAlpha = 0.8f;
                }
                //fluffles random haunting script.
                if (guardian.OwnerPos == -1 && guardian.TalkPlayerID == -1 && !guardian.UsingFurniture && !guardian.DoAction.InUse && guardian.TargetID == -1 && 
                    Main.rand.Next(300) == 0 && guardian.CurrentIdleAction == TerraGuardian.IdleActions.Wait) //Needs debugging
                {
                    List<KeyValuePair<byte, int>> PossibleTargets = new List<KeyValuePair<byte, int>>();
                    //0 = Player, 1 = Tg
                    const float RangeX = 100, RangeY = 60;
                    for(int p = 0; p < 255; p++)
                    {
                        if(Main.player[p].active && !Main.player[p].dead && !guardian.IsPlayerHostile(Main.player[p]) &&
                            Math.Abs(Main.player[p].Center.X - guardian.Position.X) < RangeX + 20 + guardian.Width * 0.5f &&
                            Math.Abs(Main.player[p].Center.Y - guardian.CenterY) < RangeY + 27 + guardian.Height * 0.5f)
                        {
                            PossibleTargets.Add(new KeyValuePair<byte, int>(0, p));
                        }
                    }
                    foreach(int key in MainMod.ActiveGuardians.Keys)
                    {
                        if(key != guardian.WhoAmID && !guardian.IsGuardianHostile(MainMod.ActiveGuardians[key]) && !guardian.PlayerMounted && !guardian.PlayerControl && guardian.OwnerPos == -1 &&
                            Math.Abs(MainMod.ActiveGuardians[key].Position.X - guardian.Position.X) < RangeX + (guardian.Width + MainMod.ActiveGuardians[key].Width) * 0.5f &&
                            Math.Abs(MainMod.ActiveGuardians[key].CenterY - guardian.CenterY) < RangeY + (guardian.Height + MainMod.ActiveGuardians[key].Height) * 0.5f)
                        {
                            PossibleTargets.Add(new KeyValuePair<byte, int>(1, key));
                        }
                    }
                    if(PossibleTargets.Count > 0)
                    {
                        KeyValuePair<byte, int> PickedTarget = PossibleTargets[Main.rand.Next(PossibleTargets.Count)];
                        if(PickedTarget.Key == 0)
                        {
                            guardian.StartNewGuardianAction(new Creatures.Fluffles.FriendlyHauntAction(Main.player[PickedTarget.Value]));
                        }
                        else
                        {
                            guardian.StartNewGuardianAction(new Creatures.Fluffles.FriendlyHauntAction(MainMod.ActiveGuardians[PickedTarget.Value]));
                        }
                    }
                }
            }
            if (guardian.KnockedOut)
            {
                if (data.SoulOpacity < 1f)
                {
                    data.SoulOpacity += 1f / 60;
                    if (data.SoulOpacity > 1)
                        data.SoulOpacity = 1f;
                }
                else if(guardian.OwnerPos > -1)
                {
                    Player player = Main.player[guardian.OwnerPos];
                    if (data.SoulAttached)
                    {
                        Vector2 EndPosition = player.Center;
                        EndPosition.X += 4 * player.direction;
                        EndPosition.Y += 2 * player.gravDir;
                        data.SoulPosition.X += (EndPosition.X - data.SoulPosition.X) * 0.8f;
                        data.SoulPosition.Y += (EndPosition.Y - data.SoulPosition.Y) * 0.8f;
                    }
                    else if (data.KnockoutAlpha <= 0)
                    {
                        float Distance = player.Center.X - data.SoulPosition.X;
                        const float MaxSoulMoveSpeed = 6f;
                        bool AtPointHorizontally = false, AtPointVertically = false;
                        if (Math.Abs(Distance) < 20)
                        {
                            data.SoulVelocity.X *= 0.3f;
                            AtPointHorizontally = true; // Math.Abs(data.SoulVelocity.X) < 1f;
                        }
                        else
                        {
                            if (Distance < 0)
                            {
                                data.SoulVelocity.X -= 0.15f;
                                if (data.SoulVelocity.X < -MaxSoulMoveSpeed)
                                    data.SoulVelocity.X = -MaxSoulMoveSpeed;
                            }
                            else
                            {
                                data.SoulVelocity.X += 0.15f;
                                if (data.SoulVelocity.X > MaxSoulMoveSpeed)
                                    data.SoulVelocity.X = MaxSoulMoveSpeed;
                            }
                        }
                        Distance = player.Center.Y - data.SoulPosition.Y;
                        if (Math.Abs(Distance) < 28)
                        {
                            data.SoulVelocity.Y *= 0.3f;
                            AtPointVertically = true; //Math.Abs(data.SoulVelocity.Y) < 1f;
                        }
                        else
                        {
                            if (Distance < 0)
                            {
                                data.SoulVelocity.Y -= 0.15f;
                                if (data.SoulVelocity.Y < -MaxSoulMoveSpeed)
                                    data.SoulVelocity.Y = -MaxSoulMoveSpeed;
                            }
                            else
                            {
                                data.SoulVelocity.Y += 0.15f;
                                if (data.SoulVelocity.Y > MaxSoulMoveSpeed)
                                    data.SoulVelocity.Y = MaxSoulMoveSpeed;
                            }
                        }
                        data.SoulPosition += data.SoulVelocity;
                        if (AtPointHorizontally && AtPointVertically)
                        {
                            data.SoulAttached = true;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                if (data.SoulOpacity > 0)
                {
                    data.SoulOpacity -= 1f / 60;
                    if (data.SoulOpacity < 0)
                        data.SoulOpacity = 0f;
                }
            }
            if(data.SoulOpacity > 0)
            {
                Lighting.AddLight(data.SoulPosition, new Vector3(0.82f, 2.17f, 1.61f) * 0.33f * data.SoulOpacity);
                data.SoulFrame++;
                if (data.SoulFrame >= 4 * 6)
                    data.SoulFrame -= 4 * 6;
                if (Main.rand.Next(3) == 0)
                    Dust.NewDust(data.SoulPosition - new Vector2(8, 10), 16, 20, 75, 0f, -0.5f); //CursedTorchID
            }
        }

        public static bool IsCompanionHaunted(TerraGuardian tg)
        {
            return tg.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.FriendlyHaunt>());
        }

        public class FlufflesData : GuardianData
        {
            public Vector2 SoulPosition = Vector2.Zero, SoulVelocity = Vector2.Zero;
            public bool SoulAttached = false;
            public float SoulOpacity = 0f;
            public float KnockoutAlpha = 0f;
            public byte SoulFrame = 0;

            public FlufflesData(int ID, string ModID = "") : base(ID, ModID)
            {

            }
        }
    }
}
