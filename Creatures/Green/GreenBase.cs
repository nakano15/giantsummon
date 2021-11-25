using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Creatures
{
    public class GreenBase : GuardianBase
    {
        public GreenBase()
        {
            Name = "Green";
            Description = "Treated many TerraGuardians in the Ether Realm,\nhis newest challenge now is on the Terra Realm.";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 86;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 89f / 86;
            CompanionSlotWeight = 1.15f;
            Age = 31;
            SetBirthday(SEASON_SPRING, 4);
            Male = true;
            InitialMHP = 170; //895
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 25;
            Accuracy = 0.6f;
            Mass = 0.36f;
            MaxSpeed = 5.15f;
            Acceleration = 0.21f;
            SlowDown = 0.39f;
            //MaxJumpHeight = 15;
            //JumpSpeed = 7.08f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            DontUseHeavyWeapons = true;
            SetTerraGuardian();
            CallUnlockLevel = 0;

            PopularityContestsWon = 2;
            ContestSecondPlace = 3;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.FlintlockPistol, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);
            AddInitialItem(Terraria.ID.ItemID.MeteorShot, 50);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 15;
            ChairSittingFrame = 14;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 17;
            BedSleepingFrame = 16;
            SleepingOffset.X = 16;
            ReviveFrame = 18;
            DownedFrame = 19;

            BackwardStanding = 20;
            BackwardRevive = 21;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(15, 1);

            RightArmFrontFrameSwap.Add(14, 0);
            RightArmFrontFrameSwap.Add(15, 1);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(19, 40);

            //Mount
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(18, 15);
            MountShoulderPoints.AddFramePoint2x(14, 17, 20);
            MountShoulderPoints.AddFramePoint2x(17, 17, 15);
            MountShoulderPoints.AddFramePoint2x(18, 29, 21);
            MountShoulderPoints.AddFramePoint2x(21, 29, 21);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 3);
            LeftHandPoints.AddFramePoint2x(11, 33, 11);
            LeftHandPoints.AddFramePoint2x(12, 37, 21);
            LeftHandPoints.AddFramePoint2x(13, 30, 29);

            LeftHandPoints.AddFramePoint2x(15, 29, 31);

            LeftHandPoints.AddFramePoint2x(18, 37, 41);

            LeftHandPoints.AddFramePoint2x(18, 41, 41);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 25, 3);
            RightHandPoints.AddFramePoint2x(11, 37, 11);
            RightHandPoints.AddFramePoint2x(12, 39, 21);
            RightHandPoints.AddFramePoint2x(13, 33, 29);

            RightHandPoints.AddFramePoint2x(18, 41, 41);

            RightHandPoints.AddFramePoint2x(18, 37, 41);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(24, 11);
        }
    }
}
