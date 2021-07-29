using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class GuardianHealingPotion : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Restores 800 Life.");  //The (English) text shown below your weapon's name
		}

        public override bool GuardianCanUseItem(TerraGuardian guardian)
        {
            return guardian.Base.IsTerraGuardian;
        }

		public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.healLife = 100; //800
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 120;
            item.useTime = 120;
            item.maxStack = 50;
            item.consumable = true;
            item.potion = true;
            item.value = Item.buyPrice(0, 1);
			item.width = 36;            //Weapon's texture's width
			item.height = 42;           //Weapon's texture's height
            ItemOrigin = new Vector2(17, 18);
            Heavy = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.HealingPotion, 5);
            recipe.alchemy = true;
            recipe.AddTile(Terraria.ID.TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
