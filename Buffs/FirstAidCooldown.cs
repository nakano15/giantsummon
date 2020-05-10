using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class FirstAidCooldown : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("First Aid Cooldown");
            Description.SetDefault("Can't use First Aid again on this guardian.");
            Main.debuff[Type] = Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
    }
}
