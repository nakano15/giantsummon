using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Creatures
{
    public class TerrarianBase : GuardianBase
    {
        public TerrarianBase()
        {
            Name = "TerrarianGirl";
            Description = ".";
            Age = 16;
            Male = false;
            InitialMHP = 100; //500
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 10;
            Accuracy = 0.32f;
            Mass = 0.3f;
            MaxSpeed = 3f;
            Acceleration = 0.08f;
            SlowDown = 0.2f;
            MaxJumpHeight = 15;
            JumpSpeed = 5.01f;
            DrinksBeverage = false;
            CanChangeGender = false;
            SetTerrarian();
            CallUnlockLevel = 0;

            TerrarianInfo.HairStyle = 21;
            TerrarianInfo.SkinVariant = 1;
            TerrarianInfo.HairColor = new Microsoft.Xna.Framework.Color(55, 215, 255);
            TerrarianInfo.EyeColor = new Microsoft.Xna.Framework.Color(196, 10, 227);
            TerrarianInfo.SkinColor = new Microsoft.Xna.Framework.Color(237, 118, 85);

            TerrarianInfo.ShirtColor = new Microsoft.Xna.Framework.Color(248, 28, 28);
            TerrarianInfo.UnderShirtColor = new Microsoft.Xna.Framework.Color(30, 249, 20);
            TerrarianInfo.PantsColor = new Microsoft.Xna.Framework.Color(179, 36, 245);
            TerrarianInfo.ShoeColor = new Microsoft.Xna.Framework.Color(206, 29, 29);

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);
        }
    }
}
