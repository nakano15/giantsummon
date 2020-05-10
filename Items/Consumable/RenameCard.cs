using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace giantsummon.Items.Consumable
{
    public class RenameCard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows you to change the name of the leader guardian.");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.consumable = true;
            item.width = 18;
            item.height = 18;
            item.useStyle = 4;
            item.useTime = 30;
            item.UseSound = SoundID.Item4;
            item.useAnimation = 30;
            item.rare = 2;
            item.value = 150000;
        }
        
        public override bool UseItem(Player player)
        {
            bool Useable = true;
            TerraGuardian guardian = player.GetModPlayer<PlayerMod>().Guardian;
            if (!guardian.Active)
            {
                Useable = false;
                Main.NewText("Have the guardian you want to rename summoned.");
            }
            else if (guardian.Data.CanChangeName)
            {
                Useable = false;
                Main.NewText("You are still able to name that guardian.");
            }
            if (Useable)
            {
                guardian.Name = null;
                Main.NewText("Name resetted, change it on the guardian selection window.");
            }
            return Useable;
        }
    }
}
