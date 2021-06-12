using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Outfit.Bree
{
    public class DamselOutfit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlocks Damsel Outfit when placed on Bree's inventory.");
        }

        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 5);
            item.maxStack = 1;
            item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}
