using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class CinnamonBase : GuardianBase
    {
        public CinnamonBase() //Her recruitment could involve Soup.
        {
            Name = "Cinnamon";
            Description = "";
            Size = GuardianSize.Large;
            Width = 24;
            Height = 68;
            DuckingHeight = 60;
            CompanionSlotWeight = 1.5f;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 59f / 68;
            Age = 13;
            SetBirthday(SEASON_SPRING, 28);
            Male = true;
            InitialMHP = 160; //860
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 20;
            Accuracy = 0.43f;
            Mass = 0.36f;
            MaxSpeed = 4.7f;
            Acceleration = 0.33f;
            SlowDown = 0.22f;
            MaxJumpHeight = 18;
            JumpSpeed = 7.19f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();
            CallUnlockLevel = 4;
            MoveInLevel = 2;
            MountUnlockLevel = 6;

            AddInitialItem(Terraria.ID.ItemID.RedRyder, 1);
            AddInitialItem(Terraria.ID.ItemID.LesserHealingPotion, 5);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 10;
            PlayerMountedArmAnimation = 23;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            DuckingFrame = 18;
            DuckingSwingFrames = new int[] { 20, 21, 22 };
            SittingFrame = 16;
            ChairSittingFrame = 15;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 24;
            BedSleepingFrame = 25;
            SleepingOffset.X = 16;
            ReviveFrame = 19;
            DownedFrame = 17;
            //PetrifiedFrame = 28;

            BackwardStanding = 26;
            BackwardRevive = 27;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);

            RightArmFrontFrameSwap.Add(15, 0);
            RightArmFrontFrameSwap.Add(18, 1);
            RightArmFrontFrameSwap.Add(19, 1);

            //Left Hand
            LeftHandPoints.AddFramePoint2x(11, 16, 4);
            LeftHandPoints.AddFramePoint2x(12, 30, 22);
            LeftHandPoints.AddFramePoint2x(13, 32, 28);
            LeftHandPoints.AddFramePoint2x(14, 29, 34);

            LeftHandPoints.AddFramePoint2x(19, 26, 38);

            LeftHandPoints.AddFramePoint2x(20, 15, 17);
            LeftHandPoints.AddFramePoint2x(21, 29, 25);
            LeftHandPoints.AddFramePoint2x(22, 29, 36);

            //Right Hand
            RightHandPoints.AddFramePoint2x(11, 29, 14);
            RightHandPoints.AddFramePoint2x(12, 32, 22);
            RightHandPoints.AddFramePoint2x(13, 35, 28);
            RightHandPoints.AddFramePoint2x(14, 31, 35);

            RightHandPoints.AddFramePoint2x(20, 26, 17);
            RightHandPoints.AddFramePoint2x(21, 31, 25);
            RightHandPoints.AddFramePoint2x(22, 31, 36);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(18 * 2, 23 * 2);
            MountShoulderPoints.AddFramePoint2x(18, 18, 27);
            MountShoulderPoints.AddFramePoint2x(19, 18, 27);

            //Sitting Position
            SittingPoint = new Point(21 * 2, 39 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(23, 18 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 23, 22 + 2);
            HeadVanityPosition.AddFramePoint2x(19, 23, 22 + 2);

            //Wing Position
            //WingPosition.DefaultCoordinate2x = new Point(20, 23);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (player.HasItem(Terraria.ID.ItemID.BowlofSoup))
            {
                Mes.Add("*(Snif, Snif) Humm.... (Snif, Snif) You have something that smells delicious. Could you share It with me?*");
            }
            Mes.Add("*Oh, hello. Do you like tasty food too?*");
            Mes.Add("*Hi, I love tasty foods. What do you love?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
