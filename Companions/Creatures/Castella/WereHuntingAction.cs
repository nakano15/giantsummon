using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon.Companions.Creatures.Castella
{
    public class WereHuntingAction : GuardianActions
    {
        private TerraGuardian guardian;
        private TerraGuardian.TargetTypes VictimType = TerraGuardian.TargetTypes.Player;
        private int VictimID = -1;
        private byte DialogueFatigue = 0;

        public override void Update(TerraGuardian guardian) //Add a overrideable method for custom dialogues.
        {
            this.guardian = guardian;
            bool IsWere = CastellaBase.OnWerewolfForm(guardian);
            bool LookForVictim = false;
            switch (Step)
            {
                case 0: //Pick behavior step;
                    IgnoreCombat = true;
                    if (IsWere)
                    {
                        if (StepStart) Main.NewText("Awooooooooooo!!");
                        if (Time >= 300) ChangeStep(1);
                    }
                    else
                    {
                        InUse = false;
                    }
                    break;
                case 1:
                    {
                        IgnoreCombat = false;
                        LookForVictim = true;
                    }
                    break;
                case 200:
                case 201:
                case 202:
                    {
                        IgnoreCombat = true;
                        AvoidItemUsage = true;
                        guardian.MoveLeft = guardian.MoveRight = guardian.Jump = guardian.MoveDown = false;
                        switch (VictimType)
                        {
                            case TerraGuardian.TargetTypes.Guardian:
                                {
                                    if (!MainMod.ActiveGuardians.ContainsKey(VictimID))
                                    {
                                        ChangeStep(1);
                                        return;
                                    }
                                    TerraGuardian Victim = MainMod.ActiveGuardians[VictimID];
                                    Victim.IsBeingPulledByPlayer = false;
                                    if (Victim.PlayerMounted)
                                        Victim.ToggleMount(true, false);
                                    Vector2 Position = guardian.GetGuardianBetweenHandPosition;
                                    Victim.Position = Position + new Vector2(0, Victim.Height * 0.5f);
                                    Victim.Direction = -guardian.Direction;
                                    Victim.Velocity = Vector2.Zero;
                                    Victim.SetFallStart();
                                    Victim.AddBuff(Terraria.ID.BuffID.Cursed, 5, true);
                                    if (Time == 120)
                                    {
                                        Victim.EnterDownedState();
                                        Victim.KnockedOutCold = true;
                                    }
                                    if(Time >= 150)
                                    {
                                        ChangeStep(1);
                                    }
                                }
                                break;
                            case TerraGuardian.TargetTypes.Player:
                                {
                                    if(!Main.player[VictimID].active || Main.player[VictimID].dead)
                                    {
                                        ChangeStep(1);
                                        return;
                                    }
                                    Player Victim = Main.player[VictimID];
                                    PlayerMod pm = Victim.GetModPlayer<PlayerMod>();
                                    Vector2 Position = guardian.GetGuardianBetweenHandPosition;
                                    Position.X -= Victim.width * 0.5f;
                                    Position.Y -= Victim.height * 0.5f;
                                    Victim.position = Position;
                                    Victim.direction = -guardian.Direction;
                                    Victim.velocity = Vector2.Zero;
                                    Victim.fallStart = (int)(Victim.position.Y * (1f / 16));
                                    Victim.AddBuff(Terraria.ID.BuffID.Cursed, 5);
                                    if(guardian.TalkPlayerID > -1)
                                    {
                                        ChangeStep(201);
                                    }
                                    if (Step == 200 || Step == 202)
                                    {
                                        if (Time == 90)
                                        {
                                            if (Victim.statLife == 1)
                                                Victim.statLife++;
                                            if (!pm.KnockedOut) Victim.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Victim.name + " couldn't endure the bite."), 1, 0);
                                            pm.EnterDownedState(true);
                                            Victim.statLife = 1;
                                            Victim.Bottom = guardian.Position;
                                            if (Main.netMode == 0 && Victim.whoAmI == Main.myPlayer)
                                            {
                                                foreach (TerraGuardian tg in pm.GetAllGuardianFollowers)
                                                {
                                                    if (tg.Active)
                                                        tg.EnterDownedState();
                                                }
                                                if (guardian.IsTownNpc)
                                                {
                                                    guardian.TeleportHome();
                                                }
                                                else
                                                {
                                                    if (Main.rand.Next(3) != 0)
                                                    {
                                                        NpcMod.DespawnGuardianNPC(guardian);
                                                    }
                                                }
                                                MainMod.DoBlackoutPlayer();
                                                WorldMod.SkipTimeUntilMorning();
                                                return;
                                            }
                                        }
                                        if (Time >= 120)
                                        {
                                            ChangeStep(1);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
            if (LookForVictim)
            {
                if (guardian.TargetID > -1 && guardian.TargetType != TerraGuardian.TargetTypes.Npc)
                {
                    AvoidItemUsage = true;
                    Rectangle GrabBox = guardian.HitBox;
                    GrabBox.Width += 8;
                    if (guardian.LookingLeft)
                        GrabBox.X -= 8;
                    switch (guardian.TargetType)
                    {
                        case TerraGuardian.TargetTypes.Guardian:
                            if (GrabBox.Intersects(MainMod.ActiveGuardians[guardian.TargetID].HitBox))
                            {
                                VictimID = guardian.TargetID;
                                VictimType = guardian.TargetType;
                                ChangeStep(200);
                            }
                            break;
                        case TerraGuardian.TargetTypes.Player:
                            if (GrabBox.Intersects(Main.player[guardian.TargetID].getRect()))
                            {
                                VictimID = guardian.TargetID;
                                VictimType = guardian.TargetType;
                                ChangeStep(200);
                            }
                            break;
                    }
                }
                else
                {
                    AvoidItemUsage = false;
                }
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            bool IsWere = CastellaBase.OnWerewolfForm(guardian);
            if (Step >= 200 && Step <= 202)
            {
                int Frame = IsWere ? 41 : 12;
                if (Step != 201 && Time >= 30)
                    Frame--;
                if (!UsingLeftArmAnimation) guardian.LeftArmAnimationFrame = Frame;
                if (!UsingRightArmAnimation) guardian.RightArmAnimationFrame = Frame;
                if(guardian.Velocity.X == 0 && guardian.Velocity.Y == 0)
                {
                    guardian.BodyAnimationFrame = Frame;
                }
                if(Step != 201 && Time >= 60 && Time < 90)
                {
                    guardian.BodyAnimationFrame = 62;
                }
            }
        }

        public override bool? ModifyPlayerHostile(TerraGuardian guardian, Player player)
        {
            return !guardian.IsPlayerBuddy(player) && Step < 200;
        }

        public override bool? ModifyGuardianHostile(TerraGuardian guardian, TerraGuardian guardian2)
        {
            return Step > 0 && Step != 201;
        }

        #region Dialogues

        private void ClearOptions()
        {
            GuardianMouseOverAndDialogueInterface.Options.Clear();
        }

        private void AddOption(string Mes, Action NextBranch)
        {
            GuardianMouseOverAndDialogueInterface.Options.Add(new DialogueOption(Mes, NextBranch));
        }

        private void Message(string Mes)
        {
            GuardianMouseOverAndDialogueInterface.SetDialogue(Mes);
        }

        private bool ChangeDialogueFatigue(byte Increase = 1)
        {
            DialogueFatigue += Increase;
            if (DialogueFatigue >= 5)
            {
                FailResult();
                return true;
            }
            return false;
        }

        public override string ModifyDialogue(TerraGuardian guardian, List<DialogueOption> Options)
        {
            bool IsWere = CastellaBase.OnWerewolfForm(guardian);
            string Mes = "";
            if (IsWere)
            {
                Options.Clear();
                switch (Main.rand.Next(3))
                {
                    default:
                        Mes = "*You want to talk to me? I don't think things are too favorable on your side, Terrarian.*";
                        break;
                    case 1:
                        Mes = "*Generally my prey doesn't try speaking to me. I don't know if you're courageous or stupid.*";
                        break;
                    case 2:
                        Mes = "*You had to open your mouth, didn't you? Now I suppose I should talk to you?*";
                        break;
                }
                Options.Add(new DialogueOption("Who are you?", AskWhoSheIs));
                Options.Add(new DialogueOption("Why are you doing this?", AskWhyShesDoingThat)); //An array of dialogues picking 2~3 by random?
            }
            return Mes;
        }

        private void AskWhoSheIs()
        {
            if (ChangeDialogueFatigue())
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*Interessed in knowing me, are you? Very well, I'll tell you. I am Castella, and you will be my personal chew toy for this night.*");
                    break;
                case 1:
                    Message("*I am Castella. Don't worry, we'll have enough time to get acquantaince.*");
                    break;
            }
        }

        private void AskWhyShesDoingThat()
        {
            if (ChangeDialogueFatigue(2))
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*I like hunting people in this season just for fun, and chew througout the night.*");
                    break;
                case 1:
                    Message("*Every full moon night I need toys to play with, so I catch someone to put my teeth on.*");
                    break;
            }
        }

        private void AskWhatDidSheDoToYourCompanions()
        {
            if (ChangeDialogueFatigue())
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*They're taking a nice little slumber. You shall have the same destiny, too.*");
                    break;
                case 1:
                    Message("*They wont be able to help you while blacked out. Now, shall you black out too?*");
                    break;
            }
        }

        private void AskToLetYouGo()
        {
            if (ChangeDialogueFatigue())
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*Why? I didn't begun nibbling yet.*");
                    break;
                case 1:
                    Message("*As much as was fun to catch you, my teeth are still itching to bite something.*");
                    break;
            }
        }

        private void TellYouArentAToy()
        {
            if (ChangeDialogueFatigue(2))
            {
                return;
            }
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*Don't worry, I don't usually break my toys. You will simply blackout on the first bite.*");
                    break;
                case 1:
                    Message("*No no no, you are wrong. For this night, you're my personal toy.*");
                    break;
            }
        }

        private void FailResult()
        {
            switch (Main.rand.Next(2))
            {
                default:
                    Message("*The night might be will be over soon, and for you, way sooner.*");
                    break;
                case 1:
                    Message("*My teeth are itching, time for nibbling.*");
                    break;
            }
            ClearOptions();
            //GuardianMouseOverAndDialogueInterface.CloseDialogue();
            ChangeStep(202);
            AddOption("No, NO!", GuardianMouseOverAndDialogueInterface.CloseDialogueButtonAction);
        }

        #endregion
    }
}
