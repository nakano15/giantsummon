using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Creatures.SnakeGuardian
{
    public class SnakeGuardianBase : GuardianBase
    {
        public SnakeGuardianBase()
        {
            Name = "SnakeGuardian";
            Description = "Treated many TerraGuardians in the\nEther Realm, his new challenge now is on the Terra Realm.";
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
            BodyFrontFrameSwap.Add(23, 0);
        }
    }
}
