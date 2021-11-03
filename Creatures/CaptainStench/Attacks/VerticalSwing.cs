using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures.CaptainStench.Attacks
{
    public class VerticalSwing : GuardianSpecialAttack
    {
        public VerticalSwing()
        {
            Name = "Vertical Swing";
            MinRange = 0; //16
            MaxRange = 99; //52
            CanMove = true;
        }

        public override void OnUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            data.Damage = CaptainStenchBase.GetCalculatedSwordDamage(tg);
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData d)
        {
            if (d.FirstFrame && d.Step == 1)
            {
                CaptainStenchBase.CaptainStenchData data = (CaptainStenchBase.CaptainStenchData)tg.Data;
                Rectangle AttackHitbox = new Rectangle((int)(-16 * tg.Direction * tg.Scale) + (int)tg.Position.X, (int)(-110 * tg.Scale + tg.Position.Y + tg.Base.CharacterPositionYDiscount), (int)(78 * tg.Scale), (int)(94 * tg.Scale));
                if (tg.LookingLeft)
                    AttackHitbox.X -= AttackHitbox.Width;
                for (int i = 0; i < 4; i++)
                {
                    Vector2 Position = new Vector2(AttackHitbox.X, AttackHitbox.Y);
                    switch (i)
                    {
                        case 1:
                            Position.X += AttackHitbox.Width;
                            break;
                        case 2:
                            Position.Y += AttackHitbox.Height;
                            break;
                        case 3:
                            Position.X += AttackHitbox.Width;
                            Position.Y += AttackHitbox.Height;
                            break;
                    }
                    Dust dust = Dust.NewDustDirect(Position, 1, 1, 50);
                    dust.noGravity = true;
                    dust.velocity = Vector2.Zero;
                }
                int Damage = d.Damage;
                int CriticalRate = 4 + tg.MeleeCriticalRate;
                float Knockback = 8f;
                byte SwordID = data.SwordID;
                if (SwordID > 0)
                {
                    switch (SwordID)
                    {
                        case CaptainStenchBase.AmethystFalchion:
                            break;
                        case CaptainStenchBase.TopazFalchion:
                            Knockback += 12f;
                            break;
                        case CaptainStenchBase.SapphireFalchion:
                            //Knockback *= 0.11f;
                            break;
                        case CaptainStenchBase.EmeraldFalchion:
                            CriticalRate += 50;
                            break;
                    }
                }
                bool HitSomething = false;
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
                            HitSomething = true;
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
                                    int HealthRecover = (int)(Math.Max(1, result * 0.05f));
                                    tg.RestoreHP(HealthRecover);
                                }
                                else if (SwordID == CaptainStenchBase.DiamondFalchion)
                                {
                                    if (Main.rand.NextDouble() < 0.5)
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

                if (HitSomething)
                {
                    if (SwordID == CaptainStenchBase.SapphireFalchion)
                    {
                        tg.ChangeSubAttackCooldown(CaptainStenchBase.GPSubAttack, -0.25f);
                        tg.MP += (int)(2 * tg.ManaHealMult);
                        if (tg.MP > tg.MMP)
                            tg.MP = tg.MMP;
                    }
                }
            }
            if (d.Step < 2)
            {
                if (d.Time >= 4)
                    d.ChangeStep();
            }
            else
            {
                if (d.Time >= 8)
                    d.EndUse();
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            UsingRightArm = true;
            tg.RightArmAnimationFrame = 43 + data.Step;
        }
    }
}
