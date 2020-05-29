using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures
{
    public class MalishaBase : GuardianBase
    {
        public MalishaBase()
        {
            Name = "Malisha";
            Description = "";
            Size = GuardianSize.Large;
            Width = 28;
            Height = 84;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 15;
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
            CallUnlockLevel = 0;

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;
            
            AddInitialItem(Terraria.ID.ItemID.WoodenSword, 1);
            AddInitialItem(Terraria.ID.ItemID.Mushroom, 3);

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            HeavySwingFrames = new int[] { 14, 15, 16 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 18;
            ChairSittingFrame = 17;
            DrawLeftArmInFrontOfHead.AddRange(new int[] { 9, 10, 11, 16, 17, 21, 22 });
            //ThroneSittingFrame = 24;
            //BedSleepingFrame = 25;
            //SleepingOffset.X = 16;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 1);

            SittingPoint = new Point(21 * 2, 36 * 2);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 13, 2);
            LeftHandPoints.AddFramePoint2x(11, 33, 10);
            LeftHandPoints.AddFramePoint2x(12, 35, 18);
            LeftHandPoints.AddFramePoint2x(13, 31, 27);

            LeftHandPoints.AddFramePoint2x(14, 5, 7);
            LeftHandPoints.AddFramePoint2x(15, 31, 6);
            LeftHandPoints.AddFramePoint2x(16, 41, 40);

            //Right Arm
            RightHandPoints.AddFramePoint2x(10, 15, 2);
            RightHandPoints.AddFramePoint2x(11, 35, 10);
            RightHandPoints.AddFramePoint2x(12, 37, 18);
            RightHandPoints.AddFramePoint2x(13, 33, 27);

            RightHandPoints.AddFramePoint2x(14, 7, 7);
            RightHandPoints.AddFramePoint2x(15, 33, 6);
            RightHandPoints.AddFramePoint2x(16, 43, 40);

            //MountedPosition
            MountShoulderPoints.DefaultCoordinate2x = new Point(16, 31);
            MountShoulderPoints.AddFramePoint2x(1, 17, 31);
            MountShoulderPoints.AddFramePoint2x(2, 18, 30);
            MountShoulderPoints.AddFramePoint2x(3, 17, 31);
            MountShoulderPoints.AddFramePoint2x(5, 15, 31);
            MountShoulderPoints.AddFramePoint2x(6, 14, 30);
            MountShoulderPoints.AddFramePoint2x(7, 15, 31);

            MountShoulderPoints.AddFramePoint2x(14, 20, 31);
            MountShoulderPoints.AddFramePoint2x(15, 22, 31);
            MountShoulderPoints.AddFramePoint2x(16, 25, 30);

            //Hat Position
            HeadVanityPosition.DefaultCoordinate2x = new Point(22, 10);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture("tails", "tails");
        }

        public override void GuardianPreDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
            Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
            if (TailTexture == null)
            {
                return;
            }
            if (!guardian.PlayerMounted)
            {
                GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
                rect.Y += rect.Height * 2;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
            else
            {
                rect.Y += rect.Height;
                GuardianDrawData dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
                rect.Y += rect.Height * 2;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, false);
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, Microsoft.Xna.Framework.Graphics.SpriteEffects seffect)
        {
            if (guardian.PlayerMounted)
            {
                Rectangle rect = guardian.GetAnimationFrameRectangle(guardian.BodyAnimationFrame);
                Microsoft.Xna.Framework.Graphics.Texture2D TailTexture = sprites.GetExtraTexture("tails");
                if (TailTexture == null)
                    return;
                GuardianDrawData dd;
                if (guardian.BodyAnimationFrame == HeavySwingFrames[0])
                {
                    if (!guardian.PlayerMounted)
                    {
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                        guardian.AddDrawData(dd, true);
                        rect.Y += rect.Height * 2;
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                        guardian.AddDrawData(dd, true);
                    }
                    else
                    {
                        rect.Y += rect.Height;
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                        guardian.AddDrawData(dd, true);
                        rect.Y += rect.Height * 2;
                        dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBody, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                        guardian.AddDrawData(dd, true);
                    }
                }
                rect.Y = rect.Height * 4;
                dd = new GuardianDrawData(GuardianDrawData.TextureType.TGBodyFront, TailTexture, DrawPosition, rect, color, Rotation, Origin, Scale, seffect);
                guardian.AddDrawData(dd, true);
            }
        }
    }
}
