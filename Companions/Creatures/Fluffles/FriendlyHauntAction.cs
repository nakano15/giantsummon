using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace giantsummon.Companions.Creatures.Fluffles
{
    public class FriendlyHauntAction : GuardianActions
    {
        private TerraGuardian TargetGuardian;
        private Player TargetPlayer;
        public bool ByPlayerOrder = false;
        private bool LastPlayerFollower = false;
        private int DurationTime = 0;
        private bool Reviving = false;

        public Player GetPlayer { get { return TargetPlayer; } }
        public TerraGuardian GetGuardian { get { return TargetGuardian; } }
        public bool TargetIsPlayer { get { return TargetPlayer != null; } }

        public FriendlyHauntAction(Player Target, bool ByPlayerOrder = false, int Duration = 3* 3600)
        {
            TargetPlayer = Target;
            TargetGuardian = null;
            BlockOffHandUsage = NoAggro = true;
            this.ByPlayerOrder = ByPlayerOrder;
            DurationTime = Duration;
        }

        public FriendlyHauntAction(TerraGuardian Target, bool ByPlayerOrder = false, int Duration = 3 * 3600)
        {
            TargetGuardian = Target;
            TargetPlayer = null;
            BlockOffHandUsage = NoAggro = true;
            this.ByPlayerOrder = ByPlayerOrder;
            DurationTime = Duration;
        }

        public override void Update(TerraGuardian guardian)
        {
            if(TargetPlayer != null)
            {
                if (TargetPlayer.dead || !TargetPlayer.active)
                {
                    InUse = false;
                    return;
                }
            }
            if(TargetGuardian != null)
            {
                if(!TargetGuardian.Active || TargetGuardian.Downed)
                {
                    InUse = false;
                    return;
                }
            }
            switch (Step)
            {
                case 0:
                    {
                        if (StepStart)
                            LastPlayerFollower = guardian.OwnerPos > -1;
                        Rectangle TargetHitbox;
                        Vector2 TargetPosition;
                        if (guardian.UsingFurniture)
                            guardian.LeaveFurniture();
                        if(TargetPlayer != null)
                        {
                            TargetPosition = TargetPlayer.Center;
                            TargetHitbox = TargetPlayer.getRect();
                        }
                        else
                        {
                            TargetPosition = TargetGuardian.CenterPosition;
                            TargetHitbox = TargetGuardian.HitBox;
                        }
                        if(TargetHitbox.Intersects(guardian.HitBox) || Time >= 10 * 60)
                        {
                            ChangeStep();
                            guardian.AddFlag(GuardianFlags.IgnoreGfx);
                        }
                        else if(guardian.Position.X > TargetPosition.X)
                        {
                            guardian.MoveLeft = true;
                            guardian.MoveRight = false;
                        }
                        else
                        {
                            guardian.MoveRight = true;
                            guardian.MoveLeft = false;
                        }
                    }
                    break;

                case 1:
                    {
                        if(guardian.OwnerPos > -1 != LastPlayerFollower)
                        {
                            InUse = false;
                            return;
                        }
                        guardian.IsBeingPulledByPlayer = false;
                        if (!ByPlayerOrder && Time >= DurationTime)
                        {
                            if (TargetPlayer != null)
                            {
                                guardian.Position = TargetPlayer.Bottom;
                            }
                            else
                            {
                                guardian.Position = TargetGuardian.Position;
                            }
                            InUse = false;
                            return;
                        }
                        guardian.MoveLeft = guardian.MoveRight = guardian.Jump = false;
                        guardian.ChangeIdleAction(TerraGuardian.IdleActions.Wait, 300);
                        Vector2 MountedPosition = guardian.Base.GetBetweenHandsPositionVector(Reviving ? guardian.Base.ReviveFrame : guardian.Base.PlayerMountedArmAnimation);
                        MountedPosition.X = MountedPosition.X - guardian.Base.SpriteWidth * 0.5f;
                        Vector2 HauntPosition = Vector2.Zero;
                        if (TargetPlayer != null)
                        {
                            if (Reviving)
                            {
                                if (guardian.ItemAnimationTime == 0)
                                    guardian.Direction = -TargetPlayer.direction;
                                HauntPosition = TargetPlayer.Center;
                            }
                            else
                            {
                                if (guardian.ItemAnimationTime == 0)
                                    guardian.Direction = TargetPlayer.direction;
                                HauntPosition = TargetPlayer.position;
                            }
                            if (guardian.Direction > 0)
                                MountedPosition.X *= -1;
                            HauntPosition.X += TargetPlayer.width * 0.5f;
                            HauntPosition.Y += TargetPlayer.height + (guardian.Base.SpriteHeight - MountedPosition.Y - 30) * guardian.Scale;
                            HauntPosition.X += (MountedPosition.X - 6 * guardian.Direction) * guardian.Scale;
                            guardian.AddDrawMomentToPlayer(TargetPlayer);
                            TargetPlayer.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.GhostFoxHaunts.FriendlyHaunt>(), 5);
                            Reviving = TargetPlayer.GetModPlayer<PlayerMod>().KnockedOut;
                        }
                        else
                        {
                            if (Reviving)
                            {
                                if (guardian.ItemAnimationTime == 0)
                                    guardian.Direction = -TargetGuardian.Direction;
                                HauntPosition = TargetGuardian.CenterPosition;
                            }
                            else
                            {
                                if (guardian.ItemAnimationTime == 0)
                                    guardian.Direction = TargetGuardian.Direction;
                                HauntPosition = TargetGuardian.Position;
                            }
                            if (guardian.Direction > 0)
                                MountedPosition.X *= -1;
                            HauntPosition.X += MountedPosition.X * guardian.Scale - TargetGuardian.Width * 0.2f * guardian.Direction;
                            //HauntPosition.X += (MountedPosition.X - (TargetGuardian.Width * 0.5f - 8) * guardian.Direction) * guardian.Scale; //- 8
                            HauntPosition.Y += MountedPosition.Y * guardian.Scale - TargetGuardian.Height * 0.95f;
                            guardian.AddDrawMomentToTerraGuardian(TargetGuardian);
                            TargetGuardian.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.GhostFoxHaunts.FriendlyHaunt>(), 5, true);
                            Reviving = TargetGuardian.KnockedOut || TargetGuardian.IsUsingBed;
                        }
                        guardian.Velocity = Vector2.Zero;
                        guardian.Position = HauntPosition;
                    }
                    break;
            }
            LastPlayerFollower = guardian.OwnerPos > -1;
        }

        public override void OnActionEnd(TerraGuardian guardian)
        {
            if (TargetPlayer != null)
            {
                guardian.Position = TargetPlayer.Bottom;
            }
            else
            {
                guardian.Position = TargetGuardian.Position;
            }
            guardian.RemoveFlag(GuardianFlags.IgnoreGfx);
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Step == 1)
            {
                int Animation = Reviving ? guardian.Base.ReviveFrame : guardian.Base.PlayerMountedArmAnimation;
                guardian.BodyAnimationFrame = Animation;
                if (!UsingLeftArmAnimation) guardian.LeftArmAnimationFrame = Animation;
                if (!UsingRightArmAnimation) guardian.RightArmAnimationFrame = Animation;
            }
        }
    }
}
