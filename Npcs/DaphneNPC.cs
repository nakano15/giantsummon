using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Npcs
{
    public class DaphneNPC : GuardianActorNPC
    {
        public byte MeetStep = 0;
        private int Time = 0;

        public DaphneNPC() : base(GuardianBase.Daphne, "")
        {

        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Daphne");
        }

        public override void AI()
        {
            switch (MeetStep) {
                case 0:
                    {
                        Rectangle rect = new Rectangle((int)npc.Center.X, (int)npc.Center.Y, 300, 300);
                        if (npc.direction < 0)
                            rect.X -= rect.Width;
                        rect.Y -= rect.Height / 2;
                        Idle = true;
                        for (int p = 0; p < 255; p++)
                        {
                            if(Main.player[p].active && 
                                rect.Intersects(Main.player[p].getRect()) && 
                                Collision.CanHit(Main.player[p].position, Main.player[p].width, Main.player[p].height, 
                                npc.position, npc.width, npc.height))
                            {
                                npc.target = p;
                                MeetStep = 1;
                                break;
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if(!Main.player[npc.target].active || Main.player[npc.target].dead)
                        {
                            MeetStep = 0;
                        }
                        else
                        {
                            if(Math.Abs(Main.player[npc.target].Center.X - npc.Center.X) > 80)
                            {
                                if (Main.player[npc.target].Center.X < npc.Center.X)
                                    MoveLeft = true;
                                else
                                    MoveRight = true;
                            }
                            else
                            {
                                MeetStep = 2;
                                Time = 0;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        if (!Main.player[npc.target].active || Main.player[npc.target].dead)
                        {
                            MeetStep = 0;
                        }
                        else
                        {
                            if(npc.velocity.Length () == 0)
                            {
                                if (Main.player[npc.target].Center.X < npc.Center.X)
                                    npc.direction = -1;
                                else
                                    npc.direction = 1;
                            }
                            Time++;
                            if (Time == 20)
                                SayMessage("Bark! Bark! *Wags tail.*");
                            if (Time >= 180)
                            {
                                NpcMod.AddGuardianMet(GuardianID, GuardianModID);
                                PlayerMod.AddPlayerGuardian(Main.player[npc.target], GuardianID, GuardianModID);
                                WorldMod.TurnNpcIntoGuardianTownNpc(npc, GuardianID, GuardianModID);
                                return;
                            }
                        }
                    }
                    break;
            }
            base.AI();
        }

        public override bool CanChat()
        {
            return false;
        }

        /*public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            base.PostDraw(spriteBatch, drawColor);
            Vector2 HaloDrawPosition = new Vector2(37, 15) * 2;
            HaloDrawPosition.X -= Base.SpriteWidth * 0.5f;
            if (npc.direction < 0)
                HaloDrawPosition.X *= -1;
            HaloDrawPosition.X += npc.width * 0.5f;
            HaloDrawPosition.Y -= Base.SpriteHeight;
            HaloDrawPosition *= npc.scale;
            HaloDrawPosition.Y += npc.height;
            HaloDrawPosition += npc.position - Main.screenPosition;
            spriteBatch.Draw(Base.sprites.GetExtraTexture(Creatures.DaphneBase.HaloTextureID), HaloDrawPosition, new Rectangle(0, 0, 26, 12), Color.White, 0f, new Vector2(13, 6), 1f, SpriteEffects.None, 0f);
        }*/
    }
}
