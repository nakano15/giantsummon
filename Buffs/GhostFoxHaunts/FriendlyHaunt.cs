using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs.GhostFoxHaunts
{
    public class FriendlyHaunt : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Haunted");
            Description.SetDefault("This time she just wants to rest on your shoulder.");
            Main.debuff[this.Type] = false;
            Main.persistentBuff[this.Type] = false; //true
            Main.buffNoTimeDisplay[this.Type] = true;
            Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            this.canBeCleared = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<PlayerMod>().KnockedOut)
            {
                player.GetModPlayer<PlayerMod>().ReviveBoost += 1;
            }
        }

        public override void Update(TerraGuardian guardian)
        {
            if (guardian.KnockedOut)
            {
                guardian.ReviveBoost++;
            }
        }
    }
}
