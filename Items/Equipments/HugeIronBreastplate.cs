using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Equipments
{
    public class HugeIronBreastplate : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("You can't really take a deep breath while wearing this.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.bodySlot = 2;
            item.defense = 9;
			item.width = 56;            //Weapon's texture's width
			item.height = 56;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
            item.rare = 0;              //The rarity of the weapon, from -1 to 13
            Heavy = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.IronBar, 45);
            recipe.anyIronBar = true;
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
