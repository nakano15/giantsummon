using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace giantsummon
{
    public class GuardianShopInterface
    {
        public static bool ShopOpen = false;
        private static Player MainPlayer { get { return Main.player[Main.myPlayer]; } }
        private static GuardianShopHandler.GuardianShop Shop;
        public static Vector2 ShopStartPosition = new Vector2();
        private static GuardianShopHandler.GuardianShopItem[] ItemsForSale
        {
            get
            {
                List<GuardianShopHandler.GuardianShopItem> Items = new List<GuardianShopHandler.GuardianShopItem>();
                for (int i = 0; i < Shop.Items.Count; i++)
                {
                    if(Shop.Items[i].ItemOnDisplay)
                        Items.Add(Shop.Items[i]);
                }
                return Items.ToArray();
            }
        }
        private static int Scroll = 0;
        private static int SelectedItem = -1;

        public static void OpenShop()
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            if (pm.IsTalkingToAGuardian && MainMod.ActiveGuardians.ContainsKey(pm.TalkingGuardianPosition))
            {
                TerraGuardian tg = MainMod.ActiveGuardians[pm.TalkingGuardianPosition];
                Shop = GuardianShopHandler.GetShop(tg.MyID);
                if (Shop != null)
                {
                    byte Count = 0;
                    for (int i = 0; i < Shop.Items.Count; i++)
                    {
                        Shop.Items[i].ItemOnDisplay = false;
                        if (Shop.Items[i].IsItemDisponible)
                        {
                            Shop.Items[i].ItemOnDisplay = true;
                        }
                    }
                    ShopOpen = true;
                    ShopStartPosition.X = 0;
                    ShopStartPosition.Y = 258;
                    Main.playerInventory = true;
                }
            }
            else
            {
                Shop = null;
                ShopOpen = false;
            }
        }

        public static void UpdateShop()
        {
            if (!ShopOpen)
            {
                return;
            }
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            if (!Main.playerInventory)
            {
                MainPlayer.GetModPlayer<PlayerMod>().IsTalkingToAGuardian = false;
                ShopOpen = false;
                return;
            }
        }

        public static void DrawShop()
        {
            if (!ShopOpen)
            {
                return;
            }
            Main.craftingHide = true;
            for (int i = 0; i < Main.availableRecipeY.Length; i++)
            {
                Main.availableRecipeY[i] = Main.screenHeight * 2;
            }
            TerraGuardian tg = MainMod.ActiveGuardians[MainPlayer.GetModPlayer<PlayerMod>().TalkingGuardianPosition];
            Vector2 DrawPosition = ShopStartPosition;
            Color color = new Color(200, 200, 200, 200);
            GuardianMouseOverAndDialogueInterface.DrawBackgroundPanel(DrawPosition, 464, 32, color);
            if (Main.mouseX >= DrawPosition.X && Main.mouseX < DrawPosition.X + 464 && Main.mouseY >= DrawPosition.Y && Main.mouseY < DrawPosition.Y + 32 + 60 * 6)
                MainPlayer.mouseInterface = true;
            DrawPosition.X += 16;
            DrawPosition.Y += 16;
            tg.DrawHead(DrawPosition);
            DrawPosition.X += 16;
            Utils.DrawBorderString(Main.spriteBatch, tg.Name + ": *That is what I have for sale.*", DrawPosition, Color.White * Main.cursorAlpha, 1f, 0, 0.5f);
            DrawPosition.Y += 16;
            DrawPosition.X -= 32;
            GuardianMouseOverAndDialogueInterface.DrawBackgroundPanel(DrawPosition, 464, 60 * 6, color);
            //Main.inventoryScale = 0.8f;
            GuardianShopHandler.GuardianShopItem[] Items = ItemsForSale;
            int NewSelectedItem = -1;
            for (int x = 0; x < 1; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    int Index = y + x * 6 + Scroll;
                    Vector2 SlotPosition = DrawPosition;
                    //if (x == 1)
                    //    SlotPosition.X += 256;
                    SlotPosition.Y += 60 * y;
                    if (Index < Items.Length)
                    {
                        GuardianShopHandler.GuardianShopItem Item = Items[Index];
                        Texture2D ItemTexture = Main.itemTexture[Item.ItemID];
                        float ItemScale = 1f;
                        if (ItemTexture.Width > ItemTexture.Height)
                        {
                            ItemScale = 48 / (float)ItemTexture.Width;
                        }
                        else
                        {
                            ItemScale = 48 / (float)ItemTexture.Height;
                        }
                        Vector2 ItemPosition = SlotPosition;
                        ItemPosition.X += 28;
                        ItemPosition.Y += 28;
                        Main.spriteBatch.Draw(ItemTexture, ItemPosition, null, Color.White, 0f, new Vector2(ItemTexture.Width, ItemTexture.Height) * 0.5f, ItemScale, SpriteEffects.None, 0f);
                        SlotPosition.X += 56;
                        bool CanBuy = Item.IsItemDisponible && Item.Stack != 0;
                        Color itemcolor = CanBuy ? Color.White : Color.Red;
                        string ItemName = Item.ItemName;
                        if (Item.Stack > -1)
                            ItemName += " " + Item.Stack;
                        if (Item.disponibility == GuardianShopHandler.GuardianShopItem.DisponibilityTime.Timed)
                        {
                            int h = 0, m = 0, s = Item.TimedSaleTime / 60;
                            if (s >= 60)
                            {
                                m += s / 60;
                                s -= m * 60;
                            }
                            if (m >= 60)
                            {
                                h += m / 60;
                                m -= h * 60;
                            }
                            if (h > 0)
                            {
                                ItemName += " " + h + " Hours left.";
                            }
                            else if (m > 0)
                            {
                                ItemName += " " + m + " Minutes left.";
                            }
                            else if (s > 0)
                            {
                                ItemName += " " + s + " Seconds left.";
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, ItemName, SlotPosition, itemcolor, 1f);
                        SlotPosition.Y += 30;
                        string PriceText = "";
                        int Price = Item.Price;
                        bool IsSale = false;
                        if (Item.SaleTime > 0 && Item.SaleFactor > 0)
                        {
                            IsSale = true;
                            Price -= (int)(Price * Item.SaleFactor);
                        }
                        {
                            int p = 0, g = 0, s = 0, c = Price;
                            if (c >= 100)
                            {
                                s += c / 100;
                                c -= s * 100;
                            }
                            if (s >= 100)
                            {
                                g += s / 100;
                                s -= g * 100;
                            }
                            if (g >= 100)
                            {
                                p += g / 100;
                                g -= p * 100;
                            }
                            bool First = true;
                            if (p > 0)
                            {
                                PriceText += p + "p";
                                First = false;
                            }
                            if (g > 0)
                            {
                                if (!First)
                                    PriceText += " ";
                                PriceText += g + "g";
                                First = false;
                            }
                            if (s > 0)
                            {
                                if (!First)
                                    PriceText += " ";
                                PriceText += s + "s";
                                First = false;
                            }
                            if (c > 0)
                            {
                                if (!First)
                                    PriceText += " ";
                                PriceText += c + "c";
                                First = false;
                            }
                        }
                        if (IsSale)
                        {
                            PriceText += " | (" + Math.Round(Item.SaleFactor * 100) + "% Off) ";
                            int h = 0, m = 0, s = Item.SaleTime / 60;
                            if (s >= 60)
                            {
                                m += s / 60;
                                s -= m * 60;
                            }
                            if (m >= 60)
                            {
                                h += m / 60;
                                m -= h * 60;
                            }
                            bool First = true;
                            if (h > 0)
                            {
                                PriceText += h + " hours left.";
                                First = false;
                            }
                            else if (m > 0)
                            {
                                if (First)
                                    PriceText += ", ";
                                if (!First && m < 10)
                                    PriceText += '0';
                                PriceText += m + " minutes left.";
                                First = false;
                            }
                            else if (s > 0)
                            {
                                if (First)
                                    PriceText += ", ";
                                if (!First && s < 10)
                                    PriceText += '0';
                                PriceText += s + " seconds left.";
                                First = false;
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, PriceText, SlotPosition, itemcolor, 1f, 0, 0);
                        SlotPosition.X += 336f;
                        Vector2 Dim = Utils.DrawBorderString(Main.spriteBatch, (Item.Stack == 0 ? "Sold Out" : (CanBuy ? "Buy Item" : "Indisponible")), SlotPosition, SelectedItem == Index ? Color.Yellow : Color.White, 1f, 0, 0.5f);
                        if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + Dim.X &&
                            Main.mouseY >= SlotPosition.Y - Dim.Y * 0.5f && Main.mouseY < SlotPosition.Y + Dim.Y * 0.5f)
                        {
                            NewSelectedItem = Index;
                            if (CanBuy && Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                //Buy Item :D

                            }
                        }
                    }
                }
            }
            SelectedItem = NewSelectedItem;
        }
    }
}
