using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class LiftPlayerAction : GuardianActions
    {
        public Player Target;
        public TerraGuardian TargetTg;

        public LiftPlayerAction(Player target)
        {
            ID = (int)ActionIDs.LiftPlayer;
            Target = target;
            TargetTg = null;
        }

        public LiftPlayerAction(TerraGuardian target)
        {
            ID = (int)ActionIDs.LiftPlayer;
            Target = null;
            TargetTg = target;
        }

        public override void Update(TerraGuardian guardian)
        {
            IgnoreCombat = true;
            AvoidItemUsage = true;
            if (guardian.SittingOnPlayerMount)
                guardian.DoSitOnPlayerMount(false);
            if (guardian.PlayerMounted)
                guardian.ToggleMount(true);
            if (guardian.IsBeingPulledByPlayer)
                guardian.IsBeingPulledByPlayer = false;
            guardian.MoveLeft = guardian.MoveRight = false;
            switch (Step)
            {
                case 0:
                    if (guardian.furniturex > -1)
                        guardian.LeaveFurniture();
                    if (guardian.ItemAnimationTime == 0 && ((Target != null && TryReachingPlayer(guardian, Target)) || (TargetTg != null && TryReachingTg(guardian, TargetTg)))) //guardian.HitBox.Intersects(p.getRect()) && !guardian.BeingPulledByPlayer && 
                    {
                        ChangeStep();
                        if (Target != null && Target.mount.Active)
                            Target.mount.Dismount(Target);
                    }
                    else
                    {
                        if (Time >= 300)
                            InUse = false;
                    }
                    break;
                case 1:
                    guardian.Ducking = false;
                    Vector2 HandPosition = guardian.GetGuardianBetweenHandPosition;
                    BlockOffHandUsage = true;
                    if (Time < 12)
                    {
                        if (Target != null)
                        {
                            Target.Center = HandPosition;
                            Target.velocity = Vector2.Zero;
                            Target.velocity.Y = -Player.defaultGravity;
                            Target.fallStart = (int)Target.position.Y / 16;
                        }
                        else
                        {
                            TargetTg.Position = HandPosition + new Vector2(0, TargetTg.Height * 0.5f);
                            TargetTg.Velocity = Vector2.Zero;
                            TargetTg.SetFallStart();
                        }
                        FocusCameraOnGuardian = true;
                    }
                    else
                    {
                        if (Time == 18 && Target != null && Collision.SolidCollision(Target.position, Target.width, Target.height))
                        {
                            Target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" should've noticed the ceiling was low, before asking " + guardian.Name + " to lift it up..."), 20, 0);
                            guardian.DisplayEmotion(TerraGuardian.Emotions.Sweat);
                            if (Target.dead || Target.GetModPlayer<PlayerMod>().KnockedOut)
                                InUse = false;
                            ChangeStep();
                        }
                        else if (Time == 18 && TargetTg != null && Collision.SolidCollision(TargetTg.TopLeftPosition, TargetTg.Width, TargetTg.Height))
                        {
                            TargetTg.Hurt(20, 0, DeathMessage: " should've noticed the ceiling was low, before asking " + guardian.Name + " to lift it up...");
                            guardian.DisplayEmotion(TerraGuardian.Emotions.Sweat);
                            if (TargetTg.Downed || TargetTg.KnockedOut)
                                InUse = false;
                            ChangeStep();
                        }
                        else
                        {
                            FocusCameraOnGuardian = false;
                            if (Target != null)
                            {
                                Target.position.Y = HandPosition.Y - Target.height;
                                Target.position.X = HandPosition.X - Target.width * 0.5f;
                                Target.velocity.Y = -Target.gravity;
                                Target.velocity.X = 0;
                                Target.fallStart = (int)Target.position.Y / 16;
                                if (Target.controlRight)
                                {
                                    guardian.MoveRight = true;
                                }
                                if (Target.controlLeft)
                                {
                                    guardian.MoveLeft = true;
                                }
                                if (Target.controlJump)
                                {
                                    Target.justJumped = true;
                                    Target.velocity.Y = -Player.jumpSpeed * Target.gravDir;
                                    Target.jump = Player.jumpHeight;
                                    InUse = false;
                                }
                            }
                            else
                            {
                                TargetTg.Position = HandPosition;
                                TargetTg.Velocity.Y = -0.3f;
                                TargetTg.Velocity.X = 0;
                                TargetTg.SetFallStart();
                                if (TargetTg.LastMoveRight)
                                    guardian.MoveRight = true;
                                if (TargetTg.LastMoveLeft)
                                    guardian.MoveLeft = true;
                                if(TargetTg.LastJump)
                                {
                                    TargetTg.Velocity.Y = -TargetTg.JumpSpeed * TargetTg.GravityDirection;
                                    InUse = false;
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    FocusCameraOnGuardian = true;
                    if (Time >= 22)
                    {
                        if (Target != null)
                        {
                            Target.position.X = guardian.Position.X - Target.width * 0.5f;
                            Target.position.Y = guardian.Position.Y - Target.height;
                            Target.fallStart = (int)Target.position.Y / 16;
                            Target.velocity = Vector2.Zero;
                            Target.velocity.Y = -Player.defaultGravity;
                            InUse = false;
                        }
                        else
                        {
                            TargetTg.Position = guardian.Position;
                            TargetTg.Velocity.Y = 0;
                            TargetTg.Velocity.X = 0;
                            TargetTg.SetFallStart();
                        }
                    }
                    else
                    {
                        if (Target != null)
                        {
                            Target.Center = guardian.GetGuardianBetweenHandPosition;
                            Target.fallStart = (int)Target.position.Y / 16;
                        }
                        else
                        {
                            TargetTg.Position = guardian.GetGuardianBetweenHandPosition + new Vector2(0, TargetTg.Height * 0.5f);
                            TargetTg.SetFallStart();
                        }
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
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
    }
}
