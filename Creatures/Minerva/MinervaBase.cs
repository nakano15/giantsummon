using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class MinervaBase : GuardianBase
    {
        public MinervaBase()
        {
            Name = "Minerva";
            Description = "";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 90;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 112;
            Age = 19;
            Male = true;
            InitialMHP = 300; //1000
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 20;
            Accuracy = 0.47f;
            Mass = 0.62f;
            MaxSpeed = 4.8f;
            Acceleration = 0.16f;
            SlowDown = 0.52f;
            MaxJumpHeight = 16;
            JumpSpeed = 7.60f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            PlayerMountedArmAnimation = JumpFrame = 10;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 16;
            ChairSittingFrame = 15;
            //ThroneSittingFrame = 24;
            //BedSleepingFrame = 25;
            //SleepingOffset.X = 16;
            //ReviveFrame = 26;
            //DownedFrame = 27;
            //PetrifiedFrame = 28;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(15, 1);
            RightArmFrontFrameSwap.Add(16, 1);

            SittingPoint2x = new Point(22, 43);

            MountShoulderPoints.DefaultCoordinate2x = new Point(18, 21);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(11, 14, 7);
            LeftHandPoints.AddFramePoint2x(12, 36, 14);
            LeftHandPoints.AddFramePoint2x(13, 41, 26);
            LeftHandPoints.AddFramePoint2x(14, 33, 36);

            //Right Arm
            RightHandPoints.AddFramePoint2x(11, 17, 7);
            RightHandPoints.AddFramePoint2x(12, 38, 14);
            RightHandPoints.AddFramePoint2x(13, 43, 26);
            RightHandPoints.AddFramePoint2x(14, 35, 36);

            //Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(24, 17);
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if (guardian.PlayerMounted && Frame == 0)
                Frame = 1;
        }
    }
}
