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
        public static string Age, Time, Name, BirthdayTime, BirthdayDate, ModName;
        public static int ScrollY = 0;
        public static bool HasRequestRequiringIt = false;

        public static int Selected = -1, LastSelected = -1, HoveredButton = -1;
        public static int[] GuardianList = new int[0];
        public static ContentElement[] ContentList = new ContentElement[0];
        public static byte SortingOrder = 0;
        public const byte SortByLetter = 0, SortByLivingInWorld = 1, SortBySize = 2, SortByWeight = 3, SortByHeight = 4, SortByWidth = 5;
        public const int MaxLines = 24;
        public const int MaxDrawWidth = 262, MaxDrawHeight = 133;
        public static int ContributionIconAnimationTime = 0;
        private static List<string> InfosText = new List<string>();
        private static List<sbyte> InfoIcons = new List<sbyte>();

        public static void OpenInterface()
        {
            Main.playerInventory = false;
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            IsActive = true;
            DisplayGuardian = new TerraGuardian();
            Selected = -1;
            LastSelected = -1;
            HasRequestRequiringIt = false;
            GetGuardianList(player);
            PickMainGuardian(player);
            Width = (int)(Main.screenWidth * 0.5f);
            Height = (int)(Main.screenHeight * 0.5f);
            if (Width < 640) Width = 640;
            if (Height < 480) Height = 480;
            InterfacePosition.X = (Main.screenWidth - Width) * 0.5f;
            InterfacePosition.Y = (Main.screenHeight - Height + 36) * 0.5f;
            ScrollY = 0;
            UpdateButtonStates(player);
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

        public static void PickMainGuardian(PlayerMod player)
        {
            if (!player.Guardian.Active)
            {
                Selected = -1;
            }
            else
            {
                for(int i = 0; i < ContentList.Length; i++)
                {
                    if(ContentList[i].SortingType == ContentElement.SortTypes.GuardianID && ContentList[i].Index == player.SelectedGuardian)
                    {
                        ChangeSelectedGuardian(i, player);
                        return;
                    }
                }
            }
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
                    case SortBySize:
                        keys = player.MyGuardians.OrderBy(x => x.Value.Base.Size).Select(x => x.Key).ToArray();
                        break;
                    case SortByWeight:
                        keys = player.MyGuardians.OrderBy(x => x.Value.Base.CompanionSlotWeight).Select(x => x.Key).ToArray();
                        break;
                    case SortByWidth:
                        keys = player.MyGuardians.OrderBy(x => x.Value.Base.Width * x.Value.Base.Scale).Select(x => x.Key).ToArray();
                        break;
                    case SortByHeight:
                        keys = player.MyGuardians.OrderBy(x => x.Value.Base.Height * x.Value.Base.Scale).Select(x => x.Key).ToArray();
                        break;
                }
                List<ContentElement> contents = new List<ContentElement>();
                int LastValue = -1;
                foreach(int k in keys)
                {
                    GuardianData gd = player.MyGuardians[k];
                    switch (SortingOrder)
                    {
                        case SortByLetter:
                            char Letter = gd.Name[0];
                            if (Letter >= '0' && Letter < '9')
                            {
                                Letter = '0';
                            }
                            if (Letter != LastValue)
                            {
                                LastValue = Letter;
                                contents.Add(new ContentElement(Letter));
                            }
                            break;
                        case SortBySize:
                            {
                                int SizeValue = (int)gd.Base.Size;
                                if(SizeValue != LastValue)
                                {
                                    LastValue = SizeValue;
                                    contents.Add(new ContentElement(gd.Base.Size));
                                }
                            }
                            break;
                        case SortByWeight:
                            {
                                int WeightValue = (int)(gd.Base.CompanionSlotWeight * 1000);
                                int Value = 0;
                                if (WeightValue >= 2400)
                                    Value = 5;
                                else if (WeightValue >= 1800)
                                    Value = 4;
                                else if (WeightValue >= 1200)
                                    Value = 3;
                                else if (WeightValue >= 800)
                                    Value = 2;
                                else if (WeightValue >= 500)
                                    Value = 1;
                                if (LastValue != Value)
                                {
                                    LastValue = Value;
                                    contents.Add(new ContentElement(Value, 0));
                                }
                            }
                            break;
                        case SortByHeight:
                            {
                                int WeightValue = (int)(Math.Max(gd.Base.Width, gd.Base.Height) * gd.Base.Scale);
                                int Value = 0;
                                if (WeightValue >= 286)
                                    Value = 6;
                                else if (WeightValue >= 134)
                                    Value = 5;
                                else if (WeightValue >= 82)
                                    Value = 4;
                                else if (WeightValue >= 60)
                                    Value = 3;
                                else if (WeightValue >= 28)
                                    Value = 2;
                                else if (WeightValue >= 12)
                                    Value = 1;
                                if (LastValue != Value)
                                {
                                    LastValue = Value;
                                    contents.Add(new ContentElement(Value, 1));
                                }
                            }
                            break;
                    }
                    contents.Add(new ContentElement(k, WorldMod.CanGuardianNPCSpawnInTheWorld(gd.ID, gd.ModID), PlayerMod.HasGuardianSummoned(Main.player[Main.myPlayer], gd.ID, gd.ModID)));
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

        private static void ChangeSelectedGuardian(int Position, PlayerMod player)
        {
            if (Position < 0 || Position >= ContentList.Length || Position == Selected)
                return;
            ContentElement element = ContentList[Position];
            if (element.SortingType == ContentElement.SortTypes.GuardianID && player.MyGuardians.ContainsKey(element.Index))
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
                if (DisplayGuardian.Height * DisplayGuardian.Scale > MaxDrawHeight)
                {
                    DisplayGuardian.Scale *= (float)MaxDrawHeight / (DisplayGuardian.Height * DisplayGuardian.Scale);
                }
                else if (DisplayGuardian.Width * DisplayGuardian.Scale > MaxDrawWidth)
                {
                    DisplayGuardian.Scale *= (float)MaxDrawWidth / (DisplayGuardian.Width * DisplayGuardian.Scale);
                }
                DisplayGuardian.Data.UpdateAge();
                DisplayGuardian.LookingLeft = false;
                if (DisplayGuardian.Data.IsBirthday)
                {
                    BirthdayTime = "Turns " + DisplayGuardian.Data.GetBirthdayAge + " years old today.";
                }
                else
                {
                    BirthdayTime = "Birthday: " + DisplayGuardian.Data.TimeUntilBirthdayString();
                }
                {
                    Season season;
                    int Day;
                    DisplayGuardian.GetBirthday(out season, out Day);
                    Day++;
                    BirthdayDate = "Birthday: " + season.ToString() + " " + Day;
                    switch (Day)
                    {
                        case 1:
                            BirthdayDate += "st";
                            break;
                        case 2:
                            BirthdayDate += "nd";
                            break;
                        case 3:
                            BirthdayDate += "rd";
                            break;
                        default:
                            BirthdayDate += "th";
                            break;
                    }
                }
                //
                InfosText.Clear();
                int AgeVal = DisplayGuardian.Age, YearlyAgeVal = DisplayGuardian.Data.YearlyAge;
                InfosText.Add("Age: " + DisplayGuardian.Age + (YearlyAgeVal != AgeVal ? "(" + YearlyAgeVal + " Years Old)" : ""));
                InfosText.Add("Size: " + DisplayGuardian.Base.Size.ToString());
                InfosText.Add(DisplayGuardian.GetGroup.Name);
                InfosText.Add("Mod:" + DisplayGuardian.ModID);
                InfosText.Add(BirthdayTime);
                InfosText.Add(BirthdayDate);
                //
                InfoIcons.Clear();
                if (DisplayGuardian.Base.Genderless)
                {
                    InfoIcons.Add(13);
                }
                else if (DisplayGuardian.Male)
                    InfoIcons.Add(4);
                else
                    InfoIcons.Add(5);
                if (DisplayGuardian.Base.DontUseRightHand)
                    InfoIcons.Add(-1);
                else
                    InfoIcons.Add(1);
                if (DisplayGuardian.Base.DontUseHeavyWeapons)
                    InfoIcons.Add(-2);
                else
                    InfoIcons.Add(2);
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
                    InfoIcons.Add(-9);
                else
                    InfoIcons.Add(9);
                if (DisplayGuardian.Base.DrinksBeverage)
                    InfoIcons.Add(10);
                else
                    InfoIcons.Add(-10);
                if (!DisplayGuardian.Base.IsNocturnal)
                    InfoIcons.Add(11);
                else
                    InfoIcons.Add(12);
            }
            UpdateButtonStates(player);
        }

        private static bool CallButtonActive = false, SendHomeButtonActive = false;

        private static void UpdateButtonStates(PlayerMod player)
        {
            CallButtonActive = (Selected > -1 && !DisplayGuardian.Base.InvalidGuardian &&
                (!PlayerMod.HasBuddiesModeOn(player.player) || !PlayerMod.GetPlayerBuddy(player.player).IsSameID(DisplayGuardian)) &&
                (((DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.CallUnlockLevel || DisplayGuardian.Data.IsStarter)) || (DisplayGuardian.request.Active && DisplayGuardian.request.Base.RequiresRequesterSummoned) ||
                PlayerMod.PlayerHasGuardianSummoned(player.player, DisplayGuardian.ID, DisplayGuardian.ModID)) &&
                (player.TitanGuardian == 255 || player.TitanGuardian == player.GetGuardianSlot(ContentList[Selected].Index)) &&
                ((player.GetEmptyGuardianSlot() < 255 && (!player.Guardian.Active || player.GuardianFollowersWeight + DisplayGuardian.Base.CompanionSlotWeight < player.MaxGuardianFollowersWeight)) ||
                player.GetGuardianSlot(ContentList[Selected].Index) < 255));
            SendHomeButtonActive = Selected > -1 && (DisplayGuardian.Data.IsStarter || DisplayGuardian.IsPlayerBuddy(Main.LocalPlayer) ||
                DisplayGuardian.FriendshipLevel >= DisplayGuardian.Base.MoveInLevel);// && (NpcMod.HasGuardianNPC(DisplayGuardian.ID, DisplayGuardian.ModID))
        }

        public static void DrawInterface()
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
            const float ElementScale = 0.7f;
            string MouseText = "";
            DrawNameList(HudPosition, player);
            if(player.Guardian.Active)
                DrawWeightBar(HudPosition, player, Selected > -1 ? ContentList[Selected].Index == player.SelectedGuardian ? 0 : (DisplayGuardian.Base.CompanionSlotWeight * (!PlayerMod.HasGuardianSummoned(player.player, DisplayGuardian.ID, DisplayGuardian.ModID) ? 1 : -1)) : 0);
            //Book Tag
            {
                Vector2 TagPosition = Vector2.Zero;
                TagPosition.X = HudPosition.X + 38;
                TagPosition.Y = HudPosition.Y + 10;
                if(Main.mouseX >= TagPosition.X && Main.mouseX < TagPosition.X + 55 && 
                    Main.mouseY >= TagPosition.Y && Main.mouseY < TagPosition.Y + 53)
                {
                    player.player.mouseInterface = true;
                    MouseText = "Sorting by: ";
                    switch (SortingOrder)
                    {
                        case SortByLetter:
                            MouseText += "Alphabetical Order";
                            break;
                        case SortByLivingInWorld:
                            MouseText += "Living in the World";
                            break;
                        case SortBySize:
                            MouseText += "Size";
                            break;
                        case SortByWeight:
                            MouseText += "Weight";
                            break;
                        case SortByHeight:
                            MouseText += "Height";
                            break;
                    }
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        SortingOrder++;
                        if (SortingOrder >= 5)
                            SortingOrder = 0;
                        ScrollY = 0;
                        GetGuardianList(player);
                        PickMainGuardian(player);
                    }
                }
                Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, TagPosition, new Rectangle(38, 10, 55, 53), Color.White);
            }
            const int ElementCenterX = 175 + 131;
            {
                int BarWidth = (int)((float)player.FriendshipExp / (player.FriendshipMaxExp - 1) * 262);
                Vector2 RankPosition = Vector2.Zero;
                RankPosition.X = HudPosition.X + 175;
                RankPosition.Y = HudPosition.Y + 87;
                Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, RankPosition, new Rectangle(175, 87, BarWidth, 5), Color.White);
                RankPosition.X = HudPosition.X + ElementCenterX;
                RankPosition.Y = HudPosition.Y + 84 + 6;
                Utils.DrawBorderString(Main.spriteBatch, "Friendship Rank: " + player.FriendshipLevel, RankPosition, Color.White, ElementScale, 0.5f, 1f);
                int ProgressIncreasePerWidth = (int)(1f / (player.FriendshipMaxExp - 1) * 262);
                for (byte i = 1; i < player.FriendshipMaxExp; i++)
                {
                    Vector2 BarPosition = new Vector2(HudPosition.X + 175 + ProgressIncreasePerWidth * i, HudPosition.Y + 87);
                    Main.spriteBatch.Draw(Terraria.Main.blackTileTexture, new Rectangle((int)BarPosition.X, (int)BarPosition.Y, 2, 6), null, Color.Black);
                }
            }
            if (Selected > -1)
            {
                {
                    Vector2 TgPos = Vector2.Zero;
                    TgPos.X = Main.screenPosition.X + HudPosition.X + ElementCenterX;
                    TgPos.Y = Main.screenPosition.Y + HudPosition.Y + 235;
                    DisplayGuardian.Position = TgPos;
                    DisplayGuardian.UpdateAnimation();
                    DisplayGuardian.Draw(true, true);
                }
                {
                    Vector2 NamePos = Vector2.Zero;
                    NamePos.X = HudPosition.X + ElementCenterX;
                    NamePos.Y = HudPosition.Y + 253 + 6;
                    string NameText = DisplayGuardian.Name;
                    if (DisplayGuardian.Data._Name != null)
                        NameText += "(" + DisplayGuardian.RealName + ")";
                    float TextWidth = Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Name, NamePos, Color.White, 0.8f, 0.5f, 1).X * 0.5f + 4;
                    NamePos.X += TextWidth;
                    if (DisplayGuardian.Data.CanChangeName)
                    {
                        NamePos.Y -= 7 + 16;
                        Main.spriteBatch.Draw(MainMod.EditButtonTexture, NamePos, Color.White);
                        if(Main.mouseX >= NamePos.X && Main.mouseX < NamePos.X + 16 && 
                            Main.mouseY >= NamePos.Y && Main.mouseY < NamePos.Y + 16)
                        {
                            MouseText = "Click to rename companion.\nCheck chat if you do so.";
                            if(Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                int GPos = ContentList[Selected].Index;
                                Main.chatText = "/renameguardian " + player.MyGuardians[GPos].ID + " " + player.MyGuardians[GPos].ModID + " ";
                                Main.drawingPlayerChat = true;
                                IsActive = false;
                                Main.NewText("Insert new name. Press Esc to cancel.");
                            }
                        }
                    }
                }
                //Upper Left Display Infos
                {
                    Vector2 ElementPos = Vector2.Zero;
                    ElementPos.X = HudPosition.X + 175 + 4;//NameWidth - 20 * 2;
                    ElementPos.Y = HudPosition.Y + 102 + 4;//-= 24 + 6;
                    DisplayGuardian.DrawFriendshipHeart(ElementPos, 255, -1, 1);
                    //Crowns Drawing
                    Vector2 TextPos = Vector2.Zero;
                    ElementPos.Y += 26 + 4;
                    //Gold Crown
                    Main.spriteBatch.Draw(MainMod.CrownTexture, ElementPos, new Rectangle(0, 0, 16, 12), Color.White);
                    TextPos.X = ElementPos.X + 20;
                    TextPos.Y = ElementPos.Y - 6;
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsWon().ToString(), TextPos, Color.White, ElementScale);
                    if (Main.mouseX >= ElementPos.X && Main.mouseX < ElementPos.X + 40 && Main.mouseY >= ElementPos.Y && Main.mouseY < ElementPos.Y + 16)
                    {
                        MouseText = "Popularity Contests won: " + DisplayGuardian.Base.GetPopularityContestsWon();
                    }
                    ElementPos.Y += 20;
                    //Silver Crown
                    Main.spriteBatch.Draw(MainMod.CrownTexture, ElementPos, new Rectangle(16, 0, 16, 12), Color.White);
                    //TextPos.X = ElementPos.X + 20;
                    TextPos.Y = ElementPos.Y - 6;
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsSecondPlace().ToString(), TextPos, Color.White, ElementScale);
                    if (Main.mouseX >= ElementPos.X && Main.mouseX < ElementPos.X + 40 && Main.mouseY >= ElementPos.Y && Main.mouseY < ElementPos.Y + 16)
                    {
                        MouseText = "Popularity Contests won in 2nd place: " + DisplayGuardian.Base.GetPopularityContestsSecondPlace();
                    }
                    ElementPos.Y += 20;
                    //Bronze Crown
                    Main.spriteBatch.Draw(MainMod.CrownTexture, ElementPos, new Rectangle(32, 0, 16, 12), Color.White);
                    //TextPos.X = ElementPos.X + 20;
                    TextPos.Y = ElementPos.Y - 6;
                    Utils.DrawBorderString(Main.spriteBatch, DisplayGuardian.Base.GetPopularityContestsThirdPlace().ToString(), TextPos, Color.White, ElementScale);
                    if (Main.mouseX >= ElementPos.X && Main.mouseX < ElementPos.X + 40 && Main.mouseY >= ElementPos.Y && Main.mouseY < ElementPos.Y + 16)
                    {
                        MouseText = "Popularity Contests won in 3nd place: " + DisplayGuardian.Base.GetPopularityContestsThirdPlace();
                    }
                    ElementPos.Y += 20;
                }
                //Info Icons
                {
                    Vector2 IconPosition = Vector2.Zero;
                    IconPosition.X = HudPosition.X + 175 + 131 - 20 * InfoIcons.Count * 0.5f;
                    IconPosition.Y = HudPosition.Y + 102;
                    foreach(sbyte i in InfoIcons)
                    {
                        bool Negation = i < 0;
                        byte Value = (i >= 0 ? (byte)i : (byte)(-i));
                        IconPosition.X += 2;
                        if(Main.mouseX >= IconPosition.X && Main.mouseX < IconPosition.X + 16 && 
                            Main.mouseY >= IconPosition.Y && Main.mouseY < IconPosition.Y + 16)
                        {
                            switch (Value)
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
                                        MouseText = "Can't Crouch";
                                    }
                                    else
                                    {
                                        MouseText = "Can Crouch";
                                    }
                                    break;
                                case 9:
                                    if (!Negation)
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
                                case 13:
                                    MouseText = "Genderless";
                                    break;
                            }
                        }
                        Main.spriteBatch.Draw(MainMod.GuardianInfoIcons, IconPosition, new Rectangle(Value * 16, 0, 16, 16), Color.White);
                        if(Negation)
                            Main.spriteBatch.Draw(MainMod.GuardianInfoIcons, IconPosition, new Rectangle(0, 0, 16, 16), Color.White);
                        IconPosition.X += 18;
                    }
                }
                //Contributor Icon Part
                if (DisplayGuardian.Base.CompanionContributorName != "")
                {
                    Vector2 ContributionIconPosition = Vector2.Zero;
                    ContributionIconPosition.X = HudPosition.X + 434 - 17;
                    ContributionIconPosition.Y = HudPosition.Y + 232 - 17;
                    ContributionIconAnimationTime++;// += (1f / 60 * 100 * 0.5f);
                    if (ContributionIconAnimationTime >= 9 * 6)
                        ContributionIconAnimationTime -= 9 * 6;
                    Main.spriteBatch.Draw(MainMod.ContributorIconTexture, ContributionIconPosition, new Rectangle(17 * (int)(ContributionIconAnimationTime * (1f / 6)), 0, 17, 17), Color.White);
                    if(Main.mouseX >= ContributionIconPosition.X && Main.mouseX < ContributionIconPosition.X + 17 && 
                        Main.mouseY >= ContributionIconPosition.Y && Main.mouseY < ContributionIconPosition.Y + 17)
                    {
                        MouseText = "Contributed Companion \nby " + DisplayGuardian.Base.CompanionContributorName + ".";
                    }
                }                
                //Description
                {
                    //int lines;
                    string Description = DisplayGuardian.Base.Description.Replace('\n', ' ');
                    int lines;
                    string[] DescriptionTextParsed = Utils.WordwrapString(Description, Main.fontMouseText, (int)(262 * (1f / (ElementScale != 0 ? ElementScale : 1))), 5, out lines);
                    
                    for (int l = 0; l <= lines; l++)
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
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            int index = y * 2 + x;
                            if (index >= InfosText.Count)
                                break;
                            ExtraInfosPos.X = HudPosition.X + 175 + 130 * x;
                            ExtraInfosPos.Y = HudPosition.Y + 326 - 2 + 12 * y;
                            Utils.DrawBorderString(Main.spriteBatch, InfosText[index], ExtraInfosPos, Color.White, ElementScale);
                        }
                    }
                }
            }
            //Dismiss Button (left)
            if (CallButtonActive)
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 175 + 39;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                bool MouseOver = false;
                string Text = "Call";
                bool IsCallButton = player.GetGuardianSlot(ContentList[Selected].Index) == 255;
                byte SummonSlot = player.GetEmptyGuardianSlot();
                if(IsCallButton)
                {
                    int ID = player.MyGuardians[ContentList[Selected].Index].ID;
                    string ModID = player.MyGuardians[ContentList[Selected].Index].ModID;
                    if (Main.netMode == 0 && NpcMod.HasGuardianNPC(ID, ModID))
                    {
                        if(NpcMod.GetGuardianNPCCharacter(ID, ModID).OwnerPos != -1 || NpcMod.GetGuardianNPCCharacter(ID, ModID).IsCommander)
                            Text = "";
                    }
                }
                else if (!IsCallButton)
                {
                    Text = "Dismiss";
                    int ID = player.MyGuardians[ContentList[Selected].Index].ID;
                    string ModID = player.MyGuardians[ContentList[Selected].Index].ModID;
                    if (Main.netMode == 0 && NpcMod.HasGuardianNPC(ID, ModID))
                    {
                        if (NpcMod.GetGuardianNPCCharacter(ID, ModID).IsCommander)
                        {
                            Text = "";
                        }
                    }
                }
                else if(SummonSlot > 0 && SummonSlot < 255)
                {
                    Text += " Assist";
                }
                if(Text != "" && Math.Abs(Main.mouseX - ButtonCenter.X) < 39 && 
                    Math.Abs(Main.mouseY - ButtonCenter.Y) < 14)
                {
                    MouseOver = true;
                    if (IsCallButton)
                    {
                        MouseText = "Calls the companion to aid you on your quest.";
                    }
                    else
                    {
                        MouseText = "Dismisses a companion from your group.";
                    }
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (IsCallButton)
                        {
                            player.CallGuardian(ContentList[Selected].Index, SummonSlot);
                            foreach (TerraGuardian tg in player.GetAllGuardianFollowers)
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
                        ContentList[Selected].Summoned = IsCallButton;
                        UpdateButtonStates(player);
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), ElementScale, 0.5f, 0.5f);
            }
            else
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 175 + 39;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                Utils.DrawBorderString(Main.spriteBatch, "Can't Call", ButtonCenter, Color.Red, ElementScale, 0.5f, 0.5f);
            }
            //Home Button (Center)
            if (SendHomeButtonActive)
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
                        if (!IsMoveout)
                            MouseText = "Lets the companion live in your world.";
                        else
                            MouseText = "Makes the companion stop living in your world.";
                        MouseOver = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            if (IsMoveout)
                            {
                                WorldMod.RemoveGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                            }
                            else
                            {
                                WorldMod.AllowGuardianNPCToSpawn(DisplayGuardian.ID, DisplayGuardian.ModID);
                            }
                            ContentList[Selected].LivingHere = !IsMoveout;
                            UpdateButtonStates(player);
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), ElementScale, 0.5f, 0.5f);
                }
            }
            else
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 265 + 41;
                ButtonCenter.Y = HudPosition.Y + 404 + 14;
                Utils.DrawBorderString(Main.spriteBatch, "Can't Move In", ButtonCenter, Color.Red, ElementScale, 0.5f, 0.5f);
            }
            //Inventory Button (Right)
            if (Selected > -1)
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
                    MouseText = "Allows management of companion inventory and skins.";
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if (GuardianManagement.OpenInterfaceForGuardian(ContentList[Selected].Index))
                            IsActive = false;
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), ElementScale, 0.5f, 0.5f);
            }
            //Wiki Button
            if(Selected > -1)
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 175 + 20;
                ButtonCenter.Y = HudPosition.Y + 235 - 5;
                bool MouseOver = false;
                string Text = "Wiki";
                if (Math.Abs(Main.mouseX - ButtonCenter.X) < 39 &&
                    Math.Abs(Main.mouseY - ButtonCenter.Y) < 14)
                {
                    MouseText = "Opens the wiki page related to this companion.";
                    MouseOver = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        string Url = "https://nakano15-mods.fandom.com/wiki/";
                        if (DisplayGuardian.Base.WikiPageLink != null)
                            Url += DisplayGuardian.Base.WikiPageLink;
                        else
                            Url += DisplayGuardian.Base.Name;
                        System.Diagnostics.Process.Start(Url);
                        Main.NewText("Opening wiki page of " + DisplayGuardian.Name + ".", Color.Yellow);
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.White), ElementScale, 0.5f, 0.5f);
            }
            //Close Button
            {
                Vector2 ButtonCenter = Vector2.Zero;
                ButtonCenter.X = HudPosition.X + 460;
                ButtonCenter.Y = HudPosition.Y + 63;
                bool MouseOver = false;
                string Text = "X";
                if (Math.Abs(Main.mouseX - ButtonCenter.X) < 6 &&
                    Math.Abs(Main.mouseY - ButtonCenter.Y) < 6)
                {
                    MouseOver = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        IsActive = false;
                    }
                }
                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonCenter, (MouseOver ? Color.Yellow : Color.Red), ElementScale, 0.5f, 0.5f);
            }
            if(MouseText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
        }

        public static void DrawWeightBar(Vector2 InterfacePosition, PlayerMod player, float SelectedGuardianWeight)
        {
            Vector2 BarPosition = Vector2.Zero;
            BarPosition.X = InterfacePosition.X + 184;
            BarPosition.Y = InterfacePosition.Y + 34;
            const int BarWidth = 248, BarHeight = 28;
            int CurrentWeight = (int)(Math.Round(player.GuardianFollowersWeight, 2) * 1000), 
                GuardianWeightValue = (int)(Math.Round(SelectedGuardianWeight, 2) * 1000), 
                MaxWeight = (int)(player.MaxGuardianFollowersWeight * 1000);
            {
                int FirstBarSize = (int)((float)(CurrentWeight + (GuardianWeightValue > 0 ? 0 : GuardianWeightValue)) / MaxWeight * BarWidth),
                    SecondBarSize = (int)((float)Math.Abs(GuardianWeightValue) / MaxWeight * BarWidth);
                if(SelectedGuardianWeight > 0 && FirstBarSize + SecondBarSize > BarWidth)
                {
                    FirstBarSize = 0;
                    SecondBarSize = BarWidth;
                }
                else
                {
                    if (FirstBarSize > BarWidth)
                        FirstBarSize = BarWidth;
                    if (SecondBarSize > BarWidth)
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
                const int ScrollBarMaxHeight = 366 - 24;
                float ScrollBarSize = 1f;
                int OverflowValue = ContentList.Length - MaxLines;
                float BarY = 0;
                if(OverflowValue > 0)
                {
                    ScrollBarSize = (float)MaxLines / ContentList.Length;
                    BarY = (ScrollBarMaxHeight * ((1f / OverflowValue) * (1f - ScrollBarSize) * ScrollY));
                }
                ScrollBarSize *= ScrollBarMaxHeight;
                for(int i = 0; i < 3; i++)
                {
                    Vector2 ThisPosition = Vector2.Zero;
                    ThisPosition.X = ElementPosition.X;
                    ThisPosition.Y = ElementPosition.Y;
                    int dy = 66, dh = 0;
                    int scaley = 0;
                    bool MouseOver = false;
                    switch (i)
                    {
                        case 0: //bg
                            scaley = (int)ScrollBarSize - 16;
                            dy = 96;
                            dh = 110;
                            ThisPosition.Y += 24 + BarY;
                            break;
                        case 1: //upper
                            dh = 28;
                            scaley = dh;
                            if(Main.mouseX >= ThisPosition.X && Main.mouseX < ThisPosition.X + 12 && 
                                Main.mouseY >= ThisPosition.Y && Main.mouseY < ThisPosition.Y + dh)
                            {
                                MouseOver = true;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    ScrollY--;
                                    if (ScrollY < 0)
                                        ScrollY = 0;
                                }
                            }
                            break;
                        case 2: //lower
                            dy = 208;
                            dh = 28;
                            scaley = dh;
                            /*if (ScrollBarSize < 28)
                                ThisPosition.Y += 28;
                            else
                            {
                                ThisPosition.Y += ScrollBarSize - 28;
                            }*/
                            ThisPosition.Y = ElementPosition.Y + 366 - 28;
                            if (Main.mouseX >= ThisPosition.X && Main.mouseX < ThisPosition.X + 12 &&
                                Main.mouseY >= ThisPosition.Y && Main.mouseY < ThisPosition.Y + dh)
                            {
                                MouseOver = true;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    ScrollY++;
                                    if (ScrollY > ContentList.Length - MaxLines)
                                        ScrollY = ContentList.Length - MaxLines;
                                    if (ScrollY < 0)
                                        ScrollY = 0;
                                }
                            }
                            break;
                    }
                    Main.spriteBatch.Draw(MainMod.GSI_ForegroundInterfaceTexture, new Rectangle((int)ThisPosition.X, (int)ThisPosition.Y, 12, scaley), new Rectangle(152, dy, 12, dh), (MouseOver ? Color.Yellow : Color.White));
                }
            }
            // Companion List
            ElementPosition.X = InterfaceStartPosition.X + 30;
            ElementPosition.Y = InterfaceStartPosition.Y + 96 + 6;
            const float ElementScale = 0.7f;
            Utils.DrawBorderString(Main.spriteBatch, "Companions", ElementPosition, Color.White, 1, 0f, 1f);
            ElementPosition.Y += 14;
            for(int element = 0; element < MaxLines; element++)
            {
                int index = element + ScrollY;
                if(index >= ContentList.Length)
                {
                    break;
                }
                ContentElement ce = ContentList[index];
                switch (ce.SortingType)
                {
                    case ContentElement.SortTypes.Letter:
                        Utils.DrawBorderString(Main.spriteBatch, " #" + ((char)ce.Index == '0' ? "0~9" : ((char)ce.Index).ToString()), ElementPosition, Color.LightSalmon, ElementScale, 0f, 1f);
                        break;
                    case ContentElement.SortTypes.Size:
                        Utils.DrawBorderString(Main.spriteBatch, " #" + Enum.GetName(typeof(GuardianBase.GuardianSize), ce.Index), ElementPosition, Color.LightSalmon, ElementScale, 0f, 1f);
                        break;
                    case ContentElement.SortTypes.Weight:
                        Utils.DrawBorderString(Main.spriteBatch, " #" + WeightValueToString(ce.Index), ElementPosition, Color.LightSalmon, ElementScale, 0f, 1f);
                        break;
                    case ContentElement.SortTypes.Dimension:
                        Utils.DrawBorderString(Main.spriteBatch, " #" + HeightValueToString(ce.Index), ElementPosition, Color.LightSalmon, ElementScale, 0f, 1f);
                        break;
                    case ContentElement.SortTypes.GuardianID:
                        GuardianData gd = player.MyGuardians[ce.Index];
                        Color color = Color.White;
                        if (Selected == index)
                            color = Color.Yellow;
                        else if (ce.Summoned)
                            color = Color.LightGreen;
                        else if (ce.LivingHere)
                            color = Color.White;
                        else
                            color = Color.Gray;
                        // Selected == index ? Color.Yellow : (ce.LivingHere ? Color.White : Color.Gray);
                        if (Main.mouseX >= ElementPosition.X && Main.mouseX < ElementPosition.X + 118 &&
                            Main.mouseY >= ElementPosition.Y - 18 && Main.mouseY < ElementPosition.Y - 6)
                        {
                            color = Color.Yellow;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                ChangeSelectedGuardian(index, player);
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, gd.Name, ElementPosition, color, ElementScale, 0f, 1f);
                        break;
                }
                ElementPosition.Y += 14;
            }
        }

        public static void DrawRectangle(float x, float y, int width, int height, Color c)
        {
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)x, (int)y, width, height), c);
        }

        public static string WeightValueToString(int Value)
        {
            switch (Value)
            {
                case 0: return "Trivial";
                case 1: return "Minimal";
                case 2: return "Normal";
                case 3: return "Relevant";
                case 4: return "Spacious";
                case 5: return "Very Spacious";
                default: return "Colossal";
            }
        }

        public static string HeightValueToString(int Value)
        {
            switch (Value)
            {
                case 0: return "Extremelly Small";
                case 1: return "Very Small";
                case 2: return "Small";
                case 3: return "Normal";
                case 4: return "Large";
                case 5: return "Very Large";
                case 6: return "Extra Large";
                default: return "Can only see the feet.";
            }
        }

        public struct ContentElement
        {
            public int Index;
            public bool LivingHere, Summoned;
            public SortTypes SortingType;

            public ContentElement(int MyTgID, bool LivingHere, bool Summoned)
            {
                Index = MyTgID;
                this.LivingHere = LivingHere;
                SortingType = SortTypes.GuardianID;
                this.Summoned = Summoned;
            }

            public ContentElement(char Letter)
            {
                Index = Letter;
                SortingType = SortTypes.Letter;
                LivingHere = false;
                Summoned = false;
            }

            public ContentElement(GuardianBase.GuardianSize size)
            {
                Index = (int)size;
                SortingType = SortTypes.Size;
                LivingHere = false;
                Summoned = false;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Value"></param>
            /// <param name="ValueType">0 = Weight, 1 = Width/Height size</param>
            public ContentElement(int Value, byte ValueType)
            {
                Index = Value;
                SortingType = (ValueType == 0 ? SortTypes.Weight : SortTypes.Dimension);
                LivingHere = false;
                Summoned = false;
            }

            public enum SortTypes : byte
            {
                GuardianID,
                Letter,
                Size,
                Weight,
                Dimension
            }
        }
    }
}
