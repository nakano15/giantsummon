using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class LightFatigue : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tired");
            Description.SetDefault("Showing signs of being tired, no penalty right now.");
            Main.debuff[Type] = Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
    }
}
