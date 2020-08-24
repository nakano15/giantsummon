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

        public static void StartDialogue(TerraGuardian tg)
        {
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
            //GetCompanionDialogue
            GetDefaultOptions(tg);
            string Message = "";
            if (!modPlayer.HasGuardian(tg.ID, tg.ModID))
            {
                modPlayer.AddNewGuardian(tg.ID, tg.ModID);
                if (tg.ID == WorldMod.SpawnGuardian.Key && tg.ModID == WorldMod.SpawnGuardian.Value)
                    modPlayer.GetGuardian(tg.ID, tg.ModID).IsStarter = true;
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
                    else if (tg.OwnerPos == -1 && tg.GetTownNpcInfo != null && tg.GetTownNpcInfo.Homeless)
                        Message = tg.Base.HomelessMessage(MainPlayer, tg);
                    else
                        Message = tg.Base.NormalMessage(MainPlayer, tg);
                }
            }
            if (!NpcMod.HasMetGuardian(tg.ID, tg.ModID))
            {
                Message = tg.Base.GreetMessage(MainPlayer, tg);
                NpcMod.AddGuardianMet(tg.ID, tg.ModID);
                if (WorldMod.HasEmptyGuardianNPCSlot())
                    WorldMod.AllowGuardianNPCToSpawn(tg.ID, tg.ModID);
                if (modPlayer.HasGuardian(tg.ID, tg.ModID))
                    modPlayer.GetGuardian(tg.ID, tg.ModID).IncreaseFriendshipProgress(1);
            }
            if (tg.request.requestState == RequestData.RequestState.RequestActive && tg.request.GetRequestBase(tg.Data).Objectives.Any(x => x.objectiveType == RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian))
            {
                RequestBase rb = tg.request.GetRequestBase(tg.Data);
                for (int obj = 0; obj < rb.Objectives.Count; obj++)
                {
                    if (rb.Objectives[obj].objectiveType == RequestBase.RequestObjective.ObjectiveTypes.TalkToGuardian && tg.request.GetIntegerValue(obj) == 0)
                    {
                        RequestBase.TalkToGuardianRequestObjective to = (RequestBase.TalkToGuardianRequestObjective)rb.Objectives[obj];
                        if (to.GuardianID == tg.ID && to.ModID == tg.ModID)
                        {
                            Message = to.MessageText;
                            tg.request.SetIntegerValue(obj, 1);
                        }
                    }
                }
            }
            SetDialogue(Message, tg);
        }

        public static void Update()
        {
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
                if (!IsInChattingRange(tg) || Main.playerInventory || (MainPlayer.talkNPC > -1 && Main.npc[MainPlayer.talkNPC].active) || MainPlayer.sign > -1 || MainPlayer.chest > -1 || tg.Downed || tg.KnockedOut)
                {
                    player.IsTalkingToAGuardian = false;
                    if (Main.playerInventory)
                        Main.playerInventory = false;
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
            if (MayGiveBirthdayGift(tg))
            {
                AddOption("I have a gift for you", GiveGiftButtonAction);
            }
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
