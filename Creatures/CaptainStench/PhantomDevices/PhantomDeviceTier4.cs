﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria.ModLoader;

namespace giantsummon.Creatures.CaptainStench.PhantomDevices
{
    public class PhantomDeviceTier4 : Items.GuardianItemPrefab
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows Smelly to use Phantom Blitz attack 4 times.");
        }

        public override void SetDefaults()
        {
            item.value = 100;
            item.stack = 1;
            item.accessory = true;
            item.scale = 1.25f;
        }

        public override void ItemStatusScript(TerraGuardian g)
        {
            g.MMP += 60;
            if (g.Data is CaptainStenchBase.CaptainStenchData)
            {
                CaptainStenchBase.CaptainStenchData data = (CaptainStenchBase.CaptainStenchData)g.Data;
                const int DID = 4;
                if (data.DeviceID < DID)
                    data.DeviceID = DID;
            }
        }
    }
}
