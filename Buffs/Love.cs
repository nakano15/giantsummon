using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Love : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("In Love");
            Description.SetDefault("Damage you cause reduced.");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage -= (player.meleeDamage - 1) * 0.25f;
            player.rangedDamage -= (player.rangedDamage - 1) * 0.25f;
            player.magicDamage -= (player.magicDamage - 1) * 0.25f;
            player.minionDamage -= (player.minionDamage - 1) * 0.25f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.damage -= (int)(npc.damage * (Main.expertMode ? 0.05f : 0.25f));
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MeleeDamageMultiplier -= (guardian.MeleeDamageMultiplier - 1) * 0.25f;
            guardian.RangedDamageMultiplier -= (guardian.RangedDamageMultiplier - 1) * 0.25f;
            guardian.MagicDamageMultiplier -= (guardian.MagicDamageMultiplier - 1) * 0.25f;
            guardian.SummonDamageMultiplier -= (guardian.SummonDamageMultiplier - 1) * 0.25f;
        }
    }
}
