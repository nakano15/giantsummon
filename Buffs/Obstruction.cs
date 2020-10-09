using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Buffs
{
    public class Obstruction : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Obstruction");
            Description.SetDefault("Greatly lowers the player visibility.\nMakes the player literally intangible, guardian takes all attacks.\nPlayer can't attack.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.headcovered = true;
            player.noItems = true;
        }
    }
}
