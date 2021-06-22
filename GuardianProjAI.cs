using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public partial class ProjMod : GlobalProjectile
    {
        private void GroundPetsAndBabySlimeAI(Projectile proj)
        {
            if (!GuardianProj.ContainsKey(proj.whoAmI) || !MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID))
            {
                proj.active = false;
                return;
            }
            else
            {
                TerraGuardian guardian = GuardianProj[proj.whoAmI];
                bool flag1 = false;
                bool flag2 = false;
                bool flag3 = false;
                bool flag4 = false;
                int num1 = 85;
                bool flag5 = proj.type >= 191 && proj.type <= 194;
                if (proj.type == 324)
                    num1 = 120;
                if (proj.type == 112)
                    num1 = 100;
                if (proj.type == (int)sbyte.MaxValue)
                    num1 = 50;
                if (flag5)
                {
                    if (proj.lavaWet)
                    {
                        proj.ai[0] = 1f;
                        proj.ai[1] = 0.0f;
                    }
                    num1 = 60 + 30 * proj.minionPos;
                }
                else if (proj.type == 266)
                    num1 = 60 + 30 * proj.minionPos;
                if (!guardian.Downed)
                {
                    proj.timeLeft = 2;
                }
                /*if (proj.type == 111)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].bunny = false;
                    if (Main.player[proj.owner].bunny)
                        proj.timeLeft = 2;
                }
                if (proj.type == 112)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].penguin = false;
                    if (Main.player[proj.owner].penguin)
                        proj.timeLeft = 2;
                }
                if (proj.type == 334)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].puppy = false;
                    if (Main.player[proj.owner].puppy)
                        proj.timeLeft = 2;
                }
                if (proj.type == 353)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].grinch = false;
                    if (Main.player[proj.owner].grinch)
                        proj.timeLeft = 2;
                }
                if (proj.type == (int)sbyte.MaxValue)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].turtle = false;
                    if (Main.player[proj.owner].turtle)
                        proj.timeLeft = 2;
                }
                if (proj.type == 175)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].eater = false;
                    if (Main.player[proj.owner].eater)
                        proj.timeLeft = 2;
                }
                if (proj.type == 197)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].skeletron = false;
                    if (Main.player[proj.owner].skeletron)
                        proj.timeLeft = 2;
                }
                if (proj.type == 198)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].hornet = false;
                    if (Main.player[proj.owner].hornet)
                        proj.timeLeft = 2;
                }
                if (proj.type == 199)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].tiki = false;
                    if (Main.player[proj.owner].tiki)
                        proj.timeLeft = 2;
                }
                if (proj.type == 200)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].lizard = false;
                    if (Main.player[proj.owner].lizard)
                        proj.timeLeft = 2;
                }
                if (proj.type == 208)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].parrot = false;
                    if (Main.player[proj.owner].parrot)
                        proj.timeLeft = 2;
                }
                if (proj.type == 209)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].truffle = false;
                    if (Main.player[proj.owner].truffle)
                        proj.timeLeft = 2;
                }
                if (proj.type == 210)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].sapling = false;
                    if (Main.player[proj.owner].sapling)
                        proj.timeLeft = 2;
                }
                if (proj.type == 324)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].cSapling = false;
                    if (Main.player[proj.owner].cSapling)
                        proj.timeLeft = 2;
                }
                if (proj.type == 313)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].spider = false;
                    if (Main.player[proj.owner].spider)
                        proj.timeLeft = 2;
                }
                if (proj.type == 314)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].squashling = false;
                    if (Main.player[proj.owner].squashling)
                        proj.timeLeft = 2;
                }
                if (proj.type == 211)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].wisp = false;
                    if (Main.player[proj.owner].wisp)
                        proj.timeLeft = 2;
                }
                if (proj.type == 236)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].dino = false;
                    if (Main.player[proj.owner].dino)
                        proj.timeLeft = 2;
                }
                if (proj.type == 499)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].babyFaceMonster = false;
                    if (Main.player[proj.owner].babyFaceMonster)
                        proj.timeLeft = 2;
                }
                if (proj.type == 266)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].slime = false;
                    if (Main.player[proj.owner].slime)
                        proj.timeLeft = 2;
                }
                if (proj.type == 268)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].eyeSpring = false;
                    if (Main.player[proj.owner].eyeSpring)
                        proj.timeLeft = 2;
                }
                if (proj.type == 269)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].snowman = false;
                    if (Main.player[proj.owner].snowman)
                        proj.timeLeft = 2;
                }
                if (proj.type == 319)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].blackCat = false;
                    if (Main.player[proj.owner].blackCat)
                        proj.timeLeft = 2;
                }
                if (proj.type == 380)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].zephyrfish = false;
                    if (Main.player[proj.owner].zephyrfish)
                        proj.timeLeft = 2;
                }
                if (flag5)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].pygmy = false;
                    if (Main.player[proj.owner].pygmy)
                        proj.timeLeft = Main.rand.Next(2, 10);
                }
                if (proj.type >= 390 && proj.type <= 392)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].spiderMinion = false;
                    if (Main.player[proj.owner].spiderMinion)
                        proj.timeLeft = 2;
                }
                if (proj.type == 398)
                {
                    if (Main.player[proj.owner].dead)
                        Main.player[proj.owner].miniMinotaur = false;
                    if (Main.player[proj.owner].miniMinotaur)
                        proj.timeLeft = 2;
                }*/
                if (flag5 || proj.type == 266 || proj.type >= 390 && proj.type <= 392)
                {
                    int num2 = 10;
                    int num3 = 40 * (proj.minionPos + 1) * guardian.Direction;
                    if ((double)guardian.Position.X < (double)proj.position.X + (double)(proj.width / 2) - (double)num2 + (double)num3)
                        flag1 = true;
                    else if ((double)guardian.Position.X > (double)proj.position.X + (double)(proj.width / 2) + (double)num2 + (double)num3)
                        flag2 = true;
                }
                else if ((double)guardian.Position.X < (double)proj.position.X + (double)(proj.width / 2) - (double)num1)
                    flag1 = true;
                else if ((double)guardian.Position.X > (double)proj.position.X + (double)(proj.width / 2) + (double)num1)
                    flag2 = true;
                if (proj.type == 175)
                {
                    float num2 = 0.1f;
                    proj.tileCollide = false;
                    int num3 = 300;
                    Vector2 vector2_1 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                    float num4 = guardian.Position.X - vector2_1.X;
                    float num5 = guardian.Position.Y - guardian.Height * 0.5f - vector2_1.Y;
                    if (proj.type == (int)sbyte.MaxValue)
                        num5 = guardian.Position.Y - guardian.Height - vector2_1.Y;
                    float num6 = (float)Math.Sqrt((double)num4 * (double)num4 + (double)num5 * (double)num5);
                    float num7 = 7f;
                    if ((double)num6 < (double)num3 && (double)guardian.Velocity.Y == 0.0 && ((double)proj.position.Y + (double)proj.height <= (double)guardian.Position.Y && !Collision.SolidCollision(proj.position, proj.width, proj.height)))
                    {
                        proj.ai[0] = 0.0f;
                        if ((double)proj.velocity.Y < -6.0)
                            proj.velocity.Y = -6f;
                    }
                    if ((double)num6 < 150.0)
                    {
                        if ((double)Math.Abs(proj.velocity.X) > 2.0 || (double)Math.Abs(proj.velocity.Y) > 2.0)
                        {
                            proj.velocity *= 0.99f;
                        }
                        num2 = 0.01f;
                        if ((double)num4 < -2.0)
                            num4 = -2f;
                        if ((double)num4 > 2.0)
                            num4 = 2f;
                        if ((double)num5 < -2.0)
                            num5 = -2f;
                        if ((double)num5 > 2.0)
                            num5 = 2f;
                    }
                    else
                    {
                        if ((double)num6 > 300.0)
                            num2 = 0.2f;
                        float num8 = num7 / num6;
                        num4 *= num8;
                        num5 *= num8;
                    }
                    if ((double)Math.Abs(num4) > (double)Math.Abs(num5) || (double)num2 == 0.0500000007450581)
                    {
                        if ((double)proj.velocity.X < (double)num4)
                        {
                            proj.velocity.X += num2;
                            if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X < 0.0)
                                proj.velocity.X += num2;
                        }
                        if ((double)proj.velocity.X > (double)num4)
                        {
                            proj.velocity.X -= num2;
                            if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X > 0.0)
                                proj.velocity.X -= num2;
                        }
                    }
                    if ((double)Math.Abs(num4) <= (double)Math.Abs(num5) || (double)num2 == 0.0500000007450581)
                    {
                        if ((double)proj.velocity.Y < (double)num5)
                        {
                            proj.velocity.Y += num2;
                            if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y < 0.0)
                                proj.velocity.Y += num2;
                        }
                        if ((double)proj.velocity.Y > (double)num5)
                        {
                            proj.velocity.Y -= num2;
                            if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y > 0.0)
                                proj.velocity.Y -= num2;
                        }
                    }
                    proj.rotation = (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) - 1.57f;
                    ++proj.frameCounter;
                    if (proj.frameCounter > 6)
                    {
                        ++proj.frame;
                        proj.frameCounter = 0;
                    }
                    if (proj.frame <= 1)
                        return;
                    proj.frame = 0;
                }
                else if (proj.type == 197)
                {
                    float num2 = 0.1f;
                    proj.tileCollide = false;
                    int num3 = 300;
                    Vector2 vector2_1 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                    float num4 = guardian.Position.X - vector2_1.X;
                    float num5 = guardian.Position.Y - guardian.Height * 0.5f - vector2_1.Y;
                    if (proj.type == (int)sbyte.MaxValue)
                        num5 = guardian.Position.Y - guardian.Height - vector2_1.Y;
                    float num6 = (float)Math.Sqrt((double)num4 * (double)num4 + (double)num5 * (double)num5);
                    float num7 = 3f;
                    if ((double)num6 > 500.0)
                        proj.localAI[0] = 10000f;
                    if ((double)proj.localAI[0] >= 10000.0)
                        num7 = 14f;
                    if ((double)num6 < (double)num3 && (double)guardian.Velocity.Y == 0.0 && ((double)proj.position.Y + (double)proj.height <= (double)guardian.Position.Y && !Collision.SolidCollision(proj.position, proj.width, proj.height)))
                    {
                        proj.ai[0] = 0.0f;
                        if ((double)proj.velocity.Y < -6.0)
                            proj.velocity.Y = -6f;
                    }
                    if ((double)num6 < 150.0)
                    {
                        if ((double)Math.Abs(proj.velocity.X) > 2.0 || (double)Math.Abs(proj.velocity.Y) > 2.0)
                        {
                            proj.velocity *= 0.99f;
                        }
                        num2 = 0.01f;
                        if ((double)num4 < -2.0)
                            num4 = -2f;
                        if ((double)num4 > 2.0)
                            num4 = 2f;
                        if ((double)num5 < -2.0)
                            num5 = -2f;
                        if ((double)num5 > 2.0)
                            num5 = 2f;
                    }
                    else
                    {
                        if ((double)num6 > 300.0)
                            num2 = 0.2f;
                        float num8 = num7 / num6;
                        num4 *= num8;
                        num5 *= num8;
                    }
                    if ((double)proj.velocity.X < (double)num4)
                    {
                        proj.velocity.X += num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X < 0.0)
                            proj.velocity.X += num2;
                    }
                    if ((double)proj.velocity.X > (double)num4)
                    {
                        proj.velocity.X -= num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X > 0.0)
                            proj.velocity.X -= num2;
                    }
                    if ((double)proj.velocity.Y < (double)num5)
                    {
                        proj.velocity.Y += num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y < 0.0)
                            proj.velocity.Y += num2;
                    }
                    if ((double)proj.velocity.Y > (double)num5)
                    {
                        proj.velocity.Y -= num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y > 0.0)
                            proj.velocity.Y -= num2;
                    }
                    proj.localAI[0] += (float)Main.rand.Next(10);
                    if ((double)proj.localAI[0] > 10000.0)
                    {
                        if ((double)proj.localAI[1] == 0.0)
                            proj.localAI[1] = (double)proj.velocity.X >= 0.0 ? 1f : -1f;
                        proj.rotation += 0.25f * proj.localAI[1];
                        if ((double)proj.localAI[0] > 12000.0)
                            proj.localAI[0] = 0.0f;
                    }
                    else
                    {
                        proj.localAI[1] = 0.0f;
                        float num8 = proj.velocity.X * 0.1f;
                        if ((double)proj.rotation > (double)num8)
                        {
                            proj.rotation -= (float)(((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y)) * 0.00999999977648258);
                            if ((double)proj.rotation < (double)num8)
                                proj.rotation = num8;
                        }
                        if ((double)proj.rotation < (double)num8)
                        {
                            proj.rotation += (float)(((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y)) * 0.00999999977648258);
                            if ((double)proj.rotation > (double)num8)
                                proj.rotation = num8;
                        }
                    }
                    if ((double)proj.rotation > 6.28)
                        proj.rotation -= 6.28f;
                    if ((double)proj.rotation >= -6.28)
                        return;
                    proj.rotation += 6.28f;
                }
                else if (proj.type == 198 || proj.type == 380)
                {
                    float num2 = 0.4f;
                    if (proj.type == 380)
                        num2 = 0.3f;
                    proj.tileCollide = false;
                    int num3 = 100;
                    Vector2 vector2_1 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                    float num4 = guardian.Position.X - vector2_1.X;
                    float num5 = guardian.Position.Y - guardian.Height * 0.5f - vector2_1.Y + (float)Main.rand.Next(-10, 21);
                    float num6 = num4 + (float)Main.rand.Next(-10, 21) + (float)(60 * -guardian.Direction);
                    float num7 = num5 - 60f;
                    if (proj.type == (int)sbyte.MaxValue)
                        num7 = guardian.Position.Y - guardian.Height - vector2_1.Y;
                    float num8 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num7 * (double)num7);
                    float num9 = 14f;
                    if (proj.type == 380)
                        num9 = 6f;
                    if ((double)num8 < (double)num3 && (double)guardian.Velocity.Y == 0.0 && ((double)proj.position.Y + (double)proj.height <= (double)guardian.Position.Y && !Collision.SolidCollision(proj.position, proj.width, proj.height)))
                    {
                        proj.ai[0] = 0.0f;
                        if ((double)proj.velocity.Y < -6.0)
                            proj.velocity.Y = -6f;
                    }
                    if ((double)num8 < 50.0)
                    {
                        if ((double)Math.Abs(proj.velocity.X) > 2.0 || (double)Math.Abs(proj.velocity.Y) > 2.0)
                        {
                            proj.velocity *= 0.99f;
                        }
                        num2 = 0.01f;
                    }
                    else
                    {
                        if (proj.type == 380)
                        {
                            if ((double)num8 < 100.0)
                                num2 = 0.1f;
                            if ((double)num8 > 300.0)
                                num2 = 0.4f;
                        }
                        else if (proj.type == 198)
                        {
                            if ((double)num8 < 100.0)
                                num2 = 0.1f;
                            if ((double)num8 > 300.0)
                                num2 = 0.6f;
                        }
                        float num10 = num9 / num8;
                        num6 *= num10;
                        num7 *= num10;
                    }
                    if ((double)proj.velocity.X < (double)num6)
                    {
                        proj.velocity.X += num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X < 0.0)
                            proj.velocity.X += num2;
                    }
                    if ((double)proj.velocity.X > (double)num6)
                    {
                        proj.velocity.X -= num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.X > 0.0)
                            proj.velocity.X -= num2;
                    }
                    if ((double)proj.velocity.Y < (double)num7)
                    {
                        proj.velocity.Y += num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y < 0.0)
                            proj.velocity.Y += num2 * 2f;
                    }
                    if ((double)proj.velocity.Y > (double)num7)
                    {
                        proj.velocity.Y -= num2;
                        if ((double)num2 > 0.0500000007450581 && (double)proj.velocity.Y > 0.0)
                            proj.velocity.Y -= num2 * 2f;
                    }
                    if ((double)proj.velocity.X > 0.25)
                        proj.direction = -1;
                    else if ((double)proj.velocity.X < -0.25)
                        proj.direction = 1;
                    proj.spriteDirection = proj.direction;
                    proj.rotation = proj.velocity.X * 0.05f;
                    ++proj.frameCounter;
                    int num11 = 2;
                    if (proj.type == 380)
                        num11 = 5;
                    if (proj.frameCounter > num11)
                    {
                        ++proj.frame;
                        proj.frameCounter = 0;
                    }
                    if (proj.frame <= 3)
                        return;
                    proj.frame = 0;
                }
                else if (proj.type == 211)
                {
                    float num2 = 0.2f;
                    float num3 = 5f;
                    proj.tileCollide = false;
                    Vector2 vector2_1 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                    float num4 = guardian.Position.X - vector2_1.X;
                    float num5 = guardian.Position.Y - guardian.Height * 0.5f - vector2_1.Y;
                    if (guardian.MoveLeft)
                        num4 -= 120f;
                    else if (guardian.MoveRight)
                        num4 += 120f;
                    float num6;
                    if (guardian.MoveDown)
                    {
                        num6 = num5 + 120f;
                    }
                    else
                    {
                        if (guardian.MoveUp)
                            num5 -= 120f;
                        num6 = num5 - 60f;
                    }
                    float num7 = (float)Math.Sqrt((double)num4 * (double)num4 + (double)num6 * (double)num6);
                    if ((double)num7 > 1000.0)
                    {
                        proj.position.X += num4;
                        proj.position.Y += num6;
                    }
                    if ((double)proj.localAI[0] == 1.0)
                    {
                        if ((double)num7 < 10.0 && (double)Math.Abs(guardian.Velocity.X) + (double)Math.Abs(guardian.Velocity.Y) < (double)num3 && (double)guardian.Velocity.Y == 0.0)
                            proj.localAI[0] = 0.0f;
                        float num8 = 12f;
                        if ((double)num7 < (double)num8)
                        {
                            proj.velocity.X = num4;
                            proj.velocity.Y = num6;
                        }
                        else
                        {
                            float num9 = num8 / num7;
                            proj.velocity.X = num4 * num9;
                            proj.velocity.Y = num6 * num9;
                        }
                        if ((double)proj.velocity.X > 0.5)
                            proj.direction = -1;
                        else if ((double)proj.velocity.X < -0.5)
                            proj.direction = 1;
                        proj.spriteDirection = proj.direction;
                        proj.rotation -= (float)(0.200000002980232 + (double)Math.Abs(proj.velocity.X) * 0.025000000372529) * (float)proj.direction;
                        ++proj.frameCounter;
                        if (proj.frameCounter > 3)
                        {
                            ++proj.frame;
                            proj.frameCounter = 0;
                        }
                        if (proj.frame < 5)
                            proj.frame = 5;
                        if (proj.frame > 9)
                            proj.frame = 5;
                        for (int index1 = 0; index1 < 2; ++index1)
                        {
                            int index2 = Dust.NewDust(new Vector2(proj.position.X + 3f, proj.position.Y + 4f), 14, 14, 156, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].velocity *= 0.2f;
                            Main.dust[index2].noGravity = true;
                            Main.dust[index2].scale = 1.25f;
                            Main.dust[index2].shader = GameShaders.Armor.GetSecondaryShader(Main.player[proj.owner].cLight, Main.player[proj.owner]);
                        }
                    }
                    else
                    {
                        if ((double)num7 > 200.0)
                            proj.localAI[0] = 1f;
                        if ((double)proj.velocity.X > 0.5)
                            proj.direction = -1;
                        else if ((double)proj.velocity.X < -0.5)
                            proj.direction = 1;
                        proj.spriteDirection = proj.direction;
                        if ((double)num7 < 10.0)
                        {
                            proj.velocity.X = num4;
                            proj.velocity.Y = num6;
                            proj.rotation = proj.velocity.X * 0.05f;
                            if ((double)num7 < (double)num3)
                            {
                                proj.position = proj.position + proj.velocity;
                                proj.velocity = Vector2.Zero;
                                num2 = 0.0f;
                            }
                            proj.direction = -guardian.Direction;
                        }
                        float num8 = num3 / num7;
                        float num9 = num4 * num8;
                        float num10 = num6 * num8;
                        if ((double)proj.velocity.X < (double)num9)
                        {
                            proj.velocity.X += num2;
                            if ((double)proj.velocity.X < 0.0)
                                proj.velocity.X *= 0.99f;
                        }
                        if ((double)proj.velocity.X > (double)num9)
                        {
                            proj.velocity.X -= num2;
                            if ((double)proj.velocity.X > 0.0)
                                proj.velocity.X *= 0.99f;
                        }
                        if ((double)proj.velocity.Y < (double)num10)
                        {
                            proj.velocity.Y += num2;
                            if ((double)proj.velocity.Y < 0.0)
                                proj.velocity.Y *= 0.99f;
                        }
                        if ((double)proj.velocity.Y > (double)num10)
                        {
                            proj.velocity.Y -= num2;
                            if ((double)proj.velocity.Y > 0.0)
                                proj.velocity.Y *= 0.99f;
                        }
                        if ((double)proj.velocity.X != 0.0 || (double)proj.velocity.Y != 0.0)
                            proj.rotation = proj.velocity.X * 0.05f;
                        ++proj.frameCounter;
                        if (proj.frameCounter > 3)
                        {
                            ++proj.frame;
                            proj.frameCounter = 0;
                        }
                        if (proj.frame <= 4)
                            return;
                        proj.frame = 0;
                    }
                }
                else if (proj.type == 199)
                {
                    float num2 = 0.1f;
                    proj.tileCollide = false;
                    int num3 = 200;
                    Vector2 vector2 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                    float num4 = guardian.Position.X - vector2.X;
                    float num5 = guardian.Position.Y - guardian.Height * 0.5f - vector2.Y - 60f;
                    float num6 = num4 - 2f;
                    if (proj.type == (int)sbyte.MaxValue)
                        num5 = guardian.Position.Y - guardian.Height - vector2.Y;
                    float num7 = (float)Math.Sqrt((double)num6 * (double)num6 + (double)num5 * (double)num5);
                    float num8 = 4f;
                    float num9 = num7;
                    if ((double)num7 < (double)num3 && (double)guardian.Velocity.Y == 0.0 && ((double)proj.position.Y + (double)proj.height <= (double)guardian.Position.Y && !Collision.SolidCollision(proj.position, proj.width, proj.height)))
                    {
                        proj.ai[0] = 0.0f;
                        if ((double)proj.velocity.Y < -6.0)
                            proj.velocity.Y = -6f;
                    }
                    if ((double)num7 < 4.0)
                    {
                        proj.velocity.X = num6;
                        proj.velocity.Y = num5;
                        num2 = 0.0f;
                    }
                    else
                    {
                        if ((double)num7 > 350.0)
                        {
                            num2 = 0.2f;
                            num8 = 10f;
                        }
                        float num10 = num8 / num7;
                        num6 *= num10;
                        num5 *= num10;
                    }
                    if ((double)proj.velocity.X < (double)num6)
                    {
                        proj.velocity.X += num2;
                        if ((double)proj.velocity.X < 0.0)
                            proj.velocity.X += num2;
                    }
                    if ((double)proj.velocity.X > (double)num6)
                    {
                        proj.velocity.X -= num2;
                        if ((double)proj.velocity.X > 0.0)
                            proj.velocity.X -= num2;
                    }
                    if ((double)proj.velocity.Y < (double)num5)
                    {
                        proj.velocity.Y += num2;
                        if ((double)proj.velocity.Y < 0.0)
                            proj.velocity.Y += num2;
                    }
                    if ((double)proj.velocity.Y > (double)num5)
                    {
                        proj.velocity.Y -= num2;
                        if ((double)proj.velocity.Y > 0.0)
                            proj.velocity.Y -= num2;
                    }
                    proj.direction = -guardian.Direction;
                    proj.spriteDirection = 1;
                    proj.rotation = proj.velocity.Y * 0.05f * (float)-proj.direction;
                    if ((double)num9 >= 50.0)
                    {
                        ++proj.frameCounter;
                        if (proj.frameCounter <= 6)
                            return;
                        proj.frameCounter = 0;
                        if ((double)proj.velocity.X < 0.0)
                        {
                            if (proj.frame < 2)
                                ++proj.frame;
                            if (proj.frame <= 2)
                                return;
                            --proj.frame;
                        }
                        else
                        {
                            if (proj.frame < 6)
                                ++proj.frame;
                            if (proj.frame <= 6)
                                return;
                            --proj.frame;
                        }
                    }
                    else
                    {
                        ++proj.frameCounter;
                        if (proj.frameCounter > 6)
                        {
                            proj.frame += proj.direction;
                            proj.frameCounter = 0;
                        }
                        if (proj.frame > 7)
                            proj.frame = 0;
                        if (proj.frame >= 0)
                            return;
                        proj.frame = 7;
                    }
                }
                else
                {
                    if ((double)proj.ai[1] == 0.0)
                    {
                        int num2 = 500;
                        if (proj.type == (int)sbyte.MaxValue)
                            num2 = 200;
                        if (proj.type == 208)
                            num2 = 300;
                        if (flag5 || proj.type == 266 || proj.type >= 390 && proj.type <= 392)
                        {
                            num2 += 40 * proj.minionPos;
                            if ((double)proj.localAI[0] > 0.0)
                                num2 += 500;
                            if (proj.type == 266 && (double)proj.localAI[0] > 0.0)
                                num2 += 100;
                            if (proj.type >= 390 && proj.type <= 392 && (double)proj.localAI[0] > 0.0)
                                num2 += 400;
                        }
                        num2 += guardian.Height - 42;
                        if (guardian.HasCooldown(GuardianCooldownManager.CooldownType.RocketDelay))
                            proj.ai[0] = 1f;
                        Vector2 vector2 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                        float num3 = guardian.Position.X - vector2.X;
                        float num4 = guardian.Position.Y - guardian.Height * 0.5f - vector2.Y;
                        float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
                        if ((double)num5 > 2000.0)
                        {
                            proj.position.X = guardian.Position.X - (float)(proj.width / 2);
                            proj.position.Y = guardian.Position.Y - guardian.Height * 0.5f - (float)(proj.height / 2);
                        }
                        else if ((double)num5 > (double)num2 || (double)Math.Abs(num4) > 300.0 && (!flag5 && proj.type != 266 && (proj.type < 390 || proj.type > 392) || (double)proj.localAI[0] <= 0.0))
                        {
                            if (proj.type != 324)
                            {
                                if ((double)num4 > 0.0 && (double)proj.velocity.Y < 0.0)
                                    proj.velocity.Y = 0.0f;
                                if ((double)num4 < 0.0 && (double)proj.velocity.Y > 0.0)
                                    proj.velocity.Y = 0.0f;
                            }
                            proj.ai[0] = 1f;
                        }
                    }
                    if (proj.type == 209 && (double)proj.ai[0] != 0.0)
                    {
                        if ((double)guardian.Velocity.Y == 0.0 && proj.alpha >= 100)
                        {
                            proj.position.X = guardian.Position.X - (float)(proj.width / 2);
                            proj.position.Y = guardian.Position.Y - (float)proj.height;
                            proj.ai[0] = 0.0f;
                        }
                        else
                        {
                            proj.velocity.X = 0.0f;
                            proj.velocity.Y = 0.0f;
                            proj.alpha += 5;
                            if (proj.alpha <= (int)byte.MaxValue)
                                return;
                            proj.alpha = (int)byte.MaxValue;
                        }
                    }
                    else if ((double)proj.ai[0] != 0.0)
                    {
                        float num2 = 0.2f;
                        int num3 = 200;
                        if (proj.type == (int)sbyte.MaxValue)
                            num3 = 100;
                        if (flag5)
                        {
                            num2 = 0.5f;
                            num3 = 100;
                        }
                        proj.tileCollide = false;
                        Vector2 vector2_1 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                        float num4 = guardian.Position.X - vector2_1.X;
                        if (flag5 || proj.type == 266 || proj.type >= 390 && proj.type <= 392)
                        {
                            num4 -= (float)(40 * guardian.Direction);
                            float num5 = 700f;
                            if (flag5)
                                num5 += 100f;
                            bool flag6 = false;
                            int num6 = -1;
                            for (int index = 0; index < 200; ++index)
                            {
                                if (Main.npc[index].CanBeChasedBy((object)proj, false))
                                {
                                    float num7 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                                    float num8 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                                    if ((double)(Math.Abs(guardian.Position.X - num7) + Math.Abs(guardian.Position.Y - guardian.Height * 0.5f - num8)) < (double)num5)
                                    {
                                        if (Collision.CanHit(proj.position, proj.width, proj.height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                                            num6 = index;
                                        flag6 = true;
                                        break;
                                    }
                                }
                            }
                            if (!flag6)
                                num4 -= (float)(40 * proj.minionPos * guardian.Direction);
                            if (flag6 && num6 >= 0)
                                proj.ai[0] = 0.0f;
                        }
                        float num9 = guardian.Position.Y - guardian.Height * 0.5f - vector2_1.Y;
                        if (proj.type == (int)sbyte.MaxValue)
                            num9 = guardian.Position.Y - guardian.Height - vector2_1.Y;
                        float num10 = (float)Math.Sqrt((double)num4 * (double)num4 + (double)num9 * (double)num9);
                        float num11 = 10f;
                        float num12 = num10;
                        if (proj.type == 111)
                            num11 = 11f;
                        if (proj.type == (int)sbyte.MaxValue)
                            num11 = 9f;
                        if (proj.type == 324)
                            num11 = 20f;
                        if (flag5)
                        {
                            num2 = 0.4f;
                            num11 = 12f;
                            if ((double)num11 < (double)Math.Abs(guardian.Velocity.X) + (double)Math.Abs(guardian.Velocity.Y))
                                num11 = Math.Abs(guardian.Velocity.X) + Math.Abs(guardian.Velocity.Y);
                        }
                        if (proj.type == 208 && (double)Math.Abs(guardian.Velocity.X) + (double)Math.Abs(guardian.Velocity.Y) > 4.0)
                            num3 = -1;
                        if ((double)num10 < (double)num3 && (double)guardian.Velocity.Y == 0.0 && ((double)proj.position.Y + (double)proj.height <= (double)guardian.Position.Y && !Collision.SolidCollision(proj.position, proj.width, proj.height)))
                        {
                            proj.ai[0] = 0.0f;
                            if ((double)proj.velocity.Y < -6.0)
                                proj.velocity.Y = -6f;
                        }
                        float num13;
                        float num14;
                        if ((double)num10 < 60.0)
                        {
                            num13 = proj.velocity.X;
                            num14 = proj.velocity.Y;
                        }
                        else
                        {
                            float num5 = num11 / num10;
                            num13 = num4 * num5;
                            num14 = num9 * num5;
                        }
                        if (proj.type == 324)
                        {
                            if ((double)num12 > 1000.0)
                            {
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < (double)num11 - 1.25)
                                {
                                    proj.velocity *= 1.025f;
                                }
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) > (double)num11 + 1.25)
                                {
                                    proj.velocity *= 0.975f;
                                }
                            }
                            else if ((double)num12 > 600.0)
                            {
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < (double)num11 - 1.0)
                                {
                                    proj.velocity *= 1.05f;
                                }
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) > (double)num11 + 1.0)
                                {
                                    proj.velocity *= 0.95f;
                                }
                            }
                            else if ((double)num12 > 400.0)
                            {
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < (double)num11 - 0.5)
                                {
                                    proj.velocity *= 1.075f;
                                }
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) > (double)num11 + 0.5)
                                {
                                    proj.velocity *= 0.925f;
                                }
                            }
                            else
                            {
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < (double)num11 - 0.25)
                                {
                                    proj.velocity *= 1.1f;
                                }
                                if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) > (double)num11 + 0.25)
                                {
                                    proj.velocity *= 0.9f;
                                }
                            }
                            proj.velocity.X = (float)(((double)proj.velocity.X * 34.0 + (double)num13) / 35.0);
                            proj.velocity.Y = (float)(((double)proj.velocity.Y * 34.0 + (double)num14) / 35.0);
                        }
                        else
                        {
                            if ((double)proj.velocity.X < (double)num13)
                            {
                                proj.velocity.X += num2;
                                if ((double)proj.velocity.X < 0.0)
                                    proj.velocity.X += num2 * 1.5f;
                            }
                            if ((double)proj.velocity.X > (double)num13)
                            {
                                proj.velocity.X -= num2;
                                if ((double)proj.velocity.X > 0.0)
                                    proj.velocity.X -= num2 * 1.5f;
                            }
                            if ((double)proj.velocity.Y < (double)num14)
                            {
                                proj.velocity.Y += num2;
                                if ((double)proj.velocity.Y < 0.0)
                                    proj.velocity.Y += num2 * 1.5f;
                            }
                            if ((double)proj.velocity.Y > (double)num14)
                            {
                                proj.velocity.Y -= num2;
                                if ((double)proj.velocity.Y > 0.0)
                                    proj.velocity.Y -= num2 * 1.5f;
                            }
                        }
                        if (proj.type == 111)
                            proj.frame = 7;
                        if (proj.type == 112)
                            proj.frame = 2;
                        if (flag5 && proj.frame < 12)
                        {
                            proj.frame = Main.rand.Next(12, 18);
                            proj.frameCounter = 0;
                        }
                        if (proj.type != 313)
                        {
                            if ((double)proj.velocity.X > 0.5)
                                proj.spriteDirection = -1;
                            else if ((double)proj.velocity.X < -0.5)
                                proj.spriteDirection = 1;
                        }
                        if (proj.type == 398)
                        {
                            if ((double)proj.velocity.X > 0.5)
                                proj.spriteDirection = 1;
                            else if ((double)proj.velocity.X < -0.5)
                                proj.spriteDirection = -1;
                        }
                        if (proj.type == 112)
                            proj.rotation = proj.spriteDirection != -1 ? (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.57f : (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.57f;
                        else if (proj.type >= 390 && proj.type <= 392)
                        {
                            int index1 = (int)((double)proj.Center.X / 16.0);
                            int index2 = (int)((double)proj.Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].wall > 0)
                            {
                                proj.rotation = proj.velocity.ToRotation() + 1.570796f;
                                proj.frameCounter = proj.frameCounter + (int)((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y));
                                if (proj.frameCounter > 5)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 7)
                                    proj.frame = 4;
                                if (proj.frame < 4)
                                    proj.frame = 7;
                            }
                            else
                            {
                                proj.frameCounter = proj.frameCounter + 1;
                                if (proj.frameCounter > 2)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame < 8 || proj.frame > 10)
                                    proj.frame = 8;
                                proj.rotation = proj.velocity.X * 0.1f;
                            }
                        }
                        else if (proj.type == 334)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 1)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 7 || proj.frame > 10)
                                proj.frame = 7;
                            proj.rotation = proj.velocity.X * 0.1f;
                        }
                        else if (proj.type == 353)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 10 || proj.frame > 13)
                                proj.frame = 10;
                            proj.rotation = proj.velocity.X * 0.05f;
                        }
                        else if (proj.type == (int)sbyte.MaxValue)
                        {
                            proj.frameCounter = proj.frameCounter + 3;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame <= 5 || proj.frame > 15)
                                proj.frame = 6;
                            proj.rotation = proj.velocity.X * 0.1f;
                        }
                        else if (proj.type == 269)
                        {
                            if (proj.frame == 6)
                                proj.frameCounter = 0;
                            else if (proj.frame < 4 || proj.frame > 6)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 4;
                            }
                            else
                            {
                                proj.frameCounter = proj.frameCounter + 1;
                                if (proj.frameCounter > 6)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                            }
                            proj.rotation = proj.velocity.X * 0.05f;
                        }
                        else if (proj.type == 266)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 2 || proj.frame > 5)
                                proj.frame = 2;
                            proj.rotation = proj.velocity.X * 0.1f;
                        }
                        else if (proj.type == 324)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 1)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 6 || proj.frame > 9)
                                proj.frame = 6;
                            proj.rotation = (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.58f;
                            Lighting.AddLight((int)proj.Center.X / 16, (int)proj.Center.Y / 16, 0.9f, 0.6f, 0.2f);
                            for (int index1 = 0; index1 < 2; ++index1)
                            {
                                int num5 = 4;
                                int index2 = Dust.NewDust(new Vector2(proj.Center.X - (float)num5, proj.Center.Y - (float)num5) - proj.velocity * 0.0f, num5 * 2, num5 * 2, 6, 0.0f, 0.0f, 100, new Color(), 1f);
                                Main.dust[index2].scale *= (float)(1.79999995231628 + (double)Main.rand.Next(10) * 0.100000001490116);
                                Main.dust[index2].velocity *= 0.2f;
                                if (index1 == 1)
                                    Main.dust[index2].position -= proj.velocity * 0.5f;
                                Main.dust[index2].noGravity = true;
                                int index3 = Dust.NewDust(new Vector2(proj.Center.X - (float)num5, proj.Center.Y - (float)num5) - proj.velocity * 0.0f, num5 * 2, num5 * 2, 31, 0.0f, 0.0f, 100, new Color(), 0.5f);
                                Main.dust[index3].fadeIn = (float)(1.0 + (double)Main.rand.Next(5) * 0.100000001490116);
                                Main.dust[index3].velocity *= 0.05f;
                                if (index1 == 1)
                                    Main.dust[index3].position -= proj.velocity * 0.5f;
                            }
                        }
                        else if (proj.type == 268)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 4)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 6 || proj.frame > 7)
                                proj.frame = 6;
                            proj.rotation = (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.58f;
                        }
                        else if (proj.type == 200)
                        {
                            proj.frameCounter = proj.frameCounter + 3;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame <= 5 || proj.frame > 9)
                                proj.frame = 6;
                            proj.rotation = proj.velocity.X * 0.1f;
                        }
                        else if (proj.type == 208)
                        {
                            proj.rotation = proj.velocity.X * 0.075f;
                            ++proj.frameCounter;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame > 4)
                                proj.frame = 1;
                            if (proj.frame < 1)
                                proj.frame = 1;
                        }
                        else if (proj.type == 236)
                        {
                            proj.rotation = proj.velocity.Y * 0.05f * (float)proj.direction;
                            if ((double)proj.velocity.Y < 0.0)
                                proj.frameCounter += 2;
                            else
                                ++proj.frameCounter;
                            if (proj.frameCounter >= 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame > 12)
                                proj.frame = 9;
                            if (proj.frame < 9)
                                proj.frame = 9;
                        }
                        else if (proj.type == 499)
                        {
                            proj.rotation = proj.velocity.Y * 0.05f * (float)proj.direction;
                            if ((double)proj.velocity.Y < 0.0)
                                proj.frameCounter += 2;
                            else
                                ++proj.frameCounter;
                            if (proj.frameCounter >= 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame >= 12)
                                proj.frame = 8;
                            if (proj.frame < 8)
                                proj.frame = 8;
                        }
                        else if (proj.type == 314)
                        {
                            proj.rotation = (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.58f;
                            ++proj.frameCounter;
                            if (proj.frameCounter >= 3)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame > 12)
                                proj.frame = 7;
                            if (proj.frame < 7)
                                proj.frame = 7;
                        }
                        else if (proj.type == 319)
                        {
                            proj.rotation = proj.velocity.X * 0.05f;
                            ++proj.frameCounter;
                            if (proj.frameCounter >= 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame > 10)
                                proj.frame = 6;
                            if (proj.frame < 6)
                                proj.frame = 6;
                        }
                        else if (proj.type == 210)
                        {
                            proj.rotation = (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 1.58f;
                            proj.frameCounter += 3;
                            if (proj.frameCounter > 6)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame > 11)
                                proj.frame = 7;
                            if (proj.frame < 7)
                                proj.frame = 7;
                        }
                        else if (proj.type == 313)
                        {
                            proj.position.Y += (float)proj.height;
                            proj.height = 54;
                            proj.position.Y -= (float)proj.height;
                            proj.position.X += (float)(proj.width / 2);
                            proj.width = 54;
                            proj.position.X -= (float)(proj.width / 2);
                            proj.rotation += proj.velocity.X * 0.01f;
                            proj.frameCounter = 0;
                            proj.frame = 11;
                        }
                        else if (proj.type == 398)
                        {
                            proj.frameCounter = proj.frameCounter + 1;
                            if (proj.frameCounter > 1)
                            {
                                ++proj.frame;
                                proj.frameCounter = 0;
                            }
                            if (proj.frame < 6 || proj.frame > 9)
                                proj.frame = 6;
                            proj.rotation = proj.velocity.X * 0.1f;
                        }
                        else
                            proj.rotation = proj.spriteDirection != -1 ? (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X) + 3.14f : (float)Math.Atan2((double)proj.velocity.Y, (double)proj.velocity.X);
                        if (flag5 || proj.type == 499 || (proj.type == 398 || proj.type == 390) || (proj.type == 391 || proj.type == 392 || (proj.type == (int)sbyte.MaxValue || proj.type == 200)) || (proj.type == 208 || proj.type == 210 || (proj.type == 236 || proj.type == 266) || (proj.type == 268 || proj.type == 269 || (proj.type == 313 || proj.type == 314))) || (proj.type == 319 || proj.type == 324 || (proj.type == 334 || proj.type == 353)))
                            return;
                        int index4 = Dust.NewDust(new Vector2((float)((double)proj.position.X + (double)(proj.width / 2) - 4.0), (float)((double)proj.position.Y + (double)(proj.height / 2) - 4.0)) - proj.velocity, 8, 8, 16, (float)(-(double)proj.velocity.X * 0.5), proj.velocity.Y * 0.5f, 50, new Color(), 1.7f);
                        Main.dust[index4].velocity.X = Main.dust[index4].velocity.X * 0.2f;
                        Main.dust[index4].velocity.Y = Main.dust[index4].velocity.Y * 0.2f;
                        Main.dust[index4].noGravity = true;
                    }
                    else
                    {
                        if (flag5)
                        {
                            float num2 = (float)(40 * proj.minionPos);
                            int num3 = 30;
                            int num4 = 60;
                            --proj.localAI[0];
                            if ((double)proj.localAI[0] < 0.0)
                                proj.localAI[0] = 0.0f;
                            if ((double)proj.ai[1] > 0.0)
                            {
                                --proj.ai[1];
                            }
                            else
                            {
                                float num5 = proj.position.X;
                                float num6 = proj.position.Y;
                                float num7 = 100000f;
                                float num8 = num7;
                                int num9 = -1;
                                NPC minionAttackTargetNpc = proj.OwnerMinionAttackTargetNPC;
                                if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy((object)proj, false))
                                {
                                    float num10 = minionAttackTargetNpc.position.X + (float)(minionAttackTargetNpc.width / 2);
                                    float num11 = minionAttackTargetNpc.position.Y + (float)(minionAttackTargetNpc.height / 2);
                                    float num12 = Math.Abs(proj.position.X + (float)(proj.width / 2) - num10) + Math.Abs(proj.position.Y + (float)(proj.height / 2) - num11);
                                    if ((double)num12 < (double)num7)
                                    {
                                        if (num9 == -1 && (double)num12 <= (double)num8)
                                        {
                                            num8 = num12;
                                            num5 = num10;
                                            num6 = num11;
                                        }
                                        if (Collision.CanHit(proj.position, proj.width, proj.height, minionAttackTargetNpc.position, minionAttackTargetNpc.width, minionAttackTargetNpc.height))
                                        {
                                            num7 = num12;
                                            num5 = num10;
                                            num6 = num11;
                                            num9 = minionAttackTargetNpc.whoAmI;
                                        }
                                    }
                                }
                                if (num9 == -1)
                                {
                                    for (int index = 0; index < 200; ++index)
                                    {
                                        if (Main.npc[index].CanBeChasedBy((object)proj, false))
                                        {
                                            float num10 = Main.npc[index].position.X + (float)(Main.npc[index].width / 2);
                                            float num11 = Main.npc[index].position.Y + (float)(Main.npc[index].height / 2);
                                            float num12 = Math.Abs(proj.position.X + (float)(proj.width / 2) - num10) + Math.Abs(proj.position.Y + (float)(proj.height / 2) - num11);
                                            if ((double)num12 < (double)num7)
                                            {
                                                if (num9 == -1 && (double)num12 <= (double)num8)
                                                {
                                                    num8 = num12;
                                                    num5 = num10;
                                                    num6 = num11;
                                                }
                                                if (Collision.CanHit(proj.position, proj.width, proj.height, Main.npc[index].position, Main.npc[index].width, Main.npc[index].height))
                                                {
                                                    num7 = num12;
                                                    num5 = num10;
                                                    num6 = num11;
                                                    num9 = index;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (num9 == -1 && (double)num8 < (double)num7)
                                    num7 = num8;
                                float num13 = 400f;
                                if ((double)proj.position.Y > Main.worldSurface * 16.0)
                                    num13 = 200f;
                                if ((double)num7 < (double)num13 + (double)num2 && num9 == -1)
                                {
                                    float num10 = num5 - (proj.position.X + (float)(proj.width / 2));
                                    if ((double)num10 < -5.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if ((double)num10 > 5.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                }
                                else if (num9 >= 0 && (double)num7 < 800.0 + (double)num2)
                                {
                                    proj.localAI[0] = (float)num4;
                                    float num10 = num5 - (proj.position.X + (float)(proj.width / 2));
                                    if ((double)num10 > 300.0 || (double)num10 < -300.0)
                                    {
                                        if ((double)num10 < -50.0)
                                        {
                                            flag1 = true;
                                            flag2 = false;
                                        }
                                        else if ((double)num10 > 50.0)
                                        {
                                            flag2 = true;
                                            flag1 = false;
                                        }
                                    }
                                    else if (proj.owner == Main.myPlayer)
                                    {
                                        proj.ai[1] = (float)num3;
                                        float num11 = 12f;
                                        Vector2 vector2 = new Vector2(proj.position.X + (float)proj.width * 0.5f, (float)((double)proj.position.Y + (double)(proj.height / 2) - 8.0));
                                        float num12 = num5 - vector2.X + (float)Main.rand.Next(-20, 21);
                                        float num14 = (float)((double)(Math.Abs(num12) * 0.1f) * (double)Main.rand.Next(0, 100) * (1.0 / 1000.0));
                                        float num15 = num6 - vector2.Y + (float)Main.rand.Next(-20, 21) - num14;
                                        float num16 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num15 * (double)num15);
                                        float num17 = num11 / num16;
                                        float SpeedX = num12 * num17;
                                        float SpeedY = num15 * num17;
                                        int damage = proj.damage;
                                        int Type = 195;
                                        int index = Projectile.NewProjectile(vector2.X, vector2.Y, SpeedX, SpeedY, Type, damage, proj.knockBack, Main.myPlayer, 0.0f, 0.0f);
                                        Main.projectile[index].timeLeft = 300;
                                        if ((double)SpeedX < 0.0)
                                            proj.direction = -1;
                                        if ((double)SpeedX > 0.0)
                                            proj.direction = 1;
                                        proj.netUpdate = true;
                                    }
                                }
                            }
                        }
                        bool flag6 = false;
                        Vector2 vector2_1 = Vector2.Zero;
                        bool flag7 = false;
                        if (proj.type == 266 || proj.type >= 390 && proj.type <= 392)
                        {
                            float num2 = (float)(40 * proj.minionPos);
                            int num3 = 60;
                            --proj.localAI[0];
                            if ((double)proj.localAI[0] < 0.0)
                                proj.localAI[0] = 0.0f;
                            if ((double)proj.ai[1] > 0.0)
                            {
                                --proj.ai[1];
                            }
                            else
                            {
                                float x1 = proj.position.X;
                                float y1 = proj.position.Y;
                                float num4 = 100000f;
                                float num5 = num4;
                                int index1 = -1;
                                NPC minionAttackTargetNpc = proj.OwnerMinionAttackTargetNPC;
                                if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy((object)proj, false))
                                {
                                    float x2 = minionAttackTargetNpc.Center.X;
                                    float y2 = minionAttackTargetNpc.Center.Y;
                                    float num6 = Math.Abs(proj.position.X + (float)(proj.width / 2) - x2) + Math.Abs(proj.position.Y + (float)(proj.height / 2) - y2);
                                    if ((double)num6 < (double)num4)
                                    {
                                        if (index1 == -1 && (double)num6 <= (double)num5)
                                        {
                                            num5 = num6;
                                            x1 = x2;
                                            y1 = y2;
                                        }
                                        if (Collision.CanHit(proj.position, proj.width, proj.height, minionAttackTargetNpc.position, minionAttackTargetNpc.width, minionAttackTargetNpc.height))
                                        {
                                            num4 = num6;
                                            x1 = x2;
                                            y1 = y2;
                                            index1 = minionAttackTargetNpc.whoAmI;
                                        }
                                    }
                                }
                                if (index1 == -1)
                                {
                                    for (int index2 = 0; index2 < 200; ++index2)
                                    {
                                        if (Main.npc[index2].CanBeChasedBy((object)proj, false))
                                        {
                                            float num6 = Main.npc[index2].position.X + (float)(Main.npc[index2].width / 2);
                                            float num7 = Main.npc[index2].position.Y + (float)(Main.npc[index2].height / 2);
                                            float num8 = Math.Abs(proj.position.X + (float)(proj.width / 2) - num6) + Math.Abs(proj.position.Y + (float)(proj.height / 2) - num7);
                                            if ((double)num8 < (double)num4)
                                            {
                                                if (index1 == -1 && (double)num8 <= (double)num5)
                                                {
                                                    num5 = num8;
                                                    x1 = num6;
                                                    y1 = num7;
                                                }
                                                if (Collision.CanHit(proj.position, proj.width, proj.height, Main.npc[index2].position, Main.npc[index2].width, Main.npc[index2].height))
                                                {
                                                    num4 = num8;
                                                    x1 = num6;
                                                    y1 = num7;
                                                    index1 = index2;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (proj.type >= 390 && proj.type <= 392 && !Collision.SolidCollision(proj.position, proj.width, proj.height))
                                    proj.tileCollide = true;
                                if (index1 == -1 && (double)num5 < (double)num4)
                                    num4 = num5;
                                else if (index1 >= 0)
                                {
                                    flag6 = true;
                                    vector2_1 = new Vector2(x1, y1) - proj.Center;
                                    if (proj.type >= 390 && proj.type <= 392)
                                    {
                                        if ((double)Main.npc[index1].position.Y > (double)proj.position.Y + (double)proj.height)
                                        {
                                            int index2 = (int)((double)proj.Center.X / 16.0);
                                            int index3 = (int)(((double)proj.position.Y + (double)proj.height + 1.0) / 16.0);
                                            if (Main.tile[index2, index3] != null && Main.tile[index2, index3].active() && TileID.Sets.Platforms[(int)Main.tile[index2, index3].type])
                                                proj.tileCollide = false;
                                        }
                                        Microsoft.Xna.Framework.Rectangle rectangle1 = new Microsoft.Xna.Framework.Rectangle((int)proj.position.X, (int)proj.position.Y, proj.width, proj.height);
                                        Microsoft.Xna.Framework.Rectangle rectangle2 = new Microsoft.Xna.Framework.Rectangle((int)Main.npc[index1].position.X, (int)Main.npc[index1].position.Y, Main.npc[index1].width, Main.npc[index1].height);
                                        int num6 = 10;
                                        rectangle2.X -= num6;
                                        rectangle2.Y -= num6;
                                        rectangle2.Width += num6 * 2;
                                        rectangle2.Height += num6 * 2;
                                        if (rectangle1.Intersects(rectangle2))
                                        {
                                            flag7 = true;
                                            Vector2 v = Main.npc[index1].Center - proj.Center;
                                            if ((double)proj.velocity.Y > 0.0 && (double)v.Y < 0.0)
                                                proj.velocity.Y *= 0.5f;
                                            if ((double)proj.velocity.Y < 0.0 && (double)v.Y > 0.0)
                                                proj.velocity.Y *= 0.5f;
                                            if ((double)proj.velocity.X > 0.0 && (double)v.X < 0.0)
                                                proj.velocity.X *= 0.5f;
                                            if ((double)proj.velocity.X < 0.0 && (double)v.X > 0.0)
                                                proj.velocity.X *= 0.5f;
                                            if ((double)v.Length() > 14.0)
                                            {
                                                v.Normalize();
                                                v *= 14f;
                                            }
                                            proj.rotation = (float)(((double)proj.rotation * 5.0 + (double)v.ToRotation() + 1.57079637050629) / 6.0);
                                            proj.velocity = (proj.velocity * 9f + v) / 10f;
                                            for (int index2 = 0; index2 < 1000; ++index2)
                                            {
                                                if (proj.whoAmI != index2 && proj.owner == Main.projectile[index2].owner && GuardianProj.ContainsKey(index2) && GuardianProj[index2] == guardian && (Main.projectile[index2].type >= 390 && Main.projectile[index2].type <= 392) && (double)(Main.projectile[index2].Center - proj.Center).Length() < 15.0)
                                                {
                                                    float num7 = 0.5f;
                                                    if ((double)proj.Center.Y > (double)Main.projectile[index2].Center.Y)
                                                    {
                                                        Main.projectile[index2].velocity.Y -= num7;
                                                        proj.velocity.Y += num7;
                                                    }
                                                    else
                                                    {
                                                        Main.projectile[index2].velocity.Y += num7;
                                                        proj.velocity.Y -= num7;
                                                    }
                                                    if ((double)proj.Center.X > (double)Main.projectile[index2].Center.X)
                                                    {
                                                        proj.velocity.X += num7;
                                                        Main.projectile[index2].velocity.X -= num7;
                                                    }
                                                    else
                                                    {
                                                        proj.velocity.X -= num7;
                                                        Main.projectile[index2].velocity.Y += num7;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                float num9 = 300f;
                                if ((double)proj.position.Y > Main.worldSurface * 16.0)
                                    num9 = 150f;
                                if (proj.type >= 390 && proj.type <= 392)
                                {
                                    num9 = 500f;
                                    if ((double)proj.position.Y > Main.worldSurface * 16.0)
                                        num9 = 250f;
                                }
                                if ((double)num4 < (double)num9 + (double)num2 && index1 == -1)
                                {
                                    float num6 = x1 - (proj.position.X + (float)(proj.width / 2));
                                    if ((double)num6 < -5.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if ((double)num6 > 5.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                }
                                bool flag8 = false;
                                if (proj.type >= 390 && proj.type <= 392 && (double)proj.localAI[1] > 0.0)
                                {
                                    flag8 = true;
                                    --proj.localAI[1];
                                }
                                if (index1 >= 0 && (double)num4 < 800.0 + (double)num2)
                                {
                                    proj.friendly = true;
                                    proj.localAI[0] = (float)num3;
                                    float num6 = x1 - (proj.position.X + (float)(proj.width / 2));
                                    if ((double)num6 < -10.0)
                                    {
                                        flag1 = true;
                                        flag2 = false;
                                    }
                                    else if ((double)num6 > 10.0)
                                    {
                                        flag2 = true;
                                        flag1 = false;
                                    }
                                    if ((double)y1 < (double)proj.Center.Y - 100.0 && (double)num6 > -50.0 && ((double)num6 < 50.0 && (double)proj.velocity.Y == 0.0))
                                    {
                                        float num7 = Math.Abs(y1 - proj.Center.Y);
                                        if ((double)num7 < 120.0)
                                            proj.velocity.Y = -10f;
                                        else if ((double)num7 < 210.0)
                                            proj.velocity.Y = -13f;
                                        else if ((double)num7 < 270.0)
                                            proj.velocity.Y = -15f;
                                        else if ((double)num7 < 310.0)
                                            proj.velocity.Y = -17f;
                                        else if ((double)num7 < 380.0)
                                            proj.velocity.Y = -18f;
                                    }
                                    if (flag8)
                                    {
                                        proj.friendly = false;
                                        if ((double)proj.velocity.X < 0.0)
                                            flag1 = true;
                                        else if ((double)proj.velocity.X > 0.0)
                                            flag2 = true;
                                    }
                                }
                                else
                                    proj.friendly = false;
                            }
                        }
                        if ((double)proj.ai[1] != 0.0)
                        {
                            flag1 = false;
                            flag2 = false;
                        }
                        else if (flag5 && (double)proj.localAI[0] == 0.0)
                            proj.direction = guardian.Direction;
                        else if (proj.type >= 390 && proj.type <= 392)
                        {
                            int index1 = (int)((double)proj.Center.X / 16.0);
                            int index2 = (int)((double)proj.Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].wall > 0)
                                flag1 = flag2 = false;
                        }
                        if (proj.type == (int)sbyte.MaxValue)
                        {
                            if ((double)proj.rotation > -0.1 && (double)proj.rotation < 0.1)
                                proj.rotation = 0.0f;
                            else if ((double)proj.rotation < 0.0)
                                proj.rotation += 0.1f;
                            else
                                proj.rotation -= 0.1f;
                        }
                        else if (proj.type != 313 && !flag7)
                            proj.rotation = 0.0f;
                        if (proj.type < 390 || proj.type > 392)
                            proj.tileCollide = true;
                        float num18 = 0.08f;
                        float num19 = 6.5f;
                        if (proj.type == (int)sbyte.MaxValue)
                        {
                            num19 = 2f;
                            num18 = 0.04f;
                        }
                        if (proj.type == 112)
                        {
                            num19 = 6f;
                            num18 = 0.06f;
                        }
                        if (proj.type == 334)
                        {
                            num19 = 8f;
                            num18 = 0.08f;
                        }
                        if (proj.type == 268)
                        {
                            num19 = 8f;
                            num18 = 0.4f;
                        }
                        if (proj.type == 324)
                        {
                            num18 = 0.1f;
                            num19 = 3f;
                        }
                        if (flag5 || proj.type == 266 || proj.type >= 390 && proj.type <= 392)
                        {
                            num19 = 6f;
                            num18 = 0.2f;
                            if ((double)num19 < (double)Math.Abs(guardian.Velocity.X) + (double)Math.Abs(guardian.Velocity.Y))
                            {
                                num19 = Math.Abs(guardian.Velocity.X) + Math.Abs(guardian.Velocity.Y);
                                num18 = 0.3f;
                            }
                        }
                        if (proj.type >= 390 && proj.type <= 392)
                            num18 *= 2f;
                        if (flag1)
                        {
                            if ((double)proj.velocity.X > -3.5)
                                proj.velocity.X -= num18;
                            else
                                proj.velocity.X -= num18 * 0.25f;
                        }
                        else if (flag2)
                        {
                            if ((double)proj.velocity.X < 3.5)
                                proj.velocity.X += num18;
                            else
                                proj.velocity.X += num18 * 0.25f;
                        }
                        else
                        {
                            proj.velocity.X *= 0.9f;
                            if ((double)proj.velocity.X >= -(double)num18 && (double)proj.velocity.X <= (double)num18)
                                proj.velocity.X = 0.0f;
                        }
                        if (proj.type == 208)
                        {
                            proj.velocity.X *= 0.95f;
                            if ((double)proj.velocity.X > -0.1 && (double)proj.velocity.X < 0.1)
                                proj.velocity.X = 0.0f;
                            flag1 = false;
                            flag2 = false;
                        }
                        if (flag1 || flag2)
                        {
                            int num2 = (int)((double)proj.position.X + (double)(proj.width / 2)) / 16;
                            int j = (int)((double)proj.position.Y + (double)(proj.height / 2)) / 16;
                            if (proj.type == 236)
                                num2 += proj.direction;
                            if (flag1)
                                --num2;
                            if (flag2)
                                ++num2;
                            if (WorldGen.SolidTile(num2 + (int)proj.velocity.X, j))
                                flag4 = true;
                        }
                        if ((double)guardian.Position.Y - 8.0 > (double)proj.position.Y + (double)proj.height)
                            flag3 = true;
                        if (proj.type == 268 && proj.frameCounter < 10)
                            flag4 = false;
                        Collision.StepUp(ref proj.position, ref proj.velocity, proj.width, proj.height, ref proj.stepSpeed, ref proj.gfxOffY, 1, false, 0);
                        if ((double)proj.velocity.Y == 0.0 || proj.type == 200)
                        {
                            if (!flag3 && ((double)proj.velocity.X < 0.0 || (double)proj.velocity.X > 0.0))
                            {
                                int i = (int)((double)proj.position.X + (double)(proj.width / 2)) / 16;
                                int j = (int)((double)proj.position.Y + (double)(proj.height / 2)) / 16 + 1;
                                if (flag1)
                                    --i;
                                if (flag2)
                                    ++i;
                                WorldGen.SolidTile(i, j);
                            }
                            if (flag4)
                            {
                                int i1 = (int)((double)proj.position.X + (double)(proj.width / 2)) / 16;
                                int j = (int)((double)proj.position.Y + (double)proj.height) / 16 + 1;
                                if (WorldGen.SolidTile(i1, j) || Main.tile[i1, j].halfBrick() || ((int)Main.tile[i1, j].slope() > 0 || proj.type == 200))
                                {
                                    if (proj.type == 200)
                                    {
                                        proj.velocity.Y = -3.1f;
                                    }
                                    else
                                    {
                                        try
                                        {
                                            int num2 = (int)((double)proj.position.X + (double)(proj.width / 2)) / 16;
                                            int num3 = (int)((double)proj.position.Y + (double)(proj.height / 2)) / 16;
                                            if (flag1)
                                                --num2;
                                            if (flag2)
                                                ++num2;
                                            int i2 = num2 + (int)proj.velocity.X;
                                            if (!WorldGen.SolidTile(i2, num3 - 1) && !WorldGen.SolidTile(i2, num3 - 2))
                                                proj.velocity.Y = -5.1f;
                                            else if (!WorldGen.SolidTile(i2, num3 - 2))
                                                proj.velocity.Y = -7.1f;
                                            else if (WorldGen.SolidTile(i2, num3 - 5))
                                                proj.velocity.Y = -11.1f;
                                            else if (WorldGen.SolidTile(i2, num3 - 4))
                                                proj.velocity.Y = -10.1f;
                                            else
                                                proj.velocity.Y = -9.1f;
                                        }
                                        catch
                                        {
                                            proj.velocity.Y = -9.1f;
                                        }
                                    }
                                    if (proj.type == (int)sbyte.MaxValue)
                                        proj.ai[0] = 1f;
                                }
                            }
                            else if (proj.type == 266 && (flag1 || flag2))
                                proj.velocity.Y -= 6f;
                        }
                        if ((double)proj.velocity.X > (double)num19)
                            proj.velocity.X = num19;
                        if ((double)proj.velocity.X < -(double)num19)
                            proj.velocity.X = -num19;
                        if ((double)proj.velocity.X < 0.0)
                            proj.direction = -1;
                        if ((double)proj.velocity.X > 0.0)
                            proj.direction = 1;
                        if ((double)proj.velocity.X > (double)num18 && flag2)
                            proj.direction = 1;
                        if ((double)proj.velocity.X < -(double)num18 && flag1)
                            proj.direction = -1;
                        if (proj.type != 313)
                        {
                            if (proj.direction == -1)
                                proj.spriteDirection = 1;
                            if (proj.direction == 1)
                                proj.spriteDirection = -1;
                        }
                        if (proj.type == 398)
                            proj.spriteDirection = proj.direction;
                        if (flag5)
                        {
                            if ((double)proj.ai[1] > 0.0)
                            {
                                if ((double)proj.localAI[1] == 0.0)
                                {
                                    proj.localAI[1] = 1f;
                                    proj.frame = 1;
                                }
                                if (proj.frame != 0)
                                {
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 4)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame == 4)
                                        proj.frame = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y == 0.0)
                            {
                                proj.localAI[1] = 0.0f;
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame < 5)
                                        proj.frame = 5;
                                    if (proj.frame >= 11)
                                        proj.frame = 5;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 4;
                            }
                            else if ((double)proj.velocity.Y > 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 4;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y > 10.0)
                                proj.velocity.Y = 10f;
                            double y = (double)proj.velocity.Y;
                        }
                        else if (proj.type == 268)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if (proj.frame > 5)
                                    proj.frameCounter = 0;
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    int num2 = 3;
                                    ++proj.frameCounter;
                                    if (proj.frameCounter < num2)
                                        proj.frame = 0;
                                    else if (proj.frameCounter < num2 * 2)
                                        proj.frame = 1;
                                    else if (proj.frameCounter < num2 * 3)
                                        proj.frame = 2;
                                    else if (proj.frameCounter < num2 * 4)
                                        proj.frame = 3;
                                    else
                                        proj.frameCounter = num2 * 4;
                                }
                                else
                                {
                                    proj.velocity.X *= 0.8f;
                                    ++proj.frameCounter;
                                    int num2 = 3;
                                    if (proj.frameCounter < num2)
                                        proj.frame = 0;
                                    else if (proj.frameCounter < num2 * 2)
                                        proj.frame = 1;
                                    else if (proj.frameCounter < num2 * 3)
                                        proj.frame = 2;
                                    else if (proj.frameCounter < num2 * 4)
                                        proj.frame = 3;
                                    else if (flag1 || flag2)
                                    {
                                        proj.velocity.X *= 2f;
                                        proj.frame = 4;
                                        proj.velocity.Y = -6.1f;
                                        proj.frameCounter = 0;
                                        for (int index1 = 0; index1 < 4; ++index1)
                                        {
                                            int index2 = Dust.NewDust(new Vector2(proj.position.X, (float)((double)proj.position.Y + (double)proj.height - 2.0)), proj.width, 4, 5, 0.0f, 0.0f, 0, new Color(), 1f);
                                            Main.dust[index2].velocity += proj.velocity;
                                            Main.dust[index2].velocity *= 0.4f;
                                        }
                                    }
                                    else
                                        proj.frameCounter = num2 * 4;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 5;
                            }
                            else
                            {
                                proj.frame = 4;
                                proj.frameCounter = 3;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 269)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    int index = Dust.NewDust(new Vector2(proj.position.X, (float)((double)proj.position.Y + (double)proj.height - 2.0)), proj.width, 6, 76, 0.0f, 0.0f, 0, new Color(), 1f);
                                    Main.dust[index].noGravity = true;
                                    Main.dust[index].velocity *= 0.3f;
                                    Main.dust[index].noLight = true;
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 3)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 2;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 313)
                        {
                            int index1 = (int)((double)proj.Center.X / 16.0);
                            int index2 = (int)((double)proj.Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].wall > 0)
                            {
                                proj.position.Y += (float)proj.height;
                                proj.height = 34;
                                proj.position.Y -= (float)proj.height;
                                proj.position.X += (float)(proj.width / 2);
                                proj.width = 34;
                                proj.position.X -= (float)(proj.width / 2);
                                float num2 = 4f;
                                Vector2 vector2_2 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
                                float num3 = guardian.Position.X - vector2_2.X;
                                float num4 = guardian.Position.Y - guardian.Height * 0.5f -vector2_2.Y;
                                float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
                                float num6 = num2 / num5;
                                float num7 = num3 * num6;
                                float num8 = num4 * num6;
                                if ((double)num5 < 120.0)
                                {
                                    proj.velocity.X *= 0.9f;
                                    proj.velocity.Y *= 0.9f;
                                    if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < 0.1)
                                    {
                                        proj.velocity = Vector2.Zero;
                                    }
                                }
                                else
                                {
                                    proj.velocity.X = (float)(((double)proj.velocity.X * 9.0 + (double)num7) / 10.0);
                                    proj.velocity.Y = (float)(((double)proj.velocity.Y * 9.0 + (double)num8) / 10.0);
                                }
                                if ((double)num5 >= 120.0)
                                {
                                    proj.spriteDirection = proj.direction;
                                    proj.rotation = (float)Math.Atan2((double)proj.velocity.Y * (double)-proj.direction, (double)proj.velocity.X * (double)-proj.direction);
                                }
                                proj.frameCounter = proj.frameCounter + (int)((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y));
                                if (proj.frameCounter > 6)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 10)
                                    proj.frame = 5;
                                if (proj.frame >= 5)
                                    return;
                                proj.frame = 10;
                            }
                            else
                            {
                                proj.rotation = 0.0f;
                                if (proj.direction == -1)
                                    proj.spriteDirection = 1;
                                if (proj.direction == 1)
                                    proj.spriteDirection = -1;
                                proj.position.Y += (float)proj.height;
                                proj.height = 30;
                                proj.position.Y -= (float)proj.height;
                                proj.position.X += (float)(proj.width / 2);
                                proj.width = 30;
                                proj.position.X -= (float)(proj.width / 2);
                                if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                                {
                                    if ((double)proj.velocity.X == 0.0)
                                    {
                                        proj.frame = 0;
                                        proj.frameCounter = 0;
                                    }
                                    else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                    {
                                        proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                        ++proj.frameCounter;
                                        if (proj.frameCounter > 6)
                                        {
                                            ++proj.frame;
                                            proj.frameCounter = 0;
                                        }
                                        if (proj.frame > 3)
                                            proj.frame = 0;
                                    }
                                    else
                                    {
                                        proj.frame = 0;
                                        proj.frameCounter = 0;
                                    }
                                }
                                else
                                {
                                    proj.frameCounter = 0;
                                    proj.frame = 4;
                                }
                                proj.velocity.Y += 0.4f;
                                if ((double)proj.velocity.Y <= 10.0)
                                    return;
                                proj.velocity.Y = 10f;
                            }
                        }
                        else if (proj.type >= 390 && proj.type <= 392)
                        {
                            int index1 = (int)((double)proj.Center.X / 16.0);
                            int index2 = (int)((double)proj.Center.Y / 16.0);
                            if (Main.tile[index1, index2] != null && (int)Main.tile[index1, index2].wall > 0)
                            {
                                proj.position.Y += (float)proj.height;
                                proj.height = 34;
                                proj.position.Y -= (float)proj.height;
                                proj.position.X += (float)(proj.width / 2);
                                proj.width = 34;
                                proj.position.X -= (float)(proj.width / 2);
                                float num2 = 9f;
                                float num3 = (float)(40 * (proj.minionPos + 1));
                                Vector2 v = guardian.CenterPosition - proj.Center;
                                if (flag6)
                                {
                                    v = vector2_1;
                                    num3 = 10f;
                                }
                                else if (!Collision.CanHitLine(proj.Center, 1, 1, guardian.CenterPosition, 1, 1))
                                    proj.ai[0] = 1f;
                                if ((double)v.Length() < (double)num3)
                                {
                                    proj.velocity *= 0.9f;
                                    if ((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y) < 0.1)
                                    {
                                        proj.velocity = Vector2.Zero;
                                    }
                                }
                                else if ((double)v.Length() < 800.0 || !flag6)
                                    proj.velocity = (proj.velocity * 9f + Vector2.Normalize(v) * num2) / 10f;
                                if ((double)v.Length() >= (double)num3)
                                {
                                    proj.spriteDirection = proj.direction;
                                    proj.rotation = proj.velocity.ToRotation() + 1.570796f;
                                }
                                else
                                    proj.rotation = v.ToRotation() + 1.570796f;
                                proj.frameCounter = proj.frameCounter + (int)((double)Math.Abs(proj.velocity.X) + (double)Math.Abs(proj.velocity.Y));
                                if (proj.frameCounter > 5)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 7)
                                    proj.frame = 4;
                                if (proj.frame >= 4)
                                    return;
                                proj.frame = 7;
                            }
                            else
                            {
                                if (!flag7)
                                    proj.rotation = 0.0f;
                                if (proj.direction == -1)
                                    proj.spriteDirection = 1;
                                if (proj.direction == 1)
                                    proj.spriteDirection = -1;
                                proj.position.Y += (float)proj.height;
                                proj.height = 30;
                                proj.position.Y -= (float)proj.height;
                                proj.position.X += (float)(proj.width / 2);
                                proj.width = 30;
                                proj.position.X -= (float)(proj.width / 2);
                                if (!flag6 && !Collision.CanHitLine(proj.Center, 1, 1, guardian.CenterPosition, 1, 1))
                                    proj.ai[0] = 1f;
                                if (!flag7 && proj.frame >= 4 && proj.frame <= 7)
                                {
                                    Vector2 vector2_2 = guardian.CenterPosition - proj.Center;
                                    if (flag6)
                                        vector2_2 = vector2_1;
                                    float num2 = -vector2_2.Y;
                                    if ((double)vector2_2.Y <= 0.0)
                                    {
                                        if ((double)num2 < 120.0)
                                            proj.velocity.Y = -10f;
                                        else if ((double)num2 < 210.0)
                                            proj.velocity.Y = -13f;
                                        else if ((double)num2 < 270.0)
                                            proj.velocity.Y = -15f;
                                        else if ((double)num2 < 310.0)
                                            proj.velocity.Y = -17f;
                                        else if ((double)num2 < 380.0)
                                            proj.velocity.Y = -18f;
                                    }
                                }
                                if (flag7)
                                {
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 3)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 8)
                                        proj.frame = 4;
                                    if (proj.frame <= 3)
                                        proj.frame = 7;
                                }
                                else if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                                {
                                    if ((double)proj.velocity.X == 0.0)
                                    {
                                        proj.frame = 0;
                                        proj.frameCounter = 0;
                                    }
                                    else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                    {
                                        proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                        ++proj.frameCounter;
                                        if (proj.frameCounter > 5)
                                        {
                                            ++proj.frame;
                                            proj.frameCounter = 0;
                                        }
                                        if (proj.frame > 2)
                                            proj.frame = 0;
                                    }
                                    else
                                    {
                                        proj.frame = 0;
                                        proj.frameCounter = 0;
                                    }
                                }
                                else
                                {
                                    proj.frameCounter = 0;
                                    proj.frame = 3;
                                }
                                proj.velocity.Y += 0.4f;
                                if ((double)proj.velocity.Y <= 10.0)
                                    return;
                                proj.velocity.Y = 10f;
                            }
                        }
                        else if (proj.type == 314)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 6)
                                        proj.frame = 1;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 7;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 319)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 8)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 5)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 236)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    if (proj.frame < 2)
                                        proj.frame = 2;
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 8)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 499)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    if (proj.frame < 2)
                                        proj.frame = 2;
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 8)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 266)
                        {
                            if ((double)proj.velocity.Y >= 0.0 && (double)proj.velocity.Y <= 0.8)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                    ++proj.frameCounter;
                                else
                                    proj.frameCounter += 3;
                            }
                            else
                                proj.frameCounter += 5;
                            if (proj.frameCounter >= 20)
                            {
                                proj.frameCounter -= 20;
                                ++proj.frame;
                            }
                            if (proj.frame > 1)
                                proj.frame = 0;
                            if (proj.wet && (double)guardian.Position.Y < (double)proj.position.Y + (double)proj.height && (double)proj.localAI[0] == 0.0)
                            {
                                if ((double)proj.velocity.Y > -4.0)
                                    proj.velocity.Y -= 0.2f;
                                if ((double)proj.velocity.Y > 0.0)
                                    proj.velocity.Y *= 0.95f;
                            }
                            else
                                proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 334)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    if (proj.frame > 0)
                                    {
                                        proj.frameCounter += 2;
                                        if (proj.frameCounter > 6)
                                        {
                                            ++proj.frame;
                                            proj.frameCounter = 0;
                                        }
                                        if (proj.frame >= 7)
                                            proj.frame = 0;
                                    }
                                    else
                                    {
                                        proj.frame = 0;
                                        proj.frameCounter = 0;
                                    }
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs((double)proj.velocity.X * 0.75);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 7 || proj.frame < 1)
                                        proj.frame = 1;
                                }
                                else if (proj.frame > 0)
                                {
                                    proj.frameCounter += 2;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 7)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 2;
                            }
                            else if ((double)proj.velocity.Y > 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 4;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 353)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 9)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            else if ((double)proj.velocity.Y > 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 111)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 7)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 4;
                            }
                            else if ((double)proj.velocity.Y > 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 6;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 112)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 3)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y < 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 2;
                            }
                            else if ((double)proj.velocity.Y > 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 2;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == (int)sbyte.MaxValue)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.1 || (double)proj.velocity.X > 0.1)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 5)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frame = 0;
                                proj.frameCounter = 0;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 200)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.1 || (double)proj.velocity.X > 0.1)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 5)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.rotation = proj.velocity.X * 0.1f;
                                ++proj.frameCounter;
                                if ((double)proj.velocity.Y < 0.0)
                                    proj.frameCounter += 2;
                                if (proj.frameCounter > 6)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 9)
                                    proj.frame = 6;
                                if (proj.frame < 6)
                                    proj.frame = 6;
                            }
                            proj.velocity.Y += 0.1f;
                            if ((double)proj.velocity.Y <= 4.0)
                                return;
                            proj.velocity.Y = 4f;
                        }
                        else if (proj.type == 208)
                        {
                            if ((double)proj.velocity.Y == 0.0 && (double)proj.velocity.X == 0.0)
                            {
                                if ((double)guardian.Position.X < (double)proj.position.X + (double)(proj.width / 2))
                                    proj.direction = -1;
                                else if ((double)guardian.Position.X > (double)proj.position.X + (double)(proj.width / 2))
                                    proj.direction = 1;
                                proj.rotation = 0.0f;
                                proj.frame = 0;
                            }
                            else
                            {
                                proj.rotation = proj.velocity.X * 0.075f;
                                ++proj.frameCounter;
                                if (proj.frameCounter > 6)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 4)
                                    proj.frame = 1;
                                if (proj.frame < 1)
                                    proj.frame = 1;
                            }
                            proj.velocity.Y += 0.1f;
                            if ((double)proj.velocity.Y <= 4.0)
                                return;
                            proj.velocity.Y = 4f;
                        }
                        else if (proj.type == 209)
                        {
                            if (proj.alpha > 0)
                            {
                                proj.alpha -= 5;
                                if (proj.alpha < 0)
                                    proj.alpha = 0;
                            }
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.1 || (double)proj.velocity.X > 0.1)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 11)
                                        proj.frame = 2;
                                    if (proj.frame < 2)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frame = 1;
                                proj.frameCounter = 0;
                                proj.rotation = 0.0f;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else if (proj.type == 324)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X < -0.1 || (double)proj.velocity.X > 0.1)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 5)
                                        proj.frame = 2;
                                    if (proj.frame < 2)
                                        proj.frame = 2;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.frameCounter = 0;
                                proj.frame = 1;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 14.0)
                                return;
                            proj.velocity.Y = 14f;
                        }
                        else if (proj.type == 210)
                        {
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X < -0.1 || (double)proj.velocity.X > 0.1)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame > 6)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else
                            {
                                proj.rotation = proj.velocity.X * 0.05f;
                                ++proj.frameCounter;
                                if (proj.frameCounter > 6)
                                {
                                    ++proj.frame;
                                    proj.frameCounter = 0;
                                }
                                if (proj.frame > 11)
                                    proj.frame = 7;
                                if (proj.frame < 7)
                                    proj.frame = 7;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                        else
                        {
                            if (proj.type != 398)
                                return;
                            if ((double)proj.velocity.Y == 0.0)
                            {
                                if ((double)proj.velocity.X == 0.0)
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                                else if ((double)proj.velocity.X < -0.8 || (double)proj.velocity.X > 0.8)
                                {
                                    proj.frameCounter = proj.frameCounter + (int)Math.Abs(proj.velocity.X);
                                    ++proj.frameCounter;
                                    if (proj.frameCounter > 6)
                                    {
                                        ++proj.frame;
                                        proj.frameCounter = 0;
                                    }
                                    if (proj.frame >= 5)
                                        proj.frame = 0;
                                }
                                else
                                {
                                    proj.frame = 0;
                                    proj.frameCounter = 0;
                                }
                            }
                            else if ((double)proj.velocity.Y != 0.0)
                            {
                                proj.frameCounter = 0;
                                proj.frame = 5;
                            }
                            proj.velocity.Y += 0.4f;
                            if ((double)proj.velocity.Y <= 10.0)
                                return;
                            proj.velocity.Y = 10f;
                        }
                    }
                }
            }
        }

        private void HornetSummonAI(Projectile proj)
        {
            if (!GuardianProj.ContainsKey(proj.whoAmI) || !MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID))
            {
                proj.active = false;
                return;
            }
            TerraGuardian guardian = GuardianProj[proj.whoAmI];
            if (proj.type == 423)
            {
                if ((double)proj.ai[0] == 2.0)
                {
                    --proj.ai[1];
                    proj.tileCollide = false;
                    if ((double)proj.ai[1] > 3.0)
                    {
                        int index = Dust.NewDust(proj.Center, 0, 0, 220 + Main.rand.Next(2), proj.velocity.X, proj.velocity.Y, 100, new Color(), 1f);
                        Main.dust[index].scale = (float)(0.5 + Main.rand.NextDouble() * 0.300000011920929);
                        Main.dust[index].velocity /= 2.5f;
                        Main.dust[index].noGravity = true;
                        Main.dust[index].noLight = true;
                        Main.dust[index].frame.Y = 80;
                    }
                    if ((double)proj.ai[1] != 0.0)
                        return;
                    proj.ai[1] = 30f;
                    proj.ai[0] = 0.0f;
                    proj.velocity /= 5f;
                    proj.velocity.Y = 0.0f;
                    proj.extraUpdates = 0;
                    proj.numUpdates = 0;
                    proj.netUpdate = true;
                    proj.extraUpdates = 0;
                    proj.numUpdates = 0;
                }
                if (proj.extraUpdates > 1)
                    proj.extraUpdates = 0;
                if (proj.numUpdates > 1)
                    proj.numUpdates = 0;
            }
            if (proj.type == 613)
            {
                if ((double)proj.ai[0] == 2.0)
                {
                    --proj.ai[1];
                    proj.tileCollide = false;
                    if ((double)proj.ai[1] > 3.0)
                    {
                        if (proj.numUpdates < 20)
                        {
                            for (int index = 0; index < 3; ++index)
                            {
                                Dust dust = Main.dust[Dust.NewDust(proj.position, proj.width, proj.height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                                dust.noGravity = true;
                                dust.position = proj.Center;
                                dust.velocity *= 3f;
                                dust.velocity += proj.velocity * 3f;
                                dust.fadeIn = 1f;
                            }
                        }
                        float num1 = (float)(2.0 - (double)proj.numUpdates / 30.0);
                        if ((double)proj.scale > 0.0)
                        {
                            float num2 = 2f;
                            for (int index = 0; (double)index < (double)num2; ++index)
                            {
                                Dust dust = Main.dust[Dust.NewDust(proj.position, proj.width, proj.height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                                dust.noGravity = true;
                                dust.position = proj.Center + Vector2.UnitY.RotatedBy((double)proj.numUpdates * 0.104719758033752 + (double)proj.whoAmI * 0.785398185253143 + 1.57079637050629, new Vector2()) * (float)(proj.height / 2) - proj.velocity * ((float)index / num2);
                                dust.velocity = proj.velocity / 3f;
                                dust.fadeIn = num1 / 2f;
                                dust.scale = num1;
                            }
                        }
                    }
                    if ((double)proj.ai[1] != 0.0)
                        return;
                    proj.ai[1] = 30f;
                    proj.ai[0] = 0.0f;
                    proj.velocity /= 5f;
                    proj.velocity.Y = 0.0f;
                    proj.extraUpdates = 0;
                    proj.numUpdates = 0;
                    proj.netUpdate = true;
                    float num = 15f;
                    for (int index = 0; (double)index < (double)num; ++index)
                    {
                        Dust dust = Main.dust[Dust.NewDust(proj.position, proj.width, proj.height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                        dust.noGravity = true;
                        dust.position = proj.Center - proj.velocity * 5f;
                        dust.velocity *= 3f;
                        dust.velocity += proj.velocity * 3f;
                        dust.fadeIn = 1f;
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    for (int index = 0; (double)index < (double)num; ++index)
                    {
                        Dust dust = Main.dust[Dust.NewDust(proj.position, proj.width, proj.height, 229, 0.0f, 0.0f, 0, new Color(), 1f)];
                        dust.noGravity = true;
                        dust.position = proj.Center;
                        dust.velocity *= 3f;
                        dust.velocity += proj.velocity * 3f;
                        dust.fadeIn = 1f;
                        if (Main.rand.Next(3) != 0)
                        {
                            dust.fadeIn = 2f;
                            dust.scale = 2f;
                            dust.velocity /= 8f;
                        }
                    }
                    proj.extraUpdates = 0;
                    proj.numUpdates = 0;
                }
                if (proj.extraUpdates > 1)
                    proj.extraUpdates = 0;
                if (proj.numUpdates > 1)
                    proj.numUpdates = 0;
            }
            if (proj.type == 423 && (double)proj.localAI[0] > 0.0)
                --proj.localAI[0];
            if (proj.type == 613 && (double)proj.localAI[0] > 0.0)
                --proj.localAI[0];
            float num3 = 0.05f;
            float width = (float)proj.width;
            if (proj.type == 407)
            {
                num3 = 0.1f;
                width *= 2f;
            }
            for (int index = 0; index < 1000; ++index)
            {
                if (index != proj.whoAmI && Main.projectile[index].active && (Main.projectile[index].owner == proj.owner && Main.projectile[index].type == proj.type) && (double)Math.Abs(proj.position.X - Main.projectile[index].position.X) + (double)Math.Abs(proj.position.Y - Main.projectile[index].position.Y) < (double)width)
                {
                    if ((double)proj.position.X < (double)Main.projectile[index].position.X)
                        proj.velocity.X -= num3;
                    else
                        proj.velocity.X += num3;
                    if ((double)proj.position.Y < (double)Main.projectile[index].position.Y)
                        proj.velocity.Y -= num3;
                    else
                        proj.velocity.Y += num3;
                }
            }
            Vector2 vector2_1 = proj.position;
            float num4 = 400f;
            if (proj.type == 423)
                num4 = 300f;
            if (proj.type == 613)
                num4 = 300f;
            bool flag = false;
            int num5 = -1;
            proj.tileCollide = true;
            if (proj.type == 407)
            {
                proj.tileCollide = false;
                if (Collision.SolidCollision(proj.position, proj.width, proj.height))
                {
                    proj.alpha += 20;
                    if (proj.alpha > 150)
                        proj.alpha = 150;
                }
                else
                {
                    proj.alpha -= 50;
                    if (proj.alpha < 60)
                        proj.alpha = 60;
                }
            }
            if (proj.type == 407 || proj.type == 613 || proj.type == 423)
            {
                Vector2 center = guardian.CenterPosition;
                Vector2 vector2_2 = new Vector2(0.5f);
                if (proj.type == 423)
                    vector2_2.Y = 0.0f;
                NPC minionAttackTargetNpc = proj.OwnerMinionAttackTargetNPC;
                if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy((object)proj, false))
                {
                    Vector2 vector2_3 = minionAttackTargetNpc.position + minionAttackTargetNpc.Size * vector2_2;
                    float num1 = Vector2.Distance(vector2_3, center);
                    if (((double)Vector2.Distance(center, vector2_1) > (double)num1 && (double)num1 < (double)num4 || !flag) && Collision.CanHitLine(proj.position, proj.width, proj.height, minionAttackTargetNpc.position, minionAttackTargetNpc.width, minionAttackTargetNpc.height))
                    {
                        num4 = num1;
                        vector2_1 = vector2_3;
                        flag = true;
                        num5 = minionAttackTargetNpc.whoAmI;
                    }
                }
                if (!flag)
                {
                    for (int index = 0; index < 200; ++index)
                    {
                        NPC npc = Main.npc[index];
                        if (npc.CanBeChasedBy((object)proj, false))
                        {
                            Vector2 vector2_3 = npc.position + npc.Size * vector2_2;
                            float num1 = Vector2.Distance(vector2_3, center);
                            if (((double)Vector2.Distance(center, vector2_1) > (double)num1 && (double)num1 < (double)num4 || !flag) && Collision.CanHitLine(proj.position, proj.width, proj.height, npc.position, npc.width, npc.height))
                            {
                                num4 = num1;
                                vector2_1 = vector2_3;
                                flag = true;
                                num5 = index;
                            }
                        }
                    }
                }
            }
            else
            {
                NPC minionAttackTargetNpc = proj.OwnerMinionAttackTargetNPC;
                if (minionAttackTargetNpc != null && minionAttackTargetNpc.CanBeChasedBy((object)proj, false))
                {
                    float num1 = Vector2.Distance(minionAttackTargetNpc.Center, proj.Center);
                    if (((double)Vector2.Distance(proj.Center, vector2_1) > (double)num1 && (double)num1 < (double)num4 || !flag) && Collision.CanHitLine(proj.position, proj.width, proj.height, minionAttackTargetNpc.position, minionAttackTargetNpc.width, minionAttackTargetNpc.height))
                    {
                        num4 = num1;
                        vector2_1 = minionAttackTargetNpc.Center;
                        flag = true;
                        num5 = minionAttackTargetNpc.whoAmI;
                    }
                }
                if (!flag)
                {
                    for (int index = 0; index < 200; ++index)
                    {
                        NPC npc = Main.npc[index];
                        if (npc.CanBeChasedBy((object)proj, false))
                        {
                            float num1 = Vector2.Distance(npc.Center, proj.Center);
                            if (((double)Vector2.Distance(proj.Center, vector2_1) > (double)num1 && (double)num1 < (double)num4 || !flag) && Collision.CanHitLine(proj.position, proj.width, proj.height, npc.position, npc.width, npc.height))
                            {
                                num4 = num1;
                                vector2_1 = npc.Center;
                                flag = true;
                                num5 = index;
                            }
                        }
                    }
                }
            }
            int num6 = 500;
            if (flag)
                num6 = 1000;
            if (flag && proj.type == 423)
                num6 = 1200;
            if (flag && proj.type == 613)
                num6 = 1350;
            if ((double)Vector2.Distance(guardian.CenterPosition, proj.Center) > (double)num6)
            {
                proj.ai[0] = 1f;
                proj.netUpdate = true;
            }
            if ((double)proj.ai[0] == 1.0)
                proj.tileCollide = false;
            if (flag && (double)proj.ai[0] == 0.0)
            {
                Vector2 vector2_2 = vector2_1 - proj.Center;
                float num1 = vector2_2.Length();
                vector2_2.Normalize();
                if (proj.type == 423)
                {
                    Vector2 vector2_3 = vector2_1 - Vector2.UnitY * 80f;
                    int index = (int)vector2_3.Y / 16;
                    if (index < 0)
                        index = 0;
                    Tile tile1 = Main.tile[(int)vector2_3.X / 16, index];
                    if (tile1 != null && tile1.active() && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]))
                    {
                        vector2_3 += Vector2.UnitY * 16f;
                        Tile tile2 = Main.tile[(int)vector2_3.X / 16, (int)vector2_3.Y / 16];
                        if (tile2 != null && tile2.active() && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]))
                            vector2_3 += Vector2.UnitY * 16f;
                    }
                    vector2_2 = vector2_3 - proj.Center;
                    num1 = vector2_2.Length();
                    vector2_2.Normalize();
                    if ((double)num1 > 300.0 && (double)num1 <= 800.0 && (double)proj.localAI[0] == 0.0)
                    {
                        proj.ai[0] = 2f;
                        proj.ai[1] = (float)(int)((double)num1 / 10.0);
                        proj.extraUpdates = (int)proj.ai[1];
                        proj.velocity = vector2_2 * 10f;
                        proj.localAI[0] = 60f;
                        return;
                    }
                }
                if (proj.type == 613)
                {
                    Vector2 vector2_3 = vector2_1;
                    Vector2 vector2_4 = proj.Center - vector2_3;
                    if (vector2_4 == Vector2.Zero)
                        vector2_4 = -Vector2.UnitY;
                    vector2_4.Normalize();
                    Vector2 vector2_5 = vector2_3 + vector2_4 * 60f;
                    int index = (int)vector2_5.Y / 16;
                    if (index < 0)
                        index = 0;
                    Tile tile1 = Main.tile[(int)vector2_5.X / 16, index];
                    if (tile1 != null && tile1.active() && (Main.tileSolid[(int)tile1.type] && !Main.tileSolidTop[(int)tile1.type]))
                    {
                        vector2_5 += Vector2.UnitY * 16f;
                        Tile tile2 = Main.tile[(int)vector2_5.X / 16, (int)vector2_5.Y / 16];
                        if (tile2 != null && tile2.active() && (Main.tileSolid[(int)tile2.type] && !Main.tileSolidTop[(int)tile2.type]))
                            vector2_5 += Vector2.UnitY * 16f;
                    }
                    vector2_2 = vector2_5 - proj.Center;
                    num1 = vector2_2.Length();
                    vector2_2.Normalize();
                    if ((double)num1 > 400.0 && (double)num1 <= 800.0 && (double)proj.localAI[0] == 0.0)
                    {
                        proj.ai[0] = 2f;
                        proj.ai[1] = (float)(int)((double)num1 / 10.0);
                        proj.extraUpdates = (int)proj.ai[1];
                        proj.velocity = vector2_2 * 10f;
                        proj.localAI[0] = 60f;
                        return;
                    }
                }
                if (proj.type == 407)
                {
                    if ((double)num1 > 400.0)
                    {
                        float num2 = 2f;
                        vector2_2 *= num2;
                        proj.velocity = (proj.velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        proj.velocity *= 0.96f;
                    }
                }
                if ((double)num1 > 200.0)
                {
                    float num2 = 6f;
                    Vector2 vector2_3 = vector2_2 * num2;
                    proj.velocity.X = (float)(((double)proj.velocity.X * 40.0 + (double)vector2_3.X) / 41.0);
                    proj.velocity.Y = (float)(((double)proj.velocity.Y * 40.0 + (double)vector2_3.Y) / 41.0);
                }
                else if (proj.type == 423 || proj.type == 613)
                {
                    if ((double)num1 > 70.0 && (double)num1 < 130.0)
                    {
                        float num2 = 7f;
                        if ((double)num1 < 100.0)
                            num2 = -3f;
                        Vector2 vector2_3 = vector2_2 * num2;
                        proj.velocity = (proj.velocity * 20f + vector2_3) / 21f;
                        if ((double)Math.Abs(vector2_3.X) > (double)Math.Abs(vector2_3.Y))
                            proj.velocity.X = (float)(((double)proj.velocity.X * 10.0 + (double)vector2_3.X) / 11.0);
                    }
                    else
                    {
                        proj.velocity *= 0.97f;
                    }
                }
                else if (proj.type == 375)
                {
                    if ((double)num1 < 150.0)
                    {
                        float num2 = 4f;
                        Vector2 vector2_3 = vector2_2 * -num2;
                        proj.velocity.X = (float)(((double)proj.velocity.X * 40.0 + (double)vector2_3.X) / 41.0);
                        proj.velocity.Y = (float)(((double)proj.velocity.Y * 40.0 + (double)vector2_3.Y) / 41.0);
                    }
                    else
                    {
                        proj.velocity *= 0.97f;
                    }
                }
                else if ((double)proj.velocity.Y > -1.0)
                    proj.velocity.Y -= 0.1f;
            }
            else
            {
                if (!Collision.CanHitLine(proj.Center, 1, 1, guardian.CenterPosition, 1, 1))
                    proj.ai[0] = 1f;
                float num1 = 6f;
                if ((double)proj.ai[0] == 1.0)
                    num1 = 15f;
                if (proj.type == 407)
                    num1 = 9f;
                Vector2 center = proj.Center;
                Vector2 vector2_2 = guardian.CenterPosition - center + new Vector2(0.0f, -60f);
                if (proj.type == 407)
                    vector2_2 += new Vector2(0.0f, 40f);
                if (proj.type == 375)
                {
                    proj.ai[1] = 3600f;
                    proj.netUpdate = true;
                    vector2_2 = guardian.CenterPosition - center;
                    int num2 = 1;
                    for (int index = 0; index < proj.whoAmI; ++index)
                    {
                        if (Main.projectile[index].active && Main.projectile[index].owner == proj.owner && Main.projectile[index].type == proj.type)
                            ++num2;
                    }
                    vector2_2.X -= (float)(10 * guardian.Direction);
                    vector2_2.X -= (float)(num2 * 40 * guardian.Direction);
                    vector2_2.Y -= 10f;
                }
                float num7 = vector2_2.Length();
                if ((double)num7 > 200.0 && (double)num1 < 9.0)
                    num1 = 9f;
                if (proj.type == 375)
                    num1 = (float)(int)((double)num1 * 0.75);
                if ((double)num7 < 100.0 && (double)proj.ai[0] == 1.0 && !Collision.SolidCollision(proj.position, proj.width, proj.height))
                {
                    proj.ai[0] = 0.0f;
                    proj.netUpdate = true;
                }
                if ((double)num7 > 2000.0)
                {
                    proj.position.X = guardian.Position.X - (float)(proj.width / 2);
                    proj.position.Y = guardian.Position.Y - guardian.Height * 0.5f - (float)(proj.width / 2);
                }
                if (proj.type == 375)
                {
                    if ((double)num7 > 10.0)
                    {
                        vector2_2.Normalize();
                        if ((double)num7 < 50.0)
                            num1 /= 2f;
                        vector2_2 *= num1;
                        proj.velocity = (proj.velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        proj.direction = guardian.Direction;
                        proj.velocity *= 0.9f;
                    }
                }
                else if (proj.type == 407)
                {
                    if ((double)Math.Abs(vector2_2.X) > 40.0 || (double)Math.Abs(vector2_2.Y) > 10.0)
                    {
                        vector2_2.Normalize();
                        vector2_2 *= num1;
                        vector2_2 *= new Vector2(1.25f, 0.65f);
                        proj.velocity = (proj.velocity * 20f + vector2_2) / 21f;
                    }
                    else
                    {
                        if ((double)proj.velocity.X == 0.0 && (double)proj.velocity.Y == 0.0)
                        {
                            proj.velocity.X = -0.15f;
                            proj.velocity.Y = -0.05f;
                        }
                        proj.velocity *= 1.01f;
                    }
                }
                else if ((double)num7 > 70.0)
                {
                    vector2_2.Normalize();
                    vector2_2 *= num1;
                    proj.velocity = (proj.velocity * 20f + vector2_2) / 21f;
                }
                else
                {
                    if ((double)proj.velocity.X == 0.0 && (double)proj.velocity.Y == 0.0)
                    {
                        proj.velocity.X = -0.15f;
                        proj.velocity.Y = -0.05f;
                    }
                    proj.velocity *= 1.01f;
                }
            }
            proj.rotation = proj.velocity.X * 0.05f;
            ++proj.frameCounter;
            if (proj.type == 373)
            {
                if (proj.frameCounter > 1)
                {
                    ++proj.frame;
                    proj.frameCounter = 0;
                }
                if (proj.frame > 2)
                    proj.frame = 0;
            }
            if (proj.type == 375)
            {
                if (proj.frameCounter >= 16)
                    proj.frameCounter = 0;
                proj.frame = proj.frameCounter / 4;
                if ((double)proj.ai[1] > 0.0 && (double)proj.ai[1] < 16.0)
                    proj.frame += 4;
                if (Main.rand.Next(6) == 0)
                {
                    int index = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 6, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                }
            }
            if (proj.type == 407)
            {
                int num1 = 2;
                if (proj.frameCounter >= 6 * num1)
                    proj.frameCounter = 0;
                proj.frame = proj.frameCounter / num1;
                if (Main.rand.Next(5) == 0)
                {
                    int index = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 217, 0.0f, 0.0f, 100, new Color(), 2f);
                    Main.dust[index].velocity *= 0.3f;
                    Main.dust[index].noGravity = true;
                    Main.dust[index].noLight = true;
                }
            }
            if (proj.type == 423 || proj.type == 613)
            {
                int num1 = 3;
                if (proj.frameCounter >= 4 * num1)
                    proj.frameCounter = 0;
                proj.frame = proj.frameCounter / num1;
            }
            if ((double)proj.velocity.X > 0.0)
                proj.spriteDirection = proj.direction = -1;
            else if ((double)proj.velocity.X < 0.0)
                proj.spriteDirection = proj.direction = 1;
            if (proj.type == 373)
            {
                if ((double)proj.ai[1] > 0.0)
                    proj.ai[1] += (float)Main.rand.Next(1, 4);
                if ((double)proj.ai[1] > 90.0)
                {
                    proj.ai[1] = 0.0f;
                    proj.netUpdate = true;
                }
            }
            else if (proj.type == 375)
            {
                if ((double)proj.ai[1] > 0.0)
                {
                    ++proj.ai[1];
                    if (Main.rand.Next(3) == 0)
                        ++proj.ai[1];
                }
                if ((double)proj.ai[1] > (double)Main.rand.Next(180, 900))
                {
                    proj.ai[1] = 0.0f;
                    proj.netUpdate = true;
                }
            }
            else if (proj.type == 407)
            {
                if ((double)proj.ai[1] > 0.0)
                {
                    ++proj.ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++proj.ai[1];
                }
                if ((double)proj.ai[1] > 60.0)
                {
                    proj.ai[1] = 0.0f;
                    proj.netUpdate = true;
                }
            }
            else if (proj.type == 423)
            {
                if ((double)proj.ai[1] > 0.0)
                {
                    ++proj.ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++proj.ai[1];
                }
                if ((double)proj.ai[1] > 30.0)
                {
                    proj.ai[1] = 0.0f;
                    proj.netUpdate = true;
                }
            }
            else if (proj.type == 613)
            {
                if ((double)proj.ai[1] > 0.0)
                {
                    ++proj.ai[1];
                    if (Main.rand.Next(3) != 0)
                        ++proj.ai[1];
                }
                if ((double)proj.ai[1] > 60.0)
                {
                    proj.ai[1] = 0.0f;
                    proj.netUpdate = true;
                }
            }
            if ((double)proj.ai[0] != 0.0)
                return;
            float num8 = 0.0f;
            int Type = 0;
            if (proj.type == 373)
            {
                num8 = 10f;
                Type = 374;
            }
            else if (proj.type == 375)
            {
                num8 = 11f;
                Type = 376;
            }
            else if (proj.type == 407)
            {
                num8 = 14f;
                Type = 408;
            }
            else if (proj.type == 423)
            {
                num8 = 4f;
                Type = 433;
            }
            else if (proj.type == 613)
            {
                num8 = 14f;
                Type = 614;
            }
            if (!flag)
                return;
            if (proj.type == 375)
            {
                if ((double)(vector2_1 - proj.Center).X > 0.0)
                    proj.spriteDirection = proj.direction = -1;
                else if ((double)(vector2_1 - proj.Center).X < 0.0)
                    proj.spriteDirection = proj.direction = 1;
            }
            if (proj.type == 407 && Collision.SolidCollision(proj.position, proj.width, proj.height))
                return;
            if (proj.type == 423)
            {
                if ((double)Math.Abs((vector2_1 - proj.Center).ToRotation() - 1.570796f) > 0.785398185253143)
                {
                    proj.velocity += Vector2.Normalize(vector2_1 - proj.Center - Vector2.UnitY * 80f);
                }
                else
                {
                    if ((double)(vector2_1 - proj.Center).Length() > 400.0 || (double)proj.ai[1] != 0.0)
                        return;
                    ++proj.ai[1];
                    if (proj.owner != Main.myPlayer)
                        return;
                    Vector2 vector2_2 = vector2_1 - proj.Center;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * num8;
                    Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vector2_3.X, vector2_3.Y, Type, proj.damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
                    proj.netUpdate = true;
                }
            }
            else if ((double)proj.ai[1] == 0.0 && proj.type == 613)
            {
                if ((double)(vector2_1 - proj.Center).Length() > 500.0 || (double)proj.ai[1] != 0.0)
                    return;
                ++proj.ai[1];
                if (proj.owner == Main.myPlayer)
                {
                    Vector2 vector2_2 = vector2_1 - proj.Center;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * num8;
                    int index = Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vector2_3.X, vector2_3.Y, Type, proj.damage, 0.0f, Main.myPlayer, 0.0f, (float)num5);
                    Main.projectile[index].timeLeft = 300;
                    Main.projectile[index].netUpdate = true;
                    proj.velocity -= vector2_3 / 3f;
                    proj.netUpdate = true;
                }
                for (int index1 = 0; index1 < 5; ++index1)
                {
                    int num1 = proj.width / 4;
                    Vector2 vector2_2 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2() * (float)Main.rand.Next(24, 41) / 8f;
                    int index2 = Dust.NewDust(proj.Center - Vector2.One * (float)num1, num1 * 2, num1 * 2, 88, 0.0f, 0.0f, 0, new Color(), 1f);
                    Dust dust = Main.dust[index2];
                    Vector2 vector2_3 = Vector2.Normalize(dust.position - proj.Center);
                    dust.position = proj.Center + vector2_3 * (float)num1 * proj.scale - new Vector2(4f);
                    dust.velocity = index1 >= 30 ? 2f * vector2_3 * (float)Main.rand.Next(45, 91) / 10f : vector2_3 * dust.velocity.Length() * 2f;
                    dust.noGravity = true;
                    dust.scale = 0.7f + Main.rand.NextFloat();
                }
            }
            else
            {
                if ((double)proj.ai[1] != 0.0)
                    return;
                ++proj.ai[1];
                if (proj.owner != Main.myPlayer)
                    return;
                Vector2 vector2_2 = vector2_1 - proj.Center;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * num8;
                int index = Projectile.NewProjectile(proj.Center.X, proj.Center.Y, vector2_3.X, vector2_3.Y, Type, proj.damage, 0.0f, Main.myPlayer, 0.0f, 0.0f);
                Main.projectile[index].timeLeft = 300;
                Main.projectile[index].netUpdate = true;
                proj.netUpdate = true;
            }
        }

        private void PhantasmAI(Projectile proj)
        {
            if (!GuardianProj.ContainsKey(proj.whoAmI) || !MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID))
            {
                proj.active = false;
                return;
            }
            TerraGuardian guardian = GuardianProj[proj.whoAmI];
            float num = 1.57079637f;
            Vector2 vector = guardian.HeldItemHand == HeldHand.Left ? guardian.GetGuardianLeftHandPosition : guardian.GetGuardianRightHandPosition; //player.RotatedRelativePoint(player.MountedCenter, true);
            bool IsThisClientCharacter = true;
            if (proj.type == 439)
            {
                proj.ai[0] += 1f;
                int num2 = 0;
                if (proj.ai[0] >= 40f)
                {
                    num2++;
                }
                if (proj.ai[0] >= 80f)
                {
                    num2++;
                }
                if (proj.ai[0] >= 120f)
                {
                    num2++;
                }
                int num3 = 24;
                int num4 = 6;
                proj.ai[1] += 1f;
                bool flag = false;
                if (proj.ai[1] >= (float)(num3 - num4 * num2))
                {
                    proj.ai[1] = 0f;
                    flag = true;
                }
                proj.frameCounter += 1 + num2;
                if (proj.frameCounter >= 4)
                {
                    proj.frameCounter = 0;
                    proj.frame++;
                    if (proj.frame >= 6)
                    {
                        proj.frame = 0;
                    }
                }
                if (proj.soundDelay <= 0)
                {
                    proj.soundDelay = num3 - num4 * num2;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 91);
                    }
                }
                if (proj.ai[1] == 1f && proj.ai[0] != 1f)
                {
                    Vector2 vector2 = Vector2.UnitX * 24f;
                    vector2 = vector2.RotatedBy((double)(proj.rotation - 1.57079637f), default(Vector2));
                    Vector2 value = proj.Center + vector2;
                    for (int i = 0; i < 2; i++)
                    {
                        int num5 = Dust.NewDust(value - Vector2.One * 8f, 16, 16, 135, proj.velocity.X / 2f, proj.velocity.Y / 2f, 100, default(Color), 1f);
                        Main.dust[num5].velocity *= 0.66f;
                        Main.dust[num5].noGravity = true;
                        Main.dust[num5].scale = 1.4f;
                    }
                }
                if (flag && IsThisClientCharacter)
                {
                    bool flag2 = guardian.Channeling && guardian.UseMana(guardian.Inventory[guardian.SelectedItem].mana) && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed;
                    if (flag2)
                    {
                        float scaleFactor = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        Vector2 value2 = vector;
                        Vector2 value3 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value2;
                        if (guardian.GravityDirection == -1f)
                        {
                            value3.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value2.Y;
                        }
                        Vector2 vector3 = Vector2.Normalize(value3);
                        if (float.IsNaN(vector3.X) || float.IsNaN(vector3.Y))
                        {
                            vector3 = -Vector2.UnitY;
                        }
                        vector3 *= scaleFactor;
                        if (vector3.X != proj.velocity.X || vector3.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector3;
                        int num6 = 440;
                        float scaleFactor2 = 14f;
                        int num7 = 7;
                        for (int j = 0; j < 2; j++)
                        {
                            value2 = proj.Center + new Vector2((float)Main.rand.Next(-num7, num7 + 1), (float)Main.rand.Next(-num7, num7 + 1));
                            Vector2 spinningpoint = Vector2.Normalize(proj.velocity) * scaleFactor2;
                            spinningpoint = spinningpoint.RotatedBy(Main.rand.NextDouble() * 0.19634954631328583 - 0.098174773156642914, default(Vector2));
                            if (float.IsNaN(spinningpoint.X) || float.IsNaN(spinningpoint.Y))
                            {
                                spinningpoint = -Vector2.UnitY;
                            }
                            Projectile.NewProjectile(value2.X, value2.Y, spinningpoint.X, spinningpoint.Y, num6, proj.damage, proj.knockBack, proj.owner, 0f, 0f);
                        }
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
            }
            if (proj.type == 445)
            {
                proj.localAI[0] += 1f;
                if (proj.localAI[0] >= 60f)
                {
                    proj.localAI[0] = 0f;
                }
                if (Vector2.Distance(vector, proj.Center) >= 5f)
                {
                    float num8 = proj.localAI[0] / 60f;
                    if (num8 > 0.5f)
                    {
                        num8 = 1f - num8;
                    }
                    Vector3 value4 = new Vector3(0f, 1f, 0.7f);
                    Vector3 value5 = new Vector3(0f, 0.7f, 1f);
                    Vector3 value6 = Vector3.Lerp(value4, value5, 1f - num8 * 2f) * 0.5f;
                    if (Vector2.Distance(vector, proj.Center) >= 30f)
                    {
                        Vector2 vector4 = proj.Center - vector;
                        vector4.Normalize();
                        vector4 *= Vector2.Distance(vector, proj.Center) - 30f;
                        DelegateMethods.v3_1 = value6 * 0.8f;
                        Utils.PlotTileLine(proj.Center - vector4, proj.Center, 8f, new Utils.PerLinePoint(DelegateMethods.CastLightOpen));
                    }
                    Lighting.AddLight((int)proj.Center.X / 16, (int)proj.Center.Y / 16, value6.X, value6.Y, value6.Z);
                }
                if (IsThisClientCharacter)
                {
                    if (proj.localAI[1] > 0f)
                    {
                        proj.localAI[1] -= 1f;
                    }
                    if (!guardian.Channeling || guardian.HasFlag(GuardianFlags.Cursed) || guardian.CCed)
                    {
                        proj.Kill();
                    }
                    else if (proj.localAI[1] == 0f)
                    {
                        Vector2 value7 = vector;
                        Vector2 vector5 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value7;
                        if (guardian.GravityDirection == -1f)
                        {
                            vector5.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value7.Y;
                        }
                        Tile tile = Main.tile[Player.tileTargetX, Player.tileTargetY];
                        if (tile.active())
                        {
                            vector5 = new Vector2((float)Player.tileTargetX, (float)Player.tileTargetY) * 16f + Vector2.One * 8f - value7;
                            proj.localAI[1] = 2f;
                        }
                        vector5 = Vector2.Lerp(vector5, proj.velocity, 0.7f);
                        if (float.IsNaN(vector5.X) || float.IsNaN(vector5.Y))
                        {
                            vector5 = -Vector2.UnitY;
                        }
                        float num9 = 30f;
                        if (vector5.Length() < num9)
                        {
                            vector5 = Vector2.Normalize(vector5) * num9;
                        }
                        int tileBoost = guardian.Inventory[guardian.SelectedItem].tileBoost;
                        int num10 = -Player.tileRangeX - tileBoost + 1;
                        int num11 = Player.tileRangeX + tileBoost - 1;
                        int num12 = -Player.tileRangeY - tileBoost;
                        int num13 = Player.tileRangeY + tileBoost - 1;
                        int num14 = 12;
                        bool flag3 = false;
                        if (vector5.X < (float)(num10 * 16 - num14))
                        {
                            flag3 = true;
                        }
                        if (vector5.Y < (float)(num12 * 16 - num14))
                        {
                            flag3 = true;
                        }
                        if (vector5.X > (float)(num11 * 16 + num14))
                        {
                            flag3 = true;
                        }
                        if (vector5.Y > (float)(num13 * 16 + num14))
                        {
                            flag3 = true;
                        }
                        if (flag3)
                        {
                            Vector2 value8 = Vector2.Normalize(vector5);
                            float num15 = -1f;
                            if (value8.X < 0f && ((float)(num10 * 16 - num14) / value8.X < num15 || num15 == -1f))
                            {
                                num15 = (float)(num10 * 16 - num14) / value8.X;
                            }
                            if (value8.X > 0f && ((float)(num11 * 16 + num14) / value8.X < num15 || num15 == -1f))
                            {
                                num15 = (float)(num11 * 16 + num14) / value8.X;
                            }
                            if (value8.Y < 0f && ((float)(num12 * 16 - num14) / value8.Y < num15 || num15 == -1f))
                            {
                                num15 = (float)(num12 * 16 - num14) / value8.Y;
                            }
                            if (value8.Y > 0f && ((float)(num13 * 16 + num14) / value8.Y < num15 || num15 == -1f))
                            {
                                num15 = (float)(num13 * 16 + num14) / value8.Y;
                            }
                            vector5 = value8 * num15;
                        }
                        if (vector5.X != proj.velocity.X || vector5.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector5;
                    }
                }
            }
            if (proj.type == 460)
            {
                proj.ai[0] += 1f;
                int num16 = 0;
                if (proj.ai[0] >= 60f)
                {
                    num16++;
                }
                if (proj.ai[0] >= 180f)
                {
                    num16++;
                }
                bool flag4 = false;
                if (proj.ai[0] == 60f || proj.ai[0] == 180f || (proj.ai[0] > 180f && proj.ai[0] % 20f == 0f))
                {
                    flag4 = true;
                }
                bool flag5 = proj.ai[0] >= 180f;
                int num17 = 10;
                if (!flag5)
                {
                    proj.ai[1] += 1f;
                }
                bool flag6 = false;
                if (flag5 && proj.ai[0] % 20f == 0f)
                {
                    flag6 = true;
                }
                if (proj.ai[1] >= (float)num17 && !flag5)
                {
                    proj.ai[1] = 0f;
                    flag6 = true;
                    if (!flag5)
                    {
                        float scaleFactor3 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        Vector2 value9 = vector;
                        Vector2 value10 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value9;
                        if (guardian.GravityDirection == -1f)
                        {
                            value10.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value9.Y;
                        }
                        Vector2 vector6 = Vector2.Normalize(value10);
                        if (float.IsNaN(vector6.X) || float.IsNaN(vector6.Y))
                        {
                            vector6 = -Vector2.UnitY;
                        }
                        vector6 *= scaleFactor3;
                        if (vector6.X != proj.velocity.X || vector6.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector6;
                    }
                }
                if (proj.soundDelay <= 0 && !flag5)
                {
                    proj.soundDelay = num17 - num16;
                    proj.soundDelay *= 2;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 15);
                    }
                }
                if (proj.ai[0] > 10f && !flag5)
                {
                    Vector2 vector7 = Vector2.UnitX * 18f;
                    vector7 = vector7.RotatedBy((double)(proj.rotation - 1.57079637f), default(Vector2));
                    Vector2 value11 = proj.Center + vector7;
                    for (int k = 0; k < num16 + 1; k++)
                    {
                        int num18 = 226;
                        float num19 = 0.4f;
                        if (k % 2 == 1)
                        {
                            num18 = 226;
                            num19 = 0.65f;
                        }
                        Vector2 vector8 = value11 + ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (12f - (float)(num16 * 2));
                        int num20 = Dust.NewDust(vector8 - Vector2.One * 8f, 16, 16, num18, proj.velocity.X / 2f, proj.velocity.Y / 2f, 0, default(Color), 1f);
                        Main.dust[num20].velocity = Vector2.Normalize(value11 - vector8) * 1.5f * (10f - (float)num16 * 2f) / 10f;
                        Main.dust[num20].noGravity = true;
                        Main.dust[num20].scale = num19;
                        //Main.dust[num20].customData = player; //Will this crash the game?
                    }
                }
                if (flag6 && IsThisClientCharacter)
                {
                    bool flag7 = !flag4 || guardian.UseMana(guardian.Inventory[guardian.SelectedItem].mana);
                    bool flag8 = guardian.Channeling && flag7 && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed;
                    if (flag8)
                    {
                        if (proj.ai[0] == 180f)
                        {
                            Vector2 center = proj.Center;
                            Vector2 vector9 = Vector2.Normalize(proj.velocity);
                            if (float.IsNaN(vector9.X) || float.IsNaN(vector9.Y))
                            {
                                vector9 = -Vector2.UnitY;
                            }
                            int num21 = (int)((float)proj.damage * 3f);
                            int num22 = Projectile.NewProjectile(center.X, center.Y, vector9.X, vector9.Y, 461, num21, proj.knockBack, proj.owner, 0f, (float)proj.whoAmI);
                            proj.ai[1] = (float)num22;
                            proj.netUpdate = true;
                        }
                        else if (flag5)
                        {
                            Projectile projectile = Main.projectile[(int)proj.ai[1]];
                            if (!projectile.active || projectile.type != 461)
                            {
                                proj.Kill();
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!flag5)
                        {
                            int num23 = 459;
                            float scaleFactor4 = 10f;
                            Vector2 center2 = proj.Center;
                            Vector2 vector10 = Vector2.Normalize(proj.velocity) * scaleFactor4;
                            if (float.IsNaN(vector10.X) || float.IsNaN(vector10.Y))
                            {
                                vector10 = -Vector2.UnitY;
                            }
                            float num24 = 0.7f + (float)num16 * 0.3f;
                            int num25 = (num24 < 1f) ? proj.damage : ((int)((float)proj.damage * 2f));
                            Projectile.NewProjectile(center2.X, center2.Y, vector10.X, vector10.Y, num23, num25, proj.knockBack, proj.owner, 0f, num24);
                        }
                        proj.Kill();
                    }
                }
            }
            if (proj.type == 633)
            {
                float num26 = 30f;
                if (proj.ai[0] > 90f)
                {
                    num26 = 15f;
                }
                if (proj.ai[0] > 120f)
                {
                    num26 = 5f;
                }
                proj.damage = (int)((float)guardian.Inventory[guardian.SelectedItem].damage * guardian.MagicDamageMultiplier);
                proj.ai[0] += 1f;
                proj.ai[1] += 1f;
                bool flag9 = false;
                if (proj.ai[0] % num26 == 0f)
                {
                    flag9 = true;
                }
                int num27 = 10;
                bool flag10 = false;
                if (proj.ai[0] % num26 == 0f)
                {
                    flag10 = true;
                }
                if (proj.ai[1] >= 1f)
                {
                    proj.ai[1] = 0f;
                    flag10 = true;
                    if (IsThisClientCharacter)
                    {
                        float scaleFactor5 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        Vector2 value12 = vector;
                        Vector2 value13 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value12;
                        if (guardian.GravityDirection == -1f)
                        {
                            value13.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value12.Y;
                        }
                        Vector2 vector11 = Vector2.Normalize(value13);
                        if (float.IsNaN(vector11.X) || float.IsNaN(vector11.Y))
                        {
                            vector11 = -Vector2.UnitY;
                        }
                        vector11 = Vector2.Normalize(Vector2.Lerp(vector11, Vector2.Normalize(proj.velocity), 0.92f));
                        vector11 *= scaleFactor5;
                        if (vector11.X != proj.velocity.X || vector11.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector11;
                    }
                }
                proj.frameCounter++;
                int num28 = (proj.ai[0] < 120f) ? 4 : 1;
                if (proj.frameCounter >= num28)
                {
                    proj.frameCounter = 0;
                    if (++proj.frame >= 5)
                    {
                        proj.frame = 0;
                    }
                }
                if (proj.soundDelay <= 0)
                {
                    proj.soundDelay = num27;
                    proj.soundDelay *= 2;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 15);
                    }
                }
                if (flag10 && IsThisClientCharacter)
                {
                    bool flag11 = !flag9 || guardian.UseMana(guardian.Inventory[guardian.SelectedItem].mana);
                    bool flag12 = guardian.Channeling && flag11 && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed;
                    if (flag12)
                    {
                        if (proj.ai[0] == 1f)
                        {
                            Vector2 center3 = proj.Center;
                            Vector2 vector12 = Vector2.Normalize(proj.velocity);
                            if (float.IsNaN(vector12.X) || float.IsNaN(vector12.Y))
                            {
                                vector12 = -Vector2.UnitY;
                            }
                            int num29 = proj.damage;
                            for (int l = 0; l < 6; l++)
                            {
                                Projectile.NewProjectile(center3.X, center3.Y, vector12.X, vector12.Y, 632, num29, proj.knockBack, proj.owner, (float)l, (float)proj.whoAmI);
                            }
                            proj.netUpdate = true;
                        }
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
            }
            if (proj.type == 595)
            {
                num = 0f;
                if (proj.spriteDirection == -1)
                {
                    num = 3.14159274f;
                }
                if (++proj.frame >= Main.projFrames[proj.type])
                {
                    proj.frame = 0;
                }
                proj.soundDelay--;
                if (proj.soundDelay <= 0)
                {
                    Main.PlaySound(2, (int)proj.Center.X, (int)proj.Center.Y, 1);
                    proj.soundDelay = 12;
                }
                if (IsThisClientCharacter)
                {
                    if (guardian.Channeling && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed)
                    {
                        float scaleFactor6 = 1f;
                        if (guardian.Inventory[guardian.SelectedItem].shoot == proj.type)
                        {
                            scaleFactor6 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        }
                        Vector2 vector13 = Main.MouseWorld - vector;
                        vector13.Normalize();
                        if (vector13.HasNaNs())
                        {
                            vector13 = Vector2.UnitX * (float)guardian.Direction;
                        }
                        vector13 *= scaleFactor6;
                        if (vector13.X != proj.velocity.X || vector13.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector13;
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
                Vector2 vector14 = proj.Center + proj.velocity * 3f;
                Lighting.AddLight(vector14, 0.8f, 0.8f, 0.8f);
                if (Main.rand.Next(3) == 0)
                {
                    int num30 = Dust.NewDust(vector14 - proj.Size / 2f, proj.width, proj.height, 63, proj.velocity.X, proj.velocity.Y, 100, default(Color), 2f);
                    Main.dust[num30].noGravity = true;
                    Main.dust[num30].position -= proj.velocity;
                }
            }
            if (proj.type == 600)
            {
                if (proj.ai[0] == 0f)
                {
                    if (proj.ai[1] != 0f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 114);
                    }
                    else
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 115);
                    }
                }
                proj.ai[0] += 1f;
                if (IsThisClientCharacter && proj.ai[0] == 1f)
                {
                    float scaleFactor7 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                    Vector2 value14 = vector;
                    Vector2 value15 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value14;
                    if (guardian.GravityDirection == -1f)
                    {
                        value15.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value14.Y;
                    }
                    Vector2 vector15 = Vector2.Normalize(value15);
                    if (float.IsNaN(vector15.X) || float.IsNaN(vector15.Y))
                    {
                        vector15 = -Vector2.UnitY;
                    }
                    vector15 *= scaleFactor7;
                    if (vector15.X != proj.velocity.X || vector15.Y != proj.velocity.Y)
                    {
                        proj.netUpdate = true;
                    }
                    proj.velocity = vector15;
                    int num31 = 601;
                    float scaleFactor8 = 3f;
                    value14 = proj.Center;
                    Vector2 vector16 = Vector2.Normalize(proj.velocity) * scaleFactor8;
                    if (float.IsNaN(vector16.X) || float.IsNaN(vector16.Y))
                    {
                        vector16 = -Vector2.UnitY;
                    }
                    Projectile.NewProjectile(value14.X, value14.Y, vector16.X, vector16.Y, num31, proj.damage, proj.knockBack, proj.owner, proj.ai[1], 0f);
                }
                if (proj.ai[0] >= 30f)
                {
                    proj.Kill();
                }
            }
            if (proj.type == 611)
            {
                if (proj.localAI[1] > 0f)
                {
                    proj.localAI[1] -= 1f;
                }
                proj.alpha -= 42;
                if (proj.alpha < 0)
                {
                    proj.alpha = 0;
                }
                if (proj.localAI[0] == 0f)
                {
                    proj.localAI[0] = proj.velocity.ToRotation();
                }
                float num32 = (float)((proj.localAI[0].ToRotationVector2().X >= 0f) ? 1 : -1);
                if (proj.ai[1] <= 0f)
                {
                    num32 *= -1f;
                }
                Vector2 vector17 = (num32 * (proj.ai[0] / 30f * 6.28318548f - 1.57079637f)).ToRotationVector2();
                vector17.Y *= (float)Math.Sin((double)proj.ai[1]);
                if (proj.ai[1] <= 0f)
                {
                    vector17.Y *= -1f;
                }
                vector17 = vector17.RotatedBy((double)proj.localAI[0], default(Vector2));
                proj.ai[0] += 1f;
                if (proj.ai[0] < 30f)
                {
                    proj.velocity += 48f * vector17;
                }
                else
                {
                    proj.Kill();
                }
            }
            if (proj.type == 615)
            {
                num = 0f;
                if (proj.spriteDirection == -1)
                {
                    num = 3.14159274f;
                }
                proj.ai[0] += 1f;
                int num33 = 0;
                if (proj.ai[0] >= 40f)
                {
                    num33++;
                }
                if (proj.ai[0] >= 80f)
                {
                    num33++;
                }
                if (proj.ai[0] >= 120f)
                {
                    num33++;
                }
                int num34 = 5;
                int num35 = 0;
                proj.ai[1] -= 1f;
                bool flag13 = false;
                int num36 = -1;
                if (proj.ai[1] <= 0f)
                {
                    proj.ai[1] = (float)(num34 - num35 * num33);
                    flag13 = true;
                    int num37 = (int)proj.ai[0] / (num34 - num35 * num33);
                    if (num37 % 7 == 0)
                    {
                        num36 = 0;
                    }
                }
                proj.frameCounter += 1 + num33;
                if (proj.frameCounter >= 4)
                {
                    proj.frameCounter = 0;
                    proj.frame++;
                    if (proj.frame >= Main.projFrames[proj.type])
                    {
                        proj.frame = 0;
                    }
                }
                if (proj.soundDelay <= 0)
                {
                    proj.soundDelay = num34 - num35 * num33;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 36);
                    }
                }
                if (flag13 && IsThisClientCharacter)
                {
                    bool flag14 = guardian.Channeling && guardian.HasAmmo(guardian.Inventory[guardian.SelectedItem]) && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed;
                    int num38 = 14;
                    float scaleFactor9 = 14f;
                    int weaponDamage = guardian.Inventory[guardian.SelectedItem].damage;
                    float weaponKnockback = guardian.Inventory[guardian.SelectedItem].knockBack;
                    if (flag14)
                    {
                        {
                            int projid, damage;
                            float speed, kb;
                            guardian.GetAmmoInfo(guardian.Inventory[guardian.SelectedItem], true, out projid, out speed, out damage, out kb);
                            num38 = projid;
                            scaleFactor9 = speed;
                            weaponDamage += damage;
                            weaponKnockback += kb;
                        }
                        float scaleFactor10 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        Vector2 value16 = vector;
                        Vector2 value17 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - value16;
                        if (guardian.GravityDirection == -1f)
                        {
                            value17.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - value16.Y;
                        }
                        Vector2 vector18 = Vector2.Normalize(value17);
                        if (float.IsNaN(vector18.X) || float.IsNaN(vector18.Y))
                        {
                            vector18 = -Vector2.UnitY;
                        }
                        vector18 *= scaleFactor10;
                        vector18 = vector18.RotatedBy(Main.rand.NextDouble() * 0.13089969754219055 - 0.065449848771095276, default(Vector2));
                        if (vector18.X != proj.velocity.X || vector18.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = vector18;
                        for (int m = 0; m < 1; m++)
                        {
                            Vector2 spinningpoint2 = Vector2.Normalize(proj.velocity) * scaleFactor9;
                            spinningpoint2 = spinningpoint2.RotatedBy(Main.rand.NextDouble() * 0.19634954631328583 - 0.098174773156642914, default(Vector2));
                            if (float.IsNaN(spinningpoint2.X) || float.IsNaN(spinningpoint2.Y))
                            {
                                spinningpoint2 = -Vector2.UnitY;
                            }
                            Projectile.NewProjectile(value16.X, value16.Y, spinningpoint2.X, spinningpoint2.Y, num38, weaponDamage, weaponKnockback, proj.owner, 0f, 0f);
                        }
                        if (num36 == 0)
                        {
                            num38 = 616;
                            scaleFactor9 = 8f;
                            for (int n = 0; n < 1; n++)
                            {
                                Vector2 spinningpoint3 = Vector2.Normalize(proj.velocity) * scaleFactor9;
                                spinningpoint3 = spinningpoint3.RotatedBy(Main.rand.NextDouble() * 0.39269909262657166 - 0.19634954631328583, default(Vector2));
                                if (float.IsNaN(spinningpoint3.X) || float.IsNaN(spinningpoint3.Y))
                                {
                                    spinningpoint3 = -Vector2.UnitY;
                                }
                                Projectile.NewProjectile(value16.X, value16.Y, spinningpoint3.X, spinningpoint3.Y, num38, weaponDamage + 20, weaponKnockback * 1.25f, proj.owner, 0f, 0f);
                            }
                        }
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
            }
            if (proj.type == 630)
            {
                num = 0f;
                if (proj.spriteDirection == -1)
                {
                    num = 3.14159274f;
                }
                proj.ai[0] += 1f;
                int num39 = 0;
                if (proj.ai[0] >= 40f)
                {
                    num39++;
                }
                if (proj.ai[0] >= 80f)
                {
                    num39++;
                }
                if (proj.ai[0] >= 120f)
                {
                    num39++;
                }
                int num40 = 24;
                int num41 = 2;
                proj.ai[1] -= 1f;
                bool flag15 = false;
                if (proj.ai[1] <= 0f)
                {
                    proj.ai[1] = (float)(num40 - num41 * num39);
                    flag15 = true;
                    int arg_1F5C_0 = (int)proj.ai[0] / (num40 - num41 * num39);
                }
                bool flag16 = guardian.Channeling && guardian.HasAmmo(guardian.Inventory[guardian.SelectedItem]) && !guardian.HasFlag(GuardianFlags.Cursed) && !guardian.CCed;
                if (proj.localAI[0] > 0f)
                {
                    proj.localAI[0] -= 1f;
                }
                if (proj.soundDelay <= 0 && flag16)
                {
                    proj.soundDelay = num40 - num41 * num39;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 5);
                    }
                    proj.localAI[0] = 12f;
                }
                guardian.SetCooldownValue(GuardianCooldownManager.CooldownType.PhantasmCooldown, 2);
                if (flag15 && IsThisClientCharacter)
                {
                    int num42 = 14;
                    float scaleFactor11 = 14f;
                    int weaponDamage2 = guardian.Inventory[guardian.SelectedItem].damage;
                    float weaponKnockback2 = guardian.Inventory[guardian.SelectedItem].knockBack;
                    if (flag16)
                    {
                        {
                            int projid, damage;
                            float speed, kb;
                            guardian.GetAmmoInfo(guardian.Inventory[guardian.SelectedItem], true, out projid, out speed, out damage, out kb);
                            num42 = projid;
                            scaleFactor11 = speed;
                            weaponDamage2 += damage;
                            weaponKnockback2 += kb;
                        }
                        float scaleFactor12 = guardian.Inventory[guardian.SelectedItem].shootSpeed * proj.scale;
                        Vector2 vector19 = vector;
                        Vector2 value18 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - vector19;
                        if (guardian.GravityDirection == -1f)
                        {
                            value18.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - vector19.Y;
                        }
                        Vector2 value19 = Vector2.Normalize(value18);
                        if (float.IsNaN(value19.X) || float.IsNaN(value19.Y))
                        {
                            value19 = -Vector2.UnitY;
                        }
                        value19 *= scaleFactor12;
                        if (value19.X != proj.velocity.X || value19.Y != proj.velocity.Y)
                        {
                            proj.netUpdate = true;
                        }
                        proj.velocity = value19 * 0.55f;
                        for (int num43 = 0; num43 < 4; num43++)
                        {
                            Vector2 vector20 = Vector2.Normalize(proj.velocity) * scaleFactor11 * (0.6f + Main.rand.NextFloat() * 0.8f);
                            if (float.IsNaN(vector20.X) || float.IsNaN(vector20.Y))
                            {
                                vector20 = -Vector2.UnitY;
                            }
                            Vector2 vector21 = vector19 + Utils.RandomVector2(Main.rand, -15f, 15f);
                            int num44 = Projectile.NewProjectile(vector21.X, vector21.Y, vector20.X, vector20.Y, num42, weaponDamage2, weaponKnockback2, proj.owner, 0f, 0f);
                            Main.projectile[num44].noDropItem = true;
                        }
                    }
                    else
                    {
                        proj.Kill();
                    }
                }
            }
            proj.position = (guardian.HeldItemHand == HeldHand.Left ? guardian.GetGuardianLeftHandPosition : guardian.GetGuardianRightHandPosition) - proj.Size / 2f;
            proj.rotation = proj.velocity.ToRotation() + num;
            proj.spriteDirection = proj.direction;
            proj.timeLeft = 2;
            guardian.FaceDirection(proj.direction);
            guardian.HeldProj = proj.whoAmI;
            guardian.ItemUseTime = 2;
            guardian.ItemAnimationTime = 2;
            guardian.ItemRotation = (float)Math.Atan2((double)(proj.velocity.Y * (float)proj.direction), (double)(proj.velocity.X * (float)proj.direction));
            if (proj.type == 460 || proj.type == 611)
            {
                /*Vector2 vector22 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
                if (guardian.Direction != 1)
                {
                    vector22.X = (float)player.bodyFrame.Width - vector22.X;
                }
                if (player.gravDir != 1f)
                {
                    vector22.Y = (float)player.bodyFrame.Height - vector22.Y;
                }
                vector22 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
                base.Center = player.RotatedRelativePoint(player.position + vector22, true) - proj.velocity;*/
                //proj.Center = (guardian.HeldItemHand == HeldHand.Left ? guardian.GetGuardianLeftHandPosition : guardian.GetGuardianRightHandPosition);
            }
            if (proj.type == 615)
            {
                proj.position.Y = proj.position.Y + guardian.GravityDirection * 2f;
            }
            if (proj.type == 611 && proj.alpha == 0)
            {
                for (int num45 = 0; num45 < 2; num45++)
                {
                    Dust dust = Main.dust[Dust.NewDust(proj.position + proj.velocity * 2f, proj.width, proj.height, 6, 0f, 0f, 100, Color.Transparent, 2f)];
                    dust.noGravity = true;
                    dust.velocity *= 2f;
                    dust.velocity += proj.localAI[0].ToRotationVector2();
                    dust.fadeIn = 1.5f;
                }
                float num46 = 18f;
                int num47 = 0;
                while ((float)num47 < num46)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        Vector2 position = proj.position + proj.velocity + proj.velocity * ((float)num47 / num46);
                        Dust dust2 = Main.dust[Dust.NewDust(position, proj.width, proj.height, 6, 0f, 0f, 100, Color.Transparent, 1f)];
                        dust2.noGravity = true;
                        dust2.fadeIn = 0.5f;
                        dust2.velocity += proj.localAI[0].ToRotationVector2();
                        dust2.noLight = true;
                    }
                    num47++;
                }
            }
        }

        private void MedusaRayAI(Projectile proj)
        {
            if (!GuardianProj.ContainsKey(proj.whoAmI) || !MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID))
            {
                proj.active = false;
                return;
            }
            TerraGuardian guardian = GuardianProj[proj.whoAmI];
            Vector2 zero2 = Vector2.Zero;
            bool IsThisClientCharacter = true;
            if (proj.type == 535)
            {
                zero2.X = (float)guardian.Direction * 6f;
                zero2.Y = guardian.GravityDirection * -14f;
                proj.ai[0] += 1f;
                int num914 = 0;
                if (proj.ai[0] >= 60f)
                {
                    num914++;
                }
                if (proj.ai[0] >= 180f)
                {
                    num914++;
                }
                if (proj.ai[0] >= 240f)
                {
                    proj.Kill();
                    return;
                }
                bool flag40 = false;
                if (proj.ai[0] == 60f || proj.ai[0] == 180f)
                {
                    flag40 = true;
                }
                bool flag41 = proj.ai[0] >= 180f;
                if (flag41)
                {
                    if (proj.frame < 8)
                    {
                        proj.frame = 8;
                    }
                    if (proj.frame >= 12)
                    {
                        proj.frame = 8;
                    }
                    proj.frameCounter++;
                    if (++proj.frameCounter >= 5)
                    {
                        proj.frameCounter = 0;
                        if (++proj.frame >= 12)
                        {
                            proj.frame = 8;
                        }
                    }
                }
                else if (++proj.frameCounter >= 5)
                {
                    proj.frameCounter = 0;
                    if (++proj.frame >= 8)
                    {
                        proj.frame = 0;
                    }
                }
                Vector2 center11 = guardian.CenterPosition;
                Vector2 vector94 = new Vector2((float)guardian.AimDirection.X, (float)guardian.AimDirection.Y) - center11;
                if (guardian.GravityDirection == -1f)
                {
                    vector94.Y = (float)(Main.screenHeight - guardian.AimDirection.Y) - center11.Y;
                }
                Vector2 velocity2 = new Vector2((float)Math.Sign((vector94.X == 0f) ? ((float)guardian.Direction) : vector94.X), 0f);
                if (velocity2.X != proj.velocity.X || velocity2.Y != proj.velocity.Y)
                {
                    proj.netUpdate = true;
                }
                proj.velocity = velocity2;
                if (proj.soundDelay <= 0 && !flag41)
                {
                    proj.soundDelay = 10;
                    proj.soundDelay *= 2;
                    if (proj.ai[0] != 1f)
                    {
                        Main.PlaySound(2, (int)proj.position.X, (int)proj.position.Y, 15);
                    }
                }
                if (proj.ai[0] == 181f)
                {
                    Main.PlaySound(4, (int)proj.position.X, (int)proj.position.Y, 17);
                }
                if (proj.ai[0] > 10f && !flag41)
                {
                    Vector2 vector95 = proj.Center + new Vector2((float)(guardian.Direction * 2), guardian.GravityDirection * 5f);
                    float scaleFactor10 = MathHelper.Lerp(30f, 10f, (proj.ai[0] - 10f) / 180f);
                    float num915 = Main.rand.NextFloat() * 6.28318548f;
                    for (float num916 = 0f; num916 < 1f; num916 += 1f)
                    {
                        Vector2 value57 = Vector2.UnitY.RotatedBy((double)(num916 / 1f * 6.28318548f + num915), default(Vector2));
                        Dust dust9 = Main.dust[Dust.NewDust(vector95, 0, 0, 228, 0f, 0f, 0, default(Color), 1f)];
                        dust9.position = vector95 + value57 * scaleFactor10;
                        dust9.noGravity = true;
                        //dust9.customData = player5; //Please don't crash
                        dust9.velocity = value57 * -2f;
                    }
                }
                if (proj.ai[0] > 180f && proj.ai[0] <= 182f)
                {
                    Vector2 vector96 = proj.Center + new Vector2((float)(guardian.Direction * 2), guardian.GravityDirection * 5f);
                    float scaleFactor11 = MathHelper.Lerp(20f, 30f, (proj.ai[0] - 180f) / 182f);
                    Main.rand.NextFloat();
                    for (float num917 = 0f; num917 < 10f; num917 += 1f)
                    {
                        Vector2 value58 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * (Main.rand.NextFloat() * 0.5f + 0.5f);
                        Dust dust10 = Main.dust[Dust.NewDust(vector96, 0, 0, 228, 0f, 0f, 0, default(Color), 1f)];
                        dust10.position = vector96 + value58 * scaleFactor11;
                        dust10.noGravity = true;
                        //dust10.customData = player5;
                        dust10.velocity = value58 * 4f;
                        dust10.scale = 0.5f + Main.rand.NextFloat();
                    }
                }
                if (IsThisClientCharacter)
                {
                    bool flag42 = !flag40 || guardian.UseMana(guardian.Inventory[guardian.SelectedItem].mana);
                    bool flag43 = guardian.Channeling && flag42;
                    if ((!flag41 && !flag43) || proj.ai[0] == 180f)
                    {
                        Vector2 vector97 = guardian.CenterPosition + new Vector2((float)(guardian.Direction * 4), guardian.GravityDirection * 2f);
                        int num918 = proj.damage * (1 + num914);
                        vector97 = guardian.CenterPosition;
                        int num919 = 0;
                        float num920 = 0f;
                        for (int num921 = 0; num921 < 200; num921++)
                        {
                            NPC nPC9 = Main.npc[num921];
                            if (nPC9.active && proj.Distance(nPC9.Center) < 500f && nPC9.CanBeChasedBy(this, false) && Collision.CanHitLine(nPC9.position, nPC9.width, nPC9.height, vector97, 0, 0))
                            {
                                Vector2 v4 = nPC9.Center - vector97;
                                num920 += v4.ToRotation();
                                num919++;
                                int num922 = Projectile.NewProjectile(vector97.X, vector97.Y, v4.X, v4.Y, 536, 0, 0f, proj.owner, (float)proj.whoAmI, 0f);
                                Main.projectile[num922].Center = nPC9.Center;
                                Main.projectile[num922].damage = num918;
                                Main.projectile[num922].Damage();
                                Main.projectile[num922].damage = 0;
                                Main.projectile[num922].Center = vector97;
                                proj.ai[0] = 180f;
                            }
                        }
                        if (num919 != 0)
                        {
                            num920 /= (float)num919;
                        }
                        else
                        {
                            num920 = ((guardian.Direction == 1) ? 0f : 3.14159274f);
                        }
                        for (int num923 = 0; num923 < 6; num923++)
                        {
                            Vector2 vector98 = Vector2.Zero;
                            if (Main.rand.Next(4) != 0)
                            {
                                vector98 = Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy((double)num920, default(Vector2)) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
                            }
                            else
                            {
                                vector98 = Vector2.UnitX.RotatedByRandom(6.2831854820251465) * new Vector2(200f, 50f) * (Main.rand.NextFloat() * 0.7f + 0.3f);
                            }
                            Projectile.NewProjectile(vector97.X, vector97.Y, vector98.X, vector98.Y, 536, 0, 0f, proj.owner, (float)proj.whoAmI, 0f);
                        }
                        proj.ai[0] = 180f;
                        proj.netUpdate = true;
                    }
                }
                Lighting.AddLight(proj.Center, 0.9f, 0.75f, 0.1f);
            }
            proj.rotation = ((guardian.GravityDirection == 1f) ? 0f : 3.14159274f);
            proj.spriteDirection = proj.direction;
            proj.timeLeft = 2;
            Vector2 vector99 = guardian.HeldItemHand == HeldHand.Left ? guardian.GetGuardianLeftHandPosition : guardian.GetGuardianRightHandPosition;
            proj.Center = vector99.Floor();
            guardian.FaceDirection(proj.direction);
            guardian.HeldProj = proj.whoAmI;
            guardian.ItemUseTime = 2;
            guardian.ItemAnimationTime = 2;
        }

        private void LifeDrainBlobAI(Projectile proj)
        {
            if (!GuardianProj.ContainsKey(proj.whoAmI) || !MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID))
            {
                proj.active = false;
                return;
            }
            TerraGuardian guardian = GuardianProj[proj.whoAmI];
            bool IsThisClientCharacter = true;
            int num487 = (int)proj.ai[0];
            float num488 = 4f;
            Vector2 vector36 = new Vector2(proj.position.X + (float)proj.width * 0.5f, proj.position.Y + (float)proj.height * 0.5f);
            float num489 = Main.player[num487].Center.X - vector36.X;
            float num490 = Main.player[num487].Center.Y - vector36.Y;
            float num491 = (float)Math.Sqrt((double)(num489 * num489 + num490 * num490));
            if (num491 < 50f && proj.position.X < guardian.Position.X + (float)guardian.Width * 0.5f && proj.position.X + (float)proj.width > guardian.Position.X - guardian.Width * 0.5f && proj.position.Y < guardian.Position.Y && proj.position.Y + (float)proj.height > guardian.Position.Y - guardian.Height)
            {
                if (IsThisClientCharacter && !guardian.HasFlag(GuardianFlags.MoonLeech))
                {
                    int num492 = (int)proj.ai[1];
                    //Main.player[num487].HealEffect(num492, false);
                    //Main.player[num487].statLife += num492;
                    /*if (Main.player[num487].statLife > Main.player[num487].statLifeMax2)
                    {
                        Main.player[num487].statLife = Main.player[num487].statLifeMax2;
                    }*/
                    if (guardian.OwnerPos > -1)
                    {
                        Player owner = Main.player[guardian.OwnerPos];
                        owner.HealEffect(num492, false);
                        owner.statLife += num492;
                        if (owner.statLife > owner.statLifeMax2)
                        {
                            owner.statLife = owner.statLifeMax2;
                        }
                        owner.GetModPlayer<PlayerMod>().ShareHealthReplenishWithGuardians(num492);
                    }
                    else
                    {
                        guardian.RestoreHP(num492);
                    }
                    //NetMessage.SendData(66, -1, -1, "", num487, (float)num492, 0f, 0f, 0, 0, 0);
                }
                proj.Kill();
            }
            num491 = num488 / num491;
            num489 *= num491;
            num490 *= num491;
            proj.velocity.X = (proj.velocity.X * 15f + num489) / 16f;
            proj.velocity.Y = (proj.velocity.Y * 15f + num490) / 16f;
            if (proj.type == 305)
            {
                for (int num493 = 0; num493 < 3; num493++)
                {
                    float num494 = proj.velocity.X * 0.334f * (float)num493;
                    float num495 = -(proj.velocity.Y * 0.334f) * (float)num493;
                    int num496 = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 183, 0f, 0f, 100, default(Color), 1.1f);
                    Main.dust[num496].noGravity = true;
                    Main.dust[num496].velocity *= 0f;
                    Dust expr_153E2_cp_0 = Main.dust[num496];
                    expr_153E2_cp_0.position.X = expr_153E2_cp_0.position.X - num494;
                    Dust expr_15401_cp_0 = Main.dust[num496];
                    expr_15401_cp_0.position.Y = expr_15401_cp_0.position.Y - num495;
                }
                return;
            }
            for (int num497 = 0; num497 < 5; num497++)
            {
                float num498 = proj.velocity.X * 0.2f * (float)num497;
                float num499 = -(proj.velocity.Y * 0.2f) * (float)num497;
                int num500 = Dust.NewDust(new Vector2(proj.position.X, proj.position.Y), proj.width, proj.height, 175, 0f, 0f, 100, default(Color), 1.3f);
                Main.dust[num500].noGravity = true;
                Main.dust[num500].velocity *= 0f;
                Dust expr_154F9_cp_0 = Main.dust[num500];
                expr_154F9_cp_0.position.X = expr_154F9_cp_0.position.X - num498;
                Dust expr_15518_cp_0 = Main.dust[num500];
                expr_15518_cp_0.position.Y = expr_15518_cp_0.position.Y - num499;
            }
        }
    }
}
