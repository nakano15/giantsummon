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
