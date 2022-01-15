using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class RequestDataNew
    {
        public int RequestID = 0;
        public string RequestModID = "";
        public ThisRequestReward[] Rewards = new ThisRequestReward[] { new ThisRequestReward(), new ThisRequestReward(), new ThisRequestReward() };
        public int ObjectiveCount = 0, MaxObjectiveCount = 0;
        public RequestState state = RequestState.Cooldown;
        public int RequestTimeLeft = 0;
        public GuardianData RequestGiver;
        public RequestBaseNew Base;
        public const int MaxRequests = 3;

        public bool IsComplete { get { return ObjectiveCount >= MaxObjectiveCount; } }

        public RequestDataNew(GuardianData Giver)
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
                        //Spawn new request
                        List<KeyValuePair<string, int>> RequestToPick = RequestContainer.GetPossibleRequests(data, player);
                        KeyValuePair<string, int> PickedOne = RequestToPick[Main.rand.Next(RequestToPick.Count)];
                        SpawnRequest(PickedOne.Key, PickedOne.Value, player);
                        RequestToPick.Clear();
                    }
                    break;
                case RequestState.Active:
                    RequestTimeLeft--;
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
            player.SellItem(reward.value, 1);
            Item.NewItem(player.getRect(), reward.item.type, reward.item.stack, true, reward.item.prefix, true);
            gd.IncreaseFriendshipProgress(1);
            gd.ChangeTrustValue(TrustLevels.TrustGainFromComplettingRequest);
            SetRequestOnCooldown();
        }

        public void OnCancelRequest(Player player, TerraGuardian gd)
        {
            gd.ChangeTrustValue(TrustLevels.TrustLossWhenCancellingRequest);
            SetRequestOnCooldown(true);
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
            state = RequestState.Ready;
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
                string RewardID = UniqueID + "_rwi";
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
                string RewardID = UniqueID + "_rwi";
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
            Active
        }
    }
}
