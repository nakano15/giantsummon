using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace giantsummon
{
    public class GuardianArmorAndSetEffects
    {
        public static void GetArmorPieceEffect(TerraGuardian guardian, Item equip)
        {
            switch (equip.type)
            {
                case ItemID.MiningHelmet:
                    guardian.AddFlag(GuardianFlags.MiningHelmet);
                    break;
                case ItemID.NinjaHood:
                    guardian.ShotSpeedMult += 0.15f;
                    break;
                case ItemID.NinjaShirt:
                    guardian.RangedDamageMultiplier += 0.15f;
                    break;
                case ItemID.NinjaPants:
                    guardian.RangedCriticalRate += 10;
                    break;
                case ItemID.FossilHelm:
                    guardian.ShotSpeedMult += 0.2f;
                    break;
                case ItemID.FossilShirt:
                    guardian.RangedDamageMultiplier += 0.2f;
                    break;
                case ItemID.FossilPants:
                    guardian.RangedCriticalRate += 15;
                    break;
                case ItemID.BeeHeadgear:
                    guardian.SummonDamageMultiplier += 0.04f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.BeeBreastplate:
                    guardian.SummonDamageMultiplier += 0.04f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.BeeGreaves:
                    guardian.SummonDamageMultiplier += 0.05f;
                    break;
                case ItemID.JungleHat:
                case ItemID.AncientCobaltHelmet:
                    guardian.MagicCriticalRate += 4;
                    guardian.MMP += 40;
                    break;
                case ItemID.JungleShirt:
                case ItemID.JunglePants:
                case ItemID.AncientCobaltBreastplate:
                case ItemID.AncientCobaltLeggings:
                    guardian.MagicCriticalRate += 4;
                    guardian.MMP += 20;
                    break;
                case ItemID.MeteorHelmet:
                case ItemID.MeteorSuit:
                case ItemID.MeteorLeggings:
                    guardian.MagicDamageMultiplier += 0.07f;
                    break;
                case ItemID.AncientNecroHelmet:
                case ItemID.NecroHelmet:
                case ItemID.NecroBreastplate:
                case ItemID.NecroGreaves:
                    guardian.RangedDamageMultiplier += 0.05f;
                    break;
                case ItemID.AncientShadowHelmet:
                case ItemID.ShadowHelmet:
                case ItemID.AncientShadowScalemail:
                case ItemID.ShadowScalemail:
                case ItemID.AncientShadowGreaves:
                case ItemID.ShadowGreaves:
                    guardian.MeleeSpeed += 0.07f;
                    break;
                case ItemID.CrimsonHelmet:
                case ItemID.CrimsonScalemail:
                case ItemID.CrimsonGreaves:
                    guardian.MeleeDamageMultiplier += 0.02f;
                    break;
                case ItemID.SpiderMask:
                case ItemID.SpiderBreastplate:
                case ItemID.SpiderGreaves:
                    guardian.SummonDamageMultiplier += 0.06f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.CobaltHat:
                    guardian.MMP += 40;
                    guardian.MagicCriticalRate += 9;
                    break;
                case ItemID.CobaltHelmet:
                    guardian.MoveSpeed += 0.07f;
                    guardian.MeleeSpeed += 0.12f;
                    break;
                case ItemID.CobaltMask:
                    guardian.RangedDamageMultiplier += 0.1f;
                    guardian.RangedCriticalRate += 6;
                    break;
                case ItemID.CobaltBreastplate:
                    guardian.MeleeCriticalRate += 3;
                    guardian.RangedCriticalRate += 3;
                    guardian.MagicCriticalRate += 3;
                    break;
                case ItemID.CobaltLeggings:
                    guardian.MoveSpeed += 0.10f;
                    break;
                case ItemID.PalladiumHeadgear:
                    guardian.MagicDamageMultiplier += 0.07f;
                    guardian.MMP += 60;
                    break;
                case ItemID.PalladiumHelmet:
                    guardian.RangedDamageMultiplier += 0.09f;
                    guardian.RangedCriticalRate += 9;
                    break;
                case ItemID.PalladiumMask:
                    guardian.MeleeDamageMultiplier += 0.08f;
                    guardian.MeleeSpeed += 0.12f;
                    break;
                case ItemID.PalladiumBreastplate:
                    guardian.MeleeDamageMultiplier += 0.03f;
                    guardian.RangedDamageMultiplier += 0.03f;
                    guardian.MagicDamageMultiplier += 0.03f;
                    guardian.SummonDamageMultiplier += 0.03f;
                    guardian.MeleeCriticalRate += 2;
                    guardian.RangedCriticalRate += 2;
                    guardian.MagicCriticalRate += 2;
                    break;
                case ItemID.PalladiumLeggings:
                    guardian.MeleeDamageMultiplier += 0.02f;
                    guardian.RangedDamageMultiplier += 0.02f;
                    guardian.MagicDamageMultiplier += 0.02f;
                    guardian.SummonDamageMultiplier += 0.02f;
                    guardian.MeleeCriticalRate += 1;
                    guardian.RangedCriticalRate += 1;
                    guardian.MagicCriticalRate += 1;
                    break;
                case ItemID.MythrilHood:
                    guardian.MMP += 60;
                    guardian.MagicDamageMultiplier += 0.15f;
                    break;
                case ItemID.MythrilHelmet:
                    guardian.MeleeCriticalRate += 5;
                    guardian.MeleeDamageMultiplier += 0.1f;
                    break;
                case ItemID.MythrilHat:
                    guardian.RangedDamageMultiplier += 0.12f;;
                    guardian.RangedCriticalRate += 7;
                    break;
                case ItemID.MythrilChainmail:
                    guardian.MeleeDamageMultiplier += 0.05f;
                    guardian.RangedDamageMultiplier += 0.05f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.SummonDamageMultiplier += 0.05f;
                    break;
                case ItemID.MythrilGreaves:
                    guardian.MeleeCriticalRate += 3;
                    guardian.RangedCriticalRate += 3;
                    guardian.MagicCriticalRate += 3;
                    break;
                case ItemID.OrichalcumHeadgear:
                    guardian.MagicCriticalRate += 18;
                    guardian.MMP += 80;
                    break;
                case ItemID.OrichalcumHelmet:
                    guardian.RangedCriticalRate += 15;
                    guardian.RangedSpeed += 0.08f;
                    break;
                case ItemID.OrichalcumMask:
                    guardian.MeleeDamageMultiplier += 0.07f;
                    guardian.MeleeCriticalRate += 7;
                    break;
                case ItemID.OrichalcumBreastplate:
                    guardian.MeleeDamageMultiplier += 0.06f;
                    guardian.RangedDamageMultiplier += 0.06f;
                    guardian.MagicDamageMultiplier += 0.06f;
                    guardian.SummonDamageMultiplier += 0.06f;
                    break;
                case ItemID.OrichalcumLeggings:
                    guardian.MoveSpeed += 0.11f;
                    break;
                case ItemID.AdamantiteHeadgear:
                    guardian.MMP += 80;
                    guardian.MagicDamageMultiplier += 0.11f;
                    guardian.MagicCriticalRate += 11;
                    break;
                case ItemID.AdamantiteHelmet:
                    guardian.MeleeCriticalRate += 7;
                    guardian.MeleeDamageMultiplier += 0.14f;
                    break;
                case ItemID.AdamantiteMask:
                    guardian.RangedDamageMultiplier += 0.14f;
                    guardian.RangedCriticalRate += 8;
                    break;
                case ItemID.AdamantiteBreastplate:
                    guardian.MeleeDamageMultiplier += 0.06f;
                    guardian.RangedDamageMultiplier += 0.06f;
                    guardian.MagicDamageMultiplier += 0.06f;
                    guardian.SummonDamageMultiplier += 0.06f;
                    break;
                case ItemID.AdamantiteLeggings:
                    guardian.MeleeCriticalRate += 4;
                    guardian.RangedCriticalRate += 4;
                    guardian.MagicCriticalRate += 4;
                    guardian.MoveSpeed += 0.05f;
                    break;
                case ItemID.TitaniumHeadgear:
                    guardian.MagicDamageMultiplier += 0.16f;
                    guardian.MagicCriticalRate += 7;
                    guardian.MMP += 100;
                    break;
                case ItemID.TitaniumHelmet:
                    guardian.RangedDamageMultiplier += 0.16f;
                    guardian.RangedCriticalRate += 7;
                    break;
                case ItemID.TitaniumMask:
                    guardian.MeleeDamageMultiplier += 0.08f;
                    guardian.MeleeSpeed += 0.08f;
                    break;
                case ItemID.TitaniumBreastplate:
                    guardian.MeleeDamageMultiplier += 0.04f;
                    guardian.RangedDamageMultiplier += 0.04f;
                    guardian.MagicDamageMultiplier += 0.04f;
                    guardian.SummonDamageMultiplier += 0.04f;
                    guardian.MeleeCriticalRate += 3;
                    guardian.RangedCriticalRate += 3;
                    guardian.MagicCriticalRate += 3;
                    break;
                case ItemID.TitaniumLeggings:
                    guardian.MeleeDamageMultiplier += 0.03f;
                    guardian.RangedDamageMultiplier += 0.03f;
                    guardian.MagicDamageMultiplier += 0.03f;
                    guardian.SummonDamageMultiplier += 0.03f;
                    guardian.MeleeCriticalRate += 3;
                    guardian.RangedCriticalRate += 3;
                    guardian.MagicCriticalRate += 3;
                    guardian.MoveSpeed += 0.06f;
                    break;
                case ItemID.FrostHelmet:
                    guardian.MeleeDamageMultiplier += 0.16f;
                    guardian.RangedDamageMultiplier += 0.16f;
                    break;
                case ItemID.FrostBreastplate:
                    guardian.MeleeCriticalRate += 11;
                    guardian.RangedCriticalRate += 11;
                    break;
                case ItemID.FrostLeggings:
                    guardian.MoveSpeed += 0.08f;
                    guardian.MeleeSpeed += 0.07f;
                    break;
                case 3776: //Forbidden Mask
                    guardian.MagicDamageMultiplier += 0.15f;
                    guardian.SummonDamageMultiplier += 0.15f;
                    break;
                case 3777: //Forbidden Robes
                    guardian.MMP += 80;
                    break;
                case 3778: //Forbidden Treads
                    guardian.MaxMinions += 2;
                    break;
                case ItemID.MagicHat:
                    guardian.MagicDamageMultiplier += 0.07f;
                    guardian.MagicCriticalRate += 7;
                    break;
                case ItemID.WizardHat:
                    guardian.MagicDamageMultiplier += 0.15f;
                    break;
                case ItemID.AmethystRobe:
                    guardian.MMP += 20;
                    guardian.ManaCostMult -= 0.05f;
                    break;
                case ItemID.TopazRobe:
                    guardian.MMP += 40;
                    guardian.ManaCostMult -= 0.07f;
                    break;
                case ItemID.SapphireRobe:
                    guardian.MMP += 40;
                    guardian.ManaCostMult -= 0.09f;
                    break;
                case ItemID.EmeraldRobe:
                    guardian.MMP += 60;
                    guardian.ManaCostMult -= 0.11f;
                    break;
                case ItemID.RubyRobe:
                    guardian.MMP += 60;
                    guardian.ManaCostMult -= 0.13f;
                    break;
                case ItemID.DiamondRobe:
                    guardian.MMP += 80;
                    guardian.ManaCostMult -= 0.15f;
                    break;
                case ItemID.GypsyRobe:
                    guardian.MagicDamageMultiplier += 0.06f;
                    guardian.MagicCriticalRate += 6;
                    guardian.ManaCostMult -= 0.1f;
                    break;
                case ItemID.DivingHelmet:
                    guardian.AddFlag(GuardianFlags.ExtendedBreath);
                    break;
                case ItemID.Gi:
                    guardian.MeleeDamageMultiplier += 0.15f;
                    guardian.RangedDamageMultiplier += 0.05f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.SummonDamageMultiplier += 0.05f;
                    guardian.MeleeSpeed += 0.1f;
                    guardian.MoveSpeed += 0.1f;
                    break;
                case ItemID.NightVisionHelmet:
                    guardian.AddFlag(GuardianFlags.NightVisionPotion);
                    break;
                case ItemID.ApprenticeHat:
                    guardian.MaxSentries++;
                    guardian.ManaCostMult -= 0.1f;
                    break;
                case ItemID.ApprenticeRobe:
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.MagicDamageMultiplier += 0.1f;
                    break;
                case ItemID.ApprenticeTrousers:
                    guardian.SummonDamageMultiplier += 0.1f;
                    guardian.MoveSpeed += 0.2f;
                    break;
                case ItemID.SquireGreatHelm:
                    guardian.MaxSentries++;
                    guardian.AddFlag(GuardianFlags.SquireHelmetHealthRegen);
                    break;
                case ItemID.SquirePlating:
                    guardian.SummonDamageMultiplier += 0.15f;
                    guardian.MeleeDamageMultiplier += 0.15f;
                    break;
                case ItemID.SquireGreaves:
                    guardian.SummonDamageMultiplier += 0.15f;
                    guardian.MeleeCriticalRate += 20;
                    guardian.MoveSpeed += 0.2f;
                    break;
                case ItemID.HuntressWig:
                    guardian.MaxSentries++;
                    guardian.RangedCriticalRate += 10;
                    break;
                case ItemID.HuntressJerkin:
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.RangedDamageMultiplier += 0.2f;
                    break;
                case ItemID.HuntressPants:
                    guardian.SummonDamageMultiplier += 0.1f;
                    guardian.MoveSpeed += 0.2f;
                    break;
                case ItemID.MonkBrows:
                    guardian.MaxSentries++;
                    guardian.MeleeSpeed += 0.2f;
                    break;
                case ItemID.MonkShirt:
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.MeleeDamageMultiplier += 0.2f;
                    break;
                case ItemID.MonkPants:
                    guardian.SummonDamageMultiplier += 0.1f;
                    guardian.MeleeCriticalRate += 10;
                    guardian.MoveSpeed += 0.2f;
                    break;
                case ItemID.HallowedHeadgear:
                    guardian.MMP += 100;
                    guardian.MagicDamageMultiplier += 0.12f;
                    guardian.MagicCriticalRate += 12;
                    break;
                case ItemID.HallowedMask:
                    guardian.MeleeDamageMultiplier += 0.1f;
                    guardian.MeleeSpeed += 0.1f;
                    break;
                case ItemID.HallowedHelmet:
                    guardian.RangedDamageMultiplier += 0.15f;
                    guardian.RangedCriticalRate += 8;
                    break;
                case ItemID.HallowedPlateMail:
                    guardian.MeleeCriticalRate += 7;
                    guardian.RangedCriticalRate += 7;
                    guardian.MagicCriticalRate += 7;
                    break;
                case ItemID.HallowedGreaves:
                    guardian.MeleeDamageMultiplier += 0.07f;
                    guardian.RangedDamageMultiplier += 0.07f;
                    guardian.MagicDamageMultiplier += 0.07f;
                    guardian.SummonDamageMultiplier += 0.07f;
                    guardian.MoveSpeed += 0.08f;
                    break;
                case ItemID.ChlorophyteMask:
                    guardian.MeleeDamageMultiplier += 0.16f;
                    guardian.MeleeCriticalRate += 6;
                    break;
                case ItemID.ChlorophyteHelmet:
                    guardian.RangedDamageMultiplier += 0.16f;
                    guardian.AddFlag(GuardianFlags.ChlorophyteAmmoCostReduction);
                    break;
                case ItemID.ChlorophyteHeadgear:
                    guardian.MMP += 80;
                    guardian.ManaCostMult -= 0.17f;
                    guardian.MagicDamageMultiplier += 0.16f;
                    break;
                case ItemID.ChlorophytePlateMail:
                    guardian.MeleeDamageMultiplier += 0.05f;
                    guardian.RangedDamageMultiplier += 0.05f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.SummonDamageMultiplier += 0.05f;
                    guardian.MeleeCriticalRate += 7;
                    guardian.RangedCriticalRate += 7;
                    guardian.MagicCriticalRate += 7;
                    break;
                case ItemID.ChlorophyteGreaves:
                    guardian.MeleeCriticalRate += 8;
                    guardian.RangedCriticalRate += 8;
                    guardian.MagicCriticalRate += 8;
                    guardian.MoveSpeed += 0.05f;
                    break;
                case ItemID.TurtleHelmet:
                    guardian.MeleeDamageMultiplier += 0.06f;
                    guardian.Aggro += 250;
                    break;
                case ItemID.TurtleScaleMail:
                    guardian.MeleeDamageMultiplier += 0.08f;
                    guardian.MeleeCriticalRate += 8;
                    guardian.Aggro += 250;
                    break;
                case ItemID.TurtleLeggings:
                    guardian.MeleeCriticalRate += 4;
                    guardian.Aggro += 250;
                    break;
                case ItemID.TikiMask:
                case ItemID.TikiShirt:
                case ItemID.TikiPants:
                    guardian.MaxMinions++;
                    guardian.SummonDamageMultiplier += 0.1f;
                    break;
                case ItemID.SpookyHelmet:
                case ItemID.SpookyBreastplate:
                case ItemID.SpookyLeggings:
                    guardian.SummonDamageMultiplier += 0.11f;
                    guardian.MaxMinions++;
                    break;
                case ItemID.ShroomiteHeadgear:
                    guardian.AddFlag(GuardianFlags.ShroomiteArrow);
                    guardian.RangedCriticalRate += 5;
                    break;
                case ItemID.ShroomiteHelmet:
                    guardian.AddFlag(GuardianFlags.ShroomiteRocket);
                    guardian.RangedCriticalRate += 5;
                    break;
                case ItemID.ShroomiteMask:
                    guardian.AddFlag(GuardianFlags.ShroomiteBullet);
                    guardian.RangedCriticalRate += 5;
                    break;
                case ItemID.ShroomiteBreastplate:
                    guardian.RangedDamageMultiplier += 0.13f;
                    guardian.AddFlag(GuardianFlags.ShroomiteAmmoReduction);
                    break;
                case ItemID.ShroomiteLeggings:
                    guardian.RangedCriticalRate += 7;
                    guardian.MoveSpeed += 0.12f;
                    break;
                case ItemID.SpectreMask:
                    guardian.MMP += 60;
                    guardian.ManaCostMult -= 0.13f;
                    guardian.MagicDamageMultiplier += 0.05f;
                    guardian.MagicCriticalRate += 5;
                    break;
                case ItemID.SpectreHood:
                    guardian.MagicDamageMultiplier -= 0.4f;
                    guardian.MMP += 80;
                    guardian.ManaCostMult -= 0.17f;
                    break;
                case ItemID.SpectreRobe:
                    guardian.MagicDamageMultiplier += 0.07f;
                    guardian.MagicCriticalRate += 7;
                    break;
                case ItemID.SpectrePants:
                    guardian.MagicDamageMultiplier += 0.08f;
                    guardian.MoveSpeed += 0.08f;
                    break;
                case ItemID.BeetleHelmet:
                    guardian.MeleeDamageMultiplier += 0.06f;
                    guardian.Aggro += 250;
                    break;
                case ItemID.BeetleScaleMail:
                    guardian.MeleeDamageMultiplier += 0.08f;
                    guardian.MeleeCriticalRate += 8;
                    guardian.MoveSpeed += 0.06f;
                    guardian.MeleeSpeed += 0.06f;
                    break;
                case ItemID.BeetleShell:
                    guardian.MeleeDamageMultiplier += 0.05f;
                    guardian.MeleeCriticalRate += 5;
                    guardian.Aggro += 500;
                    break;
                case ItemID.BeetleLeggings:
                    guardian.MoveSpeed += 0.06f;
                    guardian.MeleeSpeed += 0.06f;
                    guardian.Aggro += 250;
                    break;
                case 3874: //Dark Artist Hat
                    guardian.MaxSentries++;
                    guardian.MagicDamageMultiplier += 0.1f;
                    guardian.SummonDamageMultiplier += 0.1f;
                    break;
                case 3875: //Dark Artist Robe
                    guardian.SummonDamageMultiplier += 0.3f;
                    guardian.MagicDamageMultiplier += 0.15f;
                    break;
                case 3876: //Dark Artist Leggings
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.MagicCriticalRate += 25;
                    break;
                case 3880: //Shinobi Infiltrator Helmet
                    guardian.MaxSentries++;
                    guardian.MeleeDamageMultiplier += 0.2f;
                    guardian.SummonDamageMultiplier += 0.2f;
                    break;
                case 3881: //Shinobi Infiltrator Torso
                    guardian.MeleeSpeed += 0.2f;
                    guardian.SummonDamageMultiplier += 0.2f;
                    break;
                case 3882: //Shinobi Infiltrator Pants
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.MoveSpeed += 0.2f;
                    guardian.MeleeCriticalRate += 20;
                    break;
                case 3877: //Red Riding Hood
                    guardian.MaxSentries++;
                    guardian.SummonDamageMultiplier += 0.1f;
                    guardian.RangedCriticalRate += 10;
                    break;
                case 3878: //Red Riding Dress
                    guardian.SummonDamageMultiplier += 0.25f;
                    guardian.RangedDamageMultiplier += 0.25f;
                    break;
                case 3879: //Red Riding Legs
                    guardian.SummonDamageMultiplier += 0.25f;
                    guardian.MoveSpeed += 0.2f;
                    break;
                case 3871: //Valhalla Knight Helm
                    guardian.MaxSentries++;
                    guardian.SummonDamageMultiplier += 0.1f;
                    break;
                case 3872: //Valhalla Knight Breastplate
                    guardian.SummonDamageMultiplier += 0.3f;
                    guardian.AddFlag(GuardianFlags.DD2ValhallaKnightBreastplateLifeRegen); //No idea of where is the life regen code
                    break;
                case 3873: //Valhalla Knight Greaves
                    guardian.SummonDamageMultiplier += 0.2f;
                    guardian.MeleeCriticalRate += 20;
                    guardian.RangedCriticalRate += 20;
                    guardian.MagicCriticalRate += 20;
                    guardian.MoveSpeed += 0.3f;
                    break;
                case ItemID.SolarFlareHelmet:
                    guardian.MeleeCriticalRate += 17;
                    guardian.Aggro += 300;
                    break;
                case ItemID.SolarFlareBreastplate:
                    guardian.MeleeDamageMultiplier += 0.22f;
                    guardian.Aggro += 300;
                    break;
                case ItemID.SolarFlareLeggings:
                    guardian.MoveSpeed += 0.15f;
                    guardian.MeleeSpeed += 0.15f;
                    guardian.Aggro += 300;
                    break;
                case ItemID.VortexHelmet:
                    guardian.RangedDamageMultiplier += 0.16f;
                    guardian.RangedCriticalRate += 7;
                    break;
                case ItemID.VortexBreastplate:
                    guardian.RangedDamageMultiplier += 0.12f;
                    guardian.RangedCriticalRate += 12;
                    guardian.AddFlag(GuardianFlags.VortexAmmoUseReduction);
                    break;
                case ItemID.VortexLeggings:
                    guardian.RangedDamageMultiplier += 0.08f;
                    guardian.RangedCriticalRate += 8;
                    guardian.MoveSpeed += 0.1f;
                    break;
                case ItemID.NebulaHelmet:
                    guardian.MMP += 60;
                    guardian.ManaCostMult -= 0.15f;
                    guardian.MagicDamageMultiplier += 0.07f;
                    guardian.MagicCriticalRate += 7;
                    break;
                case ItemID.NebulaBreastplate:
                    guardian.MagicDamageMultiplier += 0.09f;
                    guardian.MagicCriticalRate += 9;
                    break;
                case ItemID.NebulaLeggings:
                    guardian.MagicDamageMultiplier += 0.1f;
                    guardian.MoveSpeed += 0.1f;
                    break;
                case ItemID.StardustHelmet:
                    guardian.MaxMinions++;
                    guardian.SummonDamageMultiplier += 0.22f;
                    break;
                case ItemID.StardustBreastplate:
                    guardian.MaxMinions += 2;
                    guardian.SummonDamageMultiplier += 0.22f;
                    break;
                case ItemID.StardustLeggings:
                    guardian.MaxMinions += 2;
                    guardian.SummonDamageMultiplier += 0.22f;
                    break;
            }
        }

        public static void GetArmorSetEffect(TerraGuardian guardian)
        {
            int Head = guardian.Equipments[0].type, Armor = guardian.Equipments[1].type, Leggings = guardian.Equipments[2].type;
            if (Head == 88 && Armor == 410 && Leggings == 411) //Mining Set
            {
                //30% mining speed
            }
            if ((Head == 727 && Armor == 728 && Leggings == 729) || (Head == 733 && Armor == 734 && Leggings == 735) || (Head == 2509 && Armor == 2510 && Leggings == 2511) ||
                (Head == 2512 && Armor == 2513 && Leggings == 2514) || (Head == 730 && Armor == 731 && Leggings == 732) || (Head == 924 && Armor == 925 && Leggings == 926) ||
                (Head == 736 && Armor == 737 && Leggings == 738)) //Wood Sets
            {
                guardian.Defense++;
            }
            if (Head == 894 && Armor == 895 && Leggings == 896) //Cactus Set 
            {
                guardian.Defense++;
            }
            if ((Head == 89 && Armor == 80 && Leggings == 76) || (Head == 687 && Armor == 688 && Leggings == 689)) //Tier 1 Set
            {
                guardian.Defense += 2;
            }
            if (Head == 1731 && Armor == 1732 && Leggings == 1733) //Pumpkin Set 
            {
                guardian.MeleeDamageMultiplier += 0.1f;
                guardian.RangedDamageMultiplier += 0.1f;
                guardian.MagicDamageMultiplier += 0.1f;
                guardian.SummonDamageMultiplier += 0.1f;
            }
            if ((Head == 90 || Head == 954) && Armor == 81 && Leggings == 77) //Iron Set
            {
                guardian.Defense += 2;
            }
            if (Head == 690 && Armor == 691 && Leggings == 692) //Lead Set
            {
                guardian.Defense += 2;
            }
            if ((Head == 91 && Armor == 82 && Leggings == 78) || (Head == 693 && Armor == 694 && Leggings == 695)) // Tier 3 set
                guardian.Defense += 3;
            if (Head == 92 && Armor == 83 && Leggings == 79) //Gold Set
                guardian.Defense += 3;
            if (Head == 696 && Armor == 697 && Leggings == 698) //Platinum Set
                guardian.Defense += 4;
            if (Head == 256 && Armor == 257 && Leggings == 258) //Ninja Set
            {
                guardian.AddFlag(GuardianFlags.NinjaSetEffect);
                guardian.TrailLength = 5;
                guardian.TrailDelay = 2;
            }
            if (Head == 3374 && Armor == 3375 && Leggings == 3376) //Fossil Set
            {
                guardian.AddFlag(GuardianFlags.FossilSetEffect);
            }
            if (Head == 2361 && Armor == 2362 && Leggings == 2363)
            {
                guardian.SummonDamageMultiplier += 0.1f;
            }
            if (Head == 228 && Armor == 229 && Leggings == 230)
            {
                guardian.ManaCostMult -= 0.16f;
                guardian.AddFlag(GuardianFlags.JungleSetEffect);
            }
            if (Head == 690 && Armor == 691 && Leggings == 692)
            {
                guardian.ManaCostMult -= 0.16f;
                guardian.AddFlag(GuardianFlags.AncientCobaltSetEffect);
            }
            if (Head == 123 && Armor == 124 && Leggings == 125)
            {
                guardian.AddFlag(GuardianFlags.MeteorSetEffect);
            }
            if ((Head == 959 || Head == 151) && Armor == 152 && Leggings == 153)
            {
                guardian.AddFlag(GuardianFlags.NecroSetEffect);
                guardian.TrailLength = 3;
                guardian.TrailDelay = 3;
            }
            if ((Head == 102 || Head == 956) && (Armor == 101 || Armor == 957) && (Leggings == 100 || Leggings == 958))
            {
                guardian.MoveSpeed += 0.15f;
                guardian.AddFlag(GuardianFlags.ShadowSetEffect);
            }
            if (Head == 792 && Armor == 793 && Leggings == 794)
            {
                guardian.AddFlag(GuardianFlags.CrimsonSetEffect);
            }
            if (Head == 231 && Armor == 232 && Leggings == 233)
            {
                guardian.MeleeDamageMultiplier += 0.17f;
                guardian.AddFlag(GuardianFlags.MoltenSetEffect);
            }
            if (Head == 2370 && Armor == 2371 && Leggings == 2372)
            {
                guardian.SummonDamageMultiplier += 0.12f;
            }
            if (Armor == 374 && Leggings == 375) //Cobalt Set
            {
                bool IsSet = true;
                if (Head == 371)
                {
                    guardian.ManaCostMult -= 0.14f;
                }
                else if (Head == 372)
                {
                    guardian.MeleeSpeed += 0.15f;
                }
                else if (Head == 373)
                {
                    guardian.AddFlag(GuardianFlags.CobaltSetAmmoCostReduction);
                }
                else
                {
                    IsSet = false;
                }
                if (IsSet)
                    guardian.AddFlag(GuardianFlags.ColbaltSetEffect);
                guardian.TrailLength = 5;
                guardian.TrailDelay = 2;
            }
            if ((Head == 1207 || Head == 1206 || Head == 1205) && Armor == 1208 && Leggings == 1209) //Palladium Set
            {
                guardian.AddFlag(GuardianFlags.PalladiumSetEffect);
            }
            if (Armor == 379 && Leggings == 380) //Mythril Set
            {
                if (Head == 376)
                {
                    guardian.ManaCostMult -= 0.17f;
                }
                else if (Head == 377)
                {
                    guardian.MeleeCriticalRate += 5;
                }
                else if (Head == 378)
                {
                    guardian.AddFlag(GuardianFlags.MythrilSetAmmoCostReduction);
                }
            }
            if ((Head == 1212 || Head == 1211 || Head == 1210) && Armor == 1213 && Leggings == 1214) //Orichalcum Set
            {
                guardian.AddFlag(GuardianFlags.OrichalcumSetEffect);
            }
            if (Armor == 403 && Leggings == 404) //Adamantite Set
            {
                bool IsSet = true;
                if (Head == 400)
                {
                    guardian.ManaCostMult -= 0.19f;
                }
                else if (Head == 401)
                {
                    guardian.MeleeSpeed += 0.18f;
                    guardian.MoveSpeed += 0.18f;
                }
                else if (Head == 402)
                {
                    guardian.AddFlag(GuardianFlags.AdamantiteSetAmmoCostReduction);
                }
                else
                {
                    IsSet = false;
                }
                if (IsSet)
                {
                    guardian.AddFlag(GuardianFlags.AdamantiteSetEffect);
                    guardian.PulsePower = 5;
                }
            }
            if ((Head == 1217 || Head == 1216 || Head == 1215) && Armor == 1218 && Leggings == 1219)
            {
                guardian.AddFlag(GuardianFlags.TitaniumSetEffect);
            }
            if (Head == 684 && Armor == 685 && Leggings == 686) //Frost Set
            {
                guardian.AddFlag(GuardianFlags.FrostSetEffect);
            }
            if (Head == 3776 && Armor == 3777 && Leggings == 3778) //Forbidden Set
            {
                guardian.AddFlag(GuardianFlags.ForbiddenSetEffect);
            }
            if (UsingRobe(Armor))
            {
                if (Head == 2275)
                {
                    guardian.MMP += 60;
                }
                else if (Head == 238)
                {
                    guardian.MagicCriticalRate += 10;
                }
            }
            //Implement apprentice set and ahead.
            if (Head == 3797 && Armor == 3798 && Leggings == 3799) //Apprentice Set
            {
                guardian.AddFlag(GuardianFlags.DD2ApprenticeSet);
                guardian.MaxSentries++;
            }
            if (Head == 3800 && Armor == 3801 && Leggings == 3802) //Squire Set
            {
                guardian.AddFlag(GuardianFlags.DD2SquireSet);
                guardian.MaxSentries++;
            }
            if (Head == 3803 && Armor == 3804 && Leggings == 3805) //Huntress Set
            {
                guardian.AddFlag(GuardianFlags.DD2HuntressSet);
                guardian.MaxSentries++;
            }
            if (Head == 3806 && Armor == 3807 && Leggings == 3808) //Monk Set
            {
                guardian.AddFlag(GuardianFlags.DD2MonkSet);
                guardian.MaxSentries++;
            }
            if (Armor == 551 && Leggings == 552) //Hallowed Set
            {
                bool IsSet = true;
                if (Head == 558)
                {
                    guardian.ManaCostMult -= 0.2f;
                }
                else if (Head == 559)
                {
                    guardian.MeleeSpeed += 0.19f;
                    guardian.MoveSpeed += 0.19f;
                }
                else if (Head == 553)
                {
                    guardian.AddFlag(GuardianFlags.HallowedAmmoReduction);
                }
                else IsSet = false;
                if (IsSet)
                {
                    guardian.AddFlag(GuardianFlags.HallowedSetEffect);
                    guardian.PulsePower = 5;
                }
            }
            if ((Head >= 1001 && Head <= 1003) && Armor == 1004 && Leggings == 1005) //Chlorophyte Set
            {
                guardian.AddFlag(GuardianFlags.ChlorophyteSetEffect);
            }
            if (Head == 1316 && Armor == 1317 && Leggings == 1318) //Turtle Set
            {
                guardian.AddFlag(GuardianFlags.TurtleSetEffect);
            }
            if (Head == 1159 && Armor == 1160 && Leggings == 1161) //Tiki set
            {
                guardian.MaxMinions++;
            }
            if (Head == 1832 && Armor == 1833 && Leggings == 1834) //Spooky Set
            {
                guardian.SummonDamageMultiplier += 0.25f;
            }
            if ((Head == 1546 || Head == 1548 || Head == 1547) && Armor == 1549 && Leggings == 1550) //Shroomite Set
            {
                guardian.AddFlag(GuardianFlags.ShroomiteSetEffect);
            }
            if (Armor == 1504 && Leggings == 1505) //Spectre Set
            {
                if (Head == 2189)
                {
                    guardian.AddFlag(GuardianFlags.SpectreSplashSetEffect);
                }
                if (Head == 1503)
                {
                    guardian.AddFlag(GuardianFlags.SpectreHealSetEffect);
                }
            }
            if (Head == 2199 && Leggings == 2202) //Beetle Set
            {
                if (Armor == 2200)
                {
                    guardian.AddFlag(GuardianFlags.BeetleOffenseEffect);
                }
                else if (Armor == 2201)
                {
                    guardian.AddFlag(GuardianFlags.BeetleDefenseEffect);
                }
            }
            if (Head == 3874 && Armor == 3875 && Leggings == 3876) //Dark Artist Set
            {
                guardian.AddFlag(GuardianFlags.DD2DarkArtistEffect);
                guardian.MaxSentries++;
            }
            if (Head == 3880 && Armor == 3881 && Leggings == 3882) //Shinobi Infiltrator Set
            {
                guardian.AddFlag(GuardianFlags.DD2ShinobiInfiltratorEffect);
                guardian.MaxSentries++;
            }
            if (Head == 3877 && Armor == 3878 && Leggings == 3879) //Red Riding Set
            {
                guardian.AddFlag(GuardianFlags.DD2RedRidingEffect);
                guardian.MaxSentries++;
            }
            if (Head == 3871 && Armor == 3872 && Leggings == 3873) //Valhalla Knight Set
            {
                guardian.AddFlag(GuardianFlags.DD2ValhallaKnightEffect);
                guardian.MaxSentries++;
            }
            if (Head == 2763 && Armor == 2764 && Leggings == 2765) //Solar Flare Set
            {
                guardian.AddFlag(GuardianFlags.SolarFlareSetEffect);
                guardian.PulsePower = 5;
            }
            if (Head == 2757 && Armor == 2758 && Leggings == 2759) //Vortex Set
            {
                guardian.AddFlag(GuardianFlags.VortexSetEffect);
            }
            if (Head == 2760 && Armor == 2761 && Leggings == 2762) //Nebula Set
            {
                guardian.AddFlag(GuardianFlags.NebulaSetEffect);
            }
            if (Head == 3381 && Armor == 3382 && Leggings == 3383) //Stardust Set
            {
                guardian.AddFlag(GuardianFlags.StardustSetEffect);
            }
        }

        public static void DoPrefixEffect(Item item, TerraGuardian guardian)
        {
            switch (item.prefix)
            {
                case 62:
                    guardian.Defense++;
                    break;
                case 63:
                    guardian.Defense += 2;
                    break;
                case 64:
                    guardian.Defense += 3;
                    break;
                case 65:
                    guardian.Defense += 4;
                    break;
                case 66:
                    guardian.MMP += 20;
                    break;
                case 67:
                    guardian.MeleeCriticalRate += 2;
                    guardian.RangedCriticalRate += 2;
                    guardian.MagicCriticalRate += 2;
                    break;
                case 68:
                    guardian.MeleeCriticalRate += 4;
                    guardian.RangedCriticalRate += 4;
                    guardian.MagicCriticalRate += 4;
                    break;
                case 69:
                    guardian.MeleeDamageMultiplier += 0.01f;
                    guardian.RangedDamageMultiplier += 0.01f;
                    guardian.MagicDamageMultiplier += 0.01f;
                    guardian.SummonDamageMultiplier += 0.01f;
                    break;
                case 70:
                    guardian.MeleeDamageMultiplier += 0.02f;
                    guardian.RangedDamageMultiplier += 0.02f;
                    guardian.MagicDamageMultiplier += 0.02f;
                    guardian.SummonDamageMultiplier += 0.02f;
                    break;
                case 71:
                    guardian.MeleeDamageMultiplier += 0.03f;
                    guardian.RangedDamageMultiplier += 0.03f;
                    guardian.MagicDamageMultiplier += 0.03f;
                    guardian.SummonDamageMultiplier += 0.03f;
                    break;
                case 72:
                    guardian.MeleeDamageMultiplier += 0.04f;
                    guardian.RangedDamageMultiplier += 0.04f;
                    guardian.MagicDamageMultiplier += 0.04f;
                    guardian.SummonDamageMultiplier += 0.04f;
                    break;
                case 73:
                    guardian.MoveSpeed += 0.01f;
                    break;
                case 74:
                    guardian.MoveSpeed += 0.02f;
                    break;
                case 75:
                    guardian.MoveSpeed += 0.03f;
                    break;
                case 76:
                    guardian.MoveSpeed += 0.04f;
                    break;
                case 77:
                    guardian.MeleeSpeed += 0.01f;
                    break;
                case 78:
                    guardian.MeleeSpeed += 0.02f;
                    break;
                case 79:
                    guardian.MeleeSpeed += 0.03f;
                    break;
                case 80:
                    guardian.MeleeSpeed += 0.04f;
                    break;
            }
        }

        public static bool UsingRobe(int Armor)
        {
            return (Armor >= 1282 && Armor <= 1287) || Armor == 2279;
        }
    }
}
