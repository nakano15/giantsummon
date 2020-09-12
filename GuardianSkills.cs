using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianSkills
    {
        public SkillTypes skillType = 0;
        public float Progress = 0, MaxProgress = 0;
        public int Level = 0;
        public const int MaxLevel = 1000000000;

        public override string ToString()
        {
            string Text = skillType.ToString();
            Text += " " + Math.Round((MainMod.IndividualSkillLeveling ? Progress * 100 / MaxProgress : Progress * 100), 2) + "%";
            return Text;
        }

        public string SkillName
        {
            get
            {
                string s = "";
                bool First = true;
                foreach (char c in skillType.ToString())
                {
                    if (!First && Char.IsUpper(c))
                        s += " ";
                    s += c;
                    First = false;
                }
                return s;
            }
        }

        public string GetSkillInfo(TerraGuardian guardian)
        {
            string Text = skillType.ToString();
            Text += " Lv: " + Level;
            if (MainMod.IndividualSkillLeveling)
                Text += " (" + Math.Round(Progress * 100 / MaxProgress, 2) + "%)";
            else
                Text += " (" + Math.Round((float)Progress / guardian.Data.LastSkillRateMaxValue * 100, 2) + "%)";
            return Text;
        }

        public StatusValues[] CalculateStatusBonus(TerraGuardian guardian)
        {
            List<StatusValues> FinalStatusCount = new List<StatusValues>();
            float Percentage = (float)Level / 100;
            switch (skillType)
            {
                case SkillTypes.Strength:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MeleeDamageMult, 0.3f * Percentage));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MeleeSpeedMult, 0.2f * Percentage));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MeleeKnockback, 2.5f * Percentage));
                    }
                    break;
                case SkillTypes.Endurance:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MaxHealth, guardian.Base.InitialMHP * 1.5f * Percentage));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.DefenseRate, 5f * Percentage));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.DefenseBonus, 200f * Percentage));
                        if (guardian.BlockRate > 0)
                            FinalStatusCount.Add(new StatusValues(StatusTypes.BlockRate, 150f * Percentage));
                    }
                    break;
                case SkillTypes.Athletic:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MoveSpeed, Percentage * 0.3f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MaxHealth, guardian.Base.InitialMHP * 1.5f * Percentage));
                    }
                    break;
                case SkillTypes.Acrobatic:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.JumpSpeed, Percentage * 0.033f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.FallHeightTolerance, guardian.FallHeightTolerance * Percentage * 0.3f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.DodgeRate, Percentage * 150));
                    }
                    break;
                case SkillTypes.Ballistic:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.RangedDamageMult, Percentage * 0.3f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.RangedBulletSpeed, Percentage * 0.2f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.Accuracy, Percentage * 2.5f));
                    }
                    break;
                case SkillTypes.Mysticism:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MagicDamageMult, Percentage * 0.3f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MagicSpeed, Percentage * 0.2f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.Accuracy, Percentage * 2.5f));
                    }
                    break;
                case SkillTypes.Leadership:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.SummonDamageMult, Percentage * 0.3f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MaxSummons, (int)(Percentage * 5)));
                    }
                    break;
                case SkillTypes.Luck:
                    {
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MeleeCritRate, Percentage * 15f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.RangedCritRate, Percentage * 15f));
                        FinalStatusCount.Add(new StatusValues(StatusTypes.MagicCritRate, Percentage * 15f));
                    }
                    break;
            }
            return FinalStatusCount.ToArray();
        }

        public void OnStatusUpdate(TerraGuardian guardian)
        {
            /*StatusValues[] Values = CalculateStatusBonus(guardian);
            foreach (StatusValues value in Values)
            {
                switch (value.status)
                {
                    case StatusTypes.MeleeDamageMult:
                        guardian.MeleeDamageMultiplier += value.StatusValue;
                        break;
                    case StatusTypes.MeleeSpeedMult:
                        guardian.MeleeSpeed += value.StatusValue;
                        break;
                    case StatusTypes.MeleeKnockback:
                        guardian.MeleeKnockback += value.StatusValue;
                        break;
                    case StatusTypes.MaxHealth:
                        guardian.MHP += (int)value.StatusValue;
                        break;
                    case StatusTypes.DefenseRate:
                        guardian.DefenseRate += value.StatusValue;
                        break;
                    case StatusTypes.DefenseBonus:
                        guardian.Defense += (int)value.StatusValue;
                        break;
                    case StatusTypes.BlockRate:
                        guardian.BlockRate += value.StatusValue;
                        break;
                    case StatusTypes.MoveSpeed:
                        guardian.MoveSpeed += value.StatusValue;
                        break;
                    case StatusTypes.JumpSpeed:
                        guardian.JumpSpeed += value.StatusValue;
                        break;
                    case StatusTypes.FallHeightTolerance:
                        guardian.FallHeightTolerance += (int)value.StatusValue;
                        break;
                    case StatusTypes.DodgeRate:
                        guardian.DodgeRate += value.StatusValue;
                        break;
                    case StatusTypes.RangedDamageMult:
                        guardian.RangedDamageMultiplier += value.StatusValue;
                        break;
                    case StatusTypes.RangedBulletSpeed:
                        guardian.RangedSpeed += value.StatusValue;
                        break;
                    case StatusTypes.Accuracy:
                        guardian.Accuracy += value.StatusValue;
                        break;
                    case StatusTypes.MagicDamageMult:
                        guardian.MagicDamageMultiplier += value.StatusValue;
                        break;
                    case StatusTypes.MagicSpeed:
                        guardian.MagicSpeed += value.StatusValue;
                        break;
                    case StatusTypes.SummonDamageMult:
                        guardian.SummonDamageMultiplier += value.StatusValue;
                        break;
                    case StatusTypes.MaxSummons:
                        guardian.MaxMinions += (int)value.StatusValue;
                        break;
                    case StatusTypes.MeleeCritRate:
                        guardian.MeleeCriticalRate += (int)value.StatusValue;
                        break;
                    case StatusTypes.RangedCritRate:
                        guardian.RangedCriticalRate += (int)value.StatusValue;
                        break;
                    case StatusTypes.MagicCritRate:
                        guardian.MagicCriticalRate += (int)value.StatusValue;
                        break;
                }
            }*/
            float LevelValue = (float)Math.Log(Level + 1);
            switch (skillType)
            {
                case SkillTypes.Strength:
                    guardian.MeleeDamageMultiplier += LevelValue * 0.03f;
                    guardian.MeleeSpeed += LevelValue * 0.02f;
                    guardian.MeleeKnockback += LevelValue * 0.25f;
                    break;
                case SkillTypes.Endurance:
                    guardian.MHP += (int)(guardian.Base.InitialMHP * (LevelValue * 0.05f));
                    guardian.DefenseRate += (int)Math.Floor(LevelValue * 0.05f);
                    guardian.Defense += (int)(LevelValue * 2f);
                    if (guardian.BlockRate > 0) guardian.BlockRate += LevelValue * 1.5f;
                    break;
                case SkillTypes.Athletic:
                    guardian.MoveSpeed += LevelValue * 0.01f;
                    guardian.MHP += (int)(guardian.Base.InitialMHP * (LevelValue * 0.025f));
                    break;
                case SkillTypes.Acrobatic:
                    guardian.JumpSpeed += LevelValue * 0.033f;
                    guardian.FallHeightTolerance += (int)(guardian.FallHeightTolerance * LevelValue * 0.3f);
                    guardian.DodgeRate += LevelValue * 1.5f;
                    break;
                case SkillTypes.Ballistic:
                    guardian.RangedDamageMultiplier += LevelValue * 0.03f;
                    guardian.RangedSpeed += LevelValue * 0.02f;
                    guardian.Accuracy += LevelValue * 0.025f;
                    break;
                case SkillTypes.Mysticism:
                    guardian.MagicDamageMultiplier += LevelValue * 0.03f;
                    guardian.MagicSpeed += LevelValue * 0.02f;
                    guardian.Accuracy += LevelValue * 0.025f;
                    break;
                case SkillTypes.Leadership:
                    guardian.SummonDamageMultiplier += LevelValue * 0.03f;
                    int SummonBonus = (int)(Level * 2);
                    if (SummonBonus > 5) SummonBonus = 5;
                    guardian.MaxMinions += SummonBonus;
                    break;
                case SkillTypes.Luck:
                    guardian.MeleeCriticalRate += (int)(LevelValue * 0.25f);
                    guardian.RangedCriticalRate += (int)(LevelValue * 0.25f);
                    guardian.MagicCriticalRate += (int)(LevelValue * 0.25f);
                    break;
            }
        }

        public string GetSkillDescription
        {
            get
            {
                string Desc = "";
                switch (skillType)
                {
                    case SkillTypes.Strength:
                        Desc = "Increases Damage, Attack Speed and Knockback of the Melee attacks.";
                        break;
                    case SkillTypes.Endurance:
                        Desc = "Increases Max Health, Defense, Block Rate and Defense Rate.";
                        break;
                    case SkillTypes.Athletic:
                        Desc = "Increases Movement Speed and Max Health.";
                        break;
                    case SkillTypes.Acrobatic:
                        Desc = "Increases Jump Speed, Fall Distance Tolerance and Dodge Rate.";
                        break;
                    case SkillTypes.Ballistic:
                        Desc = "Increases Damage and Attack Speed of Ranged attacks, and increases Accuracy.";
                        break;
                    case SkillTypes.Mysticism:
                        Desc = "Increases Damage and Attack Speed of Magic attacks, and increases Accuracy.";
                        break;
                    case SkillTypes.Leadership:
                        Desc = "Increases the Damage of Summon attacks, and allows control of more Summons.";
                        break;
                    case SkillTypes.Luck:
                        Desc = "Not only increases general critical rate, but also may cause good things to happen.";
                        break;
                    default:
                        Desc = "Is a mystery what it does right now, maybe tell Nakano to add this skill info?";
                        break;
                }
                return Desc;
            }
        }

        public string[] GetStatusBoost(StatusValues[] Values)
        {
            List<string> StatusText = new List<string>();
            foreach (StatusValues sv in Values)
            {
                string Text = "";
                switch (sv.status)
                {
                    case StatusTypes.MeleeDamageMult:
                        Text = "Melee Damage Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MeleeSpeedMult:
                        Text = "Melee Speed Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MeleeKnockback:
                        Text = "Melee Knockback Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MaxHealth:
                        Text = "Max Health Bonus [" + sv.StatusValue + "]";
                        break;
                    case StatusTypes.DefenseRate:
                        Text = "Defense Rate Bonus [" + sv.StatusValue + "%]";
                        break;
                    case StatusTypes.DefenseBonus:
                        Text = "Defense Bonus [" + sv.StatusValue + "]";
                        break;
                    case StatusTypes.BlockRate:
                        Text = "Block Rate [" + sv.StatusValue + "%]";
                        break;
                    case StatusTypes.MoveSpeed:
                        Text = "Movement Speed Bonus [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.JumpSpeed:
                        Text = "Jump Speed Bonus [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.FallHeightTolerance:
                        Text = "Fall Rate Tolerance [" + sv.StatusValue + " tiles]";
                        break;
                    case StatusTypes.DodgeRate:
                        Text = "Dodge Rate [" + sv.StatusValue + "%]";
                        break;
                    case StatusTypes.RangedDamageMult:
                        Text = "Ranged Damage Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.RangedBulletSpeed:
                        Text = "Bullet Speed Bonus [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.Accuracy:
                        Text = "Accuracy Bonus [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MagicDamageMult:
                        Text = "Magic Damage Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MagicSpeed:
                        Text = "Magic Speed Bonus [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.SummonDamageMult:
                        Text = "Summon Damage Multiplier [" + sv.StatusValue + "x]";
                        break;
                    case StatusTypes.MaxSummons:
                        Text = "Max Summon Bonus [" + sv.StatusValue + "]";
                        break;
                    case StatusTypes.MeleeCritRate:
                        Text = "Melee Critical Rate [" + sv.StatusValue + "%]";
                        break;
                    case StatusTypes.RangedCritRate:
                        Text = "Ranged Critical Rate [" + sv.StatusValue + "%]";
                        break;
                    case StatusTypes.MagicCritRate:
                        Text = "Magic Critical Rate [" + sv.StatusValue + "%]";
                        break;
                }
                if (Text != "")
                    StatusText.Add(Text);
            }
            return StatusText.ToArray();
        }

        public static float ReturnSkillMaxProgress(int StatusSum, bool IndividualSkill = false)
        {
            if (false && IndividualSkill)
            {
                float Value = 2000 + StatusSum * 250; //20 + x * 25
                if (StatusSum >= 10)
                {
                    Value += Value * 0.1f;
                }
                if (StatusSum >= 20)
                {
                    Value += Value * 0.1f;
                }
                if (StatusSum >= 40)
                {
                    Value += Value * 0.25f;
                }
                if (StatusSum >= 60)
                {
                    Value += Value * 0.4f;
                }
                if (StatusSum >= 80)
                {
                    Value += Value * 0.6f;
                }
                return Value;
            }
            else
            {
                float Value = 0;
                if (IndividualSkill)
                    Value = 20 + StatusSum * 25 - (StatusSum - 1) * 12;
                else
                    Value = 20 + StatusSum * 15 - (StatusSum - 1) * 8;
                const int MaxPossibleValue = 500000000;
                if (Value > MaxPossibleValue || Value < 0) //To avoid overflow
                    Value = MaxPossibleValue;
                return Value;
            }
        }

        public class StatusValues
        {
            public StatusTypes status = StatusTypes.MeleeDamageMult;
            public float StatusValue = 0;

            public StatusValues(StatusTypes status, float Value)
            {
                this.status = status;
                this.StatusValue = Value;
            }
        }

        public enum StatusTypes
        {
            MeleeDamageMult,
            MeleeSpeedMult,
            MeleeKnockback,
            MaxHealth,
            DefenseRate,
            DefenseBonus,
            BlockRate,
            MoveSpeed,
            JumpSpeed,
            FallHeightTolerance,
            DodgeRate,
            RangedDamageMult,
            RangedBulletSpeed,
            Accuracy,
            MagicDamageMult,
            MagicSpeed,
            SummonDamageMult,
            MaxSummons,
            MeleeCritRate,
            RangedCritRate,
            MagicCritRate
        }

        public enum SkillTypes : byte
        {
            Endurance,
            Athletic,
            Acrobatic,
            Ballistic,
            Strength,
            Mysticism,
            Leadership,
            Luck
        }
    }
}
