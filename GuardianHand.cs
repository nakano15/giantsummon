using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianHand
    {
        public GuardianHand(HeldHand hand)
        {
            HandType = hand;
        }

        public int SelectedItem = -1;
        public int ItemPositionX = 0, ItemPositionY = 0;
        public int ItemAnimationTime = 0, ItemMaxAnimationTime = 0, ItemUseTime = 0, ItemMaxUseTime = 0, ToolUseTime = 0;
        public int ArmAnimationFrame = 0;
        public float ItemRotation = 0f, ItemScale = 1f;
        public byte ArmOrientation = 0;
        public bool IsDelay = false;
        public TerraGuardian.ItemUseTypes UseType = TerraGuardian.ItemUseTypes.HeavyVerticalSwing;
        public HeldHand HandType = HeldHand.Left;
    }
}
