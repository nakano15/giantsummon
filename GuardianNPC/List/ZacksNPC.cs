using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class ZombieWolfGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Zacks_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "ZombieWolfGuardian";
            return mod.Properties.Autoload;
        }

        public ZombieWolfGuardian()
            : base(3)
        {

        }
    }
}
