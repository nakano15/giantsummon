using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures.Brandon
{
    public class BrandonBase : GuardianBase
    {
        /// <summary>
        /// -Our antagonist, or is It?
        /// -Zacks old partner.
        /// </summary>
        public BrandonBase()
            : base()
        {
            Name = "Brandon";
            Description = "";
            Age = 21;
            Male = true;
            Accuracy = 0.59f;
            DrinksBeverage = true;
            CanChangeGender = false;
            SetTerrarian();
            CallUnlockLevel = 2;

            TerrarianInfo.HairStyle = 20;
            TerrarianInfo.SkinVariant = 3;
            TerrarianInfo.HairColor = new Microsoft.Xna.Framework.Color(145, 41, 229);
            TerrarianInfo.EyeColor = new Microsoft.Xna.Framework.Color(47, 157, 198);
            TerrarianInfo.SkinColor = new Microsoft.Xna.Framework.Color(255, 159, 133);

            TerrarianInfo.ShirtColor = new Microsoft.Xna.Framework.Color(27, 112, 198);
            TerrarianInfo.UnderShirtColor = new Microsoft.Xna.Framework.Color(234, 67, 157);
            TerrarianInfo.PantsColor = new Microsoft.Xna.Framework.Color(66, 210, 0);
            TerrarianInfo.ShoeColor = new Microsoft.Xna.Framework.Color(160, 105, 60);
        }

        public override string NormalMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("What?");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Terraria.Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("");
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
