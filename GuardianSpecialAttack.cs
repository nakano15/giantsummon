using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
