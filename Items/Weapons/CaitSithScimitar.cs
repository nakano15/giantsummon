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
            item.damage = 22;
            item.melee = true;
            item.width = 44;
            item.height = 50;
            item.useTime = 44;
            item.useAnimation = 44;
            item.useStyle = 1;
            item.knockBack = 3.2f;
            item.crit = 12;
            item.rare = 6;
            item.UseSound = Terraria.ID.SoundID.Item1;
            item.autoReuse = false;
        }
    }
}
