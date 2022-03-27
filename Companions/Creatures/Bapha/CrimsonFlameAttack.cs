using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon.Companions.Creatures.Bapha
{
    public class CrimsonFlameAttack : HellFlameAttack
    {
        public CrimsonFlameAttack()
        {
            Name = "Crimson Flame";
            CanMove = true;
            AttackType = SubAttackCombatType.Magic;
            ManaCost = 30;
            IsAwakenedVersion = true;
        }
    }
}
