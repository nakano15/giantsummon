using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Shields
{
    public class LifeShield : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Give it to your favorite guardian. Or maybe not, we'll see.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.defense = 12;
			item.width = 22;            //Weapon's texture's width
			item.height = 30;           //Weapon's texture's height
			item.value = Item.buyPrice(0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
            ItemOrigin = new Vector2(8, 10);
            BlockRate = 10;
            Shield = true;
		}

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.MHP += 100;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.LifeCrystal, 20);
			recipe.AddTile(Terraria.ID.TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
