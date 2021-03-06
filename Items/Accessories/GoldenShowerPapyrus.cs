using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class GoldenShowerPapyrus : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Teaches your Guardian a stance for increasing efficiently the Golden Shower book.\nIncreases Golden Shower damage by 20%.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.accessory = true;
			item.width = 48;            //Weapon's texture's width
			item.height = 76;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
		}

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.GoldenShowerStance);
        }

        public override void AddRecipes()
        {/*
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(489, 1);
            recipe.AddIngredient(Terraria.ID.ItemID.Chain, 30);
            recipe.AddTile(Terraria.ID.TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
		}
	}
}
