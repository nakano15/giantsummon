using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Compatibility
{
    public struct GuardianIdentity //Used to locate the refered guardian.
    {
        public GuardianID ID;
        public int Owner;

        public GuardianIdentity(TerraGuardian guardian)
        {
            Owner = guardian.OwnerPos;
            ID = guardian.MyID;
        }

        public bool IsSame(TerraGuardian guardian)
        {
            return guardian.OwnerPos == Owner && ID.IsSameID(guardian);
        }
    }
}
