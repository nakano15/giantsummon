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
        public const int FullfilledMood = 0,
            DisappointedMood = 1,
            FrustratedMood = 2;

        public GuardianMoodBase(string Name, string Description, sbyte SadnessToHappinessValue, sbyte AngerToFearValue)
        {
            MoodName = Name;
            this.Description = Description;
            HorizontalMod = SadnessToHappinessValue;
            VerticalMod = AngerToFearValue;
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
                case 0:
                    mood = new GuardianMoodBase("Fullfilled", "You helped this person, and It's feeling happy.", 20, 0);
                    break;
                case 1:
                    mood = new GuardianMoodBase("Disappointed", "You have failed to complete this person request.", -10, -20);
                    break;
                case 2:
                    mood = new GuardianMoodBase("Frustrated", "You failed to complete this person request in time.", -20, -10);
                    break;
            }
            return mood;
        }
    }
}
