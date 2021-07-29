using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class SkillResetPotion : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Resets your guardian skills.\nGains a 2x skill leveling up if the total level is under previous level.");  //The (English) text shown below your weapon's name
		}

        public override void WhenGuardianUses(TerraGuardian guardian)
        {
            guardian.Data.ResetSkillsProgress();
        }

        public override bool GuardianCanUseItem(TerraGuardian guardian)
        {
            return true;
        }

		public override void SetDefaults()
        {
            item.UseSound = SoundID.Item3;
            item.useStyle = 2;
            item.useTurn = true;
            item.useAnimation = 60;
            item.useTime = 60;
            item.maxStack = 50;
            item.consumable = true;
            item.potion = true;
            item.value = Item.buyPrice(0, 1);
			item.width = 22;
			item.height = 22;
            ItemOrigin = new Vector2(11, 13);
		}
	}
}
