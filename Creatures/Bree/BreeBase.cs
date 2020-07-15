﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class BreeBase : GuardianBase
    {
        /// <summary>
        /// -A bit grumpy.
        /// -Sardine's Wife.
        /// -Feels lonely.
        /// -Wants to go back home.
        /// -Still loyal to Sardine.
        /// -Fears Zacks.
        /// -Protects Sardine from Blue.
        /// </summary>

        public BreeBase()
        {
            Name = "Bree";
            Description = "Her trek begun after her husband has disappeared,\neven after she find him, she might stay for a while,\nuntil she remembers which world they lived on.";
            Size = GuardianSize.Small;
            Width = 16;
            Height = 46;
            SpriteWidth = 64;
            SpriteHeight = 64;
            FramesInRows = 21;
            //DuckingHeight = 54;
            Age = 5;
            Male = false;
            InitialMHP = 85; //375
            LifeCrystalHPBonus = 14;
            LifeFruitHPBonus = 4;
            Accuracy = 0.73f;
            Mass = 0.46f;
            MaxSpeed = 4.76f;
            Acceleration = 0.14f;
            SlowDown = 0.6f;
            MaxJumpHeight = 14;
            JumpSpeed = 9.88f;
            CanDuck = false;
            ReverseMount = true;
            SetTerraGuardian();
            DontUseHeavyWeapons = true;
            DodgeRate = 35;
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit51);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath54);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 2;

            AddInitialItem(Terraria.ID.ItemID.PlatinumBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.FlintlockPistol, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Friendship Levels
            KnownLevel = 4;
            FriendsLevel = 7;
            BestFriendLevel = 13;
            BestFriendForeverLevel = 19;
            BuddiesForLife = 28;
            //
            CallUnlockLevel = 2;
            LootingUnlockLevel = 4;
            MaySellYourLoot = 4;
            MountUnlockLevel = 4;
            StopMindingAFK = 8;
            MountDamageReductionLevel = 11;
            ControlUnlockLevel = 14;
            FriendshipBondUnlockLevel = 15;
            FallDamageReductionLevel = 18;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 14;
            ChairSittingFrame = 15;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 12, 16, 17, 18, 19 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            ReviveFrame = 18;
            DownedFrame = 19;
            PetrifiedFrame = 20;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 14);
            LeftHandPoints.AddFramePoint2x(11, 17, 17);
            LeftHandPoints.AddFramePoint2x(12, 21, 23);
            LeftHandPoints.AddFramePoint2x(13, 19, 26);

            LeftHandPoints.AddFramePoint2x(13, 19, 29);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 14, 14);
            RightHandPoints.AddFramePoint2x(11, 21, 17);
            RightHandPoints.AddFramePoint2x(12, 25, 23);
            RightHandPoints.AddFramePoint2x(13, 23, 26);

            //Mount Sitting Point
            SittingPoint = new Point(15 * 2, 29 * 2);
            MountShoulderPoints.DefaultCoordinate = SittingPoint;

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(16, 20);
            HeadVanityPosition.AddFramePoint2x(16, 14, 13);

            HeadVanityPosition.AddFramePoint2x(18, 16, 22);

            RequestList();
            LoadSkinList();
        }

        public void LoadSkinList()
        {
            AddSkin(1, "Bagless", delegate(GuardianData gd, Player player)
            {
                return gd.HasPersonalRequestBeenCompleted(0);
            });
        }

        public void RequestList()
        {
            AddNewRequest("Stay", 400, "You people really nag me to stay for longer here. I'll make a deal, catch me quite a number of fish, and I'll stay.",
                "Before you go, keep in mind that must not be just any fish. It must be the most delicious fish in my taste. Yes, I'm talking about the Double Cod. Now go, before I change my mind.",
                "You don't really want me to stay, right? I didn't wanted to stay, anyway.",
                "You managed to do that? Alright, I can put down my things in my house and stay for longer. My back was beggining to ache, anyway.",
                "You don't know where to find a Double Cod? The Jungle is where you should go!");
            AddItemCollectionObjective(Terraria.ID.ItemID.DoubleCod, 15, 0);
            AddRequestRequirement(RequestBase.GetFishingRodRequirement);
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0f, float Value4 = 0f, float Value5 = 0f)
        {
            if (trigger == TriggerTypes.NpcSpotted)
            {
                NPC npc = Main.npc[Value];
                if (npc.type == Terraria.ID.NPCID.KingSlime && NpcMod.TrappedCatKingSlime == npc.whoAmI)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            guardian.SaySomething("I can't believe! We have to help him!", true);
                            break;
                        case 1:
                            guardian.SaySomething("Well, we found him. Now we need to get him out of there.", true);
                            break;
                        case 2:
                            guardian.SaySomething("Oh no! We have to do something, or else that giant slime will lunch my husband!", true);
                            break;
                    }
                }
            }
            if (trigger == TriggerTypes.NpcDies)
            {
                NPC npc = Main.npc[Value];
                if (npc.type == Terraria.ID.NPCID.KingSlime && NpcMod.TrappedCatKingSlime == npc.whoAmI)
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            guardian.SaySomething("Whew... He's safe... Let's check him out.", true);
                            break;
                        case 1:
                            guardian.SaySomething("Whew, he's safe! At least he looks okay in blue color.", true);
                            break;
                        case 2:
                            guardian.SaySomething("We saved him! Let's go see if he's okay.", true);
                            break;
                    }
                    guardian.IncreaseFriendshipProgress(1);
                }
            }
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture("bagless_body", "body_no_bag");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            switch (guardian.Data.SkinID) //Todo - Add some way of activating skins.
            {
                case 1:
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                        {
                            gdd.Texture = sprites.GetExtraTexture("bagless_body");
                        }
                    }
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                        {
                            gdd.Texture = sprites.GetExtraTexture("bagless_body");
                        }
                    }
                    break;
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 2))
                {
                    return "IWould you mind if I accompany my husband on your quest? In case he does something stupid, I mean.";
                }
                else if (PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], 2))
                {
                    return "I want to ask you, would you mind if I accompany you? A dame needs to take a walk sometimes, but I don't really know this world, or have any reason to explore it.";
                }
                else
                {
                    return "I want to ask you, would you mind If I accompany you? You may end up bumping into my husband during your travels, so I want to be there, so I can pull his ear back home.";
                }
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "Say, would you mind if I mount on your back? This bag is weightening my feet, and they are hurting.";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "Don't make me regret saying this, but you can send me to some places you can't go. Period.";
            }
        }

        public override string FriendLevelMessage
        {
            get
            {
                return "I miss my house so much... I wonder how my son is doing...";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "The neighborhood here is great, but... I'm more used o quieter places...";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "Say... Would you mind if I... No. No... I have to go home... My son...";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "I... I... Have to go home... I have... But... This place... *She started crying*";
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.DefenseRate += 0.05f;
            g.Defense += 5;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "Don't you go thinking I'll stay here for too long. It's just temporary.";
                case 1:
                    return "Who are you? Did you see my husband somewhere?";
                case 2:
                    return "Ugh, I need some place to put off some steam.";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("A year after that imbecile I call husband went out on one of his \"adventures\", I started searching for him.");
            }
            Mes.Add("The floor is awful, nobody cleans this place? Looks like I'll have to clean this place.");
            Mes.Add("The people in your town are nice, but I preffer a quiet and less noisy place.");
            Mes.Add("I wont place my things on the floor, soon I'll be going back home. I just need to remember which world I lived.");
            Mes.Add("[gn:2] and I have a son, he's currently at home. He's old enough to take care of himself, but he's probably missing us.");
            Mes.Add("At first, this bag was being quite heavy on my shoulders. As I kept using it, started to feel ligher. Did I grow stronger?");
            Mes.Add("Most of the time I'm busy cleaning up the place, looks like nobody else does.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
                Mes.Add("I tried asking [nn:" + Terraria.ID.NPCID.Dryad + "] for clues of which world I lived. She said that she also visited several worlds, so can't pinpoint places. I should be so lucky...");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("[nn:" + Terraria.ID.NPCID.Merchant + "] disappoints me everytime I check his store. He should improve his store stock.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                Mes.Add("[nn:" + Terraria.ID.NPCID.ArmsDealer + "] should be ashamed of selling such outdated guns.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("Do you want to hear a joke? [nn:" + Terraria.ID.NPCID.Angler + "] doesn't knows how to catch two fishs at once, can you believe? Wait, you don't either? You must be kidding!");
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("I really love having [gn:0] in the town, I can ask him to do things without questioning.");
                Mes.Add("Everytime [gn:0] asks If I want to play some kids game, I ask him what is his age. That creates a delay of a day before he asks me again.");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("Sometimes I try getting beauty tips from [gn:1]. She seems to be expert on that.");
                if (NpcMod.HasGuardianNPC(2))
                    Mes.Add("If I ever see [gn:1] bullying my husband again, she will regret!");
                Mes.Add("Looks like [gn:1] and I had the same objective, but the result...");
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                    Mes.Add("Have you passed through [nn:" + Terraria.ID.NPCID.Stylist + "]'s shop? I'm waiting about 4 hours for her to finish [gn:1]'s hair treatment so I can start mine.");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("Once I remember which world I lived, I'm taking [gn:2] back with me.");
                Mes.Add("I used to be happy and cheerful, until [gn:2] happened. I should have heard my mom.");
                Mes.Add("I once say [gn:2] kill a giant monster alone, by using a Katana. I was so amazed with it, that I fell for him. Big mistake I did.");
                Mes.Add("Soon, [gn:2] and I will go back home and try to restart our life. Soon...");
                Mes.Add("Have you seen [gn:2]? He's probably doing something stupid.");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("Would you please tell [gn:3] to stay away from me? He creeps me out.");
                if (NpcMod.HasGuardianNPC(2))
                    Mes.Add("I have to tell you something! I went outside for a walk, and I saw [gn:3] pulling my husband, and then biting him! BITING, HIM! I ran back home after that, and then suddenly, I saw my husband covered in some sticky goo complaining about something. Is he alright? Is [gn:2] going to be alright?! Wait, AM I EVEN SAFE HERE?!");
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("Wait, you're telling me that [gn:3] is [gn:1]'s boyfriend? She did one weird choice.");
            }
            if (NpcMod.HasGuardianNPC(4))
            {
                Mes.Add("I can't really tell much about [gn:4], he doesn't say much, either.");
                Mes.Add("Sometimes I see [gn:4] starting at the dungeon entrance. I wonder what is on his mind.");
                Mes.Add("[gn:4] seems to have only one emotion. -_-");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("Sometimes [gn:5] makes me company. I love it when he lies down next to me while I'm doing somethings. I feel less alone.");
                Mes.Add("I'm not very fond of dogs, but [gn:5] is an exception. I guess I should thank his old owner for that.");
                Mes.Add("Sometimes I see [gn:5] staring at the moon. What could be coming on his mind?");
            }
            if (NpcMod.HasGuardianNPC(6))
            {
                Mes.Add("[gn:6] keeps bragging how strong he is, until I challenged him on a arm wrestling.");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("Sometimes I think that [gn:6] should get a haircut from [nn:" + Terraria.ID.NPCID.Stylist + "], at least would be better than that thing he has on his head.");
                }
                Mes.Add("I have some drinks with [gn:6] sometimes, he has some funny stories from the Ether World, like when a magician apprentice put fire on the king's robe during a celebration.");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("I know that [gn:2] spends way too much time with [gn:8], I hope that cat doesn't plan to cheat on me.");
                }
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Michelle))
            {
                Mes.Add("If you want to make me feel annoyed, just leave me 5 minutes with [gn:" + GuardianBase.Michelle + "] in the same room.");
                Mes.Add("I hate [gn:" + GuardianBase.Michelle + "], she just don't stop talking!");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("Eek!! Turn the other side!");
                Mes.Add("Do you really have to enter here and talk to me while I'm using the toilet?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Just no. Not right now.";
            return "Humph, no.";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Even though I'm away from home, I still need some things done, but I'm busy right now. So... I ask you...";
            return "I hope you are more reliable than my husband. I need a thing to be done.";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "That doesn't mean I'll give you a star of good person. But... You're nice.";
            return "Maybe that will make me be less furious.";
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Would be nice to have some comodity in this world. I'm in desperate need of a hot bath.");
            Mes.Add("Do you have any housing you guys stays, or do you live in the open?");
            Mes.Add("Don't worry, I can clean the house you give, I wont stay for long anyway.");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
                Mes.Add("Since you gave [gn:2] a house, what about giving me one too?");
            if (Main.raining)
                Mes.Add("I am wet and cold. Do you want me to continue? *She's showing a angry and frightening face!!*");
            if(Main.bloodMoon)
                Mes.Add("This night is melting my mood so hard that I could rip someone's face off! At least I have enough oportunities to do so.");
            if (!Main.dayTime)
                Mes.Add("Don't you have some place for me to stay? It's not good for a lady to stay alone in the dark like this.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("Do you want to know how to anger [gn:1]? Easy, throw a bucket of water on her hair. Now, do you know how much long it takes for her anger to pass?");
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("Sometimes I don't know if [gn:2] even cares about me. It's like, his adventures are the top priority.");
                Mes.Add("I don't entirelly hate [gn:2], but what he has done isn't okay. Beside I shouldn't throw a stone, either.");
            }
            Mes.Add("Maybe you can help me remember which world I came from. It had a grass land, then there were that evil land, It also had a dungeon, and a jungle... All worlds have those? Oh...");
            Mes.Add("Sometimes I like to rest on a window.");
            Mes.Add("I like chasing butterflies, but they always escape.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 7) && Main.rand.NextDouble() < 0.5)
                return "You've got a gift for me? Well, let me see what is inside it.";
            return "Is It okay If I wish to return home with my husband?";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID && ReviveGuardian.ID == GuardianBase.Sardine)
            {
                Mes.Add("Wait! Come on! Wake up! Don't leave me again!");
                Mes.Add("Please, don't die! It took me a year to find you again! Your son is even waiting for you at home!");
                Mes.Add("Open your eyes! Look at me! Please!");
            }
            else
            {
                Mes.Add("You're safe... I'm here with you...");
                Mes.Add("Here, this will make you feel better.");
                Mes.Add("Shh... You'll be fine. Just rest.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
