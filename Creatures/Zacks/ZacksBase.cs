using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using giantsummon.Trigger;

namespace giantsummon.Creatures
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
            CompanionSlotWeight = 1.65f;
            Age = 16;
            Size = GuardianSize.Medium;
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
            AddOutfit(MeatBagOutfitID, "Meat Bag", delegate (GuardianData gd, Player pl) { return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ZacksMeatBagOutfitQuestStep >= 8; });

            GetTopics();
        }

        public void GetTopics()
        {
            //Related to Zacks Meat Bag outfit quest.
            AddTopic("About Blue's request related to you.", Quests.ZacksMeatBagOutfit.UponAskingAboutRequest, delegate (TerraGuardian tg, PlayerMod pm)
            {
                return pm.ZacksMeatBagOutfitQuestStep >= 2 && pm.ZacksMeatBagOutfitQuestStep < 6;
            });
            AddTopic("We have something to give you.", Quests.ZacksMeatBagOutfit.UponDeliveringToZacksDialogue, delegate (TerraGuardian tg, PlayerMod pm)
            {
                return pm.ZacksMeatBagOutfitQuestStep == 7;
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

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect, ref List<GuardianDrawData> gdd)
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
                        guardian.StartNewGuardianAction(new Creatures.Zacks.ZacksFullMoonBehavior(), FullMoonBehaviorID);
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
            if (g.KnockedOut)
            {
                g.AddFlag(GuardianFlags.CantBeHurt);
                g.AddFlag(GuardianFlags.DontTakeAggro);
            }
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
                return "*[name] is saying that he's entire right now.*";
            return "*[name] said that his hunger is still on bearable levels.*";
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] said that he needs some things for his personal collection.*";
            return "*[name] wants you to do something, but certainly isn't his final wish.*";
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "*[name] is so happy at you for doing his request that could die again. He then apologized for the bad pun.*";
            return "*[name] got the things I brought him and is trying to fake his joy with a neutral look.*";
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            bool BlueInTheWorld = NpcMod.HasGuardianNPC(1), SardineInTheWorld = NpcMod.HasGuardianNPC(2);
            if (!Main.bloodMoon)
            {
                if (BlueInTheWorld)
                {
                    Mes.Add("*[name] said to not worry about your and your citizens safety, since he has an agreement with [gn:1] that he would not devour anyone in her presence.*");
                    if (player.head == 17)
                    {
                        Mes.Add("*[name] tells you to beware when using that hood. He says that you can end up being hugged by a known bunny lover.*");
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
                    Mes.Add("*[name] said that the sun doesn't do good on his rotten skin, so he preffers to stay on areas with shade.*");
                    Mes.Add("*[name] misses being alive, and feels bad about not being able to move his left leg.*");
                }
                else
                {
                    Mes.Add("*[name] said that he preffers the day like this, but with less monsters.*");
                    Mes.Add("*[name] is puzzled about what kind of creatures are those.*");
                }
            }
            else
            {
                if (!NPC.downedBoss1)
                    Mes.Add("*[name] questions himself if that Giant Eye gazing at us is edible. But is looking weird at me after I asked what Giant Eye.*");
                if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.downedPlantBoss)
                    Mes.Add("*[name] says that he will try a vegan meal. Is asking me when I will take him to the Jungle.*");
                if (!Main.bloodMoon)
                {
                    Mes.Add("*[name] is telling me that beside not feeling any kind of pain on his body, he feels an unending hunger that only eases by eating something.*");
                    Mes.Add("*[name] is asking if you will leave him behind, too.*");
                    Mes.Add("*[name] told me that he always feels hunger for flesh, It simply doesn't end.*");
                    Mes.Add("*[name] is asking if he could go outside... For... A walk?*");
                }
                else
                {
                    Mes.Add("*[name] is feeling very aggressive tonight, he says he would be able to eat something whole.*");
                    Mes.Add("*[name] is trying very hard to hold his hunger.*");
                    Mes.Add("*[name] said that this night remembers him of when he died. He don't want to share the horrors he passed through with you. That seems to make him very angry.*");
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
                Mes.Add("*[name] has got the thriller or something.*");
                Mes.Add("*What kind of dance moves are those? Seems like 90's ones?*");
                if (BlueInTheWorld)
                    Mes.Add("*[name] is dancing with [gn:1]. They look joyful when dancing.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[name] is telling me that [nn:" + Terraria.ID.NPCID.Merchant + "] asked if he had any unused skin he wanted to sell.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Nurse))
                Mes.Add("*[name] is telling me that [nn:" + Terraria.ID.NPCID.Nurse + "] asked if he wanted to donate some blood, if there is any useable blood left.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("*[name] said that he went to [nn:" + Terraria.ID.NPCID.Stylist + "] and asked her to do some treatment with his fur, she then replied that can't do miracles. And recommended a taxidermist.*");
            }
            if (NpcMod.HasGuardianNPC(0))
            {
                Mes.Add("*[name] said that If [gn:0] comes annoy him again, he'll try kicking his behind.*");
                Mes.Add("*[name] is telling [gn:0] that if he comes back with any funny jokes again, he'll kick his behind.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[name] feels bad when goes to talk with [gn:1], because she looks partially joyful and partially saddened.*");
                Mes.Add("*[name] is thinking about [gn:1], he wishes to be alive and entire again just to be with her.*");
                if(PlayerMod.PlayerHasGuardianSummoned(player, Blue))
                {
                    switch(PlayerMod.GetPlayerGuardian(player, Blue).OutfitID)
                    {
                        case 1:
                            Mes.Add("*[name] says that would howl at [gn:" + Blue + "], if his lungs weren't badly damaged.*");
                            Mes.Add("*[name] says that [gn:" + Blue + "] looks as great as when they first met.*");
                            Mes.Add("*[name] says that looking at [gn:" + Blue + "] remembered how much he liked her.*");
                            break;
                        case 2:
                            Mes.Add("*[name] asks [gn:" + Blue + "] where is her cloak. He says that didn't really liked It, but couldn't imagine her not using It.*");
                            Mes.Add("*[name] seems impressed at [gn:" + Blue + "]'s outfit.*");
                            break;
                    }
                }
            }
            else
            {
                Mes.Add("*[name] misses being with someone.*");
                if(Main.moonPhase == 0 && !Main.dayTime)
                    Mes.Add("*[name] is trying to howl at the moon, but his lungs were too damaged to be able to do that.*");
            }
            if (NpcMod.HasGuardianNPC(2))
            {
                Mes.Add("*[name] is telling me that he's playing some game with [gn:2], should I start getting worried?*");
                Mes.Add("*[name] is saying that he's getting better at lasso with the help of [gn:2]. But I wonder, where did he got a rope. Wait...*");
                Mes.Add("*[name] asks if you are worried if [gn:2] will turn into a zombie, he told you that you should not worry, at least If he doesn't die, he'll be fine.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*[name] says that dislodged It's left knee when kicking [gn:0]'s behind, and is asking if you can help him put it into place.*");
                Mes.Add("*[name] seems to be paying attention to what [gn:0] is saying, but as soon as he said something stupid, the conversation ended.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] told you to take real good care of [gn:1] on your adventures, or else he'll take care of you.*");
                Mes.Add("*[name] is telling you both to take care, since he doesn't want you and [gn:1] to be another Blood moon miniboss.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*[name] shows a distorted smile while looking in [gn:2] direction, making him back away slowly.*");
                Mes.Add("*[name] said that wants to play a game, and now [gn:2] is begging me that we should go, now.*");
            }
            if (NpcMod.HasGuardianNPC(5))
            {
                Mes.Add("*[name] says that really wanted to play with [gn:5], but his left leg doesn't really helps, so he always dismisses him.*");
                if (NpcMod.HasGuardianNPC(2))
                {
                    Mes.Add("*[name] says that ever since [gn:5] has arrived, he barelly were able to play with [gn:2].*");
                }
                if (NpcMod.HasGuardianNPC(1))
                    Mes.Add("*[name] says that [gn:1] haven't been playing with [gn:5] latelly. He wonders what happened.*");
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
            }
            if (NpcMod.HasGuardianNPC(Cinnamon))
            {
                Mes.Add("*It may sound weird, but I care for [gn:" + Cinnamon + "]'s well being and safety.*");
                Mes.Add("*I hound around [gn:" + Cinnamon + "]'s house during the night, since being scared and locked inside, means not being outside and in danger.*");
                Mes.Add("*Maybe if I take care of [gn:" + Cinnamon + "], I'll practice to be a good parent when I have a child... If I have a child...*");
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
            else if (guardian.IsUsingBed)
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
            Mes.Add("*[name] is saying that just because he's a zombie he doesn't need to live outdoors.*");
            Mes.Add("*[name] doesn't want to be alone anymore, he wants a house with people to talk with.*");
            Mes.Add("*[name] told me that wont have the other villagers as a meal. Give him some place to live.*");
            Mes.Add("*[name] is saying that if you doesn't want to give him a house because of his smell and \"decay state\", there is nothing he can do about it, but It's not nice to be left in the wilderness.*");
            if (NpcMod.HasGuardianNPC(1))
                Mes.Add("*[name] says that he wants to be with [gn:1], so please build him a house next to her.*");
            if (Main.dayTime)
                Mes.Add("*[name] says that the sun is burning the rest of his skin, build a house for him.*");
            else
                Mes.Add("*[name] doesn't want to stay on this deplorable state anymore, give him some place to live.*");
            if (Main.raining)
                Mes.Add("*[name] is saying that the water is bad for his innards. Give him some place to live.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] says that being unable to move his left leg hinders him a bit, but he'll try his best to follow you anyway.*");
            Mes.Add("*[name] is asking you if he should consider doing fishing as a hobby. He got the worms, anyway.*");
            Mes.Add("*[name] seems to be worried, trying to keep up with me, as if I was going to leave him behind.*");
            Mes.Add("*You asked [name] why zombies comes out from the floor, he answers that he doesn't knows, since he was never buried.*");
            Mes.Add("*[name] is asking you if will eventually find the Terrarian that left him to the zombies.*");
            if(PlayerMod.PlayerHasGuardian(player, GuardianBase.Sardine))
                Mes.Add("*I asked [name] what he uses to make his lasso. He told me that used his intestine for that. After knowing that, not only I got striked by an instant regret, but also think that shouldn't tell this to [gn:2].*");
            if (PlayerMod.HasGuardianSummoned(player, 3) && PlayerMod.GetPlayerSummonedGuardian(player, 3).Wet)
            {
                Mes.Add("*[name] is saying that there's water even where you wouldn't believe. And he preffers not to give details.*");
            }
            if (NpcMod.HasGuardianNPC(5) && NpcMod.HasGuardianNPC(1))
                Mes.Add("*[name] says that after knowing of " + NpcMod.GetGuardianNPCName(5) + "'s loss, he thinks he should take it easy with " + NpcMod.GetGuardianNPCName(1) + ", since she might have felt nearly the same.*");
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 0))
            {
                Mes.Add("*[name] is asking me why [gn:0] is so annoying, looks like he's not even grown up.*");
                Mes.Add("*[name] is showing his fist to [gn:0], and said that It's reserved for the next time he comes with a funny zombie joke.*");
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Merchant) && !PlayerMod.PlayerHasGuardianSummoned(player, 1) &&NpcMod.HasGuardianNPC(1))
                    Mes.Add("*[name] wonders if he could gift [gn:1], to try cheering her up.*");
            }
            if (!PlayerMod.PlayerHasGuardianSummoned(player, 1))
            {
                Mes.Add("*[name] started to be happy by seeying [gn:1], but once he saw the bits of despair in her face, he started to get saddened too.*");
            }
            if (PlayerMod.PlayerHasGuardianSummoned(player, 2))
            {
                Mes.Add("*[name] is asking [gn:2] if he wants to play \"The Walking Guardian\", as soon as he said that, [gn:2] started to scream and run away.*");
            }
            if (!Main.dayTime)
            {
                if (!Main.bloodMoon)
                    Mes.Add("*[name] told me that the night he died, he were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies.*");
                else
                    Mes.Add("*[name] told me that the night he died, he were walking in the forest following a Terrarian, until a blood moon started. The Terrarian left him in the middle of the hordes of zombies, whose resulted in his horrible death. He doesn't remember anything past that, other than coming to his senses and seeying yours and Blue's face.*");
            }
            if (NpcMod.HasGuardianNPC(1))
            {
                Mes.Add("*[name] says that he doesn't knows why [gn:1] never mentions this to anybody, but she really loves bunnies. Told me to try giving her one and thank him later.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            if (!PlayerMod.HasGuardianBeenGifted(player, 3) && Main.rand.Next(2) == 0)
                return "*[name] is saying that he's so excited wanting to know what you will give him as a gift, that he could... Oops, he couldn't hold any longer.*";
            return "*[name] is asking if in his current state, growing older is a good thing.*";
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
                                    guardian.StartNewGuardianAction(new Creatures.Zacks.ZacksPullSomeoneAction(player), PullSomeoneID);
                                }
                                break;
                            case TriggerTarget.TargetTypes.TerraGuardian:
                                TerraGuardian tg = MainMod.ActiveGuardians[Sender.TargetID];
                                if (!guardian.DoAction.InUse && !guardian.IsGuardianHostile(tg))
                                {
                                    guardian.StartNewGuardianAction(new Creatures.Zacks.ZacksPullSomeoneAction(tg), PullSomeoneID);
                                }
                                break;
                        }
                    }
                    return true;
            }
            return base.WhenTriggerActivates(guardian, trigger, Sender, Value, Value2, Value3, Value4, Value5);
        }

        public const int FullMoonBehaviorID = 0, PullSomeoneID = 1;

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
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
            }
            return base.GetSpecialMessage(MessageID);
        }
    }
}
