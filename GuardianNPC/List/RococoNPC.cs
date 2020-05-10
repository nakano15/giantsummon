using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class RaccoonGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Rococo_Head"; //Necessary
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "RaccoonGuardian";
            return mod.Properties.Autoload;
        }
        
        public RaccoonGuardian()
            : base(0)
        {

        }
    }
}
