using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs.GhostFoxHaunts
{
    public class ConstructHaunt : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Haunted");
            Description.SetDefault("A Ghost Guardian haunts you.\nHaunt is lifted if you defeat what killed It.\nYou sees flashes of her being squished by a giant construct with yellow eyes.");
            Main.debuff[this.Type] = true;
            Main.persistentBuff[this.Type] = true; //true
            Main.buffNoTimeDisplay[this.Type] = true;
            Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            this.canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 60;
            if (player.GetModPlayer<PlayerMod>().KnockedOut)
            {
                player.GetModPlayer<PlayerMod>().ReviveBoost += 1;
            }
        }
    }
}
