using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianSpecialAttack : IDisposable
    {
        public string Name = "", Description = "";
        public SubAttackCombatType AttackType = SubAttackCombatType.Melee;
        public bool CanMove = true;
        public float MinRange = 10, MaxRange = 400;
        public int Cooldown = 0;
        public int ManaCost = 0;

        public virtual GuardianSpecialAttackData GetSpecialAttackData { get { return new GuardianSpecialAttackData(this); } }

        public virtual bool CanUse(TerraGuardian tg)
        {
            return true;
        }

        public virtual void OnUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {

        }

        public virtual void OnEndUse(TerraGuardian tg, GuardianSpecialAttackData data)
        {

        }

        public virtual void Update(TerraGuardian tg, GuardianSpecialAttackData data)
        {

        }

        public virtual void UpdateAnimation(TerraGuardian tg, GuardianSpecialAttackData data, ref bool UsingLeftArm, ref bool UsingRightArm)
        {

        }

        public virtual void ModifyDrawing(TerraGuardian tg, GuardianSpecialAttackData data)
        {

        }

        public void SetCooldown(int s, int m = 0, int h = 0)
        {
            Cooldown = s * 60 + m * 3600 + h * (3600 * 3600);
        }

        public GuardianSpecialAttackData.AffectedTargets[] AreaDamage(TerraGuardian Caster, int Damage, float Knockback, float PositionX, float PositionY, float Width, float Height, byte CriticalRate = 0, bool AffectAllies = false, bool BlockConsecutiveHits = true)
        {
            Rectangle rect = new Rectangle((int)PositionX, (int)PositionY, (int)Width, (int)Height);
            if(rect.Width < 0)
            {
                rect.Width *= -1;
                rect.X -= rect.Width;
            }
            if(rect.Height < 0)
            {
                rect.Height *= -1;
                rect.Y -= rect.Height;
            }
            return AreaDamage(Caster, Damage, Knockback, rect, CriticalRate, AffectAllies, BlockConsecutiveHits);
        }

        public GuardianSpecialAttackData.AffectedTargets[] AreaDamage(TerraGuardian Caster, int Damage, float Knockback, Rectangle Hitbox, byte CriticalRate = 0, bool AffectAllies = false, bool BlockConsecutiveHits = true)
        {
            List<GuardianSpecialAttackData.AffectedTargets> Targets = new List<GuardianSpecialAttackData.AffectedTargets>();
            float StackedDamage = 0;
            float StackedCritical = 0;
            int NewDamage = Damage;
            if (Caster.OwnerPos > -1 && !MainMod.DisableDamageReductionByNumberOfCompanions)
            {
                float DamageMult = Main.player[Caster.OwnerPos].GetModPlayer<PlayerMod>().DamageMod;
                NewDamage = (int)(NewDamage * DamageMult);
            }
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    Player player = Main.player[i];
                    if ((!BlockConsecutiveHits || !Caster.PlayerHasBeenHit(i)) && (AffectAllies || Caster.IsPlayerHostile(player)) && player.getRect().Intersects(Hitbox))
                    {
                        int HitDirection = Caster.Direction;
                        if ((HitDirection == 1 && player.Center.X < Caster.Position.X) ||
                            (HitDirection == -1 && player.Center.X > Caster.Position.X))
                        {
                            HitDirection *= -1;
                        }
                        bool Critical = Main.rand.Next(100) < CriticalRate;
                        double result = player.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(" didn't survived " + Caster.Name + "'s attack."), NewDamage, HitDirection, false, false, Critical);
                        if (result > 0)
                        {
                            GuardianSpecialAttackData.AffectedTargets affected = new GuardianSpecialAttackData.AffectedTargets(player);
                            affected.SetDamage((int)result);
                            Targets.Add(affected);
                            if (BlockConsecutiveHits)
                                Caster.AddPlayerHit(player.whoAmI);
                            Caster.IncreaseDamageStacker((int)result, player.statLifeMax2);
                            StackedDamage += (float)result;
                            if (Critical)
                                StackedCritical += (float)result;
                        }
                    }
                }
                if (i < 200 && Main.npc[i].active)
                {
                    NPC npc = Main.npc[i];
                    if ((!BlockConsecutiveHits || !Caster.NpcHasBeenHit(i)) && Caster.IsNpcHostile(npc) && npc.getRect().Intersects(Hitbox))
                    {
                        int HitDirection = Caster.Direction;
                        if ((HitDirection == 1 && npc.Center.X < Caster.Position.X) ||
                            (HitDirection == -1 && npc.Center.X > Caster.Position.X))
                        {
                            HitDirection *= -1;
                        }
                        bool Critical = Main.rand.Next(100) < CriticalRate;
                        double result = npc.StrikeNPC(NewDamage, Knockback, HitDirection, Critical);
                        if (result > 0)
                        {
                            GuardianSpecialAttackData.AffectedTargets affected = new GuardianSpecialAttackData.AffectedTargets(npc);
                            affected.SetDamage((int)result);
                            Targets.Add(affected);
                            if (BlockConsecutiveHits)
                                Caster.AddNpcHit(npc.whoAmI);
                            Caster.IncreaseDamageStacker((int)result, npc.lifeMax);
                            StackedDamage += (float)result;
                            if (Critical)
                                StackedCritical += (float)result;
                        }
                    }
                }
            }
            foreach (int i in MainMod.ActiveGuardians.Keys)
            {
                if (i != Caster.WhoAmID && (AffectAllies || Caster.IsGuardianHostile(MainMod.ActiveGuardians[i])))
                {
                    TerraGuardian tg = MainMod.ActiveGuardians[i];
                    if (tg.HitBox.Intersects(Hitbox))
                    {
                        int HitDirection = Caster.Direction;
                        if ((HitDirection == 1 && tg.Position.X < Caster.Position.X) ||
                            (HitDirection == -1 && tg.Position.X > Caster.Position.X))
                        {
                            HitDirection *= -1;
                        }
                        bool Critical = Main.rand.Next(100) < CriticalRate;
                        double result = tg.Hurt(NewDamage, HitDirection, Critical, DeathMessage: " didn't survived " + Caster.Name + "'s attack.");
                        if (result > 0)
                        {
                            GuardianSpecialAttackData.AffectedTargets affected = new GuardianSpecialAttackData.AffectedTargets(tg);
                            affected.SetDamage((int)result);
                            Targets.Add(affected);
                            Caster.IncreaseDamageStacker((int)result, tg.MHP);
                            StackedDamage += (float)result;
                            if (Critical)
                                StackedCritical += (float)result;
                        }
                    }
                }
            }
            if (StackedDamage > 0)
            {
                GuardianSkills.SkillTypes AttackSkill = GuardianSkills.SkillTypes.Strength;
                switch (AttackType)
                {
                    case SubAttackCombatType.Ranged:
                        AttackSkill = GuardianSkills.SkillTypes.Ballistic;
                        break;
                    case SubAttackCombatType.Magic:
                        AttackSkill = GuardianSkills.SkillTypes.Mysticism;
                        break;
                }
                Caster.AddSkillProgress(StackedDamage, AttackSkill);
                if (StackedCritical > 0)
                {
                    Caster.AddSkillProgress(StackedDamage, GuardianSkills.SkillTypes.Luck);
                }
            }
            return Targets.ToArray();
        }

        public void Dispose()
        {

        }

        public enum SubAttackCombatType
        {
            Melee,
            Ranged,
            Magic
        }
    }

    public class GuardianSpecialAttackData
    {
        private GuardianSpecialAttack _Special;
        private TerraGuardian _tg;
        private float _LastTime = 0, _Time = 0;
        private ushort _Step = 0;
        private bool _FirstFrame = true, _InUse = false, _JustChangedStep = false;
        private int _Damage = -1;
        public ushort ID = 0;
        public float Time { get { return _Time; } }
        public ushort Step { get { return _Step; } }
        public bool FirstFrame { get { return _FirstFrame; } }
        public bool InUse { get { return _InUse; } }
        private float _UseTime = 1;

        public string Name { get { return _Special.Name; } }
        public string Description { get { return _Special.Description; } }
        public int ManaCost { get { return _Special.ManaCost; } }
        public int Cooldown { get { return _Special.Cooldown; } }
        public bool CanMove { get { return _Special.CanMove; } }

        public GuardianSpecialAttackData(GuardianSpecialAttack special)
        {
            _Special = special;
            _InUse = special != null;
        }

        /// <summary>
        /// Not necessary to assign this. It's only created for convenience, in case you want to store the damage before using special attack.
        /// </summary>
        public int Damage
        {
            get
            {
                return _Damage;
            }
            set { this._Damage = value; }
        }

        /// <summary>
        /// Do not change this when updating
        /// </summary>
        public void ChangeUseTime(float NewTime)
        {
            _UseTime = NewTime;
        }

        public void ForceStopSkillUsage()
        {
            _InUse = false;
        }

        public void Update(TerraGuardian tg)
        {
            _tg = tg;
            _Special.Update(tg, this);
            if (_JustChangedStep)
            {
                _JustChangedStep = false;
            }
            else
            {
                _FirstFrame = false;
                _LastTime = _Time;
                _Time += _UseTime;
            }
        }
        
        public bool PassedTime(int Frame)
        {
            return _LastTime < Frame && (_Time == _LastTime ? _Time + _UseTime : _Time) >= Frame;
        }

        public void UpdateAnimation(TerraGuardian tg, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            _Special.UpdateAnimation(tg, this, ref UsingLeftArm, ref UsingRightArm);
        }

        public void UpdateDrawing(TerraGuardian tg)
        {
            _Special.ModifyDrawing(tg, this);
        }

        public void EndUse()
        {
            _Special.OnEndUse(_tg, this);
            _InUse = false;
            _tg.ChangeSubAttackCooldown(ID, _Special.Cooldown);
        }

        public void ChangeStep(ushort SpecificStep = ushort.MaxValue, int TimeToDiscount = -1)
        {
            if (SpecificStep == ushort.MaxValue)
                _Step++;
            else
                _Step = SpecificStep;
            if (TimeToDiscount > -1)
                _Time -= TimeToDiscount;
            else
                _Time = 0;
            _FirstFrame = true;
            _JustChangedStep = true;
        }

        public struct AffectedTargets
        {
            public int TargetID;
            public TargetTypes Target;
            public int Damage;

            public AffectedTargets(NPC npc)
            {
                Target = TargetTypes.NPC;
                TargetID = npc.whoAmI;
                Damage = 0;
            }

            public AffectedTargets(Player player)
            {
                Target = TargetTypes.Player;
                TargetID = player.whoAmI;
                Damage = 0;
            }

            public AffectedTargets(TerraGuardian tg)
            {
                Target = TargetTypes.TerraGuardian;
                TargetID = tg.WhoAmID;
                Damage = 0;
            }

            public void SetDamage(int Value)
            {
                Damage = Value;
            }

            public enum TargetTypes : byte
            {
                Game,
                Player,
                NPC,
                TerraGuardian
            }
        }
    }
}
