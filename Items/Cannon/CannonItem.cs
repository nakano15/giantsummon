using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Cannon
{
    public class CannonItem : ModItem
    {
        public const int CannonType = 122;
        public static int AmmoType { get { return ModContent.ItemType<CannonShell>(); } }
        
        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.useAmmo = AmmoType;
            item.autoReuse = true;
            item.useAnimation = 46;
            item.useTime = 46;
            item.width = 34;
            item.height = 22;
            item.shoot = Terraria.ID.ProjectileID.CannonballFriendly;
            item.UseSound = Terraria.ID.SoundID.Item11;
            item.damage = 12;
            item.shootSpeed = 5f;
            item.noMelee = true;
            item.value = Item.buyPrice(0,0, 18);
            item.knockBack = 6f;
            item.rare = 2;
            item.ranged = true;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You shouldn't have this item.");
        }
        
        /*public override bool HoldItemFrame(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height;
            return true;
        }

        public override void HoldStyle(Player player)
        {
            player.itemLocation.X = (int)(player.Center.X - 10);
            player.itemLocation.Y = (int)(player.position.Y + 4f);
        }*/

        public override Microsoft.Xna.Framework.Vector2? HoldoutOrigin()
        {
            Microsoft.Xna.Framework.Vector2 Origin = new Microsoft.Xna.Framework.Vector2(Main.itemTexture[item.type].Width * 0.5f, Main.itemTexture[item.type].Height - 4f);
            return Origin;
        }
    }
}
