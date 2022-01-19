using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;

namespace giantsummon
{
    public class RequestContainer
    {
        private static Dictionary<string, RequestContainer> RequestContainers = new Dictionary<string, RequestContainer>();

        private Dictionary<int, RequestBase> Requests = new Dictionary<int, RequestBase>();

        public static void DisposeRequests()
        {
            foreach(string Request in RequestContainers.Keys)
            {
                RequestContainers[Request].Requests.Clear();
            }
            RequestContainers.Clear();
        }

        public static RequestBase GetRequest(int ID, string ModID)
        {
            if (!RequestContainers.ContainsKey(ModID) || !RequestContainers[ModID].Requests.ContainsKey(ID))
                return null;
            return RequestContainers[ModID].Requests[ID];
        }

        public static List<KeyValuePair<string, int>> GetPossibleRequests(GuardianData tg, Player player)
        {
            List<KeyValuePair<string, int>> PossibleRequests = new List<KeyValuePair<string, int>>();
            foreach (string modid in RequestContainers.Keys)
            {
                RequestContainer container = RequestContainers[modid];
                foreach(int id in container.Requests.Keys)
                {
                    if(container.Requests[id].CanGetRequest(player, tg))
                    {
                        PossibleRequests.Add(new KeyValuePair<string, int>(modid, id));
                    }
                }
            }
            return PossibleRequests;
        }

        public RequestContainer(Mod mod)
        {
            if (!RequestContainers.ContainsKey(mod.Name))
                RequestContainers.Add(mod.Name, this);
        }

        public HuntRequestBase AddHuntRequest(int RequestID, int MobID, string MobName = "", int InitialCount = 5, float ExtraFriendshipLevelCount = 0.334f, int RewardValue = 0)
        {
            if (Requests.ContainsKey(RequestID))
                return null;
            HuntRequestBase req = new HuntRequestBase(MobID, MobName, InitialCount, ExtraFriendshipLevelCount, RewardValue);
            Requests.Add(RequestID, req);
            return req;
        }

        public ItemRequestBase AddItemRequest(int RequestID, int ItemID, string ItemName = "", int InitialCount = 5, float ExtraFriendshipLevelCount = 0.334f, int RewardValue = 0)
        {
            if (Requests.ContainsKey(RequestID))
                return null;
            ItemRequestBase req = new ItemRequestBase(ItemID, ItemName, InitialCount, ExtraFriendshipLevelCount, RewardValue);
            Requests.Add(RequestID, req);
            return req;
        }

        public TravelRequestBase AddTravelRequest(int RequestID, int ExtraDistance, int RewardValue = 0)
        {
            if (Requests.ContainsKey(RequestID))
                return null;
            TravelRequestBase req = new TravelRequestBase(ExtraDistance, RewardValue);
            Requests.Add(RequestID, req);
            return req;
        }

        public static bool LifeCrystalUsedRequirement(Player player, GuardianData tg)
        {
            return player.statLifeMax > 100;
        }

        public static bool CorruptWorldRequirement(Player player, GuardianData tg)
        {
            return !WorldGen.crimson;
        }

        public static bool CrimsonWorldRequirement(Player player, GuardianData tg)
        {
            return WorldGen.crimson;
        }

        public static bool CorruptWorldAndBossKilledRequirement(Player player, GuardianData tg)
        {
            return !WorldGen.crimson && NPC.downedBoss2;
        }

        public static bool CrimsonWorldAndBossKilledRequirement(Player player, GuardianData tg)
        {
            return WorldGen.crimson && NPC.downedBoss2;
        }

        public static bool CorruptWorldHardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode && !WorldGen.crimson;
        }

        public static bool CrimsonWorldHardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode && WorldGen.crimson;
        }

        public static bool HardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode;
        }

        public static bool AllMechBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public static bool AnyMechBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMechBossAny;
        }

        public static bool EoCKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss1;
        }

        public static bool EvilBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss2;
        }

        public static bool SkeletronKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss3;
        }

        public static bool AnyFirstBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3;
        }

        public static bool AnyFirstBossKillRequirementCorruption(Player player, GuardianData tg)
        {
            return !WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3);
        }

        public static bool AnyFirstBossKillRequirementCrimson(Player player, GuardianData tg)
        {
            return WorldGen.crimson && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3);
        }

        public static bool KingSlimeKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedSlimeKing;
        }

        public static bool QueenBeeKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedQueenBee;
        }

        public static bool PlanteraKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedPlantBoss;
        }

        public static bool GolemKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedGolemBoss;
        }

        public static bool LunaticCultistKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedAncientCultist;
        }

        public static bool MoonlordKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMoonlord;
        }

        public static bool HalloweenRequirement(Player player, GuardianData tg)
        {
            return Main.halloween;
        }

        public static bool XmasRequirement(Player player, GuardianData tg)
        {
            return Main.xMas;
        }

        public static bool DefeatedGoblinsRequirement(Player player, GuardianData tg)
        {
            return NPC.downedGoblins;
        }

        public static bool DefeatedPiratesRequirement(Player player, GuardianData tg)
        {
            return NPC.downedPirates;
        }

        public static bool DefeatedMartiansRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMartians;
        }

        public static bool DefeatedFrostLegionRequirement(Player player, GuardianData tg)
        {
            return Main.xMas && NPC.downedFrost;
        }

        public static bool BugnetRequirement(Player player, GuardianData tg)
        {
            return player.HasItem(Terraria.ID.ItemID.BugNet) || player.HasItem(Terraria.ID.ItemID.GoldenBugNet);
        }
    }
}
