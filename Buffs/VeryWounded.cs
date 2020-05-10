using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class VeryWounded : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Very Injured");
            Description.SetDefault("Guardian needs to recover from wounds. \nDefense rate dropped. \nMovement speed drops. \nPain.\n Fall Tolerance drops.");
            Main.debuff[Type] = Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.DefenseRate *= 0.66f;
            guardian.MoveSpeed -= 0.6f;
            guardian.FallHeightTolerance = (int)(guardian.FallHeightTolerance * 0.66f);
        }
    }
}
