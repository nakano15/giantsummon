using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace giantsummon
{
    public class NpcMod : GlobalNPC
    {
        public static bool TryPlacingCatGuardianOnSlime = false;
        public static int TrappedCatKingSlime = -1;
        public static Vector2[] PlayerPositionBackup = new Vector2[256];
        public static bool[] PlayerDeadStatusBackup = new bool[256];
        public static bool[] PlayerWetBackup = new bool[256];
        public MobTypes mobType = MobTypes.Normal;
        public short KbResistance = 1000;
        public bool RechargingKbResist = false;
        public static MobTypes LatestMobType = MobTypes.Normal;

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public int DamageBonus
        {
            get
            {
                switch (mobType)
                {
                    case MobTypes.Veteran:
                        return 20;
                    case MobTypes.Elite:
                        return 40;
                    case MobTypes.Champion:
                        return 60;
                    case MobTypes.Legendary:
                        return 80;
                    case MobTypes.Epic:
                        return 100;
                }
                return 0;
            }
        }

        public int DefenseBonus
        {
            get
            {
                switch (mobType)
                {
                    case MobTypes.Veteran:
                        return 5;
                    case MobTypes.Elite:
                        return 10;
                    case MobTypes.Champion:
                        return 15;
                    case MobTypes.Legendary:
                        return 20;
                    case MobTypes.Epic:
                        return 30;
                }
                return 0;
            }
        }

        public float HealthBonus
        {
            get
            {
                switch (mobType)
                {
                    case MobTypes.Veteran:
                        return 4f;
                    case MobTypes.Elite:
                        return 8f;
                    case MobTypes.Champion:
                        return 12f;
                    case MobTypes.Legendary:
                        return 16f;
                    case MobTypes.Epic:
                        return 20f;
                }
                return 1f;

            }
        }

        public float CoinBonus
        {
            get
            {
                switch (mobType)
                {
                    case MobTypes.Veteran:
                        return 1.2f;
                    case MobTypes.Elite:
                        return 1.4f;
                    case MobTypes.Champion:
                        return 1.8f;
                    case MobTypes.Legendary:
                        return 2.6f;
                    case MobTypes.Epic:
                        return 3.2f;
                }
                return 1f;
            }
        }

        private void UpdateSoulDrain(NPC npc)
        {
            const int MaxDistance = 1100;
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    if (!Main.player[i].dead && Main.player[i].soulDrain > 0)
                        continue;
                    TerraGuardian[] guardians = Main.player[i].GetModPlayer<PlayerMod>().GetAllGuardianFollowers;
                    bool HasGuardianUsingSoulDrain = guardians.Any(x => x.Active && !x.Downed && x.SelectedItem > -1 && x.Inventory[x.SelectedItem].type == 3006 && x.ItemAnimationTime > 0 && (x.CenterPosition - npc.Center).Length() < MaxDistance);
                    if (HasGuardianUsingSoulDrain)
                    {
                        if (!Main.player[i].dead && i == Main.myPlayer)
                        {
                            Main.player[i].soulDrain++;
                        }
                        foreach (TerraGuardian guardian in guardians)
                        {
                            if (guardian.Active && !guardian.Downed)
                            {
                                guardian.AddBuff(Terraria.ID.BuffID.SoulDrain, 5);
                            }
                        }
                        if (Main.rand.Next(3) != 0)
                        {
                            Vector2 center = npc.Center;
                            center.X += (float)Main.rand.Next(-100, 100) * 0.05f;
                            center.Y += (float)Main.rand.Next(-100, 100) * 0.05f;
                            int index2 = Dust.NewDust(center + npc.velocity, 1, 1, 235, 0.0f, 0.0f, 0, new Color(), 1f);
                            Main.dust[index2].velocity *= 0.0f;
                            Main.dust[index2].scale = (float)Main.rand.Next(70, 85) * 0.01f;
                            Main.dust[index2].fadeIn = (float)(i + 1);
                        }
                    }
                }
            }
        }

        public override bool PreAI(NPC npc)
        {
            if(npc.soulDrain)
                UpdateSoulDrain(npc);
            if (npc.type == Terraria.ID.NPCID.KingSlime && TryPlacingCatGuardianOnSlime)
            {
                if (!NpcMod.HasMetGuardian(2))
                    TrappedCatKingSlime = npc.whoAmI;
                TryPlacingCatGuardianOnSlime = false;
            }
            if (TrappedCatKingSlime == npc.whoAmI && npc.type != Terraria.ID.NPCID.KingSlime)
                TrappedCatKingSlime = -1;
            MaskGuardianPositionToPlayers(npc);
            if(npc.whoAmI == GuardianBountyQuest.TargetMonsterSpawnPosition)
                GuardianBountyQuest.UpdateBountyNPC(npc);
            if (MainMod.UseNewMonsterModifiersSystem)
            {
                if (npc.realLife > -1)
                {
                    mobType = Main.npc[npc.realLife].GetGlobalNPC<NpcMod>().mobType;
                }
                npc.defense = npc.defDefense;
                npc.damage = npc.defDamage;
            }
            LatestMobType = npc.GetGlobalNPC<NpcMod>().mobType;
            return base.PreAI(npc);
        }

        public override void SetDefaults(NPC npc)
        {
            if (npc.type == Terraria.ID.NPCID.KingSlime && Main.netMode != 1 && TrappedCatKingSlime == -1 && !NpcMod.HasMetGuardian(2) && !NpcMod.HasGuardianNPC(2) && Main.rand.NextDouble() < 0.4)
            {
                TryPlacingCatGuardianOnSlime = true;
            }
            if (GuardianBountyQuest.SpawningBountyMob)
            {
                npc.GivenName = GuardianBountyQuest.TargetFullName;
                GuardianBountyQuest.ApplyModifier(npc);
                npc.defDamage = npc.damage;
                npc.defDefense = npc.defense;
            }
            if (MainMod.MobHealthBoost && npc.lifeMax > 5 && Main.netMode == 0 && !Main.gameMenu)
            {
                int MyGuardians = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetSummonedGuardianCount;
                if(MyGuardians > 1)
                {
                    npc.lifeMax += (int)(npc.lifeMax * 0.05f * (MyGuardians - 1));
                }
            }
            if (MainMod.UseNewMonsterModifiersSystem)
            {
                if (npc.boss || Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type] || npc.type == Terraria.ID.NPCID.Sharkron || npc.type == Terraria.ID.NPCID.Sharkron2)
                {
                    mobType = GetBossDifficulty(npc);
                }
                else if (npc.catchItem == 0)
                    mobType = LatestMobType;
                if (mobType > MobTypes.Normal)
                {
                    //npc.GivenName = mobType.ToString() + " " + Lang.GetNPCName(npc.netID);
                    float HealthBonus = this.HealthBonus;
                    if (Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type])
                        HealthBonus *= 0.5f;
                    if(npc.lifeMax > 5 && npc.type != Terraria.ID.NPCID.MothronEgg) npc.lifeMax = (int)(npc.lifeMax * HealthBonus);
                    if (npc.damage > 0) npc.damage += DamageBonus;
                    npc.defense += DefenseBonus;
                    npc.value *= CoinBonus;
                }
                KbResistance = 1000;
                RechargingKbResist = false;
                npc.knockBackResist -= npc.knockBackResist * 0.1f * (int)mobType;
            }
        }

        private MobTypes IsInvasionMonster(NPC npc)
        {
            if (npc.type == NPCID.DD2Betsy)
                return GetBossDifficulty(npc);
            if (npc.type == NPCID.DD2GoblinT1 || npc.type == NPCID.DD2GoblinT2 || npc.type == NPCID.DD2GoblinT3 || npc.type == NPCID.DD2GoblinBomberT1 || npc.type == NPCID.DD2GoblinBomberT2 || 
                npc.type == NPCID.DD2GoblinBomberT3 || npc.type == NPCID.DD2JavelinstT1 || npc.type == NPCID.DD2JavelinstT2 || npc.type == NPCID.DD2JavelinstT3 || 
                npc.type == NPCID.DD2WyvernT1 || npc.type == NPCID.DD2WyvernT2 || npc.type == NPCID.DD2WyvernT3 || npc.type == NPCID.DD2DarkMageT1 || npc.type == NPCID.DD2DarkMageT3 || 
                npc.type == NPCID.DD2SkeletonT1 || npc.type == NPCID.DD2SkeletonT3 || npc.type == NPCID.DD2KoboldWalkerT2 || npc.type == NPCID.DD2KoboldWalkerT3 || 
                npc.type == NPCID.DD2DrakinT2 || npc.type == NPCID.DD2DrakinT3 || npc.type == NPCID.DD2WitherBeastT2 || npc.type == NPCID.DD2WitherBeastT3 || 
                npc.type == NPCID.DD2OgreT2 || npc.type == NPCID.DD2OgreT3 || npc.type == NPCID.DD2LightningBugT3 || npc.type == NPCID.DD2EterniaCrystal)
            {
                return GetBossDifficulty(npc, -1);
            }
            return MobTypes.Normal;
        }

        public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
        {
            int SpawnDifficulty = 0;
            bool HasTitan = false;
            byte GuardianCount = 0;
            foreach (TerraGuardian guardian in player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Where(x => x.Active))
            {
                GuardianCount++;
                SpawnDifficulty += guardian.LifeFruitHealth * 3 + guardian.LifeCrystalHealth;
                //if (guardian.HasFlag(GuardianFlags.TitanGuardian))
                //    HasTitan = true;
            }
            const int MaxLifeCrystalBoost = 15 * 3, MaxLifeFruitBoost = 20, TotalHealthBoost = MaxLifeCrystalBoost + MaxLifeFruitBoost;
            if (HasTitan)
            {
                SpawnDifficulty += TotalHealthBoost * 3;
            }
            float SpawnChanceBooster = (float)SpawnDifficulty / TotalHealthBoost;
            MobTypes HighestLevelMobType = MobTypes.Normal;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && !Main.npc[n].friendly)
                {
                    if (Main.npc[n].modNPC != null && Main.npc[n].GetGlobalNPC<NpcMod>().mobType > HighestLevelMobType)
                        HighestLevelMobType = Main.npc[n].GetGlobalNPC<NpcMod>().mobType;
                }
            }
            LatestMobType = MobTypes.Normal;
            if (HighestLevelMobType < MobTypes.Epic && SpawnDifficulty >= TotalHealthBoost * 5 && (SpawnChanceBooster - 5) * 0.01f >= Main.rand.Next(32)) // && Main.rand.Next(3600) < SpawnDifficulty - TotalHealthBoost * 5)
            {
                LatestMobType = MobTypes.Epic;
            }
            else if (HighestLevelMobType < MobTypes.Legendary && SpawnDifficulty >= TotalHealthBoost * 4 && (SpawnChanceBooster - 4) * 0.125f >= Main.rand.NextDouble() * (18))
            {
                LatestMobType = MobTypes.Legendary;
            }
            else if (HighestLevelMobType < MobTypes.Champion && SpawnDifficulty >= TotalHealthBoost * 3 && (SpawnChanceBooster - 3) * 0.25f >= Main.rand.NextDouble() * (12))
            {
                LatestMobType = MobTypes.Champion;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 2 && (SpawnChanceBooster - 2) * 0.5f >= Main.rand.NextDouble() * (8))
            {
                LatestMobType = MobTypes.Elite;
            }
            else if ((GuardianCount > 1 || HasTitan) && SpawnDifficulty >= MaxLifeCrystalBoost * 0.3f && SpawnChanceBooster >= Main.rand.NextDouble() * (4))
            {
                LatestMobType = MobTypes.Veteran;
            }
        }

        public static MobTypes GetBossDifficulty(NPC npc, int DifficultyMod = 0)
        {
            int SpawnDifficulty = 0;
            bool HasTitanGuardian = false;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    foreach (TerraGuardian guardian in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Where(x => x.Active))
                    {
                        SpawnDifficulty += guardian.LifeFruitHealth * 3 + guardian.LifeCrystalHealth;
                        if (guardian.HasFlag(GuardianFlags.TitanGuardian))
                            HasTitanGuardian = true;
                    }
                }
            }
            const int MaxLifeCrystalBoost = 15 * 3, MaxLifeFruitBoost = 20, TotalHealthBoost = MaxLifeCrystalBoost + MaxLifeFruitBoost;
            /*if (HasTitanGuardian)
            {
                SpawnDifficulty += TotalHealthBoost * 4;
            }*/
            //float SpawnChanceBooster = (float)SpawnDifficulty / TotalHealthBoost;
            MobTypes mobType = MobTypes.Normal;
            if (SpawnDifficulty >= TotalHealthBoost * 10) // && Main.rand.Next(3600) < SpawnDifficulty - TotalHealthBoost * 5)
            {
                mobType = MobTypes.Epic;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 8)
            {
                mobType = MobTypes.Legendary;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 6)
            {
                mobType = MobTypes.Champion;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 4)
            {
                mobType = MobTypes.Elite;
            }
            else if (SpawnDifficulty >= MaxLifeCrystalBoost * 2)
            {
                mobType = MobTypes.Veteran;
            }
            if (mobType > MobTypes.Normal)
            {
                if (npc.type == Terraria.ID.NPCID.MartianSaucerCore || npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking || npc.type == NPCID.Everscream || 
                    npc.type == NPCID.SantaNK1 || npc.type == NPCID.IceQueen)
                {
                    mobType = mobType - 1;
                }
            }
            DifficultyMod += (int)mobType;
            if (DifficultyMod >= 0 && DifficultyMod < Enum.GetValues(typeof(MobTypes)).Length)
            {
                mobType = (MobTypes)DifficultyMod;
            }
            return mobType;
        }

        public static int GetGuardianNPC(int GuardianID, Mod mod)
        {
            return GetGuardianNPC(GuardianID, mod.Name);
        }
        
        public static int GetGuardianNPC(int GuardianID, string ModID = "")
        {
            int Pos = -1;
            if(ModID == "")
                ModID = MainMod.mod.Name;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).GuardianID == GuardianID &&
                    ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).GuardianModID == ModID)
                {
                    Pos = n;
                    break;
                }
            }
            return Pos;
        }

        public override bool PreDraw(NPC npc, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<Buffs.Love>()) && Main.rand.Next(15) == 0)
            {
                Vector2 Velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                Velocity.Normalize();
                Velocity.X *= 0.66f;
                int gore = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width + 1), Main.rand.Next(npc.height + 1)), Velocity * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[gore].sticky = false;
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.Y -= 0.6f;
            }
            if (TrappedCatKingSlime == npc.whoAmI)
            {
                Main.ninjaTexture = MainMod.TrappedCatTexture;
            }
            return true;
        }

        public override void PostDraw(NPC npc, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            if (TrappedCatKingSlime == npc.whoAmI)
            {
                Main.ninjaTexture = MainMod.NinjaTextureBackup;
            }
        }

        public override bool CheckActive(NPC npc)
        {
            if (npc.target > -1)
            {
                Player p = Main.player[npc.target];
                Vector2 NpcCenter = npc.Center;
                foreach (TerraGuardian g in p.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (g.Active && !g.Downed)// && g.TakesAggro(npc.Center))
                    {
                        Vector2 GuardianCenter = g.CenterPosition;
                        if (!npc.townNPC && g.Position.X >= NpcCenter.X - NPC.sWidth * 0.5f && g.Position.X < NpcCenter.X + NPC.sWidth * 0.5f &&
                            GuardianCenter.Y >= NpcCenter.Y - NPC.sHeight * 0.5f && GuardianCenter.Y < NpcCenter.Y + NPC.sHeight * 0.5f)
                        {
                            npc.timeLeft = NPC.activeTime;
                            g.ActiveNpcs += npc.npcSlots;
                        }
                        /*else if (npc.townNPC && g.Position.X >= NpcCenter.X - NPC.sWidth && g.Position.X < NpcCenter.X + NPC.sWidth &&
                            GuardianCenter.Y >= NpcCenter.Y - NPC.sHeight && GuardianCenter.Y < NpcCenter.Y + NPC.sHeight)
                        {
                            g.TownNpcs += npc.npcSlots;
                        }*/
                    }
                }
            }
            return true;
        }

        public static bool HasGuardianNPC(int GuardianID, Mod mod)
        {
            return HasGuardianNPC(GuardianID, mod.Name);
        }

        public static bool HasGuardianNPC(int GuardianID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return GetGuardianNPC(GuardianID, ModID) > -1;
        }

        public static int GetMaleCompanionNPCCount()
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).Guardian.Male)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static int GetFemaleCompanionNPCCount()
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && !((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).Guardian.Male)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static int GetTerraGuardianNPCCount()
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).Guardian.Base.IsTerraGuardian)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static int GetTerrarianNPCCount()
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).Guardian.Base.IsTerrarian)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static int GetGroupNPCCount(string GroupID)
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab && ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC).Guardian.GroupID == GroupID)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static int GetCompanionNPCCount()
        {
            int Count = 0;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab)
                {
                    Count++;
                }
            }
            return Count;
        }

        public static string GetGuardianNPCName(int GuardianID, Mod mod)
        {
            return GetGuardianNPCName(GuardianID, mod.Name);
        }

        public static string GetGuardianNPCName(int GuardianID, string ModID = "")
        {
            int Pos = GetGuardianNPC(GuardianID, ModID);
            if (Pos > -1)
            {
                return ((GuardianNPC.GuardianNPCPrefab)Main.npc[Pos].modNPC).Guardian.Name;
            }
            return "";
        }

        public static void MaskGuardianPositionToPlayers(NPC npc)
        {
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    //if (PlayerPositionBackup[p] != Vector2.Zero)
                    //Main.player[p].position = PlayerPositionBackup[p];
                    PlayerPositionBackup[p] = Main.player[p].position;
                    PlayerDeadStatusBackup[p] = Main.player[p].dead;
                    PlayerWetBackup[p] = Main.player[p].wet;
                    if (npc.type != ModContent.NPCType<Npcs.ZombieGuardian>() && npc.type != ModContent.NPCType<Npcs.BlueNPC>() && npc.type != ModContent.NPCType<Npcs.BrutusNPC>() && npc.type != ModContent.NPCType<Npcs.AlexNPC>() && npc.type != ModContent.NPCType<Npcs.VladimirNPC>() && npc.type != ModContent.NPCType<GuardianNPC.List.BearNPC>())
                    {
                        TerraGuardian PlayerGuardian = null;
                        float LowestAggroCount = (Main.player[p].Center - npc.Center).Length() - Main.player[p].aggro;
                        foreach (TerraGuardian g in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                        {
                            if (g.Active && !g.Downed && !g.KnockedOutCold)
                            {
                                float AggroRange = (g.CenterPosition - npc.Center).Length() - g.Aggro;
                                if (AggroRange < LowestAggroCount)
                                {
                                    LowestAggroCount = AggroRange;
                                    PlayerGuardian = g;
                                }
                            }
                        }
                        if (PlayerGuardian != null)
                        {
                            Main.player[p].position = PlayerGuardian.TopLeftPosition;
                            Main.player[p].dead = PlayerGuardian.Downed || PlayerGuardian.KnockedOutCold;
                            Main.player[p].wet = PlayerGuardian.Wet;
                        }
                        else
                        {
                            PlayerPositionBackup[p] = Vector2.Zero;
                            if (Main.player[p].GetModPlayer<PlayerMod>().KnockedOutCold)
                                Main.player[p].dead = true;
                        }
                    }
                    else
                    {
                        PlayerPositionBackup[p] = Vector2.Zero;
                    }
                }
            }
        }

        public static void RestorePlayersPosition()
        {
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    if (PlayerPositionBackup[p] != Vector2.Zero)
                    {
                        Main.player[p].position = PlayerPositionBackup[p];
                        Main.player[p].wet = PlayerWetBackup[p];
                    }
                    Main.player[p].dead = PlayerDeadStatusBackup[p];
                    PlayerPositionBackup[p] = Vector2.Zero;
                }
            }
        }

        public static void AddGuardianMet(int ID, string ModID = "")
        {
            if (!WorldMod.GuardiansMet.Any(x => x.Key == ID && x.Value == ModID))
            {
                WorldMod.GuardiansMet.Add(new KeyValuePair<int, string>(ID, ModID));
                WorldMod.AllowGuardianNPCToSpawn(ID, ModID);
                //Not only send the info on multiplayer about the guardian met, but also tell everyone that it has been found.
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == Terraria.ID.NPCID.Dryad)
            {
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.RenameCard>());
            }
            if (type == Terraria.ID.NPCID.PartyGirl)
            {
                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Misc.BirthdayPresent>());
            }
            if (type == Terraria.ID.NPCID.Merchant)
            {
                if(MainMod.UsingGuardianNecessitiesSystem)
                    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.FirstAidKit>());
            }
        }
        
        public static bool HasMetGuardian(int ID, Mod mod)
        {
            return HasGuardianNPC(ID, mod.Name);
        }

        public static bool HasMetGuardian(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return WorldMod.GuardiansMet.Any(x => x.Key == ID && x.Value == ModID);
        }

        public override void OnHitNPC(NPC npc, NPC target, int damage, float knockback, bool crit)
        {
            if (npc.townNPC)
            {
                //Try getting nearest Guardian to help repel the attacker.
                int NearestGuardian = -1;
                float NearestDistance = 768f;
                for (int n = 0; n < 200; n++)
                {
                    if (n != npc.whoAmI && Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab)
                    {
                        float Distance = Main.npc[n].Distance(npc.Center);
                        if (NearestDistance < Distance)
                        {
                            NearestGuardian = n;
                            NearestDistance = Distance;
                        }
                    }
                }
                if (NearestGuardian > -1)
                {
                    TerraGuardian guardian = ((GuardianNPC.GuardianNPCPrefab)Main.npc[NearestGuardian].modNPC).Guardian;
                    if (!guardian.IsAttackingSomething)
                    {
                        guardian.TargetID = npc.whoAmI;
                        guardian.AttackingTarget = true;
                        guardian.TargetType = TerraGuardian.TargetTypes.Npc;
                        guardian.DisplayEmotion(TerraGuardian.Emotions.Alarmed);
                    }
                }
            }
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            //base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
            if (!MainMod.UseNewMonsterModifiersSystem)
            {
                TerraGuardian[] guardians = player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers;
                foreach (TerraGuardian g in guardians)
                {
                    if (g.Active)
                        maxSpawns += 3;
                }
            }
            if (player.GetModPlayer<PlayerMod>().KnockedOut)
                maxSpawns = 0;
        }

        public override void OnChatButtonClicked(NPC npc, bool firstButton)
        {
            if (npc.type == 22 && firstButton && Main.rand.NextDouble() < 0.333)
            {
                switch (Main.rand.Next(16))
                {
                    case 0:
                        Main.npcChatText = "You can give orders to your summoned TerraGuardians by holding " + MainMod.orderCallButton.GetAssignedKeys() + ".";
                        break;
                    case 1:
                        Main.npcChatText = "You can modify how they will behave in combat, by checking the brain icon bellow your inventory. Each tab allows you to modify a specific behavior of the guardian, and some new behaviors may appear as their friendship level increases.";
                        break;
                    case 2:
                        Main.npcChatText = "The Dryad can give you tips on how you can find new TerraGuardians. She seems to have more knowledge about this than me.";
                        break;
                    case 3:
                        Main.npcChatText = "It is said that another player can control the TerraGuardian, possibly by just pressing Start button on the controller, but I'm not sure if It will work.";
                        break;
                    case 4:
                        Main.npcChatText = "Even if you don't want to have the guardian following you, they can be helpful If you need someone to protect your town, by placing them in strategic points.";
                        break;
                    case 5:
                        Main.npcChatText = "That cat, Sardine, isn't it? Sometimes he gives his spare fishing rewards as bounty rewards.";
                        break;
                    case 6:
                        Main.npcChatText = "The TerraGuardians will try to using the best weapon of each kind they have in the inventory, and they will priorize using Magic Weapons over other weapon types. And will try using Ranged attacks when their target is away. Just beware, they may not know how to use some weapons.";
                        break;
                    case 7:
                        Main.npcChatText = "Placing some vanity gears in one of the first 10 guardian inventory slots may allow It to use them as vanity gear. Most likelly helmets will bit on this.";
                        break;
                    case 8:
                        Main.npcChatText = "If you mount your guardian for mobility, you may want to disable the 2 handed weapon usage when mounted, on their behavior, that may hinder you when trying to move some place If there's creatures in the way.";
                        break;
                    case 9:
                        Main.npcChatText = "TerraGuardians can use magic mirrors, and they may even allow you to order them to do some things that requires them, like selling the items in their inventory, If you have enough friendship level.";
                        break;
                    case 10:
                        Main.npcChatText = "If your guardian can sell loots for you, be sure to mark as favorite the items they shouldn't try selling, in their inventory.";
                        break;
                    case 11:
                        Main.npcChatText = "There are guardians that allows you to mount on them, and some that mounts on your back. Guardians mounted on your back allows you to use your mount, while being mounted on the guardian, allows you to control of their mobility.";
                        break;
                    case 12:
                        Main.npcChatText = "There is a speed penalty that is applied when mounting, or having a guardian mounted on your back, but It's so low that is about trivial.";
                        break;
                    case 13:
                        Main.npcChatText = "As their friendship level increases, they will care more about you, trying to heal you when you are hurt, or even protect you in combat when you are away.";
                        break;
                    case 14:
                        Main.npcChatText = "Order your guardian to go in front of you, If you want them to protect you from the dangers that may come ahead of you. Be sure to give them good speed accessories too.";
                        break;
                    case 15:
                        Main.npcChatText = "You can share your mount with your TerraGuardian, your mount may not like it, though.";
                        break;
                }
            }
            else
            {
                base.OnChatButtonClicked(npc, firstButton);
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if (Main.rand.NextDouble() >= 0.25)
                return;
            List<string> PossibleMessages = new List<string>();
            switch (npc.type)
            {
                case Terraria.ID.NPCID.Guide:
                    if (HasGuardianNPC(0))
                        PossibleMessages.Add("Have you seen " + GetGuardianNPCName(0) + "? I think he just stole my Guide book to prank on me.");
                    if (HasGuardianNPC(2))
                        PossibleMessages.Add(GetGuardianNPCName(2) + " comes frequently to me, asking if there is any tough creature he can take down.");
                    if (HasGuardianNPC(5))
                    {
                        if (Main.rand.Next(2) == 0)
                            PossibleMessages.Add("You want to know about " + AlexRecruitScripts.AlexOldPartner + "? Sorry, I don't know that person.");
                        else
                            PossibleMessages.Add(GetGuardianNPCName(5) + " has been a positive addition to the town. But I wonder who cleans up his mess.");
                    }
                    if (HasGuardianNPC(8))
                    {
                        PossibleMessages.Add("I keep dismissing " + GetGuardianNPCName(8) + ", she distracts me.");
                    }
                    if (HasGuardianNPC(GuardianBase.Vladimir))
                    {
                        PossibleMessages.Add("I've been hearing good things about " + GetGuardianNPCName(GuardianBase.Vladimir) + ", It seems like he's been helping several people with their problems.");
                    }
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add("Since you've found " + GetGuardianNPCName(GuardianBase.Michelle) + ", I wonder If there are other people going to join your travels.");
                    }
                    if (HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("D-d-did you s-see " + GetGuardianNPCName(GuardianBase.Malisha) + " with a doll that looks like me?");
                    }
                    break;
                case Terraria.ID.NPCID.Nurse:
                    {
                        PossibleMessages.Add("I can heal try healing your companions too, but I will charge more for that.");
                        if (HasGuardianNPC(GuardianBase.Vladimir) && HasGuardianNPC(GuardianBase.Blue) && NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                        {
                            PossibleMessages.Add("I've got some good tips of things I could do on my date with " + NPC.firstNPCName(Terraria.ID.NPCID.ArmsDealer) + " from " + GetGuardianNPC(1) + ". She said that her boyfriend fell for her after she did that, so what could go wrong?! The only weird thing is the method she used.");
                        }
                    }
                    break;
                case Terraria.ID.NPCID.ArmsDealer:
                    if (HasGuardianNPC(0))
                        PossibleMessages.Add("I tried to teach " + GetGuardianNPCName(0) + " how to use a gun, he nearly shot my head off. Never. Again.");
                    if (HasGuardianNPC(2))
                        PossibleMessages.Add(GetGuardianNPCName(2) + " comes at me frequently with very absurdly overpowered weapon requests, from rocket launchers to sniper rifles. Seriously, where would I get those?");
                    if (HasGuardianNPC(6))
                        PossibleMessages.Add("What is " + GetGuardianNPCName(6) + "'s skin made of? He asked me to shot him for training and he just stud still, like as If nothing happened.");
                    if (HasGuardianNPC(7))
                        PossibleMessages.Add("Can you believe that " + GetGuardianNPCName(7) + " had the audacity of coming to MY STORE, and saying that my weapons are DATED?");
                    if (HasGuardianNPC(8))
                    {
                        PossibleMessages.Add("Everyone keeps staring at " + GetGuardianNPCName(8) + ", but something on me wants to shoot her, instead.");
                    }
                    if (HasGuardianNPC(9))
                    {
                        PossibleMessages.Add("Finally, a worthy TerraGuardian. " + GetGuardianNPCName(9) + " can be quite of a business partner for me.");
                    }
                    if (HasGuardianNPC(GuardianBase.Vladimir) && HasGuardianNPC(GuardianBase.Blue) && NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                    {
                        PossibleMessages.Add("Ack!! You scared me! Can you tell me what is wrong with "+NPC.firstNPCName(Terraria.ID.NPCID.Nurse)+"? We were going on a date, until she pounced on me on the table and tried to bite my arm! I ran as fast as I could after that!");
                    }
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add(GetGuardianNPCName(GuardianBase.Michelle) + " came here earlier looking for a gun. I asked If she wanted a pistol or a machinegun. She said that wanted a rocket launcher.");
                    }
                    break;
                case Terraria.ID.NPCID.Truffle:
                    if (HasGuardianNPC(1))
                        PossibleMessages.Add("Seeing " + GetGuardianNPCName(1) + " makes me feel Blue. That is the logic.");
                    if (HasGuardianNPC(2))
                        PossibleMessages.Add(GetGuardianNPCName(2) + " said that talking to me was making him feel like being under catnip effect.");
                    if (HasGuardianNPC(5))
                        PossibleMessages.Add("A long time ago I've met " + GetGuardianNPCName(5) + " and " + AlexRecruitScripts.AlexOldPartner + ". They were exploring the caverns when they found the town I lived. People was overjoyed when they discovered that they didn't went there to eat them.");
                    break;
                case Terraria.ID.NPCID.Stylist:
                    if (HasGuardianNPC(1))
                        PossibleMessages.Add("If you want me to do your hair, you will have to wait a day or two for my arms to rest, because " + GetGuardianNPCName(1) + " wanted me to do her hair, but do you know how much hair she has?");
                    if (HasGuardianNPC(7))
                        PossibleMessages.Add("I feel pitty of " + GetGuardianNPCName(7) + ", she asks me to do her hair, but she nearly has any, so I pretend that I'm doing something.");
                    if (HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("I think I could do some hair work on " + GetGuardianNPCName(GuardianBase.Malisha) + ".");
                    }
                    break;
                case Terraria.ID.NPCID.Mechanic:
                    if (HasGuardianNPC(0))
                    {
                        PossibleMessages.Add("If you manage to see " + GetGuardianNPCName(0) + " again, tell him to stop pressing my switches?");
                        PossibleMessages.Add(GetGuardianNPCName(0) + " asked me if I could make him a giant robot, but ever since that dungeon incident, I say: No more.");
                    }
                    if (HasGuardianNPC(2))
                        PossibleMessages.Add("I always love it when " + GetGuardianNPCName(2) + " comes to visit me. I can't wait for the next time.");
                    if (HasGuardianNPC(11))
                    {
                        PossibleMessages.Add("(She's humming. Something must have brightened her day.)");
                        PossibleMessages.Add("I love having "+GetGuardianNPCName(11) + " around, he always gives me a happiness boost. I want to be his friend forever.");
                    }
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add("I think " + GetGuardianNPCName(GuardianBase.Michelle) + " must have some kind of trigger. She can't see any kind of switch, she wants to flip them.");
                    }
                    break;
                case Terraria.ID.NPCID.GoblinTinkerer:
                    if (HasGuardianNPC(2) && NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
                    {
                        PossibleMessages.Add("That creepy cat keeps visiting " + NPC.firstNPCName(Terraria.ID.NPCID.Mechanic) + " from time to time, If I could fake out an accident to stop him from...");
                    }
                    if (HasGuardianNPC(11))
                    {
                        if (HasGuardianNPC(2))
                            PossibleMessages.Add("Oh boy... First that creepy cat appeared, now a giant bear?! That will kill my chances with " + NPC.firstNPCName(Terraria.ID.NPCID.Mechanic) + "...");
                        PossibleMessages.Add("Why " + NPC.firstNPCName(Terraria.ID.NPCID.Mechanic) + " always comes radiating happiness from " + NpcMod.GetGuardianNPCName(GuardianBase.Vladimir) + "?");
                    }
                    break;
                case Terraria.ID.NPCID.Steampunker:
                    if (HasGuardianNPC(0))
                        PossibleMessages.Add("I asked " + GetGuardianNPCName(0) + " the other day to test my newest jetpack. He flew with it, then lost control while in the air and then fell in a lake. Now he wants to know when's the next test.");
                    break;
                case Terraria.ID.NPCID.WitchDoctor:
                    if (HasGuardianNPC(0))
                        PossibleMessages.Add(GetGuardianNPCName(0) + " asked me how to use a blowpipe, then I tried teaching him. He nearly died out of suffocation because inhaled the seed.");
                    if (HasGuardianNPC(1))
                        PossibleMessages.Add(GetGuardianNPCName(1) + " seems to have some interest in poisons, just watch out if she gives you a drink. Who knows?");
                    if (HasGuardianNPC(8))
                        PossibleMessages.Add("Red paint? I didn't painted my mask red... Uh oh... Forget what you saw.");
                    break;
                case Terraria.ID.NPCID.DD2Bartender:
                    PossibleMessages.Add("Bringing a TerraGuardian to the defense of the crystal is a bit of a cheat, but It is very helpful when defending it alone.");
                    if (HasGuardianNPC(6))
                        PossibleMessages.Add(GetGuardianNPCName(6) + " is my best client, he always drinks about 10~15 mugs of Ale before returning to his post. He still looks fine afterwards, I guess.");
                    if (HasGuardianNPC(9))
                        PossibleMessages.Add("I always have to watch over " + GetGuardianNPCName(9) + ", because he doesn't knows when to stop drinking.");
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add(GetGuardianNPCName(GuardianBase.Michelle) + " got angry when I asked her age when she asked for a drink.");
                    }
                    break;
                case Terraria.ID.NPCID.Merchant:
                    if (HasGuardianNPC(0))
                    {
                        PossibleMessages.Add(GetGuardianNPCName(0) + " keeps coming to me asking if I have Giant Healing Potions, but I have no idea of where I could find such a thing.");
                        PossibleMessages.Add("Have you found someone to take care of the trash? Every morning, I check my trash can and It's clean.");
                    }
                    if (HasGuardianNPC(1))
                        PossibleMessages.Add(GetGuardianNPCName(1) + " is one of my best clients, she buys a new Shampoo flask every day.");
                    if (HasGuardianNPC(5))
                        PossibleMessages.Add(AlexRecruitScripts.AlexOldPartner + " you say? She used to buy pet food for " + GetGuardianNPCName(5) + " from me. She really wasn't into talking with people, most of the time you saw her with " + GetGuardianNPCName(5) + ".");
                    if (HasGuardianNPC(7))
                        PossibleMessages.Add("What " + GetGuardianNPCName(7) + " expects of my store? My products have quality and I get them in high stack.");
                    if (HasGuardianNPC(GuardianBase.Vladimir))
                        PossibleMessages.Add("What are you saying?! I never complained to anyone about my sales!");
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add("Everytime " + GetGuardianNPCName(GuardianBase.Michelle) + " returns from her travels, she buys several stacks of potions. I think she's not very good at adventuring.");
                    }
                    break;
                case Terraria.ID.NPCID.TravellingMerchant:
                    if (HasGuardianNPC(7) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add(GetGuardianNPCName(1) + " really loves when I bring products from the land that produces the Sake. She says that reminds her of home.");
                    if (HasGuardianNPC(8) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("I've got a huge increase in tissues sale latelly, It seems like there's an incidence of nose bleeding around this place...? I hope isn't contagious.");
                    if (HasGuardianNPC(9) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("That " + GetGuardianNPCName(9) + " is such a jester, he thinks my products are trash.");
                    break;
                case Terraria.ID.NPCID.Dryad:
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            PossibleMessages.Add("I forgot to mention about the TerraGuardians, they are mythical creatures that live in this world. They may be willing to help you on your adventure, if you manage to find them.");
                            break;
                        case 1:
                            PossibleMessages.Add("The TerraGuardians have no problems with humans, at least most of them don't. But If you manage to be a good friend to them, they will retribute the favor.");
                            break;
                        case 2:
                            PossibleMessages.Add("It's weird the fact that the TerraGuardians started to appear right now... That may mean that something big is about to happen...");
                            break;
                        case 3:
                            PossibleMessages.Add("Try befriending as many TerraGuardians as possible, no one knows when we may end up needing their help.");
                            break;
                        case 4:
                            PossibleMessages.Add("Do you ask yourself why you can understand what some guardians says without saying a word? It's easy, once they meet someone, they create a bond with it, so they not only can express themselves, but also understand what the other wants.");
                            break;
                        case 5:
                            PossibleMessages.Add("There are two realms in this world, the Terra realm, and the Ether realm. You live in the Terra realm, the TerraGuardians lives on the Ether realm, but It is really weird to see them here in Terra.");
                            break;
                        case 6:
                            PossibleMessages.Add("The TerraGuardians grows stronger when they travel with you, they may get stronger on many of their characteristics depending on what they do during your travels.");
                            break;
                    }
                    if (true)
                    {
                        if (!HasMetGuardian(0) && NPC.downedBoss1)
                        {
                            PossibleMessages.Add("I have seen a guardian earlier this day, he's looking for a town with friendly people to live with. It may still be around this world, If you want to let him live here.");
                        }
                        if (!HasMetGuardian(3))
                        {
                            if (HasMetGuardian(1))
                            {
                                PossibleMessages.Add("There was a Bloodmoon which I couldn't forget, I was on the far lands of the world, being swarmed by zombies, until a big wolf zombie appeared. That zombie, looked a lot like " + GetGuardianNPCName(1) + ".");
                            }
                            else
                            {
                                PossibleMessages.Add("There was a Bloodmoon which I couldn't forget, I was on the far lands of the world, being swarmed by zombies, until a big wolf zombie appeared.");
                            }
                        }
                        else if (!HasMetGuardian(2))
                        {
                            PossibleMessages.Add("There was that cat... I forgot his name... He told me that was going to pursue his biggest bounty: The King Slime. I wonder If he were successfull.");
                        }
                        else if (!HasMetGuardian(7))
                        {
                            PossibleMessages.Add("I've bumped during my travels with a cat who was looking for her husband. She's been travelling world by world looking for him, and she looked a bit worn out the last time I saw her. She could try getting some place to rest for a while before continuing her search. Would you help her, If you bump into her?");
                        }
                        if (!HasMetGuardian(1))
                        {
                            PossibleMessages.Add("I've once met a Guardian that liked camping, If you place a campfire somewhere, It may show up.");
                        }
                        if (!HasMetGuardian(0))
                        {
                            PossibleMessages.Add("There is a TerraGuardian looking for a town with people to live with. He may end up showing up here anytime.");
                        }
                        if (!HasMetGuardian(5))
                        {
                            PossibleMessages.Add("There is a story about a Giant Dog TerraGuardian and a Terrarian Woman, they lived happy and played together everyday, until one day she died, and that dog guards her tombstone since she was buried.\nI don't know if the legends are true, but nothing stops you from finding that out.");
                        }
                        if (!HasMetGuardian(6))
                        {
                            if (Npcs.BrutusNPC.ChanceCounter() >= Npcs.BrutusNPC.ProgressCountForBrutusToAppear)
                            {
                                PossibleMessages.Add("I've heard stories of an old Royal Guard from the Ether Realm who lost his job, and is now roaming through worlds to work as a bodyguard for anyone who hurts him, or pay for his job. I think the chance of him appearing will increase if you overcome challenges on this world.");
                            }
                            else
                            {
                                PossibleMessages.Add("I've heard stories of an old Royal Guard from the Ether Realm that lost his job, and now is now roaming through worlds to work as a bodyguard for someone. I think he visits worlds where a number of challenges have been overcome. I'm not sure If he would visit this world.");
                            }
                        }
                        if (!HasMetGuardian(8) && Npcs.MabelNPC.CanSpawnMabel)
                        {
                            PossibleMessages.Add("I've met a Guardian that was trying to fly like a reindeer. The problem is that she's not a reindeer. I don't think that It will end in a good thing.");
                        }
                        if (!HasMetGuardian(9) && Npcs.DominoNPC.CanSpawnDomino(Main.player[Main.myPlayer]))
                        {
                            PossibleMessages.Add("There is a shady Guardian wandering around the world. It looks like he's running from something. I think It's a good idea to ask him what is up.");
                        }
                        if (!HasMetGuardian(10) && Npcs.LeopoldNPC.CanSpawnLeopold)
                        {
                            PossibleMessages.Add("Hey, did you hear? A sage from the Ether Realm is exploring the Terra Realm. You may bump into him anytime! Can I go with you on your adventures?! I'm kind of his fan.");
                        }
                        if (!HasMetGuardian(11) && Npcs.VladimirNPC.CanRecruitVladimir)
                        {
                            PossibleMessages.Add("I heard a rummor about a Bear creature travelling around this world, I didn't believed at first because the one who told me about that, said that the bear were wanting to give her a hug. The person said that the bear was found in the Jungle, before running away in fear. Maybe It were looking for honey?");
                        }
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 3))
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                PossibleMessages.Add("You said that when [gn:1] spoke to [gn:3] and then he remember who he was? It seems like at that moment of the battle where he was weakened, the bond with her has strengthened, recovering his senses.");
                                break;
                            case 1:
                                PossibleMessages.Add("Actually, I guess I remember vaguelly that Terrarian who [gn:3] was following, It seems he had the same goal as you. But he wanted to try exploring the powers of the evil to save the world. I wonder what happened to him?");
                                break;
                        }
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 5))
                    {
                        PossibleMessages.Add("Oh, you're the TerraGuardian of the tale? You says that her name was " + AlexRecruitScripts.AlexOldPartner + "? I guess I remember her, I actually guided her on the beggining of her adventure, but didn't heard about her after that.");
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 6))
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                PossibleMessages.Add("The TerraGuardians are leaving the Ether Realm? That is really strange. That didn't happened for over 5000 years. I didn't even existed at the time it happened.");
                                break;
                            case 1:
                                PossibleMessages.Add("If you really like the TerraGuardians, then the recent events will certainly be good for you. The only problem is that the fact they are moving from the Ether Realm isn't a good sign.");
                                break;
                            case 2:
                                PossibleMessages.Add("No matter what happens, try recruiting as many TerraGuardians as possible. We will need their help in the future, if my guess is right.");
                                break;
                        }
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 10))
                    {
                        PossibleMessages.Add("Wait... You're... AAAAAAAAAHHHH!!! PLEASE!! GIVE ME YOUR AUTOGRAPH!!! AAAAAAAAAAAAAHHHHH!!!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Leopold) && NpcMod.HasGuardianNPC(GuardianBase.Bree))
                    {
                        PossibleMessages.Add("Even though " + NpcMod.GetGuardianNPCName(GuardianBase.Leopold) + " has said something stupid during the Popularity Contest, I'm still his number one fan! " + NpcMod.GetGuardianNPCName(GuardianBase.Leopold)+ "! I love you!!");
                    }
                    if (NpcMod.HasGuardianNPC(11))
                    {
                        PossibleMessages.Add("I don't know the reason, but " + NpcMod.GetGuardianNPCName(11) + " gets extremelly aggressive during Bloodmoons. I recommend you to avoid contact with him during those events.");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("I know trouble when I see one, and that one is named " + NpcMod.GetGuardianNPCName(GuardianBase.Malisha) + ".");
                    }
                    break;
                case Terraria.ID.NPCID.Angler:
                    if (HasGuardianNPC(0) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("If I see " + GetGuardianNPCName(0) + " again, I will skin him alive, because he keeps stealing all my fish!");
                    if (HasGuardianNPC(2) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("Did " + GetGuardianNPCName(2) + " told you what is his trick for catching more than one fish at a time?");
                    if (HasGuardianNPC(8) && Main.rand.NextDouble() < 0.5)
                    {
                        switch(Main.rand.Next(5)){
                            case 0:
                                PossibleMessages.Add(GetGuardianNPCName(8) + " keeps trying to give me vegetables to eat, I don't need vegetables, I have fish!");
                                break;
                            case 1:
                                PossibleMessages.Add("Can't " + GetGuardianNPCName(8) + " just stop telling me to clean my room? She isn't my mom, neither I do need one!");
                                break;
                            case 2:
                                PossibleMessages.Add("Eugh, I'm clean. " + GetGuardianNPCName(8) + " gave me a bath, and removed my fish stench. I'm even hurting in some parts of my body, because the smell wasn't going away.");
                                break;
                            case 3:
                                PossibleMessages.Add(GetGuardianNPCName(8) + " surelly don't want to let me in peace. She always asks how's my day, or asks how I'm feeling.");
                                break;
                            case 4:
                                PossibleMessages.Add("Every night, " + GetGuardianNPCName(8) + " tells me bedtime stories for me to sleep.");
                                break;
                        }
                    }
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add("If you don't watch out, " + GetGuardianNPCName(GuardianBase.Michelle) + " may take your place as my minion. She's probably waiting for that Sextant.");
                    }
                    break;
                case Terraria.ID.NPCID.TaxCollector:
                    if (HasGuardianNPC(0))
                        PossibleMessages.Add("I tried to collect the rent from " + GetGuardianNPCName(0) + ", he gave me a pile of trash. What is that supposed to mean?");
                    if (HasGuardianNPC(1))
                        PossibleMessages.Add("I tried to collect the rent from " + GetGuardianNPCName(1) + ", and she tried to stab me with her sword! Do something about your tenants.");
                    if (HasGuardianNPC(2))
                        PossibleMessages.Add("Hey! You, look at this. Do you see this wound? That happened when I tried to collect " + GetGuardianNPCName(2) + "'s rent. Now you have to pay for this.");
                    if (HasGuardianNPC(3))
                        PossibleMessages.Add("I don't think I and " + GetGuardianNPCName(3) + " are speaking in the same language, I asked him for GOLDS, and he VOMITED in my shoes, and you don't want to know the kinds of things that were in it. I had to throw my shoes away and brush VERY hard my feet.");
                    if (HasGuardianNPC(4))
                        PossibleMessages.Add("I have a job for you, go talk to " + GetGuardianNPCName(4) + " and collect from him my rent. What? I collect the rent? Are you mad?");
                    if (HasGuardianNPC(5))
                        PossibleMessages.Add("You owe me a new cane. I tried to collect the rent from " + GetGuardianNPCName(5) + " and he tried to bite me! Good thing that my cane was in the way, but now It's nearly breaking.");
                    if (HasGuardianNPC(6))
                        PossibleMessages.Add("Ow-ow ow ow. That brute lion you hired has hit my head with his huge paw when I asked him for the rent. Didn't he noticed that I'm a fragile old man?");
                    if (HasGuardianNPC(7))
                        PossibleMessages.Add("What is the problem with your Ether Realm tenants? That psichotic white cat you found tried hit me with a rolling pin when I tried to collect her rent. She even chased me about 50 meters while swinging that thing!");
                    if (HasGuardianNPC(8))
                    {
                        PossibleMessages.Add("Ah? Nothing, nothing! Uh... Don't tell anyone that you saw me leaving " + GetGuardianNPCName(8) + "'s room, and DARE YOU to comment about the blood coming from my nose!");
                    }
                    if (HasGuardianNPC(9))
                        PossibleMessages.Add("Where were you?! " + GetGuardianNPCName(9) + " nearly sent me back to hell just because I wanted to collect his rent!");
                    if (HasGuardianNPC(10))
                        PossibleMessages.Add("You're saying you didn't have seen me in a long time? Of course! That crazy bunny " + GetGuardianNPCName(10) + " turned me into a frog! All I did was use my methods of asking for rent!");
                    if (HasGuardianNPC(11))
                        PossibleMessages.Add("Do you want to talk? Just stay away from me! I need as little physical contact as possible. I tried collecting " + GetGuardianNPCName(11) + "'s rent earlier, and I ended up being hugged for several hours. I nearly wet my pants because of that.");
                    if (HasGuardianNPC(GuardianBase.Michelle))
                    {
                        PossibleMessages.Add("That girl is the devil! " + GetGuardianNPCName(GuardianBase.Michelle) + " nearly dropped me into a lava pit because I tried to collect her rent!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("Missed me? It's because " + NpcMod.GetGuardianNPCName(GuardianBase.Malisha) + " turned me into a frog, and has placed me inside a cage for hours! I had only flies to eat meanwhile, FLIES!");
                    }
                    break;
                case Terraria.ID.NPCID.PartyGirl:
                    if (HasGuardianNPC(6) && Main.rand.Next(2) == 0)
                        PossibleMessages.Add("Looks like " + GetGuardianNPCName(6) + " flourishes when It's his birthday party. He enjoys the most of the special day, I say.");
                    break;
                case Terraria.ID.NPCID.Wizard:
                    if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("I'm really glad of meeting some one as enthusiast of magic as me, I would have " + NpcMod.GetGuardianNPCName(GuardianBase.Malisha) + " as my apprentice If I had met her earlier.");
                        PossibleMessages.Add(NpcMod.GetGuardianNPCName(GuardianBase.Malisha) + "s researches have quite some interesting results, but some of them are extremelly volatile.");
                    }
                    break;
            }
            if (PossibleMessages.Count > 0)
            {
                chat = PossibleMessages[Main.rand.Next(PossibleMessages.Count)];
            }
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == Terraria.ID.NPCID.PossessedArmor && !NpcMod.HasMetGuardian(4) && Main.rand.Next(100) == 0)
            {
                int npcpos = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<GuardianNPC.List.ArmorWraith>());
                Main.NewText("There's something over there.");
                NpcMod.AddGuardianMet(4);
            }
            if (npc.type == Terraria.ID.NPCID.Bunny && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Accessories.BunnyFoot>());
            }
            if (npc.type == Terraria.ID.NPCID.GoblinWarrior && Main.rand.Next(100) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Accessories.ProvocationBadge>());
            }
            if (npc.type == Terraria.ID.NPCID.WallofFlesh)
                MainMod.LastWof = false;
            bool SomeGuardianHurt = false, SomeGuardianNeedMana = false;
            for (int p = 0; p < 255; p++)
            {
                if (npc.playerInteraction[p] && Main.player[p].active)
                {
                    SomeGuardianHurt = Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && !x.Downed && x.HP < x.MHP);
                    SomeGuardianNeedMana = Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && !x.Downed && x.MP < x.MMP);
                    if (p == Main.myPlayer)
                    {
                        foreach (GuardianData d in Main.player[p].GetModPlayer<PlayerMod>().GetGuardians())
                        {
                            if (!d.request.Active)
                                continue;
                            if (d.request.CountObjective(d, Main.player[p]))
                            {
                                if (npc.realLife > -1)
                                {
                                    d.request.OnMobKill(d, Main.npc[npc.realLife]);
                                }
                                else
                                {
                                    d.request.OnMobKill(d, npc);
                                }
                            }
                        }
                    }
                }
            }
            if (SomeGuardianHurt && Main.rand.Next(12) == 0)
            {
                int ItemID = Terraria.ID.ItemID.Heart;
                if (Main.halloween)
                    ItemID = Terraria.ID.ItemID.CandyApple;
                if (Main.xMas)
                    ItemID = Terraria.ID.ItemID.CandyCane;
                Item.NewItem(npc.getRect(), ItemID, 1);
            }
            if (SomeGuardianNeedMana && Main.rand.Next(2) == 0)
            {
                int ItemID = Terraria.ID.ItemID.Star;
                if (Main.halloween)
                    ItemID = Terraria.ID.ItemID.SoulCake;
                if (Main.xMas)
                    ItemID = Terraria.ID.ItemID.SugarPlum;
                Item.NewItem(npc.getRect(), ItemID, 1);
            }
            if (TrappedCatKingSlime == npc.whoAmI)
            {
                int npcpos = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<GuardianNPC.List.CatGuardian>());
                ((GuardianNPC.GuardianNPCPrefab)Main.npc[npcpos].modNPC).Guardian.AddBuff(Terraria.ID.BuffID.Slimed, 300);
                TrappedCatKingSlime = -1;
            }
            if (GuardianBountyQuest.TargetMonsterID > 0)
            {
                if (npc.whoAmI == GuardianBountyQuest.TargetMonsterSpawnPosition)
                {
                    GuardianBountyQuest.OnBountyMonsterKilled(npc);
                }
                else
                {
                    GuardianBountyQuest.OnMobKilled(npc);
                }
            }
            if (mobType > MobTypes.Normal)
                StrongMonsterLoot(npc);
        }

        public void StrongMonsterLoot(NPC npc)
        {
            if (npc.type == Terraria.ID.NPCID.EaterofWorldsBody || npc.type == Terraria.ID.NPCID.EaterofWorldsHead || npc.type == Terraria.ID.NPCID.EaterofWorldsTail)
            {
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && (Main.npc[n].type == Terraria.ID.NPCID.EaterofWorldsBody || Main.npc[n].type == Terraria.ID.NPCID.EaterofWorldsHead))
                    {
                        return;
                    }
                }
            }
            List<KeyValuePair<int, float>> ExtraLootChances = new List<KeyValuePair<int, float>>();
            Action<int, int> DropLoot = delegate(int ID, int Stack)
            {
                Item.NewItem(npc.getRect(), ID, Stack);
            };
            switch (npc.type)
            {
                case 4:
                    if (WorldGen.crimson)
                    {
                        DropLoot(Terraria.ID.ItemID.CrimtaneOre, Main.rand.Next(15, 26) * (int)mobType);
                    }
                    else
                    {
                        DropLoot(Terraria.ID.ItemID.DemoniteOre, Main.rand.Next(15, 26) * (int)mobType);
                    }
                    break;
                case 13:
                case 14:
                case 15:
                    DropLoot(Terraria.ID.ItemID.DemoniteOre, Main.rand.Next(120, 181) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.ShadowScale, Main.rand.Next(50, 71) * (int)mobType);
                    break;
                case 267:
                    DropLoot(Terraria.ID.ItemID.CrimtaneOre, Main.rand.Next(120, 181) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.TissueSample, Main.rand.Next(50, 71) * (int)mobType);
                    break;
                case 35:
                    DropLoot(Terraria.ID.ItemID.Bone, Main.rand.Next(30, 41) * (int)mobType);
                    break;
                case 50:
                    ExtraLootChances.Add(new KeyValuePair<int, float>(256, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(257, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(258, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(2430, 25f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(2585, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(2610, 66.67f));
                    break;
                case 222:
                    DropLoot(Terraria.ID.ItemID.BeeWax, Main.rand.Next(20, 31) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.Beenade, Main.rand.Next(10, 21) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.BottledHoney, 5 * (int)mobType);
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.BeeGun, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.BeeKeeper, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.BeesKnees, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.HoneyComb, 33.33f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Nectar, 6.7f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.HoneyedGoggles, 5f));
                    break;
                case 113:
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.BreakerBlade, 16.67f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.ClockworkAssaultRifle, 16.67f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.LaserRifle, 16.67f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.WarriorEmblem, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.RangerEmblem, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.SorcererEmblem, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.SummonerEmblem, 12.5f));
                    break;
                case 125:
                case 126:
                    DropLoot(Terraria.ID.ItemID.SoulofSight, Main.rand.Next(5, 11) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.HallowedBar, Main.rand.Next(10, 21) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 127:
                    DropLoot(Terraria.ID.ItemID.SoulofFright, Main.rand.Next(5, 11) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.HallowedBar, Main.rand.Next(10, 21) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 134:
                case 135:
                case 136:
                    DropLoot(Terraria.ID.ItemID.SoulofMight, Main.rand.Next(5, 11) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.HallowedBar, Main.rand.Next(10, 21) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 262:
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Seedling, 5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.TheAxe, 2f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.PygmyStaff, 25f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.GrenadeLauncher, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.VenusMagnum, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.NettleBurst, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.LeafBlower, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.FlowerPow, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.WaspGun, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Seedler, 14.29f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.ThornHook, 10f));
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 245:
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Stynger, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.PossessedHatchet, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.SunStone, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.EyeoftheGolem, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Picksaw, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.HeatRay, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.StaffofEarth, 12.5f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.GolemFist, 12.5f));
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.BeetleHusk, 3 * (int)mobType);
                    break;
                case 370:
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.FishronWings, 6.66f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.BubbleGun, 20f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Flairon, 20f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.RazorbladeTyphoon, 20f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.TempestStaff, 20f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Tsunami, 20f));
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 439:
                    DropLoot(Terraria.ID.ItemID.GreaterHealingPotion, 5 * (int)mobType);
                    break;
                case 398:
                    DropLoot(Terraria.ID.ItemID.LunarOre, Main.rand.Next(35, 61) * (int)mobType);
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Meowmere, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.Terrarian, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.StarWrath, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.SDMG, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(3546, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.LastPrism, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.LunarFlareBook, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(Terraria.ID.ItemID.RainbowCrystalStaff, 11.11f));
                    ExtraLootChances.Add(new KeyValuePair<int, float>(3569, 11.11f));
                    break;
            }
            if (ExtraLootChances.Count > 0)
            {
                float MaxChance = ExtraLootChances.Sum(x => x.Value);
                for (int ExtraLoot = 0; ExtraLoot < (int)mobType; ExtraLoot++)
                {
                    if (mobType == MobTypes.Veteran && Main.rand.NextDouble() < 0.6)
                        break;
                    if (ExtraLoot > 0 && Main.rand.NextDouble() < (int)mobType * 0.1)
                        continue;
                    float PickedChance = Main.rand.NextFloat() * MaxChance;
                    float Stack = 0;
                    foreach (KeyValuePair<int, float> kvp in ExtraLootChances)
                    {
                        if (PickedChance >= Stack && PickedChance < kvp.Value + Stack)
                        {
                            DropLoot(kvp.Key, 1);
                            break;
                        }
                        Stack += kvp.Value;
                    }
                }
            }
            if (Main.hardMode && npc.target > -1)
            {
                if (Main.player[npc.target].ZoneRockLayerHeight || Main.player[npc.target].ZoneUnderworldHeight)
                {
                    if ((Main.player[npc.target].ZoneCorrupt || Main.player[npc.target].ZoneCrimson) && Main.rand.Next(5) == 0)
                    {
                        DropLoot(Terraria.ID.ItemID.SoulofNight, (int)mobType);
                    }
                    if (Main.player[npc.target].ZoneHoly && Main.rand.Next(5) == 0)
                    {
                        DropLoot(Terraria.ID.ItemID.SoulofLight, (int)mobType);
                    }
                }
            }
        }

        public override void PostAI(NPC npc)
        {
            LatestMobType = MobTypes.Normal;
            if (npc.type == Terraria.ID.NPCID.NebulaHeadcrab && npc.ai[0] == 5)
            {
                if (PlayerPositionBackup[npc.target] != Vector2.Zero && Main.player[npc.target].HasBuff(163))
                    Main.player[npc.target].DelBuff(Main.player[npc.target].FindBuffIndex(163));
            }
            if ((npc.type == Terraria.ID.NPCID.DD2WitherBeastT2 || npc.type == Terraria.ID.NPCID.DD2WitherBeastT3) && npc.ai[0] == 1)
            {
                if ((Main.player[Main.myPlayer].Center - npc.Center).Length() > 400 && Main.player[npc.target].HasBuff(195))
                    Main.player[npc.target].DelBuff(Main.player[npc.target].FindBuffIndex(195));
            }
            RestorePlayersPosition();
            if (MainMod.UseNewMonsterModifiersSystem)
            {
                if (npc.damage > 0) npc.damage += DamageBonus;
                if (RechargingKbResist)
                {
                    KbResistance++;
                    if (KbResistance >= 1000)
                    {
                        RechargingKbResist = false;
                        KbResistance = 1000;
                    }
                }
                else
                {
                    npc.defense += DefenseBonus;
                }
            }
        }

        public override bool CheckDead(NPC npc)
        {
            if (npc.life < 1)
            {
                RestorePlayersPosition();
                TriggerHandler.FireNpcDeathTrigger(npc.Center, npc);
            }
            return base.CheckDead(npc);
        }

        public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            if (MainMod.UseNewMonsterModifiersSystem)
            {
                damage -= (int)(damage * (int)(mobType) * 0.1f);
                if (mobType > MobTypes.Elite)
                {
                    if (!RechargingKbResist)
                    {
                        int KnockbackDamage = (int)Math.Max(1, (damage - defense) / 2);
                        if(crit)
                            KnockbackDamage *= 2;
                        if (npc.ichor)
                            KnockbackDamage += (int)(KnockbackDamage * .2f);
                        if (npc.betsysCurse)
                            KnockbackDamage += (int)(KnockbackDamage * 0.3f);
                        KbResistance -= (short)KnockbackDamage;
                        if (KbResistance <= 0)
                        {
                            RechargingKbResist = true;
                            knockback *= 1.5f;
                            CombatText.NewText(npc.getRect(), Color.Silver, "Broken!", true);
                        }
                        else
                        {
                            damage -= (int)((damage - defense) * 0.8f);
                            knockback = 0;
                        }
                    }
                    else
                    {

                    }
                }
            }
            if (damage > 0 && npc.life > 0)
            {
                TriggerHandler.FireNpcHurtTrigger(npc.Center, npc, (int)damage, crit);
            }
            return true;
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            if (npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Elite)
            {
                Vector2 KbBarPos = position - Main.screenPosition;
                KbBarPos.Y += 18;
                const int BarMaxWidth = 72;
                KbBarPos.X -= BarMaxWidth * 0.5f;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)KbBarPos.X - 2, (int)KbBarPos.Y - 2, BarMaxWidth + 4, 4 + 4), Color.Black);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)KbBarPos.X, (int)KbBarPos.Y, (int)(BarMaxWidth * ((float)KbResistance / 1000)), 4), (RechargingKbResist ? Color.Red : Color.LightGoldenrodYellow));
            }
            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (npc.realLife == -1 && npc.GetGlobalNPC<NpcMod>().mobType > MobTypes.Normal)
            {
                Vector2 TextPosition = npc.Top - Main.screenPosition;
                TextPosition.Y -= 22f;
                //Utils.DrawBorderString(Main.spriteBatch, npc.GetGlobalNPC<NpcMod>().mobType.ToString(), TextPosition, Color.White, 1f, 0.5f);
                int MaxGems = (int)npc.GetGlobalNPC<NpcMod>().mobType;
                float RotationSum = MathHelper.ToRadians(360) / (float)(MaxGems);
                for (int g = 0; g < MaxGems; g++)
                {
                    Vector2 GemPos = TextPosition + (MaxGems > 1 ?  (RotationSum * g).ToRotationVector2() * 16 : Vector2.Zero);
                    Main.spriteBatch.Draw(Main.gemTexture[g], GemPos, null, drawColor, 0f, new Vector2(16, 32), 0.5f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
                }
            }
        }
    }

    public enum MobTypes : byte
    {
        Normal,
        Veteran,
        Elite,
        Champion,
        Legendary,
        Epic
    }
}
