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
            ItemStatusScript = (delegate(TerraGuardian Guardian)
            {
                if (!Guardian.Base.IsTerraGuardian)
                    return;
                Guardian.ScaleMult *= 3;
                Guardian.MHP *= 3;
                Guardian.MeleeDamageMultiplier += Guardian.MeleeDamageMultiplier * 0.3f;
                Guardian.RangedDamageMultiplier += Guardian.RangedDamageMultiplier * 0.3f;
                Guardian.MagicDamageMultiplier += Guardian.MagicDamageMultiplier * 0.3f;
                Guardian.SummonDamageMultiplier += Guardian.SummonDamageMultiplier * 0.3f;
                Guardian.Defense += (int)(Guardian.Defense * 0.3f);
                Guardian.DefenseRate += Guardian.DefenseRate * 0.3f;
                Guardian.MeleeSpeed -= Guardian.MeleeSpeed * 0.3f;
                //Guardian.MoveSpeed -= Guardian.MoveSpeed * 0.3f;
                Guardian.AddFlag(GuardianFlags.TitanGuardian);
            });
		}
	}
}
