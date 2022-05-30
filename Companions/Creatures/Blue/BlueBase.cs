using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using giantsummon.Trigger;

namespace giantsummon.Companions
{
    public class BlueBase : GuardianBase
    {
        public const int FullMoonBehaviorID = 0;

        public const byte RedHoodOutfitID = 1, CloaklessOutfitID = 2;
        public const string RedHoodSkinOutfitID = "red_hood_outfit", RedHoodSkinOutfitBodyFrontID = "red_hood_outfit_body_f", BunnyTextureID = "bunny";

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

        public BlueBase() //Todo - Need to work on her skin sprites.
        {
            Name = "Blue"; //Green Eyes
            Description = "It may not look like it, but she really cares about her look.\nShe constantly does her hair and paints her nail.";
            //She also likes bunnies.
            Size = GuardianSize.Large;
            Width = 26;
            Height = 82;
            SpriteWidth = 96;
            SpriteHeight = 96;
            FramesInRows = 20;
            DuckingHeight = 54;
            Scale = 99f / 82;
            CompanionSlotWeight = 1.5f;
            Age = 17;
            SetBirthday(SEASON_SPRING, 27);
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
            Roles = GuardianRoles.PopularityContestHost;
            CallUnlockLevel = 0;

            PopularityContestsWon = 4;
            ContestSecondPlace = 2;
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

            BackwardStanding = 35;
            BackwardRevive = 37;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(24, 0);
            //BodyFrontFrameSwap.Add(26, 0);
            BodyFrontFrameSwap.Add(31, 2);

            //Left Hand Position
            LeftHandPoints.AddFramePoint2x(10, 6, 14);
            LeftHandPoints.AddFramePoint2x(11, 40, 9);
            LeftHandPoints.AddFramePoint2x(12, 43, 41);

            LeftHandPoints.AddFramePoint2x(16, 12, 5);
            LeftHandPoints.AddFramePoint2x(17, 30, 7);
            LeftHandPoints.AddFramePoint2x(18, 37, 19);
            LeftHandPoints.AddFramePoint2x(19, 31, 32);

            LeftHandPoints.AddFramePoint2x(21, 43, 22);
            LeftHandPoints.AddFramePoint2x(22, 43, 31);
            LeftHandPoints.AddFramePoint2x(23, 40, 42);

            LeftHandPoints.AddFramePoint2x(33, 43, 43);

            //Right Hand Position
            RightHandPoints.AddFramePoint2x(10, 9, 14);
            RightHandPoints.AddFramePoint2x(11, 42, 9);
            RightHandPoints.AddFramePoint2x(12, 45, 41);

            RightHandPoints.AddFramePoint2x(16, 15, 5);
            RightHandPoints.AddFramePoint2x(17, 34, 7);
            RightHandPoints.AddFramePoint2x(18, 39, 19);
            RightHandPoints.AddFramePoint2x(19, 33, 32);

            RightHandPoints.AddFramePoint2x(21, 45, 22);
            RightHandPoints.AddFramePoint2x(22, 45, 31);
            RightHandPoints.AddFramePoint2x(23, 43, 42);

            //RightArmFrontFrameSwap.Add(29, 0);

            //Shoulder Position
            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(16, 16);
            MountShoulderPoints.AddFramePoint2x(11, 29, 22);
            MountShoulderPoints.AddFramePoint2x(12, 33, 29);

            MountShoulderPoints.AddFramePoint2x(20, 30, 31);
            MountShoulderPoints.AddFramePoint2x(21, 30, 31);
            MountShoulderPoints.AddFramePoint2x(22, 30, 31);

            //Sitting Point
            SittingPoint = new Point(21 * 2, 37 * 2);

            //Armor Head Points
            HeadVanityPosition.DefaultCoordinate2x = new Point(21 + 1, 12 - 2);
            HeadVanityPosition.AddFramePoint2x(11, 33 - 1, 17);
            HeadVanityPosition.AddFramePoint2x(12, 38 - 1, 24);
            HeadVanityPosition.AddFramePoint2x(20, 38 - 1, 24);
            HeadVanityPosition.AddFramePoint2x(21, 38 - 1, 24);
            HeadVanityPosition.AddFramePoint2x(22, 38 - 1, 24);
            HeadVanityPosition.AddFramePoint2x(23, 38 - 1, 24);
            HeadVanityPosition.AddFramePoint2x(33, 36 + 1, 27 - 2);

            HeadVanityPosition.AddFramePoint2x(30, -1000, -1000);
            HeadVanityPosition.AddFramePoint2x(31, -1000, -1000);

            //Wing
            WingPosition.DefaultCoordinate2x = new Point(22, 21);

            SetupOutfits();
            GetRewards();
        }

