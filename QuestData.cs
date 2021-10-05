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
        public bool IsUnexistingQuest = false;
        private Terraria.ModLoader.IO.TagCompound SavedQuestData = null;

        public int QuestID = 0;
        public string QuestModID = "";
        public string GetQuestKey { get { return "quest_" + QuestID + ":" + QuestModID; } }

        public void SaveQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Writer)
        {
            Writer.Add(QuestKey + "_ID", QuestID);
            Writer.Add(QuestKey + "_ModID", QuestModID);
            Terraria.ModLoader.IO.TagCompound QuestData = new Terraria.ModLoader.IO.TagCompound();
            if (IsUnexistingQuest)
                QuestData = SavedQuestData;
            else
                CustomSaveQuest(QuestKey, QuestData);
            Writer.Add(QuestKey + "_QuestData", QuestData);
        }

        public static void LoadQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Reader, int ModVersion, PlayerMod pm)
        {
            int QuestID = Reader.GetInt(QuestKey + "_ID");
            string QuestModID = Reader.GetString(QuestKey + "_ModID");
            QuestData qd = null;
            foreach(QuestData qd2 in pm.QuestDatas)
            {
                if(qd2.QuestID == QuestID && qd2.QuestModID == QuestModID)
                {
                    qd = qd2;
                    break;
                }
            }
            Terraria.ModLoader.IO.TagCompound QuestData = Reader.Get<Terraria.ModLoader.IO.TagCompound>(QuestKey + "_QuestData");
            if (qd == null)
            {
                qd = QuestContainer.GetQuestBase(QuestID, QuestModID).GetQuestData;
                qd.QuestID = QuestID;
                qd.QuestModID = QuestModID;
                qd.IsUnexistingQuest = true;
                qd.SavedQuestData = QuestData;
                pm.QuestDatas.Add(qd);
            }
            else
            {
                qd.CustomLoadQuest(QuestKey, QuestData, ModVersion);
            }
        }

        public virtual void CustomSaveQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Writer)
        {

        }

        public virtual void CustomLoadQuest(string QuestKey, Terraria.ModLoader.IO.TagCompound Reader, int ModVersion)
        {

        }
    }
}
