using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Defended : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Defended");
            Description.SetDefault("Brutus is defending you from harm.");
            Main.debuff[this.Type] = Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.immuneTime = 3;
            player.immuneNoBlink = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.ImmuneTime = 3;
            guardian.ImmuneNoBlink = true;
        }
    }
}
