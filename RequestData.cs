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
        public bool IsTalkQuest = false;
        public const int MaxRequestCount = 3; //It's hard to focus when you have several requests active, even more since they have a time limit.

        public bool RequiresGuardianActive(GuardianData d)
        {
            foreach (RequestBase.RequestObjective o in d.Base.RequestDB[RequestID].Objectives)
            {
                if (o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.Explore || o.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester)
                    return true;
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
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag, int ModVersion, int UniqueID, GuardianData gd)
        {
            string IDText = "_" + UniqueID;
            RequestID = tag.GetInt("RequestID" + IDText);
            IsTalkQuest = tag.GetBool("RequestIsTalk" + IDText);
            RequestState requestState = (RequestState)tag.GetByte("RequestState" + IDText);
            if (requestState >= RequestState.HasRequestReady)
            {
                if (IsTalkQuest)
                {
                    CreateTalkRequest();
                }
                else
                {
                    ChangeRequest(gd, RequestID);
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
                                List<int> Requests = new List<int>();
                                for (int req = 0; req < gd.Base.RequestDB.Count; req++)
                                {
                                    if (gd.Base.RequestDB[req].Requirement(player.player))
                                    {
                                        Requests.Add(req);
                                    }
                                }
                                bool HasCompanionSummonedOrInTheWorld = (PlayerMod.HasGuardianSummoned(player.player, gd.ID, gd.ModID) || NpcMod.HasGuardianNPC(gd.ID, gd.ModID));
                                if (Main.rand.NextDouble() < 0.333f && HasCompanionSummonedOrInTheWorld)
                                {
                                    CreateTalkRequest();
                                    Main.NewText(gd.Name + " wants to speak with you.");
                                }
                                else if (Requests.Count > 0 && HasCompanionSummonedOrInTheWorld)
                                {
                                    ChangeRequest(gd, Requests[Main.rand.Next(Requests.Count)]);
                                    Main.NewText(gd.Name + " has a request for you.");
                                }
                                else
                                {
                                    Time = Main.rand.Next(MinRequestSpawnTime, MaxRequestSpawnTime) * 60;
                                }
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
                                int ObjectivesCompleted = 0, ObjectiveCount = 0;
                                for (int o = 0; o < gd.Base.RequestDB[RequestID].Objectives.Count; o++)
                                {
                                    ObjectiveCount++;
                                    switch (gd.Base.RequestDB[RequestID].Objectives[o].objectiveType)
                                    {
                                        case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                            {
                                                RequestBase.HuntRequestObjective req = (RequestBase.HuntRequestObjective)gd.Base.RequestDB[RequestID].Objectives[o];
                                                if (GetIntegerValue(o) <= 0)
                                                {
                                                    ObjectivesCompleted++;
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                            {
                                                RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                                                int ItemStack = 0;
                                                for (int i = 0; i < 50; i++)
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
                                                RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)gd.Base.RequestDB[RequestID].Objectives[o];
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
                                                            DistanceStack -= Math.Abs(Speed);
                                                            SetFloatValue(o, DistanceStack);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                                            {
                                                if (GetIntegerValue(0) > 0)
                                                {
                                                    RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                                                    if (Main.invasionType == req.EventID && Main.invasionProgressWave != MainMod.LastEventWave && MainMod.LastEventWave > 0)
                                                    {
                                                        switch ((EventList)req.EventID)
                                                        {
                                                            case EventList.PumpkinMoon:
                                                            case EventList.FrostMoon:
                                                            case EventList.DD2Event:
                                                                SetIntegerValue(0, GetIntegerValue(0) - 1);
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
                int RewardScore = (IsTalkQuest ? 500 : gd.Base.RequestDB[RequestID].RequestScore);
                if (IsTalkQuest)
                {
                    if (guardian.ID == gd.ID && guardian.ModID == gd.ModID)
                    {
                        if (player.player.talkNPC > -1)
                        {
                            Main.npcChatText = gd.Base.TalkMessage(player.player, guardian);
                        }
                        else
                        {
                            Main.NewText(guardian.Name + ": " + gd.Base.TalkMessage(player.player, guardian));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    for (int o = 0; o < gd.Base.RequestDB[RequestID].Objectives.Count; o++)
                    {
                        switch (gd.Base.RequestDB[RequestID].Objectives[o].objectiveType)
                        {
                            case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                {
                                    RequestBase.HuntRequestObjective req = ((RequestBase.HuntRequestObjective)gd.Base.RequestDB[RequestID].Objectives[o]);
                                    RewardScore += (int)(req.Stack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel) * 50;
                                }
                                break;
                            case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                {
                                    RequestBase.CollectItemRequest req = ((RequestBase.CollectItemRequest)gd.Base.RequestDB[RequestID].Objectives[o]);
                                    int ItemID = req.ItemID;
                                    int Stack = GetIntegerValue(o);
                                    Player p = player.player;
                                    for (int i = 49; i >= 0; i--)
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
                                    RequestBase.ExploreRequest req = ((RequestBase.ExploreRequest)gd.Base.RequestDB[RequestID].Objectives[o]);
                                    RewardScore += (int)((req.InitialDistance + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel) * 8);
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
                return true;
            }
            return false;
        }

        public string GetRequestBrief(GuardianData gd)
        {
            return gd.Base.RequestDB[RequestID].BriefText;
        }

        public string GetRequestAccept(GuardianData gd)
        {
            return gd.Base.RequestDB[RequestID].AcceptText;
        }

        public string GetRequestDeny(GuardianData gd)
        {
            return gd.Base.RequestDB[RequestID].DenyText;
        }

        public string GetRequestComplete(GuardianData gd)
        {
            return gd.Base.RequestDB[RequestID].CompleteText;
        }

        public string GetRequestInfo(GuardianData gd)
        {
            return gd.Base.RequestDB[RequestID].RequestInfoText;
        }

        public string[] GetRequestText(Player player, GuardianData gd)
        {
            List<string> QuestObjectives = new List<string>();
            bool ShowDuration = false;
            switch (requestState)
            {
                case RequestState.Cooldown:
                    QuestObjectives.Add(gd.Name + " has no requests right now.");
                    break;
                case RequestState.HasRequestReady:
                    QuestObjectives.Add(gd.Name + " has something for you.");
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
                            for (int o = 0; o < gd.Base.RequestDB[RequestID].Objectives.Count; o++)
                            {
                                switch (gd.Base.RequestDB[RequestID].Objectives[o].objectiveType)
                                {
                                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                                        {
                                            RequestBase.HuntRequestObjective req = (RequestBase.HuntRequestObjective)gd.Base.RequestDB[RequestID].Objectives[o];
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Hunt " + GetIntegerValue(o) + " " + Lang.GetNPCName(req.NpcID) + ".");
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                                        {
                                            RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                                            if (GetIntegerValue(o) > 0)
                                            {
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Collect " + GetIntegerValue(o) + " " + Lang.GetItemName(req.ItemID) + ".");
                                            }

                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                                        {
                                            RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                                            if (GetFloatValue(o) > 0)
                                            {
                                                HasPendingObjective = true;
                                                QuestObjectives.Add(" Travel with " + gd.Name + ".");
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                                        {
                                            RequestBase.EventParticipationRequest req = (RequestBase.EventParticipationRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                                            if (GetIntegerValue(0) > 0)
                                            {
                                                HasPendingObjective = true;
                                                string EventName = "";
                                                switch ((EventList)req.EventID)
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
                                                if (req.EventID == (int)EventList.FrostMoon || req.EventID == (int)EventList.PumpkinMoon || req.EventID == (int)EventList.DD2Event)
                                                {
                                                    QuestObjectives.Add(" Survive " + GetIntegerValue(o) + " waves in the " + EventName + " event.");
                                                }
                                                else
                                                {
                                                    QuestObjectives.Add(" Defeat " + GetIntegerValue(o) + " foes in the " + EventName + " event.");
                                                }
                                            }
                                        }
                                        break;
                                    case RequestBase.RequestObjective.ObjectiveTypes.RequiresRequester:
                                        {
                                            QuestObjectives.Add(" Requires " + gd.Name + " in the group.");
                                        }
                                        break;
                                }
                            }
                            if (HasPendingObjective)
                            {
                                QuestObjectives.Insert(0, "Help " + gd.Name + " by:");
                                QuestObjectives.Insert(0, "[" + gd.Base.RequestDB[RequestID].Name + "]");
                            }
                            else
                            {
                                QuestObjectives.Add("[" + gd.Base.RequestDB[RequestID].Name + "]");
                                QuestObjectives.Add(" Report success to " + gd.Name + ".");
                            }
                        }
                        break;
                    }
            }
            if (ShowDuration)
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

        public void CreateTalkRequest()
        {
            IntegerVars.Clear();
            FloatVars.Clear();
            RequestID = 0;
            IsTalkQuest = true;
            requestState = RequestState.HasRequestReady;
        }

        public void ChangeRequest(GuardianData gd, int ID)
        {
            IntegerVars.Clear();
            FloatVars.Clear();
            RequestID = ID;
            IsTalkQuest = false;
            requestState = RequestState.HasRequestReady;
            for (int o = 0; o < gd.Base.RequestDB[RequestID].Objectives.Count; o++)
            {
                switch (gd.Base.RequestDB[RequestID].Objectives[o].objectiveType)
                {
                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                        {
                            RequestBase.HuntRequestObjective req = (RequestBase.HuntRequestObjective)gd.Base.RequestDB[RequestID].Objectives[o];
                            SetIntegerValue(o, (int)(req.Stack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.CollectItem:
                        {
                            RequestBase.CollectItemRequest req = (RequestBase.CollectItemRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                            SetIntegerValue(o, (int)(req.ItemStack + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel));
                        }
                        break;
                    case RequestBase.RequestObjective.ObjectiveTypes.Explore:
                        {
                            RequestBase.ExploreRequest req = (RequestBase.ExploreRequest)gd.Base.RequestDB[RequestID].Objectives[o];
                            SetFloatValue(o, req.InitialDistance + req.StackIncreasePerFriendshipLevel * gd.FriendshipLevel);
                        }
                        break;
                }
            }

        }

        public void OnMobKill(GuardianData gd, NPC npc)
        {
            if (requestState != RequestState.RequestActive) return;
            for (int o = 0; o < gd.Base.RequestDB[RequestID].Objectives.Count; o++)
            {
                switch (gd.Base.RequestDB[RequestID].Objectives[o].objectiveType)
                {
                    case RequestBase.RequestObjective.ObjectiveTypes.HuntMonster:
                        {
                            int Stack = GetIntegerValue(o);
                            if (Stack > 0)
                            {
                                int MobID = ((RequestBase.HuntRequestObjective)gd.Base.RequestDB[RequestID].Objectives[o]).NpcID;
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
                    case RequestBase.RequestObjective.ObjectiveTypes.EventParticipation:
                        {
                            if (GetIntegerValue(o) > 0)
                            {
                                int EventID = ((RequestBase.EventParticipationRequest)gd.Base.RequestDB[RequestID].Objectives[o]).EventID;
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
