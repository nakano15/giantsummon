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
        private const byte IdleNoHostile = 255;
        private const byte CaughtSomeone = 200, CaughtPlayerTalk = 201;
        private int FrenzyTime = 0;
        private bool FrenzyBite = false;
        private bool BegunPlayerChatter = false;

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
                        if (Time >= 30)
                        {
                            ChangeStep(1);
                            if (guardian.OwnerPos != -1)
                            {
                                Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian(guardian.ID, guardian.ModID, false);
                            }
                        }
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
                case CaughtSomeone:
                case CaughtPlayerTalk:
                    {
                        IgnoreCombat = true;
                        AvoidItemUsage = true;
                        guardian.MoveLeft = guardian.MoveRight = guardian.Jump = guardian.MoveDown = false;
                        HasPlayerChatterToDo();
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
                                    if (Time == 90)
                                    {
                                        Victim.EnterDownedState();
                                        if (Victim.PlayerControl)
                                            Victim.TogglePlayerControl(true);
                                        Victim.KnockedOutCold = true;
                                    }
                                    if(Time >= 120)
                                    {
                                        ChangeStep(1);
                                        VictimID = -1;
                                        FrenzyTime = 0;
                                        FrenzyBite = false;
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
                                                bool TakeSomewhereSafe = false;
                                                if (guardian.IsPlayerBuddy(Victim))
                                                {
                                                    TakeSomewhereSafe = true;
                                                }
                                                foreach (TerraGuardian tg in pm.GetAllGuardianFollowers)
                                                {
                                                    if (tg.Active)
                                                        tg.EnterDownedState();
                                                }
                                                WorldMod.SkipTimeUntilMorning();
                                                InUse = false;
                                                if (guardian.IsTownNpc)
                                                {
                                                    guardian.TeleportHome();
                                                }
                                                else
                                                {
                                                    if (Main.dayTime && Main.rand.Next(3) != 0)
                                                    {
                                                        NpcMod.DespawnGuardianNPC(guardian);
                                                    }
                                                }
                                                MainMod.DoBlackoutPlayer();
                                                if (TakeSomewhereSafe)
                                                {
                                                    Victim.Spawn();
                                                }
                                                else //Inflict injury debuff
                                                {

                                                }
                                                return;
                                            }
                                        }
                                        if (Time >= 120)
                                        {
                                            ChangeStep(1);
                                            VictimID = -1;
                                            FrenzyTime = 0;
                                            FrenzyBite = false;
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case IdleNoHostile:
                    {
                        BlockIdleAI = false;
                    }
                    return;
            }
            if (LookForVictim)
            {
                if (guardian.TargetID > -1 && guardian.TargetType != TerraGuardian.TargetTypes.Npc)
                {
                    AvoidItemUsage = true;
                    Rectangle GrabBox = guardian.HitBox;
                    GrabBox.Width += 16;
                    if (guardian.LookingLeft)
                        GrabBox.X -= 16;
                    switch (guardian.TargetType)
                    {
                        case TerraGuardian.TargetTypes.Guardian:
                            if (!MainMod.ActiveGuardians[guardian.TargetID].KnockedOut && GrabBox.Intersects(MainMod.ActiveGuardians[guardian.TargetID].HitBox))
                            {
                                VictimID = guardian.TargetID;
                                VictimType = guardian.TargetType;
                                if(FrenzyTime > 0)
                                {
                                    FrenzyBite = true;
                                }
                                ChangeStep(200);
                            }
                            break;
                        case TerraGuardian.TargetTypes.Player:
                            if (!Main.player[guardian.TargetID].GetModPlayer<PlayerMod>().KnockedOut && GrabBox.Intersects(Main.player[guardian.TargetID].getRect()))
                            {
                                VictimID = guardian.TargetID;
                                VictimType = guardian.TargetType;
                                if (FrenzyTime > 0)
                                {
                                    FrenzyBite = true;
                                }
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
            return Step < CaughtSomeone && Step != IdleNoHostile;// && Step < 200;
        }

        public override bool? ModifyGuardianHostile(TerraGuardian guardian, TerraGuardian guardian2)
        {
            return Step > 0 && Step < CaughtSomeone && Step != IdleNoHostile;
        }

        private bool HasPlayerChatterToDo()
        {
            bool HasChat = false;
            if (!BegunPlayerChatter)
            {
                HasChat = GeneratePlayerChatter();
            }
            else
            {
                if (guardian.MessageTime > 0 || guardian.MessageSchedule.Count > 0)
                {
                    HasChat = true;
                }
            }
            if (HasChat && Time < 90)
            {
                Time = 30;
            }
            return HasChat;
        }

        private bool GeneratePlayerChatter()
        {
            Player player = null;
            if(VictimType == TerraGuardian.TargetTypes.Guardian)
            {
                if (MainMod.ActiveGuardians[VictimID].PlayerControl)
                {
                    player = Main.player[MainMod.ActiveGuardians[VictimID].OwnerPos];
                }
            }
            else
            {
                player = Main.player[VictimID];
            }
            if (player == null)
                return false;
            BegunPlayerChatter = true;
            List<string> Messages = new List<string>();
            if (guardian.IsPlayerBuddy(player))
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        Messages.Add("*I shall be going to hunt someone.*");
                        Messages.Add("*I can't have you getting in danger in a while.*");
                        Messages.Add("*So I hope you forgive me if I knock you out for a while.*");
                        break;
                    case 1:
                        Messages.Add("*This night I shall catch something to nibble.*");
                        Messages.Add("*But I can't have you getting in the way, so forgive me for this.*");
                        break;
                }
            }
            else
            {
                if (!PlayerMod.PlayerHasGuardian(player, guardian.ID, guardian.ModID))
                {
                    PlayerMod.AddPlayerGuardian(player, guardian.ID, guardian.ModID);
                }
                int LastFriendshipLevel = PlayerMod.GetPlayerGuardianFriendshipLevel(player, guardian.ID, guardian.ModID);
                guardian.IncreaseFriendshipProgress(1);
                if (LastFriendshipLevel == 0)
                {
                    switch (Main.rand.Next(3))
                    {
                        default:
                            Messages.Add("*Hello, since you're my newest victim, I shall introduce myself.*");
                            Messages.Add("*I am Castella, and during this season, this is my hunting ground.*");
                            Messages.Add("*I like hunting people and nibbling them, and that's what I shall be doing now.*");
                            break;
                        case 1:
                            Messages.Add("*Look at that, a Terrarian. I haven't caught one of those since forever.*");
                            Messages.Add("*Since we're going to meet each other many times, I think I should introduce myself.*");
                            Messages.Add("*I am Castella. And don't worry, we shall have enough chances to get acquantaince.*");
                            break;
                        case 2:
                            Messages.Add("*This place surelly have interesting things I could get my paws on.*");
                            Messages.Add("*I am Castella, and this season is my playground.*");
                            Messages.Add("*Let's see how nibbling a Terrarian feels.*");
                            break;
                    }
                }
                else
                {
                    switch (Main.rand.Next(7))
                    {
                        default:
                            Messages.Add("*We meet again. May I have access to your neck?*");
                            break;
                        case 1:
                            Messages.Add("*Don't worry, this wont hurt, much.*");
                            break;
                        case 2:
                            Messages.Add("*What? Mesmerized by my eyes?*");
                            break;
                        case 3:
                            Messages.Add("*This shall be over soon for you. I promisse.*");
                            break;
                        case 4:
                            Messages.Add("*I won't kill you, but I can't promisse no posterior pain.*");
                            break;
                        case 5:
                            Messages.Add("*I hope you don't have any diseases.*");
                            break;
                        case 6:
                            Messages.Add("*You must be getting tired of me, right?*");
                            break;
                    }
                }
            }
            guardian.SaySomething(Messages.ToArray(), false, false);
            return true;
        }

        public override void Draw(TerraGuardian guardian)
        {
            if (VictimID == Main.myPlayer && VictimType == TerraGuardian.TargetTypes.Player)
                return;
            const float Blackness = 0.2f;
            foreach (GuardianDrawData gdd in TerraGuardian.DrawFront)
            {
                Color c = gdd.color;
                c.R = (byte)(c.R * Blackness);
                c.G = (byte)(c.G * Blackness);
                c.B = (byte)(c.B * Blackness);
                gdd.color = c;
            }
            foreach (GuardianDrawData gdd in TerraGuardian.DrawBehind)
            {
                Color c = gdd.color;
                c.R = (byte)(c.R * Blackness);
                c.G = (byte)(c.G * Blackness);
                c.B = (byte)(c.B * Blackness);
                gdd.color = c;
            }
        }
    }
}
