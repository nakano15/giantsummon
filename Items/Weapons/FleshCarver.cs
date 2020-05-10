using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Weapons
{
    public class FleshCarver : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Sometimes I forget to reload, too.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
			item.damage = 27;           //The damage of your weapon
			item.ranged = true;          //Is your weapon a melee weapon?
			item.width = 40;            //Weapon's texture's width
			item.height = 32;           //Weapon's texture's height
			item.useTime = 23;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
			item.useAnimation = 23;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
			item.useStyle = 5;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
			item.knockBack = 1;         //The force of knockback of the weapon. Maximum is 20
            item.value = Item.buyPrice(0, 0, 15);           //The value of the weapon
            item.shoot = Terraria.ID.ProjectileID.Bullet;
            item.shootSpeed = 12f;
            item.crit = 7;
			item.rare = 1;              //The rarity of the weapon, from -1 to 13
            item.UseSound = SoundID.Item41;      //The sound when the weapon is using
            ItemOrigin = new Vector2(12, 20);
            ShotSpawnPosition = new Point(38, 10);
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.CrimtaneBar, 30);
            recipe.AddIngredient(Terraria.ID.ItemID.TissueSample, 12);
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
