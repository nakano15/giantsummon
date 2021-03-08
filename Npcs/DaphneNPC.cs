using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Npcs
{
    public class DaphneNPC : GuardianActorNPC
    {
        public byte MeetStep = 0;

        public DaphneNPC() : base(GuardianBase.Daphne, "")
        {

        }

        public override void AI()
        {
            base.AI();
        }
    }
}
