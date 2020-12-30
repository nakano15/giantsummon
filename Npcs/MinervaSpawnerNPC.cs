using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class MinervaSpawnerNPC : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "giantsummon/GuardianNPC/BlankNPC";
            }
        }

        public override void AI()
        {
            NpcMod.SpawnGuardianNPC(npc.Bottom.X, npc.Bottom.Y, GuardianBase.Minerva);
            npc.active = false;
            Main.NewText("You sense someone to the " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " of " + Main.player[Main.myPlayer].name + ".");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime && Main.time < 3600 * 6.5f && !NpcMod.HasMetGuardian(GuardianBase.Minerva) && !NpcMod.HasGuardianNPC(GuardianBase.Minerva))
                return 1f / 250;
            return 0;
        }
    }
}
