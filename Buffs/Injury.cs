using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class Injury : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Injury");
            Description.SetDefault("Body is still recovering from injury.\nYou will have only 25% of max health restored upon entering Ko'd state.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statLifeMax2 = (int)(player.statLifeMax2 * 0.9f);
            player.statDefense -= 5;
            player.moveSpeed *= 0.95f;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MHP = (int)(guardian.MHP * 0.9f);
            guardian.Defense -= 5;
            guardian.MoveSpeed *= 0.85f;
        }
    }
}