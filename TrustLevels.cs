using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace giantsummon
{
    public class TrustLevels
    {
        public const sbyte MoveInTrust = 0, RequestGivingTrust = -5, FollowTrust = -10, ControlTrust = 60;
        public const sbyte TrustPointsPerFriendshipExp = 2, TrustPointsPerComfortStack = 1, ReviveHelpTrustGain = 5, TrustGainFromComplettingRequest = 1;
        public const sbyte TrustLossWhenEatenByWof = -10, TrustLossWhenCancellingRequest = -3, TrustLossOnFailRequest = -3;

        public const sbyte MoveOutOfWorldTrustLevel = -60,
            StopFollowingTrustLevel = -40;

        public static int GetTrustLevel(sbyte Trust)
        {
            if (Trust == 100)
                return (int)TrustLevelEnum.Radiant;
            int TrustValue = (int)((Trust + 100) * 0.025f);
            if (TrustValue < 0)
                TrustValue = 0;
            return TrustValue;
        }

        public static string GetTrustInfo(sbyte TrustLevel)
        {
            string Info = "Trust: " + TrustLevel + "\n" + Enum.GetName(typeof(TrustLevelEnum), GetTrustLevel(TrustLevel));
            if (TrustLevel >= MoveInTrust)
            {
                Info += "\n - Will move in to your world.";
            }
            else
            {
                Info += "\n - Refuses to move in to your world.";
            }
            if (TrustLevel >= FollowTrust)
            {
                Info += "\n - Will follow you on your quest.";
            }
            else
            {
                Info += "\n - Will not follow you.";
            }
            if (TrustLevel >= FollowTrust)
            {
                Info += "\n - Will follow you on your quest.";
            }
            else
            {
                Info += "\n - Will not follow you.";
            }
            if (TrustLevel >= RequestGivingTrust)
            {
                Info += "\n - Will give you requests.";
            }
            else
            {
                Info += "\n - Doesn't trusts you to give requests.";
            }
            if (TrustLevel >= ControlTrust)
            {
                Info += "\n - Will let you control them.";
            }
            else
            {
                Info += "\n - Doesn't trust their control to you.";
            }
            return Info;
        }

        public enum TrustLevelEnum : byte
        {
            Furious,
            Angry,
            Neutral,
            Happy,
            VeryHappy,
            Radiant
        }
    }
}
