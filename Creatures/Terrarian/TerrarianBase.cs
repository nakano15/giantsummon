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
            Accuracy = 0.27f;
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

        public override string GreetMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.GreetMessage(player, guardian);
        }

        public override string NormalMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.NormalMessage(player, guardian);
        }

        public override string TalkMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.TalkMessage(player, guardian);
        }

        public override string NoRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.NoRequestMessage(player, guardian);
        }

        public override string HasRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.HasRequestMessage(player, guardian);
        }

        public override string CompletedRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.CompletedRequestMessage(player, guardian);
        }

        public override string BirthdayMessage(Terraria.Player player, TerraGuardian guardian)
        {
            return base.BirthdayMessage(player, guardian);
        }
    }
}
