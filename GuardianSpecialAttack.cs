using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianSpecialAttack
    {
        public List<GuardianSpecialAttackFrame> SpecialAttackFrames = new List<GuardianSpecialAttackFrame>();
        public SubAttackCombatType combatType = SubAttackCombatType.Melee;
        public bool CanMove = true;
        public float MinRange = 10, MaxRange = 400;
        public int Cooldown = 0;
        public int ManaCost = 0;
        public Action<TerraGuardian, int> WhenFrameBeginsScript = delegate (TerraGuardian tg, int FrameID) { };
        public Action<TerraGuardian, int, int> WhenFrameUpdatesScript = delegate (TerraGuardian tg, int FrameID, int FrameTime) { },
            WhenCompanionIsBeingDrawn = delegate(TerraGuardian tg, int FrameID, int FrameTime) { };
        public Action<TerraGuardian> WhenSubAttackBegins = delegate (TerraGuardian tg)
        {

        };
        public delegate void AnimationReplaceDel(TerraGuardian tg, int FrameID, int FrameTime, ref int BodyFrame, ref int LeftArmFrame, ref int RightArmFrame);
        public AnimationReplaceDel AnimationReplacer = delegate (TerraGuardian tg, int FrameID, int FrameTime, ref int BodyFrame, ref int LeftArmFrame, ref int RightArmFrame)
        {

        };
        public Func<TerraGuardian, int> CalculateAttackDamage = delegate (TerraGuardian tg) { return 0; };
        public GuardianSpecialAttack(SubAttackCombatType subAttackCombatType = SubAttackCombatType.Melee)
        {
            this.combatType = subAttackCombatType;
        }

        public void SetCooldown(int s, int m = 0, int h = 0)
        {
            Cooldown = s * 60 + m * 3600 + h * (3600 * 3600);
        }

        public enum SubAttackCombatType
        {
            Melee,
            Ranged,
            Magic
        }
    }

    public class GuardianSpecialAttackFrame
    {
        public int Duration = 8;
        public int BodyFrame = 0, LeftArmFrame = 0, RightArmFrame = 0;
    }
}
