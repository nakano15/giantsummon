using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Accessories
{
    public class BlueHairclip : GuardianItemPrefab
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("One of Blue's hair clips, she gave you as a token of friendship.\nCritical Rate increases by 5%.");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.width = 24;            //Weapon's texture's width
            item.height = 24;           //Weapon's texture's height
            item.value = Item.buyPrice(0, 1, 50);           //The value of the weapon
            item.rare = 7;              //The rarity of the weapon, from -1 to 13
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 5;
            player.rangedCrit += 5;
            player.magicCrit += 5;
        }

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.MeleeCriticalRate += 5;
            g.RangedCriticalRate += 5;
            g.MagicCriticalRate += 5;
        }
    }
}
