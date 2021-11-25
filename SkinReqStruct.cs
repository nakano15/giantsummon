using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public struct SkinReqStruct
    {
        public delegate bool SkinRequirementDel(GuardianData gd, Player player);
        public byte SkinID;
        public string Name;
        public SkinRequirementDel Requirement;
        public bool SkinUsesHead;

        public SkinReqStruct(byte SkinID, string Name, SkinRequirementDel ReqScript)
        {
            this.SkinID = SkinID;
            this.Name = Name;
            this.Requirement = ReqScript;
            SkinUsesHead = false;
        }

        public SkinReqStruct(byte SkinID, string Name, SkinRequirementDel ReqScript, bool SkinUsesHead)
        {
            this.SkinID = SkinID;
            this.Name = Name;
            this.Requirement = ReqScript;
            this.SkinUsesHead = SkinUsesHead;
        }
    }
}
