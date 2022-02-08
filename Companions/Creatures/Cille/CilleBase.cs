using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions
{
    public class CilleBase : GuardianBase
    {
        public const string DefaultOutfitBodyID = "default_o_body", DefaultOutfitBodyFrontID = "default_o_bodyfront", DefaultOutfitLeftArmID = "default_o_left", DefaultOutfitRightArmID = "default_o_right";
        public const string MurderFaceID = "murderface";
        public const int DefaultOutfitID = 1;
        public const int BeastStateID = 0;

        public CilleBase()
        {
            Name = "Cille";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 90;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 96f / 90;
            CompanionSlotWeight = 1.15f;
            Age = 21;
            SetBirthday(SEASON_SUMMER, 7);
            Male = false;
            InitialMHP = 225; //950
            LifeCrystalHPBonus = 35;
            LifeFruitHPBonus = 10;
            Accuracy = 0.89f;
            Mass = 0.48f;
            MaxSpeed = 5.6f;
            Acceleration = 0.18f;
            SlowDown = 0.52f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            FriendsLevel = 3;
            CallUnlockLevel = 3;
            MoveInLevel = 5;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 15;
            ChairSittingFrame = 14;
            ThroneSittingFrame = 17;
            BedSleepingFrame = 16;
            SleepingOffset.X = 16;
            ReviveFrame = 18;
            DownedFrame = 19;
            //PetrifiedFrame = 28;

            BackwardStanding = 20;
            //BackwardRevive = 30;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(15, 0);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(20, 34);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 11, 3);
            LeftHandPoints.AddFramePoint2x(11, 33, 11);
            LeftHandPoints.AddFramePoint2x(12, 36, 21);
            LeftHandPoints.AddFramePoint2x(13, 30, 30);

            LeftHandPoints.AddFramePoint2x(14, 25, 26);
            LeftHandPoints.AddFramePoint2x(15, 25, 26);

            LeftHandPoints.AddFramePoint2x(18, 35, 33);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 23, 3);
            RightHandPoints.AddFramePoint2x(11, 35, 11);
            RightHandPoints.AddFramePoint2x(12, 38, 21);
            RightHandPoints.AddFramePoint2x(13, 32, 30);

            RightHandPoints.AddFramePoint2x(14, 29, 27);
            RightHandPoints.AddFramePoint2x(15, 34, 24);

            RightHandPoints.AddFramePoint2x(18, 42, 33);

            //Mounted Placement
            MountShoulderPoints.DefaultCoordinate2x = new Point(16, 15);

            //Headgear Placement
            HeadVanityPosition.DefaultCoordinate2x = new Point(24, 11);
            HeadVanityPosition.AddFramePoint2x(18, 30, 18);

            AddOutfit(DefaultOutfitID, "Bad Outfit", delegate (GuardianData gd, Player player) { return true; });
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new CilleData(ID, ModID);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(DefaultOutfitBodyID, "outfit_body");
            sprites.AddExtraTexture(DefaultOutfitBodyFrontID, "outfit_bodyfront");
            sprites.AddExtraTexture(DefaultOutfitLeftArmID, "outfit_leftarm");
            sprites.AddExtraTexture(DefaultOutfitRightArmID, "outfit_rightarm");
            sprites.AddExtraTexture(MurderFaceID, "murder_face");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            CilleData data = (CilleData)guardian.Data;
            if (data.InBeastState)
            {
                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(MurderFaceID), DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), Color.White, Rotation, Origin, Scale, seffect);
                InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
            }
            switch (guardian.OutfitID)
            {
                case DefaultOutfitID:
                    {
                        Texture2D OutfitTexture = sprites.GetExtraTexture(DefaultOutfitBodyID),
                            LeftArmTexture = sprites.GetExtraTexture(DefaultOutfitLeftArmID),
                            RightArmTexture = sprites.GetExtraTexture(DefaultOutfitRightArmID),
                            BodyFrontTexture = sprites.GetExtraTexture(DefaultOutfitBodyFrontID);
                        GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, OutfitTexture, DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                        if (guardian.BodyAnimationFrame == SittingFrame)
                        {
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BodyFrontTexture, DrawPosition, guardian.GetAnimationFrameRectangle(BodyFrontFrameSwap[guardian.BodyAnimationFrame]), color, Rotation, Origin, Scale, seffect);
                            InjectTextureAfter(GuardianDrawData.TextureType.TGBodyFront, gdd);
                        }
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, LeftArmTexture, DrawPosition, guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGLeftArm, gdd);
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, RightArmTexture, DrawPosition, guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame), color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGRightArm, gdd);
                    }
                    break;
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            CilleData data = (CilleData)guardian.Data;
            if (data.InBeastState)
            {
                if (guardian.KnockedOut)
                {
                    data.InBeastState = false;
                }
                else if (!TriggerBeastState(guardian))
                {
                    data.InBeastState = false;
                    if (!guardian.KnockedOut && !guardian.Downed)
                    {
                        string Message = "";
                        switch (Main.rand.Next(3))
                        {
                            default:
                                Message = "*Huh? It's over... I hope I didn't hurt anyone.*";
                                break;
                            case 1:
                                Message = "*What happened? Did someone got hurt?*";
                                break;
                            case 2:
                                Message = "*I'm so glad it's over. I didn't hurt anyone, right?*";
                                break;
                        }
                        guardian.SaySomethingCanSchedule(Message, false, Main.rand.Next(30, 180));
                    }
                    /*if (!guardian.DoAction.InUse)
                    {
                        data.InBeastState = false;
                    }*/
                }
                else
                {
                    if (guardian.OwnerPos > -1 && !guardian.IsPlayerBuddy(Main.player[guardian.OwnerPos]))
                    {
                        if (!NpcMod.HasGuardianNPC(guardian.ID, guardian.ModID))
                            WorldMod.GuardianTownNPC.Add(guardian);
                        Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian(guardian.ID, guardian.ModID);
                    }
                }
            }
            else
            {
                if (TriggerBeastState(guardian))
                {
                    if (!guardian.DoAction.InUse)
                    {
                        guardian.StartNewGuardianAction(new Companions.Creatures.Cille.BeastStateAction(), BeastStateID);
                    }
                }
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.RangedDamageMultiplier += 0.03f;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Stay away from me, please.*");
            Mes.Add("*Don't come closer. Leave me alone.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CallUnlockMessage => "*Do you... Need help on your adventures..? Can I accompany you..?*";
        public override string MoveInUnlockMessage => "*..I'd like to have more contact with you, if you want...*";

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Add("(She seems to be having a nightmare.)");
                Mes.Add("(She seems to be telling someone to stop. Did she say her own name?)");
                Mes.Add("(You see tears falling from her eyes.)");
            }
            else if (guardian.FriendshipLevel < FriendsLevel)
            {
                Mes.Add("*Leave me alone...*");
                if(guardian.FriendshipLevel > 0)
                    Mes.Add("*You returned...*");
                Mes.Add("*Don't come closer..*");
                Mes.Add("*Stay away..*");
                Mes.Add("*I hurt someone in the past... I didn't wanted to.. But I couldn't stop.. Please... Leave me alone..*");
                Mes.Add("*I'm nobody.. Go away..*");
                if (!Main.dayTime)
                {
                    Mes.Add("*You... Avoid the dark around me..*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*Grr!! Rawr!!*");
                    Mes.Add("*Arrgh!! Grrr!! Rrr!!*");
                }
                else
                {
                    Mes.Add("*You always came back to see me... Thank you..*");
                    Mes.Add("*You came to see me?*");
                    Mes.Add("*It's not that I don't like people... It's just... Safer for us if we don't stay close.*");
                    Mes.Add("*...*");
                    if (Main.dayTime)
                    {
                        Mes.Add("*Are you finding it weird that I only eat fruits and bugs? Sorry, I'm avoiding touching meat.*");
                        if (!Main.raining)
                        {
                            Mes.Add("*It seems like a good day for running.*");
                            Mes.Add("*Do you want to race against me, [nickname]?*");
                        }
                        else
                            Mes.Add("*Aww... It's raining..*");
                        Mes.Add("*I think I can talk, while it's still daytime.*");
                    }
                    else
                    {
                        Mes.Add("*Feeling tired..? Go home get some sleep.*");
                        Mes.Add("*I can't forget that night... What night? Forget that I said that.*");
                    }
                    if (guardian.IsPlayerRoomMate(player))
                    {
                        Mes.Add("*I don't like the idea of sharing a room with you. What if I...*");
                    }
                    if (guardian.IsUsingToilet)
                    {
                        Mes.Clear();
                        Mes.Add("*Is it really necessary of you to speak with me right now?*");
                        Mes.Add("*You're distracting me right now. Please, go away.*");
                        Mes.Add("*I can talk to you any other time than this.*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Have you ever hurt someone dear for you? I did when I was younger. I'm avoiding people because of that..*");
            Mes.Add("*I used to have everything when I was younger. Due to not controlling myself, I hurt someone, and now am a run away, wandering through worlds..*");
            Mes.Add("*There was one person who used to visit me, before I moved to this place. I wonder if they miss me..*");
            Mes.Add("*I wonder how are things back home..*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.FriendshipLevel < FriendsLevel)
            {
                Mes.Add("*No. Go away.*");
                Mes.Add("*I don't. Please leave.*");
            }
            else
            {
                Mes.Add("*Not right now.*");
                Mes.Add("*I have everything I need for now.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.FriendshipLevel < FriendsLevel)
            {
                Mes.Add("*If you help me, will you go away? Then please [objective].*");
                Mes.Add("*Fine... If you say so.. [objective] is what I need. Can you do that?*");
            }
            else
            {
                Mes.Add("*I'm glad you asked. I need you to [objective]. Can you?*");
                Mes.Add("*There is something I need done.. [objective] is it. Can you help me with it?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.FriendshipLevel < FriendsLevel)
            {
                Mes.Add("*...Thanks..*");
            }
            else
            {
                Mes.Add("*Thank you so much..*");
                Mes.Add("*I'm happy that you helped me..*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            return (Main.rand.Next(2) == 0 ? "*..How did you knew It was my birthday?*" : "*Yes.. I'm growing older..*");
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            if(Guardian.FriendshipLevel >= FriendsLevel)
            {
                if (Main.rand.Next(2) == 0)
                    return "*Yes.. Continue breathing..*";
                return "*I'll help you...*";
            }
            return "*...*";
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.Next(2) == 0)
                return "*I don't like living the way I live. I'd like to have a place for myself, which also should be away from other people.*";
            return "*I'd like to talk to you more, but not to stay too close. Do you have some place I could stay?*";
        }

        public static bool TriggerBeastState(TerraGuardian tg)
        {
            return !Main.dayTime && Main.moonPhase == 3 && tg.Position.X < Main.worldSurface * 16;
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*..I don't know why you picked me, but.. Thank you...*";
                case MessageIDs.RescueMessage:
                    return "*I found you, let me help.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "*..I could at least get some sleep. I know nothing happens if I do.*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "*You woke me up. Is it about my request?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I can join you, just please run away if I act strange.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's too many people.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*No... Leave me here.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*Couldn't you take me close to my home, first?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I'll be back home, then..*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I guess I'll have to find my way home, then...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Thank you.*";
                case MessageIDs.RequestAccepted:
                    return "*You know where to find me.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*Don't you have too many things on your hands?*";
                case MessageIDs.RequestRejected:
                    return "*I can do that later, then.*";
                case MessageIDs.RequestPostpone:
                    return "*Have more important things to do?*";
                case MessageIDs.RequestFailed:
                    return "*You what? Please... Go away..*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Did you do my request?*";
                case MessageIDs.RequestRemindObjective:
                    return "*You forgot what I asked for? It was just to [objective].*";
                case MessageIDs.RestAskForHowLong:
                    return "*I don't think it's safe to rest with me around, but.. If you think it is. How long will we rest?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't a good moment for that.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Back to my nightmares..*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*That is... Let's check [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*I'll trade this for this...";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you.*";
                case MessageIDs.ChatAboutSomething:
                    return "*You want to talk to me..? I don't think I have much that will interest you.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Can you please go now?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You want to cancel my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Why did you accepted it in first place? Sigh... Fine, you no longer need to do it.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*You surprised me.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "*Thank you for caring of me.*";
                case MessageIDs.RevivedByRecovery:
                    return "*Uh? What happened?*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Argh! Poison!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Fire! Fire!! Aaaahh!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head is spinning?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*My arms wont respond!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My feet!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I'm not feeling good..*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Argh! Open wound!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What in the nether is that?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*I hope that's not what I think it is!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Someone has a b-blanket?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Ahhh!! I'm stuck!!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Grrr! GRRR!!!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Try hurting me!*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Delicious. I missed those.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Even my legs feel stronger.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Take my dust!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Amazing!*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Prepare to feel pain!*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*I hope it doesn't ruins meat.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*I was needing this.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I could lie in a bathtub full of this all day.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Look! Life crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Stop! Pressure plate.*";
                case MessageIDs.FoundMineTile:
                    return "*Watch your step.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Better not play with that detonator.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Doesn't look like the kind of thing we should mess with.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Alright, some of us should defend one side, and the rest the other.*";
                case MessageIDs.FoundTreasureTile:
                    return "*That looks interesting. Open it.*";
                case MessageIDs.FoundGemTile:
                    return "*Shiny!*";
                case MessageIDs.FoundRareOreTile:
                    return "*A necklace of that would look good on me.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Jewelry of that would look good on me.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Wow! I found some tracks. Are we going to ride it?*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Back home, we go.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Uh... Hello..*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*There's no end of new people here, right..?*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Summon!*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*That looks soooo cutee.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname] fell!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*Oh no! [nickname]!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone's injured!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*What's that? Looks like you're lucky.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back, all those items are hindering me.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Do you need to rest, [nickname]?*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*I'm not okay...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Wait? I only have all those potions? I need more!*";
                case MessageIDs.UsesLastPotion:
                    return "*Oh no! That was my last potion!*";
                case MessageIDs.SpottedABoss:
                    return "*Giant big bad thing incoming!*";
                case MessageIDs.DefeatedABoss:
                    return "*Yes!*";
                case MessageIDs.InvasionBegins:
                    return "*I'm starting to think I'm the least dangerous thing here..*";
                case MessageIDs.RepelledInvasion:
                    return "*I hope they don't return..*";
                case MessageIDs.EventBegins:
                    return "*What's going on?*";
                case MessageIDs.EventEnds:
                    return "*It's over... Good..*";
                case MessageIDs.RescueComingMessage:
                    return "*I'm coming for you!*";
                case MessageIDs.RescueGotMessage:
                    return "*I'll not let anything touch you.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*There's this terrarian, [player], who keeps coming back to see me..*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I heard that [player] took care of [subject] recently. They seem heroic.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*It looks like [player] found a [subject] during their travels.*";
                case MessageIDs.FeatEventFinished:
                    return "*There was a [subject] that happened recently, and [player] managed to survive it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*I heard that [player] met [subject] latelly. I don't know who [subject] is.*";
                case MessageIDs.FeatPlayerDied:
                    return "*Do you know [player]? They died recently...*";
                case MessageIDs.FeatOpenTemple:
                    return "*A temple was found in [subject], from what I know. What is inside? I think [player] is about to find out.*";
                case MessageIDs.FeatCoinPortal:
                    return "*[player] witnessed a coin portal appearing close to them. They must be richer now.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I've met another Terrarian... Their name was [player].. They also ignored me when I told them to stay away...*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] seems to be enjoying their hobbie of fishing.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*I heard that a giant space creature was slain in [subject] by [player]. Maybe will be a good place to take a vacation.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*I heard that at [subject], things has... Changed..*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Yes, I heard about [player] picking [subject] as buddy. That would never happen to me.. I'm a danger to everyone around me..*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Why? Why did you picked me as your buddy? I feel like I'm endangering you.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public class CilleData : GuardianData
        {
            public bool InBeastState = false;

            public CilleData(int ID, string ModID) : base (ID, ModID)
            {

            }
        }
    }
}
