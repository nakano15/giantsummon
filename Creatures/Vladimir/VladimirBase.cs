using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class VladimirBase : GuardianBase
    {
        /// <summary>
        /// -Very friendly when not in a bloodmoon.
        /// -Turns extremelly aggressive during bloodmoons.
        /// -Loves giving hugs.
        /// -Listens to confessions of people he hugs.
        /// -Had a brother that is younger than him, who he gave hugs to.
        /// -Came from a family of warriors. Choose to be friendly, instead.
        /// </summary>

        public VladimirBase()
        {
            Name = "Vladimir";
            Description = "A bear that likes giving hugs to people.";
            Size = GuardianSize.Large;
            Width = 44;
            Height = 116;
            DuckingHeight = 52;
            SpriteWidth = 128;
            SpriteHeight = 160;
            FramesInRows = 15;
            Age = 26;
            Male = true;
            CalculateHealthToGive(1600, 0.85f, 0.7f); //Lc: 95, LF: 16
            //InitialMHP = 200; //1000
            //LifeCrystalHPBonus = 40;
            //LifeFruitHPBonus = 10;
            Accuracy = 0.72f;
            Mass = 0.7f;
            MaxSpeed = 4.9f;
            Acceleration = 0.14f;
            SlowDown = 0.42f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            OneHanded2HWeaponWield = true;
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);
            CallUnlockLevel = 0;
            MountUnlockLevel = 3;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 10;
            PlayerMountedArmAnimation = 1;
            HeavySwingFrames = new int[] { 13, 14, 19 };
            ItemUseFrames = new int[] { 13, 14, 15, 16 };
            DuckingFrame = 11;
            DuckingSwingFrames = new int[] { 17, 18, 19 };
            SittingFrame = 20;
            ChairSittingFrame = 21;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 22;
            BedSleepingFrame = 24;
            DownedFrame = 26;
            ReviveFrame = 27;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(20, 0);
            BodyFrontFrameSwap.Add(21, 1);
            BodyFrontFrameSwap.Add(23, 2);

            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(12, 1);
            RightArmFrontFrameSwap.Add(21, 2);

            SittingPoint = new Microsoft.Xna.Framework.Point((27 + 3) * 2, 62 * 2);

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(39, 46);
            MountShoulderPoints.AddFramePoint2x(11, 39, 46 + 6);
            MountShoulderPoints.AddFramePoint2x(12, 39, 46 + 6);

            MountShoulderPoints.AddFramePoint2x(23, 32, 58);
            MountShoulderPoints.AddFramePoint2x(25, 23, 70);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(13, 21, 14);
            LeftHandPoints.AddFramePoint2x(14, 45, 26);
            LeftHandPoints.AddFramePoint2x(15, 52, 40);
            LeftHandPoints.AddFramePoint2x(16, 44, 56);

            LeftHandPoints.AddFramePoint2x(17, 21, 14 + 6);
            LeftHandPoints.AddFramePoint2x(18, 45, 26 + 6);
            LeftHandPoints.AddFramePoint2x(19, 44, 56 + 6);

            LeftHandPoints.AddFramePoint2x(23, 32, 58);
            LeftHandPoints.AddFramePoint2x(25, 23, 72);

            LeftHandPoints.AddFramePoint2x(27, 44, 71);

            //Right Arm
            RightHandPoints.AddFramePoint2x(13, 35, 14);
            RightHandPoints.AddFramePoint2x(14, 48, 26);
            RightHandPoints.AddFramePoint2x(15, 55, 40);
            RightHandPoints.AddFramePoint2x(16, 48, 56);

            RightHandPoints.AddFramePoint2x(17, 35, 14 + 6);
            RightHandPoints.AddFramePoint2x(18, 48, 26 + 6);
            RightHandPoints.AddFramePoint2x(19, 48, 56 + 6);

            RightHandPoints.AddFramePoint2x(23, 32, 58);
            RightHandPoints.AddFramePoint2x(25, 40, 72);

            RightHandPoints.AddFramePoint2x(27, 51, 71);

            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(30, 28);
            HeadVanityPosition.AddFramePoint2x(11, 30, 28 + 6);
            HeadVanityPosition.AddFramePoint2x(12, 30, 28 + 6);
            HeadVanityPosition.AddFramePoint2x(17, 30, 28 + 6);
            HeadVanityPosition.AddFramePoint2x(18, 30, 28 + 6);
            HeadVanityPosition.AddFramePoint2x(19, 30, 28 + 6);

            HeadVanityPosition.AddFramePoint2x(23, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(25, -1000, -1000);

            HeadVanityPosition.AddFramePoint2x(27, 50, 47);

            //Wing Position
            WingPosition.AddFramePoint(23, -1000, -1000);
            WingPosition.AddFramePoint(25, -1000, -1000);
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*Hey buddy, I don't mind hugging you all the way during your travels. At least If I do, you wont get tired.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*Hey again! I have some of my family's resistence in me, so If you need me to do something extremelly dangerous, I can do it.*";
            }
        }
        
        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian Guardian)
        {
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> Options = new List<GuardianMouseOverAndDialogueInterface.DialogueOption>();
            GuardianMouseOverAndDialogueInterface.DialogueOption Option = new GuardianMouseOverAndDialogueInterface.DialogueOption((Guardian.DoAction.InUse && Guardian.DoAction.ID == 0 && Guardian.DoAction.IsGuardianSpecificAction ? "Enough" : "Be Hugged"), HugOptionAction);
            Options.Add(Option);
            return Options;
        }

        public void HugOptionAction(TerraGuardian Guardian)
        {
            if (Guardian.DoAction.InUse && Guardian.DoAction.ID == 0 && Guardian.DoAction.IsGuardianSpecificAction)
            {
                Guardian.DoAction.InUse = false;

                Main.npcChatText = (((Creatures.VladimirBase)Guardian.Base).GetEndHugMessage(Guardian));
                Guardian.DoAction.Players[0].Bottom = Guardian.Position;
            }
            else if (!Guardian.DoAction.InUse)
            {
                PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                if (player.MountedOnGuardian || player.GuardianMountingOnPlayer)
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("*Get off your guardian first.*");
                }
                else
                {
                    GuardianActions ga = Guardian.StartNewGuardianAction(0);
                    if (ga != null)
                    {
                        ga.Players.Add(Main.player[Main.myPlayer]);
                        GuardianMouseOverAndDialogueInterface.SetDialogue("*Press Jump button If you want me to stop.*");
                    }
                }
            }
            else
            {
                GuardianMouseOverAndDialogueInterface.SetDialogue("*I can't right now.*");
            }
            GuardianMouseOverAndDialogueInterface.GetDefaultOptions(Guardian);
        }

        public override void ForceDrawInFrontOfPlayer(TerraGuardian guardian, ref bool LeftArmInFront, ref bool RightArmInFront)
        {
            if (guardian.LeftArmAnimationFrame == 1 || guardian.LeftArmAnimationFrame == 12)
                LeftArmInFront = true;
            if (guardian.RightArmAnimationFrame == 1 || guardian.RightArmAnimationFrame == 12)
            {
                RightArmInFront = true;
                LeftArmInFront = true;
            }
            if (guardian.RightArmAnimationFrame == 16 && guardian.PlayerMounted && (guardian.SelectedItem == -1 || Items.GuardianItemPrefab.IsHeavyItem(guardian.Inventory[guardian.SelectedItem])))
                LeftArmInFront = true;
            if (guardian.BodyAnimationFrame == 25)
                LeftArmInFront = true;
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            bool UsingHeavyWeapon = guardian.ItemAnimationTime > 0 && (guardian.SelectedItem == -1 || Items.GuardianItemPrefab.IsHeavyItem(guardian.Inventory[guardian.SelectedItem]));
            if (UsingHeavyWeapon)
            {
                if (AnimationID == 0 && Frame != HeavySwingFrames[2])
                {
                    if (guardian.Velocity.Y != 0)
                    {
                        Frame = JumpFrame;
                    }
                }
                if (guardian.PlayerMounted)
                {
                    if (AnimationID == 2)
                    {
                        if (Frame >= 13 && Frame <= 16)
                        {
                            Frame = 1;
                        }
                        else if (Frame >= 17 && Frame <= 19)
                        {
                            Frame = 12;
                        }
                    }
                }
            }
        }

        public override void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if (guardian.PlayerMounted)
            {
                int Frame = 1;
                if (guardian.BodyAnimationFrame == ThroneSittingFrame)
                    Frame = 23;
                else if (guardian.BodyAnimationFrame == BedSleepingFrame)
                    Frame = 25;
                else if (guardian.Ducking)
                    Frame = 12;
                if (guardian.BodyAnimationFrame == StandingFrame || guardian.BodyAnimationFrame == DuckingFrame)
                    guardian.BodyAnimationFrame = Frame;
                if (guardian.BodyAnimationFrame == ThroneSittingFrame)
                    guardian.BodyAnimationFrame = ThroneSittingFrame + 1;
                if (guardian.BodyAnimationFrame == BedSleepingFrame)
                    guardian.BodyAnimationFrame = BedSleepingFrame + 1;
                if (!UsingLeftArm)
                {
                    guardian.LeftArmAnimationFrame = Frame;
                    UsingLeftArm = true;
                }
                if (!UsingRightArm)
                {
                    guardian.RightArmAnimationFrame = Frame;
                    UsingRightArm = true;
                }
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (guardian.LookingLeft && (guardian.BodyAnimationFrame == ThroneSittingFrame || guardian.BodyAnimationFrame == BedSleepingFrame))
                guardian.FaceDirection(false);
            if (guardian.OwnerPos > -1)
            {
                if (guardian.SittingOnPlayerMount)
                {
                    Main.player[guardian.OwnerPos].AddBuff(ModContent.BuffType<Buffs.Obstruction>(), 5);
                }
                else if (guardian.PlayerMounted)
                {
                    Main.player[guardian.OwnerPos].AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                    guardian.AddBuff(ModContent.BuffType<Buffs.Protecting>(), 5);
                }
            }
        }

        public override void GuardianActionUpdate(TerraGuardian guardian, GuardianActions action)
        {
            switch (action.ID)
            {
                case 0:
                    {
                        action.ProceedIdleAIDuringDialogue = true;
                        action.NpcCanFacePlayer = false;
                        Player player = action.Players[0];
                        guardian.AddDrawMomentToPlayer(player);
                        const int BuffRefreshTime = 10 * 60;
                        if (action.Time >= BuffRefreshTime && action.Time % BuffRefreshTime == 0)
                        {
                            int Stack = action.Time / BuffRefreshTime;
                            if (Stack > 3)
                                Stack = 3;
                            player.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * Stack);
                            PlayerMod pm = player.GetModPlayer<PlayerMod>();
                            if (pm.ControllingGuardian)
                            {
                                pm.Guardian.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * Stack);
                            }
                        }
                        bool End = player.controlJump;
                        if (PlayerMod.PlayerHasGuardianSummoned(player, guardian.ID, guardian.ModID))
                            End = true;
                        if (Main.bloodMoon)
                            End = false;
                        if (End)
                        {
                            action.InUse = false;
                            player.Bottom = guardian.Position;
                            PlayerMod pm = player.GetModPlayer<PlayerMod>();
                            if (pm.ControllingGuardian)
                            {
                                pm.Guardian.Position = guardian.Position;
                            }
                            guardian.SaySomething(GetEndHugMessage(guardian));
                        }
                        else
                        {
                            if (player.mount.Active)
                                player.mount.Dismount(player);
                            PlayerMod pm = player.GetModPlayer<PlayerMod>();
                            bool FaceBear = (guardian.BodyAnimationFrame != 20 && guardian.BodyAnimationFrame != 21) || guardian.BodyAnimationFrame == 25;
                            if (pm.ControllingGuardian)
                            {
                                pm.Guardian.Position = guardian.GetGuardianShoulderPosition;
                                pm.Guardian.Position.Y += guardian.Height * 0.5f;
                                pm.Guardian.Velocity.Y = -pm.Guardian.Mass;
                                pm.Guardian.FallStart = (int)pm.Guardian.Position.Y / 16;
                                if (pm.Guardian.ItemAnimationTime == 0 && !pm.Guardian.MoveLeft && !pm.Guardian.MoveRight)
                                    pm.Guardian.FaceDirection((guardian.Direction * (FaceBear ? -1 : 1)) == -1);
                                pm.Guardian.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                                if (pm.Guardian.KnockedOut)
                                {
                                    pm.Guardian.ReviveBoost += 3;
                                }
                                if (!Main.bloodMoon)
                                {
                                    pm.Guardian.ImmuneTime = 3;
                                    pm.Guardian.ImmuneNoBlink = true;
                                }
                            }
                            else
                            {
                                player.gfxOffY = 0;
                                player.Center = guardian.GetGuardianShoulderPosition;
                                player.velocity.Y = -Player.defaultGravity;
                                player.fallStart = (int)player.Center.Y / 16;
                                if (player.itemAnimation == 0 && !player.controlLeft && !player.controlRight)
                                    player.ChangeDir(guardian.Direction * (FaceBear ? -1 : 1));
                                player.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                                if (pm.KnockedOut)
                                    pm.ReviveBoost += 3;
                                if (!Main.bloodMoon)
                                {
                                    player.immuneTime = 3;
                                    player.immuneNoBlink = true;
                                }
                            }
                            if ((guardian.MessageTime == 0 || Main.bloodMoon) && (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || (Main.bloodMoon && player.controlJump)))
                            {
                                if (Main.bloodMoon)
                                {
                                    player.controlJump = false;
                                    bool Defeated = false, Hurt = false;
                                    if (pm.ControllingGuardian)
                                    {
                                        pm.Guardian.FriendlyDuelDefeat = true;
                                        Hurt = 0 != pm.Guardian.Hurt((int)(pm.Guardian.MHP * 0.22f), guardian.Direction, false, true, " were crushed by " + guardian.Name + "'s arms.");
                                        if (pm.Guardian.Downed)
                                            Defeated = true;
                                    }
                                    else
                                    {
                                        player.GetModPlayer<PlayerMod>().FriendlyDuelDefeat = true;
                                        Hurt = 0 != player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " were crushed by " + guardian.Name + "'s arms."), (int)(player.statLifeMax2 * 0.22f), guardian.Direction);
                                        if (player.dead)
                                            Defeated = true;
                                    }
                                    if (Hurt)
                                    {
                                        if (!Defeated)
                                        {
                                            if (guardian.BodyAnimationFrame == guardian.Base.ChairSittingFrame)
                                            {
                                                switch (Main.rand.Next(5))
                                                {
                                                    case 0:
                                                        guardian.SaySomething("*I'll crush you if you move again!*");
                                                        break;
                                                    case 1:
                                                        guardian.SaySomething("*I'll hurt you worser than those monsters would If you keep moving!*");
                                                        break;
                                                    case 2:
                                                        guardian.SaySomething("*I can crush you with my arms or my legs, you pick!*");
                                                        break;
                                                    case 3:
                                                        guardian.SaySomething("*Want me to turn your bones to dust?*");
                                                        break;
                                                    case 4:
                                                        guardian.SaySomething("*You are angering me, more than this night does!*");
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                switch (Main.rand.Next(5))
                                                {
                                                    case 0:
                                                        guardian.SaySomething("*Stay quiet!*");
                                                        break;
                                                    case 1:
                                                        guardian.SaySomething("*I'll crush your bones If you continue doing that!*");
                                                        break;
                                                    case 2:
                                                        guardian.SaySomething("*I have my arms around you, I can pull them against my body, and you wont like it!*");
                                                        break;
                                                    case 3:
                                                        guardian.SaySomething("*Want to try that again?*");
                                                        break;
                                                    case 4:
                                                        guardian.SaySomething("*This is what you want?*");
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            switch (Main.rand.Next(5))
                                            {
                                                case 0:
                                                    guardian.SaySomething("*Finally! You got quiet!*");
                                                    break;
                                                case 1:
                                                    guardian.SaySomething("*See what you made me do?!*");
                                                    break;
                                                case 2:
                                                    guardian.SaySomething("*My mood is already bad, you didn't helped either!*");
                                                    break;
                                                case 3:
                                                    guardian.SaySomething("*At least you stopped moving around!*");
                                                    break;
                                                case 4:
                                                    guardian.SaySomething("*You behave better when unconscious!*");
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    guardian.SaySomething("*Press Jump button If that's enough.*");
                                }
                            }
                            if (player.whoAmI == Main.myPlayer)
                            {
                                MainMod.FocusCameraPosition = guardian.CenterPosition;
                                MainMod.FocusCameraPosition.X -= Main.screenWidth * 0.5f;
                                MainMod.FocusCameraPosition.Y -= Main.screenHeight * 0.5f;
                                const byte ChatTimeVar = 0, FriendshipPointsVar = 1;
                                int Timer = action.GetIntegerValue(ChatTimeVar) - 1, FriendshipPoints = action.GetIntegerValue(FriendshipPointsVar);
                                const int InitialTime = 300;
                                if (Timer == -1)
                                {
                                    action.SetIntegerValue(ChatTimeVar, InitialTime);
                                }
                                else if (Timer < 1)
                                {
                                    action.SetIntegerValue(ChatTimeVar, 60 * 10); // + Main.rand.Next(InitialTime)
                                    FriendshipPoints++;
                                    if (FriendshipPoints >= 10 + guardian.FriendshipLevel / 3)
                                    {
                                        FriendshipPoints = 0;
                                        guardian.IncreaseFriendshipProgress(1);
                                    }
                                    action.SetIntegerValue(FriendshipPointsVar, FriendshipPoints);
                                    string Message = GuardianMouseOverAndDialogueInterface.MessageParser(Main.rand.Next(10) == 0 ? guardian.Base.TalkMessage(player, guardian) : guardian.Base.NormalMessage(player, guardian), guardian);
                                    if (player.talkNPC > -1 && Main.npc[player.talkNPC].type == ModContent.NPCType<GuardianNPC.List.BearNPC>())
                                    {
                                        Main.npcChatText = Message;
                                    }
                                    else
                                    {
                                        //guardian.SaySomething(Message);
                                    }
                                }
                                else
                                {
                                    action.SetIntegerValue(ChatTimeVar, Timer);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public override void GuardianActionUpdateAnimation(TerraGuardian guardian, GuardianActions action, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if (guardian.OwnerPos == -1)
            {
                switch (action.ID)
                {
                    case 0:
                        {
                            if (guardian.BodyAnimationFrame == BedSleepingFrame)
                            {
                                if (!Main.bloodMoon)
                                {
                                    guardian.BodyAnimationFrame++;
                                    guardian.LeftArmAnimationFrame = guardian.BodyAnimationFrame;
                                    guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame;
                                }
                            }
                            else if (guardian.BodyAnimationFrame == ThroneSittingFrame)
                            {
                                if (!Main.bloodMoon)
                                {
                                    guardian.BodyAnimationFrame++;
                                }
                                else
                                {
                                    if (!UsingLeftArm)
                                    {
                                        UsingLeftArm = true;
                                        guardian.LeftArmAnimationFrame = guardian.BodyAnimationFrame++;
                                    }
                                    if (!UsingRightArm)
                                    {
                                        UsingRightArm = true;
                                        guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame++;
                                    }
                                }
                            }
                            else if (guardian.BodyAnimationFrame == ChairSittingFrame)
                            {
                                if (!UsingLeftArm)
                                {
                                    guardian.LeftArmAnimationFrame = SittingFrame;
                                    UsingLeftArm = true;
                                }
                                if (!UsingRightArm)
                                {
                                    guardian.RightArmAnimationFrame = SittingFrame;
                                    UsingRightArm = true;
                                }
                            }
                            else
                            {
                                int Frame = 1;
                                if (guardian.Ducking)
                                    Frame = 12;
                                if (!Main.bloodMoon && (guardian.BodyAnimationFrame == StandingFrame || guardian.BodyAnimationFrame == DuckingFrame))
                                    guardian.BodyAnimationFrame = Frame;
                                if (!UsingLeftArm)
                                {
                                    guardian.LeftArmAnimationFrame = Frame;
                                    UsingLeftArm = true;
                                }
                                if (!UsingRightArm)
                                {
                                    guardian.RightArmAnimationFrame = Frame;
                                    UsingRightArm = true;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public string GetEndHugMessage(TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("*GOOD.*");
                Mes.Add("*I don't need a burden now!*");
            }
            else if (guardian.IsUsingToilet)
            {
                Mes.Add("*I hope it wasn't because of the smell.*");
                Mes.Add("*I have to admit, this is quite unpleasant.*");
                Mes.Add("*The noises scared you?*");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Add("*Zzz... Goodbyezzzz....*");
                Mes.Add("(Continues snoring)");
                Mes.Add("*See you... Zzzz*");
                Mes.Add("*... Sheeps... Zzz...*");
            }
            else
            {
                Mes.Add("*Alright.*");
                Mes.Add("*I hope I helped.*");
                Mes.Add("*Feeling better?*");
                Mes.Add("*That is enough? See you then.*");
                Mes.Add("*Anytime.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GreetMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hello there, little buddy. Need a hug? I'll provide you as many as you need.*");
            Mes.Add("*Aren't you a Terrarian? Hello! I'm expert in hugging.*");
            Mes.Add("*Oh, You're a Terrarian. Do you need a new friend?*");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool IsPlayerBeingHugged = guardian.DoAction.InUse && guardian.DoAction.ID == 0 && guardian.DoAction.IsGuardianSpecificAction && guardian.DoAction.Players[0] == player;
            List<string> Mes = new List<string>();
            Mes.Add("*I really don't mind giving hugs to anybody.*");
            Mes.Add("*Why I like giving hugs? I used to give them to my younger brother, It always helped in various situations where he was sad.*");
            Mes.Add("*I feel that right now there are people needing me here.*");
            Mes.Add("*I don't think I can help solving people problems, but I can at least try with hugs.*");
            Mes.Add("*I most likelly get confessions from the people I hug, so I mostly know of things many don't.*");
            Mes.Add("*I can try telling you some of the things I heard from the people I hug, just don't tell anyone.*");
            if (!Main.bloodMoon)
            {
                Mes.Add("*I heard from other citizens that I get really scary during blood moons... I would like to apologize for my behavior.*");
            }
            if (!PlayerMod.HasGuardianSummoned(player, Vladimir))
            {
                Mes.Add("*Don't you want to take \"Teddy\" out for a walk?*");
                Mes.Add("*Let's go on an adventure!*");
                Mes.Add("*Are you going somewhere? I love adventures too!*");
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    if (!Main.raining)
                    {
                        Mes.Add("*(Takes a deep breath) Ah... I love this weather. It's perfect for an adventure.*");
                        Mes.Add("*I really enjoy days like this, I feel like doing anything.*");
                    }
                    else
                    {
                        Mes.Add("*Well, I didn't really felt like doing anything outside, anyway.*");
                    }
                }
                else
                {
                    Mes.Add("*Don't worry, nothing will harm you while in my arms.*");
                    Mes.Add("*Hey! I think I have watched the movie that monster is from! What monster? That one, don't you see?*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*The night smiles upon us.*");
                    Mes.Add("*Yawn~ I'm starting to get sleepy.*");
                    Mes.Add("*This night remembers me of my brother, I used to help him sleep when he was scared of monsters under his bed.*");
                }
                else
                {
                    Mes.Add("*Could you please MAKE THOSE MONSTERS BE SILENT!!! I'M TRYING TO SLEEP!!*");
                    Mes.Add("*You! Go make them stop! My head is even aching, because I CAN'T GET EVEN A LITTLE BIT OF SLEEP!*");
                    Mes.Add("*SHUT UP OUT THERE!!! I WANT TO SLEEP!*");
                }
            }
            bool HasBlue = NpcMod.HasGuardianNPC(Blue), HasZacks = NpcMod.HasGuardianNPC(Zacks);
            if (HasBlue && HasZacks)
            {
                if(!IsPlayerBeingHugged)
                    Mes.Add("*I discovered [gn:" + Blue + "]'s secret. [gn:" + Zacks + "] said that she really likes bunnies, I'm very sure she will like it if you give one to her to hold.*");
                Mes.Add("*I can't heal [gn:"+Blue+"] and [gn:"+Zacks+"] pain with hugs... It seems like they need to heal It by themselves.*");
            }
            else if (HasBlue && !HasZacks)
            {
                Mes.Add("*I saw [gn:" + Blue + "] crying earlier this day, I gave her a hug, but she didn't stopped crying. I wonder what makes her sad.*");
                Mes.Add("*It pains me to see [gn:" + Blue + "] sad. Can we do something for her?*");
            }
            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*It looks like [gn:" + Rococo + "] thinks like me in some ways.*");
                Mes.Add("*Sometimes It feels like what makes the guardians be attracted to this place, is [gn:" + Rococo + "].*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                if (NPC.downedBoss3) Mes.Add("*I will never go drink with [gn:" + Brutus + "] again. I don't even remember how I ended up inside the Dungeon.*");
                Mes.Add("*Whenever I try giving a hug to [gn:" + Brutus + "], he tells me to back off, because he's male. What that has to do with hugging?!*");
                Mes.Add("*I wonder if [gn:"+Brutus+"] has any kind of pain.*");
            }
            if (NpcMod.HasGuardianNPC(Mabel) && !IsPlayerBeingHugged)
            {
                Mes.Add("*I... I didn't wanted to mention this, but hugging [gn:" + Mabel + "] was an error. I need to find the toilet ASAP.*");
            }
            if (NpcMod.HasGuardianNPC(Alex))
            {
                Mes.Add("*I sometimes make company to [gn:" + Alex + "], when he goes to visit " + AlexRecruitScripts.AlexOldPartner + "'s tombstone. It's really tragic what happened to her.*");
                Mes.Add("*Everyone has loss, but I don't know if [gn:" + Alex + "] is getting over his.*");
            }
            if (NpcMod.HasGuardianNPC(Domino))
            {
                Mes.Add("*You wont believe this, but after several attempts, I managed to be able to hug [gn:" + Domino + "]! But I wonder, why did he needed a hug?*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("*For a sage, [gn:" + Leopold + "] has a hundred one troubles.*");
                Mes.Add("*Generally when [gn:" + Leopold + "] comes, he keeps debating himself his theories. When I wake up, he's already gone. I've never seen him angry at me for doing that.*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*I like hugging [gn:" + Michelle + "], she's one of the few persons that wants a hug for no actual reason.*");
                Mes.Add("*Sometimes I sing a lullaby at night for [gn:"+Michelle+"].*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*I'm glad that [gn:"+Malisha+"] noticed that I'm not much into chatting while hugging.*");
                Mes.Add("*How did [gn:"+Malisha+"] knew of my family? I hope she doesn't tell them that I'm here. It could bring trouble to this realm.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*It's a bit hard to help [gn:" + Fluffles + "], because I can't touch her, so I kind of pretend to be hugging her. It seems to be working.*");
                Mes.Add("*I question myself why [gn:" + Fluffles + "] can't talk. I don't mind the silence when hugging, but It bothers me that she can't speak at all. Can you help her solve that problem?*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*Since the moment we met, [gn:"+Minerva+"] guessed right the kind of food I like. I always love eating her food.*");
                Mes.Add("*Whenever I'm eating the food [gn:"+Minerva+"] makes, stares at me with a smile on her face. I think she's glad that I'm liking It.*");
                Mes.Add("*Sometimes [gn:"+Minerva+"] asks me to hug her, and she eventually falls asleep when being hugged, and that's all fine. The problem is that she has a little flatulence problem. Sometimes I accidentally wake her up because I'm trying to blow the smell away.*");
            }
            bool HasSardine = NpcMod.HasGuardianNPC(Sardine), HasBree = NpcMod.HasGuardianNPC(Bree);
            if (HasSardine)
            {
                Mes.Add("*[gn:" + Sardine + "] told me that the reason why he left home was to look for good treasures, so he could make his family live well. He didn't expected to forget which world he lives.*");
                if (!HasBree)
                {
                    Mes.Add("*[gn:" + Sardine + "] is very worried about his wife, he wonders how she's doing.*");
                }
                else
                {
                    Mes.Add("*[gn:" + Sardine + "] wonders why [gn:"+Bree+"] keeps trying to control his steps.*");
                }
            }
            if (HasBree)
            {
                Mes.Add("*[gn:"+Bree+"] is worried about her son. It stayed at home when she went to look for "+(HasSardine ? "[gn:" + Sardine+"]" : "her husband")+"*");
                if (!HasSardine)
                {
                    Mes.Add("*[gn:" + Bree + "] keeps being worried about her husband, she sometimes even think he may be dead, but I always tell her not to be foolish to think of that.*");
                }
                else
                {
                    Mes.Add("*[gn:"+Bree+"] feels lonely sometimes. She thinks [gn:"+Sardine+"] cares more about adventure than her.*");
                }
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("*You wont believe me, but [nn:" + Terraria.ID.NPCID.Merchant + "] is really sad because nobody wants to buy his angel statues.*");
                Mes.Add("*About how much dirt sells overseas... [nn:" + Terraria.ID.NPCID.Merchant + "] was lying.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
            {
                Mes.Add("*I like hugging [nn:" + Terraria.ID.NPCID.Mechanic + "], she seems to be the only person who gets a hug because want one.*");
                Mes.Add("*People like [nn:" + Terraria.ID.NPCID.Mechanic + "] brighten my day.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] sometimes comes asking me for a hug, but he always fall asleep after that.*");
                Mes.Add("*Sometimes I refuse hugging [nn:" + Terraria.ID.NPCID.Angler + "], because my mouth starts filling with salivae when he gets close, so I tell him that he must take a bath before I can hug.*");
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] sometimes comes to me, complain about how [gn:" + Mabel + "] keeps telling him to eat variated food, taking bath, or go sleep. I say that she is trying.*");
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] told me earlier that likes some things [gn:" + Mabel + "] does, like taking him on a trip, telling him bedtime stories, and singing.*");
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] said that whenever he's with [gn:" + Mabel + "], he feels safe. He also said that didn't felt like that since... He stopped talking here, but could he be talking about...*");
                }
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse) && NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
            {
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Nurse + "] sometimes comes to me to talk about [nn:" + Terraria.ID.NPCID.ArmsDealer + "], but I don't know what to answer, that's not my thing! That's when I directed her to [gn:"+Blue+"].*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TaxCollector))
            {
                Mes.Add("*I discovered earlier that hugging [nn:" + Terraria.ID.NPCID.Mechanic + "] is a good way of avoiding tax collection.*");
                Mes.Add("*I actually dislike hugging [nn:" + Terraria.ID.NPCID.Mechanic + "], he smells like an abandoned house. I nearly sneezed last time.*");
            }

            if (guardian.IsUsingToilet)
            {
                Mes.Add("*We have to talk. We need a bigger toilet, I don't think this one will handle what is coming.*");
                Mes.Add("*I didn't begun yet, I'm trying to aim at the hole, but It's too small.*");
            }

            if (IsPlayerBeingHugged)
            {
                if (guardian.IsAttackingSomething)
                {
                    if (!Main.bloodMoon)
                    {
                        Mes.Add("*I WILL SILENCE YOU NOW!!!*");
                        Mes.Add("*DIE VILE CREATURE!!!!*");
                        Mes.Add("*I WILL MAKE YOUR HEAD MY TROPHY!!!!*");
                    }
                    else
                    {
                        Mes.Clear();
                        if (guardian.HP >= guardian.MHP * 0.5f)
                        {
                            Mes.Add("*Don't worry, I'll handle this.*");
                            Mes.Add("*It will be gone in a minute, don't mind.*");
                            Mes.Add("*Ack, It's a monster! I will deal with it.*");
                            Mes.Add("*Don't mind that, I will take care of that.*");
                            Mes.Add("*I will protect you.*");
                        }
                        else
                        {
                            Mes.Add("*Gasp~! I may need some help here.*");
                            Mes.Add("*I didn't wanted to ask this, but this is being quite tough for me.*");
                            Mes.Add("*Uh... A little help, please?*");
                            Mes.Add("*I think you may need to run if this get harder.*");
                        }
                    }
                }
                else if (Main.bloodMoon)
                {
                    Mes.Clear();
                    Mes.Add("*Those monsters should Be More QUIET!! I'M WANT TO GET SOME SLEEP!*");
                    Mes.Add("*WHY ARE YOU GUYS BEING SO NOISY! I WANT TO TAKE A REST!!*");
                    Mes.Add("*IF YOU DON'T GET QUIET, I WILL RIP THE FLESH OUT OF YOUR BODY!*");
                    Mes.Add("*STOP SHIVERING ALREADY!!*");
                }
                else
                {
                    if (player.GetModPlayer<PlayerMod>().ControllingGuardian)
                    {
                        Mes.Add("*I sense someone else's presence in you... Is that you, " + player.name + "?*");
                        Mes.Add("*It is you " + player.name + "? Sorry, but you can't fool me, I can sense that It's you because we've met before. The bond never fails.*");
                    }
                    if (!guardian.HasBuff(Terraria.ID.BuffID.WellFed))
                    {
                        Mes.Add("*Huh? My stomach? Oh, sorry. It's because I'm getting hungry.*");
                    }
                    Mes.Add("*I always feel happiness whenever I hug someone.*");
                    Mes.Add("*Sometimes, I feel a bit depressed after listening to other people stories. That is why I preffer to hug while silent.*");
                    Mes.Add("*It's funny how from everyone in the world, you are the only one who doesn't confess anything.*");
                    Mes.Add("*I wonder if people think if I have problems too... But I don't think I would openly talk about them to someone.*");
                    Mes.Add("*Say... Could you try catching some salmons for me, sometime.*");
                    if (player.ZoneSnow)
                    {
                        Mes.Add("*You wont feel cold like this.*");
                        Mes.Add("*Brr... It's cold here. Good thing that you are being warmed by me.*");
                        if (player.ZoneRain)
                        {
                            Mes.Add("*A-a-are you feeling c-c-old t-t-too?*");
                            Mes.Add("*My f-fur, It's b-barelly being able t-t-to protect m-me from c-cold.*");
                        }
                    }
                    if (player.ZoneDesert)
                    {
                        Mes.Add("*It's too hot here. Do you have a bottle of water with you?*");
                        Mes.Add("*I can't stop sweating... You're dripping. Better drink some water, or you will pass out.*");
                        Mes.Add("*We need to go some place cool, or else we may end up passing out due to the heat.*");
                    }
                    if (player.ZoneRain)
                    {
                        Mes.Add("*It's raining cats and dogs outside, good thing that we're not out there.*");
                        Mes.Add("*It's a nice weather for a rest. Let's enjoy the sound of rain drops.*");
                        Mes.Add("*Achooo~! Sorry, I think I have caught flu.*");
                    }
                    if (!Main.dayTime)
                    {
                        Mes.Add("*This night seems perfect to sleep.*");
                        Mes.Add("*Do you mind if I hug you during my sleep?*");
                        Mes.Add("*...Zzz... Oh- ah! I'm awake!*");
                    }
                    else
                    {
                        if (!player.ZoneRain && !Main.eclipse)
                        {
                            Mes.Add("*It's a beautiful day outside!*");
                            Mes.Add("*This weather makes me happy.*");
                        }
                        else if (Main.eclipse)
                        {
                            Mes.Add("*Is this house safe against those creatures?*");
                            Mes.Add("*Yikes! Where did those things came from?*");
                            Mes.Add("*I'm not scared, It's cold here.*");
                        }
                    }
                    Mes.Add("*Do I talk during sleep? Some of the people I hug say that I do.*");
                    Mes.Add("*Some people says I talk during my sleep. I don't believe.*");
                    if (guardian.IsUsingBed)
                    {
                        Mes.Add("(pulls closer.)");
                        Mes.Add("*Zzz... Warm.... Zzz...*");
                        Mes.Add("*Zzz.... Bro... Are you... Crying... Zzz...*");
                        Mes.Add("*Zzz... Don't... Feel sad... I'm here... Zzz...*");
                        Mes.Add("*Zzz.... You're not... Weak... Bro... Zzz...*");
                        Mes.Add("*Zzz... Bro... Where are you... Going... Zzz...*");
                        Mes.Add("*Zzz... Mom... Zzz...*");
                        Mes.Add("*Zzz... I miss you... Mom... Zzz...*");
                        Mes.Add("*Zzz... Father... Don't be mean... I don't wanna... Fight... Zzz...*");
                        Mes.Add("*Zzz... I'm... Not a fighter... Zzz...*");
                        Mes.Add("*Zzz... I'm not alone... Zzz...*");
                        Mes.Add("*Zzz... Friends... Good... Zzz...*");
                        Mes.Add("*Zzz... Where I Belong... Octave fantasy... Zzz...*");
                    }
                    if (guardian.IsUsingToilet)
                    {
                        Mes.Add("*If the smell bother you, I can hug you another time.*");
                        Mes.Add("*Did you smell that? Sorry. I had to release.*");
                        Mes.Add("*I hope you don't mind the noises.*");
                        Mes.Add("*Ugh... My stomach... I think this is a bad time for a hug... Oww...*");
                        Mes.Add("*I don't mind hugging at this moment, It's not like as if you are watching me doing my business.*");
                        Mes.Add("*Sorry If I'm hugging too strong, but I really need to use some strength right now.*");
                        Mes.Add("*Oh! Did that noise scared you?*");
                    }
                    if (guardian.IsPlayerRoomMate(player))
                    {
                        Mes.Add("*I'm very happy of sharing my room with you, I never feel lonelly again.*");
                        Mes.Add("*Having someone to hug during the night always makes the night better.*");
                    }
                }
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("(He seems to have whited out.)");
                Mes.Add("(You don't see any sign of pain or sorrow on his face, It's like as if he was sleeping.)");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(Even when sleeping he seems happy)");
                Mes.Add("*Brother, don't feel sad...* (He says when sleeping)");
                Mes.Add("*Where are you going, brother...? * (He says when sleeping)");
                Mes.Add("*I want to help everybody... Hug everyone... In need..* (He says when sleeping)");
                Mes.Add("(He seems to be having nightmares)");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I like giving hugs because I used to give them to my younger brother. I always felt great when giving him hugs whenever he needed. But then... He grew up...*");
            Mes.Add("*My father never liked the fact I used my huge paws to hug, he said I should have been a fierce warrior like the ancestors from my family.*");
            Mes.Add("*Some people look at me with weird eyes whenever they see me hugging people. What is on their mind?*");
            Mes.Add("*Remember when I mentioned my younger brother? I don't feel bad about the fact he grew up, but he rarelly needs me for anything, It's like as if I got a hole in my life...*");
            Mes.Add("*Okay, Okay, I admit. I begun exploring to look for places where I could help people, and fill the hole left by my brother. Good thing that there is way more to fill that hole than I expected.*");
            Mes.Add("*Whenever I hug someone, most of the times they start talking about things that troubles them. So I feel like a therapist most of the time, but one who mostly listens only.*");
            Mes.Add("*You would be impressed at the variety of troubles I listen to when hugging people.*");
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*I have to tell you, [gn:"+Malisha+"] scares me a bit.*");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Nope, I don't need anything.*");
            Mes.Add("*I have everything I need with me.*");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I feel awkward for asking that but... Wait, don't laugh, I don't mind giving hugs, but asking things isn't my kind of thing.*");
            Mes.Add("*Uh... Say... Could you do something for me?*");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You're really a great person, I like that.*");
            Mes.Add("*I think only hugging isn't enough, here some things I had stored.*");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Say, do you have any place I could stay?*");
            Mes.Add("*Those creatures aren't very friendly, I tried to hug a Slime and nearly lost the hair in my chest. Do you have a less dangerous place I could stay?*");
            Mes.Add("*I'm sure It will give you some trouble to build a house for me, but I really need to keep my head straight, since I had a few problems with my neck weeks ago.*");
            if (Main.raining)
            {
                Mes.Add("*This is awful... Is there some place I can stay?*");
                Mes.Add("*Gruff! I smell awful when wet. I need some place to dry myself.*");
            }
            if (!Main.dayTime)
            {
                Mes.Add("*I'm sorry but, I don't want to be zombie food.*");
                Mes.Add("*There are creatures out there wanting a piece of me, please, HELP!*");
                Mes.Add("*I want to sleep!! I can't simply sleep here in the open, It's too dangerous!*");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("*Listen up! You will build me a house, OR I WILL CRUSH ALL YOUR BONES UNTIL THEY TURN TO DUST!*");
                Mes.Add("*GRRRRRR... Why you didn't made my house yet?!*");
                Mes.Add("*Do you want to see how hard my paw hit your frail body? BUILD MY A HOUSE! NOW!*");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's my birthday? Then you know what gift you can give me.*");
            Mes.Add("*I have to be extra careful when dancing, I don't want to hurt anybody when moving.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("*Do you have something for me? I'm curious.*");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I could try hugging you while on the ground, but I fear about crushing you with my weight.*");
            Mes.Add("*Let me help you again.*");
            Mes.Add("*I've been able to stop your bleeding.*");
            if (IsPlayer)
            {
                Mes.Add("*You're cold. I'll solve that.*");
            }
            else if(ReviveGuardian.ModID == Guardian.ModID)
            {
                switch (ReviveGuardian.ID)
                {
                    case GuardianBase.Brutus:
                        Mes.Add("*Why you don't let me hug you?*");
                        break;
                    case GuardianBase.Leopold:
                        Mes.Add("*It's weird seeing you quiet.*");
                        break;
                    case GuardianBase.Mabel:
                        Mes.Add("*This.. is.. no.. time... for..... fear...!*");
                        break;
                    case GuardianBase.Rococo:
                        Mes.Add("*I'm here to help you, buddy!*");
                        break;
                    case GuardianBase.Sardine:
                        Mes.Add("*You seem to be sleeping.*");
                        break;
                    case GuardianBase.Zacks:
                        Mes.Add("*Okay, which hole should I make the blood stop coming from?*");
                        break;
                }
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*Take as much rest you need, you're in a friendly place now.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*Yaaaaaawnn.... Oh.. Hello [nickname]... Do you need something?*";
                        case 1:
                            return "*Oh, hello [nickname]. Are you in need of a hug?";
                        case 2:
                            return "*You need something from me? I was having some sleep.*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Oh... Hello.. Just stretching a bit... Done. Did you do what I asked?*";
                        case 1:
                            return "*Hey [nickname], you woke up because of my request, right? Did you finish It?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Sure. I would love making you company during your travels.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I'm sorry, but there are too many people in the group right now.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*I can't right now. I have some other things to do right now. I'm sorry.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*But [nickname], this place is dangerous for me. Do you really want to leave me all alone in this place?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Alright. I had so much fun exploring the world with you. Feel free to call me another time.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I'll try getting home then. Have a safe travel, [nickname].*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I'm happy that you changed your mind, [nickname].*";
                case MessageIDs.RequestAccepted:
                    return "*Thank you.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*I'm sorry, but I can't allow that. You will get yourself stressed out if you do many things at the same time. Go do your other requests before I give you mine.*";
                case MessageIDs.RequestRejected:
                    return "*Oh... Okay... (Did I ask something too hard?)*";
                case MessageIDs.RequestPostpone:
                    return "*Sure. My request can wait.*";
                case MessageIDs.RequestFailed:
                    return "*Don't worry [nickname], you tried your best. If you want, I can give you a hug so you can feel better.*";
                case MessageIDs.RestAskForHowLong:
                    return "*That's a lovelly idea. How long are we going to rest?*";
                case MessageIDs.RestNotPossible:
                    return "*This is not a good moment to rest, sadly.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*I think I know of a lullaby or two, if you need.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*I think [shop] has a thing I need, could we go visit It?*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Hm... Let's see...*";
                case MessageIDs.GenericYes:
                    return "*I find that good.*";
                case MessageIDs.GenericNo:
                    return "*I disliked that idea.*";
                case MessageIDs.GenericThankYou:
                    return "*You have my sincere Thank You.*";
                case MessageIDs.ChatAboutSomething:
                    return "*You want to chat? It's always good to hang out with a friend sometimes. What do you want to know?*";
                case MessageIDs.NevermindTheChatting:
                    return "*I enjoyed the chatting, let's talk more later.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*I see... You don't want to do what I asked you to, right?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*That's fine, [nickname]. Not everytime It's possible to do something we thing is possible, right? Don't worry.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Okay. Do you need something else?*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
