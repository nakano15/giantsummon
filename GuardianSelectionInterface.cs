﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianSelectionInterface
    {
        public static bool IsActive = false;
        public static TerraGuardian DisplayGuardian = new TerraGuardian();
        public static Vector2 InterfacePosition = Vector2.Zero;
        public static int Width = 0, Height = 0;
        public const int GuardianDisplayWidth = 128, GuardianDisplayHeight = 192;
        public static bool HoveringSummonButton = false, HoveringMoveButton = false, HoveringWikiButton = false, HoveringEquipmentButton = false, HoveringVoteButton = false;
        public static bool VoteButtonClickedOnce = false;
        public static string Age, Time, Name, BirthdayTime, ModName;
        public static int ScrollY = 0;
        public static bool HasRequestRequiringIt = false;

        public static int Selected = -1, LastSelected = -1, HoveredButton = -1;
        public static int[] GuardianList = new int[0];
        public static ContentElement[] ContentList = new ContentElement[0];
        public static byte SortingOrder = 0;
        public const byte SortByLetter = 0, SortByLivingInWorld = 1;
        const int MaxLines = 18;

        public static void OpenInterface()
        {
            Main.playerInventory = false;
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            IsActive = true;
            DisplayGuardian = new TerraGuardian();
            DisplayGuardian.LookingLeft = true;
            Selected = -1;
            LastSelected = -1;
            HasRequestRequiringIt = false;
            GetGuardianList(player);
            Width = (int)(Main.screenWidth * 0.5f);
            Height = (int)(Main.screenHeight * 0.5f);
            if (Width < 640) Width = 640;
            if (Height < 480) Height = 480;
            InterfacePosition.X = (Main.screenWidth - Width) * 0.5f;
            InterfacePosition.Y = (Main.screenHeight - Height + 36) * 0.5f;
            ScrollY = 0;
            /*for (int k = 0; k < GuardianList.Length; k++)
            {
                if (player.SelectedGuardian == GuardianList[k])
                    Selected = k;
            }
            if (Selected > -1)
            {
                DisplayGuardian.Data = player.MyGuardians[GuardianList[Selected]];
                HasRequestRequiringIt = RequestData.PlayerHasRequestRequiringCompanion(player.player, DisplayGuardian.Data);
            }*/
        }

        public static void GetGuardianList(PlayerMod player)
        {
            {
                int[] keys = new int[0];
                switch (SortingOrder)
                {
                    default:
                        keys = player.MyGuardians.OrderBy(x => x.Value.Name).Select(x => x.Key).ToArray();
                        break;
                    case SortByLivingInWorld:
                        keys = player.MyGuardians.OrderBy(x => !WorldMod.CanGuardianNPCSpawnInTheWorld(x.Value.ID)).Select(x => x.Key).ToArray();
                        break;
                }
                List<ContentElement> contents = new List<ContentElement>();
                char LastLetter = ' ';
                foreach(int k in keys)
                {
                    GuardianData gd = player.MyGuardians[k];
                    if (SortingOrder == SortByLetter)
                    {
                        char Letter = gd.Name[0];
                        if (Letter >= '0' && Letter < '9')
                        {
                            Letter = '0';
                        }
                        if (Letter != LastLetter)
                        {
                            LastLetter = Letter;
                            contents.Add(new ContentElement(Letter));
                        }
                    }
                    contents.Add(new ContentElement(k, WorldMod.CanGuardianNPCSpawnInTheWorld(gd.ID, gd.ModID)));
                }
                ContentList = contents.ToArray();
            }
            //
            {
                List<int> GuardiansLivingHere = new List<int>(), GuardiansNotLivingHere = new List<int>();
                int[] keys = player.MyGuardians.Keys.ToArray();
                foreach (int key in keys)
                {
                    GuardianData gd = player.MyGuardians[key];
                    if (WorldMod.CanGuardianNPCSpawnInTheWorld(gd.ID, gd.ModID))
                    {
                        GuardiansLivingHere.Add(key);
                    }
                    else
                    {
                        GuardiansNotLivingHere.Add(key);
                    }
                }
                GuardiansLivingHere.AddRange(GuardiansNotLivingHere);
                GuardianList = GuardiansLivingHere.ToArray();
            }
        }

        public static void DrawLevelInfoInterface(PlayerMod player)
        {
            Rectangle BarPosition = new Rectangle((int)InterfacePosition.X - 2, (int)InterfacePosition.Y - 8 - 36, Width + 4, 8+36);
            Main.spriteBatch.Draw(Main.blackTileTexture, BarPosition, Color.Black);
            BarPosition.X += 2;
            BarPosition.Y += 2 + 36;
            BarPosition.Width -= 4;
            BarPosition.Height -= 4 + 36;
            Main.spriteBatch.Draw(Main.blackTileTexture, BarPosition, Color.Gray);
            BarPosition.Width = (int)(BarPosition.Width * ((float)(player.FriendshipExp) / (player.FriendshipMaxExp - 1)));
            Main.spriteBatch.Draw(Main.blackTileTexture, BarPosition, Color.Yellow);
            float BarFillWidth = 1f / (player.FriendshipMaxExp - 1) * Width;
            for (int i = 1; i < player.FriendshipMaxExp - 1; i++)
            {
                BarPosition.X = (int)(InterfacePosition.X + BarFillWidth * i);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(BarPosition.X, BarPosition.Y, 2, BarPosition.Height), Color.Black);
            }
            Vector2 TextPosition = new Vector2(InterfacePosition.X + Width * 0.5f, InterfacePosition.Y - 4);
            Utils.DrawBorderString(Main.spriteBatch, "Friendship Rank: " + player.FriendshipLevel, TextPosition, Color.White, 1.2f, 0.5f, 1);
        }

        public static void DrawInterface()
        {
            DrawNewInterface();
            return;
            try
            {
                if (Main.playerInventory)
                    IsActive = false;
                if (!IsActive) return;
                string MouseText = "";
                float ScaleX = (float)Width / 640, ScaleY = (float)Height / 480;
                PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                DrawLevelInfoInterface(player);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePosition.X - 2, (int)InterfacePosition.Y - 2, Width + 4, Height + 4), Color.Black);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)InterfacePosition.X, (int)InterfacePosition.Y, Width, Height), Color.DarkBlue);
                Vector2 ElementPosition = InterfacePosition;
                if (Main.mouseX >= InterfacePosition.X && Main.mouseX < InterfacePosition.X + Width &&
                    Main.mouseY >= InterfacePosition.Y && Main.mouseY < InterfacePosition.Y + Height)
                    Main.player[Main.myPlayer].mouseInterface = true;
                ElementPosition.X += 8;
                ElementPosition.Y += 8;
                const int ListWidth = 164;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)ElementPosition.X, (int)ElementPosition.Y, ListWidth, Height - 16), Color.Blue);
                int MaxGuardiansY = (Height - 16) / 26;
                for (int i = 0; i < MaxGuardiansY; i++)
                {
                    int index = i + ScrollY;
                    if (index >= GuardianList.Length)
                        break;
                    GuardianData g = player.MyGuardians[GuardianList[index]];
                    Vector2 Position = ElementPosition;
                    Position.Y += i * 26;
                    Color c = Color.White;
                    if (!WorldMod.CanGuardianNPCSpawnInTheWorld(g.ID, g.ModID))
                        c = Color.Gray;
                    if (Selected == index)
                        c = Color.Yellow;
                    if (HoveredButton == index)
                        c = Color.Cyan;
                    string t = g.Name;
                    if (g.request.Active)
                        t = "*" + t;
                    Vector2 ButtonDim = Utils.DrawBorderString(Main.spriteBatch, t, Position, c, ScaleX);
                    if (Main.mouseX >= Position.X && Main.mouseX < Position.X + ButtonDim.X && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + ButtonDim.Y)
                    {
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            Selected = index;
                            DisplayGuardian.Data = g; //player.MyGuardians[GuardianList[index]];
                            HasRequestRequiringIt = RequestData.PlayerHasRequestRequiringCompanion(Main.player[Main.myPlayer], g);
                        }
                        else
                        {
                            HoveredButton = index;
                        }
                    }
                }
                ElementPosition.X += ListWidth * ScaleX;
                int MaxScrollY = GuardianList.Length - MaxGuardiansY;
                if (MaxScrollY < 0)
                    MaxScrollY = 0;
                bool ScrollbarShowing = false;
                if (MaxScrollY > 0)
                {
                    ScrollbarShowing = true;
                    ElementPosition.X -= 8f * ScaleX;
                    //Draw scroll bar
                    int MaxHeight = Height - 48;
                    int ExtraScrollHeight = GuardianList.Length - MaxGuardiansY;
                    float ScrollbarSize = ((float)MaxGuardiansY / GuardianList.Length);
                    if (ScrollbarSize > 1f)
                        ScrollbarSize = 1f;
                    ScrollbarSize *= MaxHeight;
                    Vector2 ButtonPos = ElementPosition;
                    Color background = Color.Blue, foreground = Color.Gray;
                    if (Main.mouseX >= ButtonPos.X && Main.mouseX < ButtonPos.X + 16 && Main.mouseY >= ButtonPos.Y && Main.mouseY < ButtonPos.Y + 16)
                    {
                        background = Color.LightBlue;
                        foreground = Color.White;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            ScrollY--;
                            if (ScrollY < 0)
                                ScrollY = 0;
                        }
                    }
                    DrawRectangle(ButtonPos.X, ButtonPos.Y, 16, 16, background);
                    DrawRectangle(ButtonPos.X + 8 - 1, ButtonPos.Y + 4, 2, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 2, ButtonPos.Y + 6, 4, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 3, ButtonPos.Y + 8, 6, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 4, ButtonPos.Y + 10, 8, 2, foreground);
                    ButtonPos.Y += 16 + (MaxHeight - ScrollbarSize) * (ScrollY * (1f / ExtraScrollHeight));
                    DrawRectangle(ButtonPos.X, ButtonPos.Y, 16, (int)ScrollbarSize, Color.Blue);
                    background = Color.Blue;
                    foreground = Color.Gray;
                    ButtonPos.Y = ElementPosition.Y + MaxHeight + 16;
                    if (Main.mouseX >= ButtonPos.X && Main.mouseX < ButtonPos.X + 16 && Main.mouseY >= ButtonPos.Y && Main.mouseY < ButtonPos.Y + 16)
                    {
                        background = Color.LightBlue;
                        foreground = Color.White;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            ScrollY++;
                            if (ScrollY > MaxScrollY)
                                ScrollY = MaxScrollY;
                            if (ScrollY < 0)
                                ScrollY = 0;
                        }
                    }
                    DrawRectangle(ButtonPos.X, ButtonPos.Y, 16, 16, background);
                    DrawRectangle(ButtonPos.X + 8 - 1, ButtonPos.Y + 10, 2, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 2, ButtonPos.Y + 8, 4, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 3, ButtonPos.Y + 6, 6, 2, foreground);
                    DrawRectangle(ButtonPos.X + 8 - 4, ButtonPos.Y + 4, 8, 2, foreground);

                    ElementPosition.X += 16f * ScaleX;
                }
                //DrawRectangle(ElementPosition.X, ElementPosition.Y, Width - (int)ElementPosition.X - 8, Height - (int)ElementPosition.Y - 8, Color.LightBlue);
                ElementPosition.X += 8f * ScaleX;
                float TabStartX = (int)ElementPosition.X;
                ElementPosition.X += (Width - (ElementPosition.X - InterfacePosition.X)) * 0.5f;
                ElementPosition.Y += (Height - 8) * 0.33f;
                if (Selected > -1)
                {
                    List<sbyte> InfoIcons = new List<sbyte>();
                    if (DisplayGuardian.Base.DontUseRightHand)
                        InfoIcons.Add(-1);
                    else
                        InfoIcons.Add(1);
                    if (DisplayGuardian.Base.DontUseHeavyWeapons)
                        InfoIcons.Add(-2);
                    else
                        InfoIcons.Add(2);
                    if (DisplayGuardian.Male)
                        InfoIcons.Add(4);
                    else
                        InfoIcons.Add(5);
                    if (DisplayGuardian.Base.IsTerraGuardian)
                        InfoIcons.Add(6);
                    else
                        InfoIcons.Add(-6);
                    if (DisplayGuardian.Base.OneHanded2HWeaponWield)
                        InfoIcons.Add(7);
                    else
                        InfoIcons.Add(-7);
                    if (DisplayGuardian.Base.CanDuck)
                        InfoIcons.Add(8);
                    else
                        InfoIcons.Add(-8);
                    if (DisplayGuardian.Base.ReverseMount)
                        InfoIcons.Add(9);
                    else
                        InfoIcons.Add(-9);
                    if (DisplayGuardian.Base.DrinksBeverage)
                        InfoIcons.Add(10);
                    else
                        InfoIcons.Add(-10);
                    if (!DisplayGuardian.Base.IsNocturnal)
                        InfoIcons.Add(11);
                    else
                        InfoIcons.Add(12);

                    DisplayGuardian.Position = ElementPosition + Main.screenPosition;
                    DisplayGuardian.Position.Y -= 8f;
                    DisplayGuardian.LookingLeft = false;
                    DisplayGuardian.LastFriendshipLevel = DisplayGuardian.FriendshipLevel;
                    DisplayGuardian.LastFriendshipValue = DisplayGuardian.FriendshipProgression;
                    DisplayGuardian.UpdateAnimation();
                    if (LastSelected != Selected)
                    {
                        Name = DisplayGuardian.Name;
                        if (DisplayGuardian.Data._Name != null)
                        {
                            Name += " (" + DisplayGuardian.Base.Name + ")";
                        }
                        Age = DisplayGuardian.Data.GetAgeString();
                        Time = DisplayGuardian.Data.GetTime();
                        ModName = DisplayGuardian.Data.ModID;
                        Mod mod = ModLoader.GetMod(ModName);
                        if (mod != null)
                            ModName = mod.DisplayName;
                        DisplayGuardian.Scale = DisplayGuardian.GetAgeSize() * DisplayGuardian.Base.GetScale;
                        if (DisplayGuardian.Data.IsBirthday)
                        {
                            BirthdayTime = "Turns " + DisplayGuardian.Data.GetBirthdayAge + " years old today.";
                        }
                        else
                        {
                            BirthdayTime = "Birthday in " + DisplayGuardian.Data.TimeUntilBirthdayString();
                        }
                    }
                    DisplayGuardian.Draw(true);
                    {
                        Vector2 CrownPosition = new Vector2(TabStartX, InterfacePosition.Y + 8 + 8);
                        Main.spriteBatch.Draw(MainMod.CrownTexture, CrownPosition, new Rectangle(0, 0, 16, 12), Color.White);
                        if (Main.mouseX >= CrownPosition.X && Main.mouseX < CrownPosition.X + 32 && Main.mouseY >= CrownPosition.Y && Main.mouseY < CrownPosition.Y + 24)
                        {
                            MouseText = "Popularity Contests Winner: " + DisplayGuardian.Base.GetPopularityContestsWon();
                        }
                        CrownPosition.Y -= 8;
                        CrownPosition.X += 16;
                        Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsWon().ToString(), CrownPosition, Color.White);
                        CrownPosition.X -= 16;
                        CrownPosition.Y += 22;
                        Main.spriteBatch.Draw(MainMod.CrownTexture, CrownPosition, new Rectangle(16, 0, 16, 12), Color.White);
                        if (Main.mouseX >= CrownPosition.X && Main.mouseX < CrownPosition.X + 32 && Main.mouseY >= CrownPosition.Y && Main.mouseY < CrownPosition.Y + 24)
                        {
                            MouseText = "Popularity Contests 2nd Place: " + DisplayGuardian.Base.GetPopularityContestsSecondPlace();
                        }
                        CrownPosition.Y -= 8;
                        CrownPosition.X += 16;
                        Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsSecondPlace().ToString(), CrownPosition, Color.White);
                        CrownPosition.X -= 16;
                        CrownPosition.Y += 22;
                        Main.spriteBatch.Draw(MainMod.CrownTexture, CrownPosition, new Rectangle(32, 0, 16, 12), Color.White);
                        if (Main.mouseX >= CrownPosition.X && Main.mouseX < CrownPosition.X + 32 && Main.mouseY >= CrownPosition.Y && Main.mouseY < CrownPosition.Y + 24)
                        {
                            MouseText = "Popularity Contests 3rd Place: " + DisplayGuardian.Base.GetPopularityContestsThirdPlace();
                        }
                        CrownPosition.Y -= 8;
                        CrownPosition.X += 16;
                        Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsThirdPlace().ToString(), CrownPosition, Color.White);
                    }
                    {
                        Vector2 GroupWeightPosition = new Vector2(TabStartX + 60, InterfacePosition.Y + 8 + 8);
                        float WeightValue = player.GuardianFollowersWeight * 1000,
                            NextWeightValue = DisplayGuardian.Base.CompanionSlotWeight * 1000,
                            MaxWeightValue = player.MaxGuardianFollowersWeight * 1000;
                        if(player.GetGuardianSlot(GuardianList[Selected]) < 255)
                        {
                            NextWeightValue *= -1;
                        }
                        float GroupWeightValue = WeightValue / MaxWeightValue;
                        Color color = Color.White;
                        if(GroupWeightValue > 0)
                        {
                            if (GroupWeightValue < 0.333f)
                                color = Color.Green;
                            else if (GroupWeightValue < 0.667f)
                                color = Color.Orange;
                            else
                                color = Color.Red;
                        }
                        Utils.DrawBorderString(Main.spriteBatch, "Group Size: " + (int)(WeightValue) + (NextWeightValue > 0 ? "+" + NextWeightValue : NextWeightValue.ToString()) + "=" + (WeightValue + NextWeightValue) + "/" + (int)(MaxWeightValue), GroupWeightPosition, color);
                    }
                    float ElementStartY = ElementPosition.Y;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y + 24;
                    {
                        Vector2 IconsPosition = Vector2.Zero + ElementPosition;
                        IconsPosition.X -= 24 * InfoIcons.Count * 0.5f;
                        IconsPosition.Y -= 24;
                        foreach (sbyte i in InfoIcons)
                        {
                            bool Negation = i < 0;
                            byte Val = (i >= 0 ? (byte)i : (byte)(i * -1));
                            Vector2 IconPos = IconsPosition;
                            IconPos.X += 3;
                            if (Main.mouseX >= IconPos.X && Main.mouseX < IconPos.X + 24 && Main.mouseY >= IconPos.Y && Main.mouseY < IconPos.Y + 24)
                            {
                                switch (Val)
                                {
                                    case 1:
                                        if (Negation)
                                            MouseText = "Doesn't use Right Hand";
                                        else
                                            MouseText = "Use Right Hand";
                                        break;
                                    case 2:
                                        if (Negation)
                                        {
                                            MouseText = "Doesn't Use Two Handed Weapons";
                                        }
                                        else
                                        {
                                            MouseText = "Use Two Handed Weapons";
                                        }
                                        break;
                                    case 3:
                                        if (Negation)
                                        {
                                            MouseText = "Doesn't Use Player Armor as Vanity";
                                        }
                                        else
                                        {
                                            MouseText = "Use Player Armor as Vanity";
                                        }
                                        break;
                                    case 4:
                                        MouseText = "Male";
                                        break;
                                    case 5:
                                        MouseText = "Female";
                                        break;
                                    case 6:
                                        if (Negation)
                                        {
                                            MouseText = "Terra Creature";
                                        }
                                        else
                                        {
                                            MouseText = "Ether Realm Creature";
                                        }
                                        break;
                                    case 7:
                                        if (Negation)
                                        {
                                            MouseText = "Uses Both Hands to wield Heavy Weapons.";
                                        }
                                        else
                                        {
                                            MouseText = "Wield Heavy Weapons with one hand";
                                        }
                                        break;
                                    case 8:
                                        if (Negation)
                                        {
                                            MouseText = "Can't Duck";
                                        }
                                        else
                                        {
                                            MouseText = "Can Duck";
                                        }
                                        break;
                                    case 9:
                                        if (Negation)
                                        {
                                            MouseText = "Player Mounts on the Guadian";
                                        }
                                        else
                                        {
                                            MouseText = "Guardian Mounts on Player back";
                                        }
                                        break;
                                    case 10:
                                        if (Negation)
                                        {
                                            MouseText = "Doesn't Drink Alcoholic Drinks";
                                        }
                                        else
                                        {
                                            MouseText = "Drinks Alcoholic Drinks";
                                        }
                                        break;
                                    case 11:
                                        MouseText = "Diurnal";
                                        break;
                                    case 12:
                                        MouseText = "Nocturnal";
                                        break;
                                }
                            }
                            Main.spriteBatch.Draw(MainMod.GuardianInfoIcons, IconPos, new Rectangle(Val * 16, 0, 16, 16), Color.White);
                            if (Negation)
                                Main.spriteBatch.Draw(MainMod.GuardianInfoIcons, IconPos, new Rectangle(0, 0, 16, 16), Color.White);
                            IconsPosition.X += 24;
                        }
                    }
                    Vector2 NameDim = Utils.DrawBorderString(Main.spriteBatch, Name, ElementPosition, Color.White, ScaleY, 0.5f);
                    {
                        Vector2 FriendshipLevelPos = ElementPosition;
                        FriendshipLevelPos.X -= NameDim.X * 0.5f + 26;
                        DisplayGuardian.DrawFriendshipHeart(FriendshipLevelPos);

                        if (DisplayGuardian.Data.CanChangeName)
                        {
                            Vector2 RenamePosition = ElementPosition;
                            RenamePosition.X += NameDim.X * 0.5f + 8;
                            Main.spriteBatch.Draw(MainMod.EditButtonTexture, RenamePosition, null, Color.White);
                            if (Main.mouseX >= RenamePosition.X && Main.mouseX < RenamePosition.X + 16 && Main.mouseY >= RenamePosition.Y && Main.mouseY < RenamePosition.Y + 16)
                            {
                                MouseText = (DisplayGuardian.Data.Name == null ? "You can set a nickname to the guardian by clicking here." : "You can change your guardian nickname by clicking here.");
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    int GuardianPos = GuardianList[Selected];
                                    Main.chatText = "/renameguardian " + player.MyGuardians[GuardianPos].ID + " " + player.MyGuardians[GuardianPos].ModID + " ";
                                    Main.drawingPlayerChat = true;
                                    IsActive = false;
                                }
                            }
                        }
                    }
                    ElementPosition.Y += NameDim.Y + 8f * ScaleY;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y;
                    ElementPosition.Y += Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.Description, ElementPosition, Color.White, ScaleY, 0.5f).Y + 8f * ScaleY;
                    ElementPosition.X = InterfacePosition.X + 16 * ScaleX + ListWidth;
                    if (ScrollbarShowing)
                        ElementPosition.X += 16;
                    ElementStartY = ElementPosition.Y;
                    //
                    List<string> ExtraInfoList = new List<string>();
                    ExtraInfoList.Add("Age: " + Age);
                    ExtraInfoList.Add(DisplayGuardian.Base.GetGroup.Name);
                    ExtraInfoList.Add("Time: " + Time);
                    ExtraInfoList.Add("Size: " + DisplayGuardian.Base.Size.ToString());
                    ExtraInfoList.Add("Friend Grade: " + DisplayGuardian.FriendshipGradeText);
                    if (MainMod.UsingGuardianNecessitiesSystem)
                        ExtraInfoList.Add("Status: " + DisplayGuardian.Data.GetNecessityStatus);
                    if (DisplayGuardian.Data.request.requestState == RequestData.RequestState.HasExistingRequestReady)
                        ExtraInfoList.Add("Has a request for you.");
                    ExtraInfoList.Add(BirthdayTime);
                    ExtraInfoList.Add("Mod: " + ModName);
                    //
                    //DrawRectangle(ElementPosition.X, ElementPosition.Y, Width - (int)ElementPosition.X - 8, Height - (int)ElementPosition.Y - 8, Color.LightBlue);
                    foreach (string s in ExtraInfoList)
                    {
                        if (ElementPosition.Y + 26 * 2 >= InterfacePosition.Y + Height - 8f)
                        {
                            ElementPosition.X += 164f;
                            ElementPosition.Y = ElementStartY;
                        }
                        ElementPosition.X = (int)ElementPosition.X;
                        ElementPosition.Y = (int)ElementPosition.Y;
                        Utils.DrawBorderString(Main.spriteBatch, s, ElementPosition, Color.White, ScaleY);
                        ElementPosition.Y += 26;
                    }
                }
                LastSelected = Selected;
                ElementPosition.X = InterfacePosition.X + (ListWidth + 8) * ScaleX;
                ElementPosition.Y = InterfacePosition.Y + Height - 18;
                int ButtonRegionWidth = (int)(InterfacePosition.X + Width - ElementPosition.X - 8);
                byte SummonSlot = player.GetEmptyGuardianSlot();
                ElementPosition.X += ButtonRegionWidth * 0.25f;
                if (Selected > -1 && !DisplayGuardian.Base.InvalidGuardian && (!PlayerMod.HasBuddiesModeOn(player.player) || !PlayerMod.GetPlayerBuddy(player.player).IsSameID(DisplayGuardian)) && (DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.CallUnlockLevel || DisplayGuardian.Data.IsStarter || (DisplayGuardian.request.Active && DisplayGuardian.request.RequiresGuardianActive(DisplayGuardian.Data)) || PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], DisplayGuardian.ID, DisplayGuardian.ModID)) && (player.TitanGuardian == 255 || player.TitanGuardian == player.GetGuardianSlot(GuardianList[Selected])) && ((SummonSlot < 255 && (player.GuardianFollowersWeight == 0 || player.GuardianFollowersWeight < player.MaxExtraGuardiansAllowed + DisplayGuardian.Base.CompanionSlotWeight)) || player.GetGuardianSlot(GuardianList[Selected]) < 255))
                {
                    string ButtonText = "Call";
                    bool IsCallButton = player.GetGuardianSlot(GuardianList[Selected]) == 255;
                    if (!IsCallButton)
                        ButtonText = "Dismiss";
                    else if (SummonSlot > 0 && SummonSlot < 255)
                    {
                        ButtonText += " Assist";
                    }
                    Color ButtonColor = Color.White;
                    if (HoveringSummonButton)
                        ButtonColor = Color.Yellow;
                    HoveringSummonButton = false;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y;
                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ElementPosition, ButtonColor, 1.2f, 0.5f, 0.5f);
                    if (Main.mouseX >= ElementPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ElementPosition.X + ButtonDimension.X * 0.5f &&
                        Main.mouseY >= ElementPosition.Y - ButtonDimension.Y * 0.5f && Main.mouseY < ElementPosition.Y + ButtonDimension.Y * 0.5f)
                    {
                        HoveringSummonButton = true;
                        MouseText = "Calls the guardian to help you on your adventure, or dismiss It.";
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (IsCallButton)
                            {
                                player.CallGuardian(GuardianList[Selected], SummonSlot);
                                foreach (TerraGuardian tg in player.GetAllGuardianFollowers)
                                {
                                    if (tg.ID == DisplayGuardian.ID && tg.ModID == DisplayGuardian.ModID)
                                    {
                                        string mes = tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess);
                                        if (mes != "")
                                        {
                                            tg.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(mes, tg));
                                        }
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                TerraGuardian tg = player.GetAllGuardianFollowers.First(x => x.ID == DisplayGuardian.ID && x.ModID == DisplayGuardian.ModID);
                                string Mes = tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer);
                                if (Mes != "")
                                    Main.NewText(tg.Name + ": " + GuardianMouseOverAndDialogueInterface.MessageParser(Mes, tg));
                                player.DismissGuardian(player.GetGuardianSlot(GuardianList[Selected]));
                            }
                        }
                    }
                }
                else
                {

                }
                ElementPosition.X += ButtonRegionWidth * 0.25f;
                if (Selected > -1 && (DisplayGuardian.Data.IsStarter || DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.MoveInLevel))
                {
                    string ButtonText = "";
                    if (WorldMod.CanGuardianNPCSpawnInTheWorld(DisplayGuardian.ID, DisplayGuardian.ModID))
                    {
                        ButtonText = "Send Home";
                    }
                    else
                    {
                        if (WorldMod.HasEmptyGuardianNPCSlot())
                        {
                            ButtonText = "Allow Move In";
                        }
                    }
                    Color ButtonColor = Color.White;
                    if (HoveringMoveButton)
                        ButtonColor = Color.Yellow;
                    HoveringMoveButton = false;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y;
                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ElementPosition, ButtonColor, 1.2f, 0.5f, 0.5f);
                    if (Main.mouseX >= ElementPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ElementPosition.X + ButtonDimension.X * 0.5f &&
                        Main.mouseY >= ElementPosition.Y - ButtonDimension.Y * 0.5f && Main.mouseY < ElementPosition.Y + ButtonDimension.Y * 0.5f)
                    {
                        HoveringMoveButton = true;
                        MouseText = "Toggles if the Guardian can move in to a house in your world or not.";
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (WorldMod.CanGuardianNPCSpawnInTheWorld(DisplayGuardian.ID, DisplayGuardian.ModID))
                            {
                                WorldMod.RemoveGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                            }
                            else
                            {
                                WorldMod.AllowGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                            }
                        }
                    }
                }
                ElementPosition.X += ButtonRegionWidth * 0.25f;
                if(Selected > -1)
                {
                    string ButtonText = "Wiki";
                    Color ButtonColor = Color.White;
                    if (HoveringWikiButton)
                        ButtonColor = Color.Yellow;
                    HoveringWikiButton = false;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y;
                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ElementPosition, ButtonColor, 1.2f, 0.5f, 0.5f);
                    if (Main.mouseX >= ElementPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ElementPosition.X + ButtonDimension.X * 0.5f &&
                        Main.mouseY >= ElementPosition.Y - ButtonDimension.Y * 0.5f && Main.mouseY < ElementPosition.Y + ButtonDimension.Y * 0.5f)
                    {
                        HoveringWikiButton = true;
                        MouseText = "Opens the wiki for this companion.";
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            string Url = "https://nakano15-mods.fandom.com/wiki/";
                            if (DisplayGuardian.Base.WikiPageLink != null)
                                Url += DisplayGuardian.Base.WikiPageLink;
                            else
                                Url += DisplayGuardian.Base.Name;
                            System.Diagnostics.Process.Start(Url);
                        }
                    }
                }
                if(Selected > -1)
                {
                    string ButtonText = "Inventory";
                    Color ButtonColor = Color.White;
                    if (HoveringEquipmentButton)
                        ButtonColor = Color.Yellow;
                    HoveringEquipmentButton = false;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y - 28f;
                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ElementPosition, ButtonColor, 1.2f, 0.5f, 0.5f);
                    if (Main.mouseX >= ElementPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ElementPosition.X + ButtonDimension.X * 0.5f &&
                        Main.mouseY >= ElementPosition.Y - ButtonDimension.Y * 0.5f && Main.mouseY < ElementPosition.Y + ButtonDimension.Y * 0.5f)
                    {
                        HoveringEquipmentButton = true;
                        MouseText = "Manages Inventory.";
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (GuardianManagement.OpenInterfaceForGuardian(GuardianList[Selected]))
                            {
                                IsActive = false;
                            }
                        }
                    }
                    ElementPosition.Y += 28f;
                }
                ElementPosition.X = InterfacePosition.X + (ListWidth + 8) * ScaleX + 22f;
                if (false && !VoteButtonClickedOnce) //Disabled because there is no contest.
                {
                    string ButtonText = "Vote";
                    Color ButtonColor = Color.White;
                    if (HoveringVoteButton)
                    {
                        ButtonColor = Color.Yellow;
                        MouseText = "This will open your browser. Vote on your favorite TerraGuardians.";
                    }
                    HoveringVoteButton = false;
                    ElementPosition.X = (int)ElementPosition.X;
                    ElementPosition.Y = (int)ElementPosition.Y;
                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ElementPosition, ButtonColor, 1.2f, 0.5f, 0.5f);
                    if (Main.mouseX >= ElementPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ElementPosition.X + ButtonDimension.X * 0.5f &&
                        Main.mouseY >= ElementPosition.Y - ButtonDimension.Y * 0.5f && Main.mouseY < ElementPosition.Y + ButtonDimension.Y * 0.5f)
                    {
                        HoveringVoteButton = true;
                        Main.player[Main.myPlayer].mouseInterface = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            System.Diagnostics.Process.Start(MainMod.VoteLink);
                            VoteButtonClickedOnce = true;
                        }
                    }
                }
                //if(Selected > -1) //?
                {
                    Vector2 CloseButtonPosition = InterfacePosition;
                    CloseButtonPosition.X += Width - 4;
                    CloseButtonPosition.Y += 4;
                    const float CloseButtonSize = 0.85f;
                    Color color = Color.Red;
                    if(Main.mouseX >= CloseButtonPosition.X - 60 * CloseButtonSize && Main.mouseX < CloseButtonPosition.X && 
                        Main.mouseY >= CloseButtonPosition.Y + 4 * CloseButtonSize && Main.mouseY < CloseButtonPosition.Y + 22 * CloseButtonSize)
                    {
                        color = Color.Yellow;
                        if(Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            IsActive = false;
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Close", CloseButtonPosition, color, CloseButtonSize, 1, 0);
                }
                if (MouseText != "")
                {
                    Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 12f, Main.mouseY + 12f), Color.White);
                }
                if (!IsActive)
                    DisplayGuardian = null;
            }
            catch
            {

            }
        }

        private static void ChangeSelectedGuardian(int Position, PlayerMod player)
        {
            if (Position < 0 || Position >= ContentList.Length)
                return;
            ContentElement element = ContentList[Position];
            if (!element.Letter && player.MyGuardians.ContainsKey(element.Index))
            {
                Selected = Position;
                DisplayGuardian.Data = player.MyGuardians[element.Index];
                Age = DisplayGuardian.Data.GetAgeString();
                Time = DisplayGuardian.Data.GetTime();
                ModName = DisplayGuardian.Data.ModID;
                Mod mod = ModLoader.GetMod(ModName);
                if (mod != null)
                    ModName = mod.DisplayName;
                DisplayGuardian.Scale = DisplayGuardian.GetAgeSize() * DisplayGuardian.Base.GetScale;
                if (DisplayGuardian.Data.IsBirthday)
                {
                    BirthdayTime = "Turns " + DisplayGuardian.Data.GetBirthdayAge + " years old today.";
                }
                else
                {
                    BirthdayTime = "Birthday: " + DisplayGuardian.Data.TimeUntilBirthdayString();
                }
            }
        }

        public static void DrawNewInterface()
        {
            if (Main.playerInventory)
                IsActive = false;
            if (!IsActive)
                return;
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            Vector2 HudPosition = new Vector2(Main.screenWidth * 0.5f - 240, Main.screenHeight * 0.5f - 240);
            Main.spriteBatch.Draw(MainMod.GSI_BackgroundInterfaceTexture, HudPosition, Color.White);
            if (Main.mouseX >= HudPosition.X + 22 && Main.mouseX < HudPosition.X + 470 &&
               Main.mouseY >= HudPosition.Y + 54 && Main.mouseY < HudPosition.Y + 447)
            {
                player.player.mouseInterface = true;
            }
            if (Main.mouseX >= HudPosition.X + 180 && Main.mouseX < HudPosition.X + 23 &&
               Main.mouseY >= HudPosition.Y + 436 && Main.mouseY < HudPosition.Y + 62)
            {
                player.player.mouseInterface = true;
            }
            DrawNameList(HudPosition, player);
            DrawWeightBar(HudPosition, player, Selected > -1 ? (DisplayGuardian.Base.CompanionSlotWeight * (!PlayerMod.HasGuardianSummoned(player.player, DisplayGuardian.ID, DisplayGuardian.ModID) ? 1 : -1)) : 0);
            if(Selected > -1)
            {
                const float ElementScale = 0.7f;
                const int ElementCenterX = 175 + 131;
                {
                    int BarWidth = (int)((float)player.FriendshipExp / player.FriendshipMaxExp * 262);
                    Vector2 RankPosition = Vector2.Zero;
                    RankPosition.X = HudPosition.X + 175;
                    RankPosition.Y = HudPosition.Y + 87;
                    Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, RankPosition, new Rectangle(185, 87, BarWidth, 5), Color.White);
                    RankPosition.X = HudPosition.X + ElementCenterX;
                    RankPosition.Y = HudPosition.Y + 84 + 6;
                    Utils.DrawBorderString(Main.spriteBatch, "Friendship Rank: " + player.FriendshipLevel, RankPosition, Color.White, ElementScale, 0.5f, 1f);

                }
                {
                    Vector2 TgPos = Vector2.Zero;
                    TgPos.X = Main.screenPosition.X + HudPosition.X + ElementCenterX;
                    TgPos.Y = Main.screenPosition.Y + HudPosition.Y + 235;
                    DisplayGuardian.Position = TgPos;
                    DisplayGuardian.Draw(true);
                }
                {
                    Vector2 NamePos = Vector2.Zero;
                    NamePos.X = HudPosition.X + ElementCenterX;
                    NamePos.Y = HudPosition.Y + 253 + 6;
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Name, NamePos, Color.White, ElementScale, 0.5f, 1);
                }
                {
                    //int lines;
                    string[] DescriptionTextParsed = DisplayGuardian.Base.Description.Split('\n'); //Utils.WordwrapString(DisplayGuardian.Base.Description, Main.fontMouseText, (int)(261 * (1f / (ElementScale != 0 ? ElementScale : 1))), 5, out lines);
                    for (int l = 0; l < DescriptionTextParsed.Length; l++)
                    {
                        Vector2 DescriptionPos = Vector2.Zero;
                        DescriptionPos.X = HudPosition.X + ElementCenterX;
                        DescriptionPos.Y = HudPosition.Y + 255 + 12 * l;
                        Utils.DrawBorderString(Main.spriteBatch, DescriptionTextParsed[l], DescriptionPos, Color.White, ElementScale, 0.5f, 0);
                    }
                }
                {
                    Vector2 ExtraInfosPos = Vector2.Zero;
                    ExtraInfosPos.X = HudPosition.X + ElementCenterX;
                    ExtraInfosPos.Y = HudPosition.Y + 323 + 6;
                    Utils.DrawBorderString(Main.spriteBatch, "Extra Infos", ExtraInfosPos, Color.White, ElementScale, 0.5f, 1f);
                    //
                    List<string> InfosText = new List<string>();
                    {
                        int AgeVal = DisplayGuardian.Age, YearlyAgeVal = DisplayGuardian.Data.YearlyAge;
                        InfosText.Add("Age: " + DisplayGuardian.Age + (YearlyAgeVal != AgeVal ? "(" + YearlyAgeVal + " Years Old)" : ""));
                    }
                    InfosText.Add("Size: " + DisplayGuardian.Base.Size.ToString());
                    InfosText.Add(DisplayGuardian.GetGroup.Name);
                    InfosText.Add("Mod:" + DisplayGuardian.ModID);
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            int index = y * 2 + x;
                            if (index >= InfosText.Count)
                                break;
                            ExtraInfosPos.X = HudPosition.X + 175 + 130 * x;
                            ExtraInfosPos.Y = HudPosition.Y + 326 - 2 + 10 * y;
                            Utils.DrawBorderString(Main.spriteBatch, InfosText[index], ExtraInfosPos, Color.White, ElementScale);
                        }
                    }
                }
            }
            //Dismiss Button (left)
            if (Selected > -1 && !DisplayGuardian.Base.InvalidGuardian &&
                (!PlayerMod.HasBuddiesModeOn(player.player) || !PlayerMod.GetPlayerBuddy(player.player).IsSameID(DisplayGuardian)) &&
                (DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.CallUnlockLevel || DisplayGuardian.Data.IsStarter || (DisplayGuardian.request.Active && DisplayGuardian.request.RequiresGuardianActive(DisplayGuardian.Data)) ||
                PlayerMod.PlayerHasGuardianSummoned(player.player, DisplayGuardian.ID, DisplayGuardian.ModID)) &&
                (player.TitanGuardian == 255 || player.TitanGuardian == player.GetGuardianSlot(ContentList[Selected].Index)) &&
                ((player.GetEmptyGuardianSlot() < 255 && (player.GuardianFollowersWeight == 0 || player.GuardianFollowersWeight + DisplayGuardian.Base.CompanionSlotWeight < player.MaxGuardianFollowersWeight)) ||
                player.GetGuardianSlot(ContentList[Selected].Index) < 255))
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 175 + 39;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                bool MouseOver = false;
                string Text = "Call";
                bool IsCallButton = player.GetGuardianSlot(ContentList[Selected].Index) == 255;
                byte SummonSlot = player.GetEmptyGuardianSlot();
                if (!IsCallButton)
                {
                    Text = "Dismiss";
                }
                else if(SummonSlot > 0 && SummonSlot < 255)
                {
                    Text += " Assist";
                }
                if(Math.Abs(Main.mouseX - ButtonCenter.X) < 39 && 
                    Math.Abs(Main.mouseY - ButtonCenter.Y) < 14)
                {
                    MouseOver = true;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (IsCallButton)
                        {
                            player.CallGuardian(ContentList[Selected].Index, SummonSlot);
                            foreach(TerraGuardian tg in player.GetAllGuardianFollowers)
                            {
                                if(tg.Active && tg.ID == DisplayGuardian.ID && tg.ModID == DisplayGuardian.ModID)
                                {
                                    string mes = tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess);
                                    if(mes != "")
                                    {
                                        tg.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(mes, tg));
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            TerraGuardian tg = PlayerMod.GetPlayerSummonedGuardian(player.player, DisplayGuardian.ID, DisplayGuardian.ModID);
                            string Mes = tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer);
                            if (Mes != "")
                                Main.NewText(tg.Name + ": " + GuardianMouseOverAndDialogueInterface.MessageParser(Mes, tg));
                            player.DismissGuardian(player.GetGuardianSlot(ContentList[Selected].Index));
                        }
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), 1f, 0.5f, 0.5f);
            }
            //Home Button (Center)
            if (Selected > -1 && (DisplayGuardian.Data.IsStarter || DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.MoveInLevel))
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 265 + 41;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                bool MouseOver = false;
                string Text = "";
                bool IsMoveout = false;
                if(WorldMod.CanGuardianNPCSpawnInTheWorld(DisplayGuardian.ID, DisplayGuardian.ModID))
                {
                    IsMoveout = true;
                    Text = "Send Home";
                }
                else if(WorldMod.HasEmptyGuardianNPCSlot())
                {
                    Text = "Allow Move In";
                }
                if (Text != "")
                {
                    if (Math.Abs(Main.mouseX - ButtonCenter.X) < 41 &&
                        Math.Abs(Main.mouseY - ButtonCenter.Y) < 14)
                    {
                        MouseOver = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (IsMoveout)
                                WorldMod.RemoveGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                            else
                                WorldMod.AllowGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), 1f, 0.5f, 0.5f);
                }
            }
            //Inventory Button (Right)
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 360 + 39;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                bool MouseOver = false;
                string Text = "Inventory";
                if (Math.Abs(Main.mouseX - ButtonCenter.X) < 39 &&
                    Math.Abs(Main.mouseY - ButtonCenter.Y) < 14)
                {
                    MouseOver = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {

                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), 1f, 0.5f, 0.5f);
            }
        }

        public static void DrawWeightBar(Vector2 InterfacePosition, PlayerMod player, float SelectedGuardianWeight)
        {
            Vector2 BarPosition = Vector2.Zero;
            BarPosition.X = InterfacePosition.X + 184;
            BarPosition.Y = InterfacePosition.Y + 34;
            const int BarWidth = 248, BarHeight = 28;
                int CurrentWeight = (int)(player.GuardianFollowersWeight * 1000), GuardianWeightValue = (int)(SelectedGuardianWeight * 1000), MaxWeight = (int)(player.MaxGuardianFollowersWeight * 1000);
            {
                int FirstBarSize = (int)((float)(CurrentWeight + (GuardianWeightValue > 0 ? 0 : GuardianWeightValue)) / MaxWeight * BarWidth),
                    SecondBarSize = (int)((float)Math.Abs(GuardianWeightValue) / MaxWeight * BarWidth);
                if(SelectedGuardianWeight > 0 && FirstBarSize + SecondBarSize > BarWidth)
                {
                    FirstBarSize = 0;
                    SecondBarSize = BarWidth;
                }
                Rectangle rect = new Rectangle((int)BarPosition.X, (int)BarPosition.Y, FirstBarSize, BarHeight);
                Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, rect, new Rectangle(266, 34, 88, 28), Color.White);
                if (SelectedGuardianWeight != 0)
                {
                    rect.X += FirstBarSize;
                    rect.Width = SecondBarSize;
                    Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, rect, new Rectangle(SelectedGuardianWeight < 0 ? 186 : 358, 34, 70, 28), Color.White);
                }
            }
            BarPosition.X += BarWidth * 0.5f;
            BarPosition.Y = InterfacePosition.Y + 56 + 6;
            Utils.DrawBorderString(Main.spriteBatch, "Weight: " + CurrentWeight + "/" + MaxWeight, BarPosition, Color.White, 1f, 0.5f, 1f);
        }

        public static void DrawNameList(Vector2 InterfaceStartPosition, PlayerMod player)
        {
            Vector2 ElementPosition = Vector2.Zero;
            // Scroll Bar
            {
                ElementPosition.X = InterfaceStartPosition.X + 152;
                ElementPosition.Y = InterfaceStartPosition.Y + 66;
                const int ScrollBarMaxHeight = 366;
                float ScrollBarSize = 1f;
                int OverflowValue = ContentList.Length - MaxLines;
                if(OverflowValue > 0)
                {
                    ScrollBarSize = (float)MaxLines / ContentList.Length;
                    ElementPosition.Y += (ScrollBarMaxHeight * ((1f / OverflowValue) * (1f - ScrollBarSize)));
                }
                ScrollBarSize *= ScrollBarMaxHeight;
                for(int i = 0; i < 3; i++)
                {
                    Vector2 ThisPosition = ElementPosition;
                    int dy = 66, dh = 0;
                    int scaley = 0;
                    switch (i)
                    {
                        case 0: //bg
                            scaley = (int)ScrollBarSize;
                            dy = 96;
                            dh = 110;
                            break;
                        case 1: //upper
                            dh = 28;
                            break;
                        case 2: //lower
                            dh = 28;
                            if (ScrollBarSize < 56)
                                ThisPosition.Y += 28;
                            else
                            {
                                ThisPosition.Y += ScrollBarMaxHeight - 28;
                            }
                            break;
                    }
                    Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, new Rectangle((int)ThisPosition.X, (int)ThisPosition.Y, 12, scaley), new Rectangle(152, dy, 12, dh), Color.White);
                }
            }
            // Companion List
            ElementPosition.X = InterfaceStartPosition.X + 30;
            ElementPosition.Y = InterfaceStartPosition.Y + 96 + 6;
            const float ElementScale = 0.7f;
            Utils.DrawBorderString(Main.spriteBatch, "Companions", ElementPosition, Color.White, ElementScale, 0f, 1f);
            ElementPosition.Y += 14;
            for(int element = 0; element < MaxLines; element++)
            {
                int index = element + ScrollY;
                if(index >= ContentList.Length)
                {
                    break;
                }
                ContentElement ce = ContentList[index];
                if (ce.Letter)
                {
                    Utils.DrawBorderString(Main.spriteBatch, " " + ((char)ce.Index == '0' ? "0~9" : ((char)ce.Index).ToString()), ElementPosition, Color.Brown, ElementScale, 0f, 1f);
                }
                else
                {
                    GuardianData gd = player.MyGuardians[ce.Index];
                    Color color = Selected == index ? Color.Yellow : (ce.LivingHere ? Color.White : Color.Gray);
                    if(Main.mouseX >= ElementPosition.X && Main.mouseX < ElementPosition.X + 34 && 
                        Main.mouseY >= ElementPosition.Y - 14 && Main.mouseY < ElementPosition.Y - 4)
                    {
                        color = Color.Cyan;
                        if(Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            ChangeSelectedGuardian(index, player);
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, gd.Name, ElementPosition, color, ElementScale, 0f, 1f);
                }
                ElementPosition.Y += 14;
            }
        }

        public static void DrawRectangle(float x, float y, int width, int height, Color c)
        {
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)x, (int)y, width, height), c);
        }

        public struct ContentElement
        {
            public int Index;
            public bool Letter, LivingHere;

            public ContentElement(int MyTgID, bool LivingHere)
            {
                Index = MyTgID;
                Letter = false;
                this.LivingHere = LivingHere;
            }

            public ContentElement(char Letter)
            {
                Index = Letter;
                this.Letter = true;
                LivingHere = false;
            }
        }
    }
}
