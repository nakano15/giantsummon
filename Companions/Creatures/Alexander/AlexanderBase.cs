﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giantsummon.Trigger;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace giantsummon.Companions
{
    public class AlexanderBase : GuardianBase
    {
        public const int SleuthStaringAnimationID = 27, SleuthAnimationID = 28, SleuthAnimationID2 = 29, SleuthBackAnimationID = 30;

        public AlexanderBase()
        {
            Name = "Alexander";
            Description = "Member of a mystery solving gang,\nuntil they disappeared, and now he looks for them.\nDoesn't miss a clue.";
            Size = GuardianSize.Large;
            DefaultTactic = CombatTactic.Charge;
            Width = 28;
            Height = 86;
            DuckingHeight = 62;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 94f / 86;
            FramesInRows = 20;
            CompanionSlotWeight = 1.2f;
            Age = 19;
            SetBirthday(SEASON_SUMMER, 4);
            Male = true;
            //CalculateHealthToGive(1200, 0.45f, 0.6f); //Lc: 95, LF: 16
            InitialMHP = 375; //1200
            LifeCrystalHPBonus = 35;
            LifeFruitHPBonus = 15;
            Accuracy = 0.72f;
            Mass = 0.45f;
            MaxSpeed = 5.1f;
            Acceleration = 0.19f;
            SlowDown = 0.39f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.Muramasa, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = PlayerMountedArmAnimation = 9;
            HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 23;
            DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 17;
            ChairSittingFrame = 18;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 20;
            BedSleepingFrame = 19;
            DownedFrame = 21;
            ReviveFrame = 22;

            BackwardStanding = 31;
            BackwardRevive = 32;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);
            BodyFrontFrameSwap.Add(27, 1);
            BodyFrontFrameSwap.Add(28, 1);
            BodyFrontFrameSwap.Add(29, 1);
            BodyFrontFrameSwap.Add(30, 2);

            SleepingOffset.X = 16;
            
            SittingPoint = new Microsoft.Xna.Framework.Point(22 * 2, 35 * 2);

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(17, 14);
            MountShoulderPoints.AddFramePoint2x(15, 23, 19);
            MountShoulderPoints.AddFramePoint2x(16, 29, 26);

            MountShoulderPoints.AddFramePoint2x(17, 25, 13);
            MountShoulderPoints.AddFramePoint2x(18, 25, 13);

            MountShoulderPoints.AddFramePoint2x(20, 16, 21);

            MountShoulderPoints.AddFramePoint2x(22, 24, 24);
            MountShoulderPoints.AddFramePoint2x(23, 24, 24);
            MountShoulderPoints.AddFramePoint2x(24, 24, 24);
            MountShoulderPoints.AddFramePoint2x(25, 24, 24);
            MountShoulderPoints.AddFramePoint2x(26, 24, 24);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 2);
            LeftHandPoints.AddFramePoint2x(11, 34, 9);
            LeftHandPoints.AddFramePoint2x(12, 38, 21);
            LeftHandPoints.AddFramePoint2x(13, 33, 29);

            LeftHandPoints.AddFramePoint2x(14, 34, 2);
            LeftHandPoints.AddFramePoint2x(15, 41, 25);
            LeftHandPoints.AddFramePoint2x(16, 38, 41);

            LeftHandPoints.AddFramePoint2x(22, 39, 37);

            LeftHandPoints.AddFramePoint2x(24, 39, 18);
            LeftHandPoints.AddFramePoint2x(25, 43, 30);
            LeftHandPoints.AddFramePoint2x(26, 35, 40);
            
            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 23, 2);
            RightHandPoints.AddFramePoint2x(11, 38, 9);
            RightHandPoints.AddFramePoint2x(12, 41, 21);
            RightHandPoints.AddFramePoint2x(13, 36, 29);

            RightHandPoints.AddFramePoint2x(14, 36, 2);
            RightHandPoints.AddFramePoint2x(15, 43, 25);
            RightHandPoints.AddFramePoint2x(16, 40, 41);

            RightHandPoints.AddFramePoint2x(22, 44, 37);

            RightHandPoints.AddFramePoint2x(24, 41, 18);
            RightHandPoints.AddFramePoint2x(25, 45, 30);
            RightHandPoints.AddFramePoint2x(26, 37, 40);
            
            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(23 + 1, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(15, 29+ 1, 15 + 2);
            HeadVanityPosition.AddFramePoint2x(16, 36+ 1, 25 + 2);

            HeadVanityPosition.AddFramePoint2x(17, 21+ 1, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 21+ 1, 8 + 2);

            HeadVanityPosition.AddFramePoint2x(22, 31+ 1, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(23, 31+ 1, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(24, 31+ 1, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(25, 31+ 1, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(26, 31+ 1, 21 + 2);
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new AlexanderData(ID, ModID);
        }

        public override void Attributes(TerraGuardian g)
        {
            AlexanderData data = (AlexanderData)g.Data;
            foreach(GuardianID id in data.IdentifiedGuardians)
            {
                ApplySleuthGuardianStatusBonus(g, id);
            }
        }

        public static bool HasAlexanderSleuthedGuardian(Player player, int GuardianID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            if(PlayerMod.PlayerHasGuardian(player, Alexander))
            {
                AlexanderData data = (AlexanderData)PlayerMod.GetPlayerGuardian(player, Alexander);
                foreach (GuardianID id in data.IdentifiedGuardians)
                {
                    if (id.IsSameID(GuardianID, ModID))
                        return true;
                }
            }
            return false;
        }

        public void ApplySleuthGuardianStatusBonus(TerraGuardian g, GuardianID id)
        {
            if (id.ModID == MainMod.mod.Name)
            {
                switch (id.ID)
                {
                    case Rococo:
                        g.Defense++;
                        break;
                    case Blue:
                        g.MeleeSpeed += 0.03f;
                        break;
                    case Sardine:
                        g.JumpHeight++;
                        break;
                    case Alex:
                        g.MoveSpeed += 0.04f;
                        break;
                    case Brutus:
                        g.MeleeDamageMultiplier += 0.02f;
                        g.DefenseRate += 0.01f;
                        break;
                    case Bree:
                        g.MeleeDamageMultiplier += 0.02f;
                        break;
                    case Mabel:
                        g.MHP += 3;
                        break;
                    case Domino:
                        g.RangedDamageMultiplier += 0.03f;
                        break;
                    case Leopold:
                        g.MagicDamageMultiplier += 0.04f;
                        break;
                    case Vladimir:
                        g.MHP += 8;
                        g.HealthRegenPower++;
                        break;
                    case Malisha:
                        g.MMP += 5;
                        break;
                    case Wrath:
                        g.MeleeDamageMultiplier += 0.02f;
                        break;
                    case Fluffles:
                        g.DodgeRate += 3f;
                        break;
                    case Minerva:
                        g.Defense += 2;
                        break;
                    case Daphne:
                        g.CoverRate += 2f;
                        break;
                    case Glenn:
                        g.Accuracy += 0.03f;
                        break;
                    case CaptainStench:
                        g.Aggro -= 30;
                        break;
                    case Liebre:
                        g.DodgeRate += 2f;
                        g.MHP += 3;
                        break;
                    case Cinnamon:
                        g.HealthRegenPower++;
                        g.MaxJumpHeight++;
                        break;
                    case Miguel:
                        g.MeleeDamageMultiplier += 0.03f;
                        g.DefenseRate += 0.02f;
                        break;
                    case Luna:
                        g.MoveSpeed += 0.03f;
                        break;
                }
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You! No, you aren't the Terrarian I'm looking for. I can smell it.*");
            Mes.Add("*You, stop! *snif snif* Hmmm... No, you're not who I wanted to find.*");
            Mes.Add("*Hold on. *snif, snif snif, snif* Hm... I was checking if you were who I'm looking for, and you are not.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Add("(You noticed him sniffing a while, and then he slept with a smile on the face.\nI think he knows I'm near him.)");
                Mes.Add("*Snif.. Snif... "+player.name+" close... Zzzz...*");
                Mes.Add("(As he sleeps, he says the name of anyone who comes close to him, includding yours.)");
            }
            else if (guardian.IsUsingToilet)
            {
                Mes.Add("*Whatever It is, couldn't wait?*");
                Mes.Add("*[nickname], I'm trying to process some things here.*");
                Mes.Add("*Your presence here is making It harder for me to finish what I'm doing.*");
            }
            else
            {
                Mes.Add("*Any update on our little investigation?*");
                Mes.Add("*This mystery is making me intrigued. I've never found something like this before.*");
                Mes.Add("*Is there something you seek, [nickname]?*");
                Mes.Add("*Did you found the Terrarian we are looking for? I can't blame if you didn't, my description is really vague.*");

                Mes.Add("*I can identify anyone by sleuthing them, but not everyone may like that idea.*");
                Mes.Add("*Everytime I sleuth someone, I can replicate part of their strength.*");
                Mes.Add("*I can identify Terrarians by sleuthing them, but I don't get stronger by doing so.*");
                Mes.Add("*The only clue I've got from my investigation, is the scent of a unknown person that was at the place. I will find the one who has that scent.*");

                if (!Main.dayTime && !Main.bloodMoon)
                {
                    Mes.Add("*I have troubles getting into sleep, my head is always filled with thoughts when I get my head on the pillow.*");
                    Mes.Add("*The stars sometimes help me with my thoughts. Lying down on the ground and staring upwards has a mind opening effect.*");
                }
                else if(Main.dayTime && !Main.eclipse)
                {
                    if (Main.raining)
                    {
                        Mes.Add("*The sound of rain drops have a soothening effect.*");
                    }
                    else
                    {
                        Mes.Add("*I don't feel comfortable investigating while exposed to the sun, I get some annoying headaches when I do that.*");
                    }
                }

                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("*How old is [gn:" + Rococo + "]? It doesn't seems like he's that old.*");
                    Mes.Add("*I'm intrigued at how [gn:" + Rococo + "] lives his life. Maybe that's another mystery.*");
                }
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    if (!NpcMod.HasGuardianNPC(Zacks))
                    {
                        Mes.Add("*[gn:" + Blue + "] came earlier wanting me to help her find someone. I'm trying to do something to help her, but that's delaying my personal investigation.*");
                        Mes.Add("*Looks like [gn:" + Blue + "] lost someone too... I wonder if It's the same as... No, certainly not...*");
                        Mes.Add("*I think I discovered some clue about [gn:" + Blue + "]'s missing person. I heard that seems to show up during Blood Moons, on the edges of the world. Better you be careful.*");
                    }
                    else
                    {
                        Mes.Add("*It's good to see that [gn:" + Blue + "] has found who she was looking for, but the result even I didn't expected.*");
                        Mes.Add("*I was shocked when I discovered [gn:" + Zacks + "] fate, but [gn:" + Blue + "]'s reception to that seems to be the reward by Itself.*");
                    }
                }
                if (NpcMod.HasGuardianNPC(Bree))
                {
                    if (!NpcMod.HasGuardianNPC(Sardine))
                    {
                        Mes.Add("*[gn:" + Bree + "] told me that she's looking for her husband. The only clue I got is that he were pursuing the King Slime. I think that may be dangerous, actually.*");
                    }
                }
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    Mes.Add("*I can't really stay in the same room as [gn:" + Mabel + "], she simply distracts me.*");
                    Mes.Add("*I wonder what kind of thing [gn:" + Mabel + "] would like... Wait, I shouldn't be thinking about that kind of thing.*");
                }
                if (NpcMod.HasGuardianNPC(Malisha))
                {
                    Mes.Add("*You should keep a closer eye to [gn:" + Malisha + "], she caused many troubles in the ether realm, on every village she came from.*");
                    Mes.Add("*Never speak with [gn:" + Malisha + "] during a Blood Moon. She catches anyone who tries speaking with her to force doing experiments on.*");
                }
                if (NpcMod.HasGuardianNPC(Wrath))
                {
                    Mes.Add("*A TerraGuardian fragmented into 4 emotions... Where could the others be...?*");
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    Mes.Add("*Sometimes I hang around with [gn:"+Brutus+"], he sometimes gives me ideas for clues I need on my investigations.*");
                    Mes.Add("*I often have to ask [gn:"+Brutus+"] for help to try investigating some place.*");
                }
                if (NpcMod.HasGuardianNPC(Michelle))
                {
                    Mes.Add("*I sleuthed [gn:" + Michelle + "], and she's not the one I'm looking for.*");
                }
                if (NpcMod.HasGuardianNPC(Nemesis))
                {
                    Mes.Add("*I was unable to caught [gn:" + Nemesis + "] scent, no matter how hard I tried. I really hope he isn't the one I'm looking for.*");
                }
                if (NpcMod.HasGuardianNPC(Luna))
                {
                    if (HasAlexanderSleuthedGuardian(player, Luna))
                    {
                        Mes.Add("*[gn:" + Luna + "] is really furious about the fact that I know many things about her.*");
                    }
                    else
                    {
                        Mes.Add("*I will get to know [gn:"+Luna+"] more some time...*");
                    }
                }
                if (NpcMod.HasGuardianNPC(Green))
                {
                    Mes.Add("*It's really great having a medic around. Or at least one that knows how TerraGuardian bodies works.*");
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("*Yeah, I don't mind sharing my room with you, as long as you have a bed for yourself.*");
                    Mes.Add("*You think I sleuth you during your sleep? What else do I need to discover about you?*");
                }
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I don't mean to alarm you, but there is a ghost behind you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I wonder if they are fine... Who? Oh... Nevermind..*");
            Mes.Add("*I used to be part of a group with 3 Terrarians, but then they disappeared after... Something happened..*");
            Mes.Add("*My only hope is finding that Terrarian who made seems involved into my friends disappearance. If I find him, I may find a clue of where they could be.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*No.*");
            Mes.Add("*Not yet.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I can't get myself distracted from my investigation, so I need your help to [objective]. Can you do that for me?*");
            Mes.Add("*There is something else catching my attention, but It would be a great time saving if you did It for me. I need you to [objective], and then report back your progress. What do you say?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's good to know that I can count on you. I think this will suit the time you spent.*");
            Mes.Add("*You completed what I asked for, then? Good. This is a compensation for the time you spent.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.Downed:
                    {
                        if (Sender.TargetType == TriggerTarget.TargetTypes.TerraGuardian)
                        {
                            TerraGuardian tg = MainMod.ActiveGuardians[Sender.TargetID];
                            AlexanderData data = (AlexanderData)guardian.Data;
                            if (!guardian.DoAction.InUse && !tg.MyID.IsSameID(guardian) && tg.Base.IsTerraGuardian && !data.WasGuardianIdentified(tg))
                            {
                                if (guardian.StartNewGuardianAction(new Companions.Alexander.SleuthAction(tg), 0))
                                    return true;
                            }
                        }
                    }
                    break;
                case TriggerTypes.Spotted:
                    {
                        if (Sender.TargetType == TriggerTarget.TargetTypes.TerraGuardian)
                        {
                            TerraGuardian tg = MainMod.ActiveGuardians[Sender.TargetID];
                            AlexanderData data = (AlexanderData)guardian.Data;
                            if (!guardian.DoAction.InUse && !tg.MyID.IsSameID(guardian) && tg.Base.IsTerraGuardian && tg.IsSleeping && !data.WasGuardianIdentified(tg))
                            {
                                if (guardian.StartNewGuardianAction(new Companions.Alexander.SleuthAction(tg), 0))
                                    return true;
                            }
                        }
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.dayTime)
            {
                if (Main.raining)
                {
                    Mes.Add("*Acho~! Snif... I really need some place to stay...*");
                    Mes.Add("*It's getting really cold here... I don't like It...*");
                }
                else
                {
                    Mes.Add("*I'm nearly passing out due to the heat going directly to my brain. I need some place to stay.*");
                    Mes.Add("*It's hard to think with a hot head. I need a place to stay.*");
                }
            }
            else
            {
                Mes.Add("*I can't gather clues and survive at the same time, I need a place to stay!*");
                Mes.Add("*I can't process what I've already discovered, since I need to run, jump and slash things in the woods! Give me some house!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's not that I don't like parties, but my head is actually somewhere else...*");
            Mes.Add("*I don't know if It's positive to celebrate getting older or not... *");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Come on... You've got to help me solve the mystery.*");
                Mes.Add("*I think I just need to do this... It's working.*");
                Mes.Add("*Those wounds...*");
            }
            else
            {
                Mes.Add("*Hold on.*");
            }
            Mes.Add("*I know you're feeling pain, but try not to move.*");
            Mes.Add("*If It helps, hold my hand.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            Weight = 1f;
            return "*Interesting. I'd like to know more about you.*";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Domino:
                        Weight = 1.2f;
                        return "*We aren't going to do anything shady, are we?*";
                    case Minerva:
                        Weight = 1.5f;
                        return "*I like the smell of this.*";
                }
            }
            Weight = 1f;
            return "*That will increase our survival chances.*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*My buddiship is reserved for someone else, so... You'll take second place.*";
                case MessageIDs.RescueMessage:
                    return "*You're in a safe place now, let me take care of those wounds.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "*...Yawn... I really need a rest, can you be quick so I can return to sleep?*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "*Yaaaaawn... Did you do what I asked for? Or want to talk about something else?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I wouldn't feel comfortable in such a big group.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Not now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I don't think this is the best place to be left, can't we find a better place for me to leave?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I'll return to my investigations, then.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I hope I make It safelly to home.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Good, let's look for a town or some safe place for me to leave.*";
                case MessageIDs.RequestAccepted:
                    return "*Perfect. Return to me when you manage to finish It.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*Even I don't get myself overloaded with things to do.*";
                case MessageIDs.RequestRejected:
                    return "*I don't think I will manage to have the time to do It, but I'll try.*";
                case MessageIDs.RequestPostpone:
                    return "*I may end up doing It myself that way...*";
                case MessageIDs.RequestFailed:
                    return "*I expected too much from you.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Have you don't what I asked you to do?*";
                case MessageIDs.RequestRemindObjective:
                    return "*You don't really have good memory, right? Just do this to me, [objective].*";
                case MessageIDs.RestAskForHowLong:
                    return "*I need to process some thoughts, so that may be perfect. How long will we rest?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't the ideal moment to rest.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*I hope you don't have fleas.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*[nickname], I want to check [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Wait a moment...*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you. Now let's proceed.*";
                case MessageIDs.GenericNevermind:
                    return "*Changed your mind? Alright then.*";
                case MessageIDs.ChatAboutSomething:
                    return "*Do you want to know something/*";
                case MessageIDs.NevermindTheChatting:
                    return "*Had enough questions?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*Is this request too complicated for you? I may relieve you from It if you want.*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Hm... Okay, you may get It out of your head now.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Good.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Ist nicht allein, [nickname].*";
                case MessageIDs.LeopoldMessage1:
                    return "*Save us from what? What is he talking about? Why is he openly saying all that?*";
                case MessageIDs.LeopoldMessage2:
                    return "*Shh... It will be hard to rescue you if the Terrarian realizes that we are talking.*";
                case MessageIDs.LeopoldMessage3:
                    return "*The Terrarian is already listening to us talking, and we're not their hostages. You're worrying way too much.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*You all did good. Thanks.*";
                    return "*I apreciate the help, I really mean It.*";
                case MessageIDs.RevivedByRecovery:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*You could have helped me. I'm fine now, by the way.*";
                    return "*That was because of my sleuthing, right?*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*My blood... So painful...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Put it out! Put it out!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I can barelly see!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*My head is spinning...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't... Do anything!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My feet wont move faster.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I don't feel 100%...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Argh! My chest!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What is that grothesque thing?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hey! That's not funny!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Brrr... Can you place a bonfire?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I'm stuck here!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*You want to test my teeth?!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Try hitting me!*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*I was needing this.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*This will be efficient against our foes.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Take my dust!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "(Deep breath) *Ahhh....*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Now I will cause some damage.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*This will be of help.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Burp.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I was needing something sweet.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Life Crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Watch your step.*";
                case MessageIDs.FoundMineTile:
                    return "*A mine, don't get too close to It.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Be careful, we don't know where is It connected to.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I have a bad feeling about that.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Do you ever wonder why we have to defend those crystals?*";
                case MessageIDs.FoundTreasureTile:
                    return "*Loot.*";
                case MessageIDs.FoundGemTile:
                    return "*Gems!*";
                case MessageIDs.FoundRareOreTile:
                    return "*Rare ores close by.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Very rare ores around us.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Want to take a ride?*";
                    //
                case MessageIDs.TeleportHomeMessage:
                    return "*I agree, I'm sick of this place.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Handy.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I don't think It's wise to be hugged by strangers.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Great... Our leader fell...*";
                case MessageIDs.LeaderDiesMessage:
                    return "*[nickname]! Sigh...*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone needs help here!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*That looks unusual.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll unload all this trash for something more expendable.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You don't look good, [nickname].*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*It's... Hard to breath...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have a few potions on me.*";
                case MessageIDs.UsesLastPotion:
                    return "*My potions are all gone!*";
                case MessageIDs.SpottedABoss:
                    return "*Watch out! Trouble is coming.*";
                case MessageIDs.DefeatedABoss:
                    return "*We managed to get out whole, at least.*";
                case MessageIDs.InvasionBegins:
                    return "*They don't look friendly.*";
                case MessageIDs.RepelledInvasion:
                    return "*Peaceful again.*";
                case MessageIDs.EventBegins:
                    return "*I have a bad feeling..*";
                case MessageIDs.EventEnds:
                    return "*I'm so glad it's over.*";
                case MessageIDs.RescueComingMessage:
                    return "*Hang on, I'm coming.*";
                case MessageIDs.RescueGotMessage:
                    return "*Got you.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*[player] has been checking on me recently. So glad they didn't forget about me.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I heard that [player] managed to kill [subject]. I wish I had that courage.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*[player] seems to have found something valuable, recently. A [subject], they say.*";
                case MessageIDs.FeatEventFinished:
                    return "*There was that day where a [subject] happened, and [player] managed to take care of it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*From what I know, [player] has met [subject] recently. I should try meeting that person some time.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I'm not really great right now. I met a terrarian named [player], who has died recently. I'm trying to recover from that.*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] has recently opened the door to some temple at [subject]. I wonder what kind of thing they found there.*";
                case MessageIDs.FeatCoinPortal:
                    return "*You would be impressed if you saw the same gold portal [player] has found. They got richier after it showed up.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I have met [player] recently. They're not the one I'm looking for, but may help me find who I seek.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*It seems like [player] has been really into helping the angler kid.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*I heard that [player] has defeated Moon Lord in [subject], and saved their world, but I think they actually caused the danger to surge.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*Looks like [player] caused a chaotic event to happen in [subject]. Now stronger creatures roams around it.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*It seems like [player] picked [subject] as their buddy. I hope their buddiship go well.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I hope you don't mind sharing the spot of buddy, since I have someone else who's also my buddy.*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*I heard that [subject] also has a house in [world].*";
                case MessageIDs.DeliveryGiveItem:
                    return "*I can share [target] some of my spare [item].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*Funny, I thought I had the item I wanted to give... Nevermind.*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*[target] need to clean up their inventory before I can give them something.*";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "*I shall lead a group for you then.*";
                case MessageIDs.CommandingLeadGroupFail:
                    return "*I don't feel like wanting to do that for you.*";
                case MessageIDs.CommandingDisbandGroup:
                    return "*Group, disbanded.*";
                case MessageIDs.CommandingChangeOrder:
                    return "*I shall do so.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public class AlexanderData : GuardianData
        {
            public AlexanderData(int Id, string ModId) : base(Id, ModId)
            {

            }
            private const int AlexanderSaveID = 0;
            public List<GuardianID> IdentifiedGuardians = new List<GuardianID>();

            public bool WasGuardianIdentified(TerraGuardian tg)
            {
                foreach (GuardianID id in IdentifiedGuardians)
                    if (id.IsSameID(tg))
                        return true;
                return false;
            }

            public void AddIdentifiedGuardian(GuardianID id)
            {
                if(!(id.ID == Alexander && id.ModID == MainMod.mod.Name) && !IdentifiedGuardians.Any(x => x.ID == id.ID && x.ModID == id.ModID))
                {
                    IdentifiedGuardians.Add(id);
                }
            }

            public override void SaveCustom(TagCompound tag, int UniqueID)
            {
                tag.Add(UniqueID + "tg_saveID", AlexanderSaveID);
                tag.Add(UniqueID + "tg_SleuthCount", IdentifiedGuardians.Count);
                for(int i = 0; i < IdentifiedGuardians.Count; i++)
                {
                    GuardianID gid = IdentifiedGuardians[i];
                    tag.Add(UniqueID + "tg_SleuthID_" + i, gid.ID);
                    tag.Add(UniqueID + "tg_SleuthModID_" + i, gid.ModID);
                }
            }

            public override void LoadCustom(TagCompound tag, int ModVersion, int UniqueID)
            {
                int SaveID = tag.GetInt(UniqueID + "tg_saveID");
                int Count = tag.GetInt(UniqueID + "tg_SleuthCount");
                IdentifiedGuardians.Clear();
                for (int i = 0; i < Count; i++)
                {
                    int ID = tag.GetInt(UniqueID + "tg_SleuthID_" + i);
                    string ModID = tag.GetString(UniqueID + "tg_SleuthModID_" + i);
                    IdentifiedGuardians.Add(new GuardianID(ID, ModID));
                }
            }
        }
    }
}
