using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class Dialogue
    {
        public static bool InDialogue { get { return DialogueThread != null && DialogueThread.ThreadState == ThreadState.Running; } }
        public static TerraGuardian[] DialogueParticipants = new TerraGuardian[0];
        public static bool IsDialogue = false;
        private static bool ImportantDialogue = false;
        public static Thread DialogueThread;
        private static TerraGuardian LastSpeaker { get { return GuardianMouseOverAndDialogueInterface.Speaker; } set { GuardianMouseOverAndDialogueInterface.Speaker = value; } }
        public static bool ProceedButtonPressed = false;
        private static int SelectedOption = 0;
        public static Vector2 GatherCenter { get {
                switch (CenterType)
                {
                    case GatherCenterType.GatherAroundGuardian:
                        return GatherGuardian.Position;
                    case GatherCenterType.GatherAroundPlayer:
                        return Main.player[Main.myPlayer].Bottom;
                    case GatherCenterType.GatherAroundPosition:
                        return GatherCenterPosition;
                }
                return Vector2.Zero;
            } }
        public static TerraGuardian GatherGuardian = null;
        public static Vector2 GatherCenterPosition = Vector2.Zero;
        public static GatherCenterType CenterType = GatherCenterType.DontGatherAroundSomething;
        public static float GatherDistance = 32f;
        public static byte LeftGatherDistancing = 0, RightGatherDistancing = 0;

        public enum GatherCenterType : byte
        {
            DontGatherAroundSomething,
            GatherAroundPlayer,
            GatherAroundGuardian,
            GatherAroundPosition
        }

        public static void SetImportantDialogue()
        {
            if (DialogueThread != null && DialogueThread.IsAlive)
                ImportantDialogue = true;
        }

        public static bool IsImportantDialogue()
        {
            return ImportantDialogue;
        }

        /// <summary>
        /// Removes a specified number of an item from your inventory.
        /// </summary>
        /// <param name="ItemID">Id of the item to remove stacks.</param>
        /// <param name="Stack">How many stacks of that item you want to remove.</param>
        public static void TakeItem(int ItemID, int Stack = 1)
        {
            if(Stack < 1)
            {
                return;
            }
            Player player = Main.player[Main.myPlayer];
            for (int i = 0; i < 58; i++)
            {
                if (player.inventory[i].type == ItemID)
                {
                    int StackToTake = Stack;
                    if(player.inventory[i].stack < Stack)
                    {
                        StackToTake = player.inventory[i].stack;
                    }
                    player.inventory[i].stack -= StackToTake;
                    if (player.inventory[i].stack == 0)
                        player.inventory[i].SetDefaults(0);
                    Stack -= StackToTake;
                    if (Stack <= 0)
                        return;
                }
            }
        }

        /// <summary>
        /// Tells how many of that item you have in the inventory.
        /// </summary>
        /// <param name="ItemID">ID of the item to count from inventory.</param>
        /// <returns>Returns the number of that item you have in the inventory.</returns>
        public static int CountItem(int ItemID)
        {
            Player player = Main.player[Main.myPlayer];
            int Count = 0;
            for (int i = 0; i < 58; i++)
            {
                if (player.inventory[i].type == ItemID)
                    Count += player.inventory[i].stack;
            }
            return Count;
        }

        /// <summary>
        /// Tells if the player has that item on the inventory.
        /// It doesn't tells how many stacks of the item you have.
        /// If you want to get the number of that item, use CountItem instead.
        /// </summary>
        /// <param name="ItemID">ID of the item to check if you have.</param>
        /// <returns>Returns true if you have at least one of that item in the inventory.</returns>
        public static bool HasItem(int ItemID)
        {
            Player player = Main.player[Main.myPlayer];
            for (int i = 0; i < 58; i++)
            {
                if (player.inventory[i].type == ItemID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Creates an item at the player position.
        /// Unless you plan on spawning that item with multiple stacks, there isn't the need of setting Stack value.
        /// </summary>
        /// <param name="ID">ID of the item to spawn.</param>
        /// <param name="Stack">The number of that item to give.</param>
        public static void GiveItem(int ID, int Stack = 1)
        {
            if (Stack < 1)
                return; //Why?
            Player player = Main.player[Main.myPlayer];
            Item.NewItem(player.getRect(), ID, Stack);
        }

        /// <summary>
        /// Tells how many of that item you have in the inventory.
        /// </summary>
        /// <param name="ItemID">ID of the item to count from inventory.</param>
        /// <returns>Returns the number of that item you have in the inventory.</returns>
        public static int CountItemFromGuardian(int ItemID, int GuardianID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (!PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, ModID))
                return 0;
            GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, ModID);
            int Count = 0;
            for (int i = 0; i < 50; i++)
            {
                if (gd.Inventory[i].type == ItemID)
                    Count += gd.Inventory[i].stack;
            }
            return Count;
        }

        /// <summary>
        /// Tells if the player has that item on the inventory.
        /// It doesn't tells how many stacks of the item you have.
        /// If you want to get the number of that item, use CountItem instead.
        /// </summary>
        /// <param name="ItemID">ID of the item to check if you have.</param>
        /// <returns>Returns true if you have at least one of that item in the inventory.</returns>
        public static bool GuardianHasItem(int ItemID, int GuardianID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (!PlayerMod.PlayerHasGuardian(Main.player[Main.myPlayer], GuardianID, ModID))
                return false;
            GuardianData gd = PlayerMod.GetPlayerGuardian(Main.player[Main.myPlayer], GuardianID, ModID);
            for (int i = 0; i < 50; i++)
            {
                if (gd.Inventory[i].type == ItemID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Tries taking coins from the player inventory.
        /// Returns true if success.
        /// </summary>
        /// <param name="Value">The value in coins to take.</param>
        /// <returns>Returns true when successfull.</returns>
        public static bool TakeCoins(int Value)
        {
            return Main.player[Main.myPlayer].BuyItem(Value);
        }

        /// <summary>
        /// Give a value in coins to the player.
        /// </summary>
        /// <param name="Value">The value in coins to give to the player.</param>
        public static void GiveCoins(int Value)
        {
            Main.player[Main.myPlayer].SellItem(Value, 1);
        }

        /// <summary>
        /// Convert coin count into their value.
        /// Useful for methods that involves coin usage.
        /// </summary>
        /// <param name="Copper">The number of copper coins.</param>
        /// <param name="Silver">The number of silver coins.</param>
        /// <param name="Gold">The number of gold coins.</param>
        /// <param name="Platinum">The number of platinum coins.</param>
        /// <returns>Returns the result in value of all coin counts given.</returns>
        public static int GetCoinValues(int Copper, int Silver = 0, int Gold = 0, int Platinum = 0)
        {
            return Copper + Silver * 100 + Gold * 10000 + Platinum * 1000000;
        }

        /// <summary>
        /// Gets the value in coins on the player inventory.
        /// </summary>
        /// <returns>Returns the value of all coins in the inventory.</returns>
        public static int GetPlayerCoinValues()
        {
            Player player = Main.player[Main.myPlayer];
            int Value = 0;
            for(int i = 0; i < 58; i++)
            {
                switch (player.inventory[i].type)
                {
                    case Terraria.ID.ItemID.CopperCoin:
                        Value += player.inventory[i].value;
                        break;
                    case Terraria.ID.ItemID.SilverCoin:
                        Value += player.inventory[i].value * 100;
                        break;
                    case Terraria.ID.ItemID.GoldCoin:
                        Value += player.inventory[i].value * 10000;
                        break;
                    case Terraria.ID.ItemID.PlatinumCoin:
                        Value += player.inventory[i].value * 1000000;
                        break;
                }
            }
            return Value;
        }

        public static TerraGuardian GetSpeaker
        {
            get { return GuardianMouseOverAndDialogueInterface.Speaker; }
        }

        public static void ChangeSpeaker(int ParticipantID)
        {
            if (ParticipantID < 0 || ParticipantID >= DialogueParticipants.Length)
                return;
            LastSpeaker = DialogueParticipants[ParticipantID];
        }

        public static bool HasParticipant(int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            for(int i = 0; i < DialogueParticipants.Length; i++)
            {
                if (DialogueParticipants[i].ID == ID && DialogueParticipants[i].ModID == ModID)
                    return true;
            }
            return false;
        }

        public static TerraGuardian GetParticipant(int ParticipantID)
        {
            if (ParticipantID < 0 || ParticipantID >= DialogueParticipants.Length)
                return DialogueParticipants[0];
            return DialogueParticipants[ParticipantID];
        }

        public static void ChangeSpeaker(TerraGuardian tg)
        {
            LastSpeaker = tg;
        }

        public static int AddParticipant(TerraGuardian tg)
        {
            List<TerraGuardian> NewParticipants = new List<TerraGuardian>();
            NewParticipants.AddRange(DialogueParticipants);
            int Position = NewParticipants.Count;
            NewParticipants.Add(tg);
            DialogueParticipants = NewParticipants.ToArray();
            return Position;
        }

        public static void RemoveGatheringPoint()
        {
            CenterType = GatherCenterType.DontGatherAroundSomething;
            GatherGuardian = null;
            GatherCenterPosition = Vector2.Zero;
        }

        public static void GatherAroundPlayer()
        {
            RemoveGatheringPoint();
            CenterType = GatherCenterType.GatherAroundPlayer;
        }

        public static void GatherAroundGuardian(TerraGuardian tg)
        {
            RemoveGatheringPoint();
            CenterType = GatherCenterType.GatherAroundGuardian;
            GatherGuardian = tg;
        }

        public static void GatherAroundPosition(Vector2 Position)
        {
            RemoveGatheringPoint();
            CenterType = GatherCenterType.GatherAroundPosition;
            GatherCenterPosition = Position;
        }

        public static void StartNewDialogue(Action Dialogue, TerraGuardian[] Participants)
        {
            StartDialogue(Dialogue, Participants);
        }

        public static void StartNewDialogue(Action Dialogue, TerraGuardian Speaker)
        {
            StartDialogue(Dialogue, new TerraGuardian[] { Speaker });
        }

        private static void StartDialogue(Action Dialogue, TerraGuardian[] Participants)
        {
            if (DialogueThread != null && DialogueThread.ThreadState == ThreadState.Running)
                return;
            ImportantDialogue = false;
            IsDialogue = true;
            DialogueParticipants = Participants;
            GuardianMouseOverAndDialogueInterface.Speaker = LastSpeaker = GuardianMouseOverAndDialogueInterface.StarterSpeaker = Participants[0];
            ThreadStart ts = new ThreadStart(delegate () {
                if (Main.rand == null)
                {
                    Main.rand = new Terraria.Utilities.UnifiedRandom(); //This avoids the random variable being null when using a dialogue.
                }
                try
                {
                    Dialogue();
                }
                catch (Exception ex)
                {
                }
                EndDialogues();
            });
            DialogueThread = new Thread(ts);
            DialogueThread.Start();
        }

        private static void EndDialogues()
        {
            ImportantDialogue = false;
            IsDialogue = false;
            DialogueParticipants = new TerraGuardian[0];
            SelectedOption = 0;
        }

        /// <summary>
        /// Shows a message and resets dialogue options, but doesn't adds any option.
        /// </summary>
        /// <param name="Text">The message the speaker says.</param>
        /// <param name="Speaker">The companion who speakes this. Leaving as null picks the last speaker.</param>
        public static void ShowDialogueOnly(string Text, TerraGuardian Speaker = null)
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
        }

        /// <summary>
        /// Adds an option to the dialogue.
        /// </summary>
        /// <param name="Text">The text of the option, displayed on the list.</param>
        /// <param name="OptionAction">The action of the option. The TerraGuardian attribute in the action is the speaker.</param>
        public static void AddOption(string Text, Action OptionAction, bool Threaded = false)
        {
            GuardianMouseOverAndDialogueInterface.AddOption(Text, OptionAction, Threaded);
        }

        /// <summary>
        /// Resets the option list. ShowDialogueOnly already does that naturally.
        /// </summary>
        public static void ResetOptions()
        {
            GuardianMouseOverAndDialogueInterface.Options.Clear();
        }

        /// <summary>
        /// Shows a message dialogue, with only Continue as option.
        /// This dialogue will wait until the player press Continue before continuing the dialogue script.
        /// </summary>
        /// <param name="Text">The message the speaker says.</param>
        /// <param name="Speaker">The companion who speakes this. Leaving as null picks the last speaker.</param>
        public static void ShowDialogueWithContinue(string Text, TerraGuardian Speaker = null, string ContinueText = "Continue")
        {
            ShowDialogueWithOptions(Text, new string[] { ContinueText }, Speaker);
            /*if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            GuardianMouseOverAndDialogueInterface.AddOption(ContinueText, delegate ()
            {
                ProceedButtonPressed = true;
                GuardianMouseOverAndDialogueInterface.Options.Clear();
            });
            while (!ProceedButtonPressed)
                Thread.Sleep(100);*/
        }

        /// <summary>
        /// Shows a message dialogue, with only End as option.
        /// </summary>
        /// <param name="Text">The message the speaker says.</param>
        /// <param name="CloseDialogue">Set to true, will close the dialogue with this companion. Setting to false, returns to the default dialogue and options of the companion.</param>
        /// <param name="Speaker">The companion who speakes this. Leaving as null picks the last speaker.</param>
        public static void ShowEndDialogueMessage(string Text, bool CloseDialogue = true, TerraGuardian Speaker = null, string CloseOptionText = "End")
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            if (CloseDialogue)
            {
                GuardianMouseOverAndDialogueInterface.AddOption(CloseOptionText, EndDialogueButtonAction);
            }
            else
            {
                //GuardianMouseOverAndDialogueInterface.AddOption("Return", delegate(TerraGuardian tg)
                //{
                GuardianMouseOverAndDialogueInterface.Options.Clear();
                GuardianMouseOverAndDialogueInterface.GetDefaultOptions();
                ProceedButtonPressed = true;
                //});
            }
            if (Thread.CurrentThread == DialogueThread)
            {
                while (!ProceedButtonPressed)
                {
                    Thread.Sleep(100);
                }
                ProceedButtonPressed = false;
                DialogueThread.Interrupt();
            }
        }

        public static void ShowDialogueWithOptions(string Text, DialogueOption[] Options, TerraGuardian Speaker = null)
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            int Selected = 0;
            for (int o = 0; o < Options.Length; o++)
            {
                int ThisOption = o;
                GuardianMouseOverAndDialogueInterface.AddOption(Options[o].Text, delegate ()
                {
                    Selected = ThisOption;
                    ProceedButtonPressed = true;
                });
            }
            if (Thread.CurrentThread == DialogueThread)
            {
                while (!ProceedButtonPressed)
                {
                    Thread.Sleep(100);
                }
                Options[Selected].Action();
            }
        }

        public static int ShowDialogueWithOptions(string Text, string[] Options, TerraGuardian Speaker = null)
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            int SelectedOption = 0;
            for(int o = 0; o < Options.Length; o++)
            {
                string OptionText = Options[o];
                if (OptionText == null || OptionText == "")
                    continue;
                int ThisOption = o;
                GuardianMouseOverAndDialogueInterface.AddOption(OptionText, delegate()
                {
                    SelectedOption = ThisOption;
                    ProceedButtonPressed = true;
                }, Thread.CurrentThread == DialogueThread);
            }
            while (!ProceedButtonPressed)
            {
                Thread.Sleep(100);
            }
            return SelectedOption;
        }

        public static void ShowDialogueTimed(string Text, TerraGuardian Speaker = null, int Time = 0)
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            if (Time == 0)
                Time = 300 + Text.Length;
            Time = (int)(Time / 60) + 1;
            while (Time-- > 0)
            {
                Thread.Sleep(1000);
            }
        }

        public static void CloseDialogue()
        {
            GuardianMouseOverAndDialogueInterface.CloseDialogue(LastSpeaker);
        }

        public static void CompanionSaySomething(TerraGuardian Speaker, string Message, bool DisplayOnChat = false, int ExtraDialogueTime = 0)
        {
            LastSpeaker = Speaker;
            Speaker.SaySomething(Message, DisplayOnChat);
            Speaker.MessageTime += ExtraDialogueTime;
            while (Speaker.MessageTime > 0)
                Thread.Sleep(100);
        }

        public static void PlayerFaceGuardian(TerraGuardian Target)
        {
            if (Main.player[Main.myPlayer].Center.X < Target.Position.X)
                Main.player[Main.myPlayer].direction = 1;
            else
                Main.player[Main.myPlayer].direction = -1;
        }

        private static void EndDialogueButtonAction()
        {
            GuardianMouseOverAndDialogueInterface.CloseDialogueButtonAction();
            ProceedButtonPressed = true;
        }

        public static bool UpdateDialogueParticipationGuardian(TerraGuardian guardian)
        {
            if (CenterType == GatherCenterType.DontGatherAroundSomething || !DialogueParticipants.Contains(guardian) || guardian.AttackingTarget)
                return false;
            Vector2 Position = GatherCenter;
            bool PositionToTheLeft = guardian.Position.X >= Position.X;
            if (PositionToTheLeft)
            {
                Position.X += GatherDistance + RightGatherDistancing++ * 12f;
            }
            else
            {
                Position.X -= GatherDistance + LeftGatherDistancing++ * 12f;
            }
            float PositionDiference = guardian.Position.X - Position.X;
            if (Math.Abs(PositionDiference) > 8)
            {
                if (Math.Abs(PositionDiference) < 24)
                    guardian.WalkMode = true;
                if(PositionDiference > 0)
                {
                    guardian.MoveLeft = true;
                }
                else
                {
                    guardian.MoveRight = true;
                }
            }
            return true;
        }
    }
}
