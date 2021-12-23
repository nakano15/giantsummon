using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class TutorialQuest : QuestBase
    {
        public override string Name => "Know More";

        public override QuestData GetQuestData => new TutorialQuestData();

        public override string Description(QuestData data)
        {
            return "Luna will help me know how to manage companions.";
        }

        public override string QuestStory(QuestData rawdata)
        {
            TutorialQuestData data = (TutorialQuestData)rawdata;
            string Story = "";
            if (!IsQuestStarted(data))
            {
                Story = "I haven't met Luna yet.";
            }
            else
            {
                if (!data.LunaTalkedAboutTutorial)
                {
                    Story = "I have met TerraGuardians, and I've met Luna, who tutors me about them, but I don't know much about how they can help me.";
                }
                else if (!AnyStepInitiated(data))
                {
                    Story = "Luna told me that she can tell me about some things I must know about the companions.\nI should speak to her about it if I want to know more.";
                }
                else
                {
                    switch (data.CallingStep)
                    {
                        case 1:
                            Story += "Luna told me that I need to experience having a companion following me.\nI should either speak with them on the Guardian Profile List, or ask them directly if they want to follow me.";
                            break;
                        case 2:
                            Story += "I have experienced having companion followers with me.\nThe companion followers can wear equipments, weapons and accessories just like me.\nThey can also use items like Life Crystal, and Potions, when necessary.\nShe told me that I should care of them during my adventures, and that they will grow stronger as they venture with me.";
                            break;
                    }
                    if (data.OrderTutorialStep > 0 && Story.Length > 0)
                        Story += "\n\n";
                    switch (data.OrderTutorialStep)
                    {
                        case 1:
                            Story += "Luna is now introducing me to giving orders to companions.\nShe told me that before I can experience that, I must have a companion following me, and then speak with her again.";
                            break;
                        case 2:
                            Story += "Luna told me that while having a companion following me, I should try opening the order hud, and then try giving an order to my follower companions.\n" +
                                "(If you didn't setup the order button on the game Control options, do so before you can do this.)";
                            break;
                        case 3:
                            Story += "I managed to give an order to my companions, like Luna told me to do.\nI should tell her that I did that.";
                            break;
                        case 4:
                            Story += "Luna teached me how to give orders to my companions.\nI must call the orders interface by pressing a button, and then navigate through it by using the number keys.\n" +
                                "In case I have not setup a key, I should go into the game Control options, and set a key of easy access to call that interface.";
                            break;
                    }
                    if (data.TalkedAboutTentants)
                    {
                        if(Story.Length > 0)
                            Story += "\n\n";
                        Story += "Luna also told me about tenant companions. Some companions may be interessed in living in this world if I ask them.\n" +
                            "Letting them live in my world, will make them accessible in case I want to speak with them, and they will like me more if they have furnitures they can use, like chairs, and beds.";
                    }
                }
            }
            return Story;
        }

        private bool AnyStepInitiated(TutorialQuestData data)
        {
            return data.OrderTutorialStep > 0 || data.CallingStep > 0 || data.TalkedAboutTentants;
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            TutorialQuestData data = (TutorialQuestData)rawdata;
            if (!data.LunaTalkedAboutTutorial)
                return "Speak to Luna.";
            if (data.CallingStep < 2)
                return "Luna can teach me about followers.";
            if (data.OrderTutorialStep < 4)
                return "Luna can teach me about Orders.";
            if (!data.TalkedAboutTentants)
                return "Luna can teach me about Tenants.";
            return "Luna has nothing to tell you about right now.";
        }

        public override bool IsQuestStarted(QuestData data)
        {
            return PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Luna);
        }

        public override bool IsQuestComplete(QuestData data)
        {
            return base.IsQuestComplete(data);
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (IsQuestStarted(rawdata) && GuardianID == GuardianBase.Luna && GuardianModID == MainMod.mod.Name)
            {
                TutorialQuestData data = (TutorialQuestData)rawdata;
                if (!data.LunaTalkedAboutTutorial)
                {
                    data.LunaTalkedAboutTutorial = true;
                    return LunaDialogue;
                }
            }
            return base.ImportantDialogueMessage(rawdata, tg, GuardianID, GuardianModID);
        }

        private void LunaDialogue()
        {
            Dialogue.ShowDialogueWithContinue("*You've got some contact with TerraGuardians, Terrarian. There are a number of things you may need to know, that aren't well explained.*");
            Dialogue.ShowEndDialogueMessage("*When possible, I can try explaining some things about them, without going much into depth about it.*", false, CloseOptionText: "Alright");
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            if (IsQuestStarted(Data) && GuardianID == GuardianBase.Luna && GuardianModID == MainMod.mod.Name)
            {
                TutorialQuestData data = (TutorialQuestData)Data;
                List<DialogueOption> Options = new List<DialogueOption>();
                if(data.CallingStep < 2)
                    Options.Add(new DialogueOption("Tell me about Followers.", TalkAboutFollowerCompanions, true));
                if(!data.TalkedAboutTentants) Options.Add(new DialogueOption("Tell me about Tenants.", TalkAboutCompanionsLivingInTheWorld, true));
                if(data.OrderTutorialStep < 4)
                    Options.Add(new DialogueOption("Tell me about Orders.", TalkAboutOrders, true));
                return Options;
            }
            return base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
        }

        public override void UpdatePlayer(Player player, QuestData rawdata)
        {
            TutorialQuestData data = (TutorialQuestData)rawdata;
            if (data.OrderTutorialStep == 2)
            {
                if (GuardianOrderHudNew.Active || GuardianOrderHudNew.Active)
                {
                    data.OrderTutorialStep = 3;
                    Main.NewText("You have managed to open the order interface. Navigate through it with the number keys.");
                }
            }
        }

        private void TalkAboutOrders()
        {
            TutorialQuestData data = (TutorialQuestData)Data;
            switch (data.OrderTutorialStep)
            {
                case 0: //First talk
                    Dialogue.ShowDialogueWithContinue("*When you have follower companions with you, you can give them orders.*");
                    Dialogue.ShowDialogueWithContinue("*Depending on what you tell them to do, they may do it, if they can or want.*");
                    Dialogue.ShowDialogueWithContinue("*Why don't you give it a try?*");
                    if (!PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                    {
                        Dialogue.ShowDialogueWithContinue("*To try that, first you need to have a companion with you.*");
                        data.OrderTutorialStep = 1;
                        Dialogue.ShowEndDialogueMessage("*Ask a companion to join your adventure and then speak to me. You can also call me if you want. I will be glad to help you with this.*", false);
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("*You have someone following you. Try giving them an order.*");
                        Dialogue.ShowDialogueWithContinue("(If you didn't setup a key for calling the order interface, go on the game Control options, and set a key of easy access for it.)");
                        data.OrderTutorialStep = 2;
                        Dialogue.ShowEndDialogueMessage("*Once you do that, talk to me again.*", false);
                    }
                    break;
                case 1: //Try using order, but have a companion in your group first.
                    if (!PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                    {
                        Dialogue.ShowDialogueWithContinue("*In order for you to try giving the companions an order, you need to have a companion following you.*");
                        Dialogue.ShowEndDialogueMessage("*Try asking a companion to follow you, and then speak to me again. You may even ask me to join your group, if you want.*", false);
                    }
                    else
                    {
                        if (PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).MyID.IsSameID(GuardianBase.Luna, ""))
                        {
                            Dialogue.ShowDialogueWithContinue("*Since I'm now your follower, you can try giving me some order for me to do.*");
                        }
                        else
                        {
                            Dialogue.ShowDialogueWithContinue("*Perfect. Now that you have at least a companion on your group, you can try giving them an order.*");
                        }
                        Dialogue.ShowDialogueWithContinue("*Once you open the order list, you can navigate them with ease by pressing the numbers.*");
                        Dialogue.ShowDialogueWithContinue("*Orders that aren't grayed are orders the companion can use.*");
                        Dialogue.ShowDialogueWithContinue("*Why don't you try giving an order right away?*");
                        data.OrderTutorialStep = 2;
                        Dialogue.ShowDialogueWithContinue("(If you haven't setup the order key, open your game Control options, and set the order interface call button to any easy access key.)"); //Dialogue doesn't proceed here
                        Dialogue.ShowEndDialogueMessage("*Try giving an order, and then speak to me again.*", false);
                    }
                    break;
                case 2: //Try using order, you have a companion in your group.
                    {
                        Dialogue.ShowDialogueWithContinue("*You need to experience giving an order to a companion.*");
                        if (!PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                        {
                            Dialogue.ShowDialogueWithContinue("*But to do that, you need to have a companion with you. Have at least one companion with you, before you can try opening the order interface.*");
                        }
                        else
                        {
                            Dialogue.ShowDialogueWithContinue("*You already have a companion invoked, all that's left now is try to call the order.*");
                        }
                        Dialogue.ShowDialogueWithContinue("(If you haven't setup the order button yet. Go in the game Control options, and setup the order key to a easy access key.)");
                    }
                    break;
                case 3: //Used order hud
                    {
                        Dialogue.ShowDialogueWithContinue("*You now experienced how it is to give an order to a companion.*");
                        Dialogue.ShowDialogueWithContinue("*Use that to help you on your travels, change their behaviors in combat, or aid you in other ways.*");
                        data.OrderTutorialStep = 4;
                        Dialogue.ShowEndDialogueMessage("*I hope what you learned from me now will be useful on your travels.*", false);
                    }
                    break;
            }
        }

        private void TalkAboutFollowerCompanions()
        {
            TutorialQuestData data = (TutorialQuestData)Data;
            switch (data.CallingStep)
            {
                case 0: //Intro
                    Dialogue.ShowDialogueWithContinue("*Depending on the companion friendship level, and how much weight left you have on your group size, a companion may be willing to join your travels.*");
                    Dialogue.ShowDialogueWithContinue("*Just like you, they can also use equipments, which makes them stronger, and allow them to help you on your travels.*");
                    Dialogue.ShowDialogueWithContinue("*The first companion you call is the leader. The leader not only disregards the weight limit of your group, but also are the most important companions in your group.*");
                    Dialogue.ShowDialogueWithContinue("*There are a few things that only the leader can do, but that doesn't makes the assistant companions be less valuable for your travels.*");
                    if (PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                    {
                        Dialogue.ShowDialogueWithContinue("*You have a companion following you, that means you have some experience on how follower companions work.*");
                        Dialogue.ShowDialogueWithContinue("*Care for them on your travels, and they will grow stronger as they follow you on your adventures.*");
                        data.CallingStep = 2;
                        Dialogue.ShowEndDialogueMessage("*That is all I have to say about the follower companions.*", false);
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("*Why don't you try calling a companion to follow you?*");
                        data.CallingStep = 1;
                        Dialogue.ShowEndDialogueMessage("*Ask a companion to follow you. If they agree, come talk to me with them following you.*", false);
                    }
                    break;
                case 1: //Call a companion and speak to her.
                    if (!PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                    {
                        Dialogue.ShowDialogueWithContinue("*You need to experience for yourself how it is to have followers with you.*");
                        Dialogue.ShowEndDialogueMessage("*Ask a companion to follow you, and if they accept, they will happily join your travels. Talk to me when you get yourself a follower.*", false);
                    }
                    else
                    {
                        bool IsLuna = PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).MyID.IsSameID(GuardianBase.Luna);
                        if (IsLuna)
                        {
                            Dialogue.ShowDialogueWithContinue("*I am right now your follower. Anywhere you go, I will be following you.*");
                            Dialogue.ShowDialogueWithContinue("*Beside my size is really high, we have about the same dimension for blocks collision, so we can follow you almost anywhere.*");
                            Dialogue.ShowDialogueWithContinue("*Like I mentioned before, we can use equipments, weapons and accessories.*");
                            Dialogue.ShowDialogueWithContinue("*We can also use Life Crystals, Mana Crystals and those items, to increase our status, so don't forget about us.*");
                            Dialogue.ShowDialogueWithContinue("*If you care well about your followers, they will help you on your adventures no matter where you go.*");
                            Dialogue.ShowDialogueWithContinue("*We will also get stronger as we explore the world with you, so we can help you more and more during your travels.*");
                            data.CallingStep = 2;
                            Dialogue.ShowEndDialogueMessage("*I hope we may be able to assist you in any way as possible on your adventures.*", false);
                        }
                        else
                        {
                            Dialogue.ShowDialogueWithContinue("*You got yourself a follower. Anywhere you go, they will follow you.*");
                            Dialogue.ShowDialogueWithContinue("*Depending on the companion size, they may be way bigger than you, but don't worry about that. Independing on their size, they can still follow you in places you can pass through.*");
                            Dialogue.ShowDialogueWithContinue("*Just like you, followers can use equipments, weapons and accessories.*");
                            Dialogue.ShowDialogueWithContinue("*Companions can also use items like Life Crystals and Mana Crystals to increase their status. So they can get stronger if you give them those.*");
                            Dialogue.ShowDialogueWithContinue("*If you care well for your followers, they will help you on your adventures anytime you want.*");
                            Dialogue.ShowDialogueWithContinue("*And they will get stronger as the travel with you, so they can help you more and more during your travels.*");
                            data.CallingStep = 2;
                            Dialogue.ShowEndDialogueMessage("*I hope they are able to assist you on your travels.*", false);
                        }
                    }
                    break;
            }
        }

        private void TalkAboutCompanionsLivingInTheWorld()
        {
            TutorialQuestData data = (TutorialQuestData)Data;
            Dialogue.ShowDialogueWithContinue("*Some companions may be interessed in living in your world.*");
            Dialogue.ShowDialogueWithContinue("*If you have a house big enough for them to live, they may move in to it.*");
            Dialogue.ShowDialogueWithContinue("*Having companions living in your world is good. You can keep contact with them, and they can also protect your town npcs from threats.*");
            Dialogue.ShowDialogueWithContinue("*Furnishing well their houses brings benefits too. They can use furnitures, and will like you more for caring them enough for placing them.*");
            Dialogue.ShowDialogueWithContinue("*We even will use beds when it's their sleep time, if there is bed disponible on our houses.*");
            Dialogue.ShowDialogueWithContinue("*You only need to be careful when making houses for companions, since they will not like having to crouch to move in it. But you don't need to worry about the doors.*");
            data.TalkedAboutTentants = true;
            Dialogue.ShowEndDialogueMessage("*I wont give you a test for this, but I recommend you to allow companions to move in to your world.*", false);
        }

        public class TutorialQuestData : QuestData
        {
            public bool LunaTalkedAboutTutorial = false;
            public byte CallingStep = 0;
            public byte OrderTutorialStep = 0;
            public bool TalkedAboutTentants = false;
            private const int SaveVersion = 0;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add(QuestKey + "_Version", SaveVersion);
                Writer.Add(QuestKey + "_TalkedAboutTuto", LunaTalkedAboutTutorial);
                Writer.Add(QuestKey + "_CallTutoStep", CallingStep);
                Writer.Add(QuestKey + "_OrderTutoStep", OrderTutorialStep);
                Writer.Add(QuestKey + "_TenantTutoStep", TalkedAboutTentants);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                int Version = Reader.GetInt(QuestKey + "_Version");
                LunaTalkedAboutTutorial = Reader.GetBool(QuestKey + "_TalkedAboutTuto");
                CallingStep = Reader.GetByte(QuestKey + "_CallTutoStep");
                OrderTutorialStep = Reader.GetByte(QuestKey + "_OrderTutoStep");
                TalkedAboutTentants = Reader.GetBool(QuestKey + "_TenantTutoStep");
            }
        }
    }
}
