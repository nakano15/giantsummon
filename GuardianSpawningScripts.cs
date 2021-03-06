﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianSpawningScripts
    {
        public static void TrySpawningMichelle()
        {
            if (NpcMod.HasMetGuardian(GuardianBase.Michelle) || WorldMod.IsGuardianNpcInWorld(GuardianBase.Michelle))
                return;
            if (Main.fastForwardTime || Main.eclipse || !Main.dayTime || Main.time >= 27000 || Main.time % 60 > 0)
            {
                return;
            }
            if (Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return;
            if (Main.rand.Next(17) == 0)
            {
                bool HasPlayerWithDefense = false;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Main.player[p].statDefense > 0)
                    {
                        HasPlayerWithDefense = true;
                        break;
                    }
                }
                if (HasPlayerWithDefense)
                {
                    NpcMod.SpawnGuardianNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, GuardianBase.Michelle);
                    //NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, ModContent.NPCType<MichelleGuardian>());
                    Main.NewText("Michelle has logged in.", 255, 255, 0);
                }
            }
        }
    }
}
