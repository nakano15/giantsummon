﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using giantsummon.Trigger;

namespace giantsummon.Companions
{
    public class BrutusBase : GuardianBase
    {
        public const string RoyalGuardTextureID = "royal_guard", RoyalGuardTextureFID = "royal_guard_f";
        public const int RoyalGuardOutfitID = 1;

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
            CompanionSlotWeight = 1.3f;
            DefaultTactic = CombatTactic.Charge;
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
            LeadGroupUnlockLevel = 12;
            VladimirBase.AddCarryBlacklist(Brutus);

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

            LeftHandPoints.AddFramePoint2x(10, 17, 17);
            LeftHandPoints.AddFramePoint2x(11, 37, 12);
            LeftHandPoints.AddFramePoint2x(12, 43, 24);
            LeftHandPoints.AddFramePoint2x(13, 37, 36);

            LeftHandPoints.AddFramePoint2x(16, 8, 21);
            LeftHandPoints.AddFramePoint2x(17, 46, 20);
            LeftHandPoints.AddFramePoint2x(18, 50, 45);

            LeftHandPoints.AddFramePoint2x(20, 16, 12);
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

            RightHandPoints.AddFramePoint2x(10, 23, 7);
            RightHandPoints.AddFramePoint2x(11, 41, 12);
            RightHandPoints.AddFramePoint2x(12, 46, 25);
            RightHandPoints.AddFramePoint2x(13, 40, 36);

            RightHandPoints.AddFramePoint2x(16, 36, 18);
            RightHandPoints.AddFramePoint2x(17, 45, 32);
            RightHandPoints.AddFramePoint2x(18, 51, 47);

            RightHandPoints.AddFramePoint2x(20, 20, 12);
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
            HeadVanityPosition.DefaultCoordinate2x = new Point(30 - 2, 14);
            HeadVanityPosition.AddFramePoint2x(16, 16 + 1, 16);
            HeadVanityPosition.AddFramePoint2x(17, 32 + 1, 17);
            HeadVanityPosition.AddFramePoint2x(18, 41 + 1, 28);

            HeadVanityPosition.AddFramePoint2x(19, 30 - 2, 21);
            HeadVanityPosition.AddFramePoint2x(20, 30 - 2, 21);
            HeadVanityPosition.AddFramePoint2x(21, 30 - 2, 21);
            HeadVanityPosition.AddFramePoint2x(22, 30 - 2, 21);

            HeadVanityPosition.AddFramePoint2x(24, 27 - 2, 21);

            HeadVanityPosition.AddFramePoint2x(26, 43 - 2, 31);

            SkinsAndOutfits();
			TopicList();
        }
		
