using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Equipments
{
    public class HugeButcherShirt : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("It's not really stained with blood, the red markings are to fit the set.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.bodySlot = 4;
            item.defense = 12;
			item.width = 56;            //Weapon's texture's width
			item.height = 56;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
            item.rare = 0;              //The rarity of the weapon, from -1 to 13
            Heavy = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.CrimtaneBar, 35);
            recipe.AddIngredient(Terraria.ID.ItemID.TissueSample, 15);
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
