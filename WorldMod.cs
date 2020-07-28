using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace giantsummon
{
    public class WorldMod : ModWorld
    {
        public const int MaxGuardianNpcsInWorld = 10;
        public static List<KeyValuePair<int, string>> GuardiansMet = new List<KeyValuePair<int, string>>();
        public static bool LastWasDay = false, DelayedWasDay = false, DayChange = false, HourChange = false;
        public static double LastTime = 0;
        public static Dictionary<int, TerraGuardian> guardians = new Dictionary<int, TerraGuardian>();
        public static KeyValuePair<int, string>?[] GuardianNPCsInWorld = new KeyValuePair<int, string>?[MaxGuardianNpcsInWorld];
        public static KeyValuePair<int, string> SpawnGuardian = new KeyValuePair<int, string>(0, "");
        public static int GuardiansMetCount { get { return GuardiansMet.Count; } }

        public static void AllowGuardianNPCToSpawn(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            int EmptySlot = -1;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (EmptySlot == -1 && !GuardianNPCsInWorld[s].HasValue)
                    EmptySlot = s;
                if (GuardianNPCsInWorld[s].HasValue && GuardianNPCsInWorld[s].Value.Key == ID && GuardianNPCsInWorld[s].Value.Value == ModID)
                {
                    return;
                }
            }
            if(EmptySlot > -1)
                GuardianNPCsInWorld[EmptySlot] = new KeyValuePair<int, string>(ID,ModID);
        }

        public static void RemoveGuardianNPCToSpawn(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (GuardianNPCsInWorld[s].HasValue && GuardianNPCsInWorld[s].Value.Key == ID && GuardianNPCsInWorld[s].Value.Value == ModID)
                {
                    GuardianNPCsInWorld[s] = null;
                }
            }
        }

        public static bool CanSpawnGuardianNPC(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for (int s = 0; s < MaxGuardianNpcsInWorld; s++)
            {
                if (GuardianNPCsInWorld[s].HasValue && GuardianNPCsInWorld[s].Value.Key == ID && GuardianNPCsInWorld[s].Value.Value == ModID)
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
                if (!GuardianNPCsInWorld[s].HasValue)
                {
                    return true;
                }
            }
            return false;
        }

        public override void PreUpdate()
        {
            ProjMod.CheckForInactiveProjectiles();
            LastWasDay = DelayedWasDay;
            DelayedWasDay = Main.dayTime;
            DayChange = false;
            HourChange = false;
            if (Main.time == 0)
            {
                AlexRecruitScripts.CheckIfAlexIsInTheWorld();
                if (Main.dayTime)
                    DayChange = true;
            }
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
            if (LastTime != -1 && (int)TimeParser / 3600 != (int)LastTime / 3600)
            {
                HourChange = true;
            }
            LastTime = TimeParser;
            MainMod.TimeTranslated = (float)TimeParser;
        }

        public override void Initialize()
        {
            GuardiansMet.Clear();
            guardians.Clear();
            GuardianNPCsInWorld = new KeyValuePair<int,string>?[MaxGuardianNpcsInWorld];
            Compatibility.NExperienceCompatibility.ResetOnWorldLoad();
            GuardianBountyQuest.Reset();
            AlexRecruitScripts.SpawnedTombstone = false;
            LastTime = -1;
        }

        public override void NetSend(System.IO.BinaryWriter writer)
        {

        }

        public override void NetReceive(System.IO.BinaryReader reader)
        {

        }

        public override void ModifyWorldGenTasks(List<Terraria.World.Generation.GenPass> tasks, ref float totalWeight)
        {
            tasks.Add(new PassLegacy("Spawning Starter Guardian.", delegate(GenerationProgress progress)
            {
                progress.Message = "Spawning Starter Guardian";
                MainMod.GetInitialCompanionsList();
                int NpcID = MainMod.InitialGuardians[Main.rand.Next(MainMod.InitialGuardians.Count)];
                int npc = NPC.NewNPC(Main.spawnTileX * 16, Main.spawnTileY * 16, NpcID);
                ((GuardianNPC.GuardianNPCPrefab)Main.npc[npc].modNPC).UnlockGuardian();
            }));
            tasks.Add(new PassLegacy("Spawning Tombstone.", delegate(GenerationProgress progress)
            {
                AlexRecruitScripts.TrySpawningTombstone(progress);
            }));
        }

        public override Terraria.ModLoader.IO.TagCompound Save()
        {
            Terraria.ModLoader.IO.TagCompound tag = new Terraria.ModLoader.IO.TagCompound();
            tag.Add("ModVersion", MainMod.ModVersion);
            tag.Add("GuardiansMet_Count", GuardiansMet.Count);
            for (int i = 0; i < GuardiansMet.Count; i++)
            {
                tag.Add("GuardiansMet_k" + i, GuardiansMet[i].Key);
                tag.Add("GuardiansMet_v" + i, GuardiansMet[i].Value);
            }
            for (int i = 0; i < MaxGuardianNpcsInWorld; i++)
            {
                tag.Add("GuardiansCanSpawn_HasValue" + i, GuardianNPCsInWorld[i].HasValue);
                if (GuardianNPCsInWorld[i].HasValue)
                {
                    tag.Add("GuardiansCanSpawn_k" + i, GuardianNPCsInWorld[i].Value.Key);
                    tag.Add("GuardiansCanSpawn_v" + i, GuardianNPCsInWorld[i].Value.Value);
                }
            }
            GuardianBountyQuest.Save(tag);
            AlexRecruitScripts.Save(tag);
            tag.Add("DominoDismissed", Npcs.DominoNPC.DominoDismissed);
            return tag;
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            GuardiansMet.Clear();
            if (!tag.ContainsKey("ModVersion"))
                return;
            int Version = tag.GetInt("ModVersion");
            if (Version < 38)
            {
                int[] Ids = tag.GetIntArray("GuardiansMet");
                string ModID = MainMod.mod.Name;
                foreach(int i in Ids)
                    GuardiansMet.Add(new KeyValuePair<int,string>(i, ModID));
            }
            else
            {
                int Count = tag.GetInt("GuardiansMet_Count");
                for (int i = 0; i < Count; i++)
                {
                    int Key = tag.GetInt("GuardiansMet_k" + i);
                    string Mod = tag.GetString("GuardiansMet_v" + i);
                    GuardiansMet.Add(new KeyValuePair<int,string>(Key, Mod));
                }
            }
            if (Version < 39)
            {
                //Add existing guardians to the world.
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab)
                    {
                        GuardianNPC.GuardianNPCPrefab modnpc = Main.npc[n].modNPC as GuardianNPC.GuardianNPCPrefab;
                        WorldMod.AllowGuardianNPCToSpawn(modnpc.GuardianID, modnpc.GuardianModID);
                    }
                }
            }
            else
            {
                int MaxGuardianSpawnCount = MaxGuardianNpcsInWorld;
                if (Version == 39)
                    MaxGuardianSpawnCount = 5;
                for (int i = 0; i < MaxGuardianSpawnCount; i++)
                {
                    bool Exists = tag.GetBool("GuardiansCanSpawn_HasValue" + i);
                    if (Exists)
                    {
                        int ID = tag.GetInt("GuardiansCanSpawn_k" + i);
                        string ModID = tag.GetString("GuardiansCanSpawn_v" + i);
                        GuardianNPCsInWorld[i] = new KeyValuePair<int, string>(ID, ModID);
                    }
                    else
                    {
                        GuardianNPCsInWorld[i] = null;
                    }
                }
            }
            if (Version >= 36)
                GuardianBountyQuest.Load(tag, Version);
            if (Version >= 40)
                AlexRecruitScripts.Load(tag, Version);
            if(Version >= 54)
                Npcs.DominoNPC.DominoDismissed = tag.GetBool("DominoDismissed");
        }
    }
}
