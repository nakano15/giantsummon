using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace giantsummon.Creatures.CaptainSmelly.PhantomDevices
{
    public class BrokenPhantomDevice : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It broke during the fall.");
        }

        public override void SetDefaults()
        {
            item.value = 1;
            item.stack = 1;
        }
    }
}
