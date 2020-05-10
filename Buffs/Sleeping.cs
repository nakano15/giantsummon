using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class Sleeping : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Sleeping");
            Description.SetDefault("Health regeneration increases. Vulnerable to attacks.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.Defense -= (int)(guardian.Defense * 0.5f);
            guardian.DefenseRate = 0;
            guardian.DodgeRate = 0;
            guardian.BlockRate = 0;
        }
    }
}