		public void TopicList()
		{
			
		}

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            if (trigger == TriggerTypes.Downed && Sender.TargetType == TriggerTarget.TargetTypes.Player)
            {
                if (Sender.TargetID == guardian.OwnerPos)
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
                        guardian.StartNewGuardianAction(new Companions.Brutus.ProtectModeAction(Main.player[Sender.TargetID].GetModPlayer<PlayerMod>().KnockedOut), ProtectModeID);
                        string PlayerNickname = PlayerMod.GetPlayerNicknameGivenByGuardian(Main.player[Sender.TargetID], guardian.MyID);
                        if (!GuardianJustWokeUp)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*Hang on! I'm coming!*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*" + PlayerNickname + "!*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*No! " + PlayerNickname + "! I'm coming!*");
                                    break;
                            }
                        }
                        else
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*Ugh... " + PlayerNickname + "...*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*" + PlayerNickname + "... I'm... Coming...*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*... I... Can't fall... Yet...*");
                                    break;
                            }
                        }
                    }
                }
            }
            if (trigger == TriggerTypes.Hurt && Sender.TargetType == TriggerTarget.TargetTypes.Player)
            {
                if (Sender.TargetID == guardian.OwnerPos)
                {
                    Player player = Main.player[Sender.TargetID];
                    string PlayerNickname = PlayerMod.GetPlayerNicknameGivenByGuardian(player, guardian.MyID);
                    if (player.statLife < player.statLifeMax2 * 0.15f && player.statLife + Value2 >= player.statLifeMax2 * 0.15f)
                    {
                        if (guardian.KnockedOut && !guardian.KnockedOutCold)
                        {
                            switch (Main.rand.Next(3))
                            {
                                case 0:
                                    guardian.SaySomething("*" + PlayerNickname + "! Go now! Save yourself!*");
                                    break;
                                case 1:
                                    guardian.SaySomething("*I'll distract them, " + PlayerNickname + "! Go! My contract is worth nothing if you die.*");
                                    break;
                                case 2:
                                    guardian.SaySomething("*" + PlayerNickname + ", run! I'll try distracting that creature.*");
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
                                        guardian.SaySomething("*" + PlayerNickname + "! Damn...*");
                                        break;
                                    case 1:
                                        guardian.SaySomething("*Maybe having you on my shoulder wasn't a good idea.*");
                                        break;
                                    case 2:
                                        guardian.SaySomething("*" + PlayerNickname + ", are you okay?*");
                                        break;
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(3))
                                {
                                    case 0:
                                        guardian.SaySomething("*" + PlayerNickname + ", move to behind me.*");
                                        break;
                                    case 1:
                                        guardian.SaySomething("*" + PlayerNickname + ", let me lure that creature attention while you heal.*");
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
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public void SkinsAndOutfits()
        {
            AddOutfit(1, "Royal Guard Outfit", delegate(GuardianData gd, Player player) { return gd.FriendshipLevel >= 5; }); //TODO - Need to add alternative way of getting this outfit.
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
            switch (guardian.OutfitID)
            {
                case RoyalGuardOutfitID:
                    Mes.Add("*Halt! What do you think? It fits me, right?*");
                    Mes.Add("*This outfit means so much for me: I used it on my last job for years, and using it again brings me memories.*");
                    Mes.Add("*There is no way I can convince you to tell me who gave you this, right?*");
                    break;
            }
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
                Mes.Add("*You're saying that [gn:" + Wrath + "]'s punches hurts? Funny, I didn't felt anything whenever he punched me.*");
            }
            if (NpcMod.HasGuardianNPC(Fear))
            {
                Mes.Add("*I would like [gn:" + Fear + "] to stop coming seek me everytime he gets scared at anything.*");
                Mes.Add("*With [gn:" + Fear + "] screaming around, it's really hard to be ready for a true emergency.*");
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
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*It's very reckless of [gn:" + Cinnamon + "] to go gather ingredients alone outside of the town. From now on, she need to tell me so I can ensure her safety when doing so.*");
                Mes.Add("*I don't think [gn:" + Cinnamon + "] has what It takes to survive outside city walls. If she gets in danger or hurt, I wont feel good.*");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("*[gn:"+Green+"] keeps saying everytime I visit him that my wounds are light. Maybe I'm exagerating a bit on the visits.*");
            }
            if (NpcMod.HasGuardianNPC(Miguel))
            {
                Mes.Add("*[gn:" + Miguel + "] is a good addition to your world. I can strain my muscles and get even stronger for my job.*");
                Mes.Add("*[gn:" + Miguel + "] thought he could beat me on arm wrestling. Hahaha.*");
            }
            if (NpcMod.HasGuardianNPC(Cille))
            {
                Mes.Add("*I'm really interessed in offering my bodyguard job to [gn:" + Cille + "], but she always tells me to leave her alone.*");
                Mes.Add("*If you manage to make [gn:" + Cille + "] open up, please come tell me how you did that.*");
                Mes.Add("*I've been hearing rummors of [gn:" + Cille + "] attacking people, but I can't believe she would do such a thing.*");
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
            else if (guardian.IsSleeping)
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
            if (guardian.request.Base is TravelRequestBase)
                return "*I need to patrol the world for troubles. Can you [objective]?*";
            if (Main.rand.NextDouble() < 0.5)
                return "*I have no time to look for this myself, so I ask you to do something for me. I need you to [objective], can you do it?*";
            return "*I can't leave the town unprotected, so I have a thing to ask you for. Can you [objective]?*";
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
            sprites.AddExtraTexture(RoyalGuardTextureID, "royal_guard_outfit");
            sprites.AddExtraTexture(RoyalGuardTextureFID, "royal_guard_outfit_f");
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
                        Texture2D texture = sprites.GetExtraTexture(RoyalGuardTextureID);
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
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(RoyalGuardTextureFID), DrawPosition, guardian.GetAnimationFrameRectangle(FrameID), color, Rotation, Origin, SpriteScale, seffect);
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
                guardian.StartNewGuardianAction(new Brutus.ProtectModeAction(Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().KnockedOut), ProtectModeID);
            }
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Domino)
                {
                    Weight = 1.5f;
                    return "*Fine, if you think that's alright. Do let me know if he do something illegal.*";
                }
                if(WhoJoined.ID == Blue || WhoJoined.ID == Mabel || WhoJoined.ID == Malisha || WhoJoined.ID == Luna)
                {
                    Weight = 1.2f;
                    return "*Oh hello, aren't you a pret- I mean... Do you need a bodyguard?*";
                }
            }
            Weight = 1f;
            return "*Welcome to our world.*";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Blue:
                        Weight = 1.2f;
                        return "*She's coming with us? That's nice.*";
                    case Mabel:
                        Weight = 1.2f;
                        return "*I'm starting to enjoy more this job.*";
                    case Malisha:
                        Weight = 1.5f;
                        return "*I'm curious to see what magic trick she'll do next.*";
                    case Miguel:
                        Weight = 1.5f;
                        return "*I don't need a personal trainer.*";
                    case Vladimir:
                        Weight = 1.2f;
                        return "*Don't you even dare hug me.*";
                    case Liebre:
                        Weight = 1.5f;
                        return "*Wait, we'll even return home alive?*";
                }
            }
            Weight = 1f;
            return "*Don't worry, I will protect you too.*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*Take this as my vow to you, that I will protect you until the end of our lives, my buddy.*";
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
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Say, have you do what I asked?*";
                case MessageIDs.RequestRemindObjective:
                    return "*Hm... [objective] is what I asked you for.*";
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
                case MessageIDs.GenericNevermind:
                    return "*Forget it then.*";
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
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*I don't feell good...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*It burns!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*I can't see!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Everyone multiplied..*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*What's wrong with my arms?*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My legs!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I feel weak...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*I feel... Vulnerable...*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What is that abomination? Everyone fall back!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*I can take anything else you throw on me, but this...*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Cold...*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*You'll see when I cut myself out of this!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*I will show you my fury!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Go on, hit me.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Delicious. There is more?*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*I feel... Stronger! Even my muscles seems bigger.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Now I can charge into battle.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Try taking me down now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*This will hurt my foes a lot.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*I dislike doing this..*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Ahh.. A nice, and good mug of ale.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*Honey would go perfect on a well cooked steak.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*I even feel a little healthier after seeing that.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Wait! I see a pressure plate.*";
                case MessageIDs.FoundMineTile:
                    return "*Watch out! A mine!*";
                case MessageIDs.FoundDetonatorTile:
                    return "*I recommend you not to press that detonator.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I sense coming from that thing...*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I can always do some extra exercise.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Pillage!*";
                case MessageIDs.FoundGemTile:
                    return "*Look, there's some gems there.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I see some ores here, they may be useful for our armory.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*There's rare ores over there.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*Rails! Maybe that will ease our travels.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Let's end this travel then.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*I wonder if they can cook.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*That's very weird. I don't know if I should've let you do that.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname]! I'm coming! Hang on!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*No!!! [nickname]!! It should have been me, not you!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Ally down! We should help them when possible!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*I didn't knew It was my birthday.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*Stay safe, [nickname]. I'll leave to sell some stuff and be back.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*[nickname], move to behind me!*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Huff... Gruff... Don't worry... I'm fine..*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I have feel potions with me.*";
                case MessageIDs.UsesLastPotion:
                    return "*I ran out of potions! We should restock.*";
                case MessageIDs.SpottedABoss:
                    return "*Terrarian, stay behind me.*";
                case MessageIDs.DefeatedABoss:
                    return "*Threat eliminated.*";
                case MessageIDs.InvasionBegins:
                    return "*Don't worry, I will protect you from them.*";
                case MessageIDs.RepelledInvasion:
                    return "*Those are the last ones of them. We're safe now.*";
                case MessageIDs.EventBegins:
                    return "*That doesn't look good... On guard, everyone!*";
                case MessageIDs.EventEnds:
                    return "*It's finally over... I could use a drink now.*";
                case MessageIDs.RescueComingMessage:
                    return "*Hang on, I'll help you!*";
                case MessageIDs.RescueGotMessage:
                    return "*You're safe, I'll protect you.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*You're not the only one I offer my services to. A terrarian named [player] is also under my protection.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*I heard that [player] defeated [subject] recently. I was there when that happened, probably.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*[player] found something valuable during their travels. A [subject], people says.*";
                case MessageIDs.FeatEventFinished:
                    return "*I had lots of work recently. A [subject] broke out on [player]'s world, and I had to defend the townspeople. Gladly the Terrarian managed to take care of it.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*My work just got extended recently. [player] has met [subject], and their life is under my protection too.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I really wanted to drink until I drop now... I sworn to protect [player]... And still, they died... What a good for nothing bodyguard am I...*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] opened a mysterious temple at [subject] recently. I wonder what they found inside.*";
                case MessageIDs.FeatCoinPortal:
                    return "*Sometimes I think I should charge by the days of protection, even more after [player] found a portal that rained coins.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I have made a contract with [player] too. They are also under my protection, so don't try anything hostile towards them.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*I think that instead of [player] helping that odious kid, they could have kept the fish for themself. What they would do with it? I don't know, maybe place in a fishbowl?*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*Even though [player] got the giant creature killed in [subject], doesn't means my job is done. I will keep protecting their life as if was my own.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*I think my job just got harder on [subject]. [player] managed to make menacing creatures surge.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*It's good to hear that [player] got someone to look for, and also to look for himself. I think [subject] by their side will help ensure their safety on their adventure.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*Since you picked me as your buddy, that means all my focus will be on your safety. I only ask you to take me to a tavern sometimes, we could use some drinks sometimes.*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*I heard that [subject] sometimes spends their time on [world].*";
                case MessageIDs.DeliveryGiveItem:
                    return "*[target], I have some spare [item]. Take them.*";
                case MessageIDs.DeliveryItemMissing:
                    return "*Weird. Did my inventory changed?*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*It seems like [target] is carrying too much things.*";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "*I shall lead a group towards victory then.*";
                case MessageIDs.CommandingLeadGroupFail:
                    return "*I don't want to lead a group right now. The safety of our citizens is my highest priority right now.*";
                case MessageIDs.CommandingDisbandGroup:
                    return "*Understud. Everyone, take a well earned rest! You deserved it.*";
                case MessageIDs.CommandingChangeOrder:
                    return "*I shall do as you command.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
		
		public void Dialogue()
		{
			
		}
    }
}
