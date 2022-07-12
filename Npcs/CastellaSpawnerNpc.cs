using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Npcs
{
    class CastellaSpawnerNpc : ModNPC
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
            //NpcMod.SpawnGuardianNPC(npc.Bottom.X, npc.Bottom.Y, GuardianBase.Castella);
            npc.Transform(Terraria.ID.NPCID.DungeonGuardian);
            npc.active = false;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            /*if (Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].wall == 0 && PlayerMod.GetTerraGuardianCompanionsFound(spawnInfo.player) > 0 && Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].wall == 0 && spawnInfo.playerInTown && Main.dayTime && !spawnInfo.water && !NpcMod.HasMetGuardian(GuardianBase.Luna) && !NpcMod.HasGuardianNPC(GuardianBase.Luna) && !PlayerMod.PlayerHasGuardianSummoned(spawnInfo.player, GuardianBase.Luna))
            {
                return 1f;
            }*/
            return 0;
        }
    }
}
