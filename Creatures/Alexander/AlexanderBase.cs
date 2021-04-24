using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace giantsummon.Creatures
{
    public class AlexanderBase : GuardianBase
    {
        private const int SleuthStaringAnimationID = 27, SleuthAnimationID = 28, SleuthAnimationID2 = 29, SleuthBackAnimationID = 30;

        public AlexanderBase()
        {
            Name = "Alexander"; //I really want to name him Alexander. The problem is Alex.
            Description = "";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 86;
            DuckingHeight = 62;
            SpriteWidth = 96;
            SpriteHeight = 96;
            FramesInRows = 20;
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
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(23, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(15, 29, 15 + 2);
            HeadVanityPosition.AddFramePoint2x(16, 36, 25 + 2);

            HeadVanityPosition.AddFramePoint2x(17, 21, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 21, 8 + 2);

            HeadVanityPosition.AddFramePoint2x(22, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(23, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(24, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(25, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(26, 31, 21 + 2);
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
                        g.DodgeRate += 0.03f;
                        break;
                    case Minerva:
                        g.Defense += 2;
                        break;
                    case Daphne:
                        g.CoverRate += 0.02f;
                        break;
                }
            }
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsUsingBed)
            {
                Mes.Add("(You noticed him sniffing a while, and then he slept with a smile on the face.\nI think he knows I'm near him.)");
                Mes.Add("*Snif.. Snif... Known one... "+player.name+"... Zzzz...*");
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
            Mes.Add("*I can't get myself distracted from my investigation, so I need your help to do something for me.*");
            Mes.Add("*There is something else catching my attention, but It would be a great time saving if you did It for me.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's good to know that I can count on you. I think this will suit the time you spent.*");
            Mes.Add("*You completed what I asked for, then? Good. This is a compensation for the time you spent.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.GuardianDowned:
                    {
                        TerraGuardian tg = MainMod.ActiveGuardians[Value];
                        AlexanderData data = (AlexanderData)guardian.Data;
                        if (!guardian.DoAction.InUse && tg.Base.IsTerraGuardian && !data.WasGuardianIdentified(tg))
                        {
                            if (guardian.StartNewGuardianAction(new SleuthAction(tg), 0))
                                return true;
                        }
                    }
                    break;
                case TriggerTypes.GuardianSpotted:
                    {
                        TerraGuardian tg = MainMod.ActiveGuardians[Value];
                        AlexanderData data = (AlexanderData)guardian.Data;
                        if (!guardian.DoAction.InUse && tg.Base.IsTerraGuardian && tg.IsUsingBed && !data.WasGuardianIdentified(tg))
                        {
                            if (guardian.StartNewGuardianAction(new SleuthAction(tg), 0))
                                return true;
                        }
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
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

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
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

        public class SleuthAction : GuardianActions
        {
            public float SleuthPercent = 0;
            public bool Sleuthing = false;
            public SleuthAction(TerraGuardian target)
            {
                Guardians.Add(target);
                IsGuardianSpecificAction = true;
            }

            public override void Update(TerraGuardian guardian)
            {
                TerraGuardian Target = Guardians[0];
                if ((guardian.Position - Target.Position).Length() < 20)
                    guardian.WalkMode = true;
                if (guardian.UsingFurniture)
                    guardian.LeaveFurniture(true);
                guardian.StuckTimer = 0;
                if (guardian.BeingPulledByPlayer)
                {
                    guardian.SaySomething("*Alright, I'm coming, I'm coming.*");
                    InUse = false;
                    return;
                }
                guardian.MoveLeft = guardian.MoveRight = false;
                Sleuthing = false;
                if (!Target.KnockedOut && !Target.IsUsingBed)
                {
                    if (SleuthPercent > 70)
                        guardian.SaySomething("*...So close...*");
                    else
                        guardian.SaySomething(Target.GetMessage(MessageIDs.AlexanderSleuthingFail, "*I... Was just checking if you were fine.*"));
                    InUse = false;
                    return;
                }
                if (Target.Downed)
                {
                    InUse = false;
                    guardian.SaySomething("*...I should have helped instead...*");
                    return;
                }
                if (Math.Abs(guardian.Position.X - Target.Position.X) < 8f)
                {
                    if (guardian.Velocity.X == 0 && guardian.Velocity.Y == 0)
                    {
                        Sleuthing = true;
                        guardian.LookingLeft = (Target.Position.X < guardian.Position.X);
                        float LastSleuthPercent = SleuthPercent;
                        float FillSpeed = guardian.IsUsingBed ? 0.05f : 0.2f;
                        SleuthPercent += Main.rand.NextFloat() * FillSpeed;
                        if (SleuthPercent >= 100)
                        {
                            AlexanderData data = (AlexanderData)guardian.Data;
                            data.AddIdentifiedGuardian(Target.MyID);
                            InUse = false;
                            guardian.SaySomething(Target.GetMessage(MessageIDs.AlexanderSleuthingProgressFinished, "*Okay, so that's how you work.*"));
                            guardian.UpdateStatus = true;
                        }
                        else if (SleuthPercent >= 70 && LastSleuthPercent < 70)
                        {
                            guardian.SaySomething(Target.GetMessage(MessageIDs.AlexanderSleuthingProgressNearlyDone, "*Hm... Interesting...*"));
                        }
                        else if (SleuthPercent >= 35 && LastSleuthPercent < 35)
                        {
                            guardian.SaySomething(Target.GetMessage(MessageIDs.AlexanderSleuthingProgress, "*Uh huh...*"));
                        }
                        else if (SleuthPercent > 0 && LastSleuthPercent <= 0)
                        {
                            guardian.SaySomething(Target.GetMessage(MessageIDs.AlexanderSleuthingStart, "*Let's see how you work...*"));
                        }
                    }
                }
                else
                {
                    if (Target.Position.X < guardian.Position.X)
                        guardian.MoveLeft = true;
                    else
                        guardian.MoveRight = true;
                }
            }

            public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
            {
                if (Sleuthing)
                {
                    guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame = SleuthBackAnimationID;
                }
            }
        }
    }
}
