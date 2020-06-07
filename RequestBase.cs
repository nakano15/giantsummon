using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class RequestBase
    {
        public static RequestBase[] CommonRequests = new RequestBase[0];
        public string Name = "";
        public string BriefText = "", AcceptText = "", DenyText = "", CompleteText = "", RequestInfoText = "";
        public List<RequestObjective> Objectives = new List<RequestObjective>();
        public delegate bool RequestRequirementDel(Terraria.Player player);
        public RequestRequirementDel Requirement = delegate(Terraria.Player player) { return true; };
        public int RequestScore = 500;

        public RequestBase(string Name, int RequestScore, string BriefText = "", string AcceptText = "", string DenyText = "", string CompleteText = "", string RequestInfoText = "")
        {
            this.Name = Name;
            this.RequestScore = RequestScore;
            this.BriefText = BriefText;
            this.AcceptText = AcceptText;
            this.DenyText = DenyText;
            this.CompleteText = CompleteText;
            this.RequestInfoText = RequestInfoText;
        }

        public void AddRequestRequirement(RequestRequirementDel req)
        {
            this.Requirement = req;
        }

        public bool IsRequestDoable(Terraria.Player player, GuardianData gd)
        {
            bool Is = Requirement(player);
            if (Is)
            {
                foreach (RequestObjective ro in Objectives)
                {
                    if (ro.objectiveType == RequestObjective.ObjectiveTypes.CompanionRequirement)
                    {
                        CompanionRequirementRequest req = (CompanionRequirementRequest)ro;
                        if (!PlayerMod.PlayerHasGuardian(player, req.CompanionID, req.CompanionModID))
                        {
                            Is = false;
                            break;
                        }
                    }
                }
            }
            return Is;
        }

        public void AddHuntObjective(int NpcID, int Stack = 5, float StackPerFriendLevel = 0.333f)
        {
            HuntRequestObjective req = new HuntRequestObjective();
            req.NpcID = NpcID;
            req.Stack = Stack;
            req.StackIncreasePerFriendshipLevel = StackPerFriendLevel;
            Objectives.Add(req);
        }

        public void AddItemCollectionRequest(int ItemID, int Stack = 5, float StackPerFriendLevel = 0.333f)
        {
            CollectItemRequest req = new CollectItemRequest();
            req.ItemID = ItemID;
            req.ItemStack = Stack;
            req.StackIncreasePerFriendshipLevel = StackPerFriendLevel;
            Objectives.Add(req);
        }

        public void AddExploreRequest(float InitialDistance, float DistanceIncreasePerFriendLevel = 100f, bool RequiresRequester = true)
        {
            ExploreRequest req = new ExploreRequest();
            req.InitialDistance = InitialDistance;
            req.StackIncreasePerFriendshipLevel = DistanceIncreasePerFriendLevel;
            req.RequiresGuardianActive = RequiresRequester;
            Objectives.Add(req);
        }

        public void AddEventParticipationRequest(int EventID, int WavesToSurvive, float ExtraWavesPerFriendshipLevel = 0.02f)
        {
            EventParticipationRequest req = new EventParticipationRequest();
            req.EventID = EventID;
            req.EventWaves = WavesToSurvive;
            req.ExtraWavesPerFriendshipLevel = ExtraWavesPerFriendshipLevel;
            Objectives.Add(req);
        }

        public void AddEventKillsRequest(int EventID, int KillCount, float ExtraKillsPerFriendshipLevel = 0.5f)
        {
            EventKillRequest req = new EventKillRequest();
            req.EventID = EventID;
            req.InitialKills = KillCount;
            req.ExtraKillsPerFriendshipLevel = ExtraKillsPerFriendshipLevel;
            Objectives.Add(req);
        }

        public void AddRequesterRequirement()
        {
            RequestObjective req = new RequestObjective(RequestObjective.ObjectiveTypes.RequiresRequester);
            Objectives.Add(req);
        }

        public void AddObjectCollectionRequest(string ObjectName, int ObjectCount, float ExtraObjectCountPerFriendshipLevel = 0.333f)
        {
            ObjectCollectionRequest req = new ObjectCollectionRequest();
            req.ObjectName = ObjectName;
            req.ObjectCount = ObjectCount;
            req.ObjectExtraCountPerFriendshipLevel = ExtraObjectCountPerFriendshipLevel;
            Objectives.Add(req);
        }

        public void AddObjectDroppingMonster(int MonsterID, float Rate)
        {
            if (Objectives.Count > 0 && Objectives[Objectives.Count - 1].objectiveType == RequestObjective.ObjectiveTypes.ObjectCollection)
            {
                ObjectCollectionRequest.DropRateFromMonsters rate = new ObjectCollectionRequest.DropRateFromMonsters(MonsterID, Rate);
                ((ObjectCollectionRequest)Objectives[Objectives.Count - 1]).DropFromMobs.Add(rate);
            }
        }

        public void AddCompanionRequirement(int ID, string ModID = "")
        {
            CompanionRequirementRequest req = new CompanionRequirementRequest();
            req.CompanionID = ID;
            if (ModID == "")
                ModID = MainMod.mod.Name;
            req.CompanionModID = ModID;
            Objectives.Add(req);
        }

        public void AddKillBossRequest(int BossID, int GemLevelBonus = 0)
        {
            KillBossRequest req = new KillBossRequest();
            req.BossID = BossID;
            req.DifficultyBonus = GemLevelBonus;
            Objectives.Add(req);
        }

        public class KillBossRequest : RequestObjective
        {
            public int BossID = 0, DifficultyBonus = 0;

            public KillBossRequest()
                : base(ObjectiveTypes.KillBoss)
            {

            }
        }

        public class CompanionRequirementRequest : RequestObjective
        {
            public int CompanionID = 0;
            public string CompanionModID = "";

            public CompanionRequirementRequest()
                : base(ObjectiveTypes.CompanionRequirement)
            {

            }
        }

        public class ObjectCollectionRequest : RequestObjective
        {
            public string ObjectName = "";
            public int ObjectCount = 0;
            public float ObjectExtraCountPerFriendshipLevel = 0.333f;
            public List<DropRateFromMonsters> DropFromMobs = new List<DropRateFromMonsters>();

            public ObjectCollectionRequest()
                : base(ObjectiveTypes.ObjectCollection)
            {

            }

            public struct DropRateFromMonsters
            {
                public int MobID;
                public float DropRate;

                public DropRateFromMonsters(int MobID, float DropRate)
                {
                    this.MobID = MobID;
                    this.DropRate = DropRate;
                }
            }
        }

        public class EventKillRequest : RequestObjective
        {
            public int EventID = 0;
            public int InitialKills = 20;
            public float ExtraKillsPerFriendshipLevel = 0.2f;

            public EventKillRequest()
                : base(ObjectiveTypes.EventKills)
            {

            }
        }

        public class EventParticipationRequest : RequestObjective
        {
            public int EventID = 0;
            public int EventWaves = 1;
            public float ExtraWavesPerFriendshipLevel = 0.02f;

            public EventParticipationRequest()
                : base(ObjectiveTypes.EventParticipation)
            {

            }
        }

        public class ExploreRequest : RequestObjective
        {
            public float InitialDistance = 1000f,
                StackIncreasePerFriendshipLevel = 100f;
            public bool RequiresGuardianActive = true;

            public ExploreRequest()
                : base(ObjectiveTypes.Explore)
            {

            }
        }

        public class CollectItemRequest : RequestObjective
        {
            public int ItemID = 0, ItemStack = 1;
            public float StackIncreasePerFriendshipLevel = 0.5f;

            public CollectItemRequest()
                : base(ObjectiveTypes.CollectItem)
            {

            }
        }

        public class HuntRequestObjective : RequestObjective
        {
            public int NpcID = 0, Stack = 1;
            public float StackIncreasePerFriendshipLevel = 0.5f;

            public HuntRequestObjective() : base(ObjectiveTypes.HuntMonster)
            {

            }
        }

        public class RequestObjective
        {
            public ObjectiveTypes objectiveType = ObjectiveTypes.None;

            public RequestObjective(ObjectiveTypes otype)
            {
                objectiveType = otype;
            }

            public enum ObjectiveTypes
            {
                None,
                HuntMonster,
                CollectItem,
                Explore,
                EventParticipation,
                EventKills,
                RequiresRequester,
                ObjectCollection,
                CompanionRequirement,
                KillBoss
            }
        }

        public static RequestBase.RequestRequirementDel GetBugNetRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        if (player.inventory[i].type == Terraria.ID.ItemID.BugNet || player.inventory[i].type == Terraria.ID.ItemID.GoldenBugNet)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetFishingRodRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    for (int i = 0; i < 50; i++)
                    {
                        if (player.inventory[i].type > 0 && player.inventory[i].fishingPole > 0)
                        {
                            return true;
                        }
                    }
                    return false;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetNightRequestRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return player.statDefense >= 2;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetCorruptionRequestRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return !WorldGen.crimson && (player.statDefense >= 4 || NPC.downedBoss2);
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetCrimsonRequestRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return WorldGen.crimson && (player.statDefense >= 4 || NPC.downedBoss2);
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetDungeonRequestRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return NPC.downedBoss3;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetUnderworldRequestRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    if (!Main.hardMode)
                    {
                        if (NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee)
                            return true;
                        int DefSum = 0;
                        for (int armor = 0; armor < 3; armor++)
                        {
                            if (player.armor[armor].type > 0)
                                DefSum += player.armor[armor].defense;
                        }
                        return DefSum >= 13;
                    }
                    return NPC.downedMechBossAny;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetHardmodeRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return Main.hardMode;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetHardmodeCorruptionRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return Main.hardMode && !WorldGen.crimson;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetHardmodeCrimsonRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return Main.hardMode && WorldGen.crimson;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetHardmodeJungleRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
                };
            }
        }

        public static RequestBase.RequestRequirementDel GetHardmodeDungeonAndTempleRequirement
        {
            get
            {
                return delegate(Player player)
                {
                    return Main.hardMode && NPC.downedPlantBoss;
                };
            }
        }
    }
}
