using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Fit : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Fit");
            Description.SetDefault("You are feeling healthier.");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            const float DamageBonus = 0.08f;
            player.meleeDamage += DamageBonus;
            player.rangedDamage += DamageBonus;
            player.magicDamage += DamageBonus;
            player.statLifeMax2 += 40;
            player.moveSpeed += 0.1f;
            player.jumpSpeedBoost += 0.06f;
        }
    }
}
