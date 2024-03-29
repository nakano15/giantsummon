﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using giantsummon.Trigger;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Companions
{
    public class BaphaBase : GuardianBase
    {
        private const int DownedFrameID = 21, RevivingFrameID = 28;
        public const string HellPortalSpriteID = "hellgate", HellSplitterTextureID = "hellsplitter", FireEffectsTextureID = "firefxs", CrimsonFireEffectsTextureID = "crimfirefx",
            TransformTextureID = "transform", CrescentMoonShredderPentagram = "c_pentagram";
        private const string RightWingTextureID = "rwing";
        private const string AwakenedBodyTextureID = "aw_body", AwakenedBodyGlowTextureID = "aw_body_glow", AwakenedLeftArmGlowTextureID = "aw_leftarm", AwakenedRightArmGlowTextureID = "aw_rightarm", 
            AwakenedBodyFrontGlowTextureID = "aw_bodyfront", AwakenedRightArmFrontGlowTextureID = "aw_rightarmfront", AwakenedWingRightTextureID = "aw_rwing";
        private const int HellGateAnimationFrameTime = 8;
        private const byte CrimsonFlameID = 0;

        public BaphaBase()
        {
            Name = "Bapha";
            Description = "Overlord of Hell.";
            Size = GuardianSize.Large;
            Width = 17 * 2; //17
            Height = 62 * 2; //61
            Scale = 1.333f;
            CompanionSlotWeight = 3f;
            ForceScale = true;
            SpriteWidth = 99 * 2;
            SpriteHeight = 91 * 2;
            FramesInRows = 11;
            Age = 104357;
            SetBirthday(SEASON_SUMMER, 6);
            Male = true;
            InitialMHP = 666; //1000
            LifeCrystalHPBonus = 100;
            LifeFruitHPBonus = 5;
            InitialMP = 333;
            ManaCrystalMPBonus = 30;
            Accuracy = 0.15f;
            Mass = 0.5f;
            MaxSpeed = 5.2f;
            Acceleration = 0.18f;
            SlowDown = 0.47f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.08f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = false;
            SpecialAttackBasedCombat = true;
            //UsesRightHandByDefault = true;
            //ForceWeaponUseOnMainHand = true;
            CompanionContributorName = "Smokey";
            SetTerraGuardian();
            VladimirBase.AddCarryBlacklist(Bapha);

            StandingFrame = 0;
            WalkingFrames = new int[] { 3, 4, 5, 6, 7, 8, 9, 10 };
            JumpFrame = 12;
            ItemUseFrames = new int[] { 17, 18, 19, 20 };
            SittingFrame = 60;
            ThroneSittingFrame = 59;
            //BedSleepingFrame = 25;
            //SleepingOffset.X = 16;
            ReviveFrame = RevivingFrameID;
            DownedFrame = DownedFrameID;

            SittingPoint2x = new Point(48, 74);

            RightArmFrontFrameSwap.Add(1, 0);
            RightArmFrontFrameSwap.Add(59, 1);
            RightArmFrontFrameSwap.Add(60, 2);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(60, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(17, 51, 26);
            LeftHandPoints.AddFramePoint2x(18, 68, 38);
            LeftHandPoints.AddFramePoint2x(19, 66, 54);
            LeftHandPoints.AddFramePoint2x(20, 61, 65);

            LeftHandPoints.AddFramePoint2x(28, 57, 69);
            LeftHandPoints.AddFramePoint2x(29, 57, 69);
            LeftHandPoints.AddFramePoint2x(30, 57, 69);
            LeftHandPoints.AddFramePoint2x(31, 57, 69);

            LeftHandPoints.AddFramePoint2x(37, 23, 37);
            LeftHandPoints.AddFramePoint2x(38, 51, 25);
            LeftHandPoints.AddFramePoint2x(39, 72, 69);
            LeftHandPoints.AddFramePoint2x(40, 71, 73);
            LeftHandPoints.AddFramePoint2x(41, 71, 73);
            LeftHandPoints.AddFramePoint2x(42, 71, 73);

            LeftHandPoints.AddFramePoint2x(43, 26, 51);

            //Right Arm
            RightHandPoints.AddFramePoint2x(17, 63, 28);
            RightHandPoints.AddFramePoint2x(18, 74, 40);
            RightHandPoints.AddFramePoint2x(19, 73, 54);
            RightHandPoints.AddFramePoint2x(20, 64, 64);

            RightHandPoints.AddFramePoint2x(28, 66, 66);
            RightHandPoints.AddFramePoint2x(29, 66, 66);
            RightHandPoints.AddFramePoint2x(30, 66, 66);
            RightHandPoints.AddFramePoint2x(31, 66, 66);

            RightHandPoints.AddFramePoint2x(43, 73, 51);

            //Shoulders
            MountShoulderPoints.DefaultCoordinate2x = new Point(42, 44);

            //Wings
            WingPosition.DefaultCoordinate2x = new Point(-1000, -1000);

            SubAttackSetup();
        }

        public override ushort GuardianSubAttackBehaviorAI(TerraGuardian Owner, CombatTactic tactic, Vector2 TargetPosition, Vector2 TargetVelocity, int TargetWidth, int TargetHeight, ref bool Approach, ref bool Retreat, ref bool Jump, ref bool Couch, out bool DefaultBehavior)
        {
            DefaultBehavior = false;
            if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Owner.Position.X) < 100)
                Retreat = true;
            if (Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Owner.Position.X) > 600)
                Approach = true;
            if(Owner.CanUseSubAttack(1) && Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Owner.Position.X) < 120 * Owner.Scale + TargetWidth * 0.5f &&
                Math.Abs(TargetPosition.Y + TargetHeight * 0.5f - Owner.CenterY) < 120 * Owner.Scale + TargetHeight * 0.5f)
            {
                return 2;
            }
            return 0;
            //return base.GuardianSubAttackBehaviorAI(Owner, tactic, TargetPosition, TargetVelocity, TargetWidth, TargetHeight, ref Approach, ref Retreat, ref Jump, ref Couch, out DefaultBehavior);
        }

        public void SubAttackSetup()
        {
            GuardianSpecialAttack specialAttack = AddNewSubAttack(new Creatures.Bapha.HellFlameAttack());
            specialAttack = AddNewSubAttack(new Creatures.Bapha.CrimsonFlameAttack());
            specialAttack = AddNewSubAttack(new Creatures.Bapha.HellSplitterAttack());
        }

        public static bool IsAwakened(TerraGuardian g)
        {
            if (g.Data is BaphaData)
                return ((BaphaData)g.Data).AwakenedTime > 0;
            return false;
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.CantTakeDamageWhenKod);
            g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            g.AddFlag(GuardianFlags.HideKOBar);
            g.AddFlag(GuardianFlags.NotPulledWhenKOd);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HellPortalSpriteID, "Hell_Portal");
            sprites.AddExtraTexture(HellSplitterTextureID, "HellSplitter");
            sprites.AddExtraTexture(RightWingTextureID, "wing_r");
            sprites.AddExtraTexture(FireEffectsTextureID, "fire_and_despawn_effect");
            sprites.AddExtraTexture(CrimsonFireEffectsTextureID, "crimson_fire");
            sprites.AddExtraTexture(TransformTextureID, "transform");
            sprites.AddExtraTexture(CrescentMoonShredderPentagram, "pentagram_sigil");
            //
            sprites.AddExtraTexture(AwakenedBodyTextureID, "aw_body");
            sprites.AddExtraTexture(AwakenedBodyGlowTextureID, "aw_body_glow");
            sprites.AddExtraTexture(AwakenedLeftArmGlowTextureID, "aw_left_arm_glow");
            sprites.AddExtraTexture(AwakenedRightArmGlowTextureID, "aw_right_arm_glow");
            sprites.AddExtraTexture(AwakenedBodyFrontGlowTextureID, "aw_body_front_glow");
            sprites.AddExtraTexture(AwakenedRightArmFrontGlowTextureID, "aw_right_arm_front_glow");
            sprites.AddExtraTexture(AwakenedWingRightTextureID, "aw_wing_r");
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            if (guardian.SittingOnPlayerMount || (Frame != ReviveFrame && (BodyPartID == 1 && TerraGuardian.UsingLeftArmAnimation) || (BodyPartID == 2 && TerraGuardian.UsingRightArmAnimation)))
                return;
            if (guardian.Velocity.Y != 0 && guardian.WingMaxFlightTime == 0)
            {
                if (guardian.Velocity.Y < -1.5f)
                {
                    Frame = 11;
                }
                else if (guardian.Velocity.Y < 1.5f)
                {
                    Frame = 12;
                }
                else
                {
                    Frame = 13;
                }
            }
            else if((guardian.Velocity.X != 0 && guardian.DashCooldown <= 0 && guardian.DashSpeed > guardian.MoveSpeed) || (guardian.WingMaxFlightTime > 0 && guardian.Velocity.Y != 0))
            {
                const float FrameTime = 6;
                if (guardian.AnimationTime >= FrameTime * 5)
                {
                    guardian.AnimationTime -= FrameTime * 5;
                }
                else if (guardian.AnimationTime < 0)
                {
                    guardian.AnimationTime += FrameTime * 5;
                }
                Frame = 32 + (int)(guardian.AnimationTime / FrameTime);
            }
            else
            {
                switch (Frame)
                {
                    case 0:
                        {
                            if(guardian.AnimationTime >= 3600)
                            {
                                Frame = 1;
                            }
                        }
                        break;
                    case DownedFrameID:
                        {
                            const float AnimationDuration = 6;
                            if (guardian.AnimationTime > AnimationDuration * 7)
                                guardian.AnimationTime = AnimationDuration * 7;
                            byte PickedFrame = (byte)(guardian.AnimationTime / AnimationDuration);
                            Frame = PickedFrame + DownedFrame;
                        }
                        break;
                    case RevivingFrameID:
                        {
                            const float AnimationFrameDuration = 6;
                            if (guardian.AnimationTime >= AnimationFrameDuration * 4)
                                guardian.AnimationTime -= AnimationFrameDuration * 4;
                            byte PickedFrame = (byte)(guardian.AnimationTime / AnimationFrameDuration);
                            Frame = PickedFrame + RevivingFrameID;
                        }
                        break;
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            GuardianDrawData gdd;
            /*if (guardian.BodyAnimationFrame >= 22 && guardian.BodyAnimationFrame <= 28)
            {
                if(guardian.AnimationTime >= HellGateAnimationFrameTime * 6)
                {
                    TerraGuardian.DrawFront.Clear();
                    TerraGuardian.DrawBehind.Clear();
                }
                if(guardian.AnimationTime < HellGateAnimationFrameTime * 13)
                {
                    int Frame = (int)(guardian.AnimationTime / HellGateAnimationFrameTime);
                    if(Frame > 6)
                    {
                        Frame = 6 - (Frame + 6); //6 - 12 + 6 = 0, 6 - 7 + 6 = 5
                    }
                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HellGateSpriteID), DrawPosition, 
                        new Rectangle(SpriteWidth * Frame, 0, SpriteWidth, SpriteHeight), Color.White, Rotation, Origin, Scale, SpriteEffects.None);
                    TerraGuardian.DrawFront.Add(gdd);
                }
            }*/
            bool Awakened = false;
            if (Awakened)
            {
                ReplaceTexture(GuardianDrawData.TextureType.TGBody, sprites.GetExtraTexture(AwakenedBodyTextureID));
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedBodyGlowTextureID), DrawPosition,
                    guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), Color.White, Rotation, Origin, Scale, seffect);
                InjectTextureAfter(GuardianDrawData.TextureType.TGBody, gdd);
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedLeftArmGlowTextureID), DrawPosition, 
                    guardian.GetAnimationFrameRectangle(guardian.LeftArmAnimationFrame),  Color.White, Rotation, Origin, Scale, seffect);
                InjectTextureAfter(GuardianDrawData.TextureType.TGLeftArm, gdd);
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedRightArmGlowTextureID), DrawPosition,
                    guardian.GetAnimationFrameRectangle(guardian.RightArmAnimationFrame), Color.White, Rotation, Origin, Scale, seffect);
                InjectTextureAfter(GuardianDrawData.TextureType.TGRightArm, gdd);
                if (guardian.Base.BodyFrontFrameSwap.ContainsKey(guardian.BodyAnimationFrame))
                {
                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedBodyFrontGlowTextureID), DrawPosition,
                        guardian.GetAnimationFrameRectangle(guardian.Base.BodyFrontFrameSwap[guardian.BodyAnimationFrame]), Color.White, Rotation, Origin, Scale, seffect);
                    InjectTextureAfter(GuardianDrawData.TextureType.TGBodyFront, gdd);
                }
                if (guardian.Base.RightArmFrontFrameSwap.ContainsKey(guardian.RightArmAnimationFrame))
                {
                    gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedRightArmFrontGlowTextureID), DrawPosition,
                        guardian.GetAnimationFrameRectangle(guardian.Base.RightArmFrontFrameSwap[guardian.RightArmAnimationFrame]), Color.White, Rotation, Origin, Scale, seffect);
                    InjectTextureAfter(GuardianDrawData.TextureType.TGRightArmFront, gdd);
                }
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(AwakenedWingRightTextureID), DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                    Color.White, Rotation, Origin, Scale, seffect);
                TerraGuardian.DrawBehind.Insert(0, gdd);
            }
            else
            {
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(RightWingTextureID), DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame),
                    color, Rotation, Origin, Scale, seffect);
                TerraGuardian.DrawBehind.Insert(0, gdd);
            }
            byte FlamesEffect = 255;
            if(guardian.KnockedOut && guardian.BodyAnimationFrame == 28)
            {
                TerraGuardian.DrawFront.Clear();
                TerraGuardian.DrawBehind.Clear();
                return;
            }
            if (guardian.LeftArmAnimationFrame >= 14 && guardian.LeftArmAnimationFrame < 17)
            {
                FlamesEffect = (byte)(guardian.LeftArmAnimationFrame - 14);
            }
            else if (guardian.LeftArmAnimationFrame >= 28 && guardian.LeftArmAnimationFrame < 32)
            {
                FlamesEffect = (byte)(guardian.LeftArmAnimationFrame - 28 + 10);
            }
            else if (guardian.BodyAnimationFrame >= 21 && guardian.BodyAnimationFrame < 28)
            {
                FlamesEffect = (byte)(guardian.BodyAnimationFrame - 21 + 3);
            }
            if(FlamesEffect < 255)
            {
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(Awakened && FlamesEffect < 4 ? CrimsonFireEffectsTextureID : FireEffectsTextureID), DrawPosition, 
                    guardian.GetAnimationFrameRectangle(FlamesEffect), Color.White, Rotation, Origin, Scale, seffect);
                InjectTextureAfter(GuardianDrawData.TextureType.TGLeftArm, gdd);
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            BaphaData data = (BaphaData)guardian.Data;
            if (data.AwakenedTime > 0)
                data.AwakenedTime--;
            else if(data.KillTime> 0)
            {
                data.KillTime--;
                if(data.KillTime == 0)
                {
                    data.KillCount = 0;
                }
            }
        }

        public override string GreetMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Mwahaha! I'm evil!*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*What is It, mortal?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You want to know a secret from me? Well, you wont.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NoRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I want nothing right now.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HasRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need you to do me a favor, would you do it? No worry, I will reward you handsomelly for it. I need you to [objective], do I hear a yes coming from you?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string CompletedRequestMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I would have destroyed you if you didn't do that.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string BirthdayMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Am I supposed to be happy with this?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string HomelessMessage(Player player, TerraGuardian guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I need a shrine so I can spread my influence through this land.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessage(TerraGuardian Guardian, bool IsPlayer, Player RevivePlayer, TerraGuardian ReviveGuardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*This will make you feel a little warm.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string GetSpecialMessage(string MessageID) //Check other companion scripts
        {
            return base.GetSpecialMessage(MessageID);
        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new BaphaData(ID, ModID);
        }

        public class BaphaData : GuardianData
        {
            public byte KillCount = 0;
            public int KillTime = 0;
            public const int MaxKillTime = 20 * 60 * 60;
            public int AwakenedTime = 0;

            public BaphaData(int Id, string ModId) : base(Id, ModId)
            {

            }

            public void IncreaseKillTime()
            {
                if (AwakenedTime > 0) return;
                KillCount++;
                if (KillTime == 0)
                    KillTime = MaxKillTime;
                if(KillCount >= 10)
                {
                    AwakenedTime = 60 * 60 * 60;
                    KillCount = 0;
                    KillTime = 0;
                }
            }
        }
    }
}
