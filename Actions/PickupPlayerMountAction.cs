using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class PickupPlayerMountAction : GuardianActions
    {
        public Player PlayerToPickup;

        public PickupPlayerMountAction(Player Target)
        {
            ID = (int)ActionIDs.PickupPlayerMount;
            PlayerToPickup = Target;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (Step == 0) //Try reaching player
            {
                if (guardian.PlayerMounted)
                {
                    InUse = false;
                    return;
                }
                IgnoreCombat = true;
                if (!guardian.IsBeingPulledByPlayer)
                {
                    if (TryReachingPlayer(guardian, PlayerToPickup))
                    {
                        ChangeStep();
                    }
                    else
                    {
                        if (guardian.furniturex > -1)
                            guardian.LeaveFurniture();
                    }
                }
            }
            else //Pickup Player animation.
            {
                BlockOffHandUsage = true;
                //guardian.PlayerMounted = true;
                if (guardian.ReverseMount || guardian.Base.DontUseRightHand || guardian.UsingFurniture)
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
                        if (PlayerToPickup.mount.Active)
                            PlayerToPickup.mount.Dismount(PlayerToPickup);
                        int AnimFrame = guardian.Base.ItemUseFrames[3];
                        if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[0];
                        else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[1];
                        else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[2];
                        int HPosX, HPosY;
                        guardian.GetRightHandPosition(AnimFrame, out HPosX, out HPosY);
                        Vector2 HandPosition = guardian.Position;
                        HandPosition.X += HPosX;
                        HandPosition.Y += HPosY;
                        HandPosition.X -= PlayerToPickup.width * 0.5f;
                        HandPosition.Y -= PlayerToPickup.height * 0.5f;
                        PlayerToPickup.position = HandPosition;
                        PlayerToPickup.fallStart = (int)PlayerToPickup.position.Y / 16;
                        PlayerToPickup.velocity.X = 0;
                        PlayerToPickup.velocity.Y = -Player.defaultGravity;
                        if (PlayerToPickup.itemAnimation == 0)
                        {
                            PlayerToPickup.direction = guardian.Direction;
                        }
                    }
                }
                guardian.MoveRight = PlayerToPickup.controlRight;
                guardian.MoveLeft = PlayerToPickup.controlLeft;
                guardian.MoveUp = PlayerToPickup.controlUp;
                guardian.MoveDown = PlayerToPickup.controlDown;
                guardian.Jump = PlayerToPickup.controlJump;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Step == 1 && !guardian.ReverseMount)
            {
                int AnimFrame = guardian.Base.ItemUseFrames[3];
                if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[0];
                else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[1];
                else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[2];
                guardian.RightArmAnimationFrame = AnimFrame;
                UsingRightArmAnimation = true;
            }
        }
    }
}
