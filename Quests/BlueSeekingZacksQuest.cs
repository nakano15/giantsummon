using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader.IO;

namespace giantsummon.Quests
{
    public class BlueSeekingZacksQuest : QuestBase
    {
        public override string Name => "Missing";

        public override string Description(QuestData data)
        {
            return "Blue revealed that she's actually looking for someone, and asked your help.";
        }

        public override string GetQuestCurrentObjective(QuestData rawdata)
        {
            BlueQuestData data = (BlueQuestData)rawdata;
            if (!IsQuestComplete(data))
            {
                if(PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Zacks))
                {
                    if (data.BlueDialogueStep == 2)
                        return "Bring Zacks to Blue.";
                    else if(data.BlueDialogueStep == 1)
                        return "Speak to Blue about the Zombie Guardian";
                }
                else if (data.SpottedZacksOnce > 0)
                {
                    switch (data.SpottedZacksOnce)
                    {
                        case 1:
                            return "Tell Blue about the Zombie you saw.";
                        case 2:
                            return "Take Blue with you during a Bloodmoon, on the same place you found the Zombie Guardian.";
                        case 3:
                            return "Find a way of speaking to the Zombie, with Blue's help.";
                    }
                }
                else
                {
                    switch (data.BlueDialogueStep)
                    {
                        case 0:
                            return "Blue may eventually mention this upon speaking to her.";
                        case 1:
                            return "Listen to what Blue has to say if she brings up the topic again.";
                        case 2:
                            return "Find the missing person Blue seeks.";
                    }
                }
            }
            else
            {
                return "You've found the missing person.";
            }
            return base.GetQuestCurrentObjective(data);
        }

