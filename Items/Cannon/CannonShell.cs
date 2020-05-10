using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Cannon
{
    public class CannonShell : ModItem
    {
        public override void SetDefaults()
        {
            item.shootSpeed = 5f;
            item.shoot = Terraria.ID.ProjectileID.CannonballFriendly;
            item.damage = 5;
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.consumable = true;
            item.ammo = CannonItem.AmmoType;
            item.knockBack = 2f;
            item.value = 20;
            item.ranged = true;
        }
    }
}
