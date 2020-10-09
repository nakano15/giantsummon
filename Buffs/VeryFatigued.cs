using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Buffs
{
    public class VeryFatigued : GuardianModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Very Tired");
            Description.SetDefault("Guardian needs rest. \nPerformance lower. \nMay fall asleep sometimes.");
            Main.debuff[Type] = Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[this.Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(TerraGuardian guardian)
        {
            guardian.MeleeDamageMultiplier -= 0.3f;
            guardian.RangedDamageMultiplier -= 0.3f;
            guardian.MagicDamageMultiplier -= 0.3f;
            guardian.SummonDamageMultiplier -= 0.3f;
            guardian.MHP -= (int)(guardian.MHP * 0.3f);
            guardian.MMP -= (int)(guardian.MMP * 0.3f);
            guardian.MoveSpeed -= 0.3f;
        }
    }
}
