using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace giantsummon
{
    public class GuardianQuestInterface
    {
        public static bool IsActive = false;
        public static Vector2 Position = Vector2.Zero;
        public const int Width = 600, Height = 500;
        private static ushort ListScrollY = 0, StoryPage = 0;
        private static ushort SelectedItem = 0;
        private static List<int> QuestList = new List<int>();
        private static QuestData SelectedQuestInfo;
        private static string[] DescriptionText = new string[0],
            StoryText = new string[0],
            ObjectiveText = new string[0];

        public static void OpenInterface()
        {
            IsActive = true;
            Main.playerInventory = false;
            Position = new Vector2(Main.screenWidth - Width, Main.screenHeight - Height) * 0.5f;
            QuestList.Clear();
            QuestData[] quests = PlayerMod.GetPlayerQuestDatas(Main.player[Main.myPlayer]);
            for (int i = 0; i < quests.Length; i++)
            {
                if(!quests[i].IsInvalid && quests[i].IsQuestStarted)
                {
                    QuestList.Add(i);
                }
            }
            SelectedItem = 0;
            ListScrollY = 0;
            StoryPage = 0;
            DescriptionText = new string[0];
            StoryText = new string[0];
            ObjectiveText = new string[0];
            Main.playerInventory = false;
            if (QuestList.Count > 0)
            {
                ChangeQuest(SelectedItem);
            }
        }

        public static void CloseInterface()
        {
            SelectedQuestInfo = null;
            DescriptionText = new string[0];
            StoryText = new string[0];
            ObjectiveText = new string[0];
            QuestList.Clear();
            IsActive = false;
        }

        private static void ChangeQuest(int NewQuestPosition)
        {
            if (NewQuestPosition >= 0 && NewQuestPosition < QuestList.Count)
            {
                SelectedItem = (ushort)NewQuestPosition;
                SelectedQuestInfo = PlayerMod.GetPlayerQuestDatas(Main.LocalPlayer)[QuestList[SelectedItem]];
                StoryPage = 0;
                DescriptionText = ParseText(SelectedQuestInfo.Description);
                string storyText = SelectedQuestInfo.Story;
                if (storyText != "")
                    StoryText = ParseText("Story\n\n" + storyText);
                else
                    StoryText = new string[0];
                string objectiveText = SelectedQuestInfo.Objective;
                if (objectiveText != "")
                    ObjectiveText = ParseText("Objective: " + objectiveText);
                else
                    ObjectiveText = new string[0];
            }
        }

        public static string[] ParseText(string Text)
        {
            List<string> NewText = new List<string>();
            string Line = "", Word = "";
            const int CharWidth = 7;
            const int DialogueWidth = Width - 160 - 4 * 3;
            foreach(char c in Text)
            {
                if(c == '\n')
                {
                    if(Word != "")
                    {
                        if((Word.Length + Line.Length) * CharWidth >= DialogueWidth)
                        {
                            NewText.Add(Line);
                            Line = Word;
                            Word = "";
                        }
                        else
                        {
                            Line += Word;
                            NewText.Add(Line);
                            Line = "";
                            Word = "";
                        }
                    }
                    else
                    {
                        NewText.Add(Line);
                        Line = "";
                    }
                }
                else if(c == ' ')
                {
                    if((Word.Length + Line.Length) * CharWidth >= DialogueWidth)
                    {
                        NewText.Add(Line);
                        Line = Word + " ";
                        Word = "";
                    }
                    else
                    {
                        Line += Word + ' ';
                        Word = "";
                    }
                }
                else
                {
                    Word += c;
                }
            }
            if (Word != "")
                Line += Word;
            if (Line != "")
                NewText.Add(Line);
            return NewText.ToArray();
        }

        public static void Draw()
        {
            if (Main.playerInventory)
            {
                if (IsActive)
                {
                    CloseInterface();
                    return;
                }
                Vector2 TextPos = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.8f);
                Color c = Color.White;
                if(Main.mouseX >= TextPos.X - 40 && Main.mouseX < TextPos.X + 40 && Main.mouseY >= TextPos.Y - 10 && Main.mouseY < TextPos.Y + 10)
                {
                    Main.LocalPlayer.mouseInterface = true;
                    c = Color.Yellow;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                        OpenInterface();
                }
                Utils.DrawBorderString(Main.spriteBatch, "Open Quest List", TextPos, c, anchorx: 0.5f, anchory: 0.5f);
            }
            if (!IsActive)
                return;
            if (Main.mouseX >= Position.X && Main.mouseX < Position.X + Width &&
                Main.mouseY >= Position.Y && Main.mouseY < Position.Y + Height)
                Main.LocalPlayer.mouseInterface = true;
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)Position.X - 2, (int)Position.Y - 2, Width + 4, Height + 4), null, Color.Black);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), null, Color.Green);
            Vector2 InterfacePos = new Vector2(Position.X, Position.Y);
            InterfacePos.Y += 44;
            InterfacePos.X += 4;
            const int ListWidth = 160;
            const int RightPanelWidth = Width - ListWidth - 4 * 3;
            //List
            {
                int ListHeight = (int)Height - 48;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y, ListWidth, ListHeight), null, Color.LightGreen);
                int MaxElements = ListHeight / 30;
                for(int i = 0; i < MaxElements; i++)
                {
                    int index = i + ListScrollY;
                    if (index >= QuestList.Count)
                        break;
                    Vector2 OptionPosition = new Vector2(InterfacePos.X, InterfacePos.Y + 2 + i * 30);
                    Color color = Color.White;
                    if(Main.mouseX >= OptionPosition.X && Main.mouseX < OptionPosition.X + ListWidth && 
                        Main.mouseY >= OptionPosition.Y + 2 && Main.mouseY < OptionPosition.Y + 22)
                    {
                        color = Color.Cyan;
                        if(Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            ChangeQuest(index);
                        }
                    }
                    else if(index == SelectedItem)
                    {
                        color = Color.Yellow;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, SelectedQuestInfo.QuestName, OptionPosition, color);
                }
            }
            bool HasQuestSelected = SelectedQuestInfo != null;
            //Name
            {
                InterfacePos = new Vector2(Position.X + ListWidth + 4 * 2, Position.Y + 44);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y, RightPanelWidth, 30), null, Color.LightGreen);
                InterfacePos.X += (int)(RightPanelWidth * 0.5f);
                InterfacePos.Y += 6;
                if (HasQuestSelected)
                    Utils.DrawBorderString(Main.spriteBatch, SelectedQuestInfo.QuestName, InterfacePos, Color.White, anchorx: 0.5f);
            }
            //Quest Body
            {
                InterfacePos = new Vector2(Position.X + ListWidth + 4 * 2, Position.Y + 44 + 30 + 4);
                const int InterfaceHeight = Height - 42 - 40;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y, RightPanelWidth, InterfaceHeight), null, Color.LightGreen);
                if (HasQuestSelected)
                {
                    int MaxLines = 1;
                    InterfacePos.Y += 2;
                    if (SelectedQuestInfo.IsQuestCompleted)
                    {
                        float XBackup = InterfacePos.X;
                        InterfacePos.X += RightPanelWidth * 0.5f;
                        Utils.DrawBorderStringBig(Main.spriteBatch, "Complete!!", InterfacePos, Color.Yellow, anchorx:0.5f);
                        InterfacePos.X = XBackup;
                        InterfacePos.Y += 60;
                        MaxLines = (InterfaceHeight - 60) / 20;
                    }
                    else
                    {
                        {
                            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y + DescriptionText.Length * 30 - 1, RightPanelWidth, 2), null, Color.Black);
                        }
                        for (int i = 0; i < DescriptionText.Length; i++)
                        {
                            Vector2 Position = new Vector2(InterfacePos.X + 2, InterfacePos.Y + i * 30);
                            Utils.DrawBorderString(Main.spriteBatch, DescriptionText[i], Position, Color.White);
                        }
                        InterfacePos.Y += 30 * DescriptionText.Length;
                        {
                            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y - 1, RightPanelWidth, 2), null, Color.Black);
                        }
                        for (int i = 0; i < ObjectiveText.Length; i++)
                        {
                            Vector2 Position = new Vector2(InterfacePos.X + 2, InterfacePos.Y + i * 20);
                            Utils.DrawBorderString(Main.spriteBatch, ObjectiveText[i], Position, Color.White, 0.8f);
                        }
                        InterfacePos.Y += 20 * ObjectiveText.Length;
                        {
                            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y - 1, RightPanelWidth, 2), null, Color.Black);
                        }
                        MaxLines = (InterfaceHeight - 30 * DescriptionText.Length) / 20;
                        if(DescriptionText.Length > 0)
                            MaxLines -= DescriptionText.Length;
                    }
                    MaxLines--;
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)InterfacePos.Y - 1, RightPanelWidth, 2), null, Color.Black);
                    {
                        Color storyLineColor = Color.White;
                        for(int i = 0; i < MaxLines; i++)
                        {
                            int Index = i + StoryPage * MaxLines;
                            if (Index >= StoryText.Length)
                                break;
                            Vector2 Position = new Vector2(InterfacePos.X + RightPanelWidth * 0.5f, InterfacePos.Y + i * 20 + 4);
                            float Scale = 0.75f;
                            if (StoryPage == 0 && Index == 0)
                            {
                                Scale = 0.9f;
                            }
                            Utils.DrawBorderString(Main.spriteBatch, StoryText[Index], Position, storyLineColor, Scale, 0.5f);
                        }
                    }
                    int TotalPages = StoryText.Length / MaxLines;
                    if(TotalPages > 0)
                    {
                        Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePos.X, (int)(Position.Y + Height - 30), RightPanelWidth, 2), null, Color.Black);
                        const float YDiscount = 12;
                        if (StoryPage > 0)
                        {
                            Vector2 PreviousPageButtonPos = new Vector2(InterfacePos.X + 10, Position.Y + Height - YDiscount);
                            Color color = Color.White;
                            if (Main.mouseX >= PreviousPageButtonPos.X - 10 && Main.mouseX < PreviousPageButtonPos.X + 10 &&
                                Main.mouseY >= PreviousPageButtonPos.Y - 10 && Main.mouseY < PreviousPageButtonPos.Y + 10)
                            {
                                color = Color.Yellow;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                    StoryPage--;
                            }
                            Utils.DrawBorderString(Main.spriteBatch, "<", PreviousPageButtonPos, color, anchorx: 0.5f, anchory: 0.5f);
                        }
                        if (StoryPage < TotalPages)
                        {
                            Vector2 NextPageButtonPos = new Vector2(Position.X + Width - 10, Position.Y + Height - YDiscount);
                            Color color = Color.White;
                            if (Main.mouseX >= NextPageButtonPos.X - 10 && Main.mouseX < NextPageButtonPos.X + 10 &&
                                Main.mouseY >= NextPageButtonPos.Y - 10 && Main.mouseY < NextPageButtonPos.Y + 10)
                            {
                                color = Color.Yellow;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                    StoryPage++;
                            }
                            Utils.DrawBorderString(Main.spriteBatch, ">", NextPageButtonPos, color, anchorx: 0.5f, anchory: 0.5f);
                        }
                        Utils.DrawBorderString(Main.spriteBatch, "Page: " + (StoryPage + 1) + "/" + (TotalPages + 1), new Vector2(InterfacePos.X + RightPanelWidth * 0.5f, Position.Y + Height - YDiscount), Color.White, anchorx: 0.5f, anchory: 0.5f);
                    }
                }
            }
            //
            {
                Vector2 QuestTextPosition = new Vector2(Position.X + Width * 0.5f, Position.Y - 2);
                Utils.DrawBorderStringBig(Main.spriteBatch, "Quests", QuestTextPosition, Color.White, anchorx: 0.5f);
            }
            //Close X
            {
                Vector2 CloseXPosition = new Vector2(Position.X + Width, Position.Y);
                Color color = Color.Red;
                if(Main.mouseX >= CloseXPosition.X - 12 && Main.mouseX < CloseXPosition.X + 12 && 
                    Main.mouseY >= CloseXPosition.Y - 12 && Main.mouseY < CloseXPosition.Y + 12)
                {
                    color = Color.Yellow;
                    Main.LocalPlayer.mouseInterface = true;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        IsActive = false;
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, "X", CloseXPosition, color, anchorx: 0.5f, anchory: 0.5f);
            }
        }
    }
}
