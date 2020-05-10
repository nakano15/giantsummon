using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Shields
{
    public class IronBuckler : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Possibly gives a good amount of protection.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.defense = 10;
			item.width = 20;            //Weapon's texture's width
			item.height = 44;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
            ItemOrigin = new Vector2(10, 20);
            BlockRate = 15;
            Shield = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.IronBar, 40);
            recipe.anyIronBar = true;
			recipe.AddTile(Terraria.ID.TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
