using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.GuardianNPC.Terrarians
{
    [AutoloadHead]
    public class MichelleGuardian : GuardianNPCPrefab
    {
        public override string HeadTexture
        {
            get
            {
                return "giantsummon/GuardianNPC/Terrarians/Michelle_Head";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "TerrarianAdventurer";
            return mod.Properties.Autoload;
        }

        public MichelleGuardian()
            : base(GuardianBase.Michelle)
        {

        }
    }
}
