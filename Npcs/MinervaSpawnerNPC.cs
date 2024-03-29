﻿using System;
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
                return "giantsummon/Npcs/Blank";
            }
        }

        public override void AI()
        {
            NpcMod.SpawnGuardianNPC(npc.Bottom.X, npc.Bottom.Y, GuardianBase.Minerva);
            npc.active = false;
            //Main.NewText(Main.player[Main.myPlayer].name + " has sense someone " + GuardianBountyQuest.GetDirectionText(npc.Center - Main.player[Main.myPlayer].Center) + " of their position.", MainMod.MysteryCloseColor);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.dayTime && Main.time < 3600 * 6.5f && !spawnInfo.playerInTown && !spawnInfo.water && NpcMod.RecruitNpcSpawnConditionCheck(spawnInfo) && (!NpcMod.HasMetGuardian(GuardianBase.Minerva) || PlayerMod.GetPlayerGuardianFriendshipLevel(spawnInfo.player, GuardianBase.Minerva) < 3) && !NpcMod.HasGuardianNPC(GuardianBase.Minerva) && !MainMod.IsGuardianInTheWorld(GuardianBase.Minerva))
            {
                return 1f / 250; //250
            }
            return 0;
        }
    }
}
