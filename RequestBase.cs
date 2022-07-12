using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class RequestBase
    {
        public int RewardValue = 0;
        public bool RequiresRequesterSummoned = false;

        public Func<Player, GuardianData, bool> CanGetRequest = delegate (Player player, GuardianData tg)
        {
            return true;
        };

        public virtual string RequestObjective(RequestData data)
        {
            return "Unknown";
        }

        public virtual string RequestShortDescription(RequestData data)
        {
            return "do something";
        }

        public virtual int GetRequestObjectiveCount(int FriendshipLevel)
        {
            return 5 + FriendshipLevel / 3;
        }

        public virtual void UpdateRequest(Player player, RequestData data)
        {

        }

        public virtual void OnKillMob(NPC npc, RequestData data)
        {

        }

        public virtual void OnCompleteRequest(Player player, RequestData data)
        {

        }

        public virtual void OnFailRequest(Player player, RequestData data)
        {

        }
    }

    public class HuntRequestBase : RequestBase
    {
        public int NpcID = 0;
        public string NpcName = "";
        public int InitialCount = 5;
        public float ExtraFriendshipLevelCount = 0.334f;

        public HuntRequestBase(int NpcID, string NpcName = "", int InitialCount = 5, float ExtraCount = 0.334f, int RewardValue = 0)
        {
            this.NpcID = NpcID;
            NPC n = new NPC();
            n.SetDefaults(NpcID);
            if (NpcName == "")
            {
                this.NpcName = n.GivenOrTypeName;
            }
            else
            {
                this.NpcName = NpcName;
            }
            if (RewardValue == 0)
            {
                this.RewardValue = n.lifeMax / 3 * InitialCount;
            }
            else
            {
                this.RewardValue = RewardValue;
            }
            this.InitialCount = InitialCount;
            this.ExtraFriendshipLevelCount = ExtraCount;
        }

        public override int GetRequestObjectiveCount(int FriendshipLevel)
        {
            return InitialCount + (int)(FriendshipLevel * ExtraFriendshipLevelCount);
        }

        public override string RequestObjective(RequestData data)
        {
            if(data.IsComplete)
            {
                return "Report back to " + data.RequestGiver.Name + ".";
            }
            return "Slay "+(data.MaxObjectiveCount - data.ObjectiveCount)+" " + MainMod.PluralifyWord(NpcName, data.MaxObjectiveCount - data.ObjectiveCount) + ".";
        }

        public override string RequestShortDescription(RequestData data)
        {
            return "slay " + (data.MaxObjectiveCount - data.ObjectiveCount) + " " + MainMod.PluralifyWord(NpcName, data.MaxObjectiveCount - data.ObjectiveCount);
        }

        public override void OnKillMob(NPC npc, RequestData data)
        {
            if (!data.IsComplete && NpcMod.IsSameMonster(npc, NpcID))
            {
                data.ObjectiveCount++;
                if(data.ObjectiveCount == data.MaxObjectiveCount)
                {
                    Main.NewText("Killed all " + NpcName + " necessary.", new Microsoft.Xna.Framework.Color(200, 200, 200, 255));
                }
            }
        }
    }

    public class ItemRequestBase : RequestBase
    {
        public int ItemID = 0;
        public string ItemName = "";
        public int InitialCount = 5;
        public float ExtraFriendshipLevelCount = 0.334f;

        public ItemRequestBase(int ItemID, string ItemName = "", int InitialCount = 5, float ExtraCount = 0.334f, int RewardValue = 0)
        {
            this.ItemID = ItemID;
                Item i = new Item();
                i.SetDefaults(ItemID, true);
            if(ItemName == "")
            {
                this.ItemName = i.Name;
            }
            else
            {
                this.ItemName = ItemName;
            }
            if (RewardValue == 0)
            {
                this.RewardValue = i.value / 3 * InitialCount;
            }
            else
            {
                this.RewardValue = RewardValue;
            }
            this.InitialCount = InitialCount;
            this.ExtraFriendshipLevelCount = ExtraCount;
        }

        public override int GetRequestObjectiveCount(int FriendshipLevel)
        {
            return InitialCount + (int)(FriendshipLevel * ExtraFriendshipLevelCount);
        }

        public override string RequestObjective(RequestData data)
        {
            if(data.IsComplete)
            {
                return "Deliver the "+data.MaxObjectiveCount+" " + ItemName + " to " + data.RequestGiver.Name + ".";
            }
            return "Get " + data.MaxObjectiveCount + " " + ItemName + ".";
        }

        public override string RequestShortDescription(RequestData data)
        {
            return "bring me " + data.MaxObjectiveCount + " " + MainMod.PluralifyWord(ItemName, data.MaxObjectiveCount);
        }

        public override void UpdateRequest(Player player, RequestData data)
        {
            int ItemCount = 0;
            for(int i = 0; i < 58; i++)
            {
                if (player.inventory[i].type == ItemID)
                {
                    ItemCount += player.inventory[i].stack;
                }
            }
            data.ObjectiveCount = ItemCount;
        }

        public override void OnCompleteRequest(Player player, RequestData data)
        {
            int ItemCount = data.MaxObjectiveCount;
            for (int i = 57; i >= 0; i--)
            {
                if (player.inventory[i].type == ItemID)
                {
                    int StackToRemove = ItemCount;
                    if (StackToRemove > player.inventory[i].stack)
                        StackToRemove = player.inventory[i].stack;
                    player.inventory[i].stack -= StackToRemove;
                    if (player.inventory[i].stack == 0)
                        player.inventory[i].SetDefaults(0, true);
                    ItemCount -= StackToRemove;
                    if (ItemCount <= 0)
                        break;
                }
            }
        }
    }

    public class TravelRequestBase : RequestBase
    {
        public int ExtraDistance = 0;

        public TravelRequestBase(int ExtraDistance = 0, int RewardValue = 0)
        {
            RequiresRequesterSummoned = true;
            this.ExtraDistance = ExtraDistance;
            if(RewardValue == 0)
            {
                this.RewardValue = (2000 + ExtraDistance) / 3;
            }
            else
            {
                this.RewardValue = RewardValue;
            }
        }

        public override void UpdateRequest(Player player, RequestData data)
        {
            if (!data.IsComplete && PlayerMod.HasGuardianSummoned(player, data.RequestGiver.ID, data.RequestGiver.ModID))
            {
                TerraGuardian g = PlayerMod.GetPlayerSummonedGuardian(player, data.RequestGiver.ID, data.RequestGiver.ModID);
                if (!g.KnockedOut && !g.Downed)
                {
                    data.ObjectiveCount += (int)((g.Velocity.X + g.Velocity.Y) * 100);
                    if (data.IsComplete)
                    {
                        Main.NewText(data.RequestGiver.Name + " seems to have enjoyed the travel.", new Microsoft.Xna.Framework.Color(200, 200, 200));
                    }
                }
            }
        }

        public override string RequestObjective(RequestData data)
        {
            if (data.IsComplete)
                return "Talk with " + data.RequestGiver.Name + ".";
            return "Travel with " + data.RequestGiver.Name + ".";
        }

        public override string RequestShortDescription(RequestData data)
        {
            return "take me on your adventures";
        }

        public override int GetRequestObjectiveCount(int FriendshipLevel)
        {
            return (1000 + 30 * FriendshipLevel) * 100 + ExtraDistance;
        }
    }
}
