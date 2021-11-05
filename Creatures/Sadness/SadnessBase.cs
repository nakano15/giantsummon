using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Creatures
{
    public class SadnessBase : PigGuardianFragmentBase
    {
        public SadnessBase() : base(SadnessPigGuardianID)
        {
            Name = "Sadness";
            PossibleNames = new string[] { "Downcast", "Sorrow", "Unfortue", "Misera" };
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very saddened.";
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 72;
            SpriteHeight = 68;
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

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.BluePhaseblade, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.LesserHealingPotion, 10));

            //Animation Frames

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 4);
            LeftHandPoints.AddFramePoint2x(11, 24, 11);
            LeftHandPoints.AddFramePoint2x(12, 26, 19);
            LeftHandPoints.AddFramePoint2x(13, 22, 24);

            LeftHandPoints.AddFramePoint2x(17, 26, 28);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 17, 4);
            RightHandPoints.AddFramePoint2x(11, 27, 11);
            RightHandPoints.AddFramePoint2x(12, 29, 19);
            RightHandPoints.AddFramePoint2x(13, 25, 24);

            RightHandPoints.AddFramePoint2x(17, 28, 28);

            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16 + 2, 11);
            HeadVanityPosition.AddFramePoint2x(14, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(17, 23 + 2, 18);
            HeadVanityPosition.AddFramePoint2x(22, 16 + 2, 9);
            HeadVanityPosition.AddFramePoint2x(25, 23 + 2, 18);
        }
    }
}
