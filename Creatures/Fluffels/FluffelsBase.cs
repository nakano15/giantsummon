using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Creatures
{
    public class FluffelsBase : GuardianBase
    {
        private static Dictionary<int, float> KnockoutAlpha = new Dictionary<int, float>();

        public FluffelsBase()
        {
            Name = "Fluffels";
            PossibleNames = new string[] { "Fluffels", "Krümel" }; //Thank BentoFox for the names.
            Description = "";
            Size = GuardianSize.Large;
            Width = 26;
            Height = 92;
            DuckingHeight = 52;
            SpriteWidth = 96;
            SpriteHeight = 104;
            Age = 31;
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

            PopularityContestsWon = 0;
            ContestSecondPlace = 0;
            ContestThirdPlace = 0;

            AddInitialItem(Terraria.ID.ItemID.BoneSword, 1);
            AddInitialItem(Terraria.ID.ItemID.GoldBow, 1);
            AddInitialItem(Terraria.ID.ItemID.HolyArrow, 250);
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

            LeftHandPoints.AddFramePoint2x(15, 39, 48);

            //Right Arm
            RightHandPoints.AddFramePoint2x(6, 27, 3);
            RightHandPoints.AddFramePoint2x(7, 34, 11);
            RightHandPoints.AddFramePoint2x(8, 38, 20);
            RightHandPoints.AddFramePoint2x(9, 34, 29);

            RightHandPoints.AddFramePoint2x(15, 42, 48);
            //
        }

        public override void Attributes(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            g.AddFlag(GuardianFlags.HideKOBar);
            //g.AddFlag(GuardianFlags.HealthGoesToZeroWhenKod);
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
            if(KnockoutAlpha.ContainsKey(guardian.WhoAmID))
                KoAlpha = KnockoutAlpha[guardian.WhoAmID];
            ColorMod = GetColorMod;
            foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
            {
                if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
                }
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
            {
                if (gdd.textureType != GuardianDrawData.TextureType.MainHandItem && gdd.textureType != GuardianDrawData.TextureType.OffHandItem &&
                    gdd.textureType != GuardianDrawData.TextureType.Effect && gdd.textureType != GuardianDrawData.TextureType.Wings)
                {
                    gdd.color = GhostfyColor(gdd.color, KoAlpha, ColorMod);
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
            if (!guardian.KnockedOut && !guardian.MountedOnPlayer && !guardian.UsingFurniture && guardian.Velocity.Y == 0) //guardian.BodyAnimationFrame != ReviveFrame
                guardian.OffsetY -= ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
            guardian.ReviveBoost += 2;
            if (!KnockoutAlpha.ContainsKey(guardian.WhoAmID))
            {
                KnockoutAlpha.Add(guardian.WhoAmID, (guardian.KnockedOut ? 0 : 1f));
            }
            else
            {
                if (guardian.KnockedOut)
                {
                    if (KnockoutAlpha[guardian.WhoAmID] > 0)
                    {
                        KnockoutAlpha[guardian.WhoAmID] -= 0.005f;
                        if (KnockoutAlpha[guardian.WhoAmID] < 0)
                            KnockoutAlpha[guardian.WhoAmID] = 0;
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
                    }
                }
                else
                {
                    if (KnockoutAlpha[guardian.WhoAmID] < 1)
                    {
                        KnockoutAlpha[guardian.WhoAmID] += 0.005f;
                        if (KnockoutAlpha[guardian.WhoAmID] > 1)
                            KnockoutAlpha[guardian.WhoAmID] = 1;
                    }
                }
            }
            /*if ((!guardian.KnockedOut && guardian.HasFlag(GuardianFlags.CantBeHurt)) || (guardian.KnockedOut && !guardian.HasFlag(GuardianFlags.CantBeHurt)))
            {
                guardian.UpdateStatus = true;
            }*/
        }
    }
}
