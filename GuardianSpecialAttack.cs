using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianSpecialAttack
    {
        public List<GuardianSpecialAttackFrame> SpecialAttackFrames = new List<GuardianSpecialAttackFrame>();
        public bool CanMove = true;
        public Action<TerraGuardian, int, int> WhenFrameBeginsScript = delegate (TerraGuardian tg, int FrameID, int FrameTime) { }
        , WhenFrameUpdatesScript = delegate (TerraGuardian tg, int FrameID, int FrameTime) { };
    }

    public class GuardianSpecialAttackFrame
    {
        public int Duration = 8;
        public int BodyFrame = 0, LeftArmFrame = 0, RightArmFrame = 0;
    }
}
