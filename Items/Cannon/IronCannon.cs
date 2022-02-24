using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Cannon
{
    public class IronCannon : CannonItem
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 26;            //Weapon's texture's width
            item.height = 112;           //Weapon's texture's height
            item.damage = 16;           //The damage of your weapon
            item.knockBack = 6.5f;         //The force of knockback of the weapon. Maximum is 20
            item.rare = 5;              //The rarity of the weapon, from -1 to 13
            item.crit = 19;
            item.useTime = 49;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
            item.useAnimation = 49;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
            item.value = Item.buyPrice(0, 0, 85);           //The value of the weapon
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(ModContent.ItemType<IronCannon>(), 1);
            recipe.requiredItem[0].SetDefaults(Terraria.ID.ItemID.IronBar, true);
            recipe.requiredItem[0].stack = 12;
            recipe.requiredTile[0] = Terraria.ID.TileID.Anvils;
            recipe.anyIronBar = true;
            recipe.AddRecipe();
        }
    }
}
