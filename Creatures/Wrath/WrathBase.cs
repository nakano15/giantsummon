using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class WrathBase : PigGuardianFragmentBase
    {
        public WrathBase() //I'll need to think how I'll make the cloud form of them work, and toggle.
            : base(AngerPigGuardianID)
        {
            Name = "Wrath";
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very volatile.";
            Size = GuardianSize.Medium;
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 68;
            FramesInRows = 28;
            //DuckingHeight = 54;
            Age = 11;
            Male = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            //Animation Frames
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
            ReviveFrame = 18;

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
            RightArmFrontFrameSwap.Add(9, 0);
            RightArmFrontFrameSwap.Add(10, 0);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(22, 1);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 3);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 23, 17);
            LeftHandPoints.AddFramePoint2x(13, 22, 20);

            LeftHandPoints.AddFramePoint2x(17, 24, 26);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 3);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 26, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 20);

            RightHandPoints.AddFramePoint2x(17, 26, 26);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.Defense -= 4;
            if (GetIfIsCloudForm(g))
            {
                //if (!g.HasFlag(GuardianFlags.NoGravity))
                //    g.AddFlag(GuardianFlags.NoGravity);
                //if (!g.HasFlag(GuardianFlags.NoTileCollision))
                //    g.AddFlag(GuardianFlags.NoTileCollision);
            }
        }
    }
}
