using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Terraria;

namespace giantsummon
{
    public class GuardianGlobalInfos
    {
        public static string GetSaveFolder { get { return Main.SavePath + "/TerraGuardians"; } }

        //Feats are saved differently.
        public static List<FeatMentioning> Feats = new List<FeatMentioning>();
        public static TimeSpan LifeTime = new TimeSpan();
        public const float HourToDecimal = 1f / 24;
        public const int DaysInAYear = 128,
            QuarterOfAYear = DaysInAYear / 4; //32
        private static int LoggedDay = 0;
        private static Season LoggedSeason = Season.Summer;
        private static DayOfWeek LoggedWeekday = DayOfWeek.Sunday;
        private static bool GlobalInfosLoaded = false, FeatsLoaded = false;

        public static Season GetSeason
        {
            get
            {
                return LoggedSeason;
            }
        }

        public static int GetDay
        {
            get
            {
                return LoggedDay;
            }
        }

        public static DayOfWeek GetDayOfWeek
        {
            get
            {
                return LoggedWeekday;
            }
        }

        public static TimeSpan GetTime
        {
            get
            {
                return LifeTime;
                /*double TimeToDiscount = Main.time;
                if (Main.dayTime)
                    TimeToDiscount += 4.5 * 3600;
                else
                {
                    if (Main.time >= 4.5f * 3600)
                    {
                        TimeToDiscount -= (4.5f) * 3600;
                    }
                    else
                    {
                        TimeToDiscount += (12 + 7.5) * 3600;
                    }
                }
                return LifeTime - TimeSpan.FromSeconds(24 * 3600 - TimeToDiscount);*/
            }
        }

        public static void UpdateSeason()
        {
            GetDayAndSeason();
        }

        public static void UpdateGlobalInfos()
        {
            int Day = LifeTime.Days;
            LifeTime += TimeSpan.FromSeconds(Main.dayRate);
            if (Day != LifeTime.Days)
                UpdateSeason();
        }

        public static void GetDayAndSeason()
        {
            TimeSpan NewTime = GetTime;
            int Year = NewTime.Days % DaysInAYear;
            LoggedDay = Year % QuarterOfAYear;
            LoggedSeason = (Season)(Year / QuarterOfAYear);
            LoggedWeekday = (DayOfWeek)(NewTime.Days % 7);
        }

