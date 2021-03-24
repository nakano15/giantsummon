using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class CaitSithSonBase : GuardianBase
    {
        public CaitSithSonBase()
        {
            Name = "CaitSithSon";
            Description = "";
            Size = GuardianSize.Small;
            Width = 14;
            Height = 38;
            SpriteWidth = 64;
            SpriteHeight = 56;
            FramesInRows = 25;
            //DuckingHeight = 54;
            Age = 14; //3
            Male = true;
            InitialMHP = 70; //280
            LifeCrystalHPBonus = 10;
            LifeFruitHPBonus = 3;
            Accuracy = 0.88f;
            Mass = 0.28f;
            MaxSpeed = 4.80f;
            Acceleration = 0.18f;
            SlowDown = 0.9f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.52f;
            DontUseHeavyWeapons = true;
            CanDuck = false;
            ReverseMount = true;
            SetTerraGuardian();
            GroupID = TerraGuardianCaitSithGroupID;
            DodgeRate = 60;
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit51);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath54);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.SilverBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.Shuriken, 250);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = PlayerMountedArmAnimation = 15;
            ChairSittingFrame = 14;
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            DownedFrame = 19;
            ReviveFrame = 18;
            //PetrifiedFrame = 23;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(17, 21);
            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);
            BodyFrontFrameSwap.Add(15, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 14, 8);
            LeftHandPoints.AddFramePoint2x(11, 20, 13);
            LeftHandPoints.AddFramePoint2x(12, 22, 17);
            LeftHandPoints.AddFramePoint2x(13, 20, 22);

            LeftHandPoints.AddFramePoint2x(15, 18, 16);

            LeftHandPoints.AddFramePoint2x(18, 17, 22);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 18, 8);
            RightHandPoints.AddFramePoint2x(11, 22, 13);
            RightHandPoints.AddFramePoint2x(12, 24, 17);
            RightHandPoints.AddFramePoint2x(13, 22, 22);

            RightHandPoints.AddFramePoint2x(15, 21, 16);

            RightHandPoints.AddFramePoint2x(18, 20, 22);

            //Head Vanity
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16, 12);
            HeadVanityPosition.AddFramePoint2x(14, 16, 10);
            HeadVanityPosition.AddFramePoint2x(15, 16, 10);
            HeadVanityPosition.AddFramePoint2x(16, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(18, 16, 14);
            HeadVanityPosition.AddFramePoint2x(19, 22, 23);
        }
    }
}
