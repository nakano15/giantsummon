using System;
using Terraria;
using Microsoft.Xna.Framework;

namespace giantsummon.Actions
{
    public class BuySomethingFromNpcShopAction : GuardianActions
    {
        public int NpcPosition, ItemID, BuyStack, BuyPrice;
        public BuySomethingFromNpcShopAction(int NpcPosition, int ItemID, int BuyStack, int BuyPrice)
        {
            ID = (int)ActionIDs.BuySomethingFromNpcShop;
            InUse = true;
            this.NpcPosition = NpcPosition;
            this.ItemID = ItemID;
            this.BuyStack = BuyStack;
            this.BuyPrice = BuyPrice;
        }

        public override void Update(TerraGuardian guardian)
        {
            if (Step == 0 && StepStart)
            {
                Cancellable = true;
            }
            if (guardian.TargetID > -1 || guardian.TalkPlayerID > -1 || guardian.IsBeingPulledByPlayer)
            {
                InUse = false;
                return;
            }
            bool MakeNpcFocusOnGuardian = false;
            if (!guardian.PlayerMounted)
                guardian.MoveLeft = guardian.MoveRight = false;
            switch (Step)
            {
                case 0: //Try reaching npc
                    {
                        NPC npc = Main.npc[NpcPosition];
                        if (!npc.active)
                        {
                            InUse = false;
                            return;
                        }
                        if (StepStart && guardian.PlayerMounted)
                        {
                            string Message = guardian.GetMessage(GuardianBase.MessageIDs.AskPlayerToGetCloserToShopNpc, "*This companion wants to buy from [shop]'s store.\nGet closer to It so they can buy from It.*");
                            Message.Replace("[shop]", npc.GivenOrTypeName);
                            guardian.SaySomething(Message);
                        }
                        Vector2 NpcBottom = npc.Bottom;
                        if (!guardian.PlayerMounted && Time == 10 * 60)
                        {
                            guardian.Position = NpcBottom;
                            guardian.SetFallStart();
                        }
                        else if (!guardian.PlayerMounted && Math.Abs(NpcBottom.X - guardian.Position.X) < 16)
                        {
                            if (guardian.Position.X >= NpcBottom.X)
                            {
                                guardian.MoveRight = true;
                            }
                            else
                            {
                                guardian.MoveLeft = true;
                            }
                        }
                        else if (Math.Abs(NpcBottom.X - guardian.Position.X) >= 16f + guardian.Width * 0.5f)
                        {
                            if (!guardian.PlayerMounted)
                            {
                                if (guardian.Position.X < NpcBottom.X)
                                {
                                    guardian.MoveRight = true;
                                }
                                else
                                {
                                    guardian.MoveLeft = true;
                                }
                            }
                        }
                        else
                        {
                            MakeNpcFocusOnGuardian = true;
                            ChangeStep();
                            if (guardian.PlayerMounted)
                            {
                                string Message = guardian.GetMessage(GuardianBase.MessageIDs.AskPlayerToWaitAMomentWhileCompanionIsShopping, "*They ask you to wait a moment.*");
                                Message.Replace("[shop]", npc.GivenOrTypeName);
                                guardian.SaySomething(Message);
                            }
                        }
                        if (Math.Abs(NpcBottom.Y - guardian.Position.Y) > 8)
                        {
                            if (guardian.Position.Y < NpcBottom.Y)
                            {
                                if (guardian.Velocity.Y == 0)
                                {
                                    bool SolidBlockBellow = false;
                                    int CheckY = (int)(guardian.Position.Y + 2) / 16;
                                    for (int i = 0; i < guardian.Width / 16; i++)
                                    {
                                        int CheckX = (int)(guardian.TopLeftPosition.X / 16) + i;
                                        if (Main.tile[CheckX, CheckY].active() && Main.tileSolid[Main.tile[CheckX, CheckY].type] && !Terraria.ID.TileID.Sets.Platforms[Main.tile[CheckX, CheckY].type])
                                        {
                                            SolidBlockBellow = true;
                                            break;
                                        }
                                    }
                                    if (!SolidBlockBellow)
                                    {
                                        guardian.MoveDown = true;
                                        guardian.Jump = true;
                                    }
                                }
                            }
                            else
                            {
                                int CheckX = (int)(guardian.Position.X / 16);
                                if (guardian.JumpHeight == 0 && !guardian.Jump || guardian.JumpHeight > 0)
                                {
                                    for (int i = 1; i < 6; i++)
                                    {
                                        int CheckY = (int)(guardian.Position.Y / 16) - i;
                                        if (Main.tile[CheckX, CheckY].active() && Terraria.ID.TileID.Sets.Platforms[Main.tile[CheckX, CheckY].type])
                                        {
                                            guardian.Jump = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        NPC npc = Main.npc[NpcPosition];
                        MakeNpcFocusOnGuardian = true;
                        Vector2 NpcCenter = npc.Center;
                        if (Math.Abs(guardian.Position.X - NpcCenter.X) < 16)
                        {
                            if (guardian.Position.X < NpcCenter.X)
                                guardian.MoveLeft = true;
                            else
                                guardian.MoveRight = true;
                        }
                        else if (guardian.Velocity.X == 0)
                        {
                            guardian.LookingLeft = guardian.Position.X >= NpcCenter.X;
                        }
                        if (Time >= 1.5f * 60)
                        {
                            int Stack = guardian.BuyItem(ItemID, BuyPrice, BuyStack);
                        }
                        if (Time >= 2 * 60)
                        {
                            InUse = false;
                            if (guardian.PlayerMounted)
                            {
                                guardian.SaySomething(guardian.GetMessage(GuardianBase.MessageIDs.GenericThankYou, "*It thanked you.*"));
                            }
                            return;
                        }
                    }
                    break;
            }
            if (MakeNpcFocusOnGuardian)
            {
                NPC npc = Main.npc[NpcPosition];
                if (npc.ai[0] != 0)
                    npc.netUpdate = true;
                npc.ai[0] = 0;
                npc.ai[1] = 300;
                npc.localAI[3] = 100;
                if (npc.Center.X < guardian.Position.X)
                    npc.direction = 1;
                else
                    npc.direction = -1;
            }
        }
    }
}
