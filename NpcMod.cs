﻿using System;
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
        public static byte SpawnPlayerCharacter = 0;

        public static bool TryPlacingCatGuardianOnSlime = false;
        public static int TrappedCatKingSlime = -1;
        public static Vector2[] PlayerPositionBackup = new Vector2[256];
        public static bool?[] PlayerDeadStatusBackup = new bool?[256];
        public static bool[] PlayerWetBackup = new bool[256];
        public MobTypes mobType = MobTypes.Normal;
        public short KbResistance = 1000;
        public bool RechargingKbResist = false;
        public static MobTypes LatestMobType = MobTypes.Normal;
        public byte ShardDebuffCount = 0;
        public static bool HasBossSpawned = false;

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
                        return 2;
                    case MobTypes.Elite:
                        return 4;
                    case MobTypes.Champion:
                        return 6;
                    case MobTypes.Legendary:
                        return 8;
                    case MobTypes.Epic:
                        return 10;
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

        public static bool RecruitNpcSpawnConditionCheck(NPCSpawnInfo spawninfo)
        {
            return Main.tile[spawninfo.spawnTileX, spawninfo.spawnTileY].wall == 0 || Lighting.Brightness(spawninfo.spawnTileX, spawninfo.spawnTileY) >= 0.3f;
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (MainMod.BlackedOut)
            {
                pool.Clear();
                return;
            }
            /*if(Main.rand.Next(5) == 0 && !NpcMod.HasGuardianNPC(GuardianBase.Castella))
            {
                NpcMod.SpawnGuardianNPC(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16, GuardianBase.Castella);
            }*/
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
                                guardian.AddBuff(BuffID.SoulDrain, 5, true);
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

        public static void DespawnGuardianNPC(TerraGuardian tg)
        {
            DespawnGuardianNPC(tg.ID, tg.ModID);
        }

        public static void DespawnGuardianNPC(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            for(int i = 0; i < WorldMod.GuardianTownNPC.Count; i++)
            {
                if (WorldMod.GuardianTownNPC[i].MyID.IsSameID(ID, ModID))
                    WorldMod.GuardianTownNPC.RemoveAt(i);
            }
        }

        public static TerraGuardian SpawnGuardianNPC(TerraGuardian tg, bool ForceMove = false)
        {
            return SpawnGuardianNPC(tg.Position.X, tg.Position.Y, tg.ID, tg.ModID, ForceMove);
        }

        public static TerraGuardian SpawnGuardianNPC(float X, float Y, int ID, string ModID = "", bool ForceMove = false) //Will try spawning if there is no other of his type existing
        {
            TerraGuardian tg = null;
            foreach (TerraGuardian g in WorldMod.GuardianTownNPC)
            {
                if (g.ID == ID && g.ModID == ModID)
                {
                    tg = g;
                    break;
                }
            }
            if (tg == null && Main.netMode == 0)
            {
                foreach (TerraGuardian g in MainMod.ActiveGuardians.Values)
                {
                    if (g.ID == ID && g.ModID == ModID)
                    {
                        tg = g;
                        break;
                    }
                }
            }
            if (tg == null)
            {
                tg = new TerraGuardian(ID, ModID);
                tg.Position.X = X;
                tg.Position.Y = Y;
                tg.Active = true;
                tg.DoUpdateGuardianStatus();
                tg.EnforceScale();
            }
            else if (ForceMove)
            {
                tg.Position.X = X;
                tg.Position.Y = Y;
            }
            tg.SetFallStart();
            WorldMod.AddTownGuardianNpc(tg);
            return tg;
        }

        public override bool PreAI(NPC npc)
        {
            if(npc.soulDrain)
                UpdateSoulDrain(npc);
            if (npc.type == NPCID.KingSlime && TryPlacingCatGuardianOnSlime)
            {
                if (!HasMetGuardian(2))
                    TrappedCatKingSlime = npc.whoAmI;
                TryPlacingCatGuardianOnSlime = false;
            }
            if (TrappedCatKingSlime == npc.whoAmI && npc.type != NPCID.KingSlime)
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
            if (npc.HasBuff(BuffID.Dazed))
            {
                switch (npc.aiStyle) //https://terraria.fandom.com/wiki/AI
                {
                    case 1:
                    case 3:
                    case 8:
                    case 13:
                    case 16:
                    case 18:
                    case 19:
                    case 22:
                    case 23:
                    case 25:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                        return false;
                }
            }
            return base.PreAI(npc);
        }

        public override void ResetEffects(NPC npc)
        {
            ShardDebuffCount = 0;
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
            if (MainMod.MobHealthBoostPercent > 0 && npc.lifeMax > 5 && npc.catchItem == 0 && Main.netMode == 0 && !Main.gameMenu)
            {
                Player player = Main.player[SpawnPlayerCharacter];
                int MyGuardians = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetSummonedGuardianCount;
                Vector2 Position = player.Center;
                for(byte i = 0; i < 255; i++)
                {
                    if(i != SpawnPlayerCharacter && Main.player[i].active && Math.Abs(Main.player[i].Center.X - Position.X) < 1000 && Math.Abs(Main.player[i].Center.Y - Position.Y) < 600)
                    {
                        MyGuardians += Main.player[i].GetModPlayer<PlayerMod>().GetSummonedGuardianCount;
                    }
                }
                if(MyGuardians > 1)
                {
                    npc.lifeMax += (int)(npc.lifeMax * MainMod.MobHealthBoostPercent * (MyGuardians - 1));
                }
            }
            if (MainMod.UseNewMonsterModifiersSystem)
            {
                if (npc.boss || NPCID.Sets.TechnicallyABoss[npc.type] || npc.type == NPCID.Sharkron || npc.type == NPCID.Sharkron2)
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
                    if (npc.lifeMax > 5 && npc.type != NPCID.MothronEgg)
                    {
                        try
                        {
                            npc.lifeMax = checked((int)(npc.lifeMax * HealthBonus));
                        }
                        catch
                        {
                            npc.lifeMax = int.MaxValue;
                        }
                    }
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
            Vector2 CharacterPos = player.Center;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active && (p == player.whoAmI || (Math.Abs(CharacterPos.X - Main.player[p].Center.X) < 1000 && 
                    Math.Abs(CharacterPos.Y - Main.player[p].Center.Y) < 800)))
                {
                    foreach (TerraGuardian guardian in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                    {
                        if (!guardian.Active)
                            continue;
                        GuardianCount++;
                        SpawnDifficulty += guardian.LifeFruitHealth * 3 + guardian.LifeCrystalHealth;
                        //if (guardian.HasFlag(GuardianFlags.TitanGuardian))
                        //    HasTitan = true;
                    }
                }
            }
            const int MaxLifeCrystalBoost = 15, MaxLifeFruitBoost = 20, TotalHealthBoost = MaxLifeCrystalBoost + MaxLifeFruitBoost;
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
            if (GuardianCount > 1 || HasTitan)
            {
                float Chance = Main.rand.NextFloat();
                if (HighestLevelMobType < MobTypes.Epic && SpawnDifficulty >= TotalHealthBoost * 2.5f && SpawnChanceBooster * (1f / 1024f) >= Chance) // && Main.rand.Next(3600) < SpawnDifficulty - TotalHealthBoost * 5)
                {
                    LatestMobType = MobTypes.Epic;
                }
                else if (HighestLevelMobType < MobTypes.Legendary && SpawnDifficulty >= TotalHealthBoost * 2 && SpawnChanceBooster * (1f / 256) >= Chance)
                {
                    LatestMobType = MobTypes.Legendary;
                }
                else if (HighestLevelMobType < MobTypes.Champion && SpawnDifficulty >= TotalHealthBoost * 1.5f && SpawnChanceBooster * (1f / 32) >= Chance)
                {
                    LatestMobType = MobTypes.Champion;
                }
                else if (SpawnDifficulty >= TotalHealthBoost && SpawnChanceBooster * (1f / 16) >= Chance)
                {
                    LatestMobType = MobTypes.Elite;
                }
                else if (SpawnDifficulty >= MaxLifeCrystalBoost * 0.3f && SpawnChanceBooster * (1f / 4) >= Chance)
                {
                    LatestMobType = MobTypes.Veteran;
                }
            }
        }

        public static MobTypes GetBossDifficulty(NPC npc, int DifficultyMod = 0)
        {
            int SpawnDifficulty = 0;
            //bool HasTitanGuardian = false;
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    foreach (TerraGuardian guardian in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Where(x => x.Active))
                    {
                        SpawnDifficulty += guardian.LifeFruitHealth * 3 + guardian.LifeCrystalHealth;
                        //if (guardian.HasFlag(GuardianFlags.TitanGuardian))
                        //    HasTitanGuardian = true;
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
            if (SpawnDifficulty >= TotalHealthBoost * 8) // && Main.rand.Next(3600) < SpawnDifficulty - TotalHealthBoost * 5)
            {
                mobType = MobTypes.Epic;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 6)
            {
                mobType = MobTypes.Legendary;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 4)
            {
                mobType = MobTypes.Champion;
            }
            else if (SpawnDifficulty >= TotalHealthBoost * 2)
            {
                mobType = MobTypes.Elite;
            }
            else if (SpawnDifficulty >= MaxLifeCrystalBoost)
            {
                mobType = MobTypes.Veteran;
            }
            if (mobType > MobTypes.Normal)
            {
                if (npc.type == NPCID.MartianSaucerCore || npc.type == NPCID.MourningWood || npc.type == NPCID.Pumpking || npc.type == NPCID.Everscream || 
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
            if (ModID == "")
                ModID = MainMod.mod.Name;
            for (int i = 0; i < WorldMod.GuardianTownNPC.Count; i++ )
            {
                if (WorldMod.GuardianTownNPC[i].MyID.IsSameID(GuardianID, ModID))
                    return i;
            }
            return -1;
        }

        public static TerraGuardian GetGuardianNPCCharacter(int GuardianID, string ModID = "")
        {
            return WorldMod.GuardianTownNPC[GetGuardianNPC(GuardianID, ModID)];
        }

        public static bool IsGuardianPlayerRoomMate(Player player, int GuardianID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            for (int i = 0; i < WorldMod.GuardianTownNPC.Count; i++)
            {
                if (WorldMod.GuardianTownNPC[i].ID == GuardianID && WorldMod.GuardianTownNPC[i].ModID == ModID)
                {
                    return WorldMod.GuardianTownNPC[i].IsPlayerRoomMate(player);
                }
            }
            return false;
        }

        public static List<GuardianDrawData> GuardianPostDrawData = new List<GuardianDrawData>();
        public static bool DrawnCompanionsInFrontOfNpcs = false;

        public override bool PreDraw(NPC npc, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            if (!DrawnCompanionsInFrontOfNpcs)
            {
                DrawnCompanionsInFrontOfNpcs = true;
                TerraGuardian.CurrentDrawnOrderID = -2000;
                WorldMod.DrawTownNpcCompanions(DrawMoment.DrawBeforeDrawingNpcs);
            }

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
            if (GuardianPostDrawData.Count > 0)
                GuardianPostDrawData.Clear();
            foreach (GuardianDrawMoment gdm in MainMod.DrawMoment)
            {
                if (gdm.DrawTargetType == TerraGuardian.TargetTypes.Npc && gdm.DrawTargetID == npc.whoAmI && MainMod.ActiveGuardians.ContainsKey(gdm.GuardianWhoAmID))
                {
                    MainMod.ActiveGuardians[gdm.GuardianWhoAmID].DrawDataCreation();
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                        TerraGuardian.DoDrawCompanionDrawData(gdd, true); //gdd.Draw(Main.spriteBatch);
                    GuardianPostDrawData.AddRange(TerraGuardian.DrawFront);
                }
            }
            return true;
        }

        public override void PostDraw(NPC npc, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            if (TrappedCatKingSlime == npc.whoAmI)
            {
                Main.ninjaTexture = MainMod.NinjaTextureBackup;
            }
            foreach (GuardianDrawData gdd in GuardianPostDrawData)
            {
                gdd.Draw(Main.spriteBatch);
            }
            GuardianPostDrawData.Clear();
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
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (tg.Male)
                    Count++;
            }
            return Count;
        }

        public static int GetFemaleCompanionNPCCount()
        {
            int Count = 0;
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (!tg.Male)
                    Count++;
            }
            return Count;
        }

        public static int GetTerraGuardianNPCCount()
        {
            int Count = 0;
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (tg.Base.IsTerraGuardian)
                    Count++;
            }
            return Count;
        }

        public static int GetTerrarianNPCCount()
        {
            int Count = 0;
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (tg.Base.IsTerrarian)
                    Count++;
            }
            return Count;
        }

        public static int GetGroupNPCCount(string GroupID)
        {
            int Count = 0;
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (tg.GroupID == GroupID)
                    Count++;
            }
            return Count;
        }

        public static int GetCompanionNPCCount()
        {
            return WorldMod.GuardianTownNPC.Count;
        }

        public static string GetGuardianNPCName(int GuardianID, Mod mod)
        {
            return GetGuardianNPCName(GuardianID, mod.Name);
        }

        public static string GetGuardianNPCName(int GuardianID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
            {
                if (tg.ID == GuardianID && tg.ModID == ModID)
                    return tg.Name;
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
                    if (npc.type != ModContent.NPCType<Npcs.ZombieGuardian>() && npc.type != ModContent.NPCType<Npcs.BlueNPC>() && npc.type != ModContent.NPCType<Npcs.BrutusNPC>() && npc.type != ModContent.NPCType<Npcs.AlexNPC>() && npc.type != ModContent.NPCType<Npcs.VladimirNPC>()
                         && npc.type != ModContent.NPCType<Npcs.WrathNPC>() && npc.type != ModContent.NPCType<Npcs.GhostFoxGuardianNPC>() && npc.type != ModContent.NPCType<Npcs.AlexanderNPC>() && npc.type != ModContent.NPCType<Npcs.GlennNPC>())
                    {
                        TerraGuardian PlayerGuardian = null;
                        float LowestAggroCount = (Main.player[p].Center - npc.Center).Length() - Main.player[p].aggro;
                        if (!Main.player[p].dead && Main.player[p].GetModPlayer<PlayerMod>().KnockedOutCold)
                        {
                            Main.player[p].dead = true;
                        }
                        foreach (TerraGuardian g in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                        {
                            if (g.Active && !g.Downed && !g.KnockedOutCold && !g.HasFlag(GuardianFlags.DontTakeAggro) && (!g.HasFlag(GuardianFlags.CantBeKnockedOutCold) || !g.KnockedOut))
                            {
                                float AggroRange = (g.CenterPosition - npc.Center).Length() - g.Aggro;
                                if (g.KnockedOut)
                                    AggroRange += 300;
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
                    }
                    Main.player[p].wet = PlayerWetBackup[p];
                    if(PlayerDeadStatusBackup[p].HasValue)
                        Main.player[p].dead = PlayerDeadStatusBackup[p].Value;
                    PlayerPositionBackup[p] = Vector2.Zero;
                }
            }
        }

        public static void AddGuardianMet(int ID, string ModID = "", bool AllowToMoveIn = true)
        {
            if (!WorldMod.GuardiansMet.Any(x => x.ID == ID && x.ModID == ModID))
            {
                WorldMod.GuardiansMet.Add(new GuardianID(ID, ModID));
                if (AllowToMoveIn)
                {
                    if(WorldMod.HasCompanionMetSomeoneWithHighFriendshipLevel(ID, ModID))
                    {
                        WorldMod.AllowGuardianNPCToSpawn(ID, ModID);
                    }
                }
                //Not only send the info on multiplayer about the guardian met, but also tell everyone that it has been found.
            }
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Dryad:
                    {
                        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.RenameCard>(), true);
                    }
                    break;
                case NPCID.PartyGirl:
                    {
                        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Misc.BirthdayPresent>(), true);
                    }
                    break;
                case NPCID.Merchant:
                    {
                        if (MainMod.UsingGuardianNecessitiesSystem)
                            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Consumable.FirstAidKit>(), true);
                        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Misc.BirthdayPresent>(), true);
                    }
                    break;
                case NPCID.Cyborg:
                    {
                        if (!Main.halloween && PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Alex))
                            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Outfit.Alex.AlexModel3000TurquoiseShark>(), true);
                    }
                    break;
                case NPCID.Mechanic:
                    {
                        if (!Main.halloween && PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Alex))
                            shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Outfit.Alex.AlexModel3000TurquoiseShark>(), true);
                    }
                    break;
                case NPCID.Clothier:
                    {
                        if (PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Bree))
                        {
                            if (Main.dayTime && !Main.raining && Main.moonPhase % 4 < 2)
                            {
                                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Outfit.Bree.DamselOutfit>(), true);
                            }
                            if (!Main.halloween)
                            {
                                shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Items.Outfit.Bree.WitchOutfit>(), true);
                            }
                        }
                    }
                    break;
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
            return WorldMod.GuardiansMet.Any(x => x.ID == ID && x.ModID == ModID);
        }

        public override void OnHitPlayer(NPC npc, Player target, int damage, bool crit)
        {
            if(GuardianBountyQuest.TargetMonsterSpawnPosition == npc.whoAmI)
            {
                GuardianBountyQuest.OnBountyMonsterHitPlayer(target);
            }
        }

        public override void OnHitNPC(NPC npc, NPC target, int damage, float knockback, bool crit)
        {
            if (npc.townNPC)
            {
                //Try getting nearest Guardian to help repel the attacker.
                float NearestDistance = 270f;
                TerraGuardian guardian = null;
                foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                {
                    if (tg.OwnerPos == -1 && !tg.IsNpcHostile(target))
                    {
                        float Distance = npc.Distance(tg.CenterPosition);
                        if (NearestDistance < Distance)
                        {
                            guardian = tg;
                            NearestDistance = Distance;
                        }
                    }
                }
                if (guardian != null)
                {
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
            SpawnPlayerCharacter = (byte)player.whoAmI;
            if (MainMod.HavingMoreCompanionsIncreasesSpawnRate)
            {
                TerraGuardian[] guardians = player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers;
                int Count = 0;
                foreach (TerraGuardian g in guardians)
                {
                    if (g.Active)
                    {
                        maxSpawns += 3;
                        Count++;
                    }
                }
                if(Count > 1)
                {
                    Count--;
                    spawnRate -= (int)(Count / (Count + 5) * spawnRate);
                }
            }
            if (player.GetModPlayer<PlayerMod>().KnockedOut)
                maxSpawns = 0;
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if(ShardDebuffCount > 0)
            {
                damage += (int)((npc.defense * 0.5f) * ShardDebuffCount * 0.15f);
            }
            if (!MainMod.DisableDamageReductionByNumberOfCompanions)
            {
                float DamageMod = player.GetModPlayer<PlayerMod>().DamageMod;
                damage = (int)(damage * DamageMod);
            }
            if(GuardianBountyQuest.TargetMonsterID == npc.whoAmI)
            {
                byte DamageType = 0;
                if (item.ranged)
                    DamageType = 1;
                if (item.magic)
                    DamageType = 2;
                if (item.summon)
                    DamageType = 3;
                GuardianBountyQuest.ModifyBountyMonsterHitByPlayerAttack(player, DamageType, ref damage, ref knockback, ref crit);
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (!projectile.hostile && !MainMod.DisableDamageReductionByNumberOfCompanions)
            {
                float DamageMod = Main.player[projectile.owner].GetModPlayer<PlayerMod>().DamageMod;
                damage = (int)(damage * DamageMod);
            }
            if (GuardianBountyQuest.TargetMonsterID == npc.whoAmI && !projectile.hostile)
            {
                if (ProjMod.GuardianProj.ContainsKey(projectile.whoAmI))
                {
                    byte DamageType = 0;
                    if (projectile.ranged)
                        DamageType = 1;
                    if (projectile.magic)
                        DamageType = 2;
                    if (projectile.minion)
                        DamageType = 3;
                    GuardianBountyQuest.ModifyBountyMonsterHitByGuardianAttack(ProjMod.GuardianProj[projectile.whoAmI], DamageType, ref damage, ref knockback, ref crit);
                }
                else
                {
                    byte DamageType = 0;
                    if (projectile.ranged)
                        DamageType = 1;
                    if (projectile.magic)
                        DamageType = 2;
                    if (projectile.minion)
                        DamageType = 3;
                    GuardianBountyQuest.ModifyBountyMonsterHitByPlayerAttack(Main.player[projectile.owner], DamageType, ref damage, ref knockback, ref crit);
                }
            }
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
            foreach(QuestData qd in PlayerMod.GetPlayerQuestDatas(Main.LocalPlayer))
            {
                if (!qd.IsInvalid)
                {
                    QuestBase.Data = qd;
                    string Text = qd.GetBase.QuestNpcDialogue(npc);
                    if(Text != "")
                    {
                        chat = Text;
                        return;
                    }
                }
            }
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
                            PossibleMessages.Add("I've got some good tips of things I could do on my date with " + NPC.firstNPCName(Terraria.ID.NPCID.ArmsDealer) + " from " + WorldMod.GuardianTownNPC[GetGuardianNPC(1)].Name + ". She said that her boyfriend fell for her after she did that, so what could go wrong?! The only weird thing is the method she used.");
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
                    if (HasGuardianNPC(7))
                        PossibleMessages.Add(GetGuardianNPCName(1) + " really loves when I bring products from the land that produces the Sake. She says that reminds her of home.");
                    if (HasGuardianNPC(8))
                        PossibleMessages.Add("I've got a huge increase in tissues sale latelly, It seems like there's an incidence of nose bleeding around this place...? I hope isn't contagious.");
                    if (HasGuardianNPC(9))
                        PossibleMessages.Add("That " + GetGuardianNPCName(9) + " is such a jester, he thinks my products are trash.");
                    if (!HasGuardianNPC(GuardianBase.Cinnamon))
                    {
                        PossibleMessages.Add("There is a TerraGuardian that follows me into my travels, she keeps me company when moving from a world to another. She may pop up any time soon.");
                    }
                    else
                    {
                        if (WorldMod.CanGuardianNPCSpawnInTheWorld(GuardianBase.Cinnamon))
                        {
                            PossibleMessages.Add("I quite miss that TerraGuardian company on my travels, now they look a bit more boring. What is her name, by the way?");
                        }
                        else
                        {
                            PossibleMessages.Add("You know that TerraGuardian that arrived with me? She follows me during my travels. At least travelling isn't so lonely with her around.");
                        }
                    }
                    break;
                case Terraria.ID.NPCID.Dryad:
                    switch (Main.rand.Next(7))
                    {
                        case 0:
                            PossibleMessages.Add("I forgot to mention about the TerraGuardians, they are mythical creatures that live in another realm. They may be willing to help you on your adventure, if you manage to find them.");
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
                            PossibleMessages.Add("Do you ask yourself why you can understand what some guardians says without saying a word? It's easy. Once they meet someone, they create a bond with it, so they not only can express themselves, and also understand what the other wants.");
                            break;
                        case 5:
                            PossibleMessages.Add("There are two realms in this world, the Terra realm, and the Ether realm. You live in the Terra realm, the TerraGuardians lives on the Ether realm, but It is really weird to see them here in Terra.");
                            break;
                        case 6:
                            PossibleMessages.Add("The TerraGuardians grows stronger when they travel with you, they may get stronger on many of their characteristics depending on what they do during your travels.");
                            break;
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 3))
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                PossibleMessages.Add("You said that when [gn:1] spoke to [gn:3] and then he remember who he was? It seems like at that moment of the battle where he was weakened, the bond with her has strengthened, making him recover his senses.");
                                break;
                            case 1:
                                PossibleMessages.Add("Actually, I guess I remember vaguelly that Terrarian who [gn:3] was following, It seems he had the same goal as you. But he wanted to try exploring the powers of the evil to save the world. I wonder what happened to him?");
                                break;
                        }
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 5))
                    {
                        PossibleMessages.Add("Oh, you're the TerraGuardian of the tale? You says that her name was " + AlexRecruitScripts.AlexOldPartner + "? I think I remember her, I actually aided her on the beggining of her adventure, years ago, but I didn't heard about her after that.");
                    }
                    if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 6))
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                PossibleMessages.Add("The TerraGuardians are leaving the Ether Realm? That is really strange. That didn't happened in a long time. I didn't even existed at the time they used to live here.");
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
                        PossibleMessages.Add("Even though " + NpcMod.GetGuardianNPCName(GuardianBase.Leopold) + " has said something stupid during the Popularity Contest, I'm still her number one fan! " + NpcMod.GetGuardianNPCName(GuardianBase.Leopold)+ "! I love you!!");
                    }
                    if (NpcMod.HasGuardianNPC(11))
                    {
                        PossibleMessages.Add("I don't know the reason, but " + NpcMod.GetGuardianNPCName(11) + " gets extremelly aggressive during Bloodmoons. I recommend you to avoid contact with him during those events.");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
                    {
                        PossibleMessages.Add("I know trouble when I see one, and that one is named " + NpcMod.GetGuardianNPCName(GuardianBase.Malisha) + ".");
                    }
                    if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().HasGhostFoxHauntDebuff)
                    {
                        PossibleMessages.Add("You seems to be haunted by a ghost, gladly that one only needs your help. If you ask for It to show you visions of what is haunting It, you can find clues on how to lift the haunt.");
                        PossibleMessages.Add("I don't know if you know, but there is a giant guardian ghost on your back. That one seems to be wanting your help. If you ask about what's haunting It, you will know how to lift the haunt.");
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
                    if (NpcMod.HasGuardianNPC(GuardianBase.Wrath))
                    {
                        PossibleMessages.Add("The next time I need to collect the rent from " + NpcMod.GetGuardianNPCName(GuardianBase.Wrath) + ", YOU DO THAT! I think he even broke some of my bones!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Alexander))
                    {
                        PossibleMessages.Add("Don't bother me, my back is aching right now! Everytime I visit " + NpcMod.GetGuardianNPCName(GuardianBase.Alexander) + ", he jumps on me and drops me on my back on the floor!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Fluffles))
                    {
                        PossibleMessages.Add("I tried collecting " + NpcMod.GetGuardianNPCName(GuardianBase.Fluffles) + " rent earlier. I didn't found her, until someone saw her on my shoulder. What is her problem?!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Minerva))
                    {
                        PossibleMessages.Add("I charge less rent from " + NpcMod.GetGuardianNPCName(GuardianBase.Minerva) + ". She's the only person who treats me right, and also cooks something whenever I visit.");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Liebre))
                    {
                        PossibleMessages.Add("What? Are you nuts? Trying to collect rent from " + NpcMod.GetGuardianNPCName(GuardianBase.Liebre) + " is like asking to be sent to afterlife!");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Glenn))
                    {
                        PossibleMessages.Add("Kid or not, " + NpcMod.GetGuardianNPCName(GuardianBase.Glenn) + " has also to pay for the rent.");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.CaptainStench))
                    {
                        PossibleMessages.Add(NpcMod.GetGuardianNPCName(GuardianBase.CaptainStench) + " nearly splitted me in half when I tried collecting her rent.");
                    }
                    if (NpcMod.HasGuardianNPC(GuardianBase.Cinnamon))
                    {
                        PossibleMessages.Add("Can you tell "+NpcMod.GetGuardianNPCName(GuardianBase.Cinnamon) + " to stop tossing pies on my face when I try collecting her rent?");
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

        public override bool PreNPCLoot(NPC npc)
        {
            if (npc.type == NPCID.PossessedArmor && !HasMetGuardian(4) && Main.rand.Next(100) == 0)
            {
                SpawnGuardianNPC(npc.Center.X, npc.Bottom.Y, GuardianBase.Nemesis);
                Main.NewText("The wraith stayed after you broke its armor.", MainMod.MysteryCloseColor);
                AddGuardianMet(4);
            }
            Npcs.GhostFoxGuardianNPC.OnMobKill(npc.type);
            for (int p = 0; p < 255; p++)
            {
                if (npc.playerInteraction[p] && Main.player[p].active)
                {
                    if (p == Main.myPlayer)
                    {
                        foreach (GuardianData d in Main.player[p].GetModPlayer<PlayerMod>().GetGuardians())
                        {
                            if (!d.request.Active)
                                continue;
                            d.request.Base.OnKillMob(npc, d.request);
                        }
                    }
                }
            }
            if (TrappedCatKingSlime == npc.whoAmI)
            {
                TerraGuardian tg = SpawnGuardianNPC(npc.Center.X, npc.Center.Y, GuardianBase.Sardine);
                tg.AddBuff(BuffID.Slimed, 300);
                switch (Main.rand.Next(5))
                {
                    default:
                        tg.SaySomething("*Heavily breathing* Soo good to be able to breath normally again.");
                        break;
                    case 1:
                        tg.SaySomething("Eww, It will take a long time for me to lick all that out. At least tastes good.");
                        break;
                    case 2:
                        tg.SaySomething("Hey, I nearly killed it! Oh, well, whatever. You helped too..");
                        break;
                    case 3:
                        tg.SaySomething("So glad to be out of that, my skin was itching.");
                        break;
                    case 4:
                        tg.SaySomething("Please don't tell about this to other bounty hunters.");
                        break;
                }
                AddGuardianMet(GuardianBase.Sardine);
                TrappedCatKingSlime = -1;
                const int Distance = 1000;
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active && Math.Abs(Main.player[p].position.X - npc.Center.X) < Distance && Math.Abs(Main.player[p].position.Y - npc.Center.Y) < Distance)
                    {
                        if (p == Main.myPlayer && !PlayerMod.PlayerHasGuardian(Main.player[p], GuardianBase.Sardine))
                            Main.NewText("You have met Sardine.", MainMod.RecruitColor);
                        PlayerMod.AddPlayerGuardian(Main.player[p], GuardianBase.Sardine);
                    }
                }
            }
            if (NPC.downedGoblins && npc.type == NPCID.DarkCaster)
            {
                if (!HasMetGuardian(GuardianBase.Quentin) && !HasGuardianNPC(GuardianBase.Quentin) && Main.rand.Next(80) == 0)
                {
                    TerraGuardian tg = SpawnGuardianNPC(npc.Center.X, npc.Bottom.Y, GuardianBase.Quentin, "");
                    tg.SaySomethingCanSchedule("Thanks for rescue me from that dark sorcerer, he wanted to force me to become his familiar, by the way i am Quentin, the Mage's apprentice bunny.", true, Main.rand.Next(40, 60));
                    AddGuardianMet(tg.ID, tg.ModID);
                    PlayerMod.AddPlayerGuardian(Main.player[npc.target], tg.ID, tg.ModID);
                }
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
            return base.PreNPCLoot(npc);
        }

        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.Bunny && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Accessories.BunnyFoot>());
            }
            if (npc.type == NPCID.GoblinWarrior && Main.rand.Next(100) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Accessories.ProvocationBadge>());
            }
            if (npc.type == NPCID.Plantera && Main.rand.Next(50) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.UprootedTree>());
            }
            if (!Main.halloween && (npc.type == NPCID.RedDevil || npc.type == NPCID.Demon) && HasGuardianNPC(GuardianBase.Wrath) && Main.rand.Next(100) == 0)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Outfit.Wrath.UnholyAmulet>());
            }
            if (npc.type == NPCID.DarkCaster && Main.rand.Next(5) == 0)
            {
                Item.NewItem(npc.getRect(), ItemID.Book);
            }
            if (npc.type == NPCID.CursedSkull && Main.rand.Next(3) == 0)
            {
                Item.NewItem(npc.getRect(), ItemID.Book);
            }
            if (npc.type == NPCID.WallofFlesh)
                MainMod.LastWof = false;
            if (npc.playerInteraction[Main.myPlayer])
            {
                PlayerMod pm = Main.LocalPlayer.GetModPlayer<PlayerMod>();
                foreach(QuestData qd in pm.QuestDatas)
                {
                    QuestBase.Data = qd;
                    if(!qd.IsInvalid)
                        qd.GetBase.OnMobKill(npc);
                }
            }
            bool SomeGuardianHurt = false, SomeGuardianNeedMana = false;
            for (int p = 0; p < 255; p++)
            {
                if (npc.playerInteraction[p] && Main.player[p].active)
                {
                    SomeGuardianHurt = Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && !x.Downed && x.HP < x.MHP);
                    SomeGuardianNeedMana = Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && !x.Downed && x.MP < x.MMP);
                    if (p == Main.myPlayer)
                    {
                        /*foreach (GuardianData d in Main.player[p].GetModPlayer<PlayerMod>().GetGuardians())
                        {
                            if (!d.request.Active)
                                continue;
                            d.request.Base.OnKillMob(npc, d.request);
                        }*/
                        if (npc.value > 0)
                        {
                            List<TerraGuardian> tgs = new List<TerraGuardian>();
                            foreach (TerraGuardian tg in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                            {
                                if (tg.Active && !tg.KnockedOut && !tg.Downed)
                                {
                                    tgs.Add(tg);
                                }
                            }
                            if (tgs.Count > 0)
                            {
                                uint CoinReward = (uint)(npc.value * ((float)tgs.Count / 2));
                                foreach (TerraGuardian tg in tgs)
                                {
                                    uint CoinValue = (uint)(CoinReward / tgs.Count);
                                    tg.Coins += CoinValue;
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
            if (mobType > MobTypes.Normal)
                StrongMonsterLoot(npc);
            if (Main.netMode < 2)
            {
                if (IsBoss(npc))
                {
                    bool HasBossPartAlive = false;
                    byte EoWParts = 0;
                    for (int i = 0; i < 200; i++)
                    {
                        if (i != npc.whoAmI && npc.active)
                        {
                            if(Main.npc[i].type >= Terraria.ID.NPCID.EaterofWorldsHead && Main.npc[i].type <= Terraria.ID.NPCID.EaterofWorldsTail)
                            {
                                EoWParts++;
                            }
                            else if (IsBoss(Main.npc[i]))
                            {
                                HasBossPartAlive = true;
                                break;
                            }
                        }
                    }
                    if (!HasBossPartAlive && EoWParts > 1)
                        HasBossPartAlive = true;
                    if (!HasBossPartAlive)
                    {
                        MainMod.LastBossSpotted = false;
                        Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.DefeatedABoss);
                        int MaxHealthValue = npc.lifeMax;
                        if (npc.type >= NPCID.EaterofWorldsHead && npc.type <= NPCID.EaterofWorldsTail)
                            MaxHealthValue *= 30;
                        GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.BossDefeated,
                            Main.player[Main.myPlayer].name, npc.GivenOrTypeName, 16, npc.lifeMax * 0.025f,
                            GuardianGlobalInfos.GetGuardiansInTheWorld());
                    }
                }
                if (IsMiniboss(npc))
                {
                    GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.MinibossDefeated, Main.player[Main.myPlayer].name,
                        npc.GivenOrTypeName, 10, npc.lifeMax * 0.025f, GuardianGlobalInfos.GetGuardiansInTheWorld());
                }
            }
        }

        public static bool IsBoss(NPC npc)
        {
            return npc.boss || (npc.type >= Terraria.ID.NPCID.EaterofWorldsHead && npc.type <= Terraria.ID.NPCID.EaterofWorldsTail) || Terraria.ID.NPCID.Sets.TechnicallyABoss[npc.type];
        }

        public static bool IsMiniboss(NPC npc)
        {
            return npc.type == NPCID.IceGolem || npc.type == NPCID.SandElemental || npc.type == NPCID.Tim || npc.type == NPCID.RuneWizard || npc.type == NPCID.TheGroom || 
                npc.type == NPCID.TheBride || npc.type == NPCID.DoctorBones;
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
                        DropLoot(Terraria.ID.ItemID.CrimtaneOre, Main.rand.Next(10, 26) * (int)mobType);
                    }
                    else
                    {
                        DropLoot(Terraria.ID.ItemID.DemoniteOre, Main.rand.Next(10, 26) * (int)mobType);
                    }
                    break;
                case 13:
                case 14:
                case 15:
                    DropLoot(Terraria.ID.ItemID.DemoniteOre, Main.rand.Next(1, 3) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.ShadowScale, Main.rand.Next(1, 3) * (int)mobType);
                    break;
                case 267:
                    DropLoot(Terraria.ID.ItemID.CrimtaneOre, Main.rand.Next(1, 3) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.TissueSample, Main.rand.Next(1, 3) * (int)mobType);
                    break;
                case 35:
                    DropLoot(Terraria.ID.ItemID.Bone, Main.rand.Next(10, 21) * (int)mobType);
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
                    DropLoot(Terraria.ID.ItemID.BeeWax, Main.rand.Next(10, 21) * (int)mobType);
                    DropLoot(Terraria.ID.ItemID.Beenade, Main.rand.Next(5, 11) * (int)mobType);
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
            if (npc.type == NPCID.NebulaHeadcrab && npc.ai[0] == 5)
            {
                if (PlayerPositionBackup[npc.target] != Vector2.Zero && Main.player[npc.target].HasBuff(163))
                    Main.player[npc.target].DelBuff(Main.player[npc.target].FindBuffIndex(163));
            }
            if ((npc.type == NPCID.DD2WitherBeastT2 || npc.type == NPCID.DD2WitherBeastT3) && npc.ai[0] == 1)
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
            if(ShardDebuffCount > 0 && !npc.HasBuff(ModContent.BuffType<Buffs.ShardPierce>()))
            {
                ShardDebuffCount = 0;
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

        public static bool IsSameMonster(NPC npc, int ReqMobID)
        {
            int m = npc.type;
            if (m == NPCID.EaterofWorldsHead || m == NPCID.EaterofWorldsBody || m == NPCID.EaterofWorldsTail)
            {
                bool HasBodyPart = false;
                for (int n = 0; n < 200; n++)
                {
                    if (n != npc.whoAmI && Main.npc[n].active && Main.npc[n].type == NPCID.EaterofWorldsBody)
                    {
                        HasBodyPart = true;
                        break;
                    }
                }
                return !HasBodyPart;
            }
            else if (m == ReqMobID)
                return true;
            else
            {
                switch (ReqMobID)
                {
                    case NPCID.Zombie: //Add event monsters to the list.
                        return m == 430 || m == 132 || m == 186 || m == 432 || m == 187 || m == 433 || m == 188 || m == 434 || m == 189 || m == 435 ||
                            m == 200 || m == 436 || m == 319 || m == 320 || m == 321 || m == 331 || m == 332 || m == 223 || m == 52 || m == 53 || m == 536 ||
                            m == NPCID.ZombieEskimo || m == NPCID.ArmedZombieEskimo || m == 255 || m == 254 || m == NPCID.BloodZombie;
                    case NPCID.ZombieEskimo:
                        return m == NPCID.ArmedZombieEskimo;
                    case NPCID.Skeleton:
                        return m == NPCID.ArmoredSkeleton || m == NPCID.BigHeadacheSkeleton || m == NPCID.BigMisassembledSkeleton || m == NPCID.BigPantlessSkeleton || m == NPCID.BigSkeleton ||
                            m == NPCID.BoneThrowingSkeleton || m == NPCID.BoneThrowingSkeleton2 || m == NPCID.BoneThrowingSkeleton3 || m == NPCID.BoneThrowingSkeleton4 ||
                            m == NPCID.HeadacheSkeleton || m == NPCID.HeavySkeleton || m == NPCID.MisassembledSkeleton || m == NPCID.PantlessSkeleton || m == NPCID.SkeletonAlien ||
                            m == NPCID.SkeletonArcher || m == NPCID.SkeletonAstonaut || m == NPCID.SkeletonTopHat || m == NPCID.SmallHeadacheSkeleton || m == NPCID.SmallMisassembledSkeleton ||
                            m == NPCID.SmallPantlessSkeleton || m == NPCID.SmallSkeleton;
                    case NPCID.DemonEye:
                        return m == 190 || m == 191 || m == 192 || m == 193 || m == 194 || m == 317 || m == 318;
                    case NPCID.WallCreeper:
                        return m == NPCID.WallCreeperWall;
                    case NPCID.BloodCrawler:
                        return m == NPCID.BloodCrawlerWall;
                    case NPCID.Demon:
                        return m == NPCID.VoodooDemon;
                    case NPCID.JungleCreeper:
                        return m == NPCID.JungleCreeperWall;
                    case NPCID.Hornet:
                        return m == NPCID.HornetFatty || m == NPCID.HornetHoney || m == NPCID.HornetLeafy || m == NPCID.HornetSpikey || m == NPCID.HornetStingy;
                    case NPCID.AngryBones:
                        return m == 294 || m == 295 || m == 296;
                    case NPCID.BlueArmoredBones:
                        return m == NPCID.BlueArmoredBonesMace || m == NPCID.BlueArmoredBonesNoPants || m == NPCID.BlueArmoredBonesSword;
                    case NPCID.RustyArmoredBonesAxe:
                        return m == NPCID.RustyArmoredBonesFlail || m == NPCID.RustyArmoredBonesSword || m == NPCID.RustyArmoredBonesSwordNoArmor;
                    case NPCID.HellArmoredBones:
                        return m == NPCID.HellArmoredBonesMace || m == NPCID.HellArmoredBonesSpikeShield || m == NPCID.HellArmoredBonesSword;
                    case NPCID.Necromancer:
                        return m == NPCID.NecromancerArmored;
                    case NPCID.RaggedCaster:
                        return m == NPCID.RaggedCasterOpenCoat;
                    case NPCID.DiabolistRed:
                        return m == NPCID.DiabolistWhite;
                    case NPCID.BlueSlime:
                        return m == NPCID.SlimeRibbonGreen || m == NPCID.SlimeRibbonRed || m == NPCID.SlimeRibbonWhite || m == NPCID.SlimeRibbonYellow || m == 302 ||
                            m == NPCID.SandSlime || m == NPCID.IceSlime || m == NPCID.SpikedIceSlime || m == NPCID.SlimedZombie || m == NPCID.ArmedZombieSlimed ||
                            m == NPCID.LavaSlime || m == NPCID.RainbowSlime || m == NPCID.KingSlime || m == NPCID.IlluminantSlime || m == NPCID.DungeonSlime ||
                            m == NPCID.MotherSlime || m == NPCID.Slimeling || m == NPCID.SlimeMasked || m == NPCID.SlimeSpiked || m == NPCID.SpikedJungleSlime ||
                            m == NPCID.UmbrellaSlime; //302 is Bunny Slime
                    case NPCID.Lihzahrd:
                        return m == NPCID.LihzahrdCrawler;
                    case NPCID.CaveBat:
                        return m == NPCID.GiantBat || m == NPCID.IceBat || m == NPCID.IlluminantBat || m == NPCID.JungleBat || m == NPCID.VampireBat;
                    case NPCID.DesertScorpionWalk:
                        return m == NPCID.DesertScorpionWall;
                    case NPCID.DesertGhoul:
                        return m == NPCID.DesertGhoulCorruption || m == NPCID.DesertGhoulCrimson || m == NPCID.DesertGhoulHallow;
                    case NPCID.DesertLamiaDark:
                    case NPCID.DesertLamiaLight:
                        return m == NPCID.DesertLamiaLight || m == NPCID.DesertLamiaDark;
                    case NPCID.Mummy:
                        return m == NPCID.LightMummy || m == NPCID.DarkMummy;
                    default:
                        return false;
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
