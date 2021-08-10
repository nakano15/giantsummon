using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class PigGuardianFragmentBase : GuardianBase
    {
        public int CloudFormStanding = 19,
            CloudFormMoveForward = 20,
            CloudFormMoveBackward = 21,
            CloudFormChairSit = 22,
            CloudFormThroneSit = 23,
            CloudFormKO = 24,
            CloudFormRevive = 25,
            CloudFormBedSleep = 26,
            CloudFormBackwardStanding = 29,
            CloudFormBackwardRevive = 30;
        public byte PigID = 0;
        public const byte AngerPigGuardianID = 0, SadnessPigGuardianID = 1, HappinessPigGuardianID = 2, FearPigGuardianID = 3, BlandPigGuardianID = 4;

        public PigGuardianFragmentBase(byte PigID)
        {
            this.PigID = PigID;
            Size = GuardianSize.Medium;
            Age = 15;
            CompanionSlotWeight = 0.6f;
            SetBirthday(SEASON_AUTUMN, 14);

            //Same animations and settings for all!
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 14;
            ChairSittingFrame = 14;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 15 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 18;
            DownedFrame = 15;
            ReviveFrame = 17;

            BackwardStanding = 27;
            BackwardRevive = 28;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(15, 24);

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(2, 1);
            RightArmFrontFrameSwap.Add(3, 2);
            RightArmFrontFrameSwap.Add(4, 2);
            RightArmFrontFrameSwap.Add(5, 1);
            RightArmFrontFrameSwap.Add(6, 0);
            RightArmFrontFrameSwap.Add(7, 0);
            RightArmFrontFrameSwap.Add(8, 0);
            //RightArmFrontFrameSwap.Add(9, 0);
            //RightArmFrontFrameSwap.Add(10, 0);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(16, 1);
            BodyFrontFrameSwap.Add(22, 2);
            BodyFrontFrameSwap.Add(23, 3);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 4);
            LeftHandPoints.AddFramePoint2x(11, 22, 11);
            LeftHandPoints.AddFramePoint2x(12, 24, 19);
            LeftHandPoints.AddFramePoint2x(13, 20, 24);

            LeftHandPoints.AddFramePoint2x(17, 24, 28);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 16, 4);
            RightHandPoints.AddFramePoint2x(11, 26, 11);
            RightHandPoints.AddFramePoint2x(12, 28, 19);
            RightHandPoints.AddFramePoint2x(13, 24, 24);

            RightHandPoints.AddFramePoint2x(17, 26, 28);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23, 18);

            HeadVanityPosition.AddFramePoint2x(23, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(24, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(26, -1000, -1000);
        }
        
        public bool GetIfIsCloudForm(TerraGuardian guardian)
        {
            //return true; //Remove if you don't want to test It.
            if (guardian.OwnerPos > -1)
            {
                return Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PigID];
            }
            else
            {
                return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PigID];
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            bool CloudForm = GetIfIsCloudForm(guardian);
            if (CloudForm)
            {
                if ((AnimationID == 1 && TerraGuardian.UsingLeftArmAnimation) || (AnimationID == 2 && TerraGuardian.UsingRightArmAnimation))
                {
                    return;
                }
                if (Frame == BackwardRevive)
                {
                    Frame = CloudFormBackwardRevive;
                }
                else if (Frame == BackwardStanding)
                {
                    Frame = CloudFormBackwardStanding;
                }
                else if (guardian.SittingOnPlayerMount)
                {
                    Frame = CloudFormChairSit;
                }
                else if (Frame == DownedFrame)
                {
                    Frame = CloudFormKO;
                }
                else if (Frame == ReviveFrame)
                {
                    Frame = CloudFormRevive;
                }
                else if (guardian.UsingFurniture)
                {
                    if (Frame == ChairSittingFrame || Frame == SittingFrame)
                    {
                        Frame = CloudFormChairSit;
                    }
                    else if (Frame == ThroneSittingFrame)
                    {
                        Frame = CloudFormThroneSit;
                    }
                    else if (Frame == BedSleepingFrame)
                    {
                        Frame = CloudFormBedSleep;
                    }
                }
                else if (guardian.Velocity.X != 0 && guardian.Velocity.Y == 0)
                {
                    if ((guardian.LookingLeft && guardian.Velocity.X < 0) || (!guardian.LookingLeft && guardian.Velocity.X > 0))
                    {
                        Frame = 20;
                    }
                    else
                    {
                        Frame = 21;
                    }
                }
                else
                {
                    if (guardian.Velocity.Y != 0)
                    {
                        if ((AnimationID == 1 && !TerraGuardian.UsingLeftArmAnimation) || (AnimationID == 2 && !TerraGuardian.UsingRightArmAnimation))
                        {
                            Frame = JumpFrame;
                        }
                        else
                        {
                            Frame = 20;
                        }
                    }
                    else
                        Frame = 19;
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Microsoft.Xna.Framework.Vector2 DrawPosition, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color armorColor, float Rotation, Microsoft.Xna.Framework.Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            const float Opacity = 0.8f;
            if (GetIfIsCloudForm(guardian))
            {
                foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                {
                    if (!TerraGuardian.DrawingIgnoringLighting && 
                        gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings &&
                        gdd.textureType != GuardianDrawData.TextureType.TGHeadAccessory)
                    {
                        gdd.color *= Opacity;
                    }
                }
                foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                {
                    if (!TerraGuardian.DrawingIgnoringLighting &&
                        gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings &&
                        gdd.textureType != GuardianDrawData.TextureType.TGHeadAccessory)
                    {
                        gdd.color *= Opacity;
                    }
                }
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (GetIfIsCloudForm(guardian))
            {
                if (guardian.Velocity.Y >= 2.3f)
                {
                    guardian.Velocity.Y = 2.3f;
                    guardian.SetFallStart();
                }
                else
                {
                    //guardian.Velocity.Y += 0.05f;
                }
                if (!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0 && guardian.BodyAnimationFrame != CloudFormRevive)
                    guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 5;
            }
        }
    }
}
