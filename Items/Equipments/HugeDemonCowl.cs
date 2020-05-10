using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Equipments
{
    public class HugeDemonCowl : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("If I try to use this, the name changes to \"Huge Demon Cloak\"?");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.headSlot = 3;
            item.defense = 8;
			item.width = 56;            //Weapon's texture's width
			item.height = 56;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
            item.rare = 0;              //The rarity of the weapon, from -1 to 13
            Heavy = true;
		}

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.DemoniteBar, 15);
            recipe.AddIngredient(Terraria.ID.ItemID.ShadowScale, 15);
			recipe.AddTile(Terraria.ID.TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
