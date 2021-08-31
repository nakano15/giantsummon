using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianBountyQuest
    {
        public static bool SardineTalkedToAboutBountyQuests = false;
        public static bool SpawningBountyMob = false;
        public static int SignID = -1;
        public static bool IsAnnouncementBox = false;
        public static int SpawnStress = 0;
        public static int TargetMonsterID = 0, TargetMonsterSpawnPosition = -1;
        public static string TargetName = "", Suffix = "";
        public static string TargetFullName { get { return TargetName + " the " + Suffix; } }
        public static SpawnBiome spawnBiome = SpawnBiome.Corruption;
        public static List<Modifiers> modifier = new List<Modifiers>();
        public static DangerousModifier dangerousModifier = DangerousModifier.None;
        public static int ActionCooldown = 5;
        public const int NewRequestMinTime = 10 * 3600, NewRequestMaxTime = 20 * 3600,
            RequestEndMinTime = 20 * 3600, RequestEndMaxTime = 35 * 3600;
        public static int CoinReward = 0;
        public static Item[] RewardList = new Item[0];
        public const int SardineID = 2;
        public const string NoRequestText = "No bounty right now.";
        private static Dictionary<string, byte> BountyProgress = new Dictionary<string, byte>();
        public const byte BountyKilled = 1, BountyRewardRedeemed = 2;
        public static int[] BountyCounters = new int[7];
        public const int HealthRegenCounter = 0, SpecialSkillCounter = 1, LeaderSpawnFlagCounter = 2, FireRainCounter = 3, SappingCounter = 4, OsmoseCounter = 5, ImobilizeCounter = 6;

        public static void Save(Terraria.ModLoader.IO.TagCompound writer)
        {
            writer.Add("BQSardineTalkedTo", SardineTalkedToAboutBountyQuests);
            writer.Add("BQIsAnnouncementBox", IsAnnouncementBox);
            writer.Add("BQTargetMonsterID", TargetMonsterID);
            if (TargetMonsterID > 0)
            {
                writer.Add("BQSpawnStress", SpawnStress);
                writer.Add("BQTargetName", TargetName);
                writer.Add("BQSuffix", Suffix);
                writer.Add("BQSpawnBiome", (byte)spawnBiome);
                int Count = modifier.Count;
                writer.Add("BQModifier_Count", Count);
                for (int m = 0; m < Count; m++)
                {
                    writer.Add("BQModifier_" + m, (byte)modifier[m]);
                }
                writer.Add("BQDangerousModifier", (byte)dangerousModifier);
                writer.Add("BQCoinReward", CoinReward);
                writer.Add("BQRewardList", RewardList.Length);
                int c = 0;
                foreach (Item i in RewardList)
                {
                    writer.Add("BQReward_" + c, i);
                    c++;
                }
                writer.Add("BQBountyProgressEntries",BountyProgress.Count);
                c = 0;
                foreach (string key in BountyProgress.Keys)
                {
                    writer.Add("BQBountyPlayerName_" + c, key);
                    writer.Add("BQBountyPlayerProgress_" + c, BountyProgress[key]);
                    c++;
                }
            }
            writer.Add("BQActionCooldown", ActionCooldown);
        }

        public static void Load(Terraria.ModLoader.IO.TagCompound reader, int Version)
        {
            SardineTalkedToAboutBountyQuests = reader.GetBool("BQSardineTalkedTo");
            IsAnnouncementBox = reader.GetBool("BQIsAnnouncementBox");
            TargetMonsterID = reader.GetInt("BQTargetMonsterID");
            if (Version < 88)
            {
                TargetMonsterID = 0;
                SetDefaultCooldown();
                return;
            }
            if (TargetMonsterID > 0)
            {
                SpawnStress = reader.GetInt("BQSpawnStress");
                TargetName = reader.GetString("BQTargetName");
                Suffix = reader.GetString("BQSuffix");
                spawnBiome = (SpawnBiome)reader.GetByte("BQSpawnBiome");
                if (Version < 44)
                {
                    TargetMonsterID = 0;
                }
                else
                {
                    int Count = reader.GetInt("BQModifier_Count");
                    for (int m = 0; m < Count; m++)
                    {
                        modifier.Add((Modifiers)reader.GetByte("BQModifier_" + m));
                    }
                }
                dangerousModifier = (DangerousModifier)reader.GetByte("BQDangerousModifier");
                CoinReward = reader.GetInt("BQCoinReward");
                int ItemCount = reader.GetInt("BQRewardList");
                RewardList = new Item[ItemCount];
                for (int i = 0; i < ItemCount; i++ )
                {
                    RewardList[i] = reader.Get<Item>("BQReward_" + i);
                }
                if (MainMod.ModVersion >= 37)
                {
                    ItemCount = reader.GetInt("BQBountyProgressEntries");
                    for (int i = 0; i < ItemCount; i++)
                    {
                        string key = reader.GetString("BQBountyPlayerName_" + i);
                        byte Progress = reader.GetByte("BQBountyPlayerProgress_" + i);
                        BountyProgress.Add(key, Progress);
                    }
                }
            }
            ActionCooldown = reader.GetInt("BQActionCooldown");
        }

        public static void Reset()
        {
            SardineTalkedToAboutBountyQuests = false;
            SignID = -1;
            IsAnnouncementBox = false;
            TargetMonsterID = 0;
            TargetMonsterSpawnPosition = -1;
            BountyProgress.Clear();
            SetDefaultCooldown();
        }

        public static void ApplyModifier(NPC npc)
        {
            if (SpawningBountyMob)
            {
                for(int i = 0; i < BountyCounters.Length; i++)
                {
                    BountyCounters[i] = 0;
                }
            }
            npc.lifeMax *= 10;
            npc.damage += 20;
            npc.defense += 10;
            npc.knockBackResist *= 0.3f;
            int DifficultyMod = 0;
            foreach (Modifiers mod in modifier)
            {
                switch (mod)
                {
                    case Modifiers.Boss:
                        DifficultyMod++;
                        break;
                    case Modifiers.ExtraHealth:
                        npc.lifeMax += (int)(npc.lifeMax * 0.4f);
                        break;
                    case Modifiers.ExtraDamage:
                        npc.damage += (int)(npc.damage * 0.3f);
                        break;
                    case Modifiers.ExtraDefense:
                        npc.defense += (int)(npc.defense * 0.3f);
                        break;
                    case Modifiers.Armored:
                        npc.knockBackResist = 0;
                        break;
                    case Modifiers.Noob:
                        npc.knockBackResist *= 2;
                        npc.lifeMax = (int)(npc.lifeMax * 0.05f);
                        npc.damage -= (int)(npc.damage * 0.7f);
                        npc.defense -= (int)(npc.defense * 0.5f);
                        break;
                }
            }
            npc.GetGlobalNPC<NpcMod>().mobType = NpcMod.GetBossDifficulty(npc, DifficultyMod);
            npc.rarity = 10;
        }
        
        public static void OnBountyMonsterHitPlayer(Player Target)
        {
            if (modifier.Contains(Modifiers.Petrifying) && !Target.HasBuff(Terraria.ID.BuffID.Stoned) && Main.rand.NextFloat() < 0.5)
            {
                Target.AddBuff(Terraria.ID.BuffID.Stoned, 5 * 30);
            }
            if (modifier.Contains(Modifiers.Fatal))
            {
                Target.AddBuff(Terraria.ID.BuffID.BrokenArmor, 5 * 30);
            }
        }

        public static void OnBountyMonsterHitTerraGuardian(TerraGuardian Target)
        {
            if (modifier.Contains(Modifiers.Petrifying) && !Target.HasBuff(Terraria.ID.BuffID.Stoned) && Main.rand.NextFloat() < 0.5)
            {
                Target.AddBuff(Terraria.ID.BuffID.Stoned, 5 * 30);
            }
            if (modifier.Contains(Modifiers.Fatal))
            {
                Target.AddBuff(Terraria.ID.BuffID.BrokenArmor, 5 * 30);
            }
        }

        public static void ModifyBountyMonsterHitByPlayerAttack(Player player, byte DamageType, ref int damage, ref float knockback, ref bool crit)
        {
            switch (DamageType)
            {
                case 0:
                    if (modifier.Contains(Modifiers.MeleeDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
                case 1:
                    if (modifier.Contains(Modifiers.RangedDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
                case 2:
                    if (modifier.Contains(Modifiers.MagicDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
            }
            if (Main.rand.NextFloat() < 0.66f)
            {
                if (modifier.Contains(Modifiers.Retaliate))
                {
                    int Damage = (int)(Math.Max(1, damage * 0.1f));
                    player.statLife -= Damage;
                    if (player.statLife < 0)
                        player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" couldn't stand the backfire."), Damage, -player.direction);
                    CombatText.NewText(player.getRect(), CombatText.LifeRegenNegative, Damage, false, true);
                }
                if (DamageType == 2 && modifier.Contains(Modifiers.ManaBurn))
                {
                    int Damage = (int)(Math.Max(1, damage * 0.1f));
                    player.statMana -= Damage;
                    if (player.statMana < 0)
                        player.statMana = 0;
                    CombatText.NewText(player.getRect(), CombatText.HealMana * 0.5f, Damage, false, true);
                }
            }
        }

        public static void ModifyBountyMonsterHitByGuardianAttack(TerraGuardian guardian, byte DamageType, ref int damage, ref float knockback, ref bool crit)
        {
            switch (DamageType)
            {
                case 0:
                    if (modifier.Contains(Modifiers.MeleeDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
                case 1:
                    if (modifier.Contains(Modifiers.RangedDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
                case 2:
                    if (modifier.Contains(Modifiers.MagicDefense))
                    {
                        damage = (int)(damage * 0.5f);
                    }
                    break;
            }
            if (Main.rand.NextFloat() < 0.66f)
            {
                if (modifier.Contains(Modifiers.Retaliate))
                {
                    int Damage = (int)(Math.Max(1, damage * 0.1f));
                    guardian.HP -= Damage;
                    if (guardian.HP < 0)
                        guardian.Knockout(" couldn't stand the backfire.");
                    CombatText.NewText(guardian.HitBox, CombatText.LifeRegenNegative, Damage, false, true);
                }
                if (DamageType == 2 && modifier.Contains(Modifiers.ManaBurn))
                {
                    int Damage = (int)(Math.Max(1, damage * 0.1f));
                    guardian.MP -= Damage;
                    if (guardian.MP < 0)
                        guardian.MP = 0;
                    CombatText.NewText(guardian.HitBox, CombatText.HealMana * 0.5f, Damage, false, true);
                }
            }
        }

        public static void UpdateBountyNPC(NPC npc)
        {
            if (Main.rand.Next(3) == 0)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 219);
            }
            if (modifier.Contains(Modifiers.HealthRegen))
            {
                if (BountyCounters[HealthRegenCounter] >= 30)
                {
                    BountyCounters[HealthRegenCounter] -= 30;
                    npc.life += npc.defense;
                    if (npc.life > npc.lifeMax)
                        npc.life = npc.lifeMax;
                }
            }
            if (modifier.Contains(Modifiers.Leader))
            {
                if (BountyCounters[LeaderSpawnFlagCounter] == 0)
                {
                    for (int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active && Main.player[p].Distance(npc.Center) < 300)
                        {
                            BountyCounters[LeaderSpawnFlagCounter] = 1;
                            int MobsToSpawn = Main.rand.Next(2, 5);
                            for (int i = 0; i < MobsToSpawn; i++)
                            {
                                int npcpos = NPC.NewNPC((int)npc.Center.X, (int)npc.Bottom.Y, npc.type);
                                Vector2 Velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                                Velocity.Normalize();
                                Main.npc[npcpos].velocity = Velocity * 3;
                            }
                            break;
                        }
                    }
                }
            }
            if (modifier.Contains(Modifiers.FireRain))
            {
                if (BountyCounters[FireRainCounter] >= Main.rand.Next(20, 30))
                {
                    BountyCounters[FireRainCounter] = 0;
                    Vector2 RainSpawnPosition = npc.Center;
                    if (npc.position.Y < Main.worldSurface * 16)
                    {
                        RainSpawnPosition.Y -= 1000;
                        RainSpawnPosition.X += Main.rand.Next(-1000, 1001);
                    }
                    int Damage = (int)(npc.damage * 0.65f);
                    Vector2 Velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                    Velocity.Normalize();
                    Projectile.NewProjectile(RainSpawnPosition, Velocity * 15, Main.rand.Next(326, 329), Damage, 0.1f);
                }
            }
            if (modifier.Contains(Modifiers.Imobilizer))
            {
                if (BountyCounters[ImobilizeCounter] >= 10 * 60)
                {
                    BountyCounters[ImobilizeCounter] = 0;
                    Vector2 RainSpawnPosition = npc.Center;
                    int Damage = (int)(npc.damage * 0.85f);
                    Vector2 Velocity = Main.player[npc.target].Center - npc.Center;
                    Velocity.Normalize();
                    Projectile.NewProjectile(RainSpawnPosition, Velocity * 9, Terraria.ID.ProjectileID.WebSpit, Damage, 0.1f);
                }
            }
            if (modifier.Contains(Modifiers.Sapping))
            {
                bool DamageHealth = BountyCounters[SappingCounter] >= 30;
                if (DamageHealth)
                    BountyCounters[SappingCounter] = 0;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && Main.player[p].Distance(npc.Center) < 240)
                    {
                        Main.player[p].lifeRegen = 0;
                        if (DamageHealth)
                        {
                            int HealthDamage = (int)(Main.player[p].statLifeMax2 / Main.player[p].statLifeMax);
                            if (HealthDamage < 1)
                                HealthDamage = 1;
                            Main.player[p].statLife -= HealthDamage;
                            if (Main.player[p].statLife <= 0)
                                Main.player[p].KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" didn't had anymore spare blood."), 1, 0);
                            CombatText.NewText(Main.player[p].getRect(), CombatText.LifeRegenNegative, HealthDamage, false, true);
                        }
                    }
                }
                foreach(TerraGuardian tg in MainMod.ActiveGuardians.Values)
                {
                    if(!tg.Downed && tg.Distance(npc.Center) < 240)
                    {
                        tg.HealthRegenTime = 0;
                        if (DamageHealth)
                        {
                            int Damage = (int)(tg.HealthHealMult);
                            if (Damage < 1)
                                Damage = 1;
                            tg.HP -= Damage;
                            if (tg.HP <= 0)
                                tg.Knockout(" didn't had anymore spare blood.");
                            CombatText.NewText(tg.HitBox, CombatText.LifeRegenNegative, Damage, false, true);
                        }
                    }
                }
            }
            if (modifier.Contains(Modifiers.Osmose))
            {
                bool DamageMana = BountyCounters[SappingCounter] >= 30;
                if (DamageMana)
                    BountyCounters[SappingCounter] = 0;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && !Main.player[p].dead && Main.player[p].Distance(npc.Center) < 240)
                    {
                        Main.player[p].manaRegen = 0;
                        if (Main.player[p].statMana <= 0)
                            Main.player[p].lifeRegen = 0;
                        if (DamageMana)
                        {
                            if (Main.player[p].statMana > 0)
                            {
                                int ManaDamage = (int)(Main.player[p].statManaMax2 / Main.player[p].statManaMax);
                                if (ManaDamage < 1)
                                    ManaDamage = 1;
                                Main.player[p].statMana -= ManaDamage;
                            }
                            else
                            {
                                int HealthDamage = (int)(Main.player[p].statLifeMax2 / Main.player[p].statLifeMax);
                                if (HealthDamage < 1)
                                    HealthDamage = 1;
                                Main.player[p].statLife -= HealthDamage;
                                if (Main.player[p].statLife <= 0)
                                    Main.player[p].KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" was dried out."), 1, 0);
                                CombatText.NewText(Main.player[p].getRect(), CombatText.LifeRegenNegative, HealthDamage, false, true);
                            }
                        }
                    }
                }
                foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                {
                    if (!tg.Downed && tg.Distance(npc.Center) < 240)
                    {
                        tg.ManaRegenTime = 0;
                        if (tg.MP <= 0)
                            tg.HealthRegenTime = 0;
                        if (DamageMana)
                        {
                            if (tg.MP > 0)
                            {
                                int Damage = (int)tg.ManaHealMult;
                                if (Damage < 1)
                                    Damage = 1;
                                tg.MP -= Damage;
                                if (tg.MP < 0)
                                    tg.MP = 0;
                            }
                            else
                            {
                                int Damage = (int)(tg.HealthHealMult);
                                if (Damage < 1)
                                    Damage = 1;
                                tg.HP -= Damage;
                                if (tg.HP <= 0)
                                    tg.Knockout(" was dried out.");
                                CombatText.NewText(tg.HitBox, CombatText.LifeRegenNegative, Damage, false, true);
                            }
                        }
                    }
                }
            }
            switch (dangerousModifier)
            {
                case DangerousModifier.Invoker:
                    {
                        if (BountyCounters[SpecialSkillCounter] == 0)
                        {
                            bool HasBossSpawned = false;
                            for (int n = 0; n < 200; n++)
                            {
                                if (Main.npc[n].active && NPCID.Sets.TechnicallyABoss[Main.npc[n].type])
                                {
                                    HasBossSpawned = true;
                                    break;
                                }
                            }
                            if (!HasBossSpawned)
                            {
                                for (int p = 0; p < 255; p++)
                                {
                                    if (Main.player[p].active && Main.player[p].Distance(npc.Center) < 300)
                                    {
                                        //Spawn a boss
                                        int BossToSpawn = NPCID.KingSlime;
                                        if (!Main.dayTime)
                                        {
                                            BossToSpawn = NPCID.EyeofCthulhu;
                                        }
                                        if (Main.player[p].ZoneJungle)
                                        {
                                            BossToSpawn = NPCID.QueenBee;
                                        }
                                        if (Main.player[p].ZoneCorrupt)
                                        {
                                            BossToSpawn = NPCID.EaterofWorldsHead;
                                        }
                                        else if (Main.player[p].ZoneCrimson)
                                        {
                                            BossToSpawn = NPCID.BrainofCthulhu;
                                        }
                                        NPC.SpawnOnPlayer(p, BossToSpawn);
                                        BountyCounters[SpecialSkillCounter] = 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case DangerousModifier.Reaper:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 180)
                        {
                            bool Trigger = BountyCounters[SpecialSkillCounter] % 20 == 0;
                            if (BountyCounters[SpecialSkillCounter] >= 300)
                                BountyCounters[SpecialSkillCounter] -= 300;
                            if (Trigger && npc.target > -1)
                            {
                                Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                                Dir.Normalize();
                                Projectile.NewProjectile(npc.Center, Dir * 8f, 44, (int)(npc.defDamage * 1.2f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Cyclops:
                    {
                        float DelayTime = 300 - 180 * (1f - (float)npc.life / npc.lifeMax);
                        if (BountyCounters[SpecialSkillCounter] >= DelayTime)
                        {
                            BountyCounters[SpecialSkillCounter] -= (int)DelayTime;
                            if (npc.target > -1)
                            {
                                Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                                Dir.Normalize();
                                Vector2 SpawnPosition = npc.position;
                                SpawnPosition.X += npc.width * 0.5f;
                                SpawnPosition.Y += npc.height * 0.25f;
                                Projectile.NewProjectile(SpawnPosition, Dir * 10f, 259, (int)(npc.defDamage * 0.8f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.GoldenShower:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 300)
                        {
                            bool Trigger = BountyCounters[SpecialSkillCounter] % 5 == 0;
                            if (BountyCounters[SpecialSkillCounter] >= 360)
                                BountyCounters[SpecialSkillCounter] -= 360;
                            if (Trigger && npc.target > -1)
                            {
                                Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                                Dir.Normalize();
                                Vector2 ShotPosition = npc.position;
                                ShotPosition.X += npc.width * 0.5f;
                                ShotPosition.Y += npc.height * 0.6f;
                                Projectile.NewProjectile(ShotPosition, Dir * 8f, 288, (int)(npc.defDamage * 0.75f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Haunted:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 300)
                        {
                            BountyCounters[SpecialSkillCounter] -= (int)300;
                            if (npc.target > -1)
                            {
                                Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                                Dir.Normalize();
                                Projectile.NewProjectile(npc.Center, Dir * 10f, 293, (int)(npc.defDamage * 1.25f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Alchemist:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 300)
                        {
                            BountyCounters[SpecialSkillCounter] -= (int)300;
                            for (int x = -2; x <= 2; x++)
                            {
                                Vector2 Dir = new Vector2(x * 3f, -8f);
                                Projectile.NewProjectile(npc.Center, Dir, 328 - Math.Abs(x), (int)(npc.defDamage * 1.25f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Sharknado:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 60 * 30)
                        {
                            BountyCounters[SpecialSkillCounter] -= 60 * 30;
                            if (npc.target > -1)
                            {
                                Vector2 Dir = Main.player[npc.target].Center - npc.Center;
                                Dir.Normalize();
                                Projectile.NewProjectile(npc.Center, Dir * 10f, 385, (int)(npc.defDamage * 1.25f), 3f);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Sapper:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 60 * 15)
                        {
                            BountyCounters[SpecialSkillCounter] -= 60 * 15;
                            if (npc.target > -1)
                            {
                                NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, 387);
                            }
                        }
                    }
                    break;
                case DangerousModifier.Cursed:
                    {
                        if (BountyCounters[SpecialSkillCounter] >= 60)
                        {
                            BountyCounters[SpecialSkillCounter] -= 60;
                            if (npc.target > -1)
                            {
                                Projectile.NewProjectile(npc.Center, Vector2.Zero, 596, (int)(npc.defDamage * 1.25f), 3f);
                            }
                        }
                    }
                    break;
            }
        }

        public static float GetExtraRewardFromModifiers()
        {
            float Extra = 1;
            for (int m = 0; m < Enum.GetValues(typeof(Modifiers)).Length; m++)
            {
                Modifiers mod = (Modifiers)m;
                if (modifier.Contains(mod))
                {
                    switch (mod)
                    {
                        case Modifiers.Boss:
                            Extra *= 1.5f;
                            break;
                        case Modifiers.Noob:
                            Extra *= 0.5f;
                            break;
                        case Modifiers.ExtraHealth:
                            Extra += 0.15f;
                            break;
                        case Modifiers.ExtraDamage:
                            Extra += 0.3f;
                            break;
                        case Modifiers.ExtraDefense:
                            Extra += 0.2f;
                            break;
                        case Modifiers.HealthRegen:
                            Extra += 0.1f;
                            break;
                        case Modifiers.Armored:
                            Extra += 0.4f;
                            break;
                        case Modifiers.Petrifying:
                            Extra += 0.3f;
                            break;
                        case Modifiers.Leader:
                            Extra += 0.45f;
                            break;
                        case Modifiers.FireRain:
                            Extra += 0.25f;
                            break;
                        case Modifiers.Sapping:
                            Extra += 0.4f;
                            break;
                        case Modifiers.Osmose:
                            Extra += 0.2f;
                            break;
                        case Modifiers.Retaliate:
                            Extra += 0.15f;
                            break;
                        case Modifiers.ManaBurn:
                            Extra += 0.25f;
                            break;
                    }
                }
            }
            switch (dangerousModifier)
            {
                case DangerousModifier.Reaper:
                    Extra += 0.03f;
                    break;
                case DangerousModifier.Cyclops:
                    Extra += 0.2f;
                    break;
                case DangerousModifier.GoldenShower:
                    Extra += 0.1f;
                    break;
                case DangerousModifier.Haunted:
                    Extra += 0.25f;
                    break;
                case DangerousModifier.Alchemist:
                    Extra += 0.05f;
                    break;
                case DangerousModifier.Sharknado:
                    Extra += 0.5f;
                    break;
                case DangerousModifier.Sapper:
                    Extra += 0.35f;
                    break;
                case DangerousModifier.Cursed:
                    Extra += 0.08f;
                    break;
                case DangerousModifier.Invoker:
                    Extra += 0.12f;
                    break;
            }
            return Extra;
        }

        private static void PlayerHelpedKillBounty(Player player)
        {
            if (!BountyProgress.ContainsKey(player.name))
            {
                BountyProgress.Add(player.name, BountyKilled);
                if (player.whoAmI == Main.myPlayer)
                {
                    if (PlayerMod.HasGuardianSummoned(player, SardineID))
                    {
                        Main.NewText(PlayerMod.GetPlayerSummonedGuardian(player, SardineID).Name + ": Nice job, let's go back home and check the sign so I can give your reward.");
                    }
                    else
                    {
                        Main.NewText("Bounty hunted successfully. Report back to " + NpcMod.GetGuardianNPCName(SardineID) + ".");
                    }
                }
            }
        }

        public static bool PlayerCanRedeemReward(Player player)
        {
            return BountyProgress.ContainsKey(player.name) && BountyProgress[player.name] == BountyKilled;
        }

        public static bool PlayerAlreadyRedeemedReward(Player player)
        {
            return BountyProgress.ContainsKey(player.name) && BountyProgress[player.name] == BountyRewardRedeemed;
        }

        public static bool PlayerRedeemReward(Player player)
        {
            //Can lose items if inventory is full
            if (PlayerCanRedeemReward(player))
            {
                int c = CoinReward, s = 0, g = 0, p = 0;
                if (c >= 100)
                {
                    s += c / 100;
                    c -= s * 100;
                }
                if (s >= 100)
                {
                    g += s / 100;
                    s -= g * 100;
                }
                if (g >= 100)
                {
                    p += g / 100;
                    g -= p * 100;
                }
                if (c > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.CopperCoin);
                    i.stack = c;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (s > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.SilverCoin);
                    i.stack = s;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (g > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.GoldCoin);
                    i.stack = g;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                if (p > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(ItemID.PlatinumCoin);
                    i.stack = p;
                    GiveRewardToPlayer(i, player);
                    //player.GetItem(player.whoAmI, i, true);
                }
                int ValueStack = CoinReward;
                foreach (Item i in RewardList)
                {
                    Item ni = i.Clone();
                    GiveRewardToPlayer(ni, player);
                    if (ni.value > 1)
                        ValueStack += ni.value * i.stack / 8;
                    else
                        ValueStack += ni.value * i.stack;
                    //player.GetItem(player.whoAmI, ni, true);
                }
                if (Compatibility.NExperienceCompatibility.IsModActive)
                {
                    Compatibility.NExperienceCompatibility.GiveExpRewardToPlayer(player, (Main.hardMode ? 25 : 10) + (float)ValueStack / 10000, 0.15f, true, 5); //NExperience.ExpReceivedPopText.ExpSource.Quest
                }
                BountyProgress[player.name] = BountyRewardRedeemed;
                return true;
            }
            return false;
        }

        public static void GiveRewardToPlayer(Item i, Player player)
        {
            int ItemPos = Item.NewItem(player.Center, i.type);
            if (ItemPos > -1)
            {
                Main.item[ItemPos] = i;
                Main.item[ItemPos].Center = player.Center;
                //player.GetItem(player.whoAmI, Main.item[ItemPos]);
            }
        }

        public static void OnMobKilled(NPC npc)
        {
            if (npc.target > -1 && npc.target < Main.maxPlayers && Main.player[npc.target].active && !Main.player[npc.target].dead && IsPlayerOnBountyBiome(Main.player[npc.target]))
            {
                if (GetBountyState(Main.player[npc.target]) == 0)
                {
                    if (SpawnStress == 10)
                    {
                        Main.NewText("The bounty target seems to have noticed your killing spree.", 255, 100, 0);
                    }
                    if (SpawnStress == 0)
                    {
                        Main.NewText("You hear a furious roar coming from somewhere...", 255, 50, 0);
                    }
                }
                SpawnStress--;
            }
        }

        public static void OnBountyMonsterKilled(NPC npc)
        {
            for (int i = 0; i < 255; i++)
            {
                if (npc.playerInteraction[i] && Main.player[i].active)
                {
                    PlayerHelpedKillBounty(Main.player[i]);
                }
            }
        }

        public static byte GetBountyState(Player player)
        {
            if (!BountyProgress.ContainsKey(player.name))
                return 0;
            return BountyProgress[player.name];
        }

        public static void SpawnBountyMobOnPlayer(Player player)
        {
            TargetMonsterSpawnPosition = -1;
            int[] LastNpcList = new int[200];
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active)
                    LastNpcList[n] = Main.npc[n].type;
                else
                    LastNpcList[n] = 0;

            }
            int SpawnMinX = (int)player.Center.X / 16, SpawnMinY = (int)player.Center.Y / 16, SpawnMaxX = SpawnMinX, SpawnMaxY = SpawnMinY;
            SpawnMinX -= (int)(NPC.sWidth / 16 * 0.7);
            SpawnMaxX += (int)(NPC.sWidth / 16 * 0.7);
            SpawnMinY -= (int)(NPC.sWidth / 16 * 0.52);
            SpawnMaxY += (int)(NPC.sWidth / 16 * 0.52);
            if (SpawnMinX < 0) SpawnMinX = 0;
            if (SpawnMinY < 0) SpawnMinY = 0;
            if (SpawnMaxX >= Main.maxTilesX) SpawnMaxX = Main.maxTilesX;
            if (SpawnMaxY >= Main.maxTilesY) SpawnMaxY = Main.maxTilesY;
            for (int attempt = 0; attempt < 40; attempt++)
            {
                int SpawnX = Main.rand.Next(SpawnMinX, SpawnMaxX + 1), SpawnY = Main.rand.Next(SpawnMinY, SpawnMaxY + 1);
                if ((!Main.tile[SpawnX, SpawnY].active() || !Main.tileSolid[Main.tile[SpawnX, SpawnY].type]) && Main.tile[SpawnX, SpawnY + 1].active() && Main.tileSolid[Main.tile[SpawnX, SpawnY + 1].type])
                {
                    bool MaySpawn = true;
                    switch (spawnBiome)
                    {
                        case SpawnBiome.Dungeon:
                            if (!((Main.tile[SpawnX, SpawnY].wall >= 7 && Main.tile[SpawnX, SpawnY].wall <= 9) || (Main.tile[SpawnX, SpawnY].wall >= 94 && Main.tile[SpawnX, SpawnY].wall <= 99)))
                            {
                                MaySpawn = false;
                            }
                            break;
                        case SpawnBiome.LihzahrdTemple:
                            {
                                MaySpawn = Main.tile[SpawnX, SpawnY].wall == Terraria.ID.WallID.LihzahrdBrickUnsafe;
                            }
                            break;
                        case SpawnBiome.Underworld:
                            MaySpawn = SpawnX >= Main.maxTilesY - 130;
                            break;
                    }
                    if (MaySpawn)
                    {
                        SpawningBountyMob = true;
                        int NpcPos = NPC.NewNPC(SpawnX * 16, SpawnY * 16, TargetMonsterID);
                        SpawningBountyMob = false;
                        if (NpcPos < 200 && NpcPos > -1)
                        {
                            if (MainMod.UseNewMonsterModifiersSystem)
                            {
                                Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType = NpcMod.GetBossDifficulty(Main.npc[NpcPos], 1);
                                //if (Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType < MobTypes.Elite)
                                //    Main.npc[NpcPos].GetGlobalNPC<NpcMod>().mobType = MobTypes.Elite;
                            }
                            TargetMonsterSpawnPosition = NpcPos;
                            Microsoft.Xna.Framework.Vector2 Direction = Main.npc[TargetMonsterSpawnPosition].Center - Main.player[Main.myPlayer].Center;
                            string DirectionText = GetDirectionText(Direction);
                            Main.NewText(TargetFullName + " has appeared to the " + DirectionText + "!");
                            return;
                        }
                    }
                }
            }
        }

        public static string GetDirectionText(Microsoft.Xna.Framework.Vector2 Direction)
        {
            Direction.Normalize();
            bool CountVerticalDiference = Math.Abs(Direction.Y) >= 0.33f, CountHorizontalDiference = Math.Abs(Direction.X) >= 0.33f;
            string DirectionText = "";
            if (CountVerticalDiference && CountHorizontalDiference)
            {
                if (Direction.Y > 0) DirectionText += "South";
                else DirectionText += "North";
                if (Direction.X > 0) DirectionText += "east";
                else DirectionText += "west";
            }
            else if (CountVerticalDiference)
            {
                if (Direction.Y > 0) DirectionText = "South";
                else DirectionText = "North";
            }
            else if (CountHorizontalDiference)
            {
                if (Direction.X > 0) DirectionText = "East";
                else DirectionText = "West";
            }
            return DirectionText;
        }

        public static bool IsPlayerOnBountyBiome(Player player)
        {
            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    return player.ZoneCorrupt;
                case SpawnBiome.Crimson:
                    return player.ZoneCrimson;
                case SpawnBiome.Dungeon:
                    return player.ZoneDungeon;
                case SpawnBiome.Hallow:
                    return player.ZoneHoly;
                case SpawnBiome.Jungle:
                    return player.ZoneJungle;
                case SpawnBiome.Underworld:
                    return player.ZoneUnderworldHeight;
                case SpawnBiome.Ocean:
                    return player.ZoneBeach;
                case SpawnBiome.Underground:
                    return player.ZoneDirtLayerHeight || player.ZoneRockLayerHeight;
                case SpawnBiome.Sky:
                    return player.ZoneSkyHeight;
                case SpawnBiome.OldOneArmy:
                    return Terraria.GameContent.Events.DD2Event.ShouldBlockBuilding(player.position);
                case SpawnBiome.GoblinArmy:
                    return Main.invasionType == InvasionID.GoblinArmy && player.ZoneOverworldHeight;
                case SpawnBiome.Night:
                    return !Main.dayTime && player.ZoneOverworldHeight;
                case SpawnBiome.Snow:
                    return player.ZoneSnow;
                case SpawnBiome.Desert:
                    return player.ZoneDesert;
                case SpawnBiome.PirateArmy:
                    return Main.invasionType == InvasionID.PirateInvasion && player.ZoneOverworldHeight;
                case SpawnBiome.MartianMadness:
                    return Main.invasionType == InvasionID.MartianMadness && player.ZoneOverworldHeight;
                case SpawnBiome.SnowLegion:
                    return Main.invasionType == InvasionID.SnowLegion && player.ZoneOverworldHeight;
                case SpawnBiome.LihzahrdTemple:
                    {
                        Tile tile = MainMod.GetTile((int)(player.Center.X * TerraGuardian.DivisionBy16), (int)(player.Center.Y * TerraGuardian.DivisionBy16));
                        return tile != null && tile.wall == Terraria.ID.WallID.LihzahrdBrickUnsafe;
                    }
            }
            return false;
        }

        public static void TryFindingASign()
        {
            SignID = -1;
            if (!WorldMod.IsGuardianNpcInWorld(SardineID))
            {
                return;
            }
            WorldMod.GuardianTownNpcState townstate = WorldMod.GetTownNpcState(SardineID);
            if (townstate == null || townstate.Homeless)
                return;
            int HPX = townstate.HomeX,
                HPY = townstate.HomeY;
            if(townstate.HouseInfo != null)
            {
                foreach (WorldMod.GuardianBuildingInfo.FurnitureInfo fi in townstate.HouseInfo.furnitures)
                {
                    if (fi.FurnitureID == Terraria.ID.TileID.Signs || fi.FurnitureID == Terraria.ID.TileID.AnnouncementBox)
                    {
                        SignID = Sign.ReadSign(fi.FurnitureX, fi.FurnitureY);
                        if (SignID > -1)
                        {
                            IsAnnouncementBox = fi.FurnitureID == Terraria.ID.TileID.AnnouncementBox;
                            break;
                        }
                    }
                }
            }
            else if (HPX > -1 && HPY > -1)
            {
                int TileCount = 0;
                List<KeyValuePair<int, int>> NextTileCheck = new List<KeyValuePair<int, int>>(),
                    CheckedTiles = new List<KeyValuePair<int, int>>();
                NextTileCheck.Add(new KeyValuePair<int, int>(HPX, HPY));
                while (TileCount < 60 && NextTileCheck.Count > 0)
                {
                    KeyValuePair<int, int> CurrentTile = NextTileCheck[0];
                    NextTileCheck.RemoveAt(0);
                    CheckedTiles.Add(CurrentTile);
                    int TileX = CurrentTile.Key, TileY = CurrentTile.Value;
                    if (TileX >= Main.leftWorld && TileX < Main.rightWorld &&
                        TileY >= Main.topWorld && TileY < Main.bottomWorld)
                    {
                        if (Main.tile[TileX, TileY].active() && Main.tileSign[Main.tile[TileX, TileY].type])
                        {
                            SignID = Sign.ReadSign(TileX, TileY, true);
                            if (SignID > -1)
                            {
                                IsAnnouncementBox = (Main.tile[TileX, TileY].type == Terraria.ID.TileID.AnnouncementBox);
                                NextTileCheck.Clear();
                                CheckedTiles.Clear();
                                break;
                            }
                        }
                        else
                        {
                            int tx = TileX, ty = TileY;
                            ty--;
                            if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                            {
                                KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                {
                                    NextTileCheck.Add(key);
                                }
                            }
                            tx = TileX;
                            ty = TileY;
                            tx++;
                            if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                            {
                                KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                {
                                    NextTileCheck.Add(key);
                                }
                            }
                            tx = TileX;
                            ty = TileY;
                            ty++;
                            if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                            {
                                KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                {
                                    NextTileCheck.Add(key);
                                }
                            }
                            tx = TileX;
                            ty = TileY;
                            tx--;
                            if (!Main.tile[tx, ty].active() || !Main.tileSolid[Main.tile[tx, ty].type])
                            {
                                KeyValuePair<int, int> key = new KeyValuePair<int, int>(tx, ty);
                                if (!NextTileCheck.Contains(key) && !CheckedTiles.Contains(key))
                                {
                                    NextTileCheck.Add(key);
                                }
                            }
                        }
                    }
                    TileCount++;
                }
            }
            if (SignID > -1)
            {
                //UpdateBountyBoardText();
                Main.sign[SignID].text = "Request coming soon...";
            }
        }

        public static void Update()
        {
            if (SignID > -1 && Main.player[Main.myPlayer].sign == SignID && PlayerRedeemReward(Main.player[Main.myPlayer]))
            {
                string Dialogue = "";
                if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], SardineID))
                {
                    Dialogue = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], SardineID).Name;
                }
                else
                {
                    Dialogue = GuardianBase.GetGuardianBase(SardineID).Name;
                }
                Dialogue += ": Nice job, here's your reward.";
                Main.NewText(Dialogue);
            }
            if (TargetMonsterSpawnPosition > -1)
            {
                if ((!Main.npc[TargetMonsterSpawnPosition].active && Main.npc[TargetMonsterSpawnPosition].life > 0) || Main.npc[TargetMonsterSpawnPosition].type != TargetMonsterID)
                {
                    TargetMonsterSpawnPosition = -1;
                    Main.NewText(TargetName + " has retreated.");
                }
            }
            if (SignID == -1 && Main.rand.Next(200) == 0)
            {
                TryFindingASign();
            }
            if (ActionCooldown % 60 == 0)
            {
                if (TargetMonsterID > 0 && TargetMonsterSpawnPosition == -1 && SpawnStress <= 0)
                {
                    for (int p = 0; p < 255; p++)
                    {
                        Player player = Main.player[p];
                        if (player.active && !player.dead)
                        {
                            byte Progress = GetBountyState(player);
                            if (Progress == 0)
                            {
                                bool TrySpawning = IsPlayerOnBountyBiome(player);
                                if (TrySpawning)
                                    TrySpawning = Main.rand.NextDouble() < 0.1;
                                if (TrySpawning)
                                {
                                    SpawnBountyMobOnPlayer(player);
                                    if (TargetMonsterSpawnPosition > -1)
                                    {
                                        //ResetSpawnStress(false);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateBountyBoardText();
            }
            if (ActionCooldown <= 0)
            {
                TryFindingASign();
                if (SignID > -1)
                {
                    if (!SardineTalkedToAboutBountyQuests)
                    {
                        SetDefaultCooldown();
                    }
                    else
                    {
                        if (TargetMonsterID == 0)
                        {
                            GenerateRequest();
                        }
                        else
                        {
                            SetDefaultCooldown();
                            TargetMonsterID = 0;
                            UpdateBountyBoardText();
                            Main.NewText("Bounty Hunting Ended", Color.MediumPurple);
                        }
                    }
                }
                else
                {
                    SetDefaultCooldown();
                }
            }
            ActionCooldown--;
        }

        public static bool SignExists()
        {
            return SignID > -1 && (Main.sign[SignID] != null && 
                Main.tile[Main.sign[SignID].x, Main.sign[SignID].y].active() && 
                Main.tileSign[Main.tile[Main.sign[SignID].x, Main.sign[SignID].y].type]);
        }

        public static void GenerateRequest()
        {
            for (int i = 0; i < BountyCounters.Length; i++)
            {
                BountyCounters[i] = 0;
            }
            {
                List<KeyValuePair<float, SpawnBiome>> PossibleBiomes = new List<KeyValuePair<float, SpawnBiome>>();
                if (NPC.downedBoss2)
                {
                    if (!WorldGen.crimson)
                    {
                        PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(2, SpawnBiome.Corruption));
                    }
                    else
                    {
                        PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(2, SpawnBiome.Crimson));
                    }
                }
                if (Main.hardMode)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(2, SpawnBiome.Hallow));
                }
                if (NPC.downedBoss3)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(1.5f, SpawnBiome.Dungeon));
                }
                if (NPC.downedBoss3 || NPC.downedSlimeKing || NPC.downedQueenBee || NPC.downedGoblins)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(1.15f, SpawnBiome.Underworld));
                }
                if ((!Main.hardMode && (NPC.downedQueenBee || NPC.downedBoss2)) || (Main.hardMode && NPC.downedPlantBoss))
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(1.35f, SpawnBiome.Jungle));
                }
                if (NPC.downedSlimeKing)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.75f, SpawnBiome.Ocean));
                }
                PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(1f, SpawnBiome.Snow));
                PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(1f, SpawnBiome.Night));
                PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.8f, SpawnBiome.Underground));
                if(NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedGoblins|| Main.hardMode)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.85f, SpawnBiome.Sky));
                }
                if (NPC.downedGoblins)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.65f, SpawnBiome.GoblinArmy));
                }
                if (NPC.downedPirates)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.55f, SpawnBiome.PirateArmy));
                }
                if (NPC.downedMartians)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.7f, SpawnBiome.MartianMadness));
                }
                if (NPC.downedFrost && Main.xMas)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.85f, SpawnBiome.SnowLegion));
                }
                if (NPC.downedPlantBoss)
                {
                    PossibleBiomes.Add(new KeyValuePair<float, SpawnBiome>(0.95f, SpawnBiome.LihzahrdTemple));
                }
                //
                float Total = PossibleBiomes.Sum(x => x.Key);
                float Current = 0;
                float PickedOne = Main.rand.NextFloat() * Total;
                bool GotABountyPlace = false;
                foreach(KeyValuePair<float, SpawnBiome> b in PossibleBiomes)
                {
                    if(PickedOne >= Current && PickedOne < Current + b.Key)
                    {
                        GotABountyPlace = true;
                        spawnBiome = b.Value;
                        break;
                    }
                    Current += b.Key;
                }
                if (!GotABountyPlace)
                {
                    SetDefaultCooldown();
                    return;
                }
            }
            TargetName = "";
            Suffix = "";
            TargetMonsterID = 0;
            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Corruptor;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Slimer;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.SeekerHead;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.CursedHammer;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.BigMimicCorruption;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.DevourerHead;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.EaterofSouls;
                            }
                        }
                        TargetName = NameGen(new string[]{"di", "sea", "sed", "ea", "ter", "of", "so", "ul", "de", "vou", "rer", "cor", "rup", "tor", "sli", "mer", "see", "ker"});
                        Suffix = GetRandomString(new string[] { "Plague Bearer", "Thousand Souls", "Corruption Spreader", "World Destroyer", "Reaper" });
                    }
                    break;
                case SpawnBiome.Crimson:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Crimslime;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Herpling;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.CrimsonAxe;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.BigMimicCrimson;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.FaceMonster;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.Crimera;
                            }
                        }
                        TargetName = NameGen(new string[] { "crim", "son", "blo", "od", "cri", "me", "ra", "fa", "ce", "mons", "ter", "herp", "ling" });
                        Suffix = GetRandomString(new string[] { "Blood Feaster", "Defiler", "Goremancer", "Arterial Traveller", "Heart Breaker" });
                    }
                    break;
                case SpawnBiome.Hallow:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(5))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Pixie;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Unicorn;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.ChaosElemental;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.EnchantedSword;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.BigMimicHallow;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "pi", "xie", "cha", "os","ele", "men", "tal", "uni", "corn", "en", "chan", "ted" });
                        Suffix = GetRandomString(new string[] { "Hallowed Inquisitioner", "Rainbow of Suffering", "Nausea Inducer", "Annoying Thing", "Prismatic Ray" });
                    }
                    break;

                case SpawnBiome.Jungle:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(6))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Derpling;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.GiantTortoise;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.GiantFlyingFox;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.MossHornet;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.Moth;
                                    break;
                                case 5:
                                    TargetMonsterID = NPCID.BigMimicJungle;
                                    break;
                            }
                        }
                        else
                        {
                            if (Main.rand.Next(2) == 0)
                            {
                                TargetMonsterID = NPCID.Hornet;
                            }
                            else
                            {
                                TargetMonsterID = NPCID.JungleBat;
                            }
                        }
                        TargetName = NameGen(new string[] { "fox", "hor", "net", "jun", "gle", "fly", "ing", "tor", "toi", "se", "moss", "derp", "ling" });
                        Suffix = GetRandomString(new string[] { "Deadly Poison", "Arms Dealer Hunter", "Catnapper", "Armor Piercer", "Home Wrecker" });
                    }
                    break;

                case SpawnBiome.Dungeon:
                    {
                        bool IsHardmodeDungeon = Main.hardMode && NPC.downedPlantBoss;
                        if (IsHardmodeDungeon)
                        {
                            switch (Main.rand.Next(12))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.BlueArmoredBonesSword;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.RustyArmoredBonesAxe;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.HellArmoredBonesMace;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.Necromancer;
                                    break;
                                case 4:
                                    TargetMonsterID = NPCID.RaggedCaster;
                                    break;
                                case 5:
                                    TargetMonsterID = NPCID.DiabolistRed;
                                    break;
                                case 6:
                                    TargetMonsterID = NPCID.SkeletonCommando;
                                    break;
                                case 7:
                                    TargetMonsterID = NPCID.SkeletonSniper;
                                    break;
                                case 8:
                                    TargetMonsterID = NPCID.TacticalSkeleton;
                                    break;
                                case 9:
                                    TargetMonsterID = NPCID.GiantCursedSkull;
                                    break;
                                case 10:
                                    TargetMonsterID = NPCID.BoneLee;
                                    break;
                                case 11:
                                    TargetMonsterID = NPCID.Paladin;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.AngryBones;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.DarkCaster;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.CursedSkull;
                                    break;
                                case 3:
                                    TargetMonsterID = NPCID.DungeonSlime;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "ske", "le", "ton", "an","gry","bo", "nes", "cas", "ter","cur","sed","dun", "ge", "on", "pa", "la", "din" });
                        Suffix = GetRandomString(new string[] { "Scary Spooky", "Dead Awakener", "Enemy of World", "He-Man's Nemesis", "Bone Breaker" });
                    }
                    break;

                case SpawnBiome.Underworld:
                    {
                        if (Main.hardMode)
                        {
                            switch (Main.rand.Next(2))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.RedDevil;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.Lavabat;
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    TargetMonsterID = NPCID.Demon;
                                    break;
                                case 1:
                                    TargetMonsterID = NPCID.BoneSerpentHead;
                                    break;
                                case 2:
                                    TargetMonsterID = NPCID.FireImp;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "de", "mon", "bo", "ne", "ser", "pent", "he" , "ad", "fi", "re", "imp", "red", "vil", "la", "va", "bat" });
                        Suffix = GetRandomString(new string[] { "Pact Maker", "Hell Breaker", "Torturer", "Lava Eater", "Human Buster" });
                    }
                    break;

                case SpawnBiome.Ocean:
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                TargetMonsterID = 65;
                                break;
                            case 1:
                                TargetMonsterID = 67;
                                break;
                        }
                        TargetName = NameGen(new string[] {"me", "ga", "lo", "don", "shark", "crab", "de", "vo", "ur", "de", "ath", "bre" });
                        Suffix = GetRandomString(new string[] {"Man-Eater", "Menace", "Drowner" });
                    }
                    break;

                case SpawnBiome.Underground:
                    {
                        switch (Main.rand.Next(6))
                        {
                            case 0:
                                TargetMonsterID = 21;
                                break;
                            case 1:
                                TargetMonsterID = 49;
                                break;
                            case 2:
                                TargetMonsterID = Main.rand.Next(498, 507);
                                break;
                            case 3:
                                TargetMonsterID = Main.rand.Next(404, 406);
                                break;
                            case 4:
                                TargetMonsterID = Main.rand.Next(496, 498);
                                break;
                            case 5:
                                TargetMonsterID = 196;
                                break;
                        }
                        if (Main.hardMode && Main.rand.NextFloat() < 0.6f)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = 77;
                                    break;
                                case 1:
                                    TargetMonsterID = 93;
                                    break;
                                case 2:
                                    TargetMonsterID = 110;
                                    break;
                                case 3:
                                    TargetMonsterID = 172;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "ske", "le", "ton", "sa", "la", "man", "der", "craw", "dad", "gi", "ant", "shel", "ly", "ca", "ve", "bat" });
                        Suffix = GetRandomString(new string[] { "Bane of the Living", "Impaler", "Human-Hunter", "Dark Stalker" });
                    }
                    break;

                case SpawnBiome.Sky:
                    {
                        TargetMonsterID = 48;
                        TargetName = NameGen(new string[] {"mar", "le", "ne", "har", "py", "da", "ria", "ki", "ra" });
                        Suffix = GetRandomString(new string[] { "Siren", "Matriarch", "Human Snatcher", "Sky Guardian" });
                    }
                    break;

                case SpawnBiome.OldOneArmy:
                    {
                        if (NPC.downedGolemBoss)
                        {
                            TargetMonsterID = 565;
                        }
                        else
                        {
                            TargetMonsterID = 564;
                        }
                        if(NPC.downedMechBossAny && Main.rand.NextDouble() < 0.6f)
                        {
                            if (NPC.downedGolemBoss)
                            {
                                TargetMonsterID = 577;
                            }
                            else
                            {
                                TargetMonsterID = 576;
                            }
                        }
                        TargetName = NameGen(new string[] { "go","blin", "ko", "bold", "wy", "vern", "dark", "ma", "ge", "dra", "kin", "wi", "ther", "be", "ast", "ogre" });
                        Suffix = GetRandomString(new string[] {"Lieutenant", "Captain", "Strategist", "General" });
                    }
                    break;

                case SpawnBiome.GoblinArmy:
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                TargetMonsterID = 28;
                                break;
                            case 1:
                                TargetMonsterID = 111;
                                break;
                            case 2:
                                TargetMonsterID = 29;
                                break;
                        }
                        TargetName = NameGen(new string[] { "dex", "ter", "try", "xa", "bi", "dri", "dab", "bad", "chun", "gus" });
                        Suffix = GetRandomString(new string[] { "Leader", "Raider", "Looter", "Slaver" });
                    }
                    break;

                case SpawnBiome.Night:
                    {
                        if(Main.rand.Next(2) == 0)
                        {
                            TargetMonsterID = 3;
                        }
                        else
                        {
                            TargetMonsterID = 2;
                        }
                        if(Main.hardMode && Main.rand.NextDouble() < 0.6)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    TargetMonsterID = 140;
                                    break;
                                case 1:
                                    TargetMonsterID = 82;
                                    break;
                                case 2:
                                    TargetMonsterID = 104;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "zom", "bie", "de", "mon", "eye", "ra", "ven", "pos", "ses", "ed", "wra", "ith", "we", "re", "wolf" });
                        Suffix = GetRandomString(new string[] { "Night Crawler", "Restless Soul", "Prowler" });
                    }
                    break;

                case SpawnBiome.Snow:
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                TargetMonsterID = 150;
                                break;
                            case 1:
                                TargetMonsterID = 147;
                                break;
                            case 2:
                                TargetMonsterID = 197;
                                break;
                        }
                        if(Main.hardMode && Main.rand.NextDouble() < 0.6)
                        {
                            if (Main.rand.NextDouble() < 0.25)
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        TargetMonsterID = 170;
                                        break;
                                    case 1:
                                        TargetMonsterID = 171;
                                        break;
                                    case 2:
                                        TargetMonsterID = 180;
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        TargetMonsterID = 154;
                                        break;
                                    case 1:
                                        TargetMonsterID = 169;
                                        break;
                                    case 2:
                                        TargetMonsterID = 206;
                                        break;
                                }
                            }
                        }
                        TargetName = NameGen(new string[] { "ice", "bat", "snow", "flinx", "un", "de", "ad", "vi", "king", "tor", "toi", "se", "ele", "men", "tal",
                        "mer", "man"});
                        Suffix = GetRandomString(new string[] { "Cold Heart", "Shiver Causer", "Chilled One", "One Who Paints Snow in Red" });
                    }
                    break;

                case SpawnBiome.Desert:
                    {
                        switch (Main.rand.Next(4))
                        {
                            case 0:
                                TargetMonsterID = 69;
                                break;
                            case 1:
                                TargetMonsterID = 61;
                                break;
                            case 2:
                                TargetMonsterID = 508;
                                break;
                            case 3:
                                TargetMonsterID = 509;
                                break;
                        }
                        if(Main.hardMode && Main.rand.NextDouble() < 0.6f)
                        {
                            switch (Main.rand.Next(4))
                            {
                                case 0:
                                    TargetMonsterID = 78;
                                    break;
                                case 1:
                                    TargetMonsterID = 532;
                                    break;
                                case 2:
                                    TargetMonsterID = Main.rand.NextDouble() < 0.5 ? 528 : 529;
                                    break;
                                case 3:
                                    TargetMonsterID = 533;
                                    break;
                            }
                        }
                        TargetName = NameGen(new string[] { "ant", "li", "on", "ba", "si", "lisk", "poa", "cher", "la", "mia", "gho", "ul" });
                        Suffix = GetRandomString(new string[] { "Elusive", "Burier", "Forgotten", "Sandman" });
                    }
                    break;

                case SpawnBiome.PirateArmy:
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                TargetMonsterID = 212;
                                break;
                            case 1:
                                TargetMonsterID = 215;
                                break;
                            case 2:
                                TargetMonsterID = 216;
                                break;
                        }
                        TargetName = NameGen(new string[] { "yar", "bla", "ho", "rum", "ha", "scur", "vy", "sea", "bleh", "yo", "bot", "tle" });
                        Suffix = GetRandomString(new string[] { "Yaar!", "Aaargh!", "Yo ho ho", "Scurvy" } );
                    }
                    break;

                case SpawnBiome.MartianMadness:
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                TargetMonsterID = 391;
                                break;
                            case 1:
                                TargetMonsterID = 520;
                                break;
                            case 2:
                                TargetMonsterID = 383;
                                break;
                        }
                        TargetName = NameGen(new string[] { "mar", "vin", "ti", "an", "scut", "lix", "gun", "ner", "scram", "bler", "gi", "ga", "zap", "per", "tes", "la" });
                        Suffix = GetRandomString(new string[] { "Dissector", "Abductor", "World Conqueror", "Who Resents Humans" });
                    }
                    break;

                case SpawnBiome.SnowLegion:
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                TargetMonsterID = 144;
                                break;
                            case 1:
                                TargetMonsterID = 143;
                                break;
                            case 2:
                                TargetMonsterID = 145;
                                break;
                        }
                        TargetName = NameGen(new string[] { "dan", "den", "din", "don", "dun", "frost", "stab", "by", "gang", "sta", "bal", "la", "thomp", "son" });
                        Suffix = GetRandomString(new string[] { "Godfather", "Abductor", "World Conqueror", "Who Resents Humans" });
                    }
                    break;

                case SpawnBiome.LihzahrdTemple:
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                TargetMonsterID = 198;
                                break;
                            case 1:
                                TargetMonsterID = 226;
                                break;
                        }
                        TargetName = NameGen(new string[] { "lih", "zah", "rd", "fly", "ing", "sna", "ke", "go", "lem" });
                        Suffix = GetRandomString(new string[] { "Ancient", "Sun Cultist", "Mechanic", "Who Praises the Sun" });
                    }
                    break;
            }

            modifier.Clear();
            modifier.Add((Modifiers)Main.rand.Next(7));
            float ChanceCounter = 0.75f;
            while (Main.rand.NextDouble() < ChanceCounter)
            {
                Modifiers newMod = (Modifiers)Main.rand.Next((int)Modifiers.Count);
                if ((newMod == Modifiers.Osmose || newMod == Modifiers.Sapping) && !(NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode))
                    continue;
                if (!modifier.Contains(newMod))
                    modifier.Add(newMod);
                else
                    break;
                ChanceCounter *= 0.5f;
            }
            CoinReward = 5000;
            dangerousModifier = DangerousModifier.None;
            if (Main.rand.NextDouble() <= 0.45) //Boss Mods
            {
                List<KeyValuePair<DangerousModifier, float>> SpecialModifier = new List<KeyValuePair<DangerousModifier, float>>();
                if (NPC.downedGolemBoss)
                    SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Cyclops, 0.4f));
                if (NPC.downedFishron)
                    SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Sharknado, 0.4f));
                SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.GoldenShower, 0.4f));
                SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Haunted, 0.4f));
                SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Alchemist, 0.4f));
                if (Main.hardMode)
                    SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Sapper, 0.4f));
                SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Cursed, 0.4f));
                SpecialModifier.Add(new KeyValuePair<DangerousModifier, float>(DangerousModifier.Reaper, 0.4f));
                if (SpecialModifier.Count > 0)
                {
                    float PickedRate = Main.rand.NextFloat() * SpecialModifier.Sum(x => x.Value);
                    float CurrentSum = 0;
                    bool PickedSpecialMod = false;
                    foreach (KeyValuePair<DangerousModifier, float> mod in SpecialModifier)
                    {
                        if(PickedRate >= CurrentSum && PickedRate < CurrentSum + mod.Value)
                        {
                            dangerousModifier = mod.Key;
                            PickedSpecialMod = true;
                            break;
                        }
                        CurrentSum += mod.Value;
                    }
                    if (PickedSpecialMod)
                        CoinReward += 1250;
                }
            }
            CoinReward += ExtraCoinRewardFromProgress();
            CoinReward = (int)(GetExtraRewardFromModifiers() * CoinReward * (Main.rand.Next(80, 121) * 0.01f));

            CreateRewards(GetExtraRewardFromModifiers());

            ActionCooldown = RequestEndMinTime + Main.rand.Next(RequestEndMaxTime - RequestEndMinTime + 1);

            string AnnounceText = "New Bounty Quest available!";
            if (IsAnnouncementBox)
            {
                AnnounceText += "\nHunt " + TargetFullName + " in the " + spawnBiome.ToString() + ".";
                if (CoinReward > 0 || RewardList.Length > 0)
                {
                    AnnounceText += "\nReward: ";
                }
                if (CoinReward > 0)
                {
                    int p = 0, g = 0, s = 0, c = CoinReward;
                    if (c >= 100)
                    {
                        s += c / 100;
                        c -= s * 100;
                    }
                    if (s >= 100)
                    {
                        g += s / 100;
                        s -= g * 100;
                    }
                    if (g >= 100)
                    {
                        p += g / 100;
                        g -= p * 100;
                    }
                    if (p > 0)
                    {
                        AnnounceText += "[i/s" + p + ":" + Terraria.ID.ItemID.PlatinumCoin + "]";
                    }
                    if (g > 0)
                    {
                        AnnounceText += "[i/s" + g + ":" + Terraria.ID.ItemID.GoldCoin + "]";
                    }
                    if (s > 0)
                    {
                        AnnounceText += "[i/s" + s + ":" + Terraria.ID.ItemID.SilverCoin + "]";
                    }
                    if (c > 0)
                    {
                        AnnounceText += "[i/s" + c + ":" + Terraria.ID.ItemID.CopperCoin + "]";
                    }
                    //AnnounceText += Main.ValueToCoins(CoinReward);
                }
                if (RewardList.Length > 0)
                {
                    AnnounceText += " and ";// +RewardList[0].HoverName;
                    if (RewardList[0].prefix > 0)
                    {
                        AnnounceText += "[i/p" + RewardList[0].prefix + ":" + RewardList[0].type + "]";
                    }
                    else
                    {
                        AnnounceText += "[i/s" + RewardList[0].stack + ":" + RewardList[0].type + "]";
                    }
                }
                AnnounceText += ".";
            }
            Main.NewTextMultiline(AnnounceText, false, Microsoft.Xna.Framework.Color.MediumPurple);

            BountyProgress.Clear();

            ResetSpawnStress(true);

            UpdateBountyBoardText();
        }

        public static void ResetSpawnStress(bool OnGeneration)
        {
            if (OnGeneration)
                SpawnStress = Main.rand.Next(20, 41);
            else
                SpawnStress = Main.rand.Next(10, 21);
            if (Main.hardMode) SpawnStress += 20;
            if (spawnBiome == SpawnBiome.Night)
                SpawnStress /= 2;
        }

        public static void CreateRewards(float RewardMod)
        {
            List<Item> Rewards = new List<Item>();
            Item i;
            if (Main.rand.NextDouble() < 0.03 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(4))
                {
                    case 0:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.PackLeaderNecklace>());
                        break;
                    case 1:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.GoldenShowerPapyrus>());
                        break;
                    case 2:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.FirstSymbol>());
                        break;
                    case 3:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.TwoHandedMastery>());
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.1 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.FishermansGuide);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.WeatherRadio);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Sextant);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.2 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.LifeformAnalyzer);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.MetalDetector);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Radar);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.05f * RewardMod)
            {
                i = new Item();
                if (Main.rand.NextDouble() < 0.75)
                {
                    i.SetDefaults(ItemID.MagicMirror);
                }
                else
                {
                    i.SetDefaults(ItemID.PocketMirror);
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.1f * RewardMod)
            {
                i = GetRandomAccessory();
                if (i != null)
                    Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.15 * RewardMod)
            {
                i = GetRandomWeaponID();
                if (i != null)
                {
                    Rewards.Add(i);
                }
            }
            if (Main.rand.NextDouble() < 0.1 * RewardMod)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.SkillResetPotion>());
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.25 * RewardMod)
            {
                i = new Item();
                List<int> BossSpawnItems = new List<int>();
                //Check each boss that has been killed, then add the respective item to the boss spawn item list.
                if (NPC.downedBoss1)
                    BossSpawnItems.Add(ItemID.SuspiciousLookingEye);
                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                        BossSpawnItems.Add(ItemID.BloodySpine);
                    else
                        BossSpawnItems.Add(ItemID.WormFood);
                }
                if (NPC.downedBoss3)
                    BossSpawnItems.Add(ItemID.ClothierVoodooDoll);
                if (NPC.downedSlimeKing)
                    BossSpawnItems.Add(ItemID.SlimeCrown);
                if (NPC.downedQueenBee)
                    BossSpawnItems.Add(ItemID.Abeemination);
                if (Main.hardMode)
                    BossSpawnItems.Add(ItemID.GuideVoodooDoll);
                if (NPC.downedMechBossAny)
                {
                    BossSpawnItems.Add(ItemID.MechanicalEye);
                    BossSpawnItems.Add(ItemID.MechanicalSkull);
                    BossSpawnItems.Add(ItemID.MechanicalWorm);
                }
                if (NPC.downedGolemBoss)
                    BossSpawnItems.Add(ItemID.LihzahrdPowerCell);
                if (NPC.downedFishron)
                    BossSpawnItems.Add(ItemID.TruffleWorm);
                if (NPC.downedMoonlord)
                    BossSpawnItems.Add(ItemID.CelestialSigil);
                if (BossSpawnItems.Count > 0)
                {
                    i.SetDefaults(BossSpawnItems[Main.rand.Next(BossSpawnItems.Count)]);
                    /*if (i.maxStack > 0)
                    {
                        i.stack += Main.rand.Next((int)(3 * RewardMod));
                    }*/
                    Rewards.Add(i);
                }
            }
            if (!MainMod.NoEtherItems && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.EtherFruit>());
                if (Main.rand.NextDouble() < 0.6 * RewardMod)
                    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (!MainMod.NoEtherItems && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.EtherHeart>());
                //if (Main.rand.NextDouble() < 0.4 * RewardMod)
                //    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeFruit);
                if (Main.rand.NextDouble() < 0.6 * RewardMod)
                    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeCrystal);
                //if (Main.rand.NextDouble() < 0.4 * RewardMod)
                //    i.stack += Main.rand.Next((int)(RewardMod));
                Rewards.Add(i);
            }
            if (spawnBiome != SpawnBiome.Underworld && Main.rand.Next(5) == 0)
            {
                i = new Item();
                switch (spawnBiome)
                {
                    case SpawnBiome.Corruption:
                        i.SetDefaults(ItemID.CorruptFishingCrate);
                        break;
                    case SpawnBiome.Crimson:
                        i.SetDefaults(ItemID.CrimsonFishingCrate);
                        break;
                    case SpawnBiome.Dungeon:
                        i.SetDefaults(ItemID.DungeonFishingCrate);
                        break;
                    case SpawnBiome.Hallow:
                        i.SetDefaults(ItemID.HallowedFishingCrate);
                        break;
                    case SpawnBiome.Jungle:
                        i.SetDefaults(ItemID.JungleFishingCrate);
                        break;
                    case SpawnBiome.Sky:
                        i.SetDefaults(ItemID.FloatingIslandFishingCrate);
                        break;
                }
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                if(i.type != 0)
                    Rewards.Add(i);
            }
            if (Main.rand.Next(5) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.HerbBag);
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                Rewards.Add(i);
            }
            RewardList = Rewards.ToArray();
        }

        public static Item GetRandomWeaponID()
        {
            int WeaponID = 0;
            {
                List<int> RewardToGet = new List<int>();
                switch (spawnBiome)
                {
                    case SpawnBiome.Corruption:
                        RewardToGet.AddRange(new int[] { ItemID.BallOHurt, ItemID.DemonBow, ItemID.Musket, ItemID.Vilethorn, ItemID.LightsBane, ItemID.DemonBow });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Toxikarp, ItemID.DartRifle, ItemID.CursedFlames, ItemID.ClingerStaff });
                        }
                        break;

                    case SpawnBiome.Crimson:
                        RewardToGet.AddRange(new int[] { ItemID.TheRottedFork, ItemID.TheUndertaker, ItemID.TheMeatball, ItemID.TendonBow, ItemID.CrimsonRod, ItemID.BloodButcherer });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Bladetongue, ItemID.DartPistol, ItemID.GoldenShower });
                        }
                        break;

                    case SpawnBiome.Dungeon:
                        RewardToGet.AddRange(new int[] { ItemID.Muramasa, ItemID.Handgun, ItemID.AquaScepter, ItemID.MagicMissile, ItemID.BlueMoon });
                        if (NPC.downedPlantBoss)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.TacticalShotgun, ItemID.SniperRifle, ItemID.Keybrand, ItemID.RocketLauncher, ItemID.SpectreStaff, ItemID.InfernoFork, ItemID.ShadowbeamStaff, ItemID.MagnetSphere });
                        }
                        break;

                    case SpawnBiome.Jungle:
                        RewardToGet.AddRange(new int[] { ItemID.BladeofGrass, ItemID.ThornChakram, ItemID.BeeKeeper, ItemID.BeesKnees, ItemID.BeeGun, ItemID.HornetStaff });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.ChlorophyteClaymore, ItemID.ChlorophytePartisan, ItemID.ChlorophyteSaber, ItemID.ChlorophyteShotbow, ItemID.Uzi });
                        }
                        break;

                    case SpawnBiome.Underworld:
                        RewardToGet.AddRange(new int[] { ItemID.FieryGreatsword, ItemID.DarkLance, ItemID.Sunfury, ItemID.FlowerofFire, ItemID.Flamelash, ItemID.HellwingBow, ItemID.ImpStaff });
                        if (NPC.downedMechBossAny)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.UnholyTrident, ItemID.ObsidianSwordfish });
                        }
                        break;

                    case SpawnBiome.Hallow:
                        RewardToGet.AddRange(new int[] { ItemID.PearlwoodSword });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.CrystalStorm, ItemID.CrystalSerpent });
                        }
                        break;

                    case SpawnBiome.Ocean:
                        RewardToGet.AddRange(new int[] { ItemID.Swordfish });
                        break;

                    case SpawnBiome.Underground:
                        RewardToGet.AddRange(new int[] { ItemID.ChainKnife, ItemID.Spear, ItemID.WoodenBoomerang, ItemID.EnchantedBoomerang });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.BeamSword, ItemID.Marrow, ItemID.PoisonStaff, ItemID.SpiderStaff, ItemID.QueenSpiderStaff });
                        }
                        break;

                    case SpawnBiome.Sky:
                        RewardToGet.AddRange(new int[] { ItemID.Starfury, ItemID.DaedalusStormbow });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.NimbusRod });
                        }
                        break;

                    case SpawnBiome.OldOneArmy:

                        break;

                    case SpawnBiome.Snow:
                        RewardToGet.AddRange(new int[] { ItemID.IceBlade, ItemID.IceBoomerang, ItemID.SnowballCannon });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Frostbrand, ItemID.IceBow, ItemID.FlowerofFrost, ItemID.FrostStaff, ItemID.IceRod });
                        }
                        break;

                    case SpawnBiome.Desert:
                        RewardToGet.AddRange(new int[] { ItemID.AntlionMandible, ItemID.AmberStaff });
                        break;
                }
                if (!Main.hardMode)
                {
                    float LootRate = Main.rand.NextFloat();
                    RewardToGet.AddRange(new int[] { ItemID.CopperShortsword, ItemID.CopperBroadsword, ItemID.CopperBow,
                        ItemID.TinShortsword, ItemID.TinBroadsword, ItemID.TinBow});
                    //if (LootRate < 0.667)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.SilverShortsword, ItemID.SilverBroadsword, ItemID.SilverBow,
                            ItemID.TungstenShortsword, ItemID.TungstenBroadsword, ItemID.TungstenBow});
                    }
                    //if (LootRate < 0.333)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.GoldShortsword, ItemID.GoldBroadsword, ItemID.GoldBow,
                            ItemID.PlatinumShortsword, ItemID.PlatinumBroadsword, ItemID.PlatinumBow});
                    }
                }
                else
                {
                    float LootRate = Main.rand.NextFloat();
                    RewardToGet.AddRange(new int[] { ItemID.CobaltSword, ItemID.CobaltNaginata, ItemID.CobaltRepeater,
                        ItemID.PalladiumSword, ItemID.PalladiumPike, ItemID.PalladiumRepeater});
                    //if (LootRate < 0.667)
                    {
                        RewardToGet.AddRange(new int[] {ItemID.MythrilSword, ItemID.MythrilHalberd, ItemID.MythrilRepeater,
                            ItemID.OrichalcumSword, ItemID.OrichalcumHalberd, ItemID.OrichalcumRepeater });
                    }
                    //if (LootRate < 0.333)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.AdamantiteSword, ItemID.AdamantiteGlaive, ItemID.AdamantiteRepeater,
                            ItemID.TitaniumSword, ItemID.TitaniumTrident, ItemID.TitaniumRepeater});
                    }
                }
                if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    RewardToGet.AddRange(new int[] { ItemID.Excalibur, ItemID.Gungnir, ItemID.HallowedRepeater });
                }
                if (RewardToGet.Count > 0)
                    WeaponID = RewardToGet[Main.rand.Next(RewardToGet.Count)];
            }
            if (WeaponID > 0)
            {
                Item i = new Item();
                i.SetDefaults(WeaponID);
                byte prefix = 0;
                if (i.melee)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Dangerous;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Legendary;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.ranged)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Deadly2;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Rapid;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Powerful;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Unreal;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.magic)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Masterful;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Celestial;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Mystic;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Mythical;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.summon)
                {
                    switch (Main.rand.Next(9))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Murderous;
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Hurtful;
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Unpleasant;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                i.Prefix(prefix);
                return i;
            }
            return null;
        }

        public static Item GetRandomAccessory()
        {
            List<int> ItemIDs = new List<int>();
            ItemIDs.Add(ItemID.Aglet);
            ItemIDs.Add(ItemID.HermesBoots);
            ItemIDs.Add(ItemID.ClimbingClaws);
            ItemIDs.Add(ItemID.ShoeSpikes);
            ItemIDs.Add(ItemID.Flipper);
            ItemIDs.Add(ItemID.LuckyHorseshoe);
            ItemIDs.Add(ItemID.ShinyRedBalloon);
            //
            ItemIDs.Add(ItemID.BandofRegeneration);

            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    ItemIDs.Add(ItemID.BandofRegeneration);
                    break;
                case SpawnBiome.Crimson:
                    ItemIDs.Add(ItemID.PanicNecklace);
                    break;
                case SpawnBiome.Dungeon:
                    ItemIDs.Add(ItemID.CobaltShield);
                    break;
                case SpawnBiome.Jungle:
                    ItemIDs.Add(ItemID.AnkletoftheWind);
                    ItemIDs.Add(ItemID.FeralClaws);
                    ItemIDs.Add(ItemID.FlowerBoots);
                    break;
                case SpawnBiome.Underworld:
                    ItemIDs.Add(ItemID.LavaCharm);
                    ItemIDs.Add(ItemID.ObsidianRose);
                    ItemIDs.Add(ItemID.ObsidianSkull);
                    ItemIDs.Add(ItemID.MagmaStone);
                    break;
            }
            if (ItemIDs.Count > 0)
            {
                Item i = new Item();
                i.SetDefaults(ItemIDs[Main.rand.Next(ItemIDs.Count)]);
                byte prefix = 0;
                if (Main.rand.NextDouble() < 0.8f)
                {
                    switch (Main.rand.Next(12))
                    {
                        case 0:
                            prefix = Terraria.ID.PrefixID.Armored;
                            break;
                        case 1:
                            prefix = Terraria.ID.PrefixID.Warding;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Precise;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Lucky;
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Angry;
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Menacing;
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Hasty;
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Quick;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Intrepid;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Violent;
                            break;
                        case 10:
                            prefix = Terraria.ID.PrefixID.Arcane;
                            break;

                    }
                }
                i.Prefix(prefix);
                return i;
            }
            return null;
        }

        public static string GetDifficultyList()
        {
            string DifficultyString = "";
            bool First = true;
            foreach (Modifiers mod in Enum.GetValues(typeof(Modifiers)))
            {
                if (modifier.Contains(mod))
                {
                    if (First)
                        First = false;
                    else
                        DifficultyString += ", ";
                    bool FirstLetter = true;
                    foreach(char c in mod.ToString())
                    {
                        if (char.IsUpper(c) && !First)
                            DifficultyString += " ";
                        DifficultyString += c;
                        FirstLetter = false;
                    }
                    //DifficultyString += mod.ToString();
                }
            }
            if (DifficultyString == "")
                DifficultyString = "None";
            return DifficultyString;
        }

        public static void UpdateBountyBoardText()
        {
            if (SignID > -1)
            {
                string Text = NoRequestText;
                if (!SardineTalkedToAboutBountyQuests)
                {
                    if(NpcMod.HasGuardianNPC(2))
                        Text = "Hey, could you come talk to me about Bounty Quests?\n\n  - " + NpcMod.GetGuardianNPCName(2);
                }
                else if (TargetMonsterID > 0)
                {
                    Text = "Hunt " + TargetFullName + ".";
                    Text += "\n  Difficulty: " + GetDifficultyList() + ".";
                    Text += "\n  Dangerous Trait: " + dangerousModifier.ToString();
                    Text += "\n  Last seen in the " + spawnBiome.ToString() + ".";
                    Text += "\n  Reward: " + Main.ValueToCoins(CoinReward);
                    Text += "\n  ";
                    bool First = true;
                    foreach (Item i in RewardList)
                    {
                        if (!First)
                            Text += ", ";
                        else
                            First = false;
                        Text += i.HoverName;
                    }
                    if (!First)
                        Text += ".";
                    Text += "\n Time Left: ";
                    int h = 0, m = 0, s = ActionCooldown / 60;
                    if (s >= 60)
                    {
                        m += s / 60;
                        s -= m * 60;
                    }
                    if (m >= 60)
                    {
                        h += m / 60;
                        m -= h * 60;
                    }
                    First = true;
                    if (h > 0)
                    {
                        Text += h + " Hours";
                        First = false;
                    }
                    if (m > 0)
                    {
                        if (!First)
                        {
                            Text += ", ";
                        }
                        else
                        {
                            First = false;
                        }
                        Text += m + " Minutes";
                    }
                    if (h == 0 && m == 0)
                    {
                        Text += "Ending in a few seconds";
                    }
                    /*if (s > 0)
                    {
                        if (!First)
                        {
                            Text += ", ";
                        }
                        else
                        {
                            First = false;
                        }
                        Text += s + " Seconds";
                    }*/
                    Text += ".";
                    if (PlayerAlreadyRedeemedReward(Main.player[Main.myPlayer]))
                        Text += "\n    Clear!!";
                }
                if (SignExists())
                {
                    Sign.TextSign(SignID, Text);
                    if (Main.sign[SignID] == null)
                        SignID = -1;
                }
            }
        }

        public static int ExtraCoinRewardFromProgress()
        {
            int Extra = 0;
            if (NPC.downedBoss1)
                Extra += 125;
            if (NPC.downedBoss2)
                Extra += 250;
            if (NPC.downedBoss3)
                Extra += 500;
            if (NPC.downedSlimeKing)
                Extra += 350;
            if (NPC.downedQueenBee)
                Extra += 400;
            if (Main.hardMode)
                Extra += 1000;
            if (NPC.downedMechBoss1)
                Extra += 1150;
            if (NPC.downedMechBoss2)
                Extra += 1400;
            if (NPC.downedMechBoss3)
                Extra += 1850;
            if (NPC.downedPlantBoss)
                Extra += 2220;
            if (NPC.downedGolemBoss)
                Extra += 2650;
            if (NPC.downedAncientCultist)
                Extra += 3500;
            if (NPC.downedFishron)
                Extra += 4000;
            if (NPC.downedMoonlord)
                Extra += 5000;
            return Extra;
        }

        public static string GetRandomString(string[] List)
        {
            return Utils.SelectRandom(Main.rand, List);
        }

        public static string NameGen(string[] Syllabes)
        {
            string NewName = "";
            double Chance = 2f;
            bool First = true;
            List<int> UsedSyllabes = new List<int>();
            while (Main.rand.NextDouble() < Chance)
            {
                int SelectedSyllabe = Main.rand.Next(Syllabes.Length);
                int SyllabesDisponible = 0;
                for (int s = 0; s < Syllabes.Length; s++)
                {
                    if (!UsedSyllabes.Contains(s))
                    {
                        SyllabesDisponible++;
                    }
                }
                if (SyllabesDisponible == 0)
                    break;
                if (UsedSyllabes.Contains(SelectedSyllabe))
                    continue;
                UsedSyllabes.Add(SelectedSyllabe);
                string Syllabe = Syllabes[SelectedSyllabe].ToLower();
                foreach (char Letter in Syllabe)
                {
                    NewName += Letter;
                    if (First)
                    {
                        NewName = NewName.ToUpper();
                        First = false;
                    }
                }
                if (Chance > 1f)
                    Chance--;
                else if (Chance > 0.5f)
                    Chance -= 0.2f;
                else
                {
                    Chance *= 0.5f;
                }
            }
            return NewName;
        }

        public static void SetDefaultCooldown()
        {
            ActionCooldown = 5 * 3600;
        }

        public enum SpawnBiome : byte
        {
            Corruption, Crimson, Dungeon, Jungle, Underworld, Hallow,
            Ocean, Underground, Sky, OldOneArmy, GoblinArmy, Night, Snow, Desert,
            PirateArmy, MartianMadness, SnowLegion, LihzahrdTemple
        }

        public enum Modifiers : byte
        {
            ExtraHealth,
            ExtraDamage,
            ExtraDefense,
            Armored,
            HealthRegen,
            Boss,
            Noob,
            Petrifying,
            Leader,
            FireRain,
            Fatal,
            Imobilizer,
            MeleeDefense,
            RangedDefense,
            MagicDefense,
            Sapping,
            Osmose,
            Retaliate,
            ManaBurn,
            Count
        }

        public enum DangerousModifier : byte
        {
            None,
            Reaper,
            Cyclops,
            GoldenShower,
            Haunted,
            Alchemist,
            Sharknado,
            Sapper,
            Cursed,
            Invoker
        }
    }
}
