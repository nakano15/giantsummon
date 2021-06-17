using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon.Creatures.Vladimir
{
    public class HugAction : GuardianActions
    {
        public int ChatTime = 0;
        public byte FriendshipPoints = 0;
        public Player Target;

        public HugAction(Player Target)
        {
            this.Target = Target;
        }

        public override void Update(TerraGuardian guardian)
        {
            ProceedIdleAIDuringDialogue = true;
            NpcCanFacePlayer = false;
            guardian.AddDrawMomentToPlayer(Target);
            const int BuffRefreshTime = 10 * 60;
            if (Time >= BuffRefreshTime && Time % BuffRefreshTime == 0)
            {
                int Stack = Time / BuffRefreshTime;
                if (Stack > 3)
                    Stack = 3;
                Target.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * Stack);
                PlayerMod pm = Target.GetModPlayer<PlayerMod>();
                if (pm.ControllingGuardian)
                {
                    pm.Guardian.AddBuff(ModContent.BuffType<Buffs.WellBeing>(), 3600 * 30 * Stack);
                }
            }
            bool End = Target.controlJump;
            //if (PlayerMod.PlayerHasGuardianSummoned(player, guardian.ID, guardian.ModID))
            //    End = true;
            if (Main.bloodMoon)
                End = false;
            if (End)
            {
                InUse = false;
                Target.Bottom = guardian.Position;
                if (guardian.BodyAnimationFrame == guardian.Base.ChairSittingFrame)
                    Target.position.Y -= guardian.SpriteHeight - guardian.Base.SittingPoint.Y * guardian.Scale;
                PlayerMod pm = Target.GetModPlayer<PlayerMod>();
                if (pm.ControllingGuardian)
                {
                    pm.Guardian.Position = guardian.Position;
                }
                guardian.SaySomething(((Creatures.VladimirBase)guardian.Base).GetEndHugMessage(guardian));
            }
            else
            {
                if (Target.mount.Active)
                    Target.mount.Dismount(Target);
                PlayerMod pm = Target.GetModPlayer<PlayerMod>();
                bool FaceBear = (guardian.BodyAnimationFrame != 20 && guardian.BodyAnimationFrame != 21) || guardian.BodyAnimationFrame == 25;
                if (pm.ControllingGuardian)
                {
                    pm.Guardian.Position = guardian.GetGuardianShoulderPosition;
                    pm.Guardian.Position.Y += guardian.Height * 0.5f;
                    pm.Guardian.Velocity.Y = -pm.Guardian.Mass;
                    pm.Guardian.FallStart = (int)pm.Guardian.Position.Y / 16;
                    if (pm.Guardian.ItemAnimationTime == 0 && !pm.Guardian.MoveLeft && !pm.Guardian.MoveRight)
                        pm.Guardian.FaceDirection((guardian.Direction * (FaceBear ? -1 : 1)) == -1);
                    pm.Guardian.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                    if (pm.Guardian.KnockedOut)
                    {
                        pm.Guardian.ReviveBoost += 3;
                    }
                    if (!Main.bloodMoon)
                    {
                        pm.Guardian.ImmuneTime = 3;
                        pm.Guardian.ImmuneNoBlink = true;
                    }
                }
                else
                {
                    Target.gfxOffY = 0;
                    Target.Center = guardian.GetGuardianShoulderPosition;
                    Target.velocity.Y = -Player.defaultGravity;
                    Target.fallStart = (int)Target.Center.Y / 16;
                    if (Target.itemAnimation == 0 && !Target.controlLeft && !Target.controlRight)
                        Target.ChangeDir(guardian.Direction * (FaceBear ? -1 : 1));
                    Target.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                    if (pm.KnockedOut)
                        pm.ReviveBoost += 3;
                    if (!Main.bloodMoon)
                    {
                        Target.immuneTime = 3;
                        Target.immuneNoBlink = true;
                    }
                }
                if ((guardian.MessageTime == 0 || Main.bloodMoon) && (Target.controlLeft || Target.controlRight || Target.controlUp || Target.controlDown || (Main.bloodMoon && Target.controlJump)))
                {
                    if (Main.bloodMoon)
                    {
                        Target.controlJump = false;
                        bool Defeated = false, Hurt = false;
                        if (pm.ControllingGuardian)
                        {
                            pm.Guardian.FriendlyDuelDefeat = true;
                            Hurt = 0 != pm.Guardian.Hurt((int)(pm.Guardian.MHP * 0.22f), guardian.Direction, false, true, " were crushed by " + guardian.Name + "'s arms.");
                            if (pm.Guardian.Downed)
                                Defeated = true;
                        }
                        else
                        {
                            Target.GetModPlayer<PlayerMod>().FriendlyDuelDefeat = true;
                            Hurt = 0 != Target.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Target.name + " were crushed by " + guardian.Name + "'s arms."), (int)(Target.statLifeMax2 * 0.22f), guardian.Direction);
                            if (Target.dead)
                                Defeated = true;
                        }
                        if (Hurt)
                        {
                            if (!Defeated)
                            {
                                if (guardian.BodyAnimationFrame == guardian.Base.ChairSittingFrame)
                                {
                                    switch (Main.rand.Next(5))
                                    {
                                        case 0:
                                            guardian.SaySomething("*I'll crush you if you move again!*");
                                            break;
                                        case 1:
                                            guardian.SaySomething("*I'll hurt you worser than those monsters would If you keep moving!*");
                                            break;
                                        case 2:
                                            guardian.SaySomething("*I can crush you with my arms or my legs, you pick!*");
                                            break;
                                        case 3:
                                            guardian.SaySomething("*Want me to turn your bones to dust?*");
                                            break;
                                        case 4:
                                            guardian.SaySomething("*You are angering me, more than this night does!*");
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (Main.rand.Next(5))
                                    {
                                        case 0:
                                            guardian.SaySomething("*Stay quiet!*");
                                            break;
                                        case 1:
                                            guardian.SaySomething("*I'll crush your bones If you continue doing that!*");
                                            break;
                                        case 2:
                                            guardian.SaySomething("*I have my arms around you, I can pull them against my body, and you wont like it!*");
                                            break;
                                        case 3:
                                            guardian.SaySomething("*Want to try that again?*");
                                            break;
                                        case 4:
                                            guardian.SaySomething("*This is what you want?*");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (Main.rand.Next(5))
                                {
                                    case 0:
                                        guardian.SaySomething("*Finally! You got quiet!*");
                                        break;
                                    case 1:
                                        guardian.SaySomething("*See what you made me do?!*");
                                        break;
                                    case 2:
                                        guardian.SaySomething("*My mood is already bad, you didn't helped either!*");
                                        break;
                                    case 3:
                                        guardian.SaySomething("*At least you stopped moving around!*");
                                        break;
                                    case 4:
                                        guardian.SaySomething("*You behave better when unconscious!*");
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        guardian.SaySomething("*Press Jump button If that's enough.*");
                    }
                }
                if (Target.whoAmI == Main.myPlayer)
                {
                    MainMod.FocusCameraPosition = guardian.CenterPosition;
                    MainMod.FocusCameraPosition.X -= Main.screenWidth * 0.5f;
                    MainMod.FocusCameraPosition.Y -= Main.screenHeight * 0.5f;
                    ChatTime--;
                    const int InitialTime = 300;
                    if (ChatTime == -1)
                    {
                        ChatTime = InitialTime;
                    }
                    else if (ChatTime < 1)
                    {
                        ChatTime = 60 * 10; // + Main.rand.Next(InitialTime)
                        FriendshipPoints++;
                        if (FriendshipPoints >= 10 + guardian.FriendshipLevel / 3)
                        {
                            FriendshipPoints = 0;
                            guardian.IncreaseFriendshipProgress(1);
                        }
                        string Message = GuardianMouseOverAndDialogueInterface.MessageParser(Main.rand.Next(10) == 0 ? guardian.Base.TalkMessage(Target, guardian) : guardian.Base.NormalMessage(Target, guardian), guardian);
                        //if (Target.talkNPC > -1 && Main.npc[Target.talkNPC].type == ModContent.NPCType<GuardianNPC.List.BearNPC>())
                        //PlayerMod pm = Target.GetModPlayer<PlayerMod>();
                        if(pm.IsTalkingToAGuardian && pm.TalkingGuardianPosition == guardian.WhoAmID)
                        {
                            GuardianMouseOverAndDialogueInterface.SetDialogue(Message);
                        }
                        else
                        {
                            //guardian.SaySomething(Message);
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArm, ref bool UsingRightArm)
        {
            if (guardian.BodyAnimationFrame == guardian.Base.BedSleepingFrame)
            {
                if (!Main.bloodMoon)
                {
                    guardian.BodyAnimationFrame++;
                    guardian.LeftArmAnimationFrame = guardian.BodyAnimationFrame;
                    guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame;
                }
            }
            else if (guardian.BodyAnimationFrame == guardian.Base.ThroneSittingFrame)
            {
                if (!Main.bloodMoon)
                {
                    guardian.BodyAnimationFrame++;
                }
                else
                {
                    if (!UsingLeftArm)
                    {
                        UsingLeftArm = true;
                        guardian.LeftArmAnimationFrame = guardian.BodyAnimationFrame++;
                    }
                    if (!UsingRightArm)
                    {
                        UsingRightArm = true;
                        guardian.RightArmAnimationFrame = guardian.BodyAnimationFrame++;
                    }
                }
            }
            else if (guardian.BodyAnimationFrame == guardian.Base.ChairSittingFrame)
            {
                if (!UsingLeftArm)
                {
                    guardian.LeftArmAnimationFrame = guardian.Base.SittingFrame;
                    UsingLeftArm = true;
                }
                if (!UsingRightArm)
                {
                    guardian.RightArmAnimationFrame = guardian.Base.SittingFrame;
                    UsingRightArm = true;
                }
            }
            else
            {
                int Frame = 1;
                if (guardian.Ducking)
                    Frame = 12;
                if (!Main.bloodMoon && (guardian.BodyAnimationFrame == guardian.Base.StandingFrame || guardian.BodyAnimationFrame == guardian.Base.DuckingFrame))
                    guardian.BodyAnimationFrame = Frame;
                if (!UsingLeftArm)
                {
                    guardian.LeftArmAnimationFrame = Frame;
                    UsingLeftArm = true;
                }
                if (!UsingRightArm)
                {
                    guardian.RightArmAnimationFrame = Frame;
                    UsingRightArm = true;
                }
            }
        }
    }
}
