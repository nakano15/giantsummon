using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Terraria;
using NExperience;
using Microsoft.Xna.Framework;

namespace giantsummon.Compatibility
{
    public class NExperienceCompatibility
    {
        public static Dictionary<GuardianIdentity, int> SetGuardianLevel = new Dictionary<GuardianIdentity, int>();
        public static bool IsModActive { get { return MainMod.NExperienceMod != null; } }

        public static int GetLevel(GuardianIdentity gi)
        {
            if (SetGuardianLevel.ContainsKey(gi)) return SetGuardianLevel[gi];
            return -1;
        }

        public static void ChangeLevel(GuardianIdentity gi, int Level)
        {
            if (SetGuardianLevel.ContainsKey(gi)) SetGuardianLevel[gi] = Level;
            else SetGuardianLevel.Add(gi, Level);
        }

        public static void GiveExpRewardToPlayer(Player player, float Level, float Difficulty)
        {

        }

        public static void ResetOnWorldLoad()
        {
            SetGuardianLevel.Clear();
        }

        public static bool LevelChanged(TerraGuardian Guardian)
        {
            bool Changed = false;
            int Level = 1;
            GuardianIdentity identity = new GuardianIdentity(Guardian);
            int LastLevel = GetLevel(identity);
            if (Guardian.OwnerPos > -1)
            {
                Level = Main.player[Guardian.OwnerPos].GetModPlayer<NExperience.PlayerMod>().GetGameModeInfo.Level2;
            }
            else
            {
                Level = NExperience.MainMod.LastHighestLeveledPlayer;
                if (Level < NExperience.MainMod.LastHighestLeveledNpc)
                    Level = NExperience.MainMod.LastHighestLeveledNpc;
                //add level scale script.
            }
            Changed = LastLevel != Level;
            ChangeLevel(identity, Level);
            return Changed;
        }

        public static void ScaleStatus(TerraGuardian Guardian)
        {
            GameModeBase gamemode = NExperience.MainMod.GetCurrentGameMode;
            int Level = GetLevel(new GuardianIdentity(Guardian));
            Dictionary<byte, int> Status = new Dictionary<byte,int>();
            int PointsCount = gamemode.InitialStatusPoints + (int)(gamemode.StatusPointsPerLevel * (Level - 1)), PointsSpent = 0;
            int TotalStatus = gamemode.Status.Count;
            for (byte s = 0; s < gamemode.Status.Count; s++)
            {
                int PointsToGive = PointsCount / TotalStatus;
                if (PointsToGive + PointsSpent > PointsCount)
                {
                    PointsToGive = PointsCount - PointsSpent;
                }
                Status.Add(s, PointsToGive);
                PointsSpent += PointsToGive;
            }
            PlayerStatusMod mod;
            gamemode.PlayerStatus(Level, Level, Status, out mod);
            Guardian.MHP = (int)((Guardian.MHP + mod.MaxHealthSum) * mod.MaxHealthMult);
            Guardian.MMP = (int)((Guardian.MMP + mod.MaxManaSum) * mod.MaxManaMult);
            Guardian.MeleeDamageMultiplier = (Guardian.MeleeDamageMultiplier + mod.MeleeDamageSum) * mod.MeleeDamageMult;
            Guardian.RangedDamageMultiplier = (Guardian.RangedDamageMultiplier + mod.RangedDamageSum) * mod.RangedDamageMult;
            Guardian.MagicDamageMultiplier = (Guardian.MagicDamageMultiplier + mod.MagicDamageSum) * mod.MagicDamageMult;
            Guardian.SummonDamageMultiplier = (Guardian.SummonDamageMultiplier + mod.MinionDamageSum) * mod.MinionDamageMult;
            Guardian.MeleeSpeed = (Guardian.MeleeSpeed + mod.MeleeSpeedSum) * mod.MeleeSpeedMult;
            if(Guardian.Defense > 0) Guardian.Defense = (int)((Guardian.Defense + mod.DefenseSum) * mod.DefenseMult);
            Guardian.HealthHealMult += mod.MaxHealthMult * Guardian.HealthHealMult;
            Guardian.ManaHealMult += mod.MaxManaMult * Guardian.ManaHealMult;
            if (!NExperience.MainMod.ItemStatusCapper || !gamemode.AllowLevelCapping)
            {
                if (Guardian.SelectedItem > -1)
                {
                    Item weapon = Guardian.Inventory[Guardian.SelectedItem];
                    if (weapon.damage > 0)
                    {
                        int DamageCap = gamemode.LevelDamageScale(Level);
                        if (DamageCap > 0)
                        {
                            if (weapon.melee)
                                Guardian.MeleeDamageMultiplier *= (float)DamageCap / weapon.damage;
                            else if (weapon.ranged)
                                Guardian.RangedDamageMultiplier *= (float)DamageCap / weapon.damage;
                            else if (weapon.magic)
                                Guardian.MagicDamageMultiplier *= (float)DamageCap / weapon.damage;
                            else if (weapon.summon)
                                Guardian.SummonDamageMultiplier *= (float)DamageCap / weapon.damage;
                        }
                    }
                    int DefenseSum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        DefenseSum += Guardian.Equipments[i].defense;
                    }
                    if (DefenseSum > 0)
                    {
                        int DefenseCap = gamemode.LevelDefenseScale(Level);
                        if (DefenseCap > 0)
                        {
                            Guardian.Defense = (int)(Guardian.Defense * ((float)DefenseCap / DefenseSum));
                        }
                    }
                }
            }
        }
    }
}
