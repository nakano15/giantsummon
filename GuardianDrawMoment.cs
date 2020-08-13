using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public struct GuardianDrawMoment //Used to tell when the companion should be drawn. The TerraGuardian object has methods that aid on making use of this.
    {
        public TerraGuardian.TargetTypes DrawTargetType;
        public int DrawTargetID;
        public int GuardianWhoAmID;

        public GuardianDrawMoment(int GuardianWhoAmID, TerraGuardian.TargetTypes TargetType, int TargetID)
        {
            this.DrawTargetType = TargetType;
            this.DrawTargetID = TargetID;
            this.GuardianWhoAmID = GuardianWhoAmID;
        }
    }
}
