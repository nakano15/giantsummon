using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianPlayerAccessoryEffects
    {
        public static void ApplyAccessoryEffect(TerraGuardian guardian, Item i)
        {
            if (i.type == 0) return;
            switch (i.type)
            {
                case ItemID.BandofStarpower:
                    guardian.MMP += 20;
                    break;
                case ItemID.ManaRegenerationBand:
                    guardian.MMP += 20;
                    guardian.ManaRegenBonus += 25;
                    guardian.AddFlag(GuardianFlags.ManaRegenDelayReduction);
                    break;
                case ItemID.MagicCuffs:
                    guardian.MMP += 20;
                    guardian.ManaRegenBonus += 25;
                    guardian.AddFlag(GuardianFlags.MagicCuffs);
                    guardian.AddFlag(GuardianFlags.ManaRegenDelayReduction);
                    break;
                case ItemID.CelestialMagnet:
                    guardian.AddFlag(GuardianFlags.StarMagnet);
                    break;
                case ItemID.CelestialCuffs:
                    guardian.MMP += 20;
                    guardian.ManaRegenBonus += 25;
                    guardian.AddFlag(GuardianFlags.MagicCuffs);
                    guardian.AddFlag(GuardianFlags.ManaRegenDelayReduction);
                    guardian.AddFlag(GuardianFlags.StarMagnet);
                    break;
                case ItemID.CelestialEmblem:
                    guardian.AddFlag(GuardianFlags.StarMagnet);
                    guardian.MagicDamageMultiplier += 0.15f;
                    break;
                case ItemID.NaturesGift:
                    guardian.ManaCostMult -= 0.06f;
                    break;
                case ItemID.ManaFlower:
                    guardian.ManaCostMult -= 0.08f;
                    guardian.AddFlag(GuardianFlags.AutoManaPotion);
                    break;
                case ItemID.Aglet:
                    guardian.MoveSpeed += 0.05f;
                    break;
                case ItemID.ShinyRedBalloon:
                case ItemID.BalloonPufferfish:
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.FartinaJar:
                    guardian.AddFlag(GuardianFlags.ExtraJumpFart);
                    break;
                case ItemID.CloudinaBottle:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    break;
                case ItemID.BlizzardinaBottle:
                    guardian.AddFlag(GuardianFlags.ExtraJumpBlizzard);
                    break;
                case ItemID.TsunamiInABottle:
                    guardian.AddFlag(GuardianFlags.ExtraJumpWater);
                    break;
                case ItemID.SandstorminaBottle:
                    guardian.AddFlag(GuardianFlags.ExtraJumpSand);
                    break;
                case ItemID.CloudinaBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.BlizzardinaBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpBlizzard);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.SandstorminaBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpSand);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.SharkronBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    guardian.AddFlag(GuardianFlags.ExtraJumpWater);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.BalloonHorseshoeSharkron:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    guardian.AddFlag(GuardianFlags.ExtraJumpWater);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.BundleofBalloons:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    guardian.AddFlag(GuardianFlags.ExtraJumpBlizzard);
                    guardian.AddFlag(GuardianFlags.ExtraJumpSand);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.LuckyHorseshoe:
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.AnkletoftheWind:
                    guardian.MoveSpeed += 0.1f;
                    break;
                case ItemID.HermesBoots:
                case ItemID.FlurryBoots:
                case ItemID.SailfishBoots:
                    guardian.DashSpeed = guardian.Base.MaxSpeed * 2;
                    break;
                case ItemID.SpectreBoots:
                    guardian.DashSpeed = guardian.Base.MaxSpeed * 2;
                    guardian.RocketType = 2;
                    break;
                case ItemID.RocketBoots:
                    guardian.RocketType = 1;
                    break;
                case ItemID.LightningBoots:
                    guardian.DashSpeed = guardian.Base.MaxSpeed * 2.25f;
                    guardian.RocketType = 2;
                    guardian.MoveSpeed += 0.08f;
                    break;
                case ItemID.FrostsparkBoots:
                    guardian.DashSpeed = guardian.Base.MaxSpeed * 2.25f;
                    guardian.RocketType = 3;
                    guardian.MoveSpeed += 0.08f;
                    guardian.AddFlag(GuardianFlags.IceSkating);
                    break;
                case ItemID.IceSkates:
                    guardian.AddFlag(GuardianFlags.IceSkating);
                    break;
                case ItemID.BlueHorseshoeBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpCloud);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.WhiteHorseshoeBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpBlizzard);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.YellowHorseshoeBalloon:
                    guardian.AddFlag(GuardianFlags.ExtraJumpSand);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.BalloonHorseshoeFart:
                    guardian.AddFlag(GuardianFlags.ExtraJumpFart);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.BalloonHorseshoeHoney:
                    guardian.AddFlag(GuardianFlags.BeeCounter);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    break;
                case ItemID.ObsidianHorseshoe:
                    guardian.AddFlag(GuardianFlags.FallDamageImmunity);
                    guardian.AddFlag(GuardianFlags.FireblocksImmunity);
                    break;
                case ItemID.HoneyComb:
                    guardian.AddFlag(GuardianFlags.BeeCounter);
                    break;
                case ItemID.HoneyBalloon:
                    guardian.AddFlag(GuardianFlags.BeeCounter);
                    guardian.AddFlag(GuardianFlags.JumpHeightBoost);
                    break;
                case ItemID.Flipper:
                    guardian.AddFlag(GuardianFlags.SwimmingAbility);
                    break;
                case ItemID.DivingGear:
                    guardian.AddFlag(GuardianFlags.ExtendedBreath);
                    guardian.AddFlag(GuardianFlags.SwimmingAbility);
                    break;
                case ItemID.JellyfishNecklace:
                    guardian.AddFlag(GuardianFlags.UnderwaterJellyfishGlow);
                    break;
                case ItemID.JellyfishDivingGear:
                    guardian.AddFlag(GuardianFlags.ExtendedBreath);
                    guardian.AddFlag(GuardianFlags.SwimmingAbility);
                    guardian.AddFlag(GuardianFlags.UnderwaterJellyfishGlow);
                    break;
                case ItemID.ArcticDivingGear:
                    guardian.AddFlag(GuardianFlags.ExtendedBreath);
                    guardian.AddFlag(GuardianFlags.SwimmingAbility);
                    guardian.AddFlag(GuardianFlags.UnderwaterJellyfishGlow);
                    guardian.AddFlag(GuardianFlags.IceSkating);
                    break;
                case ItemID.ClimbingClaws:
                    guardian.AddFlag(GuardianFlags.ClimbingClaws);
                    break;
                case ItemID.ShoeSpikes:
                    guardian.AddFlag(GuardianFlags.ClimbingPaws);
                    break;
                case ItemID.TigerClimbingGear:
                    guardian.AddFlag(GuardianFlags.ClimbingClaws);
                    guardian.AddFlag(GuardianFlags.ClimbingPaws);
                    break;
                case ItemID.MasterNinjaGear:
                    guardian.AddFlag(GuardianFlags.ClimbingClaws);
                    guardian.AddFlag(GuardianFlags.ClimbingPaws);
                    guardian.AddFlag(GuardianFlags.Dash);
                    guardian.DodgeRate += 10;
                    break;
                case ItemID.Tabi:
                    guardian.AddFlag(GuardianFlags.Dash);
                    break;
                case ItemID.BlackBelt:
                    guardian.DodgeRate += 10;
                    break;
                case ItemID.FrogLeg:
                    guardian.AddFlag(GuardianFlags.AllowHopping);
                    guardian.JumpSpeed += 2.4f;
                    guardian.FallHeightTolerance += 15;
                    break;
                case ItemID.LavaCharm:
                    guardian.AddFlag(GuardianFlags.LavaTolerance);
                    break;
                case ItemID.WaterWalkingBoots:
                    guardian.AddFlag(GuardianFlags.WaterWalking);
                    break;
                case ItemID.ObsidianWaterWalkingBoots:
                    guardian.AddFlag(GuardianFlags.WaterWalking);
                    guardian.AddFlag(GuardianFlags.FireblocksImmunity);
                    break;
                case ItemID.LavaWaders:
                    guardian.AddFlag(GuardianFlags.LavaTolerance);
                    guardian.AddFlag(GuardianFlags.WaterWalking);
                    guardian.AddFlag(GuardianFlags.FireblocksImmunity);
                    break;
                case ItemID.CobaltShield:
                    guardian.AddFlag(GuardianFlags.KnockbackImmunity);
                    break;
                case ItemID.ObsidianShield:
                    guardian.AddFlag(GuardianFlags.KnockbackImmunity);
                    guardian.AddFlag(GuardianFlags.FireblocksImmunity);
                    break;
                case ItemID.AnkhShield:
                    guardian.AddFlag(GuardianFlags.KnockbackImmunity);
                    guardian.AddFlag(GuardianFlags.FireblocksImmunity);
                    guardian.AddBuffImmunity(BuffID.Bleeding);
                    guardian.AddBuffImmunity(BuffID.BrokenArmor);
                    guardian.AddBuffImmunity(BuffID.Confused);
                    guardian.AddBuffImmunity(BuffID.Cursed);
                    guardian.AddBuffImmunity(BuffID.Darkness);
                    guardian.AddBuffImmunity(BuffID.Poisoned);
                    guardian.AddBuffImmunity(BuffID.Silenced);
                    guardian.AddBuffImmunity(BuffID.Slow);
                    guardian.AddBuffImmunity(BuffID.Weak);
                    guardian.AddBuffImmunity(BuffID.Burning);
                    break;
                case ItemID.StarCloak:
                    guardian.AddFlag(GuardianFlags.StarCounter);
                    break;
                case ItemID.CrossNecklace:
                    guardian.AddFlag(GuardianFlags.ImprovedImmuneTime);
                    break;
                case ItemID.StarVeil:
                    guardian.AddFlag(GuardianFlags.ImprovedImmuneTime);
                    guardian.AddFlag(GuardianFlags.StarCounter);
                    break;
                case ItemID.BeeCloak:
                    guardian.AddFlag(GuardianFlags.StarCounter);
                    guardian.AddFlag(GuardianFlags.BeeCounter);
                    break;
                case ItemID.AvengerEmblem:
                    guardian.MeleeDamageMultiplier += 0.12f;
                    guardian.RangedDamageMultiplier += 0.12f;
                    guardian.MagicDamageMultiplier += 0.12f;
                    guardian.SummonDamageMultiplier += 0.12f;
                    break;
                case ItemID.WarriorEmblem:
                    guardian.MeleeDamageMultiplier += 0.15f;
                    break;
                case ItemID.RangerEmblem:
                    guardian.RangedDamageMultiplier += 0.15f;
                    break;
                case ItemID.SorcererEmblem:
                    guardian.MagicDamageMultiplier += 0.15f;
                    break;
                case ItemID.SummonerEmblem:
                    guardian.SummonDamageMultiplier += 0.15f;
                    break;
                case ItemID.MoonCharm:
                    guardian.AddFlag(GuardianFlags.WerewolfAcc);
                    break;
                case ItemID.FlowerBoots:
                    guardian.AddFlag(GuardianFlags.FlowerBoots);
                    break;
                case ItemID.NeptunesShell:
                    guardian.AddFlag(GuardianFlags.MerfolkAcc);
                    break;
                case ItemID.MoonShell:
                    guardian.AddFlag(GuardianFlags.WerewolfAcc);
                    guardian.AddFlag(GuardianFlags.MerfolkAcc);
                    break;
                case ItemID.SunStone:
                    guardian.AddFlag(GuardianFlags.SunBuff);
                    break;
                case ItemID.MoonStone:
                    guardian.AddFlag(GuardianFlags.MoonBuff);
                    break;
                case ItemID.CelestialStone:
                    guardian.AddFlag(GuardianFlags.SunBuff);
                    guardian.AddFlag(GuardianFlags.MoonBuff);
                    break;
                case ItemID.CelestialShell:
                    guardian.AddFlag(GuardianFlags.SunBuff);
                    guardian.AddFlag(GuardianFlags.MoonBuff);
                    guardian.AddFlag(GuardianFlags.WerewolfAcc);
                    guardian.AddFlag(GuardianFlags.MerfolkAcc);
                    break;
                case ItemID.RifleScope:
                    guardian.AddFlag(GuardianFlags.Scope);
                    break;
                case ItemID.SniperScope:
                    guardian.AddFlag(GuardianFlags.Scope);
                    guardian.RangedDamageMultiplier += 0.1f;
                    guardian.RangedCriticalRate += 10;
                    break;
                case ItemID.DestroyerEmblem:
                    guardian.MeleeDamageMultiplier += 0.1f;
                    guardian.RangedDamageMultiplier += 0.1f;
                    guardian.MagicDamageMultiplier += 0.1f;
                    guardian.SummonDamageMultiplier += 0.1f;
                    guardian.MeleeCriticalRate += 8;
                    guardian.RangedCriticalRate += 8;
                    guardian.MagicCriticalRate += 8;
                    break;
                case ItemID.FeralClaws:
                    guardian.MeleeSpeed += 0.12f;
                    break;
                case ItemID.ObsidianRose:
                    guardian.AddFlag(GuardianFlags.LavaDamageReduction);
                    break;
                case ItemID.MagicQuiver:
                    guardian.RangedDamageMultiplier += 0.1f;
                    guardian.AddFlag(GuardianFlags.ArrowBuff);
                    break;
                case ItemID.MagmaStone:
                    guardian.AddFlag(GuardianFlags.MagmaStone);
                    break;
                case ItemID.FireGauntlet:
                    guardian.MeleeDamageMultiplier += 0.1f;
                    guardian.MeleeKnockback += 0.8f;
                    guardian.MeleeSpeed += 0.1f;
                    guardian.AddFlag(GuardianFlags.MagmaStone);
                    break;
                case ItemID.MechanicalGlove:
                    guardian.MeleeDamageMultiplier += 0.12f;
                    guardian.MeleeKnockback += 0.7f;
                    guardian.MeleeSpeed += 0.12f;
                    break;
                case ItemID.PowerGlove:
                    guardian.MeleeKnockback += 0.7f;
                    guardian.MeleeSpeed += 0.12f;
                    break;
                case ItemID.TitanGlove:
                    guardian.MeleeKnockback += 0.7f;
                    break;
                case ItemID.FleshKnuckles:
                    guardian.AddFlag(GuardianFlags.Tanking);
                    //guardian.Defense += 7;
                    break;
                case ItemID.FrozenTurtleShell:
                    guardian.AddFlag(GuardianFlags.FrozenTurtleShell);
                    break;
                case ItemID.PaladinsShield:
                    //guardian.Defense += 6;
                    guardian.CoverRate += 25;
                    guardian.AddFlag(GuardianFlags.KnockbackImmunity);
                    break;
                case ItemID.PutridScent:
                    guardian.MeleeDamageMultiplier += 0.05f;
                    guardian.RangedDamageMultiplier += 0.05f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.SummonDamageMultiplier += 0.05f;
                    guardian.MeleeCriticalRate += 5;
                    guardian.RangedCriticalRate += 5;
                    guardian.MagicCriticalRate += 5;
                    break;
                case ItemID.SharkToothNecklace:
                    guardian.MeleeDamageMultiplier += 0.05f;
                    guardian.RangedDamageMultiplier += 0.05f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.SummonDamageMultiplier += 0.05f;
                    break;
                case ItemID.HerculesBeetle:
                    guardian.SummonDamageMultiplier += 0.15f;
                    break;
                case ItemID.NecromanticScroll:
                    guardian.SummonDamageMultiplier += 0.10f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.PapyrusScarab:
                    guardian.SummonDamageMultiplier += 0.25f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.PygmyNecklace:
                    guardian.MaxMinions++;
                    break;
                case ItemID.PhilosophersStone:
                    guardian.AddFlag(GuardianFlags.ReduceHealingPotionCooldown);
                    break;
                case ItemID.BandofRegeneration:
                    guardian.AddFlag(GuardianFlags.ImprovedHealthRegeneration);
                    break;
                case ItemID.CharmofMyths:
                    guardian.AddFlag(GuardianFlags.ReduceHealingPotionCooldown);
                    guardian.AddFlag(GuardianFlags.ImprovedImmuneTime);
                    guardian.AddFlag(GuardianFlags.ImprovedHealthRegeneration);
                    //Health regen bonus
                    break;
                case ItemID.WormScarf:
                    guardian.DefenseRate += 17;
                    break;
                case ItemID.HiveBackpack:
                    guardian.AddFlag(GuardianFlags.BeeBuff);
                    break;
                case ItemID.PanicNecklace:
                    guardian.AddFlag(GuardianFlags.PanicNecklace);
                    break;
                case ItemID.SweetheartNecklace:
                    guardian.AddFlag(GuardianFlags.PanicNecklace);
                    guardian.AddFlag(GuardianFlags.BeeCounter);
                    break;
                case ItemID.AdhesiveBandage:
                    guardian.AddBuffImmunity(BuffID.Bleeding);
                    break;
                case ItemID.AnkhCharm:
                    guardian.AddBuffImmunity(BuffID.Bleeding);
                    guardian.AddBuffImmunity(BuffID.BrokenArmor);
                    guardian.AddBuffImmunity(BuffID.Confused);
                    guardian.AddBuffImmunity(BuffID.Cursed);
                    guardian.AddBuffImmunity(BuffID.Darkness);
                    guardian.AddBuffImmunity(BuffID.Poisoned);
                    guardian.AddBuffImmunity(BuffID.Silenced);
                    guardian.AddBuffImmunity(BuffID.Slow);
                    guardian.AddBuffImmunity(BuffID.Weak);
                    break;
                case ItemID.ArmorPolish:
                    guardian.AddBuffImmunity(BuffID.BrokenArmor);
                    break;
                case ItemID.Vitamins:
                    guardian.AddBuffImmunity(BuffID.Weak);
                    break;
                case ItemID.ArmorBracing:
                    guardian.AddBuffImmunity(BuffID.Weak);
                    guardian.AddBuffImmunity(BuffID.BrokenArmor);
                    break;
                case ItemID.Bezoar:
                    guardian.AddBuffImmunity(BuffID.Poisoned);
                    break;
                case ItemID.MedicatedBandage:
                    guardian.AddBuffImmunity(BuffID.Bleeding);
                    guardian.AddBuffImmunity(BuffID.Poisoned);
                    break;
                case ItemID.Blindfold:
                    guardian.AddBuffImmunity(BuffID.Darkness);
                    break;
                case ItemID.CountercurseMantra:
                    guardian.AddBuffImmunity(BuffID.Silenced);
                    guardian.AddBuffImmunity(BuffID.Cursed);
                    break;
                case ItemID.Megaphone:
                    guardian.AddBuffImmunity(BuffID.Silenced);
                    break;
                case ItemID.Nazar:
                    guardian.AddBuffImmunity(BuffID.Cursed);
                    break;
                case ItemID.FastClock:
                    guardian.AddBuffImmunity(BuffID.Slow);
                    break;
                case ItemID.TrifoldMap:
                    guardian.AddBuffImmunity(BuffID.Confused);
                    break;
                case ItemID.ThePlan:
                    guardian.AddBuffImmunity(BuffID.Slow);
                    guardian.AddBuffImmunity(BuffID.Confused);
                    break;
                case ItemID.HandWarmer:
                    guardian.AddBuffImmunity(BuffID.Chilled);
                    guardian.AddBuffImmunity(BuffID.Frozen);
                    break;
                    //Wings
                case 492:
                case 493:
                case 2494:
                    guardian.WingMaxFlightTime = 100;
                    break;
                case 748:
                    guardian.WingMaxFlightTime = 115;
                    break;
                case 749:
                case 761:
                case 1515:
                    guardian.WingMaxFlightTime = 130;
                    break;
                case 785:
                case 786:
                case 1165:
                    guardian.WingMaxFlightTime = 140;
                    break;
                case 665:
                case 1583:
                case 1584:
                case 1585:
                case 1586:
                case 3228:
                case 3580:
                case 3582:
                case 3588:
                case 3592:
                case 3883:
                    guardian.WingMaxFlightTime = 150;
                    break;
                case 821:
                case 822:
                case 823:
                case 1162:
                case 2280:
                case 2770:
                case 3469:
                case 3470:
                    guardian.WingMaxFlightTime = 160;
                    break;
                case 1866:
                case 1871:
                    guardian.WingMaxFlightTime = 170;
                    break;
                case 2609:
                    guardian.WingMaxFlightTime = 180;
                    //Ignore water effect?
                    break;
                case 948:
                case 1797:
                case 1830:
                case 3468:
                case 3471:
                    guardian.WingMaxFlightTime = 180;
                    break;
            }
        }

        public static void CheckForAccessoryDescriptionChanges(Item i, List<TooltipLine> tooltips)
        {
            string GuardianStatus = "";
            switch (i.type)
            {
                default:
                    return;
                case ItemID.PaladinsShield:
                    GuardianStatus = "Guardians gains 25% chance of using cover, instead of the damage share.";
                    break;
            }
            if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.Active)
            {
                if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.CanUseItem(i))
                    tooltips.Insert(tooltips.Count - 1, new TooltipLine(MainMod.mod, "GuardianCanUseItemAval", "Your TerraGuardian can use this."));
            }
            tooltips.Insert(tooltips.Count - 2, new TooltipLine(MainMod.mod, "GuardianSpecificInfo", GuardianStatus));
        }
    }
}