        public static void SaveGlobalInfos()
        {
            if (!GlobalInfosLoaded)
            {
                LoadGlobalInfos();
                GlobalInfosLoaded = true;
            }
            string SaveFolder = GetSaveFolder;
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);
            string GlobalInfosFile = SaveFolder + "/globalinfos.sav";
            if (File.Exists(GlobalInfosFile))
                File.Delete(GlobalInfosFile);
            using(FileStream stream = new FileStream(GlobalInfosFile, FileMode.Create))
            {
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MainMod.ModVersion);
                    writer.Write(LifeTime.TotalSeconds);
                }
            }
        }

        public static void LoadGlobalInfos()
        {
            GlobalInfosLoaded = true;
            string SaveFolder = GetSaveFolder;
            if (!Directory.Exists(SaveFolder))
                return;
            string GlobalInfosFile = SaveFolder + "/globalinfos.sav";
            if (!File.Exists(GlobalInfosFile))
                return;
            using (FileStream stream = new FileStream(GlobalInfosFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int Version = reader.ReadInt32();
                    LifeTime = TimeSpan.FromSeconds(reader.ReadDouble());
                }
            }
        }

        public static void UpdateFeatTime()
        {
            for(int f = 0; f < Feats.Count; f++)
            {
                Feats[f].FeatDurationInGameDays -= 1;
                if (Feats[f].FeatDurationInGameDays <= 0)
                    Feats.RemoveAt(f);
            }
        }

        public static float TranslateTimeToDays(int Days = 0, int Hours = 0)
        {
            return Hours * HourToDecimal + Days;
        }

        public static GuardianID[] GetGuardiansInTheWorld(GuardianID IgnoredID = null)
        {
            List<GuardianID> guardians = new List<GuardianID>();
            foreach (int guardian in MainMod.ActiveGuardians.Keys)
            {
                if(IgnoredID == null || !MainMod.ActiveGuardians[guardian].MyID.IsSameID(IgnoredID))
                {
                    guardians.Add(MainMod.ActiveGuardians[guardian].MyID);
                }
            }
            return guardians.ToArray();
        }

        public static FeatMentioning GetAFeatToMention(GuardianID guardian, string SpeakerPlayer)
        {
            List<FeatMentioning> feats = new List<FeatMentioning>();
            foreach(FeatMentioning feat in Feats)
            {
                if(feat.GuardiansWhoMentionThis.Length == 0 || feat.GuardianMentionsThis(guardian))
                {
                    if(feat.PlayerName != SpeakerPlayer)
                    {
                        feats.Add(feat);
                    }
                }
            }
            if (feats.Count == 0)
                return null;
            return feats[Main.rand.Next(feats.Count)];
        }

        public static void EraseFeatsFromPlayer(string PlayerName)
        {
            for(int f = 0; f < Feats.Count; f++)
            {
                if (Feats[f].PlayerName == PlayerName)
                    Feats.RemoveAt(f);
            }
        }

        public static string GetFeatMessage(FeatMentioning feat, TerraGuardian tg)
        {
            string Message = "";
            string Subject = feat.FeatSubject;
            switch (feat.type)
            {
                case FeatMentioning.FeatType.BossDefeated:
                case FeatMentioning.FeatType.MinibossDefeated:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatMentionBossDefeat);
                    break;
                case FeatMentioning.FeatType.CoinPortalSpawned:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatCoinPortal);
                    break;
                case FeatMentioning.FeatType.EventFinished:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatEventFinished);
                    break;
                case FeatMentioning.FeatType.FoundSomethingGood:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatFoundSomethingGood);
                    break;
                case FeatMentioning.FeatType.MentionPlayer:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatMentionPlayer);
                    break;
                case FeatMentioning.FeatType.MetSomeoneNew:
                    {
                        if (!(tg.Base.Name == feat.FeatSubject || tg.Base.PossibleNames.Contains(feat.FeatSubject)))
                        {
                            Message = tg.GetMessage(GuardianBase.MessageIDs.FeatMetSomeoneNew);
                        }
                        else
                        {
                            Message = tg.GetMessage(GuardianBase.MessageIDs.FeatPlayerMetMe);
                        }
                    }
                    break;
                case FeatMentioning.FeatType.OpenedTemple:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatOpenTemple);
                    break;
                case FeatMentioning.FeatType.PlayerDied:
                    Message = tg.GetMessage(GuardianBase.MessageIDs.FeatPlayerDied);
                    break;
                case FeatMentioning.FeatType.SomeonePickedABuddy:
                    {
                        if (!(tg.Base.Name == feat.FeatSubject || tg.Base.PossibleNames.Contains(feat.FeatSubject)))
                        {
                            Message = tg.GetMessage(GuardianBase.MessageIDs.FeatMentionSomeonePickedAsBuddy);
                        }
                        else
                        {
                            Message = tg.GetMessage(GuardianBase.MessageIDs.FeatSpeakerPlayerPickedMeAsBuddy);
                        }
                    }
                    break;
            }
            Message = Message.Replace("[player]", feat.PlayerName).Replace("[subject]", Subject);
            return Message;
        }

        public static bool HasFeatToMention(GuardianID id)
        {
            foreach(FeatMentioning feat in Feats)
            {
                if (feat.GuardianMentionsThis(id))
                    return true;
            }
            return false;
        }

        public static void AddFeat(FeatMentioning.FeatType feat, string PlayerName, string SubjectName = "", float FeatDurationDays = 8, float ImportanceLevel = 0, GuardianID[] GuardiansWhoMentionThis = null)
        {
            if (GuardiansWhoMentionThis == null)
                GuardiansWhoMentionThis = new GuardianID[0];
            uint Duration = (uint)(FeatDurationDays * (60 * 60 * 24));
            foreach(FeatMentioning f in Feats)
            {
                if(f.type == feat && f.PlayerName == PlayerName)
                {
                    bool Skip = false;
                    foreach(GuardianID g in GuardiansWhoMentionThis)
                    {
                        if (!f.GuardianMentionsThis(g))
                        {
                            if (feat == FeatMentioning.FeatType.MentionPlayer)
                            {
                                Skip = true;
                                break;
                            }
                            f.AddGuardianWhoMentionsThis(g);
                        }
                    }
                    if (Skip)
                        continue;
                    if(f.Importance <= ImportanceLevel)
                    {
                        f.FeatSubject = SubjectName;
                        f.Importance = ImportanceLevel;
                    }
                    if (f.FeatSubject == SubjectName && f.FeatDurationInGameDays < Duration)
                        f.FeatDurationInGameDays = Duration;
                    return;
                }
            }
            FeatMentioning newFeat = new FeatMentioning();
            newFeat.type = feat;
            newFeat.PlayerName = PlayerName;
            newFeat.FeatSubject = SubjectName;
            newFeat.FeatDurationInGameDays = Duration;
            newFeat.Importance = ImportanceLevel;
            newFeat.GuardiansWhoMentionThis = GuardiansWhoMentionThis;
            Feats.Add(newFeat);
        }

        public static void SaveFeats()
        {
            if (!FeatsLoaded)
            {
                FeatsLoaded = true;
                LoadFeats();
            }
            string SaveFolder = GetSaveFolder;
            if (!Directory.Exists(SaveFolder))
                Directory.CreateDirectory(SaveFolder);
            string FeatsFile = SaveFolder + "/feats.sav";
            if (File.Exists(FeatsFile))
            {
                File.Delete(FeatsFile);
            }
            using (FileStream stream = new FileStream(FeatsFile, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MainMod.ModVersion);
                    writer.Write(Feats.Count);
                    for(int f = 0; f < Feats.Count; f++)
                    {
                        writer.Write((int)Feats[f].type);
                        writer.Write(ScrambleText(Feats[f].PlayerName, true));
                        writer.Write(ScrambleText(Feats[f].FeatSubject, true));
                        writer.Write(Feats[f].FeatDurationInGameDays);
                    }
                }
            }
        }

        public static void LoadFeats()
        {
            FeatsLoaded = true;
            Feats.Clear();
            string SaveFolder = GetSaveFolder;
            if (!Directory.Exists(SaveFolder))
                return;
            string FeatsFile = SaveFolder + "/feats.sav";
            if (!File.Exists(FeatsFile))
                return;
            using (FileStream stream = new FileStream(FeatsFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int Version = reader.ReadInt32();
                    int TotalFeats = reader.ReadInt32();
                    for(int f = 0; f < TotalFeats; f++)
                    {
                        FeatMentioning feat = new FeatMentioning();
                        feat.type = (FeatMentioning.FeatType)reader.ReadInt32();
                        feat.PlayerName = ScrambleText(reader.ReadString(), false);
                        feat.FeatSubject = ScrambleText(reader.ReadString(), false);
                        if (Version < 91)
                            feat.FeatDurationInGameDays = (uint)reader.ReadSingle();
                        else
                            feat.FeatDurationInGameDays = reader.ReadUInt32();
                        Feats.Add(feat);
                    }
                }
            }
        }

        private static string ScrambleText(string Text, bool Scramble)
        {
            string NewText = "";
            if (Scramble)
            {
                foreach(char c in Text)
                {
                    NewText += (char)(c + 64);
                }
            }
            else
            {
                foreach (char c in Text)
                {
                    NewText += (char)(c - 64);
                }
            }
            return NewText;
        }
    }
}
