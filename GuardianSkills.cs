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

        public void OnStatusUpdate(TerraGuardian guardian)
        {
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
                    guardian.MHP += (int)(guardian.Base.InitialMHP * (LevelValue * 0.05f));
                    guardian.SummonDamageMultiplier += LevelValue * 0.03f;
                    //int SummonBonus = (int)(Level * 2);
                    //if (SummonBonus > 5) SummonBonus = 5;
                    //guardian.MaxMinions += SummonBonus;
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
                        Desc = "Increases the Damage of Summon attacks and Max Health.";
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

        public static float ReturnSkillMaxProgress(int StatusSum, bool IndividualSkill = false)
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
