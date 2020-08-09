using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Creatures
{
    public class WrathBase : GuardianBase
    {
        public WrathBase() //I'll need to think how I'll make the cloud form of them work, and toggle.
            : base()
        {
            Name = "Wrath";
            Description = "One of the emotion pieces fragments\nof a TerraGuardian. Very volatile.";
            Size = GuardianSize.Medium;
            Width = 10 * 2;
            Height = 27 * 2;
            SpriteWidth = 64;
            SpriteHeight = 64;
            FramesInRows = 25;
            //DuckingHeight = 54;
            Age = 11;
            Male = true;
            InitialMHP = 110; //320
            LifeCrystalHPBonus = 15;
            LifeFruitHPBonus = 5;
            Accuracy = 0.67f;
            Mass = 0.40f;
            MaxSpeed = 3.62f;
            Acceleration = 0.12f;
            SlowDown = 0.35f;
            MaxJumpHeight = 12;
            JumpSpeed = 9.76f;
            CanDuck = false;
            ReverseMount = true;
            DontUseHeavyWeapons = true;
            SetTerraGuardian();

            MountUnlockLevel = 255;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            SittingFrame = 14;
            ChairSittingFrame = 14;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 15 });
            ThroneSittingFrame = 16;
            BedSleepingFrame = 18;
            DownedFrame = 15;
            ReviveFrame = 18;

            SittingPoint2x = new Microsoft.Xna.Framework.Point(15, 24);

            for (int i = 0; i < 10; i++)
                RightArmFrontFrameSwap.Add(i, i);

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(14, 0);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 10, 3);
            LeftHandPoints.AddFramePoint2x(11, 22, 9);
            LeftHandPoints.AddFramePoint2x(12, 23, 17);
            LeftHandPoints.AddFramePoint2x(13, 22, 20);

            LeftHandPoints.AddFramePoint2x(17, 24, 26);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 3);
            RightHandPoints.AddFramePoint2x(11, 25, 9);
            RightHandPoints.AddFramePoint2x(12, 26, 17);
            RightHandPoints.AddFramePoint2x(13, 23, 20);

            RightHandPoints.AddFramePoint2x(17, 26, 26);
        }

        public bool GetIfIsCloudForm(TerraGuardian guardian)
        {
            return true;
            if (guardian.OwnerPos > -1)
            {
                return Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PlayerMod.AngerPigGuardianID];
            }
            else
            {
                return Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().PigGuardianCloudForm[PlayerMod.AngerPigGuardianID];
            }
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte AnimationID, ref int Frame)
        {
            bool CloudForm = GetIfIsCloudForm(guardian);
            if (CloudForm)
            {
                if (guardian.Velocity.X != 0)
                {
                    if (AnimationID == 0)
                    {
                        Frame = 20;
                    }
                    else
                    {
                        if (!guardian.ExecutingAttackAnimation)
                        {
                            Frame = 20;
                        }
                    }
                }
                else
                {
                    if (AnimationID == 0)
                    {
                        Frame = 19;
                    }
                }
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Microsoft.Xna.Framework.Vector2 DrawPosition, Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color armorColor, float Rotation, Microsoft.Xna.Framework.Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            const float Opacity = 0.8f;
            if (GetIfIsCloudForm(guardian))
            {
                foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
                {
                    if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                    {
                        gdd.color *= Opacity;
                    }
                }
                foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
                {
                    if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                        gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                    {
                        gdd.color *= Opacity;
                    }
                }
            }

        }

        public override void Attributes(TerraGuardian g)
        {
            g.MeleeDamageMultiplier += 0.05f;
            g.Defense -= 4;
            if (GetIfIsCloudForm(g))
            {
                //if (!g.HasFlag(GuardianFlags.NoGravity))
                //    g.AddFlag(GuardianFlags.NoGravity);
                //if (!g.HasFlag(GuardianFlags.NoTileCollision))
                //    g.AddFlag(GuardianFlags.NoTileCollision);
            }
        }

        public override void GuardianBehaviorModScript(TerraGuardian guardian)
        {
            if (GetIfIsCloudForm(guardian))
            {
                /*if (guardian.Jump)
                {
                    if (guardian.Velocity.Y < -3f)
                    {
                        guardian.Velocity.Y = -3f;
                    }
                    else
                    {
                        guardian.Velocity.Y += -0.05f;
                    }
                }
                else*/
                {
                    if (guardian.Velocity.Y >= 2.3f)
                    {
                        guardian.Velocity.Y = 2.3f;
                        guardian.SetFallStart();
                    }
                    else
                    {
                        //guardian.Velocity.Y += 0.05f;
                    }
                }
                if(!guardian.KnockedOut && !guardian.MountedOnPlayer)
                    guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 5;
            }
        }
    }
}
