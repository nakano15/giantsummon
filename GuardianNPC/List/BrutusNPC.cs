using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class LionGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Brutus_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "LionGuardian";
            return mod.Properties.Autoload;
        }

        public LionGuardian()
            : base(6)
        {

        }
    }
}
