using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Creatures.Luna
{
    public class TutoringDialogues
    {
        public static void StartTutoringDialogue()
        {
            Dialogue.ShowDialogueOnly("*Yes, [nickname]? What do you need help with?*");
            DialogueTopics();
        }

        private static void ReturnToDialogueLobby()
        {
            Dialogue.ShowDialogueOnly("*Do you want to know about anything else?*");
            DialogueTopics();
        }

        private static void DialogueTopics()
        {
            Dialogue.AddOption("Tell me about the TerraGuardians.", AboutTerraGuardians, true);
            Dialogue.AddOption("Tell me what is a Bond.", AboutTheBond, true);
            Dialogue.AddOption("I want to know about befriending companions.", AboutBefriendingTerraGuardians, true);
            Dialogue.AddOption("About Follower Companions.", AboutLeadingTerraGuardians, true);
            Dialogue.AddOption("About The Orders.", AboutTheOrders, true);
            Dialogue.AddOption("About Friendship Rank.", AboutFriendshipRank, true);
            Dialogue.AddOption("About Buddy TerraGuardian.", AboutBuddyGuardian, true);

            //After picking what you want.
            Dialogue.AddOption("I don't have any other question.", EndTutoring, true);
        }

        private static void AboutTerraGuardians()
        {
            Dialogue.ShowDialogueWithContinue("*We TerraGuardians are inhabitants of the Ether Realm.*");
            Dialogue.ShowDialogueWithContinue("*Many Terrarians says that we look like a mix of human with animals, but for us, that's really normal.*");
            Dialogue.ShowDialogueWithContinue("*There are villages and cities in the Ether Realm too, with TerraGuardians living in them, but since recently, people have been coming to Terra Realm too.*");
            Dialogue.ShowDialogueWithContinue("*We mostly are good people, so you hardly will have troubles when meeting a new TerraGuardian. I think many of them actually like meeting Terrarians.*");
            Dialogue.ShowDialogueWithContinue("*TerraGuardians like me, speaks with other creatures through creating bond with them. That's how I'm speaking to you right now.*");
            Dialogue.ShowDialogueWithContinue("*There are some sub types of TerraGuardians that don't speak using bond, so they speak verbally just like you.*");
            ReturnToDialogueLobby();
        }

        private static void AboutBefriendingTerraGuardians()
        {
            Dialogue.ShowDialogueWithContinue("*Just like anyone else, us TerraGuardians like receiving attention.*");
            Dialogue.ShowDialogueWithContinue("*Living in a comfortable house, and helping when they're in need is a good way of making them like you more.*");
            Dialogue.ShowDialogueWithContinue("*As you get more friends with them, will make them not mind doing some things for you.*");
            ReturnToDialogueLobby();
        }

        private static void AboutLeadingTerraGuardians()
        {
            Dialogue.ShowDialogueWithContinue("*Depending on how much friends you are with someone, they may agree to follow you on your adventures.*");
            Dialogue.ShowDialogueWithContinue("*Some of them may even be able to join your adventures right away, if you ask them.*");
            Dialogue.ShowDialogueWithContinue("*You can also give them orders during your adventures, so they can take specific actions that you may find the need of.*");
            Dialogue.ShowDialogueWithContinue("*Pay attention to your group total Weight too. A companion may feel uncomfortable to join you, if there's not enough space for them.*");
            Dialogue.ShowDialogueWithContinue("*Gladly, Leader companions don't need to worry about weight.*");
            ReturnToDialogueLobby();
        }

        private static void AboutTheOrders()
        {
            string Message = "*So, which order do you want to know about?*";
            repeat:
            switch (Dialogue.ShowDialogueWithOptions(Message, new string[] {
            "What are Orders?", "Pull to Me", "Orders", "Action", "Item", "Interaction", "Tactic", "That's all."}))
            {
                case 0:
                    Dialogue.ShowDialogueWithContinue("*You can give orders to TerraGuardians at any time they are following you.*");
                    Dialogue.ShowDialogueWithContinue("(You neeed to have setup a key for calling the Orders list on the control settings of the game.)");
                    Dialogue.ShowDialogueWithContinue("*That way you can tell your companions to do things for you, or change how they should behave in combat*");
                    Dialogue.ShowDialogueWithContinue("(You can navigate through the orders by pressing the number key they are shown as.)");
                    Dialogue.ShowDialogueWithContinue("*Not all orders may be possible to be used on a companion, either due to limitation, or not being friends enough to use it.*");
                    Message = "*They are very useful on your adventure. You should give it a try.*";
                    goto repeat;
                case 1:
                    Message = "*Pull to Me forces pull companions to your position by a binding chain. They are useful if you want to take them off places they can't get, or other reasons.*";
                    goto repeat;
                case 2:
                    Message = "*Orders allows you to change how they will behave. They can follow you, wait, avoid combat, or more.*";
                    goto repeat;
                case 3:
                    Message = "*Action allows you to tell companions take some action. The Free Control action is useful when you're mounted on them, or vice versa.*";
                    goto repeat;
                case 4:
                    Message = "*Item action tells them to use specific items. It's useful when you're going to prepare to face strong challenges.*";
                    goto repeat;
                case 5:
                    Message = "*Interactions contains orders you tell the companion to do with you. Asking them to let you mount on their shoulder is an example.*";
                    goto repeat;
                case 6:
                    Message = "*Tactic allows you to force change your companion combat behavior until you change that again. That will not alter their set behavior, and will be resetted back to normal upon leaving world.*";
                    goto repeat;
                default:
                    Dialogue.ShowDialogueWithContinue("*That's all your questions about orders? Alright.*");
                    break;
            }
            ReturnToDialogueLobby();
        } 

        private static void AboutFriendshipRank()
        {
            Dialogue.ShowDialogueWithContinue("*About that... Hm...*");
            Dialogue.ShowDialogueWithContinue("*As your friendship increases with your companions, so does your Friendship Rank.*");
            Dialogue.ShowDialogueWithContinue("*At some levels, they will require you to increase friendship level a number of times before your rank increases.*");
            Dialogue.ShowDialogueWithContinue("*A number of things may end up being unlocked as friendship rank increases. And increasing it can also increase the maximum weight on your group.*");
            Dialogue.ShowDialogueWithContinue("*That means you can have more or bigger companions following you.*");
            Dialogue.ShowDialogueWithContinue("*As you can wonder, the more you increase it, the better it is for you, so It's good to have more and more people liking you.*");
            ReturnToDialogueLobby();
        }

        private static void AboutLeaderGuardians()
        {
            Dialogue.ShowDialogueWithContinue("*The leading companion has actually a important role on your group.*");
            Dialogue.ShowDialogueWithContinue("*It is the one who is found on the top of you companion list, when checking their status.*");
            Dialogue.ShowDialogueWithContinue("*They are the first companion you will try giving a order that only one companion can do.*");
            Dialogue.ShowDialogueWithContinue("*They are also the only ones you can take control of, so if you want to control a TerraGuardian, you need them as leader guardian.*");
            Dialogue.ShowDialogueWithContinue("*There is no Weight cost for summoning one, and when they're given permission to also take action in your adventures, half of your following companions will follow them.*");
            Dialogue.ShowDialogueWithContinue("(A second player can take control of the leader TerraGuardian, as long as there's 2 controllers plugged in, and they use the second. Controller index for second player can be changed on mod options. Setting it to 2 works.)");
            Dialogue.ShowDialogueWithContinue("*That's only just a few importances that a leader guardian has on your group.*");
            ReturnToDialogueLobby();
        }

        private static void AboutTheBond()
        {
            Dialogue.ShowDialogueWithContinue("*TerraGuardians like me can only speak with other creatures by creating a bond with them.*");
            Dialogue.ShowDialogueWithContinue("*Once we create a bond with a creature, we can use it to express ourselves to the person, and also allow them to understand what we say.*");
            Dialogue.ShowDialogueWithContinue("*We can still understand you if you express yourself in other ways, but if you want to speak in private, you can also use it to.*");
            Dialogue.ShowDialogueWithContinue("*You Terrarians says that it's like as if our voices comes from inside your head, but don't worry, we can't read your mind.*");
            Dialogue.ShowDialogueWithContinue("*Once the bond is created, It can't be broken, unless either dies.*");
            Dialogue.ShowDialogueWithContinue("*In a number of occasions, a bond may be created accidentally, so a TerraGuardian may not even know you're listening to them.*");
            Dialogue.ShowDialogueWithContinue("*It is said that we can speak with each other through the bond from far distances too, but It seems to only happen on a number of occasions.*");
            ReturnToDialogueLobby();
        }

        private static void AboutBuddyGuardian()
        {
            Dialogue.ShowDialogueWithContinue("*That's like... The highest honor a TerraGuardian could have.*");
            Dialogue.ShowDialogueWithContinue("*When a TerraGuardian gets someone as their buddy, it intensifies the bond between both.*");
            Dialogue.ShowDialogueWithContinue("*Both the TerraGuardian and the one they created a Bond with, will grow stronger as their friendship rises.*");
            Dialogue.ShowDialogueWithContinue("*Due to being a Buddy to that TerraGuardian, both have their fates sealed together, so one must not leave the other company.*");
            Dialogue.ShowDialogueWithContinue("*Having more TerraGuardians tagging along will weaken the bond between the two, making the benefits of their friendship drop.*");
            Dialogue.ShowDialogueWithContinue("*If you see someone who has picked a TerraGuardian as buddy, or a TerraGuardian who as picked as someone's buddy, be sure to give them their congratulations.*");
            Dialogue.ShowDialogueWithContinue("*Just thinking of this makes my heart pound. Wonder if someday I'm picked as a buddy?*");
            Dialogue.ShowDialogueWithContinue("*Oh, sorry.. I guess I got carried away by my thoughts.*");
            ReturnToDialogueLobby();
        }

        private static void EndTutoring()
        {
            Dialogue.ShowEndDialogueMessage("*That's enough questions? Alright.\n" +
                "Do you want to speak about something else?*", false);
        }
    }
}
