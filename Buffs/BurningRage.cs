using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    class BurningRage : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Burning Rage");
            Description.SetDefault("Wrath is furious!");
            Main.debuff[Type] = Main.pvpBuff[Type] = Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MeleeDamageMultiplier += 0.5f;
            guardian.RangedDamageMultiplier += 0.5f;
            guardian.MagicDamageMultiplier += 0.5f;
            guardian.SummonDamageMultiplier += 0.5f;
        }
    }
}
