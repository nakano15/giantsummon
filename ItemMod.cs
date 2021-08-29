using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class ItemMod : GlobalItem
    {
        private static List<int> LastItemsSpawned = new List<int>(),
            NewItemsSpawned = new List<int>();

        public static void RefreshItemsSpawnedLists()
        {
            LastItemsSpawned = NewItemsSpawned;
            NewItemsSpawned = new List<int>();
        }

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
            GuardianAccessoryEffects.CheckForAccessoryDescriptionChanges(item, tooltips);
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.active && Main.netMode < 2)
            {
                NewItemsSpawned.Add(item.whoAmI);
                if (!LastItemsSpawned.Contains(item.whoAmI))
                {
                    Player player = Main.player[Main.myPlayer];
                    if(!item.vanity && (item.damage > 0 || item.defense > 0 || item.accessory) && item.rare > Terraria.ID.ItemRarityID.Green)
                    {
                        if (Math.Abs(player.Center.X - item.Center.X) < Main.screenWidth * 0.5f &&
                            Math.Abs(player.Center.Y - item.Center.Y) < Main.screenHeight * 0.5f)
                        {
                            player.GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.SpotsRareTreasure);
                        }
                    }
                }
            }
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
            if(Main.npcShop == -1 && item.rare > Terraria.ID.ItemRarityID.Green)
            {
                GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.FoundSomethingGood,
                    player.name, item.Name, 8, item.rare,
                    GuardianGlobalInfos.GetGuardiansInTheWorld());
            }
            return base.OnPickup(item, player);
        }
    }
}
