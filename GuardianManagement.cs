using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianManagement
    {
        public static bool Active = false;
        public static int SelectedGuardian = 0;
        public static Tab tab = Tab.Inventory;

        public static bool OpenInterfaceForGuardian(int GuardianPosition)
        {
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (player.MyGuardians.ContainsKey(GuardianPosition))
            {
                SelectedGuardian = GuardianPosition;
                Active = true;
                Main.playerInventory = true;
                tab = Tab.Inventory;
                return true;
            }
            return false;
        }

        public static void UpdateAndDraw()
        {
            if (!Active)
                return;
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (!Main.playerInventory)
            {
                Active = false;
                return;
            }
            if (!Main.playerInventory || !player.MyGuardians.ContainsKey(SelectedGuardian))
            {
                Active = false;
                return;
            }
            if (player.player.chest != -1 || player.player.talkNPC > -1)
                return;
            GuardianData Guardian = player.MyGuardians[SelectedGuardian];
            if (PlayerMod.PlayerHasGuardianSummoned(player.player, Guardian.ID, Guardian.ModID))
            {
                Active = false;
                Main.NewText("You already have the guardian summoned. Manage it through the buttons on bellow the inventory.");
                return;
            }
            float HudStartX = 64, HudStartY = MainMod.InventoryEndY;
            Utils.DrawBorderString(Main.spriteBatch, Guardian.Name + "'s Possessions", new Vector2(HudStartX, HudStartY), Color.White);
            HudStartY += 24f;
            string[] Tabs = Enum.GetNames(typeof(Tab));
            float TabX = HudStartX;
            for (int i = 0; i < Tabs.Length; i++)
            {
                Vector2 TabPos = new Vector2(TabX, HudStartY);
                Vector2 Dimension = Utils.DrawBorderString(Main.spriteBatch, Tabs[i], TabPos, Color.White, 0.85f);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)TabPos.X, (int)TabPos.Y, (int)Dimension.X + 4, (int)Dimension.Y + 4), (i != (int)tab ? Color.Black : Color.White));
                TabPos.X += 2;
                bool MouseOver = false;
                if (Main.mouseX >= TabPos.X && Main.mouseX < TabPos.X + Dimension.X &&
                    Main.mouseY >= TabPos.Y && Main.mouseY < TabPos.Y + Dimension.Y)
                {
                    MouseOver = true;
                    player.player.mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        tab = (Tab)i;
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Tabs[i], TabPos, (MouseOver ? Color.Cyan : Color.White), 0.85f);

                TabX += Dimension.X + 4;
            }
            HudStartY += 20;
            Item HoverItem = null;
            switch (tab)
            {
                case Tab.Inventory:
                    {
                        for (int y = 0; y < 5; y++)
                        {
                            for (int x = 0; x < 10; x++)
                            {
                                Vector2 SlotPosition = new Vector2(HudStartX, HudStartY);
                                SlotPosition.X += x * 56 * Main.inventoryScale;
                                SlotPosition.Y += y * 56 * Main.inventoryScale + 20;
                                int i = x + y * 10;
                                if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + 56 * Main.inventoryScale && Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + 56 * Main.inventoryScale)
                                {
                                    Main.player[Main.myPlayer].mouseInterface = true;
                                    ItemSlot.OverrideHover(Guardian.Inventory, 0, i);
                                    HoverItem = Guardian.Inventory[i];
                                    int LastItemID = Main.mouseItem.type;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        if (Main.keyState.IsKeyDown(Main.FavoriteKey))
                                        {
                                            Guardian.Inventory[i].favorited = !Guardian.Inventory[i].favorited;
                                        }
                                        else if (ItemSlot.ShiftInUse)
                                        {
                                            if (!Guardian.Inventory[i].favorited && Guardian.Inventory[i].type != 0)
                                            {
                                                Item item = Main.player[Main.myPlayer].GetItem(Main.myPlayer, Guardian.Inventory[i]);
                                                Guardian.Inventory[i] = item;
                                            }
                                        }
                                        else
                                        {
                                            ItemSlot.LeftClick(Guardian.Inventory, 0, i);
                                        }
                                    }
                                    else
                                    {
                                        ItemSlot.RightClick(Guardian.Inventory, 0, i);
                                    }
                                    ItemSlot.MouseHover(Guardian.Inventory, 0, i);
                                }
                                ItemSlot.Draw(Main.spriteBatch, Guardian.Inventory, 0, i, SlotPosition);
                            }
                        }
                    }
                    break;
                case Tab.Equipment:
                    {
                        for (int s = 0; s < 9; s++)
                        {
                            Vector2 SlotPosition = new Vector2(HudStartX, HudStartY);
                            SlotPosition.Y += s * 56 * Main.inventoryScale + 20;
                            if (SlotPosition.Y + 56 * Main.inventoryScale >= Main.screenHeight)
                            {
                                SlotPosition.X += 56 * Main.inventoryScale + 20;
                                SlotPosition.Y -= SlotPosition.Y - 20 - HudStartY;
                            }
                            int context = 8;
                            if (s > 2)
                            {
                                context = 10;
                                SlotPosition.Y += 4;
                            }
                            if (s == 8 && (!player.player.extraAccessory || (!Main.expertMode && Guardian.Equipments[8].type == 0)))
                                continue;
                            if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + 56 * Main.inventoryScale && Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + 56 * Main.inventoryScale)
                            {
                                Main.player[Main.myPlayer].mouseInterface = true;
                                ItemSlot.OverrideHover(Guardian.Equipments, context, s);
                                HoverItem = Guardian.Equipments[s];
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    /*if (Main.mouseItem.type > 0 && !giantsummon.IsGuardianItem(Main.mouseItem))
                                    {
                                        Main.NewText("This item doesn't fit my Guardian...");
                                    }
                                    else*/
                                    {
                                        int HeldItemID = Main.mouseItem.type;
                                        bool Allow = true;//Main.mouseItem.type == 0 || (s < 3 && Main.mouseItem.modItem is Items.GuardianItemPrefab) || s >= 3;
                                        //Main.mouseItem.modItem is Items.GuardianItemPrefab ||
                                        if (Allow)
                                        {
                                            Allow = false;
                                            switch (s)
                                            {
                                                case 0:
                                                    if (Main.mouseItem.type == 0 || Main.mouseItem.headSlot > 0)
                                                        Allow = true;
                                                    break;
                                                case 1:
                                                    if (Main.mouseItem.type == 0 || Main.mouseItem.bodySlot > 0)
                                                        Allow = true;
                                                    break;
                                                case 2:
                                                    if (Main.mouseItem.type == 0 || Main.mouseItem.legSlot > 0)
                                                        Allow = true;
                                                    break;
                                                default:
                                                    if (Main.mouseItem.type == 0 || Main.mouseItem.accessory)
                                                    {
                                                        Allow = true;
                                                        if (Main.mouseItem.type != 0)
                                                        {
                                                            for (int a = 3; a < 9; a++)
                                                            {
                                                                if (Guardian.Equipments[a].type == Main.mouseItem.type)
                                                                {
                                                                    Allow = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        if (Allow)
                                        {
                                            Main.mouseItem.favorited = false;
                                            ItemSlot.LeftClick(Guardian.Equipments, 0, s);
                                        }
                                        else
                                        {
                                            Main.NewText("I can't do that...");
                                        }
                                    }
                                    ItemSlot.MouseHover(Guardian.Equipments, context, s);
                                }
                            }
                            ItemSlot.Draw(Main.spriteBatch, Guardian.Equipments, context, s, SlotPosition);
                        }
                    }
                    break;
                case Tab.Skins:
                    {
                        Vector2 SlotPosition = new Vector2(HudStartX, HudStartY + 22f);
                        bool HasSkin = false;
                        foreach (SkinReqStruct skin in Guardian.Base.SkinList)
                        {
                            HasSkin = true;
                            bool IsActive = Guardian.SkinID == skin.SkinID;
                            bool LastActive = IsActive;
                            bool RequirementBeaten = skin.Requirement(Guardian, player.player);
                            MainMod.AddOnOffButton(SlotPosition.X, SlotPosition.Y, skin.Name, ref IsActive, RequirementBeaten, !RequirementBeaten);
                            if (IsActive != LastActive)
                            {
                                if (Active)
                                    Guardian.SkinID = skin.SkinID;
                                else
                                    Guardian.SkinID = 0;
                            }
                            SlotPosition.Y += 20;
                        }
                        if (!HasSkin)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "This companion has no skin right now.", SlotPosition, Color.White);
                        }
                    }
                    break;
                case Tab.Outfits:
                    {
                        Vector2 SlotPosition = new Vector2(HudStartX, HudStartY + 22f);
                        bool HasOutfit = false;
                        foreach (SkinReqStruct skin in Guardian.Base.OutfitList)
                        {
                            HasOutfit = true;
                            bool IsActive = Guardian.OutfitID == skin.SkinID;
                            bool LastActive = IsActive;
                            bool RequirementBeaten = skin.Requirement(Guardian, player.player);
                            MainMod.AddOnOffButton(SlotPosition.X, SlotPosition.Y, skin.Name, ref IsActive, RequirementBeaten, !RequirementBeaten);
                            if (IsActive != LastActive)
                            {
                                if (Active)
                                    Guardian.OutfitID = skin.SkinID;
                                else
                                    Guardian.OutfitID = 0;
                            }
                            SlotPosition.Y += 20;
                        }
                        if (!HasOutfit)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "This companion has no outfits right now.", SlotPosition, Color.White);
                        }
                    }
                    break;
            }
            if (HoverItem != null)
            {

            }
        }

        public enum Tab
        {
            Inventory,
            Equipment,
            Skins,
            Outfits
        }
    }
}
