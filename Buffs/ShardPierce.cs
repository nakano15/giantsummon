using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;
using Terraria;

namespace giantsummon.Buffs
{
    public class ShardPierce : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Shard Pierce");
            Description.SetDefault("Defense pierced by shards.");
            Main.debuff[Type] = Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[this.Type] = false;
            longerExpertDebuff = false;
        }
    }
}
