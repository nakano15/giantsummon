﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianMouseOverAndDialogueInterface
    {
        private static string[] Dialogue = new string[0];
        private static int DialogueLines = 0;
        private static byte DialogueDelayTime = 0;
        private const byte DialogueMaxDelayTime = 60;
        private static byte DialogueCloseCheckDelayTime = 0;
        public static bool SomeoneMouseOver = false;
        private static bool ClickedOnce = false;
        public static int MouseOverGuardian = -1;
        public static List<DialogueOption> Options = new List<DialogueOption>();
        public static Player MainPlayer { get { return Main.player[Main.myPlayer]; } }
        public static int MouseOverOptionNumber = -1;
        public static float DialogueStartX
        {
            get
            {
                return Main.screenWidth * 0.5f - DialogueWidth * 0.5f;
            }
        }
        public static float DialogueStartY
        {
            get
            {
                return 100;
            }
        }
        public static int DialogueWidth
        {
            get
            {
                return (int)(Main.screenWidth * 0.5f);
            }
        }
        public static int DialogueHeight
        {
            get
            {
                return (int)(DialogueLines * 30 + 60);
            }
        }
        private static bool HideCallDismissButton = false;
        public static TerraGuardian Speaker, StarterSpeaker;

        public static void SetDialogue(string Message)
        {
            SetDialogue(Message, Speaker);
        }

        public static void SetDialogueDistanceAutoCloseDelay()
        {
            DialogueCloseCheckDelayTime = 60;
        }

        public static void SetDialogue(string Message, TerraGuardian tg) //And with auto parser :D
        {
            Speaker = tg;
            if (tg != null)
            {
                Dialogue = Utils.WordwrapString(MessageParser(Message, tg), Main.fontMouseText, DialogueWidth - 16, 10, out DialogueLines);
            }
            else
            {
                Dialogue = Utils.WordwrapString(Message, Main.fontMouseText, DialogueWidth - 16, 10, out DialogueLines);
            }
            DialogueLines--;
            if (DialogueLines < 0)
                DialogueLines = 0;
        }

        public static void StartDialogue(TerraGuardian tg)
        {
            giantsummon.Dialogue.SetImportantDialogue(false);
            Speaker = StarterSpeaker = tg;
            giantsummon.Dialogue.DialogueParticipants = new TerraGuardian[0];
            if (giantsummon.Dialogue.DialogueThread != null && giantsummon.Dialogue.DialogueThread.IsAlive)
            {
                giantsummon.Dialogue.DialogueThread.Abort();
            }
            HideCallDismissButton = false;
            GuardianShopInterface.ShopOpen = false;
            Main.CancelHairWindow();
            Main.npcShop = 0;
            Main.InGuideCraftMenu = false;
            MainPlayer.dropItemCheck();
            Main.npcChatCornerItem = 0;
            MainPlayer.sign = -1;
            Main.editSign = false;
            MainPlayer.talkNPC = -1;
            Main.playerInventory = false;
            MainPlayer.chest = -1;
            Recipe.FindRecipes();
            Main.PlaySound(24, -1, -1, 1, 1f, 0f);
            PlayerMod modPlayer = MainPlayer.GetModPlayer<PlayerMod>();
            modPlayer.IsTalkingToAGuardian = true;
            modPlayer.TalkingGuardianPosition = tg.WhoAmID;
            tg.TalkPlayerID = MainPlayer.whoAmI;
            bool PlayerToTheLeft = MainPlayer.Center.X < tg.Position.X;
            if (!tg.PlayerMounted && !tg.SittingOnPlayerMount)
            {
                if (tg.LookingLeft != PlayerToTheLeft)
                {
                    if (tg.UsingFurniture && tg.IsUsingChair)
                    {
                        tg.LeaveFurniture(false);
                    }
                    tg.LookingLeft = PlayerToTheLeft;
                }
                if (MainPlayer.GetModPlayer<PlayerMod>().MountedOnGuardian)
                {
                    TerraGuardian mount = MainPlayer.GetModPlayer<PlayerMod>().MountGuardian;
                    PlayerToTheLeft = mount.Position.X < tg.Position.X;
                    if (mount.Velocity == Vector2.Zero && ((mount.Direction > 0 && !PlayerToTheLeft) || (mount.Direction < 0 && PlayerToTheLeft)))
                    {
                        mount.LookingLeft = !PlayerToTheLeft;
                    }
                }
                else if (MainPlayer.velocity == Vector2.Zero && ((MainPlayer.direction > 0 && !PlayerToTheLeft) || (MainPlayer.direction < 0 && PlayerToTheLeft)))
                {
                    if (PlayerToTheLeft)
                        MainPlayer.direction = 1;
                    else
                        MainPlayer.direction = -1;
                }
            }
            if (!Main.LocalPlayer.GetModPlayer<PlayerMod>().IsCompanionParty)
            {
                foreach (QuestData data in PlayerMod.GetPlayerQuestDatas(MainPlayer))
                {
                    if (data.IsInvalid)
                        continue;
                    Action dialogue = data.GetBase.ImportantDialogueMessage(data, Speaker, Speaker.ID, Speaker.ModID);
                    if (dialogue != null)
                    {
                        QuestBase.Data = data;
                        giantsummon.Dialogue.StartNewDialogue(dialogue, Speaker);
                        return;
                    }
                }
            }
            //GetCompanionDialogue
            GetDefaultOptions();
            string Message = "";
            string ActionMessage = "";
            if (tg.DoAction.InUse)
            {
                ActionMessage = tg.DoAction.ModifyDialogue(tg, Options);
            }
            if (tg.Base.InvalidGuardian)
            {
                Message = "(Your memories of it are fragmented, so you can't see its true form, neither allow it to speak with you.)\nGuardian ID: " + tg.ID + ", Guardian Mod ID: " + tg.ModID + ".\nIf this isn't related to a mod you uninstalled, send a screenshot of this to the mod developer.";
            }
            else if (!modPlayer.HasGuardian(tg.ID, tg.ModID))
            {
                modPlayer.AddNewGuardian(tg.ID, tg.ModID);
                if (WorldMod.IsStarter(tg))
                    modPlayer.GetGuardian(tg.ID, tg.ModID).SetStarterGuardian();
                tg.Data = modPlayer.GetGuardian(tg.ID, tg.ModID);
                Message = tg.Base.GreetMessage(MainPlayer, tg);
                modPlayer.GetGuardian(tg.ID, tg.ModID).IncreaseFriendshipProgress(1);
            }
            else
            {
                if (!tg.IsSleeping && ActionMessage == "" && ShowImportantMessages())
                    return;
                if (!tg.IsSleeping && tg.ComfortPoints >= tg.MaxComfortExp)
                {
                    tg.ComfortPoints = 0;
                    tg.IncreaseFriendshipProgress(1);
                    GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.MentionPlayer, MainPlayer.name,
                        tg.Name, 7, 5, new GuardianID[] { tg.MyID });
                    Message = tg.Base.TalkMessage(MainPlayer, tg);
                }
                else if (!tg.IsSleeping && PlayerMod.IsGuardianBirthday(MainPlayer, tg.ID, tg.ModID) && Main.rand.Next(2) == 0)
                    Message = tg.Base.BirthdayMessage(MainPlayer, tg);
                else
                {
                    if (tg.OwnerPos == -1 && tg.GetTownNpcInfo != null && tg.GetTownNpcInfo.Homeless && !tg.IsSleeping && (tg.IsStarter || tg.FriendshipLevel >= tg.Base.MoveInLevel))
                    {
                        Message = tg.Base.HomelessMessage(MainPlayer, tg);
                    }
                    else
                    {
                        FeatMentioning feat = GuardianGlobalInfos.GetAFeatToMention(tg.MyID, MainPlayer.name);
                        bool SayNormalMessage = true;
                        if (!tg.IsSleeping && feat != null && Main.rand.NextDouble() < 0.3f)
                        {
                            Message = GuardianGlobalInfos.GetFeatMessage(feat, tg);
                            SayNormalMessage = Message == "";
                        }
                        if (SayNormalMessage)
                        {
                            Message = tg.Base.NormalMessage(MainPlayer, tg);
                        }
                    }
                }
            }
            if (!tg.Base.InvalidGuardian && !NpcMod.HasMetGuardian(tg.ID, tg.ModID))
            {
                Message = tg.Base.GreetMessage(MainPlayer, tg);
                NpcMod.AddGuardianMet(tg.ID, tg.ModID);
                if (WorldMod.HasEmptyGuardianNPCSlot() && (tg.IsStarter || tg.FriendshipLevel >= tg.Base.MoveInLevel))
                    WorldMod.AllowGuardianNPCToSpawn(tg.ID, tg.ModID);
                if(Main.netMode == 0)
                {
                    tg.Data = PlayerMod.GetPlayerGuardian(MainPlayer, tg.ID, tg.ModID);
                }
            }
            if (!tg.Base.InvalidGuardian && tg.HasBuff(ModContent.BuffType<Buffs.Sleeping>()))
            {
                Message = "(Seems to be under a heavy sleep.)";
            }
            if (ActionMessage != "")
                Message = ActionMessage;
            SetDialogue(Message, tg);
        }

        public static bool ShowImportantMessages()
        {
            string Message = "";
            Speaker.Data.CheckForImportantMessages(out Message);
            if (Message == "")
            {
                SetDialogue(Speaker.Base.NormalMessage(MainPlayer, Speaker), Speaker);
                GetDefaultOptions();
                return false;
            }
            SetDialogue(Message);
            Options.Clear();
            AddOption("Okay", delegate () { ShowImportantMessages(); });
            return true;
        }

        public static void Update()
        {
            if (Options.Count == 0 && !giantsummon.Dialogue.IsDialogue)
            {
                AddOption("Close", CloseDialogueButtonAction);
            }
            //UpdateMouseOver();
            //UpdateDialogueMouse();
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (!player.KnockedOut && !player.IsTalkingToAGuardian && SomeoneMouseOver)
            {
                if (DialogueDelayTime < DialogueMaxDelayTime)
                    DialogueDelayTime++;
            }
            if (giantsummon.Dialogue.DialogueThread != null && !player.IsTalkingToAGuardian && giantsummon.Dialogue.DialogueThread.IsAlive)
                giantsummon.Dialogue.DialogueThread.Abort();
        }

        public static void UpdateDialogueMouse()
        {
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (!player.IsTalkingToAGuardian && GuardianShopInterface.ShopOpen)
                GuardianShopInterface.ShopOpen = false;
            if (player.KnockedOut)
            {
                if (player.IsTalkingToAGuardian)
                {
                    CloseDialogue();
                    //player.IsTalkingToAGuardian = false;
                }
            }
            else if (!player.IsTalkingToAGuardian)
            {
                if (SomeoneMouseOver)
                {
                    //if (DialogueDelayTime < DialogueMaxDelayTime)
                    //    DialogueDelayTime++;
                    if (!player.IsTalkingToAGuardian || player.TalkingGuardianPosition != MouseOverGuardian)
                    {
                        if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                            if (!tg.IsAttackingSomething && !tg.Downed && !tg.KnockedOut && IsInChattingRange(tg) && 
                                ((tg.OwnerPos == Main.myPlayer && DialogueDelayTime >= DialogueMaxDelayTime) || tg.OwnerPos == -1 || 
                                (tg.IsCommander && tg.GetCommanderLeaderID == Main.myPlayer) || 
                                (Main.LocalPlayer.GetModPlayer<PlayerMod>().CompanionCommanderLeaderPlayer == tg.OwnerPos) || 
                                (tg.OwnerPos > -1 && Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().IsCompanionParty)) &&
                                (!tg.DoAction.InUse || (!tg.DoAction.Invisibility && !tg.DoAction.Inactivity)))
                            {
                                StartDialogue(tg);
                            }
                        }
                    }
                }
                else if (DialogueDelayTime > 0)
                {
                    DialogueDelayTime = 0;
                }
            }
            else
            {
                if (!MainMod.ActiveGuardians.ContainsKey(player.TalkingGuardianPosition))
                {
                    CloseDialogue();
                    //player.IsTalkingToAGuardian = false;
                    return;
                }
                TerraGuardian tg = MainMod.ActiveGuardians[player.TalkingGuardianPosition];
                if (!IsInChattingRange(tg) || (Main.playerInventory && !GuardianShopInterface.ShopOpen && !GuardianManagement.Active) || 
                    (MainPlayer.talkNPC > -1 && Main.npc[MainPlayer.talkNPC].active) || MainPlayer.sign > -1 || MainPlayer.chest > -1 || tg.Downed || tg.KnockedOut ||
                    (tg.DoAction.InUse && (tg.DoAction.Invisibility || tg.DoAction.Inactivity)))
                {
                    CloseDialogue();
                    //player.IsTalkingToAGuardian = false;
                    if (Main.playerInventory)
                        Main.playerInventory = false;
                }
                else
                {
                    if (GuardianShopInterface.ShopOpen)
                    {
                        GuardianShopInterface.UpdateShop();
                        return;
                    }
                    MouseOverOptionNumber = -1;
                    if (!GuardianManagement.Active)
                    {
                        bool DoAction = false;
                        for (int o = 0; o < Options.Count; o++)
                        {
                            Vector2 Position = new Vector2(DialogueStartX + 8, DialogueStartY + DialogueHeight + 48 + o * 30 + 8);
                            if (Main.mouseX >= Position.X && Main.mouseX < Position.X + DialogueWidth && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 30)
                            {
                                MouseOverOptionNumber = o;
                                if (!ClickedOnce && !DoAction && Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    ClickedOnce = true;
                                    DoAction = true;
                                }
                            }
                        }
                        if (DoAction && MouseOverOptionNumber > -1)
                        {
                            if (Options[MouseOverOptionNumber].qd != null)
                                QuestBase.Data = Options[MouseOverOptionNumber].qd;
                            if (Options[MouseOverOptionNumber].ThreadedDialogue)
                            {
                                if (giantsummon.Dialogue.DialogueParticipants.Length > 0)
                                    giantsummon.Dialogue.FollowUpDialogue(Options[MouseOverOptionNumber].Action);
                                else
                                    giantsummon.Dialogue.StartNewDialogue(Options[MouseOverOptionNumber].Action, Speaker);
                            }
                            else
                            {
                                Options[MouseOverOptionNumber].Action();
                            }
                        }
                    }
                }
            }
            if (ClickedOnce && !Main.mouseLeft)
                ClickedOnce = false;
        }

        public static void UpdateMouseOver()
        {
            SomeoneMouseOver = false;
            int LastMouseOverGuardian = MouseOverGuardian;
            MouseOverGuardian = -1;
            float MouseX = Main.mouseX + Main.screenPosition.X,
                MouseY = Main.mouseY + Main.screenPosition.Y;
            {
                int[] Keys = MainMod.ActiveGuardians.Keys.ToArray();
                foreach (int key in Keys)
                {
                    if (!MainMod.ActiveGuardians[key].IsPlayerHostile(Main.player[Main.myPlayer]) && !MainMod.ActiveGuardians[key].PlayerControl)
                    {
                        float Left = MainMod.ActiveGuardians[key].Position.X - MainMod.ActiveGuardians[key].Width * 0.5f,
                            Bottom = MainMod.ActiveGuardians[key].Position.Y,
                            Right = Left + MainMod.ActiveGuardians[key].Width,
                            Top = Bottom - MainMod.ActiveGuardians[key].Height;
                        if (MouseX >= Left && MouseX < Right && MouseY >= Top && MouseY < Bottom)
                        {
                            if (!MainMod.ActiveGuardians[key].DoAction.InUse || (!MainMod.ActiveGuardians[key].DoAction.Inactivity && !MainMod.ActiveGuardians[key].DoAction.Invisibility))
                            {
                                if (!MainMod.ActiveGuardians[key].KnockedOut || !MainMod.ActiveGuardians[key].HasFlag(GuardianFlags.CantReceiveHelpOnReviving))
                                {
                                    SomeoneMouseOver = true;
                                    MouseOverGuardian = key;
                                }
                            }
                        }
                    }
                }
            }
            if (LastMouseOverGuardian != MouseOverGuardian)
            {
                DialogueDelayTime = 0;
            }
        }

        public static bool IsInChattingRange(TerraGuardian tg)
        {
            if (giantsummon.Dialogue.IsImportantDialogue())
                return true;
            if (DialogueCloseCheckDelayTime > 0)
            {
                DialogueCloseCheckDelayTime--;
                return true;
            }
            Rectangle rect = new Rectangle((int)(MainPlayer.position.X + MainPlayer.width * 0.5f - Player.tileRangeX * 16 - tg.Width * 0.5f),
                (int)(MainPlayer.position.Y + MainPlayer.height * 0.5f - Player.tileRangeY * 16 - tg.Height * 0.5f),
                Player.tileRangeX * 32 + tg.Width,
                Player.tileRangeY * 32 + tg.Height);
            return tg.HitBox.Intersects(rect);
        }

        public static void DrawMouseOver()
        {
            UpdateMouseOver();
            if (SomeoneMouseOver && MainMod.ActiveGuardians.ContainsKey(MouseOverGuardian))
            {
                TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                if (IsInChattingRange(tg) && !tg.Downed && !tg.KnockedOut && !MainPlayer.GetModPlayer<PlayerMod>().KnockedOut)
                {
                    Vector2 DialogueBubblePosition = new Vector2(tg.Position.X - Main.chatTexture.Width * 0.5f - (-tg.Width * 0.5f - 8) * tg.Direction, tg.Position.Y - tg.Height - Main.chatTexture.Height) - Main.screenPosition;
                    if ((tg.OwnerPos == Main.myPlayer && DialogueDelayTime >= DialogueMaxDelayTime) || tg.OwnerPos == -1 || (tg.IsCommander && tg.GetCommanderLeaderID == Main.myPlayer) ||
                                (tg.OwnerPos > -1 && Main.player[tg.OwnerPos].GetModPlayer<PlayerMod>().IsCompanionParty) ||
                        (Main.LocalPlayer.GetModPlayer<PlayerMod>().CompanionCommanderLeaderPlayer == tg.OwnerPos))
                    {
                        Main.spriteBatch.Draw(Main.chatTexture, DialogueBubblePosition, null, Main.mouseTextColorReal, 0f, Vector2.Zero, 1f, (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
                    }
                    else
                    {
                        Color color = Color.White * ((float)DialogueDelayTime / DialogueMaxDelayTime);
                        Utils.DrawBorderString(Main.spriteBatch, "...", DialogueBubblePosition, color, 2);
                    }
                }
                Vector2 TextPosition = tg.Position;
                TextPosition.Y += 22;
                //TextPosition.Y -= tg.Base.SpriteHeight - 22;
                string Text = (tg.OwnerPos == MainPlayer.whoAmI ? tg.Name : tg.ReferenceName) + " " + tg.HP + "/" + tg.MHP;
                Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition - Main.screenPosition, Color.White * Main.cursorAlpha, 1f, 0.5f, 1f);
            }
        }

        public static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            int HalfHeight = (int)(Height * 0.5f);
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    const int DrawDimension = 30;
                    int px = (int)Position.X, py = (int)Position.Y, pw = DrawDimension, ph = DrawDimension,
                        dx = 0, dy = 0, dh = DrawDimension;
                    if (x == 2)
                    {
                        px += Width - pw;
                        dx = Main.chatBackTexture.Width - DrawDimension;
                    }
                    else if (x == 1)
                    {
                        px += pw;
                        pw = Width - pw * 2;
                        dx = DrawDimension;
                    }
                    if (y == 2)
                    {
                        py += Height - ph;
                        dy = Main.chatBackTexture.Height - DrawDimension;
                        if (ph > HalfHeight)
                        {
                            dy += DrawDimension - HalfHeight;
                            py += (int)(DrawDimension - HalfHeight);
                            ph = dh = HalfHeight;
                        }
                    }
                    else if (y == 1)
                    {
                        py += ph;
                        ph = Height - ph * 2;
                        dy = DrawDimension;
                    }
                    else
                    {
                        if (ph > HalfHeight)
                        {
                            ph = dh = HalfHeight;
                        }
                    }
                    if (pw > 0 && ph > 0)
                    {
                        Main.spriteBatch.Draw(Main.chatBackTexture, new Rectangle(px, py, pw, ph), new Rectangle(dx, dy, DrawDimension, dh), color);
                    }
                }
            }
        }

        public static void DrawDialogue()
        {
            UpdateDialogueMouse();
            if (!MainPlayer.GetModPlayer<PlayerMod>().IsTalkingToAGuardian || GuardianManagement.Active)
                return;
            if (GuardianShopInterface.ShopOpen)
            {
                GuardianShopInterface.DrawShop();
                return;
            }
            int WindowSizeX = DialogueWidth, WindowSizeY = DialogueHeight;
            Vector2 WindowStartPosition = new Vector2(DialogueStartX, DialogueStartY);
            if (Main.mouseX >= WindowStartPosition.X && Main.mouseX < WindowStartPosition.X + WindowSizeX &&
                Main.mouseY >= WindowStartPosition.Y && Main.mouseY < WindowStartPosition.Y + WindowSizeY + 48)
                MainPlayer.mouseInterface = true;
            //WindowSizeY = LineCount * 30 + 60;
            {
                DrawBackgroundPanel(WindowStartPosition, 48, 48, Color.White);
                TerraGuardian tg = MainMod.ActiveGuardians[MainPlayer.GetModPlayer<PlayerMod>().TalkingGuardianPosition];
                tg.DrawHead(WindowStartPosition + new Vector2(24, 24));
                WindowStartPosition.X += 48;
                DrawBackgroundPanel(WindowStartPosition, WindowSizeX - 48, 48, Color.White);
                WindowStartPosition.X += 4;
                Utils.DrawBorderStringBig(Main.spriteBatch, tg.Name, WindowStartPosition, Color.White);
                WindowStartPosition.X -= 52;
                tg.DrawFriendshipHeart(WindowStartPosition + new Vector2(0, 24));
                WindowStartPosition.Y += 48;
            }
            Color color = new Color(200, 200, 200, 200);
            DrawBackgroundPanel(WindowStartPosition, WindowSizeX, WindowSizeY, color);
            for (int i = 0; i < Dialogue.Length; i++)
            {
                if (Dialogue[i] != null)
                {
                    Vector2 TextPosition = WindowStartPosition;
                    TextPosition.X += 8;
                    TextPosition.Y += 6 + 30 * i;
                    Utils.DrawBorderString(Main.spriteBatch, Dialogue[i], TextPosition, Color.White);
                }
            }
            WindowStartPosition.Y += WindowSizeY;
            //Options window here.
            if (Options.Count > 0)
            {
                WindowSizeY = Options.Count * 30 + 16;
                if (Main.mouseX >= WindowStartPosition.X && Main.mouseX < WindowStartPosition.X + WindowSizeX &&
                    Main.mouseY >= WindowStartPosition.Y && Main.mouseY < WindowStartPosition.Y + WindowSizeY)
                    MainPlayer.mouseInterface = true;
                color *= 0.75f;
                DrawBackgroundPanel(WindowStartPosition, WindowSizeX, WindowSizeY, color);
                for (int o = 0; o < Options.Count; o++)
                {
                    Vector2 OptionPosition = WindowStartPosition;
                    OptionPosition.Y += 30 * o + 8;
                    OptionPosition.X += 8f;
                    color = (MouseOverOptionNumber == o ? Color.Yellow : Color.White);
                    Utils.DrawBorderString(Main.spriteBatch, Options[o].Text, OptionPosition, color);
                }
            }
        }

        public static void RefreshToStarterGuardian()
        {
            Speaker = StarterSpeaker;
        }

        public static void GetDefaultOptions()
        {
            RefreshToStarterGuardian();
            Options.Clear();
            if (Speaker.HasBuff(ModContent.BuffType<Buffs.Sleeping>()))
            {

            }
            else if (Speaker.IsSleeping && Speaker.Base.SleepsAtBed)
            {
                AddOption("Wake Up", WakeUpCompanionButtonAction);
            }
            else
            {
                if (!Speaker.Base.InvalidGuardian)
                {
                    if (!Speaker.IsCommander && !HideCallDismissButton)
                    {
                        if (!PlayerMod.HasBuddiesModeOn(Main.player[Main.myPlayer]) || !PlayerMod.GetPlayerBuddy(Main.player[Main.myPlayer]).IsSameID(Speaker))
                        {
                            ReferedGroup = Main.myPlayer;
                            if (Speaker.OwnerPos == Main.myPlayer || (Speaker.OwnerPos > -1 && Main.netMode == 0))
                            {
                                ReferedGroup = Speaker.OwnerPos;
                                AddOption("Remove from the group", AskGuardianToLeaveGroupButtonPressed);
                            }
                            else if(Speaker.OwnerPos == -1)
                            {
                                AddOption("Want to join my adventure?", AskGuardianToFollowYouButtonPressed);
                                for(int i = 0; i < 255; i++)
                                {
                                    if(Main.player[i].active && i != Main.myPlayer)
                                    {
                                        int MyID = i;
                                        AddOption("Join " + Main.player[i].name + "'s Party", delegate ()
                                        {
                                            ReferedGroup = MyID;
                                            AskGuardianToFollowYouButtonPressed();
                                        });
                                    }
                                }
                            }
                        }
                    }
                    if (Main.netMode == 0) // && Speaker.OwnerPos == -1
                    {
                        if (!Speaker.IsCommander)
                        {
                            if (Speaker.FriendshipLevel >= Speaker.Base.LeadGroupUnlockLevel && Speaker.OwnerPos == -1)
                            {
                                AddOption("Lead a group for me", SetLeadYourOwnGroup);
                            }
                        }
                        else
                        {
                            AddOption("Dismiss your group", RemoveFromLeadingGroup);
                        }
                    }
                    if (Speaker.IsCommander)
                    {
                        AddOption("Give Order", ChangeCommandingAction);
                    }
                    string OptionText = "Check Request";
                    if (Speaker.request.IsComplete)
                    {
                        OptionText = "Report Request";
                    }
                    else if (Speaker.request.state == RequestData.RequestState.Ready)
                    {
                        OptionText = "Need Something?";
                    }
                    else if (Speaker.request.state >= RequestData.RequestState.WaitingAccept)
                    {
                        OptionText = "Let's talk about your request?";
                    }
                    if (OptionText != "")
                        AddOption(OptionText, CheckRequestButtonAction);
                    if (GuardianShopHandler.HasShop(Speaker.MyID))
                    {
                        AddOption("What do you have for sale?", OpenShopButtonAction);
                    }
                    if (MayGiveBirthdayGift(Speaker) && (!Speaker.DoAction.InUse || Speaker.DoAction.IsGuardianSpecificAction || Speaker.DoAction.ID != (int)GuardianActions.ActionIDs.OpenGiftBox))
                    {
                        AddOption("I have a gift for you", GiveGiftButtonAction);
                    }
                    if (true || Speaker.Base.Topics.Count > 0)
                    {
                        AddOption("Let's talk about other things.", LetsChatButtonPressed);
                    }
                    if (Speaker.Base.Roles.HasFlag(GuardianBase.GuardianRoles.PopularityContestHost))
                    {
                        switch (MainMod.PopContestState)
                        {
                            case 1:
                                AddOption("About TerraGuardians Popularity Contest.", AskAboutTerraGuardiansPopularityContestButtonPress);
                                break;
                            case 2:
                                AddOption("Check TerraGuardians Popularity Contest Result.", OnPopContestResultButtonPress);
                                break;
                        }
                    }
                    AddOption("Let's review some things.", SomethingElseButtonPressed);
                    foreach (QuestData qd in PlayerMod.GetPlayerQuestDatas(MainPlayer))
                    {
                        if (!qd.IsInvalid)
                        {
                            QuestBase.Data = qd;
                            foreach (DialogueOption Do in qd.GetBase.AddDialogueOptions(false, Speaker.ID, Speaker.ModID))
                            {
                                Do.SetAsThreadedDialogue();
                                Do.SetQuestData(qd);
                                Options.Add(Do);
                            }
                        }
                    }
                    if (Speaker.OwnerPos == Main.myPlayer && PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        AddOption("Let's get some rest.", RestButtonAction);
                    }
                }
                if (Speaker.OwnerPos == -1 && Speaker.FriendshipLevel >= Speaker.Base.FriendsLevel)
                    AddOption("Let me see your inventory.", OpenInventoryManagementButtonAction);
                Options.AddRange(Speaker.Base.GetGuardianExtraDialogueActions(Speaker));
            }
            AddOption("Goodbye", CloseDialogueButtonAction);
        }

        public static void ChangeCommandingAction()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CommandingChangeOrder, "*What you want me to do?*"));
            if (Speaker.IsCommander)
            {
                Options.Clear();
                PlayerMod.CommandingOrders order = Main.player[Speaker.CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder;
                if (order != PlayerMod.CommandingOrders.Idle) AddOption("Just idle.", SetIdleCommand);
                if (order != PlayerMod.CommandingOrders.FollowLeader) AddOption("Follow me.", SetFollowCommand);
                if (order != PlayerMod.CommandingOrders.Defend) AddOption("Defend this place.", SetDefendCommand);
                if (order != PlayerMod.CommandingOrders.Explore) AddOption("Explore the world.", SetExploreCommand);
                AddOption("Nevermind.", delegate ()
                {
                    GetDefaultOptions();
                    SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.GenericNevermind, "*Alright then.*"), Speaker);
                });
            }
            else
            {
                GetDefaultOptions();
            }
        }

        public static void SetIdleCommand()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.GenericYes, "*I will do.*"));
            if (Speaker.IsCommander)
            {
                Main.player[Speaker.CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder = PlayerMod.CommandingOrders.Idle;
            }
            GetDefaultOptions();
        }

        public static void SetDefendCommand()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.GenericYes, "*I will do.*"));
            if (Speaker.IsCommander)
            {
                Main.player[Speaker.CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder = PlayerMod.CommandingOrders.Defend;
            }
            GetDefaultOptions();
        }

        public static void SetExploreCommand()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.GenericYes, "*I will do.*"));
            if (Speaker.IsCommander)
            {
                Main.player[Speaker.CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder = PlayerMod.CommandingOrders.Explore;
            }
            GetDefaultOptions();
        }

        public static void SetFollowCommand()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.GenericYes, "*I will do.*"));
            if (Speaker.IsCommander)
            {
                Main.player[Speaker.CommanderCharacterID].GetModPlayer<PlayerMod>().CommandingOrder = PlayerMod.CommandingOrders.FollowLeader;
            }
            GetDefaultOptions();
        }

        public static void SetLeadYourOwnGroup()
        {
            if (Speaker.SetAsCommander(Main.myPlayer))
            {
                ChangeCommandingAction();
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CommandingLeadGroupSuccess, "*I shall lead my own group then.*"));
            }
            else
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CommandingLeadGroupFail, "*I can't do that.*"));
            }
            GetDefaultOptions();
        }

        public static void RemoveFromLeadingGroup()
        {
            Speaker.RemoveFromCommanding();
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CommandingDisbandGroup, "*My group is gone now.*"));
            GetDefaultOptions();
        }

        public static void LetsChatButtonPressed()
        {
            Options.Clear();
            string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.ChatAboutSomething);
            if (Mes != "")
            {
                SetDialogue(Mes, Speaker);
            }
            foreach (GuardianBase.DialogueTopic topic in Speaker.Base.Topics)
            {
                if (topic.Requirement(Speaker, MainPlayer.GetModPlayer<PlayerMod>()))
                {
                    Action a = topic.TopicMethod;
                    AddOption(topic.TopicText, delegate ()
                    {
                        giantsummon.Dialogue.StartNewDialogue(a, Speaker);
                    });
                }
            }
            foreach (QuestData qd in PlayerMod.GetPlayerQuestDatas(MainPlayer))
            {
                if (!qd.IsInvalid)
                {
                    QuestBase.Data = qd;
                    foreach (DialogueOption Do in qd.GetBase.AddDialogueOptions(true, Speaker.ID, Speaker.ModID))
                    {
                        Do.SetQuestData(qd);
                        Do.SetAsThreadedDialogue();
                        Options.Add(Do);
                    }
                }
            }
            AddOption("Nevermind", delegate ()
            {
                GetDefaultOptions();
                string Mes2 = Speaker.GetMessage(GuardianBase.MessageIDs.NevermindTheChatting);
                if (Mes2 != "")
                    SetDialogue(Mes2, Speaker);
            });
        }

        public static void AddOption(string Mes, Action Action, bool Threaded = false)
        {
            DialogueOption option = new DialogueOption(Mes, Action, Threaded);
            Options.Add(option);
        }

        //Call/Dismiss Related

        private static int ReferedGroup = 0;

        public static void AskGuardianToFollowYouButtonPressed()
        {
            HideCallDismissButton = true;
            if ((!Speaker.Data.IsStarter && Speaker.FriendshipLevel < Speaker.Base.CallUnlockLevel && (Speaker.request.state != RequestData.RequestState.Active || !Speaker.request.Base.RequiresRequesterSummoned)))
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupFail, "(They refused.)"), Speaker);
                GetDefaultOptions();
            }
            else
            {
                PlayerMod pm = Main.player[ReferedGroup].GetModPlayer<PlayerMod>();
                if (pm.Guardian.Active && pm.GuardianFollowersWeight + Speaker.Base.CompanionSlotWeight >= pm.MaxExtraGuardiansAllowed)
                {
                    SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty, "(There is no place for this companion in the group.)"), Speaker);
                }
                else
                {
                    SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess, "(It seems happy to follow you.)"), Speaker);
                    pm.CallGuardian(Speaker.ID, Speaker.ModID);
                }
                GetDefaultOptions();
            }
        }

        public static void AskGuardianToLeaveGroupButtonPressed()
        {
            Options.Clear();
            //If not in town, ask if the player is sure. If in town, go right away to Yes option press.
            if (Speaker.TownNpcs >= 1)
                AskGuardianToLeaveGroupYesButtonPressed();
            else
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure, "(It asks if It's really okay to leave It alone in this place.)"), Speaker);
                AddOption("Yes", AskGuardianToLeaveGroupYesButtonPressed);
                AddOption("No", AskGuardianToLeaveGroupNoButtonPressed);
            }
        }

        public static void AskGuardianToLeaveGroupYesButtonPressed()
        {
            PlayerMod pm = Main.player[ReferedGroup].GetModPlayer<PlayerMod>();
            pm.DismissGuardian(Speaker.ID, Speaker.ModID, false);
            if (Speaker.TownNpcs < 3)
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace, "(They say that will try getting to the town safelly.)"), Speaker);
            }
            else
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer, "(They give you a farewell, and wishes you a good journey.)"), Speaker);
            }
            HideCallDismissButton = true;
            GetDefaultOptions();
        }

        public static void AskGuardianToLeaveGroupNoButtonPressed()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer, "(They say that didn't wanted to leave the group, anyway.)"), Speaker);
            GetDefaultOptions();
        }

        //End Call/Dismiss Related

        //Popularity Contest Vote

        public static void AskAboutTerraGuardiansPopularityContestButtonPress()
        {
            Options.Clear();
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestMessage, "(They're talking to me about the Popularity Contest up and running.\nThey say that I can vote on it now.)") + "\n(Contest will be running until "+MainMod.ContestEndDate.ToShortDateString()+".)");
            AddOption("Vote", OnPopContestVoteButtonPress);
            AddOption("Introduction", OnPopContestIntroductionButtonPress);
            AddOption("Return", OnPopContestReturnToLobby);
        }

        public static void OnPopContestReturnToLobby()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestOnReturnToOtherTopics, "(They tell you to vote on the contest before end date arrives.)"));
            GetDefaultOptions();
        }

        public static void OnPopContestVoteButtonPress()
        {
            Options.Clear();
            SetDialogue("(Clicking the Vote link will open a link on your computer browser, which will lead to a Google Form so you can input your vote.\n" +
                "You will need a Google account in order to vote.)");
            AddOption("Vote (Open Browser Link to Votting)", OnPopContestOpenVoteLinkButtonPress);
            AddOption("I've changed my mind...", OnPopContestReturnToLobby);
        }

        public static void OnPopContestOpenVoteLinkButtonPress()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestLinkOpen, "(They wish you a happy voting, and hopes to see you at the nominations.)"));
            System.Diagnostics.Process.Start(MainMod.VoteLink);
            GetDefaultOptions();
        }

        public static void OnPopContestIntroductionButtonPress()
        {
            Options.Clear();
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestIntroduction, "(They explain to me that the TerraGuardians Popularity Contest is an event where Terrarians can vote on their favorite companions, and in the nominations we find out who are the most voted ones.\n" +
                "Beside the name being TerraGuardians Popularity Contest, other companions can also participate.)"));
            AddOption("I want to vote.", OnPopContestVoteButtonPress);
            AddOption("I will think about that.", OnPopContestReturnToLobby);
        }

        //Popularity Contest Result Messages

        public static void OnPopContestResultButtonPress()
        {
            Options.Clear();
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestResultMessage, "(The results of the Popularity Contest were announced. You can check the results here.)"));
            AddOption("Check the Results.", OnPopContestResultCheckPress);
            AddOption("Nevermind", OnPopContestResultNevermindPress);
        }

        public static void OnPopContestResultCheckPress()
        {
            Options.Clear();
            SetDialogue("(Clicking to check the results, will open a Steam Group Chat page with the result on your browser.)");
            AddOption("Open Results Page.", OnPopContestResultPostWarningPress);
            AddOption("I've changed my mind.", OnPopContestResultNevermindPress);
        }

        public static void OnPopContestResultPostWarningPress()
        {
            System.Diagnostics.Process.Start(MainMod.ContestResultLink);
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestResultLinkClickMessage, "(They look curious about what are the results.)"));
            GetDefaultOptions();
        }

        public static void OnPopContestResultNevermindPress()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.PopContestResultNevermindMessage, "(They tell you that you can check that other time.)"));
            GetDefaultOptions();
        }

        //Popularity Contest End

        public static void SomethingElseButtonPressed()
        {
            Options.Clear();
            AddOption("Change my Nickname.", ChangeNicknameButtonPressed);
            AddOption("Nevermind.", GetDefaultOptions);
        }

        public static void ChangeNicknameButtonPressed()
        {
            Main.chatText = "/changenickname ";
            Main.drawingPlayerChat = true;
            CloseDialogueButtonAction();
        }

        public static void WakeUpCompanionButtonAction()
        {
            if (Speaker.IsSleeping)
            {
                //Speaker.LeaveFurniture(true);
                Speaker.LookAt(MainPlayer.Center);
                Speaker.SitOnBed();
                string Message = "";
                if (Speaker.HasRequestActive)
                    Message = Speaker.Base.GetSpecialMessage(GuardianBase.MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage);
                if (Message == "")
                    Message = Speaker.Base.GetSpecialMessage(GuardianBase.MessageIDs.GuardianWokeUpByPlayerMessage);
                if (Message == "")
                    Message = Speaker.Base.NormalMessage(MainPlayer, Speaker);
                SetDialogue(Message);
            }
            GetDefaultOptions();
        }

        public static void CheckRequestButtonAction()
        {
            GuardianData data = Speaker.Data;
            data.request.TryGettingNewRequest(MainPlayer, data);
            switch (data.request.state)
            {
                case RequestData.RequestState.Cooldown:
                    SetDialogue(data.Base.NoRequestMessage(MainPlayer, Speaker), Speaker);
                    break;
                case RequestData.RequestState.WaitingAccept:
                    {
                        string Mes = data.request.GetRequestBrief(MainPlayer, Speaker);
                        SetDialogue(Mes, Speaker);
                        Options.Clear();
                        AddOption("Accept", AcceptRequestButtonAction);
                        AddOption("Reject", RejectRequestButtonAction);
                        AddOption("Maybe later", PostponeRequestButtonAction);
                    }
                    break;
                case RequestData.RequestState.Active:
                    {
                        if (data.request.Failed)
                        {
                            data.request.FailedRequest(MainPlayer, Speaker);
                            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RequestFailed), Speaker);
                            GetDefaultOptions();
                        }
                        else if (data.request.IsComplete)
                        {
                            string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RequestAsksIfCompleted, "(They seems to be wondering if you completed the request.)");
                            SetDialogue(Mes, Speaker);
                            //GiveOptionToCancelRequest = true;
                            //GetDefaultOptions(Speaker);
                            Options.Clear();
                            AddOption(Speaker.request.GetRequestReward(0), GetFirstRewardButtonAction);
                            AddOption(Speaker.request.GetRequestReward(1), GetSecondRewardButtonAction);
                            AddOption(Speaker.request.GetRequestReward(2), GetThirdRewardButtonAction);
                            data.request.TryGivingAtLeast30SecondsToCompleteRequest();
                        }
                        else
                        {
                            string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RequestRemindObjective, "(They tell you that you need to [objective] for them.)").Replace("[objective]", Speaker.request.GetShortDescription);
                            SetDialogue(Mes, Speaker);
                            Options.Clear();
                            AddOption("Cancel Request", CancelRequestButtonAction);
                            AddOption("Thanks", delegate ()
                            {
                                GetDefaultOptions();
                            });
                        }
                    }
                    break;
            }
        }

        private static void GetFirstRewardButtonAction()
        {
            RequestData request = Speaker.request;
            if (request.CompleteRequest(MainPlayer, Speaker, 0))
            {
                string Mes = Speaker.Base.CompletedRequestMessage(MainPlayer, Speaker);
                SetDialogue(Mes, Speaker);
                Options.Clear();
                AddOption("Ok", delegate ()
                {
                    if (!ShowImportantMessages())
                    {
                        GetDefaultOptions();
                    }
                });
            }
            else
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RequestFailed), Speaker);
                GetDefaultOptions();
            }
        }

        private static void GetSecondRewardButtonAction()
        {
            RequestData request = Speaker.request;
            request.CompleteRequest(MainPlayer, Speaker, 1);
            string Mes = Speaker.Base.CompletedRequestMessage(MainPlayer, Speaker);
            SetDialogue(Mes, Speaker);
            Options.Clear();
            AddOption("Ok", delegate ()
            {
                if (!ShowImportantMessages())
                {
                    GetDefaultOptions();
                }
            });
        }

        private static void GetThirdRewardButtonAction()
        {
            RequestData request = Speaker.request;
            request.CompleteRequest(MainPlayer, Speaker, 2);
            string Mes = Speaker.Base.CompletedRequestMessage(MainPlayer, Speaker);
            SetDialogue(Mes, Speaker);
            Options.Clear();
            AddOption("Ok", delegate ()
            {
                if (!ShowImportantMessages())
                {
                    GetDefaultOptions();
                }
            });
        }

        public static void CancelRequestButtonAction()
        {
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CancelRequestAskIfSure, "(It seems like " + (Speaker.Male ? "he" : "she") + " is wonder if you are sure about that.)"));
            Options.Clear();
            AddOption("No", delegate ()
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CancelRequestNoAnswered, (Speaker.Male ? "He" : "She") + " seems relieved after hearing that.)"));
                GetDefaultOptions();
            });
            AddOption("Yes", delegate ()
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.CancelRequestYesAnswered, (Speaker.Male ? "He" : "She") + " seems a bit disappointed towards you.)"));
                GetDefaultOptions();
                Speaker.request.OnCancelRequest(MainPlayer, Speaker);
            });
        }

        public static void RestButtonAction()
        {
            Options.Clear();
            if (Main.bloodMoon || Main.eclipse || Main.invasionType >= Terraria.ID.InvasionID.GoblinArmy) //Todo - Add dialogues for when the companion asks for how long will rest, and for when It's not possible to
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RestNotPossible, "(Maybe It's not a good idea to rest right now.)"));
            }
            else
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RestAskForHowLong, "(How long should we rest?)"));
                AddOption("4 Hours", delegate ()
                {
                    if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, Speaker);
                            Speaker.SaySomething(Mes);
                        }
                        GuardianActions.RestCommand(Speaker, 0);
                        CloseDialogue(Speaker);
                    }
                });
                AddOption("8 Hours", delegate ()
                {
                    string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                    if (Mes != "")
                    {
                        Mes = MessageParser(Mes, Speaker);
                        Speaker.SaySomething(Mes);
                    }
                    if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        GuardianActions.RestCommand(Speaker, 1);
                        CloseDialogue(Speaker);
                    }
                });
                if (!Main.dayTime)
                {
                    AddOption("Until Dawn", delegate ()
                    {
                        string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, Speaker);
                            Speaker.SaySomething(Mes);
                        }
                        if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                        {
                            GuardianActions.RestCommand(Speaker, 2);
                            CloseDialogue(Speaker);
                        }
                    });
                }
                else
                {
                    AddOption("Until Night", delegate ()
                    {
                        string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, Speaker);
                            Speaker.SaySomething(Mes);
                        }
                        if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                        {
                            GuardianActions.RestCommand(Speaker, 3);
                            CloseDialogue(Speaker);
                        }
                    });
                }
            }
            AddOption("Nevermind", delegate ()
            {
                GetDefaultOptions();
            });
        }

        public static void AcceptRequestButtonAction()
        {
            if (PlayerMod.GetPlayerAcceptedRequestCount(MainPlayer) >= RequestData.MaxRequests)
            {
                SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RequestCantAcceptTooManyRequests, "(You have too many requests active.)"), Speaker);
            }
            else
            {
                Speaker.request.OnAcceptingRequest();
                string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RequestAccepted);
                if (Mes == "")
                    Mes = "(You accepted the request.)";
                SetDialogue(Mes, Speaker);
            }
            GetDefaultOptions();
        }

        public static void RejectRequestButtonAction()
        {
            Speaker.request.OnRejectingRequest();
            SetDialogue(Speaker.GetMessage(GuardianBase.MessageIDs.RequestRejected), Speaker);
            GetDefaultOptions();
        }

        public static void CloseDialogueButtonAction()
        {
            CloseDialogue(Speaker);
        }

        public static void CloseDialogue(TerraGuardian tg = null)
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            pm.IsTalkingToAGuardian = false;
            if (tg != null)
            {
                if (Speaker != null)
                    Speaker.TalkPlayerID = -1;
                else
                    tg.TalkPlayerID = -1;
            }
            Speaker = null;
            Options.Clear();
            if (giantsummon.Dialogue.DialogueThread != null && giantsummon.Dialogue.DialogueThread.IsAlive)
                giantsummon.Dialogue.DialogueThread.Abort();
            Dialogue = new string[0];
        }

        public static void GiveGiftButtonAction()
        {
            int GiftSlot = -1, EmptyGuardianSlot = -1;
            for (int i = 0; i < 50; i++)
            {
                if (Speaker.Inventory[i].type == 0)
                {
                    EmptyGuardianSlot = i;
                }
                if (MainPlayer.inventory[i].type == ModContent.ItemType<Items.Misc.BirthdayPresent>())
                {
                    GiftSlot = i;
                }
            }
            if (GiftSlot > -1 && EmptyGuardianSlot > -1)
            {
                Speaker.Inventory[EmptyGuardianSlot] = MainPlayer.inventory[GiftSlot].Clone();
                MainPlayer.inventory[GiftSlot].SetDefaults(0, true);
                GuardianActions.OpenBirthdayPresent(Speaker, EmptyGuardianSlot);
                SetDialogue("*You gave the gift.*");
                GetDefaultOptions();
                return;
            }
            else
            {
                SetDialogue("(Something went wrong... Either the guardian has no inventory space free, or you don't have a gift.)");
            }
        }

        public static bool MayGiveBirthdayGift(TerraGuardian Guardian)
        {
            bool GiveGift = false;
            Player player = Main.player[Main.myPlayer];
            if (!Guardian.DoAction.InUse && PlayerMod.PlayerHasGuardian(player, Guardian.ID, Guardian.ModID) && PlayerMod.IsGuardianBirthday(player, Guardian.ID, Guardian.ModID) && !PlayerMod.HasGuardianBeenGifted(player, Guardian.ID, Guardian.ModID) &&
                player.HasItem(ModContent.ItemType<Items.Misc.BirthdayPresent>()))
            {
                GuardianData gd = PlayerMod.GetPlayerGuardian(player, Guardian.ID, Guardian.ModID);
                for (int i = 0; i < 50; i++)
                {
                    if (gd.Inventory[i].type == 0)
                    {
                        GiveGift = true;
                        break;
                    }
                }
            }
            return GiveGift;
        }

        public static void PostponeRequestButtonAction()
        {
            string Mes = Speaker.GetMessage(GuardianBase.MessageIDs.RequestPostpone);
            if (Mes != "")
                Speaker.SaySomething(MessageParser(Mes, Speaker));
            CloseDialogueButtonAction();
        }

        public static void OpenShopButtonAction()
        {
            GuardianShopInterface.OpenShop();
        }

        public static void OpenInventoryManagementButtonAction()
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            foreach (int Key in pm.MyGuardians.Keys)
            {
                if (pm.MyGuardians[Key].ID == Speaker.ID && pm.MyGuardians[Key].ModID == Speaker.ModID)
                {
                    GuardianManagement.OpenInterfaceForGuardian(Key);
                    break;
                }
            }
        }

        public static string MessageParser(string Message, TerraGuardian guardian)
        {
            Message = Message.Replace("[name]", guardian.Name);
            Message = Message.Replace("[nickname]", guardian.PersonalNicknameToPlayer != null ? guardian.PersonalNicknameToPlayer : Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetNickname);
            string FinalMessage = "";
            string CommandType = "";
            string CommandValue = "", CommandValue2 = "";
            bool ParsingCommand = false, GettingValue = false, GettingValue2 = false;
            for (int s = 0; s < Message.Length; s++)
            {
                Char c = Message[s];
                if (c == '[')
                {
                    ParsingCommand = true;
                    GettingValue = false;
                    GettingValue2 = false;
                    CommandType = "";
                    CommandValue = "";
                    CommandValue2 = "";
                }
                else if (c == ']')
                {
                    ParsingCommand = false;
                    if (CommandType == "gn")
                    {
                        PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                        int gi = int.Parse(CommandValue);
                        string mi = CommandValue2;
                        if (player.HasGuardian(gi, mi))
                        {
                            FinalMessage += player.GetGuardian(gi).Name;
                        }
                        else
                        {
                            FinalMessage += GuardianBase.GetGuardianBase(gi).Name;
                        }
                    }
                    else if (CommandType == "nn")
                    {
                        int ni = int.Parse(CommandValue);
                        if (NPC.AnyNPCs(ni))
                        {
                            FinalMessage += NPC.firstNPCName(ni);
                        }
                        else
                        {
                            FinalMessage += "???";
                        }
                    }
                    else
                    {
                        FinalMessage += "[" + CommandType + "]";
                        CommandValue = CommandValue2 = "";
                        GettingValue = GettingValue2 = false;
                    }
                }
                else if (ParsingCommand)
                {
                    if (c == ':')
                    {
                        if (!GettingValue)
                        {
                            GettingValue = true;
                            CommandValue = "";
                        }
                        else if (!GettingValue2)
                        {
                            GettingValue2 = true;
                            CommandValue2 = "";
                        }
                    }
                    else if (!GettingValue)
                        CommandType += c;
                    else if (GettingValue2)
                    {
                        CommandValue2 += c;
                    }
                    else
                    {
                        CommandValue += c;
                    }
                }
                else
                {
                    FinalMessage += c;
                }
            }
            return FinalMessage;
        }
    }

    public struct DialogueOption
    {
        public string Text;
        public Action Action;
        public bool ThreadedDialogue;
        public QuestData qd;

        public DialogueOption(string OptionText, Action Result, bool ThreadedDialogue = false)
        {
            Text = OptionText;
            Action = Result;
            this.ThreadedDialogue = ThreadedDialogue;
            qd = null;
        }

        public void SetQuestData(QuestData qd)
        {
            this.qd = qd;
        }

        public void SetAsThreadedDialogue()
        {
            ThreadedDialogue = true;
        }
    }
}
