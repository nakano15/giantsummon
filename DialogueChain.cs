using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class DialogueChain
    {
        public int Points = 0;
        public int DialogueChainStep = 0;
        private List<Dialogue> Dialogues = new List<Dialogue>();
        public bool LastAnswerWasLeft = false;
        public bool MixedAnswer = false;
        public bool Finished { get { return DialogueChainStep >= Dialogues.Count; } }
        public Dialogue CurrentDialogue { get { return Dialogues[DialogueChainStep]; } }

        public void AddDialogue(string mes, string ans1, string ans2, bool leftisanswer)
        {
            Dialogue dialogue = new Dialogue(mes, ans1, ans2, leftisanswer);
            Dialogues.Add(dialogue);
        }

        public void AddDialogue(string mes, string mes2, string ans1, string ans2, bool leftisanswer)
        {
            Dialogue dialogue = new Dialogue(mes, mes2, ans1, ans2, leftisanswer);
            Dialogues.Add(dialogue);
        }

        public void MarkAnswer(bool Left)
        {
            if (!Finished)
            {
                if (MixedAnswer)
                    Left = !Left;
                if (CurrentDialogue.LeftIsCorrectAnswer == Left)
                {
                    Points++;
                }
                LastAnswerWasLeft = Left;
                DialogueChainStep++;
                MixedAnswer = Terraria.Main.rand.NextDouble() < 0.5;
            }
        }

        public struct Dialogue
        {
            public string Message, MessageAnswer2;
            public string AnswerOne, AnswerTwo;
            public bool LeftIsCorrectAnswer;

            public Dialogue(string mes, string ans1, string ans2, bool leftisanswer)
            {
                Message = mes;
                MessageAnswer2 = null;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = leftisanswer;
            }

            public Dialogue(string mes, string mes2, string ans1, string ans2, bool leftisanswer)
            {
                Message = mes;
                MessageAnswer2 = mes2;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = leftisanswer;
            }
        }
    }
}
