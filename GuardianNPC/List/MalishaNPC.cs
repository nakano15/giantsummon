using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.List
{
    [AutoloadHead]
    public class PantherGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/List/Malisha_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "PantherGuardian";
            return base.Autoload(ref name);
        }

        public PantherGuardian()
            : base(GuardianBase.Malisha)
        {

        }
    }
}
