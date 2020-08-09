using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianID
    {
        public int ID = 0;
        public string ModID = "";

        public GuardianID()
        {

        }

        public GuardianID(int ID, string ModID = "")
        {
            this.ID = ID;
            if (ModID == "")
                ModID = MainMod.mod.Name;
            this.ModID = ModID;
        }

        public bool IsSameID(TerraGuardian g)
        {
            return IsSameID(g.Data);
        }

        public bool IsSameID(GuardianData gd)
        {
            return gd.ID == ID && gd.ModID == ModID;
        }

        public bool IsSameID(GuardianID g)
        {
            return g.ID == ID && g.ModID == ModID;
        }
    }
}
