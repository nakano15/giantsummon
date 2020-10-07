using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class GhostFoxGuardianBase : GuardianBase
    {
        public GhostFoxGuardianBase()
        {
            Name = "GhostFoxGuardian";
            Description = "";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 92;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 104;
            Age = 31;
            Male = false;
            InitialMHP = 200; //1000
            LifeCrystalHPBonus = 40;
            LifeFruitHPBonus = 10;
            Accuracy = 0.85f;
            Mass = 0.5f;
            MaxSpeed = 5.2f;
            Acceleration = 0.18f;
            SlowDown = 0.47f;
            MaxJumpHeight = 15;
            JumpSpeed = 7.08f;
            CanDuck = false;
            ReverseMount = true;
            DrinksBeverage = true;
            SetTerraGuardian();

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);

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
            //ThroneSittingFrame = 24;
            //BedSleepingFrame = 25;
            //SleepingOffset.X = 16;
            //ReviveFrame = 26;
            //DownedFrame = 27;
            //PetrifiedFrame = 28;

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

            //Right Arm
            RightHandPoints.AddFramePoint2x(6, 27, 3);
            RightHandPoints.AddFramePoint2x(7, 34, 11);
            RightHandPoints.AddFramePoint2x(8, 38, 20);
            RightHandPoints.AddFramePoint2x(9, 34, 29);

            //
        }

        public Color GhostfyColor(Color Original)
        {
            Color color = Original;
            color *= 0.75f;
            color.R -= (byte)(color.R * ColorMod);
            color.G += (byte)((255 - color.G) * ColorMod);
            color.B += (byte)((255 - color.B) * ColorMod);
            //color.A -= (byte)(color.A * ColorMod);
            return color;
        }

        private float ColorMod = 0;

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            ColorMod = (float)Math.Sin(Main.GlobalTime * 3) * 0.3f + 0.3f;
            foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
            {
                if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color);
                }
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
            {
                if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color);
                }
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
            if ((guardian.Velocity.X > 0 && guardian.Direction < 0) || (guardian.Velocity.X < 0 && guardian.Direction > 0))
            {
                if (Frame >= 3 && Frame <= 5)
                {
                    Frame += 9;
                }
            }
        }

        public override void GuardianBehaviorModScript(TerraGuardian guardian)
        {
            if (!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0) //guardian.BodyAnimationFrame != ReviveFrame
                guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
        }
    }
}
