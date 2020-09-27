using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class BuddyModeSetupInterface
    {
        public static Vector2 WindowPosition = Vector2.Zero;
        public static int WindowWidth = 0, WindowHeight = 0;
        public static bool WindowActive = false;
        public static int SelectedBuddy = -1;
        public static int MenuScroll = 0;
        public static GuardianID[] PossibleGuardianIDs = new GuardianID[0];
        public static string[] PossibleGuardianNames = new string[0];
        public static TerraGuardian DisplayGuardian = null;
        public static string[] GameModeInfo = new string[0];

        public static void Open()
        {
            Main.playerInventory = false;
            WindowPosition.X = Main.screenWidth * 0.2f;
            WindowPosition.Y = Main.screenHeight * 0.2f;
            WindowWidth = Main.screenWidth - (int)WindowPosition.X * 2;
            WindowHeight = Main.screenHeight - (int)WindowPosition.Y * 2;
            PossibleGuardianIDs = MainMod.GetPossibleStarterGuardians();
            PossibleGuardianNames = new string[PossibleGuardianIDs.Length];
            for (int i = 0; i < PossibleGuardianIDs.Length; i++)
            {
                GuardianBase gb = GuardianBase.GetGuardianBase(PossibleGuardianIDs[i].ID, PossibleGuardianIDs[i].ModID);
                if (gb.InvalidGuardian)
                    PossibleGuardianNames[i] = "Corrupted Memory";
                else
                    PossibleGuardianNames[i] = gb.Name;
            }
            WindowActive = true;
            GameModeInfo = new string[]{"The Buddies Mode is a special game mode where you pick a companion you",
                "will play the game with since the beggining of your adventure, until forever.",
                "The companion you pick ranges from starter companions, to companions met by other player characters.",
                "You wont be able to dismiss It, or call assist companions to help you with It activated, so think well.",
                "Nothing stops you from finding new companions to live in your town, get requests, or give them equipments, though.",
                "you have 12 in-game hours (12 real life minutes) to decide if you want to try It or not."}; ;
        }

        public static void Close()
        {
            PossibleGuardianIDs = new GuardianID[0];
            DisplayGuardian = null;
            WindowActive = false;
            SelectedBuddy = -1;
            MenuScroll = 0;
            GameModeInfo = new string[0];
        }

        public static void Draw()
        {
            if (Main.playerInventory)
            {
                Main.playerInventory = false;
                WindowActive = false;
                return;
            }
            if (Main.mouseX >= WindowPosition.X && Main.mouseX < WindowPosition.X + WindowWidth &&
                Main.mouseY >= WindowPosition.Y && Main.mouseY < WindowPosition.Y + WindowHeight)
            {
                Main.player[Main.myPlayer].mouseInterface = true;
            }
            DrawRectangle(WindowPosition.X, WindowPosition.Y, WindowWidth, WindowHeight, Color.Green);
            DrawRectangle(WindowPosition.X - 2, WindowPosition.Y - 2, 2, WindowHeight + 4, Color.Black);
            DrawRectangle(WindowPosition.X, WindowPosition.Y - 2, WindowWidth, 2, Color.Black);
            DrawRectangle(WindowPosition.X + WindowWidth, WindowPosition.Y, 2, WindowHeight + 2, Color.Black);
            DrawRectangle(WindowPosition.X, WindowPosition.Y + WindowHeight, WindowWidth, 2, Color.Black);
            Vector2 DrawPosition = new Vector2(WindowPosition.X + WindowWidth * 0.5f, WindowPosition.Y - 2);
            Utils.DrawBorderString(Main.spriteBatch, "Buddies Mode", DrawPosition, Color.Yellow, 1.2f, 0.5f);
            DrawPosition.Y += 28;
            foreach (string s in GameModeInfo)
            {
                DrawPosition.Y += Utils.DrawBorderString(Main.spriteBatch, s, DrawPosition, Color.White, 0.9f, 0.5f).Y;
            }
            DrawPosition.X = WindowPosition.X + 2;
            float ElementStartPosY = DrawPosition.Y;
            int MenuWidth = 228, MenuHeight = (int)(WindowPosition.Y - DrawPosition.Y + WindowHeight - 2);
            DrawRectangle(DrawPosition.X, DrawPosition.Y, MenuWidth, MenuHeight, Color.LightBlue);
            int MaxElements = MenuHeight / 30;
            bool PreviouslyPickedSomeone = false;
            for (int i = 0; i < MaxElements; i++)
            {
                int index = i + MenuScroll;
                byte MenuElement = 1; //0 = Up Arrow, 1 = Companion, 2 = DownArrow
                if (MenuScroll > 0 && i == 0)
                {
                    MenuElement = 0;
                }
                if (MenuScroll + MaxElements < PossibleGuardianIDs.Length && i == MaxElements - 1)
                {
                    MenuElement = 2;
                }
                if (index < 0 || i >= PossibleGuardianIDs.Length)
                    continue;
                Vector2 OptionPosition = DrawPosition;
                OptionPosition.X += MenuWidth * 0.5f;
                OptionPosition.Y += 30 * i;
                string Text = "";
                if (MenuElement == 0)
                    Text = "= Up =";
                else if (MenuElement == 2)
                    Text = "= Down =";
                else
                    Text = PossibleGuardianNames[index];
                if (DrawTextButton(Text, OptionPosition, 1f) && !PreviouslyPickedSomeone)
                {
                    PreviouslyPickedSomeone = true;
                    if (MenuElement == 0)
                        MenuScroll--;
                    else if (MenuElement == 2)
                    {
                        MenuScroll++;
                    }
                    else
                    {
                        if (DisplayGuardian == null || DisplayGuardian.MyID != PossibleGuardianIDs[index])
                        {
                            DisplayGuardian = new TerraGuardian(PossibleGuardianIDs[index].ID, PossibleGuardianIDs[index].ModID);
                        }
                        SelectedBuddy = index;
                    }
                }
            }
            DrawPosition.X += MenuWidth + 2 + (WindowWidth - MenuWidth) / 2;
            DrawPosition.Y = WindowPosition.Y + WindowHeight - 152;
            if (SelectedBuddy > -1)
            {
                if (DisplayGuardian != null)
                {
                    DisplayGuardian.Position = DrawPosition + Main.screenPosition;
                    DisplayGuardian.Draw(true);
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Name, DrawPosition, Color.White, 1.1f, 0.5f);
                    DrawPosition.Y += 28;
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.Description, DrawPosition, Color.White, 1, 0.5f);
                }
            }
            DrawPosition.X = WindowPosition.X + WindowWidth * 0.5f;
            DrawPosition.Y = WindowPosition.Y + WindowHeight - 22;
            if (SelectedBuddy > -1 && !DisplayGuardian.Base.InvalidGuardian && DrawTextButton("Pick Buddy", DrawPosition, 1.2f))
            {
                if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().SetBuddyMode(PossibleGuardianIDs[SelectedBuddy].ID, PossibleGuardianIDs[SelectedBuddy].ModID))
                {
                    Close();
                }
            }
            DrawPosition.X += 120;
            if (DrawTextButton("Close", DrawPosition, 1.2f))
            {
                Close();
            }
        }

        public static bool DrawTextButton(string Text, Vector2 Position, float Scale = 1f)
        {
            Vector2 Dimension = Utils.DrawBorderString(Main.spriteBatch, Text, Position, Color.White, Scale, 0.5f);
            if (Main.mouseX >= Position.X - Dimension.X * 0.5f && Main.mouseX < Position.X + Dimension.X * 0.5f &&
                Main.mouseY >= Position.Y && Main.mouseY < Position.Y + Dimension.Y)
            {
                Utils.DrawBorderString(Main.spriteBatch, Text, Position, Color.Yellow, Scale, 0.5f);
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    return true;
                }
            }
            return false;
        }

        public static void DrawRectangle(float x, float y, int width, int height, Color c)
        {
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)x, (int)y, width, height), c);
        }
    }
}
