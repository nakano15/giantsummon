using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures
{
    public class SardineBase : GuardianBase
    {
        public const string CaitSithOutfitID = "cs_outfit";

        /// <summary>
        /// -Cheerful and Friendly.
        /// -Warrior. Literally.
        /// -Adventurer.
        /// -Bree's Husband.
        /// -Bullied by Blue and Zacks.
        /// </summary>
        public SardineBase()
        {
            Name = "Sardine";
            Description = "He's an adventurer that has visited many worlds,\nearns his life as a bounty hunter. But actually forgot\nwhich world his house is at.";
            Size = GuardianSize.Small;
            Width = 14;
            Height = 38;
            SpriteWidth = 72;
            SpriteHeight = 56;
            FramesInRows = 25;
            //DuckingHeight = 54;
            Age = 6;
            Male = true;
            InitialMHP = 80; //320
            LifeCrystalHPBonus = 12;
            LifeFruitHPBonus = 3;
            Accuracy = 0.52f;
            Mass = 0.33f;
            MaxSpeed = 4.82f;
            Acceleration = 0.15f;
            SlowDown = 0.5f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            SetTerraGuardian();
            DodgeRate = 40;
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit51);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath54);

            PopularityContestsWon = 0;
            ContestSecondPlace = 1;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.SilverBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.Shuriken, 250);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 13, 14, 15, 16 };
            SittingFrame = 17;
            ChairSittingFrame = 18;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 12, 13, 14, 15, 16 });
            ThroneSittingFrame = 19;
            BedSleepingFrame = 20;
            DownedFrame = 21;
            ReviveFrame = 22;
            PetrifiedFrame = 23;

            SleepingOffset.X = -2;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);

            //Left Hand
            LeftHandPoints.AddFramePoint2x(10, 10, 12);
            LeftHandPoints.AddFramePoint2x(11, 27, 14);
            LeftHandPoints.AddFramePoint2x(12, 31, 26);

            LeftHandPoints.AddFramePoint2x(13, 12, 9);
            LeftHandPoints.AddFramePoint2x(14, 23, 12);
            LeftHandPoints.AddFramePoint2x(15, 24, 18);
            LeftHandPoints.AddFramePoint2x(16, 21, 23);

            LeftHandPoints.AddFramePoint2x(22, 21, 23);

            //Right Hand
            RightHandPoints.AddFramePoint2x(10, 12, 12);
            RightHandPoints.AddFramePoint2x(11, 29, 14);
            RightHandPoints.AddFramePoint2x(12, 33, 26);

            RightHandPoints.AddFramePoint2x(13, 14, 9);
            RightHandPoints.AddFramePoint2x(14, 25, 12);
            RightHandPoints.AddFramePoint2x(15, 26, 18);
            RightHandPoints.AddFramePoint2x(16, 23, 23);

            //Mount
            MountShoulderPoints.DefaultCoordinate = new Point(16 * 2, 25 * 2);
            SittingPoint = new Point(16 * 2, 25 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(16, 13);
            HeadVanityPosition.AddFramePoint2x(11, 22, 16);
            HeadVanityPosition.AddFramePoint2x(12, 28, 24);

            HeadVanityPosition.AddFramePoint2x(19, 16, 13 - 7);

            HeadVanityPosition.AddFramePoint2x(22, 16, 15);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(16, 19);

            GetRequests();
            GetRewards();
            LoadSkins();
        }

        public void LoadSkins()
        {
            AddOutfit(1, "Cait Sith", delegate(GuardianData gd, Player player)
            {
                return gd.HasPersonalRequestBeenCompleted(0);
            }, true);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(CaitSithOutfitID, "caitsith_outfit");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
                switch (guardian.Data.OutfitID)
                {
                    case 1:
                        {
                            Texture2D texture = sprites.GetExtraTexture(CaitSithOutfitID);
                            int BodyFramePosition = -1, RightArmPosition = -1, HeadEquipPosition = -1, BodyFrontPosition = -1;
                            bool HasVanityItem = TerraGuardian.HeadSlot > 0;
                            for (int t = 0; t < TerraGuardian.DrawBehind.Count; t++)
                            {
                                if (TerraGuardian.DrawBehind[t].textureType == GuardianDrawData.TextureType.TGRightArm)
                                {
                                    RightArmPosition = t;
                                }
                                if (TerraGuardian.DrawBehind[t].textureType == GuardianDrawData.TextureType.TGBody)
                                {
                                    BodyFramePosition = t;
                                }
                                if (TerraGuardian.DrawBehind[t].textureType == GuardianDrawData.TextureType.TGHeadAccessory)
                                {
                                    HeadEquipPosition = t;
                                }
                            }
                            for (int t = 0; t < TerraGuardian.DrawFront.Count; t++)
                            {
                                if (TerraGuardian.DrawFront[t].textureType == GuardianDrawData.TextureType.TGBodyFront)
                                {
                                    BodyFrontPosition = t;
                                }
                            }
                            if (BodyFramePosition > -1)
                            {
                                Rectangle bodyrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                                bodyrect.Height += 16;
                                DrawPosition.Y -= 16;
                                GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                if (RightArmPosition > -1)
                                {
                                    //if (!guardian.MountedOnPlayer || guardian.Downed) dd.IgnorePlayerRotation = true;
                                    guardian.InsertDrawData(dd, RightArmPosition, false);
                                    //TerraGuardian.DrawBehind.Insert(RightArmPosition, dd);
                                }
                                BodyFramePosition += 2;
                                bodyrect.Y += bodyrect.Height;
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(dd, BodyFramePosition, false);
                                BodyFramePosition++;
                                bodyrect.Y += bodyrect.Height;
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(dd, BodyFramePosition, false);
                                BodyFramePosition++;
                                bodyrect.Y += bodyrect.Height;
                                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(dd, BodyFramePosition, false);
                                BodyFramePosition++;
                                bodyrect.Y += bodyrect.Height;
                                if (!HasVanityItem)
                                {
                                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                    if (HeadEquipPosition > -1)
                                    {
                                        guardian.AddDrawDataAfter(dd, HeadEquipPosition, false);
                                    }
                                    else
                                    {
                                        guardian.AddDrawDataAfter(dd, BodyFramePosition, false);
                                        BodyFramePosition++;
                                    }
                                }
                                if (BodyFrontPosition > -1)
                                {
                                    bodyrect.Y += bodyrect.Height;
                                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, DrawPosition, bodyrect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(dd, BodyFrontPosition, true);
                                }
                            }
                        }
                        break;
                }
        }

        public void GetRequests()
        {
            //Add a request to kill the King Slime.
            AddNewRequest("Bash the King!", 410, 
                "I didn't liked the result of my last fight against the King Slime, and I think there's still Gel inside my ears. I wonder If we could try doing the fight again, but this time doing it right. What do you think?",
                "Amazing! Just tell me when you're ready, and I will call It.",
                "What? I can't hear you! Must be the Gel. Did you deny?",
                "That is how It is done! And It was really good to face It from outside, too. I feel that now I have the right to use this outfit, the Travelling Merchant said that he got It from a Post Apocalyptic Futuristic Tokyo. I don't even have any idea of what Tokyo is, but the outfit seems to look good on me. Let's check It out?",
                "Just tell me when you want me to call It. Let's prepare ourselves first before trying.");
            AddRequestRequirement(delegate(Player player)
            {
                return NPC.downedSlimeKing;
            });
            AddKillBossRequest(Terraria.ID.NPCID.KingSlime, 1);
            //
            //AddNewRequest("", 200,
            //    "");
        }

        public void GetRewards()
        {

        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> Options = base.GetGuardianExtraDialogueActions(guardian); //It's empty, anyways.
            Options.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption(!GuardianBountyQuest.SardineTalkedToAboutBountyQuests ? "About Bounties" : "Report Bounty", BountyQuestProgressCheckButtonAction));
            return Options;
        }

        public void BountyQuestProgressCheckButtonAction(TerraGuardian tg)
        {
            if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
            {
                GuardianMouseOverAndDialogueInterface.SetDialogue("Since It's so boring staying at home, I decided to do something in to make me busy. I will open a Bounty Hunting agency here, but first I need a Sign inside my house. If you end up placing one, I will place the latest bounty here.");
                GuardianBountyQuest.SardineTalkedToAboutBountyQuests = true;
            }
            else
            {
                if (GuardianBountyQuest.SignID == -1)
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("I can see that you are eager to get requests, but I need a sign in my house first.");
                }
                else if (GuardianBountyQuest.PlayerAlreadyRedeemedReward(Main.player[Main.myPlayer]))
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("I don't have anything else for you. Wait until another bounty shows up.");
                }
                else if (GuardianBountyQuest.PlayerRedeemReward(Main.player[Main.myPlayer]))
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("Nice job! If I were there with you, you'd take half the time facing it, but whatever.");
                }
                else
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        GuardianMouseOverAndDialogueInterface.SetDialogue("The bounty target appears in the " + GuardianBountyQuest.spawnBiome.ToString() + ", cause a mayhem on it until It shows up.");
                    }
                    else
                    {
                        GuardianMouseOverAndDialogueInterface.SetDialogue("Beware when facing the target, It is not just a regular monster.");
                    }
                }
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "I will get rusty If I stay locked into my house. Take me on your adventures too, I like loot too!";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "Hey pal, my feet are kind of sore, would you mind If I ride on your back? Don't worry, I can still fight meanwhile.";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "If you need someone fast for some dangerous thing, don't think twice about sending me there.";
            }
        }

        public override string FriendLevelMessage
        {
            get
            {
               return "Hey, let's find some monsters to kill, friend!";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "If I even manage to find something cool, I will give it to you.";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "There are so many things to find in this world. With you leading the way, I guess we will end up rich!";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "There is nobody I would go adventuring with other than you. You are a great partner!";
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeSpeed += 0.15f;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Hey! I nearly killed that King Slime. Oh well, nevermind...";
            return "Tarararan-Taran! Meet the worlds biggest smallest bounty hunter ever! Me!";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.dayTime)
            {
                Mes.Add("Why female humans keep wanting to try scratching the back of my head?");
                Mes.Add("This place surelly is livelly, but I'd rather go out and beat some creatures.");
            }
            else
            {
                Mes.Add("I'm so sleepy, do you know of any window I could be at?");
                Mes.Add("Looks like a perfect moment to explore, even more with my night eyes.");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("Do you know what time it is? It's fun time! Let's go outside and beat some ugly creatures!");
            }
            if (Main.raining)
            {
                Mes.Add("I hate this weather, but at least gives me good reason to stay at home.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Where did those weird creatures came from?");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("Do you have some spare medicine? [gn:1] seems to be wanting to play with me again...");
                Mes.Add("I have tried to outrun [gn:1] so I don't play that stupid game, but she's faster than me on 4 legs.");
                Mes.Add("If you see [gn:1], say that you didn't see me. I'm tired of playing \"Cat and the Wolf\" with her, I didn't even knew that game existed, and my body is still wounded because of her teeth from the last game.");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("I really don't want to play with [gn:3], I could even run away from him, but everytime I do so, he pulls me back with... Whatever is that thing! It's smelly and yuck!");
                Mes.Add("I really hate when [gn:3] plays his game with me, everytime he acts like as if was devouring my brain I feel like my heart was going to jump out of my mouth.");
                Mes.Add("Eugh, [gn:3] \"played\" a game with me, and now I'm not only bitten on many places, but also with smelly sticky stuffs around. Wait, will I turn into a Zombie because of that?! Should I begin panicking?");
                Mes.Add("I want to remove all that stinky stuff i've got from being bullied by [gn:3] from my fur, but I don't even know what is that, so I can't really lick it away. Maybe I should... *Gulp* Take a bath? With water?");
            }
            if (NpcMod.HasGuardianNPC(1) && NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("You want to know what is worse than a wolf playing \"Cat and Wolf\" with you? Two wolves!!! And one is a Zombie!!");
                Mes.Add("First she invented that \"Cat and Wolf\" game, now that creepy [gn:3] invented the \"The Walking Guardian\" game. Why does they love bullying me? Is that a wolf thing?");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic) && NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("Everytime I chat with [nn:" + Terraria.ID.NPCID.Mechanic + "], she is always in good mood and happy. On other hand, [nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] stares at me with a killer face. Maybe I should start sharpening my knife, meow?");
            }
            if(PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("Hey [gn:0], want to play some \"Hide and Seek\"? Don't take me wrong, I like games, depending on them...");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("Hello, I- Waaaaaah!!! *He ran away, as fast as he could.*");
                Mes.Add("No! Go away! I don't want to play with you, I don't even want to see you, I.. I... Have some important things to do, I mean... Somewhere veeeeeeery far away from you!");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
            {
                Mes.Add("Yikes! Go away! Your \"game\" spooks me out so hard.");
                Mes.Add("No way, not again. *He ran away*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("I really feel bad about [nn:" + Terraria.ID.NPCID.Angler + "], he'll never know my trick for catching more than one fish. Neither you will do.");
            if (NPC.downedBoss1)
                Mes.Add("So, you have defeated the Eye of Cthulhu, right? Psh, Easy. Do you ever wanted to know who killed Cthulhu? Well, It was me. Hey, what are you laughing at?");
            if (NPC.downedBoss3)
                Mes.Add("My next bounty is set to the Skeletron, at the Dungeon Entrance. Let's go face him!");
            if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
            {
                Mes.Add("Hey, do you have a minute? I want to discuss with you about my newest bounty hunting service.");
            }
            else
            {
                if (GuardianBountyQuest.SignID == -1)
                    Mes.Add("I need a sign in my house to be able to place my bounties in it. I only need one, no more than that. If It's an Announcement Box, will be even better.");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("Playing with [gn:5] is really fun. But he has a little problem to know what \"enough\" is.");
                Mes.Add("One of the best things that ever happened was when you brought [gn:5] here, at least playing with him doesn't hurts or cause wounds... Most of the time.");
            }
            if (NpcMod.HasGuardianNPC(7))
            {
                Mes.Add("I really love [gn:7], but she keeps controlling me. She could at least give me more freedom, It's not like as if I would run or something.");
                Mes.Add("[gn:7] was the most lovely and cheerful person I've ever met, but for some reason, she started to get grumpy after we married. What happened?");
                Mes.Add("Even though [gn:7] tries to hog all my attention to her, I still love her.");
                Mes.Add("I wonder, does [gn:7] carrying that bag all day wont do bad for her back?");
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("Woah, you should have seen [gn:7] fighting with [gn:1] earlier. That remembered me of the day we met.");
                if (NpcMod.HasGuardianNPC(3))
                    Mes.Add("Ever since [gn:7] saw [gn:3] playing that stupid hateful game with me, she has been asking frequently If I'm fine, and If I wont... Turn? What is that supposed to mean?");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("I love having [gn:8] around, but she asks me to do too many things. It's a bit tiring. Refuse? Are you nuts? Did you look at her?!");
                Mes.Add("I have a favor to ask of you. If you see me staring at [gn:8] with a goof face, slap my face.");
                if (NpcMod.HasGuardianNPC(7))
                {
                    Mes.Add("[gn:7] keeps asking me if there is something happening between me and [gn:8]. No matter how many times I say no, she still remains furious.");
                }
            }
            if (NpcMod.HasGuardianNPC(Domino))
            {
                Mes.Add("Lies! I'm not buying catnip! Where did you brought that idea from?");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("Hey, you know that guy, [gn:"+Vladimir+"]? He's really helping me with some personal matters. If you feel troubled, have a talk with him.");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Michelle))
            {
                Mes.Add("Whenever I tell stories about my adventures, [gn:" + GuardianBase.Michelle + "] listen attentiously to every details of it. I think I got a fan.");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Fluffles))
            {
                bool HasBlue = NpcMod.HasGuardianNPC(GuardianBase.Blue),
                    HasZacks = NpcMod.HasGuardianNPC(GuardianBase.Zacks);
                if (HasBlue && HasZacks)
                {
                    Mes.Add("Great, now I have a narcisistic wolf, a rotting wolf and a hair rising ghost fox trying to have a piece of me. You have some kind of grudge against me?");
                    Mes.Add("If you see [gn:"+GuardianBase.Blue+"], [gn:"+GuardianBase.Zacks+"] and [gn:"+GuardianBase.Fluffles+"] looking like they are biting something on the floor, that must be me.");
                }
                else if (HasBlue)
                {
                    Mes.Add("I really hate having [gn:" + GuardianBase.Fluffles + "] around, because there are now TWO to bite me on [gn:" + GuardianBase.Blue + "]'s stupid game.");
                    Mes.Add(".");
                }
                Mes.Add("I have to tell you what happened to me the other day. I was on the toilet doing my things, having a hard time, until [gn:"+GuardianBase.Fluffles+"] surged from nowhere. She spooked me really hard! But at least solved my constipation issue.");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("Do you humans always visits bathrooms when others are using it?");
                Mes.Add("I'm trying to concentrate here, If you excuse me.");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("It's really good to have a room mate, I wonder if we could play table top games before sleeping.");
                Mes.Add("Having a room mate is so cool.");
                if (NpcMod.HasGuardianNPC(GuardianBase.Bree))
                {
                    Mes.Add("I like sharing the room with you, but sometimes I think I should share It with [gn:"+Bree+"], instead.");
                }
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, GuardianBase.Bree))
            {
                Mes.Add("Uh... So... Maybe [gn:"+Bree+"] should be sharing her room with me?");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("(He's trying to lick his wound.)");
                    Mes.Add("(You see him struggling on the floor, something must have happened to his arm.)");
                    Mes.Add("(You notice him breathing from his mouth.)");
                }
                else
                {
                    Mes.Add("(He's unconscious, you tried shaking him but he didn't woke up.)");
                }
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("Ha! Take that! And that! (He must be sleeping about facing some creature.)");
                Mes.Add("(He's sleeping happily)");
                Mes.Add("Yuck! Go away! Get off me! (He must be having nightmares with the King Slime)");
                if (NpcMod.HasMetGuardian(Bree))
                {
                    Mes.Add("I'm home! I brought all those treasures for us. (He must be dreaming about returning home with [gn:" + Bree + "].)");
                }
                else
                {
                    Mes.Add("I'm home! I brought all those treasures for us. (He must be dreaming about returning home For someone.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I don't really need anything right now. All that I want is to beat some monsters.";
            return "Hum, nothing right now. Later, maybe?";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "I feel weird for asking this but... I need your help with a particular something...";
            return "I'm not really a fan of asking for help, but I really need help for this.";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "You're the best, did you knew? Of course you knew!";
            return "I knew you would be able to help me with my little request. Here a token of my affection.";
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            if (Main.raining && Main.rand.NextDouble() < 0.5)
                return "Please, give me a house, very please! I'm soaked and cold out here!";
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "So, you have a place with several humans walking around like complete goofs? May I move in, too?";
                case 1:
                    return "Hey, I'm kind of far from my home, could you give me some place to live meanwhile?";
                case 2:
                    return "You may ask for my help anytime you want, but I'd like to have some place to stay until then.";
            }
            return base.HomelessMessage(player, guardian);
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hey, are you interessed into going on a treasure hunting? Haha, I was just wanting to start a chat, If I had an idea of hidden treasure, I'd already have got it.");
            Mes.Add("Say, how many worlds have you visited? Can you count it on your toes? Because I have visited too many worlds.");
            Mes.Add("What is the point of an Angel Statue? Not even rigging them with wire does anything.");
            Mes.Add("I know a cool world we could visit, maybe one day I'll bring you there.");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("Do you think that we will bump on my house during our travels? Beside I don't really remember how it looked like...");
            }
            else
            {
                Mes.Add("I'm starting to get rusty from all this standing around, let's go on an adventure!");
                Mes.Add("Uh, I'm a little short on coins right now, let's go farm for some?");
            }
            if (!Main.dayTime)
            {
                Mes.Add("I'm getting soooooo sleepy... Oh! I'm awake. I'm awake.");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("Hey [gn:0], want to play some Dodgeball?");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
                Mes.Add("What? No! No Way! Go away! I don't want to play some more of that painful game.");
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("May not look like it, but [gn:1] has very sharp teeth, don't ask how I found out that... Ouch...");
                Mes.Add("Sometimes I think that [gn:1] uses that \"game\" of her just to bully me.");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("I have to say, from all the things that could haunt me in my life, [gn:3] had to happen? He's even my neighbor!!");
                Mes.Add("I don't really think that [gn:3] is a bad guy, but I really hate playing that game of his. Even If I deny he plays it with me. I just can't run away, since he pulls me back using his... Whatever is that thing.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] isn't that plumberer, but looks with that exact same death stare when he sees me.");
            }
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("[gn:0] may be stupid and childish, but I really like talking to him.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 2) && Main.rand.NextDouble() < 0.5)
                return "When will be the moment I'll be getting my gifts?";
            return "You guys prepared this party for me? Wow! You guys are awesome!";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer)
            {
                Mes.Add("It wont result into a good story laying down on the ground.");
                Mes.Add("Come on, we have more adventures to make!");
                Mes.Add("You'll be okay, your adventure isn't over.");
            }
            else
            {
                bool GotAMessage = false;
                if (ReviveGuardian.ModID == Guardian.ModID)
                {
                    GotAMessage = true;
                    switch (ReviveGuardian.ID)
                    {
                        default:
                            GotAMessage = false;
                            break;
                        case GuardianBase.Blue:
                            {
                                Mes.Add("I think I will regret this...");
                                Mes.Add("I wonder, helping her right now, will make her stop bullying me?");
                                Mes.Add("Look at those teeth... Wait, better I look somewhere else, I may lose motivation.");
                            }
                            break;
                        case GuardianBase.Zacks:
                            {
                                Mes.Add("How am I supposed to heal him? His entire body has problems.");
                                Mes.Add("I'm having flashbacks... Don't think about them...");
                                Mes.Add("My heart is racing whenever I get near him. It's scary.");
                            }
                            break;
                        case GuardianBase.Bree:
                            {
                                Mes.Add(ReviveGuardian.Name + " wake up! Please wake up!");
                                Mes.Add("I never wanted to place you in danger, don't make me feel guilty now.");
                                Mes.Add("Please open your eyes! Say something! Insult me! Anything! " + ReviveGuardian.Name + "!!");
                            }
                            break;
                    }
                }
                if (!GotAMessage)
                {
                    Mes.Add("Don't worry, I'll help you!");
                    Mes.Add("You'll be 100% soon.");
                    Mes.Add("I'll take care of those wounds, no worries.");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "I'm glad you called, It was fun trying to carry ou here.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "It's really late, couldn't you speak with me during the day?";
                        case 1:
                            return "I need to recharge my energies for my adventures, buddy. Could you make It quick? I want to get back to sleep.";
                        case 2:
                            return "Aww man... Couldn't you wait until the sun rises?";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "What? Request is done? I'm happy for that but... Couldn't wait?";
                        case 1:
                            return "Woah! Oh, It's you. Did you do my request?";
                    }
                    break;
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
