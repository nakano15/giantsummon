using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianHouseManagementInterface
    {
        public static bool ManagingGuardianHouses { get { return Main.playerInventory && Main.EquipPage == 1; } }
        public static bool IsPickingAGuardianHouse = false;
        public static int PickedGuardianToGiveHousing = -1;
        public static Player MainPlayer { get { return Main.player[Main.myPlayer]; } }

        public static void Update()
        {
            if (!ManagingGuardianHouses)
                return;
        }

        public static void Draw()
        {
            if (!ManagingGuardianHouses)
                return;
            try
            {
                string MouseText = "";
                for (int g = 0; g < WorldMod.MaxGuardianNpcsInWorld; g++)
                {
                    WorldMod.GuardianTownNpcState townnpc = WorldMod.GuardianNPCsInWorld[g];
                    if (townnpc != null && !townnpc.Homeless && WorldMod.IsGuardianNpcInWorld(townnpc.CharID))
                    {
                        int BannerX = townnpc.HomeX, BannerY = townnpc.HomeY;
                        if (BannerX < 0 || BannerY < 0)
                            continue;
                        BannerY--;
                        if (Main.tile[BannerX, BannerY] == null)
                            continue;
                        bool EndsOnNullTile = false;
                        while (!Main.tile[BannerX, BannerY].active() || !Main.tileSolid[(int)Main.tile[BannerX, BannerY].type])
                        {
                            BannerY--;
                            if (BannerY < 10)
                                break;
                            if (Main.tile[BannerX, BannerY] == null)
                            {
                                EndsOnNullTile = true;
                                break;
                            }
                        }
                        if (EndsOnNullTile)
                            continue;
                        TerraGuardian guardian = null;
                        foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
                        {
                            if (townnpc.IsID(tg.ID, tg.ModID))
                            {
                                guardian = tg;
                                break;
                            }
                        }
                        if (guardian == null)
                            continue;
                        const int PaddingX = 8;
                        int PaddingY = 18;
                        if (Main.tile[BannerX, BannerY].type == 19)
                            PaddingY -= 8;
                        BannerY++;
                        Vector2 BannerPosition = new Vector2(BannerX * 16 + PaddingX, BannerY * 16 + PaddingY) - Main.screenPosition;
                        DrawBanner(guardian, BannerPosition, Lighting.GetColor(BannerX, BannerY));
                        BannerPosition -= new Vector2(Main.HouseBannerTexture[1].Width * 0.5f, Main.HouseBannerTexture[1].Height * 0.5f);
                        if (Main.mouseX >= BannerPosition.X && Main.mouseX < BannerPosition.X + Main.HouseBannerTexture[1].Width &&
                            Main.mouseY >= BannerPosition.Y && Main.mouseY < BannerPosition.Y + Main.HouseBannerTexture[1].Height)
                        {
                            //MainPlayer.mouseInterface = true;
                            MouseText = guardian.Name;
                            if (Main.mouseRight && Main.mouseRightRelease)
                            {
                                townnpc.Homeless = true;
                                townnpc.HomeX =
                                townnpc.HomeY = -1;
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            }
                        }
                    }
                }
                float Scale = 0.85f;
                float SlotSpace = (56 * Scale);
                int MaxRowItems = (int)((Main.screenWidth - 64) / SlotSpace);
                Vector2 RowPosition = new Vector2(0, Main.screenHeight - SlotSpace - 8);
                bool MouseOverHouseIcon = false;
                List<TerraGuardian[]> GuardianRows = new List<TerraGuardian[]>();
                {
                    List<TerraGuardian> Guardians = new List<TerraGuardian>();
                    int Counter = 0;
                    foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
                    {
                        if (tg.FriendshipLevel >= tg.Base.MoveInLevel)
                            Guardians.Add(tg);
                        Counter++;
                        if (Counter >= MaxRowItems)
                        {
                            GuardianRows.Add(Guardians.ToArray());
                            Guardians.Clear();
                        }
                    }
                    GuardianRows.Add(Guardians.ToArray());
                    Guardians.Clear();
                }
                for (int r = 0; r < GuardianRows.Count; r++)
                {
                    RowPosition.X = Main.screenWidth * 0.5f - GuardianRows[r].Length * SlotSpace * 0.5f;
                    for (int n = 0; n < GuardianRows[r].Length; n++)
                    {
                        Main.spriteBatch.Draw(Main.inventoryBack6Texture, RowPosition, null, Main.inventoryBack, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                        GuardianRows[r][n].DrawHead(RowPosition + new Vector2(28, 28) * Scale);
                        if (Main.mouseX >= RowPosition.X && Main.mouseX < RowPosition.X + SlotSpace &&
                            Main.mouseY >= RowPosition.Y && Main.mouseY < RowPosition.Y + SlotSpace)
                        {
                            MainPlayer.mouseInterface = true;
                            MouseOverHouseIcon = true;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                IsPickingAGuardianHouse = true;
                                PickedGuardianToGiveHousing = GuardianRows[r][n].WhoAmID;
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            }
                        }
                        RowPosition.X += SlotSpace;
                    }
                    RowPosition.Y -= SlotSpace;
                }
                if (IsPickingAGuardianHouse)
                {
                    MainPlayer.mouseInterface = true;
                    if (!MainMod.ActiveGuardians.ContainsKey(PickedGuardianToGiveHousing))
                    {
                        IsPickingAGuardianHouse = false;
                    }
                    else
                    {
                        DrawBanner(MainMod.ActiveGuardians[PickedGuardianToGiveHousing], new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
                        if (!MouseOverHouseIcon && Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            int TileX = (int)(Main.mouseX + Main.screenPosition.X) / 16, TileY = (int)(Main.mouseY + Main.screenPosition.Y) / 16;
                            if (WorldMod.MoveGuardianToHouse(MainMod.ActiveGuardians[PickedGuardianToGiveHousing], TileX, TileY))
                            {
                                Main.NewText(MainMod.ActiveGuardians[PickedGuardianToGiveHousing].Name + " will move to this room.");
                                IsPickingAGuardianHouse = false;
                                Main.PlaySound(12, -1, -1, 1, 1f, 0f);
                            }
                        }
                        if (Main.mouseRight && Main.mouseRightRelease)
                            IsPickingAGuardianHouse = false;
                    }
                }
                if (MouseText != "")
                {
                    Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX, Main.mouseY + 16), Color.White, 1f, 0.5f);
                }
            }
            catch { }
        }

        public static void DrawBanner(TerraGuardian tg, Vector2 Position, Color color)
        {
            byte BannerType = 1;
            Main.spriteBatch.Draw(Main.HouseBannerTexture[BannerType],
                Position, null,
                color, 0f, new Vector2(Main.HouseBannerTexture[BannerType].Width * 0.5f, Main.HouseBannerTexture[BannerType].Height * 0.5f),
                1f, SpriteEffects.None, 0f);
            tg.DrawHead(Position, 1f);
        }
    }
}
