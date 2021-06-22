using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures
{
    public class BrutusBase : GuardianBase
    {
        public const string RoyalGuardSkinID = "royal_guard", RoyalGuardSkinFID = "royal_guard_f";

        /// <summary>
        /// -Was a Royal Guard in the Ether Realm.
        /// -Has some sense of ethic over his actions.
        /// -Wants to keep everyone he cares about safe.
        /// -Pervert.
        /// -Hardly gets drunk.
        /// -Really loves birthday parties.
        /// </summary>

        public BrutusBase()
        {
            Name = "Brutus";
            Description = "He was once a member of the Royal Guard\non the Ether Realm. Now is just a body guard.";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 92;
            DuckingHeight = 52;
            SpriteWidth = 112;
            SpriteHeight = 112;
            Scale = 110f / 92;
            FramesInRows = 17;
            Age = 28;
            SetBirthday(SEASON_WINTER, 4);
            Male = true;
            InitialMHP = 225; //1400
            LifeCrystalHPBonus = 45;
            LifeFruitHPBonus = 25;
            Accuracy = 0.63f;
            Mass = 0.55f;
            MaxSpeed = 5.05f;
            Acceleration = 0.22f;
            SlowDown = 0.33f;
            MaxJumpHeight = 13;
            JumpSpeed = 8.3f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            SetTerraGuardian();
            DontUseRightHand = false;
            OneHanded2HWeaponWield = true;
            CallUnlockLevel = 0;
            StopMindingAFK = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 2;

            AddInitialItem(Terraria.ID.ItemID.CobaltSword, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.CobaltRepeater, 1);
            AddInitialItem(Terraria.ID.ItemID.CursedArrow, 250);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            PlayerMountedArmAnimation = 14;
            HeavySwingFrames = new int[] { 16, 17, 18 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 19;
            DuckingSwingFrames = new int[] { 20, 21, 22 };
            SittingFrame = 15;
            ChairSittingFrame = 23;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 14, 16, 20, 21 });
            ThroneSittingFrame = 24;
            BedSleepingFrame = 25;
            SleepingOffset.X = 16;
            ReviveFrame = 26;
            DownedFrame = 27;
            PetrifiedFrame = 29;

            BackwardStanding = 30;
            BackwardRevive = 31;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(0, 20, 35, 1.570796326794897f, true);
            LeftHandPoints.AddFramePoint2x(1, 27, 30, 0, true);
            LeftHandPoints.AddFramePoint2x(2, 29, 29, 0, true);
            LeftHandPoints.AddFramePoint2x(3, 27, 30, 0, true);
            LeftHandPoints.AddFramePoint2x(4, 25, 31, 0, true);
            LeftHandPoints.AddFramePoint2x(5, 23, 30, 0, true);
            LeftHandPoints.AddFramePoint2x(6, 21, 29, 0, true);
            LeftHandPoints.AddFramePoint2x(7, 23, 30, 0, true);
            LeftHandPoints.AddFramePoint2x(8, 25, 31, 0, true);
            LeftHandPoints.AddFramePoint2x(9, 25, 28, 0, true);

            LeftHandPoints.AddFramePoint2x(10, 16, 4);
            LeftHandPoints.AddFramePoint2x(11, 37, 12);
            LeftHandPoints.AddFramePoint2x(12, 43, 24);
            LeftHandPoints.AddFramePoint2x(13, 37, 36);

            LeftHandPoints.AddFramePoint2x(16, 8, 21);
            LeftHandPoints.AddFramePoint2x(17, 46, 20);
            LeftHandPoints.AddFramePoint2x(18, 50, 45);

            LeftHandPoints.AddFramePoint2x(20, 15, 11);
            LeftHandPoints.AddFramePoint2x(21, 39, 20);
            LeftHandPoints.AddFramePoint2x(22, 39, 37);

            LeftHandPoints.AddFramePoint2x(26, 43, 48);

            //Right Arm
            RightHandPoints.AddFramePoint2x(0, 34, 35, 1.570796326794897f, true);
            RightHandPoints.AddFramePoint2x(1, 39, 29, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(2, 39, 28, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(3, 39, 28, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(4, 39, 29, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(5, 39, 29, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(6, 39, 29, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(7, 39, 29, 0.7853981633974483f, true);
            RightHandPoints.AddFramePoint2x(8, 39, 29, 0.7853981633974483f, true);

            RightHandPoints.AddFramePoint2x(10, 19, 4);
            RightHandPoints.AddFramePoint2x(11, 42, 12);
            RightHandPoints.AddFramePoint2x(12, 46, 24);
            RightHandPoints.AddFramePoint2x(13, 40, 36);

            RightHandPoints.AddFramePoint2x(16, 36, 18);
            RightHandPoints.AddFramePoint2x(17, 45, 32);
            RightHandPoints.AddFramePoint2x(18, 51, 47);

            RightHandPoints.AddFramePoint2x(20, 18, 11);
            RightHandPoints.AddFramePoint2x(21, 42, 20);
            RightHandPoints.AddFramePoint2x(22, 42, 37);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate2x = new Point(18, 20);
            MountShoulderPoints.AddFramePoint2x(16, 15, 29);
            MountShoulderPoints.AddFramePoint2x(17, 28, 21);
            MountShoulderPoints.AddFramePoint2x(18, 36, 32);

            MountShoulderPoints.AddFramePoint2x(19, 19, 28);
            MountShoulderPoints.AddFramePoint2x(20, 19, 28);
            MountShoulderPoints.AddFramePoint2x(21, 19, 28);
            MountShoulderPoints.AddFramePoint2x(22, 19, 28);
            SittingPoint = new Point(27 * 2, 40 * 2);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(25, 28);
            WingPosition.AddFramePoint2x(16, 22, 27);
            WingPosition.AddFramePoint2x(17, 28, 29);
            WingPosition.AddFramePoint2x(18, 36, 37);

            WingPosition.AddFramePoint2x(19, 25, 44);
            WingPosition.AddFramePoint2x(20, 25, 44);
            WingPosition.AddFramePoint2x(21, 25, 44);
            WingPosition.AddFramePoint2x(22, 25, 44);

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(26, 15);
            HeadVanityPosition.AddFramePoint2x(16, 16 + 1, 16);
            HeadVanityPosition.AddFramePoint2x(17, 32 + 1, 17);
            HeadVanityPosition.AddFramePoint2x(18, 41 + 1, 28);

            HeadVanityPosition.AddFramePoint2x(19, 25 + 1, 22);
            HeadVanityPosition.AddFramePoint2x(20, 25 + 1, 22);
            HeadVanityPosition.AddFramePoint2x(21, 25 + 1, 22);
            HeadVanityPosition.AddFramePoint2x(22, 25 + 1, 22);

            HeadVanityPosition.AddFramePoint2x(24, 26, 23);

            HeadVanityPosition.AddFramePoint2x(26, 41, 30);

            SkinsAndOutfits();
            RequestList();
			TopicList();
        }
		
		public void TopicList()
		{
			
		}

        public void RequestList()
        {
            //0
            AddNewRequest("A Little Warm Up", 250,
                "*[nickname], I need your help to test my power. There is a creature you call Skeletron in this world, I want to face It alongside you, to see if I can try protecting you. Can you give me a help on this?*",
                "*Thank you. Maybe there is some way of calling It again, I have no idea how you could do that.*",
                "*Oh, you're not ready to face It again? Don't worry about that, I'm okay.*",
                "*I think we did good on this fight, but I feel bad about the Clothier's death. I can't really feel any sense of victory in this fight.*",
                "*You want to know how to call the Skeletron? I have no idea either. It's related to the Clother, right? Maybe there is a way.*",
                "*I can't protect anyone If I let the ones I'm supposed to protect die... I still need to practice more.*");
            AddRequestRequirement(delegate(Player player)
            {
                return NPC.downedBoss3;
            });
            AddKillBossRequest(Terraria.ID.NPCID.SkeletronHead, 2);
            AddRequesterSummonedRequirement();
            AddNobodyKodRequirement();
            //1
            AddNewRequest("Jaws Hunter", 270,
                "*I need to get stronger, which means I need to eat more meat. No, I'm nothinking about you or your citizens. There isn't much variety of edible things in this world, but I think Sharks will help me with this matter. What do you think? Ready for some fishing?*",
                "*[nickname], allow me to assist you in this request. I want to get stronger, but I can't risk endangering my client with It.*",
                "*Yes, their jaws are really big, maybe I should try something smaller, like bats.*",
                "*Wow! Look at all that Shark meat. I think I can turn all that in muscles in a few weeks.*");
            AddRequestRequirement(RequestBase.GetNightRequestRequirement);
            AddObjectCollectionRequest("Shark Meat", 1, 0.2f);
            AddObjectDroppingMonster(Terraria.ID.NPCID.Shark, 0.8f);
            //2
            AddNewRequest("Green Menace", 285,
                "*The goblins may attempt to launch a full attack to your town any time in the future. We should make them to try attacking us now, so we can weaken them, and make them spend more time trying to resuply. What do you think, [nickname]?*",
                "*I knew you would agree with my plan. We should create a Goblin Battle Standard to force them to show up. The Goblin Spies who appear in the Far Lands of the World can help us make that.*",
                "*But [nickname], the safety of the town is at stake here! Okay... We'll not try making them attack us then.*",
                "*Haha! That was a good fight! This should make them spend a long time trying to gather supplies for another attack. We should be safe, for now.*",
                "*We need to make a Goblin Battle Standard to lure them. Wood you know where to find, the clothings used for It can be acquired from Goblin Spies. They appear on the far lands of the world.*");
            AddRequestRequirement(delegate(Player player){
                return NPC.downedGoblins;
            });
            AddEventParticipationObjective(Terraria.ID.InvasionID.GoblinArmy);
            //3
            AddNewRequest("A drink with a friend", 330,
                "*[nickname], would you like sharing a drink with me?*",
                "*Great, let's meet at the pub, then.*",
                "*Busy? Okay, let's try again another time.*",
                "*[nickname], ever since you gave me that package, I've been looking at It's content day and night, and I think It's time for me to put that outfit again. I want you to be the first person to see me use It, and tell me how I look. [Unlocked Royal Guard Outfit]*",
                "*Let's go to a pub, I have something to talk about with you. The Bartender could give us some drinks.*");
            AddRequestRequirement(delegate(Player player)
            {
                if (PlayerMod.PlayerHasGuardian(player, GuardianBase.Domino) && NPC.AnyNPCs(Terraria.ID.NPCID.DD2Bartender))
                {
                    GuardianData BrutusData = PlayerMod.GetPlayerGuardian(player, GuardianBase.Brutus),
                        DominoData = PlayerMod.GetPlayerGuardian(player, GuardianBase.Domino);
                    if (BrutusData.request.RequestsCompletedIDs.Contains(0) && BrutusData.request.RequestsCompletedIDs.Contains(1) && BrutusData.request.RequestsCompletedIDs.Contains(2) &&
                        !BrutusData.request.RequestsCompletedIDs.Contains(3) && DominoData.request.RequestsCompletedIDs.Contains(1))
                        return true;
                }
                return false;
            });
            AddItemCollectionObjective(Terraria.ID.ItemID.Ale, 1, 0);
            AddRequesterSummonedRequirement();
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            if (trigger == TriggerTypes.PlayerDowned)
            {
                if (Value == guardian.OwnerPos)
                {
                    bool GuardianJustWokeUp = false;
                    if (guardian.KnockedOut)
                    {
                        guardian.ExitDownedState();
                        guardian.HP = (int)(guardian.MHP * 0.2f);
                        GuardianJustWokeUp = true;
                        guardian.SaySomething("*Urgh... I'm not dead yet...*");
                    }
                    if (!guardian.DoAction.InUse || !guardian.DoAction.IsGuardianSpecificAction || guardian.DoAction.ID != ProtectModeID)
                    {
                        guardian.StartNewGuardianAction(new Creatures.Brutus.ProtectModeAction(Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().KnockedOut), ProtectModeID);
                        if (!GuardianJustWokeUp)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*Hang on! I'm coming!*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*[nickname]!*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*No! [nickname]! I'm coming!*");
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*Ugh... [nickname]...*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*[nickname]... I'm... Coming...*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*... I... Can't fall... Yet...*");
                                    break;
                            }
                        }
                    }
                }
            }
            if (trigger == TriggerTypes.PlayerHurt)
            {
                if (Value == guardian.OwnerPos)
                {
                    Player player = Main.player[Value];
                    if (player.statLife < player.statLifeMax2 * 0.3f && player.statLife + Value2 >= player.statLifeMax2 * 0.3f)
                    {
                        if (guardian.KnockedOut && !guardian.KnockedOutCold)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*[nickname]! Go now! Save yourself!*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*I'll distract It [nickname]! Go! My contract is worth nothing if you die.*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*[nickname], run! I'll try distracting that creature.*");
                                    break;
                            }
                        }
                        else if (!guardian.KnockedOutCold)
                        {
                            if (guardian.PlayerMounted)
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        guardian.SaySomething("*[nickname]! Damn...*");
                                        break;
                                    case 1:
                                        guardian.SaySomething("*Maybe placing you on my shoulder wasn't a good idea.*");
                                        break;
                                    case 2:
                                        guardian.SaySomething("*[nickname], are you okay?*");
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        guardian.SaySomething("*[nickname], move to behind me.*");
                                        break;
                                    case 1:
                                        guardian.SaySomething("*[nickname], let me lure that creature attention while you heal.*");
                                        break;
                                    case 2:
                                        guardian.SaySomething("*Watch your health! Get yourself behind me before It's too late.*");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public void SkinsAndOutfits()
        {
            AddOutfit(1, "Royal Guard Outfit", delegate(GuardianData gd, Player player) { return PlayerMod.GetPlayerGuardian(player, GuardianBase.Brutus).request.RequestsCompletedIDs.Contains(3); });
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*If we end up getting surrounded by creatures, just tell me to put you on my shoulders, I can handle the attacks while you help me mow down the creatures.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*Hey boss, If you have any mission you need someone to do, feel free to control me to do it. For you, I guess It's a good cause.*";
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.DefenseRate += 0.10f;
            if (g.SittingOnPlayerMount)
            {
                g.CoverRate += 25;
                g.BlockRate += 20;
            }
            if (g.PlayerMounted)
            {
                g.CoverRate += 20;
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*If you need a bodyguard, you just need to call me.*";
                case 1:
                    return "*I wont charge a fee for my services now, since someone already paid it. Just call me whenever you need help.*";
                case 2:
                    return "*I will get rusty if I don't get into constant action. If there is trouble in your path, give me a call.*";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Don't worry, your town citizens are safe with me around.*");
            Mes.Add("*One thing I disliked about my last job, was standing around, waiting for something to happen. Here is not so much different, but at least the people are cool.*");
            Mes.Add("*When possible, take me with you on your travels, I need to strengthen myself with the heat of the battle.*");
            Mes.Add("*Don't tell anyone, but I like dancing when there is nobody around. I've got to keep the \"Tough guy\" fame, you know.*");
            Mes.Add("*I have trained my left arm with several kinds of swords, so I can even wield two handed swords with it only.*");
            Mes.Add("*My right arm helps on my protection, so I can shield myself from a number of attacks.*");
            Mes.Add("*My job as a Royal Guard on the Ether Realm... Wasn't good...*");
            Mes.Add("*Many Terrarians usually get shocked when I showed them my weapons, so I'll tell you why I have them. Those are the weapons the guards on my world uses. The day I lost my job, they at least let me keep my weapons.*");
            Mes.Add("*If you're asking yourself about what's the contract about, don't worry. The contract only expires if either of us dies for good. Until that doesn't happens, I'm bound to you for life.*");
            if (Main.bloodMoon)
                Mes.Add("*Hey, let's go outside? That's the perfect occasion to hone our combat skills.*");
            if (Main.bloodMoon || Main.eclipse)
                Mes.Add("*Don't worry, no creatures will harm your citizens. I guarantee.*");
            if (Main.eclipse)
                Mes.Add("*Where did those creatures came from?*");
            if (NPC.AnyNPCs(19))
                Mes.Add("*Sometimes [nn:19] helps me with my training.*");
            if (Main.hardMode && NPC.AnyNPCs(22))
                Mes.Add("*Do you know why sometimes [nn:22] simply... Explodes?*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant))
                Mes.Add("*Part of the products [nn:" + Terraria.ID.NPCID.TravellingMerchant + "] carries comes from the Ether Realm. I wonder how he acquires those, since that place is too dangerous for Terrarians.*");
            if(Main.invasionType == Terraria.ID.InvasionID.MartianMadness && Main.invasionSize > 0)
                Mes.Add("*Don't stay outside for too long, I don't know If I'll be able to rescue you if you get beamed up.*");
            if (Terraria.GameContent.Events.DD2Event.DownedInvasionAnyDifficulty)
                Mes.Add("*The Old One's Army is the perfect event for training my bodyguard skills. If I be able to protect the crystal, I can protect anyone.*");
            Mes.Add("*You want to visit the Ether Realm? I'm not sure if you will be able to, things are very tough there for Terrarians.*");
            if (NpcMod.HasGuardianNPC(0))
                Mes.Add("*[gn:0] has what it takes to be a good warrior, he just needs to take things a bit more serious.*");
            if (NpcMod.HasGuardianNPC(1))
            {
                if (!NpcMod.HasGuardianNPC(3))
                {
                    Mes.Add("*I think [gn:1] is really cute, do you think she would date me?*");
                    Mes.Add("*I think I should try asking [gn:1] for a date, she has some... Good moves.*");
                }
                else
                {
                    Mes.Add("*My plans on asking [gn:1] on a date went downhill, since she still loves that creepy zombie.*");
                    Mes.Add("*I got the chance to talk with [gn:1] early this day, but had to back away when [gn:3] appeared.*");
                }
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("*I'm not a fool, I'm certain that what [gn:1] does to [gn:2] is just to bully him. I hope he has an iron will, or else...*");
                    Mes.Add("*Do you think I should extend my bodyguard job to [gn:2] too? I don't like what [gn:1] does to him. But I don't want her to think badly of me, that could hurt my chan- I mean, my professionalism. What should I do?*");
                    Mes.Add("*Earlier this day, [gn:2] tried to hire me to give a lesson to [gn:1]. I refused it for... Some reason.*");
                }
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("*You got a zombie on your town? How were you able to do that? I mean... Wow!*");
                Mes.Add("*Do I need to look out for [gn:3]? He may offer some danger to the people in the town?*");
                Mes.Add("*If [gn:3] tries something funny, I'll slice him in half with my sword.*");
                Mes.Add("*[gn:3] seems to be a cool guy, but I keep having a chill going down my spine whenever I get near him.*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*I think [gn:5] isn't going to protect anyone from danger just by playing all the time.*");
                Mes.Add("*I have to say, sometimes when I'm on a break, I play a bit with [gn:5]. But I hate it when he ruins my mane.*");
                Mes.Add("*I wonder who is that " + AlexRecruitScripts.AlexOldPartner + ", If I were there at the time she died, would she have survived? I keep wondering about that kind of thing.*");
            }
            Mes.Add("*The chairs you placed on my house are too small. If you open a hole in the middle of them I could use them for something else.*");
            if (NpcMod.HasGuardianNPC(7))
            {
                Mes.Add("*Before you ask, [gn:7] didn't exactly won clean the arm wrestling, she unsheathed the claws before the countdown ended.*");
                if (NpcMod.HasGuardianNPC(3))
                {
                    Mes.Add("*Sometimes I do a side job for [gn:7], I don't mean to talk about my contracts but... She asked me to protect her from [gn:3]. Did he threaten her or something?*");
                }
                Mes.Add("*I am lucky for having pretty girls around this world. But you had to find some that already have a partner?!*");
                Mes.Add("*Sometimes I have a few drinks with [gn:7], she always complains about her husband. Well, almost aways, sometimes she tells funny stories about his frustrated adventures, that's why I keep drinking with her.*");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*Everytime I speak with [gn:8], meat comes on my mind.*");
                Mes.Add("*My luck seems to be changing, [gn:8] is single and cute. Do you think I have a chance?*");
            }
            if (NpcMod.HasGuardianNPC(Domino))
            {
                Mes.Add("*Of all Guardians you could have let move in, you had to allow [gn:" + Domino + "] to open a shop here? Do you know how much trouble he made me pass through on the Ether Realm?*");
                Mes.Add("*Remember when I promissed to keep all your town citizens safe? May I do an exception for [gn:" + Domino + "]?*");
                Mes.Add("*Hey, do you have any laws against smuggling in your world? I would be glad to arrest [gn:" + Domino + "] for that.*");
                Mes.Add("*[gn:" + Domino + "] always managed to escape from the guards on the Ether Realm somehow. I never managed to find out how.*");
                Mes.Add("*Keep an eye close on [gn:"+Domino+"]. If that mutt does something you disapprove, come tell me.*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                if (NpcMod.HasGuardianNPC(Zacks))
                {
                    Mes.Add("*I sure don't take a break on this world. Now [gn:"+Leopold+"] want me to protect him from [gn:"+Zacks+"]. Maybe I should start charging per hour?*");
                }
                Mes.Add("*You're looking for [gn:" + Leopold + "]? I saw him being chased by bees earlier this day.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*Yo, tell [gn:"+Vladimir+"] to keep his paws off me. I can't be seen being hugged by other people, that can put my career down the drain.*");
                Mes.Add("*Can I tell you something? I also have my own troubles that I want to confess, but I can't show any sign of weakness, or else people around will start doubting that I can protect them.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Michelle))
            {
                Mes.Add("*If you call me giant kitty, I will punch your stomach.*");
                Mes.Add("*Don't listen to what [gn:"+GuardianBase.Michelle+"] says about me, I'm as tough as a rock.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
            {
                Mes.Add("*Hey, did you came from [gn:" + Malisha + "]'s house? Do you know if she needs a test subject for something?*");
                Mes.Add("*I really like participating of [gn:" + Malisha + "]'s experiement, that way I can stay close to her for quite a long time.*");
                Mes.Add("*[gn:" + Malisha + "] once casted a shrinking spell on me, I would normally have been scared of that, if It wasn't for the view of her I had. I mean... Wow! I hope she repeats that experiement in the future.*");
                Mes.Add("*Do you think [gn"+Malisha+"] and I... No... Nevermind... Why am I talking about this to you?*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Wrath))
            {
                Mes.Add("You're saying that [gn:"+Wrath +"]'s punches hurts? Funny, I didn't felt pain whenever he punched me.");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*A ghost now? Your life seems full of weird frienship.*");
                Mes.Add("*I was alerted at first by [gn:" + Fluffles + "], until I noticed that she doesn't seems like a bad ghost... Err... Person.*");
                Mes.Add("*I kind of would like seeing [gn:"+Fluffles+"]... Float... In front of me.*");
                if (NpcMod.HasGuardianNPC(Sardine) && NpcMod.HasGuardianNPC(Blue))
                {
                    if (NpcMod.HasGuardianNPC(Zacks))
                    {
                        Mes.Add("*Sometimes I have to stop [gn:" + Blue + "] and their friends game on [gn:" + Sardine + "] because things goes way too far.*");
                    }
                    Mes.Add("*It seems like [gn:" + Sardine + "] is in a even bigger problem now that [gn:"+Fluffles+"] joined [gn:"+Blue+"]'s game.*");
                }
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*[nickname], where did you find that angel known as [gn:"+Minerva+"]? That woman cooks several tasty foods for me. I'm really glad that you found her.*");
                Mes.Add("*Aaaahh... I'm stuffed. [gn:"+Minerva+"] really cooks very well. I'll see her in again only in about 8 hours.*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                bool HasBreeMet = PlayerMod.PlayerHasGuardian(player, Bree), HasSardineMet = PlayerMod.PlayerHasGuardian(player, Sardine);
                if (!HasBreeMet && !HasSardineMet)
                {
                    Mes.Add("*Sometimes I'm walking the lands around the town, to see if I can find [gn:"+Glenn+"]'s parents. I have the feeling that they are alive, and out there.*");
                }
                else if (HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*I'm glad that [gn:"+Glenn+"] managed to find his parents.*");
                }
                else if (HasBreeMet)
                {
                    Mes.Add("*It seems like [gn:" + Glenn + "]'s mother has already been found, but his father is still missing.*");
                }
                else if (HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + Glenn + "]'s father has already been found, but his mother is still missing.*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*I have to say, the way I'm sitting, is easier for me to do this. But... Do you really have to keep staring at me?*");
                Mes.Add("*I wonder how many times I will have to flush this thing.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*I promisse to protect you while you sleep. I wont close my eyes during the entire night.*");
                Mes.Add("*If anything tries to attack you while sleeping, will never expect me to be here. You will be safe.*");
                Mes.Add("*So, you need my protection during the night? I can help you with that.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (guardian.KnockedOutCold)
                {
                    Mes.Add("(He seems to be breathing quite hard.)");
                }
                else
                {
                    Mes.Add("*[nickname]... If things gets too hard, leave me here, and save yourself... Ngh... (He looks very wounded)*");
                    Mes.Add("*Argh.. Ugh... I'm fine... Worry about yourself... I can hold them... Ugh...*");
                    Mes.Add("*[nickname]... Listen to me... My contract is worth nothing... If you perish... If you must... Go... Argh!*");
                }
                Mes.Add("(He seems to be trying to endure the pain)");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(You wonder if the way he sleeps wont make him have pain all over the body, during the morning.)");
                Mes.Add("(His bad breath from his snoring reaches your nose, making you plug It.)");
                Mes.Add("(He's sleeping like a stone, you wonder if you could wake him up whenever something happens.)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*[nickname], there is... A ghost... On your shoulder...*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*Huh? Nothing. Nothing right now, try other time.*";
            return "*I don't think so. I have everything I need right now.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I have no time to look for this myself, so I ask you to do something for me.*";
            return "*I can't leave the town unprotected, so I ask you to do something for me.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*Great job, I knew you would do it.*";
            return "*You were able to complete the task, very nice.*";
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I don't know how much guard job pays in your world, but don't I deserve a house?*");
            Mes.Add("*During my guard jobs on the Ether Realm, at least I had a house to live, what about giving me one?*");
            if (Main.raining)
                Mes.Add("*Well, this is awful. I could have a house to avoid such situation.*");
            if (!Main.dayTime)
                Mes.Add("*Living outside will hone my skills, but I wont be able to sleep and recover my energy.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Those evil zones have no chance of getting near the town, right?*");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("*Do you know of any gift that would be good for [gn:1]?*");
            Mes.Add("*The day I lost my job? My king tasked me into joining an attack to a village of farmers, but I refused. I just didn't got exiled due to the help of a friend of mine, who also were a captain of the royal guard.*");
            Mes.Add("*I was once the captain of the Royal Guard in the ether realm, you know. Right now, I'm just a guard on your town. Maybe I can make use of what I learned on my path on your world, too.*");
            Mes.Add("*Does your world have some kind of ruler? Are you some kind of ruler? Or does your people live free?*");
            Mes.Add("*Sometimes I visit the Ether realm. You know, I have parents there. Maybe one day you'll meet them.*");
            Mes.Add("*For some reason, several people from the Ether Realm are leaving it recently. I wonder why is that happening.*");
            Mes.Add("*If you are wondering why so many TerraGuardians are appearing on your world, is because they are leaving the Ether Realm. I don't know why, but I don't think It's a really bad thing.*");
            Mes.Add("*Most TerraGuardians have family and friends, and they are also friendly. But of course there are some who aren't very friendly, or follow ethics and law.*");
            if(NpcMod.HasGuardianNPC(5))
                Mes.Add("*I wonder, does riding on [gn:5]'s back makes me into a Knight?*");
            if (NpcMod.HasGuardianNPC(2) && GuardianBountyQuest.SignID > -1)
                Mes.Add("*I have interest on [gn:2]'s bounties, I think that will help improve safety on your world... And also my combat abilities.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What? What? Yes, I know It's my birthday. I can't hold my excitement.*");
            Mes.Add("*Party! Party!! Woohoo!! Let's dance!!*");
            if (!PlayerMod.HasGuardianBeenGifted(player, 6))
                Mes.Add("*What you will gift me? What? What? What?*");
            Mes.Add("*I'm really happy about this p... Is that a... Cake? Cake! CAAAAAAAAAAKE!!! The biggest slice is mine!!*");
            Mes.Add("*This is the best! Like... Really the best!! You guys are the best!!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*Boss, I wont accept a refund if you die.*");
                Mes.Add("*It will be bad to my career of bodyguard if you die.*");
                Mes.Add("*Come on Boss, I still have to share a few drinks with you.*");
            }
            Mes.Add("*It's just a flesh wound, you'll be fine.*");
            Mes.Add("*Nothing will hurt you as long as I'm here.*");
            Mes.Add("*I really hate monopolization of my services, I can protect you too.*");
            Mes.Add("*Not on my shift, buddy.*");
            Mes.Add("*You wont let a simple bleeding take you down, right?*");
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID)
            {
                switch (ReviveGuardian.ID)
                {
                    case GuardianBase.Alex:
                        Mes.Add("*You already lost "+AlexRecruitScripts.AlexOldPartner+", you don't want to lose us too, right?*");
                        break;
                    case GuardianBase.Blue:
                        Mes.Add("*You still have someone you need to stay with. I can't bear to give him bad news.*");
                        break;
                    case GuardianBase.Bree:
                        Mes.Add("*Your husband promissed to bring you treasures from his adventures, right? How would he react if his most precious treasure died?*");
                        break;
                    case GuardianBase.Domino:
                        Mes.Add("*I still need evidences to arrest you, I wont let you escape me so easily.*");
                        break;
                    case GuardianBase.Leopold:
                        Mes.Add("*You still have your researches to do, It's not the end yet.*");
                        break;
                    case GuardianBase.Mabel:
                        Mes.Add("*You have a contest to win, you should be practicing, not lying down on the floor.*");
                        break;
                    case GuardianBase.Nemesis:
                        Mes.Add("*You wont get a personality while lying down on the floor. Get up!*");
                        break;
                    case GuardianBase.Rococo:
                        Mes.Add("*The Terrarian wont be happy to see you die. You don't want to disappoint It, right?*");
                        break;
                    case GuardianBase.Sardine:
                        Mes.Add("*You promissed your wife that you'd bring her treasures. Why are you lying down on the floor?*");
                        break;
                    case GuardianBase.Vladimir:
                        Mes.Add("*You still have a lot of people to help in your... Weird ways. What would your brother think of If finds out that his older brother died?*");
                        break;
                    case GuardianBase.Zacks:
                        Mes.Add("*You have tricked death once, I wont try to find out If you can trick it twice. There is someone who wants to see you safe and sound.*");
                        break;
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(RoyalGuardSkinID, "royal_guard_outfit");
            sprites.AddExtraTexture(RoyalGuardSkinFID, "royal_guard_outfit_f");
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if (AnimationID == 2 && guardian.OutfitID == 1 && Frame == 0 && guardian.IsAttackingSomething && (!guardian.OffHandAction || guardian.SelectedOffhand == -1))
            {
                Frame = 1;
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin2, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            switch (guardian.OutfitID)
            {
                case 1:
                    {
                        int FramePosition;
                        bool IsFrameFront;
                        Texture2D texture = sprites.GetExtraTexture(RoyalGuardSkinID);
                        Rectangle LeftArmFrame = guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame),
                            RightArmFrame = guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame),
                            BodyFrame = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                        /*LeftArmFrame.X = (int)(LeftArmFrame.X * 0.5f);
                        LeftArmFrame.Y = (int)(LeftArmFrame.Y * 0.5f);
                        RightArmFrame.X = (int)(RightArmFrame.X * 0.5f);
                        RightArmFrame.Y = (int)(RightArmFrame.Y * 0.5f);
                        BodyFrame.X = (int)(BodyFrame.X * 0.5f);
                        BodyFrame.Y = (int)(BodyFrame.Y * 0.5f);
                        BodyFrame.Width = LeftArmFrame.Width = RightArmFrame.Width = (int)(RightArmFrame.Width * 0.5f);
                        BodyFrame.Height = LeftArmFrame.Height = RightArmFrame.Height = (int)(RightArmFrame.Height * 0.5f);*/
                        int FrameVerticalDistance = BodyFrame.Height * 2;
                        float SpriteScale = Scale;
                        GuardianDrawData gdd;
                        Vector2 Origin = Origin2;// *0.5f;
                        byte BodyPositionBonus = 0, LeftArmPositionBonus = 0, RightArmPositionBonus = 0;
                        if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGRightArm, out FramePosition, out IsFrameFront))
                        {
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, RightArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                            guardian.InsertDrawData(gdd, FramePosition + RightArmPositionBonus, IsFrameFront);
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                            //RightArmPositionBonus++;
                        }
                        else
                        {
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                        }
                        //Draw Right Arm and Shield
                        if (guardian.RightArmAnimationFrame == ThroneSittingFrame || guardian.RightArmAnimationFrame == BedSleepingFrame || guardian.RightArmAnimationFrame == DownedFrame)
                        {
                            if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGBody, out FramePosition, out IsFrameFront))
                            {
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, RightArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                                guardian.AddDrawDataAfter(gdd, FramePosition + BodyPositionBonus, IsFrameFront);
                                LeftArmFrame.Y += FrameVerticalDistance;
                                RightArmFrame.Y += FrameVerticalDistance;
                                BodyFrame.Y += FrameVerticalDistance;
                                BodyPositionBonus++;
                                //
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, RightArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                                guardian.AddDrawDataAfter(gdd, FramePosition + BodyPositionBonus, IsFrameFront);
                                LeftArmFrame.Y += FrameVerticalDistance;
                                RightArmFrame.Y += FrameVerticalDistance;
                                BodyFrame.Y += FrameVerticalDistance;
                                BodyPositionBonus++;
                            }
                            else
                            {
                                LeftArmFrame.Y += FrameVerticalDistance * 2;
                                RightArmFrame.Y += FrameVerticalDistance * 2;
                                BodyFrame.Y += FrameVerticalDistance * 2;
                            }
                        }
                        else
                        {
                            if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGRightArm, out FramePosition, out IsFrameFront))
                            {
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, RightArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                                guardian.AddDrawDataAfter(gdd, FramePosition + RightArmPositionBonus, IsFrameFront);
                                LeftArmFrame.Y += FrameVerticalDistance;
                                RightArmFrame.Y += FrameVerticalDistance;
                                BodyFrame.Y += FrameVerticalDistance;
                                RightArmPositionBonus++;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, RightArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                                guardian.AddDrawDataAfter(gdd, FramePosition + RightArmPositionBonus, IsFrameFront);
                                LeftArmFrame.Y += FrameVerticalDistance;
                                RightArmFrame.Y += FrameVerticalDistance;
                                BodyFrame.Y += FrameVerticalDistance;
                                RightArmPositionBonus++;
                            }
                            else
                            {
                                LeftArmFrame.Y += FrameVerticalDistance * 2;
                                RightArmFrame.Y += FrameVerticalDistance * 2;
                                BodyFrame.Y += FrameVerticalDistance * 2;
                            }
                        }
                        //Back Cloak
                        if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGBody, out FramePosition, out IsFrameFront))
                        {
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, BodyFrame, color, Rotation, Origin, SpriteScale, seffect);
                            guardian.InsertDrawData(gdd, FramePosition, IsFrameFront);
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                        }
                        else
                        {
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                        }
                        //Pants, Greaves, Shirt and Cloak
                        if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGBody, out FramePosition, out IsFrameFront))
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                if (i > 0 || !guardian.IsUsingToilet)
                                {
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, BodyFrame, color, Rotation, Origin, SpriteScale, seffect);
                                    guardian.AddDrawDataAfter(gdd, FramePosition + BodyPositionBonus, IsFrameFront);
                                }
                                LeftArmFrame.Y += FrameVerticalDistance;
                                RightArmFrame.Y += FrameVerticalDistance;
                                BodyFrame.Y += FrameVerticalDistance;
                                BodyPositionBonus++;
                            }
                        }
                        else
                        {
                            LeftArmFrame.Y += FrameVerticalDistance * 4;
                            RightArmFrame.Y += FrameVerticalDistance * 4;
                            BodyFrame.Y += FrameVerticalDistance * 4;
                        }
                        if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGBodyFront, out FramePosition, out IsFrameFront))
                        {
                            int FrameID = guardian.Base.GetBodyFrontSprite(guardian.BodyAnimationFrame);
                            if (FrameID > -1)
                            {
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(RoyalGuardSkinFID), DrawPosition, guardian.GetAnimationFrameRectangle(FrameID), color, Rotation, Origin, SpriteScale, seffect);
                                guardian.AddDrawDataAfter(gdd, FramePosition, IsFrameFront);
                            }
                        }
                        //Left Arm
                        if (guardian.GetTextureSpritePosition(GuardianDrawData.TextureType.TGLeftArm, out FramePosition, out IsFrameFront))
                        {
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, LeftArmFrame, color, Rotation, Origin, SpriteScale, seffect);
                            guardian.AddDrawDataAfter(gdd, FramePosition + LeftArmPositionBonus, IsFrameFront);

                            LeftArmPositionBonus++;
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                        }
                        else
                        {
                            LeftArmFrame.Y += FrameVerticalDistance;
                            RightArmFrame.Y += FrameVerticalDistance;
                            BodyFrame.Y += FrameVerticalDistance;
                        }
                    }
                    break;
            }
        }

        public const int ProtectModeID = 0;
        public const int ProtectModeAutoTriggerTime = 3600;

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (!guardian.DoAction.InUse && guardian.OwnerPos > -1 && (guardian.AfkCounter >= ProtectModeAutoTriggerTime || Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().KnockedOut))
            {
                guardian.StartNewGuardianAction(new Creatures.Brutus.ProtectModeAction(Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().KnockedOut), ProtectModeID);
            }
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*I think this may be able to help you recover your senses.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*W-Wha?! No, I wasn't sleeping, I was just... Reflecting!*";
                        case 1:
                            return "*[nickname]! It's not what you're thinking, I didn't closed my eyes a second.*";
                        case 2:
                            return "*What am I doing in this bed? I was watching the town for troubles.*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Huh? I can explain... Oh, request? Yes. Request, what is It? Did It?*";
                        case 1:
                            return "*When did you... I mean... Yes, I was totally expecting you, did you do what I asked?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I will protect you during your adventures!*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*It will be very difficulty protecting you, when I wont even be able to walk due to so many people.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*I'm not in the mood right now...*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*I think It would be better if I leaved the company in a safe place. Not that anything out there could hurt me, you know.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Okay, call me whenever you get into a dangerous mission.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Okay then, I'll return to the town. Take care of yourself, [nickname].*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*I shall guard your life some more then.*";
                case MessageIDs.RequestAccepted:
                    return "*Try not to get yourself killed while doing my request, [nickname]. If necessary, bring me with you.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*I think you may end up getting stressed out if I give you another request. Maybe do something about the ones you have.*";
                case MessageIDs.RequestRejected:
                    return "*Maybe I asked too much. I'll try doing that later, when I'm off service.*";
                case MessageIDs.RequestPostpone:
                    return "*I guess I can put this on hold.*";
                case MessageIDs.RequestFailed:
                    return "*You couldn't do what I asked, right? I should have thought before giving you such a request.*";
                case MessageIDs.RestAskForHowLong:
                    return "*Yes, you can rest here. How long do you plan on resting?*";
                case MessageIDs.RestNotPossible:
                    return "*[nickname], I recommend taking a rest at a less dangerous time.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*I will keep my eyes open while you rest, don't worry.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*[nickname], I'm sorry to ask but... Could we browser\n[shop]'s shop? There may be something useful for me\nthere.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*[nickname], this wont take long.*";
                case MessageIDs.GenericYes:
                    return "*As you wish.*";
                case MessageIDs.GenericNo:
                    return "*I have to deny this.*";
                case MessageIDs.GenericThankYou:
                    return "*Thanks.*";
                case MessageIDs.ChatAboutSomething:
                    return "*I may be able to answer, depending on what is It.*";
                case MessageIDs.NevermindTheChatting:
                    return "*As you wish, [nickname].*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*What? Is It too dangerous for you, [nickname]? If is, I shouldn't blame you for not doing.*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Hm... Okay. I'll see if I can do that another time.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Oh, okay.*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Let's check you out...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Ugh, I smell ale. And... *";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*How much do you drink? It seems like you've been eating, like lots.*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Ok, I think that's enough info, better I move on!*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... Uh.. Sleeping again? What a shame!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*It wasn't necessary... Thanks anyway...*";
                    return "*It takes much more to take me down, but I apreciate your help.*";
                case MessageIDs.RevivedByRecovery:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Alright, now's my turn.*";
                    return "*That is It?! I'm still standing!*";
            }
            return base.GetSpecialMessage(MessageID);
        }
		
		public void Dialogue()
		{
			
		}
    }
}