        public void SetupOutfits()
        {
            AddOutfit(RedHoodOutfitID, "Red Hood Traveller", delegate (GuardianData gd, Player player) { return gd.FriendshipLevel >= 5; }); //TODO - Need to add alternative way of getting those outfits.
            AddOutfit(CloaklessOutfitID, "Cloakless Traveller Outfit", delegate (GuardianData gd, Player player) { return gd.FriendshipLevel >= 5; });
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(RedHoodSkinOutfitID, "red_hood_outfit");
            sprites.AddExtraTexture(RedHoodSkinOutfitBodyFrontID, "red_hood_outfit_body_f");
            sprites.AddExtraTexture(BunnyTextureID, "bunny");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            bool DrawBunny = HasBunnyInInventory(guardian);
            byte BunnyFrame = 0, BunnyFrontFrame = 0;
            bool DrawBunnyFront = true;
            Texture2D BunnyTexture = sprites.GetExtraTexture(BunnyTextureID);
            if (DrawBunny)
            {
                if (guardian.BodyAnimationFrame == 30)
                {
                    BunnyFrame = 1;
                    DrawBunnyFront = false;
                }
                if (guardian.BodyAnimationFrame == 31)
                {
                    BunnyFrame = 2;
                    BunnyFrontFrame = 1;
                }
            }
            if (guardian.OutfitID > 0)
            {
                Rectangle BodyRect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                    LeftArmRect = guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame),
                    RightArmRect = guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame);
                GuardianDrawData gdd;
                Texture2D texture = sprites.GetExtraTexture(RedHoodSkinOutfitID),
                    bodyfronttexture = sprites.GetExtraTexture(RedHoodSkinOutfitBodyFrontID);
                const int TextureGap = 96 * 2;
                bool Hoodless = guardian.OutfitID == CloaklessOutfitID;
                bool DrawFront = false;
                int CloakAnimationFrame = guardian.BodyAnimationFrame;
                if(!Hoodless && (TerraGuardian.FaceSlot > 0 || TerraGuardian.HeadSlot > 0))
                {
                    if(CloakAnimationFrame < 11 || (CloakAnimationFrame >= 16 && CloakAnimationFrame <= 20) || (CloakAnimationFrame >= 24 && CloakAnimationFrame <= 26) || CloakAnimationFrame == 29)
                    {
                        CloakAnimationFrame = 13;
                    }
                    else if ((CloakAnimationFrame >= 20 && CloakAnimationFrame <= 23) || CloakAnimationFrame == 33)
                    {
                        CloakAnimationFrame = 14;
                    }
                    else if (CloakAnimationFrame == 27 || CloakAnimationFrame == 31)
                    {
                        CloakAnimationFrame = 15;
                    }
                }
                for (int f = TerraGuardian.DrawBehind.Count - 1; f >= 0; f--)
                {
                    switch (TerraGuardian.DrawBehind[f].textureType)
                    {
                        case GuardianDrawData.TextureType.TGHead:
                            {
                                if (!Hoodless)
                                {
                                    Rectangle rect = BodyRect;
                                    //Head
                                    rect.Y += 8 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGHead, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                            }
                            break;
                        case GuardianDrawData.TextureType.TGBody:
                            {
                                Rectangle rect = BodyRect;
                                if (!Hoodless)
                                {
                                    //Head
                                    rect = guardian.GetAnimationFrameRectangle(CloakAnimationFrame);
                                    rect.Y += 4 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                    //Cloak Right Arm
                                    rect = RightArmRect;
                                    rect.Y += 5 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                                //Shirt
                                rect = BodyRect;
                                rect.Y += 3 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                //Pants
                                rect = BodyRect;
                                rect.Y += 2 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                //Shoes
                                rect = BodyRect;
                                rect.Y += 1 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                        case GuardianDrawData.TextureType.TGLeftArm:
                            {
                                Rectangle rect;
                                if (!Hoodless)
                                {
                                    //Head
                                    rect = guardian.GetAnimationFrameRectangle(CloakAnimationFrame);
                                    rect.Y += 8 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                    //Cloak Front
                                    rect = LeftArmRect;
                                    rect.Y += 7 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                                //Shirt Sleeve
                                rect = LeftArmRect;
                                rect.Y += 6 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                        case GuardianDrawData.TextureType.TGRightArm:
                            {
                                if (!Hoodless)
                                {
                                    Rectangle rect = RightArmRect;
                                    //Cloak Right Arm Back
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f - 1, DrawFront);
                                }
                            }
                            break;
                        case GuardianDrawData.TextureType.TGBodyFront:
                            {
                                //Shirt
                                Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.Base.GetBodyFrontSprite(guardian.BodyAnimationFrame));
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, bodyfronttexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                    }
                }
                DrawFront = true;
                for (int f = TerraGuardian.DrawFront.Count - 1; f >= 0; f--)
                {
                    switch (TerraGuardian.DrawFront[f].textureType)
                    {
                        case GuardianDrawData.TextureType.TGHead:
                            {
                                if (!Hoodless)
                                {
                                    Rectangle rect = BodyRect;
                                    //Head
                                    rect.Y += 8 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGHead, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                            }
                            break;
                        case GuardianDrawData.TextureType.TGBody:
                            {
                                Rectangle rect = BodyRect;
                                if (!Hoodless)
                                {
                                    //Head
                                    rect = guardian.GetAnimationFrameRectangle(CloakAnimationFrame);
                                    rect.Y += 4 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                    //Cloak Right Arm
                                    rect = RightArmRect;
                                    rect.Y += 5 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                                //Shirt
                                rect = BodyRect;
                                rect.Y += 3 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                //Pants
                                rect = BodyRect;
                                rect.Y += 2 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                //Shoes
                                rect = BodyRect;
                                rect.Y += 1 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                        case GuardianDrawData.TextureType.TGLeftArm:
                            {
                                Rectangle rect;
                                if (!Hoodless)
                                {
                                    //Head
                                    rect = guardian.GetAnimationFrameRectangle(CloakAnimationFrame);
                                    rect.Y += 8 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                    //Cloak Front
                                    rect = LeftArmRect;
                                    rect.Y += 7 * TextureGap;
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f, DrawFront);
                                }
                                //Shirt Sleeve
                                rect = LeftArmRect;
                                rect.Y += 6 * TextureGap;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGLeftArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                        case GuardianDrawData.TextureType.TGRightArm:
                            {
                                if (!Hoodless)
                                {
                                    Rectangle rect = RightArmRect;
                                    //Cloak Right Arm Back
                                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGRightArm, texture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                    guardian.AddDrawDataAfter(gdd, f - 1, DrawFront);
                                }
                            }
                            break;
                        case GuardianDrawData.TextureType.TGBodyFront:
                            {
                                //Shirt
                                Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.Base.GetBodyFrontSprite(guardian.BodyAnimationFrame));
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, bodyfronttexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                                guardian.AddDrawDataAfter(gdd, f, DrawFront);
                            }
                            break;
                    }
                }
            }
            if (DrawBunny)
            {
                Rectangle rect = guardian.GetAnimationFrameRectangle(BunnyFrame),
                    frontrect = guardian.GetAnimationFrameRectangle(BunnyFrontFrame);
                Vector2 BunnyPos = DrawPosition;
                BunnyPos.Y -= (guardian.Height * (guardian.Scale - 1f)) * 0.5f;
                BunnyPos.X += (guardian.Width * (guardian.Scale - 1f)) * guardian.Direction;
                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BunnyTexture, BunnyPos, rect, color, Rotation, Origin, 1f, seffect);
                if(guardian.BodyAnimationFrame == 36)
                {
                    InjectTextureBefore(GuardianDrawData.TextureType.TGBody, gdd);
                }
                else
                {
                    InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                    if (DrawBunnyFront)
                    {
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BunnyTexture, BunnyPos, frontrect, color, Rotation, Origin, 1f, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGRightArmFront, gdd);
                    }
                }
                //guardian.AddDrawDataAfter(gdd, f, DrawFront);
            }
        }

        public void GetRewards()
        {
            AddReward(Terraria.ID.ItemID.FlaskofPoison, 3, 0.66f);
            AddReward(Terraria.ID.ItemID.NaturesGift, 1, 0.05f);
            AddReward(Terraria.ID.ItemID.CookedMarshmallow, 2, 0.7f);
            AddReward(Terraria.ID.ItemID.NeonTetra, 3, 0.2f);
            AddReward(Terraria.ModLoader.ModContent.ItemType<Items.Accessories.BlueHairclip>(), 1, 0.1f);
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
        
        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "*Hello Terrarian. If I'm a Werewolf? What is a Werewolf?*";//"\"Is... That... a Werewolf? I don't think so... It's a taller one?\"";
                case 1:
                    return "*Hi. I didn't expected to meet someone else here.*";//"\"As soon as I got closer to it, that... Wolf? Friendly waved at me.\"";
                case 2:
                    return "*Are you here for camping too? I love sitting by the fire sometimes.*"; //"\"She is asking me If I'm camping too.\"";
                default:
                    return "*Hello. I was about to setup a campfire here, want to join in?*"; //"\"She seems to be enjoying the bonfire, until I showed up.\"";
            }
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I don't want anything right now. Come back later and I may be in need of something.*"; //"*[name] told me that she wants nothing right now.*";
            return "*No, I'm not in need of anything right now.*"; //"*[name] shaked her head and then returned to what she was doing.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I'm so glad you asked. I really need a thing done, but I'm already busy with something else, if you could help me... This is my problem, if you ask: [objective]*"; //"*She seems to want something.*";
            return "*I'm so happy you asked! Here, check this: \"[objective]\". Will you do it?*"; //"*As soon as I asked if she wanted something, she gave me a list.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I'm so happy that you managed to do that. Thank you, [nickname].*"; //"*[name] got overjoyed after I fullfilled her request.*";
            return "*I'm so happy that I could kiss you. Thanks!* (She's wagging her tail while smiling)"; //"*She got so happy that smiles at you, and wags her tail.*";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            bool ZacksRecruited = PlayerMod.PlayerHasGuardian(player, 3) || NpcMod.HasMetGuardian(3);
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("*Yes, [nickname]. Need something?*"); //"*[name] is looking at me with a question mark face, while wondering what you want.*");
                Mes.Add("*I'm so happy to see you.*"); //"*[name] looks to me while smiling.*");
                if (player.head == 17)
                {
                    Mes.Add("*That's one cute little hood you got. Makes me want to hug you.*");//"*[name] says that you look cute with that hood.*");
                }
                if (MainMod.IsPopularityContestRunning)
                {
                    Mes.Add("*Hey [nickname]. Did you knew that the TerraGuardians Popularity Contest is running right now? Be sure to vote some time.*");
                    Mes.Add("*The TerraGuardians Popularity Contest is currently running. You can access the voting by speaking to me about it.*");
                }
                Mes.Add("*Have you heard Deadraccoon5? Oh, I feel bad for you right now.*");
            }
            else
            {
                Mes.Add("*Grrr.... What do you want?*"); //"*[name] is growling and showing her teeth as I approached her.*");
                Mes.Add("*Have you came to annoy me?!* (Her facial expression is very scary. I should avoid talking to her.)"); //"*[name]'s facial expressions is very scary, I should avoid talking to her at the moment.*");
            }
            if (!ZacksRecruited)
            {
                if (!Main.bloodMoon)
                {
                    if (Main.raining)
                        Mes.Add("*This weather was a lot better when I was with...*"); //"*[name] looks sad.*");
                    if (!Main.dayTime)
                        Mes.Add("*Awooooo. Snif~ Snif~* (She looks in sorrow)");//"*[name] howls to the moon, showing small signs of sorrow.*");
                }
                else
                {
                    if (!Main.dayTime)
                        Mes.Add("*I'm feeling the presence of... [nickname], could you take me and check the border of this world? I think someone I'm looking for may be found there.*"); //"*[name] is saying that she is feeling a familiar presence, coming from the far lands of the world. Saying that we should check.*");
                }
            }
            if (!Main.bloodMoon)
            {
                switch (guardian.OutfitID)
                {
                    case RedHoodOutfitID:
                        Mes.Add("*I really love this outfit! I feel very much into starting a new adventure when wearing this.*"); //"*[name] is saying that she likes that outfit. She also tells you that feels very adventurous when wearing It.*");
                        break;
                    case CloaklessOutfitID:
                        Mes.Add("*I really love this outfit, but I preffer using my cloak too...*"); //"*[name] says that likes this outfit, but would like having her cloak on too.*");
                        break;
                }
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*Yawn... Feeling sleepy, [nickname]? Me too.*"); //"*[name] looks sleepy.*");
                    Mes.Add("*Why I'm circling the room? I... Have no idea..*"); //"*[name] is circling the room, I wonder what for.*");
                }
            }
            else
            {

            }
            switch (guardian.OutfitID)
            {
                case RedHoodOutfitID:
                    Mes.Add("*Now I'm ready for adventure.*"); //"*[name] says that now she's ready for adventure.*");
                    Mes.Add("*This outfit has everything: It's comfy and has style. What else could I want?*"); //"*[name] is saying that she finds this outfit comfy and style.*");
                    Mes.Add("*The cloak is the most important part of this outfit. I'd feel naked without it.*"); //"*[name] is saying that the cloak is the most important part of her outfit.*");
                    Mes.Add("*Hey, [nickname]. What do you think of my outfit?*"); //"*[name] asks what you think of her outfit.*");
                    break;
                case CloaklessOutfitID:
                    Mes.Add("*Now I'm ready for adventure.*"); //"*[name] says that now she's ready for adventure.*");
                    Mes.Add("*This outfit doesn't feel the same without the cloak...*"); //"*[name] seems to be missing the cloak.*");
                    Mes.Add("*Hey, [nickname]. What do you think of my outfit?*"); //"*[name] asks what you think of her outfit.*");
                    break;
            }
            Mes.Add("*There was a weird Terrarian I met once, named beaverrac. It was so weird that he didn't even tried to speak to me, but I can't blame him, since a lot of weird things were happening.*"); //"*[name] tells you of a Terrarian she met, named beaverrac. She said that found weird that he didn't talked with her, beside there were a lot of weird things happening around too.");
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*Let's dance, [nickname].* (She's stealing all the spotlights of the party.)"); //"*[name] is stealing all the spotlights of the party.*");
                if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
                {
                    Mes.Add("*Hey [gn:3], let's dance!*"); //"*[name] is calling [gn:3] for a dance.*");
                    Mes.Add("(She's is dancing with [gn:3], they seems to be enjoying.)");
                }
            }
            if ((guardian.ID != 2 || guardian.ModID != MainMod.mod.Name) && !PlayerMod.PlayerHasGuardian(player, 2))
            {
                Mes.Add("*I'm so bored... I want to play a game, but nobody seems good enough for that...*"); //"*[name] is bored. She would like to play a game, but nobody seems good for that.*");
            }
            if (guardian.ID == 3 && guardian.ModID == MainMod.mod.Name && PlayerMod.PlayerHasGuardian(player, 2))
                Mes.Add("(First, [name] called [gn:3] to play a game, now they are arguing about what game they want to play. Maybe I should sneak away very slowly.)");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3, MainMod.mod) && PlayerMod.PlayerHasGuardian(player, 2))
                Mes.Add("*Hey, [nickname], may I borrow [gn:3] for a few minute? I want to play a game with [gn:2] and would love having his company.*"); //"*[name] is asking if she could borrow [gn:3] for a minute, so they could play a game with [gn:2].*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.WitchDoctor))
                Mes.Add("(She seems to be playing with flasks of poison.)");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                Mes.Add("*Check out my hair. I visitted [nn:"+Terraria.ID.NPCID.Stylist+"] and she did wonders to it.*"); //"*[name] wants you to check her hair.*");
            if (NpcMod.HasGuardianNPC(0))
                Mes.Add("*I really don't like talking to [gn:0], he's childish and annoying. I feel like I babysit him.*"); //"*[name] seems to be complaining about [gn:0], saying he's childish and annoying.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("*Urgh... You came too... Nice...* (She doesn't seems to like having [gn:0]'s presence.)"); //"*[name]'s mood goes away as soon as she saw [gn:0].*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
                Mes.Add("*Oh, hello. I'm glad to see you and [gn:3] visitting me...* (She looks a bit saddened)"); //"*[name] said that she feels good for knowing that [gn:3] is around, but she also looks a bit saddened.*");
            if (NpcMod.HasGuardianNPC(2))
                Mes.Add("*My teeth are itching right now. Do you know where [gn:2] is?*"); //"*[name] is saying that wants to bite something, and is asking If I've seen [gn:2] somewhere.*");
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
                Mes.Add("*Hey [gn:2], wanna play a game?* ([gn:2] ran away)"); //"*[name] said that she wants to play. For some reason, [gn:2] ran away.*");
            if (NpcMod.HasGuardianNPC(2) && NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("(She is watching [gn:2] and [gn:5] playing together, with a worry face.)");
                Mes.Add("*Ever since [gn:5] arrived, I didn't had much chances of playing with [gn:2]...*"); //"*[name] says that didn't had much chances to play with [gn:2], since most of the time he ends up playing with [gn:5].*");
            }
            if (NpcMod.HasGuardianNPC(5) && !PlayerMod.PlayerHasGuardianSummoned(player, 5))
            {
                Mes.Add("(She's is whistling, like as if was calling a dog, and trying to hide the broom she's holding on her back.)");
                Mes.Add("*Alright, do tell that mutt [gn:5] that the next time he leaves a smelly surprise in my front door, I'll show him how resistant to impact is my broom!*"); //"*[name] is telling me that the next time [gn:5] leaves a smelly surprise on her front door, she'll chase him with her broom.*");
            }
            if (NpcMod.HasGuardianNPC(7))
                Mes.Add("*I really hate when [gn:7] interrupts me, when I'm playing with [gn:2]. She's just plain boring.*"); //"*[name] says that really hates when [gn:7] interrupts when playing with [gn:2].*");
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*The audacity [gn:8] have... Insulting my looks in my presence! How she dares!*"); //"*[name] is angry, because [gn:8] insulted her hair earlier.*");
                Mes.Add("*Who does [gn:8] think she is? I'm prettier than her!*"); //"*[name] is complaining about [gn:8], asking who she thinks she is.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Leopold))
            {
                Mes.Add("*I'm really happy for having [gn:10] around my arms... I mean... Around. Yes, around.*"); //"*[name] is very happy for having [gn:10] around.*");
            }
            if (NpcMod.HasGuardianNPC(Vladimir))
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer) && NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                {
                    Mes.Add("*[nn:"+Terraria.ID.NPCID.Nurse+"] came earlier to me, asking for tips for her date with [nn:"+Terraria.ID.NPCID.ArmsDealer+"]. Of course I had the perfect tip, I hope she executes it well.*"); //"*[name] tells that [nn:" + Terraria.ID.NPCID.Nurse + "] appeared earlier, asking for tips on what to do on a date with [nn:"+Terraria.ID.NPCID.ArmsDealer+"]. She said that she gave some tips that she can use at that moment.*");
                }
                if (!NpcMod.HasGuardianNPC(Zacks))
                    Mes.Add("*Hey. Say... Have you seen [gn:"+Vladimir+"]? I... Really need to see him...* (She seems to be wiping some tears from her face)"); //"*[name] asks If you have seen [gn:"+Vladimir+"], after removing a tear from her face. She seems to need to speak with him.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Michelle))
            {
                Mes.Add("*I really hate when [gn:"+Michelle+"] pets my hair, she ruins my haircare.*"); //"*[name] says that hates when [gn:" + GuardianBase.Michelle + "] pets her hair.*");
                Mes.Add("*I keep telling [gn:"+Michelle+"] that I need some space, but she just don't get it!*"); //"*[name] is saying taht needs some space, but [gn:" + GuardianBase.Michelle + "] doesn't get it.*");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Malisha))
            {
                Mes.Add("(She seems to have tried casting some spell on you) *Hm... It didn't worked. Did I do it right? Better I research* (The book cover says something about polymorphing.)"); //"*[name] seems to have casted some kind of spell on you, but It didn't seem to work. With a disappointment look, she tells herself that needs to research some more.*");
                if(!PlayerMod.PlayerHasGuardian(Main.LocalPlayer, Zacks))
                    Mes.Add("(She seems to be reading some kind of magic book.)");
                else
                    Mes.Add("(She seems focused into reading books about necromancy and biology.)");
            }
            if (NpcMod.HasGuardianNPC(Fluffles))
            {
                Mes.Add("*I really enjoy having [gn:"+Fluffles+"] around. She always comes up to check up if I'm fine.*"); //"*[name] seems to be enjoying having [gn:" + Fluffles + "] around. They seems to be get along very well.*");
                Mes.Add("*I've been sharing some beauty tips with [gn:"+Fluffles+"]. Beside she can't speak, she managed to teach me some new tips related to that.*"); //"*[name] told you that she's sharing some beauty tips with [gn:" + Fluffles + "]. She said that learned something new with that.*");
                if (NpcMod.HasGuardianNPC(Sardine))
                {
                    Mes.Add("*Playing Cat and Wolf with [gn:"+Sardine+"] got more fun after I invited [gn:"+Fluffles+"] to play too. She often catches him off guard, but that kind of makes the game easier.*"); //"*[name] says that always teams up with [gn:"+Fluffles+"] to catch [gn:"+Sardine+"] on Cat and Wolf. [gn:"+Fluffles+"] catches him off guard more easier than her, but she also said that the game got easier too.*");
                }
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*I'm not in the mood now.... Grr....* (She seems to have came angry from [gn:17]'s place. I wonder what happened.)"); //"*[name] seems to have came from [gn:17]'s place angry. I wonder what happened.*");
                Mes.Add("(She seems to be eating a Squirrel on a Spit.) Oh, hi. I'm just nibbling something.");
            }
            if (NpcMod.HasGuardianNPC(Luna))
            {
                Mes.Add("*I'm so happy to have [gn:" + Luna + "] around. She has so many good points.*");
                Mes.Add("*Sometimes, [gn:" + Luna + "] and I compare whose fur has better texture.*");
            }
            if (NpcMod.HasGuardianNPC(Cille))
            {
                Mes.Add("*I greeted [gn:" + Cille + "] the other day, but she told me to go away.*");
                Mes.Add("*There is something wrong with [gn:" + Cille + "], I visited her some night, and she attacked me! Then the other day, she was back to being the shy person we know. What is wrong with her?*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*[nickname], this is embarrassing... Couldn't you talk to me other time?*"); //"*[name] is saying that you're making her embarrassed.*");
                Mes.Add("*Uh... Could you turn the other way... If you want to talk?*"); //"*[name] would like you to turn the other way, If you want to talk.*");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                if (NpcMod.HasGuardianNPC(GuardianBase.Zacks))
                {
                    Mes.Add("*Yes, I don't mind sharing my room with you, but... I wonder if [gn:"+Zacks+"] will not mind if I do, too.*"); //"*[name] says that doesn't really mind sharing the room with you, but wonder if [gn:"+GuardianBase.Zacks+"] wont mind It.*");
                }
                else
                {
                    Mes.Add("*I don't really mind sharing my room with you. It's fine.*"); //"*[name] says that doesn't really mind sharing the room with you.*");
                }
                Mes.Add("*Sharing the bed with you may be hard, since I'm... You know... Bigger than you... And the bed...*"); //"*[name] tells you that may be hard to share the same bed with you.*");
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Zacks))
            {
                Mes.Add("*So... [gn:"+Zacks+"] is sharing his room with you? I wonder... Why doesn't he shares him room with me...?* (She seems to have gotten a bit saddened)"); //"*[name] asked if you're sharing room with [gn:"+Zacks+"], she then says that may wonder why he wouldn't want to share his room with her, and then got saddened.*");
                Mes.Add("*You're sharing room with [gn:"+Zacks+"]? No... It's nothing. It's fine.*"); //"*[name] asks if you're sharing room with [gn:"+Zacks+"], then she asked if he's fine.*");
            }
            if (Main.moonPhase == 0 && !Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("*I'm sorry for calling your attention, [nickname]. I wasn't actually calling you.*"); //"*[name] apologizes, saying that she wasn't calling you at the moment.*");
                Mes.Add("(She's staring at the moon.)");
            }
            if (HasBunnyInInventory(guardian))
            {
                Mes.Add("*How did you knew I loved bunnies? I really loved this gift. Thank you.*"); //"*[name] asks how did you know, and tells you that she loved the pet you gave her.*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("*[name] seems to be breathing hard, she must be feeling pain.*");
                Mes.Add("*[name] seems to be unconscious.*");
                if (!ZacksRecruited)
                {
                    Mes.Add("*[name] said a name, Za... Before passing out.*");
                }
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                if (!ZacksRecruited)
                {
                    Mes.Add("*Where are you.... I miss you.... Why did you left me.... Zzzz....*");
                    Mes.Add("*No... Come back... Don't go.... Zzzz...*");
                    Mes.Add("(You can see some tears on [name]'s face.)");
                }
                else
                {
                    Mes.Add("(She seems to be sleeping fine.)");
                    Mes.Add("(She looks a bit worried, while in her sleep.)");
                    Mes.Add("(She seems to be having a dream with [gn:"+3+"].)");
                }
                if (PlayerMod.PlayerHasGuardian(player, Sardine))
                    Mes.Add("*Run all you want... I'll catch you.... Nibble nibble...* (She must be dreaming that she's playing with [gn:"+Sardine+"].)"); //"*[name] just said \"I'm going to catch you\", she must be dreaming that she's playing with [gn:" + Sardine + "].*");
                Mes.Add("(She seems to be dreaming about camping with other people.)");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Uh... T-t-there's someone.... Behind you...*"); //"*[name] stares with a scared face at the ghost behind you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'd like living among other people, I also like having my own place, too.*"); //"*[name] would like to move to where people are, give her a house there.*");
            Mes.Add("*I'm tired of spending the nights on the woods. Could you build me a house somewhere?*"); //"*[name] is tired of living alone in the woods, build her a house somewhere.*");
            if (!Main.dayTime)
                Mes.Add("*I can't sleep well at night, with my attention shifting due to weird noises surrounding me.*"); //"*[name] would like to sleep well at night, without worrying about the dangers of the night.*");
            if (Main.raining)
                Mes.Add("*This isn't good for my fur, and my hairdress is ruined. Please, give me a home.*"); //"*[name] is really worried about the effects of the rain on her fur, give her some place to live.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
                Mes.Add("*Oh, you brought [gn:0] with you... Do you have some \"Naggicide\" too?*"); //"*[name] is asking me if she knows any good \"Naggicide\", why? Because she wants to use it on that guy following you.*");
            if (NpcMod.HasGuardianNPC(0) && WorldMod.GuardianTownNPC[NpcMod.GetGuardianNPC(0)].Distance(player.Center) < 1024f)
                Mes.Add("*Mind sending [gn:0] somewhere far away from me?*"); //"*[name] is asking if you could send [gn:0] some place far away from her.*");
            if (NpcMod.HasGuardianNPC(3) && WorldMod.GuardianTownNPC[NpcMod.GetGuardianNPC(3)].Distance(player.Center) >= 768f)
                Mes.Add("*Say, [gn:3] is living here too, right? Could I move to somewhere close to him?*"); //"*[name] would like to move to somewhere closer to [gn:3].*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.WitchDoctor))
                Mes.Add("*Tell me, what is your favorite type of poison?*"); //"*[name] is asking you what is your favorite type of poison.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*What do you think of what I did to my room?*"); //"*[name] is asking what you think about what she did with her room.*");
                Mes.Add("*I'd like to travel the world with you. Take me some time.*"); //"*[name] wants to travel the world with you.*");
                Mes.Add("*Would you mind helping me move some furnitures?*"); //"*[name] asks if you want to help her move some furnitures.*");
                Mes.Add("*Those fleas are killing me. Do you have some remedy to kill them?*"); //"*[name] is asking if you have any flea killing remedy.*");
            }
            else
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                    Mes.Add("*I want to visit [nn:"+Terraria.ID.NPCID.Stylist+"] some time.*"); //"*[name] wants to visit [nn:" + Terraria.ID.NPCID.Stylist + "] sometime.*");
                if (Main.moonPhase == 0 )
                {
                    if (!PlayerMod.PlayerHasGuardian(player, 3))
                    {
                        Mes.Add("*I'm sorry... I'm just missing.... Someone...*"); //"*[name] seems to be missing someone.*");
                    }
                    else
                    {
                        Mes.Add("*I always love full moons, because they remind me of [gn:3].*"); //"*[name] said that the full moon always reminds her of [gn:3].*");
                    }
                }
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*Hey [gn:2], wanna play a game?* ([gn:2] is panicking right now.)"); //"*[name] said that she wants to play a game with [gn:2], causing him to panic for some reason.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 3))
            {
                Mes.Add("*It's... It's so good to see you again, [gn:3]...* (She looks relieved for seeing [gn:3], but also looks a bit saddened.)"); //"*[name] got a bit saddened when she saw [gn:3], but feels a bit relieved for seeying him.*");
            }
            else if (PlayerMod.PlayerHasGuardian(player, 3))
            {
                Mes.Add("*I wonder.... Is there some way of bringing [gn:3] to his old self?*"); //"*[name] keeps wondering if there is a way of bringing [gn:3] to his old self.*");
            }
            if (NpcMod.HasGuardianNPC(3))
                Mes.Add("*I admit. I initially came to your world looking for [gn:3], but after seeing how beautiful the environment here is, I decided to stay for longer. Since [gn:3] is here, we can then stay for even longer.*"); //"*[name] says that initially she came to the world looking for [gn:3], but after seeying how beautiful the environment is, she decided to stay more. And since [gn:3] is here, she can stay for longer.*");
            Mes.Add("*Want to go shopping, [nickname]? So... Would you like lending me some coins too?*"); //"*[name] wants to go shopping, and is asking if you would lend some coins.*");
            if (Main.bloodMoon)
                Mes.Add("*I'm so furious right now, that I could kill someone! I'm so glad outside has many options.*"); //"*[name] is so furious right now that she could kill someone, good thing that outside has many options.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 1))
            {
                if (Main.rand.Next(2) == 0)
                    return "*Hi... So.... Uh... Hows the day...?* (She's trying so hard to not ask what you will give her as a gift)"; //"*[name] is trying so hard to not ask what you will give her as a gift.*";
                return "*If you gift me a leash, prepare to be beaten up by me.*"; //"*[name] said that she will beat you up if her gift is a leash.*";
            }
            return "(She's dancing away.)";
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            if (trigger == TriggerTypes.Spotted && Sender.TargetType == TriggerTarget.TargetTypes.NPC)
            {
                NPC npc = Main.npc[Sender.TargetID];
                if (npc.type == Terraria.ModLoader.ModContent.NPCType<Npcs.ZombieGuardian>())
                {
                    if ((guardian.OwnerPos > -1 && PlayerMod.PlayerHasGuardian(Main.player[guardian.OwnerPos], GuardianBase.Zacks)) || NpcMod.HasMetGuardian(GuardianBase.Zacks))
                    {
                        guardian.SaySomething("*Hello again, Zacks.*");
                    }
                    else
                    {
                        switch (Main.rand.Next(3))
                        {
                            case 0:
                                guardian.SaySomething("*What? No! No no no! It can't be happening. Zacks!*");
                                break;
                            case 1:
                                guardian.SaySomething("*Zacks! What happened to you? It's me! Blue!*");
                                break;
                            case 2:
                                guardian.SaySomething("*Zacks? Is that you? Zacks, look at me! Zacks!!*");
                                break;
                        }
                    }
                }
            }
            if (trigger == TriggerTypes.Death && Sender.TargetType == TriggerTarget.TargetTypes.NPC)
            {
                NPC npc = Main.npc[Sender.TargetID];
                if (npc.type == Terraria.ModLoader.ModContent.NPCType<Npcs.ZombieGuardian>())
                {
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            guardian.SaySomething("*Zacks... We'll find a way of opening your eyes...*");
                            break;
                        case 1:
                            guardian.SaySomething("*No! That can't be It! There must be a way of saving him!*");
                            break;
                        case 2:
                            guardian.SaySomething("*No! Zacks!! Nooooo!!! Don't leave me again.*");
                            break;
                    }
                }
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        private static bool HasBunnyInInventory(TerraGuardian tg)
        {
            bool IsGolden;
            return HasBunnyInInventory(tg, out IsGolden);
        }

        private static bool HasBunnyInInventory(TerraGuardian tg, out bool IsGolden)
        {
            for (int i = 0; i < 10; i++)
            {
                if (tg.Inventory[i].type == Terraria.ID.ItemID.Bunny || tg.Inventory[i].type == Terraria.ID.ItemID.GoldBunny)
                {
                    IsGolden = tg.Inventory[i].type == Terraria.ID.ItemID.GoldBunny;
                    return true;
                }
            }
            IsGolden = false;
            return false;
        }

        public override void GuardianAnimationScript(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            bool HasBunnyInInventory = BlueBase.HasBunnyInInventory(guardian);
            if (HasBunnyInInventory && guardian.BodyAnimationFrame != DownedFrame)
            {
                if(guardian.BodyAnimationFrame == BackwardStanding)
                {
                    guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame = 36;
                }
                else if (guardian.BodyAnimationFrame != DuckingFrame && guardian.BodyAnimationFrame != ThroneSittingFrame && guardian.BodyAnimationFrame != BedSleepingFrame)
                {
                    const int BunnyHoldingFrame = 29;
                    if (guardian.BodyAnimationFrame == StandingFrame)
                        guardian.BodyAnimationFrame = BunnyHoldingFrame;
                    //Todo - Add the throne sitting animation here.
                    if (!UsingLeftArm || (guardian.PlayerMounted && ((guardian.Base.PlayerMountedArmAnimation > -1 && guardian.LeftArmAnimationFrame == guardian.Base.PlayerMountedArmAnimation) || guardian.LeftArmAnimationFrame == guardian.Base.JumpFrame)))
                    {
                        guardian.LeftArmAnimationFrame = BunnyHoldingFrame;
                    }
                    if (!UsingRightArm)
                    {
                        guardian.RightArmAnimationFrame = BunnyHoldingFrame;
                    }
                }
                else if (guardian.BodyAnimationFrame == BedSleepingFrame)
                {
                    guardian.BodyAnimationFrame = guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = 30;
                }
                else if (guardian.BodyAnimationFrame == ThroneSittingFrame)
                {
                    guardian.BodyAnimationFrame = guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = 31;
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

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (!guardian.DoAction.InUse)
            {
                if (Main.moonPhase == 0 && !Main.bloodMoon && Main.time >= 3600)
                {
                    if (guardian.OwnerPos == -1 || guardian.IsPlayerIdle)
                    {
                        guardian.StartNewGuardianAction(new Companions.Blue.FullMoonHowlingBehavior(), FullMoonBehaviorID);
                    }
                }
            }
        }

        public override string CompanionRecruitedMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Zacks)
                {
                    Weight = 1.5f;
                    return "*I'm so glad that you joined us, "+WhoJoined.Name+".*";
                }
            }
            Weight = 1f;
            return "*Amazing, a new person!*";
        }

        public override string CompanionJoinGroupMessage(TerraGuardian WhoReacts, GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Zacks:
                        Weight = 1.5f;
                        return "*You'll join us, "+WhoJoined.Name+"? I'm happy for having your persence.*";
                    case Rococo:
                        Weight = 1.2f;
                        return "*You're joining too... Great...*";
                    case Sardine:
                        Weight = 1.2f;
                        return "*Perfect, my teeth were in need of biting something.*";
                }
            }
            Weight = 1f;
            return "*Hello.*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*You picked me as your buddy? Yes, I can be your buddy.*";
                case MessageIDs.RescueMessage:
                    return "*I heard your call. Let me to help you.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*Yes? Yawn~ What do you want? I hope was important.*"; //"*She woke up, and asked if what you wanted to say was important. Then yawned...*";
                        case 1:
                            return "*So... Whatever you want, couldn't wait until I wake up?*"; //"*She asked if whatever you wanted couldn't wait?*";
                        case 2:
                            return "*Yawn~... What do you want, [nickname]?*"; //"*She asks what you want, after yawning?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Why you woke me up? Is it about my request?*"; //"*She asks why you woke her up... If Is It about my request?*";
                        case 1:
                            return "*Yawn~... Finished my request? I really wanted to sleep some more, by the way...*"; //"*She yawns and asks if you finished her request. Said then that she really wanted to get some more sleep.*";
                    }
                    break;
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I was getting bored of staying at home, anyways. Let's go on an adventure!*"; //"*She's telling that was feeling bored of staying at home, and joins your adventure.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I dislike crowds.*"; //"*She said that dislikes crowd.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*I'm not interessed in going on an adventure right now.*"; //"*She tells you that is not interessed in going on an adventure right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*You really want to leave me here?*"; //"*She asks if you really want to leave her here.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    if (!NpcMod.HasMetGuardian(Zacks))
                    {
                        return "*Farewell, [nickname]. Remember to find the other wolf I seek.*"; //"*She gives you a farewell, and reminds you to find the other wolf she's looking for.*";
                    }
                    else
                    {
                        return "*Farewell, and have a good adventure.*"; //"*She gives you a farewell, and wishes you a good adventure.*";
                    }
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Well, I think It will be entertaining slashing my way back home. See you there, [nickname].*"; //"*She says that may be entertaining slashing her way back home.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Phew...*"; //"*She sounds relieved.*";
                case MessageIDs.RequestAccepted:
                    return "*Be careful when doing my request, [nickname].*"; //"*She tells you to becareful when doing the request.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*You wont be able to focus on my request, due to having many other requests opened.*"; //"*She tells you that you wont be able to focus on her request, because you have too many of them accepted.*";
                case MessageIDs.RequestRejected:
                    return "*Oh... Well... Better I store this list for me to do some other time, then.*"; //"*She looked sad, and then stored away the list.*";
                case MessageIDs.RequestPostpone:
                    return "*Did you find the request impossible, or you can't do it right now?*"; //"*She asks if you found the request impossible, or if can't do It right now.*";
                case MessageIDs.RequestFailed:
                    return "*I'm really disappointed that you managed to fail my request. Don't worry, by the way... It's fine.*"; //"*Her face is filled with the disappointment over you failing on her request. She then tried to console you.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Did you do what I asked?*";
                case MessageIDs.RequestRemindObjective:
                    return "*I asked you to [objective].*";
                case MessageIDs.RestAskForHowLong:
                    return "*I agree, my feet are sore right now. How long should we rest?*"; //"*She agrees with you, and says that her feet are sore. Then asked for how long will rest.*";
                case MessageIDs.RestNotPossible:
                    return "*This isn't a good moment to rest.*"; //"*She tells you that It doesn't seems like a good moment to rest.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Alright, just don't hog all the blanket for yourself.*"; //"*She tells you not to hog all the blanket.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Hey, let's check [shop]'s shop. There's something interesting for me there.*"; //"*She's telling you that wants to check [shop]'s\nshop. She asks you to get closer to the shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Just wait until I buy this...*"; //"*She tells you to wait a moment while she shops.*";
                case MessageIDs.GenericYes:
                    return "*Yes.*"; //"*She nods.*";
                case MessageIDs.GenericNo:
                    return "*No.*"; //"*She denies.*";
                case MessageIDs.GenericThankYou:
                    return "*Thank you.*"; //"*She thanked.*";
                case MessageIDs.ChatAboutSomething:
                    return "*Oh, what do you want to talk about?*"; //"*She's wondering what you want to talk about.*";
                case MessageIDs.NevermindTheChatting:
                    return "*Alright, want to talk about something else, then?*"; //"*She's still waiting to see what you'll say.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*What?! You want to cancel my request? Are you sure?*"; //"*She got shocked after you said that. She's asking if you're sure.*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Oh.. Okay.. Done...* (Now her face is filled with rage. Run [nickname], Run!)"; //"*She tries to hide the disappointment. Now her face is filled with rage. Run [nickname], Run!*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Phew... (She puts her hand on the chest, and exhale out of relief) You nearly scared me now... So, want to talk about something else?*"; //"*She puts her hand on the chest, and exhale out of relief. Then asked if you want to talk about something else.*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Let's see...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*She's so beautiful...*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*Maybe I should ask her out... No, wait... I should focus...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Okay... Done. Maybe for future planning...*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... No.. That's not what you're thinking!*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    return "*Thank you everybody for helping me.*"; //"*She thanked everyone for the help.*";
                case MessageIDs.RevivedByRecovery:
                    return "*I'm fine now, if someone was wondering.*"; //"*She said that she's fine now, if someone were wondering.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*Argh! Everything inside me burns!*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*AAAAAAAAHHH!! My fur burns!!!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*Why there are two of you?*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*My arms, I can't move them!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*My feet are tired.*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*I don't feel good...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*Ouch... That hurt...*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*Aaaaaahhh!! What is that thing?!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Ewww! It smells like...*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*C-c-cooold....*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*There's web in my fur!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Grrrrr!*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*I can take on tough things now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*So glad to carry good food around.*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*Want to test my arm?*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*Weeee!!*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*Healthier!*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*My vision got sharper like my claws.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Always comes in handy.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*Ahhh...*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*Hmmm..*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Look at that beautiful crystal.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*Be careful, watch your foot.*";
                case MessageIDs.FoundMineTile:
                    return "*Don't get close to that.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*I don't recommend pulling that.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*What a cute flower, can you pick It up for me?*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I'm ready for this, if you want to do it.*";
                case MessageIDs.FoundTreasureTile:
                    return "*I hope there's an accessory inside.*";
                case MessageIDs.FoundGemTile:
                    return "*I want a ring with one of those in it.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I see ore nearby.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*You may want to see that.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*A rollercoaster!*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Great, I was starting to get sweaty.*";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "*Hello.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*This will help.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Do... You two know each other?*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*Watch out! [nickname] fell!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*No! It can't be! [nickname]!!*";
                case MessageIDs.AllyFallsMessage:
                    return "*Someone fell here!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*I like the look of that.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll be right back.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*Watch your health, [nickname].*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*Huff... Puff... Huff...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*I'm running out of potions.*";
                case MessageIDs.UsesLastPotion:
                    return "*That was my last potion.*";
                case MessageIDs.SpottedABoss:
                    return "*Grrrr... Bring it on!*";
                case MessageIDs.DefeatedABoss:
                    return "*We managed to take care of it. Yay!*";
                case MessageIDs.InvasionBegins:
                    return "*That's a lot of hostiles.*";
                case MessageIDs.RepelledInvasion:
                    return "*I think there's no more of them coming.*";
                case MessageIDs.EventBegins:
                    return "*I feel a horrible chill going down the spine...*";
                case MessageIDs.EventEnds:
                    return "*I'm so glad it's over.*";
                case MessageIDs.RescueComingMessage:
                    return "*Don't worry, I'm coming!*";
                case MessageIDs.RescueGotMessage:
                    return "*You're safe with me.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*Do you know [player]? I've been hearing a lot about them.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*You know that [player] has defeated [subject] latelly? Did not? Now you know.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*I'm so jealous... [player] found a [subject] during their travel.*";
                case MessageIDs.FeatEventFinished:
                    return "*I heard that [player] managed to take care of a [subject] that happened in their world. Impressive, huh?!*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*I'm so happy for [player]. They met someone named [subject] latelly. I hope we become friends.*";
                case MessageIDs.FeatPlayerDied:
                    return "*I'm sorry... My friend [player] has died recently and... I'm...*";
                case MessageIDs.FeatOpenTemple:
                    return "*[player] found opened the door to some kind of temple at [subject]. Impressive.*";
                case MessageIDs.FeatCoinPortal:
                    return "*You should have seen that! At a moment, [player] broke a vase, and the other, a portal raining coins appeared!*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I have met [player] recently. They look nice.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*[player] has been really been trying to make that hateable child like them. The many fish they got could let us make a feast for weeks.*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*I was there on the day [player] defeated a creepy giant creature at [subject]. They saved us all.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*I was minding my business in [subject], until a horrible chill went down my spine. Then a villager came saying that their village was engulfed by evil.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*I heard that [subject] got picked by [player] as their buddy. I hope their adventures go well.*";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I was not expecting you to pick me as your buddy, but now I guess I'll have to work not to make you regret the choice, hahaha.*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*So, [subject] got a new house on [world], huh? I'm not jealous! I'm just saying it.*";
                case MessageIDs.DeliveryGiveItem:
                    return "*Hey [target], have some of my [item].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*Wait, where did it go? Someone picked my pocket?*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*I wanted to give [target] something, but they can't hold anymore..*";
                //Popularity Contest Messages
                case MessageIDs.PopContestMessage:
                    return "*Hey, [nickname]! The TerraGuardians Popularity Contest is up and running. Will you vote on it now?*";
                case MessageIDs.PopContestIntroduction:
                    return "*You don't know about it? That's an amazing event where Terrarians can vote on their favorite companions, and in the end their nominations will be announced, showing who are the most popular companions. Any companion can participate, so don't miss this out.*";
                case MessageIDs.PopContestLinkOpen:
                    return "*Have a nice voting.*";
                case MessageIDs.PopContestOnReturnToOtherTopics:
                    return "*Alright. Be sure to vote on us some other time.*";
                case MessageIDs.PopContestResultMessage:
                    return "*Are you interessed in checing the Popularity Contest results? I can take you there.*";
                case MessageIDs.PopContestResultLinkClickMessage:
                    return "*Enjoy.*";
                case MessageIDs.PopContestResultNevermindMessage:
                    return "*I'll still be able to take you to the results until 14 days after the results announcement.*";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public void DialogueAskHowSheMetZacks()
        {
            bool ZacksIsPresent = PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], Zacks);
            if (ZacksIsPresent)
                Dialogue.AddParticipant(PlayerMod.GetPlayerSummonedGuardian(Main.player[Main.myPlayer], Zacks));
            Dialogue.ShowDialogueWithContinue("**");
        }

        public void HangoutDialogue()
        {
            Dialogue.ShowDialogueWithContinue("*She seems happy that you asked how she's doing.*");
            switch (Dialogue.ShowDialogueWithOptions("*She asks you what is your favorite type of food.*", new string[] { "Mushrooms", "Sweet Potatoes", "Bunny Stew", "Nothing in particular" }))
            {
                case 0:
                    Dialogue.ShowDialogueWithContinue("*She shows you her disapproval at your choice, but says that It's fine if you like It.*");
                    break;
                case 1:
                    Dialogue.ShowDialogueWithContinue("*She laughs at your bizarre taste for food.*");
                    break;
                case 2:
                    Dialogue.ShowDialogueWithContinue("*As soon as you said that, she got very furious at you, and told you that you have the audacity of saying that to her.*");
                    Dialogue.ShowDialogueWithContinue("*She calmed down a bit, and apologized for being mad at you.*");
                    break;
                case 3:
                    Dialogue.ShowDialogueWithContinue("*She asks if you're unsure of what to say, then tells you that she may help you find something you like to eat.*");
                    break;
            }
            Dialogue.ShowDialogueWithContinue("*She says that likes eating foods involving meat, but she can't remember what was the food she ate once, which made her choose as her favorite.*");
            switch(Dialogue.ShowDialogueWithOptions("*She gives the idea of making a feast sometime, and asks if you could help her with that in the future.*", new string[] { "Yes", "No" }))
            {
                case 0:
                    Dialogue.ShowEndDialogueMessage("*She says that can't wait until that happens, she seems to be even wondering how that would be.*", false);
                    break;
                case 1:
                    Dialogue.ShowEndDialogueMessage("*She looks sad, but says that in the future, maybe, she could do a feast and call all her friends to join.*", false);
                    break;
            }
        }
    }
}
