using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon.Creatures.Vladimir
{
    public class CarrySomeoneAction : GuardianActions
    {
        public bool PickedUpPerson = false;
        public int CarriedPersonID = 0;
        public TerraGuardian.TargetTypes CarriedPersonType = TerraGuardian.TargetTypes.Guardian;
        public int Duration = 0;
        public bool WasFollowingPlayerBefore = false;

        public CarrySomeoneAction(TerraGuardian Vladimir, Player player, int Time = 0)
        {
            this.ID = 1;
            WasFollowingPlayerBefore = Vladimir.OwnerPos > -1;
            CarriedPersonID = player.whoAmI;
            CarriedPersonType = TerraGuardian.TargetTypes.Player;
            Duration = Time;
        }

        public CarrySomeoneAction(TerraGuardian Vladimir, TerraGuardian tg, int Time = 0)
        {
            this.ID = 1;
            WasFollowingPlayerBefore = Vladimir.OwnerPos > -1;
            CarriedPersonID = tg.WhoAmID;
            CarriedPersonType = TerraGuardian.TargetTypes.Guardian;
            Duration = Time;
        }

        public CarrySomeoneAction(TerraGuardian Vladimir, NPC npc, int Time = 0)
        {
            this.ID = 1;
            WasFollowingPlayerBefore = Vladimir.OwnerPos > -1;
            CarriedPersonID = npc.whoAmI;
            CarriedPersonType = TerraGuardian.TargetTypes.Npc;
            Duration = Time;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (!PickedUpPerson)
            {
                if (guardian.CurrentIdleAction == TerraGuardian.IdleActions.Listening)
                    return;
                guardian.WalkMode = true;
                Time++;
                Rectangle TargetRect;
                switch (CarriedPersonType)
                {
                    default:
                        InUse = false;
                        return;
                    case TerraGuardian.TargetTypes.Guardian:
                        if (!MainMod.ActiveGuardians.ContainsKey(CarriedPersonID))
                        {
                            InUse = false;
                            return;
                        }
                        TargetRect = MainMod.ActiveGuardians[CarriedPersonID].HitBox;
                        break;
                    case TerraGuardian.TargetTypes.Player:
                        if (!Main.player[CarriedPersonID].active)
                        {
                            InUse = false;
                            return;
                        }
                        TargetRect = Main.player[CarriedPersonID].getRect();
                        break;
                    case TerraGuardian.TargetTypes.Npc:
                        if (!Main.npc[CarriedPersonID].active)
                        {
                            InUse = false;
                            return;
                        }
                        TargetRect = Main.npc[CarriedPersonID].getRect();
                        break;
                }
                float TargetCenterX = TargetRect.X + TargetRect.Width * 0.5f;
                if(guardian.Position.X < TargetCenterX)
                {
                    guardian.MoveRight = true;
                }
                else
                {
                    guardian.MoveLeft = false;
                }
                if (TargetRect.Intersects(guardian.HitBox) || Time >= 5 * 60)
                {
                    PickedUpPerson = true;
                    Time = 0;
                }
                if(!PickedUpPerson)
                    return;
            }
            bool CarryingSomeone = true;
            if (!WasFollowingPlayerBefore)
            {
                if (guardian.TargetID == -1)
                {
                    Time++;
                }
                if (Time >= Duration)
                {
                    CarryingSomeone = false;
                }
            }
            if (guardian.KnockedOut)
                CarryingSomeone = false;
            if (WasFollowingPlayerBefore && guardian.OwnerPos == -1)
            {
                guardian.SaySomething("*The Terrarian will still need your help, better you go with them.*");
                CarryingSomeone = false;
            }
            else if (!WasFollowingPlayerBefore && guardian.OwnerPos != -1)
            {
                guardian.SaySomething("*It might be dangerous, better you stay here.*");
                CarryingSomeone = false;
            }
            switch (CarriedPersonType)
            {
                case TerraGuardian.TargetTypes.Guardian:
                    if (!MainMod.ActiveGuardians.ContainsKey(CarriedPersonID))
                    {
                        CarryingSomeone = false;
                    }
                    else if (CarriedPersonID == guardian.WhoAmID)
                    {
                        CarryingSomeone = false;
                        guardian.DisplayEmotion(TerraGuardian.Emotions.Question);
                    }
                    else
                    {
                        TerraGuardian HeldGuardian = MainMod.ActiveGuardians[CarriedPersonID];
                        if(HeldGuardian.CurrentIdleAction == TerraGuardian.IdleActions.Listening)
                        {
                            guardian.ChangeIdleAction(TerraGuardian.IdleActions.Listening, 5);
                            guardian.LookAt(Main.player[HeldGuardian.TalkPlayerID].Center);
                        }
                        HeldGuardian.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                        HeldGuardian.Position = guardian.GetGuardianShoulderPosition + guardian.Velocity;
                        HeldGuardian.Position.Y += HeldGuardian.Base.SittingPoint.Y * HeldGuardian.Scale - HeldGuardian.Height;
                        HeldGuardian.Velocity = Vector2.Zero;
                        HeldGuardian.gfxOffY = 0;
                        HeldGuardian.SetFallStart();
                        if (HeldGuardian.KnockedOut)
                            HeldGuardian.ReviveBoost += 3;
                        if (HeldGuardian.ItemAnimationTime == 0)
                            HeldGuardian.Direction = guardian.Direction;
                        guardian.AddDrawMomentToTerraGuardian(HeldGuardian);
                    }
                    break;

                case TerraGuardian.TargetTypes.Npc:
                    if (!Main.npc[CarriedPersonID].active)
                    {
                        CarryingSomeone = false;
                    }
                    else
                    {
                        NPC npc = Main.npc[CarriedPersonID];
                        for(int p = 0; p < 255; p++)
                        {
                            if(Main.player[p].talkNPC == npc.whoAmI)
                            {
                                guardian.ChangeIdleAction(TerraGuardian.IdleActions.Listening, 5);
                                guardian.LookAt(Main.player[p].Center);
                                break;
                            }
                        }
                        npc.position = guardian.GetGuardianShoulderPosition;
                        npc.position.X -= npc.width * 0.5f;
                        npc.position.Y -= npc.height * 0.5f + 8;
                        if (npc.velocity.X == 0)
                            npc.direction = guardian.Direction;
                        npc.velocity = Microsoft.Xna.Framework.Vector2.Zero;
                        guardian.AddDrawMomentToNpc(npc);
                    }
                    break;

                case TerraGuardian.TargetTypes.Player:
                    if (!Main.player[CarriedPersonID].active)
                    {
                        CarryingSomeone = false;
                    }
                    else
                    {
                        Player player = Main.player[CarriedPersonID];
                        player.position = guardian.GetGuardianShoulderPosition;
                        player.position.X -= player.width * 0.5f;
                        player.position.Y -= player.height * 0.5f + 8;
                        player.fallStart = (int)(player.position.Y * TerraGuardian.DivisionBy16);
                        player.velocity = Microsoft.Xna.Framework.Vector2.Zero;
                        player.AddBuff(ModContent.BuffType<Buffs.Hug>(), 5);
                        PlayerMod pm = player.GetModPlayer<PlayerMod>();
                        if (pm.KnockedOut)
                            pm.ReviveBoost += 3;
                        if (player.itemAnimation == 0)
                            player.direction = guardian.Direction;
                        guardian.AddDrawMomentToPlayer(player);
                        if (player.controlJump)
                        {
                            CarryingSomeone = false;
                        }
                    }
                    break;
            }
            if (!CarryingSomeone)
            {
                PlaceCarriedPersonOnTheFloor(guardian);
                InUse = false;
            }
        }

        public override void UpdateAnimation(TerraGuardian guardian, ref bool UsingLeftArmAnimation, ref bool UsingRightArmAnimation)
        {
            GuardianBase VladBase = guardian.Base;
            int Frame = 1;
            if (guardian.BodyAnimationFrame == VladBase.ThroneSittingFrame)
                Frame = 23;
            else if (guardian.BodyAnimationFrame == VladBase.BedSleepingFrame)
                Frame = 25;
            else if (guardian.Ducking)
                Frame = 12;
            if (guardian.BodyAnimationFrame == VladBase.StandingFrame || guardian.BodyAnimationFrame == VladBase.DuckingFrame)
                guardian.BodyAnimationFrame = Frame;
            if (guardian.BodyAnimationFrame == VladBase.ThroneSittingFrame)
                guardian.BodyAnimationFrame = VladBase.ThroneSittingFrame + 1;
            if (guardian.BodyAnimationFrame == VladBase.BedSleepingFrame)
                guardian.BodyAnimationFrame = VladBase.BedSleepingFrame + 1;
            if (!UsingLeftArmAnimation)
            {
                guardian.LeftArmAnimationFrame = Frame;
                UsingLeftArmAnimation = true;
            }
            if (!UsingRightArmAnimation)
            {
                guardian.RightArmAnimationFrame = Frame;
                UsingRightArmAnimation = true;
            }
        }

        public string GetCarriedOneName()
        {
            switch (CarriedPersonType)
            {
                case TerraGuardian.TargetTypes.Guardian:
                    return MainMod.ActiveGuardians[CarriedPersonID].Name;
                case TerraGuardian.TargetTypes.Player:
                    return Main.player[CarriedPersonID].name;
                case TerraGuardian.TargetTypes.Npc:
                    return Main.npc[CarriedPersonID].GivenOrTypeName;
            }
            return "";
        }

        public override void OnActionEnd(TerraGuardian guardian)
        {
            PlaceCarriedPersonOnTheFloor(guardian);
        }

        public void PlaceCarriedPersonOnTheFloor(TerraGuardian guardian)
        {
            switch (CarriedPersonType)
            {
                case TerraGuardian.TargetTypes.Guardian:
                    {
                        TerraGuardian HeldGuardian = MainMod.ActiveGuardians[CarriedPersonID];
                        HeldGuardian.Position = guardian.Position;
                    }
                    break;

                case TerraGuardian.TargetTypes.Npc:
                    {
                        NPC npc = Main.npc[CarriedPersonID];
                        npc.position = guardian.Position;
                        npc.position.X -= npc.width * 0.5f;
                        npc.position.Y -= npc.height;
                    }
                    break;

                case TerraGuardian.TargetTypes.Player:
                    {
                        Player player = Main.player[CarriedPersonID];
                        player.position = guardian.Position;
                        player.position.X -= player.width * 0.5f;
                        player.position.Y -= player.height;
                    }
                    break;
            }
        }
    }
}
