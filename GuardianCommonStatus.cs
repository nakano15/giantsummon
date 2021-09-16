using System;
using System.IO;
using System.Collections.Generic;
using Terraria;

namespace giantsummon
{
    public class GuardianCommonStatus
    {
        private static Dictionary<string, CommonStatusContainer> CommonStatusContainerList = new Dictionary<string, CommonStatusContainer>();

        public byte LifeCrystalsUsed = 0, LifeFruitsUsed = 0, ManaCrystalsUsed = 0;
        public List<GuardianSkills> SkillList = new List<GuardianSkills>();
        public int LastTotalSkillLevel = 0;
        public TimeSpan? LifeTime = null;

        public static string GetSaveFolder { get { return Main.SavePath + "/TerraGuardians"; } }

        public static bool UseMaxHealthAndManaShare = true, UseSkillProgressShare = true;

        public static GuardianCommonStatus GetCommonStatus(int GuardianID, string GuardianModID = "")
        {
            if (GuardianModID == "")
                GuardianModID = MainMod.mod.Name;
            if (!CommonStatusContainerList.ContainsKey(GuardianModID))
            {
                CommonStatusContainerList.Add(GuardianModID, new CommonStatusContainer());
            }
            return CommonStatusContainerList[GuardianModID].GetCompanion(GuardianID, GuardianModID);
        }

        public GuardianCommonStatus()
        {
            while(SkillList.Count < Enum.GetValues(typeof(GuardianSkills.SkillTypes)).Length)
            {
                SkillList.Add(new GuardianSkills() { skillType = (GuardianSkills.SkillTypes)SkillList.Count });
            }
        }

        public static void SaveStatus(int CompanionID, string CompanionModID = "")
        {
            if (CompanionModID == "")
                CompanionModID = MainMod.mod.Name;
            GuardianBase Base = GuardianBase.GetGuardianBase(CompanionID, CompanionModID);
            string SaveDirectory = GetSaveFolder + "/" + CompanionModID;
            if(!Directory.Exists(SaveDirectory))
                Directory.CreateDirectory(SaveDirectory);
            string SaveFile = SaveDirectory + "/" + Base.Name + ".tgf";
            if (File.Exists(SaveFile))
            {
                File.Delete(SaveFile);
            }
            GuardianCommonStatus status = GetCommonStatus(CompanionID, CompanionModID);
            using(FileStream stream = new FileStream(SaveFile, FileMode.CreateNew))
            {
                using(BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(MainMod.ModVersion);
                    writer.Write(status.LifeCrystalsUsed);
                    writer.Write(status.LifeFruitsUsed);
                    writer.Write(status.ManaCrystalsUsed);
                    writer.Write(status.LifeTime.HasValue);
                    if (status.LifeTime.HasValue)
                        writer.Write(status.LifeTime.Value.TotalSeconds);
                    int Skills = status.SkillList.Count;
                    writer.Write(Skills);
                    for(int s = 0; s < Skills; s++)
                    {
                        GuardianSkills skill = status.SkillList[s];
                        writer.Write(skill.skillType.ToString());
                        writer.Write(skill.Level);
                        writer.Write(skill.Progress);
                    }
                    writer.Write(status.LastTotalSkillLevel);
                }
            }
        }

        public static GuardianCommonStatus LoadStatus(int CompanionID, string CompanionModID = "")
        {
            if (CompanionModID == "")
                CompanionModID = MainMod.mod.Name;
            GuardianBase Base = GuardianBase.GetGuardianBase(CompanionID, CompanionModID);
            string SaveDirectory = GetSaveFolder + "/" + CompanionModID;
            if (!Directory.Exists(SaveDirectory))
                return new GuardianCommonStatus();
            string SaveFile = SaveDirectory + "/" + Base.Name + ".tgf";
            if (!File.Exists(SaveFile))
            {
                return new GuardianCommonStatus();
            }
            GuardianCommonStatus status = new GuardianCommonStatus();
            using (FileStream stream = new FileStream(SaveFile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int ModVersion = reader.ReadInt32();
                    status.LifeCrystalsUsed = reader.ReadByte();
                    status.LifeFruitsUsed = reader.ReadByte();
                    status.ManaCrystalsUsed = reader.ReadByte();
                    if(ModVersion >= 94)
                    {
                        if (reader.ReadBoolean())
                        {
                            status.LifeTime = TimeSpan.FromSeconds(reader.ReadDouble());
                        }
                    }
                    int Skills = reader.ReadInt32();
                    for(int s = 0; s < Skills;s++)
                    {
                        string SkillName = reader.ReadString();
                        int SkillLevel = reader.ReadInt32();
                        float SkillProgress = reader.ReadSingle();
                        foreach(GuardianSkills sk in status.SkillList)
                        {
                            if(sk.skillType.ToString() == SkillName)
                            {
                                sk.Level = SkillLevel;
                                sk.Progress = SkillProgress;
                                break;
                            }
                        }
                    }
                    status.LastTotalSkillLevel = reader.ReadInt32();
                }
            }
            return status;
        }

        private class CommonStatusContainer
        {
            private Dictionary<int, GuardianCommonStatus> CompanionStatus = new Dictionary<int, GuardianCommonStatus>();

            private void AddCommonStatus(int CompanionID, string CompanionModID)
            {
                CompanionStatus.Add(CompanionID, GuardianCommonStatus.LoadStatus(CompanionID, CompanionModID));
            }

            public GuardianCommonStatus GetCompanion(int ID, string ModID)
            {
                if(!CompanionStatus.ContainsKey(ID))
                {
                    AddCommonStatus(ID, ModID);
                }
                return CompanionStatus[ID];
            }
        }
    }
}
