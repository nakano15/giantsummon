using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class GuardianGreatHealingPotion : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Restores 1200 Life.");
		}

		public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.healLife = 150; //1200
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 120;
            item.useTime = 120;
            item.maxStack = 50;
            item.consumable = true;
            item.potion = true;
            item.value = Item.buyPrice(0, 1);
			item.width = 44;
			item.height = 56;
            ItemOrigin = new Vector2(22, 18);
            Heavy = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.GreaterHealingPotion, 5);
            recipe.alchemy = true;
            recipe.AddTile(Terraria.ID.TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
