using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class ItemMod : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.Active)
            {
                TerraGuardian guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
                if (item.healLife > 0 && MainMod.IsGuardianItem(item))
                {
                    tooltips.Insert(1, new TooltipLine(this.mod, "GuardianHealingValue", "Restores " + (int)(item.healLife * guardian.HealthHealMult) + " Guardian Health."));
                }
            }
            GuardianPlayerAccessoryEffects.CheckForAccessoryDescriptionChanges(item, tooltips);
        }

        public override bool OnPickup(Item item, Player player)
        {
            if (player.GetModPlayer<PlayerMod>().Guardian.Active)
            {
                if (item.type == Terraria.ID.ItemID.Heart || item.type == Terraria.ID.ItemID.CandyApple || item.type == Terraria.ID.ItemID.CandyCane)
                {
                    player.GetModPlayer<PlayerMod>().ShareHealthReplenishWithGuardians(20);
                }
                if (item.type == Terraria.ID.ItemID.Star || item.type == Terraria.ID.ItemID.SoulCake || item.type == Terraria.ID.ItemID.SugarPlum)
                {
                    player.GetModPlayer<PlayerMod>().ShareManaReplenishWithGuardians(100);
                }
            }
            return base.OnPickup(item, player);
        }
    }
}
