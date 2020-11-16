using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class GuardianMoodBase
    {
        private static Dictionary<int, GuardianMoodBase> MoodTypeBaseList = new Dictionary<int, GuardianMoodBase>();
        public string MoodName = "", Description = "";
        public sbyte HorizontalMod = 0, VerticalMod = 0;

        public GuardianMoodBase(string Name, string Description, sbyte SadnessToHappinessValue, sbyte AngerToSadnessValue)
        {
            MoodName = Name;
            this.Description = Description;
            HorizontalMod = SadnessToHappinessValue;
            VerticalMod = AngerToSadnessValue;
        }

        public static GuardianMoodBase GetMoodBase(int ID)
        {
            if (!MoodTypeBaseList.ContainsKey(ID))
            {
                GuardianMoodBase gmb = MoodBaseDB(ID);
                MoodTypeBaseList.Add(ID, gmb);
                return gmb;
            }
            return MoodTypeBaseList[ID];
        }

        private static GuardianMoodBase MoodBaseDB(int ID)
        {
            GuardianMoodBase mood;
            switch (ID)
            {
                default:
                    mood = new GuardianMoodBase("Unknown", "It's unknown the effects of this.", 0, 0);
                    break;
            }
            return mood;
        }
    }
}
