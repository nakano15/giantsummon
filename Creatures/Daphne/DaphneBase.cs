using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures
{
    public class DaphneBase : GuardianBase
    {
        public const string HaloTextureID = "halotexture";

        public DaphneBase()
        {
            Name = "Daphne";
            Description = "";
            Size = GuardianSize.Large;
            Width = 35 * 2;
            Height = 33 * 2;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 96;
            Age = 53;
            Male = false;
            InitialMHP = 175; //1125
            LifeCrystalHPBonus = 30;
            LifeFruitHPBonus = 25;
            Accuracy = 0.36f;
            Mass = 0.7f;
            MaxSpeed = 5.65f;
            Acceleration = 0.33f;
            SlowDown = 0.83f;
            MaxJumpHeight = 15;
            JumpSpeed = 6.71f;
            CanDuck = false;
            ReverseMount = false;
            DrinksBeverage = false;
            DontUseRightHand = true;
            SetTerraGuardian();
            GroupID = GiantDogGuardianGroupID;
            //HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            //DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);
            CallUnlockLevel = 0;

            //Animation Frames
            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            //HeavySwingFrames = new int[] { 10, 11, 12 };
            ItemUseFrames = new int[] { 10, 11, 12, 13 };
            //DuckingFrame = 20;
            //DuckingSwingFrames = new int[] { 21, 22, 12 };
            SittingFrame = 15;
            ChairSittingFrame = 14;
            SittingItemUseFrames = new int[] { 16, 17, 18 };
            //DrawLeftArmInFrontOfHead.AddRange(new int[] { 10, 11, 15 });
            ThroneSittingFrame = 14;
            BedSleepingFrame = 19;
            SleepingOffset.X = 16;
            //ReviveFrame = 20;
            DownedFrame = 20;

            SpecificBodyFrontFramePositions = true;
            BodyFrontFrameSwap.Add(15, 0);
            BodyFrontFrameSwap.Add(16, 0);
            BodyFrontFrameSwap.Add(17, 0);
            BodyFrontFrameSwap.Add(18, 0);

            SittingPoint2x = new Microsoft.Xna.Framework.Point(20, 40);

            MountShoulderPoints.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(28, 24);

            //Left Arm
            LeftHandPoints.AddFramePoint2x(10, 39, 31);
            LeftHandPoints.AddFramePoint2x(11, 42, 32);
            LeftHandPoints.AddFramePoint2x(12, 43, 38);
            LeftHandPoints.AddFramePoint2x(13, 41, 42);

            LeftHandPoints.AddFramePoint2x(15, 25, 33);
            LeftHandPoints.AddFramePoint2x(16, 30, 18);
            LeftHandPoints.AddFramePoint2x(17, 32, 25);
            LeftHandPoints.AddFramePoint2x(18, 29, 30);

            //Right Arm

            //Head Equipment Position
            HeadVanityPosition.DefaultCoordinate2x = new Microsoft.Xna.Framework.Point(38, 21);
            HeadVanityPosition.AddFramePoint2x(15, 25, 16);
            HeadVanityPosition.AddFramePoint2x(16, 25, 16);
            HeadVanityPosition.AddFramePoint2x(17, 25, 16);
            HeadVanityPosition.AddFramePoint2x(18, 25, 16);

            HeadVanityPosition.AddFramePoint2x(19, 40, 39);
            HeadVanityPosition.AddFramePoint2x(20, 40, 39);
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HaloTextureID, "halo");
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            Vector2 HaloDrawPosition = Vector2.Zero;
            switch (guardian.BodyAnimationFrame)
            {
                default:
                    HaloDrawPosition = new Vector2(37, 15);
                    break;
                case 15:
                case 16:
                case 17:
                case 18:
                    HaloDrawPosition = new Vector2(24, 10);
                    break;
                case 19:
                case 20:
                    HaloDrawPosition = new Vector2(39, 35);
                    break;
            }
            HaloDrawPosition.Y -= 7;
            HaloDrawPosition.X -= guardian.Base.SpriteWidth * 0.25f - 1;
            if (guardian.LookingLeft)
                HaloDrawPosition.X *= -1;
            HaloDrawPosition.Y -= guardian.Base.SpriteHeight * 0.5f;
            HaloDrawPosition = HaloDrawPosition * 2 * guardian.Scale + DrawPosition;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HaloTextureID), HaloDrawPosition, new Rectangle(0, 0, 26, 12), Color.White, Rotation, new Vector2(13, 6), Scale, SpriteEffects.None);
            TerraGuardian.DrawFront.Add(gdd);
            if (MainMod.NemesisFadeEffect > 0)
            {
                float TransparencyValue = (float)MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.5f);
                if(TransparencyValue > 1f)
                {
                    TransparencyValue = 1f - TransparencyValue + 1f;
                }
                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HaloTextureID), HaloDrawPosition, new Rectangle(26, 0, 26, 12), Color.White * TransparencyValue, Rotation, new Vector2(13, 6), Scale, SpriteEffects.None);
                TerraGuardian.DrawFront.Add(gdd);
            }
        }
    }
}
