﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class QuestContainer
    {
        private static Dictionary<string, QuestContainer> ModQuestContainer = new Dictionary<string, QuestContainer>();
        private Dictionary<int, QuestBase> QuestList = new Dictionary<int, QuestBase>();
        private static QuestBase InvalidQuest = new QuestBase() { Invalid = true };

        public static void AddQuestContainer(Mod mod, QuestContainer container)
        {
            if (ModQuestContainer.ContainsKey(mod.Name))
                ModQuestContainer[mod.Name] = container;
            else
                ModQuestContainer.Add(mod.Name, container);
            container.CreateQuestDB();
        }

        public static void CreateQuestListToPlayer(PlayerMod Player)
        {
            Player.QuestDatas.Clear();
            foreach (string s in ModQuestContainer.Keys)
            {
                foreach(int i in ModQuestContainer[s].QuestList.Keys)
                {
                    QuestData qd = ModQuestContainer[s].QuestList[i].GetQuestData;
                    qd.QuestID = i;
                    qd.QuestModID = s;
                    Player.QuestDatas.Add(qd);
                }
            }
        }

        public virtual void CreateQuestDB()
        {

        }

        public void AddQuest(int ID, QuestBase quest)
        {
            if (QuestList.ContainsKey(ID))
                QuestList[ID] = quest;
            else
                QuestList.Add(ID, quest);
        }

        public static QuestBase GetQuestBase(int QuestID, string ModID = "")
        {
            if(ModID == "")
            {
                ModID = MainMod.mod.Name;
            }
            if (ModQuestContainer.ContainsKey(ModID))
            {
                if (ModQuestContainer[ModID].QuestList.ContainsKey(QuestID))
                {
                    return ModQuestContainer[ModID].QuestList[QuestID];
                }
            }
            return InvalidQuest;
        }
    }
}
