using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class TwoHandedMastery : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows guardian to wield a copy of their weapon in their hands.");  //The (English) text shown below your weapon's name
		}

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 24;            //Weapon's texture's width
            item.height = 24;           //Weapon's texture's height
            item.value = Item.buyPrice(0, 25);           //The value of the weapon
            item.rare = Terraria.ID.ItemRarityID.LightPurple;              //The rarity of the weapon, from -1 to 13
            ItemStatusScript = (delegate(TerraGuardian Guardian)
            {
                Guardian.AddFlag(GuardianFlags.CanDualWield);
            });
        }

	}
}
