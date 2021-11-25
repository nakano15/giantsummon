using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianBaseContainer
    {
        public string modid;
        private Dictionary<int, GuardianBase> GuardianList = new Dictionary<int, GuardianBase>();

        public int GetGuardianCount { get { return GuardianList.Count; } }

        public GuardianBaseContainer(string mod)
        {
            this.modid = mod;
        }

        public GuardianBase GetGuardian(int ID)
        {
            if (!GuardianList.ContainsKey(ID))
            {
                AddGuardian(ID);
            }
            return GuardianList[ID];
        }

        public bool GuardianExists(int ID)
        {
            return GuardianList.ContainsKey(ID);
        }

        public void AddGuardian(int ID)
        {
            Mod mod = ModLoader.GetMod(modid);
            GuardianBase gd = GuardianBase.GuardianDB(ID, mod);
            GuardianList.Add(ID, gd);
            gd.SetupShop(ID, modid);
        }
    }
}
