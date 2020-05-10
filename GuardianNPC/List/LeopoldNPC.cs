using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class BunnyGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Leopold_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "BunnyGuardian";
            return mod.Properties.Autoload;
        }

        public BunnyGuardian()
            : base(10)
        {

        }
    }
}
