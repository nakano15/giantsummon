using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Companions
{
    public class JoyBase : PigGuardianFragmentBase
    {
        public JoyBase() : base(HappinessPigGuardianID)
        {
            Name = "Joy";
            PossibleNames = new string[] { "Cheery", "Fortue", "Glee", "Joy" };
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very scaredy.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 64;
            //FramesInRows = 28;
            //DuckingHeight = 54;
            //Each pig should definitelly have the same size, birthday age and time, so I moved those infos.
            Genderless = true;
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
            DrinksBeverage = false;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.PurplePhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            //Animation Frames

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 2);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 24, 17);
            LeftHandPoints.AddFramePoint2x(13, 20, 22);

            LeftHandPoints.AddFramePoint2x(17, 24, 26);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 2);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 27, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 22);

            RightHandPoints.AddFramePoint2x(17, 26, 26);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);
        }
    }
}
