using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class GiantDogGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Alex_Head"; //Necessary
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "GiantDogGuardian";
            return mod.Properties.Autoload;
        }
        
        public GiantDogGuardian()
            : base(5)
        {

        }
    }
}
