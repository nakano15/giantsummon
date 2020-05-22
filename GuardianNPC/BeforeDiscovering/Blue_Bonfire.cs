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
    }
}
