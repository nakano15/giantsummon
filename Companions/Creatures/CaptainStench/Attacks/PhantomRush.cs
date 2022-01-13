using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon.Companions.CaptainStench.Attacks
{
    public class PhantomRush : GuardianSpecialAttack
    {
        public PhantomRush()
        {
            Name = "Phantom Rush";
            AttackType = SubAttackCombatType.Melee;
            MinRange = 0;
            MaxRange = 200;
            ManaCost = 20;
            CanMove = false;
            Cooldown = 10 * 60;
        }

        public override void OnUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            data.Damage = (int)(CaptainStenchBase.GetCalculatedSwordDamage(tg) * 1.2f);
        }

        public override void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            tg.Velocity = Vector2.Zero;
            if(data.Step >= 4)
            {
                tg.TrailDelay = 2;
                tg.TrailLength = 10;
                tg.ImmuneTime = 30;
                tg.ImmuneNoBlink = true;
                Vector2 Velocity = Collision.TileCollision(tg.TopLeftPosition, new Vector2(tg.Direction * 20, 0), tg.CollisionWidth, tg.CollisionHeight);
                tg.Position += Velocity;
                tg.Velocity = Vector2.Zero;
                if (Collision.SolidCollision(tg.TopLeftPosition, tg.CollisionWidth, tg.CollisionHeight))
                {
                    if (tg.LookingLeft)
                    {
                        tg.Position.X = (int)(tg.Position.X * 0.0625f) * 16 + 16 + tg.CollisionWidth * 0.5f;
                    }
                    else
                    {
                        tg.Position.X = (int)(tg.Position.X * 0.0625f) * 16 + tg.CollisionWidth - tg.CollisionWidth * 0.5f;
                    }
                }
                Rectangle rect = tg.HitBox;
                int Damage = data.Damage;
                float Knockback = 1.5f;
                int CriticalRate = 65 + tg.MeleeCriticalRate;
                int SwordID = (tg.Data as CaptainStenchBase.CaptainStenchData).SwordID;
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && !Main.npc[n].friendly && !tg.NpcHasBeenHit(n) && Main.npc[n].getRect().Intersects(rect))
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
                                    int HealthRecover = (int)(Math.Max(1, result * 0.05f));
                                    tg.RestoreHP(HealthRecover);
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
                    }
                }
            }
            if (data.Time >= 3)
            {
                if (data.Step < 10)
                {
                    data.ChangeStep();
                }
                else
                {
                    data.EndUse();
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            UsingLeftArm = UsingRightArm = true;
            tg.LeftArmAnimationFrame = tg.RightArmAnimationFrame = tg.BodyAnimationFrame = 63 + data.Step;
        }

        public override void OnEndUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {
            tg.UpdateStatus = true;
        }
    }
}
