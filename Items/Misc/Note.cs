using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Misc
{
    public class Note : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("There's a guardian angel by the name of \'Daphne\'.\nShe shows up if you call her.");
        }
    }
}
