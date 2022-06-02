namespace giantsummon.Companions.Creatures.Castella
{
    public class CastellaMetamorphosis : GuardianActions
    {
        private bool WereTransform = false;

        public CastellaMetamorphosis()
        {
            Forced = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            if(Step > 0)
            {
                guardian.MoveLeft = guardian.MoveRight = guardian.MoveUp = guardian.MoveDown = guardian.Jump = false;
                IgnoreCombat = true;
                CantUseInventory = true;
            }
            switch (Step)
            {
                case 0:
                    if (guardian.SittingOnPlayerMount)
                    {
                        guardian.DoSitOnPlayerMount(false);
                    }
                    if (guardian.Velocity.Y == 0)
                    {
                        WereTransform = guardian.HasFlag(GuardianFlags.Werewolf);
                        ChangeStep();
                    }
                    break;
                case 1:
                    if (Time >= 11)
                        ChangeStep();
                    break;
                case 2:
                case 3:
                case 4:
                    if (Time >= 30)
                    {
                        if (Step == 2)
                            guardian.UpdateStatus = true;
                        ChangeStep();
                    }
                    break;
                case 5:
                    if (Time >= 15)
                        InUse = false;
                    break;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            int Frame = 0;
            switch (Step)
            {
                default:
                    if(guardian.Velocity.Y != 0)
                        Frame = WereTransform ? 9 : 38;
                    else
                        Frame = WereTransform ? 0 : 29;
                    break;
                case 1:
                    Frame = WereTransform ? 55 : 59;
                    break;
                case 2:
                    Frame = WereTransform ? 56 : 58;
                    break;
                case 3:
                    Frame = 57;
                    break;
                case 4:
                    Frame = WereTransform ? 58 : 56;
                    break;
                case 5:
                    Frame = WereTransform ? 59 : 55;
                    break;
            }
            UsingLeftArmAnimation = UsingRightArmAnimation = true;
            guardian.BodyAnimationFrame = guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = Frame;
        }
    }
}
