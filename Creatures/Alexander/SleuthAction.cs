using System;
using Terraria;

namespace giantsummon.Creatures.Alexander
{
    public class SleuthAction : GuardianActions
    {
        public float SleuthPercent = 0;
        public bool Sleuthing = false;
        public TerraGuardian Target;
        public SleuthAction(TerraGuardian target)
        {
            Target = target;
        }

        public override void Update(TerraGuardian guardian)
        {
            if ((guardian.Position - Target.Position).Length() < 20)
                guardian.WalkMode = true;
            if (guardian.UsingFurniture)
                guardian.LeaveFurniture(true);
            guardian.StuckTimer = 0;
            if (guardian.BeingPulledByPlayer)
            {
                guardian.SaySomething("*Alright, I'm coming, I'm coming.*");
                InUse = false;
                return;
            }
            guardian.MoveLeft = guardian.MoveRight = false;
            Sleuthing = false;
            if (!Target.KnockedOut && !Target.IsSleeping)
            {
                if (SleuthPercent > 70)
                    guardian.SaySomething("*...So close...*");
                else
                    guardian.SaySomething(Target.GetMessage(GuardianBase.MessageIDs.AlexanderSleuthingFail, "*I... Was just checking if you were fine.*"));
                InUse = false;
                return;
            }
            if (Target.Downed)
            {
                InUse = false;
                guardian.SaySomething("*...I should have helped instead...*");
                return;
            }
            if (Math.Abs(guardian.Position.X - Target.Position.X) < 8f)
            {
                if (guardian.Velocity.X == 0 && guardian.Velocity.Y == 0)
                {
                    Sleuthing = true;
                    guardian.LookingLeft = (Target.Position.X < guardian.Position.X);
                    float LastSleuthPercent = SleuthPercent;
                    float FillSpeed = guardian.IsSleeping ? 0.07f : 0.2f;
                    SleuthPercent += Main.rand.NextFloat() * FillSpeed;
                    if (SleuthPercent >= 100)
                    {
                        AlexanderBase.AlexanderData data = (AlexanderBase.AlexanderData)guardian.Data;
                        data.AddIdentifiedGuardian(Target.MyID);
                        InUse = false;
                        guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Target.GetMessage(GuardianBase.MessageIDs.AlexanderSleuthingProgressFinished, "*Okay, so that's how you work.*"), guardian));
                        guardian.UpdateStatus = true;
                    }
                    else if (SleuthPercent >= 70 && LastSleuthPercent < 70)
                    {
                        guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Target.GetMessage(GuardianBase.MessageIDs.AlexanderSleuthingProgressNearlyDone, "*Hm... Interesting...*"), guardian));
                    }
                    else if (SleuthPercent >= 35 && LastSleuthPercent < 35)
                    {
                        guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Target.GetMessage(GuardianBase.MessageIDs.AlexanderSleuthingProgress, "*Uh huh...*"), guardian));
                    }
                    else if (SleuthPercent > 0 && LastSleuthPercent <= 0)
                    {
                        guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(Target.GetMessage(GuardianBase.MessageIDs.AlexanderSleuthingStart, "*Let's see how you work...*"), guardian));
                    }
                }
            }
            else
            {
                if (Target.Position.X < guardian.Position.X)
                    guardian.MoveLeft = true;
                else
                    guardian.MoveRight = true;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            if (Sleuthing)
            {
                guardian.LeftArmAnimationFrame = guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame = AlexanderBase.SleuthBackAnimationID;
            }
        }
    }
}
