using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace giantsummon
{
    public class GuardianOrderHud
    {
        public static bool OrderSelection = false;
        public static int OrderPickTime = 0;
        public static float StartRotation = 0, RotationSum = 0f;
        public static int MaxOrders = 0;
        public const float OrderDistance = 48f;
        public static List<Orders> OrderList = new List<Orders>();
        public static TerraGuardian Guardian
        {
            get
            {
                PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                TerraGuardian guardian = player.Guardian;
                if (SpecificGuardian > 0 && SpecificGuardian < 255)
                {
                    guardian = player.AssistGuardians[SpecificGuardian - 1];
                }
                return guardian;
            }
        }
        private const float MaxAngle = 6.283185307179586f;
        public static byte SpecificGuardian = 255;

        public static void Open()
        {
            OrderSelection = true;
            Mouse.SetPosition((int)(Main.screenWidth * 0.5f), (int)(Main.screenHeight * 0.5f));
            //SpecificGuardian = 255;
            RefreshOrders();
        }

        public static void RefreshOrders()
        {
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            TerraGuardian[] guardians = player.GetAllGuardianFollowers;
            if (SpecificGuardian < 255)
            {
                if ((SpecificGuardian == 0 && !player.Guardian.Active) || (SpecificGuardian > 0 && !player.AssistGuardians[SpecificGuardian - 1].Active))
                    SpecificGuardian = 255;
            }
            bool GuardianDoingAction = Guardian.DoAction.InUse;
            bool OrderToEverybody = SpecificGuardian == 255;
            if (MainMod.Gameplay2PMode)
            {
                if (!Guardian.PlayerMounted) OrderList.Add(Orders.CallToPosition);
            }
            else
            {
                if (!Guardian.PlayerMounted && !Guardian.SittingOnPlayerMount)
                    OrderList.Add(Orders.FollowWait);
                OrderList.Add(Orders.AvoidCombat);
                OrderList.Add(Orders.UseStatusIncreaseItems);
                if (SpecificGuardian == 255 || (!Guardian.PlayerMounted && !Guardian.PlayerControl && !Guardian.GuardingPosition.HasValue))
                    OrderList.Add(Orders.CallToPosition);
                if ((SpecificGuardian != 255 && Guardian.SittingOnPlayerMount) || (SpecificGuardian == 255 && guardians.Any(x => x.Active && x.SittingOnPlayerMount)))
                    OrderList.Add(Orders.GetOffMyMount);
                if ((SpecificGuardian != 255 && !GuardianDoingAction && Guardian.HasMagicMirror && !Guardian.PlayerControl) || (SpecificGuardian == 255 && guardians.Any(x => x.Active && !x.DoAction.InUse && x.HasMagicMirror && !x.PlayerControl)))
                    OrderList.Add(Orders.TeleportWithPlayerToTown);
                if (Main.player[Main.myPlayer].mount.Active && !Guardian.DoAction.InUse && !Guardian.PlayerMounted && !Guardian.PlayerControl && !Guardian.SittingOnPlayerMount)
                    OrderList.Add(Orders.JoinMyMount);
                if (!Guardian.PlayerMounted && !Guardian.DoAction.InUse && !MainMod.Gameplay2PMode && Guardian.HasFlag(GuardianFlags.PlayerControl))
                    OrderList.Add(Orders.PlayerControl);
                if (!Guardian.PlayerControl && !Guardian.DoAction.InUse && Guardian.HasFlag(GuardianFlags.AllowMount) && (Guardian.PlayerMounted || (Guardian.Base.ReverseMount && !player.GuardianMountingOnPlayer) || (!Guardian.Base.ReverseMount && !player.MountedOnGuardian)))
                    OrderList.Add(Orders.Mount);
                if (Guardian.Inventory.Any(x => x.buffType > 0) && !Guardian.DoAction.InUse)
                    OrderList.Add(Orders.UseBuffPotions);
                if (!Guardian.HasCooldown(GuardianCooldownManager.CooldownType.HealingPotionCooldown) && !Guardian.DoAction.InUse)
                    OrderList.Add(Orders.HealSelf);
                if (!Guardian.PlayerControl && Guardian.HasFlag(GuardianFlags.MayGoSellLoot) && !Guardian.DoAction.InUse)
                    OrderList.Add(Orders.GoSellLoot);
                if (guardians.Any(g => g.Active && g.PlayerMounted && g.FriendshipLevel >= g.Base.StopMindingAFK))
                {
                    OrderList.Add(Orders.GiveGuardianControl);
                }
                if (Guardian.Base.Size == GuardianBase.GuardianSize.Large && !Guardian.DoAction.InUse && !Guardian.PlayerControl)// && Guardian.FriendshipGrade >= 1)
                {
                    if (Main.player[Main.myPlayer].wingsLogic > 0) OrderList.Add(Orders.LaunchMe);
                    OrderList.Add(Orders.LiftMe);
                }
                if (!Guardian.PlayerMounted && !Guardian.GuardingPosition.HasValue)
                    OrderList.Add(Orders.GoAhead);
                if (!Guardian.DoAction.InUse && Guardian.HasItem(ModContent.ItemType<Items.Consumable.SkillResetPotion>()))
                    OrderList.Add(Orders.UseSkillResetPotion);
                OrderList.Add(Orders.TestFurnitureOrder);
            }
            List<Orders> SpecificOrders = new List<Orders>();
            if (SpecificGuardian != 255) SpecificOrders.Add(Orders.OrderAllGuardians);
            if (SpecificGuardian != 0 && player.Guardian.Active) SpecificOrders.Add(Orders.OrderMainGuardian);
            if (SpecificGuardian != 1 && player.AssistGuardians[0].Active) SpecificOrders.Add(Orders.OrderFirstAssistGuardian);
            if (SpecificGuardian != 2 && player.AssistGuardians[1].Active) SpecificOrders.Add(Orders.OrderSecondAssistGuardian);
            if (SpecificGuardian != 3 && player.AssistGuardians[2].Active) SpecificOrders.Add(Orders.OrderThirdAssistGuardian);
            if (SpecificGuardian != 4 && player.AssistGuardians[3].Active) SpecificOrders.Add(Orders.OrderFourthAssistGuardian);
            if (SpecificOrders.Count > 0)
            {
                int OrderPos = OrderList.Count / SpecificOrders.Count;
                for (int p = SpecificOrders.Count - 1; p >= 0; p--)
                {
                    OrderList.Insert(p * OrderPos, SpecificOrders[p]);
                }
            }
            int OrderCount = OrderList.Count;
            if (OrderCount == 2)
            {
                RotationSum = 0.5f * 6.283185307179586f;
            }
            else
            {
                RotationSum = (1f / OrderCount) * 6.283185307179586f;
            }
            StartRotation = 0f;
        }

        public static void Close()
        {
            OrderSelection = false;
            OrderList.Clear();
        }

        public static void UpdateAndDraw()
        {
            try
            {
                if (Main.gameMenu || Main.player[Main.myPlayer].dead || MainMod.Gameplay2PMode || Guardian.Downed)
                    return;
                List<TerraGuardian> HeadsToDraw = new List<TerraGuardian>();
                byte GuardianCount = 0;
                foreach (TerraGuardian g in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (SpecificGuardian == 255 || SpecificGuardian == GuardianCount)
                    {
                        if (g.Active && !g.Base.InvalidGuardian)
                        {
                            HeadsToDraw.Add(g);
                        }
                    }
                    GuardianCount++;
                }
                bool OrderButtonPressed = MainMod.OrderCallButtonPressed;
                if (!OrderSelection)
                {
                    if (Guardian.Active && OrderButtonPressed && Main.mouseItem.type == 0 && !Main.player[Main.myPlayer].mouseInterface)
                    {
                        OrderPickTime++;
                        if (OrderPickTime >= 20)
                        {
                            OrderPickTime = 0;
                            Open();
                        }
                    }
                    else if (OrderPickTime > 0)
                    {
                        OrderPickTime = 0;
                    }
                    return;
                }
                Vector2 ScreenCenter = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
                Vector2 MouseRelativePosition = ScreenCenter - new Vector2(Main.mouseX, Main.mouseY);
                float MouseRotation = (float)Math.Atan2((double)-MouseRelativePosition.X, (double)-MouseRelativePosition.Y),
                    Distance = MouseRelativePosition.Length();
                if (MouseRotation < 0)
                    MouseRotation += MaxAngle;
                int SelectedOption = (int)(MouseRotation / RotationSum);
                //MouseRotation = MathHelper.WrapAngle(MouseRotation);
                if (HeadsToDraw.Count > 0)
                {
                    float Sum = 1f / HeadsToDraw.Count * 6.283185307179586f;
                    for (int h = 0; h < HeadsToDraw.Count; h++)
                    {
                        Vector2 HeadPosition = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
                        if (HeadsToDraw.Count > 1)
                        {
                            HeadPosition += 32 * (Sum * h).ToRotationVector2();
                        }
                        if (HeadsToDraw[h].Base.IsCustomSpriteCharacter)
                        {
                            Texture2D texture = HeadsToDraw[h].Base.sprites.HeadSprite;
                            HeadPosition.X -= texture.Width * 0.5f;
                            HeadPosition.Y -= texture.Height * 0.5f;
                            Main.spriteBatch.Draw(texture, HeadPosition, null, Color.White);
                        }
                        else
                        {
                            HeadPosition.Y -= 20;
                            HeadsToDraw[h].DrawTerrarianHeadData(HeadPosition);
                        }
                    }
                }
                float LastY = 0;
                for (int o = 0; o < OrderList.Count; o++)
                {
                    float AngleStart = (o * RotationSum + StartRotation),
                        AngleEnd = (AngleStart + RotationSum);
                    Vector2 PointA = new Vector2(OrderDistance * (float)Math.Sin(AngleStart), OrderDistance * (float)Math.Cos(AngleStart)),
                        PointB = new Vector2(OrderDistance * (float)Math.Sin(AngleEnd), OrderDistance * (float)Math.Cos(AngleEnd));
                    float Angle = (float)Math.Atan2((float)(PointB.Y - PointA.Y), (float)(PointB.X - PointA.X)) - 1.570796326794897f;
                    Vector2 TextPosition = (PointA + (PointB - PointA) * 0.5f) * 4f;
                    PointA += ScreenCenter;
                    PointB += ScreenCenter;
                    TextPosition += ScreenCenter;
                    /*if (LastY - 5 < TextPosition.Y && LastY + 5 > TextPosition.Y)
                    {
                        if (TextPosition.Y >= ScreenCenter.Y)
                            TextPosition.Y += 22;
                        else
                            TextPosition.Y -= 22;
                    }
                    else
                    {
                        LastY = TextPosition.Y;
                    }*/
                    float Scale = (PointB - PointA).Length();
                    Color color = Color.White;
                    if (Distance >= OrderDistance)
                    {
                        bool InRange = false;
                        InRange = o == SelectedOption; //(MouseRotation >= AngleStart && MouseRotation <= AngleEnd);
                        if (InRange)
                        {
                            color = Color.Yellow;
                            if (!OrderButtonPressed)
                            {
                                ExecuteAction(OrderList[o]);
                            }
                        }
                    }
                    Main.spriteBatch.Draw(Main.blackTileTexture, PointA, new Rectangle(0, 0, 1, 1), color, Angle, Vector2.Zero, new Vector2(1, Scale), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                    Utils.DrawBorderString(Main.spriteBatch, OrderTranslation(OrderList[o]), TextPosition, color, 1f, 0.5f, 0.5f);
                }
                if (!OrderButtonPressed)
                {
                    Close();
                    return;
                }
            }
            catch
            {

            }
        }

        public static void ExecuteAction(Orders o)
        {
            Action<TerraGuardian> action = delegate(TerraGuardian g) { };
            bool MainGuardianOrder = (o == Orders.Mount && SpecificGuardian == 255) || o == Orders.PlayerControl;
            bool SomeoneExecutedAction = false;
            switch (o)
            {
                case Orders.Mount:
                    action = delegate(TerraGuardian g)
                    {
                        //Guardian.ToggleMount();
                        if (g.PlayerMounted)
                        {
                            GuardianActions.GuardianPutPlayerOnTheFloorCommand(g);
                            //Guardian.ToggleMount();
                        }
                        else
                        {
                            if (GuardianActions.GuardianPutPlayerOnShoulderCommand(g))
                                SomeoneExecutedAction = true;
                        }
                    };
                    break;
                case Orders.FollowWait:
                    action = delegate(TerraGuardian g)
                    {
                        g.ToggleWait();
                    };
                    break;
                case Orders.HealSelf:
                    action = delegate(TerraGuardian g)
                    {
                        if(g.HP < g.MHP * 0.7f)
                            g.AttemptToUseHealingPotion();
                    };
                    break;
                case Orders.AvoidCombat:
                    action = delegate(TerraGuardian g)
                    {
                        g.AvoidCombat = !g.AvoidCombat;
                    };
                    break;
                case Orders.GoAhead:
                    action = delegate(TerraGuardian g)
                    {
                        g.ChargeAhead = !g.ChargeAhead;
                    };
                    break;
                case Orders.CallToPosition:
                    action = delegate(TerraGuardian g)
                    {
                        g.BePulledByPlayer();
                    };
                    break;
				case Orders.PlayerControl:
                    action = delegate(TerraGuardian g)
                    {
                        g.TogglePlayerControl();
                    };
                    break;
                case Orders.LaunchMe:
                    action = delegate(TerraGuardian g)
                    {
                        if (GuardianActions.UseLaunchPlayer(g, Main.player[g.OwnerPos]))
                            SomeoneExecutedAction = true;
                    };
                    break;
                case Orders.LiftMe:
                    action = delegate(TerraGuardian g)
                    {
                        if (GuardianActions.UseLiftPlayer(g, Main.player[g.OwnerPos]))
                            SomeoneExecutedAction = true;
                    };
                    break;
                case Orders.UseBuffPotions:
                    action = delegate(TerraGuardian g)
                    {
                        GuardianActions.UseBuffPotions(g);
                    };
                    break;
                case Orders.GoSellLoot:
                    action = delegate(TerraGuardian g)
                    {
                        GuardianActions.SellLootCommand(g);
                    };
                    break;
                case Orders.JoinMyMount:
                    action = delegate(TerraGuardian g)
                    {
                        GuardianActions.GuardianJoinPlayerMountCommand(g, Main.player[g.OwnerPos]);
                    };
                    break;
                case Orders.GetOffMyMount:
                    action = delegate(TerraGuardian g)
                    {
                        if (g.SittingOnPlayerMount)
                        {
                            g.DoSitOnPlayerMount(false);
                            g.Position.X = Main.player[g.OwnerPos].position.X + Main.player[g.OwnerPos].width * 0.5f;
                            g.Position.Y = Main.player[g.OwnerPos].position.Y + Main.player[g.OwnerPos].height - 1;
                        }
                    };
                    break;
                case Orders.TeleportWithPlayerToTown:
                    action = delegate(TerraGuardian g)
                    {
                        if (GuardianActions.TeleportWithPlayerCommand(g))
                            SomeoneExecutedAction = true;
                    };
                    break;
                case Orders.UseSkillResetPotion:
                    action = delegate(TerraGuardian g)
                    {
                        GuardianActions.UseSkillResetPotionCommand(g);
                    };
                    break;
                case Orders.TestFurnitureOrder:
                    action = delegate(TerraGuardian g)
                    {
                        if (g.furniturex > -1 || g.furniturey > -1)
                        {
                            g.LeaveFurniture();
                        }
                        else
                        {
                            g.UseNearbyFurniture(9, ushort.MaxValue, true);
                        }
                    };
                    break;
                case Orders.GiveGuardianControl:
                    action = delegate(TerraGuardian g)
                    {
                        g.ToggleGuardianFullControl(!g.GuardianHasControlWhenMounted);
                    };
                    break;
                case Orders.UseStatusIncreaseItems:
                    action = delegate(TerraGuardian g)
                    {
                        if(!g.DoAction.InUse)
                            GuardianActions.UseStatusIncreaseItems(g);
                    };
                    break;
                case Orders.OrderAllGuardians:
                    SpecificGuardian = 255;
                    RefreshOrders();
                    break;
                case Orders.OrderMainGuardian:
                    SpecificGuardian = 0;
                    RefreshOrders();
                    break;
                case Orders.OrderFirstAssistGuardian:
                    SpecificGuardian = 1;
                    RefreshOrders();
                    break;
                case Orders.OrderSecondAssistGuardian:
                    SpecificGuardian = 2;
                    RefreshOrders();
                    break;
                case Orders.OrderThirdAssistGuardian:
                    SpecificGuardian = 3;
                    RefreshOrders();
                    break;
                case Orders.OrderFourthAssistGuardian:
                    SpecificGuardian = 4;
                    RefreshOrders();
                    break;
            }
            PlayerMod player = Main.player[Guardian.OwnerPos].GetModPlayer<PlayerMod>();
            if (MainGuardianOrder)
            {
                if (player.Guardian.Active)
                {
                    action(player.Guardian);
                }
            }
            else if (SpecificGuardian == 255)
            {
                foreach (TerraGuardian g in Main.player[Guardian.OwnerPos].GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Where(x => x.Active && !x.Downed))
                {
                    action(g);
                    if (SomeoneExecutedAction)
                        break;
                }
            }
            else
            {
                if (SpecificGuardian == 0)
                {
                    if (player.Guardian.Active)
                        action(player.Guardian);
                }
                else
                {
                    if (SpecificGuardian - 1 >= 0 && SpecificGuardian - 1 < MainMod.MaxExtraGuardianFollowers)
                    {
                        TerraGuardian guardian = player.AssistGuardians[SpecificGuardian - 1];
                        if (guardian.Active)
                            action(guardian);
                    }
                }
            }
        }

        public static string OrderTranslation(Orders Order)
        {
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            string Text = "";
            switch (Order)
            {
                case Orders.FollowWait:
                    if (Guardian.GuardingPosition.HasValue)
                        Text = "Follow Me";
                    else
                        Text = "Guard Here";
                    break;
                case Orders.AvoidCombat:
                    if (Guardian.AvoidCombat)
                        Text = "Attack Hostiles";
                    else
                        Text = "Avoid Combat";
                    break;
                case Orders.Mount:
                    if (Guardian.PlayerMounted)
                        Text = "Dismount";
                    else
                        Text = "Mount";
                    break;
                case Orders.HealSelf:
                    Text = "Restore Health";
                    break;
                case Orders.GoAhead:
                    if (Guardian.ChargeAhead)
                    {
                        Text = "Stay Near Me";
                    }
                    else
                    {
                        Text = "Go In Front";
                    }
                    break;
                case Orders.CallToPosition:
                    Text = "Come Here";
                    break;
				case Orders.PlayerControl:
					if(Guardian.PlayerControl)
						Text = "Release Direct Control.";
					else
						Text = "Take Over Control";
                    break;
                case Orders.LaunchMe:
                    Text = "Launch me in the air";
                    break;
                case Orders.LiftMe:
                    Text = "Give me some height";
                    break;
                case Orders.UseBuffPotions:
                    Text = "Use Buff Potions";
                    break;
                case Orders.GoSellLoot:
                    Text = "Sell Acquired Loot";
                    break;
                case Orders.JoinMyMount:
                    Text = "Giddy Up!";
                    break;
                case Orders.GetOffMyMount:
                    Text = "Dismount";
                    break;
                case Orders.TeleportWithPlayerToTown:
                    Text = "Let's Teleport Out";
                    break;
                case Orders.UseSkillResetPotion:
                    Text = "Drink Skill Reset Potion";
                    break;
                case Orders.TestFurnitureOrder:
                    if (Guardian.furniturex > -1 || Guardian.furniturey > -1)
                    {
                        Text = "Get off the furniture.";
                    }
                    else
                    {
                        Text = "Use nearby furniture.";
                    }
                    break;
                case Orders.GiveGuardianControl:
                    if (!Guardian.GuardianHasControlWhenMounted)
                        Text = "Take Control";
                    else
                        Text = "Let me Control";
                    break;
                case Orders.OrderMainGuardian:
                    Text = "Order " + player.Guardian.Name + "...";
                    break;
                case Orders.OrderFirstAssistGuardian:
                    Text = "Order " + player.AssistGuardians[0].Name + "...";
                    break;
                case Orders.OrderSecondAssistGuardian:
                    Text = "Order " + player.AssistGuardians[1].Name + "...";
                    break;
                case Orders.OrderThirdAssistGuardian:
                    Text = "Order " + player.AssistGuardians[2].Name + "...";
                    break;
                case Orders.OrderFourthAssistGuardian:
                    Text = "Order " + player.AssistGuardians[3].Name + "...";
                    break;
                case Orders.OrderAllGuardians:
                    Text = "Order Everyone...";
                    break;
                case Orders.UseStatusIncreaseItems:
                    Text = "Use Life/Mana Crystal/Fruit";
                    break;
            }
            return Text;
        }

        public enum Orders : byte
        {
            FollowWait,
            AvoidCombat,
            Mount,
            HealSelf,
            GoAhead,
            CallToPosition,
			PlayerControl,
            LaunchMe,
            LiftMe,
            UseBuffPotions,
            GoSellLoot,
            JoinMyMount,
            GetOffMyMount,
            TeleportWithPlayerToTown,
            UseSkillResetPotion,
            TestFurnitureOrder,
            GiveGuardianControl,
            UseStatusIncreaseItems,

            OrderAllGuardians,
            OrderMainGuardian,
            OrderFirstAssistGuardian,
            OrderSecondAssistGuardian,
            OrderThirdAssistGuardian,
            OrderFourthAssistGuardian
        }
    }
}
