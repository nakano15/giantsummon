using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class BunnyFoot : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases the Guardian Jump Height.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.accessory = true;
			item.width = 38;            //Weapon's texture's width
			item.height = 24;           //Weapon's texture's height
			item.value = Item.buyPrice(0,5);           //The value of the weapon
			item.rare = 1;              //The rarity of the weapon, from -1 to 13
		}

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.MaxJumpHeight *= 2;
        }

        public override void AddRecipes()
        {/*
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.HermesBoots, 1);
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
		}
	}
}
