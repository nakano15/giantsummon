using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace giantsummon.Creatures
{
    public class BaphaBase : GuardianBase
    {
        private const int DownedFrameID = 22, RevivingFrameID = 29;
        private const string HellGateSpriteID = "hellgate";
        private const int HellGateAnimationFrameTime = 8;
        private const byte CrimsonFlameID = 0;

        public BaphaBase()
        {
            Name = "Bapha";
            Description = "";
            Size = GuardianSize.Large;
            Width = 22;
            Height = 90;
            SpriteWidth = 112;
            SpriteHeight = 128;
            FramesInRows = 17;
            Age = 999;
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
            SpecialAttackBasedCombat = true;
            SetTerraGuardian();
            HurtSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldHurt);
            DeadSound = new SoundData(Terraria.ID.SoundID.DD2_KoboldDeath);

            StandingFrame = 0;
            WalkingFrames = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            JumpFrame = 9;
            ItemUseFrames = new int[] { 0, 21, 20, 19 };
            //SittingFrame = 23;
            //ThroneSittingFrame = 24;
            //BedSleepingFrame = 25;
            //SleepingOffset.X = 16;
            ReviveFrame = RevivingFrameID;
            DownedFrame = DownedFrameID;

            //Left Arm
            LeftHandPoints.AddFramePoint(10, 62, 53);
            LeftHandPoints.AddFramePoint(11, 52, 47);
            LeftHandPoints.AddFramePoint(12, 52, 47);
            LeftHandPoints.AddFramePoint(13, 47, 42);
            LeftHandPoints.AddFramePoint(14, 35, 29);
            LeftHandPoints.AddFramePoint(15, 60, 25);
            LeftHandPoints.AddFramePoint(16, 74, 36);
            LeftHandPoints.AddFramePoint(17, 74, 60);
            LeftHandPoints.AddFramePoint(18, 74, 67);

            LeftHandPoints.AddFramePoint(19, 112, 128);
            LeftHandPoints.AddFramePoint(20, 83, 55);
            LeftHandPoints.AddFramePoint(21, 75, 28);

            LeftHandPoints.AddFramePoint(29, 84, 113);
            LeftHandPoints.AddFramePoint(30, 84, 113);
            LeftHandPoints.AddFramePoint(31, 84, 113);

            //Right Arm
            RightHandPoints.AddFramePoint(29, 86, 113);
            RightHandPoints.AddFramePoint(30, 86, 113);
            RightHandPoints.AddFramePoint(31, 86, 113);

            SubAttackSetup();
        }

        public void SubAttackSetup()
        {
            //Anim duration: 10~18
            //15 = 25º
            //16 = 65º
            //17 = 120º
            //18 = 180º
            GuardianSpecialAttack specialAttack = AddNewSubAttack();
            specialAttack.CanMove = false;
            specialAttack.WhenFrameBeginsScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
                if(Frame == 0)
                {
                    tg.LookingLeft = tg.Position.X > tg.AimDirection.X;
                }
                if(Frame == 5) //= frame 15
                {
                    Vector2 ProjectileSpawnPosition = tg.GetGuardianLeftHandPosition;
                    Vector2 ShotDirection = new Vector2(tg.AimDirection.X, tg.AimDirection.Y) - ProjectileSpawnPosition;
                    ShotDirection.Normalize();
                    ShotDirection *= 8f;
                    int Damage = 20;
                    if (tg.SelectedItem > -1)
                        Damage += (int)(tg.Inventory[tg.SelectedItem].damage * 0.75f);
                    Damage = (int)(Damage * tg.MagicDamageMultiplier);
                    int resultproj = Projectile.NewProjectile(ProjectileSpawnPosition, ShotDirection, 
                        Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CrimsonFlameProjectile>(),
                        Damage, 1.2f, (tg.OwnerPos > -1 ? tg.OwnerPos : Main.myPlayer));
                    tg.SetProjectileOwnership(resultproj);
                    Main.projectile[resultproj].scale = tg.Scale;
                }
            };
            for(int i = 10; i < 19; i++)
            {
                AddNewSubAttackFrame(6, i, i, i);
            }
        }

        public override void Attributes(TerraGuardian g)
        {
            g.ScaleMult *= 1.5f;
            g.AddFlag(GuardianFlags.CantBeKnockedOutCold);
            g.AddFlag(GuardianFlags.CantReceiveHelpOnReviving);
            g.AddFlag(GuardianFlags.HideKOBar);
            g.AddFlag(GuardianFlags.NotPulledWhenKOd);
            if (g.KnockedOut)
            {
                g.AddFlag(GuardianFlags.DontTakeAggro);
                g.AddFlag(GuardianFlags.CantBeHurt);
            }
        }

        public override void ManageExtraDrawScript(GuardianSprites sprites)
        {
            sprites.AddExtraTexture(HellGateSpriteID, "hell_gate");
        }

        public override void GuardianAnimationOverride(TerraGuardian guardian, byte BodyPartID, ref int Frame)
        {
            switch (Frame)
            {
                case DownedFrameID:

                    break;
                case RevivingFrameID:
                    {
                        const float AnimationDuration = 12;
                        if (guardian.AnimationTime >= AnimationDuration * 4)
                            guardian.AnimationTime -= AnimationDuration * 4;
                        byte PickedFrame = (byte)(guardian.AnimationTime / AnimationDuration);
                        if (PickedFrame == 3)
                            PickedFrame = 1;
                        Frame = PickedFrame + RevivingFrameID;
                    }
                    break;
            }
        }

        public override void GuardianPostDrawScript(TerraGuardian guardian, Vector2 DrawPosition, Color color, Color armorColor, float Rotation, Vector2 Origin, float Scale, SpriteEffects seffect)
        {
            if(guardian.BodyAnimationFrame >= 22 && guardian.BodyAnimationFrame <= 28)
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
                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(HellGateSpriteID), DrawPosition, 
                        new Rectangle(SpriteWidth * Frame, 0, SpriteWidth, SpriteHeight), Color.White, Rotation, Origin, Scale, SpriteEffects.None);
                    TerraGuardian.DrawFront.Add(gdd);
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
            Mes.Add("*I need you to collect me some tributes, would you?*");
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
    }
}
