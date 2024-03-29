﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace giantsummon.Companions.Creatures.CaptainStench.PhantomDevices
{
    public class BrokenPhantomDevice : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Item that allows captain stench to use phantom blitz attack, it is broken and needs repair.");
        }

        public override void SetDefaults()
        {
            item.value = 1;
            item.stack = 1;
            item.scale = 1.25f;
        }
    }
}
