using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class MountPutPlayerDownAction : GuardianActions
    {
        public Player PlayerToPlaceOnFloor;

        public MountPutPlayerDownAction(Player Target)
        {
            ID = (int)ActionIDs.MountPutPlayerDown;
            InUse = true;
            PlayerToPlaceOnFloor = Target;
            BlockOffHandUsage = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (Time == 0 && guardian.PlayerMounted)
            {
                guardian.ToggleMount(false, false);
            }
            if (guardian.ReverseMount || guardian.Base.DontUseRightHand || guardian.UsingFurniture)
            {
                InUse = false;
            }
            else
            {
                FocusCameraOnGuardian = true;
                if (Time >= 20)
                {
                    InUse = false;
                    PlayerToPlaceOnFloor.position.X = guardian.Position.X - PlayerToPlaceOnFloor.width * 0.5f;
                    PlayerToPlaceOnFloor.position.Y = guardian.Position.Y - PlayerToPlaceOnFloor.height;
                    PlayerToPlaceOnFloor.velocity = guardian.Velocity;
                    PlayerToPlaceOnFloor.fallStart = (int)PlayerToPlaceOnFloor.position.Y / 16;
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
                    HandPosition.X -= PlayerToPlaceOnFloor.width * 0.5f;
                    HandPosition.Y -= PlayerToPlaceOnFloor.height * 0.5f;
                    PlayerToPlaceOnFloor.position = HandPosition;
                    PlayerToPlaceOnFloor.fallStart = (int)PlayerToPlaceOnFloor.position.Y / 16;
                    PlayerToPlaceOnFloor.velocity.X = 0;
                    PlayerToPlaceOnFloor.velocity.Y = -Player.defaultGravity;
                    if (PlayerToPlaceOnFloor.itemAnimation == 0)
                    {
                        PlayerToPlaceOnFloor.direction = guardian.Direction;
                    }
                }
            }
            guardian.MoveRight = PlayerToPlaceOnFloor.controlRight;
            guardian.MoveLeft = PlayerToPlaceOnFloor.controlLeft;
            guardian.MoveUp = PlayerToPlaceOnFloor.controlUp;
            guardian.MoveDown = PlayerToPlaceOnFloor.controlDown;
            guardian.Jump = PlayerToPlaceOnFloor.controlJump;
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (!guardian.ReverseMount)
            {
                int AnimFrame = guardian.Base.ItemUseFrames[0];
                if (Time >= 15) AnimFrame = guardian.Base.ItemUseFrames[3];
                else if (Time >= 10) AnimFrame = guardian.Base.ItemUseFrames[2];
                else if (Time >= 5) AnimFrame = guardian.Base.ItemUseFrames[1];
                guardian.RightArmAnimationFrame = AnimFrame;
                UsingRightArmAnimation = true;
            }
        }
    }
}
