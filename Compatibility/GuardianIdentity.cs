using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Compatibility
{
    public struct GuardianIdentity //Used to locate the reffered guardian.
    {
        public int Owner, ID;
        public string ModID;

        public GuardianIdentity(TerraGuardian guardian)
        {
            Owner = guardian.OwnerPos;
            ID = guardian.ID;
            ModID = guardian.ModID;
        }

        public bool IsSame(TerraGuardian guardian)
        {
            return guardian.OwnerPos == Owner && guardian.ID == ID && guardian.ModID == ModID;
        }
    }
}
