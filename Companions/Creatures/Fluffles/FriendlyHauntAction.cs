using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace giantsummon.Companions.Creatures.Fluffles
{
    public class FriendlyHauntAction : GuardianActions
    {
        private TerraGuardian TargetGuardian;
        private Player TargetPlayer;

        public FriendlyHauntAction(Player Target)
        {
            TargetPlayer = Target;
            TargetGuardian = null;
            BlockOffHandUsage = NoAggro = true;
        }

        public FriendlyHauntAction(TerraGuardian Target)
        {
            TargetGuardian = Target;
            TargetPlayer = null;
            BlockOffHandUsage = NoAggro = true;
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
                        Vector2 TargetPosition;
                        if(TargetPlayer != null)
                        {
                            TargetPosition = TargetPlayer.Center;
                        }
                        else
                        {
                            TargetPosition = TargetGuardian.CenterPosition;
                        }
                        if((Math.Abs(TargetPosition.X - guardian.Position.X) < 16 && Math.Abs(TargetPosition.Y - 24 - guardian.Position.Y) < 16) || Time >= 10 * 60)
                        {
                            ChangeStep();
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
                        if (Time >= 3 * 3600)
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
                        Vector2 MountedPosition = guardian.Base.LeftHandPoints.GetPositionFromFrameVector(guardian.Base.PlayerMountedArmAnimation);
                        MountedPosition.X = MountedPosition.X - guardian.Width * 0.5f;
                        Vector2 HauntPosition = Vector2.Zero;
                        if(TargetPlayer != null)
                        {
                            if (guardian.ItemAnimationTime == 0)
                                guardian.Direction = TargetPlayer.direction;
                            if (guardian.Direction > 0)
                                MountedPosition.X *= -1;
                            HauntPosition = TargetPlayer.position;
                            HauntPosition.X += TargetPlayer.width * 0.5f - 6 * guardian.Direction;
                            HauntPosition.Y += TargetPlayer.height + (guardian.Base.SpriteHeight - MountedPosition.Y - 30) * guardian.Scale;
                            HauntPosition.X += MountedPosition.X * guardian.Scale;
                        }
                        else
                        {
                            if (guardian.ItemAnimationTime == 0)
                                guardian.Direction = TargetGuardian.Direction;
                            if (TargetGuardian.Direction > 0)
                                MountedPosition.X *= -1;
                            HauntPosition = TargetGuardian.Position;
                            HauntPosition.X += MountedPosition.X + TargetGuardian.Width * 0.2f * guardian.Direction;
                            HauntPosition.Y += MountedPosition.Y - TargetGuardian.Height * 0.8f;

                        }
                        guardian.Position = HauntPosition;
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Step == 1)
            {
                int Animation = guardian.Base.PlayerMountedArmAnimation;
                if(TargetGuardian != null)
                {
                    if (TargetGuardian.IsUsingBed)
                        Animation = guardian.Base.ReviveFrame;
                }
                guardian.BodyAnimationFrame = Animation;
                if (!UsingLeftArmAnimation) guardian.LeftArmAnimationFrame = Animation;
                if (!UsingRightArmAnimation) guardian.RightArmAnimationFrame = Animation;
            }
        }
    }
}
