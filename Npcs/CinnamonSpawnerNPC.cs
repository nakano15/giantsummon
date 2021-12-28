using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class CinnamonSpawnerNPC : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "giantsummon/Npcs/Blank";
            }
        }

        public override void AI()
        {
            int MerchantPos = NPC.FindFirstNPC(Terraria.ID.NPCID.TravellingMerchant);
            if(MerchantPos < 0 || MerchantPos >= 200)
            {
                npc.active = false;
                return;
            }
            NPC TravellingMerchant = Main.npc[MerchantPos];
            NpcMod.SpawnGuardianNPC(TravellingMerchant.Bottom.X, TravellingMerchant.Bottom.Y, GuardianBase.Cinnamon);
            npc.active = false;
            Main.NewText("Someone has arrived by following the Travelling Merchant.");
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime && Main.time < 3600 * 8.5f && NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant) && !MainMod.IsGuardianInTheWorld(GuardianBase.Cinnamon))
            {
                return 1f / 125; //250
            }
            return 0;
        }
    }
}
