using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Outfit.Bree
{
    class WitchOutfit : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows Bree to use a Witch outfit when in the inventory.\nIs so complete that even comes with make up.");
        }

        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 5);
            item.maxStack = 1;
            item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}
