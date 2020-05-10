using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.OffHand
{
    public class GiantLantern : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("I'll never need to hold a torch again.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
			item.width = 24;            //Weapon's texture's width
			item.height = 40;           //Weapon's texture's height
			item.value = Item.buyPrice(0,0,15);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
            ItemOrigin = new Vector2(11, 6);
            ShotSpawnPosition = new Point(11, 29);
            OffHandItem = true;
		}

        public override void ItemUpdateScript(TerraGuardian g)
        {
            Vector2 LightPosition = g.CenterPosition;
            Lighting.AddLight(LightPosition, 0.8f * 1.5f, 0.95f * 1.5f, 1.5f);
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(Terraria.ID.ItemID.IronBar, 20);
            recipe.anyIronBar = true;
            recipe.AddIngredient(Terraria.ID.ItemID.Wood, 15);
            recipe.anyWood = true;
			recipe.AddTile(Terraria.ID.TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
