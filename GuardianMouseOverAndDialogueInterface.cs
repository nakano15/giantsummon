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

        public static void SetDialogue(string Message)
        {
            SetDialogue(Message, null);
        }

        public static void SetDialogueDistanceAutoCloseDelay()
        {
            DialogueCloseCheckDelayTime = 60;
        }

        public static void SetDialogue(string Message, TerraGuardian tg) //And with auto parser :D
        {
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
            if(tg.LookingLeft != PlayerToTheLeft)
            {
                if (tg.UsingFurniture && tg.IsUsingChair)
                {
                    tg.LeaveFurniture(false);
                }
                tg.LookingLeft = PlayerToTheLeft;
            }
            if (MainPlayer.velocity == Vector2.Zero && ((MainPlayer.direction > 0 && !PlayerToTheLeft) || (MainPlayer.direction < 0 && PlayerToTheLeft)))
            {
                if (PlayerToTheLeft)
                    MainPlayer.direction = 1;
                else
                    MainPlayer.direction = -1;
            }
            //GetCompanionDialogue
            GetDefaultOptions(tg);
            string Message = "";
            if (tg.Base.InvalidGuardian)
            {
                Message = "(Your memories of It are fragmented, so you can't see It's true form, neither allow It to speak with you.)\nGuardian ID: "+tg.ID+", Guardian Mod ID: "+tg.ModID+".\nIf this isn't related to a mod you uninstalled, send It's to the mod developer.";
            }
            else if (!modPlayer.HasGuardian(tg.ID, tg.ModID))
            {
                modPlayer.AddNewGuardian(tg.ID, tg.ModID);
                if (tg.ID == WorldMod.SpawnGuardian.Key && tg.ModID == WorldMod.SpawnGuardian.Value)
                    modPlayer.GetGuardian(tg.ID, tg.ModID).SetStarterGuardian();
                Message = tg.Base.GreetMessage(MainPlayer, tg);
            }
            else
            {
                if (PlayerMod.IsGuardianBirthday(MainPlayer, tg.ID, tg.ModID) && Main.rand.Next(2) == 0)
                    Message = tg.Base.BirthdayMessage(MainPlayer, tg);
                else
                {
                    string t;
                    if (tg.Data.CheckForImportantMessages(out t))
                    {
                        Message = t;
                    }
                    else if (tg.OwnerPos == -1 && tg.GetTownNpcInfo != null && tg.GetTownNpcInfo.Homeless && tg.FriendshipLevel >= tg.Base.MoveInLevel)
                        Message = tg.Base.HomelessMessage(MainPlayer, tg);
                    else
                        Message = tg.Base.NormalMessage(MainPlayer, tg);
                }
            }
            if (!tg.Base.InvalidGuardian && !NpcMod.HasMetGuardian(tg.ID, tg.ModID))
            {
                Message = tg.Base.GreetMessage(MainPlayer, tg);
                NpcMod.AddGuardianMet(tg.ID, tg.ModID);
                if (WorldMod.HasEmptyGuardianNPCSlot())
                    WorldMod.AllowGuardianNPCToSpawn(tg.ID, tg.ModID);
                if (modPlayer.HasGuardian(tg.ID, tg.ModID))
                    modPlayer.GetGuardian(tg.ID, tg.ModID).IncreaseFriendshipProgress(1);
            }
            if (!tg.Base.InvalidGuardian && tg.HasBuff(ModContent.BuffType<Buffs.Sleeping>()))
            {
                Message = "(Seems to be under a heavy sleep.)";
            }
            else
            {
                PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                foreach (int gid in pm.MyGuardians.Keys)
                {
                    GuardianData gd = pm.MyGuardians[gid];
                    if (gd.request.requestState == RequestData.RequestState.RequestActive && gd.request.GetRequestBase(gd).Objectives.Any(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian))
                    {
                        RequestBase rb = gd.request.GetRequestBase(gd);
                        bool HasObjective = false;
                        for (int obj = 0; obj < rb.Objectives.Count; obj++)
                        {
                            if (rb.Objectives[obj].objectiveType == RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian && gd.request.GetIntegerValue(obj) == 0)
                            {
                                RequestBase.TalkToGuardianRequestObjective to = (RequestBase.TalkToGuardianRequestObjective)rb.Objectives[obj];
                                if (to.GuardianID == tg.ID && to.ModID == tg.ModID)
                                {
                                    HasObjective = true;
                                    Message = to.MessageText;
                                    gd.request.SetIntegerValue(obj, 1);
                                    break;
                                }
                            }
                        }
                        if (HasObjective)
                            break;
                    }
                }
            }
            SetDialogue(Message, tg);
        }

        public static void Update()
        {
            if (Options.Count == 0 && !giantsummon.Dialogue.IsDialogue)
            {
                AddOption("Close", CloseDialogueButtonAction);
            }
            SomeoneMouseOver = false;
            int LastMouseOverGuardian = MouseOverGuardian;
            MouseOverGuardian = -1;
            int[] Keys = MainMod.ActiveGuardians.Keys.ToArray();
            float MouseX = Main.mouseX + Main.screenPosition.X,
                MouseY = Main.mouseY + Main.screenPosition.Y;
            foreach (int key in Keys)
            {
                if (!MainMod.ActiveGuardians[key].IsPlayerHostile(Main.player[Main.myPlayer]))
                {
                    float Left = MainMod.ActiveGuardians[key].Position.X - MainMod.ActiveGuardians[key].Width * 0.5f,
                        Bottom = MainMod.ActiveGuardians[key].Position.Y,
                        Right = Left + MainMod.ActiveGuardians[key].Width,
                        Top = Bottom - MainMod.ActiveGuardians[key].Height;
                    if (MouseX >= Left && MouseX < Right && MouseY >= Top && MouseY < Bottom)
                    {
                        SomeoneMouseOver = true;
                        MouseOverGuardian = key;
                    }
                }
            }
            if (LastMouseOverGuardian != MouseOverGuardian)
            {
                DialogueDelayTime = 0;
            }
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (!player.IsTalkingToAGuardian && GuardianShopInterface.ShopOpen)
                GuardianShopInterface.ShopOpen = false;
            if (player.KnockedOut)
            {
                if (player.IsTalkingToAGuardian)
                    player.IsTalkingToAGuardian = false;
            }
            else if (!player.IsTalkingToAGuardian)
            {
                if (SomeoneMouseOver)
                {
                    if (DialogueDelayTime < DialogueMaxDelayTime)
                        DialogueDelayTime++;
                    if (!player.IsTalkingToAGuardian || player.TalkingGuardianPosition != MouseOverGuardian)
                    {
                        if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                            if (!tg.IsAttackingSomething && !tg.Downed && !tg.KnockedOut && IsInChattingRange(tg) && ((tg.OwnerPos == Main.myPlayer && DialogueDelayTime >= DialogueMaxDelayTime) || tg.OwnerPos == -1))
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
                    player.IsTalkingToAGuardian = false;
                    return;
                }
                TerraGuardian tg = MainMod.ActiveGuardians[player.TalkingGuardianPosition];
                if (!IsInChattingRange(tg) || (Main.playerInventory && !GuardianShopInterface.ShopOpen && !GuardianManagement.Active) || (MainPlayer.talkNPC > -1 && Main.npc[MainPlayer.talkNPC].active) || MainPlayer.sign > -1 || MainPlayer.chest > -1 || tg.Downed || tg.KnockedOut)
                {
                    player.IsTalkingToAGuardian = false;
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
                            Options[MouseOverOptionNumber].Action(tg);
                        }
                    }
                }
            }
            if (ClickedOnce && !Main.mouseLeft)
                ClickedOnce = false;
        }

        public static bool IsInChattingRange(TerraGuardian tg)
        {
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
            if (SomeoneMouseOver && MainMod.ActiveGuardians.ContainsKey(MouseOverGuardian))
            {
                TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                if (IsInChattingRange(tg) && !tg.Downed && !tg.KnockedOut && !MainPlayer.GetModPlayer<PlayerMod>().KnockedOut)
                {
                    Vector2 DialogueBubblePosition = new Vector2(tg.Position.X - Main.chatTexture.Width * 0.5f - (-tg.Width * 0.5f - 8) * tg.Direction, tg.Position.Y - tg.Height - Main.chatTexture.Height) - Main.screenPosition;
                    if ((tg.OwnerPos == Main.myPlayer && DialogueDelayTime >= DialogueMaxDelayTime) || tg.OwnerPos == -1)
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
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    int px = (int)Position.X, py = (int)Position.Y, pw = 30, ph = 30,
                        dx = 0, dy = 0;
                    const int DrawDimension = 30;
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
                    }
                    else if (y == 1)
                    {
                        py += ph;
                        ph = Height - ph * 2;
                        dy = DrawDimension;
                    }
                    if (pw > 0 && ph > 0)
                    {
                        Main.spriteBatch.Draw(Main.chatBackTexture, new Rectangle(px, py, pw, ph), new Rectangle(dx, dy, DrawDimension, DrawDimension), color);
                    }
                }
            }
        }

        public static void DrawDialogue()
        {
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
                tg.DrawFriendshipHeart(WindowStartPosition + new Vector2(24,24));
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
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, Dialogue[i], TextPosition.X, TextPosition.Y, Color.White, Color.Black, Vector2.Zero, 1f);
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
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, Options[o].Text, OptionPosition.X, OptionPosition.Y, color, Color.Black, Vector2.Zero);
                }
            }
        }

        public static void GetDefaultOptions(TerraGuardian tg)
        {
            Options.Clear();
            if (tg.HasBuff(ModContent.BuffType<Buffs.Sleeping>()))
            {

            }
            else if (tg.IsUsingBed)
            {
                AddOption("Wake Up", WakeUpCompanionButtonAction);
            }
            else
            {
                if (!tg.Base.InvalidGuardian)
                {
                    if (!HideCallDismissButton)
                    {
                        if (tg.OwnerPos == Main.myPlayer)
                        {
                            AddOption("Remove from the group", AskGuardianToLeaveGroupButtonPressed);
                        }
                        else
                        {
                            AddOption("Want to join my adventure?", AskGuardianToFollowYouButtonPressed);
                        }
                    }
                    string OptionText = "Check Request";
                    if (tg.request.Failed)
                    {
                        OptionText = "Report Failing the Request";
                    }
                    else if (tg.request.RequestCompleted)
                    {
                        OptionText = "Report Request";
                    }
                    else if (tg.request.requestState == RequestData.RequestState.HasRequestReady)
                    {
                        if (tg.request.IsTalkQuest)
                        {
                            OptionText = "You wanted to talk to me?";
                        }
                        else
                        {
                            OptionText = "Need Something?";
                        }
                    }
                    AddOption(OptionText, CheckRequestButtonAction);
                    if (GuardianShopHandler.HasShop(tg.MyID))
                    {
                        AddOption("What do you have for sale?", OpenShopButtonAction);
                    }
                    if (MayGiveBirthdayGift(tg))
                    {
                        AddOption("I have a gift for you", GiveGiftButtonAction);
                    }
                    if(tg.Base.Topics.Count > 0)
                    {
                        AddOption("I want to talk with you.", LetsChatButtonPressed);
                    }
                    AddOption("Talk about other things.", SomethingElseButtonPressed);
                    if (tg.OwnerPos == Main.myPlayer && PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        AddOption("Let's get some rest.", RestButtonAction);
                    }
                }
                if(tg.OwnerPos == -1)
                    AddOption("Let me see your inventory.", OpenInventoryManagementButtonAction);
                Options.AddRange(tg.Base.GetGuardianExtraDialogueActions(tg));
            }
            AddOption("Goodbye", CloseDialogueButtonAction);
        }

        public static void LetsChatButtonPressed(TerraGuardian tg)
        {
            Options.Clear();
            string Mes = tg.GetMessage(GuardianBase.MessageIDs.ChatAboutSomething);
            if(Mes != "")
            {
                SetDialogue(Mes, tg);
            }
            foreach (GuardianBase.DialogueTopic topic in tg.Base.Topics)
            {
                if (topic.Requirement(tg, MainPlayer))
                {
                    Action a = topic.TopicMethod;
                    AddOption(topic.TopicText, delegate (TerraGuardian tg2)
                    {
                        giantsummon.Dialogue.StartNewDialogue(a, tg2);
                    });
                }
            }
            AddOption("Nevermind", delegate(TerraGuardian tg2)
            {
                GetDefaultOptions(tg2);
                string Mes2 = tg.GetMessage(GuardianBase.MessageIDs.NevermindTheChatting);
                if (Mes2 != "")
                    SetDialogue(Mes2, tg2);
            });
        }

        public static void AddOption(string Mes, Action<TerraGuardian> Action)
        {
            DialogueOption option = new DialogueOption(Mes, Action);
            Options.Add(option);
        }

        //Call/Dismiss Related

        public static void AskGuardianToFollowYouButtonPressed(TerraGuardian tg)
        {
            if (!tg.Data.IsStarter && tg.FriendshipLevel < tg.Base.CallUnlockLevel && (!tg.request.Active || !tg.request.RequiresGuardianActive(tg.Data)))
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupFail, "(They refused.)"), tg);
                HideCallDismissButton = true;
                GetDefaultOptions(tg);
            }
            else
            {
                PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                if (pm.GetSummonedGuardianCount >= pm.MaxExtraGuardiansAllowed + 1)
                {
                    SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupFullParty, "(There is no place for this companion in the group.)"), tg);
                }
                else
                {
                    SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToJoinYourGroupSuccess, "(It seems happy to follow you.)"), tg);
                    pm.CallGuardian(tg.ID, tg.ModID);
                }
                HideCallDismissButton = true;
                GetDefaultOptions(tg);
            }
        }

        public static void AskGuardianToLeaveGroupButtonPressed(TerraGuardian tg)
        {
            Options.Clear();
            //If not in town, ask if the player is sure. If in town, go right away to Yes option press.
            if (tg.TownNpcs >= 1)
                AskGuardianToLeaveGroupYesButtonPressed(tg);
            else
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupAskIfYoureSure, "(It asks if It's really okay to leave It alone in this place.)"), tg);
                AddOption("Yes", AskGuardianToLeaveGroupYesButtonPressed);
                AddOption("No", AskGuardianToLeaveGroupNoButtonPressed);
            }
        }

        public static void AskGuardianToLeaveGroupYesButtonPressed(TerraGuardian tg)
        {
            PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            pm.DismissGuardian(tg.ID, tg.ModID);
            if (tg.TownNpcs < 3)
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupYesAnswerDangerousPlace, "(They say that will try getting to the town safelly.)"), tg);
            }
            else
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupSuccessAnswer, "(They give you a farewell, and wishes you a good journey.)"), tg);
            }
            HideCallDismissButton = true;
            GetDefaultOptions(tg);
        }

        public static void AskGuardianToLeaveGroupNoButtonPressed(TerraGuardian tg)
        {
            SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.AfterAskingCompanionToLeaveYourGroupNoAnswer, "(They say that didn't wanted to leave the group, anyway.)"), tg);
            GetDefaultOptions(tg);
        }

        //End Call/Dismiss Related

        public static void SomethingElseButtonPressed(TerraGuardian tg)
        {
            Options.Clear();
            AddOption("Change my Nickname.", ChangeNicknameButtonPressed);
            AddOption("Nevermind.", GetDefaultOptions);
        }

        public static void ChangeNicknameButtonPressed(TerraGuardian tg)
        {
            Main.chatText = "/changenickname ";
            Main.drawingPlayerChat = true;
            CloseDialogueButtonAction(tg);
        }

        public static void WakeUpCompanionButtonAction(TerraGuardian tg)
        {
            if (tg.IsUsingBed)
            {
                tg.LeaveFurniture(true);
                string Message = "";
                if (tg.HasRequestActive)
                    Message = tg.Base.GetSpecialMessage(GuardianBase.MessageIDs.GuardianWokeUpByPlayerRequestActiveMessage);
                if (Message == "")
                    Message = tg.Base.GetSpecialMessage(GuardianBase.MessageIDs.GuardianWokeUpByPlayerMessage);
                if (Message == "")
                    Message = tg.Base.NormalMessage(MainPlayer, tg);
                SetDialogue(Message);
            }
            GetDefaultOptions(tg);
        }

        public static void CheckRequestButtonAction(TerraGuardian tg)
        {
            GuardianData data = tg.Data;
            switch (data.request.requestState)
            {
                case RequestData.RequestState.Cooldown:
                    SetDialogue(data.Base.NoRequestMessage(MainPlayer, tg), tg);
                    break;
                case RequestData.RequestState.HasRequestReady:
                    if (data.request.IsTalkQuest)
                    {
                        data.request.CompleteRequest(tg, data, MainPlayer.GetModPlayer<PlayerMod>());
                        GetDefaultOptions(tg);
                    }
                    else
                    {
                        string Mes = data.request.GetRequestBrief(data, tg);
                        if (Mes == "")
                        {
                            Mes = data.Base.HasRequestMessage(MainPlayer, tg);
                        }
                        if (data.request.IsCommonRequest)
                        {
                            foreach (string s in data.request.GetRequestText(MainPlayer, data, true))
                            {
                                Mes += "\n" + s;
                            }
                        }
                        SetDialogue(Mes, tg);
                        Options.Clear();
                        AddOption("Accept", AcceptRequestButtonAction);
                        AddOption("Reject", RejectRequestButtonAction);
                        AddOption("Maybe later", PostponeRequestButtonAction);
                    }
                    break;
                case RequestData.RequestState.RequestActive:
                    {
                        if (data.request.IsTalkQuest && data.request.CompleteRequest(tg, data, MainPlayer.GetModPlayer<PlayerMod>()))
                        {
                            GetDefaultOptions(tg);
                        }
                        else if (data.request.Failed)
                        {
                            data.request.CompleteRequest(tg, data, MainPlayer.GetModPlayer<PlayerMod>());
                            SetDialogue(data.request.GetRequestFailed(data, tg), tg);
                            GetDefaultOptions(tg);
                        }
                        else if (data.request.RequestCompleted && data.request.CompleteRequest(tg, data, MainPlayer.GetModPlayer<PlayerMod>()))
                        {
                            string Mes = data.request.GetRequestComplete(data, tg);
                            if (Mes == "")
                                Mes = data.Base.CompletedRequestMessage(MainPlayer, tg);
                            SetDialogue(Mes, tg);
                            GetDefaultOptions(tg);
                        }
                        else
                        {
                            string Mes = data.request.GetRequestInfo(data);
                            if (Mes == "")
                            {
                                Mes = "(It gave you a list of things you need to do.)";
                            }
                            Mes += "\n---------------------";
                            foreach (string s in data.request.GetRequestText(MainPlayer, data))
                            {
                                Mes += "\n" + s;
                            }
                            SetDialogue(Mes, tg);
                        }
                    }
                    break;
            }
        }

        public static void RestButtonAction(TerraGuardian tg)
        {
            Options.Clear();
            if (Main.bloodMoon || Main.eclipse || Main.invasionType >= Terraria.ID.InvasionID.GoblinArmy) //Todo - Add dialogues for when the companion asks for how long will rest, and for when It's not possible to
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.RestNotPossible, "(Maybe It's not a good idea to rest right now.)"));
            }
            else
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.RestAskForHowLong, "(How long should we rest?)"));
                AddOption("4 Hours", delegate (TerraGuardian tg2)
                {
                    if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        string Mes = tg2.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, tg2);
                            tg2.SaySomething(Mes);
                        }
                        GuardianActions.RestCommand(tg2, 0);
                        CloseDialogueButtonAction(tg2);
                    }
                });
                AddOption("8 Hours", delegate (TerraGuardian tg2)
                {
                    string Mes = tg2.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                    if (Mes != "")
                    {
                        Mes = MessageParser(Mes, tg2);
                        tg2.SaySomething(Mes);
                    }
                    if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                    {
                        GuardianActions.RestCommand(tg2, 1);
                        CloseDialogueButtonAction(tg2);
                    }
                });
                if (!Main.dayTime)
                {
                    AddOption("Until Dawn", delegate (TerraGuardian tg2)
                    {
                        string Mes = tg2.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, tg2);
                            tg2.SaySomething(Mes);
                        }
                        if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                        {
                            GuardianActions.RestCommand(tg2, 2);
                            CloseDialogueButtonAction(tg2);
                        }
                    });
                }
                else
                {
                    AddOption("Until Night", delegate (TerraGuardian tg2)
                    {
                        string Mes = tg2.GetMessage(GuardianBase.MessageIDs.RestWhenGoingSleep);
                        if (Mes != "")
                        {
                            Mes = MessageParser(Mes, tg2);
                            tg2.SaySomething(Mes);
                        }
                        if (PlayerMod.IsInASafePlace(Main.player[Main.myPlayer]))
                        {
                            GuardianActions.RestCommand(tg2, 3);
                            CloseDialogueButtonAction(tg2);
                        }
                    });
                }
            }
            AddOption("Nevermind", delegate(TerraGuardian tg2)
            {
                GetDefaultOptions(tg2);
            });
        }

        public static void AcceptRequestButtonAction(TerraGuardian tg)
        {
            if (PlayerMod.GetPlayerAcceptedRequestCount(MainPlayer) >= RequestData.MaxRequestCount)
            {
                SetDialogue(tg.GetMessage(GuardianBase.MessageIDs.RequestCantAcceptTooManyRequests, "(You have too many requests active.)"), tg);
            }
            else
            {
                tg.request.UponAccepting();
                tg.request.UpdateRequest(tg.Data, MainPlayer.GetModPlayer<PlayerMod>());
                tg.request.Time++;
                string Mes = tg.request.GetRequestAccept(tg.Data);
                if (Mes == "")
                    Mes = "(You accepted the request.)";
                SetDialogue(Mes, tg);
            }
            GetDefaultOptions(tg);
        }

        public static void RejectRequestButtonAction(TerraGuardian tg)
        {
            tg.request.UponRejecting();
            SetDialogue(tg.request.GetRequestDeny(tg.Data), tg);
            GetDefaultOptions(tg);
        }

        public static void CloseDialogueButtonAction(TerraGuardian tg)
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            pm.IsTalkingToAGuardian = false;
            Options.Clear();
            Dialogue = new string[0];
        }

        public static void GiveGiftButtonAction(TerraGuardian Guardian)
        {
            int GiftSlot = -1, EmptyGuardianSlot = -1;
            for (int i = 0; i < 50; i++)
            {
                if (Guardian.Inventory[i].type == 0)
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
                Guardian.Inventory[EmptyGuardianSlot] = MainPlayer.inventory[GiftSlot].Clone();
                MainPlayer.inventory[GiftSlot].SetDefaults(0);
                GuardianActions.OpenBirthdayPresent(Guardian, EmptyGuardianSlot);
                SetDialogue("*You gave the gift.*");
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

        public static void PostponeRequestButtonAction(TerraGuardian Guardian)
        {
            string Mes = Guardian.GetMessage(GuardianBase.MessageIDs.RequestPostpone);
            if (Mes != "")
                Guardian.SaySomething(Mes);
            CloseDialogueButtonAction(Guardian);
        }

        public static void OpenShopButtonAction(TerraGuardian Guardian)
        {
            GuardianShopInterface.OpenShop();
        }

        public static void OpenInventoryManagementButtonAction(TerraGuardian Guardian)
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            foreach (int Key in pm.MyGuardians.Keys)
            {
                if (pm.MyGuardians[Key].ID == Guardian.ID && pm.MyGuardians[Key].ModID == Guardian.ModID)
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

        public struct DialogueOption
        {
            public string Text;
            public Action<TerraGuardian> Action;

            public DialogueOption(string OptionText, Action<TerraGuardian> Result)
            {
                Text = OptionText;
                Action = Result;
            }
        }
    }
}
