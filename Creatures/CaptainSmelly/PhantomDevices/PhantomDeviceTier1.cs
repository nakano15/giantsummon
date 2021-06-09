using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace giantsummon.Creatures.CaptainSmelly.PhantomDevices
{
    public class PhantomDeviceTier1 : Items.GuardianItemPrefab
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can be used 1 time by Captain Smelly.");
        }

        public override void SetDefaults()
        {
            item.value = 100;
            item.stack = 1;
            item.accessory = true;
        }

        public override void ItemStatusScript(TerraGuardian g)
        {
            if(g.Data is CaptainSmellyBase.CaptainSmellyData)
            {
                CaptainSmellyBase.CaptainSmellyData data = (CaptainSmellyBase.CaptainSmellyData)g.Data;
                const int DID = 1;
                if(data.DeviceID < DID)
                    data.DeviceID = DID;
            }
        }
    }
}
