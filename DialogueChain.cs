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

        public void AddDialogue(string mes, string ans1, string ans2)
        {
            Dialogue dialogue = new Dialogue(mes, ans1, ans2);
            Dialogues.Add(dialogue);
        }

        public void AddDialogue(string mes, string mes2, string ans1, string ans2, bool leftisanswer)
        {
            Dialogue dialogue = new Dialogue(mes, mes2, ans1, ans2, leftisanswer);
            Dialogues.Add(dialogue);
        }

        public void AddDialogue(string mes, string mes2, string ans1, string ans2)
        {
            Dialogue dialogue = new Dialogue(mes, mes2, ans1, ans2);
            Dialogues.Add(dialogue);
        }

        public void MarkAnswer(bool Left)
        {
            if (!Finished)
            {
                if (MixedAnswer)
                    Left = !Left;
                if (CurrentDialogue.SelectionMatter && CurrentDialogue.LeftIsCorrectAnswer == Left)
                {
                    Points++;
                }
                LastAnswerWasLeft = Left;
                DialogueChainStep++;
                MixedAnswer = Terraria.Main.rand.NextDouble() < 0.5;
            }
        }

        public string GetQuestion()
        {
            if (!LastAnswerWasLeft && CurrentDialogue.MessageAnswer2 != null)
                return CurrentDialogue.MessageAnswer2;
            return CurrentDialogue.Message;
        }

        public void GetAnswers(out string Answer1, out string Answer2)
        {
            if (!MixedAnswer)
            {
                Answer1 = CurrentDialogue.AnswerOne;
                Answer2 = CurrentDialogue.AnswerTwo;
            }
            else
            {
                Answer1 = CurrentDialogue.AnswerTwo;
                Answer2 = CurrentDialogue.AnswerOne;
            }
        }

        public struct Dialogue
        {
            public string Message, MessageAnswer2;
            public string AnswerOne, AnswerTwo;
            public bool LeftIsCorrectAnswer;
            public bool SelectionMatter;

            public Dialogue(string mes, string ans1, string ans2, bool leftisanswer)
            {
                Message = mes;
                MessageAnswer2 = null;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = leftisanswer;
                SelectionMatter = true;
            }

            public Dialogue(string mes, string mes2, string ans1, string ans2, bool leftisanswer)
            {
                Message = mes;
                MessageAnswer2 = mes2;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = leftisanswer;
                SelectionMatter = true;
            }

            public Dialogue(string mes, string ans1, string ans2)
            {
                Message = mes;
                MessageAnswer2 = null;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = false;
                SelectionMatter = false;
            }

            public Dialogue(string mes, string mes2, string ans1, string ans2)
            {
                Message = mes;
                MessageAnswer2 = mes2;
                AnswerOne = ans1;
                AnswerTwo = ans2;
                LeftIsCorrectAnswer = false;
                SelectionMatter = false;
            }
        }
    }
}
