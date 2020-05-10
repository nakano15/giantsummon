using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace giantsummon
{
    public class DialogueSystem
    {
        public static Branch MessageBranch;

        public struct OptionsMessage : Branch
        {
            public string Message;
            public string OptionOne, OptionTwo;
            public OptionsMessage(string m, string OptionOneText, string OptionTwoText)
            {
                Message = m;
                OptionOne = OptionOneText;
                OptionTwo = OptionTwoText;
            }

            public void OnStart()
            {
                Main.npcChatText = Message;
            }


            public bool ButtonAction(bool first)
            {
                return true;
            }

            public void ButtonTexts(string b1, string b2)
            {
                b1 = OptionOne;
                b2 = OptionTwo;
            }

            public void ButtonActivitiy(ref bool b1, ref bool b2)
            {
                b1 = true;
                b2 = true;
            }
        }

        public struct TextMessage : Branch
        {
            public string Message;
            public bool IsDialogueClosingMessage;
            public TextMessage(string m, bool DialogueClosing)
            {
                Message = m;
                IsDialogueClosingMessage = DialogueClosing;
            }

            public void OnStart()
            {
                Main.npcChatText = Message;
            }


            public bool ButtonAction(bool first)
            {
                return true;
            }

            public void ButtonTexts(string b1, string b2)
            {
                if (IsDialogueClosingMessage)
                    b1 = "Close";
                else
                    b1 = "Next";
            }


            public void ButtonActivitiy(ref bool b1, ref bool b2)
            {
                b1 = true;
                b2 = false;
            }
        }

        public interface Branch
        {
            void OnStart();
            bool ButtonAction(bool first);
            void ButtonTexts(string b1, string b2);
            void ButtonActivitiy(ref bool b1, ref bool b2);
        }
    }
}
