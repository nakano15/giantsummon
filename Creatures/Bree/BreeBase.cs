using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using giantsummon.Trigger;

namespace giantsummon.Creatures
{
    public class BreeBase : GuardianBase
    {
        public const string BagTextureID = "bag", DamselOutfitTextureID = "damsel", DamselOutfitFrontTextureID = "damsel_f";
        public const int BaglessSkinID = 1, DamselOutfitID = 1;

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
            Scale = 38f / 46;
            FramesInRows = 23;
            CompanionSlotWeight = 0.8f;
            //DuckingHeight = 54;
            Age = 23; //5
            SetBirthday(SEASON_SUMMER, 18);
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
            GroupID = TerraGuardianCaitSithGroupID;
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
            SittingFrame = PlayerMountedArmAnimation = 14;
            ChairSittingFrame = 15;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 12, 16, 17, 18, 19 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            ReviveFrame = 18;
            DownedFrame = 19;
            PetrifiedFrame = 20;

            BackwardStanding = 21;
            BackwardRevive = 22;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 13);
            LeftHandPoints.AddFramePoint2x(11, 19, 16);
            LeftHandPoints.AddFramePoint2x(12, 23, 22);
            LeftHandPoints.AddFramePoint2x(13, 21, 28);

            LeftHandPoints.AddFramePoint2x(14, 21 - 6, 23);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 13, 13);
            RightHandPoints.AddFramePoint2x(11, 22, 16);
            RightHandPoints.AddFramePoint2x(12, 26, 22);
            RightHandPoints.AddFramePoint2x(13, 24, 28);

            //Mount Sitting Point
            SittingPoint = new Point(16 * 2, 29 * 2);
            MountShoulderPoints.DefaultCoordinate = SittingPoint;

            //Headgear Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(17, 20);
            HeadVanityPosition.AddFramePoint2x(16, 15, 13);

            HeadVanityPosition.AddFramePoint2x(18, 17, 22);

            RequestList();
            LoadSkinList();
        }

        public void LoadSkinList()
        {
            AddSkin(BaglessSkinID, "Bagless", delegate(GuardianData gd, Player player)
            {
                return gd.HasPersonalRequestBeenCompleted(0);
            });
            AddOutfit(DamselOutfitID, "Damsel", delegate (GuardianData gd, Player player)
            {
                return gd.HasItem(Terraria.ModLoader.ModContent.ItemType<Items.Outfit.Bree.DamselOutfit>());
            });
        }

        public void RequestList()
        {
            AddNewRequest("Stay", 400, "You people really nag me to stay for longer here. I'll make a deal, catch me quite a number of fish, and I'll stay.",
                "Before you go, keep in mind that must not be just any fish. It must be the most delicious fish in my taste. Yes, I'm talking about the Double Cod. Now go, before I change my mind.",
                "You don't really want me to stay, right? I didn't wanted to stay, anyway.",
                "You managed to do that? Alright, I can put down my things in my house and stay for longer. My back was beggining to ache, anyway.",
                "You don't know where to find a Double Cod? The Jungle is where you should go!");
            AddItemCollectionObjective(Terraria.ID.ItemID.DoubleCod, 5, 0);
            AddRequestRequirement(RequestBase.GetFishingRodRequirement);
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            if (trigger == TriggerTypes.Spotted && Sender.TargetType == TriggerTarget.TargetTypes.NPC)
            {
                NPC npc = Main.npc[Sender.TargetID];
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
            if (trigger == TriggerTypes.Death && Sender.TargetType == TriggerTarget.TargetTypes.NPC)
            {
                NPC npc = Main.npc[Sender.TargetID];
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
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(BagTextureID, "bags");
            sprites.AddExtraTexture(DamselOutfitTextureID, "damsel_outfit");
            sprites.AddExtraTexture(DamselOutfitFrontTextureID, "damsel_outfit_front");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            switch (guardian.Data.SkinID)
            {
                case 0:
                    {
                        Microsoft.Xna.Framework.Graphics.Texture2D BagTexture = sprites.GetExtraTexture(BagTextureID);
                        Rectangle backrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                            frontrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                        backrect.Y += backrect.Height;
                        GuardianDrawData bagback = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, backrect, color, Rotation, Origin, Scale, seffect),
                            bagfront = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, frontrect, color, Rotation, Origin, Scale, seffect);
                        InjectTexturesAt(GuardianDrawData.TextureType.TGBody, new GuardianDrawData[] { bagback }, new GuardianDrawData[] { bagfront });
                    }
                    break;
            }
            switch (guardian.Data.OutfitID)
            {
                case DamselOutfitID:
                    {
                        Microsoft.Xna.Framework.Graphics.Texture2D OutfitTexture = sprites.GetExtraTexture(DamselOutfitTextureID);
                        Rectangle bodyRect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                        GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, OutfitTexture, DrawPosition, bodyRect, color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGRightArm, gdd);
                        if(guardian.BodyAnimationFrame == SittingFrame)
                        {
                            Rectangle BodyFrontRect = guardian.GetAnimationFrameRectangle(0);
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(DamselOutfitFrontTextureID), DrawPosition, BodyFrontRect, color, Rotation, Origin, Scale, seffect);
                            InjectTextureAfter(GuardianDrawData.TextureType.TGBodyFront, gdd);
                        }
                        bodyRect.Y += bodyRect.Height;
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, OutfitTexture, DrawPosition, bodyRect, color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                        bodyRect.Y += bodyRect.Height;
                        if (TerraGuardian.HeadSlot == 0)
                        {
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, OutfitTexture, DrawPosition, bodyRect, color, Rotation, Origin, Scale, seffect);
                            InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                        }
                        bodyRect.Y += bodyRect.Height;
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, OutfitTexture, DrawPosition, bodyRect, color, Rotation, Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGLeftArm, gdd);
                    }
                    break;
            }
            return;
            if (guardian.Data.SkinID == 0)
            {
                Microsoft.Xna.Framework.Graphics.Texture2D BagTexture = sprites.GetExtraTexture(BagTextureID);
                for (int i = 0; i < TerraGuardian.DrawBehind.Count; i++)
                {
                    if (TerraGuardian.DrawBehind[i].textureType == GuardianDrawData.TextureType.TGBody)
                    {
                        Rectangle backrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                            frontrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                        backrect.Y += backrect.Height;
                        GuardianDrawData bagback = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, backrect, color, Rotation, Origin, Scale, seffect),
                            bagfront = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, frontrect, color, Rotation, Origin, Scale, seffect);
                        TerraGuardian.DrawBehind.Insert(i + 1, bagfront);
                        TerraGuardian.DrawBehind.Insert(i, bagback);
                        i++;
                    }
                }
                for (int i = 0; i < TerraGuardian.DrawFront.Count; i++)
                {
                    if (TerraGuardian.DrawFront[i].textureType == GuardianDrawData.TextureType.TGBody)
                    {
                        Rectangle backrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                            frontrect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                        backrect.Y += backrect.Height;
                        GuardianDrawData bagback = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, backrect, color, Rotation, Origin, Scale, seffect),
                            bagfront = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, BagTexture, DrawPosition, frontrect, color, Rotation, Origin, Scale, seffect);
                        TerraGuardian.DrawFront.Insert(i + 1, bagfront);
                        TerraGuardian.DrawFront.Insert(i, bagback);
                        i++;
                    }
                }
            }
            /*switch (guardian.Data.SkinID) //Todo - Add some way of activating skins.
            {
                case 1:
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                        {
                            gdd.Texture = sprites.GetExtraTexture(BaglessTextureID);
                        }
                    }
                    foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                    {
                        if (gdd.textureType == GuardianDrawData.TextureType.TGBody)
                        {
                            gdd.Texture = sprites.GetExtraTexture(BaglessTextureID);
                        }
                    }
                    break;
            }*/
        }

        public override string CallUnlockMessage
        {
            get
            {
                if (PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], 2))
                {
                    return "Would you mind if I accompany my husband on your quest? In case he does something stupid, I mean.";
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
            bool HasSardineMet = PlayerMod.PlayerHasGuardian(player, Sardine), HasGlennMet = PlayerMod.PlayerHasGuardian(player, Glenn);
            if (HasSardineMet && HasGlennMet)
            {
                Mes.Add("Thank you for finding my son and my husband. We should now try finding out which world we came from, now...");
                Mes.Add("My son and my husband are fine, all thanks to you.");
                Mes.Add("I feel so good about having my son and my husband okay....");
            }
            else if (!HasSardineMet && HasGlennMet)
            {
                Mes.Add("My son came looking for me, but I still didn't found my husband.");
                Mes.Add("My son got quite sad when I told him that I didn't found his dad yet...");
                Mes.Add("[nickname], if you could help finding my husband, It will be great. He's a black cat with some spirit for adventure.");
            }
            switch (guardian.SkinID)
            {
                case BaglessSkinID:
                    Mes.Add("It's good to not have that weight on my back, It was already starting to ache.");
                    if(player.Male)
                        Mes.Add("You've been looking at me way too much since I removed the bag, why is that?");
                    Mes.Add("What? You're impressed that I'm actually strong? House work isn't easy thing. Or was it the bag?");
                    Mes.Add("I hope there's no thieves in your world, I really don't want to return home and find out my things are gone.");
                    break;
            }
            switch (guardian.OutfitID)
            {
                case DamselOutfitID:
                    Mes.Add("I also have taste for clothing, you know.");
                    Mes.Add("I'm glad that the Clothier managed to make this clothing, it just fits in me.");
                    Mes.Add("I feel like wanting to spend and afternoon at a beach now.");
                    break;
            }
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
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("If [gn:" + Malisha + "] cause one more explosion, I will go have some serious talking with her.");
            }
            if (NpcMod.HasGuardianNPC(GuardianBase.Fluffles))
            {
                Mes.Add("I don't really have something bad to say about [gn:" + GuardianBase.Fluffles + "], maybe It's because she doesn't speaks.");
                Mes.Add("Sometimes [gn:" + GuardianBase.Fluffles + "] presence makes my hair rise. You find really unusual people to live in your world.");
                bool HasBlue = NpcMod.HasGuardianNPC(GuardianBase.Blue), HasZacks = NpcMod.HasGuardianNPC(GuardianBase.Zacks), HasSardine = NpcMod.HasGuardianNPC(Sardine);
                if (HasSardine)
                {
                    if (HasBlue && HasZacks)
                    {
                        Mes.Add("Tell me, will [gn:" + GuardianBase.Sardine + "] even survive one of [gn:" + GuardianBase.Blue + "]'s bullying? Even I am having bad times trying to get all those guardians out of him.");
                        Mes.Add("The other day I had to help my husband get some bath, because he came home all slobbered, and with some bite marks on his body.");
                    }
                    else if (HasBlue)
                    {
                        Mes.Add("After [gn:" + Fluffles + "] arrived, I had to stop [gn:" + Blue + "] and her from chasing [gn:" + Sardine + "] more frequently.");
                        Mes.Add("Do you have something that repells ghosts? I think [gn:"+Sardine+"] might need something like that.");
                    }
                    Mes.Add("There was one time when [gn:"+Sardine+"] returned home, and I got spooked after I saw [gn:"+Fluffles+"] on his shoulder. I screamed so loud that she ran away, and I nearly dirtied the floor too.");
                }
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("[gn:" + Minerva + "] still haven't got into the level for my refined taste. She still has a lot to cook.");
                Mes.Add("I tried teaching [gn:" + Minerva + "] how to cook properly, but she always misses the point when cooking.");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("My son is very studious, he literally devours several books every week.");
                Mes.Add("My son is quite introvert, so the only moment you get him to talk, is when someone else does first.");
                if (NpcMod.HasGuardianNPC(Mabel) && NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                    Mes.Add("I really dislike seeing [gn:" + Glenn + "] playing with [nn:" + Terraria.ID.NPCID.Angler + "], that kid is such a bad influence.");
                else
                {
                    Mes.Add("It kind of worries me that there aren't many kids around for my son to play with...");
                }
                if (NpcMod.HasGuardianNPC(Rococo))
                {
                    Mes.Add("I like seeing that [gn:" + Rococo + "] has been playing with [gn:" + Glenn + "].");
                }
                if (NpcMod.HasGuardianNPC(Alex))
                {
                    Mes.Add("[gn:" + Alex + "] is not only keeping me company sometimes, but also plays with my son, [gn:" + Glenn + "].");
                }
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("[gn:" + Cinnamon + "] actually knows how to put seasoning to food well, but she sometimes exagerate a bit.");
                if(HasSardineMet)
                    Mes.Add("Well, teaching [gn:" + Cinnamon + "] makes me forget the stupidities my husband does.");
            }
                if (guardian.IsPlayerRoomMate(player))
            {
                if(player.Male)
                    Mes.Add("Okay, I can share my bedroom. Just don't try anything funny during the night.");
                Mes.Add("As long as you keep It clean, you can use It for as long as you want.");
                Mes.Add("If you get a bed for yourself, I can let you stay in my bedroom.");
                if (NpcMod.HasGuardianNPC(GuardianBase.Sardine))
                {
                    Mes.Add("I'm sorry, but I kind of would preffer sharing my room with [gn:"+Sardine+"].");
                }
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Sardine))
            {
                if (!player.Male)
                    Mes.Add("I hope you aren't trying to get \'intimate\' with my husband. Remember that we are still married.");
                Mes.Add("I will never understand why [gn:"+Sardine+"] shares his room with you, but not me. I didn't made him sleep in the sofa latelly.");
            }
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("What am I supposed to do to have A MOMENT ALONE!");
                Mes.Add("GO AWAY!");
                Mes.Add("Do you have to bother me, now?");
                Mes.Add("You're annoying me!");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("(She seems to be trying to stand up, but has no strength.)");
                Mes.Add("(She's putting her paws in one of her wounds, trying to ease the bleeding.)");
                Mes.Add("(She looks at you, in pain.)");
            }
            else if (guardian.IsUsingBed)
            {
                Mes.Clear();
                Mes.Add("It's not clean... yet... (She's sleeping.)");
                Mes.Add("(She has a way more peaceful look when sleeping than awaken.)");
                Mes.Add("(She's doing gestures, like as if was hitting something, or someone.)");
                if (!NpcMod.HasMetGuardian(Sardine))
                {
                    Mes.Add("...Sardine... Where are you... (She spoke while sleeping)");
                    Mes.Add("...My home... How are things... (She seems to be worried about her home.)");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("Eek!! Turn the other side!");
                Mes.Add("Do you really have to enter here and talk to me while I'm using the toilet?");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Go away! I don't want to carry your burden.");
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
            else if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID && ReviveGuardian.ID == GuardianBase.Glenn)
            {
                Mes.Add("Oh my... [gn:"+Glenn+"]! [gn:"+Glenn+"]!! Please! Wake up!");
                Mes.Add("No... Not my son! No!!");
                Mes.Add("Don't worry, [gn:"+Glenn+"], mommy is here!");
            }
            else
            {
                Mes.Add("You're safe... I'm here with you...");
                Mes.Add("Here, this will make you feel better.");
                Mes.Add("Shh... You'll be fine. Just rest.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "Most of my heart is reserved for my husband, but I'll reserve a fraction for you.";
                case MessageIDs.RescueMessage:
                    return "I really hope you don't die, because I had trouble carrying you here.";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "Whatever you want could have waited until I woke up, right?";
                        case 1:
                            return "[nickname]. For what reason did you woke me up? SAY IT!";
                        case 2:
                            return "What? Why did you woke me up? Let me sleep!";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "You woke me up! I hope It's about my request, or else...";
                        case 1:
                            return "I hope It's important, or about my request, because I really was enjoying my sleep.";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "I really needed a break from housekeeping, anyway. I hope you don't make me miss that.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "I may be small, but I don't think I will fit in that group of yours.";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    if (!NpcMod.HasMetGuardian(Sardine))
                    {
                        return "I can't right now, I'm looking for clues about my husband.";
                    }
                    else
                    {
                        return "No way, I have many things to do in my house.";
                    }
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "Here?! Right here?! You want to leave me here all alone?! Are you nuts?";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "I need to rest my legs anyway.";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "Yes, fine. Leave a damsel fight her way back to home all alone...";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "Okay, let's find a safe place for me before we leave, then.";
                case MessageIDs.RequestAccepted:
                    return "Good. Don't delay too long with the request.";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "What? No way. You have many things to do right now.";
                case MessageIDs.RequestRejected:
                    return "Hmph. I should have wondered that It would be too hard for you.";
                case MessageIDs.RequestPostpone:
                    return "Hey, but I need that now! *Sigh* Whatever, go do your things.";
                case MessageIDs.RequestFailed:
                    return "Good job, you managed to ruin everything. Now go away!";
                case MessageIDs.RestAskForHowLong:
                    return "I was really needing to get this bag off my back for a while, so a rest seems good enough. How long we'll rest?";
                case MessageIDs.RestNotPossible:
                    return "This isn't a good moment to rest.";
                case MessageIDs.RestWhenGoingSleep:
                    return "I wont share my bag with you, use your pillow.";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "Hey [nickname], could you get closer to [shop]'s shop?";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "Alright, wait a moment...";
                case MessageIDs.GenericYes:
                    return "Yes, sure.";
                case MessageIDs.GenericNo:
                    return "No way.";
                case MessageIDs.GenericThankYou:
                    return "Thank you.";
                case MessageIDs.ChatAboutSomething:
                    return "Please make haste, I have many things on my to do list.";
                case MessageIDs.NevermindTheChatting:
                    return "Well then, anything else?";
                case MessageIDs.CancelRequestAskIfSure:
                    return "YOU WHAT?! How can you... Wait, you're really going to drop what I asked for, are you?";
                case MessageIDs.CancelRequestYesAnswered:
                    return "Grr... Fine. I'll do It myself, then.";
                case MessageIDs.CancelRequestNoAnswered:
                    return "Ah, good. Well, do you need anything else?";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*I hope you don't mind if I collect some infos...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... You actually look cute when not with an angry face.*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    if(NpcMod.HasGuardianNPC(Sardine))
                        return "*Hm... I catched [gn:"+Sardine+"]'s scent...*";
                    return "*She's carrying a photo... Who's that black cat?*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*Alright, I now know you.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Wait, what are you going to do with that frying pan?*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "Yes, thank you. Maybe being around you all isn't that bad.";
                    return "I really hope you didn't tried anything other than to help me.";
                case MessageIDs.RevivedByRecovery:
                    return "Who leaves a damsel bleeding on the ground? You?";
                    //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "Argh! This is horrible! Gr...";
                case MessageIDs.AcquiredBurningDebuff:
                    return "Water! Water!!";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "Ouch! My eyes!";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "The world is spinning...";
                case MessageIDs.AcquiredCursedDebuff:
                    return "My arm wont obbey me.";
                case MessageIDs.AcquiredSlowDebuff:
                    return "My legs aren't moving faster.";
                case MessageIDs.AcquiredWeakDebuff:
                    return "I can barelly stand...";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "Ooof... (Heavy breathing) Nice... Hit...";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "Aack!! What is that thing! It could swallow us whole!";
                case MessageIDs.AcquiredIchorDebuff:
                    return "This is so gross!";
                case MessageIDs.AcquiredChilledDebuff:
                    return "I can barelly move due to shivering...";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "No! Don't eat me!";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "Now you're making me furious!";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "You still think I can't take damage.";
                case MessageIDs.AcquiredWellFedBuff:
                    return "It's not better than my food, but filled me up.";
                case MessageIDs.AcquiredDamageBuff:
                    return "I'm feeling stronger now.";
                case MessageIDs.AcquiredSpeedBuff:
                    return "Even my feet feels lighter.";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "I even feel younger too.";
                case MessageIDs.AcquiredCriticalBuff:
                    return "I can't wait to hit something.";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "Maybe this will make things easier.";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "It's not the same drinking with Sardine...";
                case MessageIDs.AcquiredHoneyBuff:
                    return "Someone has a bottle? I want to grab some for later.";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "What a lovely crystal!";
                case MessageIDs.FoundPressurePlateTile:
                    return "Watch your step.";
                case MessageIDs.FoundMineTile:
                    return "Hey! That's dangerous!";
                case MessageIDs.FoundDetonatorTile:
                    return "If you want to press that, just tell me so I can get away.";
                case MessageIDs.FoundPlanteraTile:
                    return "What kind of plant is that?";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "Yes, I can beat some etherians.";
                case MessageIDs.FoundTreasureTile:
                    return "I will like you more if you find me some jewelry from that chest.";
                case MessageIDs.FoundGemTile:
                    return "Beautiful gems.";
                case MessageIDs.FoundRareOreTile:
                    return "You may want to check those ores.";
                case MessageIDs.FoundVeryRareOreTile:
                    return "Hey, look! Those ores may interest you.";
                case MessageIDs.FoundMinecartRailTile:
                    return "I hope It doesn't zig-zags.";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "Finally, home.";
                case MessageIDs.SomeoneJoinsTeamMessage:
                    return "Great, one more person.";
                case MessageIDs.PlayerMeetsSomeoneNewMessage:
                    return "I hope you're not stranded here, too.";
                case MessageIDs.CompanionInvokesAMinion:
                    return "I wish I had those minions back home.";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "Should... I leave you two alone?";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "Terrarian fell! Someone help them!";
                case MessageIDs.LeaderDiesMessage:
                    return "[nickname] has died! How could we let that happen?";
                case MessageIDs.AllyFallsMessage:
                    return "Someone fell there!";
                case MessageIDs.SpotsRareTreasure:
                    return "You know how to make me happy, don't you?";
                case MessageIDs.LeavingToSellLoot:
                    return "I'm feeling a bit too heavy, I'll sell some things.";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "You don't look okay, pull back for a while.";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "Nice hit... But I'm still standing.";
                case MessageIDs.RunningOutOfPotions:
                    return "I think I may run out of potions soon.";
                case MessageIDs.UsesLastPotion:
                    return "Huh? Where did my potions go? That was the last one!";
                case MessageIDs.SpottedABoss:
                    return "I can't wait to see that fall before me.";
                case MessageIDs.DefeatedABoss:
                    return "Who expected another result? Not me.";
                case MessageIDs.InvasionBegins:
                    return "They dare ruin the peace of my house?! I will make them leave this place on 4!";
                case MessageIDs.RepelledInvasion:
                    return "And don't come back again!";
                case MessageIDs.EventBegins:
                    return "[nickname], did you just do that?";
                case MessageIDs.EventEnds:
                    return "Whew... I think those were the last ones...";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
