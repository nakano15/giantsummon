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
            if (!MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID) || !GuardianProj.ContainsKey(proj.whoAmI))
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
                        if (guardian.RocketTime > 0)
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
            if (!MainMod.ActiveGuardians.ContainsKey(GuardianProj[proj.whoAmI].WhoAmID) || !GuardianProj.ContainsKey(proj.whoAmI))
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

    }
}
