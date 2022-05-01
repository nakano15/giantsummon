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
    public class ZacksBase : GuardianBase
    {
        public const string OldHeadID = "oldhead", OldBodyID = "oldbody", OldLeftArmID = "oldleftarm", MeatBagTextureID = "meatbag", MeatBagTextureFrontID = "meatbag_f";
        public const byte OldBodySkinID = 1;
        public const byte MeatBagOutfitID = 1;

        /// <summary>
        /// -Blue's Boyfriend.
        /// -Doesn't fears cheating on his relationship with Blue.
        /// -Fears being left behind.
        /// -Pal for any time.
        /// -Likes messing with Sardine.
        /// -Wants to play with Alex.
        /// -Tries very hard not to go out of control during Bloodmoons.
        /// </summary>

        public ZacksBase()
        {
            Name = "Zacks";
            Description = "He didn't used to be a zombie, but something happened.\nHe's also Blue's boyfriend.";
            Size = GuardianSize.Large;
            Width = 30;
            Height = 86;
            //DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Scale = 107f / 86;
            DefaultTactic = CombatTactic.Charge;
            CompanionSlotWeight = 1.25f;
            Age = 16;
            SetBirthday(SEASON_AUTUMN, 12);
            Male = true;
            InitialMHP = 185; //1275
            LifeCrystalHPBonus = 50;
            LifeFruitHPBonus = 17;
            Accuracy = 0.32f;
            Mass = 0.5f;
            MaxSpeed = 3.9f;
            Acceleration = 0.12f;
            SlowDown = 0.52f;
            MaxJumpHeight = 17;
            JumpSpeed = 6.15f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            IsNocturnal = false;
            SleepsAtBed = false;
            SetTerraGuardian();
            HurtSound = new SoundData(Terraria.ID.SoundID.NPCHit1);
            DeadSound = new SoundData(Terraria.ID.SoundID.ZombieMoan);
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 12, 14, 15 });
            CallUnlockLevel = 2;
            VladimirBase.AddCarryBlacklist(Zacks);

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 2;

            AddInitialItem(Terraria.ID.ItemID.BloodButcherer, 1);
            AddInitialItem(Terraria.ID.ItemID.HealingPotion, 10);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            PlayerMountedArmAnimation = 10;
            HeavySwingFrames = new int[] { 11, 12, 13 };
            ItemUseFrames = new int[] { 14, 15, 16, 17 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 18;
            ChairSittingFrame = 19;
            ThroneSittingFrame = 20;
            BedSleepingFrame = 21;
            SleepingOffset.X = 16;
            ReviveFrame = 22;
            DownedFrame = 23;
            PetrifiedFrame = 24;

            BackwardStanding = 25;
            BackwardRevive = 26;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(18, 0);

            //Left Hand
            LeftHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(17 * 2, 32 * 2);
            LeftHandPoints.AddFramePoint2x(11, 27, 2);
            LeftHandPoints.AddFramePoint2x(12, 38, 15);
            LeftHandPoints.AddFramePoint2x(13, 40, 37);

            LeftHandPoints.AddFramePoint2x(14, 21, 2);
            LeftHandPoints.AddFramePoint2x(15, 33, 12);
            LeftHandPoints.AddFramePoint2x(16, 37, 22);
            LeftHandPoints.AddFramePoint2x(17, 32, 30);

            LeftHandPoints.AddFramePoint2x(22, 38, 43);

            //Right Hand
            RightHandPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(30 * 2, 32 * 2);
            RightHandPoints.AddFramePoint2x(11, 30, 2);
            RightHandPoints.AddFramePoint2x(12, 41, 15);
            RightHandPoints.AddFramePoint2x(13, 42, 37);

            RightHandPoints.AddFramePoint2x(14, 23, 2);
            RightHandPoints.AddFramePoint2x(15, 36, 12);
            RightHandPoints.AddFramePoint2x(16, 39, 22);
            RightHandPoints.AddFramePoint2x(17, 35, 30);

            //Mount Position
            MountShoulderPoints.DefaultCoordinate = new Microsoft.Xna.Framework.Point(16 * 2, 13 * 2);
            MountShoulderPoints.AddFramePoint2x(12, 24, 19);
            MountShoulderPoints.AddFramePoint2x(13, 27, 25);

            //Sitting Position
            SittingPoint = new Point(22 * 2, 36 * 2);

            //Armor Head Points
            HeadVanityPosition.DefaultCoordinate2x = new Point(21, 10);
            HeadVanityPosition.AddFramePoint2x(12, 30 - 2, 14 + 2);
            HeadVanityPosition.AddFramePoint2x(13, 33 - 2, 21 + 2);

            HeadVanityPosition.AddFramePoint2x(20, 21 + 1, 8);

            HeadVanityPosition.AddFramePoint2x(22, 35, 39);

            //Wing
            WingPosition.DefaultCoordinate2x = new Point(22, 21);

            //Skins
            AddSkin(OldBodySkinID, "Old Body", delegate(GuardianData gd, Player pl) { return true; });

            //Outfits
            AddOutfit(MeatBagOutfitID, "Meat Bag", delegate (GuardianData gd, Player pl)
            {
                return Quests.ZacksMeatBagOutfitQuest.IsMeatbagQuestCompleted();
            });
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(OldHeadID, "head_old");
            sprites.AddExtraTexture(OldBodyID, "body_old");
            sprites.AddExtraTexture(OldLeftArmID, "left_arm_old");
            sprites.AddExtraTexture(MeatBagTextureID, "meatbagoutfit");
            sprites.AddExtraTexture(MeatBagTextureFrontID, "meatbagoutfit_f");
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect, Vector2 Origin, ref List<GuardianDrawData> gdd)
        {
            if (guardian.SkinID == OldBodySkinID)
            {
                foreach (GuardianDrawData gdd2 in gdd)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            if (guardian.SkinID == OldBodySkinID)
            {
                foreach (GuardianDrawData gdd2 in TerraGuardian.DrawFront)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
                foreach (GuardianDrawData gdd2 in TerraGuardian.DrawBehind)
                {
                    ReplaceTexturesForOldTexture(gdd2);
                }
            }
            switch (guardian.OutfitID)
            {
                case MeatBagOutfitID:
                    {
                        bool OldSkin = guardian.SkinID == OldBodySkinID;
                        Texture2D outfittexture = sprites.GetExtraTexture(MeatBagTextureID);
                        Texture2D outfitfronttexture = sprites.GetExtraTexture(MeatBagTextureFrontID);
                        Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame); //new Rectangle(0,0, SpriteWidth, SpriteHeight);
                        GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, outfittexture, DrawPosition, rect, color, Rotation,
                            Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                        if (OldSkin)
                            rect.Y += SpriteHeight * 2 * 2;
                        else
                            rect.Y += SpriteHeight * 2 * 1;
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, outfittexture, DrawPosition, rect, color, Rotation,
                            Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                        rect = guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame);
                        if (OldSkin)
                            rect.Y += SpriteHeight * 2 * 3;
                        else
                            rect.Y += SpriteHeight * 2 * 4;
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, outfittexture, DrawPosition, rect, color, Rotation,
                            Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGLeftArm, gdd);
                        rect = guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame);
                        rect.Y += SpriteHeight * 2 * 5;
                        gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, outfittexture, DrawPosition, rect, color, Rotation,
                            Origin, Scale, seffect);
                        InjectTextureAfter(GuardianDrawData.TextureType.TGRightArm, gdd);
                        if (BodyFrontFrameSwap.ContainsKey(guardian.BodyAnimationFrame))
                        {
                            rect = guardian.GetAnimationFrameRectangle(BodyFrontFrameSwap[guardian.BodyAnimationFrame]);
                            gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, outfitfronttexture, DrawPosition, rect, color, Rotation,
                                Origin, Scale, seffect);
                            InjectTextureAfter(GuardianDrawData.TextureType.TGBodyFront, gdd);
                        }
                    }
                    break;
            }
        }

        public void ReplaceTexturesForOldTexture(GuardianDrawData gdd)
        {
            switch (gdd.textureType)
            {
                case GuardianDrawData.TextureType.TGHead:
                    gdd.Texture = sprites.GetExtraTexture(OldHeadID);
                    break;
                case GuardianDrawData.TextureType.TGBody:
                    gdd.Texture = sprites.GetExtraTexture(OldBodyID);
                    break;
                case GuardianDrawData.TextureType.TGLeftArm:
                    gdd.Texture = sprites.GetExtraTexture(OldLeftArmID);
                    break;
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            if (guardian.BodyAnimationFrame == ThroneSittingFrame && guardian.LookingLeft)
                guardian.FaceDirection(false);
            if (guardian.Wet && !guardian.HasFlag(GuardianFlags.BreathUnderwater) && !guardian.HasFlag(GuardianFlags.Merfolk))
            {
                guardian.BreathCooldown += 2;
            }
            if (!guardian.DoAction.InUse)
            {
                if (Main.moonPhase == 0 && !Main.bloodMoon && Main.time >= 3600)
                {
                    if (guardian.OwnerPos == -1 || guardian.IsPlayerIdle)
                    {
                        guardian.StartNewGuardianAction(new Companions.Zacks.ZacksFullMoonBehavior(), FullMoonBehaviorID);
                    }
                }
            }
        }

        public override string CallUnlockMessage
        {
            get
            {
                return "*I... Want to be able to help you... Not be a burden... Take me on your adventures whenever you can...*";
            }
        }

        public override string MountUnlockMessage
        {
            get
            {
                return "*You know, you will be safer If I carry you on my shoulder. At least I don't feel pain. Just... Plug your nose.*";
            }
        }

        public override string ControlUnlockMessage
        {
            get
            {
                return "*You know, If you have any dangerous thing to do, send me to do it. I'm already dead, anyway.*";
            }
        }
        
        public override void Attributes(TerraGuardian g)
        {
            g.DefenseRate += 0.1f;
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            g.AddFlag(GuardianFlags.CantTakeDamageWhenKod);
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*He's asking me who am I. A zombie, asking me, who am I. Not trying to eat me. What is happening to this world?*";
                case 1:
                    return "*The rotten corpse can't really ruin the distorted smile it gives for meeting me. Should I run away?*";
                case 2:
                    return "*What kind of zombie is that? At least isn't trying to maul or bite me.*";
            }
            return base.GreetMessage(player, guardian);
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I'm whole right now.*";
            return "*My hunger is still on bearable levels.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (guardian.request.IsItemCollectionRequest && Main.rand.NextDouble() < 0.5)
                return "*I need some things for my personal collection. Can you help me with [objective]?*";
            if (guardian.request.IsTravelRequest && Main.rand.NextDouble() < 0.5)
                return "*I'm not really used to stay too much at home, so can you [objective]? Just don't leave me behind, too.*";
            return "*Glad you ask, I really need something, but certainly isn't my final wish. This is it: [objective]. What do you say?*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*I'm so happy that you managed to do my request that could die again. Sorry for the bad pun.*";
            return "(He got the things I brought him and is trying to fake his joy with a neutral look.)";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            bool BlueInTheWorld = NpcMod.HasGuardianNPC(1), SardineInTheWorld = NpcMod.HasGuardianNPC(2);
            if (!Main.bloodMoon)
            {
                if (BlueInTheWorld)
                {
                    Mes.Add("*Don't worry about your and your citizens safety, I hadan agreement with [gn:1] that I will not devour anyone in her presence. I promisse*");
                    if (player.head == 17)
                    {
                        Mes.Add("*You better be careful when wearing that hoodie, you may end up being caught by a very known bunny lover.*");
                    }
                }
                else
                {
                    Mes.Add("*I still remember the agreement we made, I wont eat your citizens, or you. I promisse.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*The sun doesn't do good to my rotten skin, so I preffer to stay on areas with shade.*");
                    Mes.Add("*I miss being alive, and being able to move my left leg.*");
                }
                else
                {
                    Mes.Add("*I preffer days like this, but with less monsters.*");
                    Mes.Add("*What kind of creatures are those?*");
                }
            }
            else
            {
                if (!NPC.downedBoss1)
                    Mes.Add("*I wonder if that Giant Eye is edible. Huh? What Giant Eye? Aren't you seeing it?*");
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss)
                    Mes.Add("*I think I'll try some vegan food. When do you plan on taking me to the Jungle?*");
                if (!Main.bloodMoon)
                {
                    Mes.Add("*I don't feel any kind of pain in my body, but I feel an unending hunger that only ceases when I eat something.*");
                    Mes.Add("*Are you going to leave me behind, too?*");
                    Mes.Add("*I only feel hunger for flesh, and when I do feel too much hunger, It doesn't end well.*");
                    Mes.Add("*Say... Could we go outside... For a walk..?*");
                }
                else
                {
                    Mes.Add("*I'm trying... To hold... Myself.. I feel like eating something whole....*");
                    Mes.Add("*Ugh.... Nghh....* (He's trying very hard to hold his hunger.");
                    Mes.Add("*Ugh... This night... Remembers me of when I died... Better... You not know of the details...* (That seems to make him very angry.)");
                }
            }
            switch (guardian.SkinID)
            {
                case OldBodySkinID:
                    {
                        Mes.Add("*Do I look less scary like this?*");
                        Mes.Add("*Well.. At least there aren't any more flies entering my mouth...*");
                        Mes.Add("*Some people said that I look less psychopathic like this.*");
                    }
                    break;
            }
            switch (guardian.OutfitID)
            {
                case MeatBagOutfitID:
                    Mes.Add("*\"Meat Bag\"... I hope you didn't helped [gn:"+Blue+"] pick this shirt.*");
                    Mes.Add("*It's good to have my wounds patched. It's really uncomfortable when people stares at your wounds, and shows their disgust face.*");
                    Mes.Add("*Well, I'm probably less scary now, but I still can't get rid of the smell.*");
                    break;
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("(He got the thriller or something?)");
                Mes.Add("*What kind of dance moves are those? Seems like 90's ones?*");
                if (BlueInTheWorld)
                    Mes.Add("(He's dancing with [gn:1]. They look joyful when dancing.)");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[nn:" + Terraria.ID.NPCID.Merchant + "] asked if I had any unused skin that I could sell. Look at me, do you think anything in this body could be of use?*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                Mes.Add("*Earlier, [nn:" + Terraria.ID.NPCID.Nurse + "] asked me if I could donate some blood, if there's some left. I asked her if she was crazy.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("*I visitted [nn:" + Terraria.ID.NPCID.Stylist + "] once and asked her to do some treatment on my fur, she then told me that can't do miracles. And recommended me a taxidermist.*");
            }
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("*If [gn:0] comes annoy him again, I'll try kicking his behind.*");
                Mes.Add("*If [gn:0] comes back with any funny jokes again, I'll kick his behind.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*I feel bad when I speak with [gn:1], because she looks partially joyful and partially saddened at me.*");
                Mes.Add("*I think about [gn:1] several times, I wish to be alive and entire again just to be with her.*");
                if(PlayerMod.PlayerHasGuardianSummoned(player, Blue))
                {
                    switch(PlayerMod.GetPlayerGuardian(player, Blue).OutfitID)
                    {
                        case 1:
                            Mes.Add("*I would howl to [gn:" + Blue + "], if my lungs weren't badly damaged.*");
                            Mes.Add("*You look as great as when we first met, [gn:" + Blue + "].*");
                            Mes.Add("*[gn:" + Blue + "]... Looking at you now remembered me how much I like you.*");
                            break;
                        case 2:
                            Mes.Add("*Uh, [gn:" + Blue + "], where is your cloak? I didn't really liked the cloak, but couldn't imagine you not using it.*");
                            Mes.Add("*Wow! I'm really impressed at your outfit, [gn:" + Blue + "].*");
                            break;
                    }
                }
            }
            else
            {
                Mes.Add("*Huh? Oh, sorry... I just... Miss someone...*");
                if(Main.moonPhase == 0 && !Main.dayTime)
                    Mes.Add("(He's trying to howl at the moon, but his lungs were too damaged to be able to do that.)");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("*I've been playing a game with [gn:2]. I were having fun with it, but I can't say the same about [gn:2].*");
                Mes.Add("*I've been getting really good at lasso, with the help of [gn:2]. What lasso? Well...*");
                Mes.Add("*Let me guess, you're worried that I may end up turning [gn:2] into a zombie? Don't worry about that, at least unless he dies, he'll be fine, probably.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*I dislodged my left knee when kicking [gn:0]'s behind, can you help me put it into it's place?*");
                Mes.Add("(He seems to be paying attention to what [gn:0] is saying, but as soon as he said something stupid, the conversation ended.)");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*Take good care of [gn:1] on your adventures, or else I'll take care of you. Understand?*");
                Mes.Add("*Both to take care on your quest, I don't want either of you turning into another Blood moon miniboss.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("(He shows a distorted smile while looking in [gn:2] direction, making him back away slowly.)");
                Mes.Add("*Hey [gn:2], let's play a game?* ([gn:2] is begging me that we should go, now.)");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*I really wanted to play with [gn:5], but my left leg doesn't really helps, so I can't do much else than dismiss him.*");
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("*Ever since [gn:5] has arrived, I barelly were able to play with [gn:2].*");
                }
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("*Weird, [gn:1] haven't been playing with [gn:5] latelly. I wonders what happened.*");
            }
            if (NpcMod.HasGuardianNPC(7))
            {
                Mes.Add("*Do you know why everytime [gn:7] sees me, she only shows one emotion, spooked?*");
                Mes.Add("*I tried greeting [gn:7], she ran away yelling like as if she saw a zombie or something. Wait...*");
                if (NpcMod.HasGuardianNPC(2))
                    Mes.Add("*You say that [gn:2] is [gn:7]'s husband? I guess the fear they have of me is from their family?*");
            }
            if (NpcMod.HasGuardianNPC(8))
            {
                Mes.Add("*I don't know if It's because I'm a wolf, a zombie, or if I'm male. But It gets quite hot when [gn:8] is around.*");
                Mes.Add("*[gn:8] said that heard a faint howling earlier? As if. I was practicing... Howling. I'm a wolf, after all.*");
            }
            if (NpcMod.HasGuardianNPC(9))
            {
                Mes.Add("*I hate [gn:" + Domino + "], he loves making bad jokes to people.*");
            }
            if (NpcMod.HasGuardianNPC(10))
            {
                Mes.Add("*If you worry about [gn:" + Leopold + "], don't worry, I wont eat him. But It is fun to make him panic.*");
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("*Sometimes I feel jealous about [gn:" + Leopold + "], [gn:"+Blue+"] hugs him way more than she does to me. But I don't really like the idea of spending hours around her arms too. So I guess I feel a bit of pity?*");
                }
            }
            if (NpcMod.HasGuardianNPC(Vladimir) && NpcMod.HasGuardianNPC(Blue))
            {
                Mes.Add("*I went earlier to ask [gn:"+Vladimir+"] why [gn:"+Blue+"] visits him so much... I didn't knew how much pain I cause to her... And how much joy I brought to her once I returned to her side...*");
            }
            if (NpcMod.HasGuardianNPC(Michelle))
            {
                Mes.Add("*Why [gn:" + Michelle + "] doesn't talk with me?*");
                Mes.Add("*I think [gn:" + Michelle + "] seems to be a cool person, but she always ignores me...*");
            }
            if (NpcMod.HasGuardianNPC(Malisha))
            {
                Mes.Add("*It's quite nice having a new girl on the town. No, I'm not cheating [gn:" + Blue + "] If that's what is on your mind.*");
            }
            if (NpcMod.HasGuardianNPC(Minerva))
            {
                Mes.Add("*Let me guess, [gn:" + Minerva + "] told you that is scared of me, and made you come to me. Don't worry, as I said before, I wont eat any citizen. That doesn't stop me from scaring them, by the way.*");
                Mes.Add("*I don't know if [gn:" + Minerva + "] charges for the food she makes, or if gives them for free. But I can only say that she cooks very good.*");
            }
            if (NpcMod.HasGuardianNPC(Glenn))
            {
                Mes.Add("*I see that you've met a new toy for me to play with... Spooking [gn:" + Glenn + "] will keep me entertained.*");
                Mes.Add("*I like following [gn:" + Glenn + "] around when he's completelly alone. His attempts to escape from me makes me want to chase him more.*");
                Mes.Add("*I don't have anything against [gn:"+Glenn+"], but I can use my current state to scare him just for fun.*");
                Mes.Add("*At night, I visit [gn:" + Glenn + "]'s house to make sure he's inside, since being scared and locked inside, means not being outside and in danger.*");
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*It may sound weird, but I care for [gn:" + Cinnamon + "]'s well being and safety.*");
                Mes.Add("*I hound around [gn:" + Cinnamon + "]'s house during the night, since being scared and locked inside, means not being outside and in danger.*");
                Mes.Add("*Maybe if I take care of [gn:" + Cinnamon + "], I'll practice to be a good parent when I have a child... If I have a child...*");
            }
            if (NpcMod.HasGuardianNPC(Miguel))
            {
                Mes.Add("*Even [gn:" + Miguel + "] thinks that I may not be able to grow a muscle, since my body is... Well.. You know... I'm quite happy that he still give me exercises for me to try.*");
                Mes.Add("*It's really complicated to visit [gn:" + Miguel + "]. My ever hungry instincts make me want to devour him.*");
            }
            if (NpcMod.HasGuardianNPC(Luna))
            {
                Mes.Add("*[gn:"+Luna+"]... She makes me droll... Not good...*");
                Mes.Add("*I try keeping my distance from [gn:"+Luna+"], for her safety.*");
            }
            if (NpcMod.HasGuardianNPC(Green))
            {
                Mes.Add("*It seems like I wont have much need of a doctor...*");
            }

            if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Yes, I can share my room with you, I can't sleep at night, anyway.*");
                Mes.Add("*If you're worried about being devoured during the night, don't worry, I wont. I know how to search for food outside.*");
                Mes.Add("*There is not much I can do during the night. Either I watch the window, or you sleep. I think I saw you putting your thumb on your mouth one night.*");
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Blue))
            {
                Mes.Add("*So, you're sharing room with [gn:"+Blue+"], huh... I may wonder why she wouldn't want to share her room with me.*");
                Mes.Add("*Say.. You're sharing room with [gn:"+Blue+"], right? How's she?*");
            }
            if (guardian.KnockedOut)
            {
                Mes.Clear();
                Mes.Add("*I'm sorry [nickname], I can't move. I think I pulled It up to It's limit.*");
                Mes.Add("*I'm paralized, I can't move at all.*");
                Mes.Add("*I can't move any part of my body.*");
            }
            else if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("*I'm awaken. I can't sleep, ever since I turned into a zombie.*");
                Mes.Add("*There isn't much I can do while in the bed, so I only watch the ceiling, and expect something interesting to happen.*");
                Mes.Add("*There are all kinds of sounds that happens when people sleeps, you wouldn't believe If I told you them all.*");
                if (NpcMod.HasGuardianNPC(Blue))
                {
                    Mes.Add("*Since I stay up all night, the least I could do is make sure that [gn:" + Blue + "] sleep safe and sound.*");
                    Mes.Add("*It comforts me a bit to watch [gn:" + Blue + "] sleep, knowing that she's safe, and here with me.*");
                }
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*Go back before It's too late! Things here are dreadful even for me!*");
                Mes.Add("*You don't know what I'm passing through here.*");
            }
            if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I don't think that just because I'm dead, I can comunicate with her. Sorry.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hey! Just because I'm a zombie doesn't mean I must live outdoor.*");
            Mes.Add("*I don't want to be alone anymore. I want a house, I want people around to talk with.*");
            Mes.Add("*I wont have your villagers as a meal. Give me a place to live.*");
            Mes.Add("If you don't want to give me a house because of my smell and \"decay state\", there is nothing I can do about it, but It's not nice to be left in the wilderness.*");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("*I want to be with [gn:1], so please give me a house next to her.*");
            if (Main.dayTime)
                Mes.Add("*The sun is burning the rest of my skin, give me a house, please.*");
            else
                Mes.Add("*I don't want to stay on this deplorable state anymore, give me some place to live.*");
            if (Main.raining)
                Mes.Add("*The water is bad for my innards. Give me some place to live.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I'm being hindered a bit by my left leg, but I'll try my best to follow you anyway.*");
            Mes.Add("*Should I consider doing fishing as a hobby. I got the bait, anyway.*");
            Mes.Add("*I keep trying to keep up to you, so you never leave me behind.*");
            Mes.Add("*Why zombies comes out from the floor? I don't know, I was never buried.*");
            Mes.Add("*Tell me [nickname], will we eventually find the Terrarian that left me to the zombies? It's not for revenge, at least a bit, but... Why?*");
            if(PlayerMod.PlayerHasGuardian(player, GuardianBase.Sardine))
                Mes.Add("*I asked [name] what he uses to make his lasso. He told me that used his intestine for that. After knowing that, not only I got striked by an instant regret, but also think that shouldn't tell this to [gn:2].*");
            if (PlayerMod.HasGuardianSummoned(player, 3) && PlayerMod.GetPlayerSummonedGuardian(player, 3).Wet)
            {
                Mes.Add("*There's water even where you wouldn't believe, I preffer not to give details.*");
            }
            if (NpcMod.HasGuardianNPC(5) && NpcMod.HasGuardianNPC(1))
                Mes.Add("*After hearing of " + NpcMod.GetGuardianNPCName(5) + "'s loss, I think I shouldn't feel so bad about my situation with " + NpcMod.GetGuardianNPCName(1) + ", she might have felt nearly the same...*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*Why [gn:0] is so annoying? Looks like he's not even grown up.*");
                Mes.Add("*See this [gn:0]? This is a fist, and It's reserved for the next time you come up with a funny zombie joke.*");
            }
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Merchant) && !PlayerMod.PlayerHasGuardianSummoned(player, 1) &&NpcMod.HasGuardianNPC(1))
                    Mes.Add("*I wonder if I could give a gift to [gn:1]. Maybe that will cheer her up.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("(He started to be happy by seeying [gn:1], but once he saw the bits of despair in her face, he started to get saddened too.)");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*Hey [gn:2], want to play \"The Walking Guardian\" with me? (As soon as he said that, [gn:2] started to scream and run away.)");
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                    Mes.Add("*The night I died, I were walking in the forest following a Terrarian, until a blood moon started. That Terrarian left me in the middle of the hordes of zombies to be eaten alive, after I helped him climb a corrupt cliff. I'm still furious about that.*");
                else
                    Mes.Add("*The night I died, I were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies to be eaten alive, after I helped him climb a corrupt cliff. I don't remember anything after that, other than suddenly \'waking up\' after listening to Blue's voice, and seeing yours and her face.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*I don't know why [gn:1] never mentions this to anybody, but she really loves bunnies. Try giving her one and thank me later.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 3) && Main.rand.Next(2) == 0)
                return "*I'm so excited wanting to know what you will give me as a gift, that I could... Oops, ignore... The sudden puddle.*";
            return "*Tell me, [nickname]. Growing older, in my current state, is a good thing?*";
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            bool IsBlue = !IsPlayer && ReviveGuardian.ModID == Guardian.ModID && ReviveGuardian.ID == GuardianBase.Blue;
            if (!IsBlue)
            {
                Mes.Add("*You look tasty when knocked out...*");
                Mes.Add("*Do you mind if I take a little bite...*");
                Mes.Add("*Don't worry buddy, you'll wake up soon.*");
                Mes.Add("*The quality of your blood is good.*");
            }
            if (!IsPlayer && ReviveGuardian.ModID == Guardian.ModID)
            {
                switch (ReviveGuardian.ID)
                {
                    case GuardianBase.Mabel:
                        Mes.Add("*Hmmmm... Venison...*");
                        Mes.Add("*I'd like to take a bite but... I'm already with someone else...*");
                        Mes.Add("*You're making me a bit uncomfortable.*");
                        break;
                    case GuardianBase.Blue:
                        Mes.Add("*How did this happened?!*");
                        Mes.Add("*" + ReviveGuardian.Name + "! " + ReviveGuardian.Name + "! Wake up! Talk to me!*");
                        Mes.Add("*I wont let you die " + ReviveGuardian.Name + "! I promisse you!*");
                        break;
                    case GuardianBase.Sardine:
                        Mes.Add("*" + ReviveGuardian.Name + ", I'll eat you if you don't wake up. ... It's not fun when you're knocked out.*");
                        Mes.Add("*It's really odd to see you not being scared or running away... Please wake up soon...*");
                        Mes.Add("*Maybe If I pretend to be biting him will make him wake up faster?*");
                        break;
                    case GuardianBase.Alex:
                        Mes.Add("*I wont let you die too. You already had one grief.*");
                        Mes.Add("*Hang on buddy, your old owner can wait.*");
                        Mes.Add("*If I could have played with him while he was awake...*");
                        break;
                    case GuardianBase.Leopold:
                        Mes.Add("*I wonder the surprise he will have when he wakes up.*");
                        Mes.Add("*Maybe I should avoid healing him from behind, I don't want to receive an easter egg or something.*");
                        Mes.Add("*You're sleeping so peacefully... Wait until you wake up. Hehe....*");
                        Mes.Add("*I'll try showing my teeth right directly in front of his face. This should be fun when he wakes up.*");
                        break;
                    case GuardianBase.Minerva:
                        Mes.Add("*Hmmmm... Beef...*");
                        Mes.Add("*I'm trying not to salivate here.*");
                        Mes.Add("*Please wake up... I can't hold for longer...*");
                        break;
                    case GuardianBase.Glenn:
                        Mes.Add("*I don't think he'll like what he'll see when wakes up.*");
                        Mes.Add("*You're just too young to die.*");
                        Mes.Add("*Sorry, but you'll live to get spooked by me for longer.*");
                        break;
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, TriggerTarget Sender, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.Downed:
                    {
                        switch(Sender.TargetType)
                        {
                            case TriggerTarget.TargetTypes.Player:
                                Player player = Main.player[Sender.TargetID];
                                if (!guardian.DoAction.InUse && !guardian.IsPlayerHostile(player))
                                {
                                    guardian.StartNewGuardianAction(new Zacks.ZacksPullSomeoneAction(player), PullSomeoneID);
                                }
                                break;
                            case TriggerTarget.TargetTypes.TerraGuardian:
                                TerraGuardian tg = MainMod.ActiveGuardians[Sender.TargetID];
                                if (!tg.Downed && !guardian.DoAction.InUse && !guardian.IsGuardianHostile(tg))
                                {
                                    guardian.StartNewGuardianAction(new Zacks.ZacksPullSomeoneAction(tg), PullSomeoneID);
                                }
                                break;
                        }
                    }
                    return true;
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public const int FullMoonBehaviorID = 0, PullSomeoneID = 1;

        public override string CompanionRecruitedMessage(GuardianData WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == Blue)
                {
                    Weight = 1.5f;
                    return "*I'm so happy to see you again.*";
                }
                if(WhoJoined.ID == Mabel)
                {
                    Weight = 1.5f;
                    return "*Woah, venison.*";
                }
            }
            Weight = 1f;
            return "*A new person. Hello, and don't worry, I wont eat you.*";
        }

        public override string CompanionJoinGroupMessage(GuardianData WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case Blue:
                        Weight = 1.5f;
                        return "*You just made this adventure better.*";
                    case Mabel:
                        Weight = 1.5f;
                        return "*I... Must... Control... My hunger...*";
                    case Sardine:
                        Weight = 1.5f;
                        return "*I was in need of a squeaky toy, how did you knew?*";
                    case Glenn:
                    case Cinnamon:
                        Weight = 1.5f;
                        return "*Better you not stray too far away from the group, zombies loves lonelly children, you know.*";
                    case Minerva:
                        Weight = 1.5f;
                        return "*She might help me control my hunger.*";
                }
            }
            Weight = 1f;
            return "*I hope you don't mind having me around, too.*";
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.BuddySelected:
                    return "*When I heard that you picked me as your buddy, I thought It was a prank, but It isn't. I'm... Thanks Buddy.*";
                case MessageIDs.RescueMessage:
                    return "*Sorry pal, but you're not dying today.*";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    switch (Main.rand.Next(3))
                    {
                        case 0:
                            return "*I was sick of lying down in the bed, anyway.*";
                        case 1:
                            return "*It's good to stretch the legs a bit.*";
                        case 2:
                            return "*The roof was well known for me, what is It that you want?*";
                    }
                    break;
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    switch (Main.rand.Next(2))
                    {
                        case 0:
                            return "*Oh yeah, I had a request for you, did you complete It?*";
                        case 1:
                            return "*I remember that I gave you a request. Is It done?*";
                    }
                    break;
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "*I can help you on your travels, just don't leave me behind.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "*I think you have way too many people with you.*";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "*Sorry, but I don't feel like I can follow you right now.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "*Are you sure? I don't really like this place. I can't really die but... Being incapacitated isn't cool.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "*Oh, okay. I'm going back home then.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "*Then I'll try returning home. Be sure to visit me safe and sound.*";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "*Great choice, [nickname].*";
                case MessageIDs.RequestAccepted:
                    return "*You know where to find me.*";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "*You seems to have too many things to do right now. Maybe later.*";
                case MessageIDs.RequestRejected:
                    return "*Didn't liked my request, [nickname]?*";
                case MessageIDs.RequestPostpone:
                    return "*Well, It can wait, then.*";
                case MessageIDs.RequestFailed:
                    return "*I'm thinking about trying eating a succulent Terrarian now. Just kidding. I'm not that mad about this, but I wanted that done.*";
                case MessageIDs.RequestAsksIfCompleted:
                    return "*Did you finally do it, [nickname]?*";
                case MessageIDs.RequestRemindObjective:
                    return "*Maybe something is wrong with your brain, I could analyze it if you allow me. Haha, just kidding. Anyways, I asked you to [objective]. Don't forget that again.*";
                case MessageIDs.RestAskForHowLong:
                    return "*I can't sleep, so doesn't makes a difference. How long do you plan on resting?*";
                case MessageIDs.RestNotPossible:
                    return "*Getting some rest right now seems very dangerous. Maybe you should try anothe time.*";
                case MessageIDs.RestWhenGoingSleep:
                    return "*Don't worry, you and your companions will be whole when you wake up.*";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "*Hey [nickname], I need to check out [shop]'s shop.*";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "*Now let's see...*";
                case MessageIDs.GenericYes:
                    return "*Yes, that's good.*";
                case MessageIDs.GenericNo:
                    return "*No. No way.*";
                case MessageIDs.GenericThankYou:
                    return "*...Thank you.*";
                case MessageIDs.ChatAboutSomething:
                    return "*You want to know something from me? I hope It's not related to my condition.*";
                case MessageIDs.NevermindTheChatting:
                    return "*I was getting tired of chatting too.*";
                case MessageIDs.CancelRequestAskIfSure:
                    return "*You're not feeling that can do my request?*";
                case MessageIDs.CancelRequestYesAnswered:
                    return "*Rrr.... Fine... You don't need to do that anymore. Just answer me, how crunchy is a Terrarian?*";
                case MessageIDs.CancelRequestNoAnswered:
                    return "*Then why you brought that, in first place?*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Is... That even safe...?*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Eugh... This smell... It's horrible!*";
                case MessageIDs.AlexanderSleuthingProgressNearlyDone:
                    return "*There's even vile creatures inside his body...*";
                case MessageIDs.AlexanderSleuthingProgressFinished:
                    return "*I hope none of them entered my nose.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*No, I'm not seeking a bone, I was just checking... Things.*";
                //
                case MessageIDs.ReviveByOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Thanks for fixing my body.*";
                    return "*I can move again. I don't know what you did, but Thank you.*";
                case MessageIDs.RevivedByRecovery:
                    return "*My body moves again, good.*";
                //
                case MessageIDs.AcquiredPoisonedDebuff:
                    return "*...*";
                case MessageIDs.AcquiredBurningDebuff:
                    return "*Fire!! It's burning my organs too!*";
                case MessageIDs.AcquiredDarknessDebuff:
                    return "*My eyes are failing me!*";
                case MessageIDs.AcquiredConfusedDebuff:
                    return "*I'm so dizzy...*";
                case MessageIDs.AcquiredCursedDebuff:
                    return "*My arms wont answer me, I feel blocked!*";
                case MessageIDs.AcquiredSlowDebuff:
                    return "*Please, don't leave me behind!*";
                case MessageIDs.AcquiredWeakDebuff:
                    return "*My body feels like crumbling...*";
                case MessageIDs.AcquiredBrokenArmorDebuff:
                    return "*My chest was cut open, and my organs are vulnerable!*";
                case MessageIDs.AcquiredHorrifiedDebuff:
                    return "*What is that thing?! We should not let It eat us!*";
                case MessageIDs.AcquiredIchorDebuff:
                    return "*Hehe... Golden Shower...*";
                case MessageIDs.AcquiredChilledDebuff:
                    return "*Sorry, my body is reacting to the cold.*";
                case MessageIDs.AcquiredWebbedDebuff:
                    return "*I can't get out of this!*";
                case MessageIDs.AcquiredFeralBiteDebuff:
                    return "*Ugh...*";
                //
                case MessageIDs.AcquiredDefenseBuff:
                    return "*I'm tougher now.*";
                case MessageIDs.AcquiredWellFedBuff:
                    return "*My hunger satiation wont last long...*";
                case MessageIDs.AcquiredDamageBuff:
                    return "*I feel able to rip someone apart now.*";
                case MessageIDs.AcquiredSpeedBuff:
                    return "*I wont be hindering you, now.*";
                case MessageIDs.AcquiredHealthIncreaseBuff:
                    return "*My body may last longer now.*";
                case MessageIDs.AcquiredCriticalBuff:
                    return "*Sharper.*";
                case MessageIDs.AcquiredMeleeWeaponBuff:
                    return "*Blue was right, this is useful.*";
                case MessageIDs.AcquiredTipsyDebuff:
                    return "*I keep forgetting that It escapes from the hole on my chest....*";
                case MessageIDs.AcquiredHoneyBuff:
                    return "*I think just by being in it, I already ruined the honey...*";
                //
                case MessageIDs.FoundLifeCrystalTile:
                    return "*Life Crystal over there.*";
                case MessageIDs.FoundPressurePlateTile:
                    return "*There's a trap there.*";
                case MessageIDs.FoundMineTile:
                    return "*Don't step on the mine.*";
                case MessageIDs.FoundDetonatorTile:
                    return "*A Detonator, better not activate It.*";
                case MessageIDs.FoundPlanteraTile:
                    return "*I smell danger.*";
                case MessageIDs.WhenOldOneArmyStarts:
                    return "*I could use biting something different.*";
                case MessageIDs.FoundTreasureTile:
                    return "*Loot time!*";
                case MessageIDs.FoundGemTile:
                    return "*Gems staring at us.*";
                case MessageIDs.FoundRareOreTile:
                    return "*I have found some ores here.*";
                case MessageIDs.FoundVeryRareOreTile:
                    return "*Rare ores there.*";
                case MessageIDs.FoundMinecartRailTile:
                    return "*We could use those tracks to reach somewhere else.*";
                //
                case MessageIDs.TeleportHomeMessage:
                    return "*Alright, let's go home then.*";
                case MessageIDs.CompanionInvokesAMinion:
                    return "*This looks interesting.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Hurt my friend and I'll examine your brain.*";
                //
                case MessageIDs.LeaderFallsMessage:
                    return "*[nickname] is in trouble!*";
                case MessageIDs.LeaderDiesMessage:
                    return "*Sigh... [nickname]... You too...?*";
                case MessageIDs.AllyFallsMessage:
                    return "*Person down!*";
                case MessageIDs.SpotsRareTreasure:
                    return "*Perfect! Now it got more interesting.*";
                case MessageIDs.LeavingToSellLoot:
                    return "*I'll unload the loot at some merchant. I'll be right back.*";
                case MessageIDs.PlayerAtDangerousHealthLevel:
                    return "*You seem in a bad state, [nickname]. Let me take some hits for you, instead.*";
                case MessageIDs.CompanionHealthAtDangerousLevel:
                    return "*My body... Is not responding sometimes...*";
                case MessageIDs.RunningOutOfPotions:
                    return "*Hm... I will need more potions soon...*";
                case MessageIDs.UsesLastPotion:
                    return "*I'm out of potions. Someone has more?*";
                case MessageIDs.SpottedABoss:
                    return "*It looks... Delicious...*";
                case MessageIDs.DefeatedABoss:
                    return "*I wonder if that will satiate my hunger.*";
                case MessageIDs.InvasionBegins:
                    return "*[nickname], their look tells me to pull our weapon.*";
                case MessageIDs.RepelledInvasion:
                    return "*Hm... I didn't had a feast like that in a long time.*";
                case MessageIDs.EventBegins:
                    return "*Look at the sky. That's not a good sign...*";
                case MessageIDs.EventEnds:
                    return "*Done, It's over.*";
                case MessageIDs.RescueComingMessage:
                    return "*Hold on! I'm coming!*";
                case MessageIDs.RescueGotMessage:
                    return "*Calm down, I'll just keep you out of reach of those monsters. I wont eat you or anything.*";
                //Feat Mentioning, [player] replaced by mentioned player. [subject] for feat subject
                case MessageIDs.FeatMentionPlayer:
                    return "*I'm glad to have people like you and [player] to check on me, from time to time.*";
                case MessageIDs.FeatMentionBossDefeat:
                    return "*[player] has defeated [subject] recently. The dinner was delicious.*";
                case MessageIDs.FeatFoundSomethingGood:
                    return "*[player] must be really lucky, for having found a [subject] in their travels.*";
                case MessageIDs.FeatEventFinished:
                    return "*The other day, a [subject] happened in a world, and [player] defeated it. I celebrated with a feast.*";
                case MessageIDs.FeatMetSomeoneNew:
                    return "*It seems like [player]'s contact list is increasing more and more recently. The latest person they met was [subject].*";
                case MessageIDs.FeatPlayerDied:
                    return "*... Sorry... My head is somewhere else... My friend [player] died... And I couldn't do much other than bury them...*";
                case MessageIDs.FeatOpenTemple:
                    return "*I heard that [player] opened a temple door at [subject]. Who would've guessed there's more secrets to uncover?*";
                case MessageIDs.FeatCoinPortal:
                    return "*That lucky one... [player] found a coin portal. I wish was I who found it.*";
                case MessageIDs.FeatPlayerMetMe:
                    return "*I actually know of another Terrarian. [player] is how they're called.*";
                case MessageIDs.FeatCompletedAnglerQuests:
                    return "*I can't stand watching [player] fish. It keeps building my hunger, and that's not good...*";
                case MessageIDs.FeatKilledMoonLord:
                    return "*Yes, I know that [player] saved their world from [subject]. I had lots of trouble trying to clean up the mess... I mean... Celestial creatures bodies wont be gone by themselves.*";
                case MessageIDs.FeatStartedHardMode:
                    return "*It looks like [player] managed to make [subject] be a harder place to live. Gladly It isn't affecting our town that much.*";
                case MessageIDs.FeatMentionSomeonePickedAsBuddy:
                    return "*Yeah, yeah... [player] got [subject] as their buddy. Nothing special... So... Want to be my buddy? *";
                case MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy:
                    return "*I'm still shocked, [nickname]. Of everyone else, you picked me.. Me. From all the people who are alive, and whole... You picked me as your buddy. Thank you.... Really...*";
                case MessageIDs.FeatMentionSomeoneMovingIntoAWorld:
                    return "*I heard that [subject] now got a reason to spend their time in [world].*";
                case MessageIDs.DeliveryGiveItem:
                    return "*I think you might be needing this [item], [target].*";
                case MessageIDs.DeliveryItemMissing:
                    return "*There is a hole on my bag? I thought I had an item here.*";
                case MessageIDs.DeliveryInventoryFull:
                    return "*You can't take this item until you clean up your bag, [target].*";
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
