using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Actions
{
    public class TeleportWithPlayerToTownAction : GuardianActions
    {
        public Player Target;

        public TeleportWithPlayerToTownAction(Player target)
        {
            ID = (int)ActionIDs.TeleportWithPlayerToTown;
            InUse = true;
            Target = target;
        }

        public override void Update(TerraGuardian guardian)
        {
            bool CanPickupPlayer = guardian.Base.MountUnlockLevel != 255 && guardian.Base.Size >= GuardianBase.GuardianSize.Large && !guardian.PlayerMounted && !guardian.SittingOnPlayerMount;
            AvoidItemUsage = true;
            bool HoldingPlayer = false;
            FocusCameraOnGuardian = false;
            /*if (Target.dead)
            {
                InUse = false;
                return;
            }*/
            switch (Step)
            {
                case 0:
                    if (guardian.furniturex > -1)
                        guardian.LeaveFurniture();
                    if (TryReachingPlayer(guardian, Target))
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
                                SetPlayerOnHandPosition(guardian, Target, guardian.Base.DuckingSwingFrames[2 - AnimFrame], HeldHand.Right);
                            }
                            else
                            {
                                SetPlayerOnHandPosition(guardian, Target, guardian.Base.ItemUseFrames[3 - AnimFrame], HeldHand.Right);
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
                            Player player = Target;
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
                                    SetPlayerOnHandPosition(guardian, Target, guardian.Base.DuckingSwingFrames[2 - AnimFrame], HeldHand.Right);
                                }
                                else
                                {
                                    int AnimFrame = 2 - Time / 4;
                                    SetPlayerOnHandPosition(guardian, Target, guardian.Base.ItemUseFrames[3 - AnimFrame], HeldHand.Right);
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
                        SetPlayerPositionOnGuardianCenter(guardian, Target);
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
                        SetPlayerOnHandPosition(guardian, Target, guardian.Base.DuckingSwingFrames[1], HeldHand.Right);
                    }
                    else
                    {
                        SetPlayerOnHandPosition(guardian, Target, guardian.Base.ItemUseFrames[2], HeldHand.Right);
                    }
                    Target.velocity.Y = -Player.defaultGravity;
                    guardian.ProtectingPlayerFromHarm = true;
                }
                else if (!guardian.PlayerMounted && !guardian.SittingOnPlayerMount)
                {
                    /*Vector2 HandPosition = GetHandPosition(guardian, guardian.Base.ItemUseFrames[2], HeldHand.Right);
                    float HorizontalPosChange = Target.Center.X - HandPosition.X;
                    if (!guardian.PlayerMounted && !guardian.SittingOnPlayerMount && Math.Abs(HorizontalPosChange) >= Target.width * 0.5f)
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

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (guardian.PlayerMounted || guardian.SittingOnPlayerMount)
                return;
            bool CanPickupPlayer = guardian.Base.MountUnlockLevel != 255 && !guardian.ReverseMount;
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
    }
}
