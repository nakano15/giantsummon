using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class DeerGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Mabel_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "DeerGuardian";
            return mod.Properties.Autoload;
        }

        public DeerGuardian()
            : base(8)
        {

        }
    }
}
