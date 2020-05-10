using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class WellBeing : GuardianModBuff
    {
        const float DamageIncrease = 0.04f;
        const int DefenseIncrease = 6;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Well Being");
            Description.SetDefault("Bonus to many status.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += DamageIncrease;
            player.rangedDamage += DamageIncrease;
            player.magicDamage += DamageIncrease;
            player.minionDamage += DamageIncrease;
            player.statDefense += DefenseIncrease;
            player.lifeRegen++;
            player.manaRegen++;
            player.manaCost -= 0.08f;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MeleeDamageMultiplier += DamageIncrease;
            guardian.RangedDamageMultiplier += DamageIncrease;
            guardian.MagicDamageMultiplier += DamageIncrease;
            guardian.SummonDamageMultiplier += DamageIncrease;
            guardian.Defense += DefenseIncrease;
            guardian.ManaCostMult -= 0.08f;
        }
    }
}
