using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class PackLeaderNecklace : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Said to give titan powers to whoever uses it.\nBut only has effect on TerraGuardians.\nIt's not possible to have multiple guardians when having one using this.\n\"The head shown in the necklace, is it of an existing guardian?\"");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            item.accessory = true;
			item.width = 52;
			item.height = 52;
            item.value = Item.sellPrice(0, 12);           //The value of the weapon
			item.rare = Terraria.ID.ItemRarityID.Orange;              //The rarity of the weapon, from -1 to 13
		}

        public override void ItemStatusScript(TerraGuardian g)
        {
            if (!g.Base.IsTerraGuardian)
                return;
            g.ScaleMult *= 3;
            g.MHP = (int)(g.MHP * 1.8f);
            g.MeleeDamageMultiplier += g.MeleeDamageMultiplier * 0.2f;
            g.RangedDamageMultiplier += g.RangedDamageMultiplier * 0.2f;
            g.MagicDamageMultiplier += g.MagicDamageMultiplier * 0.2f;
            g.SummonDamageMultiplier += g.SummonDamageMultiplier * 0.2f;
            g.Defense += (int)(g.Defense * 0.2f);
            //Guardian.DefenseRate += Guardian.DefenseRate * 0.2f;
            g.MeleeSpeed += g.MeleeSpeed * 0.2f;
            g.MoveSpeed -= g.MoveSpeed * 0.1f;
            g.Aggro += 300;
            g.AddFlag(GuardianFlags.TitanGuardian);
        }
    }
}
