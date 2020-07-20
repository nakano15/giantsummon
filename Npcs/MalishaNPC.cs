using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    public class MalishaNPC : GuardianActorNPC
    {
        public MalishaNPC()
            : base(GuardianBase.Malisha, "")
        {

        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void AI()
        {
            base.AI();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0; //Let's finish the npc first before trying to spawn It in? :3
            if (!Main.dayTime && NPC.downedBoss3 && !NpcMod.HasGuardianNPC(GuardianBase.Malisha) && !PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianBase.Malisha) && Main.time > 27000 && Main.time < 48600 && !NPC.AnyNPCs(ModContent.NPCType<MalishaNPC>()))
            {
                return (float)(Main.time - 27000) / 432000;
            }
            return 0;
        }
    }
}
