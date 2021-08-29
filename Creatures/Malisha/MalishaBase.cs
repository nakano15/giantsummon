using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class MalishaBase : GuardianBase
    {
        /// <summary>
        /// -Loves makes experiments with magic and alchemy.
        /// -Doesn't mind not using clothes.
        /// -Leopolds apprentice.
        /// -Loves tormenting her mentor.
        /// -Feels uncomfortable when people look in her chest.
        /// -Expelled from many villages due to her experiments.
        /// -Initially thought the Terrarian world is a naturalism colony.
        /// -Turns into a mad scientist during Blood Moons, testing experiments on anyone who gets in her sight.
        /// </summary>
        public MalishaBase()
        {
            Name = "Malisha";
            Description = "Two things are important for her: Practicing\nMagic and Testing. Don't volunteer.";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 84;
            DuckingHeight = 54;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 97f / 84;
            CompanionSlotWeight = 1.3f;
            Age = 21;
            SetBirthday(SEASON_SUMMER, 21);
            Male = false;
            InitialMHP = 135; //960
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 30;
            InitialMP = 30;
            Accuracy = 0.91f;
            Mass = 0.45f;
            MaxSpeed = 5.8f;
            Acceleration = 0.15f;
            SlowDown = 0.41f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.36f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = true;
            IsNocturnal = true;
            SetTerraGuardian();
            CallUnlockLevel = 5;
            MountUnlockLevel = 7;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.AmberStaff, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 5);
            AddInitialItem(Terraria.ID.ItemID.ManaPotion, 15);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            DuckingFrame = 22;
            DuckingSwingFrames = new int[] { 23, 24, 25 };
            SittingFrame = 18;
            ChairSittingFrame = 17;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            //ThroneSittingFrame = 24;
            BedSleepingFrame = 20;
            SleepingOffset.X = 16;
            ReviveFrame = 19;
            DownedFrame = 21;
            ThroneSittingFrame = 26;

            BackwardStanding = 27;
            BackwardRevive = 28;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 1);

            SittingPoint = new Point(21 * 2, 36 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 12, 3);
            LeftHandPoints.AddFramePoint2x(11, 31, 12);
            LeftHandPoints.AddFramePoint2x(12, 34, 19);
            LeftHandPoints.AddFramePoint2x(13, 30, 28);

            LeftHandPoints.AddFramePoint2x(14, 5, 7);
            LeftHandPoints.AddFramePoint2x(15, 31, 6);
            LeftHandPoints.AddFramePoint2x(16, 41, 40);

            LeftHandPoints.AddFramePoint2x(19, 37, 43);

            LeftHandPoints.AddFramePoint2x(23, 32, 14);
            LeftHandPoints.AddFramePoint2x(24, 42, 24);
            LeftHandPoints.AddFramePoint2x(25, 36, 38);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 16, 3);
            RightHandPoints.AddFramePoint2x(11, 34, 12);
            RightHandPoints.AddFramePoint2x(12, 37, 19);
            RightHandPoints.AddFramePoint2x(13, 33, 28);

            RightHandPoints.AddFramePoint2x(14, 7, 7);
            RightHandPoints.AddFramePoint2x(15, 33, 6);
            RightHandPoints.AddFramePoint2x(16, 43, 40);

            RightHandPoints.AddFramePoint2x(19, 39, 43);

            RightHandPoints.AddFramePoint2x(23, 35, 14);
            RightHandPoints.AddFramePoint2x(24, 45, 24);
            RightHandPoints.AddFramePoint2x(25, 39, 38);

            //MountedPosition
            MountShoulderPoints.DefaultCoordinate2x = new Point(16, 31);
            MountShoulderPoints.AddFramePoint2x(1, 17, 31);
            MountShoulderPoints.AddFramePoint2x(2, 18, 30);
            MountShoulderPoints.AddFramePoint2x(3, 17, 31);
            MountShoulderPoints.AddFramePoint2x(5, 15, 31);
            MountShoulderPoints.AddFramePoint2x(6, 14, 30);
            MountShoulderPoints.AddFramePoint2x(7, 15, 31);

            MountShoulderPoints.AddFramePoint2x(14, 20, 31);
            MountShoulderPoints.AddFramePoint2x(15, 22, 31);
            MountShoulderPoints.AddFramePoint2x(16, 25, 30);

            MountShoulderPoints.AddFramePoint2x(19, 11, 26);

            MountShoulderPoints.AddFramePoint2x(22, 14, 38);
            MountShoulderPoints.AddFramePoint2x(23, 14, 38);
            MountShoulderPoints.AddFramePoint2x(24, 14, 38);
            MountShoulderPoints.AddFramePoint2x(25, 14, 38);

            MountShoulderPoints.AddFramePoint2x(26, 14, 32);

            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(22 + 1 + 2, 10 + 2);
            HeadVanityPosition.AddFramePoint2x(16, 36 + 1, 21 + 2);
            HeadVanityPosition.AddFramePoint2x(19, 33 + 1, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(22, 35, 24);
            HeadVanityPosition.AddFramePoint2x(23, 33 + 1, 25 + 2);
            HeadVanityPosition.AddFramePoint2x(24, 33 + 1, 25 + 2);
            HeadVanityPosition.AddFramePoint2x(25, 33 + 1, 25 + 2);

            //Wing Position
            WingPosition.AddFramePoint2x(14, -1000, -1000);
            WingPosition.AddFramePoint2x(15, -1000, -1000);
            WingPosition.AddFramePoint2x(16, -1000, -1000);

            WingPosition.AddFramePoint2x(19, 28, 33);
            WingPosition.AddFramePoint2x(23, 28, 33);
            WingPosition.AddFramePoint2x(24, 28, 33);
            WingPosition.AddFramePoint2x(25, 28, 33);

            CreateRequests();
        }

        public override void Attributes(TerraGuardian g)
        {
            g.MaxMinions++;
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture("tails", "tails");
        }

        public override void GuardianPreDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
            Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
            if (TailTexture == null)
            {
                return;
            }
            GuardianDrawData dd;
            if (!guardian.PlayerMounted)
            {
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
            else
            {
                rect.Y += rect.Height * 2;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            if (guardian.PlayerMounted)
            {
                Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
                if (TailTexture == null)
                    return;
                GuardianDrawData dd;
                if (guardian.BodyAnimationFrame == HeavySwingFrames[0] || guardian.BodyAnimationFrame == BackwardRevive || guardian.BodyAnimationFrame == BackwardStanding)
                {
                    rect.Y += rect.Height * 2;
                    dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                    guardian.AddDrawData(dd, true);
                    rect.Y += rect.Height * 2;
                }
                else
                {
                    rect.Y += rect.Height * 4;
                }
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, true);
            }
            else
            {
                if (guardian.BodyAnimationFrame == HeavySwingFrames[0] || guardian.BodyAnimationFrame == BackwardRevive || guardian.BodyAnimationFrame == BackwardStanding)
                {
                    Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                    Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
                    if (TailTexture == null)
                        return;
                    GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                    guardian.AddDrawData(dd, true);
                }
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*You seems to have seens various kinds of places during your travels, maybe that can help me find test subjects.*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*It's not me, It's you, you're slowing down the expedition. I can carry you around during the exploration, If you want. I have better uses for my hands than holding you, so I guess I can use my tail, if you don't mind.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*Okay, It's not working either way. I have an idea, what If I go alone in the travels? Just tell me where you wanted to go.*";
            }
        }

        public override string MoveInUnlockMessage => "*I think It will be benefitial for us if I move my things to this world. There are so many potential subj... Study creatures around here that may be useful for my experiments. Just give me a call when you have a house ready.*";

        //It has been so long that I got this companion idea, that I even forgot her personality. Oops.
        public override string GreetMessage(Terraria.Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Oh, a Terrarian. I think I may have a use for you.*");
            Mes.Add("*Funny, I thought here was a naturalist colony, but you're wearing clothes. Well, whatever. I may hang around here for a while.*");
            Mes.Add("*You're really small, my neck aches a bit trying to look at you. Say, would you mind participating of some experiments?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Don't mind what people says, I'm one of the best magicians in the Ether Realm.*");
            if (player.Male)
            {
                Mes.Add("*You're making me a bit uncomfortable with the angle you're looking at me. Just a bit.*");
            }
            else
            {
                Mes.Add("*You can't see my head, or something?*");
            }
            Mes.Add("*I once tried to conjure demons, that's when I had to leave the first village I lived in hurry.*");
            Mes.Add("*Always wondered why you get stronger by using specific kinds of outfits? Well, me too.*");
            Mes.Add("*It's not easy being a prodigy, but when you're one, you have to keep working hard to continue being.*");
            Mes.Add("*I have perfect control of my magic! Now, at least. Let's not revive past experiences.*");
            Mes.Add("*Nobody really complained about my experiments. Here.*");

            Mes.Add("*Well, I could go to the Ether Realm to get some of my clothes, but It's too much work...*");
            Mes.Add("*You ever wondered why the TerraGuardians have no clothes in this world? Well, I never, that's the main reason I came here.*");
            Mes.Add("*I feel like people actually look directly into my \'mana depository\' when they look at me.*");
            Mes.Add("*I'm a bit disappointed that this isn't a naturalist colony like I initially thought, but I'm glad I can do my experiments here.*");

            Mes.Add("*Take your time to trust in me, and you will find infinity.*");

            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*No, those creatures didn't came from my lab.*");
                    Mes.Add("*Interesting. Would you mind catching one of those creatures alive for my researches?*");
                }
                else
                {
                    Mes.Add("*I really like this time of day, I can find test subjects with ease at this moment, I just need to walk a bit.*");
                    Mes.Add("*Say, would you mind if I messed with your molecular structure? No? Too bad.*");
                }
            }
            else
            {
                Mes.Add("*To reduce the annoyance levels, I try to do quiet experiments during this time, to avoid annoying neighbors of bothering me.*");
                Mes.Add("*I'm glad you came, would you mind sitting on that chair? I will just need to tie your arms and legs afterwards, though.*");
                Mes.Add("*Came visit me? Or did someone sent you? Because I'm pretty sure someone must have been annoyed by my experiments.*");
                Mes.Add("*What's with those Demon Eyes? It's like as they didn't see a TerraGuardian before.*");
                Mes.Add("*When a Demon Eye charges on someone, isn't supposed that they would get hurt too?*");
            }
            if (Main.raining)
            {
                if (player.Male)
                    Mes.Add("*I really love It when the rain drips through my body... Uh... Where are you looking at?*");
                else
                    Mes.Add("*I really love It when the rain drips through my body.*");
                Mes.Add("*Yes! Keep on raining! Bring It on!*");
                Mes.Add("*I love rain, but there is 95% chance I'll have a serious case of flu after It ends.*");
            }

            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("*" + NPC.firstNPCName(Terraria.ID.NPCID.Guide) + " got really pale when he saw me doing experiments with a doll that looks like him.*");
                Mes.Add("*Say, do you know why " + NPC.firstNPCName(Terraria.ID.NPCID.Guide) + " is on flames?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Wizard))
            {
                Mes.Add("*I tried to cast a conjuration spell with " + NPC.firstNPCName(Terraria.ID.NPCID.Wizard) + " once, we ended up spawning a rain of Corrupt Bunnies.*");
                Mes.Add("*I tend to share my work with " + NPC.firstNPCName(Terraria.ID.NPCID.Wizard) + " sometimes, at least one wont blame the other if something explodes.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("*"+NPC.firstNPCName(Terraria.ID.NPCID.Stylist) + " says that wants to do magic with my hair, but I sense that her magic level is 0.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
            {
                Mes.Add("*Can you tell " + NPC.firstNPCName(Terraria.ID.NPCID.Dryad) + " that I don't need a baby sitter? If the fauna suddenly tries to eat you alive is because... Well, probably not my fault.*");
                Mes.Add("*" +NPC.firstNPCName(Terraria.ID.NPCID.Dryad) + " says that I'm a living sign of bad omen. No matter what she says, I will keep experimenting.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
            {
                Mes.Add("*I wonder if " + NPC.firstNPCName(Terraria.ID.NPCID.Clothier) + " could make me an outfit that doesn't bother me while wearing It.*");
            }

            if (NpcMod.HasGuardianNPC(Rococo))
            {
                Mes.Add("*I tried to analyze [gn:" + Rococo + "]'s intelligence once, I got a NotANumber Exception Error at line 297.*");
                Mes.Add("*Sometimes I wonder that [gn:" + Rococo + "] is like a link between this world and Ether Realm. That may probably be wrong.*");
            }
            if (NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*[gn:" + Blue + "] seems a bit bothered for having another girl in the town.*");
                Mes.Add("*It seems like [gn:" + Blue + "] got really interessed on a spell I discovered, of turning others into humanoid bunnies. Watch your back.*");
            }
            if (NpcMod.HasGuardianNPC(Sardine))
            {
                Mes.Add("*I could have tried using a spell of turning someone into a giant on [gn:" + Sardine + "], but I don't think someone would be happy of having a Rowdy Avatar Cait Sith around.*");
            }
            if (NpcMod.HasGuardianNPC(Zacks))
            {
                Mes.Add("*Interesting what happened to [gn:" + Zacks + "], I wonder if that isn't related to... Uh... Nevermind.*");
                Mes.Add("*Impressive, [gn:" + Zacks + "] not only is a walking dead, but also a sentient one... Hm...*");
            }
            if (NpcMod.HasGuardianNPC(Alex))
            {
                Mes.Add("*[gn:" + Alex + "] doesn't really look like a TerraGuardian, I wonder if the creator has something to do with him.*");
                Mes.Add("*[gn:" + Alex + "] keeps talking about "+AlexRecruitScripts.AlexOldPartner+", I never ever heard about her, or him. Touche?*");
            }
            if (NpcMod.HasGuardianNPC(Nemesis))
            {
                Mes.Add("*I like having [gn:" + Nemesis + "] around, at least he doesn't look with angry eyes on me.*");
                Mes.Add("*You say that [gn:" + Nemesis + "] willingly joined your travel after defeating It's armor? Do you know what are the chances of that happening? About 1%!*");
            }
            if (NpcMod.HasGuardianNPC(Brutus))
            {
                Mes.Add("*It's not really hard to find test subjects in this world, I just need to tell [gn:"+Brutus+"] that I need some help with something.*");
                Mes.Add("*For someone who claims to be a bodyguard, [gn:" + Brutus + "] main weakness is women. Gladly I know how to use that to my advantage.*");
            }
            if (NpcMod.HasGuardianNPC(Bree))
            {
                Mes.Add("*I'd hate having [gn: " + Bree+ "] as a neighbor. If I wanted to hear complaints about my experiments, I would have remained on the Ether Realm.*");
            }
            if (NpcMod.HasGuardianNPC(Mabel))
            {
                Mes.Add("*[gn:" + Mabel + "] seems to have some kind of effect on male people in this world. That's actually interesting, I would have several test subjects with It.*");
                Mes.Add("*I heard from [gn:" + Mabel + "] that she's trying to participate of some kind of contest, and she asked if I didn't had anything to grow Antleers on her head, to possibly increase the chances of entering It.*");
            }
            if (NpcMod.HasGuardianNPC(Domino))
            {
                Mes.Add("*It's interesting to see someone interessed in my experiments, [gn:" + Domino + "] buys them from me often for resale. At least I got someone to fund my experiments.*");
                Mes.Add("*Some may call my researchs junk, [gn:" + Domino + "], calls them profit.*");
            }
            if (NpcMod.HasGuardianNPC(Leopold))
            {
                Mes.Add("*Interesting having [gn:" + Leopold + "] around, I could torment him with my experiments.*");
                Mes.Add("*[gn:" + Leopold + "] and I have quite a story back then, he call me an example of how not to research magic. That doesn't stops me of using what I learn on him.*");
                Mes.Add("*[gn:" + Leopold+"] is my mentor, I wouldn't say that I'm his best studdent, even more when I test what I learned on him.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*[gn:"+Vladimir+"] comes from a lineage of warriors, but he seems to be the opposite of his parents. Would he mind if I did a research to investigate why?*");
                Mes.Add("*Whenever I need to process my thoughts, I visit [gn:" + Vladimir + "]. I tried talking about them with him, but he looked uninteressed, so I remain quiet.*");
                Mes.Add("*Aaack! I fell asleep when processing my thoughts with [gn:"+Vladimir+"]! I must get back to researching.*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*Everytime [gn:" + Michelle + "] comes bother me, I transform her into a different critter.*");
                Mes.Add("*[gn:" + Michelle + "] always arrives just in time I need someone to test my experiments.*");
            }
            if (NpcMod.HasGuardianNPC(Wrath))
            {
                Mes.Add("*Hmph, [gn:"+Wrath+"] thinks he's safe from me, but my experimenting hunger will eventually reach him. Just he wait.*");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*Hmph. It looks like I got concorrence. I got to work harder on making my mentor's life not be easy.*");
                Mes.Add("*I wonder if [gn:"+Fluffles+"] would mind If I tested a vaccuum I've created on her.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*[gn:" + Minerva + "] seems to be a good test subject, but she's been rejecting my requests for food, so It's hard to lure her...*");
                Mes.Add("*It's not my fault that [gn:" + Minerva + "] is fat. Now, if there's any other collateral effects... Maybe...*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("*That kid, [gn:" + Glenn + "], always manages to escape from me... I mean... Never accepts my invites.*");
                Mes.Add("*I really would like [gn:" + Glenn + "] to participate of a little experiment... But how could I bypass his luck..?*");
            }

            if (guardian.IsUsingToilet)
            {
                Mes.Add("*Are you curious If I can use the toilet like anyone else? If your curiosity is now gone, please could go away with It?*");
                Mes.Add("*Why are you bothering me on my reflection moment?*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Yes, I would love having you as a room mate. Heh.*");
                Mes.Add("*Feel free to get some sleep anytime you want. If you do so now, would be perfect.*");
                Mes.Add("*Feeling drowzy? No worry, you can lie down in any bed in our room.*");
                Mes.Add("*It's good to have company during the night, even more when I'm working on my experiments.*");
                if (NpcMod.HasGuardianNPC(Leopold))
                {
                    Mes.Add("*How silly, like I would use my dear room mate as test subject. Psh...*");
                }
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Leopold))
            {
                Mes.Add("*You're sharing room with [gn:" + Leopold + "]? Would you mind moving somewhere else? Huh? I have no actual reason for asking. Sigh..*");
            }

            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*Mwahahaha! You have triggered my trap card!*");
                Mes.Add("*Don't worry, this wont hurt a bit.*");
                Mes.Add("*How did you know that I needed someone for my experiment?*");
                Mes.Add("*Good, I was needing someone for my brain transplant machine. Ready to turn into a Squirrel?*");
                Mes.Add("*I got you! Now drink this potion!*");

                if (!guardian.KnockedOut)
                {
                    int BuffID = -1;
                    switch (Main.rand.Next(10))
                    {
                        case 0:
                            BuffID = Terraria.ID.BuffID.Endurance;
                            break;
                        case 1:
                            BuffID = Terraria.ID.BuffID.Inferno;
                            break;
                        case 2:
                            BuffID = Terraria.ID.BuffID.Lifeforce;
                            break;
                        case 3:
                            BuffID = Terraria.ID.BuffID.MagicPower;
                            break;
                        case 4:
                            BuffID = Terraria.ID.BuffID.Titan;
                            break;
                        case 5:
                            BuffID = Terraria.ID.BuffID.Darkness;
                            break;
                        case 6:
                            BuffID = Terraria.ID.BuffID.Cursed;
                            break;
                        case 7:
                            BuffID = Terraria.ID.BuffID.Confused;
                            break;
                        case 8:
                            BuffID = Terraria.ID.BuffID.Weak;
                            break;
                        case 9:
                            BuffID = 164; //Distorted
                            break;
                    }
                    if (BuffID > -1)
                    {
                        player.AddBuff(BuffID, 10 * 60 * 60);
                    }
                }
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("*" + player.name + "... Ugh... Please... Help.. me... Ugh....* (She's trying to stay awaken.)");
                    Mes.Add("(She seems about to pass out.)");
                    Mes.Add("*Ugh... I'm... Not okay... Argh...*");
                    if (NpcMod.HasGuardianNPC(Leopold))
                    {
                        Mes.Add("*Urgh.... [gn:"+Leopold+"]... Ahrgh....*");
                    }
                }
                else
                {
                    Mes.Add("(She seems to be having pain)");
                    Mes.Add("(You can notice the wounds in her body)");
                    Mes.Add("(She passed out in pain.)");
                }
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(She's speaking about many different things, in a different language.)");
                Mes.Add("(Is she saying elements of the periodic table?)");
                Mes.Add("(She accidentally casted a spell while asleep, and turned the table nearby into a toad.)");
                Mes.Add("(She accidentally casted a spell while asleep, and turned the chair nearby into a potato.)");
                Mes.Add("(She's doing an evil laugh while sleeps)");
                if (NpcMod.HasGuardianNPC(Leopold))
                {
                    Mes.Add("*Here's my newest invention, now time to drink It...* (That brings flashbacks...)");
                    Mes.Add("*I've just learned this spell, I'm glad you offered yourself help me test It...* (Run [gn:" + Leopold + "], Run!)");
                }
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*You've got quite an accessory, [nickname]. Hahaha.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I wont burn your town to cinder, If that's what's on your mind.*");
            Mes.Add("*After I told the people on the town I was living before that I was going away for a vacation, a party has started in the town. I think they were wishing me good luck.*");
            Mes.Add("*I feel so alive when testing things on living things. It is unfortunate if they end up no longer living after the testing.*");
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                Mes.Add("*Don't tell [gn:" + GuardianBase.Leopold + "], but I love having his company. He also helps me with my experiments, even though he clearly doesn't want.*");
                Mes.Add("*I tried several times to earn [gn:"+GuardianBase.Leopold+"]'s respect, but he always complains of my methods, so I no longer care about that.*");
                Mes.Add("*I really love scaring [gn:"+Leopold+"], I even have a stack of leaves for when I take It too far.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Zacks))
            {
                Mes.Add("*I may be wrong, but the moment [gn:" + GuardianBase.Zacks + "] died was perfect. Well, he could have died for good if wasn't.*");
                Mes.Add("*Maybe I have something to do with what happened to [gn:" + GuardianBase.Zacks + "], but I may be wrong. Just try not to tell anyone about that.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Blue))
            {
                Mes.Add("*[gn:" + GuardianBase.Blue + "], seems a bit too obsessed with the bunny transformation spell. What she could possibly used It on?*");
                if (NpcMod.HasGuardianNPC(GuardianBase.Zacks))
                {
                    Mes.Add("*I'm impressed at how [gn:"+GuardianBase.Blue+"] still loves [gn:"+Zacks+"]. Tell me when something bad happen to them, I would like to analyze their brains while It's still fresh.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("I have something else to experiment on right now.");
            Mes.Add("No, I'm not looking for test subjects right now.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hey, I need your help for one of my experiments. Don't worry, I wont harm you in the process.*");
            Mes.Add("*My experiments stops me from doing this right now, If you could help me.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Thank you! Time for the experiment. I hope this time It doesn't explodes.*");
            Mes.Add("*Good. If you manage to smell something foul, or hear random screams, don't worry, It's just part of the process.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("*Be kind and give me a house to live, and I totally promisse not to turn you into a worm.*");
                Mes.Add("*Should I cast a spell on you to make you build me a house?*");
                Mes.Add("*What do you think is better? Giving me a house, or being dissection subject? You pick.*");
            }
            else
            {
                Mes.Add("*There is a lot of places to practice, but I need a moment to recharge my mana.*");
                Mes.Add("*Everything seems to want a piece of me, would be nice If I had some peaceful place to stay.*");
                if (Main.raining)
                {
                    Mes.Add("*I think I know a spell to stop this rain, but I don't want to use It. I don't mind at all. But I need a place to stay when It ends.*");
                    Mes.Add("*I really like the rain, but I need some place to stay when the flu attacks.*");
                    if (player.Male)
                        Mes.Add("*Your look bothers me a bit, would you mind stopping look in that direction, and building me a house?*");
                }
                if (!Main.dayTime)
                {
                    Mes.Add("*It's fascinating that there are dead terrarians roaming the world at night. But It'd like to study that behind walls.*");
                    Mes.Add("*I really hate the night, so many eyes peeking on me. I really need some place for myself.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You guys did all that for me? I think I can be off researching for today.*");
            Mes.Add("*Want to dance, [nickname]?*");
            Mes.Add("*Nobody knows, but I'm the best when It's about dancing. Just watch me.*");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID))
            {
                Mes.Add("*You have a gift for me? What is it? A laboratory in a remote place?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer)
            {
                Mes.Add("*If you don't make It, would you mind If I do experiments with your body?*");
                Mes.Add("*I think this may allow me to learn more about your anathomy.*");
                Mes.Add("*If I close here, may solve your bleeding problem.*");
                if(RevivePlayer.Male)
                    Mes.Add("*If It stops you from groaning, you may keep looking in that direction.*");
                Mes.Add("*Your body isn't much different from the one of a TerraGuardian. Maybe easy to fix.*");
                Mes.Add("*This will ease your pain while I work, drink It.*");
            }
            else
            {
                bool GotAnotherMessage = false;
                if (ReviveGuardian.ModID == "")
                {
                    GotAnotherMessage = true;
                    switch (ReviveGuardian.ID)
                    {
                        default:
                            GotAnotherMessage = false;
                            break;
                        case Leopold:
                            Mes.Add("*I'm just saying, but that isn't a good example.*");
                            Mes.Add("*I'm not interessed in taking your place, for now.*");
                            Mes.Add("*Here something for your wounds. You'll be fine. Not bad coming from your worst studdent.*");
                            Mes.Add("*Okay, okay, I'm closing the open wounds.*");
                            break;
                        case Brutus:
                            Mes.Add("*I can't let you die. I need you.*");
                            Mes.Add("*Come on, big boy, don't disappoint me.*");
                            Mes.Add("*Okay, If you wake up, I'll use the shrinking spell on you again. Now wake up!*");
                            Mes.Add("*I need you for my researches, If you please wake up.*");
                            break;
                        case Zacks:
                            Mes.Add("*Your nerves will be connected soon, just let me work.*");
                            Mes.Add("*You could help me pointing where you can't move.*");
                            Mes.Add("*I can't help you with your left leg, It has been too damaged. Anything else that needs fix?*");
                            break;
                        case Vladimir:
                            Mes.Add("*I preffer you when smiling.*");
                            Mes.Add("*Too much ground to fix...*");
                            Mes.Add("*Come on, big guy. You wont let such a thing kill you, right?*");
                            break;
                        case Alex:
                            Mes.Add("*Okay, I think I've got a problem.*");
                            Mes.Add("*I don't really know animal anathomy, so I may commit mistakes here.*");
                            Mes.Add("*Is this... Oh! Sorry for touching It.*");
                            break;
                    }
                }
                if (!GotAnotherMessage)
                {
                    if (ReviveGuardian.Base.IsTerrarian)
                    {
                        Mes.Add("*Okay, you'll be fine... If you survive...*");
                        Mes.Add("*If you don't make It, you wont mind if I use your body on my future experiments, right?*");
                        Mes.Add("*This will ease your pain while I work, drink It.*");
                    }
                    else
                    {
                        Mes.Add("*Time to make use of the anathomy classes.*");
                        Mes.Add("*I'll just take a bit of blood, for research purpose.*");
                        Mes.Add("*You look in pain, drink this to ease It.*");
                        Mes.Add("*One. One, two. Two, three. Three, four. What am I doing?*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*So, you want to be my buddy? I like the idea of having a familiar pet. I accept.*";
                case MessageIDs.RescueMessage:
                    return "*Oh ho ho, the many experiments I could do with you.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*I really should transform you into something for waking me up. Say It, what do you want?*";
                        case 1:
                            return "*[nickname], one of the most dangerous things ever is waking me up. But right now I'm not in the mood of doing anything.*";
                        case 2:
                            return "*You! You woke me up. Tell me the reason, NOW.*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*The only reason why you didn't turned into a toad, is because I'm really waiting for my request.*";
                        case 1:
                            return "*You woke me up. Is my request done?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*Yes, I may end up finding guinea pigs for my experiments on the way.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I hate mobs.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    if (NpcMod.HasGuardianNPC(Leopold) && Main.rand.NextDouble() < 0.5)
                    {
                        return "*Not right now, I have scheduled testing some potions on [gn:"+Leopold+"].*";
                    }
                    return "*No. I have many experiments to do.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*You want to leave me here in the wilds? Are you out of your mind?*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*I found the walking pleasant. Please call me again in the future, I may find new guinea pigs on the trip.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*You're going to regret that when I see you at the town.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Okay, I think you are able to think rationally sometimes.*";
                case MessageIDs.RequestAccepted:
                    return "*Good, that will keep me alone with my experiments. Try not to come back too soon.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*No no no no no. Go deal with your other requests. I can't have you doing a bad job at my request because you're overloaded.*";
                case MessageIDs.RequestRejected:
                    return "*Pft. Fine. Go away, now.*";
                case MessageIDs.RequestPostpone:
                    return "*No no no, come back here.*";
                case MessageIDs.RequestFailed:
                    return "*You what?! Now, try thinking of reasons as to why I shouldn't turn you into a squirrel.*";
                case MessageIDs.RestAskForHowLong:
                    return "*I enjoy that idea, seems like the perfect moment to test some spells. How long to you plan to rest?*";
                case MessageIDs.RestNotPossible:
                    return "*If It was in another moment, I would have loved that idea.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Try not to wake up earlier, will you?*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Wait, what is that [shop] is offering?*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Hm... This may be useful... Wait a moment.*";
                case MessageIDs.GenericYes:
                    return "*Yes.*";
                case MessageIDs.GenericNo:
                    return "*No.*";
                case MessageIDs.GenericThankYou:
                    return "*Yes, you did good. There.*";
                case MessageIDs.ChatAboutSomething:
                    return "*For the last time, I already said that I didn't incinerated... Oh, It's not about that? Well, go ahead then.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Yawn. It's over? Good.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*Wait, you came to me, and said that wont do what I asked for? Are you really sure?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Okay, you're relieved. Get out of my sight, NOW! Before I decide to do something to you.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*The clock is ticking, [nickname].*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Please don't wake up.. Please don't wake up...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Even her scent rises my fur...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*So many different things she came in contact with... Many are dangerous...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    if(NpcMod.HasGuardianNPC(Leopold))
                        return "*I feel pity of [gn:"+Leopold+"] now.*";
                    return "*I'm so glad I wont need to identify her again.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Uh oh... HEEEEELP!!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Awww... You remembered me... Thank you.*";
                    return "*You couldn't go on without me, couldn't you?*";
                case MessageIDs.RevivedByRecovery:
                    return "*Okay, who will be the first one I'll turn into a frog?*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*I will make you feel worser than this.*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Ah!! You!!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*Ouch, my eyes! Wait until I see you!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Did they called reinforcements?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*I hate so much this curse...*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*I injured my ankle.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*You wont see my drop...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Oh, you pervert!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Ah! Aaahhh!! What is that?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hey! That's.... Wow!*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*So... Want to warm yourself with me...?*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*Hey! Stop daydreaming and help me!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Show me the color of your blood!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*Come on, hit me now. I like it.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*Hmm... I'm so glad I don't care about conjured food.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*This is gonna hurt.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*I can definitelly outrun you, now.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Look how healthier I got.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Feel free to hate me when my attack lands you.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Hm... So usefull...*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Another cup, please.*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*How sweet. I loved having some honey.*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Lovely, a Life Crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Be careful, someone has been naughty.*";
                case MessageIDs.FoundMineTile:
                    return "*I can see that mine. Can you see too?*";
                case MessageIDs.FoundDetonatorTile:
                    return "*Go ahead, press It. I'll just stay here.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I sense trouble.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I could use some Etherian Creatures body parts, so why not.*";
                case MessageIDs.FoundTreasureTile:
                    return "*They know how to treat a girl, right?*";
                case MessageIDs.FoundGemTile:
                    return "*Awww... That would make a lovely trinket for me.*";
                case MessageIDs.FoundRareOreTile:
                    return "*Hm... There's some ores there.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Look at those ores.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*That could ease our travel.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*I think I left something on the cauldron, I'll check when we get home.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*I'm glad you joined, could you aid me on a experiment?*";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "*A new subject, perfect.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*Aww, such lovely minions.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Already throwing yourself on the arms of strangers, [nickname]?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Our mighty leader is lying down on the floor. Let's help them?*";
                case MessageIDs.LeaderDiesMessage:
                    return "*Don't worry [nickname], I can make some good use of your body.*";
                case MessageIDs.AllyFallsMessage:
                    return "*There is someone taking a rest here.*";
                case MessageIDs.SpotsRareTreasure:
                    return "*You know how to please a girl, don't you?*";
                case MessageIDs.LeavingToSellLoot:
                    return "*Don't worry, [nickname]. I'll be right back with the coins.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Breathing hard, [nickname]? Maybe I have something for you.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Ha... Haha... Hahaha.... That's all your got... Ah...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Uh oh... I should have stocked more potions...*";
                case MessageIDs.UsesLastPotion:
                    return "*I dislike this... Someone there has spare potions?*";
                case MessageIDs.SpottedABoss:
                    return "*Hohoho, I would like to make that my minion.*";
                case MessageIDs.DefeatedABoss:
                    return "*Would some of you be willing to carry the pieces of that thing?*";
                case MessageIDs.InvasionBegins:
                    return "*Don't look now, [nickname], but your fans seems to be coming.*";
                case MessageIDs.RepelledInvasion:
                    return "*They weren't friendly, right? I'm glad I managed to snatch some for future experiments.*";
                case MessageIDs.EventBegins:
                    return "*Looks like this will be interesting.*";
                case MessageIDs.EventEnds:
                    return "*Over already? What a pity.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*I have another minion who is called [player], too. They were also helpful on my researches, so I'll try not to do dangerous experiments on them.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*It seems like [player] has defeated [subject]. The body parts of that creature were very helpful to my experiments.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*You would be very jealous if you also saw [player] getting a [subject].*";
                case MessageIDs.FeatEventFinished:
                    return "*I really enjoyed my time when a [subject] happened. Too bad that [player] had to ruin everything.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*And [player] just met another subje... Person, they seem to be called [subject].*";
                case MessageIDs.FeatPlayerDied:
                    return "*A minion of mine called [player] has died recently during their travels. Gladly I managed to recover their body, so I can use for my experiments.*";
                case MessageIDs.FeatOpenTemple:
                    return "*I heard that [player] opened the door to some temple. I wonder what kind of toys they had locked behind it.*";
                case MessageIDs.FeatCoinPortal:
                    return "*A coin portal is such a weird phenomenon, gladly [player] managed to experience it and get a bit richier.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public override bool AlterRequestGiven(GuardianData Guardian, out int ForcedMissionID, out bool IsTalkRequest)
        {
            if (Guardian.FriendshipLevel < 3)
            {
                if (Guardian.FriendshipProgression % 2 == 1)
                {
                    ForcedMissionID = 2;
                }
                else
                {
                    switch (Guardian.FriendshipLevel)
                    {
                        case 1:
                            ForcedMissionID = 0;
                            break;
                        case 2:
                            ForcedMissionID = 1;
                            break;
                    }
                }
            }
            return base.AlterRequestGiven(Guardian, out ForcedMissionID, out IsTalkRequest);
        }

        private void CreateRequests()
        {
            AddNewRequest("Helper Number 1", 300, 
                "*I heard from the other people that you really love helping others. Say... Would you mind catching some Squirrels?*",
                "*That's my "+(Main.player[Main.myPlayer].Male ? "boy" : "girl")+". Now go and make me happy.*",
                "*Hmph. I shouldn't have listened them, then.*",
                "*Great! Now I can do experiements with some kind of acid I've discovered. Thank you.*",
                "*Still didn't got the Squirrels I want? You can find them in the Forest, right? Now go.*",
                "*[nickname]... I will use you for my experiement tonight... If you don't go away now...*");
            AddItemCollectionObjective(Terraria.ID.ItemID.Squirrel, 3);
            AddNewRequest("Ornithophobia", 325,
                "*I'm pretty sure that you'll be interessed in doing my current request. The Harpies you can find in the sky, they have feathers that will be useful for my experiement. Would you mind getting a number of them?*",
                "*You wouldn't disappoint me, right, [nickname]? If you find trouble finding the Harpies, try building a skybridge when nearly leaving the atmosphere. They wont resist, for sure.*",
                "*Are you alergic to feathers? I would like to examine your body constitution if you do.*",
                "*Great! This new invention I'll call... A fan! Will help me in this infernal lab.*",
                "*You lost the way to the sky? Use some Ropes or build a stairway to It. If you find a Sky Island will even be better.*",
                "*How could you fail at getting me feathers, [nickname]? You're already making me angry.*");
            AddItemCollectionObjective(Terraria.ID.ItemID.Feather, 30);
            AddNewRequest("Counter Strike of the Panther", 350, 
                "*[nickname], I'm so furious right now, and I have a important mission for you. Kill as many of those depraved slimes as you can. Will you do It?!*",
                "*Don't leave any of them alive!*",
                "*Grrr... I should do It then! Thanks for nothing.*",
                "*You did? Good! You just avenged by backside, that will teach them to not pop up behind me when I'm about to sit.*",
                "*Having troubles finding Slimes, [nickname]? They are literally everywhere! How could you be lost?*");
            AddHuntObjective(Terraria.ID.NPCID.BlueSlime, 30, 0);
        }
    }
}
