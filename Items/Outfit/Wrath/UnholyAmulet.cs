using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Outfit.Wrath
{
    class UnholyAmulet : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("It creeps you out, and its eyes seems to be staring at your soul.\nAllows changing Wrath skin when he possesses it.");
        }

        public override void SetDefaults()
        {
            item.value = Item.buyPrice(0, 5);
            item.maxStack = 1;
            item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
