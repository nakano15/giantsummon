using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class RococoBase : GuardianBase
    {
        /// <summary>
        /// -Playful.
        /// -Innocent.
        /// -Extremelly friendly.
        /// -Fears Blood Moons.
        /// -Want to be Blue's friend.
        /// -Good friend of Sardine and Alex.
        /// </summary>
        public const string ShadedBodyID = "alphapigshadedbody", ShadedHeadID = "alphapigshadedhead", ShadedBodyFrontID = "alphapigshadedbodyfront";
        public const int AlphapigShadedBodySkinID = 1;

        public RococoBase()
        {
            Name = "Rococo"; //Has red eyes
            Description = "He's a good definition of a big kid, very playful and innocent.\nLoves playing kids games, like Hide and Seek.";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 82;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 15;
            Male = true;
            InitialMHP = 200; //1000
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 10;
            Accuracy = 0.15f;
            Mass = 0.5f;
            MaxSpeed = 5.2f;
            Acceleration = 0.18f;
            SlowDown = 0.47f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.08f;
            CanDuck = true;
            ReverseMount = false;
            DrinksBeverage = false;
            SetTerraGuardian();
            HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);
            CallUnlockLevel = 0;

            PopularityContestsWon = 2;
            ContestSecondPlace = 3;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            PlayerMountedArmAnimation = JumpFrame = 9;
            HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 16, 17, 18, 19 };
            DuckingFrame = 20;
            DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 23;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 24;
            BedSleepingFrame = 25;
            SleepingOffset.X = 16;
            ReviveFrame = 26;
            DownedFrame = 27;
            PetrifiedFrame = 28;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(23, 0);

            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(17 * 2, 31 * 2);
            LeftHandPoints.AddFramePoint2x(10, 6, 9);
            LeftHandPoints.AddFramePoint2x(11, 32, 9);
            LeftHandPoints.AddFramePoint2x(12, 43, 38);

            LeftHandPoints.AddFramePoint2x(16, 14, 5);
            LeftHandPoints.AddFramePoint2x(17, 34, 7);
            LeftHandPoints.AddFramePoint2x(18, 39, 19);
            LeftHandPoints.AddFramePoint2x(19, 34, 31);

            LeftHandPoints.AddFramePoint2x(21, 33, 16);
            LeftHandPoints.AddFramePoint2x(22, 43, 27);

            LeftHandPoints.AddFramePoint2x(26, 33, 41);
            //Right Hand
            RightHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(31 * 2, 31 * 2);
            RightHandPoints.AddFramePoint2x(10, 10, 9);
            RightHandPoints.AddFramePoint2x(11, 34, 9);
            RightHandPoints.AddFramePoint2x(12, 45, 38);

            RightHandPoints.AddFramePoint2x(16, 16, 5);
            RightHandPoints.AddFramePoint2x(17, 36, 7);
            RightHandPoints.AddFramePoint2x(18, 41, 19);
            RightHandPoints.AddFramePoint2x(19, 37, 31);

            RightHandPoints.AddFramePoint2x(21, 36, 16);
            RightHandPoints.AddFramePoint2x(22, 45, 27);
            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(18 * 2, 14 * 2);
            MountShoulderPoints.AddFramePoint2x(11, 22, 20);
            MountShoulderPoints.AddFramePoint2x(12, 30, 27);
            MountShoulderPoints.AddFramePoint2x(20, 30, 27);
            MountShoulderPoints.AddFramePoint2x(21, 30, 27);
            MountShoulderPoints.AddFramePoint2x(22, 30, 27);

            MountShoulderPoints.AddFramePoint2x(24, 16, 20);
            MountShoulderPoints.AddFramePoint2x(25, 25, 28);

            //Left Arm Positions
            LeftArmOffSet.DefaultCoordinate = new Microsoft.Xna.Framework.Point(18 * 2, 15 * 2);
            //LeftArmOffSet.AddFramePoint2x(11, 21, 21);
            //LeftArmOffSet.AddFramePoint2x(12, 32, 30);
            //LeftArmOffSet.AddFramePoint2x(19, 32, 30);

            //Right Arm Positions
            RightArmOffSet.DefaultCoordinate = new Microsoft.Xna.Framework.Point(28 * 2, 15 * 2);
            //RightArmOffSet.AddFramePoint2x(11, 30, 21);
            //RightArmOffSet.AddFramePoint2x(12, 38, 32);
            //RightArmOffSet.AddFramePoint2x(19, 32, 30);

            //Sitting Position
            SittingPoint = new Point(23 * 2, 37 * 2); //21, 37

            //Head Vanity Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(24, 13);
            HeadVanityPosition.AddFramePoint2x(11, 27, 16);
            HeadVanityPosition.AddFramePoint2x(12, 36, 26);
            HeadVanityPosition.AddFramePoint2x(20, 36, 26);
            HeadVanityPosition.AddFramePoint2x(21, 36, 26);
            HeadVanityPosition.AddFramePoint2x(22, 36, 26);

            HeadVanityPosition.AddFramePoint2x(24, 24, 16);
            HeadVanityPosition.AddFramePoint2x(25, 26, 48 - 6);

            HeadVanityPosition.AddFramePoint2x(26, 31, 23);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(20, 23);

            RequestList();
            RewardList();
        }

        public void RequestList()
        {
            AddNewRequest("Deglobulation", 200,
                "*[name] told you that It is unsafe to live around with so many Slimes jumping around. He asks you to kill a number of them.*",
                "*[name] thanks you, and tells that has something for you when you finish the task.*", 
                "*[name] seems saddened after you refused the request, then said he will try doing that later.*",
                "*[name] is happy for seeing the lands safer.*",
                "*[name] tells you that there are still some Slimes that needs to be killed.*");
            AddHuntObjective(Terraria.ID.NPCID.BlueSlime, 3);
            //
            AddNewRequest("Give him some candy!", 150,
                "*[name] is asking you to get something sweet for him to eat. After he asked for that, only one thing comes on your mind: Gel.*",
                "*[name] is happy, and says that can't wait to see what you will bring him.*",
                "*[name] is looking at you with a disappointed look. Is he going to cry?*",
                "*[name] is surprised at what you brought him. He seems extremelly happy while eating the Gels.*",
                "*[name] wonder what you will bring to him.*");
            AddItemCollectionObjective(Terraria.ID.ItemID.Gel, 10, 0.02f);
            //
            AddNewRequest("Sea of Bunnies", 50,
                "*[name] is asking you if you could get him a Bunny.*",
                "*[name] seems happy, he told you that will be waiting for it.*",
                "*[name] looks very sad now.*",
                "*[name] is very happy with the bunny you brought. He said that was looking for something to play with, and you just brought.*",
                "*[name] says that It wont be hard for you to find Bunnies, but that you will need a Bug Net to get them.*");
            AddRequestRequirement(RequestBase.GetBugNetRequirement);
            AddItemCollectionObjective(Terraria.ID.ItemID.Bunny, 1, 0);
            //
            AddNewRequest("Dead Walker", 225, 
                "*[name] says that these lands are dangerous at night because of the Zombie. He asks you to kill a number of them.*",
                "*[name] tells you to becareful, because he doesn't want you to turn into one too.*",
                "*[name] doesn't seems to blame you for rejecting, since he wouldn't want to do that, either.*",
                "*[name] says that It seems a lot better to sleep now.*", 
                "*[name] says that Zombies attacks during the night, but will hardly attack a place with many people around.*");
            AddHuntObjective(Terraria.ID.NPCID.Zombie, 7);
            //
            AddNewRequest("I see with my giant eye.", 245,
                "*[name] is telling you about one of the dangers of the night, the Demon Eyes. He's asking you to take out a number of them.*",
                "*[name] tells you to becareful on that.*",
                "*[name] looks at you with a face, looking like he knew you would deny.*",
                "*[name] is happy, and says that you were really cool when fighting the Demon Eyes.*",
                "*[name] tells you that Demon Eyes floats around during the night. He also told you that they will rarelly attack places with many people around.*");
            AddHuntObjective(Terraria.ID.NPCID.DemonEye, 5, 0.02f);
            //
            AddNewRequest("That's so many Ravens.", 280,
                "*[name] tells you of the Ravens that are appearing during the night in this season. He says that they look really scary, and asks you to get rid of them.*",
                "*[name] is telling you to becareful when looking for them, since you can barelly see them in the dark.*",
                "*[name] says that he will try locking himself in his room during the night. Just for safety.*",
                "*[name] says that Ravens are very creepy creatures.*",
                "*[name] tells you that Ravens appears during the night.*");
            AddRequestRequirement(delegate(Player player)
            {
                return Main.halloween;
            });
            AddHuntObjective(Terraria.ID.NPCID.Raven, 5, 0.2f);
            //
            AddNewRequest("Werewolf? Where? Wolf?", 360,
                "*[name] is telling you about creepy wolves wandering around in full moon night. He said that tried to befriend one once, and got hurt by It, and then was attacked by more of them. He's asking you to do something about them.*",
                "*[name] tells you to be very mean to them. And also to becareful because they jump on who they are attacking.*",
                "*[name] says that It is fine. He also told you to becareful when wandering outside during full moon nights. He doesn't want you to be attacked by them too.*",
                "*[name] looks very happy after hearing about your success.*",
                "*[name] says that those wolves creatures appears during Full Moon nights. He told you to beware not to miss It.*");
            AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode && (Main.moonPhase == 7 || (Main.moonPhase == 0 && Main.dayTime));
            });
            AddHuntObjective(Terraria.ID.NPCID.Werewolf, 3, 0.1f);
            //
            AddNewRequest("Hollow Armors", 320,
                "*[name] is telling you about creepy armors that walks during the night. He said that It will be dangerous to have them around, and asks you to make a number of them stop moving.*",
                "*[name] tells you to beware when facing them, since they don't take much recoil from attacks.*",
                "*[name] says that even him doesn't want to try fighting them either, saying that he's better staying at home, than out there fighting those things.*",
                "*[name] is happy for seeing you return safe from that.*",
                "*[name] says that those walking armors appears during the night. Tells you to becareful because they are resistant to knockback.*");
            AddRequestRequirement(delegate(Player player)
            {
                return Main.hardMode;
            });
            AddHuntObjective(Terraria.ID.NPCID.PossessedArmor, 4, 0.2f);
            //
            AddNewRequest("Adventure Call", 200,
                "*[name] is saying that he wants to go on an adventure. He asks if he can accompany you on yours.*",
                "*[name] got happy after you accepted, then started to get several things he says that may be useful in the travel.*",
                "*[name] says that feels bored at the moment.*",
                "*[name] says that liked the adventure, and that doesn't mind to continue exploring the world with you.*", 
                "*[name] is asking when you'll take him on your adventures.*");
            AddExploreObjective();
            //
            AddSkin(AlphapigShadedBodySkinID, "Pigman Delta Shaded Skin", delegate(GuardianData gd, Player player) { return true; });
        }

        public void RewardList()
        {
            AddReward(Terraria.ID.ItemID.SlimeStaff, 1, 1000, 0.001f);
            AddReward(Terraria.ID.ItemID.Daybloom, 2, 35, 0.6f, 3);
            AddReward(Terraria.ID.ItemID.BowlofSoup, 3, 40, 0.55f, 2);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(ShadedBodyID, "shadedbody");
            sprites.AddExtraTexture(ShadedHeadID, "shadedhead");
            sprites.AddExtraTexture(ShadedBodyFrontID, "shadedbodyfront");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            switch (guardian.SkinID)
            {
                case AlphapigShadedBodySkinID:
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                            gdd.Texture = sprites.GetExtraTexture(ShadedBodyID);
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBodyFront)
                            gdd.Texture = sprites.GetExtraTexture(ShadedBodyFrontID);
                        if (gdd.textureType == GuardianDrawData.TextureType.TGHead)
                            gdd.Texture = sprites.GetExtraTexture(ShadedHeadID);
                    }
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                            gdd.Texture = sprites.GetExtraTexture(ShadedBodyID);
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBodyFront)
                            gdd.Texture = sprites.GetExtraTexture(ShadedBodyFrontID);
                        if (gdd.textureType == GuardianDrawData.TextureType.TGHead)
                            gdd.Texture = sprites.GetExtraTexture(ShadedHeadID);
                    }
                    break;
            }
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect, ref List<GuardianDrawData> gdd)
        {
            if (guardian.SkinID == 1)
            {
                foreach (GuardianDrawData data in gdd)
                {
                    if (data.textureType == GuardianDrawData.TextureType.TGHead)
                    {
                        data.Texture = sprites.GetExtraTexture(ShadedHeadID);
                        break;
                    }
                }
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*[name] says that he can let you ride on his shoulder, If your feet are tired.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*[name] says that entrusts his life to you.*";
            }
        }

        public override string FriendLevelMessage
        {
            get
            {
                return "*[name] says that you have been a good friend to him.*";
            }
        }

        public override string BestFriendLevelMessage
        {
            get
            {
                return "*[name] says that you're the best friend he ever had.*";
            }
        }

        public override string BFFLevelMessage
        {
            get
            {
                return "*[name] says that you're like the only family he has.*";
            }
        }

        public override string BuddyForLifeLevelMessage
        {
            get
            {
                return "*[name] is glad of having met you.*";
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "\"At first, the creature got surprised after seeing me, then starts laughing out of happiness.\"";
                case 1:
                    return "\"That creature waves at you while smiling, It must be friendly, I guess?\"";
                case 2:
                    return "\"For some reason, that creature got happy after seeing you, maybe It wasn't expecting another human in this world?";
                default:
                    return "\"What sort of creature is that? Is it dangerous? No, It doesn't looks like it.\"";
            }
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] says that doesn't need anything right now..*";
            return "*[name] told me that wants nothing right now.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] is asking me to do something for him.*";
            return "*[name] is looking at me with a funny face while telling me that he wants something to be done, like as If he didn't wanted to ask for help.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] was so happy that started laughing out loud.*";
            return "*[name] is so impressed that you did what he asked, that even gave you a hug.*";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool MerchantInTheWorld = NPC.AnyNPCs(Terraria.ID.NPCID.Merchant), SteampunkerInTheWorld = NPC.AnyNPCs(Terraria.ID.NPCID.Steampunker);
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
                Mes.Add("*[name] is happy for seeing you.*");
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*[name] asks you what's up.*");
                }
                else
                {
                    Mes.Add("*[name] seems to be watching some classic horror movie on the tv... No, wait, that's a window.*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    if (MerchantInTheWorld)
                        Mes.Add("*As soon as [name] started talking, you hastily asked him to stop, because of the bad trash breath that comes from his mouth.*");
                    Mes.Add("*[name] is sleeping while awake.*");
                    Mes.Add("*[name] is trying hard to keep It's eyes opened.*");
                }
                else
                {
                    Mes.Add("*[name] looks scared, maybe he hates blood moons.*");
                    Mes.Add("*[name] is trembling in terror..*");
                }
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*[name] seems to be enjoying the party.*");
            }
            if (SteampunkerInTheWorld)
                Mes.Add("*[name] is talking something about a jetpack joyride?*");
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[name] seems to be crying, and with a purple left eye, I guess his dialogue with [gn:1] went wrong.*");
                Mes.Add("*[name] seems to be crying, and with his right cheek having a huge red paw marking, I wonder what he were talking about with [gn:1].*");
            }
            if (NpcMod.HasGuardianNPC(3))
            {
                Mes.Add("*[name] seems to have gotten kicked in his behind. Maybe he annoyed [gn:3]?*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2) && PlayerMod.PlayerHasGuardian(player, 1))
            {
                Mes.Add("*[gn:2] is telling [name] that he's lucky that [gn:1] doesn't plays her terrible games with him. But [name] insists that he wanted to play.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] asked [gn:1] why she doesn't plays with him, she told him that she can't even bear seeing him.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3) && PlayerMod.PlayerHasGuardian(player, 1))
            {
                Mes.Add("*[name] asked [gn:3] why he doesn't plays with him, he told him that It's because he makes [gn:1] upset.*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*[name] says that loves playing with [gn:5], but wonders why he always find him on hide and seek.*");
                Mes.Add("*[name] says that bringing [gn:5] made the town very livelly.*");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*[name] said that [gn:8] looks familiar, have they met before?*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                Mes.Add("*[name] hugs you. It feels a bit weird. He never hugged you without a reason.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*[name] is telling me to plug my nose.*");
                Mes.Add("*[name] is asking if there is no other moment to chat.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*[name] is very happy for having you as his room mate.*");
                Mes.Add("*[name] says that no longer fear sleeping at night, since you shared your room with him.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                if (!guardian.KnockedOutCold)
                {
                    Mes.Add("*[name] says that will still try protecting you.*");
                    Mes.Add("*[name] is asking you to stay by his side for a while.*");
                    Mes.Add("(He's trying to endure the pain, you can see a few tears rolling from his eyes.)");
                }
                else
                {
                    Mes.Add("(He whited out.)");
                    Mes.Add("(He seems to be having pain while whited out.)");
                }
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("(He must be dreaming about playing with someone.)");
                Mes.Add("(You got startled when he looked at your direction and smiled.)");
                Mes.Add("(He seems to be sleeping fine.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.raining)
                Mes.Add("*[name] seems to have caught flu, that wouldn't have happened if he had a place to live. Bad Terrarian.*");
            if (!Main.dayTime)
            {
                Mes.Add("*[name] looks afraid of the dark, I should give him somewhere to live.*");
                Mes.Add("*[name] seems cold, give him some place to get out of the cold.*");
            }
            Mes.Add("*[name] would like to live close to other Terrarians. Build a house for him.*");
            Mes.Add("*[name] is lonely and afraid of the dangers around, I could build him a house.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] showed you a rare insect he found, he seems very happy about that.*");
            Mes.Add("*[name] is asking you when is going to happen another party.*");
            Mes.Add("*[name] seems to want a new toy, but what could I give him?*");
            Mes.Add("*[name] wants to explore the dungeon sometime.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[name] is asing me if [nn:" + Terraria.ID.NPCID.Merchant + "] has put his trash can outside.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("*[name] seems to want to go on an adventure with you.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*[name] is enjoying travelling with me.*");
                Mes.Add("*[name] seems to killing insects with gasoline, I wonder where he acquired that.*");
                if (guardian.Wet || guardian.HasBuff(Terraria.ID.BuffID.Wet))
                    Mes.Add("*[name] is soaked and cold.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
                Mes.Add("*[name] looks surprised at [gn:1], and suddenly forgets what he was going to talk about.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
                Mes.Add("*[name] is asking if you could let him play with [gn:2].*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 0) && Main.rand.NextDouble() < 0.5)
                return "*[name] is curious about what you will give him as gift.*";
            return "*[name] is asking why you aren't dancing, It's party time.*";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("*" + Guardian.Name + " is telling you to hold on.*");
                Mes.Add("*" + Guardian.Name + " tells you to not try moving.*");
                Mes.Add("*" + Guardian.Name + " tells you to rest while he heals the wounds.*");
                Mes.Add("*" + Guardian.Name + " told you to rest a bit.*");
                Mes.Add("*" + Guardian.Name + " is very focused into helping you.*");
                Mes.Add("*" + Guardian.Name + " holds you tight.*");
            }
            else
            {
                Mes.Add("*" + Guardian.Name + " is trying to calm down the knocked out person.*");
                Mes.Add("*" + Guardian.Name + " is tending the wounds.*");
                Mes.Add("*" + Guardian.Name + " is attempting to stop the bleeding.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "*You heard someone saying that you'll be fine.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
