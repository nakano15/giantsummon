using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class EtherFruit : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Permanently increases the Guardian Max Health by 5.");  //The (English) text shown below your weapon's name
		}

        public override bool GuardianCanUse(TerraGuardian guardian)
        {
            return guardian.Base.IsTerraGuardian;
        }

		public override void SetDefaults()
        {
            item.UseSound = SoundID.Item4;
            item.useStyle = 4;
            item.useAnimation = 30;
            item.useTime = 30;
            item.maxStack = 999;
            item.consumable = true;
            item.value = Item.buyPrice(0, 75);
			item.width = 18;            //Weapon's texture's width
			item.height = 18;           //Weapon's texture's height
            ItemOrigin = new Vector2(14, 18);
            item.rare = 7;
		}
	}
}
