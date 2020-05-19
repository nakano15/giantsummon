using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class BlueBase : GuardianBase
    {
        /// <summary>
        /// -Cares about her hair.
        /// -Friendly.
        /// -Likes poisons.
        /// -Dislikes competition.
        /// -Loves Bunnies.
        /// -Zacks girlfriend.
        /// -> Also worries about his current condition, wondering if can revive him.
        /// -Hates Rococo.
        /// -Likes camping.
        /// -Loves to be with other people.
        /// -Dancer.
        /// </summary>

        public BlueBase()
        {
            Name = "Blue"; //Green Eyes
            Description = "It may not look like it, but she really cares about her look.\nShe constantly does her hair and paints her nail.\nSometimes they are painted red, but doesn't really mean...";
            //She also likes bunnies.
            Size = GuardianSize.Large;
            Width = 26;
            Height = 82;
            SpriteWidth = 96;
            SpriteHeight = 96;
            FramesInRows = 20;
            DuckingHeight = 54;
            Age = 17;
            Male = false;
            InitialMHP = 175; //1150
            LifeCrystalHPBonus = 45;
            LifeFruitHPBonus = 15;
            Accuracy = 0.46f;
            Mass = 0.5f;
            MaxSpeed = 4.75f;
            Acceleration = 0.13f;
            SlowDown = 0.5f;
            MaxJumpHeight = 19;
            JumpSpeed = 7.52f;
            CanDuck = true;
            ReverseMount = false;
            SetTerraGuardian();
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit6);
            DeadSound = new SoundData(Terraria.ID.SoundID.NPCDeath1);
            CallUnlockLevel = 0;

            PopularityContestsWon = 2;
            ContestSecondPlace = 1;
            ContestThirdPlace = 1;

            AddInitialItem(Terraria.ID.ItemID.IronBroadsword, 1);
            AddInitialItem(Terraria.ID.ItemID.IronBow, 1);
            AddInitialItem(Terraria.ID.ItemID.WoodenArrow, 250);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 16, 17, 18, 19 };
            DuckingFrame = 20;
            DuckingSwingFrames = new int[] { 21, 22, 23 };
            SittingFrame = 24;
            ChairSittingFrame = 26;
            PlayerMountedArmAnimation = 25;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22, 23, 25 });
            ThroneSittingFrame = 27;
            BedSleepingFrame = 28;
            SleepingOffset.X = 16;
            DownedFrame = 32;
            ReviveFrame = 33;
            PetrifiedFrame = 34;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(24, 0);
            //BodyFrontFrameSwap.Add(26, 1);

            //Left Hand Position
            LeftHandPoints.AddFramePoint2x(10, 6, 14);
            LeftHandPoints.AddFramePoint2x(11, 40, 9);
            LeftHandPoints.AddFramePoint2x(12, 43, 41);

            LeftHandPoints.AddFramePoint2x(16, 14, 4);
            LeftHandPoints.AddFramePoint2x(17, 32, 4);
            LeftHandPoints.AddFramePoint2x(18, 39, 19);
            LeftHandPoints.AddFramePoint2x(19, 33, 31);

            LeftHandPoints.AddFramePoint2x(21, 35, 16);
            LeftHandPoints.AddFramePoint2x(22, 40, 23);
            LeftHandPoints.AddFramePoint2x(23, 43, 33);

            LeftHandPoints.AddFramePoint2x(33, 43, 43);

            //Right Hand Position
            RightHandPoints.AddFramePoint2x(10, 9, 14);
            RightHandPoints.AddFramePoint2x(11, 42, 9);
            RightHandPoints.AddFramePoint2x(12, 45, 41);

            RightHandPoints.AddFramePoint2x(16, 16, 4);
            RightHandPoints.AddFramePoint2x(17, 34, 4);
            RightHandPoints.AddFramePoint2x(18, 41, 19);
            RightHandPoints.AddFramePoint2x(19, 35, 31);

            RightHandPoints.AddFramePoint2x(21, 37, 16);
            RightHandPoints.AddFramePoint2x(22, 42, 23);
            RightHandPoints.AddFramePoint2x(23, 45, 33);

            RightArmFrontFrameSwap.Add(29, 0);

            //Shoulder Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(16 * 2, 16 * 2);
            MountShoulderPoints.AddFramePoint2x(11, 29, 22);
            MountShoulderPoints.AddFramePoint2x(12, 33, 29);

            MountShoulderPoints.AddFramePoint2x(20, 30, 31);
            MountShoulderPoints.AddFramePoint2x(21, 30, 31);
            MountShoulderPoints.AddFramePoint2x(22, 30, 31);

            //Sitting Point
            SittingPoint = new Point(21 * 2, 37 * 2);

            //Armor Head Points
            HeadVanityPosition.DefaultCoordinate2x = new Point(21, 12);
            HeadVanityPosition.AddFramePoint2x(11, 33 - 2, 17 + 2);
            HeadVanityPosition.AddFramePoint2x(12, 38 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(20, 38 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(21, 38 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(22, 38 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(23, 38 - 2, 24 + 2);
            HeadVanityPosition.AddFramePoint2x(33, 36, 27);

            //Wing
            WingPosition.DefaultCoordinate2x = new Point(22, 21);
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*[name] says that you don't need to walk any longer, that you can ride on her shoulder. As long as you don't ruin her hair.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*[name] says that you can control her movements, but tells you to be careful about what you do.*";
            }
        }

        public override string FriendLevelMessage
        {
            get
            {
                return "*[name] is thankful for letting her live on your town. She likes the people in it.*";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "*[name] is talking about throwing a party later. She wants to dance with her friends.*";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "*[name] says that It's a gift that she has met your town.*";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "*[name] would like to spend some time to lie down on the floor, and watch the stars with her best friends.*";
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "\"Is... That... a Werewolf? I don't think so... It's a taller one?\"";
                case 1:
                    return "\"As soon as I got closer to it, that... Wolf? Friendly waved at me.\"";
                case 2:
                    return "\"She is asking me If I'm camping too.\"";
                default:
                    return "\"She seems to be enjoying the bonfire, until I showed up.\"";
            }
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] told me that she wants nothing right now.*";
            return "*[name] shaked her head and then returned to what she was doing.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*She seems to want something.*";
            return "*As soon as I asked if she wanted something, she gave me a list.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] got overjoyed after I fullfilled her request.*";
            return "*She got so happy that smiles at you, and wags her tail.*";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool ZacksRecruited = PlayerMod.PlayerHasGuardian(player, 3);
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("*[name] is looking at me with a question mark face, while wondering what you want.*");
                Mes.Add("*[name] looks to me while smiling.*");
            }
            else
            {
                Mes.Add("*[name] is growling and showing her teeth as I approached her.*");
                Mes.Add("*[name]'s facial expressions is very scary, I should avoid talking to her at the moment.*");
            }
            if (!ZacksRecruited)
            {
                if (!Main.bloodMoon)
                {
                    if (Main.raining)
                        Mes.Add("*[name] looks sad.*");
                    if (!Main.dayTime)
                        Mes.Add("*[name] howls to the moon, showing small signs of sorrow.*");
                }
                else
                {
                    if (!Main.dayTime)
                        Mes.Add("*[name] is saying that she is feeling a familiar presence, coming from the far lands of the world. Saying that we should check.*");
                }
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*[name] looks sleepy.*");
                    Mes.Add("*[name] is circling the room, I wonder what for.*");
                }
            }
            else
            {

            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*[name] is stealing all the spotlights of the party.*");
                if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
                {
                    Mes.Add("*[name] is calling [gn:3] for a dance.*");
                    Mes.Add("*[name] is dancing with [gn:3], they seems to be enjoying.*");
                }
            }
            if ((guardian.ID != 2 || guardian.ModID != MainMod.mod.Name) && !PlayerMod.PlayerHasGuardian(player, 2))
            {
                Mes.Add("*[name] is bored. She would like to play a game, but nobody seems good for that.*");
            }
            if (guardian.ID == 3 && guardian.ModID == MainMod.mod.Name && PlayerMod.PlayerHasGuardian(player, 2))
                Mes.Add("*First, [name] called [gn:3] to play a game, now they are arguing about what game they want to play. Maybe I should sneak away very slowly.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3, MainMod.mod) && PlayerMod.PlayerHasGuardian(player, 2))
                Mes.Add("*[name] is asking if she could borrow [gn:3] for a minute, so they could play a game with [gn:2].*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.WitchDoctor))
                Mes.Add("*[name] seems to be playing with flasks of poison.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                Mes.Add("*[name] wants you to check her hair.*");
            if (NpcMod.HasGuardianNPC(0))
                Mes.Add("*[name] seems to be complaining about [gn:0], saying he's childish and annoying.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("*[name]'s mood goes away as soon as she saw [gn:0].*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
                Mes.Add("*[name] said that she feels good for knowing that [gn:3] is around, but she also looks a bit saddened.*");
            if (NpcMod.HasGuardianNPC(2))
                Mes.Add("*[name] is saying that wants to bite something, and is asking If I've seen [gn:2] somewhere.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
                Mes.Add("*[name] said that she wants to play. For some reason, [gn:2] ran away.*");
            if (NpcMod.HasGuardianNPC(2) && NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*[name] is watching [gn:2] and [gn:5] playing together, with a worry face.*");
                Mes.Add("*[name] says that didn't had much chances to play with [gn:2], since most of the time he ends up playing with [gn:5].*");
            }
            if (NpcMod.HasGuardianNPC(5) && !PlayerMod.PlayerHasGuardianSummoned(player, 5))
            {
                Mes.Add("*[name] is whistling, like as if was calling a dog, and trying to hide the broom she's holding on her back.*");
                Mes.Add("*[name] is telling me that the next time [gn:5] leaves a smelly surprise on her front door, she'll chase him with her broom.*");
            }
            if (NpcMod.HasGuardianNPC(7))
                Mes.Add("*[name] says that really hates when [gn:7] interrupts when playing with [gn:2].*");
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*[name] is angry, because [gn:8] insulted her hair earlier.*");
                Mes.Add("*[name] is complaining about [gn:8], asking who she thinks she is.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                Mes.Add("*[name] is very happy for having [gn:10] around.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer) && NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                {
                    Mes.Add("*[name] tells that [nn:" + Terraria.ID.NPCID.Nurse + "] appeared earlier, asking for tips on what to do on a date with [nn:"+Terraria.ID.NPCID.ArmsDealer+"]. She said that she gave some tips that she can use at that moment.*");
                }
                if(!NpcMod.HasGuardianNPC(Zacks))
                    Mes.Add("*[name] asks If you have seen [gn:"+Vladimir+"], after removing a tear from her face. She seems to need to speak with him.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*[name] is saying that you're making her embarrassed.*");
                Mes.Add("*[name] would like you to turn the other way, If you want to talk.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] would like to move to where people are, give her a house there.*");
            Mes.Add("*[name] is tired of living alone in the woods, build her a house somewhere.*");
            if (!Main.dayTime)
                Mes.Add("*[name] would like to sleep well at night, without worrying about the dangers of the night.*");
            if (Main.raining)
                Mes.Add("*[name] is really worried about the effects of the rain on her fur, give her some place to live.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("*[name] is asking me if she knows any good \"Naggicide\", why? Because she wants to use it on that guy following you.*");
            if (NpcMod.HasGuardianNPC(0) && Main.npc[NpcMod.GetGuardianNPC(0)].Distance(player.Center) < 1024f)
                Mes.Add("*[name] is asking if you could send [gn:0] some place far away from her.*");
            if (NpcMod.HasGuardianNPC(3) && Main.npc[NpcMod.GetGuardianNPC(3)].Distance(player.Center) >= 768f)
                Mes.Add("*[name] would like to move to somewhere closer to [gn:3].*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.WitchDoctor))
                Mes.Add("*[name] is asking you what is your favorite type of poison.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] is asking what you think about what she did with her room.*");
                Mes.Add("*[name] wants to travel the world with you.*");
                Mes.Add("*[name] asks if you want to help her move some furnitures.*");
                Mes.Add("*[name] is asking if you have any flea killing remedy.*");
            }
            else
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                    Mes.Add("*[name] wants to visit [nn:" + Terraria.ID.NPCID.Stylist + "] sometime.*");
                if (Main.moonPhase == 0 )
                {
                    if (!PlayerMod.PlayerHasGuardian(player, 3))
                    {
                        Mes.Add("*[name] seems to be missing someone.*");
                    }
                    else
                    {
                        Mes.Add("*[name] said that the full moon always reminds her of [gn:3].*");
                    }
                }
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*[name] said that she wants to play a game with [gn:2], causing him to panic for some reason.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
            {
                Mes.Add("*[name] got a bit saddened when she saw [gn:3], but feels a bit relieved for seeying him.*");
            }
            else if (PlayerMod.PlayerHasGuardian(player, 3))
            {
                Mes.Add("*[name] keeps wondering if there is a way of bringing [gn:3] to his old self.*");
            }
            if (NpcMod.HasGuardianNPC(3))
                Mes.Add("*[name] says that initially she came to the world looking for [gn:3], but after seeying how beautiful the environment is, she decided to stay more. And since [gn:3] is here, she can stay for longer.*");
            Mes.Add("*[name] wants to go shopping, and is asking if you would lend some coins.*");
            if(Main.bloodMoon)
                Mes.Add("*[name] is so furious right now that she could kill someone, good thing that outside has many options.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 1))
            {
                if (Main.rand.Next(2) == 0)
                    return "*[name] is trying so hard to not ask what you will give her as a gift.*";
                return "*[name] said that she will beat you up if her gift is a leash.*";
            }
            return "*[name] is dancing away.*";
        }

        public override void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if ((!UsingRightArm || !UsingRightArm) && guardian.BodyAnimationFrame != DuckingFrame && guardian.BodyAnimationFrame != ThroneSittingFrame && guardian.BodyAnimationFrame != BedSleepingFrame)
            {
                bool HasBunnyInInventory = false;
                for (int i = 0; i < 10; i++)
                {
                    if (guardian.Inventory[i].type == Terraria.ID.ItemID.Bunny || guardian.Inventory[i].type == Terraria.ID.ItemID.GoldBunny)
                    {
                        HasBunnyInInventory = true;
                        break;
                    }
                }
                if (HasBunnyInInventory)
                {
                    const int BunnyHoldingFrame = 29;
                    if (guardian.BodyAnimationFrame == StandingFrame)
                        guardian.BodyAnimationFrame = BunnyHoldingFrame;
                    if (guardian.BodyAnimationFrame == BedSleepingFrame)
                    {
                        guardian.BodyAnimationFrame = 30;
                    }
                    if (!UsingLeftArm)
                    {
                        guardian.LeftArmAnimationFrame = BunnyHoldingFrame;
                        UsingLeftArm = true;
                    }
                    if (!UsingRightArm)
                    {
                        guardian.RightArmAnimationFrame = BunnyHoldingFrame;
                        UsingRightArm = true;
                    }
                }
            }
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            bool GotMessage = false;
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID)
            {
                GotMessage = true;
                switch (ReviveGuardian.ID)
                {
                    default:
                        GotMessage = false;
                        break;
                    case GuardianBase.Zacks:
                        {
                            Mes.Add("*No! I've nearly lost you once! Don't do that again!*");
                            Mes.Add("*I don't even know If It's working, please stand up!*");
                            Mes.Add("*I can't be left without you again, please!*");
                        }
                        break;
                    case GuardianBase.Sardine:
                        {
                            Mes.Add("*It's not fun when you're knocked out.*");
                            Mes.Add("*If you don't wake up, I'll bite you! ... He's still knocked out cold.*");
                            Mes.Add("*Alright, I promisse not to chase and bite you if you wake up. Please, wake up!*");
                        }
                        break;
                }
            }
            if (!GotMessage)
            {
                Mes.Add("*Don't worry, you'll be fine in a moment.*");
                Mes.Add("*Here, hold my hand. Now stand up!*");
                Mes.Add("*I'm here with you, rest while I help you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }
    }
}
