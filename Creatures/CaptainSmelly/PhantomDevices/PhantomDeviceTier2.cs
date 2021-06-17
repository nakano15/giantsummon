using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace giantsummon.Creatures.CaptainSmelly.PhantomDevices
{
    public class PhantomDeviceTier2 : Items.GuardianItemPrefab
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows Smelly to use Phantom Blitz attack 2 times.");
        }

        public override void SetDefaults()
        {
            item.value = 100;
            item.stack = 1;
            item.accessory = true;
        }

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.MMP += 20;
            if (g.Data is CaptainSmellyBase.CaptainSmellyData)
            {
                CaptainSmellyBase.CaptainSmellyData data = (CaptainSmellyBase.CaptainSmellyData)g.Data;
                const int DID = 2;
                if (data.DeviceID < DID)
                    data.DeviceID = DID;
            }
        }
    }
}
