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
        public static Thread DialogueThread;
        private static TerraGuardian LastSpeaker;
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

        public static void AddParticipant(TerraGuardian tg)
        {
            List<TerraGuardian> NewParticipants = new List<TerraGuardian>();
            NewParticipants.AddRange(DialogueParticipants);
            NewParticipants.Add(tg);
            DialogueParticipants = NewParticipants.ToArray();
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
            IsDialogue = true;
            DialogueParticipants = Participants;
            GuardianMouseOverAndDialogueInterface.Speaker = LastSpeaker = Participants[0];
            ThreadStart ts = new ThreadStart(delegate () {
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
            IsDialogue = false;
            LastSpeaker = null;
            DialogueParticipants = new TerraGuardian[0];
            SelectedOption = 0;
        }

        public static void ShowDialogue(string Text, TerraGuardian Speaker = null)
        {
            if (Speaker == null)
                Speaker = LastSpeaker;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian = true;
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TalkingGuardianPosition = Speaker.WhoAmID;
            PlayerFaceGuardian(Speaker);
            ProceedButtonPressed = false;
            GuardianMouseOverAndDialogueInterface.SetDialogue(Text, Speaker);
            GuardianMouseOverAndDialogueInterface.Options.Clear();
            GuardianMouseOverAndDialogueInterface.AddOption("Continue", delegate (TerraGuardian tg)
            {
                ProceedButtonPressed = true;
                GuardianMouseOverAndDialogueInterface.Options.Clear();
            });
            while (!ProceedButtonPressed)
                Thread.Sleep(100);
        }

        public static void ShowEndDialogueMessage(string Text, bool CloseDialogue = true, TerraGuardian Speaker = null)
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
                GuardianMouseOverAndDialogueInterface.AddOption("End", EndDialogueButtonAction);
            }
            else
            {
                //GuardianMouseOverAndDialogueInterface.AddOption("Return", delegate(TerraGuardian tg)
                //{
                    GuardianMouseOverAndDialogueInterface.Options.Clear();
                    GuardianMouseOverAndDialogueInterface.GetDefaultOptions(Speaker);
                    ProceedButtonPressed = true;
                //});
            }
            while (!ProceedButtonPressed)
            {
                Thread.Sleep(100);
            }
            Thread.CurrentThread.Interrupt();
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
            for(int o = 0; o < Options.Length; o++)
            {
                string OptionText = Options[o];
                if (OptionText == null || OptionText == "")
                    continue;
                int ThisOption = o;
                GuardianMouseOverAndDialogueInterface.AddOption(OptionText, delegate(TerraGuardian tg)
                {
                    SelectedOption = ThisOption;
                    ProceedButtonPressed = true;
                });
            }
            while (!ProceedButtonPressed)
                Thread.Sleep(100);
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

        private static void EndDialogueButtonAction(TerraGuardian tg)
        {
            GuardianMouseOverAndDialogueInterface.CloseDialogueButtonAction(tg);
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
