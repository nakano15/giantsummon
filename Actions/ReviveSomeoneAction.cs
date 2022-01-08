using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class ReviveSomeoneAction : GuardianActions
    {
        public Player RevivePlayer;
        public TerraGuardian ReviveGuardian;
        public bool TargetIsPlayer = false;
        public int TalkTime = 0, ResTime = 0;

        public ReviveSomeoneAction(Player Target)
        {
            ID = (int)ActionIDs.ReviveSomeone;
            InUse = true;
            RevivePlayer = Target;
            TargetIsPlayer = true;
        }

        public ReviveSomeoneAction(TerraGuardian Target)
        {
            ID = (int)ActionIDs.ReviveSomeone;
            InUse = true;
            ReviveGuardian = Target;
            TargetIsPlayer = false;
        }

        public static bool IsRevivingThisGuardian(TerraGuardian OneBeingRevived, TerraGuardian OneToCheck)
        {
            if(OneToCheck.DoAction.InUse && !OneToCheck.DoAction.IsGuardianSpecificAction && OneToCheck.DoAction.ID == (int)ActionIDs.ReviveSomeone)
            {
                ReviveSomeoneAction action = (ReviveSomeoneAction)OneToCheck.DoAction;
                return action.ReviveGuardian.WhoAmID == OneBeingRevived.WhoAmID;
            }
            return false;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.furniturex > -1)
                guardian.LeaveFurniture(false);
            if (guardian.IsBeingPulledByPlayer)
            {
                InUse = false;
                return;
            }
            if ((guardian.PlayerMounted && !Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().KnockedOut) || guardian.Is2PControlled)
            {
                if (!guardian.MoveDown)
                {
                    InUse = false;
                    return;
                }
            }
            if (guardian.ItemAnimationTime > 0)
                return;
            Vector2 TargetPosition = Vector2.Zero;
            int TargetWidth = 0, TargetHeight = 0;
            bool TryReaching = false;
            bool IsMountedPlayer = false;
            bool AffectedByNegativeHealing = false;
            if (TargetIsPlayer)
            {
                PlayerMod pm = RevivePlayer.GetModPlayer<PlayerMod>();
                AffectedByNegativeHealing = pm.NegativeReviveBoost;
                TargetPosition = RevivePlayer.position;
                TargetWidth = RevivePlayer.width;
                TargetHeight = RevivePlayer.height;
                if (RevivePlayer.dead || !RevivePlayer.active || !pm.KnockedOut)
                {
                    InUse = false;
                    return;
                }
                if (RevivePlayer.whoAmI == guardian.OwnerPos && guardian.PlayerMounted)
                {
                    IsMountedPlayer = true;
                }
            }
            else
            {
                TargetPosition = ReviveGuardian.TopLeftPosition;
                TargetWidth = ReviveGuardian.Width;
                TargetHeight = ReviveGuardian.Height;
                if (!ReviveGuardian.Active || ReviveGuardian.Downed || !ReviveGuardian.KnockedOut)
                {
                    InUse = false;
                    return;
                }
                if (ReviveGuardian.OwnerPos == guardian.OwnerPos && ReviveGuardian.PlayerControl && guardian.PlayerMounted)
                {
                    IsMountedPlayer = true;
                }
            }
            bool RepelingEnemies = false;
            guardian.MoveLeft = guardian.MoveRight = false;
            if (guardian.TargetID > -1 && !AffectedByNegativeHealing)
            {
                Vector2 EnemyPosition;
                int EnemyWidth, EnemyHeight;
                guardian.GetTargetInformation(out EnemyPosition, out EnemyWidth, out EnemyHeight);
                EnemyPosition.X += EnemyWidth * 0.5f;
                EnemyPosition.Y += EnemyHeight * 0.5f;
                if (IsMountedPlayer || (Math.Abs(EnemyPosition.X - guardian.Position.X) < 168f + (EnemyWidth + guardian.Width) * 0.5f &&
                    Math.Abs(EnemyPosition.Y - guardian.CenterY) < 168f + (EnemyHeight + guardian.Height) * 0.5f))
                {
                    RepelingEnemies = true;
                    IgnoreCombat = false;
                    guardian.AttackingTarget = true;
                }
                else
                {
                    IgnoreCombat = true;
                }
            }
            else
            {
                IgnoreCombat = false;
            }
            if (!RepelingEnemies)
            {
                bool OffSetToTheLeft = TargetPosition.X + TargetWidth * 0.5f < guardian.Position.X;
                {
                    int Animation = guardian.Base.StandingFrame;
                    int ArmAnimation = -1;
                    if (IsMountedPlayer)
                    {
                        ArmAnimation = guardian.Base.ItemUseFrames[2];
                    }
                    else
                    {
                        if (guardian.Base.ReviveFrame > -1)
                        {
                            Animation = guardian.Base.ReviveFrame;
                        }
                        else if (guardian.Base.DuckingFrame > -1)
                        {
                            Animation = guardian.Base.DuckingFrame;
                            ArmAnimation = guardian.Base.DuckingSwingFrames[2];
                        }
                    }
                    if (ArmAnimation == -1)
                        ArmAnimation = Animation;
                    int x, y;
                    guardian.Base.GetBetweenHandsPosition(ArmAnimation, out x, out y);
                    float ArmXDistance = (x - guardian.SpriteWidth * 0.5f) * guardian.Scale;
                    if (ArmXDistance > 0)
                    {
                        TargetWidth += (int)ArmXDistance;
                        if (OffSetToTheLeft)
                        {
                            TargetPosition.X -= ArmXDistance;
                        }
                    }
                }
                if (IsMountedPlayer || new Rectangle((int)TargetPosition.X, (int)TargetPosition.Y, TargetWidth, TargetHeight).Intersects(guardian.HitBox))//(MainMod.RectangleIntersects(guardian.TopLeftPosition, guardian.Width, guardian.Height, TargetPosition, TargetWidth, TargetHeight))
                {
                    guardian.Jump = false;
                    float DistanceFromTarget = Math.Abs(guardian.Position.X - (TargetPosition.X + TargetWidth * 0.5f));
                    if (DistanceFromTarget < 8)
                    {
                        if (Math.Abs(guardian.Velocity.X) < 2f)
                        {
                            if (guardian.Position.X < TargetPosition.X + TargetWidth * 0.5f)
                            {
                                guardian.MoveLeft = true;
                            }
                            else
                            {
                                guardian.MoveRight = true;
                            }
                        }
                    }
                    else if (guardian.Velocity.X != 0)
                    {
                        if (Math.Abs(guardian.Position.X + guardian.Velocity.X - (TargetPosition.X + TargetWidth * 0.5f)) < 12)
                        {
                            guardian.MoveLeft = guardian.MoveRight = false;
                            guardian.Velocity.X *= 0.8f;
                        }
                    }
                    else if (guardian.Velocity.X == 0)
                    {
                        {
                            Vector2 FacingLeftPosition = guardian.GetLeftHandPosition(guardian.Base.ReviveFrame, true),
                                    FacingRightPosition = FacingLeftPosition;
                            FacingLeftPosition.X *= -1;
                            FacingLeftPosition.X += guardian.Position.X - (TargetPosition.X + TargetWidth * 0.5f);
                            FacingRightPosition.X += guardian.Position.X - (TargetPosition.X + TargetWidth * 0.5f);
                            guardian.FaceDirection(Math.Abs(FacingLeftPosition.X) < Math.Abs(FacingRightPosition.X));
                        }
                        byte ReviveBoost = 1;
                        if (!guardian.IsAttackingSomething)
                            ReviveBoost += 2;
                        bool IsMounted = guardian.PlayerMounted;
                        if (TargetIsPlayer)
                        {
                            RevivePlayer.GetModPlayer<PlayerMod>().ReviveBoost += ReviveBoost;
                        }
                        else
                        {
                            ReviveGuardian.ReviveBoost += ReviveBoost;
                        }
                        guardian.StuckTimer = 0;
                        guardian.OffHandAction = false;
                        if (TalkTime == 0)
                        {
                            if (MainMod.ReviveTalkDelay <= 0)
                            {
                                if (MainMod.CompanionsSpeaksWhileReviving)
                                {
                                    Main.NewText(guardian.Name + ">>" + (TargetIsPlayer ? RevivePlayer.name : ReviveGuardian.Name) + ": " +
                                        guardian.Base.ReviveMessage(guardian, TargetIsPlayer, (TargetIsPlayer ? RevivePlayer : null), (!TargetIsPlayer ? ReviveGuardian : null)));
                                }
                                TalkTime = (600 + Main.rand.Next(10) * 50) * 2;
                                MainMod.ReviveTalkDelay = 600 + Main.rand.Next(10) * 50;
                            }
                        }
                        else
                        {
                            TalkTime--;
                        }
                    }
                }
                else
                {
                    TryReaching = true;
                }
            }
            if (TryReaching)
            {
                if (ResTime >= 5 * 60)
                {
                    guardian.Position.X = TargetPosition.X + Main.rand.Next(TargetWidth);
                    guardian.Position.Y = TargetPosition.Y + TargetHeight - 1;
                    guardian.FallStart = (int)guardian.Position.Y / 16;
                }
                else if (TargetPosition.X + TargetWidth * 0.5f - guardian.Position.X < 0)
                {
                    guardian.MoveLeft = true;
                }
                else
                {
                    guardian.MoveRight = true;
                }
                guardian.WalkMode = false;
                ResTime++;
            }
            else
            {
                ResTime = 0;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if ((guardian.Base.DuckingFrame == -1 && guardian.Base.ReviveFrame == -1) || guardian.ItemAnimationTime > 0)
                return;
            Vector2 TargetPosition = Vector2.Zero;
            int TargetWidth = 0, TargetHeight = 0;
            bool IsMounted = false, TargetIsBeingCarried = false;
            int TargetLayer = 0;
            if (TargetIsPlayer)
            {
                TargetLayer = RevivePlayer.GetModPlayer<PlayerMod>().MyDrawOrderID;
                TargetPosition = RevivePlayer.position;
                TargetWidth = RevivePlayer.width;
                TargetHeight = RevivePlayer.height;
                TargetIsBeingCarried = RevivePlayer.GetModPlayer<PlayerMod>().BeingCarriedByGuardian;
                if ((guardian.OwnerPos != RevivePlayer.whoAmI || (guardian.OwnerPos == RevivePlayer.whoAmI && !guardian.PlayerMounted)) && RevivePlayer.GetModPlayer<PlayerMod>().MountedOnGuardian)
                    IsMounted = true;
            }
            else
            {
                TargetLayer = ReviveGuardian.MyDrawOrder;
                TargetPosition = ReviveGuardian.TopLeftPosition;
                TargetWidth = ReviveGuardian.Width;
                TargetHeight = ReviveGuardian.Height;
                TargetIsBeingCarried = ReviveGuardian.BeingCarriedByGuardian;
                if (ReviveGuardian.OwnerPos > -1 && ReviveGuardian.PlayerControl && Main.player[ReviveGuardian.OwnerPos].GetModPlayer<PlayerMod>().MountedOnGuardian)
                    IsMounted = true;
            }
            //What to do if the target is above dangerous tiles, like Spikes? How will they rescue that character?
            if (new Rectangle((int)TargetPosition.X, (int)TargetPosition.Y, TargetWidth, TargetHeight).Intersects(guardian.HitBox))
            {
                bool IsStopped = guardian.Velocity.X == 0 || guardian.HasFlag(GuardianFlags.WindPushed);
                if (guardian.Velocity.X == 0 || guardian.HasFlag(GuardianFlags.WindPushed))
                {
                    if (guardian.BodyAnimationFrame == guardian.Base.StandingFrame ||
                        guardian.BodyAnimationFrame == guardian.Base.BackwardStanding)
                    {
                        int Animation = guardian.Base.StandingFrame;
                        int ArmAnimation = -1;
                        if (IsMounted || TargetIsBeingCarried)
                        {
                            ArmAnimation = guardian.Base.ItemUseFrames[2];
                        }
                        else
                        {
                            if (MainMod.ShowBackwardAnimations && TargetLayer < guardian.MyDrawOrder && guardian.Base.BackwardRevive > -1)
                            {
                                Animation = guardian.Base.BackwardRevive;
                            }
                            else if (guardian.Base.ReviveFrame > -1)
                            {
                                Animation = guardian.Base.ReviveFrame;
                            }
                            else if (guardian.Base.DuckingFrame > -1)
                            {
                                Animation = guardian.Base.DuckingFrame;
                                ArmAnimation = guardian.Base.DuckingSwingFrames[2];
                            }
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
    }
}
