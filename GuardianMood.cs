using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianMood
    {
        public static byte MoodUpdateDelay = 0;
        public sbyte HorizontalBar = 0, VerticalBar = 0;
        public byte GetHappinessValue { get { return (byte)HorizontalBar; } }
        public byte GetSadnessValue { get { return (byte)-HorizontalBar; } }
        public byte GetFearValue { get { return (byte)VerticalBar; } }
        public byte GetAngerValue { get { return (byte)-VerticalBar; } }
        public List<MoodMod> MoodList = new List<MoodMod>();

        public void UpdateMood()
        {
            if (MoodUpdateDelay == 0)
            {
                HorizontalBar = VerticalBar = 0;
                int HorizontalStack = 0, HorizontalSummedValue = 0,
                    VerticalStack = 0, VerticalSummedValue = 0;
                foreach (MoodMod mood in MoodList)
                {
                    if (mood.Duration <= 0)
                    {
                        MoodList.Remove(mood);
                    }
                    else
                    {
                        HorizontalStack += mood.HorizontalChange;
                        HorizontalSummedValue += Math.Abs(mood.HorizontalChange);
                        VerticalStack += mood.VerticalChange;
                        VerticalSummedValue += Math.Abs(mood.VerticalChange);
                    }
                }
                float HorizontalPercentage = 0, VerticalPercentage = 0;
                if (HorizontalSummedValue < 100)
                {
                    HorizontalPercentage = (float)HorizontalStack / 100;
                }
                else
                {
                    HorizontalPercentage = (float)HorizontalStack / HorizontalSummedValue;
                }
                if (VerticalSummedValue < 100)
                {
                    VerticalPercentage = (float)VerticalStack / 100;
                }
                else
                {
                    VerticalPercentage = (float)VerticalStack / VerticalSummedValue;
                }
                HorizontalBar = (sbyte)(HorizontalPercentage * 100);
                VerticalBar = (sbyte)(VerticalPercentage * 100);
            }
            for (int m = 0; m < MoodList.Count; m++)
            {
                if (MoodList[m].Duration > 0)
                    MoodList[m].Duration--;
            }
        }

        public class MoodMod
        {
            public GuardianMoodBase MoodBase;
            public int Duration = 0;
            public sbyte HorizontalChange { get { return MoodBase.HorizontalMod; } }
            public sbyte VerticalChange { get { return MoodBase.VerticalMod; } }

            public MoodMod(GuardianMoodBase Mood, int Duration)
            {
                MoodBase = Mood;
                this.Duration = Duration;
            }
        }
    }
}
