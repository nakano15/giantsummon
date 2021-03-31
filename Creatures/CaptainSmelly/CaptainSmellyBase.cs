using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Creatures
{
    public class CaptainSmellyBase : GuardianBase
    {
        public const string PlasmaFalchionTextureID = "plasmafalchion", PhantomBlinkTextureID = "phantomblink", ScouterTextureID = "scouter", HeadNoScouterTextureID = "headnoscouter";
        public const int NumberOfSwords = 7;

        public CaptainSmellyBase()
        {
            Name = "CaptainSmelly";
            Description = "";
            Size = GuardianSize.Large;
            Width = 22;
            Height = 66;
            CharacterPositionYDiscount = 22;
            //DuckingHeight = 62;
            SpriteWidth = 160;
            SpriteHeight = 140;
            FramesInRows = 12;
            Age = 22;
            Male = false;
            InitialMHP = 100; //1100
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 5;
            Accuracy = 0.72f;
            Mass = 0.7f;
            MaxSpeed = 4.9f;
            Acceleration = 0.14f;
            SlowDown = 0.42f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.16f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = true;
            DontUseHeavyWeapons = true;
            SpecialAttackBasedCombat = true;
            SetTerraGuardian();

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };
            JumpFrame = 18;
            ItemUseFrames = new int[] { 22, 22, 23, 24 };
            HeavySwingFrames = new int[] { 43, 44, 45 };
            //DuckingFrame = 23;
            //DuckingSwingFrames = new int[] { 24, 25, 26 };
            //SittingFrame = 15;
            ChairSittingFrame = 75;
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 13, 14, 15, 16 });
            ThroneSittingFrame = 73;
            BedSleepingFrame = 74;
            DownedFrame = 25;
            ReviveFrame = 26;

            //
            RightArmFrontFrameSwap.Add(1, 1);
            for(int i = 10; i < 18; i++)
                RightArmFrontFrameSwap.Add(i, 1);
            RightArmFrontFrameSwap.Add(20, 2);
            RightArmFrontFrameSwap.Add(21, 3);
            RightArmFrontFrameSwap.Add(22, 4);
            RightArmFrontFrameSwap.Add(23, 5);
            RightArmFrontFrameSwap.Add(24, 6);
            RightArmFrontFrameSwap.Add(25, 7);
            RightArmFrontFrameSwap.Add(26, 8);
            for(int i = 35; i < 43; i++)
                RightArmFrontFrameSwap.Add(i, 9);
            RightArmFrontFrameSwap.Add(43, 10);
            RightArmFrontFrameSwap.Add(44, 0);
            RightArmFrontFrameSwap.Add(45, 11);
            //
            RightArmFrontFrameSwap.Add(46, 12);
            RightArmFrontFrameSwap.Add(47, 12);
            RightArmFrontFrameSwap.Add(48, 12);
            RightArmFrontFrameSwap.Add(49, 12);
            RightArmFrontFrameSwap.Add(50, 13);
            RightArmFrontFrameSwap.Add(51, 14);
            RightArmFrontFrameSwap.Add(52, 15);
            RightArmFrontFrameSwap.Add(53, 15);
            RightArmFrontFrameSwap.Add(54, 15);
            RightArmFrontFrameSwap.Add(55, 15);
            RightArmFrontFrameSwap.Add(56, 15);
            //
            RightArmFrontFrameSwap.Add(57, 1);
            RightArmFrontFrameSwap.Add(58, 1);
            RightArmFrontFrameSwap.Add(59, 1);
            //
            RightArmFrontFrameSwap.Add(60, 3);
            RightArmFrontFrameSwap.Add(61, 3);
            RightArmFrontFrameSwap.Add(62, 3);
            //
            RightArmFrontFrameSwap.Add(63, 16);
            RightArmFrontFrameSwap.Add(64, 16);
            RightArmFrontFrameSwap.Add(65, 16);
            RightArmFrontFrameSwap.Add(66, 16);
            RightArmFrontFrameSwap.Add(67, 17);
            RightArmFrontFrameSwap.Add(68, 17);
            RightArmFrontFrameSwap.Add(69, 17);
            RightArmFrontFrameSwap.Add(70, 17);
            RightArmFrontFrameSwap.Add(71, 17);
            RightArmFrontFrameSwap.Add(72, 17);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(57, 46, 44);
            LeftHandPoints.AddFramePoint2x(58, 46, 47);
            LeftHandPoints.AddFramePoint2x(59, 48, 34);

            LeftHandPoints.AddFramePoint2x(60, 42, 43);
            LeftHandPoints.AddFramePoint2x(61, 45, 37);
            LeftHandPoints.AddFramePoint2x(62, 44, 31);

            //Right Arm
            RightHandPoints.AddFramePoint2x(22, 50, 34);
            RightHandPoints.AddFramePoint2x(23, 50, 40);
            RightHandPoints.AddFramePoint2x(24, 48, 45);

            RightHandPoints.AddFramePoint2x(26, 53, 52);

            SubAttacksSetup();
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(PlasmaFalchionTextureID, "plasma_falchion");
            sprites.AddExtraTexture(PhantomBlinkTextureID, "phantom_blink");
            sprites.AddExtraTexture(ScouterTextureID, "scouter");
            sprites.AddExtraTexture(HeadNoScouterTextureID, "head_no_scouter");
        }

        public void SubAttacksSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(); //V Swing
            special.MinRange = 16;
            special.MaxRange = 52;
            special.CanMove = true;
            for(int i = 43; i < 45; i++)
            {
                AddNewSubAttackFrame(4, -1, -1, i);
            }
            AddNewSubAttackFrame(8, -1, -1, 45);
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
                if(Frame == 1)
                {
                    Rectangle AttackHitbox = new Rectangle(-16 * tg.Direction + (int)tg.Position.X, -110 + (int)tg.Position.Y, 78, 94);
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = 8;
                    int CriticalRate = 4;
                    float Knockback = 6f;
                    for(int n = 0; n < 200; n++)
                    {
                        if(Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(AttackHitbox))
                        {
                            if (!Main.npc[n].dontTakeDamage)
                            {
                                int HitDirection = tg.Direction;
                                if ((HitDirection == -1 && tg.CenterPosition.X < Main.npc[n].Center.X) ||
                                    (HitDirection == 1 && tg.CenterPosition.X > Main.npc[n].Center.X))
                                {
                                    HitDirection *= -1;
                                }
                                bool Critical = (Main.rand.Next(100) < CriticalRate);
                                int NewDamage = Damage;
                                if (tg.OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
                                {
                                    float DamageMult = Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                                    NewDamage = (int)(NewDamage * DamageMult);
                                }
                                double result = Main.npc[n].StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (Critical)
                                        tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Luck); //(float)result
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
            };

        }

        public override GuardianData GetGuardianData(int ID = -1, string ModID = "")
        {
            return new CaptainSmellyData(ID, ModID);
        }

        public override void GuardianModifyDrawHeadScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, float Scale, SpriteEffects seffect, ref List<GuardianDrawData> gdds)
        {
            if (guardian.LookingLeft)
            {
                foreach (GuardianDrawData gdd in gdds)
                {
                    if (gdd.textureType == GuardianDrawData.TextureType.TGHead)
                    {
                        gdd.Texture = sprites.GetExtraTexture(HeadNoScouterTextureID);
                    }
                }
            }
        }

        public override void GuardianUpdateScript(TerraGuardian guardian)
        {
            CaptainSmellyData data = (CaptainSmellyData)guardian.Data;
            if (guardian.IsAttackingSomething)
                data.HoldingWeaponTime = 5 * 60;
            else if(data.HoldingWeaponTime > 0)
            {
                data.HoldingWeaponTime--;
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            CaptainSmellyData data = (CaptainSmellyData)guardian.Data;
            bool UsingWeapon = data.HoldingWeaponTime > 0;
            if(guardian.Velocity.Y > 0)
            {
                Frame++;
            }
            else if (guardian.Velocity.X != 0 && guardian.Velocity.Y == 0 && guardian.DashCooldown < 0)
            {
                Frame += 25;
            }
            if (UsingWeapon)
            {
                if (Frame == 0)
                    Frame++;
                if (Frame >= 2 && Frame < 10)
                    Frame += 8;
                if (Frame == 18 || Frame == 19)
                    Frame += 2;
                if (Frame >= 27 && Frame < 35)
                    Frame += 8;
            }
            else
            {
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            CaptainSmellyData data = (CaptainSmellyData)guardian.Data;
            bool UsingWeapon = data.HoldingWeaponTime > 0;
            for (int i = TerraGuardian.DrawBehind.Count - 1; i >= 0; i--)
            {
                const bool DrawnBehind = true;
                switch (TerraGuardian.DrawBehind[i].textureType)
                {
                    //case GuardianDrawData.TextureType.TGRightArm:
                    case GuardianDrawData.TextureType.TGRightArmFront:
                        {
                            if (UsingWeapon)
                                PlaceSwordSpriteAt(i, DrawnBehind, data.SwordID, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!guardian.LookingLeft)
                            {
                                PlaceScouterSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                            }
                            if (guardian.BodyAnimationFrame >= 63 && guardian.BodyAnimationFrame < 71)
                                PlacePhantomBlinkSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                }
            }
            for (int i = TerraGuardian.DrawFront.Count - 1; i >= 0; i--)
            {
                const bool DrawnBehind = false;
                switch (TerraGuardian.DrawFront[i].textureType)
                {
                    //case GuardianDrawData.TextureType.TGRightArm:
                    case GuardianDrawData.TextureType.TGRightArmFront:
                        {
                            if (UsingWeapon)
                                PlaceSwordSpriteAt(i, DrawnBehind, data.SwordID, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                    case GuardianDrawData.TextureType.TGBody:
                        {
                            if (!guardian.LookingLeft)
                            {
                                PlaceScouterSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                            }
                            if (guardian.BodyAnimationFrame >= 63 && guardian.BodyAnimationFrame < 71)
                                PlacePhantomBlinkSpriteAt(i, DrawnBehind, guardian, DrawPosition, color, Rotation, Origin, Scale, seffect);
                        }
                        break;
                }
            }
        }

        public void PlacePhantomBlinkSpriteAt(int Position, bool DrawBehind, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Position++;
            int Frame = 0;
            switch (guardian.BodyAnimationFrame)
            {
                case 64:
                    Frame = 1;
                    break;
                case 65:
                case 66:
                    Frame = 2;
                    break;
                case 67:
                case 68:
                case 69:
                case 70:
                    Frame = 3;
                    break;
            }
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(PhantomBlinkTextureID), DrawPosition, new Rectangle(Frame * SpriteWidth, 0, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public void PlaceScouterSpriteAt(int Position, bool DrawBehind, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Position++;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(ScouterTextureID), DrawPosition, guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public void PlaceSwordSpriteAt(int Position, bool DrawBehind, int SwordID, TerraGuardian guardian, Vector2 DrawPosition, Color color, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            int FrameX = 0, FrameY = ((NumberOfSwords - 1) - SwordID) * 2;
            switch (guardian.RightArmAnimationFrame)
            {
                case 20:
                    FrameX = 1;
                    break;
                case 21:
                    FrameX = 2;
                    break;
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                    FrameX = 3;
                    break;
                case 43:
                    FrameX = 4;
                    break;
                case 44:
                    FrameX = 5;
                    break;
                case 45:
                    FrameX = 6;
                    break;
                    //
                case 46:
                    FrameX = 7;
                    break;
                case 47:
                    FrameX = 8;
                    break;
                case 48:
                    FrameX = 9;
                    break;
                case 49:
                    FrameX = 10;
                    break;
                case 50:
                    FrameX = 11;
                    break;
                case 51:
                    FrameX = 12;
                    break;
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                    FrameX = 13;
                    break;
                //
                case 57:
                case 58:
                case 59:
                    FrameX = 14;
                    break;
                case 60:
                case 61:
                case 62:
                    FrameX = 15;
                    break;
                //
                case 63:
                case 64:
                case 65:
                case 66:
                    FrameX = 16;
                    break;
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                    FrameX = 17;
                    break;
            }
            if(FrameX >= FramesInRows)
            {
                FrameY += FrameX / FramesInRows;
                FrameX -= FrameY * FramesInRows;
            }
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(PlasmaFalchionTextureID), DrawPosition, new Rectangle(FrameX * SpriteWidth, FrameY * SpriteHeight, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public class CaptainSmellyData : GuardianData
        {
            public int HoldingWeaponTime = 0, SwordID = 0;

            public CaptainSmellyData(int ID, string ModID) : base(ID, ModID)
            {

            }
        }
    }
}
