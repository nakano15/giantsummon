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
            Description.SetDefault("The pain hinders you.\nLower health after revive.\nYou will not survive if you get knocked out again.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 0.85f);
            player.statDefense = (int)(player.statDefense * 0.9f);
            player.moveSpeed *= 0.85f;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MHP = (int)(guardian.MHP * 0.85f);
            guardian.Defense = (int)(guardian.Defense * 0.9f);
            guardian.MoveSpeed *= 0.85f;
        }
    }
}