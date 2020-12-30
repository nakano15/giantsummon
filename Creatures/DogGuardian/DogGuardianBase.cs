using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class DogGuardianBase : GuardianBase
    {
        private const int SleuthAnimationID = 27, SleuthBackAnimationID = 28;

        public DogGuardianBase()
        {
            Name = "DogGuardian";
            Description = "";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 86;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            FramesInRows = 20;
            Age = 19;
            Male = true;
            CalculateHealthToGive(1200, 0.9f, 0.6f); //Lc: 95, LF: 16
            Accuracy = 0.72f;
            Mass = 0.7f;
            MaxSpeed = 4.9f;
            Acceleration = 0.14f;
            SlowDown = 0.42f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = PlayerMountedArmAnimation = 9;
            HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 23;
            DuckingSwingFrames = new int[] { 24, 25, 26 };
            SittingFrame = 17;
            ChairSittingFrame = 18;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 20;
            BedSleepingFrame = 19;
            DownedFrame = 21;
            ReviveFrame = 22;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);
            BodyFrontFrameSwap.Add(27, 1);
            BodyFrontFrameSwap.Add(28, 2);

            SleepingOffset.X = 16;
            
            SittingPoint = new Microsoft.Xna.Framework.Point(22 * 2, 35 * 2);

            //Mounted Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(17, 14);
            MountShoulderPoints.AddFramePoint2x(15, 23, 19);
            MountShoulderPoints.AddFramePoint2x(16, 29, 26);

            MountShoulderPoints.AddFramePoint2x(17, 25, 13);
            MountShoulderPoints.AddFramePoint2x(18, 25, 13);

            MountShoulderPoints.AddFramePoint2x(20, 16, 21);

            MountShoulderPoints.AddFramePoint2x(22, 24, 24);
            MountShoulderPoints.AddFramePoint2x(23, 24, 24);
            MountShoulderPoints.AddFramePoint2x(24, 24, 24);
            MountShoulderPoints.AddFramePoint2x(25, 24, 24);
            MountShoulderPoints.AddFramePoint2x(26, 24, 24);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 2);
            LeftHandPoints.AddFramePoint2x(11, 34, 9);
            LeftHandPoints.AddFramePoint2x(12, 38, 21);
            LeftHandPoints.AddFramePoint2x(13, 33, 29);

            LeftHandPoints.AddFramePoint2x(14, 34, 2);
            LeftHandPoints.AddFramePoint2x(15, 41, 25);
            LeftHandPoints.AddFramePoint2x(16, 38, 41);

            LeftHandPoints.AddFramePoint2x(22, 39, 37);

            LeftHandPoints.AddFramePoint2x(24, 39, 18);
            LeftHandPoints.AddFramePoint2x(25, 43, 30);
            LeftHandPoints.AddFramePoint2x(26, 35, 40);
            
            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 23, 2);
            RightHandPoints.AddFramePoint2x(11, 38, 9);
            RightHandPoints.AddFramePoint2x(12, 41, 21);
            RightHandPoints.AddFramePoint2x(13, 36, 29);

            RightHandPoints.AddFramePoint2x(14, 36, 2);
            RightHandPoints.AddFramePoint2x(15, 43, 25);
            RightHandPoints.AddFramePoint2x(16, 40, 41);

            RightHandPoints.AddFramePoint2x(22, 44, 37);

            RightHandPoints.AddFramePoint2x(24, 41, 18);
            RightHandPoints.AddFramePoint2x(25, 45, 30);
            RightHandPoints.AddFramePoint2x(26, 37, 40);
            
            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(23, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(15, 29, 15 + 2);
            HeadVanityPosition.AddFramePoint2x(16, 36, 25 + 2);

            HeadVanityPosition.AddFramePoint2x(17, 21, 8 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 21, 8 + 2);

            HeadVanityPosition.AddFramePoint2x(22, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(23, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(24, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(25, 31, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(26, 31, 21 + 2);
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Ist nicht allein, [nickname].*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
