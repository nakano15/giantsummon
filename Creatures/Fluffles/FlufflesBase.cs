using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures
{
    public class FlufflesBase : GuardianBase
    {
        public const string FoxSoulTextureID = "foxsoul";

        public FlufflesBase()
        {
            Name = "Fluffles";
            PossibleNames = new string[] { "Fluffles", "Krümel" }; //Thank BentoFox for the names.
            Description = "She was an experienced adventurer, part of a\ngroup of Terrarians and TerraGuardians.\nA traumatic experience unallows her to speak.";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 92;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 104;
            Scale = 101f / 92;
            Age = 31;
            SetBirthday(SEASON_SPRING, 11);
            Male = false;
            InitialMHP = 235; //955
            LifeCrystalHPBonus = 35;
            LifeFruitHPBonus = 10;
            Accuracy = 0.77f;
            Mass = 0.25f;
            MaxSpeed = 4.85f;
            Acceleration = 0.15f;
            SlowDown = 0.26f;
            MaxJumpHeight = 17;
            JumpSpeed = 6.18f;
            CanDuck = false;
            ReverseMount = true;
            DrinksBeverage = true;
            SetTerraGuardian();
            IsNocturnal = true;

            CallUnlockLevel = MountUnlockLevel = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.BoneSword, 1);
            AddInitialItem(Terraria.ID.ItemID.GoldBow, 1);
            AddInitialItem(Terraria.ID.ItemID.BoneArrow, 250);
            AddInitialItem(Terraria.ID.ItemID.LesserHealingPotion, 5);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 3, 4, 5, 4 };
            JumpFrame = 2;
            PlayerMountedArmAnimation = 10;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 6, 7, 8, 9 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 11;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 17;
            SleepingOffset.X = 16;
            ReviveFrame = 15;
            DownedFrame = 18;
            //PetrifiedFrame = 28;

            BackwardStanding = 20;
            BackwardRevive = 22;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(11, 0);

            //MountShoulderPoints.DefaultCoordinate2x = new Point(21, 44);
            SittingPoint2x = new Point(21, 36);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(6, 15, 3);
            LeftHandPoints.AddFramePoint2x(7, 31, 11);
            LeftHandPoints.AddFramePoint2x(8, 35, 20);
            LeftHandPoints.AddFramePoint2x(9, 31, 29);

            LeftHandPoints.AddFramePoint2x(10, 25, 31);

            LeftHandPoints.AddFramePoint2x(15, 39, 44);

            //Right Arm
            RightHandPoints.AddFramePoint2x(6, 27, 3);
            RightHandPoints.AddFramePoint2x(7, 34, 11);
            RightHandPoints.AddFramePoint2x(8, 38, 20);
            RightHandPoints.AddFramePoint2x(9, 34, 29);

            RightHandPoints.AddFramePoint2x(15, 42, 44);
            
            //Headgear
            HeadVanityPosition.DefaultCoordinate2x = new Point(22, 13);
            HeadVanityPosition.AddFramePoint2x(15, 38, 39);
            HeadVanityPosition.AddFramePoint2x(17, 10, 38);
            HeadVanityPosition.AddFramePoint2x(18, 33, 33);

            //WingPosition
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new FlufflesData(ID, ModID);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(FoxSoulTextureID, "FoxSoul");
        }

        public static bool IsHauntedByFluffles(Player player)
        {
            return player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>()) ||
                player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>());
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(It looks like she's greeting you.)");
            Mes.Add("(She smiled as she met you.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (guardian.IsUsingBed)
            {
                Mes.Add("(She's snoring gently.)");
                Mes.Add("(She seems to be having a peaceful rest.)");
                Mes.Add("(You wonder what she may be dreaming of, to make her sleep so calmly.)");
            }
            else if (Main.bloodMoon)
            {
                Mes.Add("(She looks really annoyed, growling at you.)");
                Mes.Add("(She seems to be wanting this night to end as soon as possible.)");
                Mes.Add("(Her face is full of anger, maybe I shouldn't try speaking to her.)");
            }
            else
            {
                Mes.Add("(She's asking if you're okay.)");
                Mes.Add("(She softly hits her paw on her chest a few times after she saw you.)");
                Mes.Add("(She looks relieved for seeing you.)");
                if (Main.raining)
                {
                    Mes.Add("(She looks at the sky, as the rain drops pass through her body.)");
                    Mes.Add("(Everytime It's raining, she has a sorrow look. Maybe she liked that?)");
                }
                if (Main.eclipse)
                {
                    Mes.Add("(She looks to be on alert.)");
                    Mes.Add("(She seems to be scared for the people around, more than herself.)");
                }
                if (Main.moonPhase == 0)
                {
                    Mes.Add("(She shows you a portrait she carries with her. In It you see a male and a female Terrarian, a TerraGuardian and her.)");
                }
                if (guardian.IsUsingToilet)
                {
                    Mes.Add("(She seems embarrassed. I think she wants you to leave the room.)");
                    Mes.Add("(As she notices you, she puts one of her paws on her hip, while with the other she signals for you to leave the room.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Rococo))
                {
                    Mes.Add("(She's looking at [gn:" + Rococo + "] with a puzzled face. It's like as if she has seen him before.)");
                    Mes.Add("(She seems to be trying to recall something, after looking at [gn:" + Rococo + "].)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Blue))
                {
                    if (PlayerMod.HasGuardianSummoned(player, Sardine))
                    {
                        Mes.Add("(She seems to be asking [gn:" + Blue + "] if they're going to play again.)");
                        Mes.Add("([gn:" + Sardine + "] begun panicking as [gn:" + Blue + "] and [gn:" + Fluffles + "] stares at him, with a grim smile.)");
                    }
                    else if (NpcMod.HasGuardianNPC(Sardine))
                    {
                        Mes.Add("([gn:" + Blue + "] and her must be scheming something. I wonder if that's related to [gn:"+Sardine+"].)");
                    }
                }
                if (PlayerMod.HasGuardianSummoned(player, Sardine))
                {
                    Mes.Add("(She's looking at [gn:" + Sardine + "] while pretending to be biting something. He starting backing off after noticing that.)");
                    Mes.Add("([gn:" + Sardine + "] seems to be wanting to run away.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Zacks))
                {
                    Mes.Add("(She's staring at [gn:" + Zacks + "]. She seems to be full of thoughts after looking at him. What could she be thinking about?)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Nemesis))
                {
                    Mes.Add("([gn:" + Nemesis + "] stares at her, while she stares at [gn:"+Nemesis+"]. I don't think anything will come out of this.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Alex))
                {
                    Mes.Add("(She's petting [gn:" + Alex + "] in the head. He seems to be enjoying It.)");
                    Mes.Add("(She threw a bone for [gn:" + Alex + "] to pick. After brought It back, she petted him.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Leopold))
                {
                    Mes.Add("(She gave a scare on [gn:" + Leopold + "]. Now I should be trying to find some leaves.)");
                    Mes.Add("([gn:" + Leopold + "] seems to be trying to avoid being surprised by her.)");
                    Mes.Add("(She seems to be blushing, and avoiding to stare directly at [gn:"+Leopold+"].)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Mabel))
                {
                    Mes.Add("(She looks at [gn:" + Mabel + "] up and down, then she looked at her own body. Is she trying to compare them?)");
                    Mes.Add("(She tries posing like [gn:" + Mabel + "], but by the way she shook her head, It seems like she didn't liked the idea.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Vladimir))
                {
                    Mes.Add("(As she looke at [gn:" + Vladimir + "], he opened his arms, which made her go hug him. She seems to be crying.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Michelle))
                {
                    Mes.Add("([gn:" + Michelle + "] is playing with her. They seems to be liking It.)");
                    Mes.Add("([gn:" + Michelle + "] tried petting her, but the hand passed through her body.)");
                }
                if (PlayerMod.HasGuardianSummoned(player, Glenn))
                {
                    Mes.Add("(She smiles and waves at [gn:" + Glenn + "]. He also waved back at her while smiling.)");
                    Mes.Add("(It seems like both her and [gn:" + Glenn + "] are great friends.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override bool WhenTriggerActivates(TerraGuardian guardian, TriggerTypes trigger, int Value, int Value2 = 0, float Value3 = 0, float Value4 = 0, float Value5 = 0)
        {
            switch (trigger)
            {
                case TriggerTypes.GuardianDowned:
                    {
                        if(Value == guardian.WhoAmID)
                        {
                            SpawnSoul(guardian);
                        }
                    }
                    break;
            }
            return base.WhenTriggerActivates(guardian, trigger, Value, Value2, Value3, Value4, Value5);
        }

        public void SpawnSoul(TerraGuardian guardian)
        {
            FlufflesData data = (FlufflesData)guardian.Data;
            data.SoulPosition = guardian.CenterPosition;
            data.SoulPosition.Y += guardian.Height * 0.25f;
            data.SoulPosition.X += guardian.Direction * guardian.Width * 0.25f;
            data.SoulVelocity = Vector2.Zero;
            data.SoulAttached = false;
            data.SoulOpacity = 0f;
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems to be telling that likes having your company.)");
            Mes.Add("(She shows you a portrait she carries with her, having a male and a female terrarian, a terraguardian, and her. She seems to be pointing at the terraguardian in the image. Maybe that was her boyfriend, or husband?)");
            Mes.Add("(She looks thoughtful.)");
            Mes.Add("(She touched your heart, then smiled at you, and then pulled the paw.)");
            Mes.Add("(She gave you a hug, which your felt, as she smiled while having her eyes closed.)");
            Mes.Add("(She kneels on the floor and stares at you for a while.)");
            Mes.Add("(She leaned on your shoulder for a while.)");
            Mes.Add("(She seems to be thanking you for breaking her haunt.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(After you asked, she shook her head.)");
            Mes.Add("(She waves the pointing finger left and right.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(After you asked if she had a request, she pulled a list of things she wants you to do.)");
            Mes.Add("(She seems to be happy after you asked that, then gave you a list of things she needs.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(After you reported the progress, she looked very joyful.)");
            Mes.Add("(Upon receiving what she asked, she gave you a thankful hug.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("(She stares at you with a menacing look, doesn't looks happy for not having a house to stay.)");
            }
            else
            {
                Mes.Add("(She sketches a house in the air using her fingers, she seems to want a place to live.)");
                Mes.Add("(She seems to be wanting a place to stay.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems very happy because of the surprise party that was prepared for her.)");
            Mes.Add("(She seems eager to return to the party.)");
            if (!PlayerMod.HasGuardianBeenGifted(player, guardian.ID, guardian.ModID))
            {
                Mes.Add("(She seems to be expecting you to give her something, but is trying to pretend not.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            if (IsPlayer && RevivePlayer.whoAmI == Guardian.OwnerPos)
            {
                Mes.Add("(Tear drops are falling on your body.)");
                Mes.Add("(She's trying to ease your pain.)");
                Mes.Add("(She's trying to make you comfortable on the floor.)");
            }
            else
            {
                if (!IsPlayer && Guardian.ID == Leopold && Guardian.ModID == MainMod.mod.Name)
                {
                    Mes.Add("(She seems to be trying her best to reanimate him.)");
                    Mes.Add("(She seems to be crying while attempting to revive him.)");
                    Mes.Add("(She seems to be trying to make him comfortable, while healing his wounds.)");
                }
                else
                {
                    Mes.Add("(She's trying to soothe the one she's tending.)");
                    Mes.Add("(She's trying to reduce the pain.)");
                    Mes.Add("(She seems to be tending the wounds.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID)
        {
            switch (MessageID)
            {
                case MessageIDs.RescueMessage:
                    return "(You feel a friendly presence tending you.)";
                case MessageIDs.GuardianWokeUpByPlayerMessage:
                    return "(She woke up, then yawned, and is looking at you with a sleepy face.)";
                case MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage:
                    return "(She jumped out of the bed, and is now looking at you excited, wanting to know if you did what she asked for.)";
                case MessageIDs.BuddySelected:
                    return "(She doesn't knows how to react for you to choosing her as buddy, she looks happy for the choice.)";
                //
                case MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess:
                    return "(She jumps out of excitement.)";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty:
                    return "(She looks at the size of your group, and then denies.)";
                case MessageIDs.AfterAskingCompanionToJoinYourGroupFail:
                    return "(She doesn't seems very interessed in joining your group right now.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure:
                    return "(She seems to be worried about the place you want her to leave the group. Are you sure you want to do that?)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer:
                    return "(She nods and then gives you a farewell.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace:
                    return "(She leaves your team with a sad look in her face.)";
                case MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer:
                    return "(She smiled after you said no.)";
                case MessageIDs.RequestAccepted:
                    return "(She nods at you. She seems to be counting on you.)";
                case MessageIDs.RequestCantAcceptTooManyRequests:
                    return "(She seems to have noticed that you have many things to do, and decided not to give you her request.)";
                case MessageIDs.RequestRejected:
                    return "(She looked a bit sad after you rejected.)";
                case MessageIDs.RequestPostpone:
                    return "(She's looking at you with a question mark face.)";
                case MessageIDs.RequestFailed:
                    return "(Her face shows the disappointment your failure brought, but she seems to recognize that you did your best.)";
                case MessageIDs.RestAskForHowLong:
                    return "(She seems to like the idea. She seems to be asking for how long will rest.)";
                case MessageIDs.RestNotPossible:
                    return "(She seems worried about the moment you asked that. Maybe It's better to try again another time.)";
                case MessageIDs.RestWhenGoingSleep:
                    return "(She yawned, then went to bed.)";
                case MessageIDs.AskPlayerToGetCloserToShopNpc:
                    return "(She's calling you attention, and pointing at [shop]'s shop.)";
                case MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping:
                    return "(She signals you to wait a moment.)";
                case MessageIDs.GenericYes:
                    return "(She nods)";
                case MessageIDs.GenericNo:
                    return "(She shook her head.)";
                case MessageIDs.GenericThankYou:
                    return "(She bows as a Thank You.)";
                case MessageIDs.ChatAboutSomething:
                    return "(She seems to be waiting for your questions.)";
                case MessageIDs.NevermindTheChatting:
                    return "(She nods to you.)";
                case MessageIDs.CancelRequestAskIfSure:
                    return "(She looks at you like as if didn't believed what you said. You're really wanting to cancel her request?)";
                case MessageIDs.CancelRequestYesAnswered:
                    return "(She lowers her head, then shook It slowly side ways.)";
                case MessageIDs.CancelRequestNoAnswered:
                    return "(She got a smile on her face after hearing that.)";
                case MessageIDs.LeopoldMessage1:
                    return "(Her face turned red while looking at the Rabbit Guardian. Her facial expression also changed, she seems to be shy.)";
                case MessageIDs.LeopoldMessage2:
                    return "*Is that... AAAAAHHH!! A ghost!!! Help!*";
                case MessageIDs.LeopoldMessage3:
                    return "(After he screamed, she looked at you with a sad face, and your character tried comforting her, while you looked at him with an angry face.)";
            }
            return base.GetSpecialMessage(MessageID);
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            g.AddFlag(GuardianFlags.HideKOBar);
            g.AddFlag(GuardianFlags.FallDamageImmunity);
            g.AddFlag(GuardianFlags.CantDie);
            g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
            //g.TrailLength += 2;
            //if(g.TrailDelay == 0)
            //    g.TrailDelay = 4;
            if (g.KnockedOut)
            {
                g.AddFlag(GuardianFlags.DontTakeAggro);
                g.AddFlag(GuardianFlags.CantBeHurt);
            }
        }

        public static Color GhostfyColor(Color Original, float KoAlpha, float ColorMod)
        {
            Color color = Original;
            color.R -= (byte)(color.R * ColorMod);
            color.G += (byte)((255 - color.G) * ColorMod);
            color.B += (byte)((255 - color.B) * ColorMod);
            color *= KoAlpha;
            color *= 0.75f;
            //color.A -= (byte)(color.A * ColorMod);
            return color;
        }

        public static float GetColorMod { get { return (float)Math.Sin(Main.GlobalTime * 3) * 0.3f + 0.3f; } }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            float KoAlpha = 1, ColorMod = 0;
            FlufflesData data = (FlufflesData)guardian.Data;
            KoAlpha = data.KnockoutAlpha;
            ColorMod = GetColorMod;
            foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
            {
                if(gdd.textureType == GuardianDrawData.TextureType.TGHeadAccessory)
                {
                    if (guardian.KnockedOut)
                    {
                        gdd.color = GhostfyColor(gdd.color, KoAlpha, 1f);
                    }
                }
                else if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
                }
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
            {
                if (gdd.textureType == GuardianDrawData.TextureType.TGHeadAccessory)
                {
                    if (guardian.KnockedOut)
                    {
                        gdd.color = GhostfyColor(gdd.color, KoAlpha, 1f);
                    }
                }
                else if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
                }
            }
            if(data.SoulOpacity > 0)
            {
                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.Effect, sprites.GetExtraTexture(FoxSoulTextureID), data.SoulPosition - Main.screenPosition, 
                    new Rectangle(16 * (int)(data.SoulFrame * 0.25f), 0, 16, 20), Color.White * data.SoulOpacity * 0.75f, 0f, new Vector2(8, 10), 1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
                Main.playerDrawData.Add(gdd.GetDrawData());
                //TerraGuardian.DrawFront.Add(gdd);
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            if (Frame == 0)
            {
                if (guardian.OffsetY >= 2)
                {
                    Frame = 2;
                }
                else if (guardian.OffsetY <= -2)
                {
                    Frame = 1;
                }
            }
            if (Frame == BackwardStanding)
            {
                if (guardian.OffsetY >= 2)
                {
                    Frame = BackwardStanding + 2;
                }
                else if (guardian.OffsetY <= -2)
                {
                    Frame = BackwardStanding + 1;
                }
            }
            if (guardian.Velocity.Y != 0 && Frame == JumpFrame)
            {
                if (Math.Abs(guardian.Velocity.Y) < 1)
                    Frame = 1;
                else if (guardian.Velocity.Y >= 1)
                {
                    Frame = 13;
                }
            }
            else if ((guardian.Velocity.X > 0 && guardian.Direction < 0) || (guardian.Velocity.X < 0 && guardian.Direction > 0))
            {
                if (Frame >= 3 && Frame <= 5)
                {
                    Frame += 9;
                }
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            FlufflesData data = (FlufflesData)guardian.Data;
            if (!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0) //guardian.BodyAnimationFrame != ReviveFrame
                guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
            guardian.ReviveBoost += 2;
            if (guardian.KnockedOut)
            {
                if (data.KnockoutAlpha > 0)
                {
                    data.KnockoutAlpha -= 0.005f;
                    if (data.KnockoutAlpha <= 0)
                    {
                        data.KnockoutAlpha = 0;
                        if(guardian.OwnerPos == -1)
                        {
                            guardian.WofFood = false;
                            guardian.RemoveBuff(Terraria.ID.BuffID.Horrified);
                            guardian.Spawn();
                        }
                    }
                }
                else
                {
                    if (guardian.OwnerPos > -1)
                    {
                        Player player = Main.player[guardian.OwnerPos];
                        guardian.Position.X = player.Center.X;
                        guardian.Position.Y = player.position.Y + player.height - 1;
                        guardian.LookingLeft = player.direction < 0;
                    }
                    else
                    {

                    }
                }
            }
            else
            {
                bool ReduceOpacity = Main.dayTime && !Main.eclipse && guardian.Position.Y < Main.worldSurface * 16 && Main.tile[(int)(guardian.Position.X * (1f / 16)), (int)(guardian.CenterY * (1f / 16))].wall == 0;
                if (ReduceOpacity)
                {
                    const float MinOpacity = 0.2f;
                    if (data.KnockoutAlpha > MinOpacity)
                    {
                        data.KnockoutAlpha -= 0.005f;
                        if (data.KnockoutAlpha < MinOpacity)
                            data.KnockoutAlpha = MinOpacity;
                    }
                    else if (data.KnockoutAlpha < MinOpacity)
                    {
                        data.KnockoutAlpha += 0.005f;
                        if (data.KnockoutAlpha > MinOpacity)
                            data.KnockoutAlpha = MinOpacity;
                    }
                }
                else if (data.KnockoutAlpha < 1)
                {
                    data.KnockoutAlpha += 0.005f;
                    if (data.KnockoutAlpha > 1)
                        data.KnockoutAlpha = 1;
                }
            }
            if (guardian.KnockedOut)
            {
                if (data.SoulOpacity < 1f)
                {
                    data.SoulOpacity += 1f / 60;
                    if (data.SoulOpacity > 1)
                        data.SoulOpacity = 1f;
                }
                else if(guardian.OwnerPos > -1)
                {
                    Player player = Main.player[guardian.OwnerPos];
                    if (data.SoulAttached)
                    {
                        Vector2 EndPosition = player.Center;
                        EndPosition.X += 4 * player.direction;
                        EndPosition.Y += 2 * player.gravDir;
                        data.SoulPosition.X += (EndPosition.X - data.SoulPosition.X) * 0.8f;
                        data.SoulPosition.Y += (EndPosition.Y - data.SoulPosition.Y) * 0.8f;
                    }
                    else if (data.KnockoutAlpha <= 0)
                    {
                        float Distance = player.Center.X - data.SoulPosition.X;
                        const float MaxSoulMoveSpeed = 6f;
                        bool AtPointHorizontally = false, AtPointVertically = false;
                        if (Math.Abs(Distance) < 20)
                        {
                            data.SoulVelocity.X *= 0.3f;
                            AtPointHorizontally = true; // Math.Abs(data.SoulVelocity.X) < 1f;
                        }
                        else
                        {
                            if (Distance < 0)
                            {
                                data.SoulVelocity.X -= 0.15f;
                                if (data.SoulVelocity.X < -MaxSoulMoveSpeed)
                                    data.SoulVelocity.X = -MaxSoulMoveSpeed;
                            }
                            else
                            {
                                data.SoulVelocity.X += 0.15f;
                                if (data.SoulVelocity.X > MaxSoulMoveSpeed)
                                    data.SoulVelocity.X = MaxSoulMoveSpeed;
                            }
                        }
                        Distance = player.Center.Y - data.SoulPosition.Y;
                        if (Math.Abs(Distance) < 28)
                        {
                            data.SoulVelocity.Y *= 0.3f;
                            AtPointVertically = true; //Math.Abs(data.SoulVelocity.Y) < 1f;
                        }
                        else
                        {
                            if (Distance < 0)
                            {
                                data.SoulVelocity.Y -= 0.15f;
                                if (data.SoulVelocity.Y < -MaxSoulMoveSpeed)
                                    data.SoulVelocity.Y = -MaxSoulMoveSpeed;
                            }
                            else
                            {
                                data.SoulVelocity.Y += 0.15f;
                                if (data.SoulVelocity.Y > MaxSoulMoveSpeed)
                                    data.SoulVelocity.Y = MaxSoulMoveSpeed;
                            }
                        }
                        data.SoulPosition += data.SoulVelocity;
                        if (AtPointHorizontally && AtPointVertically)
                        {
                            data.SoulAttached = true;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                if (data.SoulOpacity > 0)
                {
                    data.SoulOpacity -= 1f / 60;
                    if (data.SoulOpacity < 0)
                        data.SoulOpacity = 0f;
                }
            }
            if(data.SoulOpacity > 0)
            {
                Lighting.AddLight(data.SoulPosition, new Vector3(0.82f, 2.17f, 1.61f) * 0.33f * data.SoulOpacity);
                data.SoulFrame++;
                if (data.SoulFrame >= 4 * 6)
                    data.SoulFrame -= 4 * 6;
                if (Main.rand.Next(3) == 0)
                    Dust.NewDust(data.SoulPosition - new Vector2(8, 10), 16, 20, 75, 0f, -0.5f); //CursedTorchID
            }
        }

        public class FlufflesData : GuardianData
        {
            public Vector2 SoulPosition = Vector2.Zero, SoulVelocity = Vector2.Zero;
            public bool SoulAttached = false;
            public float SoulOpacity = 0f;
            public float KnockoutAlpha = 0f;
            public byte SoulFrame = 0;

            public FlufflesData(int ID, string ModID = "") : base(ID, ModID)
            {

            }
        }
    }
}
