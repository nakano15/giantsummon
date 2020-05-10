using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace giantsummon.Items.Consumable
{
    public class FirstAidKit : GuardianItemPrefab
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Cures Injury on TerraGuardians.");
        }

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item4;
            item.useStyle = 4;
            item.useAnimation = 30;
            item.useTime = 30;
            item.maxStack = 20;
            item.consumable = true;
            item.value = Item.buyPrice(0, 0, 60);
            item.width = 32;
            item.height = 32;
            ItemOrigin = new Vector2(14, 18);
            item.rare = 2;
        }

        public override bool GuardianCanUse(TerraGuardian guardian)
        {
            return !guardian.HasBuff(ModContent.BuffType<Buffs.FirstAidCooldown>()) && guardian.Data.Injury > 0;
        }

        public override void WhenGuardianUses(TerraGuardian guardian)
        {
            guardian.Data.Injury -= 16;
            guardian.AddBuff(ModContent.BuffType<Buffs.FirstAidCooldown>(), 8 * 3600);
        }
    }
}
