using System;
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
            GuardianList = GetGuardianList(player);//player.MyGuardians.Keys.ToArray();
            Width = (int)(Main.screenWidth * 0.5f);
            Height = (int)(Main.screenHeight * 0.5f);
            if (Width < 640) Width = 640;
            if (Height < 480) Height = 480;
            InterfacePosition.X = (Main.screenWidth - Width) * 0.5f;
            InterfacePosition.Y = (Main.screenHeight - Height + 36) * 0.5f;
            ScrollY = 0;
            for (int k = 0; k < GuardianList.Length; k++)
            {
                if (player.SelectedGuardian == GuardianList[k])
                    Selected = k;
            }
            if (Selected > -1)
            {
                DisplayGuardian.Data = player.MyGuardians[GuardianList[Selected]];
                HasRequestRequiringIt = RequestData.PlayerHasRequestRequiringCompanion(player.player, DisplayGuardian.Data);
            }
        }

        public static int[] GetGuardianList(PlayerMod player)
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
            return GuardiansLivingHere.ToArray();
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
                    ButtonPos.Y += 16 + (MaxHeight - ScrollbarSize) * ScrollY;
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
                    float ElementStartY = ElementPosition.Y;
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
                        DisplayGuardian.Scale = DisplayGuardian.GetAgeSize();
                        if (DisplayGuardian.Data.IsBirthday)
                        {
                            BirthdayTime = "Turns " + DisplayGuardian.Data.GetBirthdayAge + " years old today.";
                        }
                        else
                        {
                            BirthdayTime = "Birthday in " + DisplayGuardian.Data.TimeUntilBirthdayString();
                        }
                    }
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
                if (Selected > -1 && !DisplayGuardian.Base.InvalidGuardian && (!PlayerMod.HasBuddiesModeOn(player.player) || !PlayerMod.GetPlayerBuddy(player.player).IsSameID(DisplayGuardian)) && (DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.CallUnlockLevel || DisplayGuardian.Data.IsStarter || (DisplayGuardian.request.Active && DisplayGuardian.request.RequiresGuardianActive(DisplayGuardian.Data)) || PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], DisplayGuardian.ID, DisplayGuardian.ModID)) && (player.TitanGuardian == 255 || player.TitanGuardian == player.GetGuardianSlot(GuardianList[Selected])) && ((SummonSlot < 255 && player.GetSummonedGuardianCount < player.MaxExtraGuardiansAllowed + 1) || player.GetGuardianSlot(GuardianList[Selected]) < 255))
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

        public static void DrawRectangle(float x, float y, int width, int height, Color c)
        {
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)x, (int)y, width, height), c);
        }
    }
}
