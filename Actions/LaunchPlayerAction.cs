using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Actions
{
    public class LaunchPlayerAction : GuardianActions
    {
        public Player Target;

        public LaunchPlayerAction(Player target)
        {
            this.Target = target;
            this.InUse = true;
            ID = (int)ActionIDs.LaunchPlayer;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.SittingOnPlayerMount)
                guardian.DoSitOnPlayerMount(false);
            if (guardian.PlayerMounted)
                guardian.ToggleMount(true);
            IgnoreCombat = true;
            guardian.MoveLeft = guardian.MoveRight = false;
            switch (Step)
            {
                case 0:
                    // guardian.HitBox.Intersects(Target.getRect()) && !guardian.BeingPulledByPlayer
                    if (guardian.ItemAnimationTime == 0 && TryReachingPlayer(guardian, Target))
                    {
                        ChangeStep();
                    }
                    else
                    {
                        if (guardian.furniturex > -1)
                            guardian.LeaveFurniture();
                        /*if (Target.Center.X < guardian.CenterPosition.X)
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
                    FocusCameraOnGuardian = true;
                    if (Time >= 24)
                    {
                        InUse = false;
                    }
                    else if (Time == 12)
                    {
                        Target.Center = HandPosition;
                        Target.velocity.X = guardian.Direction * 12.5f;
                        Target.velocity.Y = guardian.GravityDirection * -16.25f;
                        Target.fallStart = (int)Target.position.Y / 16;
                    }
                    else if (Time < 12)
                    {
                        Target.direction = guardian.Direction;
                        Target.Center = HandPosition;
                        Target.fallStart = (int)Target.position.Y / 16;
                    }
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
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
    }
}
