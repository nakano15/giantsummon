using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class HeavyInjury : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Heavy Injury");
            Description.SetDefault("The pain hinders the character.\nLower health after revive.\nEntering Ko'd state will instantly defeat you.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 0.9f);
            player.statDefense -= 10;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MHP = (int)(guardian.MHP * 0.90f);
            guardian.Defense -= 10;
        }
    }
}