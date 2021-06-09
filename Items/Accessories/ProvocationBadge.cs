using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class ProvocationBadge : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows your Guardian to atract creatures attention.\nDon't forget to enable it on the tactics window.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.accessory = true;
			item.width = 44;            //Weapon's texture's width
			item.height = 44;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,25);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
		}

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.Tanking);
        }

        public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(491, 1);
            recipe.AddIngredient(Terraria.ID.ItemID.Chain, 30);
            recipe.AddTile(Terraria.ID.TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
		}
	}
}
