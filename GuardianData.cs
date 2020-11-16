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
        public GuardianBase Base { get { return GuardianBase.GetGuardianBase(ID, ModID); } }
        public Group GetGroup { get { return Base.GetGroup; } }
        public string GroupID { get { return Base.GetGroupID; } }
        public string Name
        {
            get
            {
                if (_Name != null)
                    return _Name;
                else
                {
                    if (Base.PossibleNames.Length > 0 && PickedName < Base.PossibleNames.Length)
                    {
                        return Base.PossibleNames[PickedName];
                    }
                    return Base.Name;
                }
            }
            set
            {
                _Name = value;
            }
        }
        public string _Name = null;
        public bool Male = true;
        public int ID { get { return MyID.ID; } set { MyID.ID = value; } }
        public string ModID { get { return MyID.ModID; } set { MyID.ModID = value; } } //To distinguish mod companions
        public byte PickedName = 0;
        public string PersonalNicknameToPlayer = null;
        public GuardianID MyID = new GuardianID(0);
        public bool IsStarter = false;
        public bool Tanker = false, MayLootItems = false, AvoidCombat = false, ChargeAhead = false, AttackMyTarget = false, Passive = false, SitOnTheMount = false, SetToPlayerSize = false, GetItemsISendtoTrash = false, UseWeaponsByInventoryOrder = false, ProtectMode = false, AutoSellWhenInvIsFull = false;
        public bool OverrideQuickMountToMountGuardianInstead = false, UseHeavyMeleeAttackWhenMounted = true, HideWereForm = false;
        public uint Coins = 0;
        public Item[] Equipments = new Item[9]; //3 body equipments and 6 accessories
        public Item[] Inventory = new Item[50];
        public byte SkinID = 0, OutfitID = 0; //Skin handles changing the body of the guardians. Outfits gives clothings to them.
        public Item BodyDye = new Item();
        public List<GuardianCooldownManager> Cooldowns = new List<GuardianCooldownManager>();
        public byte LifeCrystalHealth = 0, LifeFruitHealth = 0, ManaCrystals = 0;
        public const byte MaxLifeCrystals = 15, MaxLifeFruit = 20, MaxManaCrystals = 9;
        public byte FriendshipLevel = 0, FriendshipProgression = 0;
        public int LastTotalSkillLevel = 0;
        public RequestData request = new RequestData();
        public int HP = 800, MHP = 800;
        public int MP = 20, MMP = 20;
        public bool KnockedOut = false, KnockedOutCold = false;
        public bool WofFood = false;
        public float HealthHealMult = 1f, ManaHealMult = 1f;
        public int MaxLifeCrystalHealth { get { return Base.InitialMHP + Base.LifeCrystalHPBonus * TerraGuardian.MaxLifeCrystals; } }
        public int MaxLifeFruitHealth { get { return Base.InitialMHP + Base.LifeCrystalHPBonus * TerraGuardian.MaxLifeCrystals + Base.LifeFruitHPBonus * TerraGuardian.MaxLifeFruit; } }
        public float TravellingStacker = 0f, DamageStacker = 0f, ComfortStack = 0f; //For damagestacker to work, I need to know whose projectiles were spawned from a Guardian.
        public byte FoodStacker = 0, DrinkStacker = 0, ComfortPoints = 0;
        public CombatTactic tactic = CombatTactic.Assist;
        public TimeSpan LifeTime = new TimeSpan();
        public bool CanBeCalled { get { return FriendshipLevel >= Base.CallUnlockLevel; } }
        public bool CanChangeName { get { return _Name == null; } }
        public List<BuffData> Buffs = new List<BuffData>();
        public List<GuardianSkills> SkillList = new List<GuardianSkills>();
        public int SkillLevelSum = -1;
        public float LastSkillRateMaxValue = 100;
        public const double DaysToYears = 32;
        public bool GiftGiven = false;
        public bool IsBirthday
        {
            get
            {
                const float MaxTime = 54000 + 32400;
                float Time = (float)Main.time;
                if (!Main.dayTime) Time += 54000f;
                TimeSpan CurrentTime = LifeTime.Duration() - TimeSpan.FromSeconds(Time);
                TimeSpan NextTime = LifeTime.Duration() + TimeSpan.FromSeconds(MaxTime - Time);
                int CurrentAge = (int)(CurrentTime.TotalDays / DaysToYears);
                int NextAge = (int)(NextTime.TotalDays / DaysToYears);
                return CurrentAge != NextAge && NextAge > 0;
            }
        }
        public int GetBirthdayAge
        {
            get
            {
                const float MaxTime = 54000 + 32400;
                TimeSpan NextTime = LifeTime.Duration() + TimeSpan.FromSeconds(MaxTime);
                return (int)(NextTime.TotalDays / DaysToYears) + Base.Age;
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
        private BitsByte MessageTriggerFlags = new BitsByte(), FriendshipLevelFlags = new BitsByte();
        public bool CallMessageUnlocked { get { return MessageTriggerFlags[0]; } set { MessageTriggerFlags[0] = value; } }
        public bool MountMessageUnlocked { get { return MessageTriggerFlags[1]; } set { MessageTriggerFlags[1] = value; } }
        public bool ControlMessageUnlocked { get { return MessageTriggerFlags[2]; } set { MessageTriggerFlags[2] = value; } }

        public bool FriendMessageUnlocked { get { return FriendshipLevelFlags[0]; } set { FriendshipLevelFlags[0] = value; } }
        public bool BestFriendMessageUnlocked { get { return FriendshipLevelFlags[1]; } set { FriendshipLevelFlags[1] = value; } }
        public bool BFFMessageUnlocked { get { return FriendshipLevelFlags[2]; } set { FriendshipLevelFlags[2] = value; } }
        public bool BuddyForLifeMessageUnlocked { get { return FriendshipLevelFlags[3]; } set { FriendshipLevelFlags[3] = value; } }

        private Dictionary<string, ModGuardianData> ModData = new Dictionary<string, ModGuardianData>();

        //Necessities
        public const int LightWoundCount = 30, HeavyWoundCount = 50,
            LightFatigueCount = 96, HeavyFatigueCount = 96 + 24;
        public sbyte Injury = 0, Fatigue = 0;

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

        public bool HasPersonalRequestBeenCompleted(int ID)
        {
            return request.RequestsCompletedIDs.Contains(ID);
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

        private Reward[] GetCommonAndBaseRewards(Player player)
        {
            List<Reward> RewardsToGet = new List<Reward>();
            RewardsToGet.AddRange(Base.RewardsList);
            {
                Reward rwd = new Reward();
                if (!MainMod.NoEtherItems && Base.IsTerraGuardian)
                {
                    rwd.ItemID = ModContent.ItemType<Items.Consumable.EtherHeart>();
                    rwd.RewardScore = 500;
                    rwd.InitialStack = 1;
                    rwd.RewardChance = 0.333f;
                    RewardsToGet.Add(rwd);
                }
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.LifeCrystal;
                rwd.RewardScore = 1000;
                rwd.InitialStack = 1;
                rwd.RewardChance = 0.2f;
                RewardsToGet.Add(rwd);
                //
                if (!MainMod.NoEtherItems && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Base.IsTerraGuardian)
                {
                    rwd = new Reward();
                    rwd.ItemID = ModContent.ItemType<Items.Consumable.EtherFruit>();
                    rwd.RewardScore = 750;
                    rwd.InitialStack = 1;
                    rwd.MaxExtraStack = 2;
                    rwd.RewardChance = 0.333f;
                    RewardsToGet.Add(rwd);
                }
                //
                rwd = new Reward();
                rwd.ItemID = ModContent.ItemType<Items.Consumable.SkillResetPotion>();
                rwd.RewardScore = 2000;
                rwd.InitialStack = 1;
                rwd.RewardChance = 0.0667f;
                RewardsToGet.Add(rwd);
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.WoodenCrate;
                rwd.RewardScore = 250;
                rwd.InitialStack = 1;
                rwd.MaxExtraStack = 2;
                rwd.RewardChance = 0.0625f;
                RewardsToGet.Add(rwd);
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.IronCrate;
                rwd.RewardScore = 250;
                rwd.InitialStack = 1;
                rwd.MaxExtraStack = 2;
                rwd.RewardChance = 0.0390625f;
                RewardsToGet.Add(rwd);
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.GoldenCrate;
                rwd.RewardScore = 250;
                rwd.InitialStack = 1;
                rwd.MaxExtraStack = 2;
                rwd.RewardChance = 0.009765625f;
                RewardsToGet.Add(rwd);
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.CookedFish;
                rwd.RewardScore = 150;
                rwd.InitialStack = 1;
                rwd.MaxExtraStack = 2;
                rwd.RewardChance = 0.125f;
                RewardsToGet.Add(rwd);
                //
                rwd = new Reward();
                rwd.ItemID = Terraria.ID.ItemID.BowlofSoup;
                rwd.RewardScore = 125;
                rwd.InitialStack = 1;
                rwd.MaxExtraStack = 2;
                rwd.RewardChance = 0.125f;
                RewardsToGet.Add(rwd);
            }
            int HighestPickValue = 0, HighestAxeValue = 0, HighestFishingPowerValue = 0, HighestBaitValue = 0;
            int BaitPosition = -1;
            for (int i = 0; i < 50; i++)
            {
                if (this.Inventory[i].pick > HighestPickValue)
                {
                    HighestPickValue = this.Inventory[i].pick;
                }
                if (this.Inventory[i].axe > HighestAxeValue)
                {
                    HighestAxeValue = this.Inventory[i].axe;
                }
                if (this.Inventory[i].fishingPole > HighestFishingPowerValue)
                {
                    HighestFishingPowerValue = this.Inventory[i].fishingPole;
                }
                if (this.Inventory[i].bait > HighestBaitValue)
                {
                    HighestBaitValue = this.Inventory[i].bait;
                    BaitPosition = i;
                }
            }
            if (HighestPickValue > 0)
            {
                float PickPower = 0.5f + (float)HighestPickValue / 256;
                Reward rwd;
                if (Main.hardMode && HighestPickValue >= 100)
                {
                    if (HighestPickValue >= 100)
                    {
                        rwd = new Reward();
                        rwd.ItemID = (WorldGen.oreTier1 == 107 ? 364 : 1104);
                        rwd.RewardScore = 250;
                        rwd.InitialStack = (int)(40 * PickPower);
                        rwd.RewardChance = 0.45f * PickPower;
                        RewardsToGet.Add(rwd);
                    }
                    if (HighestPickValue >= 110)
                    {
                        rwd = new Reward();
                        rwd.ItemID = (WorldGen.oreTier2 == 108 ? 365 : 1105);
                        rwd.RewardScore = 325;
                        rwd.InitialStack = (int)(50 * PickPower);
                        rwd.RewardChance = 0.35f * PickPower;
                        RewardsToGet.Add(rwd);
                    }
                    if (HighestPickValue >= 150)
                    {
                        rwd = new Reward();
                        rwd.ItemID = (WorldGen.oreTier3 == 111 ? 366 : 1106);
                        rwd.RewardScore = 400;
                        rwd.InitialStack = (int)(60 * PickPower);
                        rwd.RewardChance = 0.20f * PickPower;
                        RewardsToGet.Add(rwd);
                    }
                    if (HighestPickValue >= 200 && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                        rwd = new Reward();
                        rwd.ItemID = Terraria.ID.ItemID.ChlorophyteOre;
                        rwd.RewardScore = 500;
                        rwd.InitialStack = (int)(50 * PickPower);
                        rwd.RewardChance = 0.35f * PickPower;
                        RewardsToGet.Add(rwd);
                    }
                    if (NPC.downedMoonlord)
                    {
                        rwd = new Reward();
                        rwd.ItemID = Terraria.ID.ItemID.LunarOre;
                        rwd.RewardScore = 650;
                        rwd.InitialStack = (int)(80 * PickPower);
                        rwd.RewardChance = 0.7f * PickPower;
                        RewardsToGet.Add(rwd);
                    }
                }
                else
                {
                    if (HighestPickValue >= 50)
                    {
                        if (HighestPickValue >= 50)
                        {
                            rwd = new Reward();
                            rwd.ItemID = (WorldGen.crimson ? Terraria.ID.ItemID.CrimtaneOre : Terraria.ID.ItemID.DemoniteOre);
                            rwd.RewardScore = 300;
                            rwd.InitialStack = (int)(20 * PickPower);
                            rwd.RewardChance = 0.6f * PickPower;
                            RewardsToGet.Add(rwd);
                            if (WorldGen.shadowOrbSmashed && NPC.downedBoss2)
                            {
                                rwd = new Reward();
                                rwd.ItemID = Terraria.ID.ItemID.Meteorite;
                                rwd.RewardScore = 200;
                                rwd.InitialStack = (int)(40 * PickPower);
                                rwd.RewardChance = 0.5f * PickPower;
                                RewardsToGet.Add(rwd);
                            }
                        }
                        if (HighestPickValue >= 65)
                        {
                            rwd = new Reward();
                            rwd.ItemID = Terraria.ID.ItemID.Hellstone;
                            rwd.RewardScore = 350;
                            rwd.InitialStack = (int)(30 * PickPower);
                            rwd.RewardChance = 0.55f * PickPower;
                            RewardsToGet.Add(rwd);
                            rwd = new Reward();
                            rwd.ItemID = Terraria.ID.ItemID.Obsidian;
                            rwd.RewardScore = 250;
                            rwd.InitialStack = (int)(30 * PickPower);
                            rwd.RewardChance = 0.55f * PickPower;
                            RewardsToGet.Add(rwd);
                        }
                    }
                    else
                    {

                    }
                }
            }
            if (HighestFishingPowerValue > 0)
            {
                if (HighestBaitValue > 0)
                {
                    float FishingPower = (float)(HighestFishingPowerValue + HighestBaitValue) / 256 + 0.5f;
                    Reward rwd = new Reward();
                    if (player.ZoneSnow)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.AtlanticCod;
                    }
                    else if (player.ZoneUnderworldHeight)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.Obsidifish;
                    }
                    else if (player.ZoneBeach)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.Tuna;
                    }
                    else if (player.ZoneSkyHeight)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.Damselfish;
                    }
                    else if (player.ZoneDirtLayerHeight)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.ArmoredCavefish;
                    }
                    else if (player.ZoneHoly)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.PrincessFish;
                    }
                    else if (player.ZoneCrimson)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.CrimsonTigerfish;
                    }
                    else if (player.ZoneCorrupt)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.Ebonkoi;
                    }
                    else
                    {
                        rwd.ItemID = Terraria.ID.ItemID.Bass;
                    }
                    rwd.RewardScore = 100;
                    rwd.InitialStack = 2;
                    rwd.MaxExtraStack = 3;
                    rwd.RewardChance = 0.66f * (FishingPower);
                    RewardsToGet.Add(rwd);
                    rwd = new Reward();
                    rwd.ItemID = Terraria.ID.ItemID.WoodenCrate;
                    rwd.RewardScore = 250;
                    rwd.InitialStack = 1;
                    rwd.MaxExtraStack = 2;
                    rwd.RewardChance = 0.3f * (FishingPower);
                    RewardsToGet.Add(rwd);
                    rwd.ItemID = Terraria.ID.ItemID.IronCrate;
                    rwd.RewardScore = 400;
                    rwd.InitialStack = 1;
                    rwd.MaxExtraStack = 2;
                    rwd.RewardChance = 0.02f * (FishingPower);
                    RewardsToGet.Add(rwd);
                    rwd.ItemID = Terraria.ID.ItemID.GoldenCrate;
                    rwd.RewardScore = 750;
                    rwd.InitialStack = 1;
                    rwd.MaxExtraStack = 2;
                    rwd.RewardChance = 0.01f * (FishingPower);
                    RewardsToGet.Add(rwd);
                    if (player.ZoneCorrupt)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.CorruptFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    if (player.ZoneHoly)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.HallowedFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    if (player.ZoneCrimson)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.CrimsonFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    if (player.ZoneJungle)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.JungleFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    if (player.ZoneDungeon)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.DungeonFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    if (player.ZoneSkyHeight)
                    {
                        rwd.ItemID = Terraria.ID.ItemID.FloatingIslandFishingCrate;
                        rwd.RewardScore = 1000;
                        rwd.InitialStack = 1;
                        rwd.MaxExtraStack = 2;
                        rwd.RewardChance = 0.01f * (FishingPower);
                        RewardsToGet.Add(rwd);
                    }
                    Inventory[BaitPosition].stack -= Main.rand.Next(3);
                    if (Inventory[BaitPosition].stack <= 0)
                        Inventory[BaitPosition].SetDefaults(0, true);
                }
            }
            return RewardsToGet.ToArray();
        }

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

        public void ResetSkillsProgress()
        {
            int TotalLevel = 0;
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
            LastTotalSkillLevel = TotalLevel + ExtraLevel;
            Main.NewText(Name + "'s skill levels were resetted.");
        }

        public void AddSkillProgress(float Value, GuardianSkills.SkillTypes Skill)
        {
            if (SkillLevelSum < LastTotalSkillLevel)
                Value *= 2;
            SkillList[(byte)Skill].Progress += Value;
        }

        public void UpdateData(Player player)
        {
            if (Base.InvalidGuardian)
                return;
            AddSecond();
            if (player.whoAmI != Main.myPlayer) return;
            int LevelSum = 0;
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
                    /*if (PlayerMod.HasGuardianSummoned(player, ID, ModID))
                    {

                    }
                    else
                    {

                    }*/
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
            if (StatusUpdate)
            {
                if (Injury < 0)
                    Injury = 0;
                if (Fatigue < -32)
                    Fatigue = -32;
            }
        }
        
        public Item[] GetRewards(int Score, Player player)
        {
            List<Reward> RewardsToGet = new List<Reward>();
            RewardsToGet.AddRange(GetCommonAndBaseRewards(player));
            RewardsToGet = RewardsToGet.OrderByDescending(x => x.RewardScore).ToList();
            List<Item> Rewards = new List<Item>();
            foreach (Reward rwd in RewardsToGet)
            {
                if (Score >= rwd.RewardScore && Main.rand.NextDouble() < rwd.RewardChance)
                {
                    int ItemID = rwd.ItemID;
                    int Stack = rwd.InitialStack;
                    int ScoreDeduction = rwd.RewardScore;
                    if (rwd.MaxExtraStack > 0)
                    {
                        int ExtraStackCount = Main.rand.Next(rwd.MaxExtraStack + 1);
                        int MaxPoints = Score / rwd.RewardScore;
                        if (ExtraStackCount > MaxPoints)
                            ExtraStackCount = MaxPoints;
                        ScoreDeduction = ScoreDeduction * ExtraStackCount;
                        Stack += ExtraStackCount;
                    }
                    Score -= ScoreDeduction;
                    Item i = new Item();
                    i.SetDefaults(ItemID);
                    i.stack = Stack;
                    Rewards.Add(i);
                }
            }
            if (Score > 0)
            {
                int c = Score, s = 0, g = 0, p = 0;
                if (c >= 100)
                {
                    s += c / 100;
                    c -= s * 100;
                }
                if (s >= 100)
                {
                    g += s / 100;
                    s -= g * 100;
                }
                if (g >= 100)
                {
                    p += g / 100;
                    g -= p * 100;
                }
                if (p > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(Terraria.ID.ItemID.PlatinumCoin, true);
                    i.stack = p;
                    Rewards.Add(i);
                }
                if (g > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(Terraria.ID.ItemID.GoldCoin, true);
                    i.stack = g;
                    Rewards.Add(i);
                }
                if (s > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(Terraria.ID.ItemID.SilverCoin, true);
                    i.stack = s;
                    Rewards.Add(i);
                }
                if (c > 0)
                {
                    Item i = new Item();
                    i.SetDefaults(Terraria.ID.ItemID.CopperCoin, true);
                    i.stack = c;
                    Rewards.Add(i);
                }
            }
            return Rewards.ToArray();
        }

        public void IncreaseFriendshipProgress(byte Value)
        {
            FriendshipProgression += Value;
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
            }
            //
            LifeTime += TimeSpan.FromSeconds(Main.dayRate); //A frame = a second
            //
        }

        public TimeSpan TimeUntilBirthday()
        {
            TimeSpan CurrentTime = LifeTime.Duration();
            TimeSpan NextBirthdayTime = CurrentTime.Duration();
            NextBirthdayTime = NextBirthdayTime.Subtract(TimeSpan.FromDays(NextBirthdayTime.TotalDays % DaysToYears));
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
                T += Time.Days + " days";
                First = false;
            }
            if (Time.Hours > 0)
            {
                if (!First)
                    T += ", ";
                T += Time.Hours + " hours";
                First = false;
            }
            if (Time.Minutes > 0)
            {
                if (!First)
                    T += ", ";
                T += Time.Minutes + " minutes";
                First = false;
            }
            return T + ".";
        }

        public string GetAge()
        {
            int Age = Base.Age + (int)(LifeTime.TotalDays / DaysToYears); //365.25
            string TimeInfo = Age + " Years Old";
            return TimeInfo;
        }

        public string GetTime()
        {
            return Lang.LocalizedDuration(LifeTime, false, false);
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
                tag.Add("FriendLevelFlags" + i + "_" + UniqueID, FriendshipLevelFlags[i]);
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
                tag.Add("Buff_" + b + "_type_" + UniqueID, Buffs[b].ID);
                tag.Add("Buff_" + b + "_time_" + UniqueID, Buffs[b].Time);
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
            request.Save(tag, UniqueID);
            //request.Save(tag, UniqueID);
            tag.Add("ExistenceTime_" + UniqueID, LifeTime.TotalSeconds);
            tag.Add("SkinID_" + UniqueID, SkinID);
            tag.Add("OutfitID_" + UniqueID, OutfitID);
            tag.Add("Coins_" + UniqueID, (int)Coins - int.MaxValue);
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
            }
            if (ModVersion >= 52)
            {
                IsStarter = tag.GetBool("StarterFlag_" + UniqueID);
            }
            if (ModVersion >= 7)
            {
                FriendshipLevel = tag.GetByte("FriendshipLevel_" + UniqueID);
                FriendshipProgression = tag.GetByte("FriendshipProgress_" + UniqueID);
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
                    FriendshipLevelFlags[i] = tag.GetBool("FriendLevelFlags" + i + "_" + UniqueID);
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
                    Buffs.Add(new BuffData(tag.GetInt("Buff_" + b + "_type_" + UniqueID), tag.GetInt("Buff_" + b + "_time_" + UniqueID)));
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
            if (ModVersion >= 61)
            {
                request.Load(tag, ModVersion, UniqueID, this);
            }
            //if (ModVersion >= 8)
            //    request.Load(tag, ModVersion, UniqueID);
            if (ModVersion >= 15)
                LifeTime = TimeSpan.FromSeconds(tag.GetDouble("ExistenceTime_" + UniqueID));
            if (ModVersion >= 66)
            {
                SkinID = tag.GetByte("SkinID_" + UniqueID);
                OutfitID = tag.GetByte("OutfitID_" + UniqueID);
            }
            if (ModVersion >= 67)
            {
                Coins = (uint)(tag.GetInt("Coins_" + UniqueID) + int.MaxValue);
            }
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
                Text = Base.MountUnlockMessage + "\n[You can now mount on this companion]";
                MountMessageUnlocked = true;
                return true;
            }
            if (!ControlMessageUnlocked && FriendshipLevel >= Base.ControlUnlockLevel)
            {
                Text = Base.ControlUnlockMessage + "\n[You can now control this companion]";
                ControlMessageUnlocked = true;
                return true;
            }
            return false;
        }
    }
}
