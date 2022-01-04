using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Npcs
{
    class LiebreNPC : GuardianActorNPC
    {
        private bool SpottedPlayer = false;
        private bool PlayerLeft = false;
        private ushort TalkTime = 0;

        public LiebreNPC() : base(GuardianBase.Liebre)
        {

        }

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            npc.townNPC = false;
            DisplayName.SetDefault("???");
        }

        public override void AI()
        {
            if (!SpottedPlayer)
            {
                Idle = true;
                npc.TargetClosest(false);
                Player player = Main.player[npc.target];
                if(player.active && !player.dead)
                {
                    Vector2 Distance = player.Center - npc.Center;
                    if(Math.Abs(Distance.X) < 180f && Math.Abs(Distance.Y) < 120f && 
                        Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                    {
                        if(!PlayerLeft)
                        {
                            SayMessage("*A Terrarian..*");
                        }
                        else
                        {
                            SayMessage("*Is that Terrarian again..*");
                        }
                        TalkTime = 180;
                        SpottedPlayer = true;
                    }
                }
            }
            else
            {
                Idle = false;
                Player player = Main.player[npc.target];
                Vector2 Distance = player.Center - npc.Center;
                if (TalkTime > 0)
                {
                    TalkTime--;
                    if (TalkTime == 0)
                    {
                        if (Math.Abs(Distance.X) < 180f && Math.Abs(Distance.Y) < 120f &&
                            Collision.CanHitLine(player.position, player.width, player.height, npc.position, npc.width, npc.height))
                        {
                            if(!PlayerLeft)
                                SayMessage("*Terrarian, can we talk?*");
                            else
                                SayMessage("*Please don't run away again, I must speak to you.*");
                        }
                        else
                        {
                            if (!PlayerLeft)
                                SayMessage("*Wait, Terrarian, I need to talk to you.*");
                            else
                                SayMessage("*Wait, don't go again!*");
                        }
                    }
                }
                if(Math.Abs(Distance.X) >= 600 || Math.Abs(Distance.Y) >= 240)
                {
                    SpottedPlayer = false;
                    if (!PlayerLeft)
                        SayMessage("*I guess I scared them...*");
                    else
                        SayMessage("*They left... Again...*");
                    PlayerLeft = true;
                }
            }
            base.AI();
        }

        public override void ModifyDrawDatas(List<GuardianDrawData> dds, Vector2 Position, Rectangle BodyRect, Rectangle LArmRect, Rectangle RArmRect, Vector2 Origin, Color color, SpriteEffects seffects)
        {

        }

        public override void FindFrame(int frameHeight)
        {
            base.FindFrame(frameHeight);
        }

        public static bool CanSpawn
        {
            get
            {
                return NPC.downedBoss3;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CanSpawn && !spawnInfo.water && !Main.dayTime && !Main.bloodMoon && !NpcMod.HasGuardianNPC(GuardianBase.Liebre) && !NpcMod.HasMetGuardian(GuardianBase.Liebre))
            {
                Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
                if(Lighting.Brightness(spawnInfo.spawnTileX, spawnInfo.spawnTileY) < 0.15f)
                {
                    //Can spawn :D
                }
            }
            return base.SpawnChance(spawnInfo);
        }
    }
}
