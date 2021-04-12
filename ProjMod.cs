using System;
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
            MyParent = ProjParent;
            switch (projectile.type)
            {
                case Terraria.ID.ProjectileID.BoneJavelin:
                case Terraria.ID.ProjectileID.JavelinFriendly:
                    projectile.melee = false;
                    projectile.thrown = true;
                    break;
            }
        }

        public bool IsHook(Projectile proj)
        {
            return proj.aiStyle == 7;
        }
        
        public override bool PreAI(Projectile projectile)
        {
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
            if (IsHook(projectile))
            {
                if (IsGuardianProjectile(projectile.whoAmI))
                {
                    if (GuardianProj[projectile.whoAmI].KnockedOut)
                    {
                        projectile.Kill();
                        return false;
                    }
                }
                else
                {
                    if (Main.player[projectile.owner].GetModPlayer<PlayerMod>().KnockedOut)
                    {
                        projectile.Kill();
                        return false;
                    }
                }
            }
            ProjParent = projectile.whoAmI;
            if (IsGuardianProjectile(projectile.whoAmI))
            {
                TerraGuardian g = GuardianProj[projectile.whoAmI];
                if (projectile.minion)
                {
                    Main.player[projectile.owner].slotsMinions += -projectile.minionSlots;
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
            if (ProjParent > -1 && IsGuardianProjectile(ProjParent))
            {
                GuardianProj[ProjParent].SetProjectileOwnership(projectile.whoAmI);
                if (Main.projectile[ProjParent].minion)
                    projectile.minion = true;
            }
            return base.NewInstance(projectile);
        }
                
        public override void PostAI(Projectile projectile)
        {
            ProjParent = -1;
            TryRestoringPlayerStatus(projectile);
            if (!projectile.hostile || projectile.damage == 0)
                return;
            for (int p = 0; p < 255; p++)
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
                                int DamageDealt = guardian.Hurt(projectile.damage, projectile.Center.X < guardian.CenterPosition.X ? 1 : -1, false, false, " was slain by a " + projectile.Name + ".");
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
            }
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

        public void TryRestoringPlayerStatus(Projectile projectile)
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
                    g.AddBuff(dummy.buffType[b], dummy.buffTime[b]);
                }
                dummy.buffType[b] = 0;
                dummy.buffTime[b] = 0;
            }
        }

        public override bool PreKill(Projectile projectile, int timeLeft)
        {
            return base.PreKill(projectile, timeLeft);
        }

        public static bool IsGuardianProjectile(int ProjPos)
        {
            return GuardianProj.ContainsKey(ProjPos);
        }

        public override void Kill(Projectile projectile, int timeLeft)
        {
            if (GuardianProj.ContainsKey(projectile.whoAmI))
                GuardianProj.Remove(projectile.whoAmI);
            TryRestoringPlayerStatus(projectile);
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
    }
}
