using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class LesserGuardianPotion : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("Restores 400 Life.");  //The (English) text shown below your weapon's name
		}

        public override bool GuardianCanUseItem(TerraGuardian guardian)
        {
            return guardian.Base.IsTerraGuardian;
        }

		public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.healLife = 50; //400
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
            ItemOrigin = new Vector2(14, 18);
            Heavy = true;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.LesserHealingPotion, 5);
            recipe.alchemy = true;
            recipe.AddTile(Terraria.ID.TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
