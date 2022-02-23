using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Companions.Brutus
{
    public class ProtectModeAction : GuardianActions
    {
        public const int ProtectModeAutoTriggerTime = 3600;
        public byte Action = 0;
        public bool PlayerWasKnockedOut = false;
        public int DelayBeforePlacingOnTheGround = 0;

        public ProtectModeAction(bool PlayerKod)
        {
            PlayerWasKnockedOut = PlayerKod;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.OwnerPos == -1)
            {
                InUse = false;
                return;
            }
            if (!PlayerWasKnockedOut && guardian.AfkCounter < 60)
            {
                InUse = false;
                return;
            }
            if (guardian.UsingFurniture)
                guardian.LeaveFurniture(false);
            Player defended = Main.player[guardian.OwnerPos];
            PlayerMod defendedPm = defended.GetModPlayer<PlayerMod>();
            /*if (guardian.GrabbingPlayer)
            {
                guardian.PlayerCanEscapeGrab = true;
                DelayBeforePlacingOnTheGround--;
                if (guardian.Velocity.X != 0 || guardian.Velocity.Y != 0)
                    DelayBeforePlacingOnTheGround = 5 * 60;
                return;
            }
            {
                int XStart = (int)(defended.position.X * (1f / 16)), XEnd = (int)((defended.position.X + defended.width) * (1f / 16));
                int YCheck = (int)(defended.position.Y * (1f / 16));
                bool TryPickingUpPlayer = false;
                for (int x = XStart; x < XEnd; x++)
                {
                    if(MainMod.IsDangerousTile(x, YCheck, defended.fireWalk))
                    {
                        TryPickingUpPlayer = true;
                        break;
                    }
                }
                if (TryPickingUpPlayer)
                {
                    DelayBeforePlacingOnTheGround = 5 * 60;
                    if(TryReachingPlayer(guardian, defended))
                    {
                        guardian.AttemptToGrabPlayer();
                    }
                    return;
                }
            }*/
            const int Offset = 7 * 2;
            float DefendX = defended.Center.X - Offset * defended.direction;
            if (Action == 0)
            {
                if (guardian.PlayerMounted || guardian.SittingOnPlayerMount)
                {
                    Action = 1;
                }
                else
                {
                    guardian.MoveLeft = guardian.MoveRight = false;
                    if (guardian.Position.X + guardian.Velocity.X * 0.5f > DefendX)
                        guardian.MoveLeft = true;
                    else
                        guardian.MoveRight = true;
                    if (Math.Abs(guardian.Position.X - DefendX) < 5 && Math.Abs(guardian.Velocity.X) < 3f)
                    {
                        Action = 1;
                        if (defendedPm.ControllingGuardian)
                        {
                            if (defendedPm.Guardian.ModID == MainMod.mod.Name)
                            {
                                switch (defendedPm.Guardian.ID)
                                {
                                    case GuardianBase.Domino:
                                        {
                                            switch (Main.rand.Next(5))
                                            {
                                                case 0:
                                                    guardian.SaySomething("*I'm only protecting him because of you, [nickname].*");
                                                    break;
                                                case 1:
                                                    guardian.SaySomething("*Of anyone you could take direct control, It had to be him?*");
                                                    break;
                                                case 2:
                                                    guardian.SaySomething("*Is this some kind of karma?*");
                                                    break;
                                                case 3:
                                                    guardian.SaySomething("*Okay, what have I done to you, [nickname]?*");
                                                    break;
                                                case 4:
                                                    guardian.SaySomething("*It's funny how I'm treating his wounds, instead of beating his face.*");
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            else if (Action == 1)
            {
                if (!guardian.PlayerMounted && !guardian.SittingOnPlayerMount)
                {
                    guardian.Position.X = DefendX;
                    if (!guardian.IsAttackingSomething)
                    {
                        guardian.LookingLeft = defended.direction == -1;
                    }
                }
                guardian.Jump = false;
                if (!guardian.SittingOnPlayerMount)
                    guardian.MoveDown = true;
                guardian.OffHandAction = true;
                PlayerMod pm = defended.GetModPlayer<PlayerMod>();
                if (pm.ControllingGuardian)
                {
                    pm.Guardian.AddBuff(ModContent.BuffType<Buffs.Defended>(), 3, true);
                    if (pm.Guardian.KnockedOut)
                        pm.Guardian.ReviveBoost++;
                    else
                    {
                        if (guardian.AfkCounter < ProtectModeAutoTriggerTime && PlayerWasKnockedOut)
                            InUse = false;
                    }
                }
                else
                {
                    defended.AddBuff(ModContent.BuffType<Buffs.Defended>(), 3);
                    if (pm.KnockedOut)
                        pm.ReviveBoost++;
                    else
                    {
                        if (guardian.AfkCounter < ProtectModeAutoTriggerTime && PlayerWasKnockedOut)
                            InUse = false;
                    }
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Action == 1 && guardian.SittingOnPlayerMount && !UsingRightArmAnimation)
            {
                guardian.RightArmAnimationFrame = 1;
            }
        }
    }
}
