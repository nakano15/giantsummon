using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Weapons
{
    public class MahoganyCompositeBow : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Looked better on my mind.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults() //Mahogany bow
		{
			item.damage = 23;           //The damage of your weapon
			item.ranged = true;          //Is your weapon a melee weapon?
			item.width = 24;            //Weapon's texture's width
			item.height = 56;           //Weapon's texture's height
			item.useTime = 35;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
			item.useAnimation = 35;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
			item.useStyle = 5;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
			item.knockBack = 8;         //The force of knockback of the weapon. Maximum is 20
            item.value = Item.buyPrice(0, 0, 15);           //The value of the weapon
            item.shoot = Terraria.ID.ProjectileID.WoodenArrowFriendly;
            item.shootSpeed = 12f;
            item.crit = 7;
			item.rare = 1;              //The rarity of the weapon, from -1 to 13
            item.UseSound = SoundID.Item5;      //The sound when the weapon is using
            ItemOrigin = new Vector2(20, 30);
            ShotSpawnPosition = new Point(20, 22);
            ProjectileFallRate = 8;
            hand = HeldHand.Left;
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.RichMahogany, 40);
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
