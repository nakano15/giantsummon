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

        private Dictionary<int, RequestBaseNew> Requests = new Dictionary<int, RequestBaseNew>();

        public static void DisposeRequests()
        {
            foreach(string Request in RequestContainers.Keys)
            {
                RequestContainers[Request].Requests.Clear();
            }
            RequestContainers.Clear();
        }

        public static RequestBaseNew GetRequest(int ID, string ModID)
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

        public bool CorruptWorldRequirement(Player player, GuardianData tg)
        {
            return !WorldGen.crimson;
        }

        public bool CrimsonWorldRequirement(Player player, GuardianData tg)
        {
            return WorldGen.crimson;
        }

        public bool CorruptWorldAndBossKilledRequirement(Player player, GuardianData tg)
        {
            return !WorldGen.crimson && NPC.downedBoss2;
        }

        public bool CrimsonWorldAndBossKilledRequirement(Player player, GuardianData tg)
        {
            return WorldGen.crimson && NPC.downedBoss2;
        }

        public bool CorruptWorldHardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode && !WorldGen.crimson;
        }

        public bool CrimsonWorldHardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode && WorldGen.crimson;
        }

        public bool HardmodeRequirement(Player player, GuardianData tg)
        {
            return Main.hardMode;
        }

        public bool AllMechBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public bool AnyMechBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMechBossAny;
        }

        public bool EoCKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss1;
        }

        public bool EvilBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss2;
        }

        public bool SkeletronKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss3;
        }

        public bool AnyFirstBossKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3;
        }

        public bool KingSlimeKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedSlimeKing;
        }

        public bool QueenBeeKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedQueenBee;
        }

        public bool PlanteraKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedPlantBoss;
        }

        public bool GolemKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedGolemBoss;
        }

        public bool MoonlordKillRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMoonlord;
        }

        public bool HalloweenRequirement(Player player, GuardianData tg)
        {
            return Main.halloween;
        }

        public bool XmasRequirement(Player player, GuardianData tg)
        {
            return Main.xMas;
        }

        public bool DefeatedGoblinsRequirement(Player player, GuardianData tg)
        {
            return NPC.downedGoblins;
        }

        public bool DefeatedPiratesRequirement(Player player, GuardianData tg)
        {
            return NPC.downedPirates;
        }

        public bool DefeatedMartiansRequirement(Player player, GuardianData tg)
        {
            return NPC.downedMartians;
        }

        public bool DefeatedFrostLegionRequirement(Player player, GuardianData tg)
        {
            return Main.xMas && NPC.downedFrost;
        }

        public bool BugnetRequirement(Player player, GuardianData tg)
        {
            return player.HasItem(Terraria.ID.ItemID.BugNet) || player.HasItem(Terraria.ID.ItemID.GoldenBugNet);
        }
    }
}
