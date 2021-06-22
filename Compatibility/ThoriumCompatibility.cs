using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Compatibility
{
    public class ThoriumCompatibility
    {
        public static bool IsModActive { get { return MainMod.ThoriumMod != null; } }
        public const string SymphonicDamageType = "tho_bard", HealerDamageType = "tho_heal", ThrowerDamageType = "tho_thrw";

        public static bool GetDamageValue(Item i, out float DamagePercentage)
        {
            DamagePercentage = 1f;
            if(i.modItem != null && i.modItem.mod == MainMod.ThoriumMod)
            {
                //How to get the damage type of the item?
            }
            return false;
        }
    }
}
