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
        public const string PlasmaFalchionTextureID = "plasmafalchion", PhantomBlinkTextureID = "phantomblink", ScouterTextureID = "scouter", HeadNoScouterTextureID = "headnoscouter",
            RubyGPTextureID = "rubygp", DiamondGPTextureID = "diamondgp";
        public const int NumberOfSwords = 7;
        public const int StandardFalchion = 0, AmethystFalchion = 1, TopazFalchion = 2, SapphireFalchion = 3, EmeraldFalchion = 4, RubyFalchion = 5, DiamondFalchion = 6;

        private const float TopazFalchionAttackSpeedMult = 1.5f, SapphireFalchionAttackSpeedMult = 0.7f;

        public CaptainSmellyBase()
        {
            Name = "CaptainSmelly";
            PossibleNames = new string[] { "Cpt. Smelly" };
            Description = "";
            Size = GuardianSize.Large;
            Width = 22;
            Height = 66;
            CharacterPositionYDiscount = 22;
            //DuckingHeight = 62;
            SpriteWidth = 160;
            SpriteHeight = 140;
            FramesInRows = 12;
            Age = 32;
            SetBirthday(SEASON_SUMMER, 1);
            Male = false;
            InitialMHP = 150; //1100
            LifeCrystalHPBonus = 20;
            LifeFruitHPBonus = 5;
			InitialMMP = 40;
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
            UsesRightHandByDefault = true;
            ForceWeaponUseOnMainHand = true;
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
            RightArmFrontFrameSwap.Add(49, 13);
            RightArmFrontFrameSwap.Add(50, 14);
            RightArmFrontFrameSwap.Add(51, 15);
            RightArmFrontFrameSwap.Add(52, 16);
            RightArmFrontFrameSwap.Add(53, 17);
            RightArmFrontFrameSwap.Add(54, 17);
            RightArmFrontFrameSwap.Add(55, 15);
            RightArmFrontFrameSwap.Add(56, 16);
            //
            RightArmFrontFrameSwap.Add(57, 1);
            RightArmFrontFrameSwap.Add(58, 1);
            RightArmFrontFrameSwap.Add(59, 1);
            //
            RightArmFrontFrameSwap.Add(60, 3);
            RightArmFrontFrameSwap.Add(61, 3);
            RightArmFrontFrameSwap.Add(62, 3);
            //
            RightArmFrontFrameSwap.Add(63, 20);
            RightArmFrontFrameSwap.Add(64, 20);
            RightArmFrontFrameSwap.Add(65, 20);
            RightArmFrontFrameSwap.Add(66, 20);
            RightArmFrontFrameSwap.Add(67, 21);
            RightArmFrontFrameSwap.Add(68, 21);
            RightArmFrontFrameSwap.Add(69, 21);
            RightArmFrontFrameSwap.Add(70, 21);
            RightArmFrontFrameSwap.Add(71, 21);
            RightArmFrontFrameSwap.Add(72, 21);

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
            sprites.AddExtraTexture(RubyGPTextureID, "RubyGP");
            sprites.AddExtraTexture(DiamondGPTextureID, "DiamondGP");
        }

        public override List<GuardianMouseOverAndDialogueInterface.DialogueOption> GetGuardianExtraDialogueActions(TerraGuardian guardian)
        {
            List<GuardianMouseOverAndDialogueInterface.DialogueOption> NewOptions = new List<GuardianMouseOverAndDialogueInterface.DialogueOption>();
            NewOptions.Add(new GuardianMouseOverAndDialogueInterface.DialogueOption("Weapon Infusion", delegate (TerraGuardian tg)
            {
                GuardianMouseOverAndDialogueInterface.Options.Clear();
                GuardianMouseOverAndDialogueInterface.SetDialogue("What should I infuse my weapon with?");
                for(byte i = 0; i < NumberOfSwords; i++)
                {
                    byte InfusionID = i;
                    string Mes = "";
                    switch (i)
                    {
                        case 0:
                            Mes = "Remove Infusion";
                            break;
                        case 1:
                            Mes = "Infuse with Amethyst";
                            break;
                        case 2:
                            Mes = "Infuse with Topaz";
                            break;
                        case 3:
                            Mes = "Infuse with Sapphire";
                            break;
                        case 4:
                            Mes = "Infuse with Emerald";
                            break;
                        case 5:
                            Mes = "Infuse with Ruby";
                            break;
                        case 6:
                            Mes = "Infuse with Diamond";
                            break;
                    }
                    GuardianMouseOverAndDialogueInterface.AddOption(Mes, delegate (TerraGuardian tg2)
                    {
                        CaptainSmellyData data = (CaptainSmellyData)tg2.Data;
                        data.HoldingWeaponTime = 150;
                        data.SwordID = InfusionID;
                        GuardianMouseOverAndDialogueInterface.SetDialogue("Done. What else?");
                        GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                    });
                }
                GuardianMouseOverAndDialogueInterface.AddOption("Nevermind", delegate (TerraGuardian tg2)
                {
                    GuardianMouseOverAndDialogueInterface.SetDialogue("Thanks for wasting my time.");
                    GuardianMouseOverAndDialogueInterface.GetDefaultOptions(tg2);
                });
            }));
            return NewOptions;
        }

        private int GetCalculatedSwordDamage(TerraGuardian tg)
        {
            int Damage = 15;
            if (tg.SelectedItem > -1 && tg.Inventory[tg.SelectedItem].melee)
            {
                Damage += (int)(tg.Inventory[tg.SelectedItem].damage * ((float)tg.Inventory[tg.SelectedItem].useTime / 60));
            }
            CaptainSmellyData data = (CaptainSmellyData)tg.Data;
            if (data.SwordID > 0)
                Damage = (int)(Damage * 1.2f);
            switch (data.SwordID)
            {
                case AmethystFalchion:
                    Damage += (int)(tg.MagicDamageMultiplier * Damage * 0.15f);
                    break;
            }
            /*int StrongestDamage = 0;
            for (int i = 0; i < 10; i++)
            {
                if (tg.Inventory[i].type > 0 && tg.Inventory[i].melee && tg.Inventory[i].damage > 0)
                {
                    int ThisDamage = (int)(tg.Inventory[i].damage * ((float)tg.Inventory[i].useTime / 60));
                    if (ThisDamage > StrongestDamage)
                        StrongestDamage = ThisDamage;
                }
            }*/
            return (int)(Damage * tg.MeleeDamageMultiplier);
        }

        private void SubAttackBegginingScript(TerraGuardian tg)
        {
            CaptainSmellyData data = (CaptainSmellyData)tg.Data;
            switch (data.SwordID)
            {
                case TopazFalchion:
                    tg.SubAttackSpeed = TopazFalchionAttackSpeedMult;
                    break;
                case SapphireFalchion:
                    tg.SubAttackSpeed = SapphireFalchionAttackSpeedMult;
                    break;
            }
        }

        public void SubAttacksSetup()
        {
            VSwingSetup();
            GPSetup();
            ArmBlasterSetup();
        }

        public void VSwingSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Melee); //V Swing
            special.MinRange = 16;
            special.MaxRange = 52;
            special.CanMove = true;
            for (int i = 43; i < 45; i++)
            {
                AddNewSubAttackFrame(4, -1, -1, i);
            }
            AddNewSubAttackFrame(8, -1, -1, 45);
            special.CalculateAttackDamage = delegate (TerraGuardian tg)
            {
                return GetCalculatedSwordDamage(tg);
            };
            special.WhenSubAttackBegins = delegate (TerraGuardian tg)
            {
                SubAttackBegginingScript(tg);
            };
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
                if (Frame == 1)
                {
                    CaptainSmellyData data = (CaptainSmellyData)tg.Data;
                    Rectangle AttackHitbox = new Rectangle(-16 * tg.Direction + (int)tg.Position.X, -110 + (int)tg.Position.Y, 78, 94);
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = tg.SubAttackDamage;
                    int CriticalRate = 4 + tg.MeleeCriticalRate;
                    float Knockback = 6f;
                    byte SwordID = data.SwordID;
                    if (SwordID > 0)
                    {
                        switch (SwordID)
                        {
                            case AmethystFalchion:
                                break;
                            case TopazFalchion:
                                Knockback += 3f;
                                break;
                            case SapphireFalchion:
                                Knockback *= 0.11f;
                                break;
                            case EmeraldFalchion:
                                CriticalRate += 30;
                                break;
                        }
                    }
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(AttackHitbox))
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
                                if (result > 0)
                                {
                                    if (SwordID == AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == RubyFalchion)
                                    {
                                        int HealthRecover = (int)(Math.Max(1, result * 0.05f));
                                        tg.RestoreHP(HealthRecover);
                                    }
                                    else if (SwordID == DiamondFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.2)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.Dazed, 3 * 60);
                                    }
                                }
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (SwordID == AmethystFalchion)
                                        tg.AddSkillProgress((float)result * 0.15f, GuardianSkills.SkillTypes.Mysticism);
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

        public void GPSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Melee); //GP
            //special.SetCooldown(15);
            special.MinRange = 0;
            special.MaxRange = 62;
            special.CalculateAttackDamage = delegate (TerraGuardian tg)
            {
                return (int)(GetCalculatedSwordDamage(tg));
            };
            const int AnimationTime = 6;
            special.CanMove = false;
            for (int i = 46; i < 56; i++)
            {
                AddNewSubAttackFrame(AnimationTime, i, i, i);
            }
            special.WhenSubAttackBegins = delegate (TerraGuardian tg)
            {
                SubAttackBegginingScript(tg);
            };
            special.WhenFrameUpdatesScript = delegate (TerraGuardian tg, int Frame, int Time)
            {
            };
            special.WhenFrameBeginsScript = delegate (TerraGuardian tg, int Frame)
            {
                if (Frame == 5)
                {
                    CaptainSmellyData data = (CaptainSmellyData)tg.Data;
                    Rectangle AttackHitbox = new Rectangle((int)(-32 * tg.Direction * tg.Scale) + (int)tg.Position.X, (int)(-102 * tg.Scale+ tg.Position.Y), (int)(104 * tg.Scale), (int)(98 * tg.Scale));
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = tg.SubAttackDamage;
                    int CriticalRate = 4 + tg.MeleeCriticalRate;
                    float Knockback = 6f;
                    byte SwordID = data.SwordID;
                    if (SwordID > 0)
                    {
                        switch (SwordID)
                        {
                            case AmethystFalchion:
                                {
                                    Vector2 SpawnPos = tg.PositionWithOffset;
                                    SpawnPos.Y -= 39 * tg.Scale; //78
                                    int p = Projectile.NewProjectile(SpawnPos, new Vector2(16f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.AmethystGP>(),
                                        Damage, Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case TopazFalchion:
                                {
                                    Knockback += 3f;
                                    for (int s = 0; s < 4; s++)
                                    {
                                        Vector2 ShardSpawnPosition = tg.PositionWithOffset;
                                        ShardSpawnPosition.X += Main.rand.Next((int)(tg.Width * -0.5f), (int)(tg.Width * 0.5f));
                                        ShardSpawnPosition.Y -= Main.rand.Next(8, tg.Height - 8);
                                        int p = Projectile.NewProjectile(ShardSpawnPosition, new Vector2(4f * tg.Direction, 0), 
                                            Terraria.ModLoader.ModContent.ProjectileType<Projectiles.TopazGP>(), (int)(Damage * 0.25f), Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                        Main.projectile[p].scale = tg.Scale;
                                        Main.projectile[p].netUpdate = true;
                                    }
                                }
                                break;
                            case SapphireFalchion:
                                {
                                    Knockback *= 0.11f;
                                    int p = Projectile.NewProjectile(tg.CenterPosition, new Vector2(8f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.SapphireGP>(),
                                        Damage, Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case EmeraldFalchion:
                                {
                                    CriticalRate += 30;
                                    Vector2 SpawnPosition = tg.PositionWithOffset;
                                    SpawnPosition.Y -= 40 * tg.Scale; //78
                                    int p = Projectile.NewProjectile(SpawnPosition, new Vector2(1f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.EmeraldGP>(),
                                        (int)(Damage * 0.25f), Knockback * 0.9f, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                        }
                    }
                    for (int n = 0; n < 200; n++)
                    {
                        if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(AttackHitbox))
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
                                if (result > 0)
                                {
                                    if (SwordID == AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == RubyFalchion)
                                    {
                                        float HealthRecover = 0.1f;
                                        Rectangle SweetSpotPosition = new Rectangle((int)(tg.Position.X + tg.Direction * (48 + 40) * tg.Scale), (int)(tg.CenterPosition.Y - 40 * tg.Scale), (int)(32 * tg.Scale), (int)(32 * tg.Scale));
                                        if (tg.LookingLeft)
                                            SweetSpotPosition.X -= SweetSpotPosition.Width;
                                        if (Main.npc[n].getRect().Intersects(SweetSpotPosition))
                                        {
                                            HealthRecover = 0.5f;
                                            Main.PlaySound(1, tg.CenterPosition);
                                            for(int i = 0; i < 25; i++)
                                            {
                                                Dust.NewDust(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, Terraria.ID.DustID.Blood);
                                            }
                                        }
                                        if(HealthRecover * result >= 1)
                                            tg.RestoreHP((int)(HealthRecover * result));
                                        else
                                            tg.RestoreHP(1);
                                        tg.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>(), 60);
                                    }
                                    else if (SwordID == DiamondFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.2)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.Dazed, 3 * 60);
                                    }
                                }
                                Main.PlaySound(Main.npc[n].HitSound, Main.npc[n].Center);
                                tg.AddNpcHit(n);
                                tg.IncreaseDamageStacker((int)result, Main.npc[n].lifeMax);
                                if (result > 0)
                                {
                                    if (tg.HasFlag(GuardianFlags.BeetleOffenseEffect))
                                        tg.IncreaseCooldownValue(GuardianCooldownManager.CooldownType.BeetleCounter, (int)result);
                                    tg.OnHitSomething(Main.npc[n]);
                                    tg.AddSkillProgress((float)result, GuardianSkills.SkillTypes.Strength); //(float)result
                                    if (SwordID == AmethystFalchion)
                                        tg.AddSkillProgress((float)result * 0.15f, GuardianSkills.SkillTypes.Mysticism);
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
            special.WhenCompanionIsBeingDrawn = delegate (TerraGuardian tg, int Frame, int Time)
            {
                CaptainSmellyData data = (CaptainSmellyData)tg.Data;
                switch (data.SwordID)
                {
                    case RubyFalchion:
                        {
                            if(Frame >= 4)
                            {
                                int WhipFrame = Frame - 4;
                                Texture2D texture = sprites.GetExtraTexture(RubyGPTextureID);
                                if(WhipFrame >= 0 && WhipFrame < 6)
                                {
                                    Vector2 WhipPos = tg.CenterPosition - Main.screenPosition;
                                    WhipPos.X += 40 * tg.Direction;
                                    GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, WhipPos,
                                        new Rectangle(160 * WhipFrame, 0, 160, 160), Color.White, 0f, new Vector2(80, 80), tg.Scale, 
                                        (tg.LookingLeft ? SpriteEffects.FlipHorizontally: SpriteEffects.None));
                                    TerraGuardian.DrawFront.Add(gdd);
                                }
                                if (tg.HasBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>()))
                                {
                                    int SiphonFrame = Frame - 5;
                                    if(SiphonFrame >= 0 && SiphonFrame < 7)
                                    {
                                        GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.Position - Main.screenPosition,
                                            new Rectangle(160 * SiphonFrame, 160, 160, 160), Color.White, 0f, new Vector2(80, 160), tg.Scale,
                                            (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                        TerraGuardian.DrawFront.Add(gdd);
                                    }
                                }
                            }
                        }
                        break;
                    case DiamondFalchion:
                        {
                            int FlashFrame = (int)(((Frame - 4) * AnimationTime + Time) * (1f / AnimationTime) * 0.5f);
                            if(FlashFrame >= 0 && FlashFrame < 8)
                            {
                                Texture2D texture = sprites.GetExtraTexture(DiamondGPTextureID);
                                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.CenterPosition - Main.screenPosition,
                                    new Rectangle(200 * FlashFrame, 0, 200, 200), Color.White, 0f, new Vector2(100, 100), tg.Scale,
                                    (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                TerraGuardian.DrawFront.Add(gdd);
                                FlashFrame++;
                                gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, tg.CenterPosition - Main.screenPosition,
                                    new Rectangle(200 * FlashFrame, 200, 200, 200), Color.White, 0f, new Vector2(100, 100), tg.Scale,
                                    (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                TerraGuardian.DrawFront.Add(gdd);
                            }
                        }
                        break;
                }
            };
        }

        public void ArmBlasterSetup()
        {
            GuardianSpecialAttack special = AddNewSubAttack(GuardianSpecialAttack.SubAttackCombatType.Ranged); //Arm Blaster
            special.CanMove = true;
            special.ManaCost = 1;
            special.MinRange = 100;
            special.MaxRange = 1000;
            AddNewSubAttackFrame(8, -1, 57, -1);
            special.WhenFrameBeginsScript = delegate (TerraGuardian tg, int FrameID)
            {
                //Shoot something
                Vector2 ProjectilePosition = Vector2.Zero;
                Vector2 AimPosition = tg.AimDirection.ToVector2() - tg.CenterPosition;
                float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
                if (AimPosition.X < 0)
                    Angle = (float)Math.PI - Angle;
                int LeftArmFrame = 57;
                if(Angle < 30 * 0.017453f) //Middle
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 61;
                }
                else if (AimPosition.Y > 0) //Down
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 60;
                    else
                        LeftArmFrame = 58;
                }
                else //Up
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 62;
                    else
                        LeftArmFrame = 59;
                }
                switch (LeftArmFrame)
                {
                    case 57:
                        ProjectilePosition = new Vector2(46, 43);
                        break;
                    case 58:
                        ProjectilePosition = new Vector2(46, 47);
                        break;
                    case 59:
                        ProjectilePosition = new Vector2(48, 34);
                        break;
                    case 60:
                        ProjectilePosition = new Vector2(42, 43);
                        break;
                    case 61:
                        ProjectilePosition = new Vector2(46, 37);
                        break;
                    case 62:
                        ProjectilePosition = new Vector2(45, 30);
                        break;
                }
                ProjectilePosition *= 2;
                ProjectilePosition.X = ProjectilePosition.X - SpriteWidth * 0.5f;
                if (tg.LookingLeft)
                    ProjectilePosition.X *= -1;
                ProjectilePosition.Y = -SpriteHeight + ProjectilePosition.Y;
                ProjectilePosition = tg.PositionWithOffset + ProjectilePosition * tg.Scale;
                AimPosition.Normalize();
                for (int i = 0; i < 4; i++)
                    Dust.NewDust(ProjectilePosition, 2, 2, 132, AimPosition.X, AimPosition.Y);
                int Damage = 5;
                if (tg.SelectedItem > -1 && tg.Inventory[tg.SelectedItem].ranged)
                    Damage += (int)(tg.Inventory[tg.SelectedItem].damage * 0.35f);
                Damage = (int)(Damage * tg.RangedDamageMultiplier);
                int ID = Projectile.NewProjectile(ProjectilePosition, AimPosition * 14f, Terraria.ModLoader.ModContent.ProjectileType<Projectiles.CannonBlast>(),
                    Damage, 0.03f, tg.GetSomeoneToSpawnProjectileFor);
                Main.PlaySound(2, tg.CenterPosition, 20);
                Main.projectile[ID].scale = tg.Scale;
                Main.projectile[ID].netUpdate = true;
                tg.SetProjectileOwnership(ID);
            };
            special.AnimationReplacer = delegate (TerraGuardian tg, int FrameID, int FrameTime, ref int BodyFrame, ref int LeftArmFrame, ref int RightArmFrame)
            {
                Vector2 AimPosition = tg.AimDirection.ToVector2() - tg.CenterPosition;
                float Angle = Math.Abs(MathHelper.WrapAngle((float)Math.Atan2(AimPosition.Y, AimPosition.X)));
                if (AimPosition.X < 0)
                    Angle = (float)Math.PI - Angle;
                LeftArmFrame = 57;
                if (Angle < 30 * 0.017453f) //Middle
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 61;
                }
                else if (AimPosition.Y > 0) //Down
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 60;
                    else
                        LeftArmFrame = 58;
                }
                else //Up
                {
                    if (tg.Velocity.Y != 0)
                        LeftArmFrame = 62;
                    else
                        LeftArmFrame = 59;
                }
            };
        }

        public override int GuardianSubAttackBehaviorAI(TerraGuardian Owner, CombatTactic tactic, Vector2 TargetPosition, Vector2 TargetVelocity, int TargetWidth, int TargetHeight,
            ref bool Approach, ref bool Retreat, ref bool Jump, ref bool Couch, out bool DefaultBehavior)
        {
            int ID = 0;
            float Distance = Math.Abs(TargetPosition.X + TargetWidth * 0.5f - Owner.Position.X) - (TargetWidth + Owner.Width) * 0.5f,
                DistanceYUpper = TargetPosition.Y - Owner.Position.Y,
                DistanceYLower = TargetPosition.Y + TargetHeight - Owner.Position.Y;
            bool InRangeForBlaster = Owner.MP > 1 && Distance > 100;
            DefaultBehavior = false;
            switch (tactic)
            {
                case CombatTactic.Charge:
                    if (Distance > 52)
                    {
                        Approach = true;
                        Retreat = false;
                    }
                    else if(Distance < 16)
                    {
                        Approach = false;
                        Retreat = true;
                    }
                    break;
                case CombatTactic.Assist:
                    if (InRangeForBlaster)
                    {
                        if (Distance > 52)
                        {
                            Retreat = true;
                            Approach = false;
                        }
                    }
                    else
                    {
                        Approach = true;
                        Retreat = false;
                    }
                    break;
                case CombatTactic.Snipe:
                    if(Distance < 280)
                    {
                        Retreat = true;
                        Approach = false;
                    }
                    else if (Distance > 320)
                    {
                        Retreat = false;
                        Approach = true;
                    }
                    break;
            }
            if (Distance < 62 && DistanceYLower >= -98 && DistanceYUpper < 18 && !Owner.SubAttackInCooldown(1))
            {
                ID = 1;
            }
            else if (!(Distance < 52 && DistanceYLower >= -90 && DistanceYUpper < 4 && Owner.TargetInAim))
            {
                ID = 2;
            }
            return ID;
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
            int FrameX = 0, FrameY = 0;
            switch (guardian.RightArmAnimationFrame)
            {
                case 25: //Downed
                case 26: //Reviving
                case 75: //Chair
                case 73: //Throne
                case 74: //Bed
                    return;
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
            FrameY += ((NumberOfSwords - 1) - SwordID) * 2;
            GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, sprites.GetExtraTexture(PlasmaFalchionTextureID), DrawPosition, new Rectangle(FrameX * SpriteWidth, FrameY * SpriteHeight, SpriteWidth, SpriteHeight), color, Rotation, Origin, Scale, seffect);
            if (DrawBehind)
                TerraGuardian.DrawBehind.Insert(Position, gdd);
            else
                TerraGuardian.DrawFront.Insert(Position, gdd);
        }

        public class CaptainSmellyData : GuardianData
        {
            public int HoldingWeaponTime = 0;
            public byte SwordID = 0;

            public CaptainSmellyData(int ID, string ModID) : base(ID, ModID)
            {

            }
        }
    }
}
