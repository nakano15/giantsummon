using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Compatibility
{
    public class CompatibilityHooks
    {
        public static bool GetModItemDamage(TerraGuardian tg, int Item, ref float DamageMult)
        {
            if (ThoriumCompatibility.IsModActive)
            {

            }
            return false;
        }
    }
}
