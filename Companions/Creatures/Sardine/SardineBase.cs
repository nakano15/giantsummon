﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Companions
{
    public class SardineBase : GuardianBase
    {
        public const string CaitSithTextureID = "cs_outfit";
        public const int CaitSithOutfitID = 1;

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
            Scale = 34f / 38;
            CompanionSlotWeight = 0.8f;
            DefaultTactic = CombatTactic.Charge;
            FramesInRows = 26;
            //DuckingHeight = 54;
            Age = 25; //6
            SetBirthday(SEASON_SUMMER, 8);
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
            GroupID = TerraGuardianCaitSithGroupID;
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
            SittingFrame = PlayerMountedArmAnimation = 17;
            ChairSittingFrame = 18;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 12, 13, 14, 15, 16 });
            ThroneSittingFrame = 19;
            BedSleepingFrame = 20;
            DownedFrame = 21;
            ReviveFrame = 22;
            PetrifiedFrame = 23;

            BackwardStanding = 24;
            BackwardRevive = 25;

            SleepingOffset.X = -2;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);

            //Left Hand
            LeftHandPoints.AddFramePoint2x(10, 10, 12);
            LeftHandPoints.AddFramePoint2x(11, 27, 14);
            LeftHandPoints.AddFramePoint2x(12, 31, 26);

            LeftHandPoints.AddFramePoint2x(13, 12, 9);
            LeftHandPoints.AddFramePoint2x(14, 22, 12);
            LeftHandPoints.AddFramePoint2x(15, 25, 18);
            LeftHandPoints.AddFramePoint2x(16, 21, 23);

            LeftHandPoints.AddFramePoint2x(17, 22 - 6, 18);

            LeftHandPoints.AddFramePoint2x(22, 21, 23);

            //Right Hand
            RightHandPoints.AddFramePoint2x(10, 12, 12);
            RightHandPoints.AddFramePoint2x(11, 29, 14);
            RightHandPoints.AddFramePoint2x(12, 33, 26);

            RightHandPoints.AddFramePoint2x(13, 14, 9);
            RightHandPoints.AddFramePoint2x(14, 24, 12);
            RightHandPoints.AddFramePoint2x(15, 27, 18);
            RightHandPoints.AddFramePoint2x(16, 23, 23);

            //Mount
            MountShoulderPoints.DefaultCoordinate = new Point(16 * 2, 25 * 2);
            SittingPoint = new Point(17 * 2, 25 * 2);

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(17, 13);
            HeadVanityPosition.AddFramePoint2x(11, 23, 16);
            HeadVanityPosition.AddFramePoint2x(12, 29, 24);

            HeadVanityPosition.AddFramePoint2x(19, 17, 13 - 7);

            HeadVanityPosition.AddFramePoint2x(22, 17, 15);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(16, 19);

            GetRewards();
            LoadSkins();
        }

        public void LoadSkins()
        {
            AddOutfit(1, "Cait Sith", delegate(GuardianData gd, Player player) //TODO - Need alternative way of getting this outfit.
            {
                return Quests.SardineOutfitQuest.IsThisQuestCompleted();
            }, true);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(CaitSithTextureID, "caitsith_outfit");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
                switch (guardian.Data.OutfitID)
                {
                    case 1:
                        {
                            Texture2D texture = sprites.GetExtraTexture(CaitSithTextureID);
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

        public void GetRewards()
        {

        }

        public override List<DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<DialogueOption> Options = base.GetGuardianExtraDialogueActions(guardian); //It's empty, anyways.
            Options.Add(new DialogueOption(!GuardianBountyQuest.SardineTalkedToAboutBountyQuests ? "About Bounties" : "Report Bounty", BountyQuestProgressCheckButtonAction));
            return Options;
        }

        public void BountyQuestProgressCheckButtonAction()
        {
            TerraGuardian tg = Dialogue.GetSpeaker;
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
                        GuardianMouseOverAndDialogueInterface.SetDialogue(GuardianBountyQuest.TargetFullName + " appears in the " + MainMod.SplitTextByCapitals(GuardianBountyQuest.spawnBiome.ToString()) + ", cause a mayhem on it until It shows up.");
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
        
        public override void Attributes(TerraGuardian g)
        {
            g.MeleeSpeed += 0.15f;
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Hey, aren't you an adventurer? Cool! I am too!";
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
            bool HasBreeMet = PlayerMod.PlayerHasGuardian(player, Bree), HasGlennMet = PlayerMod.PlayerHasGuardian(player, Glenn);
            switch (guardian.OutfitID)
            {
                case CaitSithOutfitID:
                    Mes.Add("I really like this outfit, but the cloak on my tail is bothering me a bit.");
                    Mes.Add("For some reason, this outfit also came with some kind of sword.");
                    Mes.Add("[nickname], do you know what Tokyo is?");
                    break;
            }
            if (HasBreeMet && HasGlennMet)
            {
                Mes.Add("I'm so glad that my son and my wife are safe.");
                Mes.Add("I caused my son and wife so much trouble when I disappeared, now neither of us knows the way home...");
                Mes.Add("I thank you for finding my wife and my son, [nickname]. You have my eternal gratitude.");
            }
            else if (!HasBreeMet && HasGlennMet)
            {
                Mes.Add("[nickname], I heard from my son that my wife left home to look for me. If you find a white cat during your travels, please bring her here.");
                Mes.Add("I'm sorry for asking this [nickname], but I just heard from my son that my wife is still looking for me. She's a white cat, please find her.");
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
                if (NpcMod.HasGuardianNPC(Fluffles) && NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("I have to tell you something bizarre that happened to me the other day. I managed to run away from [gn:" + Blue + "] and [gn:" + Fluffles + "] game, or so I thought, and managed to return home. When [gn:" + Bree + "] looked at me, she saw [gn:" + Fluffles + "] hanging on my shoulder. [gn:"+Bree+"] screamed so loud that scared her off.");
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
                    Mes.Add("If you see [gn:"+GuardianBase.Blue+"], [gn:"+GuardianBase.Zacks+"] and [gn:"+GuardianBase.Fluffles+"] looking like they are biting something on the floor, that must be me. Help me if you can?");
                }
                else if (HasBlue)
                {
                    Mes.Add("I really hate having [gn:" + GuardianBase.Fluffles + "] around, because there are now TWO to bite me on [gn:" + GuardianBase.Blue + "]'s stupid game.");
                    Mes.Add("How can you escape from something you can't even see? [gn:"+GuardianBase.Fluffles+"] always catches me because she's nearly invisible during day!");
                }
                Mes.Add("I have to tell you what happened to me the other day. I was on the toilet doing my things, having a hard time, until [gn:"+GuardianBase.Fluffles+"] surged from nowhere. She spooked me really hard! But at least solved my constipation issue.");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("Like [gn:" + Minerva + "], you're wondering why I only eat fish? It's because fishs are the best!");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("[gn:" + Glenn + "] is more the studious type than a fighter.");
                Mes.Add("Sometimes I don't have the chance of doing something with [gn:" + Glenn + "], our interests are different.");
                Mes.Add("[gn:" + Glenn + "] should stop reading so many fairy tales books, since we are literally living in one.");
                if (NpcMod.HasGuardianNPC(Zacks))
                {
                    Mes.Add("If [gn:"+Zacks+"] keep scaring my son whenever he's outside at dark, I'll show him a version of me that he didn't met when biting me!");
                }
            }
            if (NpcMod.HasGuardianNPC(Miguel))
            {
                Mes.Add("Could [gn:"+Miguel+"] stop making jokes about my belly? They hurt!");
                Mes.Add("I'm really getting some exercise tips from [gn:"+Miguel+"] to turn my fat into muscles, but he keeps making jokes about my belly.");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("You may think ghosts and stuff are scary, but you wont know what is scary, until you wake up and see [gn:"+Green+"] staring directly at your face.");
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
            else if (guardian.IsSleeping)
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
                    Mes.Add("I'm home! I brought all those treasures for us. (He must be dreaming about returning home for someone.)");
                }
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Yikes, you're being haunted by a ghost? What does she want?");
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
                return "I feel weird for asking this but... I need your help with a particular something... It's about... [objective]. Hey, don't laugh.";
            return "I'm not really a fan of asking for help, but I really need help for this. I need you to [objective]. Can you help me with that?";
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
                        case GuardianBase.Glenn:
                            {
                                Mes.Add(ReviveGuardian.Name + "! " + ReviveGuardian.Name + "! Can you hear me?!");
                                Mes.Add("No no no no NO! " + ReviveGuardian.Name+"! Hang on! Your father is here!");
                                Mes.Add("Don't worry "+ReviveGuardian.Name+", It won't let It end like this.");
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

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Bree)
                {
                    Weight = 1.5f;
                    return "Don't worry "+WhoJoined.Name+", we'll find out where our house is at*";
                }
                if(WhoJoined.ID == Glenn)
                {
                    Weight = 1.5f;
                    return "I'm disappointed that you disobeyed your mother, but I'm also happy that you made it here safelly.";
                }
                if(WhoJoined.ID == Blue)
                {
                    Weight = 1.5f;
                    return "Wait, why is she showing me her teeth?";
                }
            }
            Weight = 1f;
            return "A new person! Nice to meet you.";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Bree:
                        Weight = 1.5f;
                        return "She's coming with us? I hope isn't for keeping an eye on me.";
                    case Glenn:
                        Weight = 1.5f;
                        return "I don't mind him coming with us, as long as he doesn't get in danger.";
                    case Vladimir:
                        Weight = 1.5f;
                        return "Hey big guy, carry me.";
                    case Blue:
                        Weight = 1.5f;
                        return "No! Please! Don't!!";
                    case Zacks:
                        Weight = 1.5f;
                        return "You aren't here to bite me, right?";
                    case Fluffles:
                        Weight = 1.5f;
                        return "You don't plan on mounting on my shoulder again, don't you?";
                }
            }
            Weight = 1f;
            return "Hi! Nice to see you joining us.";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "So, you picked me as your buddy? I like that idea of having you as my buddy too.";
                case MessageIDs.RescueMessage:
                    return "I'm glad you called, It was fun trying to carry you here.";
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
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "I'm always ready for a new adventure.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "There's way too many people in your group. There is no way I can join.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "I can't go on an adventure right now...";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "Yes, I can leave the group but... Here?";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "Awww... I were having so much fun. Let's adventure some more in the future.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "Okay, okay. I wont judge your decision. I see you back at home. Be safe.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "Yes, let's look for a town so I can leave the group.";
                case MessageIDs.RequestAccepted:
                    return "Ok. See me when you get that done.";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "Don't you have many things to do right now?";
                case MessageIDs.RequestRejected:
                    return "Oh, fine.";
                case MessageIDs.RequestPostpone:
                    return "Come see me if you decide to help me with this.";
                case MessageIDs.RequestFailed:
                    return "Well, It's not everyday you can have success, right. I'm not angry. It's fine.";
                case MessageIDs.RequestAsksIfCompleted:
                    return "Hey [nickname], completed my request?";
                case MessageIDs.RequestRemindObjective:
                    return "Short memory, eh? I asked you to [objective].";
                case MessageIDs.RestAskForHowLong:
                    return "We need energy to go adventuring, getting some rest will be good. How long are we going to rest?";
                case MessageIDs.RestNotPossible:
                    return "This isn't a good moment to rest.";
                case MessageIDs.RestWhenGoingSleep:
                    return "In case you hear me snoring, do not plug my noses. Please.";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "Hey [nickname], let's check out [shop]'s shop.";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Woah, that's exactly what I need! Wait a moment.";
                case MessageIDs.GenericYes:
                    return "That's great!";
                case MessageIDs.GenericNo:
                    return "That's bad...";
                case MessageIDs.GenericThankYou:
                    return "Thanks!";
                case MessageIDs.GenericNevermind:
                    return "Forget that then..";
                case MessageIDs.ChatAboutSomething:
                    return "Want to share some adventure stories? I really like that idea!";
                case MessageIDs.NevermindTheChatting:
                    return "Had enough of chatting? Me too. Okay, we'll chat more later.";
                case MessageIDs.CancelRequestAskIfSure:
                    return "You want to cancel my request? Why? Is It tough, or no time? You're really sure?";
                case MessageIDs.CancelRequestYesAnswered:
                    return "Oh man... Better I get into doing that, then...";
                case MessageIDs.CancelRequestNoAnswered:
                    return "Whew... Good.";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Tell me, what are you hiding, cat?*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Alright...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*Uh huh...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*I see... So you have the scent of many dead creatures...*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*What am I doing...? Uh... Checking your pulse. You seems alive! Phew...*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "So glad to have you guys around.";
                    return "I'm fine, thanks for the help.";
                case MessageIDs.RevivedByRecovery:
                    return "I still stand.";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "Ugh... Not good...";
                case MessageIDs.AcquiredBurningDebuff:
                    return "Fire!! Fire!!!";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "My vision!";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "Everything seems funky...";
                case MessageIDs.AcquiredCursedDebuff:
                    return "I can't swing my weapon!";
                case MessageIDs.AcquiredSlowDebuff:
                    return "I think I got an arrow on the knee.";
                case MessageIDs.AcquiredWeakDebuff:
                    return "Huf... Puf... Just a minute...";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "Argh!! My chest!";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "What is that horrible creature! Let's slay it!";
                case MessageIDs.AcquiredIchorDebuff:
                    return "When I accepted moving to this world, I didn't expected... This...";
                case MessageIDs.AcquiredChilledDebuff:
                    return "I'm f-f-freezing.";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "Not again! Someone, help!";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "I will slit your throat!";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "I can endure some more now.";
                case MessageIDs.AcquiredWellFedBuff:
                    return "Ah! Delicious! Even better than my wife's food, too.";
                case MessageIDs.AcquiredDamageBuff:
                    return "This will make slaying better.";
                case MessageIDs.AcquiredSpeedBuff:
                    return "I feel like lightning now.";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "I got buff!";
                case MessageIDs.AcquiredCriticalBuff:
                    return "My attacks will be more impactiful now.";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "Blades ready.";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "It went down well.";
                case MessageIDs.AcquiredHoneyBuff:
                    return "I wish I could drink this all day.";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "A Life Crystal!";
                case MessageIDs.FoundPressurePlateTile:
                    return "Wait! Pressure plate.";
                case MessageIDs.FoundMineTile:
                    return "There's a mine over there!";
                case MessageIDs.FoundDetonatorTile:
                    return "Better not touch that Detonator.";
                case MessageIDs.FoundPlanteraTile:
                    return "I don't like the look of this...";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "Let's kick some Etherians ass?";
                case MessageIDs.FoundTreasureTile:
                    return "Amazing! Loot!";
                case MessageIDs.FoundGemTile:
                    return "Those would look perfect on my wife.";
                case MessageIDs.FoundRareOreTile:
                    return "I see some ores here.";
                case MessageIDs.FoundVeryRareOreTile:
                    return "Hey [nickname]! Check out those ores.";
                case MessageIDs.FoundMinecartRailTile:
                    return "I hope I don't get nauseous...";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "Yes, let's check our loot there.";
                case MessageIDs.CompanionInvokesAMinion:
                    return "Time to make things easier.";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "That is so weird.";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "What? [nickname]! Hang on!";
                case MessageIDs.LeaderDiesMessage:
                    return "[nickname]!!!";
                case MessageIDs.AllyFallsMessage:
                    return "Someone's injured!";
                case MessageIDs.SpotsRareTreasure:
                    return "Loot! Precious loot!";
                case MessageIDs.LeavingToSellLoot:
                    return "I'll go sell those items. I'll be right back.";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "I can distract them while you recover, [nickname].";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "Haha.. That didn't even hurt..";
                case MessageIDs.RunningOutOfPotions:
                    return "Uh oh... I have few potions left.";
                case MessageIDs.UsesLastPotion:
                    return "We should definitelly restock some potions. My potions just ran out.";
                case MessageIDs.SpottedABoss:
                    return "I can't wait to see it falling on the ground.";
                case MessageIDs.DefeatedABoss:
                    return "And another victory to us!";
                case MessageIDs.InvasionBegins:
                    return "They don't look friendly. Good.";
                case MessageIDs.RepelledInvasion:
                    return "That's what you all came here for?! Know that there's more if you return!";
                case MessageIDs.EventBegins:
                    return "Look, [nickname]! What could that mean?";
                case MessageIDs.EventEnds:
                    return "Everyone is alright? Good.";
                case MessageIDs.RescueComingMessage:
                    return "Oh no, hang on!";
                case MessageIDs.RescueGotMessage:
                    return "You'll be fine, I'm with you, just don't die.";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "Have you been hearing about [player]'s travels, latelly? I've been.";
                case MessageIDs.FeatMentionBossDefeat:
                    return "You should have seen! [player] took down [subject] so impressively.";
                case MessageIDs.FeatFoundSomethingGood:
                    return "You heard? [player] found a [subject] while exploring the world! They're so lucky.";
                case MessageIDs.FeatEventFinished:
                    return "I heard that [player] managed to take care of a [subject] problem.";
                case MessageIDs.FeatMetSomeoneNew:
                    return "I have heard that [player] met another person. Their name was [subject].";
                case MessageIDs.FeatPlayerDied:
                    return "Man... A buddy of mine has perished recently... Their name was [player]...";
                case MessageIDs.FeatOpenTemple:
                    return "I have heard that [player] opened the door of a temple at [subject]. I wonder what could be inside.";
                case MessageIDs.FeatCoinPortal:
                    return "Hahaha, So lucky! [player] managed to bump into a coin portal.";
                case MessageIDs.FeatPlayerMetMe:
                    return "I have met another adventurer recently, they're called [player]. Do you know them?";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "I'm impressed with [player]. They managed to help the Angler Kid a lot latelly.";
                case MessageIDs.FeatKilledMoonLord:
                    return "If you want to visit [subject], It seems like things are safer there. [player] killed some squid monster and now people are saying the world is saved.";
                case MessageIDs.FeatStartedHardMode:
                    return "If you're looking for things to kill, I think you should visit [subject]. [player] managed to do something that made stronger and uglier creatures appear.";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "Be sure to give your congratulations to [player], It's such a feat having [subject] accept them as their buddy.";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "Buddy! Came to check me out? Or want something? Haha, sorry, I'm not used to that buddy thing.";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "[subject] got a shiny new place to live in [world].";
                case MessageIDs.DeliveryGiveItem:
                    return "Take those [item], [target].";
                case MessageIDs.DeliveryItemMissing:
                    return "Hey! Where is the item!";
                case MessageIDs.DeliveryInventoryFull:
                    return "[target], your inventory is full! I can't give you anything.";
                case MessageIDs.CommandingLeadGroupSuccess:
                    return "Sure. I shall take care of them during our trek.";
                case MessageIDs.CommandingLeadGroupFail:
                    return "I have other things to focus on right now.";
                case MessageIDs.CommandingDisbandGroup:
                    return "I could use some sleep now.";
                case MessageIDs.CommandingChangeOrder:
                    return "I will do that.";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
