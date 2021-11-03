using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Creatures.CaptainStench.Attacks
{
    public class GPAttack : GuardianSpecialAttack
    {
        public GPAttack()
        {
            Name = "Gem Power Attack";
            SetCooldown(15);
            MinRange = 0;
            MaxRange = 62;
            CanMove = false;
        }

        public const int AnimationTime = 6;

        public override void OnUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            data.Damage = CaptainStenchBase.GetCalculatedSwordDamage(tg);
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData d)
        {
            if (d.FirstFrame)
            {
                if (d.Step == 5)
                {
                    CaptainStenchBase.CaptainStenchData data =
                        (CaptainStenchBase.CaptainStenchData)tg.Data;
                    Rectangle AttackHitbox = new Rectangle((int)(-32 * tg.Direction * tg.Scale) + (int)tg.Position.X, (int)(-102 * tg.Scale + tg.Position.Y), (int)(104 * tg.Scale), (int)(98 * tg.Scale));
                    if (tg.LookingLeft)
                        AttackHitbox.X -= AttackHitbox.Width;
                    int Damage = d.Damage;
                    int CriticalRate = 4 + tg.MeleeCriticalRate;
                    float Knockback = 8f;
                    byte SwordID = data.SwordID;
                    Main.PlaySound(2, tg.CenterPosition, 1);
                    if (SwordID > 0)
                    {
                        switch (SwordID)
                        {
                            case CaptainStenchBase.AmethystFalchion:
                                {
                                    Vector2 SpawnPos = tg.PositionWithOffset;
                                    float Scale = (float)tg.Height / 74 * tg.Scale * 1.5f;
                                    SpawnPos.Y -= 39 * Scale; //78
                                    int p = Projectile.NewProjectile(SpawnPos, new Vector2(16f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.AmethystGP>(),
                                        (int)(Damage * 1.333f), Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.PlaySound(2, tg.CenterPosition, 101);
                                    Main.projectile[p].scale = Scale;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case CaptainStenchBase.TopazFalchion:
                                {
                                    Knockback += 12f;
                                    for (int s = 0; s < 4; s++)
                                    {
                                        Vector2 ShardSpawnPosition = tg.PositionWithOffset;
                                        ShardSpawnPosition.X += Main.rand.Next((int)(tg.Width * -0.5f), (int)(tg.Width * 0.5f));
                                        ShardSpawnPosition.Y -= Main.rand.Next(8, tg.Height - 8);
                                        int p = Projectile.NewProjectile(ShardSpawnPosition, new Vector2(4f * tg.Direction, 0),
                                            Terraria.ModLoader.ModContent.ProjectileType<Projectiles.TopazGP>(), (int)(Damage * 0.25f), Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                        Main.projectile[p].scale = tg.Scale;
                                        Main.projectile[p].netUpdate = true;
                                        Main.PlaySound(2, tg.CenterPosition, 101);
                                    }
                                }
                                break;
                            case CaptainStenchBase.SapphireFalchion:
                                {
                                    //Knockback *= 0.11f;
                                    int p = Projectile.NewProjectile(tg.CenterPosition, new Vector2(8f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.SapphireGP>(),
                                        Damage, Knockback, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale;
                                    Main.projectile[p].netUpdate = true;
                                    Main.PlaySound(2, tg.CenterPosition, 39);
                                }
                                break;
                            case CaptainStenchBase.EmeraldFalchion:
                                {
                                    CriticalRate += 50;
                                    Vector2 SpawnPosition = tg.PositionWithOffset;
                                    SpawnPosition.Y -= 40 * tg.Scale * 1.5f; //78
                                    int p = Projectile.NewProjectile(SpawnPosition, new Vector2(1f * tg.Direction, 0), Terraria.ModLoader.ModContent.ProjectileType<Projectiles.EmeraldGP>(),
                                        (int)(Damage * 0.75f), Knockback * 0.9f, tg.GetSomeoneToSpawnProjectileFor);
                                    Main.projectile[p].scale = tg.Scale * 1.5f;
                                    Main.projectile[p].netUpdate = true;
                                }
                                break;
                            case CaptainStenchBase.DiamondFalchion:
                                {
                                    Damage += (int)(tg.MHP * 0.05f);
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
                                if ((HitDirection == -1 && tg.Position.X < Main.npc[n].Center.X) ||
                                    (HitDirection == 1 && tg.Position.X > Main.npc[n].Center.X))
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
                                    if (SwordID == CaptainStenchBase.AmethystFalchion)
                                    {
                                        if (Main.rand.NextDouble() < 0.4)
                                            Main.npc[n].AddBuff(Terraria.ID.BuffID.ShadowFlame, 10 * 60);
                                    }
                                    else if (SwordID == CaptainStenchBase.RubyFalchion)
                                    {
                                        float HealthRecover = 0.1f;
                                        Rectangle SweetSpotPosition = new Rectangle((int)(tg.Position.X + tg.Direction * (48 + 40) * tg.Scale), (int)(tg.CenterY - 40 * tg.Scale), (int)(32 * tg.Scale), (int)(32 * tg.Scale));
                                        if (tg.LookingLeft)
                                            SweetSpotPosition.X -= SweetSpotPosition.Width;
                                        if (Main.npc[n].getRect().Intersects(SweetSpotPosition))
                                        {
                                            HealthRecover = 0.5f;
                                            Main.PlaySound(1, tg.CenterPosition);
                                            for (int i = 0; i < 25; i++)
                                            {
                                                Dust.NewDust(Main.npc[n].position, Main.npc[n].width, Main.npc[n].height, Terraria.ID.DustID.Blood);
                                            }
                                        }
                                        if (HealthRecover * result >= 1)
                                            tg.RestoreHP((int)(HealthRecover * result));
                                        else
                                            tg.RestoreHP(1);
                                        tg.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>(), 60);
                                    }
                                    else if (SwordID == CaptainStenchBase.DiamondFalchion)
                                    {
                                        Main.npc[n].AddBuff(Terraria.ID.BuffID.Confused, 3 * 60);
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
                                    if (SwordID == CaptainStenchBase.AmethystFalchion)
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
            }
            if(d.Step < 10)
            {
                if (d.Time >= AnimationTime)
                    d.ChangeStep();
            }
            else
            {
                d.EndUse();
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            UsingLeftArm = UsingRightArm = true;
            tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = tg.BodyAnimationFrame = 46 + data.Step;
        }

        public override void ModifyDrawing(TerraGuardian tg, GuardianSpecialAttackData d)
        {
            CaptainStenchBase.CaptainStenchData data = 
                (CaptainStenchBase.CaptainStenchData)tg.Data;
            switch (data.SwordID)
            {
                case CaptainStenchBase.RubyFalchion:
                    {
                        if (d.Step >= 4)
                        {
                            int WhipFrame = d.Step - 4;
                            Texture2D texture = tg.GetExtraTexture(CaptainStenchBase.RubyGPTextureID);
                            if (WhipFrame >= 0 && WhipFrame < 6)
                            {
                                Vector2 WhipPos = tg.CenterPosition - Main.screenPosition;
                                WhipPos.X += 40 * tg.Direction;
                                GuardianDrawData gdd = new GuardianDrawData(GuardianDrawData.TextureType.TGExtra, texture, WhipPos,
                                    new Rectangle(160 * WhipFrame, 0, 160, 160), Color.White, 0f, new Vector2(80, 80), tg.Scale,
                                    (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None));
                                TerraGuardian.DrawFront.Add(gdd);
                            }
                            if (tg.HasBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.DrainingHealth>()))
                            {
                                int SiphonFrame = d.Step - 5;
                                if (SiphonFrame >= 0 && SiphonFrame < 7)
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
                case CaptainStenchBase.DiamondFalchion:
                    {
                        int FlashFrame = (int)(((d.Step - 4) * AnimationTime + d.Time) * (1f / AnimationTime) * 0.5f);
                        if (FlashFrame >= 0 && FlashFrame < 8)
                        {
                            Texture2D texture = tg.GetExtraTexture(CaptainStenchBase.DiamondGPTextureID);
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
        }
    }
}
