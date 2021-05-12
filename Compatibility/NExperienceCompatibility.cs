using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Compatibility
{
    public class NExperienceCompatibility
    {
        public static Dictionary<GuardianIdentity, int> SetGuardianLevel = new Dictionary<GuardianIdentity, int>();
        public static bool IsModActive { get { return MainMod.NExperienceMod != null; } }

        public static int GetLevel(TerraGuardian tg)
        {
            foreach(GuardianIdentity id in SetGuardianLevel.Keys)
            {
                if (id.IsSame(tg))
                    return SetGuardianLevel[id];
            }
            return -1;
        }

        public static void ChangeLevel(TerraGuardian tg, int Level)
        {
            foreach(GuardianIdentity gi in SetGuardianLevel.Keys)
            {
                if(gi.IsSame(tg))
                {
                    SetGuardianLevel[gi] = Level;
                    tg.UpdateStatus = true;
                    return;
                }
            }
            SetGuardianLevel.Add(new GuardianIdentity(tg), Level);
        }

        public static void GiveExpRewardToPlayer(Player player, float Level, float Difficulty, bool ShowTooltip = true, NExperience.ExpReceivedPopText.ExpSource Source = NExperience.ExpReceivedPopText.ExpSource.Other)
        {
            player.GetModPlayer<NExperience.PlayerMod>().GetExpReward(Level, Difficulty, Source, ShowTooltip);
        }

        public static void ResetOnWorldLoad()
        {
            SetGuardianLevel.Clear();
        }

        public static bool LevelChanged(TerraGuardian Guardian)
        {
            //return false;
            int Level = 1;
            int LastLevel = GetLevel(Guardian);
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
            if (Level != LastLevel)
            {
                ChangeLevel(Guardian, Level);
            }
            return LastLevel != Level;
        }

        public static void ScaleStatus(TerraGuardian Guardian) //Causes fps drop issues. The issue causes the fps to absurdly fall, after a number of seconds of gameplay in a world.
        {
            //return;
            NExperience.GameModeBase gamemode = NExperience.MainMod.GetCurrentGameMode;
            int Level = GetLevel(Guardian);
            Dictionary<byte, int> Status = new Dictionary<byte, int>();
            int PointsCount = gamemode.InitialStatusPoints + (int)(gamemode.StatusPointsPerLevel * (Level - 1)), PointsSpent = 0;
            int TotalStatus = gamemode.Status.Count;
            int PointsToGive = PointsCount / TotalStatus;
            for (byte s = 0; s < gamemode.Status.Count; s++)
            {
                if (PointsToGive + PointsSpent > PointsCount)
                {
                    PointsToGive = PointsCount - PointsSpent;
                }
                Status.Add(s, PointsToGive);
                PointsSpent += PointsToGive;
            }
            NExperience.PlayerStatusMod mod;
            gamemode.PlayerStatus(Level, Level, Status, out mod);
            Guardian.MHP = (int)((Guardian.MHP + mod.MaxHealthSum) * mod.MaxHealthMult);
            Guardian.MMP = (int)((Guardian.MMP + mod.MaxManaSum) * mod.MaxManaMult);
            Guardian.MeleeDamageMultiplier = (Guardian.MeleeDamageMultiplier + mod.MeleeDamageSum) * mod.MeleeDamageMult;
            Guardian.RangedDamageMultiplier = (Guardian.RangedDamageMultiplier + mod.RangedDamageSum) * mod.RangedDamageMult;
            Guardian.MagicDamageMultiplier = (Guardian.MagicDamageMultiplier + mod.MagicDamageSum) * mod.MagicDamageMult;
            Guardian.SummonDamageMultiplier = (Guardian.SummonDamageMultiplier + mod.MinionDamageSum) * mod.MinionDamageMult;
            Guardian.MeleeSpeed = (Guardian.MeleeSpeed + mod.MeleeSpeedSum) * mod.MeleeSpeedMult;
            if (Guardian.Defense > 0) Guardian.Defense = (int)((Guardian.Defense + mod.DefenseSum) * mod.DefenseMult);
            Guardian.HealthHealMult += mod.MaxHealthMult * Guardian.HealthHealMult;
            Guardian.ManaHealMult += mod.MaxManaMult * Guardian.ManaHealMult;
            /*if (!NExperience.MainMod.ItemStatusCapper || !gamemode.AllowLevelCapping)
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
            }*/
        }
    }
}
