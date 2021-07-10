using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Npcs
{
    public class GlennNPC : GuardianActorNPC
    {
        public GlennNPC() : base(GuardianBase.Glenn, "", "Black and White Cat")
        {

        }

        public override void AI()
        {
            Idle = true;
            base.AI();
        }
    }
}
