using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Protecting : GuardianModBuff
    {
        public override void  SetDefaults()
        {
            DisplayName.SetDefault("Protecting");
            Description.SetDefault("Guardian is protecting the player from harms.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.CoverRate += 40;
        }
    }
}
