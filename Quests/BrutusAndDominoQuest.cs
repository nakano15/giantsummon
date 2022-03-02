using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Quests
{
    public class BrutusAndDominoQuest : QuestBase
    {
        private static bool PlayerHasBrutus { get { return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Brutus); } }
        private static bool PlayerHasDomino { get { return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Domino); } }

        public override string Name => "Frenemies";

        public override string Description(QuestData data)
        {
            return "";
        }

        public override string QuestStory(QuestData data)
        {
            string Story = "";
            if (PlayerHasBrutus)
            {
                Story = "I have met a TerraGuardian that was wanting to work as someone bodyguard. I have hired him to be my bodyguard. He said that his name is Brutus.";
            }
            if (PlayerHasDomino)
            {
                if (Story != "")
                    Story += "\n\n";
                Story += "During my travels, I bumped into someone that Brutus wanted to catch. I helped him catch that person, who later I discovered was named Domino, and found out that Brutus was a royal guard in the Ether Realm, before moving to this world.\n" +
                    "Since in my world there is no issues with the business Domino does, they now does business on my world, and Brutus can't arrest him.";
            }
            return Story;
        }

        public override string GetQuestCurrentObjective(QuestData data)
        {
            return base.GetQuestCurrentObjective(data);
        }

        public override bool IsQuestStarted(QuestData data)
        {
            return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Brutus);
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return base.IsQuestComplete(data);
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            List<DialogueOption> options = base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);

            return options;
        }
    }
}
