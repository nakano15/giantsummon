using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Creatures.Leopold
{
    class LoreDialogues
    {
        public static void StartDialogue()
        {
            Dialogue.ShowDialogueOnly("*You have questions? Alright, I can try answering them.*");
            GetTopics();
        }

        private static void ReturnToTopics()
        {
            Dialogue.ShowDialogueOnly("*Do you want to know about something else?*");
            GetTopics();
        }

        private static void GetTopics()
        {
            Dialogue.AddOption("About the Ether Realm", AboutEtherRealm, true);
            Dialogue.AddOption("About the Terra Realm", AboutTerraRealm, true);

            Dialogue.AddOption("That's was all my questions.", EndDialogue, true);
        }

        private static void AboutEtherRealm()
        {
            Dialogue.ShowDialogueWithContinue("*The Ether Realm is a realm where TerraGuardians came from.*");
            Dialogue.ShowDialogueWithContinue("*We have kingdoms and towns there, and several places too, includding unknwon ones.*");
            Dialogue.ShowDialogueWithContinue("*But from what I've read in a book, the Ether Realm is a dangerous place for Terra Realm people, like you.*");
            Dialogue.ShowDialogueWithContinue("*It seems like there's something in there, that weakens Terra Realm creatures. It's not specified what it is.*");
            Dialogue.ShowDialogueWithContinue("*TerraGuardians will not have much problems there, so if you plan on visiting it some time, I may recommend you to take TerraGuardians with you.*");
            ReturnToTopics();
        }

        private static void AboutTerraRealm()
        {
            Dialogue.ShowDialogueWithContinue("*The Terra Realm is where we are now.*");
            Dialogue.ShowDialogueWithContinue("*You probably know a lot more about it than me.*");
            Dialogue.ShowDialogueWithContinue("*It is said that long time ago, TerraGuardians used to live alongside Terrarians, and help protect the Terra Realm.*");
            Dialogue.ShowDialogueWithContinue("*Maybe that explains why we're called TerraGuardians. I don't know why we left to the Ether Realm.*");
            Dialogue.ShowDialogueWithContinue("*One way we will end up finding out why, or at least I am reading many Terra Realms books to find out why.*");
            ReturnToTopics();
        }

        private static void EndDialogue()
        {
            Dialogue.ShowEndDialogueMessage("*That's all your questions? I hope the information I gave you ends up being useful." +
                "\nDo you want to speak about something else?*", false);
        }
    }
}
