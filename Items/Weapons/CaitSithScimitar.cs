using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Items.Weapons
{
    class CaitSithScimitar : GuardianItemPrefab
    {
        public override void SetDefaults()
        {
            item.damage = 29;
            item.melee = true;
            item.width = 44;
            item.height = 50;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 1;
            item.knockBack = 3.2f;
            item.crit = 12;
            item.rare = 6;
            item.UseSound = Terraria.ID.SoundID.Item1;
            item.autoReuse = false;
			PlayerCanUse = true;
            ItemOrigin = new Vector2(6, 43);
        }
    }
}
