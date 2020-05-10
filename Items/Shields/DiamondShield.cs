using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Shields
{
    public class DiamondShield : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Depending on the perspective you look... Still doesn't look like diamond..");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.defense = 16;
			item.width = 18;            //Weapon's texture's width
			item.height = 30;           //Weapon's texture's height
			item.value = Item.buyPrice(0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
            ItemOrigin = new Vector2(8, 10);
            BlockRate = 20;
            Shield = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.Diamond, 50);
			recipe.AddTile(Terraria.ID.TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
