using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class LightWound : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Injured");
            Description.SetDefault("The guardian has a few wounds on the body.");
            Main.debuff[Type] = Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
            longerExpertDebuff = false;
        }
    }
}
