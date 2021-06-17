using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace giantsummon.PlayerItems
{
    public class StaffOfRenew : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.Tooltip.SetDefault("Lets you regenerate your guardian health at the cost of mana.");
        }

        public override void SetDefaults()
        {
            item.width = 26;            //Weapon's texture's width
            item.height = 26;           //Weapon's texture's height
            item.useTime = 30;          //The time span of using the weapon. Remember in terraria, 60 frames is a second.
            item.useAnimation = 30;         //The time span of the using animation of the weapon, suggest set it the same as useTime.
            item.useStyle = 4;          //The use style of weapon, 1 for swinging, 2 for drinking, 3 act like shortsword, 4 for use like life crystal, 5 for use staffs or guns
            item.mana = 3;
            item.value = Item.buyPrice(0, 0, 15);           //The value of the weapon
            item.rare = 0;              //The rarity of the weapon, from -1 to 13
            item.UseSound = SoundID.Item1;      //The sound when the weapon is using
            item.autoReuse = true;          //Whether the weapon can use automatically by pressing mousebutton
        }

        public override bool UseItem(Player player)
        {
            if (player.itemAnimation == player.itemAnimationMax - 1)
            {
                foreach (TerraGuardian guardian in player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (guardian.Active && Math.Abs(player.Center.X - guardian.Position.X) < NPC.sWidth && Math.Abs(player.Center.Y - guardian.CenterY) < NPC.sHeight)
                    {
                        int HealthRestore = (int)(guardian.Base.InitialMHP * 0.02f * guardian.HealthHealMult * player.magicDamage);
                        if (HealthRestore < 1) HealthRestore = 1;
                        if (HealthRestore + guardian.HP > guardian.MHP)
                            HealthRestore = guardian.MHP - guardian.HP;
                        guardian.RestoreHP(HealthRestore);
                    }
                }
            }
            return base.UseItem(player);
        }
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Wood, 18);
            recipe.anyWood = true;
            recipe.AddIngredient(ItemID.Acorn, 3);
            recipe.AddIngredient(ItemID.FallenStar, 3);
            recipe.SetResult(this);
            recipe.AddTile(TileID.WorkBenches);
            recipe.AddRecipe();
        }
    }
}
