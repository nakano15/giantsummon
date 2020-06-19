using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class WolfGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Blue_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "WolfGuardian";
            return mod.Properties.Autoload;
        }

        public WolfGuardian()
            : base(1)
        {

        }
    }
}
