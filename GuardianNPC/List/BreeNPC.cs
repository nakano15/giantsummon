using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class FemaleCatGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Bree_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "CatWifeGuardian";
            return mod.Properties.Autoload;
        }

        public FemaleCatGuardian()
            : base(7)
        {

        }
    }
}