        public override string QuestStory(QuestData rawdata)
        {
            string Story = "";
            if (!IsQuestStarted(rawdata))
            {
                Story = "Blue will mention about this to me, once she trusts me enough.";
            }
            else
            {
                BlueQuestData data = (BlueQuestData)rawdata;
                switch (data.BlueDialogueStep)
                {
                    case 1:
                        Story = "Blue told me that she wanted to talk to me about something, but I denied, since I was too busy to listen to what she had to say.";
                        break;
                    case 2:
                        Story = "Blue told me that wanted to speak to me about something. I agreed, and she told me that she didn't actually came to my world just for camping. She told me that was looking for a TerraGuardian, who accompanied a Terrarian named Brandon.\nShe told me that if I find that TerraGuadian, I should tell him to find her.";
                        break;
                }
                if(data.SpottedZacksOnce > 0)
                {
                    if(Story.Length > 0)
                    {
                        Story += "\n\n";
                    }
                    Story += "A Bloodmoon happened on my world, and in the middle of the hordes of horrible monsters, a zombie Wolf TerraGuardian has appeared.";
                    switch (data.SpottedZacksOnce)
                    {
                        case 1:
                            Story += "It tried to devour me, so I had to defend myself.\nI should tell Blue about that, it may be relevant for her.";
                            break;
                        case 2:
                            Story += "It tried to devour me, so I had to defend myself.\nOnce I managed to speak with Blue, she feared that the one she's looking for may be the zombie, and asked me to take her with me the next time a Bloodmoon happens, and return to the place I found the Zombie.";
                            break;
                        case 3:
                            Story += "\nBlue seems to have recognized the Zombie, she said that their name was Zacks. We still had to defend ourselves from that zombie attack.";
                            break;
                    }
                }
                if (PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Zacks))
                {
                    if (data.SpokeToBluePosQuest)
                    {
                        if (Story.Length > 0)
                        {
                            Story += "\n\n";
                        }
                        if (data.BlueDialogueStep == 0)
                        {
                            Story += "After speaking to Blue, I discovered that she was actually looking for a Zombie Guardian I met during my travels. The Zombie Guardian presented himself to me as Zacks. Now Blue and Zacks can be together.\n\n";
                        }
                        else if (data.SpottedZacksOnce == 0)
                        {
                            Story += "I managed to find the TerraGuardian Blue was looking for. I found him living on some world.\nNow Blue and Zacks can be together.\n\n";
                        }
                        else
                        {
                            Story += "We managed to be able to speak to the Zombie Guardian, somehow. After It was weakened, ";
                            if (data.BlueWasPresent)
                            {
                                Story += " It seems like the sound of Blue's voice, made him snap out of the zombie instincts, recognize Blue, and cease his attack.";
                            }
                            else
                            {
                                Story += " It sleuthed a Hairpin Blue gave you, which made him snap out of his zombie instincts, and cease his attack.";
                            }
                            Story += "\nAfterwards, he thanked you for making him be able to think rationally again, and told me that his name is Zacks.\n\n";
                        }
                        Story += "The Zombie Guardian now lives with Blue, and is fighting off against his unending hunger, with her help.";
                        if (!PlayerMod.IsQuestCompleted(Main.LocalPlayer, TgQuestContainer.ZacksMeatbagOutfitQuest)) //If Meatbag Quest isn't completed
                        {
                            Story += "\nAt the same time they are happy for being together, they are saddened due to each other's worries and thoughts.\n\nTHE END?";
                        }
                        else
                        {
                            Story += "\nAfter Blue and I prepared a gift for Zacks, they both managed to forget for a while each other's worries, and managed to spend the " +
                                "sunset together.\n\nTHE END";
                        }
                    }
                    else
                    {
                        if(data.BlueDialogueStep == 2)
                        {
                            if (Story.Length > 0)
                            {
                                Story += "\n\n";
                            }
                            Story += "Zacks seems to meet the description that Blue gave you. You should bring him to her.";
                        }
                    }
                }
            }
            return Story;
        }

        public override bool IsQuestComplete(QuestData rawdata)
        {
            BlueQuestData data = (BlueQuestData)rawdata;
            return data.SpokeToBluePosQuest;
        }

        public override bool IsQuestStarted(QuestData rawdata)
        {
            if (IsQuestComplete(rawdata))
                return true;
            BlueQuestData data = (BlueQuestData)rawdata;
            return data.BlueDialogueStep > 0;
        }

        public override Action ImportantDialogueMessage(QuestData rawdata, TerraGuardian tg, int GuardianID, string GuardianModID)
        {
            if (!IsQuestComplete(rawdata) && GuardianModID == MainMod.mod.Name)
            {
                BlueQuestData data = (BlueQuestData)rawdata;
                if (GuardianID == GuardianBase.Blue)
                {
                    if (((data.BlueDialogueStep == 2 && PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Zacks)) || PlayerMod.PlayerHasGuardianSummoned(Main.LocalPlayer, GuardianBase.Zacks)) && !data.SpokeToBluePosQuest)
                    {
                        return BlueQuestEpilogueDialogue;
                    }
                    else if (tg.FriendshipLevel >= 5 && Main.LocalPlayer.statLifeMax > 100 && data.BlueDialogueStep < 2)
                    {
                        return BluesDialogueMentioningHerSearchForZacks;
                    }
                }
            }
            return base.ImportantDialogueMessage(rawdata, tg, GuardianID, GuardianModID);
        }

        private void BlueQuestEpilogueDialogue()
        {
            BlueQuestData data = (BlueQuestData)Data;
            TerraGuardian Blue = Dialogue.GetSpeaker;
            TerraGuardian Zacks = PlayerMod.GetPlayerSummonedGuardian(Main.LocalPlayer, GuardianBase.Zacks);
            if(Zacks != null)
                Dialogue.AddParticipant(Zacks);
            Dialogue.SetImportantDialogue();
            data.SpokeToBluePosQuest = true;
            if (data.BlueWasPresent)
            {
                if (Zacks == null)
                {
                    switch (data.BlueDialogueStep)
                    {
                        case 0:
                        case 1:
                            Dialogue.ShowDialogueWithContinue("*I'm so happy that we managed to find him...*");
                            Dialogue.ShowDialogueWithContinue("*I have to tell you, [nickname]... My initial intention when I moved here, was to look for him.*");
                            Dialogue.ShowDialogueWithContinue("*I intended to look for him by myself, but we were fortunate to bump into him during your travels.*");
                            Dialogue.ShowDialogueWithContinue("*But what worries me right now is his state current state...*");
                            Dialogue.ShowDialogueWithContinue("*I'll try my best to help him overcome the zombie instinct from trying to take him over again.*");
                            Dialogue.ShowDialogueWithContinue("*Sorry for speaking too much... It's just all too sudden...*");
                            Dialogue.ShowEndDialogueMessage("*Thank you, [nickname]. For helping me save Zacks.*", false);
                            break;
                        case 2:
                            Dialogue.ShowDialogueWithContinue("*I'm so happy that we managed to find Zacks...*");
                            Dialogue.ShowDialogueWithContinue("*But the state we found him really shocked me. I didn't thought he would turn into a Zombie.*");
                            Dialogue.ShowDialogueWithContinue("*Still... I have to do my best to help him overcome his unending hunger issue.*");
                            Dialogue.ShowDialogueWithContinue("*I hope you be able to help Zacks too, [nickname].*");
                            Dialogue.ShowDialogueWithContinue("*So, can I count on you, [nickname]?*");
                            Dialogue.ShowEndDialogueMessage("*Sorry, you don't need to answer. Thank you for helping me so far.*", false);
                            break;
                    }
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*Zacks, how are you feeling?*");
                    Dialogue.ShowDialogueWithContinue("*Not so good... I can hardly move my left leg, and I feel an unending hunger.*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*Don't worry, at least you're back to us.*", Blue);
                    Dialogue.ShowDialogueWithContinue("*Yes, but... What if I end up being a danger for everyone?*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*Then I will be there to stop you, even if I have to lock you at home.*", Blue);
                    Dialogue.ShowDialogueWithContinue("*I like staying at home, anyways.*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*Hahaha... I missed your sense of humor.*", Blue);
                    Dialogue.ShowDialogueWithContinue("*Welcome back, Zacks. I'll help you overcome those zombie instincts.*", Blue);
                    Dialogue.ShowDialogueWithContinue("*Thank you... Blue..*", Zacks);
                    if (data.BlueDialogueStep == 2)
                    {
                        Dialogue.ShowDialogueWithContinue("*I'm really glad that I asked you for help. Now I got Zacks back to my life.*", Blue);
                        Dialogue.ShowEndDialogueMessage("*Thank you very much, [nickname].*", false);
                    }
                    else
                    {
                        Dialogue.ShowDialogueWithContinue("*[nickname], I have to tell you something...*");
                        Dialogue.ShowDialogueWithContinue("*I visitted your world, because I were looking for Zacks.*");
                        Dialogue.ShowDialogueWithContinue("*I thought I could find him on my own, but we managed to bump into him during your travels.*");
                        Dialogue.ShowDialogueWithContinue("*Don't worry much about that, I think not even the Terrarian expected this outcome on their travels.*",Zacks);
                        Dialogue.ShowEndDialogueMessage("*That's true. Thank you, [nickname], for helping bring Zacks back to us.*", false, Blue);
                    }
                }
            }
            else
            {
                if (Zacks == null)
                {
                    Dialogue.ShowDialogueWithContinue("*Zacks!*");
                    Dialogue.ShowDialogueWithContinue("*Hello, Blue...*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*Zacks, what happened to you? How did you ended up like that?*", Blue);
                    Dialogue.ShowDialogueWithContinue("*I think... I was betrayed... If it wasn't for that Terrarian and you, I would still be a brainless zombie.*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*Me? How did I managed to help?*", Blue);
                    Dialogue.ShowDialogueWithContinue("*I caught your scent, on the hairpin you gave to the Terrarian.*", Zacks);
                    Dialogue.ShowDialogueWithContinue("*I'm glad that I managed to help you, somehow...*", Blue);
                    Dialogue.ShowDialogueWithContinue("*[nickname], Thank You for bringing him back to my life.*", Blue);
                    if (data.BlueDialogueStep < 2)
                    {
                        Dialogue.ShowEndDialogueMessage("*I really wish I told you that I was looking for him sooner but... I really though... No.. It's not important...\n" +
                            "Thank you.*", false);
                    }
                    else
                    {
                        Dialogue.ShowEndDialogueMessage("*I'm really happy for trusting you with looking for him...\n" +
                            "Thank you, [nickname].*", false);
                    }
                }
                else
                {
                    Dialogue.ShowDialogueWithContinue("*You managed to find him!*");
                    Dialogue.ShowDialogueWithContinue("*I... Sorry... I shouldn't straight up say that even though you don't know what I'm talking about...*");
                    Dialogue.ShowDialogueWithContinue("*I should have told you earlier, that I was looking for that TerraGuardian who now is a zombie.*");
                    Dialogue.ShowDialogueWithContinue("*I don't know how you managed to save him, but I thank you for that.*");
                    Dialogue.ShowEndDialogueMessage("*Now, is there something else you need?*", false);
                }
            }
            QuestCompletedNotification(data);
        }

        public override List<DialogueOption> AddDialogueOptions(bool IsTalkDialogue, int GuardianID, string GuardianModID)
        {
            if (IsTalkDialogue && IsQuestStarted(Data))
            {
                BlueQuestData data = (BlueQuestData)Data;
                List<DialogueOption> Options = new List<DialogueOption>();
                if (GuardianID == GuardianBase.Blue)
                {
                    if (IsQuestComplete(data))
                    {
                        Options.Add(new DialogueOption("How's Zacks?", BluePosQuestDialogue, true));
                    }
                    else if (data.SpottedZacksOnce > 0)
                    {
                        Options.Add(new DialogueOption("About the zombie TerraGuadian.", BluesDialogueAfterFindingZombifiedGuardian, true));
                    }
                    else if (data.BlueDialogueStep >= 2)
                    {
                        Options.Add(new DialogueOption("Give me more information about the TerraGuardian.", BluesDialogueAboutTheQuest, true));
                    }
                }
                if(GuardianID == GuardianBase.Zacks)
                {
                    Options.Add(new DialogueOption("How are you feeling?", ZacksPostQuestDialogue, true));
                    Options.Add(new DialogueOption("How are Blue?", ZacksDialogueAboutBlue, true));
                }
                return Options;
            }
            return base.AddDialogueOptions(IsTalkDialogue, GuardianID, GuardianModID);
        }

        private void BluesDialogueMentioningHerSearchForZacks()
        {
            BlueQuestData data = (BlueQuestData)Data;
            if(Dialogue.ShowDialogueWithOptions((data.BlueDialogueStep == 0 ? 
                "*[nickname], I have something I have to talk about... Can we talk about it now?*" : 
                "*I must tell you something, can we talk about it right now?*"), new string[] { "Yes", "No" }) == 0)
            {
                Dialogue.ShowDialogueWithContinue("*I'll be sincere with you, [nickname]. The reason why I came here, wasn't for camping. I'm actually looking for someone.*");
                Dialogue.ShowDialogueWithContinue("*The reason why I took so long to tell you this, is because I think I can trust you on this matter..*");
                switch(Dialogue.ShowDialogueWithOptions("*Anyways, do you know why Brandon is?*", new string[] { (Main.LocalPlayer.name.ToLower().Contains("brandon") ? "It's me." : ""), "No, I don't.", "Who?" }))
                {
                    case 0:
                        Dialogue.ShowDialogueWithContinue("*No, not you. The one i'm mentioning is another Brandon. Beside he's also a Terrarian but... It's not you.*");
                        Dialogue.ShowDialogueWithContinue("*I think that by that answer, you don't know who he is.*");
                        break;
                    case 1:
                        Dialogue.ShowDialogueWithContinue("*Hm.. Maybe they didn't got here yet, or you didn't arrived this world at the time they were exploring it. But It has been a really long time... It doesn't makes sense them not arriving here yet.*");
                        break;
                    case 2:
                        Dialogue.ShowDialogueWithContinue("*I think that by that answer, you don't know who he is.*");
                        break;
                }
                Dialogue.ShowDialogueWithContinue("*I really need to find him..*", ContinueText:"Why are you looking for that person?");
                Dialogue.ShowDialogueWithContinue("*I'm not exactly looking for that person, but for the TerraGuardian that accompanied him.*");
                Dialogue.ShowDialogueWithContinue("*Last time I saw them, they were off to do a mission on some Terra Realm world, and then I never heard of them again.*", ContinueText:"What can you tell me about the TerraGuardian you're looking for?");
                Dialogue.ShowDialogueWithContinue("*Well, he's a Wolf Guardian, just like me. He's also taller, and likes pulling jokes on people, really easy to find out.*");
                Dialogue.ShowDialogueWithContinue("*I am getting a bit desperated trying to look for him, so if you find him, please tell me.*", ContinueText:"Okay.");
                QuestStartedNotification(data);
                data.BlueDialogueStep = 2;
                Dialogue.ShowEndDialogueMessage("*Thank you, [nickname]...\nBy the way, want to speak about something else? Or do you want more details?*");
            }
            else
            {
                data.BlueDialogueStep = 1;
                Dialogue.ShowEndDialogueMessage("*Not now? We can speak about this later, then.\n" +
                    "Want to talk about something else?*", false);
            }
        }

        private void BluesDialogueAboutTheQuest()
        {
            string LobbyMes = "*Yes, [nickname]? What would you like to ask?*";
            while (true)
            {
                switch (Dialogue.ShowDialogueWithOptions(LobbyMes, new string[] { "Can you tell me again about his appearance?", "Do you have any leads to where should I search?", "Enough questions." }))
                {
                    case 0:
                        Dialogue.ShowDialogueWithContinue("*He should be following a Terrarian named Brandon. I don't have much memories of him, since I rarelly saw him around.*");
                        Dialogue.ShowDialogueWithContinue("*The TerraGuardian I'm looking for is a Wolf Guardian, just like me. He's taller than me, and likes making jokes.*");
                        Dialogue.ShowDialogueWithContinue("*His name is Zackary, which he hates, so people just call him Zacks instead.*");
                        Dialogue.ShowDialogueWithContinue("*If you manage to find him, could you tell him to find me? I miss him so much.*");
                        LobbyMes = "*Do you want to ask about something else?*";
                        break;

                    case 1:
                        Dialogue.ShowDialogueWithContinue("*Sorry, I don't... I asked some people in the village they got the mission from, and they pointed to this world.*");
                        Dialogue.ShowDialogueWithContinue("*Ever since, I've been looking for them here.*");
                        Dialogue.ShowDialogueWithContinue("*Maybe the people around could have any useful info about him. Asking around may help.*");
                        LobbyMes = "*Other than that, I can't really think of anything else that could help.*";
                        break;

                    case 2:
                        Dialogue.ShowEndDialogueMessage("*I hope what I said ended up being useful on the search.*", false);
                        return;
                }
            }
        }

        private void BluesDialogueAfterFindingZombifiedGuardian()
        {
            BlueQuestData data = (BlueQuestData)Data;
            switch (data.SpottedZacksOnce)
            {
                case 1: //Player mentions, but she didn't see herself
                    Dialogue.ShowDialogueWithContinue("*Zombie TerraGuardian? That's strange. What could a Zombie TerraGuardian be doing here?*", ContinueText: "It was also a Wolf TerraGuardian.");
                    Dialogue.ShowDialogueWithContinue("*What? No, It cannot be... [nickname], next time a Bloodmoon happen, take me with you to where you found them.*");
                    data.SpottedZacksOnce = 2;
                    Dialogue.ShowEndDialogueMessage("*No no no... It can't be him... I really hope not...*", false);
                    break;
                case 2: //After talking to her, after mentioning about the zombie guardian once.
                    Dialogue.ShowDialogueWithContinue("*Yes, bring me with you the next time a Bloodmoon happen, I must see that by myself.*");
                    Dialogue.ShowEndDialogueMessage("*I really hope isn't him...*", false);
                    break;
                case 3: //When player mentions, but she saw by herself.
                    Dialogue.ShowDialogueWithContinue("*I can't believe! How could him... What happ.. How did...? I don't even know what to say...*");
                    Dialogue.ShowDialogueWithContinue("*Zacks... No no no... It can't end up like this...*");
                    Dialogue.ShowDialogueWithContinue("*[nickname], there must be a way of making him be himself again.*");
                    Dialogue.ShowDialogueWithContinue("*Maybe if we speak to him will work.*");
                    Dialogue.ShowEndDialogueMessage("*I hope we can bring him back to his former self...*", false);
                    break;
            }
        }

        private void BluePosQuestDialogue()
        {
            BlueQuestData data = (BlueQuestData)Data;
            Dialogue.ShowDialogueWithContinue("*He's still a zombie, but I'm glad his mind is back to his self.*");
            Dialogue.ShowDialogueWithContinue("*He says that due to his condition, he feels hunger frequently, so I try to help him overcome that.*");
            Dialogue.ShowDialogueWithContinue("*Don't worry, I'll take care of him.*");
            if (data.BlueDialogueStep < 2)
            {
                Dialogue.ShowEndDialogueMessage("*I'm so grateful for you managing to bring Zacks back to my life... I only wished I could have told you about looking for him...*", false);
            }
            else
            {
                Dialogue.ShowEndDialogueMessage("*Thank You, [nickname]... I'm really happy of having your help with this.*", false);
            }
        }

        private void ZacksPostQuestDialogue()
        {
            Dialogue.ShowDialogueWithContinue("*Extremelly hungry, I have to fight day by day against it.*");
            Dialogue.ShowDialogueWithContinue("*I'm really glad that Blue helps me with that.*");
            Dialogue.ShowDialogueWithContinue("*Beside I'm not exactly alive, I'm really happy to be back again.*");
            Dialogue.ShowEndDialogueMessage("*I just want to apologize for whatever happened when you were... You know... Fighting against me...*", false);
        }

        private void ZacksDialogueAboutBlue()
        {
            Dialogue.ShowDialogueWithContinue("*She seems extremelly happy for having me back again, but who wouldn't?*");
            Dialogue.ShowDialogueWithContinue("*But I can see on her face sometimes, that she gets saddened about my current state.*");
            Dialogue.ShowDialogueWithContinue("*She tries her best to hide that, but I can see whenever we talk.*");
            Dialogue.ShowDialogueWithContinue("*I always try to break that with some jokes, and manage to make her laugh a bit.*");
            Dialogue.ShowEndDialogueMessage("*But the worst thing, is the pain of having worried her...*", false);
        }

        public override void OnTalkToNpc(NPC npc)
        {
            if(IsQuestStarted(Data) && !PlayerMod.PlayerHasGuardian(Main.LocalPlayer, GuardianBase.Zacks) && !IsQuestComplete(Data))
            {
                BlueQuestData data = (BlueQuestData)Data;
                if (Main.LocalPlayer.statLifeMax > 100 && Main.rand.NextDouble() < 0.25)
                {
                    if (npc.type == Terraria.ID.NPCID.Merchant)
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                Main.npcChatText = "I have been hearing that the edges of the world are extremelly dangerous during Bloodmoons. I don't know why.";
                                break;
                            case 1:
                                Main.npcChatText = "A person said that a Zombie TerraGuardian tried to devour them when they were by the Beach, last Bloodmoon.";
                                break;
                            case 2:
                                Main.npcChatText = "Watch yourself. There is a terrifying creature that creeps the edges of the world during Bloodmoons.";
                                break;
                        }
                    }
                    if (npc.type == Terraria.ID.NPCID.Nurse)
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                Main.npcChatText = "I know someone who nearly turned into a lunch of a zombified TerraGuardian. They had to incapacitate it so they could escape.";
                                break;
                            case 1:
                                Main.npcChatText = "I don't recommend trying to talk to the zombie TerraGuardian. Anyone who tried that was bited by it.";
                                break;
                        }
                    }
                    if (npc.type == Terraria.ID.NPCID.Dryad)
                    {
                        switch (Main.rand.Next(2))
                        {
                            case 0:
                                Main.npcChatText = "I have been hearing of those rummors of zombie TerraGuardian. I wonder what could have caused them to rise as a zombie.";
                                break;
                            case 1:
                                Main.npcChatText = "Maybe if the zombie TerraGuardian had someone they have affection with close, could cause their bond to be stronger than their desire for flesh.";
                                break;
                        }
                    }
                }
            }
        }

        public override QuestData GetQuestData => new BlueQuestData();

        public class BlueQuestData : QuestData
        {
            /// <summary>
            /// 0 = Blue didn't mentioned that wanted to speak about her search.
            /// 1 = Blue mentioned her searching for someone, but the player didn't listened yet.
            /// 2 = Blue tells the player that she's searching for someone.
            /// 3+ = After spotting Zacks zombie
            /// </summary>
            public byte BlueDialogueStep = 0;
            public bool BlueWasPresent = false, SpokeToBluePosQuest = false;
            public byte SpottedZacksOnce = 0; //1 = Blue wasn't present, 2 = Wasn't present but asked the player to take her with them, 3 = Blue was present
            private const int QuestVersion = 1;

            public override void CustomSaveQuest(string QuestKey, TagCompound Writer)
            {
                Writer.Add("Version", QuestVersion);
                Writer.Add("BlueDialogueStep", BlueDialogueStep);
                Writer.Add("BlueWasPresent", BlueWasPresent);
                Writer.Add("SpokeToBluePosQuest", SpokeToBluePosQuest);
                Writer.Add("SpottedZacksonce", SpottedZacksOnce);
            }

            public override void CustomLoadQuest(string QuestKey, TagCompound Reader, int ModVersion)
            {
                int Version = Reader.GetInt("Version");
                if(Version > 0)
                {
                    BlueDialogueStep = Reader.GetByte("BlueDialogueStep");
                    BlueWasPresent = Reader.GetBool("BlueWasPresent");
                    SpokeToBluePosQuest = Reader.GetBool("SpokeToBluePosQuest");
                    SpottedZacksOnce = Reader.GetByte("SpottedZacksonce");
                }
            }
        }
    }
}