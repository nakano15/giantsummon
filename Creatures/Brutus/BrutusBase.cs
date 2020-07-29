using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class BrutusBase : GuardianBase
    {
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
            FramesInRows = 17;
            Age = 28;
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

            LeftHandPoints.AddFramePoint2x(16, 7, 21);
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

        public override string FriendLevelMessage
        {
            get
            {
                return "*Hey Boss, I'm about to have a few drinks, want to go too?*";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "*I didn't really liked working as a Royal Guard in the Ether Realm, all that standing around was boring. Being your Body Guard is giving me enough adrenaline to love this job.*";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "*It gradually crushes me when I think about the possibility of not being able to defend everyone in this world. What makes me move on, is seeing the faces and talking to the people I defend everyday. That is an automatic reward.*";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "*Hey Boss. I... Uh... Thank you for hiring me. I love this world, I like the town I live, and the citizens who live in it. It's like family. And for my family, I gladly give my life. My sword is yours, and I will be your shield.*";
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

        public const int ProtectModeID = 0;

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (!guardian.DoAction.InUse && guardian.OwnerPos > -1 && guardian.AfkCounter >= 60 * 60)
            {
                guardian.StartNewGuardianAction(ProtectModeID);
            }
        }

        public override void GuardianActionUpdate(TerraGuardian guardian, GuardianActions action)
        {
            if (action.IsGuardianSpecificAction)
            {
                switch (action.ID)
                {
                    case ProtectModeID:
                        {
                            if (guardian.OwnerPos == -1)
                            {
                                action.InUse = false;
                                return;
                            }
                            if (guardian.AfkCounter < 60)
                            {
                                action.InUse = false;
                                return;
                            }
                            const byte CurrentActionID = 0;
                            int Action = action.GetIntegerValue(CurrentActionID);
                            Player defended = Main.player[guardian.OwnerPos];
                            const int Offset = 7 * 2;
                            float DefendX = defended.Center.X - Offset * defended.direction;
                            if (Action == 0)
                            {
                                if (guardian.Position.X + guardian.Velocity.X > DefendX)
                                    guardian.MoveLeft = true;
                                else
                                    guardian.MoveRight = true;
                                if (Math.Abs(guardian.Position.X - DefendX) < 5 && Math.Abs(guardian.Velocity.X) < 3f)
                                {
                                    Action = 1;
                                }
                            }
                            else if (Action == 1)
                            {
                                guardian.Position.X = DefendX;
                                if (!guardian.IsAttackingSomething)
                                {
                                    guardian.LookingLeft = defended.direction == -1;
                                }
                                guardian.Jump = false;
                                guardian.MoveDown = true;
                                guardian.OffHandAction = true;
                                defended.AddBuff(ModContent.BuffType<Buffs.Defended>(), 3);
                            }
                            action.SetIntegerValue(CurrentActionID, Action);
                        }
                        break;
                }
            }
        }
    }
}
