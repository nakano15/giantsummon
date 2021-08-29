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
        public const int MinRequestTimer = 1800 * 60, MaxRequestTimer = 3600 * 60, MinRequestSpawnTime = 600 * 60, MaxRequestSpawnTime = 1200 * 60;
        public bool RequestCompleted = false, Failed = false;
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
            if (IsTalkQuest)
                return false;
            foreach (RequestBase.RequestObjective o in GetRequestBase(d).Objectives)
            {
                if ((o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.Explore && ((RequestBase.ExploreRequest)o).RequiresGuardianActive) || 
                    o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester)
                    return true;
            }
            return false;
        }

        public static bool PlayerHasRequestRequiringCompanion(Player p, GuardianData d)
        {
            if (d.request.IsTalkQuest)
                return false;
            if (d.request.requestState == RequestState.RequestActive)
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
            Time = Main.rand.Next(MinRequestTimer, MaxRequestTimer);
            requestState = RequestState.RequestActive;
        }

        public void UponRejecting()
        {
            Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
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
            if (requestState >= RequestState.HasExistingRequestReady)
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
            if (IsCommonRequest)
            {
                if (RequestID < 0 || RequestID >= RequestBase.CommonRequests.Length)
                {
                    requestState = RequestState.Cooldown;
                }
            }
            else
            {
                if (RequestID < 0 || RequestID >= gd.Base.RequestDB.Count)
                {
                    requestState = RequestState.Cooldown;
                }
            }
        }

        public bool CountObjective(GuardianData gd, Player player)
        {
            bool Count = !RequiresGuardianActive(gd) || PlayerMod.HasGuardianSummoned(player, gd.ID, gd.ModID);
            if (Count)
            {
                RequestBase rb = GetRequestBase(gd);
                foreach (RequestBase.RequestObjective obj in rb.Objectives)
                {
                    if (obj.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.CompanionRequirement)
                    {
                        RequestBase.CompanionRequirementRequest comp = (RequestBase.CompanionRequirementRequest)obj;
                        if (!PlayerMod.HasGuardianSummoned(player, comp.CompanionID, comp.CompanionModID))
                        {
                            Count = false;
                            break;
                        }
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
                Requests.Clear();
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
            if (false && (gd.FriendshipLevel == 0 || Main.rand.NextDouble() < 0.333f) && HasCompanionSummonedOrInTheWorld) //A talk request should never reset the quest chain.
            {
                CreateTalkRequest();
                if (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID))
                    Main.NewText(gd.Name + " wants to speak with you.");
                GotRequest = true;
            }
            else if (Requests.Count > 0 && HasCompanionSummonedOrInTheWorld)
            {
                ChangeRequest(gd, Requests[Main.rand.Next(Requests.Count)], MakeCommonRequest);
                GotRequest = true;
            }
            else
            {
                Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
            }
            if (GotRequest && !player.TutorialRequestIntroduction) //got to move this
            {
                player.TutorialRequestIntroduction = true;
                Main.NewText("Someone gave you a request. Helping them will reward with friendship experience, and also with some interesting rewards.");
                Main.NewText("Check out " + gd.Name + " and see what It wants.");
            }
        }

        public void UpdateRequest(GuardianData gd, PlayerMod player)
        {
            if (gd.Base.InvalidGuardian)
                return;
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
                                {
                                    int SpawnRequestID;
                                    bool SpawnTalkRequest;
                                    if (!gd.Base.AlterRequestGiven(gd, out SpawnRequestID, out SpawnTalkRequest))
                                    {
                                        Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
                                        return;
                                    }
                                    if (SpawnTalkRequest)
                                    {
                                        CreateTalkRequest();
                                        //if (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID))
                                        //    Main.NewText(gd.Name + " wants to speak with you.");
                                        return;
                                    }
                                    else if(SpawnRequestID > -1 && SpawnRequestID < gd.Base.RequestDB.Count)
                                    {
                                        ChangeRequest(gd, SpawnRequestID, false);
                                        return;
                                    }
                                }
                                if ((!gd.IsStarter && gd.FriendshipLevel < gd.Base.MoveInLevel) || Main.rand.NextDouble() < 0.333)
                                {
                                    CreateTalkRequest();
                                    if (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID))
                                        Main.NewText(gd.Name + " wants to speak with you.");
                                }
                                else
                                {
                                    requestState = RequestState.NewRequestReady;
                                }
                                //SpawnNewRequest(gd, player);
                            }
                        }
                    }
                    break;
                case RequestState.RequestActive:
                    {
                        if (WorldMod.IsGuardianNpcInWorld(gd.ID, gd.ModID))
                        {
                            if (!WorldMod.GuardianTownNPC.First(x => x.ID == gd.ID && x.ModID == gd.ModID).IsUsingBed)
                            {
                                Time--;
                            }
                        }
                        else
                        {
                            Time--;
                        }
                        if (Time <= 0)
                        {
                            requestState = RequestState.Cooldown;
                            Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
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
                                    case RequestBase.RequestObjective.ObjectiveTypes.KillBoss:
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
                                    case RequestBase.RequestObjective.ObjectiveTypes.RequesterCannotKnockout: //Either RequiresRequester, or this, are causing the request to not be completeable.
                                        {
                                            if (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID))
                                            {
                                                TerraGuardian tg = PlayerMod.GetPlayerSummonedGuardian(player.player, gd.ID, gd.ModID);
                                                if (tg.KnockedOutCold || tg.Downed)
                                                {
                                                    SetIntegerValue(o, 1);
                                                    if (!Failed && player.player.whoAmI == Main.myPlayer)
                                                        Main.NewText("Requester was defeated, " + gd.Name + " request failed.", 255);
                                                    Failed = true;
                                                }
                                                if (GetIntegerValue(o) == 0)
                                                    ObjectivesCompleted++;
                                                else
                                                    Failed = true;
                                            }
                                            else if (GetIntegerValue(o) == 1)
                                                Failed = true;
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.NobodyCanBeKod:
                                        {
                                            if (player.KnockedOutCold || player.player.dead)
                                            {
                                                if (!Failed && player.player.whoAmI == Main.myPlayer)
                                                    Main.NewText("You were defeated, " + gd.Name + " request failed.", 255);
                                                SetIntegerValue(o, 1);
                                                Failed = true;
                                            }
                                            foreach (TerraGuardian tg in player.GetAllGuardianFollowers)
                                            {
                                                if (!tg.Active) continue;
                                                if (tg.KnockedOutCold || tg.Downed)
                                                {
                                                    if (!Failed && player.player.whoAmI == Main.myPlayer)
                                                        Main.NewText("One of your companions was defeated, " + gd.Name + " request failed.", 255);
                                                    SetIntegerValue(o, 1);
                                                    Failed = true;
                                                }
                                            }
                                            if (GetIntegerValue(o) == 0)
                                                ObjectivesCompleted++;
                                            else
                                                Failed = true;
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.TalkTo:
                                        {
                                            if (GetIntegerValue(o) < 1)
                                            {
                                                RequestBase.TalkRequestObjective req = (RequestBase.TalkRequestObjective)rb.Objectives[o];
                                                if (player.player.talkNPC > -1 && Main.npc[player.player.talkNPC].type == req.NpcID)
                                                {
                                                    Main.npcChatText += "\n" + req.MessageText;
                                                }
                                            }
                                            else
                                            {
                                                ObjectivesCompleted++;
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian:
                                        {
                                            if (GetIntegerValue(o) == 1)
                                                ObjectivesCompleted++;
                                        }
                                        break;
                                }
                            }
                            RequestCompleted = ObjectivesCompleted >= ObjectiveCount;
                        }
                    }
                    break;
            }
        }

        public bool CompleteRequest(TerraGuardian guardian, GuardianData gd, PlayerMod player)
        {
            if (gd.Base.InvalidGuardian)
                return false;
            if (Failed)
            {
                RequestCompleted = false;
                Failed = false;
                requestState = RequestState.Cooldown;
                Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
                return true;
            }
            if (RequestCompleted || IsTalkQuest)
            {
                int RewardScore = (IsTalkQuest ? 500 : GetRequestBase(gd).RequestScore + 200);
                if (Compatibility.NExperienceCompatibility.IsModActive)
                {
                    Compatibility.NExperienceCompatibility.GiveExpRewardToPlayer(player.player, 3 + (float)RewardScore * (1f / 180), 0.1f, true, 5); //NExperience.ExpReceivedPopText.ExpSource.Quest
                }
                if (IsTalkQuest)
                {
                    if (guardian.ID == gd.ID && guardian.ModID == gd.ModID)
                    {
                        if (player.IsTalkingToAGuardian)
                        {
                            GuardianMouseOverAndDialogueInterface.SetDialogue(gd.Base.TalkMessage(player.player, guardian), guardian);
                        }
                        else
                        {
                            guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(gd.Base.TalkMessage(player.player, guardian), guardian), true);
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
                                    RewardScore += 200;
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
                Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime);
                foreach (TerraGuardian tg in player.GetAllGuardianFollowers)
                {
                    if (tg.Active && !tg.Data.IsStarter && (!PlayerMod.HasBuddiesModeOn(player.player) || !PlayerMod.GetPlayerBuddy(player.player).IsSameID(tg)) && tg.FriendshipLevel < tg.Base.CallUnlockLevel && !PlayerHasRequestRequiringCompanion(player.player, tg.Data))
                    {
                        Main.NewText(tg.Name + " was dismissed");
                        player.DismissGuardian(tg.AssistSlot);
                    }
                }
                if (!IsCommonRequest && !IsTalkQuest)
                    RequestCompleteCombo = 0;
                else
                    RequestCompleteCombo++;
                GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.MentionPlayer,
                    player.player.name, guardian.Name, 15, RequestCompleteCombo, 
                    new GuardianID[] { guardian.MyID });
                if (!IsCommonRequest && !IsTalkQuest && !RequestsCompletedIDs.Contains(RequestID))
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
                return gd.GetMessage(GuardianBase.MessageIDs.RequestAccepted, "(You have accepted the request.)");
            return GetRequestBase(gd).AcceptText;
        }

        public string GetRequestDeny(GuardianData gd)
        {
            if (IsCommonRequest)
                return gd.GetMessage(GuardianBase.MessageIDs.RequestRejected, "(Request rejected.)");
            return GetRequestBase(gd).DenyText;
        }

        public string GetRequestComplete(GuardianData gd, TerraGuardian giver)
        {
            if (IsCommonRequest)
                return gd.Base.CompletedRequestMessage(Main.player[Main.myPlayer], giver);
            return GetRequestBase(gd).CompleteText;
        }

        public string GetRequestFailed(GuardianData gd, TerraGuardian giver)
        {
            if (IsCommonRequest)
                return gd.GetMessage(GuardianBase.MessageIDs.RequestFailed, "(Request failed)");
            return GetRequestBase(gd).FailureText;
        }

        public string GetRequestInfo(GuardianData gd)
        {
            return GetRequestBase(gd).RequestInfoText;
        }

        public string[] GetRequestText(Player player, GuardianData gd, bool ForceShowObjective = false)
        {
            if (gd.Base.InvalidGuardian)
                return new string[] { "Fragmented request memory." };
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
                case RequestState.HasExistingRequestReady:
                    if (IsTalkQuest)
                    {
                        QuestObjectives.Add(gd.Name + " want to talk to you.");
                    }
                    else
                    {
                        QuestObjectives.Add(gd.Name + " want something from you.");
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
                                                QuestObjectives.Add(" Hunt " + GetIntegerValue(o) + " " + (req.NpcID == 1 ? "Slime" : "" + Lang.GetNPCName(req.NpcID)) + ".");
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
                                                    QuestObjectives.Add(" Face " + EventName + ".");
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
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                HasPendingObjective = true;
                                                RequestBase.KillBossRequest req = (RequestBase.KillBossRequest)rb.Objectives[o];
                                                string BossName = "";
                                                if (req.BossID == NPCID.Spazmatism || req.BossID == NPCID.Retinazer)
                                                {
                                                    BossName = "The Twins";
                                                }
                                                else if (req.BossID == NPCID.EaterofWorldsBody || req.BossID == NPCID.EaterofWorldsHead || req.BossID == NPCID.EaterofWorldsTail)
                                                {
                                                    BossName = "Eater of Worlds";
                                                }
                                                else if (req.BossID == NPCID.TheDestroyer || req.BossID == NPCID.TheDestroyerBody || req.BossID == NPCID.TheDestroyerTail)
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
                                    case RequestBase.RequestObjective.ObjectiveTypes.RequesterCannotKnockout:
                                        {
                                            QuestObjectives.Add("Requester must not be defeated.");
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.NobodyCanBeKod:
                                        {
                                            QuestObjectives.Add("Nobody can be defeated.");
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.TalkTo:
                                        {
                                            RequestBase.TalkRequestObjective req = (RequestBase.TalkRequestObjective)rb.Objectives[o];
                                            if (GetIntegerValue(o) == 0)
                                            {
                                                QuestObjectives.Add("Speak with " + req.NpcName + ".");
                                                HasPendingObjective = true;
                                            }
                                            else
                                            {
                                                QuestObjectives.Add("Spoken with " + req.NpcName + ".");
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian:
                                        {
                                            RequestBase.TalkToGuardianRequestObjective req = (RequestBase.TalkToGuardianRequestObjective)rb.Objectives[o];
                                            string Name = "";
                                            if (PlayerMod.PlayerHasGuardian(player, req.GuardianID, req.ModID))
                                                Name = PlayerMod.GetPlayerGuardian(player, req.GuardianID, req.ModID).Name;
                                            else
                                                Name = GuardianBase.GetGuardianBase(req.GuardianID, req.ModID).Name;
                                            if (GetIntegerValue(o) == 0)
                                            {
                                                QuestObjectives.Add("Speak with " + Name + ".");
                                                HasPendingObjective = true;
                                            }
                                            else
                                            {
                                                QuestObjectives.Add("Spoken with " + Name + ".");
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
                                    if (!IsCommonRequest) QuestObjectives.Insert(0, "[" + rb.Name + "]");
                                    if(rb.Objectives.Count == 0)
                                        QuestObjectives.Add("Empty Request ID: " + RequestID + " Common? " + IsCommonRequest + " Talk? " + IsTalkQuest);
                                    QuestObjectives.Add(" Report success to " + gd.Name + ".");
                                }
                            }
                        }
                        break;
                    }
            }
            if (ShowDuration && !ForceShowObjective)
            {
                int Seconds = GetDivisionBy60(Time), Minutes = 0, Hours = 0, Days = 0;
                if (Seconds >= 60)
                {
                    Minutes += GetDivisionBy60(Seconds);
                    Seconds -= Minutes * 60;
                }
                if (Minutes >= 60)
                {
                    Hours += GetDivisionBy60(Minutes);
                    Minutes -= Hours * 60;
                }
                if (Hours >= 24)
                {
                    Days += (int)(Hours * (1f / 24));
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

        private int GetDivisionBy60(int Value)
        {
            float DivisionBy60 = 1f / 60;
            return (int)(Value * DivisionBy60);
        }

        public bool TrySpawningBoss(int PlayerID, int ID, int DifficultyBonus = 0)
        {
            bool Success = false;
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active && (Main.npc[n].boss || Main.npc[n].type == NPCID.EaterofWorldsHead))
                {
                    return false;
                }
            }
            if(ID == NPCID.EaterofWorldsBody || ID== NPCID.EaterofWorldsTail)
                ID = NPCID.EaterofWorldsHead;
            if(ID == NPCID.TheDestroyerBody || ID == NPCID.TheDestroyerTail)
                ID = NPCID.TheDestroyer;
            if (ID == NPCID.EyeofCthulhu || ID == NPCID.SkeletronHead || ID == NPCID.Spazmatism || ID == NPCID.Retinazer ||
                ID == NPCID.TheDestroyer)
            {
                if (!Main.dayTime && Main.time < 2.5f * 3600)
                {
                    NPC.SpawnOnPlayer(PlayerID, ID);
                    if (ID == NPCID.Spazmatism)
                        NPC.SpawnOnPlayer(PlayerID, NPCID.Retinazer);
                    else if (ID == NPCID.Retinazer)
                        NPC.SpawnOnPlayer(PlayerID, NPCID.Spazmatism);
                    Success = true;
                }
            }
            else if (ID == NPCID.EaterofWorldsHead && Main.player[PlayerID].ZoneCorrupt)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == NPCID.BrainofCthulhu && Main.player[PlayerID].ZoneCrimson)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == NPCID.WallofFlesh && Main.player[PlayerID].ZoneUnderworldHeight)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == NPCID.KingSlime)
            {
                NPC.SpawnOnPlayer(PlayerID, ID);
                Success = true;
            }
            else if (ID == NPCID.QueenBee && Main.player[PlayerID].ZoneJungle)
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
                    if (Main.npc[n].type == ID || (ID == NPCID.Spazmatism && Main.npc[n].type == NPCID.Retinazer) ||
                        (ID == NPCID.Retinazer && Main.npc[n].type == NPCID.Spazmatism) || 
                        (Main.npc[n].type >= NPCID.EaterofWorldsHead && Main.npc[n].type <= NPCID.EaterofWorldsTail && ID == NPCID.EaterofWorldsHead))
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
            Failed = false;
            IsTalkQuest = true;
            IsCommonRequest = false;
            requestState = RequestState.HasExistingRequestReady;
        }

        public void ChangeRequest(GuardianData gd, int ID, bool CommonRequest = false)
        {
            IntegerVars.Clear();
            FloatVars.Clear();
            RequestID = ID;
            Failed = false;
            IsTalkQuest = false;
            requestState = RequestState.HasExistingRequestReady;
            this.IsCommonRequest = CommonRequest;
            if (gd.Base.InvalidGuardian)
                return;
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
                            SetIntegerValue(o, 1);
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
                    case RequestBase.RequestObjective.ObjectiveTypes.NobodyCanBeKod:
                    case RequestBase.RequestObjective.ObjectiveTypes.RequesterCannotKnockout:
                        {
                            SetIntegerValue(o, 0);
                        }
                        break;
                }
            }
        }

        public static bool IsRequiredMonster(NPC npc, int ReqMobID)
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
                    case NPCID.BlueSlime:
                        return m == NPCID.SlimeRibbonGreen || m == NPCID.SlimeRibbonRed || m == NPCID.SlimeRibbonWhite || m == NPCID.SlimeRibbonYellow || m == 302 ||
                            m == NPCID.SandSlime || m == NPCID.IceSlime || m == NPCID.SpikedIceSlime || m == NPCID.SlimedZombie || m == NPCID.ArmedZombieSlimed ||
                            m == NPCID.LavaSlime || m == NPCID.RainbowSlime || m == NPCID.KingSlime || m == NPCID.IlluminantSlime || m == NPCID.DungeonSlime ||
                            m == NPCID.MotherSlime || m == NPCID.Slimeling || m == NPCID.SlimeMasked || m == NPCID.SlimeSpiked || m == NPCID.SpikedJungleSlime ||
                            m == NPCID.UmbrellaSlime; //302 is Bunny Slime
                    case NPCID.Lihzahrd:
                        return m == NPCID.LihzahrdCrawler;
                    default:
                        return false;
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
                    case RequestBase.RequestObjective.ObjectiveTypes.ObjectCollection:
                        {
                            int Stack = GetIntegerValue(o);
                            if (Stack > 0)
                            {
                                RequestBase.ObjectCollectionRequest req = (RequestBase.ObjectCollectionRequest)rb.Objectives[o];
                                bool Drop = false;
                                foreach (RequestBase.ObjectCollectionRequest.DropRateFromMonsters mon in req.DropFromMobs)
                                {
                                    if (IsRequiredMonster(npc, mon.MobID) && Main.rand.NextDouble() < mon.DropRate)
                                    {
                                        Drop = true;
                                        break;
                                    }
                                }
                                if (Drop)
                                {
                                    SetIntegerValue(o, Stack - 1);
                                    if (Stack == 1)
                                        Main.NewText("You acquired all the necessary " + req.ObjectName + ".");
                                    else
                                        Main.NewText("You got a " + req.ObjectName + ".");
                                }
                            }
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.KillBoss:
                        {
                            int Stack = GetIntegerValue(o);
                            if (Stack > 0)
                            {
                                bool Count = false;
                                RequestBase.KillBossRequest req = (RequestBase.KillBossRequest)rb.Objectives[o];
                                if (req.BossID == NPCID.EaterofWorldsBody || req.BossID == NPCID.EaterofWorldsHead || req.BossID == NPCID.EaterofWorldsTail)
                                {
                                    if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail)
                                    {
                                        bool HasBodyAlive = false;
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && (Main.npc[n].type == NPCID.EaterofWorldsHead || Main.npc[n].type == NPCID.EaterofWorldsBody))
                                            {
                                                HasBodyAlive = true;
                                                break;
                                            }
                                        }
                                        if (!HasBodyAlive)
                                            Count = true;
                                    }
                                }
                                else if (req.BossID == NPCID.Spazmatism || req.BossID == NPCID.Retinazer)
                                {
                                    if (npc.type == NPCID.Spazmatism || npc.type == NPCID.Retinazer)
                                    {
                                        bool OneOfTheTwoAlive = false;
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && req.BossID != npc.type && (Main.npc[n].type == NPCID.Spazmatism || Main.npc[n].type == NPCID.Retinazer))
                                            {
                                                OneOfTheTwoAlive = true;
                                                break;
                                            }
                                        }
                                        if (!OneOfTheTwoAlive)
                                        {
                                            Count = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (npc.type == req.BossID)
                                    {
                                        Count = true;
                                    }
                                }
                                if (Count)
                                {
                                    SetIntegerValue(o, 0);
                                    Main.NewText("You managed to overcome the challenge!");
                                }
                            }
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                        {
                            int Stack = GetIntegerValue(o);
                            if (Stack > 0)
                            {
                                int MobID = ((RequestBase.HuntRequestObjective)rb.Objectives[o]).NpcID;
                                if (IsRequiredMonster(npc, MobID))
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
                                int EventID = ((RequestBase.EventKillRequest)rb.Objectives[o]).EventID;
                                if (IsEventMob(npc.type, EventID))
                                {
                                    SetIntegerValue(o, GetIntegerValue(o) - 1);
                                }
                            }
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                        {
                            if (GetIntegerValue(o) > 0)
                            {
                                int EventID = ((RequestBase.EventParticipationRequest)rb.Objectives[o]).EventID;
                                if (IsEventMob(npc.type, EventID) && Main.invasionType == 0)
                                {
                                    SetIntegerValue(o, 0);
                                }
                            }
                        }
                        break;
                }
            }
        }

        public bool IsEventMob(int MobID, int EventID)
        {
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
                            return true;
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
                            return true;
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
                            return true;
                    }
                    break;
                case EventList.MartianArmy:
                    switch (MobID)
                    {
                        case 144:
                        case 143:
                        case 145:
                            return true;
                    }
                    break;
                case EventList.DD2Event:
                    switch (MobID)
                    {
                        case NPCID.DD2GoblinT1:
                        case NPCID.DD2GoblinT2:
                        case NPCID.DD2GoblinT3:
                        case NPCID.DD2GoblinBomberT1:
                        case NPCID.DD2GoblinBomberT2:
                        case NPCID.DD2GoblinBomberT3:
                        case NPCID.DD2Betsy:
                        case NPCID.DD2DarkMageT1:
                        case NPCID.DD2DarkMageT3:
                        case NPCID.DD2DrakinT2:
                        case NPCID.DD2DrakinT3:
                        case NPCID.DD2JavelinstT1:
                        case NPCID.DD2JavelinstT2:
                        case NPCID.DD2JavelinstT3:
                        case NPCID.DD2KoboldFlyerT2:
                        case NPCID.DD2KoboldFlyerT3:
                        case NPCID.DD2KoboldWalkerT2:
                        case NPCID.DD2KoboldWalkerT3:
                        case NPCID.DD2LightningBugT3:
                        case NPCID.DD2OgreT2:
                        case NPCID.DD2OgreT3:
                        case NPCID.DD2SkeletonT1:
                        case NPCID.DD2SkeletonT3:
                        case NPCID.DD2WitherBeastT2:
                        case NPCID.DD2WitherBeastT3:
                        case NPCID.DD2WyvernT1:
                        case NPCID.DD2WyvernT2:
                        case NPCID.DD2WyvernT3:
                            return true;
                    }
                    break;

                case EventList.PumpkinMoon:
                    switch (MobID)
                    {
                        case NPCID.Scarecrow1:
                        case NPCID.Scarecrow2:
                        case NPCID.Scarecrow3:
                        case NPCID.Scarecrow4:
                        case NPCID.Scarecrow5:
                        case NPCID.Scarecrow6:
                        case NPCID.Scarecrow7:
                        case NPCID.Scarecrow8:
                        case NPCID.Scarecrow9:
                        case NPCID.Scarecrow10:
                        case NPCID.Splinterling:
                        case NPCID.Hellhound:
                        case NPCID.Poltergeist:
                        case NPCID.HeadlessHorseman:
                        case NPCID.MourningWood:
                        case NPCID.Pumpking:
                            return true;
                    }
                    break;

                case EventList.FrostMoon:
                    switch (MobID)
                    {
                        case NPCID.PresentMimic:
                        case NPCID.Flocko:
                        case NPCID.GingerbreadMan:
                        case NPCID.ZombieElf:
                        case NPCID.ZombieElfBeard:
                        case NPCID.ZombieElfGirl:
                        case NPCID.ElfArcher:
                        case NPCID.Nutcracker:
                        case NPCID.NutcrackerSpinning:
                        case NPCID.Yeti:
                        case NPCID.ElfCopter:
                        case NPCID.Krampus:
                        case NPCID.Everscream:
                        case NPCID.SantaNK1:
                        case NPCID.IceQueen:
                            return true;
                    }
                    break;
            }
            return false;
        }

        public enum RequestState : byte
        {
            Cooldown,
            NewRequestReady,
            HasExistingRequestReady,
            RequestActive
        }

        public enum EventList : byte
        {
            NotANumber,
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
