using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class RequestBase
    {
        public string Name = "";
        public string BriefText = "", AcceptText = "", DenyText = "", CompleteText = "", RequestInfoText = "";
        public List<RequestObjective> Objectives = new List<RequestObjective>();
        public delegate bool RequestRequirementDel(Terraria.Player player);
        public RequestRequirementDel Requirement = delegate(Terraria.Player player) { return true; };
        public int RequestScore = 500;

        public RequestBase(string Name, int RequestScore, string BriefText, string AcceptText, string DenyText, string CompleteText, string RequestInfoText)
        {
            this.Name = Name;
            this.RequestScore = RequestScore;
            this.BriefText = BriefText;
            this.AcceptText = AcceptText;
            this.DenyText = DenyText;
            this.CompleteText = CompleteText;
            this.RequestInfoText = RequestInfoText;
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

        public void AddExploreRequest(float InitialDistance, float DistanceIncreasePerFriendLevel = 100f)
        {
            ExploreRequest req = new ExploreRequest();
            req.InitialDistance = InitialDistance;
            req.StackIncreasePerFriendshipLevel = DistanceIncreasePerFriendLevel;
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

        public void AddRequesterRequirement()
        {
            RequestObjective req = new RequestObjective(RequestObjective.ObjectiveTypes.RequiresRequester);
            Objectives.Add(req);
        }

        public void AddTalkRequest()
        {
            RequestObjective req = new RequestObjective(RequestObjective.ObjectiveTypes.TalkQuest);
            Objectives.Add(req);
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
                TalkQuest,
                RequiresRequester
            }
        }
    }
}
