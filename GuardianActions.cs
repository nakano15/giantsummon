using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon
{
    public class GuardianActions
    {
        public bool InUse = false;
        public int _ID = 0;
        public int ID { get { return _ID; } set { _ID = value; } }
        public bool IsGuardianSpecificAction = false;
        public int Time = 0, Step = 0;
        public List<Player> Players = new List<Player>();
        public List<NPC> Npcs = new List<NPC>();
        public List<TerraGuardian> Guardians = new List<TerraGuardian>();
        private Dictionary<byte, int> IntegerVariables = new Dictionary<byte, int>();
        private Dictionary<byte, float> FloatVariables = new Dictionary<byte, float>();
        public bool IgnoreCombat = false, Cancellable = false, AvoidItemUsage = false, FocusCameraOnGuardian = false, EffectOnlyMirror = false, Invisibility = false, Immune = false, NoAggro = false, Inactivity = false, CantUseInventory = false, NpcCanFacePlayer = true, ProceedIdleAIDuringDialogue = false;
        private bool StepStart { get { return Time == 0; } }
        private bool StepChanged = false;

        public void ChangeStep(int Number = -1)
        {
            if (Number == -1) Step++;
            else Step = Number;
            Time = 0;
            StepChanged = true;
        }

        public int GetIntegerValue(byte VarID)
        {
            if (!IntegerVariables.ContainsKey(VarID))
                return 0;
            return IntegerVariables[VarID];
        }

        public float GetFloatValue(byte VarID)
        {
            if (!FloatVariables.ContainsKey(VarID))
                return 0;
            return FloatVariables[VarID];
        }

        public void SetIntegerValue(byte VarID, int Value)
        {
            if (!IntegerVariables.ContainsKey(VarID))
            {
                IntegerVariables.Add(VarID, Value);
                return;
            }
            IntegerVariables[VarID] = Value;
        }

        public void SetFloatValue(byte VarID, float Value)
        {
            if (!FloatVariables.ContainsKey(VarID))
            {
                FloatVariables.Add(VarID, Value);
                return;
            }
            FloatVariables[VarID] = Value;
        }

        public static bool TryRevivingPlayer(TerraGuardian Guardian, Player target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.ReviveSomeone;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Players.Add(target);
            return true;
        }

        public static bool TryRevivingGuardian(TerraGuardian Guardian, TerraGuardian target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.ReviveSomeone;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Guardians.Add(target);
            return true;
        }

        public static void UseThrowPotion(TerraGuardian Guardian, Player Target)
        {
            const byte PotionPosVar = 0;
            int HighestHealingDifference = -1, Potion = -1;
            for (int i = 0; i < 50; i++)
            {
                if (Guardian.Inventory[i].type != 0 && Guardian.Inventory[i].potion && !MainMod.IsGuardianItem(Guardian.Inventory[i]))
                {
                    int PosHealingValue = Math.Abs((Target.statLifeMax2 - Target.statLife) - (int)(Guardian.Inventory[i].healLife * Guardian.HealthHealMult));
                    if (HighestHealingDifference == -1 || PosHealingValue < HighestHealingDifference)
                    {
                        Potion = i;
                        HighestHealingDifference = PosHealingValue;
                    }
                }
            }
            if (Potion < 0)
                return;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.ThrowPotion;
            Guardian.DoAction.Players.Add(Target);
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.SetIntegerValue(PotionPosVar, Potion);
        }

        public static bool UseLaunchPlayer(TerraGuardian Guardian, Player Target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.LaunchPlayer;
            Guardian.DoAction.Players.Add(Target);
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static bool UseLiftPlayer(TerraGuardian Guardian, Player Target)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.LiftPlayer;
            Guardian.DoAction.Players.Add(Target);
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static bool UseResurrectOnPlayer(TerraGuardian Guardian, Player Target)
        {
            if (!Target.dead || Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.ResurrectPlayer;
            Guardian.DoAction.Players.Add(Target);
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static bool UsePotionOnPlayer(TerraGuardian Guardian, Player Target)
        {
            if (!Target.dead || Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.UsePotionOnPlayer;
            Guardian.DoAction.Players.Add(Target);
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static bool UseBuffPotions(TerraGuardian Guardian)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.UseBuffPotions;
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static bool UseStatusIncreaseItems(TerraGuardian Guardian)
        {
            if (Guardian.DoAction.InUse) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.UseStatusIncreaseItems;
            Guardian.DoAction.InUse = true;
            return true;
        }

        public static void OpenBirthdayPresent(TerraGuardian Guardian, int BoxPosition)
        {
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.OpenGiftBox;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.SetIntegerValue(0, BoxPosition);
        }

        public static void SellLootCommand(TerraGuardian Guardian)
        {
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.SellItems;
            Guardian.DoAction.InUse = true;
        }

        public static void GuardianJoinPlayerMountCommand(TerraGuardian Guardian, Player TargetPlayer)
        {
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.JoinPlayerMount;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Players.Add(TargetPlayer);
        }

        public static bool GuardianPutPlayerOnShoulderCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1 || Guardian.DoAction.InUse)
                return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.PickupPlayerMount;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Players.Add(Main.player[Guardian.OwnerPos]);
            return true;
        }

        public static void GuardianPutPlayerOnTheFloorCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1)
                return;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.MountPutPlayerDown;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Players.Add(Main.player[Guardian.OwnerPos]);
        }

        public static bool TeleportWithPlayerCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1 || Guardian.DoAction.InUse || !Guardian.HasMagicMirror) return false;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.TeleportWithPlayerToTown;
            Guardian.DoAction.InUse = true;
            Guardian.DoAction.Players.Add(Main.player[Guardian.OwnerPos]);
            return true;
        }

        public static void UseSkillResetPotionCommand(TerraGuardian Guardian)
        {
            if (Guardian.OwnerPos == -1) return;
            Guardian.DoAction = new GuardianActions();
            Guardian.DoAction.ID = (int)ActionIDs.UseSkillResetPotion;
            Guardian.DoAction.InUse = true;
        }
        
        public void Update(TerraGuardian guardian)
        {
            if (!InUse) return;
            if (guardian.Downed || guardian.KnockedOut || guardian.KnockedOutCold)
            {
                InUse = false;
                return;
            }
            if (IsGuardianSpecificAction)
            {
                guardian.Base.GuardianActionUpdate(guardian, this);
            }
            else
            {
                switch ((ActionIDs)ID)
                {
                    case ActionIDs.ReviveSomeone:
                        {
                            if (guardian.furniturex > -1)
                                guardian.LeaveFurniture(false);
                            const byte IReviveTime = 0;
                            Vector2 TargetPosition = Vector2.Zero;
                            int TargetWidth = 0, TargetHeight = 0;
                            bool TryReaching = false;
                            bool IsPlayer = Players.Count > 0;
                            if (Players.Count > 0)
                            {
                                TargetPosition = Players[0].position;
                                TargetWidth = Players[0].width;
                                TargetHeight = Players[0].height;
                                if (Players[0].dead || !Players[0].active || !Players[0].GetModPlayer<PlayerMod>().KnockedOut)
                                {
                                    InUse = false;
                                    return;
                                }
                            }
                            if (Guardians.Count > 0)
                            {
                                TargetPosition = Guardians[0].TopLeftPosition;
                                TargetWidth = Guardians[0].Width;
                                TargetHeight = Guardians[0].Height;
                                if (!Guardians[0].Active || Guardians[0].Downed || !Guardians[0].KnockedOut)
                                {
                                    InUse = false;
                                    return;
                                }
                            }
                            /*if (guardian.TopLeftPosition.X < TargetPosition.X + TargetWidth &&
                                guardian.TopLeftPosition.X + guardian.Width >= TargetPosition.X &&
                                guardian.TopLeftPosition.Y - guardian.Height < TargetPosition.Y + TargetHeight &&
                                guardian.TopLeftPosition.Y >= TargetPosition.Y)*/
                            bool RepelingEnemies = false;
                            guardian.MoveLeft = guardian.MoveRight = false;
                            if (guardian.TargetID > -1)
                            {
                                Vector2 EnemyPosition;
                                int EnemyWidth, EnemyHeight;
                                guardian.GetTargetInformation(out EnemyPosition, out EnemyWidth, out EnemyHeight);
                                EnemyPosition.X += EnemyWidth * 0.5f;
                                EnemyPosition.Y += EnemyHeight * 0.5f;
                                if ((EnemyPosition - guardian.CenterPosition).Length() < 168f + EnemyWidth * 0.5f)
                                {
                                    RepelingEnemies = true;
                                }
                            }
                            if (!RepelingEnemies)
                            {
                                if (new Rectangle((int)TargetPosition.X, (int)TargetPosition.Y, TargetWidth, TargetHeight).Intersects(guardian.HitBox))//(MainMod.RectangleIntersects(guardian.TopLeftPosition, guardian.Width, guardian.Height, TargetPosition, TargetWidth, TargetHeight))
                                {
                                    if (guardian.Velocity.X == 0)
                                    {
                                        guardian.FaceDirection(TargetPosition.X + TargetWidth * 0.5f - guardian.Position.X < 0);
                                        byte ReviveBoost = 1;
                                        if (guardian.TargetID == -1)
                                            ReviveBoost += 2;
                                        if (IsPlayer)
                                            Players[0].GetModPlayer<PlayerMod>().ReviveBoost += ReviveBoost;
                                        else
                                            Guardians[0].ReviveBoost += ReviveBoost;
                                        guardian.StuckTimer = 0;
                                        guardian.OffHandAction = false;
                                    }
                                }
                                else
                                {
                                    TryReaching = true;
                                }
                                IgnoreCombat = true;
                            }
                            else
                            {
                                IgnoreCombat = false;
                            }
                            if (TryReaching)
                            {
                                int ResTime = GetIntegerValue(IReviveTime);
                                if (ResTime >= 5 * 60)
                                {
                                    guardian.Position.X = TargetPosition.X + Main.rand.Next(TargetWidth);
                                    guardian.Position.Y = TargetPosition.Y + TargetHeight - 1;
                                }
                                else if (TargetPosition.X + TargetWidth * 0.5f - guardian.Position.X < 0)
                                {
                                    guardian.MoveLeft = true;
                                }
                                else
                                {
                                    guardian.MoveRight = true;
                                }
                                SetIntegerValue(IReviveTime, ResTime + 1);
                            }
                            else
                            {
                                SetIntegerValue(IReviveTime, 0);
                            }
                        }
                        break;

                    case ActionIDs.UseSkillResetPotion:
                        {
                            if (!guardian.LastAction && guardian.ItemAnimationTime <= 0)
                            {
                                int ItemID = ModContent.ItemType<Items.Consumable.SkillResetPotion>();
                                for (int i = 0; i < 50; i++)
                                {
                                    if (guardian.Inventory[i].type == ItemID)
                                    {
                                        guardian.SelectedItem = i;
                                        guardian.Action = true;
                                        break;
                                    }
                                }
                                InUse = false;
                            }
                        }
                        break;
                    case ActionIDs.TeleportWithPlayerToTown:
                        {
                            bool CanPickupPlayer = guardian.Base.MountUnlockLevel != 255 && !guardian.Base.ReverseMount && !guardian.PlayerMounted && !guardian.SittingOnPlayerMount;
                            AvoidItemUsage = true;
                            bool HoldingPlayer = false;
                            FocusCameraOnGuardian = false;
                            /*if (Players[0].dead)
                            {
                                InUse = false;
                                return;
                            }*/
                            switch (Step)
                            {
                                case 0:
                                    if (guardian.furniturex > -1)
                                        guardian.LeaveFurniture();
                                    if (TryReachingPlayer(guardian, Players[0]))
                                    {
                                        ChangeStep();
                                    }
                                    break;
                                case 1:
                                    if (!CanPickupPlayer)
                                    {
                                        ChangeStep();
                                    }
                                    else
                                    {
                                        FocusCameraOnGuardian = true;
                                        if (Time < 8)
                                        {
                                            int AnimFrame = Time / 4;
                                            if (guardian.Ducking)
                                            {
                                                SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.DuckingSwingFrames[2 - AnimFrame], HeldHand.Right);
                                            }
                                            else
                                            {
                                                SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.ItemUseFrames[3 - AnimFrame], HeldHand.Right);
                                            }
                                        }
                                        else
                                        {
                                            HoldingPlayer = true;
                                            ChangeStep();
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        IgnoreCombat = true;
                                        AvoidItemUsage = false;
                                        EffectOnlyMirror = true;
                                        if (!guardian.HasMagicMirror)
                                        {
                                            ChangeStep(4);
                                        }
                                        if (guardian.ItemAnimationTime <= 0)
                                        {
                                            guardian.UseMagicMirror();
                                            if (Time >= 10 * 60)
                                            {
                                                ChangeStep(4);
                                                Main.NewText("For some reason, " + guardian.Name + " got Itself confused when using the magic mirror.");
                                            }
                                        }
                                        if (guardian.MagicMirrorTrigger)
                                        {
                                            ChangeStep();
                                            Player player = Players[0];
                                            if (guardian.HasBuff(Terraria.ID.BuffID.Horrified))
                                            {
                                                string Mes;
                                                switch (Main.rand.Next(6))
                                                {
                                                    default:
                                                        Mes = " is asking if you are nuts.";
                                                        break;
                                                    case 0:
                                                        Mes = " is asking what you have on your head.";
                                                        break;
                                                    case 1:
                                                        Mes = " says that this is not the right time and place to do that.";
                                                        break;
                                                    case 2:
                                                        Mes = " questions itself \"WHAT?!\" a few moments before attempting to teleport.";
                                                        break;
                                                    case 3:
                                                        Mes = " asks if there is something wrong with your head.";
                                                        break;
                                                    case 4:
                                                        Mes = " said that It's not the wisest thing to do right now.";
                                                        break;
                                                }
                                                Main.NewText("*" + guardian.Name + Mes + "*");
                                            }
                                            else
                                            {
                                                player.Spawn();
                                                guardian.Spawn();
                                                PlayerMod pm = player.GetModPlayer<PlayerMod>();
                                                /*guardian.Spawn();
                                                guardian.Position.X = player.SpawnX * 16;
                                                guardian.Position.Y = player.SpawnY * 16;
                                                player.position.X = player.SpawnX * 16 + 8 - player.width * 0.5f;
                                                player.position.Y = player.SpawnY * 16 - player.height;*/
                                                Vector2 PlayerBottom = new Vector2(player.position.X + player.width * 0.5f, player.position.Y + player.height);
                                                guardian.Position = PlayerBottom;
                                                guardian.FallStart = (int)guardian.Position.Y / 16;
                                                foreach (TerraGuardian mg in pm.GetAllGuardianFollowers)
                                                {
                                                    if (mg.Active && (mg.PlayerMounted || mg.PlayerControl))
                                                    {
                                                        mg.Spawn();
                                                        mg.Position = PlayerBottom;
                                                        mg.FallStart = (int)mg.Position.Y / 16;
                                                    }
                                                }
                                            }
                                        }
                                        HoldingPlayer = true;
                                    }
                                    break;
                                case 3:
                                    {
                                        if (guardian.ItemAnimationTime == 0)
                                        {
                                            ChangeStep();
                                        }
                                        HoldingPlayer = true;
                                    }
                                    break;
                                case 4:
                                    {
                                        if (!CanPickupPlayer)
                                        {
                                            ChangeStep();
                                        }
                                        else
                                        {
                                            FocusCameraOnGuardian = true;
                                            if (Time < 8)
                                            {
                                                if (guardian.Ducking)
                                                {
                                                    int AnimFrame = 2 - Time / 4;
                                                    SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.DuckingSwingFrames[2 - AnimFrame], HeldHand.Right);
                                                }
                                                else
                                                {
                                                    int AnimFrame = 2 - Time / 4;
                                                    SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.ItemUseFrames[3 - AnimFrame], HeldHand.Right);
                                                }
                                            }
                                            else
                                            {
                                                ChangeStep();
                                            }
                                        }
                                    }
                                    break;
                                case 5:
                                    if (CanPickupPlayer)
                                        SetPlayerPositionOnGuardianCenter(guardian, Players[0]);
                                    InUse = false;
                                    break;
                            }
                            if (HoldingPlayer)
                            {
                                if (CanPickupPlayer)
                                {
                                    FocusCameraOnGuardian = true;
                                    if (guardian.Ducking)
                                    {
                                        SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.DuckingSwingFrames[1], HeldHand.Right);
                                    }
                                    else
                                    {
                                        SetPlayerOnHandPosition(guardian, Players[0], guardian.Base.ItemUseFrames[2], HeldHand.Right);
                                    }
                                    Players[0].velocity.Y = -Player.defaultGravity;
                                    guardian.ProtectingPlayerFromHarm = true;
                                }
                                else if (!guardian.PlayerMounted && !guardian.SittingOnPlayerMount)
                                {
                                    /*Vector2 HandPosition = GetHandPosition(guardian, guardian.Base.ItemUseFrames[2], HeldHand.Right);
                                    float HorizontalPosChange = Players[0].Center.X - HandPosition.X;
                                    if (!guardian.PlayerMounted && !guardian.SittingOnPlayerMount && Math.Abs(HorizontalPosChange) >= Players[0].width * 0.5f)
                                    {
                                        //guardian.Position.X += HorizontalPosChange;
                                        if (HorizontalPosChange < 0)
                                        {
                                            guardian.MoveLeft = true;
                                        }
                                        else
                                        {
                                            guardian.MoveRight = true;
                                        }
                                    }*/
                                }
                            }
                        }
                        break;
                    case ActionIDs.MountPutPlayerDown:
                        {
                            Player Target = Players[0];
                            if (Time == 0 && guardian.PlayerMounted)
                            {
                                guardian.ToggleMount(false, false);
                            }
                            if (guardian.Base.ReverseMount || guardian.Base.DontUseRightHand || guardian.UsingFurniture)
                            {
                                InUse = false;
                            }
                            else
                            {
                                FocusCameraOnGuardian = true;
                                if (Time >= 20)
                                {
                                    InUse = false;
                                    Target.position.X = guardian.Position.X - Target.width * 0.5f;
                                    Target.position.Y = guardian.Position.Y - Target.height;
                                    Target.velocity = guardian.Velocity;
                                    Target.fallStart = (int)Target.position.Y / 16;
                                }
                                else
                                {
                                    int AnimFrame = guardian.Base.ItemUseFrames[0];
                                    if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[3];
                                    else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[2];
                                    else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[1];
                                    int HPosX, HPosY;
                                    guardian.GetRightHandPosition(AnimFrame, out HPosX, out HPosY);
                                    Vector2 HandPosition = guardian.Position;
                                    HandPosition.X += HPosX;
                                    HandPosition.Y += HPosY;
                                    HandPosition.X -= Target.width * 0.5f;
                                    HandPosition.Y -= Target.height * 0.5f;
                                    Target.position = HandPosition;
                                    Target.fallStart = (int)Target.position.Y / 16;
                                    Target.velocity.X = 0;
                                    Target.velocity.Y = -Player.defaultGravity;
                                    if (Target.itemAnimation == 0)
                                    {
                                        Target.direction = guardian.Direction;
                                    }
                                }
                            }
                            guardian.MoveRight = Target.controlRight;
                            guardian.MoveLeft = Target.controlLeft;
                            guardian.MoveUp = Target.controlUp;
                            guardian.MoveDown = Target.controlDown;
                            guardian.Jump = Target.controlJump;
                        }
                        break;
                    case ActionIDs.PickupPlayerMount:
                        {
                            Player Target = Players[0];
                            if (Step == 0) //Try reaching player
                            {
                                if (guardian.PlayerMounted)
                                {
                                    InUse = false;
                                    return;
                                }
                                IgnoreCombat = true;
                                if (!guardian.BeingPulledByPlayer)
                                {
                                    if (TryReachingPlayer(guardian, Target))
                                    {
                                        ChangeStep();
                                    }
                                    else
                                    {
                                        if (guardian.furniturex > -1)
                                            guardian.LeaveFurniture();
                                    }
                                    /*if (guardian.HitBox.Intersects(Target.getRect()))
                                    {
                                        ChangeStep();
                                    }
                                    else
                                    {
                                        guardian.MoveLeft = guardian.MoveRight = false;
                                        if (Target.Center.X < guardian.Position.X)
                                        {
                                            guardian.MoveLeft = true;
                                        }
                                        if (Target.Center.X > guardian.Position.X)
                                        {
                                            guardian.MoveRight = true;
                                        }
                                    }*/
                                }
                            }
                            else //Pickup Player animation.
                            {
                                //guardian.PlayerMounted = true;
                                if (guardian.Base.ReverseMount || guardian.Base.DontUseRightHand || guardian.UsingFurniture)
                                {
                                    guardian.ToggleMount(false, false);
                                    InUse = false;
                                }
                                else
                                {
                                    FocusCameraOnGuardian = true;
                                    if (Time >= 20)
                                    {
                                        guardian.ToggleMount(false, false);
                                        InUse = false;
                                    }
                                    else
                                    {
                                        if (Target.mount.Active)
                                            Target.mount.Dismount(Target);
                                        int AnimFrame = guardian.Base.ItemUseFrames[3];
                                        if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[0];
                                        else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[1];
                                        else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[2];
                                        int HPosX, HPosY;
                                        guardian.GetRightHandPosition(AnimFrame, out HPosX, out HPosY);
                                        Vector2 HandPosition = guardian.Position;
                                        HandPosition.X += HPosX;
                                        HandPosition.Y += HPosY;
                                        HandPosition.X -= Target.width * 0.5f;
                                        HandPosition.Y -= Target.height * 0.5f;
                                        Target.position = HandPosition;
                                        Target.fallStart = (int)Target.position.Y / 16;
                                        Target.velocity.X = 0;
                                        Target.velocity.Y = -Player.defaultGravity;
                                        if (Target.itemAnimation == 0)
                                        {
                                            Target.direction = guardian.Direction;
                                        }
                                    }
                                }
                                guardian.MoveRight = Target.controlRight;
                                guardian.MoveLeft = Target.controlLeft;
                                guardian.MoveUp = Target.controlUp;
                                guardian.MoveDown = Target.controlDown;
                                guardian.Jump = Target.controlJump;
                            }
                        }
                        break;

                    case ActionIDs.JoinPlayerMount:
                        {
                            Player Target = Players[0];
                            IgnoreCombat = true;
                            if (!Target.mount.Active)
                            {
                                InUse = false;
                                return;
                            }
                            if (TryReachingPlayer(guardian, Target))//guardian.HitBox.Intersects(Target.getRect()))
                            {
                                guardian.DoSitOnPlayerMount(true);
                                InUse = false;
                            }
                            else
                            {
                                if (guardian.furniturex > -1)
                                    guardian.LeaveFurniture();
                                /*if (Target.Center.X < guardian.Position.X)
                                {
                                    guardian.MoveLeft = true;
                                }
                                if (Target.Center.X > guardian.Position.X)
                                {
                                    guardian.MoveRight = true;
                                }*/
                            }
                        }
                        break;

                    case ActionIDs.SellItems:
                        {
                            EffectOnlyMirror = true;
                            CantUseInventory = true;
                            const byte TeleportTimeVar = 0, PlayerLastMountedCheckVar = 1, TeleportMethod = 2;
                            bool TeleportedEffects = false;
                            switch (Step)
                            {
                                case 0: //Check distance to town, calculate time to sell items.
                                    {
                                        if (StepStart)
                                        {
                                            if (guardian.furniturex > -1)
                                                guardian.LeaveFurniture();
                                            if (!guardian.HasMagicMirror)
                                            {
                                                SetIntegerValue(TeleportMethod, 1);
                                                //InUse = false;
                                                //return;
                                            }
                                            else
                                            {
                                                Vector2 GuardianPos = guardian.CenterPosition;
                                                Vector2 ResultPoint = new Vector2(Main.spawnTileX * 16, Main.spawnTileY * 16);
                                                int Time = 60 * 10;
                                                //Time += (int)Math.Abs(ResultPoint.X - GuardianPos.X) / 128;
                                                //Time += (int)Math.Abs(ResultPoint.Y - GuardianPos.Y) / 128;
                                                SetIntegerValue(TeleportTimeVar, Time);
                                                SetIntegerValue(TeleportMethod, 0);
                                            }
                                        }
                                        if (GetIntegerValue(TeleportMethod) == 1)
                                        {
                                            //make guardian walk away from the player. If It gets away from screen distance, teleport.
                                            //If passes 5 seconds and It didn't teleport, make it disappear and calculate the time until nearest vendor.
                                            bool DoMove = false;
                                            if (this.Time >= 60 * 5)
                                                DoMove = true;
                                            else if (Math.Abs(guardian.Position.X - Main.player[guardian.OwnerPos].Center.X) < NPC.sWidth * 16)
                                            {
                                                IgnoreCombat = true;
                                                CantUseInventory = true;
                                                Immune = true;
                                                guardian.StuckTimer = 0;
                                                guardian.MoveLeft = guardian.MoveRight = false;
                                                if (Main.player[guardian.OwnerPos].Center.X - guardian.Position.X < 0)
                                                {
                                                    guardian.MoveRight = true;
                                                }
                                                else
                                                {
                                                    guardian.MoveLeft = true;
                                                }
                                            }
                                            else
                                            {
                                                DoMove = true;
                                            }
                                            if (DoMove)
                                            {
                                                int NearestTownNPC = -1;
                                                float NearestDist = -1;
                                                for (int n = 0; n < 200; n++)
                                                {
                                                    if (Main.npc[n].active && Main.npc[n].townNPC && MainMod.VendorNpcs.Contains(Main.npc[n].type))
                                                    {
                                                        float Distance = (guardian.CenterPosition - Main.npc[n].Center).Length();
                                                        if (NearestDist == -1 || Distance < NearestDist)
                                                        {
                                                            NearestTownNPC = n;
                                                            NearestDist = Distance;
                                                        }
                                                    }
                                                }
                                                Vector2 ResultPosition = Vector2.Zero;
                                                if (NearestTownNPC > -1)
                                                {
                                                    ResultPosition = Main.npc[NearestTownNPC].Center;
                                                }
                                                else
                                                {
                                                    ResultPosition.X = Main.spawnTileX * 16;
                                                    ResultPosition.Y = Main.spawnTileY * 16;
                                                }
                                                float WalkTime = 16f / guardian.MoveSpeed;
                                                int Time = (int)((Math.Abs(ResultPosition.X - guardian.CenterPosition.X) + Math.Abs(ResultPosition.Y - guardian.CenterPosition.Y)) * WalkTime) / (16 * 16) + 60 * 7;
                                                SetIntegerValue(TeleportTimeVar, Time);
                                                SetIntegerValue(PlayerLastMountedCheckVar, (guardian.PlayerMounted ? 1 : 0));
                                                if (guardian.PlayerMounted)
                                                    guardian.ToggleMount(true);
                                                ChangeStep();
                                                TeleportedEffects = true;
                                            }
                                        }
                                        else
                                        {
                                            if (guardian.ItemAnimationTime <= 0)
                                            {
                                                guardian.UseMagicMirror();
                                            }
                                            if (guardian.MagicMirrorTrigger)
                                            {
                                                SetIntegerValue(PlayerLastMountedCheckVar, (guardian.PlayerMounted ? 1 : 0));
                                                if (guardian.PlayerMounted)
                                                    guardian.ToggleMount(true);
                                                ChangeStep();
                                                TeleportedEffects = true;
                                            }
                                        }
                                    }
                                    break;
                                case 1:
                                    {
                                        int TeleportTime = GetIntegerValue(TeleportTimeVar);
                                        TeleportedEffects = true;
                                        if (Time >= TeleportTime)
                                        {
                                            bool SendToPiggyBank = guardian.FriendshipGrade >= 3;
                                            bool SoldItems = false, SentItemsToPiggyBank = false;
                                            int p = 0, g = 0, s = 0, c = 0;
                                            int token = 0;
                                            int copperstack = 0;
                                            for (int i = 10; i < 50; i++)
                                            {
                                                if (i != guardian.SelectedItem && guardian.Inventory[i].type != 0 && !guardian.Inventory[i].favorited)
                                                {
                                                    if ((guardian.Inventory[i].type < Terraria.ID.ItemID.CopperCoin || guardian.Inventory[i].type > Terraria.ID.ItemID.PlatinumCoin) &&
                                                    guardian.Inventory[i].type != Terraria.ID.ItemID.DefenderMedal)
                                                    {
                                                        c += guardian.Inventory[i].value * guardian.Inventory[i].stack;
                                                        guardian.Inventory[i].SetDefaults(0);
                                                        SoldItems = true;
                                                    }
                                                    else if (SendToPiggyBank)
                                                    {
                                                        SentItemsToPiggyBank = true;
                                                        switch (guardian.Inventory[i].type)
                                                        {
                                                            case Terraria.ID.ItemID.CopperCoin:
                                                                copperstack += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                                break;
                                                            case Terraria.ID.ItemID.SilverCoin:
                                                                s += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                                break;
                                                            case Terraria.ID.ItemID.GoldCoin:
                                                                g += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                                break;
                                                            case Terraria.ID.ItemID.PlatinumCoin:
                                                                p += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                                break;
                                                            case Terraria.ID.ItemID.DefenderMedal:
                                                                token += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                                break;
                                                        }
                                                    }
                                                }
                                            }
                                            c = c / 5 + copperstack;
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
                                            string ResultText = "";
                                            bool First = true;
                                            if (p > 0)
                                            {
                                                First = false;
                                                ResultText += p + " Platinum";
                                            }
                                            if (g > 0)
                                            {
                                                if (!First)
                                                    ResultText += ", ";
                                                First = false;
                                                ResultText += g + " Gold";
                                            }
                                            if (s > 0)
                                            {
                                                if (!First)
                                                    ResultText += ", ";
                                                First = false;
                                                ResultText += s + " Silver";
                                            }
                                            if (c > 0)
                                            {
                                                if (!First)
                                                    ResultText += ", ";
                                                First = false;
                                                ResultText += c + " Copper";
                                            }
                                            if (SentItemsToPiggyBank && !SoldItems)
                                            {
                                                ResultText = guardian.Name + " stored " + ResultText + " on your Piggy Bank";
                                            }
                                            else if (c == 0 && s == 0 && g == 0 && p == 0)
                                            {
                                                ResultText = guardian.Name + " gained nothing from selling the items";
                                            }
                                            else
                                            {
                                                ResultText = guardian.Name + " got " + ResultText + " Coins from item sale";
                                                if (SendToPiggyBank)
                                                    ResultText += ", and they were sent to your Piggy Bank";
                                            }
                                            if (token > 0)
                                            {
                                                ResultText += ", and stored " + token + " defender medals";
                                            }
                                            Main.NewText(ResultText + ".", Color.Yellow);
                                            if (SendToPiggyBank && guardian.OwnerPos != -1) //Store on player piggy bank
                                            {
                                                Chest bank = Main.player[guardian.OwnerPos].bank;
                                                for (byte Coin = 0; Coin < 5; Coin++)
                                                {
                                                    int EmptySlot = -1;
                                                    int CoinID = Coin + Terraria.ID.ItemID.CopperCoin;
                                                    if (Coin == 4)
                                                        CoinID = Terraria.ID.ItemID.DefenderMedal;
                                                    int CoinsToDiscount = 0;
                                                    switch (Coin)
                                                    {
                                                        case 0:
                                                            CoinsToDiscount = c;
                                                            c = 0;
                                                            break;
                                                        case 1:
                                                            CoinsToDiscount = s;
                                                            s = 0;
                                                            break;
                                                        case 2:
                                                            CoinsToDiscount = g;
                                                            g = 0;
                                                            break;
                                                        case 3:
                                                            CoinsToDiscount = p;
                                                            p = 0;
                                                            break;
                                                        case 4:
                                                            CoinsToDiscount = token;
                                                            token = 0;
                                                            break;
                                                    }
                                                    if (CoinsToDiscount == 0)
                                                        continue;
                                                    for (int i = 0; i < bank.item.Length; i++)
                                                    {
                                                        if (bank.item[i].type == 0)
                                                            EmptySlot = i;
                                                        if (CoinsToDiscount > 0 && bank.item[i].type == CoinID)
                                                        {
                                                            bank.item[i].stack += CoinsToDiscount;
                                                            CoinsToDiscount = 0;
                                                            if (bank.item[i].stack >= 100 && CoinID != Terraria.ID.ItemID.PlatinumCoin && CoinID != Terraria.ID.ItemID.DefenderMedal)
                                                            {
                                                                int NextSum = bank.item[i].stack / 100;
                                                                bank.item[i].stack -= NextSum * 100;
                                                                switch (Coin)
                                                                {
                                                                    case 0:
                                                                        s += NextSum;
                                                                        break;
                                                                    case 1:
                                                                        g += NextSum;
                                                                        break;
                                                                    case 2:
                                                                        p += NextSum;
                                                                        break;
                                                                }
                                                            }
                                                            if (CoinID == Terraria.ID.ItemID.PlatinumCoin && bank.item[i].stack > 1000)
                                                            {
                                                                CoinsToDiscount = bank.item[i].stack - 1000;
                                                                bank.item[i].stack = 1000;
                                                            }
                                                            if (CoinID == Terraria.ID.ItemID.DefenderMedal && bank.item[i].stack > 999)
                                                            {
                                                                CoinsToDiscount = bank.item[i].stack - 999;
                                                                bank.item[i].stack = 999;
                                                            }
                                                            if (bank.item[i].stack == 0)
                                                            {
                                                                bank.item[i].SetDefaults(0);
                                                            }
                                                        }
                                                    }
                                                    while (CoinsToDiscount > 0)
                                                    {
                                                        if (EmptySlot > -1)
                                                        {
                                                            bank.item[EmptySlot].SetDefaults(CoinID);
                                                            if (CoinsToDiscount > 1000)
                                                            {
                                                                bank.item[EmptySlot].stack = 1000;
                                                                CoinsToDiscount -= 1000;
                                                                EmptySlot = -1;
                                                                for (int i = 0; i < bank.item.Length; i++)
                                                                {
                                                                    if (bank.item[i].type == 0)
                                                                        EmptySlot = i;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bank.item[EmptySlot].stack = CoinsToDiscount;
                                                                CoinsToDiscount = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            switch (Coin)
                                                            {
                                                                case 0:
                                                                    c = CoinsToDiscount;
                                                                    break;
                                                                case 1:
                                                                    s = CoinsToDiscount;
                                                                    break;
                                                                case 2:
                                                                    g = CoinsToDiscount;
                                                                    break;
                                                                case 3:
                                                                    p = CoinsToDiscount;
                                                                    break;
                                                                case 4:
                                                                    token = CoinsToDiscount;
                                                                    break;
                                                            }
                                                            CoinsToDiscount = 0;
                                                        }
                                                    }
                                                }
                                            }
                                                for (int i = 0; i < 50; i++)
                                                {
                                                    switch (guardian.Inventory[i].type)
                                                    {
                                                        case Terraria.ID.ItemID.CopperCoin:
                                                            {
                                                                c += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                            }
                                                            break;
                                                        case Terraria.ID.ItemID.SilverCoin:
                                                            {
                                                                s += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                            }
                                                            break;
                                                        case Terraria.ID.ItemID.GoldCoin:
                                                            {
                                                                g += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                            }
                                                            break;
                                                        case Terraria.ID.ItemID.PlatinumCoin:
                                                            {
                                                                p += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                            }
                                                            break;
                                                        case Terraria.ID.ItemID.DefenderMedal:
                                                            {
                                                                token += guardian.Inventory[i].stack;
                                                                guardian.Inventory[i].SetDefaults(0);
                                                            }
                                                            break;
                                                    }
                                                }
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
                                                for (int i = 0; i < 50; i++)
                                                {
                                                    if (guardian.Inventory[i].type == 0)
                                                    {
                                                        if (token > 0)
                                                        {
                                                            guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.DefenderMedal);
                                                            guardian.Inventory[i].stack = token;
                                                            token = 0;
                                                        }
                                                        else if (p > 0)
                                                        {
                                                            guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.PlatinumCoin);
                                                            guardian.Inventory[i].stack = p;
                                                            p = 0;
                                                        }
                                                        else if (g > 0)
                                                        {
                                                            guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.GoldCoin);
                                                            guardian.Inventory[i].stack = g;
                                                            g = 0;
                                                        }
                                                        else if (s > 0)
                                                        {
                                                            guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.SilverCoin);
                                                            guardian.Inventory[i].stack = s;
                                                            s = 0;
                                                        }
                                                        else if (c > 0)
                                                        {
                                                            guardian.Inventory[i].SetDefaults(Terraria.ID.ItemID.CopperCoin);
                                                            guardian.Inventory[i].stack = c;
                                                            c = 0;
                                                        }
                                                    }
                                                }
                                            for (byte Coin = 0; Coin < 5; Coin++)
                                            {
                                                int CoinID = Coin + Terraria.ID.ItemID.CopperCoin;
                                                if (Coin == 4)
                                                    CoinID = Terraria.ID.ItemID.DefenderMedal;
                                                int Stack = 0;
                                                switch (Coin)
                                                {
                                                    case 0:
                                                        Stack = c;
                                                        break;
                                                    case 1:
                                                        Stack = s;
                                                        break;
                                                    case 2:
                                                        Stack = g;
                                                        break;
                                                    case 3:
                                                        Stack = p;
                                                        if (Stack > 1000)
                                                        {
                                                            p -= 1000;
                                                            Stack = 1000;
                                                            Coin--;
                                                        }
                                                        break;
                                                    case 4:
                                                        Stack = token;
                                                        if (Stack > 999)
                                                        {
                                                            token -= 999;
                                                            Stack = 999;
                                                            Coin--;
                                                        }
                                                        break;
                                                }
                                                if (Stack > 0)
                                                    Item.NewItem(guardian.HitBox, CoinID, Stack);
                                            }
                                            ChangeStep();
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        if (GetIntegerValue(TeleportMethod) == 0)
                                        {
                                            if (guardian.ItemAnimationTime <= 0)
                                            {
                                                guardian.UseMagicMirror();
                                            }
                                            if (guardian.MagicMirrorTrigger)
                                            {
                                                InUse = false;
                                                if (GetIntegerValue(PlayerLastMountedCheckVar) == 1)
                                                    guardian.ToggleMount(true);
                                                else if (guardian.OwnerPos > -1)
                                                    guardian.Velocity = Main.player[guardian.OwnerPos].velocity;
                                            }
                                            else
                                            {
                                                TeleportedEffects = true;
                                            }
                                        }
                                        else
                                        {
                                            if (StepStart)
                                            {
                                                SetIntegerValue(TeleportTimeVar, 60 * 5);
                                            }
                                            int Time = GetIntegerValue(TeleportTimeVar);
                                            if (Time > 0)
                                            {
                                                Time--;
                                                TeleportedEffects = true;
                                                SetIntegerValue(TeleportTimeVar, Time);
                                            }
                                            else
                                            {
                                                Main.NewText(guardian.Name + " has returned.");
                                                guardian.StuckTimer = 0;
                                                InUse = false;
                                                if (GetIntegerValue(PlayerLastMountedCheckVar) == 1)
                                                    guardian.ToggleMount(true);
                                                else if (guardian.OwnerPos > -1)
                                                    guardian.Velocity = Main.player[guardian.OwnerPos].velocity;

                                            }
                                        }
                                    }
                                    break;
                            }
                            IgnoreCombat = TeleportedEffects;
                            //AvoidItemUsage = TeleportedEffects;
                            Invisibility = TeleportedEffects;
                            Immune = TeleportedEffects;
                            NoAggro = TeleportedEffects;
                            Inactivity = TeleportedEffects;
                            if (TeleportedEffects)
                            {
                                if (guardian.GuardingPosition.HasValue)
                                {
                                    guardian.Position.X = guardian.GuardingPosition.Value.X * 16;
                                    guardian.Position.Y = guardian.GuardingPosition.Value.Y * 16;
                                }
                                else
                                {
                                    guardian.Position.X = Main.player[guardian.OwnerPos].Center.X;
                                    guardian.Position.Y = Main.player[guardian.OwnerPos].position.Y + Main.player[guardian.OwnerPos].height - 1;
                                }
                                guardian.Velocity.X = guardian.Velocity.Y = 0;
                            }
                        }
                        break;

                    case ActionIDs.ThrowPotion:
                        {
                            const byte PotionPosVar = 0;
                            if (Time >= 30) //Do potion effect, end skill effect
                            {
                                Item i = guardian.Inventory[GetIntegerValue(PotionPosVar)];
                                if (i.type != 0 && i.potion && i.healLife > 0 && !Players[0].dead)
                                {
                                    if (Players[0].HasBuff(Terraria.ID.BuffID.PotionSickness))
                                    {
                                        string[] Messages = new string[] { "Ow!!", "Hey!!", "Ouch!!", "Watch it!", "That hurts!" };
                                        Players[0].chatOverhead.NewMessage(Messages[Main.rand.Next(Messages.Length)], 300);
                                        CombatText.NewText(Players[0].getRect(), CombatText.DamagedFriendly, 8);
                                    }
                                    else
                                    {
                                        int Value = Players[0].GetHealLife(i, true);
                                        Players[0].statLife += Value;
                                        if (Players[0].statLife > Players[0].statLifeMax2)
                                            Players[0].statLife = Players[0].statLifeMax2;
                                        if (Players[0].potionDelay <= 0)
                                        {
                                            Players[0].potionDelay = Players[0].potionDelayTime;
                                            Players[0].AddBuff(21, Players[0].potionDelay);
                                        }
                                        Players[0].HealEffect(Value, true);
                                        i.stack--;
                                        if (i.stack == 0)
                                            i.SetDefaults(0);
                                    }
                                    Main.PlaySound(Terraria.ID.SoundID.Item107, Players[0].Center);
                                }
                                InUse = false;
                                return;
                            }
                            if (Time == 0) //Pick potion, if there is one
                            {
                                if (Players.Count == 0)
                                {
                                    InUse = false;
                                    Main.NewText("No players");
                                    return;
                                }
                                guardian.LookingLeft = Players[0].Center.X < guardian.CenterPosition.X;
                            }
                            else
                            {

                            }
                        }
                        break;
                    case ActionIDs.LaunchPlayer:
                        {
                            if (Players.Count == 0)
                            {
                                InUse = false;
                                return;
                            }
                            if (guardian.SittingOnPlayerMount)
                                guardian.DoSitOnPlayerMount(false);
                            if (guardian.PlayerMounted)
                                guardian.ToggleMount(true);
                            IgnoreCombat = true;
                            guardian.MoveLeft = guardian.MoveRight = false;
                            switch (Step)
                            {
                                case 0:
                                    // guardian.HitBox.Intersects(Players[0].getRect()) && !guardian.BeingPulledByPlayer
                                    if (guardian.ItemAnimationTime == 0 && TryReachingPlayer(guardian, Players[0]))
                                    {
                                        ChangeStep();
                                    }
                                    else
                                    {
                                        if (guardian.furniturex > -1)
                                            guardian.LeaveFurniture();
                                        /*if (Players[0].Center.X < guardian.CenterPosition.X)
                                            guardian.MoveLeft = true;
                                        else
                                            guardian.MoveRight = true;*/
                                    }
                                    break;
                                case 1:
                                    HeldHand hand = HeldHand.Left;
                                    guardian.PickHandToUse(ref hand);
                                    Vector2 HandPosition = Vector2.Zero;
                                    if (hand == HeldHand.Left)
                                    {
                                        HandPosition = guardian.GetGuardianLeftHandPosition;
                                    }
                                    else if (hand == HeldHand.Right)
                                    {
                                        HandPosition = guardian.GetGuardianRightHandPosition;
                                    }
                                    Player p = Players[0];
                                    FocusCameraOnGuardian = true;
                                    if (Time >= 24)
                                    {
                                        InUse = false;
                                    }
                                    else if (Time == 12)
                                    {
                                        p.Center = HandPosition;
                                        p.velocity.X = guardian.Direction * 12.5f;
                                        p.velocity.Y = guardian.GravityDirection * -16.25f;
                                        p.fallStart = (int)p.position.Y / 16;
                                    }
                                    else if (Time < 12)
                                    {
                                        p.direction = guardian.Direction;
                                        p.Center = HandPosition;
                                        p.fallStart = (int)p.position.Y / 16;
                                    }
                                    break;
                            }
                        }
                        break;

                    case ActionIDs.LiftPlayer:
                        {
                            if (Players.Count == 0)
                            {
                                InUse = false;
                                return;
                            }
                            IgnoreCombat = true;
                            AvoidItemUsage = true;
                            Player p = Players[0];
                            if (guardian.SittingOnPlayerMount)
                                guardian.DoSitOnPlayerMount(false);
                            if (guardian.PlayerMounted)
                                guardian.ToggleMount(true);
                            guardian.MoveLeft = guardian.MoveRight = false;
                            switch (Step)
                            {
                                case 0:
                                    if (guardian.furniturex > -1)
                                        guardian.LeaveFurniture();
                                    if (guardian.ItemAnimationTime == 0 && TryReachingPlayer(guardian, p)) //guardian.HitBox.Intersects(p.getRect()) && !guardian.BeingPulledByPlayer && 
                                    {
                                        ChangeStep();
                                        if (p.mount.Active)
                                            p.mount.Dismount(p);
                                    }
                                    else
                                    {
                                        if (Time >= 300)
                                            InUse = false;
                                        /*if (p.Center.X < guardian.CenterPosition.X)
                                            guardian.MoveLeft = true;
                                        else
                                            guardian.MoveRight = true;*/
                                    }
                                    break;
                                case 1:
                                    guardian.Ducking = false;
                                    Vector2 HandPosition = guardian.GetGuardianBetweenHandPosition;
                                    if (Time < 12)
                                    {
                                        p.Center = HandPosition;
                                        p.velocity = Vector2.Zero;
                                        p.velocity.Y = -Player.defaultGravity;
                                        p.fallStart = (int)p.position.Y / 16;
                                        FocusCameraOnGuardian = true;
                                    }
                                    else
                                    {
                                        if (Time == 18 && Collision.SolidCollision(p.position, p.width, p.height))
                                        {
                                            p.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" should've noticed the ceiling was low, before asking " + guardian.Name + " to lift it up..."), 20, 0);
                                            guardian.DisplayEmotion(TerraGuardian.Emotions.Sweat);
                                            if (p.dead)
                                                InUse = false;
                                            ChangeStep();
                                        }
                                        else
                                        {
                                            FocusCameraOnGuardian = false;
                                            p.position.Y = HandPosition.Y - p.height;
                                            p.position.X = HandPosition.X - p.width * 0.5f;
                                            p.velocity.Y = -p.gravity;
                                            p.velocity.X = 0;
                                            p.fallStart = (int)p.position.Y / 16;
                                            if (p.controlRight)
                                            {
                                                guardian.MoveRight = true;
                                            }
                                            if (p.controlLeft)
                                            {
                                                guardian.MoveLeft = true;
                                            }
                                            if (p.controlJump)
                                            {
                                                p.justJumped = true;
                                                p.velocity.Y = -Player.jumpSpeed * p.gravDir;
                                                p.jump = Player.jumpHeight;
                                                InUse = false;
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    FocusCameraOnGuardian = true;
                                    if (Time >= 22)
                                    {
                                        p.position.X = guardian.Position.X - p.width * 0.5f;
                                        p.position.Y = guardian.Position.Y - p.height;
                                        p.fallStart = (int)p.position.Y / 16;
                                        p.velocity = Vector2.Zero;
                                        p.velocity.Y = -Player.defaultGravity;
                                        InUse = false;
                                    }
                                    else
                                    {
                                        p.Center = guardian.GetGuardianBetweenHandPosition;
                                        p.fallStart = (int)p.position.Y / 16;
                                    }
                                    break;
                            }
                        }
                        break;

                    case ActionIDs.ResurrectPlayer:
                        {
                            if (Players.Count == 0)
                            {
                                InUse = false;
                                return;
                            }
                            AvoidItemUsage = true;
                            Player target = Players[0];
                            Vector2 CastingPosition = guardian.CenterPosition;
                            CastingPosition.Y -= guardian.Height;
                            guardian.Ducking = false;
                            float Percentage = (float)Time / 150;
                            if (target.dead)
                            {
                                target.Center += (CastingPosition - target.Center) * Percentage;
                                //target.headPosition += (CastingPosition - target.headPosition) * Percentage;
                                //target.bodyPosition += (CastingPosition - target.bodyPosition) * Percentage;
                                //target.legPosition += (CastingPosition - target.legPosition) * Percentage;
                            }
                            switch (Step)
                            {
                                case 0:
                                    if (Time >= 150) //Res
                                    {
                                        if (Time == 150)
                                        {
                                            if (target.dead || target.ghost)
                                            {
                                                target.ghost = false;
                                                target.respawnTimer = 0;
                                                target.Spawn();
                                                target = Players[0];
                                                //target.statLife = (int)(target.statLifeMax2 * 0.5f);
                                                target.immuneTime *= 2;
                                                //Add cooldown.
                                                Main.NewText(guardian.Name + " has resurrected " + target.name + ".", 0, 255, 0);
                                            }
                                        }
                                        else if (Time == 151)
                                        {
                                            target.velocity.X = 0f;
                                            target.velocity.Y = -7.25f;
                                            target.Center = CastingPosition;
                                            target.fallStart = (int)guardian.Position.Y / 16;
                                            InUse = false;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < 10; i++)
                                        {
                                            int d = Dust.NewDust(CastingPosition, target.width, target.height, 5, 0, 0, 175, default(Color), 1.75f);
                                            Main.dust[d].noGravity = true;
                                            Main.dust[d].velocity *= 0.75f;
                                            int XChange = Main.rand.Next(-40, 41), YChange = Main.rand.Next(-40, 41);
                                            Main.dust[d].position.X += XChange;
                                            Main.dust[d].position.Y += YChange;
                                            Main.dust[d].velocity.X -= XChange * 0.075f;
                                            Main.dust[d].velocity.Y -= YChange * 0.075f;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case ActionIDs.UsePotionOnPlayer:
                        {
                            if (Players.Count == 0)
                            {
                                InUse = false;
                                return;
                            }
                            byte PotionPosVar = 0;
                            Player target = Players[0];
                            if (target.dead)
                            {
                                InUse = false;
                                return;
                            }
                            switch (Step)
                            {
                                case 0: //Find Potion
                                    {
                                        int PotionPos = -1, HighestHealingValue = 0;
                                        for (int i = 0; i < 50; i++)
                                        {
                                            if (guardian.Inventory[i].type != 0 && guardian.Inventory[i].potion && guardian.Inventory[i].healLife > HighestHealingValue)
                                            {
                                                PotionPos = i;
                                                HighestHealingValue = guardian.Inventory[i].healLife;
                                            }
                                        }
                                        if (PotionPos == -1)
                                        {
                                            InUse = false;
                                            return;
                                        }
                                        SetIntegerValue(PotionPosVar, PotionPos);
                                        ChangeStep();
                                    }
                                    break;
                                case 1: //Get the Player
                                    {
                                        if (guardian.furniturex > -1)
                                            guardian.LeaveFurniture();
                                        if (guardian.AttemptToGrabPlayer())
                                        {
                                            ChangeStep();
                                        }
                                    }
                                    break;
                                case 2: //Use potion
                                    {
                                        Item potion = guardian.Inventory[GetIntegerValue(PotionPosVar)];
                                        guardian.ProtectingPlayerFromHarm = true;
                                        if (Time >= potion.useAnimation)
                                        {
                                            int Val = target.GetHealLife(potion);
                                            target.statLife += Val;
                                            target.HealEffect(Val);
                                            if (target.statLife > target.statLifeMax2)
                                                target.statLife = target.statLifeMax2;
                                            guardian.GrabbingPlayer = false;
                                            InUse = false;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;

                    case ActionIDs.UseBuffPotions:
                        if (guardian.ItemAnimationTime <= 0 && guardian.TargetID == -1)
                        {
                            bool AllBuffsUsed = true;
                            for (int i = 0; i < 50; i++)
                            {
                                if (guardian.Inventory[i].buffType > 0 && !Main.vanityPet[guardian.Inventory[i].buffType] && !Main.lightPet[guardian.Inventory[i].buffType] && !Main.projPet[guardian.Inventory[i].buffType] && !guardian.Inventory[i].summon && !guardian.Inventory[i].DD2Summon && !guardian.Inventory[i].sentry && !guardian.HasBuff(guardian.Inventory[i].buffType))
                                {
                                    guardian.SelectedItem = i;
                                    guardian.Action = true;
                                    AllBuffsUsed = false;
                                    break;
                                }
                            }
                            if (AllBuffsUsed)
                                InUse = false;
                        }
                        break;

                    case ActionIDs.UseStatusIncreaseItems:
                        if (guardian.ItemAnimationTime <= 0 && guardian.TargetID < 0)
                        {
                            bool AllItemsUsed = true;
                            for (int i = 0; i < 50; i++)
                            {
                                if ((guardian.Inventory[i].type == Terraria.ID.ItemID.LifeCrystal || guardian.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherHeart>()) && guardian.LifeCrystalHealth < TerraGuardian.MaxLifeCrystals)
                                {
                                    guardian.SelectedItem = i;
                                    guardian.Action = true;
                                    AllItemsUsed = false;
                                    break;
                                }
                                if ((guardian.Inventory[i].type == Terraria.ID.ItemID.LifeFruit || guardian.Inventory[i].type == ModContent.ItemType<Items.Consumable.EtherFruit>()) && guardian.LifeCrystalHealth == TerraGuardian.MaxLifeCrystals && guardian.LifeFruitHealth < TerraGuardian.MaxLifeFruit)
                                {
                                    guardian.SelectedItem = i;
                                    guardian.Action = true;
                                    AllItemsUsed = false;
                                    break;
                                }
                                if (guardian.Inventory[i].type == Terraria.ID.ItemID.ManaCrystal && guardian.ManaCrystals < GuardianData.MaxManaCrystals)
                                {
                                    guardian.SelectedItem = i;
                                    guardian.Action = true;
                                    AllItemsUsed = false;
                                    break;
                                }
                            }
                            if (AllItemsUsed)
                                InUse = false;
                        }
                        break;

                    case ActionIDs.OpenGiftBox:
                        IgnoreCombat = true;
                        AvoidItemUsage = true;
                        switch (Step)
                        {
                            case 0:
                                if (StepStart)
                                {
                                    guardian.LeaveFurniture();
                                    if (Main.rand.NextDouble() < 0.01f)
                                        Main.PlaySound(29, (int)guardian.CenterPosition.X, (int)guardian.CenterPosition.Y, 89);
                                    guardian.DisplayEmotion(TerraGuardian.Emotions.Question);
                                    int ItemPosition = GetIntegerValue(0);
                                    SetIntegerValue(0, guardian.Inventory[ItemPosition].type);
                                    guardian.Inventory[ItemPosition].SetDefaults(0);
                                }
                                if (Time >= 120)
                                    ChangeStep();
                                break;
                            case 1:
                                if (StepStart)
                                    guardian.DisplayEmotion(TerraGuardian.Emotions.Alarmed);
                                if (Time % 5 == 0)
                                {
                                    //Spawn item every 5 ticks;
                                    int ItemID = Terraria.ID.ItemID.CopperCoin, Stack = Main.rand.Next(40, 81);
                                    if (Main.rand.Next(100) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.PlatinumCoin;
                                        Stack = Main.rand.Next(1, 3);
                                    }
                                    else if (Main.rand.Next(25) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.GoldCoin;
                                        Stack = Main.rand.Next(3, 5);
                                    }
                                    else if (Main.rand.Next(5) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.SilverCoin;
                                        Stack = Main.rand.Next(5, 20);
                                    }
                                    else if (Main.rand.Next(5) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.WoodenCrate;
                                        Stack = Main.rand.Next(3, 6);
                                    }
                                    else if (Main.rand.Next(15) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.IronCrate;
                                        Stack = Main.rand.Next(2, 4);
                                    }
                                    else if (Main.rand.Next(25) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.GoldenCrate;
                                        Stack = Main.rand.Next(1, 3);
                                    }
                                    else if (Main.rand.Next(3) == 0)
                                    {
                                        ItemID = Terraria.ID.ItemID.HerbBag;
                                        Stack = 1;
                                    }
                                    Item.NewItem(guardian.CenterPosition, ItemID, Stack);
                                }
                                if (Time >= 120)
                                    ChangeStep();
                                break;
                            case 2:
                                if (StepStart)
                                {
                                    guardian.DisplayEmotion(TerraGuardian.Emotions.Happy);
                                    guardian.IncreaseFriendshipProgress(5);
                                    guardian.Data.GiftGiven = true;
                                    InUse = false;
                                }
                                break;
                        }
                        break;
                }
            }
            if (StepChanged)
                StepChanged = false;
            else
                Time++;
        }

        public void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (!InUse) return;
            if (IsGuardianSpecificAction)
            {
                guardian.Base.GuardianActionUpdateAnimation(guardian, this, ref UsingLeftArmAnimation, ref UsingRightArmAnimation);
                return;
            }
            switch ((ActionIDs)ID)
            {
                case ActionIDs.ReviveSomeone:
                    {
                        if (guardian.Base.DuckingFrame == -1)
                            return;
                        Vector2 TargetPosition = Vector2.Zero;
                        int TargetWidth = 0, TargetHeight = 0;
                        bool IsPlayer = Players.Count > 0;
                        if (Players.Count > 0)
                        {
                            TargetPosition = Players[0].position;
                            TargetWidth = Players[0].width;
                            TargetHeight = Players[0].height;
                        }
                        if (Guardians.Count > 0)
                        {
                            TargetPosition = Guardians[0].TopLeftPosition;
                            TargetWidth = Guardians[0].Width;
                            TargetHeight = Guardians[0].Height;
                        }
                        if (new Rectangle((int)TargetPosition.X, (int)TargetPosition.Y, TargetWidth, TargetHeight).Intersects(guardian.HitBox))
                        {
                            if (guardian.Velocity.X == 0)
                            {
                                if (guardian.BodyAnimationFrame == guardian.Base.StandingFrame)
                                {
                                    int Animation = guardian.Base.StandingFrame;
                                    int ArmAnimation = -1;
                                    if (guardian.Base.ReviveFrame > -1)
                                    {
                                        Animation = guardian.Base.ReviveFrame;
                                    }
                                    else if (guardian.Base.DuckingFrame > -1)
                                    {
                                        Animation = guardian.Base.DuckingFrame;
                                        ArmAnimation = guardian.Base.DuckingSwingFrames[2];
                                    }
                                    if (ArmAnimation == -1)
                                        ArmAnimation = Animation;
                                    guardian.BodyAnimationFrame = Animation;
                                    guardian.RightArmAnimationFrame = guardian.LeftArmAnimationFrame = ArmAnimation;
                                    UsingRightArmAnimation = UsingLeftArmAnimation = true;
                                }
                            }
                        }
                    }
                    break;
                case ActionIDs.TeleportWithPlayerToTown:
                    {
                        if (guardian.PlayerMounted || guardian.SittingOnPlayerMount)
                            break;
                        bool CanPickupPlayer = guardian.Base.MountUnlockLevel != 255 && !guardian.Base.ReverseMount;
                        bool HoldingPlayer = false;
                        HeldHand hand = HeldHand.Right;
                        if ((hand == HeldHand.Right || hand == HeldHand.Both) && guardian.Base.DontUseRightHand)
                            hand = HeldHand.Left;
                        switch (Step)
                        {
                            case 1:
                                {
                                    int Frame = Time / 4;
                                    if (Frame > 1)
                                        Frame = 1;
                                    if (guardian.Ducking)
                                    {
                                        Frame = guardian.Base.DuckingSwingFrames[2 - Frame];
                                    }
                                    else
                                    {
                                        Frame = guardian.Base.ItemUseFrames[3 - Frame];
                                    }
                                    if (hand == HeldHand.Left)
                                    {
                                        guardian.LeftArmAnimationFrame = Frame;
                                        UsingLeftArmAnimation = true;
                                    }
                                    else if (hand == HeldHand.Right)
                                    {
                                        guardian.RightArmAnimationFrame = Frame;
                                        UsingRightArmAnimation = true;
                                    }
                                }
                                break;
                            case 2:
                            case 3:
                                HoldingPlayer = true;
                                break;
                            case 4:
                                if (Time < 8)
                                {
                                    int Frame = 2 - Time / 4;
                                    if (guardian.Ducking)
                                        Frame = guardian.Base.DuckingSwingFrames[2 - Frame];
                                    else
                                        Frame = guardian.Base.ItemUseFrames[3 - Frame];
                                    if (hand == HeldHand.Left)
                                    {
                                        guardian.LeftArmAnimationFrame = Frame;
                                        UsingLeftArmAnimation = true;
                                    }
                                    else if (hand == HeldHand.Right)
                                    {
                                        guardian.RightArmAnimationFrame = Frame;
                                        UsingRightArmAnimation = true;
                                    }
                                }
                                break;
                        }
                        if (HoldingPlayer)
                        {
                            int Frame = 0;
                            if (CanPickupPlayer)
                            {
                                if (guardian.Ducking)
                                    Frame = guardian.Base.DuckingSwingFrames[1];
                                else
                                    Frame = guardian.Base.ItemUseFrames[2];
                            }
                            else
                            {
                                Frame = guardian.Base.ItemUseFrames[2];
                            }
                            if (hand == HeldHand.Left)
                            {
                                guardian.LeftArmAnimationFrame = Frame;
                                UsingLeftArmAnimation = true;
                            }
                            else if (hand == HeldHand.Right)
                            {
                                guardian.RightArmAnimationFrame = Frame;
                                UsingRightArmAnimation = true;
                            }
                        }
                    }
                    break;
                case ActionIDs.MountPutPlayerDown:
                    {
                        if (!guardian.Base.ReverseMount)
                        {
                            int AnimFrame = guardian.Base.ItemUseFrames[0];
                            if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[3];
                            else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[2];
                            else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[1];
                            guardian.RightArmAnimationFrame = AnimFrame;
                            UsingRightArmAnimation = true;
                        }
                    }
                    break;
                case ActionIDs.PickupPlayerMount:
                    {
                        if (Step == 1 && !guardian.Base.ReverseMount)
                        {
                            int AnimFrame = guardian.Base.ItemUseFrames[3];
                            if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[0];
                            else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[1];
                            else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[2];
                            guardian.RightArmAnimationFrame = AnimFrame;
                            UsingRightArmAnimation = true;
                        }
                    }
                    break;
                case ActionIDs.ThrowPotion:
                    if (Time < 15)
                    {
                        HeldHand hand = HeldHand.Left;
                        guardian.PickHandToUse(ref hand);
                        int AnimationFrame = Time / 5;
                        if (AnimationFrame == 0)
                            AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[0] : guardian.Base.ItemUseFrames[0];
                        else if (AnimationFrame == 1)
                        {
                            AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[1] : guardian.Base.ItemUseFrames[1];
                        }
                        else if (AnimationFrame == 2)
                        {
                            AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[2] : guardian.Base.ItemUseFrames[3];
                        }
                        if (hand == HeldHand.Left)
                        {
                            guardian.LeftArmAnimationFrame = AnimationFrame;
                            UsingLeftArmAnimation = true;
                        }
                        else
                        {
                            guardian.RightArmAnimationFrame = AnimationFrame;
                            UsingRightArmAnimation = true;
                        }
                    }
                    break;
                case ActionIDs.LaunchPlayer:
                    {
                        switch (Step)
                        {
                            case 1:
                                HeldHand hand = HeldHand.Left;
                                guardian.PickHandToUse(ref hand);
                                int AnimationFrame = Time / 8;
                                if (AnimationFrame == 0)
                                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[0] : guardian.Base.ItemUseFrames[0];
                                else if (AnimationFrame == 1)
                                {
                                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[1] : guardian.Base.ItemUseFrames[1];
                                }
                                else if (AnimationFrame == 2)
                                {
                                    AnimationFrame = guardian.Ducking ? guardian.Base.DuckingSwingFrames[2] : guardian.Base.ItemUseFrames[3];
                                }
                                if (hand == HeldHand.Left)
                                {
                                    guardian.LeftArmAnimationFrame = AnimationFrame;
                                    UsingLeftArmAnimation = true;
                                }
                                else
                                {
                                    guardian.RightArmAnimationFrame = AnimationFrame;
                                    UsingRightArmAnimation = true;
                                }
                                break;
                        }
                    }
                    break;

                case ActionIDs.LiftPlayer:
                    {
                        switch (Step)
                        {
                            case 1:
                                {
                                    int AnimationFrame = Time / 6;
                                    if (AnimationFrame > 2) AnimationFrame = 2;
                                    if (AnimationFrame == 0)
                                        AnimationFrame = guardian.Base.ItemUseFrames[3];
                                    else if (AnimationFrame == 1)
                                    {
                                        AnimationFrame = guardian.Base.ItemUseFrames[2];
                                    }
                                    else if (AnimationFrame == 2)
                                    {
                                        AnimationFrame = guardian.Base.ItemUseFrames[1];
                                    }
                                    guardian.LeftArmAnimationFrame = AnimationFrame;
                                    guardian.RightArmAnimationFrame = AnimationFrame;
                                    UsingLeftArmAnimation = true;
                                    UsingRightArmAnimation = true;
                                }
                                break;
                            case 2:
                                {
                                    int AnimationFrame = 2 - (Time - 10) / 6;
                                    if (AnimationFrame > 2) AnimationFrame = 2;
                                    if (Time < 10)
                                        AnimationFrame = 2;
                                    if (AnimationFrame == 0)
                                        AnimationFrame = guardian.Base.ItemUseFrames[3];
                                    else if (AnimationFrame == 1)
                                    {
                                        AnimationFrame = guardian.Base.ItemUseFrames[2];
                                    }
                                    else if (AnimationFrame == 2)
                                    {
                                        AnimationFrame = guardian.Base.ItemUseFrames[1];
                                    }
                                    guardian.LeftArmAnimationFrame = AnimationFrame;
                                    guardian.RightArmAnimationFrame = AnimationFrame;
                                    UsingLeftArmAnimation = true;
                                    UsingRightArmAnimation = true;
                                }
                                break;
                        }
                    }
                    break;

                case ActionIDs.ResurrectPlayer:
                    {
                        guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.Base.JumpFrame;
                        UsingLeftArmAnimation = UsingRightArmAnimation = true;
                    }
                    break;

                case ActionIDs.OpenGiftBox:
                    {
                        switch (Step)
                        {
                            case 0:
                            case 1:
                                guardian.LeftArmAnimationFrame = guardian.Base.ItemUseFrames[2];
                                guardian.RightArmAnimationFrame = guardian.Base.ItemUseFrames[2];
                                UsingLeftArmAnimation = UsingRightArmAnimation = true;
                                break;
                        }
                    }
                    break;
            }
        }

        public void Draw(TerraGuardian guardian)
        {
            if (!InUse) return;
            if (IsGuardianSpecificAction)
            {
                guardian.Base.GuardianActionDraw(guardian, this);
                return;
            }
            switch ((ActionIDs)ID)
            {
                case ActionIDs.ThrowPotion:
                    if (Time > 0)
                    {
                        const byte PotionPosVar = 0;
                        Vector2 StartPosition = guardian.CenterPosition,
                            EndPosition = Players[0].Center;
                        float Percentage = (float)Time / 30;
                        Vector2 PotionPosition = StartPosition + (EndPosition - StartPosition) * Percentage;
                        PotionPosition.Y -= UtilityMethods.Bezier(Percentage, 0, 368f, 0);
                        float Rotation = 0.4363323129985824f * Time * guardian.Direction;
                        Microsoft.Xna.Framework.Graphics.Texture2D Texture = Main.itemTexture[guardian.Inventory[GetIntegerValue(PotionPosVar)].type];
                        Main.spriteBatch.Draw(Texture, PotionPosition - Main.screenPosition, null, Color.White, Rotation, new Vector2(Texture.Width * 0.5f, Texture.Height * 0.5f), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                    }
                    break;
                case ActionIDs.UsePotionOnPlayer:
                    if (Step == 2)
                    {
                        const byte PotionPosVar = 0;
                        Vector2 PotionPosition = guardian.GetGuardianLeftHandPosition;
                        Texture2D texture = Main.itemTexture[guardian.Inventory[GetIntegerValue(PotionPosVar)].type];
                        float Rotation = 2.356194490192345f * guardian.Direction;
                        Main.spriteBatch.Draw(texture, PotionPosition - Main.screenPosition, null, Color.White, Rotation, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f),1f, SpriteEffects.None, 0f);
                    }
                    break;
                case ActionIDs.OpenGiftBox:
                    {
                        const byte ItemPosVar = 0;
                        Vector2 BoxPosition = guardian.GetGuardianBetweenHandPosition;
                        Texture2D texture = Main.itemTexture[GetIntegerValue(ItemPosVar)];
                        Main.spriteBatch.Draw(texture, BoxPosition - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                    }
                    break;
            }
        }

        public Vector2 GetHandPosition(TerraGuardian guardian, int AnimationFrame, HeldHand hand)
        {
            Vector2 HandPosition = guardian.Position;
            if ((hand == HeldHand.Right || hand == HeldHand.Both) && guardian.Base.DontUseRightHand)
                hand = HeldHand.Left;
            switch (hand)
            {
                case HeldHand.Left:
                    HandPosition += guardian.GetLeftHandPosition(AnimationFrame);
                    break;
                case HeldHand.Right:
                    HandPosition += guardian.GetRightHandPosition(AnimationFrame);
                    break;
                case HeldHand.Both:
                    HandPosition += guardian.GetBetweenHandsPosition(AnimationFrame);
                    break;
            }
            return HandPosition;
        }

        public void SetPlayerOnHandPosition(TerraGuardian guardian, Player player, int AnimationFrame, HeldHand hand)
        {
            Vector2 HandPosition = GetHandPosition(guardian, AnimationFrame, hand);
            player.position.X = HandPosition.X - player.width * 0.5f;
            player.position.Y = HandPosition.Y - player.height * 0.5f;
            player.velocity.X = 0;
            player.velocity.Y = 0;
            player.fallStart = (int)player.position.Y / 16;
        }

        public void SetPlayerPositionOnGuardianCenter(TerraGuardian guardian, Player player)
        {
            player.position.X = guardian.Position.X - player.width * 0.5f;
            player.position.Y = guardian.Position.Y - player.height - 1;
            player.fallStart = (int)player.position.Y / 16;
        }

        public bool TryReachingPlayer(TerraGuardian guardian, Player player)
        {
            if (guardian.OwnerPos == player.whoAmI && guardian.PlayerMounted)
                return true;
            if (guardian.HitBox.Intersects(player.getRect()) && !guardian.BeingPulledByPlayer && guardian.ItemAnimationTime == 0)
            {
                return true;
            }
            else
            {
                guardian.MoveLeft = guardian.MoveRight = false;
                if (player.Center.X < guardian.CenterPosition.X)
                    guardian.MoveLeft = true;
                else
                    guardian.MoveRight = true;
            }
            return false;
        }

        public enum ActionIDs : byte
        {
            ThrowPotion,
            LaunchPlayer,
            LiftPlayer,
            ResurrectPlayer,
            UsePotionOnPlayer,
            UseBuffPotions,
            UseStatusIncreaseItems,
            OpenGiftBox,
            SellItems,
            JoinPlayerMount,
            PickupPlayerMount,
            MountPutPlayerDown,
            TeleportWithPlayerToTown,
            UseSkillResetPotion,
            ReviveSomeone
        }
    }
}
