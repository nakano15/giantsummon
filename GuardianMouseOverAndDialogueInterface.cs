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
        public static bool SomeoneMouseOver = false;
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

        public static void SetDialogue(string Message) //And with auto parser :D
        {
            SetDialogue(Message, null);
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

        public static void Update()
        {
            SomeoneMouseOver = false;
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
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            if (player.IsTalkingToAGuardian)
            {
                if (player.player.sign > -1 || player.player.talkNPC > -1 || !Keys.Contains(player.TalkingGuardianPosition))
                {
                    player.IsTalkingToAGuardian = false;
                    return;
                }
            }
            else
            {
                if (SomeoneMouseOver)
                {
                    if (!player.IsTalkingToAGuardian || player.TalkingGuardianPosition != MouseOverGuardian)
                    {
                        if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                            if (!tg.IsAttackingSomething && !tg.Downed && !tg.KnockedOut && IsInChattingRange(tg))
                            {
                                Main.CancelHairWindow();
                                Main.npcShop = 0;
                                Main.InGuideCraftMenu = false;
                                Main.player[Main.myPlayer].dropItemCheck();
                                Main.npcChatCornerItem = 0;
                                Main.player[Main.myPlayer].sign = -1;
                                Main.editSign = false;
                                Main.player[Main.myPlayer].talkNPC = -1;
                                Main.playerInventory = false;
                                Main.player[Main.myPlayer].chest = -1;
                                Recipe.FindRecipes();
                                Main.PlaySound(24, -1, -1, 1, 1f, 0f);
                                player.IsTalkingToAGuardian = true;
                                player.TalkingGuardianPosition = MouseOverGuardian;
                                //GetCompanionDialogue
                                SetDialogue(tg.Base.TalkMessage(player.player, tg), tg);
                                GetDefaultOptions(tg);
                            }
                        }
                    }
                }
            }
            if (player.IsTalkingToAGuardian)
            {
                TerraGuardian tg = MainMod.ActiveGuardians[player.TalkingGuardianPosition];
                if (!IsInChattingRange(tg) || MainPlayer.chest > -1 || tg.Downed || tg.KnockedOut)
                {
                    player.IsTalkingToAGuardian = false;
                }
                else
                {
                    MouseOverOptionNumber = -1;
                    bool DoAction = false;
                    for (int o = 0; o < Options.Count; o++)
                    {
                        Vector2 Position = new Vector2(DialogueStartX + 8, DialogueStartY + DialogueHeight + 48 + o * 30 + 8);
                        if (Main.mouseX >= Position.X && Main.mouseX < Position.X + DialogueWidth && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 30)
                        {
                            MouseOverOptionNumber = o;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                DoAction = true;
                        }
                    }
                    if (DoAction && MouseOverOptionNumber > -1)
                    {
                        Options[MouseOverOptionNumber].Action(tg);
                    }
                }
            }
        }

        public static bool IsInChattingRange(TerraGuardian tg)
        {
            Rectangle rect = new Rectangle((int)(MainPlayer.position.X + MainPlayer.width * 0.5f - Player.tileRangeX * 16),
                (int)(MainPlayer.position.Y + MainPlayer.height * 0.5f - Player.tileRangeY * 16),
                Player.tileRangeX * 32,
                Player.tileRangeY * 32);
            return tg.HitBox.Intersects(rect);
        }

        public static void DrawMouseOver()
        {
            if (SomeoneMouseOver && MainMod.ActiveGuardians.ContainsKey(MouseOverGuardian))
            {
                TerraGuardian tg = MainMod.ActiveGuardians[MouseOverGuardian];
                if(IsInChattingRange(tg))
                    Main.spriteBatch.Draw(Main.chatTexture, new Vector2(tg.Position.X - Main.chatTexture.Width * 0.5f - (-tg.Width * 0.5f - 8) * tg.Direction, tg.Position.Y - tg.Height - Main.chatTexture.Height) - Main.screenPosition, null, Main.mouseTextColorReal, 0f, Vector2.Zero, 1f, (tg.LookingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
                Vector2 TextPosition = tg.Position;
                TextPosition.Y -= tg.Base.Height - 22;
                string Text = tg.ReferenceName + " " + tg.HP + "/" + tg.MHP;
                Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition - Main.screenPosition, Color.White * Main.cursorAlpha, 1f, 0.5f, 0.5f);
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
            if (!MainPlayer.GetModPlayer<PlayerMod>().IsTalkingToAGuardian)
                return;
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
                WindowStartPosition.X -= 48;
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

        public static void GetDefaultOptions(TerraGuardian tg)
        {
            Options.Clear();
            string OptionText = "Check Request";
            if (tg.request.RequestCompleted || tg.request.Failed)
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
            Options.AddRange(tg.Base.GetGuardianExtraDialogueActions(tg));
            AddOption("Goodbye", CloseDialogueButtonAction);
        }

        public static void AddOption(string Mes, Action<TerraGuardian> Action)
        {
            DialogueOption option = new DialogueOption(Mes, Action);
            Options.Add(option);
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

        public static void AcceptRequestButtonAction(TerraGuardian tg)
        {
            if (PlayerMod.GetPlayerAcceptedRequestCount(MainPlayer) >= RequestData.MaxRequestCount)
            {
                SetDialogue("(You have too many requests active.)", tg);
            }
            else
            {
                tg.request.UponAccepting();
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
        }

        public static void CloseDialogueButtonAction(TerraGuardian tg)
        {
            PlayerMod pm = MainPlayer.GetModPlayer<PlayerMod>();
            pm.IsTalkingToAGuardian = false;
            Options.Clear();
            Dialogue = new string[0];
        }

        public static string MessageParser(string Message, TerraGuardian guardian)
        {
            Message = Message.Replace("[name]", guardian.Name);
            //Message += "[gn:1:""]";
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
