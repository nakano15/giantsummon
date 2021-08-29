using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class FeatMentioning
    {
        public GuardianID[] GuardiansWhoMentionThis = new GuardianID[0];
        public string PlayerName = "";
        public string FeatSubject = ""; //Holds the subject of the feat, like the name of an item you acquired, or a boss you killed.
        public float FeatDurationInGameDays = 0;
        public float Importance = 0;
        public FeatType type = FeatType.MentionPlayer;

        public void AddGuardianWhoMentionsThis(GuardianID guardian)
        {
            GuardianID[] NewMentioners = new GuardianID[GuardiansWhoMentionThis.Length + 1];
            for(int i = 0; i < GuardiansWhoMentionThis.Length; i++)
            {
                NewMentioners[i] = GuardiansWhoMentionThis[i];
            }
            NewMentioners[GuardiansWhoMentionThis.Length] = guardian;
            GuardiansWhoMentionThis = NewMentioners;
        }

        public bool GuardianMentionsThis(GuardianID guardian)
        {
            foreach(GuardianID g in GuardiansWhoMentionThis)
            {
                if (g.IsSameID(guardian))
                    return true;
            }
            return false;
        }

        public enum FeatType
        {
            MentionPlayer,
            BossDefeated,
            FoundSomethingGood,
            EventFinished,
            MetSomeoneNew,
            PlayerDied,
            OpenedTemple,
            CoinPortalSpawned
        }
    }
}
