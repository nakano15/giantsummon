using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class QuestBase
    {
        public static QuestData Data = new QuestData();

        public virtual string Name { get; }
        public virtual string Description(QuestData data) { return ""; }
        public virtual string QuestStory(QuestData data) { return ""; }
        public virtual string GetQuestCurrentObjective(QuestData data) { return ""; }

        public virtual bool IsQuestStarted(QuestData data) { return false; }
        public virtual bool IsQuestComplete(QuestData data) { return false; }

        public bool Invalid = false;

        public virtual QuestData GetQuestData { get { return new QuestData(); } }

        public virtual List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            return new List<DialogueOption>();
        }

        public virtual Action ImportantDialogueMessage(QuestData data, TerraGuardian tg, int GuardianID, string GuardianModID) //Shows up once you talk to the companion. If return an action, will make the dialogue on the action to show up.
        {
            return null;
        }

        public virtual void UpdatePlayer(Player player, QuestData data)
        {

        }

        public virtual void OnMobKill(NPC killedNpc)
        {

        }

        public virtual void OnTalkToNpc(NPC npc)
        {

        }

        public virtual string QuestNpcDialogue(NPC npc)
        {
            return "";
        }
    }
}
