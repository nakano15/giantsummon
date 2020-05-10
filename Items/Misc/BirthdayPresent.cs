using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Misc
{
    public class BirthdayPresent : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Give this to a guardian having a birthday. Only one is enough, by the way.");
        }

        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.rare = 5;
            item.value = 5000;
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
    }
}
