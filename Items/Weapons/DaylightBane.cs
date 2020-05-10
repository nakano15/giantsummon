using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Weapons
{
    public class DaylightBane : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Legends says that you can cut the sun in half with this weapon.");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
			item.damage = 38;           //The damage of your weapon
			item.melee = true;          //Is your weapon a melee weapon?
			item.width = 30;            //Weapon's texture's width
			item.height = 96;           //Weapon's texture's height
			item.useTime = 47;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
			item.useAnimation = 47;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
			item.useStyle = 1;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
			item.knockBack = 6;         //The force of knockback of the weapon. Maximum is 20
            item.crit = 14;
			item.value = Item.buyPrice(0,0,85);           //The value of the weapon
			item.rare = 0;              //The rarity of the weapon, from -1 to 13
			item.UseSound = SoundID.Item1;      //The sound when the weapon is using
			item.autoReuse = false;          //Whether the weapon can use automatically by pressing mousebutton
            this.Heavy = true;
            ItemOrigin = new Vector2(4, 86);
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(Terraria.ID.ItemID.DemoniteBar, 35);
            recipe.AddIngredient(Terraria.ID.ItemID.ShadowScale, 15);
            recipe.AddTile(Terraria.ID.TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
