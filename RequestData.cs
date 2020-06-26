using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public class RequestData
    {
        public bool Active { get { return requestState == RequestState.RequestActive; } }
        public int RequestID = 0;
        
        public int Time = 30 * 60;
        public RequestState requestState = RequestState.Cooldown;
        public const int MinRequestTimer = 1800, MaxRequestTimer = 3600, MinRequestSpawnTime = 600, MaxRequestSpawnTime = 1200;
        public bool RequestCompleted = false;
        public Dictionary<int, int> IntegerVars = new Dictionary<int, int>();
        public Dictionary<int, float> FloatVars = new Dictionary<int, float>();
        public bool IsTalkQuest = false, IsCommonRequest = false;
        public const int MaxRequestCount = 3, RequestsUntilSpecialRequest = 3; //It's hard to focus when you have several requests active, even more since they have a time limit.
        public int RequestCompleteCombo = 0;
        public List<int> RequestsCompletedIDs = new List<int>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gd">Must be the companion having this request.</param>
        /// <returns></returns>
        public RequestBase GetRequestBase(GuardianData gd)
        {
            if (IsCommonRequest)
                return RequestBase.CommonRequests[RequestID];
            return gd.Base.RequestDB[RequestID];
        }

        public bool RequiresGuardianActive(GuardianData d)
        {
            foreach (RequestBase.RequestObjective o in GetRequestBase(d).Objectives)
            {
                if ((o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.Explore && ((RequestBase.ExploreRequest)o).RequiresGuardianActive) || o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester)
                    return true;
            }
            return false;
        }

        public static bool PlayerHasRequestRequiringCompanion(Player p, GuardianData d)
        {
            if (!d.request.IsTalkQuest && d.request.requestState == RequestState.RequestActive)
            {
                RequestBase rb = d.request.GetRequestBase(d);
                if (rb.Objectives.Any(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester))
                    return true;
                foreach (RequestBase.ExploreRequest rbxp in rb.Objectives.Where(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.Explore))
                {
                    if (rbxp.RequiresGuardianActive)
                        return true;
                }
            }
            PlayerMod pm = p.GetModPlayer<PlayerMod>();
            foreach (int r in pm.MyGuardians.Keys)
            {
                GuardianData gd = pm.MyGuardians[r];
                if (!gd.request.IsTalkQuest && gd.request.requestState == RequestState.RequestActive)
                {
                    RequestBase rb = gd.request.GetRequestBase(gd);
                    foreach (RequestBase.CompanionRequirementRequest rbcomp in rb.Objectives.Where(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement))
                    {
                        if (rbcomp.CompanionID == d.ID && rbcomp.CompanionModID == d.ModID)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int GetIntegerValue(int ObjectiveID)
        {
            if (!IntegerVars.ContainsKey(ObjectiveID))
            {
                IntegerVars.Add(ObjectiveID, 0);
            }
            return IntegerVars[ObjectiveID];
        }

        public float GetFloatValue(int ObjectiveID)
        {
            if (!FloatVars.ContainsKey(ObjectiveID))
            {
                FloatVars.Add(ObjectiveID, 0);
            }
            return FloatVars[ObjectiveID];
        }

        public void SetIntegerValue(int ObjectiveID, int Value)
        {
            if (!IntegerVars.ContainsKey(ObjectiveID))
            {
                IntegerVars.Add(ObjectiveID, 0);
            }
            IntegerVars[ObjectiveID] = Value;
        }

        public void SetFloatValue(int ObjectiveID, float Value)
        {
            if (!FloatVars.ContainsKey(ObjectiveID))
            {
                FloatVars.Add(ObjectiveID, 0);
            }
            FloatVars[ObjectiveID] = Value;
        }

        public void UponAccepting()
        {
            Time = Main.rand.Next(MinRequestTimer, MaxRequestTimer) * 60;
            requestState = RequestState.RequestActive;
        }

        public void UponRejecting()
        {
            Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime) * 60;
            requestState = RequestState.Cooldown;
        }

        public void Save(Terraria.ModLoader.IO.TagCompound tag, int UniqueID)
        {
            string IDText = "_"+UniqueID;
            tag.Add("RequestID" + IDText, RequestID);
            tag.Add("RequestTime" + IDText, Time);
            tag.Add("RequestState" + IDText, (byte)requestState);
            tag.Add("RequestIsTalk" + IDText, IsTalkQuest);
            tag.Add("RequestIsCommon" + IDText, IsCommonRequest);
            tag.Add("RequestIntegers" + IDText, IntegerVars.Count);
            byte Counter = 0;
            foreach (int Key in IntegerVars.Keys)
            {
                tag.Add("RequestIntegerKey" + Counter + IDText, Key);
                tag.Add("RequestIntegerValue" + Counter + IDText, IntegerVars[Key]);
                Counter++;
            }
            tag.Add("RequestFloats" + IDText, FloatVars.Count);
            Counter = 0;
            foreach (int Key in FloatVars.Keys)
            {
                tag.Add("RequestIntegerKey" + Counter + IDText, Key);
                tag.Add("RequestIntegerValue" + Counter + IDText, FloatVars[Key]);
                Counter++;
            }
            tag.Add("RequestsCompletedCount" + IDText, RequestCompleteCombo);
            tag.Add("RequestsSpecialCompletedIDCount" + IDText, RequestsCompletedIDs.Count);
            for (int i = 0; i < RequestsCompletedIDs.Count; i++)
            {
                tag.Add("RequestsCompleteID_"+i + IDText, RequestsCompletedIDs[i]);
            }
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag, int ModVersion, int UniqueID, GuardianData gd)
        {
            string IDText = "_" + UniqueID;
            RequestID = tag.GetInt("RequestID" + IDText);
            IsTalkQuest = tag.GetBool("RequestIsTalk" + IDText);
            if (ModVersion >= 62)
            {
                IsCommonRequest = tag.GetBool("RequestIsCommon" + IDText);
            }
            RequestState requestState = (RequestState)tag.GetByte("RequestState" + IDText);
            if (requestState >= RequestState.HasRequestReady)
            {
                if (IsTalkQuest)
                {
                    CreateTalkRequest();
                }
                else
                {
                    ChangeRequest(gd, RequestID, IsCommonRequest);
                }
            }
            this.requestState = requestState;
            Time = tag.GetInt("RequestTime" + IDText);
            int MaxKeys = tag.GetInt("RequestIntegers" + IDText);
            IntegerVars.Clear();
            for (int k = 0; k < MaxKeys; k++)
            {
                int Key = tag.GetInt("RequestIntegerKey" + k + IDText);
                int Value = tag.GetInt("RequestIntegerValue" + k + IDText);
                IntegerVars.Add(Key, Value);
            }
            MaxKeys = tag.GetInt("RequestFloats" + IDText);
            FloatVars.Clear();
            for (int k = 0; k < MaxKeys; k++)
            {
                int Key = tag.GetInt("RequestIntegerKey" + k + IDText);
                float Value = tag.GetFloat("RequestIntegerValue" + k + IDText);
                FloatVars.Add(Key, Value);
            }
            if (ModVersion >= 62)
            {
                RequestCompleteCombo = tag.GetInt("RequestsCompletedCount" + IDText);
                MaxKeys = tag.GetInt("RequestsSpecialCompletedIDCount" + IDText);
                RequestsCompletedIDs.Clear();
                for (int i = 0; i < MaxKeys; i++)
                {
                    RequestsCompletedIDs.Add(tag.GetInt("RequestsCompleteID_" + i + IDText));
                }
            }
        }

        public bool CountObjective(GuardianData gd, Player player)
        {
            bool Count = !RequiresGuardianActive(gd) || PlayerMod.HasGuardianSummoned(player, gd.ID, gd.ModID);
            if (Count)
            {
                RequestBase rb = GetRequestBase(gd);
                RequestBase.RequestObjective[] objs = rb.Objectives.Where(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement).ToArray();
                foreach (RequestBase.RequestObjective obj in objs)
                {
                    RequestBase.CompanionRequirementRequest comp = (RequestBase.CompanionRequirementRequest)obj;
                    if (!PlayerMod.HasGuardianSummoned(player, comp.CompanionID, comp.CompanionModID))
                    {
                        Count = false;
                        break;
                    }
                }
            }
            return Count;
        }

        public void SpawnNewRequest(GuardianData gd, PlayerMod player)
        {
            List<int> Requests = new List<int>();
            bool MakeCommonRequest = RequestCompleteCombo < RequestsUntilSpecialRequest;
            if (!MakeCommonRequest) //First, when not making a common request, check if there's special request unlocked.
            {
                List<int> PossibleRequests = new List<int>(), NotCompletedRequests = new List<int>();
                for (int req = 0; req < gd.Base.RequestDB.Count; req++)
                {
                    if (gd.Base.RequestDB[req].IsRequestDoable(player.player, gd))
                    {
                        PossibleRequests.Add(req);
                        if (!RequestsCompletedIDs.Contains(req))
                        {
                            NotCompletedRequests.Add(req);
                        }
                    }
                }
                if (NotCompletedRequests.Count > 0) //If there is not yet completed special request that the player can do at the moment, pick it.
                {
                    Requests.AddRange(NotCompletedRequests);
                }
                else if (PossibleRequests.Count > 0) //Otherwise, check for all other special requests to pick at random.
                {
                    Requests.AddRange(PossibleRequests);
                }
                else //If It all fails, generate a common request.
                {
                    MakeCommonRequest = true;
                }
            }
            if (MakeCommonRequest)
            {
                for (int req = 0; req < RequestBase.CommonRequests.Length; req++)
                {
                    if (RequestBase.CommonRequests[req].IsRequestDoable(player.player, gd))
                    {
                        Requests.Add(req);
                    }
                }
            }
            bool HasCompanionSummonedOrInTheWorld = (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID) || NpcMod.HasGuardianNPC(gd.ID, gd.ModID));
            bool GotRequest = false;
            if ((gd.FriendshipLevel == 0 || Main.rand.NextDouble() < 0.333f) && HasCompanionSummonedOrInTheWorld) //A talk request should never reset the quest chain.
            {
                CreateTalkRequest();
                Main.NewText(gd.Name + " wants to speak with you.");
                GotRequest = true;
            }
            else if (Requests.Count > 0 && HasCompanionSummonedOrInTheWorld)
            {
                ChangeRequest(gd, Requests[Main.rand.Next(Requests.Count)], MakeCommonRequest);
                Main.NewText(gd.Name + " has a request for you.");
                GotRequest = true;
            }
            else
            {
                Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime) * 60;
            }
            if (GotRequest && !player.TutorialRequestIntroduction)
            {
                player.TutorialRequestIntroduction = true;
                Main.NewText("Someone gave you a request. Helping them will reward with friendship experience, and also with some interesting rewards.");
            }
        }

        public void UpdateRequest(GuardianData gd, PlayerMod player)
        {
            switch (requestState)
            {
                case RequestState.Cooldown:
                    {
                        Time--;
                        if (Time <= 0)
                        {
                            Time = 0;
                            if (player.player.whoAmI == Main.myPlayer)
                            {
                                SpawnNewRequest(gd, player);
                            }
                        }
                    }
                    break;
                case RequestState.RequestActive:
                    {
                        Time--;
                        if (Time <= 0)
                        {
                            requestState = RequestState.Cooldown;
                            Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime) * 60;
                        }
                        else
                        {
                            if (IsTalkQuest)
                            {
                                if (GetIntegerValue(0) == 0)
                                {
                                    if (player.player.talkNPC > -1 && Main.npc[player.player.talkNPC].modNPC is GuardianNPC.GuardianNPCPrefab)
                                    {
                                        GuardianNPC.GuardianNPCPrefab gnpc = (GuardianNPC.GuardianNPCPrefab)Main.npc[player.player.talkNPC].modNPC;
                                        if (gnpc.GuardianID == gd.ID && gnpc.GuardianModID == gd.ModID && CompleteRequest(gnpc.Guardian, gd, player))
                                        {
                                            SetIntegerValue(0, 1);
                                            Main.npcChatText = gd.Base.TalkMessage(player.player, gnpc.Guardian);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool CanCountObjective = CountObjective(gd, player.player);
                                int ObjectivesCompleted = 0, ObjectiveCount = 0;
                                RequestBase rb = GetRequestBase(gd);
                                for (int o = 0; o < rb.Objectives.Count; o++)
                                {
                                    ObjectiveCount++;
                                    switch (rb.Objectives[o].objectiveType)
                                    {
                                        default:
                                            {
                                                ObjectivesCompleted++;
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement:
                                            {
                                                RequestBase.CompanionRequirementRequest req = (RequestBase.CompanionRequirementRequest)rb.Objectives[o];
                                                if (player.GetAllGuardianFollowers.Any(x => x.Active && x.ID == req.CompanionID && x.ModID == req.CompanionModID))
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                            {
                                                if (GetIntegerValue(o) <= 0)
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                            {
                                                RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)rb.Objectives[o];
                                                int ItemStack = 0;
                                                for (int i = 0; i < 58; i++)
                                                {
                                                    if (player.player.inventory[i].type == req.ItemID)
                                                    {
                                                        ItemStack += player.player.inventory[i].stack;
                                                    }
                                                }
                                                if (ItemStack >= GetIntegerValue(o))
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                                            {
                                                RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)rb.Objectives[o];
                                                float DistanceStack = GetFloatValue(o);
                                                if (DistanceStack <= 0)
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                                else
                                                {
                                                    foreach (TerraGuardian guardian in player.GetAllGuardianFollowers)
                                                    {
                                                        if (guardian.ID == gd.ID && guardian.ModID == gd.ModID)
                                                        {
                                                            float Speed = guardian.Velocity.X;
                                                            if (guardian.MountedOnPlayer || guardian.SittingOnPlayerMount)
                                                                Speed = player.player.velocity.X;
                                                            DistanceStack -= Math.Abs(Speed) * 0.05f;
                                                            SetFloatValue(o, DistanceStack);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                                            {
                                                if (GetIntegerValue(o) > 0)
                                                {
                                                    RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)rb.Objectives[o];
                                                    if (CanCountObjective && Main.invasionType == req.EventID && Main.invasionProgressWave != MainMod.LastEventWave && MainMod.LastEventWave > 0)
                                                    {
                                                        switch ((EventList)req.EventID)
                                                        {
                                                            case EventList.PumpkinMoon:
                                                            case EventList.FrostMoon:
                                                            case EventList.DD2Event:
                                                                SetIntegerValue(0, GetIntegerValue(o) - 1);
                                                                break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.EventKills:
                                        case RequestBase.RequestObjective.ObjectiveTypes.ObjectCollection:
                                            {
                                                if (GetIntegerValue(o) <= 0)
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.None:
                                            {
                                                ObjectivesCompleted++;
                                            }
                                            break;
                                    }
                                }
                                RequestCompleted = ObjectivesCompleted >= ObjectiveCount;
                            }
                        }
                    }
                    break;
            }
        }

        public bool CompleteRequest(TerraGuardian guardian, GuardianData gd, PlayerMod player)
        {
            if (RequestCompleted || IsTalkQuest)
            {
                int RewardScore = (IsTalkQuest ? 500 : GetRequestBase(gd).RequestScore + 200);
                if (IsTalkQuest)
                {
                    if (guardian.ID == gd.ID && guardian.ModID == gd.ModID)
                    {
                        if (player.player.talkNPC > -1)
                        {
                            Main.npcChatText = GuardianNPC.GuardianNPCPrefab.MessageParser(gd.Base.TalkMessage(player.player, guardian), guardian);
                        }
                        else
                        {
                            guardian.SaySomething(GuardianNPC.GuardianNPCPrefab.MessageParser(gd.Base.TalkMessage(player.player, guardian), guardian), true);
                            //Main.NewText(guardian.Name + ": " + GuardianNPC.GuardianNPCPrefab.MessageParser(gd.Base.TalkMessage(player.player, guardian), guardian));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    RequestBase rb = GetRequestBase(gd);
                    for (int o = 0; o < rb.Objectives.Count; o++)
                    {
                        switch (rb.Objectives[o].objectiveType)
                        {
                            case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                {
                                    RequestBase.HuntRequestObjective req = ((RequestBase.HuntRequestObjective)rb.Objectives[o]);
                                    RewardScore += (int)(req.Stack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel) * 50;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                {
                                    RequestBase.CollectItemRequest req = ((RequestBase.CollectItemRequest)rb.Objectives[o]);
                                    int ItemID = req.ItemID;
                                    int Stack = GetIntegerValue(o);
                                    Player p = player.player;
                                    for (int i = 57; i >= 0; i--)
                                    {
                                        if (p.inventory[i].type == ItemID)
                                        {
                                            if (p.inventory[i].stack < Stack)
                                            {
                                                Stack -= p.inventory[i].stack;
                                                p.inventory[i].SetDefaults(0);
                                            }
                                            else
                                            {
                                                p.inventory[i].stack -= Stack;
                                                Stack = 0;
                                                if (p.inventory[i].stack == 0)
                                                    p.inventory[i].SetDefaults(0);
                                            }
                                        }
                                        if (Stack == 0)
                                            break;
                                    }
                                    RewardScore += (int)(req.ItemStack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel) * 50;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                                {
                                    RequestBase.ExploreRequest req = ((RequestBase.ExploreRequest)rb.Objectives[o]);
                                    RewardScore += (int)((req.InitialDistance + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel) * 8);
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement:
                                {
                                    RewardScore += 200;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.EventKills:
                                {
                                    RequestBase.EventKillRequest req = ((RequestBase.EventKillRequest)rb.Objectives[o]);
                                    RewardScore += (int)(req.InitialKills + req.ExtraKillsPerFriendshipLevel * gd.FriendshipLevel) * 10;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                                {
                                    RequestBase.EventParticipationRequest req = ((RequestBase.EventParticipationRequest)rb.Objectives[o]);
                                    RewardScore += (int)(req.EventWaves + req.ExtraWavesPerFriendshipLevel * gd.FriendshipLevel) * 80;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.ObjectCollection:
                                {
                                    RequestBase.ObjectCollectionRequest req = ((RequestBase.ObjectCollectionRequest)rb.Objectives[o]);
                                    int ScoreStack = (int)(req.ObjectCount + req.ObjectExtraCountPerFriendshipLevel * gd.FriendshipLevel) * 30;
                                    float Rates = 0, Total = 0;
                                    foreach (RequestBase.ObjectCollectionRequest.DropRateFromMonsters rate in req.DropFromMobs)
                                    {
                                        Rates += rate.DropRate;
                                        Total += 1;
                                    }
                                    ScoreStack = (int)(ScoreStack * (1f - (Rates / Total) + 0.5f) * 10);
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester:
                                {
                                    RewardScore += 200;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.KillBoss:
                                {
                                    RewardScore += 600;
                                    RequestBase.KillBossRequest req = ((RequestBase.KillBossRequest)rb.Objectives[o]);
                                    RewardScore += req.DifficultyBonus * req.DifficultyBonus * 30;
                                }
                                break;
                        }
                    }
                }
                RewardScore += (int)(RewardScore * 0.1f * gd.FriendshipLevel);
                Item[] Rewards = gd.GetRewards(RewardScore, player.player);
                foreach (Item i in Rewards)
                {
                    Item ni = Main.item[Item.NewItem(player.player.getRect(), i.type, i.stack)];
                    ni.owner = player.player.whoAmI;
                }
                RequestCompleted = false;
                gd.IncreaseFriendshipProgress(1);
                requestState = RequestState.Cooldown;
                Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime) * 60;
                foreach (TerraGuardian tg in player.GetAllGuardianFollowers)
                {
                    if (tg.Active && tg.FriendshipLevel < tg.Base.CallUnlockLevel && !PlayerHasRequestRequiringCompanion(player.player, tg.Data))
                    {
                        Main.NewText(tg.Name + " was dismissed");
                        player.DismissGuardian(tg.AssistSlot);
                    }
                }
                if (!IsCommonRequest && !IsTalkQuest)
                    RequestCompleteCombo = 0;
                else
                    RequestCompleteCombo++;
                if (!IsCommonRequest && !IsTalkQuest && RequestsCompletedIDs.Contains(RequestID))
                {
                    RequestsCompletedIDs.Add(RequestID);
                }
                return true;
            }
            return false;
        }

        public string GetRequestBrief(GuardianData gd, TerraGuardian giver)
        {
            if (IsCommonRequest)
            {
                string Text = gd.Base.HasRequestMessage(Main.player[Main.myPlayer], giver);
                return Text;
            }
            return GetRequestBase(gd).BriefText;
        }

        public string GetRequestAccept(GuardianData gd)
        {
            if (IsCommonRequest)
                return "(You have accepted the request.)";
            return GetRequestBase(gd).AcceptText;
        }

        public string GetRequestDeny(GuardianData gd)
        {
            if (IsCommonRequest)
                return "(Request rejected.)";
            return GetRequestBase(gd).DenyText;
        }

        public string GetRequestComplete(GuardianData gd, TerraGuardian giver)
        {
            if (IsCommonRequest)
                return gd.Base.CompletedRequestMessage(Main.player[Main.myPlayer], giver);
            return GetRequestBase(gd).CompleteText;
        }

        public string GetRequestInfo(GuardianData gd)
        {
            return GetRequestBase(gd).RequestInfoText;
        }

        public string[] GetRequestText(Player player, GuardianData gd, bool ForceShowObjective = false)
        {
            List<string> QuestObjectives = new List<string>();
            bool ShowDuration = false;
            RequestState reqstate = requestState;
            if (ForceShowObjective)
                reqstate = RequestState.RequestActive;
            switch (reqstate)
            {
                case RequestState.Cooldown:
                    QuestObjectives.Add(gd.Name + " has no requests right now.");
                    break;
                case RequestState.HasRequestReady:
                    if (IsTalkQuest)
                    {
                        QuestObjectives.Add(gd.Name + " want to talk to you.");
                    }
                    else
                    {
                        QuestObjectives.Add(gd.Name + " has something for you.");
                    }
                    break;
                case RequestState.RequestActive:
                    {
                        ShowDuration = true;
                        bool HasPendingObjective = false;
                        if (IsTalkQuest)
                        {
                            QuestObjectives.Add("" + gd.Name + " wants to talk to you.");
                        }
                        else
                        {
                            RequestBase rb = GetRequestBase(gd);
                            for (int o = 0; o < rb.Objectives.Count; o++)
                            {
                                switch (rb.Objectives[o].objectiveType)
                                {
                                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                        {
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                RequestBase.HuntRequestObjective req = (RequestBase.HuntRequestObjective)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Hunt " + GetIntegerValue(o) + " " + Lang.GetNPCName(req.NpcID) + ".");
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                        {
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Collect " + GetIntegerValue(o) + " " + Lang.GetItemName(req.ItemID) + ".");
                                            }

                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                                        {
                                            if (GetFloatValue(o) > 0)
                                            {
                                                RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                if (req.RequiresGuardianActive)
                                                {
                                                    QuestObjectives.Add(" Travel with " + gd.Name + ".");
                                                }
                                                else
                                                {
                                                    QuestObjectives.Add(" Explore the world.");
                                                }
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                                        {
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                string EventName = GetEventName(req.EventID);
                                                if (req.EventID == (int)EventList.FrostMoon || req.EventID == (int)EventList.PumpkinMoon || req.EventID == (int)EventList.DD2Event)
                                                {
                                                    QuestObjectives.Add(" Survive " + GetIntegerValue(o) + " waves in the " + EventName + " event.");
                                                }
                                                else
                                                {
                                                    QuestObjectives.Add(" Face " + EventName + " " + GetIntegerValue(o) + " times.");
                                                }
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester:
                                        {
                                            QuestObjectives.Add(" Requires " + gd.Name + " in the group.");
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.EventKills:
                                        {
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                string EventName = GetEventName(req.EventID);
                                                QuestObjectives.Add(" Defeat " + GetIntegerValue(o) + " foes in the " + EventName + " event.");
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.ObjectCollection:
                                        {
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                RequestBase.ObjectCollectionRequest req = (RequestBase.ObjectCollectionRequest)rb.Objectives[o];
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Collect " + GetIntegerValue(o) + " " + req.ObjectName + " from:");
                                                if (req.DropFromMobs.Count == 0)
                                                    QuestObjectives.Add("  Anything.");
                                                else
                                                {
                                                    foreach (RequestBase.ObjectCollectionRequest.DropRateFromMonsters rate in req.DropFromMobs)
                                                    {
                                                        QuestObjectives.Add("  " + Lang.GetNPCName(rate.MobID) + " (" + Math.Round(rate.DropRate * 100, 2) + "%)");
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement:
                                        {
                                            RequestBase.CompanionRequirementRequest req = (RequestBase.CompanionRequirementRequest)rb.Objectives[o];
                                            if (!PlayerMod.PlayerHasGuardian(player, req.CompanionID, req.CompanionModID))
                                            {
                                                QuestObjectives.Add("  You don't know " + GuardianBase.GetGuardianBase(req.CompanionID, req.CompanionModID).Name + " yet.");
                                            }
                                            else
                                            {
                                                GuardianData data = PlayerMod.GetPlayerGuardian(player, req.CompanionID, req.CompanionModID);
                                                if (PlayerMod.HasGuardianSummoned(player, req.CompanionID, req.CompanionModID))
                                                {
                                                    QuestObjectives.Add("  " + data.Name + " is in your team.");
                                                }
                                                else
                                                {
                                                    QuestObjectives.Add("  Need " + data.Name + " in the team.");
                                                }
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.KillBoss:
                                        {
                                            if (GetIntegerValue(o) == 0)
                                            {
                                                RequestBase.KillBossRequest req = (RequestBase.KillBossRequest)rb.Objectives[o];
                                                string BossName = "";
                                                if (req.BossID == Terraria.ID.NPCID.Spazmatism || req.BossID == Terraria.ID.NPCID.Retinazer)
                                                {
                                                    BossName = "The Twins";
                                                }
                                                else if (req.BossID == Terraria.ID.NPCID.EaterofWorldsBody || req.BossID == Terraria.ID.NPCID.EaterofWorldsHead || req.BossID == Terraria.ID.NPCID.EaterofWorldsTail)
                                                {
                                                    BossName = "Eater of Worlds";
                                                }
                                                else if (req.BossID == Terraria.ID.NPCID.TheDestroyer || req.BossID == Terraria.ID.NPCID.TheDestroyerBody || req.BossID == Terraria.ID.NPCID.TheDestroyerTail)
                                                {
                                                    BossName = "The Destroyer";
                                                }
                                                else
                                                {
                                                    BossName = Lang.GetNPCName(req.BossID).Value;
                                                }
                                                QuestObjectives.Add("  Defeat " + BossName + ".");
                                            }
                                        }
                                        break;
                                }
                            }
                            if (!ForceShowObjective)
                            {
                                if (HasPendingObjective)
                                {
                                    QuestObjectives.Insert(0, "Help " + gd.Name + " by:");
                                    if (!IsCommonRequest) QuestObjectives.Insert(0, "[" + rb.Name + "]");
                                }
                                else
                                {
                                    if (!IsCommonRequest) QuestObjectives.Add("[" + rb.Name + "]");
                                    QuestObjectives.Add(" Report success to " + gd.Name + ".");
                                }
                            }
                        }
                        break;
                    }
            }
            if (ShowDuration && !ForceShowObjective)
            {
                int Seconds = Time / 60, Minutes = 0, Hours = 0, Days = 0;
                if (Seconds >= 60)
                {
                    Minutes += Seconds / 60;
                    Seconds -= Minutes * 60;
                }
                if (Minutes >= 60)
                {
                    Hours += Minutes / 60;
                    Minutes -= Hours * 60;
                }
                if (Hours >= 24)
                {
                    Days += Hours / 24;
                    Hours -= Days * 24;
                }
                string T = "";
                if (Days > 0)
                {
                    T += Days + " Days";
                }
                if (Hours > 0)
                {
                    if (T != "")
                        T += ", ";
                    T += Hours + " Hours";
                }
                if (Minutes > 0)
                {
                    if (T != "")
                        T += ", ";
                    T += Minutes + " Minutes";
                }
                if (Seconds > 0 || T == "")
                {
                    if (T != "")
                        T += ", ";
                    T += Seconds + " Seconds";
                }
                QuestObjectives.Add("Duration: " + T + ".");
            }
            return QuestObjectives.ToArray();
        }

        public bool TrySpawningBoss(int PlayerID, int ID, int DifficultyBonus = 0)
        {
            bool Success = false;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && (Main.npc[n].boss || Main.npc[n].type == Terraria.ID.NPCID.EaterofWorldsHead))
                {
                    return false;
                }
            }
            if(ID == Terraria.ID.NPCID.EaterofWorldsBody || ID== Terraria.ID.NPCID.EaterofWorldsTail)
                ID = Terraria.ID.NPCID.EaterofWorldsHead;
            if(ID == Terraria.ID.NPCID.TheDestroyerBody || ID == Terraria.ID.NPCID.TheDestroyerTail)
                ID = Terraria.ID.NPCID.TheDestroyer;
            if (ID == Terraria.ID.NPCID.EyeofCthulhu || ID == Terraria.ID.NPCID.SkeletronHead || ID == Terraria.ID.NPCID.Spazmatism || ID == Terraria.ID.NPCID.Retinazer ||
                ID == Terraria.ID.NPCID.TheDestroyer)
            {
                if (!Main.dayTime && Main.time < 2.5f * 3600)
                {
                    NPC.SpawnOnPlayer(PlayerID, ID);
                    if (ID == Terraria.ID.NPCID.Spazmatism)
                        NPC.SpawnOnPlayer(PlayerID, Terraria.ID.NPCID.Retinazer);
                    else if (ID == Terraria.ID.NPCID.Retinazer)
                        NPC.SpawnOnPlayer(PlayerID, Terraria.ID.NPCID.Spazmatism);
                    Success = true;
                }
            }
            else if (ID == Terraria.ID.NPCID.EaterofWorldsHead && Main.player[PlayerID].ZoneCorrupt)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == Terraria.ID.NPCID.BrainofCthulhu && Main.player[PlayerID].ZoneCrimson)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == Terraria.ID.NPCID.WallofFlesh && Main.player[PlayerID].ZoneUnderworldHeight)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == Terraria.ID.NPCID.KingSlime)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == Terraria.ID.NPCID.QueenBee && Main.player[PlayerID].ZoneJungle)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            if (Success)
            {
                for (int n = 0; n < 200; n++)
                {
                    if (!Main.npc[n].active)
                        continue;
                    if (Main.npc[n].type == ID || (ID == Terraria.ID.NPCID.Spazmatism && Main.npc[n].type == Terraria.ID.NPCID.Retinazer) ||
                        (ID == Terraria.ID.NPCID.Retinazer && Main.npc[n].type == Terraria.ID.NPCID.Spazmatism) || 
                        (Main.npc[n].type >= Terraria.ID.NPCID.EaterofWorldsHead && Main.npc[n].type <= Terraria.ID.NPCID.EaterofWorldsTail && ID == Terraria.ID.NPCID.EaterofWorldsHead))
                    {
                        int Difficulty = (int)Main.npc[n].GetGlobalNPC<NpcMod>().mobType + DifficultyBonus;
                        if (Difficulty >= Enum.GetValues(typeof(MobTypes)).Length)
                        {
                            Difficulty = Enum.GetValues(typeof(MobTypes)).Length - 1;
                        }
                        Main.npc[n].GetGlobalNPC<NpcMod>().mobType = (MobTypes)Difficulty;
                    }
                }
            }
            return Success;
        }

        public string GetEventName(int ID)
        {
            string EventName = "";
            switch ((EventList)ID)
            {
                case EventList.GoblinArmy:
                    EventName = "Goblin Army";
                    break;
                case EventList.PirateArmy:
                    EventName = "Pirate Invasion";
                    break;
                case EventList.MartianArmy:
                    EventName = "Martian Madness";
                    break;
                case EventList.FrostArmy:
                    EventName = "Frost Legion";
                    break;
                case EventList.PumpkinMoon:
                    EventName = "Pumpkin Moon";
                    break;
                case EventList.FrostMoon:
                    EventName = "Frost Moon";
                    break;
                case EventList.DD2Event:
                    EventName = "Old One's Army";
                    break;
            }
            return EventName;
        }

        public void CreateTalkRequest()
        {
            IntegerVars.Clear();
            FloatVars.Clear();
            RequestID = 0;
            IsTalkQuest = true;
            IsCommonRequest = false;
            requestState = RequestState.HasRequestReady;
        }

        public void ChangeRequest(GuardianData gd, int ID, bool CommonRequest = false)
        {
            IntegerVars.Clear();
            FloatVars.Clear();
            RequestID = ID;
            IsTalkQuest = false;
            requestState = RequestState.HasRequestReady;
            this.IsCommonRequest = CommonRequest;
            RequestBase rb = GetRequestBase(gd);
            for (int o = 0; o < rb.Objectives.Count; o++)
            {
                switch (rb.Objectives[o].objectiveType)
                {
                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                        {
                            RequestBase.HuntRequestObjective req = (RequestBase.HuntRequestObjective)rb.Objectives[o];
                            SetIntegerValue(o, (int)(req.Stack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                        {
                            RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)rb.Objectives[o];
                            SetIntegerValue(o, (int)(req.ItemStack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                        {
                            RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)rb.Objectives[o];
                            SetFloatValue(o, req.InitialDistance + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel);
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                        {
                            RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)rb.Objectives[o];
                            SetIntegerValue(o, req.EventWaves + (int)(req.ExtraWavesPerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.EventKills:
                        {
                            RequestBase.EventKillRequest req = (RequestBase.EventKillRequest)rb.Objectives[o];
                            SetIntegerValue(o, req.InitialKills + (int)(req.ExtraKillsPerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.ObjectCollection:
                        {
                            RequestBase.ObjectCollectionRequest req = (RequestBase.ObjectCollectionRequest)rb.Objectives[o];
                            SetIntegerValue(o, req.ObjectCount + (int)(req.ObjectExtraCountPerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.KillBoss:
                        {
                            SetIntegerValue(o, 1);
                        }
                        break;
                }
            }
        }

        public void OnMobKill(GuardianData gd, NPC npc)
        {
            if (requestState != RequestState.RequestActive) return;
            RequestBase rb = GetRequestBase(gd);
            for (int o = 0; o < rb.Objectives.Count; o++)
            {
                switch (rb.Objectives[o].objectiveType)
                {
                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                        {
                            int Stack = GetIntegerValue(o);
                            if (Stack > 0)
                            {
                                int MobID = ((RequestBase.HuntRequestObjective)rb.Objectives[o]).NpcID;
                                int m = npc.type;
                                bool IsQuestMob = false;
                                if (m == NPCID.EaterofWorldsHead)
                                {
                                    bool HasBodyPart = false;
                                    for (int n = 0; n < 200; n++)
                                    {
                                        if (Main.npc[n].active && Main.npc[n].type == NPCID.EaterofWorldsBody)
                                        {
                                            HasBodyPart = true;
                                            break;
                                        }
                                    }
                                    IsQuestMob = !HasBodyPart;
                                }
                                else if (m == MobID)
                                    IsQuestMob = true;
                                else
                                {
                                    switch (MobID)
                                    {
                                        case NPCID.Zombie:
                                            IsQuestMob = m == 430 || m == 132 || m == 186 || m == 432 || m == 187 || m == 433 || m == 188 || m == 434 || m == 189 || m == 435 ||
                                                m == 200 || m == 436;
                                            break;
                                        case NPCID.ZombieEskimo:
                                            IsQuestMob = m == NPCID.ArmedZombieEskimo;
                                            break;
                                        case NPCID.DemonEye:
                                            IsQuestMob = m == 190 || m == 191 || m == 192 || m == 193 || m == 194 || m == 317 || m == 318;
                                            break;
                                        case NPCID.BloodCrawler:
                                            IsQuestMob = m == NPCID.BloodCrawlerWall;
                                            break;
                                        case NPCID.Demon:
                                            IsQuestMob = m == NPCID.VoodooDemon;
                                            break;
                                        case NPCID.JungleCreeper:
                                            IsQuestMob = m == NPCID.JungleCreeperWall;
                                            break;
                                        case NPCID.Hornet:
                                            IsQuestMob = m == NPCID.HornetFatty || m == NPCID.HornetHoney || m == NPCID.HornetLeafy || m == NPCID.HornetSpikey || m == NPCID.HornetStingy;
                                            break;
                                        case NPCID.AngryBones:
                                            IsQuestMob = m == 294 || m == 295 || m == 296;
                                            break;
                                        case NPCID.BlueArmoredBones:
                                            IsQuestMob = m == NPCID.BlueArmoredBonesMace || m == NPCID.BlueArmoredBonesNoPants || m == NPCID.BlueArmoredBonesSword;
                                            break;
                                        case NPCID.RustyArmoredBonesAxe:
                                            IsQuestMob = m == NPCID.RustyArmoredBonesFlail || m == NPCID.RustyArmoredBonesSword || m == NPCID.RustyArmoredBonesSwordNoArmor;
                                            break;
                                        case NPCID.HellArmoredBones:
                                            IsQuestMob = m == NPCID.HellArmoredBonesMace || m == NPCID.HellArmoredBonesSpikeShield || m == NPCID.HellArmoredBonesSword;
                                            break;
                                        //case NPCID.EaterofWorldsHead:
                                        //    IsQuestMob = m == NPCID.EaterofWorldsBody || m == NPCID.EaterofWorldsTail;
                                        //    break;
                                    }
                                }
                                if (IsQuestMob)
                                {
                                    Stack--;
                                    SetIntegerValue(o, Stack);
                                }
                            }
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.EventKills:
                        {
                            if (GetIntegerValue(o) > 0)
                            {
                                int EventID = ((RequestBase.EventParticipationRequest)rb.Objectives[o]).EventID;
                                int MobID = npc.type;
                                bool EventMobKilled = false;
                                switch ((EventList)EventID)
                                {
                                    case EventList.GoblinArmy:
                                        switch (MobID)
                                        {
                                            case 26:
                                            case 29:
                                            case 27:
                                            case 28:
                                            case 111:
                                            case 417:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;
                                    case EventList.PirateArmy:
                                        switch (MobID)
                                        {
                                            case 216:
                                            case 213:
                                            case 215:
                                            case 214:
                                            case 212:
                                            case 252:
                                            case 491:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;
                                    case EventList.FrostArmy:
                                        switch (MobID)
                                        {
                                            case 381:
                                            case 385:
                                            case 382:
                                            case 383:
                                            case 386:
                                            case 389:
                                            case 520:
                                            case 390:
                                            case 395:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;
                                    case EventList.MartianArmy:
                                        switch (MobID)
                                        {
                                            case 144:
                                            case 143:
                                            case 145:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;
                                    case EventList.DD2Event:
                                        switch (MobID)
                                        {
                                            case Terraria.ID.NPCID.DD2GoblinT1:
                                            case Terraria.ID.NPCID.DD2GoblinT2:
                                            case Terraria.ID.NPCID.DD2GoblinT3:
                                            case Terraria.ID.NPCID.DD2GoblinBomberT1:
                                            case Terraria.ID.NPCID.DD2GoblinBomberT2:
                                            case Terraria.ID.NPCID.DD2GoblinBomberT3:
                                            case Terraria.ID.NPCID.DD2Betsy:
                                            case Terraria.ID.NPCID.DD2DarkMageT1:
                                            case Terraria.ID.NPCID.DD2DarkMageT3:
                                            case Terraria.ID.NPCID.DD2DrakinT2:
                                            case Terraria.ID.NPCID.DD2DrakinT3:
                                            case Terraria.ID.NPCID.DD2JavelinstT1:
                                            case Terraria.ID.NPCID.DD2JavelinstT2:
                                            case Terraria.ID.NPCID.DD2JavelinstT3:
                                            case Terraria.ID.NPCID.DD2KoboldFlyerT2:
                                            case Terraria.ID.NPCID.DD2KoboldFlyerT3:
                                            case Terraria.ID.NPCID.DD2KoboldWalkerT2:
                                            case Terraria.ID.NPCID.DD2KoboldWalkerT3:
                                            case Terraria.ID.NPCID.DD2LightningBugT3:
                                            case Terraria.ID.NPCID.DD2OgreT2:
                                            case Terraria.ID.NPCID.DD2OgreT3:
                                            case Terraria.ID.NPCID.DD2SkeletonT1:
                                            case Terraria.ID.NPCID.DD2SkeletonT3:
                                            case Terraria.ID.NPCID.DD2WitherBeastT2:
                                            case Terraria.ID.NPCID.DD2WitherBeastT3:
                                            case Terraria.ID.NPCID.DD2WyvernT1:
                                            case Terraria.ID.NPCID.DD2WyvernT2:
                                            case Terraria.ID.NPCID.DD2WyvernT3:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;

                                    case EventList.PumpkinMoon:
                                        switch (MobID)
                                        {
                                            case Terraria.ID.NPCID.Scarecrow1:
                                            case Terraria.ID.NPCID.Scarecrow2:
                                            case Terraria.ID.NPCID.Scarecrow3:
                                            case Terraria.ID.NPCID.Scarecrow4:
                                            case Terraria.ID.NPCID.Scarecrow5:
                                            case Terraria.ID.NPCID.Scarecrow6:
                                            case Terraria.ID.NPCID.Scarecrow7:
                                            case Terraria.ID.NPCID.Scarecrow8:
                                            case Terraria.ID.NPCID.Scarecrow9:
                                            case Terraria.ID.NPCID.Scarecrow10:
                                            case Terraria.ID.NPCID.Splinterling:
                                            case Terraria.ID.NPCID.Hellhound:
                                            case Terraria.ID.NPCID.Poltergeist:
                                            case Terraria.ID.NPCID.HeadlessHorseman:
                                            case Terraria.ID.NPCID.MourningWood:
                                            case Terraria.ID.NPCID.Pumpking:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;

                                    case EventList.FrostMoon:
                                        switch (MobID)
                                        {
                                            case Terraria.ID.NPCID.PresentMimic:
                                            case Terraria.ID.NPCID.Flocko:
                                            case Terraria.ID.NPCID.GingerbreadMan:
                                            case Terraria.ID.NPCID.ZombieElf:
                                            case Terraria.ID.NPCID.ZombieElfBeard:
                                            case Terraria.ID.NPCID.ZombieElfGirl:
                                            case Terraria.ID.NPCID.ElfArcher:
                                            case Terraria.ID.NPCID.Nutcracker:
                                            case Terraria.ID.NPCID.NutcrackerSpinning:
                                            case Terraria.ID.NPCID.Yeti:
                                            case Terraria.ID.NPCID.ElfCopter:
                                            case Terraria.ID.NPCID.Krampus:
                                            case Terraria.ID.NPCID.Everscream:
                                            case Terraria.ID.NPCID.SantaNK1:
                                            case Terraria.ID.NPCID.IceQueen:
                                                EventMobKilled = true;
                                                break;
                                        }
                                        break;
                                }
                                if (EventMobKilled)
                                {
                                    SetIntegerValue(o, GetIntegerValue(o) - 1);
                                }
                            }
                        }
                        break;
                }
            }
        }

        public enum RequestState : byte
        {
            Cooldown,
            HasRequestReady,
            RequestActive
        }

        public enum EventList : byte
        {
            GoblinArmy,
            PirateArmy,
            MartianArmy,
            FrostArmy,
            DD2Event,
            Party,
            PumpkinMoon,
            FrostMoon
        }
    }
}
