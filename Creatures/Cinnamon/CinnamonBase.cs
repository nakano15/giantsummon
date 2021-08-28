using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class CinnamonBase : GuardianBase
    {
        public CinnamonBase() //Her recruitment could involve Soup.
        {
            Name = "Cinnamon";
            PossibleNames = new string[] { "Cinnamon", "Canela" };
            Description = "A food enthusiast who is travelling worlds,\nseeking the best seasonings for food.";
            Size = GuardianSize.Medium;
            Width = 24;
            Height = 68;
            DuckingHeight = 60;
            CompanionSlotWeight = 0.9f;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 59f / 68;
            Age = 13;
            SetBirthday(SEASON_SPRING, 28);
            Male = true;
            InitialMHP = 160; //860
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 20;
            Accuracy = 0.43f;
            Mass = 0.36f;
            MaxSpeed = 4.7f;
            Acceleration = 0.33f;
            SlowDown = 0.22f;
            MaxJumpHeight = 18;
            JumpSpeed = 7.19f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();
            CallUnlockLevel = 4;
            MoveInLevel = 3;
            MountUnlockLevel = 6;

            AddInitialItem(Terraria.ID.ItemID.RedRyder, 1);
            AddInitialItem(Terraria.ID.ItemID.LesserHealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.CookedFish, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 10;
            PlayerMountedArmAnimation = 23;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 11, 12, 13, 14 };
            DuckingFrame = 18;
            DuckingSwingFrames = new int[] { 20, 21, 22 };
            SittingFrame = 16;
            ChairSittingFrame = 15;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 24;
            BedSleepingFrame = 25;
            SleepingOffset.X = 16;
            ReviveFrame = 19;
            DownedFrame = 17;
            //PetrifiedFrame = 28;

            BackwardStanding = 26;
            BackwardRevive = 27;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);

            RightArmFrontFrameSwap.Add(15, 0);
            RightArmFrontFrameSwap.Add(18, 1);
            RightArmFrontFrameSwap.Add(19, 1);

            //Left Hand
            LeftHandPoints.AddFramePoint2x(11, 16, 4);
            LeftHandPoints.AddFramePoint2x(12, 30, 22);
            LeftHandPoints.AddFramePoint2x(13, 32, 28);
            LeftHandPoints.AddFramePoint2x(14, 29, 34);

            LeftHandPoints.AddFramePoint2x(19, 26, 38);

            LeftHandPoints.AddFramePoint2x(20, 15, 17);
            LeftHandPoints.AddFramePoint2x(21, 29, 25);
            LeftHandPoints.AddFramePoint2x(22, 29, 36);

            //Right Hand
            RightHandPoints.AddFramePoint2x(11, 29, 14);
            RightHandPoints.AddFramePoint2x(12, 32, 22);
            RightHandPoints.AddFramePoint2x(13, 35, 28);
            RightHandPoints.AddFramePoint2x(14, 31, 35);

            RightHandPoints.AddFramePoint2x(20, 26, 17);
            RightHandPoints.AddFramePoint2x(21, 31, 25);
            RightHandPoints.AddFramePoint2x(22, 31, 36);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(18 * 2, 23 * 2);
            MountShoulderPoints.AddFramePoint2x(18, 18, 27);
            MountShoulderPoints.AddFramePoint2x(19, 18, 27);

            //Sitting Position
            SittingPoint = new Point(21 * 2, 39 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(23, 18 + 2);
            HeadVanityPosition.AddFramePoint2x(18, 23, 22 + 2);
            HeadVanityPosition.AddFramePoint2x(19, 23, 22 + 2);

            //Wing Position
            //WingPosition.DefaultCoordinate2x = new Point(20, 23);

            GetRequestList();
        }

        public override string CallUnlockMessage => "*I think you're a cool guy. You can call me anytime for your adventures.*";

        public override string MountUnlockMessage => "*If you want, I can carry you on my shoulder.*";

        public override string MoveInUnlockMessage => "*This place seems actually nice. May I live here with you?*";

        public override string ControlUnlockMessage => "*As long as you help me get more tasty food, I can let you control me.*";

        public void GetRequestList()
        {
            //0
            AddNewRequest("A Good Soup", 225, 
                "*I need something warm and delicious to eat. I can think of a Soup as that thing. Would you try getting some for me?*",
                "*I'll be waiting for the soup, then.*",
                "*Awww... But I'm hungry...*",
                "*Thank you! Be sure that I'll enjoy eating this.*", 
                "*I don't know much about soups, but I think they involve Mushrooms or Goldfishs?*", 
                "");
            AddItemCollectionObjective(Terraria.ID.ItemID.BowlofSoup, 1, 0.2f);
            //1
            AddNewRequest("You Can Fish That", 220, 
                "*I want to taste some food that involves fish. I need to check also how you cook it to make it edible and delicious. Can you do that?*",
                "*Yay! I'll be waiting.*",
                "*You don't know how to cook fish...?*",
                "*I can't stop drooling, that looks DELICIOUS! Thank you!*",
                "*You will need to fish up some fish from water before you can cook them. You will need a fishing rod and bait to do that, obviously.*");
            AddItemCollectionObjective(Terraria.ID.ItemID.CookedFish, 1, 0.2f);
            //2
            AddNewRequest("Shrips For Good!", 235,
                "*I want to taste shrips! People says that shrips is tasty, and I want to know if It's true. Do you know how to cook them? Could you make some cooked shrips for me?*",
                "*Okay, I'll be waiting here. I think you can find them on the ocean, but I may be wrong.*",
                "*But I want to taste shrips....*",
                "*That's the shrips?! (Snif, snif) Hm... It looks tasty... What? The name is shrimp? How can I say right that name?*",
                "*I think Shrips are found in the ocean. You will need to try fishing them there, if you want to find them, if they're there.*");
            AddItemCollectionObjective(Terraria.ID.ItemID.CookedShrimp, 1, 0.2f);
            //3
            AddNewRequest("Who Takes the First Bite?", 260,
                "*I also need to taste exotic kinds of food, so I wonder if you could make me a Grub Soup. Would you make one for me?*",
                "*Thanks. I have to say that I'm not excited to try that food, but let's see how it looks...*",
                "*I think you may be right, neither I am interessed in trying that...*",
                "*That's... The grub soup...? I ... Really don't want to take a bite of that... Why don't you try It and tell me the taste? Uh... [nickname], are you feeling alright? Your face seems pale.*",
                "*The insects for a Grub Soup can be found in the Jungle. You will need a Bug Net to catch them.*");
            AddItemCollectionObjective(Terraria.ID.ItemID.GrubSoup, 1, 0.2f);
            //4
            AddNewRequest("Probably Non Existing Place Culinary", 275,
                "*I heard from a Travelling Merchant, that there is a food called \'Sashimi\' in one of the places he has been into. He said that It's good food, so I want to try It too. Do you know how to make It?*",
                "*Wow [nickname], you really know many foods. I'll be waiting for you to bring me a \'Sashimi\'.*",
                "*You never heard of It, either? Maybe I should ask the Travelling Merchant to see if could find the recipe if he visit that place.*",
                "*The way It's cut... The smell... Hmm... You need to give me the recipe of this sometime.*",
                "*The only thing I know from \'Sashimi\', is that It's made using fish, but you know that already, don't you?*");
            AddItemCollectionObjective(Terraria.ID.ItemID.Sashimi, 1, 0.2f);
        }

        public override bool AlterRequestGiven(GuardianData Guardian, out int ForcedMissionID, out bool IsTalkRequest)
        {
            if(Guardian.FriendshipLevel < MoveInLevel)
            {
                ForcedMissionID = Main.rand.Next(5);
                IsTalkRequest = (Main.rand.NextDouble() < 0.3f);
                return true;
            }
            return base.AlterRequestGiven(Guardian, out ForcedMissionID, out IsTalkRequest);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (player.HasItem(Terraria.ID.ItemID.BowlofSoup))
            {
                Mes.Add("*(Snif, Snif) Humm.... (Snif, Snif) You have something that smells delicious. Could you share It with me?*");
            }
            else
            {
                Mes.Add("*Oh, hello. Do you like tasty food too?*");
                Mes.Add("*Hi, I love tasty foods. What do you love?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm loving my time here.*");
            Mes.Add("*(singing) Lalala...*");
            Mes.Add("*Hello [nickname], want something?*");
            Mes.Add("*Are you cooking something? I want to see your cooking secrets, teehee.*");
            Mes.Add("*Happiness is contagious, am I right?*");

            if (FlufflesBase.IsHauntedByFluffles(player))
            {
                Mes.Add("*Aahh!! There's a ghost on your back!!*");
            }

            if (!guardian.HasBuff(Terraria.ID.BuffID.WellFed))
            {
                Mes.Add("(Growl) *Oh, my stomach is complaining... Is "+(Main.dayTime ? "lunch" : "dinner")+" ready?*");
                Mes.Add("*I hate being hungry... I want to eat something...*");
                Mes.Add("*Ow... I think I should be cooking something to eat.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*I'm quite busy doing the number 2 here... Go away, please.*");
                Mes.Add("*I really didn't wanted you to see me like this.*");
                Mes.Add("*I can't concentrate with you staring at me.*");
            }
            else if (Main.eclipse)
            {
                Mes.Add("*I'm sorry, but I really feel like locking myself inside my house now.*");
                Mes.Add("*Do you think I'll be safe if I lock myself in the toilet?*");
                Mes.Add("*Please, get those horrible creatures away!*");
            }
            else if (Main.dayTime)
            {
                if (!Main.raining)
                {
                    Mes.Add("*What a beautiful day.*");
                    Mes.Add("*I like seeing butterflies flying and critters around.*");
                }
                else
                {
                    Mes.Add("*Oh nice... It's raining... Well.... I'll take the day off.*");
                    Mes.Add("*Acho~! Sorry... I'm alergic to this weather.*");
                    Mes.Add("*I'm getting a bit drowzy...*");
                }
            }
            else
            {
                Mes.Add("*What a silent night.*");
                Mes.Add("*I think I saw something moving in the dark.*");
                Mes.Add("*It's too quiet, and that doesn't makes me feel okay.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*I love having you as my room mate. Your help when making the morning set will be very helpful.*");
                Mes.Add("*It's nice sharing the room with you.*");
            }
            //
            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*[gn:"+Rococo+"] helps me with testing food. He seems to enjoy that.*");
                Mes.Add("*Sometimes [gn:"+Rococo+"] brings trash, wanting me to add them to the food. I keep denying but he keeps bringing them.*");
            }
            if (NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*[gn:" + Blue + "] has a really cool hair.*");
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*I were learning how to cook from [gn:" + Bree + "].*");
                Mes.Add("*[gn:" + Bree + "] isn't that much grumpy when you talk about something she likes.*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("*Sometimes I play with [gn:"+Glenn+"]. I like that there's someone of my age around.*");
                Mes.Add("*There are some times where I don't like playing a game with [gn:"+Glenn+"], he can't accept when I win.*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*[gn:"+Brutus+"] told me not to wander on the outside alone... He said that I should call him when I'm going to do so. But I'm already a grown girl, I can take care of myself!*");
                Mes.Add("*I offered [gn:" + Brutus + "] some food earlier, It looked like he liked It.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*A lot of people seems to be scared of [gn:" + Fluffles + "], but I'm not.*");
            }
            if (NpcMod.HasGuardianNPC(Liebre))
            {
                Mes.Add("*I saw [gn:" + Liebre + "] watching me the other day. Am I going to die?*");
                Mes.Add("*It's so scary! Sometimes I'm playing around my house, I see [gn:" + Liebre + "] watching me. Is my time coming? I don't want to die!*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*Sometimes, I ask [gn:" + Vladimir + "] to help me test food. He has an accurate taste for seasoning, he impresses me.*");
                Mes.Add("*I'm curious to meet [gn:" + Vladimir + "]'s brother. Is he as nice as him?*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*Why people keep staring at [gn:" + Mabel + "]? It's so weird.*");
                Mes.Add("*I asked [gn:" + Mabel + "] if she wanted to test the newest food I cooked, but she said she can't, or else she would gain weight.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*I love hanging around with [gn:" + Minerva + "], we keep testing each other's meal, to see if we make the best ones.*");
                Mes.Add("*I actually don't exagerate when testing my meals, so you don't need to worry about me ending up like [gn:" + Minerva + "].*");
            }
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*The other day, [gn:" + Zacks + "] was at my house and pulled my blanked! I woke up and screamed so loud that he run away.*");
                Mes.Add("*Sometimes, during the night, I see [gn:" + Zacks + "] staring through the window.*");
                Mes.Add("*I fear leaving my house at night, because [gn:" + Zacks + "] may be out there.*");
            }
            //
            if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("*Nom nom nom nom nom nom...* (She seems to be dreaming about eating lots of food)");
                Mes.Add("*Zzzzz.... (Snif snif) Hmmm....* (She smelled something good.)");
                Mes.Add("*Zzzz... Zzzzzz..... More food.... Seasoning.... Eat.... Zzzzz...*");
            }
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*What now?!*");
                Mes.Add("*Don't you have someone else to bother?*");
                Mes.Add("*Enough!*");
                Mes.Add("*No! I wont cook for you!*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I was a little kid when I got passion for tasty food.*");
            Mes.Add("*My name is the same of a seasoning, since I know. It's funny that my mom picked specifically that name to me.*");
            Mes.Add("*Once we had food shortage at home, because I cooked many different food with what we had at home... We had to eat apples for week.*");
            Mes.Add("*I wonder if my mom is worried about me...*");
            Mes.Add("*I heard from the Travelling Merchant that he travels through worlds, so I'm following him to meet new places, and find new kinds of foods.*");
            Mes.Add("*Every food tastes better with the right seasoning, but sometimes placing them is not necessary.*");
            bool HasSomeoneToTutorInCooking = false;
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*I have been learning how to cook food at the right point from [gn:" + Minerva + "]. Even my food tastes better now.*");
                HasSomeoneToTutorInCooking = true;
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*[gn:" + Bree + "] have been teaching me how to not exagerate when placing seasoning, so my food no longer feels over or under seasoned.*");
                HasSomeoneToTutorInCooking = true;
            }
            if (!HasSomeoneToTutorInCooking)
            {
                Mes.Add("*I'm looking for someone to more about cooking from. I wonder if this world has someone like that.*");
            }
            bool HasSomeoneToTasteFood = false;
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*[gn:" + Brutus + "] sometimes eat of the food I made. He always compliments It.*");
                HasSomeoneToTasteFood = true;
            }
            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*[gn:" + Rococo + "] is always happy when he tastes my food, but I always have to reject his seasoning suggestions.*");
                HasSomeoneToTasteFood = true;
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*I really like testing food with [gn:" + Vladimir + "], we chat about so many things when we do so.*");
                HasSomeoneToTasteFood = true;
            }
            if (!HasSomeoneToTasteFood)
            {
                Mes.Add("*I really don't have a second opinion about the food I cook... I can't know if what I cook is good if only I test It.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm not in need of anything now, but come back later.*");
            Mes.Add("*No, I don't have anything I need. Beside we can chat, if you're interessed.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I have something that I reeeeeeeeeeally need done... Could you help me with it?*");
            Mes.Add("*I'm so glad you asked, you're a life saver. I need help with this, can you give me a hand?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Yay! Thank you! You're the best person ever!*");
            Mes.Add("*I'm so glad to have met you. Thanks.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Since I can live in your world, I will need a house for me to stay. Do you have one for me?*");
            Mes.Add("*I need a place to stay and place my things.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You all prepared this party for me? I'm so happy!*");
            Mes.Add("*I really love being around here, you guys are really nice for doing this party for me.*");
            if(!PlayerMod.HasGuardianBeenGifted(player, guardian.ID))
            {
                Mes.Add("*Saaaaaaaay... Do you have some kind of package for me, [nickname]?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*You choose me as your buddy? I'm so happy!*";
                case MessageIDs.RescueMessage:
                    return "*I'm tending to your wounds, don't worry.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Oooohh... Okay... I'm awaken... Do you need something...?*";
                    return "*Yawnn... I'm so sleepy... What do you need, [nickname]?*";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Yes.. I'm waking up... Is It about my request?*";
                    return "*Zzzz... Oh! Ah... Uh... Oh...! Did you complete my request?*";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    if (NpcMod.HasGuardianNPC(Brutus) && Main.rand.NextDouble() < 0.33)
                        return "*I think [gn:"+Brutus+"] wont feel angry at me if I accompany you on your quest.*";
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Travel the world with you? Yes, let's go!*";
                    return "*Yes! That sounds so fun... Can I gather some ingredients on the way?*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*There's way too many people following you, I'd feel cramped in there.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I... Don't really trust you... Right now...*";
                    return "*Sorry, but... No...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*But this place is dangerous! How can I make It home from here? Do you really want to make me leave the group here?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Alright, I'll be heading home, then.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    if (NpcMod.HasGuardianNPC(Brutus))
                        return "*... I really wish [gn:"+Brutus+"] was here now...*";
                    return "*...Fine... I'll see if I can reach home alive..*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*More travelling! Yay!*";
                case MessageIDs.RequestAccepted:
                    return "*Thank you! Tell me when you get It done.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*Don't you have too many things to do?*";
                case MessageIDs.RequestRejected:
                    return "*Aww....*";
                case MessageIDs.RequestPostpone:
                    return "*Later? Fine, it can wait then.*";
                case MessageIDs.RequestFailed:
                    return "*It seems like you couldn't do it, then... (You see tears rolling down from her eyes)*";
                case MessageIDs.RestAskForHowLong:
                    return "*My feet are hurting, so I could really use some rest. How long we'll rest?*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't a good moment to do that.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*I think I can use my tail as a kind of blanket.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*That looks interesting! Let's check [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*I want one of this, and one of that, and...*";
                case MessageIDs.GenericYes:
                    return "*Yeah!*";
                case MessageIDs.GenericNo:
                    return "*Nay~....*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you!*";
                case MessageIDs.ChatAboutSomething:
                    return "*Do you want to chat? It's good so I can take a little break. What do you want to know?*";
                case MessageIDs.NevermindTheChatting:
                    return "*Alright, anything else?*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You can't do my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Aww... Well... At least you tried.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Okay, so what was that for?*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*What can you tell me about you, rodent?*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... You've been in contact with many foods.*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*I'm starting to get hungry now...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*I guess I should visit your house sometimes when you're cooking...*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ouch! You didn't needed to slap my face!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "*I was so scary of dying! Thanks for helping.*";
                case MessageIDs.RevivedByRecovery:
                    return "*Uh, what happened? Why was I in the floor?*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*My body burns inside!!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Aaahh!!! I don't want to be cooked alive!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I can't keep my eyes wide open!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Why is everybody floating around?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I can't use my weapons!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My feet aren't moving as fast as I wanted!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I'm not slacking! I'm feeling weak!*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Ahh!! My belly!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Aahhh!! That monster is really big!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hey! If you want to alleviate yourself, do that somewhere else!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*N-not even my t-tail is helping a-against the c-chill..*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Let me go! Let me go!! Aaaahh!!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*I'll cut you into pieces!!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Hey! I got tougher!*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Lovelly food!*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*I'm stronger!*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Let's do a race!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*I got healthier!*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*This will be super effective!*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*This will ruin the meat...*";
                case MessageIDs.AcquiredTipsyDebuff: //She doesn't drinks beverage
                    return "";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*Yumm. I'm even licking my paws now.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*I see a crystal shining over there.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Watch it! Be careful not to step on the pressure plate.*";
                case MessageIDs.FoundMineTile:
                    return "*Some bad intentioned person left that mine there.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Can I press It? Please?*";
                case MessageIDs.FoundPlanteraTile:
                    return "*Is that plant even edible?*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I don't know if we should, but I'll stay with you during the invasion.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Come on! Open it!*";
                case MessageIDs.FoundGemTile:
                    return "*Oooohhh! Gems!*";
                case MessageIDs.FoundRareOreTile:
                    return "*There's some ores over there.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Woah! Look at those ores!*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Let's ride a Minecart?*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*We could get somethign to eat at home.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Welcome!*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*A new person! Do you like tasty foods?*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Can they hold cooking utensils?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I want to be hugged next.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname] is injured!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*Oh my! That's horrible! [nickname] is fine?*";
                case MessageIDs.AllyFallsMessage:
                    return "*A friend of ours fell!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Look, [nickname]! Loot!*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'm loaded with things, I'll go sell them.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You need some healing, [nickname].*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*I'm not feeling great...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have few potions left.*";
                case MessageIDs.UsesLastPotion:
                    return "*It's all gone! My potions are gone!*";
                case MessageIDs.SpottedABoss:
                    return "*Waah! I'm scared!!*";
                case MessageIDs.DefeatedABoss:
                    return "*Uff... Uff... My heart is still racing...*";
                case MessageIDs.InvasionBegins:
                    return "*Who are they? They don't look friendly.*";
                case MessageIDs.RepelledInvasion:
                    return "*I'm so glad It's over. Why they attacked us?*";
                case MessageIDs.EventBegins:
                    return "*Why the air feels so cold?*";
                case MessageIDs.EventEnds:
                    return "*It's over... It's over... Phew...*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
