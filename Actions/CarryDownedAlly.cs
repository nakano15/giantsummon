﻿using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Actions
{
    public class CarryDownedAlly : GuardianActions
    {
        public Player CarriedPlayer;
        public TerraGuardian CarriedGuardian;
        public bool Carrying = false;
        private ushort DelayBeforePlacingOnGround = 0;
        private const ushort MaxDelay = 7 * 60;

        public CarryDownedAlly(Player player)
        {
            ID = (int)ActionIDs.CarryDownedAlly;
            InUse = true;
            CarriedPlayer = player;
        }

        public CarryDownedAlly(TerraGuardian tg)
        {
            ID = (int)ActionIDs.CarryDownedAlly;
            InUse = true;
            CarriedGuardian = tg;
        }

        public static bool CanCarryAlly(TerraGuardian Carrier, Player player)
        {
            return (player.height < Carrier.Height * 0.95f);
        }

        public static bool CanCarryAlly(TerraGuardian Carrier, TerraGuardian guardian)
        {
            return (guardian.Height < Carrier.Height * 0.95f);
        }

        public override void Update(TerraGuardian guardian)
        {
            bool TargetIsKod = true;
            if(CarriedPlayer != null)
            {
                TargetIsKod = CarriedPlayer.GetModPlayer<PlayerMod>().KnockedOut;
                if(CarriedPlayer.dead)
                {
                    InUse = false;
                    return;
                }
                if (!PlayerMod.IsBeingCarriedBySomeone(CarriedPlayer) || PlayerMod.IsBeingCarriedByThisGuardian(CarriedPlayer, guardian))
                {
                    CarriedPlayer.GetModPlayer<PlayerMod>().CarriedByGuardianID = guardian.WhoAmID;
                }
                else
                {
                    InUse = false;
                    return;
                }
            }
            else
            {
                TargetIsKod = CarriedGuardian.KnockedOut;
                if(CarriedGuardian.Downed)
                {
                    InUse = false;
                    return;
                }
                if(!CarriedGuardian.IsBeingCarriedBySomeone() || CarriedGuardian.IsBeingCarriedByThisGuardian(guardian))
                {
                    CarriedGuardian.CarriedByGuardianID = guardian.WhoAmID;
                }
                else
                {
                    InUse = false;
                    return;
                }
            }
            if (!Carrying)
            {
                float TargetX = CarriedPlayer != null ? CarriedPlayer.Center.X : CarriedGuardian.Position.X;
                float TargetY = CarriedPlayer != null ? CarriedPlayer.Bottom.Y : CarriedGuardian.Position.Y;
                if (Math.Abs(guardian.Position.X - TargetX) < 16 && Math.Abs(guardian.Position.Y - TargetY) < guardian.Height * 0.5f)
                {
                    Carrying = true;
                    ChangeStep();
                }
                else
                {
                    IgnoreCombat = true;
                    guardian.MoveRight = guardian.MoveLeft = false;
                    if (TargetX < guardian.Position.X)
                        guardian.MoveLeft = true;
                    else
                        guardian.MoveRight = true;
                    if(Time >= 5 * 60)
                    {
                        guardian.Position = new Microsoft.Xna.Framework.Vector2(TargetX, TargetY);
                        guardian.SetFallStart();
                        Carrying = true;
                        ChangeStep();
                    }
                    else
                    {
                        return;
                    }
                }
                DelayBeforePlacingOnGround = MaxDelay;
            }
            IgnoreCombat = false;
            bool SafeToPlaceAllyDown = true;
            {
                if(guardian.Velocity.Y == 0)
                {
                    int StartCheckX = (int)((guardian.Position.X - guardian.CollisionWidth * 0.5f) * TerraGuardian.DivisionBy16),
                        EndCheckX = (int)((guardian.Position.X + guardian.CollisionWidth * 0.5f + 1) * TerraGuardian.DivisionBy16);
                    int CheckY = (int)((guardian.Position.Y + 1) * TerraGuardian.DivisionBy16);
                    for (int x = StartCheckX; x < EndCheckX; x++)
                    {
                        if (MainMod.IsDangerousTile(x, CheckY, false))
                        {
                            SafeToPlaceAllyDown = false;
                            break;
                        }
                    }
                }
                else
                {
                    SafeToPlaceAllyDown = false;
                }
                if (!TargetIsKod && DelayBeforePlacingOnGround > 2.5f * 60)
                    DelayBeforePlacingOnGround = (int)(2.5f * 60);
                if (SafeToPlaceAllyDown && TargetIsKod)
                {
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && guardian.IsNpcHostile(Main.npc[n]) && guardian.Distance(Main.npc[n].Center) < 400 && (Main.npc[n].noTileCollide || Collision.CanHitLine(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, guardian.TopLeftPosition, guardian.Width, guardian.Height)))
                        {
                            SafeToPlaceAllyDown = false;
                            break;
                        }
                    }
                }
            }
            if (SafeToPlaceAllyDown)
            {
                if (DelayBeforePlacingOnGround <= 0)
                {
                    if (CarriedPlayer != null)
                    {
                        CarriedPlayer.position.X = guardian.Position.X - CarriedPlayer.width * 0.5f;
                        CarriedPlayer.position.Y = guardian.Position.Y - CarriedPlayer.height;
                        InUse = false;
                        return;
                    }
                    CarriedGuardian.Position.X = guardian.Position.X;
                    CarriedGuardian.Position.Y = guardian.Position.Y;
                    InUse = false;
                    return;
                }
                DelayBeforePlacingOnGround--;
            }
            else
            {
                DelayBeforePlacingOnGround = MaxDelay;
                bool AllyIsDying = CarriedPlayer != null ? CarriedPlayer.statLife < CarriedPlayer.statLifeMax2 * 0.35f : CarriedGuardian.HP < CarriedGuardian.MHP * 0.35f;
                ForcedTactic = AllyIsDying ? CombatTactic.Snipe : CombatTactic.Assist;
            }
            {
                Vector2 CarryPosition = Vector2.Zero;
                Vector2 Origin = Vector2.One * 0.5f;
                if (guardian.Ducking)
                {
                    CarryPosition = guardian.GetBetweenHandsPosition(guardian.Base.DuckingSwingFrames[2]);
                }
                else
                {
                    CarryPosition = guardian.GetBetweenHandsPosition(guardian.Base.ItemUseFrames[2]);
                }
                CarryPosition += guardian.Position;
                if(CarriedPlayer != null)
                {
                    CarriedPlayer.position.X = CarryPosition.X - CarriedPlayer.width * Origin.X;
                    CarriedPlayer.position.Y = CarryPosition.Y - CarriedPlayer.height * Origin.X;
                    CarriedPlayer.fallStart = (int)(CarriedPlayer.position.Y * TerraGuardian.DivisionBy16);
                    CarriedPlayer.direction = guardian.Direction;
                    CarriedPlayer.GetModPlayer<PlayerMod>().ReviveBoost++;
                    MainMod.DrawMoment.Add(new GuardianDrawMoment(guardian.WhoAmID, TerraGuardian.TargetTypes.Player, CarriedPlayer.whoAmI));
                }
                else
                {
                    CarriedGuardian.BeingPulledByPlayer = false;
                    CarriedGuardian.Position.X = CarryPosition.X - (CarriedGuardian.Width * (Origin.X - 0.5f));
                    CarriedGuardian.Position.Y = CarryPosition.Y + (CarriedGuardian.Height * (0.5f - Origin.Y));
                    CarriedGuardian.SetFallStart();
                    CarriedGuardian.Direction = guardian.Direction;
                    CarriedGuardian.ReviveBoost++;
                    MainMod.DrawMoment.Add(new GuardianDrawMoment(guardian.WhoAmID, TerraGuardian.TargetTypes.Guardian, CarriedGuardian.WhoAmID, true));
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Carrying)
            {
                int ArmFrame = 0;
                if (guardian.Ducking)
                {
                    ArmFrame = guardian.Base.DuckingSwingFrames[2];
                }
                else
                {
                    ArmFrame = guardian.Base.ItemUseFrames[2];
                }
                if (!UsingLeftArmAnimation)
                    guardian.LeftArmAnimationFrame = ArmFrame;
                if (!UsingRightArmAnimation)
                    guardian.RightArmAnimationFrame = ArmFrame;
            }
        }
    }
}