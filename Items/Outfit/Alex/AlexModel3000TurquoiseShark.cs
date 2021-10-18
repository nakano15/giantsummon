using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Outfit.Alex
{
    class AlexModel3000TurquoiseShark : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Unlocks Android Outfit when placed on Alex's inventory.\nMade with love.");
        }

        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 5);
            item.maxStack = 1;
            item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}
