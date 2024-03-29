﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public partial class ProjMod : GlobalProjectile
    {
        public static Dictionary<int, TerraGuardian> GuardianProj = new Dictionary<int, TerraGuardian>();
        public static int ProjParent = -1;
        public static PlayerDataBackup backup, drawBackup;
        public static bool BackupUsed = false, drawBackupUsed = false;
        private int MyParent = -1;
        public bool SpawnClearOwnership = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public static void CheckForInactiveProjectiles()
        {
            int[] Keys = GuardianProj.Keys.ToArray();
            foreach (int k in Keys)
            {
                if (!Main.projectile[k].active)
                    GuardianProj.Remove(k);
            }
        }

        public override void SetDefaults(Projectile projectile)
        {
            SpawnClearOwnership = true;
            MyParent = ProjParent;
            if (projectile.whoAmI > -1 && GuardianProj.ContainsKey(projectile.whoAmI))
                GuardianProj.Remove(projectile.whoAmI);
            switch (projectile.type)
            {
                case Terraria.ID.ProjectileID.BoneJavelin:
                case Terraria.ID.ProjectileID.JavelinFriendly:
                    projectile.melee = false;
                    projectile.thrown = true;
                    break;
                case Terraria.ID.ProjectileID.CoinPortal:
                    if (!Main.gameMenu)
                    {
                        GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.CoinPortalSpawned,
                            Main.player[Main.myPlayer].name, "", 12, 10,
                            GuardianGlobalInfos.GetGuardiansInTheWorld());
                    }
                    break;
            }
        }

        public bool IsHook(Projectile proj)
        {
            return proj.aiStyle == 7;
        }
        
        public override bool PreAI(Projectile projectile)
        {
            if (SpawnClearOwnership)
            {
                if (IsGuardianProjectile(projectile.whoAmI))
                {
                    GuardianProj.Remove(projectile.whoAmI);
                }
                SpawnClearOwnership = false;
            }
            BackupUsed = false;
            if (MyParent > -1 && GuardianProj.ContainsKey(MyParent))
            {
                if (GuardianProj.ContainsKey(projectile.whoAmI))
                {
                    GuardianProj[projectile.whoAmI] = GuardianProj[MyParent];
                }
                else
                {
                    GuardianProj.Add(projectile.whoAmI, GuardianProj[MyParent]);
                }
                MyParent = -1;
            }
            ProjParent = projectile.whoAmI;
            if (IsGuardianProjectile(projectile.whoAmI))
            {
                if (IsHook(projectile) || (projectile.minion && projectile.minionSlots < 0))
                {
                    if (GuardianProj[projectile.whoAmI].KnockedOut)
                    {
                        projectile.Kill();
                        return false;
                    }
                }
                TerraGuardian g = GuardianProj[projectile.whoAmI];
                if(projectile.position.X < 5 * 16 || projectile.position.X > (Main.maxTilesX - 5) * 16 ||
                    projectile.position.Y < 5 * 16 || projectile.position.Y > (Main.maxTilesY - 5) * 16)
                {
                    projectile.Kill();
                    GuardianProj.Remove(projectile.whoAmI);
                    return false;
                }
                if (projectile.minion)
                {
                    Main.player[projectile.owner].slotsMinions += projectile.minionSlots; //Was -projectile.minionSlots
                    if (!g.Active || (g.MinionSlotCount + projectile.minionSlots > g.MaxMinions && projectile.owner == Main.myPlayer))
                    {
                        if (projectile.type == 627 || projectile.type == 628)
                        {
                            int byUuid = Projectile.GetByUUID(projectile.owner, projectile.ai[0]);
                            if (byUuid > -1)
                            {
                                Projectile proj = Main.projectile[byUuid];
                                if (proj.type != 625)
                                {
                                    proj.localAI[1] = projectile.localAI[1];
                                }
                                proj = Main.projectile[(int)projectile.localAI[1]];
                                proj.ai[0] = projectile.ai[0];
                                proj.ai[1] = 1;
                                proj.netUpdate = true;
                            }
                        }
                        projectile.Kill();
                    }
                    else
                    {
                        projectile.minionPos = g.NumMinions;
                        g.NumMinions++;
                        g.MinionSlotCount += -projectile.minionSlots;
                    }
                }
                if (projectile.sentry)
                {
                    Main.player[projectile.owner].maxTurrets--;
                    if(!g.Active || (g.NumSentries + 1 > g.MaxSentries && projectile.owner == Main.myPlayer))
                    {
                        projectile.Kill();
                    }
                    else
                    {
                        g.NumSentries++;
                    }
                }
                if (projectile.aiStyle == 62)
                {
                    HornetSummonAI(projectile);
                    return false;
                }
                if (projectile.aiStyle == 26)
                {
                    GroundPetsAndBabySlimeAI(projectile);
                    return false;
                }
                if(projectile.aiStyle == 52)
                {
                    LifeDrainBlobAI(projectile);
                    return false;
                }
                if (projectile.aiStyle == 75)
                {
                    PhantasmAI(projectile);
                    return false;
                }
                if (projectile.aiStyle == 100)
                {
                    MedusaRayAI(projectile);
                    return false;
                }
                backup = new PlayerDataBackup(Main.player[projectile.owner], g);
                BackupUsed = true;
            }
            else
            {
                if (IsHook(projectile) || (projectile.minion && projectile.minionSlots > 0))
                {
                    if (Main.player[projectile.owner].GetModPlayer<PlayerMod>().KnockedOut)
                    {
                        projectile.Kill();
                        return false;
                    }
                }
                if (Main.player[projectile.owner].GetModPlayer<PlayerMod>().Guardian.Active)
                {
                    TerraGuardian g = Main.player[projectile.owner].GetModPlayer<PlayerMod>().Guardian;
                    //Main.player[projectile.owner].numMinions += g.MaxMinions;
                    //Main.player[projectile.owner].maxMinions += g.MaxMinions;
                }
            }
            return base.PreAI(projectile);
        }

        public override GlobalProjectile NewInstance(Projectile projectile)
        {
            MyParent = ProjParent;
            return base.NewInstance(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            ProjParent = -1;
            if (MyParent > -1 && IsGuardianProjectile(ProjParent))
            {
                GuardianProj[ProjParent].SetProjectileOwnership(projectile.whoAmI);
                if (Main.projectile[ProjParent].minion)
                    projectile.minion = true;
                MyParent = -1;
            };
            TryRestoringPlayerStatus();
            if (projectile.damage > 0)
            {
                foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                {
                    if (tg.Active && !tg.Downed)
                    {
                        bool ProjIsHostile = projectile.hostile;
                        bool PvPHostile = false;
                        if (IsGuardianProjectile(projectile.whoAmI) && tg.WhoAmID != GuardianProj[projectile.whoAmI].WhoAmID)
                        {
                            ProjIsHostile = GuardianProj[projectile.whoAmI].IsGuardianHostile(tg);
                            PvPHostile = true;
                        }
                        else if(!ProjIsHostile && tg.IsPlayerHostile(Main.player[projectile.owner]))
                        {
                            ProjIsHostile = true;
                            PvPHostile = true;
                        }
                        if (ProjIsHostile && projectile.getRect().Intersects(tg.HitBox))
                        {
                            if (tg.Active && !tg.Downed && projectile.getRect().Intersects(tg.HitBox))
                            {
                                if (tg.Base.GuardianWhenAttackedProjectile(tg, projectile.damage, false, projectile))
                                {
                                    int DamageDealt = tg.Hurt(projectile.damage, projectile.Center.X < tg.Position.X ? 1 : -1, false, false, " was slain by a " + projectile.Name + ".", PvPHostile);
                                    if (DamageDealt > 0)
                                    {
                                        TrySimulatingProjectileDamageOnGuardian(projectile, tg);
                                        if (projectile.penetrate > 0)
                                        {
                                            projectile.penetrate--;
                                            if (projectile.penetrate == 0)
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (IsGuardianProjectile(projectile.whoAmI))
                {
                    projectile.hostile = GuardianProj[projectile.whoAmI].IsPlayerHostile(Main.player[Main.myPlayer]);
                }
            }
            if (!projectile.hostile || projectile.damage == 0)
                return;
            /*for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    PlayerMod player = Main.player[p].GetModPlayer<PlayerMod>();
                    foreach (TerraGuardian guardian in player.GetAllGuardianFollowers)
                    {
                        if (guardian.Active && !guardian.Downed && projectile.getRect().Intersects(guardian.HitBox))
                        {
                            if (guardian.Base.GuardianWhenAttackedProjectile(guardian, projectile.damage, false, projectile))
                            {
                                int DamageDealt = guardian.Hurt(projectile.damage, projectile.Center.X < guardian.Position.X ? 1 : -1, false, false, " was slain by a " + projectile.Name + ".");
                                if (DamageDealt > 0)
                                {
                                    TrySimulatingProjectileDamageOnGuardian(projectile, guardian);
                                    if (projectile.penetrate > 0)
                                    {
                                        projectile.penetrate--;
                                        if (projectile.penetrate == 0)
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
            if (projectile.active)
            {
                if (projectile.aiStyle == 111)
                {
                    DryadBuffScript(projectile);
                }
            }
        }

        private void DryadBuffScript(Projectile proj)
        {
            float num1 = 300f;
            if ((double)proj.ai[0] >= 100.0)
                num1 = Microsoft.Xna.Framework.MathHelper.Lerp(300f, 600f, (float)(((double)proj.ai[0] - 100.0) / 200.0));
            if ((double)num1 > 600.0)
                num1 = 600f;
            if ((double)proj.ai[0] >= 500.0)
            {
                num1 = Microsoft.Xna.Framework.MathHelper.Lerp(600f, 1200f, (float)(((double)proj.ai[0] - 500.0) / 100.0));
            }
            foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                //Check if is hostile, if isn't, gives 165, if is, gives 186
                if (tg.Distance(proj.Center) <= num1)
                {
                    bool Hostile = false;
                    if (!Hostile)
                    {
                        tg.AddBuff(165, 120);
                    }
                    else if((!tg.HasBuff(186) || tg.GetBuff(186).Time <= 20) && Collision.CanHit(proj.Center, 1, 1, tg.TopLeftPosition, tg.Width, tg.Height))
                    {
                        tg.AddBuff(186, 120);
                    }
                }
            }
        }

        public void TryRestoringPlayerStatus()
        {
            if (BackupUsed)
            {
                backup.RestorePlayerStatus();
                BackupUsed = false;
            }
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (IsGuardianProjectile(projectile.whoAmI))
            {
                TerraGuardian g = GuardianProj[projectile.whoAmI];
                if (damage > 0 && target.lifeMax > 5 && projectile.friendly && !projectile.hostile && projectile.aiStyle != 59)
                {
                    if (target.canGhostHeal)
                    {
                        if (g.HasFlag(GuardianFlags.SpectreHealSetEffect) && !g.HasFlag(GuardianFlags.MoonLeech))
                        {
                            projectile.ghostHeal(damage, target.Center);
                        }
                        if (g.HasFlag(GuardianFlags.SpectreSplashSetEffect))
                        {
                            projectile.ghostHurt(damage, target.Center);
                        }
                        if (g.HasFlag(GuardianFlags.NebulaSetEffect) && !g.HasCooldown(GuardianCooldownManager.CooldownType.NebulaCD) && Main.rand.Next(3) == 0)
                        {
                            g.AddCooldown(GuardianCooldownManager.CooldownType.NebulaCD, 30);
                            int Type = Utils.SelectRandom<int>(Main.rand, new int[3] { 3453, 3454, 3455 });
                            int number = Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, Type, 1, false, 0, false, false);
                            Main.item[number].velocity.Y = (float)Main.rand.Next(-20, 1) * 0.2f;
                            Main.item[number].velocity.X = (float)Main.rand.Next(10, 31) * 0.2f * (float)projectile.direction;
                        }
                    }
                }
            }
        }

        public static void TrySimulatingProjectileDamageOnGuardian(Projectile p, TerraGuardian g)
        {
            Player dummy = Main.player[255];
            try
            {
                if (p.owner > -1 && Main.player[p.owner].hostile)
                {
                    p.StatusPvP(255);
                }
                else
                {
                    p.StatusPlayer(255);
                }
            }
            catch
            {

            }
            for (int b = 0; b < dummy.buffType.Length; b++)
            {
                if (dummy.buffType[b] > 0)
                {
                    g.AddBuff(dummy.buffType[b], dummy.buffTime[b], true);
                }
                dummy.buffType[b] = 0;
                dummy.buffTime[b] = 0;
            }
        }

        public static bool IsGuardianProjectile(int ProjPos)
        {
            return GuardianProj.ContainsKey(ProjPos);
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (GuardianProj.ContainsKey(projectile.whoAmI))
                GuardianProj.Remove(projectile.whoAmI);
            TryRestoringPlayerStatus();
            if (projectile.aiStyle == 52 && projectile.owner == Main.myPlayer && !IsGuardianProjectile(projectile.whoAmI) && !Main.player[projectile.owner].moonLeech)
            {
                int HealthValue = (int)projectile.ai[1];
                Main.player[projectile.owner].GetModPlayer<PlayerMod>().ShareHealthReplenishWithGuardians(HealthValue);
            }
            if (Main.netMode < 2 && projectile.type == 676)
            {
                foreach (TerraGuardian g in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (g.Active && !g.Downed && (g.CenterPosition - projectile.Center).Length() < 300)
                    {
                        g.AddBuff(197, 900);
                    }
                }
            }
        }

        public override bool PreDraw(Projectile projectile, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color lightColor)
        {
            drawBackupUsed = false;
            if (IsGuardianProjectile(projectile.whoAmI))
            {
                TerraGuardian tg = GuardianProj[projectile.whoAmI];
                drawBackup = new PlayerDataBackup(Main.player[projectile.owner], tg);
                drawBackupUsed = true;
            }
            return true;
        }

        public override void PostDraw(Projectile projectile, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Microsoft.Xna.Framework.Color lightColor)
        {
            if (drawBackupUsed)
            {
                drawBackup.RestorePlayerStatus();
                drawBackupUsed = false;
            }
        }

        public static int GetProjectileChain(int Type)
        {
            switch (Type)
            {
                case 32:
                    return 5;
                case 73:
                    return 8;
                case 74:
                    return 9;
                case 171:
                    return 16;
                case 186:
                    return 17;
                case 475:
                    return 38;
                case 505:
                    return 4;
                case 506:
                    return 6;
                case 165:
                    return 15;
                case 230:
                    return 1;
                case 231:
                    return 2;
                case 232:
                    return 3;
                case 233:
                    return 4;
                case 234:
                    return 5;
                case 235:
                    return 6;
                case 256:
                    return 20;
                case 322:
                    return 29;
                case 315:
                    return 28;
                case 331:
                    return 30;
                case 332:
                    return 31;
                case 372:
                    return 33;
                case 383:
                    return 34;
                case 396:
                    return 35;
                case 403:
                    return 36;
                case 404:
                    return 37;
                case 486:
                    return 0;
                case 487:
                    return 1;
                case 488:
                    return 2;
                case 489:
                    return 3;
                case 646:
                    return 8;
                case 647:
                    return 9;
                case 648:
                    return 10;
                case 649:
                    return 11;
                case 652:
                    return 16;
                case 262:
                    return 22;
                case 273:
                    return 23;
                case 481:
                    return 40;
                case 271:
                    return 18;
            }
            return 0;
        }
    }
}
