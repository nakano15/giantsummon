using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Companions
{
    public class CastellaBase : GuardianBase
    {
        public CastellaBase()
        {
            Name = "Castella";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 88;
            DuckingHeight = 56;
            SpriteWidth = 128;
            SpriteHeight = 96;
            Scale = 102f / 96;
            CompanionSlotWeight = 1.1f;
            Age = 36;
            SetBirthday(SEASON_SUMMER, 28);
            DefaultTactic = CombatTactic.Charge;
            Male = false;
            InitialMHP = 250; //1150
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 15;
            Accuracy = 0.36f;
            Mass = 0.5f;
            MaxSpeed = 5.3f;
            Acceleration = 0.21f;
            SlowDown = 0.47f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.81f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();

            //Animations
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
           // HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 23;
            DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 18;
            ChairSittingFrame = 17;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 20;
            BedSleepingFrame = 19;
            //SleepingOffset.X = 32;
            ReviveFrame = 21;
            DownedFrame = 22;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(32, 36);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);

            BodyFrontFrameSwap.Add(43, 1);
            BodyFrontFrameSwap.Add(44, 1);

            RightArmFrontFrameSwap.Add(17, 0);
            RightArmFrontFrameSwap.Add(43, 1);

            //Left Hand Positions
            LeftHandPoints.AddFramePoint2x(10, 21, 4);
            LeftHandPoints.AddFramePoint2x(11, 41, 11);
            LeftHandPoints.AddFramePoint2x(12, 46, 23);
            LeftHandPoints.AddFramePoint2x(13, 40, 31);

            LeftHandPoints.AddFramePoint2x(14, 14, 9);
            LeftHandPoints.AddFramePoint2x(15, 46, 7);
            LeftHandPoints.AddFramePoint2x(16, 58, 31);

            LeftHandPoints.AddFramePoint2x(18, 42, 26);

            LeftHandPoints.AddFramePoint2x(21, 40, 37);

            LeftHandPoints.AddFramePoint2x(24, 32, 21);
            LeftHandPoints.AddFramePoint2x(25, 50, 27);
            LeftHandPoints.AddFramePoint2x(26, 45, 41);

            LeftHandPoints.AddFramePoint2x(39, 32, 4);
            LeftHandPoints.AddFramePoint2x(40, 39, 12);
            LeftHandPoints.AddFramePoint2x(41, 43, 20);
            LeftHandPoints.AddFramePoint2x(42, 38, 28);

            LeftHandPoints.AddFramePoint2x(43, 38, 27);
            LeftHandPoints.AddFramePoint2x(44, 38, 27);

            LeftHandPoints.AddFramePoint2x(48, 35, 26);

            LeftHandPoints.AddFramePoint2x(50, 45, 17);
            LeftHandPoints.AddFramePoint2x(51, 52, 35);
            LeftHandPoints.AddFramePoint2x(52, 42, 41);

            LeftHandPoints.AddFramePoint2x(62, 39, 12);

            //Right Hand Positions
            RightHandPoints.AddFramePoint2x(10, 32, 4);
            RightHandPoints.AddFramePoint2x(11, 44, 11);
            RightHandPoints.AddFramePoint2x(12, 48, 23);
            RightHandPoints.AddFramePoint2x(13, 42, 31);

            RightHandPoints.AddFramePoint2x(14, 16, 9);
            RightHandPoints.AddFramePoint2x(15, 48, 7);
            RightHandPoints.AddFramePoint2x(16, 59, 32);

            RightHandPoints.AddFramePoint2x(18, 45, 27);

            RightHandPoints.AddFramePoint2x(21, 44, 37);

            RightHandPoints.AddFramePoint2x(24, 34, 21);
            RightHandPoints.AddFramePoint2x(25, 52, 27);
            RightHandPoints.AddFramePoint2x(26, 47, 42);

            RightHandPoints.AddFramePoint2x(39, 37, 4);
            RightHandPoints.AddFramePoint2x(40, 42, 12);
            RightHandPoints.AddFramePoint2x(41, 47, 20);
            RightHandPoints.AddFramePoint2x(42, 42, 28);

            RightHandPoints.AddFramePoint2x(44, 44, 27);

            RightHandPoints.AddFramePoint2x(48, 49, 36);

            RightHandPoints.AddFramePoint2x(50, 49, 17);
            RightHandPoints.AddFramePoint2x(51, 55, 36);
            RightHandPoints.AddFramePoint2x(52, 48, 41);

            RightHandPoints.AddFramePoint2x(62, 42, 12);
        }

        public override void Attributes(TerraGuardian g) //Add transformation action, and replace frames based on her form.
        {
            g.AddFlag(GuardianFlags.WerewolfAcc);
        }

        public class CastellaData : GuardianData
        {
            public bool LastWereform = false;

            public CastellaData(int ID, string ModID) : base(ID, ModID)
            {

            }
        }
    }
}
