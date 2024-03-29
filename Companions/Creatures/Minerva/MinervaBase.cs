﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Companions
{
    public class MinervaBase : GuardianBase
    {
        /// <summary>
        /// -Not very sociable.
        /// -Loves cooking.
        /// -Likes Vladimir.
        /// -Has flatulence problems.
        /// -Loves clear weather days.
        /// -Likes seeing people enjoy her food.
        /// -Is fat due to experimenting her own food, to see if It's good.
        /// </summary>
        public MinervaBase()
        {
            Name = "Minerva";
            Description = "She's not very sociable, but is\na good cook. If you're feeling\nhungry, go see her to get food.";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 90;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 112;
            CompanionSlotWeight = 1.4f;
            Scale = 108f / 90f;
            Age = 19;
            SetBirthday(SEASON_WINTER, 9);
            Male = false;
            InitialMHP = 300; //1000
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 20;
            Accuracy = 0.47f;
            Mass = 0.62f;
            MaxSpeed = 4.8f;
            Acceleration = 0.16f;
            SlowDown = 0.52f;
            MaxJumpHeight = 16;
            JumpSpeed = 7.60f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();

            CallUnlockLevel = 2;
            MoveInLevel = 3;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            PlayerMountedArmAnimation = JumpFrame = 10;
            HeavySwingFrames = new int[] { 21, 22, 23 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            DuckingFrame = 24;
            DuckingSwingFrames = new int[] { 25, 26, 27 };
            SittingFrame = 16;
            ChairSittingFrame = 15;
            ThroneSittingFrame = 18;
            BedSleepingFrame = 17;
            SleepingOffset.X = 16;
            ReviveFrame = 20;
            DownedFrame = 19;
            //PetrifiedFrame = 28;

            BackwardStanding = 28;
            BackwardRevive = 29;

            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.PearlwoodSword, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.AmethystStaff, 1));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.BottledHoney, 5));
            InitialItems.Add(new ItemPair(Terraria.ID.ItemID.BowlofSoup, 5));

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);

            RightArmFrontFrameSwap.Add(0, 0);
            RightArmFrontFrameSwap.Add(15, 1);
            RightArmFrontFrameSwap.Add(16, 1);
            RightArmFrontFrameSwap.Add(20, 2);
            RightArmFrontFrameSwap.Add(21, 3);

            SittingPoint2x = new Point(22, 43);

            MountShoulderPoints.DefaultCoordinate2x = new Point(18, 21);
            MountShoulderPoints.AddFramePoint2x(20, 23, 33);
            MountShoulderPoints.AddFramePoint2x(24, 18, 35);
            MountShoulderPoints.AddFramePoint2x(25, 18, 35);
            MountShoulderPoints.AddFramePoint2x(26, 18, 35);
            MountShoulderPoints.AddFramePoint2x(27, 18, 35);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(11, 14, 7);
            LeftHandPoints.AddFramePoint2x(12, 36, 14);
            LeftHandPoints.AddFramePoint2x(13, 41, 26);
            LeftHandPoints.AddFramePoint2x(14, 33, 36);

            LeftHandPoints.AddFramePoint2x(20, 30, 47);

            LeftHandPoints.AddFramePoint2x(21, 21, 15);
            LeftHandPoints.AddFramePoint2x(22, 38, 22);
            LeftHandPoints.AddFramePoint2x(23, 34, 43);

            LeftHandPoints.AddFramePoint2x(25, 24, 19);
            LeftHandPoints.AddFramePoint2x(26, 34, 30);
            LeftHandPoints.AddFramePoint2x(27, 35, 45);

            //Right Arm
            RightHandPoints.AddFramePoint2x(11, 17, 7);
            RightHandPoints.AddFramePoint2x(12, 38, 14);
            RightHandPoints.AddFramePoint2x(13, 43, 26);
            RightHandPoints.AddFramePoint2x(14, 35, 36);

            RightHandPoints.AddFramePoint2x(20, 34, 47);

            RightHandPoints.AddFramePoint2x(21, 24, 15);
            RightHandPoints.AddFramePoint2x(22, 41, 22);
            RightHandPoints.AddFramePoint2x(23, 36, 43);

            RightHandPoints.AddFramePoint2x(25, 26, 19);
            RightHandPoints.AddFramePoint2x(26, 37, 30);
            RightHandPoints.AddFramePoint2x(27, 37, 45);

            //Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(24, 17);
            HeadVanityPosition.AddFramePoint2x(20, 28, 28);

            HeadVanityPosition.AddFramePoint2x(23, 28, 22);

            HeadVanityPosition.AddFramePoint2x(24, 24, 31);
            HeadVanityPosition.AddFramePoint2x(25, 24, 31);
            HeadVanityPosition.AddFramePoint2x(26, 24, 31);
            HeadVanityPosition.AddFramePoint2x(27, 24, 31);

            SetDialogues();
            SetRewards();
        }

        public void SetDialogues()
        {

        }

        public void SetRewards()
        {
            AddReward(Terraria.ID.ItemID.CookedFish, 3, 0.66f);
            AddReward(Terraria.ID.ItemID.BowlofSoup, 3, 0.5f);
        }

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> Options = new List<DialogueOption>();
            Options.Add(new DialogueOption("Can you cook something for me?", CookDialogue, true));
            return Options;
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if ((guardian.PlayerMounted || guardian.ItemAnimationTime > 0) && Frame == 0)
                Frame = 1;
        }

        public override string CallUnlockMessage => "*[nickname]... I have a thing to ask of you... Do you mind... If I follow you on your quest? You walk a lot, right? Maybe I will lose some weight If I follow you. Can I?*";
        public override string MountUnlockMessage => "*[nickname], you seems to be very tired. I can carry you on my shoulder, if you need some rest.*";
        public override string ControlUnlockMessage => "*Ah! [nickname]... Here. This is just to tell you... That I trust you.*";
        public override string MoveInUnlockMessage => "*I think It will be benefitial for both of us, If I moved to this world... I could cook for anyone in need, without the need of anyone waiting for me to show up. Do you mind if I live here?*";

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hello... I'm.. A cook... Do you mind... If I cook for you..?*");
            Mes.Add("*You're a... Terrarian..? I could... cook for you, if you... If you don't mind...*");
            Mes.Add("*Hi... I'm a bit lost... I love cooking... So I could cook something for you..*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Add("(She's really deep on her sleep. She looks very tired.)");
                Mes.Add("(You heard a weird air sound, and now is smelling something awful.)");
                Mes.Add("(She seems to be snoring a bit.)");
            }
            else if (Main.eclipse)
            {
                Mes.Add("*Hmm... Maybe I could have use... For a Chainsaw...*");
                Mes.Add("*I can't concentrate on the cooking, with all those weird creatures running outside.*");
                Mes.Add("*I'm sorry... I can't cook for you right now... I'm busy trying to survive this day.*");
            }
            else if (!Main.bloodMoon)
            {
                if (player.HasBuff(Terraria.ID.BuffID.WellFed))
                {
                    Mes.Add("*You're looking healthy...*");
                    Mes.Add("*It's good to see you well.*");
                    Mes.Add("*I'm glad to help keep you nourished.*");
                }
                else
                {
                    Mes.Add("*You look hungry... I can make something for you.*");
                    Mes.Add("*It's not good to keep the belly empty. I can try making something for you to eat.*");
                    Mes.Add("*The look in your face... Just let me know what would you like me to make.*");
                }
                Mes.Add("*I will need ingredients to cook... If you find something good to cook, give me, so I can make food for you anytime you want.*");
                Mes.Add("*I didn't watched my belly when testing my food, so now I'm fat. I needed to see if what I cooked was tasting better.*");
                Mes.Add("*I love cooking... The mix of several different ingredients, can lead to the ultimate tasty food.*");
                Mes.Add("*Are you here to get some food?*");
                Mes.Add("*Do you... Have anything to say... About the foods I make?*");
                Mes.Add("*Smelled something awful? Sorry, I couldn't hold.*");
                if (!Main.raining)
                {
                    if (Main.dayTime)
                    {
                        Mes.Add("*I like this weather.*");
                        Mes.Add("*Enjoying the weather, [nickname]?*");
                    }
                    else
                    {
                        Mes.Add("*I'm feeling a bit drowzy right now...*");
                        Mes.Add("*You're preparing yourself for sleep?*");
                    }
                }
                else
                {
                    Mes.Add("*The smell of wet leaves in the air makes me nostalgic.*");
                    Mes.Add("*Are you watching the rain, [nickname]?*");
                    Mes.Add("*Achoo~! Better, not get too close to me, [nickname]. I don't want to give you flu.*");
                }
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("*The easiest food I make in this place is for [gn:0], he always eats Sweet Potatoes.*");
                    Mes.Add("*I'm peeling some Sweet Potatoes, because [gn:0] may be coming soon to eat something.*");
                    Mes.Add("*[gn:0] seems to be very grateful for the food I make everyday. I wonder if he always had something to eat?*");
                }
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("*I thought It would be a good idea to make a Bunny Stew to [gn:1], but she got very angry when she saw It.*");
                    Mes.Add("*What [gn:1] eats? Well, It ranges from Squirrels in a Stick to Marshmellows. She seems very addicted to Marshmellows, for some reason.*");
                }
                if (NpcMod.HasGuardianNPC(Sardine))
                {
                    Mes.Add("*[gn:2]'s diet must really be composed of fish, like his name...*");
                    Mes.Add("*I tried offering something other than fish to [gn:2], but he refused, and said that wanted to eat cooked fish.*");
                }
                if (NpcMod.HasGuardianNPC(Zacks))
                {
                    Mes.Add("*Whenever I see [gn:3], I give him the first piece of food I have around. I don't want to end up being part of his meal.*");
                    Mes.Add("*The other day, [gn:3] asked if he could have some hamburguer. I was so scared, but gladly could make him go away by giving him some Bunny Stew.*");
                }
                if (NpcMod.HasGuardianNPC(Brutus))
                {
                    Mes.Add("*You wouldn't believe how much [gn:6] eats. I end up exausted after cooking for him.*");
                    Mes.Add("*I asked [gn:6] one day, if he could take a bit ligher on eating. He answered saying that he needs all that food for all the physical effort he does daily.*");
                }
                if (NpcMod.HasGuardianNPC(Bree))
                {
                    Mes.Add("*The most annoying citizen to please with food is [gn:7]. Anything I make is not good enough for her.*");
                }
                if (NpcMod.HasGuardianNPC(Mabel))
                {
                    Mes.Add("*[gn:8] asked me earlier to give her some food that didn't had a lot of things. I gave her a cup of water, but she didn't seems to have liked that.*");
                    Mes.Add("*You would think that [gn:8] eats salad to lose weight. She not only eats a lot of salad, but you can expect at least a piece of meat on her plate.*");
                }
                if (NpcMod.HasGuardianNPC(Vladimir))
                {
                    Mes.Add("*I like cooking to [gn:11]. You will not believe what I will say, but he actually doesn't eats a mountain of food.*");
                    Mes.Add("*I heard some creepy stories from [gn:11] parents. He said that one of his parents can even swallow someone whole. Better you watch out for that If you manage to meet It.*");
                    Mes.Add("*I find [gn:11]'s story with his brother sad.*");
                }
                if (NpcMod.HasGuardianNPC(Wrath))
                {
                    Mes.Add("*I can understand that [gn:14] can't control his anger, but could he at least stop yelling at me when I'm cooking?*");
                }
                if (NpcMod.HasGuardianNPC(Fear))
                {
                    Mes.Add("*Sorry if I'm not really into talking too much right now... [gn:"+Fear+"] has been screaming due to being scared of random things, and I'm getting a minor headache...*");
                }
                if (NpcMod.HasGuardianNPC(Fluffles))
                {
                    Mes.Add("*[nickname], is [gn:16] on my shoulder? No? Good. She's very scary, and I hate when she does that.*");
                    Mes.Add("*[gn:16] asked if I could make some food for her, but she couldn't manage to eat It... The food fell through her body.*");
                }
                if (NpcMod.HasGuardianNPC(Cinnamon))
                {
                    Mes.Add("*Having [gn:" + Cinnamon + "] around is perfect to try improving my cooking. Maybe I could ask her to be my assistant in the future.*");
                    Mes.Add("*While my strength is knowing how to cook things, [gn:" + Cinnamon + "] is good at setting the correct seasonings and their amount on the food.*");
                }
                if (NpcMod.HasGuardianNPC(Miguel))
                {
                    Mes.Add("*[gn:"+Miguel+"] is trying to help me lose my fat, but my belly isn't going away. He told me that is because I eat too much, but how else can I find out if the meal is good?*");
                }
                if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("*I like sharing my room with you, but I can't do anything about the gas related issue...*");
                    Mes.Add("*What kind of breakfast do you like? I ask so I can prepare something when you wake up.*");
                }
            }
            else
            {
                Mes.Add("*...*");
                Mes.Add("*...Grr.*");
                Mes.Add("*...NO!*");
                Mes.Add("(She's staring at you with a angry face, better not bother her.)");
                Mes.Add("(Her eyes are closed, anger is seen on her face, and she's seems to be trying to breath calmly. If I want to talk to her, better I do that cautiously.)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*..You came wanting some food? Does she wants some too...?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It fills me with joy, when I see people happy when eating my food.*");
            Mes.Add("*Have you been eating sufficiently latelly? I'm glad to hear that you are.*");
            Mes.Add("*I don't mind cooking for anyone, It actually fills me with joy.*");
            Mes.Add("*I would like to apologize to you for any noxious gas I release, but I really needed to check if the food was good for the people to eat.*");
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*I don't know why you let [gn:3] live here. Whenever he shows up on my kitchen, he's salivating while looking at me. Is he planning to eat me?*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*Beside [gn:6] eats a lot, I don't mind cooking for him, because the stories he tells when I'm eating are interesting. I still want to know the rest of the story he told me last night.*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("*I tried several times to offer food to [gn:10], but he always says that he conjures his own food and water.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*I think... I'm starting to like [gn:11]. He's very different from many guys I've met.*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*I refuse to cook for [gn:12]. Everytime I try making food for her, I end up tied to a chair, and spending the entire night being forced to drink awfully tasting potions.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Thanks for asking, but I have nothing that I need right now.*");
            Mes.Add("*Hm... No. I don't need anything.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (!guardian.request.IsTravelRequest)
                Mes.Add("*[nickname], I'm a bit busy making some food, so I can't [objective] myself. Could you do that for me?*");
            else
                Mes.Add("*I need to lose some weight, can you [objective]?*");
            Mes.Add("*You're a life saver! I need your help to [objective] for me. I'm currently busy doing some things, so I can't do that myself.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You're a gift, [nickname]. Thank you for helping me.*");
            Mes.Add("*I prepared this specially for you, I hope you like It.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I... Get anxious in my birthday parties... I hope.. I don't wet the floor...*");
            Mes.Add("*I think I'll sitdown somewhere, to hold the anxiety...*");
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("*You have a surprise for me, [nickname]?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("*...*");
                Mes.Add("*...I don't even... Have a place to STAY!*");
                Mes.Add("(She doesn't look okay about staying on the outdoor. Better give her a house.)");
            }
            else
            {
                Mes.Add("*I would be more comfortable, about cooking in my own place. Could you give me a house?*");
                Mes.Add("*I can make you food anytime you want, but I would be more comfortable about having a house for myself...*");
                Mes.Add("*This place is unsafe for me to unleash all my cooking talent. I need a house.*");
                if (Main.raining)
                {
                    Mes.Add("*Yes, I can make some food for you, at least if It doesn't requires cooking, because the rain will ruin the food. Wouldn't be a problem if I had a house...*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Cinnamon:
                        Weight = 1.5f;
                        return "*You're also interessed in cooking..? I think we should try cooking sometime...*";
                    case Brutus:
                        Weight = 1.2f;
                        return "*Are you hungry? I could prepare something for you..*";
                    case Vladimir:
                        Weight = 1.2f;
                        return "*I'll prepare something special for you..*";
                }
            }
            Weight = 1f;
            return "*Hello, I'm [name].*";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Brutus:
                        Weight = 1.5f;
                        return "*I'm happy that you joined us..*";
                    case Vladimir:
                        Weight = 1.5f;
                        return "*I'm so happy that you'll go with us.*";
                    case Cinnamon:
                        Weight = 1.5f;
                        return "*I will enjoy have your company.*";
                }
            }
            Weight = 1f;
            return "*Welcome...*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*You picked me, of everyone? I'm... I'm so happy right now. Thank you.*";
                case MessageIDs.RescueMessage:
                    if (Main.rand.Next(2) == 0)
                        return "*Drink this, It will make you feel better...*";
                    return "*Try not to move, I know how to help you.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "*You woke me up... If you want to eat something, couldn't you wait until I woke up?*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "*Hm... Oh, [nickname]. Why did you wake me up? Did you do what I asked?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I can. Maybe I will lose some weight as we travel, or get more ingredients.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I'm sorry, but I dislike aglomerations...*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*I can't at the moment... I'm sorry to disappoint...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*... I dislike this place... Seems pretty unsafe. Do you really want to leave me here?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Be sure to visit me at "+(!Main.dayTime ? "lunch" : "dinner")+" time. It's not good to skip meals.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*I will try getting at home in one piece. Probably.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Then I'm with you for longer.*";
                case MessageIDs.RequestAccepted:
                    return "*Thank you. While you do what I asked, I will try preparing something special for you.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*You look a bit overloaded. Better you do your other requests, before doing mine.*";
                case MessageIDs.RequestRejected:
                    return "*You can't do It... I'll try doing It later, when I get a break from cooking.*";
                case MessageIDs.RequestPostpone:
                    return "*It can wait, take your time, but not too long.*";
                case MessageIDs.RequestFailed:
                    return "*You couldn't fulfill what I asked for... It's fine...*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*You completed my request..? I can offer you those for it..*";
                case MessageIDs.RequestRemindObjective:
                    return "*It's fine.. Anyone can end up forgetting things. [objective] is what I asked you to do.*";
                case MessageIDs.RestAskForHowLong:
                    return "*I agree, my feet are sore... How long will we rest?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't the best time for a rest, [nickname].*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*If you want, use my belly as a pillow, I don't mind.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*[nickname], [shop] has something that caught my eye. Get close to It.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Just a moment, let's see..*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No..*";
                case MessageIDs.GenericThankYou:
                    return "*I thank you deeply.*";
                case MessageIDs.GenericNevermind:
                    return "*Oh... Nevermind...*";
                case MessageIDs.ChatAboutSomething:
                    return "*I can cook while talking, sure. What do you want to talk about?*";
                case MessageIDs.NevermindTheChatting:
                    return "*Feel free to call me for a chatting anytime you want.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*What? Are you serious?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*I... Okay... If that's what you want...*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Uff... Well.. Do you want anything else?*";
                case MessageIDs.LeopoldMessage1:
                    return "*Uh.. We're not... Hostages...*";
                case MessageIDs.LeopoldMessage2:
                    return "*What? Then why...*";
                case MessageIDs.LeopoldMessage3:
                    return "*The Terrarian... Is leading us.. On their adventure... And can understand what you're saying...*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Let's see what are you cooking...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... So many different food smell...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*Huh... What was that noise...?*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Eww... This smell... I'm glad I already identified you but... Ugh..*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... Eh... Good morning?*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*I... Um... Thank you...*";
                    return "*I'm thankful for your help.*";
                case MessageIDs.RevivedByRecovery:
                    return "*...*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Ugh... Pain...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Ahh!! There's fire everywhere!! Around me!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I can't see clearly!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*I want to throw up...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*Why I can't attack?*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I'm... Out of breath...*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*My legs hurts...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Argh... Nasty cut...*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*No no no! That thing wont eat me! No way!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*You vile creature...*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Not even my f-fat helps much ag-gainst c-cold.*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I don't think this web will last for long.*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*I'll make hamburguer out of you!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*My defenses seems more unpenetrable now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "(Farts)*I'm sorry... Just plug your nose a bit.*";
                    return "*It still lacks a few spices...*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*This will help me when chopping.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*I think this can make me lose some weight, if I make use of It.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Does this potion makes me bigger?*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*I got a bit tougher, somehow.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Okay, blades coated.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*I preffer drinking while eating..*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*Sweet.. I could use some for my recipes..*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Ah, a Life Crystal. Lucky.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Look, Pressure Plate over there.*";
                case MessageIDs.FoundMineTile:
                    return "*Watch out, a Mine.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*...Don't.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*...I have a bad feeling about that...*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*Do we have to face them...?*";
                case MessageIDs.FoundTreasureTile:
                    return "*Look, a chest. Let's look inside It.*";
                case MessageIDs.FoundGemTile:
                    return "*I see some gems over there..*";
                case MessageIDs.FoundRareOreTile:
                    return "*Uncommon ores that way.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*I see some rare ores.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*I hope we both fit in the minecart.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Would you like something to eat, when we arrive?*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Could they also help me in cooking?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I guess I can wait a while longer...*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname]! I'll give you something that may help you...*";
                case MessageIDs.LeaderDiesMessage:
                    return "*Oh no! [nickname]...*";
                case MessageIDs.AllyFallsMessage:
                    return "*There's someone badly hurt here!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Look, [nickname].*";
                case MessageIDs.LeavingToSellLoot:
                    return "*My bag is quite heavy right now. I'll go sell those things.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*[nickname], do you need to take a rest?*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*I can't fall like this...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*My potions...*";
                case MessageIDs.UsesLastPotion:
                    return "*I'm... I ran out of potions... Sorry...*";
                case MessageIDs.SpottedABoss:
                    return "*I... Shall face It too... As long as you're with me, [nickname].*";
                case MessageIDs.DefeatedABoss:
                    return "*We won! Should we celebrate our victory with a feast?*";
                case MessageIDs.InvasionBegins:
                    return "*[nickname]! Look! At the horizon!*";
                case MessageIDs.RepelledInvasion:
                    return "*There's no more of them... Glad to have some peace again..*";
                case MessageIDs.EventBegins:
                    return "*There is something wrong...*";
                case MessageIDs.EventEnds:
                    return "*..It's over.. Phew... I'm glad to have some moment to rest.*";
                case MessageIDs.RescueComingMessage:
                    return "*That's bad.. That's too bad! Hang on!*";
                case MessageIDs.RescueGotMessage:
                    return "*You're safe... Don't worry...*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*You terrarians have quite a good taste for food... There is a terrarian, named [player], who also loves my food.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*[player] did managed to defeat [subject]. I'm glad they're okay.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*I heard that [player] found a [subject] during their travels. I wonder what does It look like.*";
                case MessageIDs.FeatEventFinished:
                    return "*I was there when [player] managed to overcome the [subject]... It was amazing.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*It seems like [player] met [subject] recently. I wonder if they will like my culinary.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I'd like to spend some time alone... A person I met has died and I'm trying to digest that info... [player]... How could you...*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] opened the door of some kind of temple in [subject], so the people there are launching expeditions to it. I wonder if there's a lost recipe in there.*";
                case MessageIDs.FeatCoinPortal:
                    return "*I'm just a little jealous of [player]. They found a coin portal, and got quite a lot of coins from it.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I met [player]... Recently.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] seems to be really enjoying fishing.. It's good to spend time with something you like.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*We celebrated with a feast on [subject], after [player] killed some godly creature.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*I'm a bit scared of returning to [subject]... Things seems dangerous there... But [player]'s presence comforts me into staying...*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*I'm happy for [player] and [subject] having formed a buddiship with each other. I wish them well.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Hi... I'm good happy speaking with you... I want to thank you for picking me as your buddy. I hope to retribute this honor to you.*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*[subject] got a place to live in [world]. I hope they be happy there.*";
                case MessageIDs.DeliveryGiveItem:
                    return "*Here... Take this [item].. You need it, [target]...*";
                case MessageIDs.DeliveryItemMissing:
                    return "*Hm... I was going to give someone an item, but it's gone..*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*... Your inventory... [target]... It's full..*";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "*I will... And I will ensure they never go hungry too..*";
                case MessageIDs.CommandingLeadGroupFail:
                    return "*Sorry... No..*";
                case MessageIDs.CommandingDisbandGroup:
                    return "*Okay... We'll go back home then.*";
                case MessageIDs.CommandingChangeOrder:
                    return "*I will do that...*";
            }
            return "";
        }

        public static void AllowGettingMoreFoodFromMinerva()
        {
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ReceivedFoodFromMinerva = false;
        }

        //Dialogue Interactions
        public void CookDialogue()
        {
            bool PlayerReceivedFood = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ReceivedFoodFromMinerva;
            if (PlayerReceivedFood)
            {
                Dialogue.ShowEndDialogueMessage("*I already gave you some food... Wait until " + (Main.dayTime ? "dinner" : "lunch") + " time for more.*", false);
                return;
            }
            string[] PossibleFoods = new string[]{
                "Soup",
                "Cooked Fish",
                "Cooked Shrimp",
                "Pumpkin Pie",
                "Sashimi",
                "Grub Soup",
                "Nevermind"
                };
            Player player = Main.player[Main.myPlayer];
            int CountPlayerFood = player.inventory.Where(x => x.buffType == Terraria.ID.BuffID.WellFed).Sum(x => x.stack);
            if (player.position.Y >= Main.worldSurface * 16)
                PossibleFoods[0] = "";
            if (player.Center.X / 16 >= 250 && player.Center.X / 16 <= Main.maxTilesX / 250)
                PossibleFoods[2] = "";
            if (!Main.halloween)
                PossibleFoods[3] = "";
            if (!NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant))
                PossibleFoods[4] = "";
            if (!player.ZoneJungle)
                PossibleFoods[5] = "";
            bool GotFood = false;
            int ItemToGet = 0;
            switch(Dialogue.ShowDialogueWithOptions("*That is what I can cook for you right now:*", PossibleFoods))
            {
                case 0:
                    {
                        ItemToGet = Terraria.ID.ItemID.BowlofSoup;
                        Dialogue.ShowDialogueWithContinue("*I hope you enjoy It... The mushroom and the goldfish are fresh.*");
                        GotFood = true;
                    }
                    break;
                case 1:
                    {
                        ItemToGet = Terraria.ID.ItemID.CookedFish;
                        Dialogue.ShowDialogueWithContinue("*The fish were caught last morning... I hope they're at your taste.*");
                        GotFood = true;
                    }
                    break;
                case 2:
                    {
                        ItemToGet = Terraria.ID.ItemID.CookedShrimp;
                        Dialogue.ShowDialogueWithContinue("*The shrimps were caught some time ago and are fresh... Enjoy your meal.*");
                        GotFood = true;
                    }
                    break;
                case 3:
                    {
                        ItemToGet = Terraria.ID.ItemID.PumpkinPie;
                        Dialogue.ShowDialogueWithContinue("*The pie is still fresh... I hope you like It.*");
                        GotFood = true;
                    }
                    break;
                case 4:
                    {
                        ItemToGet = Terraria.ID.ItemID.Sashimi;
                        Dialogue.ShowDialogueWithContinue("*I'm not really experienced with that... So I kind of bought that from the Travelling Merchant, instead.*");
                        GotFood = true;
                    }
                    break;
                case 5:
                    {
                        ItemToGet = Terraria.ID.ItemID.GrubSoup;
                        Dialogue.ShowDialogueWithContinue("*Probably is not as horrible as you may think... Tell me what do you think when you eat. Me? Of course I wont put any of that on my mouth!*");
                        GotFood = true;
                    }
                    break;
                case 6:
                    Dialogue.ShowEndDialogueMessage("*...Then why did you ask for food...*", false);
                    break;
            }
            if (GotFood)
            {
                Vector2 LeaderPos = player.Center;
                player.GetModPlayer<PlayerMod>().ReceivedFoodFromMinerva = true;
                if (CountPlayerFood > 10)
                {
                    Dialogue.ShowDialogueWithContinue("*You still got a lot of food with you. Eat them before asking me for more.*");
                }
                else
                {
                    player.GetItem(Main.myPlayer, Main.item[Item.NewItem(player.getRect(), ItemToGet, 3, true)]);
                }
                //Deliver also to other teams close by.
                for (int i = 0; i < 255; i++)
                {
                    Player player2 = Main.player[i];
                    if (!player2.active)
                        continue;
                    if (player2.whoAmI == player.whoAmI || (Math.Abs(player2.Center.X - LeaderPos.X) < 1000 && Math.Abs(player2.Center.Y - LeaderPos.Y) < 800))
                    {
                        foreach (TerraGuardian tg in player2.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                        {
                            if (tg.Active)
                            {
                                if (tg.Inventory.Where(x => x.buffType == Terraria.ID.BuffID.WellFed).Sum(x => x.stack) <= 10)
                                {
                                    if (tg.ID == Minerva && tg.ModID == MainMod.mod.Name && ItemToGet == Terraria.ID.ItemID.GrubSoup)
                                    {
                                        tg.GetItem(Terraria.ID.ItemID.BowlofSoup, 3);
                                    }
                                    else
                                    {
                                        tg.GetItem(ItemToGet, 3);
                                    }
                                }
                            }
                        }
                    }
                }
                Dialogue.ShowEndDialogueMessage("*I... Also gave some food to your companions... If you don't mind...*", false);
            }
        }
    }
}
