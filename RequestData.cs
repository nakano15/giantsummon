using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class RequestData
    {
        public int RequestID = 0;
        public string RequestModID = "";
        public ThisRequestReward[] Rewards = new ThisRequestReward[] { new ThisRequestReward(), new ThisRequestReward(), new ThisRequestReward() };
        public int ObjectiveCount = 0, MaxObjectiveCount = 0;
        public RequestState state = RequestState.Cooldown;
        public int RequestTimeLeft = 0;
        public GuardianData RequestGiver;
        public RequestBase Base;
        public const int MaxRequests = 3;

        public bool IsTravelRequest { get { return Base is TravelRequestBase; } }

        public bool IsItemCollectionRequest { get { return Base is ItemRequestBase; } }

        public bool IsHuntRequest { get { return Base is HuntRequestBase; } }

        public string GetObjective { get { return Base.RequestObjective(this); } }

        public string GetShortDescription { get { return Base.RequestShortDescription(this); } }

        public bool Active { get { return state == RequestState.Active; } }

        public bool IsComplete { get { return state == RequestState.Active && ObjectiveCount >= MaxObjectiveCount; } }

        public RequestData(GuardianData Giver)
        {
            RequestGiver = Giver;
        }

        public void Update(Player player, GuardianData data)
        {
            switch (state)
            {
                case RequestState.Cooldown:
                    RequestTimeLeft--;
                    if(RequestTimeLeft <= 0)
                    {
                        state = RequestState.Ready;
                    }
                    break;
                case RequestState.Active:
                    RequestTimeLeft--;
                    Base.UpdateRequest(player, this);
                    if(RequestTimeLeft <= 0)
                    {
                        data.ChangeTrustValue(TrustLevels.TrustLossOnFailRequest);
                        SetRequestOnCooldown(false);
                        Main.NewText("You took so long to complete " + data.Name + "'s request, that they forgot about it...", new Microsoft.Xna.Framework.Color(200, 0, 0));
                    }
                    break;
            }
        }

        public void CompleteRequest(Player player, TerraGuardian gd, byte RewardToGet)
        {
            if (ObjectiveCount < MaxObjectiveCount)
                return;
            if (RewardToGet > 2)
                RewardToGet = 2;
            Base.OnCompleteRequest(player, this);
            ThisRequestReward reward = Rewards[RewardToGet];
            Item.NewItem(player.getRect(), reward.item.type, reward.item.stack, true, reward.item.prefix, true);
            int p = 0, g = 0, s = 0, c = reward.value;
            if(c >= 100)
            {
                s += c / 100;
                c -= s * 100;
            }
            if(s >= 100)
            {
                g += s / 100;
                s -= g * 100;
            }
            if(g >= 100)
            {
                p += g / 100;
                g -= p * 100;
            }
            if (p > 0)
                Item.NewItem(player.getRect(), Terraria.ID.ItemID.PlatinumCoin, p, true, 0, true);
            if (g > 0)
                Item.NewItem(player.getRect(), Terraria.ID.ItemID.GoldCoin, g, true, 0, true);
            if (s > 0)
                Item.NewItem(player.getRect(), Terraria.ID.ItemID.SilverCoin, s, true, 0, true);
            if (c > 0)
                Item.NewItem(player.getRect(), Terraria.ID.ItemID.CopperCoin, c, true, 0, true);
            gd.IncreaseFriendshipProgress(1);
            gd.ChangeTrustValue(TrustLevels.TrustGainFromComplettingRequest);
            SetRequestOnCooldown();
        }

        public void OnCancelRequest(Player player, TerraGuardian gd)
        {
            gd.ChangeTrustValue(TrustLevels.TrustLossWhenCancellingRequest);
            SetRequestOnCooldown(true);
        }

        public void OnAcceptingRequest()
        {
            state = RequestState.Active;
        }

        public void OnRejectingRequest()
        {
            SetRequestOnCooldown(true);
        }

        public void TryGettingNewRequest(Player player, GuardianData data)
        {
            if (state != RequestState.Ready)
                return;
            List<KeyValuePair<string, int>> RequestToPick = RequestContainer.GetPossibleRequests(data, player);
            KeyValuePair<string, int> PickedOne = RequestToPick[Main.rand.Next(RequestToPick.Count)];
            SpawnRequest(PickedOne.Key, PickedOne.Value, player);
            RequestToPick.Clear();
        }

        public string GetRequestBrief(Player player, TerraGuardian gd)
        {
            return gd.Base.HasRequestMessage(player, gd).Replace("[objective]", GetShortDescription);
        }

        public string GetRequestReward(byte RewardPosition)
        {
            if (RewardPosition > 2)
                RewardPosition = 2;
            string RewardString = "";
            if (Rewards[RewardPosition].item.type != 0)
                RewardString = Rewards[RewardPosition].item.HoverName + " and ";
            int p = 0, g = 0, s = 0, c = Rewards[RewardPosition].value;
            if (c >= 100)
            {
                s += c / 100;
                c -= s * 100;
            }
            if (s >= 100)
            {
                g += s / 100;
                s -= g * 100;
            }
            if (g >= 100)
            {
                p += g / 100;
                g -= p * 100;
            }
            if (p > 0)
                RewardString += p + "p ";
            if (g > 0)
                RewardString += g + "g ";
            if (s > 0)
                RewardString += s + "s ";
            if (c > 0)
                RewardString += c + "c";
            return RewardString;
        }

        public void SpawnRequest(string ModID, int ID, Player player)
        {
            RequestModID = ModID;
            RequestID = ID;
            Base = RequestContainer.GetRequest(ID, ModID);
            if(Base == null)
            {
                SetRequestOnCooldown(true);
                return;
            }
            ObjectiveCount = 0;
            MaxObjectiveCount = Base.GetRequestObjectiveCount(RequestGiver.FriendshipLevel);
            state = RequestState.WaitingAccept;
            RequestTimeLeft = Main.rand.Next(36 * 3600, 48 * 3600);
            GenerateRewards(player);
        }

        private void GenerateRewards(Player player)
        {
            float MaxChance;
            List<RequestReward> PossibleRewards = RequestReward.GetPossibleRewards(player, RequestGiver, out MaxChance);
            float RewardPercentage = 1f + RequestGiver.FriendshipLevel * 0.1f;
            RewardPercentage *= (float)MaxObjectiveCount / Base.GetRequestObjectiveCount(0);
            float RewardVariationStart = 0.8f, VariationRange = 0.4f;
            if (RequestGiver.FriendshipLevel < 1)
            {
                MaxChance += (int)(MaxChance * 0.5f);
                RewardVariationStart = 0.5f;
                VariationRange = 0.3f;
            }
            else if (RequestGiver.FriendshipLevel < 3)
            {
                MaxChance += (int)(MaxChance * 0.25f);
            }
            else if (RequestGiver.FriendshipLevel < 5)
            {
                MaxChance += (int)(MaxChance * 0.15f);
                RewardVariationStart = 0.9f;
                VariationRange = 0.2f;
            }
            else if (RequestGiver.FriendshipLevel < 7)
            {
                MaxChance += (int)(MaxChance * 0.05f);
                RewardVariationStart = 0.95f;
                VariationRange = 0.3f;
            }
            else
            {
                RewardVariationStart = 1;
                VariationRange = 0.5f;
            }
            for (byte i = 0; i < 3; i++)
            {
                ThisRequestReward reward = Rewards[i];
                float PickedChance = Main.rand.NextFloat() * MaxChance;
                float Stack = 0;
                Item item = new Item();
                foreach(RequestReward r in PossibleRewards)
                {
                    if(PickedChance >= Stack && PickedChance < Stack + r.AcquisitionChance)
                    {
                        item.SetDefaults(r.itemID);
                        item.stack = r.Stack;
                        break;
                    }
                    Stack += r.AcquisitionChance;
                }
                reward.item = item;
                reward.value = (int)(Base.RewardValue * RewardPercentage * (RewardVariationStart + VariationRange * Main.rand.NextFloat()));
            }
        }

        public void SetRequestOnCooldown(bool Shorter = false)
        {
            state = RequestState.Cooldown;
            RequestTimeLeft = Main.rand.Next(8 * 3600, 24 * 3600);
            if (Shorter)
                RequestTimeLeft /= 2;
        }

        public void Save(Terraria.ModLoader.IO.TagCompound writer, string UniqueID)
        {
            writer.Add(UniqueID + "_ID", RequestID);
            writer.Add(UniqueID + "_ModID", RequestModID);
            writer.Add(UniqueID + "_State", (byte)state);
            writer.Add(UniqueID + "_ObjectiveCount", ObjectiveCount);
            writer.Add(UniqueID + "_MaxObjectiveCount", MaxObjectiveCount);
            writer.Add(UniqueID + "_TimeLeft", RequestTimeLeft);
            for(byte i = 0; i < 3; i++)
            {
                string RewardID = UniqueID + "_rwi"+i;
                ThisRequestReward reward = Rewards[i];
                writer.Add(RewardID + "_item", reward.item);
                writer.Add(RewardID + "_value", reward.value);
            }
        }

        public void Load(Terraria.ModLoader.IO.TagCompound writer, string UniqueID, int ModVersion)
        {
            RequestID = writer.GetInt(UniqueID + "_ID");
            RequestModID = writer.GetString(UniqueID + "_ModID");
            state = (RequestState)writer.GetByte(UniqueID + "_State");
            ObjectiveCount = writer.GetInt(UniqueID + "_ObjectiveCount");
            MaxObjectiveCount =  writer.GetInt(UniqueID + "_MaxObjectiveCount");
            RequestTimeLeft = writer.GetInt(UniqueID + "_TimeLeft");
            for (byte i = 0; i < 3; i++)
            {
                string RewardID = UniqueID + "_rwi" + i;
                ThisRequestReward reward = Rewards[i];
                reward.item = writer.Get<Item>(RewardID + "_item");
                reward.value = writer.GetInt(RewardID + "_value");
            }
            if (state != RequestState.Cooldown)
                Base = RequestContainer.GetRequest(RequestID, RequestModID);
        }

        public class ThisRequestReward
        {
            public int value;
            public Item item;

            public ThisRequestReward()
            {
                item = new Item();
                value = 0;
            }
            
            public ThisRequestReward(int value, int ItemID, int ItemStack = 1)
            {
                this.value = value;
                item = new Item();
                item.SetDefaults(value, true);
                item.stack = ItemStack;
            }
        }

        public enum RequestState : byte
        {
            Cooldown,
            Ready,
            WaitingAccept,
            Active
        }
    }
}
