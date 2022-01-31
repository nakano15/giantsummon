using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class GuardianData
    {
        public GuardianBase Base
        {
            get
            {
                if (_Base == null)
                    _Base = GuardianBase.GetGuardianBase(ID, ModID);
                return _Base;
            }
        }
        private GuardianBase _Base = null;
        public GuardianCommonStatus GetCommonStatus
        {
            get
            {
                if (_CommonStatus == null)
                    _CommonStatus = GuardianCommonStatus.GetCommonStatus(ID, ModID);
                return _CommonStatus;
            }
        }
        private GuardianCommonStatus _CommonStatus;
        public Group GetGroup { get { return Base.GetGroup; } }
        public string GroupID { get { return Base.GetGroupID; } }
        public GuardianMood Mood = new GuardianMood();
        public string Name
        {
            get
            {
                if (_Name != null)
                    return _Name;
                else
                {
                    return RealName;
                }
            }
            set
            {
                _Name = value;
            }
        }
        public string _Name = null;
        public string RealName
        {
            get
            {
                if (Base.PossibleNames.Length > 0 && PickedName < Base.PossibleNames.Length)
                {
                    return Base.PossibleNames[PickedName];
                }
                return Base.Name;
            }
        }
        public bool Male = true;
        public int ID { get { return MyID.ID; } set { MyID.ID = value; } }
        public string ModID { get { return MyID.ModID; } set { MyID.ModID = value; } } //To distinguish mod companions
        public byte PickedName = 0;
        public int Age = 18, YearlyAge = 18;
        public string PersonalNicknameToPlayer = null;
        public GuardianID MyID = new GuardianID(0);
        public int MyGuardianID = 0;
        public bool IsStarter = false;
        public bool Tanker = false, MayLootItems = false, AvoidCombat = false, ChargeAhead = false, AttackMyTarget = false, Passive = false, SitOnTheMount = false, SetToPlayerSize = false, GetItemsISendtoTrash = false, UseWeaponsByInventoryOrder = false, ProtectMode = false, AutoSellWhenInvIsFull = false;
        public bool OverrideQuickMountToMountGuardianInstead = false, UseHeavyMeleeAttackWhenMounted = true, HideWereForm = false;
        public uint Coins = 0;
        public Item[] Equipments = new Item[9]; //3 body equipments and 6 accessories
        public Item[] Inventory = new Item[50];
        public GuardianItemSlotFlag[] InventorySlotFlags = new GuardianItemSlotFlag[50];
        public byte SkinID = 0, OutfitID = 0; //Skin handles changing the body of the guardians. Outfits gives clothings to them.
        public Item BodyDye = new Item();
        public List<GuardianCooldownManager> Cooldowns = new List<GuardianCooldownManager>();
        public byte LifeCrystalHealth = 0, LifeFruitHealth = 0, ManaCrystals = 0;
        public const byte MaxLifeCrystals = 15, MaxLifeFruit = 20, MaxManaCrystals = 9;
        public byte FriendshipLevel = 0, FriendshipProgression = 0;
        public sbyte TrustLevel = 30;
        public int LastTotalSkillLevel = 0;
        public RequestData request;
        public int HP = 800, MHP = 800;
        public int MP = 20, MMP = 20;
        public bool KnockedOut = false, KnockedOutCold = false;
        public bool WofFood = false;
        public float HealthHealMult = 1f, ManaHealMult = 1f;
        public int MaxLifeCrystalHealth { get { return (int)(Base.InitialMHP + Base.LifeCrystalHPBonus * TerraGuardian.MaxLifeCrystals); } }
        public int MaxLifeFruitHealth { get { return (int)(Base.InitialMHP + Base.LifeCrystalHPBonus * TerraGuardian.MaxLifeCrystals + Base.LifeFruitHPBonus * TerraGuardian.MaxLifeFruit); } }
        public float TravellingStacker = 0f, DamageStacker = 0f, ComfortStack = 0f; //For damagestacker to work, I need to know whose projectiles were spawned from a Guardian.
        public byte FoodStacker = 0, DrinkStacker = 0, ComfortPoints = 0;
        public CombatTactic tactic = CombatTactic.Assist;
        public TimeSpan? LifeTime { get { return GetCommonStatus.LifeTime; } set { GetCommonStatus.LifeTime = value; } }
        public TimeSpan GetLifeTime
        {
            get
            {
                if (LifeTime.HasValue)
                    return LifeTime.Value;
                return GuardianGlobalInfos.LifeTime;
            }
        }
        public int SavedAge = 0;
        public bool CanBeCalled { get { return FriendshipLevel >= Base.CallUnlockLevel; } }
        public bool CanChangeName { get { return _Name == null; } }
        public List<BuffData> Buffs = new List<BuffData>();
        public List<GuardianSkills> SkillList = new List<GuardianSkills>();
        public int SkillLevelSum = -1;
        public float LastSkillRateMaxValue = 100;
        public const double DaysToYears = GuardianGlobalInfos.DaysInAYear;
        public bool GiftGiven = false;
        public bool IsBirthday
        {
            get
            {
                const float MaxTime = 54000 + 32400;
                float Time = (float)Main.time;
                if (!Main.dayTime) Time += 54000f;
                TimeSpan CurrentTime = GetLifeTime.Duration() - TimeSpan.FromSeconds(Time);
                TimeSpan NextTime = GetLifeTime.Duration() + TimeSpan.FromSeconds(MaxTime - Time);
                int CurrentAge = (int)((CurrentTime.TotalDays - Base.Birthday) / DaysToYears);
                int NextAge = (int)((NextTime.TotalDays - Base.Birthday) / DaysToYears);
                return CurrentAge != NextAge && NextAge > 0;
            }
        }
        public int GetBirthdayAge
        {
            get
            {
                const float MaxTime = 54000 + 32400;
                TimeSpan NextTime = GetLifeTime.Duration() - TimeSpan.FromSeconds(MaxTime);
                return (int)((NextTime.TotalDays + Base.Birthday) / DaysToYears) + (SavedAge > 0 ? SavedAge : Base.Age);
            }
        }
        public byte FriendshipGrade
        {
            get
            {
                byte Grade = 0;
                if (FriendshipLevel >= Base.KnownLevel)
                    Grade++;
                if (FriendshipLevel >= Base.FriendsLevel)
                    Grade++;
                if (FriendshipLevel >= Base.BestFriendLevel)
                    Grade++;
                if (FriendshipLevel >= Base.BestFriendForeverLevel)
                    Grade++;
                if (FriendshipLevel >= Base.BuddiesForLife)
                    Grade++;
                return Grade;
            }
        }
        private BitsByte MessageTriggerFlags = new BitsByte();
        public bool CallMessageUnlocked { get { return MessageTriggerFlags[0]; } set { MessageTriggerFlags[0] = value; } }
        public bool MountMessageUnlocked { get { return MessageTriggerFlags[1]; } set { MessageTriggerFlags[1] = value; } }
        public bool ControlMessageUnlocked { get { return MessageTriggerFlags[2]; } set { MessageTriggerFlags[2] = value; } }
        public bool MoveInMessageUnlocked { get { return MessageTriggerFlags[3]; } set { MessageTriggerFlags[3] = value; } }

        private Dictionary<string, ModGuardianData> ModData = new Dictionary<string, ModGuardianData>();

        //Necessities
        public const int LightWoundCount = 30, HeavyWoundCount = 50,
            LightFatigueCount = 96, HeavyFatigueCount = 96 + 24;
        public sbyte Injury = 0, Fatigue = 0;

        public bool IsSameAs(GuardianData guardian)
        {
            return IsSameAs(guardian.MyID);
        }

        public bool IsSameAs(TerraGuardian guardian)
        {
            return IsSameAs(guardian.MyID);
        }

        public bool IsSameAs(GuardianID GiD)
        {
            return IsSameAs(GiD.ID, GiD.ModID);
        }

        public bool IsSameAs(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (MyID.IsSameID(ID, ModID))
            {
                return true;
            }
            foreach (GuardianID id in Base.IsSameAs)
            {
                if (id.IsSameID(ID, ModID))
                    return true;
            }
            return false;
        }

        public string GetNecessityStatus
        {
            get
            {
                string Status = "Fine";
                if (Injury > 0 || Fatigue > 0)
                {
                    Status = "Normal";
                    if (Injury >= LightWoundCount && Fatigue >= LightFatigueCount)
                    {
                        if (Injury >= HeavyWoundCount)
                            Status = "Crippled";
                        else if (Injury >= LightWoundCount)
                            Status = "Wounded";
                        if (Fatigue > HeavyFatigueCount)
                            Status += " & Exausted";
                        else if (Fatigue >= LightFatigueCount)
                            Status += " & Tired";
                    }
                    else if (Injury >= LightWoundCount)
                    {
                        if (Injury >= HeavyWoundCount)
                            Status = "Crippled";
                        else if (Injury >= LightWoundCount)
                            Status = "Wounded";
                    }
                    else
                    {
                        if (Fatigue >= HeavyFatigueCount)
                            Status = "Exausted";
                        else if(Fatigue >=LightFatigueCount)
                            Status = "Tired";
                    }
                }
                return Status;
            }
        }

        public void GetBirthday(out Season season, out int Day)
        {
            double Birthday = Base.Birthday;
            season = (Season)(Birthday / GuardianGlobalInfos.QuarterOfAYear);
            Day = (int)((Birthday - (int)season * GuardianGlobalInfos.QuarterOfAYear) * GuardianGlobalInfos.QuarterOfAYear);
        }

        public bool HasItem(int ItemID)
        {
            for (int i = 0; i < Inventory.Length; i++)
            {
                if (Inventory[i].type == ItemID)
                    return true;
            }
            return false;
        }

        public void UpdateMood()
        {
            Mood.UpdateMood();
        }

        public void SetStarterGuardian()
        {
            IsStarter = true;
            for (int i = 0; i < Inventory.Length; i++)
            {
                Inventory[i] = new Item();
            }
            Inventory[0].SetDefaults(Terraria.ID.ItemID.WoodenSword);
            Inventory[1].SetDefaults(Terraria.ID.ItemID.Mushroom);
            Inventory[1].stack = 5;
        }

        /*public T GetModGuardian<T>(Mod mod) where T : ModGuardianData
        {
            if (ModData.ContainsKey(mod.Name))
            {
                return (T)ModData[mod.Name];
            }
            ModData.Add(mod.Name, T);
        }*/

        public void AddInjury(byte Value)
        {
            if (Injury + Value < 90)
                Injury += (sbyte)Value;
            else
            {
                Injury = 90;
            }
        }

        public void AddFatigue(byte Value)
        {
            if (Fatigue + Value < 127)
                Fatigue += (sbyte)Value;
            else
            {
                Fatigue = 127;
            }
        }

        public void AddCoins(int p = 0, int g = 0, int s = 0, int c = 0)
        {
            int v = c;
            if (s > 0) v += s * 100;
            if (g > 0) v += g * 10000;
            if (p > 0) v += p * 1000000;
            AddCoins(v);
        }

        public void AddCoins(int Sum)
        {
            if (Sum < 0)
            {
                return;
            }
            if ((long)Sum + this.Coins > uint.MaxValue)
            {
                this.Coins = uint.MaxValue;
            }
            else
            {
                this.Coins += (uint)Sum;
            }
        }

        public bool SubtractCoins(int p = 0, int g = 0, int s = 0, int c = 0)
        {
            int v = c;
            if (s > 0) v += s * 100;
            if (g > 0) v += g * 10000;
            if (p > 0) v += p * 1000000;
            return SubtractCoins(v);
        }
        
        public bool SubtractCoins(int Sub)
        {
            if (Sub < Coins)
                return false;
            Coins -= (uint)Sub;
            return true;
        }

        public ModGuardianData GetModData(Mod mod)
        {
            return GetModData(mod.Name);
        }

        public ModGuardianData GetModData(string ModName)
        {
            if (ModData.ContainsKey(ModName))
                return ModData[ModName];
            return null;
        }

        public GuardianData(int ID = -1, string ModID = "")
        {
            request = new RequestData(this);
            request.SetRequestOnCooldown(true);
            if (Main.rand == null)
                Main.rand = new Terraria.Utilities.UnifiedRandom();
            if (ID != -1)
            {
                this.ID = ID;
                if (ModID == "")
                {
                    ModID = MainMod.mod.Name;
                }
                this.ModID = ModID;
                MyID = new GuardianID(ID, ModID);
                PickedName = (byte)Main.rand.Next(Base.PossibleNames.Length);
            }
            for (int e = 0; e < Equipments.Length; e++)
                Equipments[e] = new Item();
            for (int i = 0; i < Inventory.Length; i++)
                Inventory[i] = new Item();
            while (SkillList.Count < Enum.GetValues(typeof(GuardianSkills.SkillTypes)).Length)
            {
                SkillList.Add(new GuardianSkills() { skillType = (GuardianSkills.SkillTypes)SkillList.Count });
            }
            if (ID != -1)
            {
                int Slot = 0;
                foreach (GuardianBase.ItemPair i in Base.InitialItems)
                {
                    Inventory[Slot].SetDefaults(i.ItemID);
                    Inventory[Slot].stack = i.Stack;
                    Slot++;
                }
            }
        }

        public List<GuardianSkills> GetSkillList
        {
            get
            {
                if (GuardianCommonStatus.UseSkillProgressShare)
                    return GetCommonStatus.SkillList;
                return SkillList;
            }
        }

        public void ResetSkillsProgress()
        {
            int TotalLevel = 0;
            List<GuardianSkills> SkillList = GetSkillList;
            for (int s = 0; s < SkillList.Count; s++)
            {
                TotalLevel += SkillList[s].Level;
                SkillList[s].Level = 0;
                SkillList[s].Progress = 0;
                SkillList[s].MaxProgress = GuardianSkills.ReturnSkillMaxProgress(0, true);
            }
            int ExtraLevel = LastTotalSkillLevel - TotalLevel;
            if (ExtraLevel < 0)
                ExtraLevel = 0;
            int TotalSkillLevel = TotalLevel + ExtraLevel;
            if (GuardianCommonStatus.UseSkillProgressShare)
                GetCommonStatus.LastTotalSkillLevel = TotalSkillLevel;
            else
                LastTotalSkillLevel = TotalSkillLevel;
            Main.NewText(Name + "'s skill levels were resetted.");
        }

        public void AddSkillProgress(float Value, GuardianSkills.SkillTypes Skill)
        {
            if (SkillLevelSum < LastTotalSkillLevel)
                Value *= 2;
            GetSkillList[(byte)Skill].Progress += Value;
        }

        public void UpdateData(Player player)
        {
            if (Base.InvalidGuardian)
                return;
            AddSecond();
            if (player.whoAmI != Main.myPlayer) return;
            int LevelSum = 0;
            List<GuardianSkills> SkillList = GetSkillList;
            for (int s = 0; s < SkillList.Count; s++)
            {
                if (SkillList[s].MaxProgress == 0)
                {
                    SkillList[s].MaxProgress = GuardianSkills.ReturnSkillMaxProgress(SkillList[s].Level, true);
                }
                int LastSkillIncreaseLevel = -1;
                while (SkillLevelSum > -1 && SkillList[s].Level < GuardianSkills.MaxLevel && ((!MainMod.IndividualSkillLeveling && SkillList[s].Progress >= LastSkillRateMaxValue) || (MainMod.IndividualSkillLeveling && SkillList[s].Progress >= SkillList[s].MaxProgress)))
                {
                    SkillList[s].Level++;
                    SkillList[s].Progress -= (MainMod.IndividualSkillLeveling ? SkillList[s].MaxProgress : LastSkillRateMaxValue);
                    SkillList[s].MaxProgress = GuardianSkills.ReturnSkillMaxProgress(SkillList[s].Level, true);
                    if (SkillList[s].Level % 25 == 0)
                    {
                        LastSkillIncreaseLevel = SkillList[s].Level;
                    }
                }
                if (LastSkillIncreaseLevel > -1)
                    Main.NewText(Name + " has reached " + SkillList[s].skillType.ToString() + " level " + LastSkillIncreaseLevel + ".");
                LevelSum += SkillList[s].Level;
            }
            if (LevelSum != SkillLevelSum)
            {
                SkillLevelSum = LevelSum;
                LastSkillRateMaxValue = GuardianSkills.ReturnSkillMaxProgress(LevelSum);
                if (PlayerMod.HasGuardianSummoned(player, ID, ModID))
                {
                    PlayerMod.GetPlayerSummonedGuardian(player, ID, ModID).UpdateStatus = true;
                }
            }
            bool StatusUpdate = false;
            if (MainMod.UsingGuardianNecessitiesSystem)
            {
                if (WorldMod.HourChange)
                {
                    StatusUpdate = true;
                    if (Injury > 0)
                    {
                        Injury--;
                        if (PlayerMod.HasGuardianSummoned(player, ID, ModID))
                        {
                            PlayerMod.GetPlayerSummonedGuardian(player, ID, ModID).UpdateStatus = true;
                        }
                    }
                }
                if (WorldMod.DayChange)
                {
                    if (PlayerMod.HasGuardianSummoned(player, ID, ModID))
                    {
                        Injury -= 2;
                        if (PlayerMod.HasGuardianSummoned(player, ID, ModID))
                        {
                            PlayerMod.GetPlayerSummonedGuardian(player, ID, ModID).UpdateStatus = true;
                        }
                    }
                    else
                    {
                        Injury -= 8;
                        Fatigue -= 36;
                    }
                    StatusUpdate = true;
                }
            }
            if (WorldMod.DayChange && TrustLevel < 20)
            {
				sbyte TrustIncrease = 1;
                TrustLevel++;
				if(!NpcMod.HasGuardianNPC(ID, ModID))
				{
					if (TrustLevel < -40 )
						TrustIncrease += 2;
					else if (TrustLevel < 20 )
						TrustIncrease++;
				}
                ChangeTrustValue(TrustIncrease);
            }
            if (StatusUpdate)
            {
                if (Injury < 0)
                    Injury = 0;
                if (Fatigue < -32)
                    Fatigue = -32;
            }
            UpdateMood();
            if (ShouldScheduleVisit())
            {
                WorldMod.ScheduledVisits.Add(MyID);
            }
        }

        public bool ShouldScheduleVisit()
        {
            return request.state == RequestData.RequestState.Active && WorldMod.HourChange && !WorldMod.IsGuardianNpcInWorld(MyID) && !WorldMod.ScheduledVisits.Contains(MyID);
        }

        public void IncreaseFriendshipProgress(byte Value)
        {
            FriendshipProgression += Value;
            ChangeTrustValue(TrustLevels.TrustPointsPerFriendshipExp);
        }

        public void ChangeTrustValue(sbyte Value)
        {
            sbyte LastTrustLevel = TrustLevel;
            int NewTrustValue = LastTrustLevel + Value;
            if (NewTrustValue < -100)
                NewTrustValue = -100;
            if (NewTrustValue > 100)
                NewTrustValue = 100;
            TrustLevel = (sbyte)NewTrustValue;
        }

        public void AddSecond()
        {
            if (Main.dayTime && Main.time == 0)
            {
                if (IsBirthday)
                {
                    GiftGiven = false;
                    Main.NewText("Today is " + Name + "'s birthday. Let's party!", Color.LightGreen);
                    if (NpcMod.HasGuardianNPC(ID, ModID) && !Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                        Terraria.GameContent.Events.BirthdayParty.ToggleManualParty();
                }
                UpdateAge();
            }
            //
            if(LifeTime != null)
                LifeTime += TimeSpan.FromSeconds(Main.dayRate); //A frame = a second
            //
        }

        public TimeSpan TimeUntilBirthday()
        {
            TimeSpan CurrentTime = GetLifeTime.Duration();
            TimeSpan NextBirthdayTime = CurrentTime.Duration();
            NextBirthdayTime = NextBirthdayTime.Subtract(TimeSpan.FromDays((NextBirthdayTime.TotalDays - Base.Birthday) % DaysToYears));
            NextBirthdayTime = NextBirthdayTime.Add(TimeSpan.FromDays(DaysToYears));
            TimeSpan T = (NextBirthdayTime - CurrentTime);
            //Main.NewText(T.ToString());
            return T;
        }

        public string TimeUntilBirthdayString()
        {
            string T = "";
            TimeSpan Time = TimeUntilBirthday();
            bool First = true;
            if (Time.Days > 0)
            {
                return "In " + Time.Days + " day" + (Time.Days > 1 ? "s" : "");
            }
            if (Time.Hours > 0)
            {
                return "In " + Time.Hours + " hour" + (Time.Hours > 1 ? "s" : "");
            }
            return "Starting soon.";
        }

        public string GetAgeString()
        {
            if(Age != YearlyAge)
            {
                return Age + " (" + YearlyAge + " Years)";
            }
            return Age + " Years Old";
        }

        public int GetAge(bool ByCompanionLifeTimeSpeed = false)
        {
            double AgingFactor = (!ByCompanionLifeTimeSpeed ? (double)Base.GetGroup.AgingSpeed : 1d);
            return (int)((SavedAge > 0 ? SavedAge : Base.Age) * (ByCompanionLifeTimeSpeed ? 1f : 1f / AgingFactor) + ((GetLifeTime.TotalDays - Base.Birthday) * AgingFactor) / DaysToYears);
        }

        public float GetRealAgeDecimal()
        {
            return TerraGuardian.GetAgeDecimalValue((SavedAge > 0 ? SavedAge : Base.Age), Base.Birthday, GetLifeTime, Base.GetGroup.AgingSpeed);
            //return (float)((SavedAge > 0 ? SavedAge : Base.Age) * Base.GetGroup.AgingSpeed + (GetLifeTime.TotalDays * (double)Base.GetGroup.AgingSpeed) / DaysToYears);
        }

        public string GetTime(bool Compressed = false)
        {
            return Lang.LocalizedDuration(GetLifeTime, Compressed, false);
        }

        public void Save(Terraria.ModLoader.IO.TagCompound tag, int UniqueID)
        {
            tag.Add("GuardianID_"+UniqueID, ID);
            tag.Add("GuardianModID_" + UniqueID, ModID);
            if (_Name != null) tag.Add("Name_" + UniqueID, _Name);
            tag.Add("NameID_" + UniqueID, PickedName);
            tag.Add("StarterFlag_" + UniqueID, IsStarter);
            tag.Add("FriendshipLevel_" + UniqueID, FriendshipLevel);
            tag.Add("FriendshipProgress_" + UniqueID, FriendshipProgression);
            tag.Add("TrustLevel_"+UniqueID, (int)TrustLevel);
            tag.Add("TravellingStacker_" + UniqueID, TravellingStacker);
            tag.Add("DamageStacker_" + UniqueID, DamageStacker);
            tag.Add("FoodStacker_" + UniqueID, FoodStacker);
            tag.Add("DrinkStacker_" + UniqueID, DrinkStacker);
            tag.Add("ComfortStack_" + UniqueID, ComfortStack);
            tag.Add("ComfortPoints_" + UniqueID, ComfortPoints);
            tag.Add("HealthPercentage_" + UniqueID, HP <= 0 ? 0 : (HP == MHP ? 1f : (float)HP / MHP));
            tag.Add("IsKnockedOut_" + UniqueID, KnockedOut);
            tag.Add("IsKnockedOutCold_" + UniqueID, KnockedOutCold);
            tag.Add("LifeCrystals_" + UniqueID, LifeCrystalHealth);
            tag.Add("LifeFruits_" + UniqueID, LifeFruitHealth);
            tag.Add("IsMale_" + UniqueID, Male);
            tag.Add("ManaCrystals_" + UniqueID, ManaCrystals);
            tag.Add("CombatTactic_" + UniqueID, (byte)tactic);
            tag.Add("TankingFlag_" + UniqueID, Tanker);
            tag.Add("QuickMountOverride_" + UniqueID, OverrideQuickMountToMountGuardianInstead);
            tag.Add("MountMeleeUsageToggle_" + UniqueID, UseHeavyMeleeAttackWhenMounted);
            tag.Add("AvoidCombat_" + UniqueID, AvoidCombat);
            tag.Add("ChargeAhead_" + UniqueID, ChargeAhead);
            tag.Add("Passive_" + UniqueID, Passive);
            tag.Add("AttackMyTarget_" + UniqueID, AttackMyTarget);
            tag.Add("SitOnMount_" + UniqueID, SitOnTheMount);
            tag.Add("MayLootItems_" + UniqueID, MayLootItems);
            tag.Add("SetToPlayerSize_" + UniqueID, SetToPlayerSize);
            tag.Add("GetItemsISendtoTrash_" + UniqueID, GetItemsISendtoTrash);
            tag.Add("UseWeaponsByInventoryOrder" + UniqueID, UseWeaponsByInventoryOrder);
            tag.Add("ProtectMode" + UniqueID, ProtectMode);
            tag.Add("AutoSell" + UniqueID, AutoSellWhenInvIsFull);
            tag.Add("HideWereForm" + UniqueID, HideWereForm);
            tag.Add("GiftGiven_" + UniqueID, GiftGiven);
            for (int i = 0; i < 8; i++)
            {
                tag.Add("MessageFlags"+i+"_" + UniqueID, MessageTriggerFlags[i]); //It crashes
            }
            int InventorySize = Inventory.Length;
            tag.Add("InventorySize_" + UniqueID, InventorySize);
            for (int i = 0; i < InventorySize; i++)
            {
                tag.Add("Inventory_" + i + "_exists_" + UniqueID, this.Inventory[i].type != 0);
                if (this.Inventory[i].type != 0)
                {
                    tag.Add("Inventory_" + i + "_" + UniqueID, this.Inventory[i]);
                }
            }
            int EquipmentsSize = Equipments.Length;
            tag.Add("EquipmentsSize_" + UniqueID, EquipmentsSize);
            for (int e = 0; e < EquipmentsSize; e++)
            {
                tag.Add("Equipment_" + e + "_exists_" + UniqueID, this.Equipments[e].type != 0);
                if (this.Equipments[e].type != 0)
                {
                    tag.Add("Equipment_" + e + "_" + UniqueID, this.Equipments[e]);
                }
            }
            int BuffCount = Buffs.Count;
            tag.Add("BuffCount_" + UniqueID, BuffCount);
            for (int b = 0; b < Buffs.Count; b++)
            {
                int ID = Buffs[b].ID, Time = Buffs[b].Time;
                if (ID < 0 || ID >= Main.maxBuffTypes)
                    ID = Time = 0;
                tag.Add("Buff_" + b + "_type_" + UniqueID, ID);
                tag.Add("Buff_" + b + "_time_" + UniqueID, Time);
            }
            int SkillCount = SkillList.Count;
            tag.Add("SkillCount_"+UniqueID, SkillCount);
            for (int s = 0; s < SkillCount; s++)
            {
                tag.Add("SkillType_" + s + "_" + UniqueID, SkillList[s].skillType.ToString());
                tag.Add("SkillLevel_" + s + "_" + UniqueID, SkillList[s].Level);
                tag.Add("SkillProgress_" + s + "_" + UniqueID, SkillList[s].Progress);
            }
            tag.Add("BodyDye_" + UniqueID, BodyDye.type);
            tag.Add("FatigueCount_" + UniqueID, (byte)Fatigue);
            tag.Add("InjuryCount_" + UniqueID, (byte)Injury);
            //request.Save(tag, UniqueID);
            request.Save(tag, "Request_"+UniqueID);
            /*tag.Add("HasExistenceTime_" + UniqueID, LifeTime.HasValue);
            if(LifeTime.HasValue)
                tag.Add("ExistenceTime_" + UniqueID, LifeTime.Value.TotalSeconds);*/
            tag.Add("SkinID_" + UniqueID, SkinID);
            tag.Add("OutfitID_" + UniqueID, OutfitID);
            tag.Add("Coins_" + UniqueID, (int)Coins - int.MaxValue);
            SaveCustom(tag, UniqueID);
            GuardianCommonStatus.SaveStatus(ID, ModID);
        }

        public void Load(Terraria.ModLoader.IO.TagCompound tag, int ModVersion, int UniqueID)
        {
            ID = tag.GetInt("GuardianID_" + UniqueID);
            if (ModVersion >= 35)
            {
                ModID = tag.GetString("GuardianModID_" + UniqueID);
            }
            else
            {
                ModID = MainMod.mod.Name;
            }
            if (ModVersion >= 19 && tag.ContainsKey("Name_"+UniqueID))
            {
                _Name = tag.GetString("Name_" + UniqueID);
                if (_Name == "")
                    _Name = null;
            }
            if (ModVersion >= 72)
            {
                PickedName = tag.GetByte("NameID_" + UniqueID);
                GuardianBase gb = Base;
                if(gb.PossibleNames.Length > 0 && PickedName > gb.PossibleNames.Length)
                {
                    PickedName = (byte)Main.rand.Next(gb.PossibleNames.Length);
                }
            }
            if (ModVersion >= 52)
            {
                IsStarter = tag.GetBool("StarterFlag_" + UniqueID);
            }
            if (ModVersion >= 7)
            {
                FriendshipLevel = tag.GetByte("FriendshipLevel_" + UniqueID);
                FriendshipProgression = tag.GetByte("FriendshipProgress_" + UniqueID);
                if (ModVersion >= 93)
                    TrustLevel = (sbyte)tag.GetInt("TrustLevel_" + UniqueID);
                TravellingStacker = tag.GetFloat("TravellingStacker_" + UniqueID);
            }
            if (ModVersion >= 18)
                DamageStacker = tag.GetFloat("DamageStacker_" + UniqueID);
            if (ModVersion >= 25)
            {
                FoodStacker = tag.GetByte("FoodStacker_" + UniqueID);
                DrinkStacker = tag.GetByte("DrinkStacker_" + UniqueID);
            }
            if (ModVersion >= 64)
            {
                ComfortStack = tag.GetFloat("ComfortStack_" + UniqueID);
                ComfortPoints = tag.GetByte("ComfortPoints_" + UniqueID);
            }
            float HealthPercentage = -1;
            if (ModVersion >= 1)
            {
                HealthPercentage = tag.GetFloat("HealthPercentage_" + UniqueID);
                if (HealthPercentage >= 0)
                {
                    if (HealthPercentage <= 0)
                        HP = 1;
                    else
                        HP = (int)(MHP * HealthPercentage);
                }
            }
            if (ModVersion >= 58)
            {
                KnockedOut = tag.GetBool("IsKnockedOut_" + UniqueID);
                KnockedOutCold = tag.GetBool("IsKnockedOutCold_" + UniqueID);
            }
            LifeCrystalHealth = (ModVersion < 42 ? (byte)tag.GetInt("LifeCrystals_" + UniqueID) : tag.GetByte("LifeCrystals_" + UniqueID));
            LifeFruitHealth = (ModVersion < 42 ? (byte)tag.GetInt("LifeFruits_" + UniqueID) : tag.GetByte("LifeFruits_" + UniqueID));
            if (ModVersion >= 33)
                Male = tag.GetBool("IsMale_" + UniqueID);
            if(ModVersion >= 32 && ModVersion < 42)
                tag.GetBool("ExtraAccessorySlot_" + UniqueID);
            if (ModVersion >= 27)
                ManaCrystals = (ModVersion < 42 ? (byte)tag.GetInt("ManaCrystals_" + UniqueID) : tag.GetByte("ManaCrystals_" + UniqueID));
            if (ModVersion >= 5)
                tactic = (CombatTactic)tag.GetByte("CombatTactic_" + UniqueID);
            if (ModVersion >= 6)
                Tanker = tag.GetBool("TankingFlag_" + UniqueID);
            if (ModVersion >= 11)
                OverrideQuickMountToMountGuardianInstead = tag.GetBool("QuickMountOverride_" + UniqueID);
            if (ModVersion >= 12)
                UseHeavyMeleeAttackWhenMounted = tag.GetBool("MountMeleeUsageToggle_" + UniqueID);
            if (ModVersion >= 13)
            {
                AvoidCombat = tag.GetBool("AvoidCombat_" + UniqueID);
                ChargeAhead = tag.GetBool("ChargeAhead_" + UniqueID);
            }
            if (ModVersion >= 17)
            {
                Passive = tag.GetBool("Passive_" + UniqueID);
                AttackMyTarget = tag.GetBool("AttackMyTarget_" + UniqueID);
            }
            if (ModVersion >= 21)
            {
                SitOnTheMount = tag.GetBool("SitOnMount_" + UniqueID);
            }
            if (ModVersion >= 26)
            {
                MayLootItems = tag.GetBool("MayLootItems_" + UniqueID);
            }
            if (ModVersion >= 28)
            {
                SetToPlayerSize = tag.GetBool("SetToPlayerSize_" + UniqueID);
            }
            if (ModVersion >= 31)
            {
                GetItemsISendtoTrash = tag.GetBool("GetItemsISendtoTrash_" + UniqueID);
            }
            if (ModVersion >= 41)
            {
                UseWeaponsByInventoryOrder = tag.GetBool("UseWeaponsByInventoryOrder" + UniqueID);
            }
            if (ModVersion >= 46)
            {
                ProtectMode = tag.GetBool("ProtectMode" + UniqueID);
            }
            if (ModVersion >= 49)
            {
                AutoSellWhenInvIsFull = tag.GetBool("AutoSell" + UniqueID);
            }
            if (ModVersion >= 65)
            {
                HideWereForm = tag.GetBool("HideWereForm" + UniqueID);
            }
            if (ModVersion >= 30)
            {
                GiftGiven = tag.GetBool("GiftGiven_" + UniqueID);
            }
            if (ModVersion >= 53)
            {
                for (int i = 0; i < 8; i++)
                {
                    MessageTriggerFlags[i] = tag.GetBool("MessageFlags" + i + "_" + UniqueID);
                    if(ModVersion < 78)
                        tag.GetBool("FriendLevelFlags" + i + "_" + UniqueID);
                }
            }
            int ContainerSize = tag.GetInt("InventorySize_" + UniqueID);
            for (int i = 0; i < ContainerSize; i++)
            {
                Item j = new Item();
                if (ModVersion < 2)
                {
                    j.SetDefaults(tag.GetInt("Inventory_" + i + "_Type_" + UniqueID));
                    j.stack = tag.GetInt("Inventory_" + i + "_Stack_" + UniqueID);
                    j.prefix = tag.GetByte("Inventory_" + i + "_Prefix_" + UniqueID);
                }
                else
                {
                    bool ItemExists = true;
                    if (ModVersion >= 3) ItemExists = tag.GetBool("Inventory_" + i + "_exists_" + UniqueID);
                    if (ItemExists)
                    {
                        j = tag.Get<Item>("Inventory_" + i + "_" + UniqueID);
                    }
                }
                if (i < Inventory.Length)
                    Inventory[i] = j;
            }
            ContainerSize = tag.GetInt("EquipmentsSize_" + UniqueID);
            for (int e = 0; e < ContainerSize; e++)
            {
                Item j = new Item();
                if (ModVersion < 4)
                {
                    j.SetDefaults(tag.GetInt("Equipment_" + e + "_Type_" + UniqueID));
                    j.stack = tag.GetInt("Equipment_" + e + "_Stack_" + UniqueID);
                    j.prefix = tag.GetByte("Equipment_" + e + "_Prefix_" + UniqueID);
                }
                else
                {
                    if (tag.GetBool("Equipment_" + e + "_exists_" + UniqueID))
                    {
                        j = tag.Get<Item>("Equipment_" + e + "_" + UniqueID);
                    }
                }
                if (e < Equipments.Length)
                    Equipments[e] = j;
            }
            if (ModVersion >= 22)
            {
                int BuffCount = tag.GetInt("BuffCount_" + UniqueID);
                for (int b = 0; b < BuffCount; b++)
                {
                    int ID = tag.GetInt("Buff_" + b + "_type_" + UniqueID),
                        Time = tag.GetInt("Buff_" + b + "_time_" + UniqueID);
                    if(Time > 0)
                        Buffs.Add(new BuffData(ID, Time));
                }
            }
            if (ModVersion >= 23)
            {
                int SkillCount = tag.GetInt("SkillCount_" + UniqueID);
                string[] SkillNames = Enum.GetNames(typeof(GuardianSkills.SkillTypes));
                for (int s = 0; s < SkillCount; s++)
                {
                    string SkillType = tag.GetString("SkillType_" + s + "_" + UniqueID);
                    int SkillLevel = tag.GetInt("SkillLevel_" + s + "_" + UniqueID);
                    float SkillProgress = tag.GetFloat("SkillProgress_" + s + "_" + UniqueID);
                    for (int s2 = 0; s2 < SkillNames.Length; s2++)
                    {
                        if (SkillNames[s2] == SkillType)
                        {
                            SkillList[s2].Level = SkillLevel;
                            SkillList[s2].Progress = SkillProgress;
                            break;
                        }
                    }
                }
            }
            if (ModVersion >= 29)
                BodyDye.SetDefaults(tag.GetInt("BodyDye_" + UniqueID));
            if (ModVersion >= 56)
            {
                Fatigue = (sbyte)tag.GetByte("FatigueCount_" + UniqueID);
                Injury = (sbyte)tag.GetByte("InjuryCount_" + UniqueID);
            }
            /*if (ModVersion >= 61)
            {
                request.Load(tag, ModVersion, UniqueID, this);
            }*/
            if(ModVersion >= 101)
                request.Load(tag, "Request_" + UniqueID, ModVersion);
            //if (ModVersion >= 8)
            //    request.Load(tag, ModVersion, UniqueID);
            /*if (ModVersion < 95)
            {
                if (ModVersion <= 80)
                {
                    if (ModVersion >= 15)
                        LifeTime = TimeSpan.FromSeconds(tag.GetDouble("ExistenceTime_" + UniqueID));
                }
                else
                {
                    if (tag.GetBool("HasExistenceTime_" + UniqueID))
                    {
                        LifeTime = TimeSpan.FromSeconds(tag.GetDouble("ExistenceTime_" + UniqueID));
                    }
                }
            }*/
            if (ModVersion >= 66)
            {
                SkinID = tag.GetByte("SkinID_" + UniqueID);
                OutfitID = tag.GetByte("OutfitID_" + UniqueID);
            }
            if (ModVersion >= 67)
            {
                Coins = (uint)(tag.GetInt("Coins_" + UniqueID) + int.MaxValue);
            }
            if (ModVersion < 80)
                ResetSkillsProgress();
            if(ModVersion >= 83)
                LoadCustom(tag, ModVersion, UniqueID);
            if(ModVersion < 90)
            {
                GuardianCommonStatus status = GetCommonStatus;
                if (status.LifeCrystalsUsed < LifeCrystalHealth)
                    status.LifeCrystalsUsed = LifeCrystalHealth;
                if (status.LifeFruitsUsed < LifeFruitHealth)
                    status.LifeFruitsUsed = LifeFruitHealth;
                if (status.ManaCrystalsUsed < ManaCrystals)
                    status.ManaCrystalsUsed = ManaCrystals;
                foreach(GuardianSkills skill in status.SkillList)
                {
                    foreach(GuardianSkills other in SkillList)
                    {
                        if(skill.skillType == other.skillType && skill.Level < other.Level)
                        {
                            skill.Level = other.Level;
                            skill.Progress = other.Progress;
                        }
                    }
                }
            }
            if (ModVersion < 95 && TrustLevel < 30)
                TrustLevel = 30;
        }

        public void UpdateAge()
        {
            Age = GetAge();
            YearlyAge = GetAge(true);
        }

        public virtual void SaveCustom(Terraria.ModLoader.IO.TagCompound tag, int UniqueID)
        {

        }

        public virtual void LoadCustom(Terraria.ModLoader.IO.TagCompound tag, int ModVersion, int UniqueID)
        {

        }

        public string GetMessage(string MessageID, string DefaultMessage = "")
        {
            string Mes = Base.GetSpecialMessage(MessageID);
            if (Mes == "" && DefaultMessage != "")
                Mes = DefaultMessage;
            return Mes;
        }

        public bool CheckForImportantMessages(out string Text)
        {
            Text = "";
            if (Base.CallUnlockLevel > 0 && !CallMessageUnlocked && !IsStarter && FriendshipLevel >= Base.CallUnlockLevel)
            {
                Text = Base.CallUnlockMessage + "\n[You can now call this companion]";
                CallMessageUnlocked = true;
                return true;
            }
            if (!MountMessageUnlocked && FriendshipLevel >= Base.MountUnlockLevel)
            {
                Text = Base.MountUnlockMessage + (!Base.ReverseMount ? "\n[You can now mount on this companion]" : "\n[This companion can now mount on your back.");
                MountMessageUnlocked = true;
                return true;
            }
            if (!ControlMessageUnlocked && FriendshipLevel >= Base.ControlUnlockLevel)
            {
                Text = Base.ControlUnlockMessage + "\n[You can now control this companion]";
                ControlMessageUnlocked = true;
                return true;
            }
            if (Base.MoveInLevel > 0 && !MoveInMessageUnlocked && !IsStarter && FriendshipLevel >= Base.MoveInLevel)
            {
                Text = Base.MoveInUnlockMessage + "\n[This companion can now move in to your world.]";
                MoveInMessageUnlocked = true;
                return true;
            }
            return false;
        }
    }
}
