using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Hug : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hug");
            Description.SetDefault("Increased health regeneration.\nGreat damage reduction.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 3;
            player.endurance += 0.15f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen += 3;
        }

        public override void Update(TerraGuardian guardian)
        {
            //guardian.lifeRegen += 3;
            //if (guardian.velocity.X == 0)
            //    guardian.lifeRegen += 3;
            guardian.DefenseRate += 0.15f;
        }
    }
}
