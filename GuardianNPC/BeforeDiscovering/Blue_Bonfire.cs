using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.GuardianNPC.BeforeDiscovering
{
    public class Blue_Bonfire : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "giantsummon/GuardianNPC/BeforeDiscovering/Blue_Bonfire";
            }
        }

        public override bool Autoload(ref string name)
        {
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blue");
            Main.npcFrameCount[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 104;
            npc.height = 90;
            npc.aiStyle = 0;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.knockBackResist = 0.5f;
            npc.scale = 0.9f;
            npc.rarity = 2;
            npc.dontTakeDamage = npc.dontTakeDamageFromHostiles = true;
        }

        public override void AI()
        {

        }

        public override bool PreAI()
        {
            bool PlayerNearby = false;
            for (int index = 0; index < (int)byte.MaxValue; ++index)
            {
                if (Main.player[index].active && Main.player[index].Distance(npc.Center) <= 96f)
                {
                    PlayerNearby = true;
                }
            }
            npc.townNPC = PlayerNearby;
            return false;
        }

        public override string GetChat()
        {
            //int LastWidth = npc.width, LastDirection = npc.oldDirection;
            npc.Transform(ModContent.NPCType<GuardianNPC.List.WolfGuardian>());
            //npc.position.X -= (LastWidth - npc.width) * LastDirection * 0.5f;
            Main.NewText("You startled her.", 255, 0, 0);
            return "";
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool MaySpawn = !NpcMod.HasMetGuardian(1) && !NpcMod.HasGuardianNPC(1) && GuardianNPC.List.WolfGuardian.SpawnRequirement && !NPC.AnyNPCs(npc.type);
            //Main.NewText("Blue can spawn? " +MaySpawn);
            //if (!MaySpawn)
            //    Main.NewText("  Met? " + NpcMod.HasMetGuardian(1) + "  Eoc down?" + NPC.downedBoss1 + "  Already spawned? " + NPC.AnyNPCs(npc.type));
            return (MaySpawn? 1 : 0f);
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            int BonfireX = -1, BonfireY = -1;
            bool Found = false;
            for (int x = -16; x < 16; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    Tile t = Framing.GetTileSafely(tileX + x, tileY + y);
                    if (t.active() && t.type == 215)
                    {
                        BonfireX = tileX + x;
                        BonfireY = tileY + y;
                        Found = true;
                        break;
                    }
                }
                if (Found)
                    break;
            }
            if (Found)
            {
                bool Done = false;
                int attempts = 5;
                while (!Done)
                {
                    Tile t = Framing.GetTileSafely(BonfireX, BonfireY);
                    if (t.frameX < 1)
                        BonfireX++;
                    if (t.frameX > 1)
                        BonfireX--;
                    if (t.frameY < 1)
                        BonfireY++;
                    if (attempts <= 0 || (t.frameX == 1 && t.frameY == 1))
                        Done = true;
                    attempts--;
                }
                bool FacingLeft = Main.rand.NextDouble() < 0.5;
                int NpcPos = NPC.NewNPC(BonfireX * 16 + 8, BonfireY * 16, ModContent.NPCType<Blue_Bonfire>());
                if (NpcPos < 200)
                {
                    Main.npc[NpcPos].direction = FacingLeft ? -1 : 1;
                    Main.npc[NpcPos].position.X -= Main.npc[NpcPos].width * 0.8f * Main.npc[NpcPos].direction;
                    Main.NewText("There's something on the campfire!");
                }
                return NpcPos;
            }
            return 200;
        }
    }
}
