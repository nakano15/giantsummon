using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class DrainingHealth : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Draining Health");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }
    }
}
