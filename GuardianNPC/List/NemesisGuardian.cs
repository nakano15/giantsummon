using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class ArmorWraith : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Nemesis_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "ArmorWraith";
            return mod.Properties.Autoload;
        }

        public ArmorWraith()
            : base(4)
        {

        }
    }
}
