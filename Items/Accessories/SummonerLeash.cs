using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class SummonerLeash : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Increases Guardian's Summon Damage by 15%.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.accessory = true;
			item.width = 48;            //Weapon's texture's width
			item.height = 76;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
            ItemStatusScript = (delegate(TerraGuardian Guardian)
            {
                Guardian.SummonDamageMultiplier += 0.15f;
            });
		}

		public override void AddRecipes()
        {/*
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(2998, 1);
            recipe.AddIngredient(Terraria.ID.ItemID.Chain, 30);
            recipe.AddTile(Terraria.ID.TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
		}
	}
}
