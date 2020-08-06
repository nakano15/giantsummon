using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class WrathBase : GuardianBase
    {
        public WrathBase()
            : base()
        {
            Name = "Wrath";
            Description = "One of the emotion pieces fragments\nof a TerraGuardian.";
            Size = GuardianSize.Medium;
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 64;
            FramesInRows = 25;
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

            for (int i = 0; i < 10; i++)
                RightArmFrontFrameSwap.Add(i, i);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);

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
        }
    }
}
