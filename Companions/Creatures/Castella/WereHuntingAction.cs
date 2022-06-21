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
        private TerraGuardian.TargetTypes VictimType = TerraGuardian.TargetTypes.Player;
        private int VictimID = -1;

        public override void Update(TerraGuardian guardian) //Add a overrideable method for custom dialogues.
        {
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
                                    if (Time == 120)
                                    {
                                        if (Victim.statLife == 1)
                                            Victim.statLife++;
                                        if(!pm.KnockedOut)Victim.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(Victim.name + " couldn't endure the bite."), 1, 0);
                                        pm.EnterDownedState(true);
                                        Victim.statLife = 1;
                                        Victim.Bottom = guardian.Position;
                                        if(Main.netMode == 0 && Victim.whoAmI == Main.myPlayer)
                                        {
                                            foreach(TerraGuardian tg in pm.GetAllGuardianFollowers)
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
                                                if(Main.rand.Next(3) != 0)
                                                {
                                                    NpcMod.DespawnGuardianNPC(guardian);
                                                }
                                            }
                                            MainMod.DoBlackoutPlayer();
                                            WorldMod.SkipTimeUntilMorning();
                                            return;
                                        }
                                    }
                                    if (Time >= 150)
                                    {
                                        ChangeStep(1);
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
            if (Step == 200)
            {
                int Frame = IsWere ? 41 : 12;
                if (Time >= 30)
                    Frame--;
                if (!UsingLeftArmAnimation) guardian.LeftArmAnimationFrame = Frame;
                if (!UsingRightArmAnimation) guardian.RightArmAnimationFrame = Frame;
                if(guardian.Velocity.X == 0 && guardian.Velocity.Y == 0)
                {
                    guardian.BodyAnimationFrame = Frame;
                }
                if(Time >= 90 && Time < 120)
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
            return Step > 0;
        }
    }
}
