using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Items.Consumable
{
    public class PortraitOfAFriend : ModItem
    {

        public override void SetDefaults()
        {
            item.UseSound = Terraria.ID.SoundID.Item4;
            item.useStyle = 4;
            item.useAnimation = 30;
            item.useTime = 30;
            item.maxStack = 1;
            item.consumable = true;
            item.value = 0;
            item.width = 24;            //Weapon's texture's width
            item.height = 16;           //Weapon's texture's height
            item.rare = 7;
        }

        public override void UpdateInventory(Player player)
        {
            if (player.whoAmI == Main.myPlayer && Main.ActivePlayerFileData.GetPlayTime() >= TimeSpan.FromMinutes(12))
            {
                item.SetDefaults(0);
                Main.NewText("Portrait of a Friend has suddenly disappeared from your inventory.");
            }
        }

        public override bool CanUseItem(Player player)
        {
            //item.stack++;
            if (!BuddyModeSetupInterface.WindowActive)
                BuddyModeSetupInterface.Open();
            return true;
        }

        public override void OnConsumeItem(Player player)
        {
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            while (tooltips.Count > 1)
                tooltips.RemoveAt(1);
            tooltips.Add(new TooltipLine(mod, "bmln0", "This is the portrait of some dear or important for you."));
            tooltips.Add(new TooltipLine(mod, "bmln1", "Using It to open the interface of starting Buddies Mode."));
            tooltips.Add(new TooltipLine(mod, "bmln1", "You have 12 in-game minutes to use this item."));
        }
    }
}
