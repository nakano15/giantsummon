using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class FirstSymbol : ModItem
    {
        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 22;            //Weapon's texture's width
            item.height = 24;           //Weapon's texture's height
            item.value = Item.sellPrice(0, 15);           //The value of the weapon
            item.rare = 1;              //The rarity of the weapon, from -1 to 13
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases Guardians status based on Summon Damage.");
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PlayerMod>().HasFirstSymbol = true;
        }
    }
}
