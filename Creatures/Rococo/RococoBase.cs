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
            Scale = 94f / 82;
            CompanionSlotWeight = 1.3f;
            Age = 15;
            SetBirthday(SEASON_SUMMER, 20); //Coincidence?
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

            BackwardStanding = 29;
            BackwardRevive = 30;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(23, 0);

            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point((17 + 1) * 2, 31 * 2);
            LeftHandPoints.AddFramePoint2x(10, 7 + 1, 10);
            LeftHandPoints.AddFramePoint2x(11, 31 + 1, 9);
            LeftHandPoints.AddFramePoint2x(12, 43 + 1, 37);

            LeftHandPoints.AddFramePoint2x(16, 14 + 1, 4);
            LeftHandPoints.AddFramePoint2x(17, 34 + 1, 7);
            LeftHandPoints.AddFramePoint2x(18, 39 + 1, 19);
            LeftHandPoints.AddFramePoint2x(19, 34 + 1, 31);

            LeftHandPoints.AddFramePoint2x(21, 34 + 1, 16);
            LeftHandPoints.AddFramePoint2x(22, 43 + 1, 29);

            LeftHandPoints.AddFramePoint2x(26, 33 + 1, 41);
            //Right Hand
            RightHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point((31 - 1) * 2, 31 * 2);
            RightHandPoints.AddFramePoint2x(10, 9 - 1, 10);
            RightHandPoints.AddFramePoint2x(11, 33 - 1, 9);
            RightHandPoints.AddFramePoint2x(12, 45 - 1, 37);

            RightHandPoints.AddFramePoint2x(16, 16 - 1, 4);
            RightHandPoints.AddFramePoint2x(17, 36 - 1, 7);
            RightHandPoints.AddFramePoint2x(18, 41 - 1, 19);
            RightHandPoints.AddFramePoint2x(19, 37 - 1, 31);

            RightHandPoints.AddFramePoint2x(21, 36 - 1, 16);
            RightHandPoints.AddFramePoint2x(22, 45 - 1, 29);
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
            HeadVanityPosition.AddFramePoint2x(12, 36, 28);
            HeadVanityPosition.AddFramePoint2x(20, 36, 28);
            HeadVanityPosition.AddFramePoint2x(21, 36, 28);
            HeadVanityPosition.AddFramePoint2x(22, 36, 28);

            HeadVanityPosition.AddFramePoint2x(24, 24, 16);
            HeadVanityPosition.AddFramePoint2x(25, 26, 48 - 6);

            HeadVanityPosition.AddFramePoint2x(26, 31, 23);

            //Wing Position
            WingPosition.DefaultCoordinate2x = new Point(20, 23);

            RequestList();
            RewardList();
            GetTopics();
        }

        public void GetTopics()
        {
            AddTopic("Want to play Rock, Paper and Scissor?", PlayRPS);
        }

        public void PlayRPS()
        {
            Random RococoRNG = new Random();
            int PlayerVictories = 0, RococoVictories = 0;
            Dialogue.ShowDialogueWithContinue("*[name] says that accepts playing Rock Paper and Scissors with you.*");
            RestartGame:
            byte PlayerChoice = (byte)Dialogue.ShowDialogueWithOptions("*[name] tells you to be ready.*", new string[] { "Rock", "Paper", "Scissor" });
            Dialogue.ShowDialogueTimed("Rock, Paper, Scissor!!!", null, 60);
            byte RococoChoice = (byte)RococoRNG.Next(3);
            Dialogue.ShowDialogueTimed("You: [" + RPSResult(PlayerChoice) + "] - [" + RPSResult(RococoChoice) + "] : [name]", null, 120);
            byte Winner = 0; //0 = Draw, 1 = Player, 2 = Rococo
            switch (PlayerChoice)
            {
                case 0: //Rock
                    if(RococoChoice == 1) //Paper
                    {
                        Winner = 2;
                    }
                    else if(RococoChoice == 2) //Scissor
                    {
                        Winner = 1;
                    }
                    break;
                case 1: // Paper
                    if(RococoChoice == 2) //Scissor
                    {
                        Winner = 2;
                    }
                    else if(RococoChoice == 0) //Rock
                    {
                        Winner = 1;
                    }
                    break;
                case 2: //Scissor
                    if (RococoChoice == 0) //Rock
                    {
                        Winner = 2;
                    }
                    else if (RococoChoice == 1) //Paper
                    {
                        Winner = 1;
                    }
                    break;
            }
            string VictoryMessage = "";
            switch (Winner)
            {
                case 0:
                    Dialogue.ShowDialogueWithContinue("It's a tie!");
                    VictoryMessage = "*[name] feels funny about having picked the same as you.*";
                    break;
                case 1:
                    PlayerVictories++;
                    Dialogue.ShowDialogueWithContinue("You won!");
                    VictoryMessage = "*[name] doesn't seems that happy about losing, but tries to laugh to relieve the frustration.*";
                    break;
                case 2:
                    RococoVictories++;
                    Dialogue.ShowDialogueWithContinue("[name] won!");
                    VictoryMessage = "*[name] seems very radiant about winning, he's even filled with joy.*";
                    break;
            }
            if (Dialogue.ShowDialogueWithOptions(VictoryMessage + "\n*He then asked if you wanted another round.*\n You: "+PlayerVictories+"\n [name]: "+RococoVictories+"", new string[] { "Yes", "No" }) == 0)
            {
                goto RestartGame;
            }
            Dialogue.ShowEndDialogueMessage("*[name] says that It was a good match.*", false);
        }

        private string RPSResult(byte Choice)
        {
            switch (Choice)
            {
                case 0:
                    return "Rock";
                case 1:
                    return "Paper";
                case 2:
                    return "Scissor";
            }
            return "Spock is not a valid choice.";
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
            if (!Main.bloodMoon && !Main.eclipse)
            {
                Mes.Add("*[name] is happy for seeing you.*");
                Mes.Add("*[name] asks if you brought him something to eat.*");
                Mes.Add("*[name] is asking if you want to play with him.*");
                Mes.Add("*[name] wants you to check some of his toys.*");
                Mes.Add("*[name] seems very glad to see you safe.*");
                if(guardian.OwnerPos == -1)
                    Mes.Add("*[name] is asking you if you came to ask him to go on an adventure.*");
                if (guardian.HasBuff(Terraria.ID.BuffID.WellFed))
                {
                    Mes.Add("*[name] thanks you for the food.*");
                    Mes.Add("*[name] seems to be relaxing after eating something.*");
                }
                else
                {
                    Mes.Add("*You can hear [name]'s stomach growl.*");
                    Mes.Add("*[name] seems to be a bit hungry.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*[name] asks you what's up.*");
                    Mes.Add("*[name] is telling that is liking the weather.*");
                }
                else
                {
                    Mes.Add("*[name] seems to be watching some classic horror movie on the tv... No, wait, that's a window.*");
                    Mes.Add("*[name] is trying to hide behind you, he seems scared of the monsters.*");
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
                    Mes.Add("*[name] seems sleepy.*");
                }
                else
                {
                    Mes.Add("*[name] looks scared.*");
                    Mes.Add("*[name] is trembling in terror..*");
                    Mes.Add("*[name] asks if his house is safe.*");
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
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*[name] says that sometimes he feels weird when [gn:" + Fluffles + "] stares at him for too long.*");
                Mes.Add("*[name] is asking you if you know why [gn:" + Fluffles + "] looks at him, with her paw on the chin.*");
                if (NpcMod.HasGuardianNPC(Alex))
                {
                    Mes.Add("*[name] says that playing with [gn:"+Alex+"] and [gn:"+Fluffles+"] has been one of the most enjoyable things he has done, and asks you to join too.*");
                }
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("*[name] is telling you that [gn:" + Glenn + "] is his newest friend.*");
                Mes.Add("*[name] says that loves playing with [gn:" + Glenn + "].*");
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*[name] says that after meeting [gn:" + Cinnamon + "], he has been eating several tasty foods.*");
                Mes.Add("*[name] asks what is wrong with the seasonings he brings to [gn:" + Cinnamon + "].*");
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
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*[name] seems about scared of the ghost on your shoulders.*");
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
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*[name] woke up, and smiled upon seeing you.*";
                        case 1:
                            return "*[name] looks at you after waking up, and asks what do you need.*";
                        case 2:
                            return "*[name] seems quite tired, but stood up to see what you want.*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*[name] asks you if you did his request.*";
                        case 1:
                            return "*[name] smiles at you, and asks if you did his request.*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*[name] seems happy, and follows you.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*[name] says that is worried about the number of people in your group. He sees no way of fitting in It.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*[name] doesn't feels okay into joining your group right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*[name] seems worried about leaving the group outside of a safe place.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*[name] gives you a farewell, and tells you that had fun on the adventure.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*[name] tells you to becareful on your travel, and that will see you back at home.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*[name] seems relieved when you changed your mind.*";
                case MessageIDs.RequestAccepted:
                    return "*[name] smiles to you.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*[name] is worried because you have too many requests.*";
                case MessageIDs.RequestRejected:
                    return "*[name] seems sad.*";
                case MessageIDs.RequestPostpone:
                    return "*[name] waves you goodbye.*";
                case MessageIDs.RequestFailed:
                    return "*[name] looks at you with a sad face.*";
                case MessageIDs.RestAskForHowLong:
                    return "*[name] tells you that he seems fine with taking a rest. He's asking how long will you rest.*";
                case MessageIDs.RestNotPossible:
                    return "*[name] says that It's not possible at this moment.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*[name] wishes you a good night.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*[name] is telling you that wants to check [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*[name] tells you to wait a moment while he buys something.";
                case MessageIDs.GenericYes:
                    return "*[name] nods.*";
                case MessageIDs.GenericNo:
                    return "*[name] shook head.*";
                case MessageIDs.GenericThankYou:
                    return "*[name] seems very thankful.*";
                case MessageIDs.ChatAboutSomething:
                    return "*[name] asks what you want to talk about.*";
                case MessageIDs.NevermindTheChatting:
                    return "*[name] seems okay.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*[name] scratches his head, then asks if you really don't want to do his request anymore.*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*[name] shows you a sad face, and then say that It's fine.*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*[name] shows a little smile.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if(Main.rand.NextDouble() <= 0.5f)
                    {
                        return "*[name] thanks you all.*";
                    }
                    return "*[name] seems happy for the help.*";
                case MessageIDs.RevivedByRecovery:
                    return "*[name] says that he's fine.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*[name] seems in pain.*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*[name] is panicking because of the fire.*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*[name] can't see properly.*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*[name] is confused.*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*[name] tells that can't attack.*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*[name] seems slower.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*[name] says that is feeling down.*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*[name] had It's defense penetrated.*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*[name] is very scared of the creature ahead.*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*[name] is bothered by the ichor around him.*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*[name] is shivering.*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*[name] is stuck in a web.*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*[name] seems rabid.*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*[name] says that is tougher now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*[name] ate the food like as if didn't ate one for long.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*[name] feels stronger.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*[name] seems faster now.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*[name] feels healthier.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*[name]'s precision were enhanced.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*[name] used poison on his weapon.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*[name] tells you of the Life Crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*[name] warns you of the Pressure Plate.*";
                case MessageIDs.FoundMineTile:
                    return "*[name] warns you of the mine.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*[name] asks if can press the Detonator.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*[name] seems scared of the Bulb.*";
                case MessageIDs.FoundEterniaCrystalStandTile:
                    return "*[name] seems a bit nervous.*";
                case MessageIDs.FoundTreasureTile:
                    return "*[name] tells you of the Chest.*";
                case MessageIDs.FoundGemTile:
                    return "*[name] tells you of gems he found.*";
                case MessageIDs.FoundRareOreTile:
                    return "*[name] points at some rare ores.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*[name] shows you some very rare ores.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*[name] seems anxious to ride a minecart.*";
            }
            return base.GetSpecialMessage(MessageID);
        }
		
		public void GetTopicList()
		{
			//AddTopic("What can you tell me about your past?", DialogueRococoPast);
		}
		
		public void DialogueRococoPast()
		{
			TerraGuardian Rococo = Dialogue.DialogueParticipants[0];
			if(Rococo.FriendshipGrade < 2)
			{
				Dialogue.ShowDialogueWithContinue("*[name] seems a bit uncomfortable of speaking about that.*");
				Dialogue.ShowDialogueWithContinue("*[name] replies saying that doesn't want to recall how was his life in the Ether Realm.*");
				Dialogue.ShowEndDialogueMessage("*[name] only tells you that he likes a lot more his life here than there.*", false);
				return;
			}
			string[] Options = new string[]{"How was your life in the Ether Realm?", "How did you end up here?", "That's all I wanted to know."};
			int PickedOption = Dialogue.ShowDialogueWithOptions("*[name] tells you that doesn't like remembering that, but he may answer what you want to know.*", Options);
			OptionList:
			switch(PickedOption)
			{
				default:
					Dialogue.ShowEndDialogueMessage("*[name] seems to be recovering from the questions.*", false);
					return;
				case 0:
					//Rococo's past was like of a beggar, and with people not giving him attention on the ether realm.
					break;
				case 1:
					Dialogue.ShowDialogueWithContinue("*[name] said that after leaving the town, he wandered by several places.*");
					Dialogue.ShowDialogueWithContinue("*[name] tells that met the Guide when travelling around the world.*");
					Dialogue.ShowDialogueWithContinue("*[name] says that the Guide got spooked when he saw "+Rococo.Name+", until the Guide asked if he was friendly.*");
					Dialogue.ShowDialogueWithContinue("*[name] told that laughed very happily for meeting a human, and then was making company to him.*");
					break;
			}
			PickedOption = Dialogue.ShowDialogueWithOptions("*[name] asked you if you have any other question.*", Options);
			goto OptionList;
		}
    }
}
