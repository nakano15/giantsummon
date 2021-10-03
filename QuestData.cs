using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class QuestData
    {
        public QuestBase GetBase { get { if (_Base == null) { _Base = QuestContainer.GetQuestBase(QuestID, QuestModID); } return _Base; } }
        private QuestBase _Base;

        public string QuestName { get { return GetBase.Name; } }
        public string Description { get { return GetBase.Description(this); } }
        public string Story { get { return GetBase.QuestStory(this); } }
        public string Objective { get { return GetBase.GetQuestCurrentObjective(this); } }
        public bool IsInvalid { get { return GetBase.Invalid; } }
        public bool IsQuestStarted { get { return GetBase.IsQuestStarted(this); } }
        public bool IsQuestCompleted { get { return GetBase.IsQuestComplete(this); } }

        public int QuestID = 0;
        public string QuestModID = "";
        public string GetQuestKey { get { return "quest_" + QuestID + ":" + QuestModID; } }

        public void SaveQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Writer)
        {
            Writer.Add(QuestKey + "_ID", QuestID);
            Writer.Add(QuestKey + "_ModID", QuestModID);
            CustomSaveQuest(QuestKey, Writer);
        }

        public void LoadQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Reader, int ModVersion)
        {
            QuestID = Reader.GetInt(QuestKey + "_ID");
            QuestModID = Reader.GetString(QuestKey + "_ModID");
            CustomLoadQuest(QuestKey, Reader, ModVersion);
        }

        public virtual void CustomSaveQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Writer)
        {

        }

        public virtual void CustomLoadQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Reader, int ModVersion)
        {

        }
    }
}
