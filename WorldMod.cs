using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon
{
    public class WorldMod : ModWorld
    {
        public const int MaxGuardianNpcsInWorld = 30;
        public static List<GuardianID> GuardiansMet = new List<GuardianID>();
        public static bool LastWasDay = false, DelayedWasDay = false, DayChange = false, HourChange = false;
        public static double LastTime = 0;
        public static List<TerraGuardian> GuardianTownNPC = new List<TerraGuardian>();
        public static GuardianTownNpcState[] GuardianNPCsInWorld = new GuardianTownNpcState[MaxGuardianNpcsInWorld];
        public static GuardianID[] SpawnGuardian = new GuardianID[0];
        public static int GuardiansMetCount { get { return GuardiansMet.Count; } }
        public static byte SpawnDelay = 0, LeaveCooldown = 0;
        public static List<GuardianID> ScheduledVisits = new List<GuardianID>();
        public static List<GuardianBuildingInfo> HouseInfos = new List<GuardianBuildingInfo>();
        private static bool OldOneArmyWasLastActive = false;
        public static bool IsEtherRealm = false;

        public static bool IsStarter(TerraGuardian guardian)
        {
            return IsStarter(guardian.MyID);
        }

        public static bool IsStarter(GuardianData gd)
        {
            return IsStarter(gd.MyID);
        }

        public static bool IsStarter(GuardianID gid)
        {
            foreach (GuardianID ngid in SpawnGuardian)
            {
                if (ngid.IsSameID(gid))
                    return true;
            }
            return false;
        }

        public static bool IsStarter(int ID, string ModID)
        {
            foreach(GuardianID gid in SpawnGuardian)
            {
                if (gid.IsSameID(ID, ModID))
                    return true;
            }
            return false;
        }

        public static void AllowGuardianNPCToSpawn(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            int EmptySlot = -1;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (EmptySlot == -1 && GuardianNPCsInWorld[s] == null)
                    EmptySlot = s;
                if (GuardianNPCsInWorld[s] != null && GuardianNPCsInWorld[s].IsID(ID, ModID))
                {
                    return;
                }
            }
            if (EmptySlot > -1)
                GuardianNPCsInWorld[EmptySlot] = new GuardianTownNpcState(ID, ModID);
        }

        public static void RemoveGuardianNPCToSpawn(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (GuardianNPCsInWorld[s] != null && GuardianNPCsInWorld[s].IsID(ID, ModID))
                {
                    GuardianNPCsInWorld[s] = null;
                }
            }
        }

        public static bool CanGuardianNPCSpawnInTheWorld(int ID, string ModID = "") //TODO - Rename this method, this is misleading.
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (GuardianNPCsInWorld[s] != null && GuardianNPCsInWorld[s].IsID(ID, ModID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasEmptyGuardianNPCSlot()
        {
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (GuardianNPCsInWorld[s] == null)
                {
                    return true;
                }
            }
            return false;
        }

        public static void AddTownGuardianNpc(TerraGuardian tg)
        {
            if (!GuardianTownNPC.Any(x => x.MyID == tg.MyID))
            {
                GuardianTownNPC.Add(tg);
            }
        }

        public static void SkipTime(float Time)
        {
            Main.time += Time * 3600;
            GuardianGlobalInfos.LifeTime += TimeSpan.FromSeconds(Time * 3600);
            bool CheckDayPhaseChange = true;
            while (CheckDayPhaseChange)
            {
                if (Main.dayTime)
                {
                    if (Main.time >= 15 * 3600)
                    {
                        Main.time -= 15 * 3600;
                        Main.dayTime = false;
                    }
                    else if (Main.time < 0)
                    {
                        Main.time += 15 * 3600;
                        Main.dayTime = false;
                        Main.AnglerQuestSwap();
                    }
                    else { CheckDayPhaseChange = false; }
                }
                else
                {
                    if (Main.time >= 9 * 3600)
                    {
                        Main.time -= 9 * 3600;
                        Main.dayTime = true;
                        DayChange = true;
                        Main.AnglerQuestSwap();
                    }
                    else if (Main.time < 0)
                    {
                        Main.time += 9 * 3600;
                        Main.dayTime = true;
                        DayChange = true;
                    }
                    else { CheckDayPhaseChange = false; }
                }
            }
        }

        public override void PreUpdate()
        {
            ProjMod.CheckForInactiveProjectiles();
            if (!Main.gameMenu && Terraria.GameContent.Events.DD2Event.Ongoing && !OldOneArmyWasLastActive)
            {
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.WhenOldOneArmyStarts);
            }
            OldOneArmyWasLastActive = Terraria.GameContent.Events.DD2Event.Ongoing;
            LastWasDay = DelayedWasDay;
            DelayedWasDay = Main.dayTime;
            DayChange = false;
            HourChange = false;
            bool HasTimeOfDayChanged = false;
            double TimeParser = Main.time;
            if (Main.dayTime)
            {
                TimeParser += 4.5f * 3600;
            }
            else
            {
                TimeParser += 19.5f * 3600;
                if (TimeParser >= 24)
                    TimeParser -= 24;
            }
            if (Main.time == 0)
            {
                AlexRecruitScripts.CheckIfAlexIsInTheWorld();
                Companions.MinervaBase.AllowGettingMoreFoodFromMinerva();
                Companions.MiguelBase.RefreshExercisesOfAllPlayers();
                if (Math.Abs(TimeParser - LastTime) < 30f)
                {
                    HasTimeOfDayChanged = true;
                }
                if (Main.dayTime && HasTimeOfDayChanged)
                {
                    DayChange = true;
                    GuardianGlobalInfos.UpdateSeason();
                }
                TriggerHandler.FireDayNightChange(Main.dayTime);
            }
            const float HourToFrames = 1f / 3600;
            if (LastTime != -1 && (int)TimeParser * HourToFrames != (int)LastTime * HourToFrames)
            {
                HourChange = true;
                TriggerHandler.FireHourChange((int)(TimeParser * HourToFrames));
            }
            LastTime = TimeParser;
            MainMod.TimeTranslated = (float)TimeParser;
            if (Main.netMode != 1)
            {
                SpawnDelay++;
                if (SpawnDelay >= 20)
                {
                    LeaveCooldown++;
                    SpawnDelay = 0;
                    CheckIfGuardianNPCCanSpawn();
                    if (LeaveCooldown >= 10)
                    {
                        LeaveCooldown = 0;
                        if (!CheckIfSomeoneMustLeaveWorld())
                        {
                            CheckIfSomeoneCanVisit();
                        }
                    }
                }
            }
            foreach (TerraGuardian tg in GuardianTownNPC)
            {
                if (tg.OwnerPos == -1)
                {
                    tg.Update();
                }
            }
        }

        public static void CheckIfSomeoneCanVisit()
        {
            if ((!Main.dayTime && (Main.time < 3f * 3600 || Main.time >= 5.5f * 3600)) || (Main.dayTime && (Main.time < 2 * 3600 || Main.time >= 4.5 * 3600)))
            {
                return;
            }
            float VisitRate = 1f;
            foreach (TerraGuardian tg in GuardianTownNPC)
            {
                if (tg.GetTownNpcInfo == null)
                    VisitRate *= 0.85f;
            }
            if ((ScheduledVisits.Count > 0 && Main.rand.NextDouble() < 0.5f) || Main.rand.NextDouble() < VisitRate * 0.025f) //0.01665f <- 0.00333f
            {
                List<GuardianID> PossibleIDs = new List<GuardianID>();
                List<GuardianID> GuardianListToCheck = MainMod.CompanionsCanVisitWorld ? GuardiansMet : new List<GuardianID>();
                bool IsFromSchedule = false;
                if (ScheduledVisits.Count > 0)
                {
                    IsFromSchedule = true;
                    GuardianListToCheck = ScheduledVisits;
                }
                foreach (GuardianID ids in GuardianListToCheck)
                {
                    if (!MainMod.IsGuardianInTheWorld(ids.ID, ids.ModID) && !GuardianNPCsInWorld.Any(x => x != null && x.IsID(ids.ID, ids.ModID)))
                    {
                        GuardianBase gb = GuardianBase.GetGuardianBase(ids.ID, ids.ModID);
                        bool SomeoneHasTheTrust = false;
                        if (Main.netMode == 0)
                        {
                            GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], ids.ID, ids.ModID);
                            if (gd != null)
                            {
                                if (gd.TrustLevel >= TrustLevels.VisitTrust)
                                {
                                    SomeoneHasTheTrust = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int p = 0; p < 255; p++)
                            {
                                if (Main.player[p].active)
                                {
                                    GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[p], ids.ID, ids.ModID);
                                    if (gd != null)
                                    {
                                        if (gd.TrustLevel >= TrustLevels.VisitTrust)
                                        {
                                            SomeoneHasTheTrust = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (SomeoneHasTheTrust && Main.dayTime != gb.IsNocturnal)
                        {
                            PossibleIDs.Add(ids);
                        }
                    }
                }
                if (PossibleIDs.Count > 0)
                {
                    GuardianID WinnerID = PossibleIDs[Main.rand.Next(PossibleIDs.Count)];
                    if (IsFromSchedule)
                        ScheduledVisits.Remove(WinnerID);
                    List<Vector2> PossibleSpawnPosition = new List<Vector2>();
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless && Main.npc[n].type != NPCID.OldMan)
                        {
                            Vector2 npcPos = Main.npc[n].Center;
                            bool PlayerNearby = false;
                            for (int p = 0; p < 255; p++)
                            {
                                if (Main.player[p].active)
                                {
                                    if (Math.Abs(Main.player[p].Center.X - npcPos.X) < NPC.safeRangeX + NPC.sWidth * 0.5f &&
                                        Math.Abs(Main.player[p].Center.Y - npcPos.Y) < NPC.safeRangeY + NPC.sHeight * 0.5f)
                                    {
                                        PlayerNearby = true;
                                        break;
                                    }
                                }
                            }
                            if (!PlayerNearby)
                            {
                                npcPos.Y += Main.npc[n].height * 0.5f - 2f;
                                PossibleSpawnPosition.Add(npcPos);
                            }
                        }
                    }
                    foreach (TerraGuardian tg in GuardianTownNPC)
                    {
                        if (tg.GetTownNpcInfo != null)
                        {
                            Vector2 guardianPos = tg.CenterPosition;
                            bool PlayerNearby = false;
                            for (int p = 0; p < 255; p++)
                            {
                                if (Main.player[p].active)
                                {
                                    if (Math.Abs(Main.player[p].Center.X - guardianPos.X) < NPC.safeRangeX + NPC.sWidth * 0.5f &&
                                        Math.Abs(Main.player[p].Center.Y - guardianPos.Y) < NPC.safeRangeY + NPC.sHeight * 0.5f)
                                    {
                                        PlayerNearby = true;
                                        break;
                                    }
                                }
                            }
                            if (!PlayerNearby)
                            {
                                guardianPos.Y += tg.Height * 0.5f - 2f;
                                PossibleSpawnPosition.Add(guardianPos);
                            }
                        }
                    }
                    if (PossibleSpawnPosition.Count > 0)
                    {
                        Vector2 WinningPosition = PossibleSpawnPosition[Main.rand.Next(PossibleSpawnPosition.Count)];
                        PossibleIDs.Clear();
                        PossibleSpawnPosition.Clear();
                        NpcMod.SpawnGuardianNPC(WinningPosition.X, WinningPosition.Y, WinnerID.ID, WinnerID.ModID, false);
                        Main.NewText(NpcMod.GetGuardianNPCName(WinnerID.ID, WinnerID.ModID) + " came to visit.", Color.Aquamarine);
                    }
                }
            }
        }

        /// <returns>Returns true if someone left.</returns>
        public static bool CheckIfSomeoneMustLeaveWorld()
        {
            if (GuardianTownNPC.Count == 0)
            {
                return false;
            }
            //pick a random guardian to try making leave.
            int Pos = Main.rand.Next(GuardianTownNPC.Count);
            if (!GuardianTownNPC[Pos].Base.IsNocturnal == Main.dayTime) //(!GuardianTownNPC[Pos].Base.IsNocturnal && Main.dayTime) || (GuardianTownNPC[Pos].Base.IsNocturnal && !Main.dayTime))
            {
                return false;
            }
            if (GuardianTownNPC[Pos].GetTownNpcInfo != null && !GuardianTownNPC[Pos].GetTownNpcInfo.Homeless)
            {
                return false;
            }
            else
            {
                if (GuardianTownNPC[Pos].IsStarter || IsStarter(GuardianTownNPC[Pos]))
                    return false;
            }
            bool HasPlayerNearby = false;
            for (int p = 0; p < 255; p++)
            {
                Player player = Main.player[p];
                if (player.active && Math.Abs(player.Center.X - GuardianTownNPC[Pos].Position.X) < NPC.sWidth && Math.Abs(player.Center.Y - GuardianTownNPC[Pos].CenterY) < NPC.sHeight)
                {
                    HasPlayerNearby = true;
                    break;
                }
            }
            if (!HasPlayerNearby)
            {
                if (PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], GuardianTownNPC[Pos].ID, GuardianTownNPC[Pos].ModID))
                {
                    Main.NewText(GuardianTownNPC[Pos].Name + GuardianTownNPC[Pos].Base.LeavingWorldMessageGuardianSummoned);
                }
                else
                {
                    Main.NewText(GuardianTownNPC[Pos].Name + GuardianTownNPC[Pos].Base.LeavingWorldMessage);
                }
                GuardianTownNPC.RemoveAt(Pos);
                return true;
            }
            return false;
        }

        public override void Initialize()
        {
            GuardianBountyQuest.Initialize();
            Npcs.LiebreNPC.EncounterTimes = 0;
            Npcs.BrutusNPC.WarnedAboutBrutus = false;
            MainMod.LastBossSpotted = MainMod.LastInvasionSpotted = MainMod.LastEventStarted = false;
            MainMod.LastEvent = 0;
            MainMod.LastInvasion = -1;
            TerraGuardian.UpdateAge = true;
            ProjMod.GuardianProj.Clear();
            MainMod.ScreenColorAlpha = 0;
            HouseInfos.Clear();
            GuardiansMet.Clear();
            GuardianTownNPC.Clear();
            for (int i = 0; i < MaxGuardianNpcsInWorld; i++)
            {
                GuardianNPCsInWorld[i] = null;
            }
            if (Compatibility.NExperienceCompatibility.IsModActive)
                Compatibility.NExperienceCompatibility.ResetOnWorldLoad();
            GuardianBountyQuest.Reset();
            AlexRecruitScripts.SpawnedTombstone = false;
            LastTime = -1;
            MainMod.FlufflesHauntOpacity = -0.5f;
            Npcs.GhostFoxGuardianNPC.GhostFoxHauntLifted = false;
            MainMod.SoulSaved = false;
        }

        public override void NetSend(System.IO.BinaryWriter writer)
        {

        }

        public override void NetReceive(System.IO.BinaryReader reader)
        {

        }

        public override void ModifyWorldGenTasks(List<Terraria.World.Generation.GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("Spawning Starter Guardian.", delegate (GenerationProgress progress)
            {
                progress.Message = "Spawning Starter Guardian";
                List<GuardianID> PossibleGuardians = MainMod.GetPossibleStarterGuardians().ToList();
                List<GuardianID> Starters = new List<GuardianID>();
                byte StartersToSpawn = 1;
                if (PossibleGuardians.Count >= 5 && Main.rand.NextDouble() < 0.5)
                    StartersToSpawn++;
                if (PossibleGuardians.Count >= 10 && Main.rand.NextDouble() < 0.2)
                    StartersToSpawn++;
                if (PossibleGuardians.Count >= 20 && Main.rand.NextDouble() < 0.10)
                    StartersToSpawn++;
                if (PossibleGuardians.Count >= 30 && Main.rand.NextDouble() < 0.05)
                    StartersToSpawn++;
                for (int i = 0; i < StartersToSpawn; i++)
                {
                    if (PossibleGuardians.Count == 0)
                        break;
                    int Picked = Main.rand.Next(PossibleGuardians.Count);
                    GuardianID id = PossibleGuardians[Picked];
                    TerraGuardian tg = new TerraGuardian(id.ID, id.ModID);
                    tg.Active = true;
                    tg.Position.X = Main.spawnTileX * 16 + 8;
                    tg.Position.Y = Main.spawnTileY * 16 + 16;
                    GuardianTownNPC.Add(tg);
                    NpcMod.AddGuardianMet(id.ID, id.ModID);
                    AllowGuardianNPCToSpawn(id.ID, id.ModID);
                    Starters.Add(id);
                }
                SpawnGuardian = Starters.ToArray();
            }));
            tasks.Add(new PassLegacy("Spawning Tombstone.", delegate (GenerationProgress progress)
            {
                AlexRecruitScripts.TrySpawningTombstone(progress);
            }));
        }

        public override Terraria.ModLoader.IO.TagCompound Save()
        {
            Terraria.ModLoader.IO.TagCompound tag = new Terraria.ModLoader.IO.TagCompound();
            tag.Add("ModVersion", MainMod.ModVersion);
            tag.Add("SpawnGuardian_Count", SpawnGuardian.Length);
            for (int i = 0; i < SpawnGuardian.Length; i++)
            {
                tag.Add("SpawnGuardian_ID_" + i, SpawnGuardian[i].ID);
                tag.Add("SpawnGuardian_ModID_" + i, SpawnGuardian[i].ModID);
            }
            //Save spawn guardian
            tag.Add("GuardiansMet_Count", GuardiansMet.Count);
            for (int i = 0; i < GuardiansMet.Count; i++)
            {
                tag.Add("GuardiansMet_k" + i, GuardiansMet[i].ID);
                tag.Add("GuardiansMet_v" + i, GuardiansMet[i].ModID);
            }
            for (int i = 0; i < MaxGuardianNpcsInWorld; i++)
            {
                tag.Add("GuardiansCanSpawn_HasValue" + i, GuardianNPCsInWorld[i] != null);
                if (GuardianNPCsInWorld[i] != null)
                {
                    tag.Add("GuardiansCanSpawn_k" + i, GuardianNPCsInWorld[i].CharID.ID);
                    tag.Add("GuardiansCanSpawn_v" + i, GuardianNPCsInWorld[i].CharID.ModID);
                    tag.Add("GuardiansCanSpawn_h" + i, GuardianNPCsInWorld[i].Homeless);
                    tag.Add("GuardiansCanSpawn_hx" + i, GuardianNPCsInWorld[i].HomeX);
                    tag.Add("GuardiansCanSpawn_hy" + i, GuardianNPCsInWorld[i].HomeY);
                }
            }
            tag.Add("NumTownGuardians", GuardianTownNPC.Count);
            for (int i = 0; i < GuardianTownNPC.Count; i++)
            {
                tag.Add("GuardianTownNPC_ID_" + i, GuardianTownNPC[i].ID);
                tag.Add("GuardianTownNPC_ModID_" + i, GuardianTownNPC[i].ModID);
                tag.Add("GuardianTownNPC_LastFollowingPlayer_" + i, GuardianTownNPC[i].OwnerPos > -1);
                if (GuardianTownNPC[i].OwnerPos == -1)
                {
                    tag.Add("GuardianTownNPC_HP_" + i, (GuardianTownNPC[i].HP == GuardianTownNPC[i].MHP ? 1f : (float)GuardianTownNPC[i].HP / GuardianTownNPC[i].MHP));
                    tag.Add("GuardianTownNPC_PX_" + i, GuardianTownNPC[i].Position.X);
                    tag.Add("GuardianTownNPC_PY_" + i, GuardianTownNPC[i].Position.Y);
                }
            }
            GuardianBountyQuest.Save(tag);
            AlexRecruitScripts.Save(tag);
            tag.Add("DominoDismissed", Npcs.DominoNPC.DominoDismissed);
            tag.Add("GhostFoxHauntLifted", Npcs.GhostFoxGuardianNPC.GhostFoxHauntLifted);
            tag.Add("LiebreEncounterTimes", Npcs.LiebreNPC.EncounterTimes);
            GuardianShopHandler.SaveShops(tag);
            GuardianGlobalInfos.SaveGlobalInfos();
            GuardianGlobalInfos.SaveFeats();
            return tag;
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            MainMod.LastMoonLordKilled = NPC.downedMoonlord;
            MainMod.LastHardMode = Main.hardMode;
            GuardiansMet.Clear();
            if (!tag.ContainsKey("ModVersion"))
                return;
            int Version = tag.GetInt("ModVersion");
            if (Version >= 70)
            {
                if (Version >= 98)
                {
                    int Count = tag.GetInt("SpawnGuardian_Count");
                    List<GuardianID> StarterIds = new List<GuardianID>();
                    for (int i = 0; i < Count; i++)
                    {
                        int ID = tag.GetInt("SpawnGuardian_ID_" + i);
                        string ModId = tag.GetString("SpawnGuardian_ModID_" + i);
                        StarterIds.Add(new GuardianID(ID, ModId));
                    }
                    SpawnGuardian = StarterIds.ToArray();
                }
                else
                {
                    int ID = tag.GetInt("SpawnGuardian_ID");
                    string ModID = tag.GetString("SpawnGuardian_ModID");
                    SpawnGuardian = new GuardianID[] { new GuardianID(ID, ModID) };
                }
            }
            if (Version < 38)
            {
                int[] Ids = tag.GetIntArray("GuardiansMet");
                string ModID = MainMod.mod.Name;
                foreach (int i in Ids)
                    GuardiansMet.Add(new GuardianID(i, ModID));
            }
            else
            {
                int Count = tag.GetInt("GuardiansMet_Count");
                for (int i = 0; i < Count; i++)
                {
                    int Key = tag.GetInt("GuardiansMet_k" + i);
                    string Mod = tag.GetString("GuardiansMet_v" + i);
                    GuardiansMet.Add(new GuardianID(Key, Mod));
                }
            }
            if (Version < 39)
            {
                //Add existing guardians to the world.
                //No longer necessary.
            }
            else
            {
                int MaxGuardianSpawnCount = MaxGuardianNpcsInWorld;
                if (Version == 39)
                    MaxGuardianSpawnCount = 5;
                else if (Version < 76)
                    MaxGuardianSpawnCount = 10;
                for (int i = 0; i < MaxGuardianSpawnCount; i++)
                {
                    bool Exists = tag.GetBool("GuardiansCanSpawn_HasValue" + i);
                    if (Exists)
                    {
                        int ID = tag.GetInt("GuardiansCanSpawn_k" + i);
                        string ModID = tag.GetString("GuardiansCanSpawn_v" + i);
                        GuardianTownNpcState townnpcstate = new GuardianTownNpcState(ID, ModID);
                        if (Version >= 68)
                        {
                            townnpcstate.Homeless = tag.GetBool("GuardiansCanSpawn_h" + i);
                            townnpcstate.HomeX = tag.GetInt("GuardiansCanSpawn_hx" + i);
                            townnpcstate.HomeY = tag.GetInt("GuardiansCanSpawn_hy" + i);
                            townnpcstate.ValidateHouse();
                        }
                        GuardianNPCsInWorld[i] = townnpcstate;
                    }
                    else
                    {
                        GuardianNPCsInWorld[i] = null;
                    }
                }
            }
            if (Version >= 69)
            {
                int NumTownGuardians = tag.GetInt("NumTownGuardians");
                List<GuardianID> SpawnedGuardianIDs = new List<GuardianID>();
                GuardianTownNPC.Clear();
                for (int i = 0; i < NumTownGuardians; i++)
                {
                    int ID = tag.GetInt("GuardianTownNPC_ID_" + i);
                    string ModID = tag.GetString("GuardianTownNPC_ModID_" + i);
                    bool LastWasFollowingPlayer = tag.GetBool("GuardianTownNPC_LastFollowingPlayer_" + i);
                    TerraGuardian tg = new TerraGuardian(ID, ModID);
                    tg.Active = true;
                    tg.DoUpdateGuardianStatus();
                    if (!LastWasFollowingPlayer)
                    {
                        float HPPercentage = tag.GetFloat("GuardianTownNPC_HP_" + i);
                        float PositionX = tag.GetFloat("GuardianTownNPC_PX_" + i);
                        float PositionY = tag.GetFloat("GuardianTownNPC_PY_" + i);
                        tg.Position.X = PositionX;
                        tg.Position.Y = PositionY;
                        tg.SetFallStart();
                        if (HPPercentage != 1)
                        {
                            tg.HP = (int)((float)tg.MHP * HPPercentage);
                            if (tg.HP < 0)
                                tg.Knockout();
                        }
                    }
                    else
                    {
                        tg.Spawn();
                    }
                    tg.SetAimPositionToCenter();
                    if (!SpawnedGuardianIDs.Any(x => x.ID == ID && x.ModID == ModID))
                    {
                        GuardianTownNPC.Add(tg);
                        SpawnedGuardianIDs.Add(new GuardianID(ID, ModID));
                    }
                }
            }
            if (Version >= 36)
                GuardianBountyQuest.Load(tag, Version);
            if (Version >= 40)
                AlexRecruitScripts.Load(tag, Version);
            if (Version >= 54)
                Npcs.DominoNPC.DominoDismissed = tag.GetBool("DominoDismissed");
            if (Version >= 77)
                Npcs.GhostFoxGuardianNPC.GhostFoxHauntLifted = tag.GetBool("GhostFoxHauntLifted");
            if (Version >= 100)
                Npcs.LiebreNPC.EncounterTimes = tag.GetByte("LiebreEncounterTimes");
            if (Version >= 74)
                GuardianShopHandler.LoadShops(tag, Version);
            foreach (GuardianTownNpcState tns in GuardianNPCsInWorld)
            {
                if (tns != null && !tns.Homeless)
                    tns.ValidateHouse();
            }
            GuardianGlobalInfos.LoadGlobalInfos();
            GuardianGlobalInfos.LoadFeats();
        }

        public override void PostDrawTiles()
        {
            Main.spriteBatch.Begin(Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate,
                Microsoft.Xna.Framework.Graphics.BlendState.AlphaBlend,
                Main.DefaultSamplerState,
                Microsoft.Xna.Framework.Graphics.DepthStencilState.None,
                Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone,
                null,
                Main.GameViewMatrix.TransformationMatrix);
            TerraGuardian.CurrentDrawnOrderID = -1000;
            DrawTownNpcCompanions(DrawMoment.DrawAfterDrawingTiles);
            Main.spriteBatch.End();
        }

        public static void DrawTownNpcCompanions(DrawMoment drawMoment)
        {
            foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                if (!(tg.Active && tg.drawMoment == drawMoment))
                    continue;
                bool HasDrawMoment = false;
                foreach (GuardianDrawMoment dm in MainMod.DrawMoment)
                {
                    if (dm.GuardianWhoAmID == tg.WhoAmID)
                    {
                        HasDrawMoment = true;
                        break;
                    }
                }
                if (HasDrawMoment)
                    continue;
                tg.Draw(DoShading: true);
                /*if ((tg.OwnerPos == -1 || Main.player[tg.OwnerPos].ghost || (tg.UsingFurniture && !tg.PlayerMounted) || Main.player[tg.OwnerPos].stoned) && tg.InCameraRange())
                {
                    bool HasDrawMoment = false;
                    foreach (GuardianDrawMoment dm in MainMod.DrawMoment)
                    {
                        if (dm.GuardianWhoAmID == tg.WhoAmID)
                        {
                            HasDrawMoment = true;
                            break;
                        }
                    }
                    if (HasDrawMoment)
                        continue;
                    tg.Draw();
                }*/
            }
        }

        public static void AnalyzeDrawMoment()
        {
            MainMod.CompanionsToShowArrowFor.Clear();
            foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                if (tg.InCameraRange())
                {
                    if (tg.UsingFurniture)
                    {
                        if (MainMod.NpcInCameraRange)
                        {
                            tg.drawMoment = DrawMoment.DrawBeforeDrawingNpcs;
                        }
                        else
                        {
                            tg.drawMoment = DrawMoment.DrawAfterDrawingTiles;
                        }
                    }
                    else if ((tg.OwnerPos == -1 || Main.player[tg.OwnerPos].ghost || Main.player[tg.OwnerPos].stoned || Main.player[tg.OwnerPos].invis))
                    {
                        tg.drawMoment = DrawMoment.DrawAfterDrawingTiles;
                    }
                    else
                    {
                        tg.drawMoment = DrawMoment.DontDraw;
                    }
                }
                else
                {
                    Vector2 CameraCenter = MainMod.GetScreenCenter;
                    if (Math.Abs(tg.Position.X - CameraCenter.X) < Main.screenWidth + 800 &&
                        Math.Abs(tg.CenterY - CameraCenter.Y) < Main.screenHeight + 600)
                        MainMod.CompanionsToShowArrowFor.Add(tg);
                    tg.drawMoment = DrawMoment.DontDraw;
                }
            }
        }

        public static GuardianTownNpcState GetTownNpcState(GuardianID Id)
        {
            return GetTownNpcState(Id.ID, Id.ModID);
        }

        public static GuardianTownNpcState GetTownNpcState(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for (int t = 0; t < MaxGuardianNpcsInWorld; t++)
            {
                if (GuardianNPCsInWorld[t] != null && GuardianNPCsInWorld[t].IsID(ID, ModID))
                    return GuardianNPCsInWorld[t];
            }
            return null;
        }

        public static bool IsGuardianNpcInWorld(GuardianID Id)
        {
            return IsGuardianNpcInWorld(Id.ID, Id.ModID);
        }

        public static bool IsGuardianNpcInWorld(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            foreach (TerraGuardian g in GuardianTownNPC)
            {
                if (g.ID == ID && g.ModID == ModID)
                    return true;
            }
            return false;
        }

        public static void TurnNpcIntoGuardianTownNpc(NPC npc, int GuardianID, string GuardianModID = "")
        {
            if (GuardianModID == "")
                GuardianModID = MainMod.mod.Name;
            TerraGuardian guardian = null;
            if (IsGuardianNpcInWorld(GuardianID, GuardianModID))
            {
                foreach (TerraGuardian tg in GuardianTownNPC)
                {
                    if (tg.ID == GuardianID && tg.ModID == GuardianModID)
                    {
                        guardian = tg;
                        break;
                    }
                }
            }
            else if (Main.netMode == 0)
            {
                foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
                {
                    if (tg.ID == GuardianID && tg.ModID == GuardianModID)
                    {
                        guardian = tg;
                        break;
                    }
                }
            }
            if (guardian == null)
            {
                guardian = new TerraGuardian(GuardianID, GuardianModID);
                guardian.Position = npc.Bottom;
                guardian.SetFallStart();
                guardian.Direction = npc.direction;
                guardian.Active = true;
                guardian.ChangeIdleAction(TerraGuardian.IdleActions.Wait, 200);
                guardian.DoUpdateGuardianStatus();
                guardian.EnforceScale();
                MainMod.AddActiveGuardian(guardian, true);
            }
            if (!IsGuardianNpcInWorld(GuardianID, GuardianModID))
            {
                GuardianTownNPC.Add(guardian);
            }
            guardian.SetAimPositionToCenter();
            if (npc.modNPC is Npcs.GuardianActorNPC)
            {
                Npcs.GuardianActorNPC n = (Npcs.GuardianActorNPC)npc.modNPC;
                guardian.SaySomething(n.MessageText);
            }
            npc.active = false;
            if (Main.player[Main.myPlayer].talkNPC == npc.whoAmI)
            {
                GuardianMouseOverAndDialogueInterface.StartDialogue(guardian);
                GuardianMouseOverAndDialogueInterface.SetDialogue(Main.npcChatText);
                GuardianMouseOverAndDialogueInterface.SetDialogueDistanceAutoCloseDelay();
                Main.player[Main.myPlayer].talkNPC = npc.whoAmI;
            }
        }

        public static int Housing_GetMaxNumberOfHabitants()
        {
            int ChairCount = 0, BedCount = 0;
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                Tile tile = Main.tile[WorldGen.roomX[i], WorldGen.roomY[i]],
                    uppertile = Main.tile[WorldGen.roomX[i], WorldGen.roomY[i] - 1];
                if (tile.type == Terraria.ID.TileID.Beds && uppertile.type == Terraria.ID.TileID.Beds)
                    BedCount++;
                if (tile.type == Terraria.ID.TileID.Chairs && uppertile.type == Terraria.ID.TileID.Chairs)
                    ChairCount++;
            }
            if (ChairCount > 0 && BedCount > 0)
            {
                if (ChairCount < BedCount)
                    return ChairCount;
                return BedCount;
            }
            else if (ChairCount > 0 && BedCount == 0)
            {
                return ChairCount;
            }
            return 1;
        }

        public static void CheckIfGuardianNPCCanSpawn()
        {
            GuardianID ToSpawn = null;
            //Making use of this else to get the list of companions to spawn
            {
                List<GuardianID> PotentialGuardians = new List<GuardianID>();
                bool HasHomelessGuardian = false;
                for (int g = 0; g < MaxGuardianNpcsInWorld; g++)
                {
                    if (GuardianNPCsInWorld[g] != null)
                    {
                        if (IsGuardianNpcInWorld(GuardianNPCsInWorld[g].CharID.ID, GuardianNPCsInWorld[g].CharID.ModID))
                        {
                            if (GuardianNPCsInWorld[g].Homeless)
                            {
                                if (!HasHomelessGuardian)
                                    PotentialGuardians.Clear();
                                HasHomelessGuardian = true;
                                PotentialGuardians.Add(new GuardianID(GuardianNPCsInWorld[g].CharID.ID, GuardianNPCsInWorld[g].CharID.ModID));
                            }
                        }
                        else if (!HasHomelessGuardian)
                        {
                            GuardianBase gb = GuardianBase.GetGuardianBase(GuardianNPCsInWorld[g].CharID.ID, GuardianNPCsInWorld[g].CharID.ModID);
                            if (gb.IsNocturnal == !Main.dayTime)
                                PotentialGuardians.Add(new GuardianID(GuardianNPCsInWorld[g].CharID.ID, GuardianNPCsInWorld[g].CharID.ModID));
                        }
                    }
                }
                if (PotentialGuardians.Count > 0)
                {
                    ToSpawn = PotentialGuardians[Main.rand.Next(PotentialGuardians.Count)];
                }
            }
            if (ToSpawn == null)
                return;
            //Check if a guardian spawn. If can spawn, get the id and mod id, then try finding a house.
            //In case nobody can spawn, return the method and retry when spawn delay reaches 20 again.
            float MaxCheck = (Main.maxTilesX * Main.maxTilesY) * 1.5E-05f * Main.worldRate; //I have no idea what that 1.5E-05f is for, but It's on the source so..
            for (int n = 0; n < MaxCheck; n++)
            {
                int CenterX = Main.rand.Next(10, Main.maxTilesX - 10),
                    CenterY = Main.rand.Next(10, Main.maxTilesY - 20);
                if (Main.tile[CenterX, CenterY] == null || Main.tile[CenterX, CenterY].liquid > 32 || Main.tile[CenterX, CenterY].nactive())
                {
                    continue;
                }
                if (Main.tile[CenterX, CenterY].wall == 34)
                {
                    if (Main.rand.Next(4) == 0)
                    {
                        TrySpawningOrMovingGuardianNPC(ToSpawn, CenterX, CenterY);
                    }
                }
                else
                {
                    TrySpawningOrMovingGuardianNPC(ToSpawn, CenterX, CenterY);
                }
            }
        }

        public static void TrySpawningOrMovingGuardianNPC(GuardianID id, int X, int Y)
        {
            TrySpawningOrMovingGuardianNPC(id.ID, id.ModID, X, Y);
        }
        public static bool RoomOccupied, RoomEvil;
        public static int RoomScore;

        public static bool HasCompanionMetSomeoneWithHighFriendshipLevel(int ID, string ModID)
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (GuardianBase.GetGuardianBase(ID, ModID).MoveInLevel == 0)
                return true;
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && PlayerMod.PlayerHasGuardian(Main.player[i], ID, ModID))
                {
                    GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[i], ID, ModID);
                    if (gd.IsStarter || gd.FriendshipLevel >= gd.Base.MoveInLevel)
                        return true;
                }
            }
            return false;
        }

        public static void TrySpawningOrMovingGuardianNPC(int GuardianID, string ModID, int X, int Y)
        {
            if (!Main.wallHouse[Main.tile[X, Y].wall] || !WorldGen.StartRoomCheck(X, Y))
                return;
            GuardianBase gb = GuardianBase.GetGuardianBase(GuardianID, ModID);
            if (!Housing_IsRoomTallEnoughForGuardian(gb))
                return;
            GetRoomScoreForGuardian(gb, out RoomOccupied, out RoomEvil, out RoomScore);
            if (RoomScore <= 0 || !gb.RoomNeeds())
            {
                return;
            }
            if (Housing_IsRoomCrowded(gb)) return;
            if (!HasCompanionMetSomeoneWithHighFriendshipLevel(GuardianID, ModID))
                return;
            bool IsInTheWorld = false;
            for (int npc = 0; npc < GuardianTownNPC.Count; npc++)
            {
                GuardianTownNpcState townstate = GuardianTownNPC[npc].GetTownNpcInfo;
                if (townstate != null && townstate.Homeless)
                {
                    IsInTheWorld = true;
                    break;
                }
            }
            int SpawnX = WorldGen.bestX, SpawnY = WorldGen.bestY;
            Housing_TryGettingPlaceForCompanionToStay(ref SpawnX, ref SpawnY);
            int SpawnXBackup = SpawnX, SpawnYBackup = SpawnY;
            if (!IsInTheWorld)
            {
                bool NoPlayerNearby = true;
                Rectangle rect = new Rectangle(SpawnX * 16 + 8 - (int)(NPC.sWidth * 0.5f) - NPC.safeRangeX, SpawnY * 16 + 8 - (int)(NPC.sHeight * 0.5f) - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                for (int i = 0; i < 255; i++)
                {
                    if (!Main.player[i].active)
                        continue;
                    if (Main.player[i].getRect().Intersects(rect))
                    {
                        NoPlayerNearby = false;
                        break;
                    }
                }
                if (!NoPlayerNearby && SpawnY <= Main.worldSurface)
                {
                    for (int dist = 1; dist < 500; dist++)
                    {
                        for (int dir = 0; dir < 2; dir++)
                        {
                            if (dir == 0)
                            {
                                SpawnX = WorldGen.bestX + dist;
                            }
                            else
                            {
                                SpawnX = WorldGen.bestX - dist;
                            }
                            if (SpawnX > 10 && SpawnX < Main.maxTilesX - 10)
                            {
                                int YCheck = WorldGen.bestY - dist;
                                double SurfacePosition = WorldGen.bestY + dist;
                                if (YCheck < 10) YCheck = 10;
                                if (SurfacePosition > Main.worldSurface) SurfacePosition = Main.worldSurface;
                                while (YCheck < SurfacePosition)
                                {
                                    SpawnY = YCheck;
                                    if (!(Main.tile[SpawnX, YCheck].nactive() && Main.tileSolid[Main.tile[SpawnX, YCheck].type]))
                                    {
                                        YCheck++;
                                        continue;
                                    }
                                    if (!Collision.SolidTiles(SpawnX - 1, SpawnX + 1, YCheck - 3, YCheck - 1))
                                    {
                                        NoPlayerNearby = true;
                                        rect = new Rectangle(SpawnX * 16 + 8 - (int)(NPC.sWidth * 0.5f) - NPC.safeRangeX, YCheck * 16 + 8 - (int)(NPC.sHeight * 0.5f) - NPC.safeRangeY, NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                                        for (int n = 0; n < 255; n++)
                                        {
                                            if (!Main.player[n].active)
                                                continue;
                                            if (Main.player[n].getRect().Intersects(rect))
                                            {
                                                NoPlayerNearby = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                            }
                            if (NoPlayerNearby)
                                break;
                        }
                        if (NoPlayerNearby)
                            break;
                    }
                }
            }
            TerraGuardian guardian = null;
            if (Main.netMode == 0) //Never. Do. This. On. MP. Wonder not being able to complete a request, because your friend called your companion.
            {
                foreach (int Key in MainMod.ActiveGuardians.Keys)
                {
                    if (MainMod.ActiveGuardians[Key].ID == GuardianID && MainMod.ActiveGuardians[Key].ModID == ModID)
                    {
                        guardian = MainMod.ActiveGuardians[Key];
                        break;
                    }
                }
            }
            bool SpawnGuardian = guardian == null;
            if (guardian == null)
            {
                guardian = new TerraGuardian(GuardianID, ModID); //Probably is somewhere in the script, but I had a swarm of Rococos in my test town. Seems to happen when you have a companion following you, and It's trying to settle in.
                guardian.Active = true;
                guardian.Spawn();
                guardian.Position.X = SpawnX * 16;
                guardian.Position.Y = (SpawnY - 2) * 16;
            }
            if (!IsGuardianNpcInWorld(GuardianID, ModID))
            {
                AddTownGuardianNpc(guardian);
            }
            guardian.TryFindingTownNpcInfo();
            GuardianTownNpcState npcstate = guardian.GetTownNpcInfo;
            npcstate.HomeX = SpawnXBackup;
            npcstate.HomeY = SpawnYBackup;
            npcstate.Homeless = false;
            npcstate.ValidateHouse();
            string Message = guardian.Name + (SpawnGuardian ? " arrives." : " settles in your world.");
            Color color = (guardian.Base.Male ? new Color(3, 206, 228) : new Color(255, 28, 124));
            if (Main.netMode == 0)
            {
                Main.NewText(Message, color);
            }
            else if (Main.netMode == 2)
            {
                NetMessage.SendData(25, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Message), color.R, color.G, color.B, color.A);
            }
        }

        public static bool Housing_IsRoomTallEnoughForGuardian(GuardianBase gb)
        {
            bool CanMoveIn = (int)(gb.Height * gb.GetScale) / 16 <= WorldGen.roomY2 - WorldGen.roomY1; //(gb.Height * gb.Scale)
            return CanMoveIn;
        }

        public static bool Housing_IsRoomCrowded(GuardianBase tg)
        {
            int TownNPCCounterForHouse = Housing_GetMaxNumberOfHabitants();
            int CountHouseUsers = 0;
            //bool TownNPC = false;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless)
                {
                    if (Housing_IsInRoom(Main.npc[n].homeTileX, Main.npc[n].homeTileY - 1))
                    {
                        //TownNPC = true;
                        //CountHouseUsers++;
                        break;
                    }
                }
                if (CountHouseUsers >= TownNPCCounterForHouse)
                    return true;
            }
            for (int g = 0; g < WorldMod.MaxGuardianNpcsInWorld; g++)
            {
                if (WorldMod.GuardianNPCsInWorld[g] != null && !WorldMod.GuardianNPCsInWorld[g].Homeless)
                {
                    if (Housing_IsInRoom(WorldMod.GuardianNPCsInWorld[g].HomeX, WorldMod.GuardianNPCsInWorld[g].HomeY - 1))
                    {
                        CountHouseUsers++;
                    }
                }
                if (CountHouseUsers >= TownNPCCounterForHouse)
                    return true;
            }
            return false;
        }

        public static void Housing_TryGettingPlaceForCompanionToStay(ref int SpawnX, ref int SpawnY)
        {
            List<Point> ImpossiblePoints = new List<Point>();
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless)
                {
                    ImpossiblePoints.Add(new Point(Main.npc[n].homeTileX, Main.npc[n].homeTileY));
                    //ImpossiblePoints.Add(new Point(Main.npc[n].homeTileX - 1, Main.npc[n].homeTileY));
                }
            }
            for (int g = 0; g < WorldMod.MaxGuardianNpcsInWorld; g++)
            {
                if (WorldMod.GuardianNPCsInWorld[g] != null && !WorldMod.GuardianNPCsInWorld[g].Homeless)
                {
                    ImpossiblePoints.Add(new Point(WorldMod.GuardianNPCsInWorld[g].HomeX, WorldMod.GuardianNPCsInWorld[g].HomeY));
                    //ImpossiblePoints.Add(new Point(WorldMod.GuardianNPCsInWorld[g].HomeX - 1, WorldMod.GuardianNPCsInWorld[g].HomeY));
                }
            }
            List<Point> PossiblePlaces = new List<Point>();
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                int x = WorldGen.roomX[i], y = WorldGen.roomY[i];
                //while (!Main.tile[x, y].active() && !Main.tileSolid[Main.tile[x, y].type] && y < Main.maxTilesY - 20)
                //    y++;
                if (y == Main.maxTilesY - 20)
                    continue;
                if (!ImpossiblePoints.Any(z => z.X == x && z.Y == y) && Housing_IsInRoom(x, y) && !Housing_CheckIfIsCeiling(x, y) && Main.tile[x, y].type == Terraria.ID.TileID.Chairs && Main.tile[x, y - 1].type == Terraria.ID.TileID.Chairs)//TileID.Sets.RoomNeeds.CountsAsChair.Any(t => t == Main.tile[x, y].type) && TileID.Sets.RoomNeeds.CountsAsChair.Any(t => t == Main.tile[x, y - 1].type))
                {
                    SpawnX = x;
                    SpawnY = y;
                    /*if (SpawnX < WorldGen.roomX1 + 3)
                        SpawnX = WorldGen.roomX1 + 3;
                    if (SpawnX > WorldGen.roomX2 - 3)
                        SpawnX = WorldGen.roomX2 - 3;*/
                    break;
                }
            }
        }

        public static bool MoveGuardianToHouse(TerraGuardian tg, int x, int y, bool Silent = false)
        {
            if (!WorldGen.StartRoomCheck(x, y))
            {
                if (!Silent) Main.NewText("Is that a house?", new Color(255, 240, 20), false);
                return false;
            }
            if (!tg.Base.RoomNeeds())
            {
                if (!Silent) Main.NewText("That house lacks something!", new Color(255, 240, 20), false);
            }
            bool Occupied, Evil;
            int Score;
            GetRoomScoreForGuardian(tg.Base, out Occupied, out Evil, out Score);
            if (Score <= 0)
            {
                if (!Silent) Main.NewText("That's not a valid house!", new Color(255, 240, 20), false);
                return false;
            }
            if (Housing_IsRoomCrowded(tg.Base))
            {
                if (!Silent) Main.NewText("Too many people here!", new Color(255, 240, 20), false);
                return false;
            }
            if (!Housing_IsRoomTallEnoughForGuardian(tg.Base))
            {
                if (!Silent) Main.NewText("House is too small! Needs to be " + ((int)(tg.Height * tg.Scale) / 16) + " tiles tall.", new Color(255, 240, 20), false);
                return false;
            }
            GuardianTownNpcState townstate = tg.GetTownNpcInfo;
            if (townstate == null)
            {
                AllowGuardianNPCToSpawn(tg.ID, tg.ModID);
                tg.TryFindingTownNpcInfo();
                townstate = tg.GetTownNpcInfo;
                if (townstate == null)
                {
                    if (!Silent)
                        Main.NewText("The world is too crowded for " + tg.Name + " to move in.", Color.Red, false);
                    return false;
                }
            }
            int HomeX = WorldGen.bestX, HomeY = WorldGen.bestY;
            Housing_TryGettingPlaceForCompanionToStay(ref HomeX, ref HomeY);
            townstate.Homeless = false;
            townstate.HomeX = HomeX;
            townstate.HomeY = HomeY;
            townstate.ValidateHouse();
            return true;
        }

        public static bool BasicRoomNeeds()
        {
            bool HasChair = false, HasEntrance = false, HasTable = false, HasLightsource = false;
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsChair.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsChair[i]])
                {
                    HasChair = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsDoor.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsDoor[i]])
                {
                    HasEntrance = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsTable.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsTable[i]])
                {
                    HasTable = true;
                    break;
                }
            }
            for (int i = 0; i < TileID.Sets.RoomNeeds.CountsAsTorch.Length; i++)
            {
                if (WorldGen.houseTile[TileID.Sets.RoomNeeds.CountsAsTorch[i]])
                {
                    HasLightsource = true;
                    break;
                }
            }
            return HasChair && HasEntrance && HasTable && HasLightsource && !WorldMod.RoomEvil;
        }

        public static int NpcWhoLivesInTheRoom = -1;

        public static void GetRoomScoreForGuardian(GuardianBase gb, out bool Occupied, out bool Evil, out int RoomScore)
        {
            Occupied = Evil = false;
            RoomScore = 0;
            NpcWhoLivesInTheRoom = -1;
            for (int n = 0; n < 200; n++)
            {
                if (!(Main.npc[n].active && Main.npc[n].townNPC && !Main.npc[n].homeless))
                {
                    continue;
                }
                for (int j = 0; j < WorldGen.numRoomTiles; j++)
                {
                    if (Main.npc[n].homeTileX == WorldGen.roomX[j] && Main.npc[n].homeTileY == WorldGen.roomY[j])
                    {
                        //NPC lives here
                        NpcWhoLivesInTheRoom = n;
                        break;
                    }
                }
            }
            int ScoreCounter = 50,
                EvilCounter = 0;
            int Left = WorldGen.roomX1 - Main.zoneX / 8 - 1 - Lighting.offScreenTiles,
                Right = WorldGen.roomX2 + Main.zoneX / 8 + 1 + Lighting.offScreenTiles,
                Up = WorldGen.roomY1 - Main.zoneY / 8 - 1 - Lighting.offScreenTiles,
                Down = WorldGen.roomY2 + Main.zoneY / 8 + 1 + Lighting.offScreenTiles;
            if (Left < 0) Left = 0;
            if (Right >= Main.maxTilesX) Right = Main.maxTilesX - 1;
            if (Up < 0) Up = 0;
            if (Down >= Main.maxTilesY) Down = Main.maxTilesY - 1;
            for (int x = Left + 1; x < Right; x++)
            {
                for (int y = Up + 1; y < Down; y++)
                {
                    if (Main.tile[x, y].active())
                    {
                        int type = Main.tile[x, y].type;
                        if (type == 23 || type == 24 || type == 25 || type == 32 || type == 112 || type == 163 || type == 199 || type == 201 || type == 200 || type == 203 || type == 234)
                        {
                            EvilCounter++;
                        }
                        else if (type == 27)
                        {
                            EvilCounter -= 5;
                        }
                        else if (type == 109 || type == 110 || type == 113 || type == 116 || type == 164)
                        {
                            EvilCounter--;
                        }
                    }
                }
            }
            if (EvilCounter < 50)
                EvilCounter = 0;
            //ScoreCounter -= EvilCounter;
            if (ScoreCounter - EvilCounter <= -250)
            {
                Evil = true;
                //RoomScore = ScoreCounter;
            }
            for (int x = WorldGen.roomX1 + 1; x < WorldGen.roomX2; x++)
            {
                for (int y = WorldGen.roomY1 + 2; y < WorldGen.roomY2 + 2; y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (!tile.nactive())
                        continue;
                    int score = ScoreCounter;
                    if (!(Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && !Collision.SolidTiles(x - 1, x + 1, y - 3, y - 1) && Main.tile[x - 1, y].nactive() && Main.tileSolid[Main.tile[x - 1, y].type] && Main.tile[x + 1, y].nactive() && Main.tileSolid[Main.tile[x + 1, y].type]))
                        continue;
                    for (int x2 = x - 2; x2 < x + 3; x2++)
                    {
                        for (int y2 = y - 4; y2 < y; y2++)
                        {
                            if (!Main.tile[x2, y2].nactive())
                                continue;
                            if (x2 == x)
                            {
                                if (score > 0)
                                {
                                    score -= 15;
                                    if (score < 0)
                                        score = 0;
                                }
                            }
                            else if (Main.tile[x2, y2].type == 21)
                            {
                                if (score > 0)
                                {
                                    score -= 30;
                                    if (score < 1)
                                        score = 1;
                                }
                            }
                            else if (Main.tile[x2, y2].type == 10 || Main.tile[x2, y2].type == 11)
                            {
                                score -= 20;
                            }
                            else if (Main.tileSolid[Main.tile[x2, y2].type])
                            {
                                score -= 5;
                            }
                            else
                            {
                                score += 5;
                            }
                        }
                    }
                    if (score > RoomScore)
                    {
                        bool ValidPosition = Housing_IsInRoom(x, y);
                        bool[] HeightChecker = new bool[3];
                        for (int i = 1; i <= 3; i++)
                        {
                            if (!Main.tile[x, y - i].active() || !Main.tileSolid[Main.tile[x, y - i].type])
                            {
                                HeightChecker[i - 1] = true;
                            }
                            if (!Housing_IsInRoom(x, y - i))
                            {
                                HeightChecker[i - 1] = false;
                            }
                        }
                        for (int i = 0; i < 3; i++)
                        {
                            if (!HeightChecker[i])
                            {
                                ValidPosition = false;
                                break;
                            }
                        }
                        if (ValidPosition && !Housing_CheckIfIsCeiling(x, y))
                        {
                            RoomScore = score;
                            WorldGen.bestX = x;
                            WorldGen.bestY = y;
                        }
                    }
                }
            }
        }

        public static bool Housing_CheckIfIsCeiling(int x, int y)
        {
            for (int i = 0; i < WorldGen.roomCeilingsCount; i++)
            {
                if (WorldGen.roomCeilingX[i] == x && WorldGen.roomCeilingY[i] == y)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool Housing_IsInRoom(int x, int y)
        {
            for (int i = 0; i < WorldGen.numRoomTiles; i++)
            {
                if (WorldGen.roomX[i] == x && WorldGen.roomY[i] == y)
                    return true;
            }
            return false;
        }

        public static void UpdateTileStateOnGuardianHouses(int PositionX, int PositionY, bool Addition)
        {
            Tile tile = MainMod.GetTile(PositionX, PositionY);
            foreach (GuardianBuildingInfo ghi in HouseInfos)
            {
                if (ghi.BelongsToThisHousing(PositionX, PositionY)) // PositionX >= ghi.HouseStartX && PositionX <= ghi.HouseEndX && PositionY >= ghi.HouseStartY && PositionY <= ghi.HouseEndY)
                {
                    ghi.UpdateTileState(tile.type, PositionX, PositionY, Addition);
                }
            }
        }

        public class GuardianBuildingInfo
        {
            public int HomePointX = 0, HomePointY = 0;
            public int HouseStartX = -1, HouseEndX = -1, HouseStartY = -1, HouseEndY = -1;
            public List<FurnitureInfo> furnitures = new List<FurnitureInfo>();
            private List<BytePoint> HousingPoints = new List<BytePoint>();
            public bool ValidHouse = false;
            public List<GuardianTownNpcState> GuardiansLivingHere = new List<GuardianTownNpcState>();

            public void UpdateTileState(ushort Type, int PositionX, int PositionY, bool Addition)
            {
                if (Addition)
                {
                retry:
                    Tile tile = MainMod.GetTile(PositionX, PositionY);
                    bool Add = false, FacingLeft = false;
                    switch (tile.type)
                    {
                        case TileID.Chairs:
                            FacingLeft = tile.frameX < 18;
                            if (tile.frameY % 40 >= 18)
                                Add = true;
                            else
                            {
                                PositionY++;
                                goto retry;
                            }
                            break;
                        case TileID.Thrones:
                            if (tile.frameY % 72 >= 54 && tile.frameX == 18)
                                Add = true;
                            else
                            {
                                if (tile.frameX < 18)
                                    PositionX++;
                                else if (tile.frameX > 18)
                                    PositionX--;
                                if (tile.frameY % 72 < 54)
                                    PositionY++;
                            }
                            break;
                        case TileID.Benches:
                            if (tile.frameY % 36 >= 18 && tile.frameX == 18)
                                Add = true;
                            else
                            {
                                if (tile.frameX < 18)
                                    PositionX++;
                                else if (tile.frameX > 18)
                                    PositionX--;
                                if (tile.frameY % 36 < 18)
                                    PositionY++;
                            }
                            break;
                        case TileID.Beds:
                            //FacingLeft = tile.frameX < 72;
                            if (tile.frameY % 36 >= 18 && tile.frameX % 72 == 36)
                                Add = true;
                            else
                            {
                                int frameX = tile.frameX % 72;
                                /*if (FacingLeft)
                                {
                                    if (frameX < 18)
                                        PositionX++;
                                    else if (frameX > 18)
                                        PositionX--;
                                }
                                else
                                {*/
                                    if (frameX < 36)
                                        PositionX++;
                                    else if (frameX > 36)
                                        PositionX--;
                                //}
                                if (tile.frameY % 36 < 18)
                                    PositionY++;
                            }
                            break;
                        case TileID.Signs:
                        case TileID.AnnouncementBox:
                        case TileID.Tombstones:
                            {
                                PositionX -= (int)(tile.frameX * (1f / 18)) % 2;
                                PositionY -= (int)(tile.frameY * (1f / 18));
                                Add = true;
                            }
                            break;
                    }
                    foreach (FurnitureInfo fi in furnitures)
                    {
                        if (fi.FurnitureID == Type && fi.FurnitureX == PositionX && fi.FurnitureY == PositionY)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                        furnitures.Add(new FurnitureInfo(tile.type, PositionX, PositionY));
                }
                else
                {
                    for (int t = 0; t < furnitures.Count; t++)
                    {
                        if (furnitures[t].FurnitureID == Type)
                        {
                            FurnitureInfo fi = furnitures[t];
                            switch (Type)
                            {
                                case TileID.Chairs:
                                    if (fi.FurnitureX == PositionX && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Thrones:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 3 && PositionY <= fi.FurnitureY)
                                    {
                                        furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Benches:
                                    if (PositionX >= fi.FurnitureX - 1 && PositionX <= fi.FurnitureX + 1 && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Beds:
                                    if (PositionX >= fi.FurnitureX - (fi.FacingLeft ? 1 : 2) && PositionX <= fi.FurnitureX + (fi.FacingLeft ? 2 : 1) && PositionY >= fi.FurnitureY - 1 && PositionY <= fi.FurnitureY)
                                    {
                                        furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                                case TileID.Signs:
                                case TileID.AnnouncementBox:
                                case TileID.Tombstones:
                                    if(PositionX >= fi.FurnitureX && PositionX < fi.FurnitureX + 1 && PositionY >= fi.FurnitureY &&PositionY < fi.FurnitureY + 1)
                                    {
                                        furnitures.RemoveAt(t);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            public void ValidateHouse()
            {
                if (HomePointX == -1 || HomePointY == -1)
                    return;
                HousingPoints.Clear();
                if (WorldGen.StartRoomCheck(HomePointX, HomePointY))
                {
                    HouseStartX = WorldGen.roomX1;
                    HouseEndX = WorldGen.roomX2 + 1;
                    HouseStartY = WorldGen.roomY1;
                    HouseEndY = WorldGen.roomY2 + 1;
                    furnitures.Clear();
                    for (int i = 0; i < WorldGen.numRoomTiles; i++)
                    {
                        int X = WorldGen.roomX[i], Y = WorldGen.roomY[i];
                        Tile tile = MainMod.GetTile(X, Y);
                        UpdateTileState(tile.type, X, Y, true);
                        HousingPoints.Add(new BytePoint((byte)(X - WorldGen.roomX1), (byte)(Y - WorldGen.roomY1)));
                    }
                    ValidHouse = true;
                }
                else
                {
                    ValidHouse = false;
                }
            }

            public bool BelongsToThisHousing(int X, int Y)
            {
                foreach(BytePoint bp in HousingPoints)
                {
                    if (X == bp.X + HouseStartX && Y == bp.Y + HouseStartY)
                        return true;
                }
                return false;
            }

            public struct FurnitureInfo
            {
                public ushort FurnitureID;
                public int FurnitureX, FurnitureY;
                public bool FacingLeft;

                public FurnitureInfo(ushort FID, int FX, int FY, bool FacingLeft = false)
                {
                    FurnitureID = FID;
                    FurnitureX = FX;
                    FurnitureY = FY;
                    this.FacingLeft = FacingLeft;
                }
            }

            public struct BytePoint
            {
                public byte X, Y;
                
                public BytePoint(byte X, byte Y)
                {
                    this.X = X;
                    this.Y = Y;
                }
            }
        }

        public class GuardianTownNpcState
        {
            public GuardianID CharID = new GuardianID();
            public bool Homeless = true;
            public int HomeX = -1, HomeY = -1;
            public GuardianBuildingInfo HouseInfo;

            public GuardianTownNpcState()
            {

            }

            public GuardianTownNpcState(int ID, string ModID)
            {
                this.CharID = new GuardianID(ID, ModID);
            }

            public bool IsID(int ID, string ModID)
            {
                return ID == this.CharID.ID && ModID == this.CharID.ModID;
            }
            
            public bool IsAtHome(Vector2 FeetPosition)
            {
                if (Homeless || HomeX == -1 || HomeY == -1 || HouseInfo == null)
                {
                    return true;
                }
                return FeetPosition.X >= HouseInfo.HouseStartX * 16 && FeetPosition.X <= HouseInfo.HouseEndX * 16 &&
                    FeetPosition.Y >= HouseInfo.HouseStartY * 16 && FeetPosition.Y <= (HouseInfo.HouseEndY + 1) * 16;
            }

            public void ValidateHouse()
            {
                if (HomeX == -1 || HomeY == -1 || Homeless)
                {
                    Homeless = true;
                    foreach(GuardianBuildingInfo ghi in WorldMod.HouseInfos)
                    {
                        for (int i = 0; i < ghi.GuardiansLivingHere.Count; i++)
                        {
                            GuardianTownNpcState tns = ghi.GuardiansLivingHere[i];
                            if (tns == this)
                                ghi.GuardiansLivingHere.RemoveAt(i);
                        }
                    }
                    return;
                }
                foreach (GuardianBuildingInfo ghi in WorldMod.HouseInfos)
                {
                    if(ghi.BelongsToThisHousing(HomeX, HomeY))
                    {
                        if (ghi.ValidHouse)
                        {
                            Homeless = false;
                            HouseInfo = ghi;
                            bool CompanionAlreadyHere = false;
                            foreach(GuardianTownNpcState gtns in ghi.GuardiansLivingHere)
                            {
                                if(gtns.CharID.ID == CharID.ID && gtns.CharID.ModID == CharID.ModID)
                                {
                                    CompanionAlreadyHere = true;
                                    break;
                                }
                            }
                            if (!CompanionAlreadyHere)
                            {
                                ghi.GuardiansLivingHere.Add(this);
                            }
                        }
                        else
                        {
                            Homeless = true;
                        }
                        return;
                    }
                }
                GuardianBuildingInfo newhouseinfo = new GuardianBuildingInfo();
                newhouseinfo.HomePointX = HomeX;
                newhouseinfo.HomePointY = HomeY;
                newhouseinfo.ValidateHouse();
                if (!newhouseinfo.ValidHouse)
                {
                    HouseInfo = null;
                    Homeless = true;
                }
                else
                {
                    WorldMod.HouseInfos.Add(newhouseinfo);
                    Homeless = false;
                    HouseInfo = newhouseinfo;
                }
            }
        }
    }
}
