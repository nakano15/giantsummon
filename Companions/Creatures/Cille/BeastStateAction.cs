using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon.Companions.Creatures.Cille
{
    public class BeastStateAction : GuardianActions
    {
        public const int TimeUntilActivates = 7 * 60;

        public BeastStateAction()
        {
            Forced = true;
            BlockOffHandUsage = true;
        }

        public override void Update(TerraGuardian guardian)
        {
            switch (Step)
            {
                case 0:
                    {
                        const float DistanceBonus = 1; //3
                        if (Time == 0)
                        {
                            if (guardian.PlayerMounted)
                                guardian.Dismount();
                            if (guardian.SittingOnPlayerMount)
                                guardian.DoSitOnPlayerMount(false);
                            if (guardian.UsingFurniture)
                                guardian.LeaveFurniture(false);
                            if (guardian.IsLeader)
                                guardian.RemoveFromCommanding();
                            string Message;
                            switch (Main.rand.Next(3))
                            {
                                default:
                                    Message = "*No! Not again! Everyone, stay away from me!*";
                                    break;
                                case 1:
                                    Message = "*It's happening again! Don't follow me! Leave me alone!*";
                                    break;
                                case 2:
                                    Message = "*Save yourselves! I'm losing myself again!!*";
                                    break;
                            }
                            guardian.SaySomethingCanSchedule(Message, false, Main.rand.Next(60, 180));
                            if (guardian.OwnerPos > -1 && !guardian.IsPlayerBuddy(Main.player[guardian.OwnerPos]))
                            {
                                if (!NpcMod.HasGuardianNPC(guardian.ID, guardian.ModID))
                                    WorldMod.GuardianTownNPC.Add(guardian);
                                Main.player[guardian.OwnerPos].GetModPlayer<PlayerMod>().DismissGuardian(guardian.ID, guardian.ModID);
                            }
                        }
                        else
                        {
                            bool FleeToLeft = false;
                            float FarthestAllyLeft = 0, FarthestAllyRight = 0;
                            foreach(TerraGuardian tg in MainMod.ActiveGuardians.Values)
                            {
                                if (guardian.InPerceptionRange(tg.CenterPosition, DistanceBonus))
                                {
                                    float Distance = Math.Abs(tg.Position.X - guardian.Position.X);
                                    if (tg.Position.X < guardian.Position.X)
                                    {
                                        if (Distance > FarthestAllyLeft)
                                            FarthestAllyLeft = Distance;
                                    }
                                    else
                                    {
                                        if (Distance > FarthestAllyRight)
                                            FarthestAllyRight = Distance;
                                    }
                                }
                            }
                            for(int p = 0; p < 255; p++)
                            {
                                if(Main.player[p].active && !Main.player[p].dead && guardian.InPerceptionRange(Main.player[p].Center, DistanceBonus))
                                {
                                    float Distance = Math.Abs(Main.player[p].Center.X - guardian.Position.X);
                                    if (Main.player[p].Center.X < guardian.Position.X)
                                    {
                                        if (Distance > FarthestAllyLeft)
                                            FarthestAllyLeft = Distance;
                                    }
                                    else
                                    {
                                        if (Distance > FarthestAllyRight)
                                            FarthestAllyRight = Distance;
                                    }
                                }
                            }
                            FleeToLeft = FarthestAllyLeft < FarthestAllyRight; //Run to the position where allies have less distance from her.
                            guardian.MoveLeft = guardian.MoveRight = false;
                            guardian.WalkMode = false;
                            if (FleeToLeft)
                                guardian.MoveLeft = true;
                            else
                                guardian.MoveRight = true;
                            if (Time >= TimeUntilActivates)
                                ChangeStep();
                            IgnoreCombat = true;
                        }
                    }
                    break;
                case 1:
                    {
                        IgnoreCombat = false;
                        CilleBase.CilleData data = (CilleBase.CilleData)guardian.Data;
                        if (CilleBase.TriggerBeastState(guardian))
                        {
                            data.InBeastState = true;
                            ForcedTactic = CombatTactic.Charge;
                            if (guardian.TargetID > -1)
                            {
                                guardian.AttackingTarget = true;
                            }
                            //InUse = false;
                        }
                        else
                        {
                            data.InBeastState = false;
                            string Message;
                            switch (Main.rand.Next(3))
                            {
                                default:
                                    Message = "*Huh? It's over... I hope I didn't hurt anyone.*";
                                    break;
                                case 1:
                                    Message = "*What happened? Did someone got hurt?*";
                                    break;
                                case 2:
                                    Message = "*I'm so glad it's over. I didn't hurt anyone, right?*";
                                    break;
                            }
                            guardian.SaySomethingCanSchedule(Message, false, Main.rand.Next(30, 180));
                            InUse = false;
                        }
                    }
                    break;
            }
        }

        public override bool? ModifyGuardianHostile(TerraGuardian guardian, TerraGuardian guardian2)
        {
            if (Step >= 1)
                return true;
            return base.ModifyGuardianHostile(guardian, guardian2);
        }

        /*public override bool? ModifyNPCHostile(TerraGuardian guardian,NPC npc)
        {
            if (Step >= 1)
                return true;
            return base.ModifyNPCHostile(guardian, npc);
        }*/

        public override bool? ModifyPlayerHostile(TerraGuardian guardian, Player player)
        {
            if (Step >= 1)
            {
                if (guardian.IsPlayerBuddy(player))
                    return false;
                return true;
            }
            return base.ModifyPlayerHostile(guardian, player);
        }
    }
}
