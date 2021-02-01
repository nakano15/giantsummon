using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework;

namespace giantsummon
{
    public class PlayerMod : ModPlayer
    {
        public static bool ForceKill = false;
        public bool IsBuddiesMode { get { return BuddiesMode; } }
        private bool BuddiesMode = false;
        public GuardianID BuddiesModeBuddyID = null;
        public EyeState eye = EyeState.Open;
        public Dictionary<int, GuardianData> MyGuardians = new Dictionary<int, GuardianData>();
        public TerraGuardian Guardian = new TerraGuardian();
        public TerraGuardian[] AssistGuardians = new TerraGuardian[MainMod.MaxExtraGuardianFollowers];
        public TerraGuardian[] GetAllGuardianFollowers
        {
            get
            {
                List<TerraGuardian> Guardians = new List<TerraGuardian>();
                Guardians.Add(Guardian);
                Guardians.AddRange(AssistGuardians);
                return Guardians.ToArray();
            }
        }
        public TerraGuardian MountGuardian
        {
            get
            {
                TerraGuardian g = null;
                foreach (TerraGuardian guardian in GetAllGuardianFollowers)
                {
                    if (guardian.Active && guardian.PlayerMounted && !guardian.Base.ReverseMount)
                    {
                        g = guardian;
                    }
                }
                return g;
            }
        }
        public bool KnockedOut = false, KnockedOutCold = false, FriendlyDuelDefeat = false;
        public short RescueTime = 0;
        public const int MaxRescueTime = 10;
        public bool RescueWakingUp = false;
        public int SelectedGuardian = -1;
        public int[] SelectedAssistGuardians = new int[MainMod.MaxExtraGuardianFollowers];
        public int LastHealthValue = -1;
        public byte LifeCrystalsUsed = 0, LifeFruitsUsed = 0, ManaCrystalsUsed = 0;
        public int ExtraMaxHealthValue = 0;
        public bool InEtherRealm = false;
        public BehaviorChanges CurrentTactic = BehaviorChanges.FreeWill;
        public bool MountedOnGuardian
        {
            get
            {
                TerraGuardian[] Guardians = GetAllGuardianFollowers;
                foreach (TerraGuardian g in Guardians)
                {
                    if (g.Active && g.PlayerMounted && !g.Base.ReverseMount)
                        return true;
                }
                return false;
            }
        }
        public bool GuardianMountingOnPlayer
        {
            get
            {
                TerraGuardian[] Guardians = GetAllGuardianFollowers;
                foreach (TerraGuardian g in Guardians)
                {
                    if (g.Active && g.PlayerMounted && g.Base.ReverseMount)
                        return true;
                }
                return false;
            }
        }
        public bool ControllingGuardian
        {
            get
            {
                return Guardian.Active && Guardian.PlayerControl;
            }
        }
        public bool BeingGrabbedByGuardian
        {
            get
            {
                TerraGuardian[] Guardians = GetAllGuardianFollowers;
                foreach (TerraGuardian g in Guardians)
                {
                    if (g.Active && g.GrabbingPlayer)
                        return true;
                }
                return false;
            }
        }
        public byte TitanGuardian = 255;
        public bool HasFirstSymbol = false;
        public byte MountSitOrder = 0, FollowBackOrder = 0, FollowFrontOrder = 0;
        public int MaxExtraGuardiansAllowed
        {
            get
            {
                int Count = FriendshipLevel;
                int Allowance = 0;
                if (Count >= 2)
                    Allowance++;
                if (Count >= 6) //5
                    Allowance++;
                if (Count >= 12) //8
                    Allowance++;
                if (Count >= 18) //10
                    Allowance++;
                if (Count >= 25) //14
                    Allowance++;
                if (Allowance > MainMod.MaxExtraGuardianFollowers)
                    Allowance = MainMod.MaxExtraGuardianFollowers;
                return Allowance;
            }
        }

        public void FriendshipLevelNotification()
        {
            Main.NewText("You reached Friendship Rank " + FriendshipLevel + ".");
            if (!BuddiesMode)
            {
                if (FriendshipLevel == 2 || FriendshipLevel == 6 || FriendshipLevel == 12 || FriendshipLevel == 18 || FriendshipLevel == 25)
                {
                    Main.NewText("You can now have " + (MaxExtraGuardiansAllowed + 1) + " companions following you.");
                }
            }
        }
        public int GetSummonedGuardianCount
        {
            get
            {
                int c = (SelectedGuardian != -1 ? 1 : 0);
                for (int g = 0; g < MainMod.MaxExtraGuardianFollowers; g++)
                {
                    if (SelectedAssistGuardians[g] > -1)
                        c++;
                }
                return c;
            }
        }
        public byte ReviveBoost = 0;
        public bool NegativeReviveBoost = false;
        public BitsByte TutorialFlags = new BitsByte();
        public bool TutorialCompanionIntroduction { get { return TutorialFlags[0]; } set { TutorialFlags[0] = value; } }
        public bool TutorialOrderIntroduction { get { return TutorialFlags[1]; } set { TutorialFlags[1] = value; } }
        public bool TutorialVanityIntroduction { get { return TutorialFlags[2]; } set { TutorialFlags[2] = value; } }
        public bool TutorialRequestIntroduction { get { return TutorialFlags[3]; } set { TutorialFlags[3] = value; } }
        public bool TutorialKnockOutIntroduction { get { return TutorialFlags[4]; } set { TutorialFlags[4] = value; } }
        public bool TutorialStatusIncreaseItemIntroduction { get { return TutorialFlags[5]; } set { TutorialFlags[5] = value; } }
        public byte FriendshipLevel = 0, FriendshipExp = 0;
        public byte FriendshipMaxExp { get { return FriendshipLevel == 0 ? (byte)2 : (byte)(3 + FriendshipLevel / 5); } }
        public int LastFriendshipCount = -1;
        public int GetAcceptedRequestCount { get { return GetGuardians().Where(x => !x.Base.InvalidGuardian && x.request.requestState == RequestData.RequestState.RequestActive).Count(); } }
        public bool[] PigGuardianCloudForm = new bool[5]; //Must be saved with the player. Last one is for the big form.
        public bool TalkedToLeopoldAboutThePigs
        {
            get
            {
                return
                    (HasGuardian(GuardianBase.Wrath) && GetGuardian(GuardianBase.Wrath).request.RequestsCompletedIDs.Contains(0)); //Add here the checking if requests giving ideas on how to solidify the pigs are complete.
            }
        }
        public int TalkingGuardianPosition = 0;
        public bool IsTalkingToAGuardian = false;
        public byte TerraGuardiansNearby = 0;
        public float DamageMod = 1f;
        public bool HasGhostFoxHauntDebuff
        {
            get
            {
                return player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SkullHaunt>()) ||
                    player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.BeeHaunt>()) ||
                    player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.MeatHaunt>()) ||
                    player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.SawHaunt>()) ||
                    player.HasBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.ConstructHaunt>());
            }
        }
        private string Nickname = null;
        public string GetNickname { get { if (Nickname == null) return "Terrarian"; return Nickname; } set { Nickname = value; } }
        public bool ReceivedFoodFromMinerva = false;

        public static int GetPlayerDefenseCount(Player player)
        {
            int Def = 0;
            for (int i = 0; i < 3; i++)
            {
                Def += player.armor[i].defense;
            }
            return Def;
        }

        public static bool IsInASafePlace(Player player, bool ShowWarnMessages = false)
        {
            int PlayerX = (int)player.Center.X / 16, PlayerY = (int)player.Center.Y / 16;
            Tile CenterTile = Framing.GetTileSafely(PlayerX, PlayerY);
            if (CenterTile == null || !Main.wallHouse[CenterTile.wall])
            {
                if(ShowWarnMessages) Main.NewText("You are not in a house.");
                return false;
            }
            if (!WorldGen.StartRoomCheck(PlayerX, PlayerY))
            {
                if (ShowWarnMessages) Main.NewText("This is not a safe house.");
                return false;
            }
            return true;
        }

        public static int GetPlayerAcceptedRequestCount(Player player)
        {
            return player.GetModPlayer<PlayerMod>().GetAcceptedRequestCount;
        }

        public void RecalculateFriendshipLevel()
        {
            FriendshipLevel = 0;
            FriendshipExp = 0;
            int ExpSum = 0;
            foreach (GuardianData gd in MyGuardians.Values)
            {
                ExpSum += gd.FriendshipLevel;
            }
            LastFriendshipCount = ExpSum;
            while (ExpSum >= FriendshipMaxExp && FriendshipLevel < 255)
            {
                ExpSum -= FriendshipMaxExp;
                FriendshipLevel++;
            }
            if (FriendshipLevel < 255)
            {
                FriendshipExp = (byte)ExpSum;
            }
        }

        public static bool PlayerControllingGuardian(Player player)
        {
            return player.GetModPlayer<PlayerMod>().ControllingGuardian;
        }

        public static bool PlayerMountedOnGuardian(Player player)
        {
            return player.GetModPlayer<PlayerMod>().MountedOnGuardian;
        }

        public static bool PlayerWithGuardianMounted(Player player)
        {
            return player.GetModPlayer<PlayerMod>().GuardianMountingOnPlayer;
        }

        public static bool ControlledIsTerraGuardian(Player player)
        {
            return player.GetModPlayer<PlayerMod>().ControllingGuardian && player.GetModPlayer<PlayerMod>().Guardian.Active && player.GetModPlayer<PlayerMod>().Guardian.PlayerControl;
        }

        public PlayerMod()
        {
            for (int g = 0; g < MainMod.MaxExtraGuardianFollowers; g++)
            {
                AssistGuardians[g] = new TerraGuardian();
                SelectedAssistGuardians[g] = -1;
            }
            for (int i = 0; i < 5; i++)
            {
                PigGuardianCloudForm[i] = true;
            }
        }

        public bool SetBuddyMode(int BuddyID, string BuddyModID = "")
        {
            if (BuddyModID == "")
                BuddyModID = mod.Name;
            GuardianBase gb = GuardianBase.GetGuardianBase(BuddyID, BuddyModID);
            if (gb.InvalidGuardian)
                return false;
            if (!HasGuardian(BuddyID, BuddyModID))
            {
                AddNewGuardian(BuddyID, BuddyModID);
            }
            for (byte g = 0; g < GetAllGuardianFollowers.Length; g++)
            {
                if (GetAllGuardianFollowers[g].Active)
                {
                    DismissGuardian(g);
                }
            }
            if (!HasGuardianSummoned(player, BuddyID, BuddyModID))
            {
                CallGuardian(BuddyID, BuddyModID, 0);
            }
            if (Guardian.Base.GetSpecialMessage(GuardianBase.MessageIDs.BuddySelected) != "")
            {
                Guardian.SaySomething(Guardian.Base.GetSpecialMessage(GuardianBase.MessageIDs.BuddySelected), true);
            }
            BuddiesModeBuddyID = new GuardianID(BuddyID, BuddyModID);
            BuddiesMode = true;
            int PortraitID = ModContent.ItemType<Items.Consumable.PortraitOfAFriend>();
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i].type == PortraitID)
                {
                    player.inventory[i].SetDefaults(0);
                }
            }
            if (Main.myPlayer == player.whoAmI && Main.mouseItem.type == PortraitID)
                Main.mouseItem.SetDefaults(0);
            if(player.whoAmI == Main.myPlayer)
                Main.NewText("Buddy mode activated.");
            return true;
        }

        public bool IsPlayerBuddy(GuardianID id)
        {
            if (BuddiesModeBuddyID != null)
            {
                return id == BuddiesModeBuddyID;
            }
            return false;
        }

        public static bool HasBuddiesModeOn(Player player)
        {
            return player.GetModPlayer<PlayerMod>().BuddiesMode;
        }

        public override bool CloneNewInstances
        {
            get
            {
                return false;
            }
        }

        public void ShareHealthReplenishWithGuardians(int Value)
        {
            foreach (TerraGuardian g in GetAllGuardianFollowers)
            {
                if (g.Active && !g.Downed && !g.KnockedOutCold)
                {
                    g.RestoreHP((int)(Value * g.HealthHealMult));
                }
            }
        }

        public void ShareManaReplenishWithGuardians(int Value)
        {
            foreach (TerraGuardian g in GetAllGuardianFollowers)
            {
                if (g.Active && !g.Downed && !g.KnockedOutCold)
                {
                    g.RestoreMP((int)(Value * g.ManaHealMult), false);
                }
            }
        }

        public override void ResetEffects()
        {
            //player.height = Player.defaultHeight;
            HasFirstSymbol = false;
            if (MainMod.SharedCrystalValues)
            {
                byte llc = LifeCrystalsUsed, llf = LifeFruitsUsed, lmc = ManaCrystalsUsed;
                LifeCrystalsUsed = LifeFruitsUsed = ManaCrystalsUsed = 0;
                if (player.statLifeMax >= 400)
                    LifeFruitsUsed = (byte)((player.statLifeMax - 400) / 5);
                if (player.statLifeMax > 100)
                {
                    int crystals = (player.statLifeMax - 100) / 20;
                    if (crystals > 15)
                        crystals = 15;
                    LifeCrystalsUsed = (byte)crystals;
                }
                if (player.statManaMax > 20)
                {
                    ManaCrystalsUsed = (byte)((player.statManaMax - 20) / 20);
                    if (ManaCrystalsUsed > 9)
                        ManaCrystalsUsed = 9;
                }
                if (Guardian.Active && (llc != LifeCrystalsUsed || llf != LifeFruitsUsed || lmc != ManaCrystalsUsed))
                {
                    Guardian.UpdateStatus = true;
                }
            }
            int LastExtraMaxHealth = ExtraMaxHealthValue;
            ExtraMaxHealthValue = 0;
            if (player.statLifeMax > 500)
                ExtraMaxHealthValue = player.statLifeMax - 500;
            if (ExtraMaxHealthValue != LastExtraMaxHealth)
            {
                foreach (TerraGuardian g in GetAllGuardianFollowers)
                {
                    if (g.Active)
                        g.UpdateStatus = true;
                }
            }
            if (KnockedOut)
            {
                if (!NegativeReviveBoost)
                {
                    player.lifeRegen += ReviveBoost * 5;
                }
                if ((ReviveBoost == 0 || NegativeReviveBoost) && KnockedOutCold)
                {
                    player.lifeRegenTime = 0;
                    player.lifeRegenCount = 0;
                    player.bleed = true;
                }
                for (int b = 0; b < player.buffType.Length; b++)
                {
                    if (player.buffType[b] > 0 && player.buffTime[b] > 0)
                    {
                        if (player.buffType[b] == ModContent.BuffType<Buffs.Injury>() || player.buffType[b] == ModContent.BuffType<Buffs.HeavyInjury>())
                        {
                            player.buffTime[b]++;
                        }
                        else if (ReviveBoost > 0 && !NegativeReviveBoost && player.buffType[b] != Terraria.ID.BuffID.PotionSickness && Main.debuff[player.buffType[b]])
                        {
                            player.buffTime[b] -= ReviveBoost;
                            if (player.buffTime[b] <= 0)
                            {
                                player.buffTime[b] = player.buffType[b] = 0;
                            }
                        }
                    }
                }
                player.aggro -= 50;
                if (player.potionDelayTime <= 5)
                    player.potionDelayTime = 5;
                //player.height = player.width;
                //player.position.Y += Player.defaultHeight - player.width;
            }
            if(!NegativeReviveBoost)
                ReviveBoost = 0;
            NegativeReviveBoost = false;
            if (BuddiesMode && Guardian.Active)
            {
                float HealthMod, DamageMod;
                int DefenseMod;
                Guardian.GetBuddyModeBenefits(out HealthMod, out DamageMod, out DefenseMod);
                player.statLifeMax2 += (int)(player.statLifeMax * HealthMod);
                player.meleeDamage += DamageMod;
                player.rangedDamage += DamageMod;
                player.magicDamage += DamageMod;
                player.thrownDamage += DamageMod;
                player.minionDamage += DamageMod;
                player.statDefense += DefenseMod;
            }
        }

        public override void NaturalLifeRegen(ref float regen)
        {
            if (KnockedOutCold)
            {
                if (ReviveBoost == 0 || NegativeReviveBoost)
                    player.bleed = true;
                else
                    regen *= 2;
            }
        }

        public static bool AddPlayerGuardian(Player player, int ID, Mod mod)
        {
            return AddPlayerGuardian(player, ID, mod.Name);
        }

        public static bool AddPlayerGuardian(Player player, int ID, string ModID = "")
        {
            return player.GetModPlayer<PlayerMod>().AddNewGuardian(ID, ModID);
        }

        public static bool PlayerHasGuardian(Player player, int ID, Mod mod)
        {
            return PlayerHasGuardian(player, ID, mod.Name);
        }

        public static bool PlayerHasGuardian(Player player, int ID, string ModID = "")
        {
            return player.GetModPlayer<PlayerMod>().HasGuardian(ID, ModID);
        }

        public static bool PlayerHasGuardianSummoned(Player player, int ID, Mod mod)
        {
            return PlayerHasGuardianSummoned(player, ID, mod.Name);
        }

        public static bool PlayerHasGuardianSummoned(Player player, int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers.Any(x => x.Active && x.ID == ID && x.ModID == ModID);
        }

        public bool HasGuardian(int ID, Mod mod)
        {
            return HasGuardian(ID, mod.Name);
        }

        public bool HasGuardian(int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            int[] Keys = MyGuardians.Keys.ToArray();
            foreach (int k in Keys)
            {
                if (MyGuardians[k].ID == ID && MyGuardians[k].ModID == ModID)
                    return true;
            }
            return false;
        }

        public override void PostUpdateBuffs()
        {
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active)
                {
                    guardian.ShareBuffsWithPlayer();
                    guardian.CheckForInformationAccs(player);
                    if (guardian.HasFlag(GuardianFlags.FriendshipBondBuff))
                    {
                        player.meleeDamage += 0.05f;
                        player.rangedDamage += 0.05f;
                        player.magicDamage += 0.05f;
                        player.minionDamage += 0.05f;
                        player.statDefense += 5;
                    }
                }
            }
            if (Guardian.MountedOnPlayer)
            {
                player.moveSpeed -= player.moveSpeed * Guardian.Base.MountBurdenPercentage;
            }
        }

        public override void PostUpdateEquips()
        {
            if (KnockedOut)
            {
                player.waterWalk = player.waterWalk2 = false;
            }
        }

        public static TerraGuardian GetPlayerSummonedGuardian(Player player, int ID, Mod mod)
        {
            return GetPlayerSummonedGuardian(player, ID, mod.Name);
        }

        public static TerraGuardian GetPlayerSummonedGuardian(Player player, int ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.mod.Name;
            foreach (TerraGuardian g in player.GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
            {
                if (g.ID == ID && g.ModID == ModID)
                    return g;
            }
            if (player.GetModPlayer<PlayerMod>().Guardian.MyID.IsSameID(ID, ModID))
            {
                return player.GetModPlayer<PlayerMod>().Guardian;
            }
            return null;
        }

        public static TerraGuardian GetPlayerMainGuardian(Player player)
        {
            return player.GetModPlayer<PlayerMod>().Guardian;
        }

        public static GuardianData GetPlayerGuardian(Player player, int ID, string ModID = "")
        {
            return player.GetModPlayer<PlayerMod>().GetGuardian(ID, ModID);
        }

        public static bool HasGuardianSummoned(Player player, int ID, Mod mod)
        {
            return HasGuardianSummoned(player, ID, mod.Name);
        }

        public static bool HasGuardianSummoned(Player player, int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            return PlayerHasGuardianSummoned(player, ID, ModID);
        }

        public static bool IsGuardianBirthday(Player player, int ID, string ModID = "")
        {
            if (ModID == "")
                ModID = MainMod.mod.Name;
            if (player.GetModPlayer<PlayerMod>().HasGuardian(ID, ModID))
            {
                return player.GetModPlayer<PlayerMod>().GetGuardian(ID, ModID).IsBirthday;
            }
            return false;
        }

        public static bool HasGuardianBeenGifted(Player player, int ID, string ModID = "")
        {
            if (player.GetModPlayer<PlayerMod>().HasGuardian(ID, ModID))
            {
                return player.GetModPlayer<PlayerMod>().GetGuardian(ID, ModID).GiftGiven;
            }
            return false;
        }

        public byte GetEmptyGuardianSlot()
        {
            if (!Guardian.Active)
                return 0;
            for (int i = 0; i < MainMod.MaxExtraGuardianFollowers; i++)
            {
                if (!AssistGuardians[i].Active)
                {
                    return (byte)(i + 1);
                }
            }
            return 255;
        }

        public byte GetGuardianSlot(int MyId)
        {
            if (SelectedGuardian == MyId)
                return 0;
            for (int i = 0; i < MainMod.MaxExtraGuardianFollowers; i++)
            {
                if (SelectedAssistGuardians[i] == MyId)
                {
                    return (byte)(i + 1);
                }
            }
            return 255;
        }

        public TerraGuardian GetGuardianFromSlot(byte AssistSlot)
        {
            if (AssistSlot == 0)
                return Guardian;
            return AssistGuardians[AssistSlot - 1];
        }

        public GuardianData GetGuardian(int ID, string ModID = "")
        {
            int[] Keys = MyGuardians.Keys.ToArray();
            if (ModID == "")
                ModID = MainMod.mod.Name;
            foreach (int k in Keys)
            {
                if (MyGuardians[k].ID == ID && MyGuardians[k].ModID == ModID)
                    return MyGuardians[k];
            }
            AddNewGuardian(ID, ModID);
            return GetGuardian(ID, ModID);
        }

        public GuardianData[] GetGuardians()
        {
            return MyGuardians.Values.ToArray();
        }

        public void EnterEtherRealm()
        {
            if (!Guardian.Active)
            {
                Main.NewText("You need to have a guardian summoned to enter it.");
                return;
            }
            if (Guardian.FriendshipLevel < Guardian.Base.ControlUnlockLevel)
            {
                Main.NewText("Not enough Friendship level to do that.");
                return;
            }
            InEtherRealm = true;
            if (!Guardian.PlayerControl)
                Guardian.TogglePlayerControl(true);
            Guardian.Position.X = Main.spawnTileX * 16;
            Guardian.Position.Y = Main.spawnTileY * 16;
            Guardian.UpdateStatus = true;
        }

        public void ExitEtherRealm()
        {
            InEtherRealm = false;
            Guardian.Position.X = Main.spawnTileX * 16;
            Guardian.Position.Y = Main.spawnTileY * 16;
            if (Guardian.PlayerControl)
                Guardian.TogglePlayerControl(true);
            Guardian.UpdateStatus = true;
        }

        public override void PostUpdate()
        {
            UpdateGuardian();
            for (int e = 0; e < 3; e++)
            {
                if (player.armor[e].type > 0 && player.armor[e].modItem is Items.GuardianItemPrefab)
                {
                    Item i = player.armor[e];
                    Item.NewItem(player.getRect(), i.type);
                    //player.GetItem(player.whoAmI, i);
                    player.armor[e].SetDefaults();
                    Main.NewText("This item doesn't fit on me.");
                }
                if (player.armor[e + 10].type > 0 && player.armor[e + 10].modItem is Items.GuardianItemPrefab)
                {
                    Item i = player.armor[e + 10];
                    Item.NewItem(player.getRect(), i.type);
                    //player.GetItem(player.whoAmI, i);
                    player.armor[e + 10].SetDefaults();
                    Main.NewText("This item doesn't fit on me.");
                }
            }
            LastHealthValue = player.statLife;
            if (KnockedOut)
            {
                UpdateKnockOut();
            }
            FriendlyDuelDefeat = false;
            if (player.whoAmI == Main.myPlayer)
            {
                GuardianMouseOverAndDialogueInterface.Update();
            }
            TerraGuardiansNearby = 0;
            Vector2 Center = player.Center;
            foreach (TerraGuardian g in WorldMod.GuardianTownNPC)
            {
                if (g.Position.X >= Center.X - NPC.sWidth * 0.5f && g.Position.X < Center.X + NPC.sWidth * 0.5f &&
                    g.CenterPosition.Y >= Center.Y - NPC.sHeight * 0.5f && g.CenterPosition.Y < Center.Y + NPC.sHeight * 0.5f)
                {
                    TerraGuardiansNearby++;
                    if(g.OwnerPos == -1)
                        player.townNPCs += g.Base.TownNpcSlot;
                }
            }
        }

        public void UpdateReviveSystem()
        {
            bool Reviving = player.controlUseItem && player.itemAnimation == 0;
            int SelectedOne = -1;
            bool SelectedIsGuardian = false;
            float MouseX = Main.mouseX + Main.screenPosition.X, MouseY = Main.mouseY + Main.screenPosition.Y;
            Rectangle rect = player.getRect();
            if (MountedOnGuardian)
            {
                foreach (TerraGuardian tg in GetAllGuardianFollowers)
                {
                    if (tg.Active && tg.PlayerMounted && !tg.Base.ReverseMount && !tg.HasFlag(GuardianFlags.CantReceiveHelpOnReviving))
                        rect = tg.HitBox;
                }
            }
            else if (ControllingGuardian)
            {
                rect = Guardian.HitBox;
            }
            for (int p = 0; p < 255; p++)
            {
                if (p != player.whoAmI && Main.player[p].active && !Main.player[p].dead && Main.player[p].GetModPlayer<PlayerMod>().KnockedOut &&
                    MouseX >= Main.player[p].position.X && MouseX < Main.player[p].position.X + Main.player[p].width &&
                    MouseY >= Main.player[p].position.Y + Main.player[p].height * 0.5f && MouseY < Main.player[p].position.Y + Main.player[p].height && 
                    rect.Intersects(Main.player[p].getRect()))
                {
                    SelectedOne = p;
                    SelectedIsGuardian = false;
                    break;
                }
            }
            if (SelectedOne == -1)
            {
                foreach (int key in MainMod.ActiveGuardians.Keys)
                {
                    TerraGuardian g = MainMod.ActiveGuardians[key];
                    if (!g.Downed && g.KnockedOut && !g.IsPlayerHostile(player) &&
                        MouseX >= g.Position.X - g.Width * 0.5f && MouseX < g.Position.X + g.Width * 0.5f &&
                        MouseY >= g.Position.Y - g.Height * 0.5f && MouseY < g.Position.Y && 
                        rect.Intersects(g.HitBox))
                    {
                        SelectedOne = key;
                        SelectedIsGuardian = true;
                        break;
                    }
                }
            }
            if (SelectedOne > -1)
            {
                MainMod.IsReviving = Reviving;
                if (Reviving)
                {
                    if (SelectedIsGuardian)
                    {
                        MainMod.ActiveGuardians[SelectedOne].ReviveBoost += 3;
                    }
                    else
                    {
                        Main.player[SelectedOne].GetModPlayer<PlayerMod>().ReviveBoost += 3;
                    }
                }
                MainMod.ToReviveID = SelectedOne;
                MainMod.ToReviveIsGuardian = SelectedIsGuardian;
                player.mouseInterface = true;
            }
            else
            {
                MainMod.ToReviveID = -1;
            }
        }

        public void UpdateKnockOut()
        {
            if (player.dead)
                return;
            if (KnockedOutCold)
            {
                if (player.breath <= 0 && !MainMod.PlayersDontDiesAfterDownedDefeat)
                {
                    bool Mermaid = player.head == Terraria.ID.ArmorIDs.Head.SeashellHairpin &&
                        player.body == Terraria.ID.ArmorIDs.Body.MermaidAdornment &&
                        player.legs == Terraria.ID.ArmorIDs.Legs.MermaidTail;
                    DoForceKill((Mermaid ? " drowned?" : (Main.rand.Next(2) == 0 ? " suffocated" : " ran out of air.")));
                }
                else if (player.lavaWet && !player.lavaImmune)
                {
                    DoForceKill(" has turned into ash.");
                }
            }
            else
            {
                if (RescueTime > 0)
                {
                    RescueTime = 0;
                }
            }
            if (KnockedOut)
            {
                if (true || KnockedOutCold)
                    eye = EyeState.Closed;
                else
                    eye = EyeState.HalfOpen;
                if (player.potionDelayTime < 5)
                {
                    player.AddBuff(Terraria.ID.BuffID.PotionSickness, 5);
                    player.potionDelayTime = 5;
                }
                if (ReviveBoost > 0 && player.breath < player.breathMax - 1)
                {
                    player.breathCD += 1 + (ReviveBoost / 2);
                    player.breath++;
                    if (player.breathCD > 5)
                    {
                        player.breathCD -= 5;
                        player.breath++;
                    }
                }
            }
            if (player.grapCount > 0)
            {
                for (int i = 0; i < player.grapCount; i++)
                {
                    if (player.grappling[i] > -1)
                    {
                        Main.projectile[player.grappling[i]].active = false;
                        player.grappling[i] = 0;
                    }
                }
                player.grapCount = 0;
            }
            if (MountedOnGuardian)
            {
                if (!MountGuardian.IsAttackingSomething && MountGuardian.Velocity.X == 0)
                {
                    player.fullRotation = 1.570796326794897f * MountGuardian.Direction;
                    player.fullRotationOrigin.X = player.width * 0.5f;
                    player.fullRotationOrigin.Y = player.height * 0.5f;
                }
                else
                {
                    player.fullRotation = 0;
                    player.fullRotationOrigin.X = 0;
                    player.fullRotationOrigin.Y = 0;
                }
                ReviveBoost += 1;
            }
            else if (player.velocity.Y != 0)
            {
                player.fullRotation += player.velocity.X * 0.05f;
                player.fullRotationOrigin.X = player.width * 0.5f;
                player.fullRotationOrigin.Y = player.height * 0.5f;
            }
            else
            {
                if (true || KnockedOutCold)
                {
                    player.fullRotation = -1.570796326794897f * player.direction;
                    //player.headRotation = -1.570796326794897f * player.direction;
                    player.fullRotationOrigin.X = player.width * 0.5f;
                    player.fullRotationOrigin.Y = player.height * 0.5f + 12;
                    player.legRotation = 0f;
                }
                else
                {
                    player.fullRotation = 0;
                    player.legRotation = -1.570796326794897f * player.direction;
                    player.gfxOffY = 12;
                    player.legPosition.Y += 20 - (40 * player.direction);
                    player.legPosition.X -= 10 * player.direction;
                }
                /*if (!KnockedOutCold)
                {
                    player.bodyRotation = 1.570796326794897f * player.direction;
                    player.headRotation = 1.570796326794897f * player.direction;
                }*/
            }
            if (player.statLife >= player.statLifeMax2)
            {
                LeaveDownedState();
            }
            else if (player.statLife <= 0)
            {
                if (MainMod.PlayersDontDiesAfterDownedDefeat)
                {
                    if (player.statLife < 0 && !player.dead) player.statLife = 0;
                }
                else
                {
                    player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " blacked out."), 1, 0, false);
                }
            }
            if (player.tongued && Main.wof >= 0)
            {
                Vector2 WofCenter = Main.npc[Main.wof].Center;
                Vector2 MoveDirection = (WofCenter - player.Center);
                float Distance = MoveDirection.Length();
                MoveDirection.Normalize();
                MoveDirection *= 11f;
                player.position += MoveDirection;
                if (Distance < 11f * 2)
                {
                    foreach (TerraGuardian tg in GetAllGuardianFollowers)
                    {
                        if (tg.WofFood)
                        {
                            tg.DoForceKill(" died with " + player.name + ".");
                        }
                    }
                    DoForceKill(player.name + " was eaten by the Wall of Flesh.");
                    player.immuneAlpha = 255;
                }
            }
            if (!player.dead && KnockedOut && player.gross && Main.wof > -1)
            {
                float Distance = (Main.npc[Main.wof].Center - player.Center).Length();
                if (Distance >= 3000)
                    ForceKill = true;
            }
            if (KnockedOutCold)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    bool HasDPSDebuff = player.onFire || player.poisoned || player.suffocating;
                    if (MainMod.StartRescueCountdownWhenKnockedOutCold || HasDPSDebuff || !GetAllGuardianFollowers.Any(x => x.Active && !x.KnockedOutCold && !x.Downed) || player.controlHook)
                    {
                        if (ReviveBoost == 0 || player.controlHook || HasDPSDebuff)
                            RescueTime++;
                        else if (RescueTime > 0)
                            RescueTime--;
                        if (RescueTime >= MaxRescueTime * 60)
                        {
                            RescueTime = 0;
                            RescueWakingUp = true;
                            if (MainMod.DoNotUseRescue)
                            {
                                DoForceKill(" succumbed to It's injuries.");
                            }
                            else
                            {
                                KnockedOutCold = false;
                                KnockedOut = true;
                                TerraGuardian rescuer = null;
                                bool RescuerIsHomeless = true;
                                float NearestRescuerDistance = float.MaxValue;
                                foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
                                {
                                    if (tg.OwnerPos == -1 && !tg.DoAction.InUse && !tg.Downed && !tg.KnockedOut && tg.GetTownNpcInfo != null)
                                    {
                                        float Distance = player.Distance(tg.CenterPosition);
                                        bool IsHomeless = tg.GetTownNpcInfo.Homeless;
                                        if (Distance < NearestRescuerDistance)
                                        {
                                            if ((RescuerIsHomeless && IsHomeless) || !IsHomeless)
                                            {
                                                rescuer = tg;
                                                NearestRescuerDistance = Distance;
                                                RescuerIsHomeless = IsHomeless;
                                            }
                                        }
                                    }
                                }
                                if (rescuer != null)
                                {
                                    string RescueMessage = rescuer.Base.GetSpecialMessage(GuardianBase.MessageIDs.RescueMessage);
                                    rescuer.Spawn();
                                    if (rescuer.furniturex > -1)
                                        rescuer.LeaveFurniture(false);
                                    if (RescueMessage != "")
                                        rescuer.SaySomething(RescueMessage, true);
                                    player.position = rescuer.Position;
                                    player.position.X -= player.width * 0.5f;
                                    player.position.Y -= player.height + 8;
                                    GuardianActions.TryRevivingPlayer(rescuer, player);
                                }
                                else
                                {
                                    player.Spawn();
                                }
                                const float DistanceToTeleportNearbyGuardians = 336f;
                                foreach (TerraGuardian tg in WorldMod.GuardianTownNPC)
                                {
                                    if (tg != rescuer && tg.OwnerPos == -1 && !tg.Downed && !tg.KnockedOut && tg.Distance(player.Center) < DistanceToTeleportNearbyGuardians)
                                    {
                                        if (tg.furniturex > -1)
                                            tg.LeaveFurniture();
                                        tg.Position = rescuer.Position;
                                        tg.SetFallStart();
                                    }
                                }
                                if (MountedOnGuardian)
                                {
                                    Guardian.TeleportToPlayer(true, player);
                                }
                                for (int b = player.buffType.Length - 1; b >= 0; b--)
                                {
                                    if (player.buffTime[b] > 0 && Main.debuff[player.buffType[b]] && player.buffType[b] != Terraria.ID.BuffID.PotionSickness)
                                    {
                                        player.DelBuff(b);
                                    }
                                }
                                //player.statLife = (int)(player.statLifeMax2 * (Main.bloodMoon || Main.eclipse || Main.pumpkinMoon || Main.snowMoon || Main.invasionProgress > 0 ? 0.8f : 0.5f));
                                player.statLife = 1;
                                //player.lifeRegen = 100;
                                //Send to some guardian house and then move It in.
                                //If the above isn't possible, send to spawn, and place some guardian nearby.
                                List<Point> ValidPositions = new List<Point>();
                                bool BlockedLeft = false, BlockedRight = false;
                                byte Tries = 0;
                                while (ValidPositions.Count < GetAllGuardianFollowers.Count(x => x.Active) && Tries++ < 10)
                                {
                                    for (int d = 0; d < 8; d++)
                                    {
                                        for (int dir = -1; dir < 2; dir += 2)
                                        {
                                            if ((dir == -1 && BlockedLeft) || (dir == 1 && BlockedRight))
                                            {
                                                continue;
                                            }
                                            Point point = new Point((int)player.position.X / 16 + dir * d, (int)player.position.Y / 16);
                                            byte MoveDist = 0;
                                            while (true)
                                            {
                                                bool Blocked = false;
                                                for (int x = 0; x < 2; x++)
                                                {
                                                    Tile t = Framing.GetTileSafely(point.X + x, point.Y + 2);
                                                    if (t.active() && Main.tileSolid[t.type])
                                                    {
                                                        if (MoveDist == 2)
                                                        {
                                                            if (dir < 0)
                                                                BlockedLeft = true;
                                                            else
                                                                BlockedRight = true;
                                                        }
                                                        point.Y--;
                                                        MoveDist++;
                                                        Blocked = true;
                                                        break;
                                                    }
                                                }
                                                if (!Blocked || MoveDist == 2)
                                                    break;
                                            }
                                            if (MoveDist == 0)
                                            {
                                                while (true)
                                                {
                                                    bool Blocked = false;
                                                    for (int x = 0; x < 2; x++)
                                                    {
                                                        Tile t = Framing.GetTileSafely(point.X + x, point.Y + 2 + 1);
                                                        if (t.active() && Main.tileSolid[t.type])
                                                        {
                                                            if (MoveDist == 2)
                                                            {
                                                                if (dir < 0)
                                                                    BlockedLeft = true;
                                                                else
                                                                    BlockedRight = true;
                                                            }
                                                            point.Y++;
                                                            MoveDist++;
                                                            Blocked = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!Blocked || MoveDist == 2)
                                                        break;
                                                }
                                            }
                                            bool HasTileBlocking = false;
                                            for (int y = 0; y < 3; y++)
                                            {
                                                for (int x = 0; x < 2; x++)
                                                {
                                                    if (HasTileBlocking)
                                                        break;
                                                    Tile t = Framing.GetTileSafely(point.X + x, point.Y + y);
                                                    if (t.active() && Main.tileSolid[t.type])
                                                    {
                                                        HasTileBlocking = true;
                                                    }
                                                }
                                            }
                                            if (!HasTileBlocking)
                                            {
                                                point.X++;
                                                ValidPositions.Add(point);
                                            }
                                        }
                                    }
                                }
                                foreach (TerraGuardian tg in GetAllGuardianFollowers)
                                {
                                    if (tg.Active)
                                    {
                                        if (tg.Downed)
                                            tg.Spawn();
                                        if ((tg.KnockedOut || tg.KnockedOutCold))
                                        {
                                            tg.TeleportToPlayer();
                                            if (ValidPositions.Count > 0)
                                            {
                                                Vector2 Position = new Vector2(ValidPositions[0].X * 16, ValidPositions[0].Y * 16 + 48 - 1);
                                                ValidPositions.RemoveAt(0);
                                                tg.Position = Position;
                                                tg.SetFallStart();
                                            }
                                            tg.KnockedOutCold = false;
                                            tg.HP = (int)(1 + Main.rand.Next(tg.MHP - 1));
                                            //tg.ExitDownedState();
                                        }
                                    }
                                }
                                if (player.whoAmI == Main.myPlayer)
                                {
                                    Main.BlackFadeIn = 255;
                                    Main.renderNow = true;
                                    /*if (Main.netMode == 0)
                                    {
                                        player.active = false;
                                        for (int n = 0; n < 200; n++)
                                        {
                                            if (Main.npc[n].active && !Main.npc[n].townNPC && Main.npc[n].type != Terraria.ID.NPCID.LunarTowerNebula
                                                 && Main.npc[n].type != Terraria.ID.NPCID.LunarTowerSolar && Main.npc[n].type != Terraria.ID.NPCID.LunarTowerStardust
                                                 && Main.npc[n].type != Terraria.ID.NPCID.LunarTowerVortex && Main.npc[n].type != Terraria.ID.NPCID.CultistTablet)
                                            {
                                                Main.npc[n].timeLeft = 0;
                                                Main.npc[n].CheckActive();
                                            }
                                        }
                                        player.active = true;
                                    }*/
                                }
                                if (Main.netMode == 0)
                                {
                                    WorldMod.SkipTime(Main.rand.Next(10, 36) * 0.1f);
                                    /*Main.time += Main.rand.Next(10, 35) * 0.1f * 3600;
                                    if (Main.dayTime)
                                    {
                                        if (Main.time >= 15 * 3600)
                                        {
                                            Main.time -= 15 * 3600;
                                            Main.dayTime = false;
                                        }
                                    }
                                    else
                                    {
                                        if (Main.time >= 9 * 3600)
                                        {
                                            Main.time -= 9 * 3600;
                                            Main.dayTime = true;
                                            Main.AnglerQuestSwap();
                                        }
                                    }*/
                                }
                            }
                        }
                    }
                    if (RescueTime > 0 && !HasDPSDebuff && !player.controlHook && ReviveBoost > 0)
                        RescueTime = 0;
                }
            }
        }

        public override void FrameEffects()
        {
            if (MountedOnGuardian)
            {
                player.legFrame.Y = 0;
                player.headFrame.Y = 0;
                if (player.itemAnimation == 0)
                    player.bodyFrame.Y = 0;
                player.velocity = Microsoft.Xna.Framework.Vector2.Zero;
            }
        }

        public override void PreUpdate()
        {
            eye = EyeState.Open;
            if (player.whoAmI == Main.myPlayer && HasGhostFoxHauntDebuff)
            {
                bool ReduceOpacity = Main.dayTime && !Main.eclipse && player.position.Y < Main.worldSurface * 16 && Main.tile[(int)(player.Center.X) / 16, (int)(player.Center.Y / 16)].wall == 0;
                if (player.dead || ReduceOpacity)
                {
                    if (MainMod.FlufflesHauntOpacity > 0)
                    {
                        MainMod.FlufflesHauntOpacity -= 0.005f;
                        if (MainMod.FlufflesHauntOpacity < 0)
                            MainMod.FlufflesHauntOpacity = 0;
                    }
                }
                else
                {
                    if (MainMod.FlufflesHauntOpacity < 1)
                    {
                        MainMod.FlufflesHauntOpacity += 0.005f;
                        if (MainMod.FlufflesHauntOpacity > 1)
                            MainMod.FlufflesHauntOpacity = 1;
                    }
                }
            }
        }

        public override void PreUpdateMovement()
        {
        }

        public override void UpdateDead()
        {
            UpdateGuardian();
            LastHealthValue = -1;
            if (KnockedOutCold)
            {
                player.fullRotation = 0f;
                player.fullRotationOrigin.X = 0;
                player.fullRotationOrigin.Y = 0;
            }
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (damage > 0 && player.statLife > 0) TriggerHandler.FirePlayerHurtTrigger(player.Center, player, (int)damage, crit, pvp);
            if (player.statLife > 0)
                FriendlyDuelDefeat = false;
        }

        public override void ModifyNursePrice(NPC nurse, int health, bool removeDebuffs, ref int price)
        {
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active)
                {
                    if (!guardian.Downed)
                    {
                        float PriceValue = (guardian.MHP - guardian.HP) * guardian.HealthHealMult;
                        //PriceValue *= 1f + guardian.LifeCrystalHealth * 0.5f + guardian.LifeFruitHealth * 0.01f;
                        price += (int)(PriceValue);
                    }
                    else
                    {
                        price += 1000 + guardian.LifeCrystalHealth * guardian.Base.LifeCrystalHPBonus * 50 + guardian.LifeFruitHealth * guardian.Base.LifeFruitHPBonus * 1000;
                    }
                }
            }
        }

        public override void PostNurseHeal(NPC nurse, int health, bool removeDebuffs, int price)
        {
            //float HealthDif = (float)(player.player.statLife - player.LastHealthValue) / player.player.statLifeMax2;
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active)
                {
                    if (!guardian.Downed)
                    {
                        guardian.RestoreHP((int)(guardian.MHP - guardian.HP));
                    }
                    else
                    {
                        guardian.Spawn();
                        guardian.RestoreHP(guardian.MHP);
                    }
                }
            }
        }

        public override void ProcessTriggers(Terraria.GameInput.TriggersSet triggersSet)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                MainMod.OrderCallButtonPressed = MainMod.orderCallButton.Current;
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref Terraria.DataStructures.PlayerDeathReason damageSource)
        {
            if (KnockedOutCold)
            {
                damage = 0;
                return false;
            }
            if (Guardian.Active && Guardian.ProtectingPlayerFromHarm)
            {
                damage = 0;
                return false;
            }
            if (player.HasBuff(ModContent.BuffType<Buffs.Obstruction>()))
            {
                damage = 0;
                player.immuneTime = 5;
                return false;
            }
            //if (player.immune || player.immuneTime > 0)
            //    return false;
            if (Guardian.PlayerControl)
                return false;
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (player.immuneTime <= 0 && guardian.Active && damage < player.statLifeMax2 && guardian.TriggerCover((int)damage, hitDirection))
                {
                    player.immuneTime = 60;
                    //player.immune = true;
                    playSound = false;
                    genGore = false;
                    return false;
                }
            }
            if (damage > 1 && Guardian.Active && Guardian.HasFlag(GuardianFlags.MountDamageReceivedReduction))
            {
                damage = (int)(damage * 0.67f);
            }
            if(damage > 1 && KnockedOut)
            {
                damage -= (int)(damage * 0.5f);
                if (damage < 1)
                    damage = 1;
            }
            return true;
        }

        public void EnterDownedState(bool Friendly = false)
        {
            KnockedOut = true;
            player.statLife += player.statLifeMax2 / 2;
            if (player.mount.Active)
                player.mount.Dismount(player);
            player.pulley = false;
            if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialKnockOutIntroduction)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    Main.NewText("You got knocked out! You will need to get the help of companions or other players to be revived. You wont be able to do anything while in this state.");
                    if (MainMod.PlayersDontDiesAfterDownedDefeat)
                        Main.NewText("You can still be hurt while in this state, but when health reaches 0, you will enter knocked out cold state. You wont be attacked by anything anymore, but health stops regenerating without the help of companions. If they are unable to revive you, you can force your own death by pressing '" + Main.cHook + "' key.");
                    else
                        Main.NewText("You can still be hurt while in this state, but your aggro is lower. If your character health reaches 0, your character dies.");
                }
                else
                {
                    Main.NewText("Someone got knocked out! Companions that aren't attacking enemies can help revive the one knocked out if they are in range. You can resurrect too, by standing on the position and holding the left mouse button on the character.");
                }
                Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialKnockOutIntroduction = true;
            }
        }

        public void LeaveDownedState()
        {
            RescueWakingUp = false;
            KnockedOut = KnockedOutCold = false;
            player.fullRotation = player.headRotation = player.bodyRotation = player.legRotation = 0f;
            float HealthRestoreValue = 1f;
            if (player.HasBuff(ModContent.BuffType<Buffs.HeavyInjury>()))
            {
                player.AddBuff(ModContent.BuffType<Buffs.HeavyInjury>(), 600 * 60);
                HealthRestoreValue = 0.5f;
            }
            else if (player.HasBuff(ModContent.BuffType<Buffs.Injury>()))
            {
                player.DelBuff(player.FindBuffIndex(ModContent.BuffType<Buffs.Injury>()));
                player.AddBuff(ModContent.BuffType<Buffs.HeavyInjury>(), 300 * 60);
                HealthRestoreValue = 0.75f;
            }
            else
            {
                player.AddBuff(ModContent.BuffType<Buffs.Injury>(), 90 * 60);
            }
            player.statLife = (int)((player.statLifeMax2 / 2 + ReviveBoost * 10) * HealthRestoreValue);
            CombatText.NewText(player.getRect(), Color.Cyan, "Revived!", true);
            player.immuneTime = (player.longInvince ? 120 : 60) * 3;
            player.immune = true;
        }

        public static bool IsPlayerDead(Player player)
        {
            return player.dead || player.GetModPlayer<PlayerMod>().KnockedOutCold;
        }

        public void DoForceKill(string DeathMessage = "")
        {
            if (DeathMessage == "")
                DeathMessage = player.name + " has died...";
            else
                DeathMessage = player.name + DeathMessage;
            ForceKill = true;
            player.statLife = 0;
            player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(DeathMessage), 1, 1, false);
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref Terraria.DataStructures.PlayerDeathReason damageSource)
        {
            if (ForceKill)
            {
                ForceKill = false;
                player.statLife = 0;
                KnockedOut = KnockedOutCold = false;
            }
            else if (FriendlyDuelDefeat || (MainMod.PlayersGetKnockedOutUponDefeat && !player.dead && !KnockedOut && !player.lavaWet && player.breath > 0 && (player.statLife + player.statLifeMax2 / 2 > 0 || MainMod.PlayersDontDiesAfterDownedDefeat)))
            {
                FriendlyDuelDefeat = false;
                EnterDownedState();
                TriggerHandler.FirePlayerDownedTrigger(player.Center, player, (int)damage, pvp);
                return false;
            }
            else if (MainMod.PlayersDontDiesAfterDownedDefeat && !player.dead && KnockedOut)
            {
                KnockedOutCold = true;
                player.statLife = 0;
                return false;
            }
            return true;
        }

        public override void Kill(double damage, int hitDirection, bool pvp, Terraria.DataStructures.PlayerDeathReason damageSource)
        {
            TriggerHandler.FirePlayerDeathTrigger(player.Center, player, (int)damage, pvp);
            if (Guardian.Active)
            {
                Guardian.WhenPlayerDies();
            }
        }

        public void UpdateGuardian()
        {
            DamageMod = 1f;
            if (player.whoAmI == Main.myPlayer)
            {
                int NewFriendshipCount = 0;
                foreach (GuardianData g in MyGuardians.Values)
                {
                    g.request.UpdateRequest(g, this);
                    g.UpdateData(this.player);
                    NewFriendshipCount += g.FriendshipLevel;
                }
                if (NewFriendshipCount != LastFriendshipCount)
                {
                    if (LastFriendshipCount == -1)
                    {
                        RecalculateFriendshipLevel();
                    }
                    else
                    {
                        byte LastFriendshipLevel = FriendshipLevel;
                        RecalculateFriendshipLevel();
                        if (FriendshipLevel > LastFriendshipLevel)
                        {
                            FriendshipLevelNotification();
                        }
                    }
                }
            }
            byte AssistSlot = 0;
            bool TeleportTrigger = (player.inventory[player.selectedItem].type == 50 || player.inventory[player.selectedItem].type == 3124 || player.inventory[player.selectedItem].type == 3199 || player.inventory[player.selectedItem].type == Terraria.ID.ItemID.RecallPotion || player.inventory[player.selectedItem].type == Terraria.ID.ItemID.TeleportationPotion) && player.itemAnimation > 0 && player.itemTime == player.inventory[player.selectedItem].useTime / 2;
            bool FoundFirstTitanGuardian = false;
            MountSitOrder = FollowFrontOrder = FollowBackOrder = 0;
            byte GuardianSlot = 0;
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (!guardian.Active)
                {
                    AssistSlot++;
                    continue;
                }
                if (!FoundFirstTitanGuardian && guardian.HasFlag(GuardianFlags.TitanGuardian))
                {
                    FoundFirstTitanGuardian = true;
                    TitanGuardian = AssistSlot;
                }
                if (GuardianSlot > MaxExtraGuardiansAllowed && TitanGuardian < 255 && TitanGuardian != AssistSlot)
                {
                    Main.NewText(guardian.Name + " were dismissed.");
                    DismissGuardian(AssistSlot);
                    AssistSlot++;
                    continue;
                }
                if (TeleportTrigger && !guardian.WofFacing)
                {
                    guardian.TeleportToPlayer(true);
                }
                if (player.HasBuff(Terraria.ID.BuffID.SoulDrain))
                {
                    guardian.AddBuff(Terraria.ID.BuffID.SoulDrain, 5);
                }
                guardian.AssistSlot = AssistSlot;
                guardian.Update(this.player);
                if (player.whoAmI == Main.myPlayer && guardian.Base.InvalidGuardian)
                {
                    this.DismissGuardian();
                    Main.NewText("Your guardian has been dismissed because the mod using it doesn't exists.");
                }
                if (guardian.GrabbingPlayer)
                {
                    if (guardian.Base.DontUseRightHand)
                    {
                        player.position = guardian.GetGuardianLeftHandPosition;
                    }
                    else
                    {
                        player.position = guardian.GetGuardianRightHandPosition;
                    }
                    player.position.X -= player.width * 0.5f;
                    player.position.Y -= player.height * 0.5f * player.gravDir;
                    player.gfxOffY = 0;
                    player.fallStart = player.fallStart2 = (int)player.position.Y / 16;
                    if (player.mount.Active)
                        player.mount.Dismount(player);
                    if (player.itemAnimation == 0)
                        player.direction = guardian.Direction * -1;
                    player.velocity = guardian.Velocity;
                    //player.position -= Guardian.Velocity;
                }
                else
                {
                    if (guardian.PlayerMounted && !guardian.Base.ReverseMount && !player.tongued)
                    {
                        if (player.mount.Active)
                            player.mount.Dismount(player);
                        player.velocity = Microsoft.Xna.Framework.Vector2.Zero;
                        player.velocity.Y = 0;
                        if (KnockedOut && !guardian.IsAttackingSomething && guardian.Velocity.X == 0)
                        {
                            player.position = guardian.PositionWithOffset;
                            player.position.X += player.width * 0.5f * guardian.Direction;
                            player.position.Y -= player.height - 12;
                            player.direction = -guardian.Direction;
                            player.gfxOffY = 0;
                            player.fallStart = player.fallStart2 = (int)player.position.Y / 16;
                        }
                        else
                        {
                            player.fullRotation = 0;
                            player.position = guardian.GetGuardianShoulderPosition;
                            player.position.X -= player.width * 0.5f;
                            player.position.Y -= (player.height * 0.5f) + 8; //Bugs out when gravity is reverse
                            player.gfxOffY = 0;
                            player.itemLocation += guardian.Velocity;
                            player.fallStart = player.fallStart2 = (int)player.position.Y / 16;
                            if (player.itemAnimation == 0 && player.direction != guardian.Direction)
                                player.direction = guardian.Direction;
                            if (player.stealth > 0 && guardian.Velocity.X * guardian.Direction > 0.1f)
                            {
                                player.stealth += 0.2f;
                                if (player.stealth > 1f)
                                    player.stealth = 1f;
                            }
                        }
                    }
                    if (guardian.PlayerControl)
                    {
                        player.gravDir = guardian.GravityDirection;
                        player.position.X = guardian.Position.X - player.width * 0.5f;
                        player.position.Y = guardian.Position.Y - player.height * player.gravDir - Player.defaultGravity;
                        player.gfxOffY = 0;
                        player.velocity = Microsoft.Xna.Framework.Vector2.Zero;
                        player.direction = guardian.Direction;
                        player.gills = true;
                        player.immuneTime = 5;
                        //player.immune = true;
                        if (player.mount.Active)
                            player.mount.Dismount(player);
                    }
                }
                //player.maxMinions += guardian.NumMinions;
                AssistSlot++;
                GuardianSlot++;
            }
            if (GuardianSlot > 0)
            {
                DamageMod = 0f;
                for (byte c = 0; c < GuardianSlot; c++)
                {
                    if (c == 0)
                    {
                        DamageMod = 0.1f;
                    }
                    else
                    {
                        DamageMod += DamageMod * 0.1f; //0.2f
                    }
                }
                DamageMod = 1f - DamageMod;
            }
            if (!FoundFirstTitanGuardian)
                TitanGuardian = 255;
            CheckIfLeaderHasBeenRemoved();
            /*if (Guardian.ProtectingPlayerFromHarm)
            {
                player.immuneTime = 5;
                //player.immuneNoBlink = true;
            }*/
        }

        public override void ModifyScreenPosition()
        {
            foreach (TerraGuardian Guardian in GetAllGuardianFollowers)
            {
                if (Guardian.Active && ((Guardian.PlayerMounted && !Guardian.Base.ReverseMount) || Guardian.PlayerControl || Guardian.GrabbingPlayer || (Guardian.DoAction.InUse && Guardian.DoAction.FocusCameraOnGuardian)))
                {
                    Main.screenPosition.X = (int)(Guardian.Position.X - (Main.screenWidth * 0.5f));
                    Main.screenPosition.Y = (int)(Guardian.Position.Y - (Guardian.SpriteHeight * 0.5f * Guardian.Scale) * Guardian.GravityDirection - (Main.screenHeight * 0.5f));
                }
            }
            if (MainMod.FocusCameraPosition.X != 0 && MainMod.FocusCameraPosition.Y != 0)
            {
                Main.screenPosition = MainMod.FocusCameraPosition;
                MainMod.FocusCameraPosition = Vector2.Zero;
            }
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active && guardian.AttackMyTarget)
                    guardian.SetTargetNpc(target);
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
        {
            if (!ProjMod.IsGuardianProjectile(proj.whoAmI))
            {
                //if (damage < target.lifeMax * 2 && !proj.minion)
                //    Guardian.ReduceAfkAbuseCounter((float)damage * 10 / proj.damage);
                foreach (TerraGuardian guardian in GetAllGuardianFollowers)
                {
                    if (guardian.Active && !proj.minion && guardian.AttackMyTarget)
                        guardian.SetTargetNpc(target);
                }
            }
            else
            {
                TerraGuardian tg = ProjMod.GuardianProj[proj.whoAmI];
                tg.IncreaseDamageStacker(damage, target.lifeMax);
                if (damage > 0)
                {
                    tg.OnHitSomething(target);
                    tg.IncreaseSkillByProjDamage(proj, damage, crit);
                }
            }
        }

        public override void OnHitPvp(Item item, Player target, int damage, bool crit)
        {
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active && guardian.AttackMyTarget)
                    guardian.SetTargetPlayer(target);
            }
            base.OnHitPvp(item, target, damage, crit);
        }

        public override void OnHitPvpWithProj(Projectile proj, Player target, int damage, bool crit)
        {
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (guardian.Active && !proj.minion && guardian.AttackMyTarget)
                    guardian.SetTargetPlayer(target);
            }
            if (ProjMod.IsGuardianProjectile(proj.whoAmI))
            {
                TerraGuardian tg = ProjMod.GuardianProj[proj.whoAmI];
                if (damage > 0)
                {
                    tg.OnHitSomething(target);
                    tg.IncreaseSkillByProjDamage(proj, damage, crit);
                }
            }
            base.OnHitPvpWithProj(proj, target, damage, crit);
        }

        public override bool PreItemCheck()
        {
            if (Guardian.Active)
            {
                if (Guardian.PlayerControl || MountedOnGuardian)
                {
                    player.gravDir = Guardian.GravityDirection;
                }
            }
            foreach (TerraGuardian guardian in AssistGuardians)
            {
                guardian.GravityDirection = (int)player.gravDir;
            }
            return true;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (!MainMod.NetplaySync)
                return;
            if (newPlayer)
            {
                for (int p = 0; p < MainMod.PlayerGuardianSync.Count; p++)
                {
                    if (MainMod.PlayerGuardianSync[p].Key == fromWho)
                        MainMod.PlayerGuardianSync.RemoveAt(p);
                }
                NetMod.SendGuardianData(player, SelectedGuardian, toWho, fromWho);
                NetMod.SendGuardianBehaviorFlags(player, SelectedGuardian, toWho, fromWho);
                for (int i = 0; i < 50; i++)
                {
                    NetMod.SendGuardianInventoryItem(player, SelectedGuardian, i, toWho, fromWho);
                }
                for (int e = 0; e < 9; e++)
                {
                    NetMod.SendGuardianEquippedItem(player, SelectedGuardian, e, toWho, fromWho);
                }
                for (int s = 0; s < Guardian.SkillList.Count; s++)
                {
                    NetMod.SendGuardianSkillProgress(player, SelectedGuardian, s, toWho, fromWho);
                }
            }
            NetMod.SendGuardianSummonState(player, SelectedGuardian, toWho, fromWho);
        }

        public override void SetControls()
        {
            if (player.whoAmI == Main.myPlayer && !player.dead && !KnockedOut)
                UpdateReviveSystem();
            if (player.whoAmI == Main.myPlayer && KnockedOut) //Controls
            {
                if (KnockedOutCold && player.controlHook && !MainMod.PlayersDontDiesAfterDownedDefeat)
                {
                    DoForceKill(player.name + (Main.rand.Next(2) == 1 ? " succumbed to injuries." : " couldn't get help to be revived."));
                    //player.KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(player.name + " couldn't get help to revive."), 0, 0, false);
                    player.fullRotation = 0;
                }
                player.controlLeft = player.controlRight = player.controlUp = player.controlDown = player.controlJump = player.controlMount =
                    player.controlQuickMana = player.controlSmart = player.controlThrow = player.controlInv =
                    player.controlUseTile = false;
                if (true || KnockedOutCold)
                {
                    player.controlUseItem = false;
                }
                player.releaseQuickHeal = player.releaseQuickMana = false;
            }
            if (player.whoAmI == Main.myPlayer && MainMod.TestNewOrderHud)
            {
                MainMod.DButtonPress = 255;
                for (byte k = 0; k < 10; k++)
                {
                    if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D1 + k) && Main.oldKeyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D1 + k))
                    {
                        MainMod.DButtonPress = k;
                        break;
                    }
                }
                if (GuardianOrderHudNew.Active)
                {
                    GuardianOrderHudNew.Update(this);
                }
                else
                {
                    if (MainMod.orderCallButton.JustPressed)
                    {
                        GuardianOrderHudNew.Open();
                    }
                }
            }
            if (BeingGrabbedByGuardian)
            {
                player.controlLeft = player.controlRight = player.controlUp = player.controlDown = player.controlUseItem = player.controlHook = player.controlMount = false;
            }
            //if (!Guardian.Active) return;
            bool BuffButtonPressed = false;
            foreach (Microsoft.Xna.Framework.Input.Keys k in Main.keyState.GetPressedKeys())
            {
                if (k.ToString() == Main.cBuff)
                    BuffButtonPressed = true;
            }
            foreach (Microsoft.Xna.Framework.Input.Keys k in Main.oldKeyState.GetPressedKeys())
            {
                if (k.ToString() == Main.cBuff)
                    BuffButtonPressed = false;
            }
            bool ResetPlayerControls = false;
            if (ControllingGuardian)
            {
                if (Guardian.ItemAnimationTime == 0 && player.selectedItem >= 0 && player.selectedItem < 10) Guardian.SelectedItem = player.selectedItem;
                if (!MountedOnGuardian)
                {
                    Guardian.MoveLeft = player.controlLeft;
                    Guardian.MoveRight = player.controlRight;
                    Guardian.MoveUp = player.controlUp;
                    Guardian.MoveDown = player.controlDown;
                    Guardian.Jump = player.controlJump;
                    Guardian.Action = player.controlUseItem;
                    ResetPlayerControls = true;
                }
                else
                {
                    Guardian.Action = player.controlUseItem;
                    player.controlUseItem = false;
                }
                Guardian.OffHandAction = player.controlTorch;
                if (player.controlQuickHeal)
                {
                    Guardian.AttemptToUseHealingPotion();
                }
                if (player.controlQuickMana)
                {
                    Guardian.AttemptToUseManaPotion();
                }
                if (BuffButtonPressed && !Guardian.DoAction.InUse)
                {
                    GuardianActions.UseBuffPotions(Guardian);
                }
                Guardian.AimDirection = Main.screenPosition.ToPoint();
                Guardian.AimDirection.X += Main.mouseX;
                Guardian.AimDirection.Y += Main.mouseY;
                if (Guardian.PlayerControl)
                    player.controlTorch = false;
            }
            if (LastMountButtonPressed)
            {
                if (!player.controlMount)
                {
                    LastMountButtonPressed = false;
                }
            }
            bool AdjascentTileChange = false;
            bool SomeoneQuickMounted = false;
            bool MountKeyPressed = false;
            if (player.controlMount)
            {
                if (!LastMountButtonPressed)
                {
                    MountKeyPressed = true;
                }
            }
            LastMountButtonPressed = player.controlMount;
            foreach (TerraGuardian guardian in GetAllGuardianFollowers)
            {
                if (!guardian.Active)
                    continue;
                if (Main.playerInventory && (guardian.PlayerControl || (guardian.PlayerMounted && !guardian.Base.ReverseMount)))
                {
                    if (guardian.AdjascentTileChecker())
                        AdjascentTileChange = true;
                }
                if (player.controlJump && player.releaseJump)
                    guardian.TryToDisableAfkPunishment();
                bool MountbuttonPressed = false;
                if (!SomeoneQuickMounted && MountKeyPressed && !guardian.SittingOnPlayerMount && !ControllingGuardian && guardian.HasFlag(GuardianFlags.AllowMount) && guardian.OverrideQuickMountToMountGuardianInstead)
                {
                    bool LastMountState = guardian.PlayerMounted;
                    if (!guardian.PlayerMounted && !MountedOnGuardian && !GuardianMountingOnPlayer)
                    {
                        if (player.getRect().Intersects(guardian.HitBox))//Guardian.ToggleMount())
                        {
                            GuardianActions.GuardianPutPlayerOnShoulderCommand(guardian);
                            SomeoneQuickMounted = true;
                        }
                    }
                    else if (guardian.PlayerMounted)
                    {
                        GuardianActions.GuardianPutPlayerOnTheFloorCommand(guardian);
                        SomeoneQuickMounted = true;
                    }
                }
                if (MountedOnGuardian)
                {
                    player.controlHook = false;
                    if (!guardian.OverrideQuickMountToMountGuardianInstead)
                        player.controlMount = false;
                }
                if (guardian.GuardianHasControlWhenMounted && guardian.PlayerMounted && guardian.Base.ReverseMount)
                {
                    player.controlUp = guardian.LastMoveUp;
                    player.controlDown = guardian.LastMoveDown;
                    player.controlLeft = guardian.LastMoveLeft;
                    player.controlRight = guardian.LastMoveRight;
                    player.controlJump = guardian.LastJump;
                }
            }
            if (SomeoneQuickMounted)
            {
                player.controlMount = false;
            }
            if (ResetPlayerControls)
                player.controlLeft = player.controlRight = player.controlUp = player.controlDown = player.controlJump = player.controlUseItem = player.controlHook = player.controlMount = false;
            if (player.whoAmI == Main.myPlayer)
                MainMod.Update2PControl(Guardian);
            if (AdjascentTileChange)
                Recipe.FindRecipes();
        }
        private bool LastMountButtonPressed = false;

        public override void ModifyDrawHeadLayers(List<PlayerHeadLayer> layers)
        {
            base.ModifyDrawHeadLayers(layers);
        }

        public override void Initialize()
        {
            if (!MainMod.HasPlayerAwareOfContestMonthChange)
            {
                Main.NewText("The Popularity Contest Result is out.");
                Main.NewText(MainMod.ContestResultLink);
                Main.NewText("New month voting is now up, pick your favorite TerraGuardians.");
                MainMod.HasPlayerAwareOfContestMonthChange = true;
            }
            if (BuddiesMode)
            {
                if (!HasGuardianSummoned(player, BuddiesModeBuddyID.ID, BuddiesModeBuddyID.ModID))
                    CallGuardian(BuddiesModeBuddyID.ID, BuddiesModeBuddyID.ModID);
            }
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            if (!mediumcoreDeath && Main.PlayerList.Count > 1)
            {
                Item i = new Item();
                i.SetDefaults(ModContent.ItemType<Items.Consumable.PortraitOfAFriend>());
                items.Add(i);
            }
        }

        public override Terraria.ModLoader.IO.TagCompound Save()
        {
            Terraria.ModLoader.IO.TagCompound tag = new Terraria.ModLoader.IO.TagCompound();
            tag.Add("ModVersion", MainMod.ModVersion);
            tag.Add("TutorialStep", (byte)TutorialFlags);
            tag.Add("BuddiesMode", BuddiesMode);
            if (BuddiesMode)
            {
                tag.Add("BuddyID", BuddiesModeBuddyID.ID);
                tag.Add("BuddyModID", BuddiesModeBuddyID.ModID);
            }
            tag.Add("KnockedOut", KnockedOut);
            int[] Keys = MyGuardians.Keys.ToArray();
            tag.Add("GuardianUIDs", Keys);
            foreach (int g in Keys)
            {
                MyGuardians[g].Save(tag, g);
            }
            tag.Add("SelectedGuardian", SelectedGuardian);
            int SelectedAssistGuardianCount = MainMod.MaxExtraGuardianFollowers;
            tag.Add("SelectedAssistCount", SelectedAssistGuardianCount);
            for (int i = 0; i < SelectedAssistGuardianCount; i++)
            {
                tag.Add("SelectedAssistGuardian_" + i, SelectedAssistGuardians[i]);
            }
            for (int i = 0; i < 5; i++)
            {
                tag.Add("PigForm_" + i, PigGuardianCloudForm[i]);
            }
            tag.Add("ReceivedFoodFromMinerva", ReceivedFoodFromMinerva);
            return tag;
        }

        public override void Load(Terraria.ModLoader.IO.TagCompound tag)
        {
            int ModVersion = tag.GetInt("ModVersion");
            if (ModVersion > MainMod.LastContestModVersion)
            {
                MainMod.HasPlayerAwareOfContestMonthChange = true;
            }
            if (ModVersion >= 34)
            {
                if (ModVersion <= 42)
                {
                    TutorialCompanionIntroduction = tag.GetInt("TutorialStep") > 0;
                }
                else if (ModVersion <= 59)
                {
                    TutorialCompanionIntroduction = tag.GetBool("TutorialStep");
                }
                else
                {
                    TutorialFlags = tag.GetByte("TutorialStep");
                }
            }
            else
                TutorialCompanionIntroduction = true;
            if (ModVersion >= 75)
            {
                BuddiesMode = tag.GetBool("BuddiesMode");
                if (BuddiesMode)
                {
                    BuddiesModeBuddyID = new GuardianID(tag.GetInt("BuddyID"), tag.GetString("BuddyModID"));
                }
            }
            if (ModVersion >= 59)
                KnockedOut = tag.GetBool("KnockedOut");
            Guardian = new TerraGuardian();
            MyGuardians = new Dictionary<int, GuardianData>();
            //Guardian.Load(tag);
            if (ModVersion <= 13) //Retro compatibility of save loading
            {
                DoRetroCompatibilityLoad(tag, ModVersion);
            }
            else
            {
                int[] Keys = tag.GetIntArray("GuardianUIDs");
                foreach (int g in Keys)
                {
                    GuardianData d = new GuardianData();
                    d.Load(tag, ModVersion, g);
                    MyGuardians.Add(g, d);
                }
            }
            SelectedGuardian = tag.GetInt("SelectedGuardian");
            if (ModVersion >= 48)
            {
                int SelectedAssistGuardianCount = tag.GetInt("SelectedAssistCount");
                for (int i = 0; i < SelectedAssistGuardianCount; i++)
                {
                    if (i < MainMod.MaxExtraGuardianFollowers)
                    {
                        SelectedAssistGuardians[i] = tag.GetInt("SelectedAssistGuardian_" + i);
                    }
                }
            }
            if (ModVersion >= 71)
            {
                for (int i = 0; i < 5; i++)
                {
                    PigGuardianCloudForm[i] = tag.GetBool("PigForm_" + i);
                }
            }
            if(ModVersion >= 79)
            {
                ReceivedFoodFromMinerva = tag.GetBool("ReceivedFoodFromMinerva");
            }
        }

        public override void OnEnterWorld(Player player)
        {
            MainMod.PlayerGuardianSync.Clear();
            ProjMod.GuardianProj.Clear();
            if (SelectedGuardian > -1)
            {
                CallGuardian(SelectedGuardian, 0);
            }
            if (Main.netMode == 0)
            {
                byte AssistSlot = 1;
                for (int slot = 0; slot < MainMod.MaxExtraGuardianFollowers; slot++)
                {
                    int Assist = SelectedAssistGuardians[slot];
                    //Main.NewText("Slot " + slot + ": " + (Assist == -1 ? "No Guardian" : MyGuardians[Assist].Name));
                    if (Assist > -1)
                    {
                        CallGuardian(Assist, AssistSlot);
                    }
                    else
                    {
                        SelectedAssistGuardians[slot] = -1;
                    }
                    AssistSlot++;
                }
            }
            if (!HasGuardian(4) && MainMod.CanGiveFreeNemesis())
            {
                AddNewGuardian(4);
                Main.NewText("You gained a free Nemesis guardian as halloween reward.");
            }
            if (!HasGuardian(GuardianBase.Vladimir) && MainMod.CanGiveFreeVladimir())
            {
                AddNewGuardian(GuardianBase.Vladimir);
                int DaysCounter = (int)(new DateTime(2020, 05, 19) - DateTime.Now).TotalDays;
                Main.NewText("With Terraria 1.4 just " + DaysCounter + " days away, Vladimir is being given away until then.");
            }
            RecalculateFriendshipLevel();
        }

        public bool CallGuardian(int GuardianID, string GuardianModID = "")
        {
            byte Slot = GetEmptyGuardianSlot();
            if (Slot != 255)
            {
                return CallGuardian(GuardianID, GuardianModID, Slot);
            }
            return false;
        }

        public bool CallGuardian(int GuardianID, string GuardianModID = "", byte AssistSlot = 0)
        {
            if (GuardianModID == "") GuardianModID = mod.Name;
            int[] Keys = MyGuardians.Keys.ToArray();
            foreach (int key in Keys)
            {
                if (MyGuardians[key].ID == GuardianID && MyGuardians[key].ModID == GuardianModID)
                {
                    CallGuardian(key, AssistSlot);
                    return true;
                }
            }
            return false;
        }

        public void CallGuardian(int Id)
        {
            byte Slot = GetEmptyGuardianSlot();
            if (Slot != 255)
                CallGuardian(Id, Slot);
        }

        public void CallGuardian(int Id, byte AssistSlot)
        {
            if (AssistSlot > 0 && Main.netMode > 0)
                return;
            if (MyGuardians.ContainsKey(Id))
            {
                if (AssistSlot == 0)
                {
                    if (Guardian.Active)
                    {
                        if (Guardian.PlayerMounted)
                            Guardian.ToggleMount();
                        if (Guardian.PlayerControl)
                            Guardian.TogglePlayerControl();
                    }
                    SelectedGuardian = Id;
                }
                else
                {
                    SelectedAssistGuardians[AssistSlot - 1] = Id;
                }
                TerraGuardian guardian = null;
                bool HasTownNpc = false;
                if (Main.netMode == 0 && WorldMod.IsGuardianNpcInWorld(MyGuardians[Id].ID, MyGuardians[Id].ModID))
                {
                    for (int gn = 0; gn < WorldMod.GuardianTownNPC.Count; gn++)
                    {
                        if (WorldMod.GuardianTownNPC[gn].ID == MyGuardians[Id].ID && WorldMod.GuardianTownNPC[gn].ModID == MyGuardians[Id].ModID)
                        {
                            guardian = WorldMod.GuardianTownNPC[gn];
                            HasTownNpc = true;
                            break;
                        }
                    }
                }
                else
                {
                    guardian = new TerraGuardian();
                }
                guardian.OwnerPos = player.whoAmI;
                guardian.Active = true;
                guardian.AssistSlot = AssistSlot;
                if (!Main.gameMenu && !HasTownNpc)
                {
                    guardian.Spawn();
                }
                if (guardian.furniturex > -1)
                    guardian.LeaveFurniture();
                guardian.DoUpdateGuardianStatus();
                guardian.Scale = guardian.ScaleMult;
                guardian.LastFriendshipLevel = guardian.FriendshipLevel;
                if (AssistSlot == 0)
                    this.Guardian = guardian;
                else
                    AssistGuardians[AssistSlot - 1] = guardian;
                if (guardian.Distance(player.Center) >= 512f)
                {
                    guardian.TeleportToPlayer();
                }
                MainMod.AddActiveGuardian(guardian);
                if (player.whoAmI == Main.myPlayer && !TutorialOrderIntroduction)
                {
                    TutorialOrderIntroduction = true;
                    string OrderKeys = "";
                    foreach (string key in MainMod.orderCallButton.GetAssignedKeys())
                    {
                        if (OrderKeys != "")
                            OrderKeys += "+";
                        OrderKeys += key;
                    }
                    Main.NewText("You can give it orders by pressing '" + OrderKeys + "' key, and navigating with the number buttons. You can undo an order step by pressing '" + OrderKeys + "' again. You can also change order call key on the input settings.");
                }
                if (MainMod.NetplaySync && Main.netMode == 1 && player.whoAmI == Main.myPlayer && AssistSlot == 0)
                {
                    for (byte p = 0; p < 255; p++)
                    {
                        if (p == player.whoAmI)
                            continue;
                        if (!Main.player[p].active)
                            continue;
                        bool AlreadySync = MainMod.HasSyncFlag(p, Id);
                        if (!AlreadySync)
                            NetMod.SendGuardianData(player, Id, p);
                        NetMod.SendGuardianSummonState(player, Id, p);
                        if (!AlreadySync)
                        {
                            NetMod.SendGuardianBehaviorFlags(player, Id, p);
                            for (int i = 0; i < 50; i++)
                            {
                                NetMod.SendGuardianInventoryItem(player, Id, i, p);
                            }
                            for (int e = 0; e < 9; e++)
                            {
                                NetMod.SendGuardianEquippedItem(player, Id, e, p);
                            }
                            for (int s = 0; s < guardian.SkillList.Count; s++)
                            {
                                NetMod.SendGuardianSkillProgress(player, Id, s, p);
                            }
                        }
                    }
                }
            }
            else
            {
                Main.NewText("Invalid ID.");
            }
        }

        public void DismissGuardian(int ID, string ModID)
        {
            if (ModID == "")
                ModID = mod.Name;
            TerraGuardian[] guardians = GetAllGuardianFollowers;
            for (byte i = 0; i < guardians.Length; i++)
            {
                if (guardians[i].Active && guardians[i].ID == ID && guardians[i].ModID == ModID)
                {
                    DismissGuardian(i);
                    return;
                }
            }
        }

        public void DismissGuardian(byte AssistSlot = 0)
        {
            TerraGuardian Guardian;
            if (AssistSlot == 0)
            {
                Guardian = this.Guardian;
                if(Main.netMode == 0)
                    this.Guardian = new TerraGuardian();
            }
            else
            {
                Guardian = AssistGuardians[AssistSlot - 1];
                if (Main.netMode == 0)
                    AssistGuardians[AssistSlot - 1] = new TerraGuardian();
            }
            if (Guardian.Active)
            {
                if (BuddiesMode && Guardian.MyID == BuddiesModeBuddyID) //Never remove the buddy. NEVER.
                {
                    return;
                }
                if (Guardian.PlayerMounted)
                    Guardian.ToggleMount();
                if (Guardian.SittingOnPlayerMount)
                    Guardian.DoSitOnPlayerMount(false);
                if (Guardian.PlayerControl)
                    Guardian.TogglePlayerControl(true);
                if (Guardian.GuardingPosition.HasValue)
                    Guardian.ToggleWait(false);
                if (Main.netMode > 0)
                {
                    Guardian.Active = false;
                }
                else
                {
                    Guardian.OwnerPos = -1;
                    Guardian.AssistSlot = 0;
                    //Guardian.Spawn();
                }
                if (MainMod.NetplaySync && Main.netMode == 1 && Main.myPlayer == player.whoAmI && AssistSlot == 0)
                    NetMod.SendGuardianSummonState(player, SelectedGuardian, -1, player.whoAmI);
                if (AssistSlot == 0)
                    SelectedGuardian = -1;
                else
                    SelectedAssistGuardians[AssistSlot - 1] = -1;
            }
        }

        public void CheckIfLeaderHasBeenRemoved()
        {
            //return;
            if (!Guardian.Active)
            {
                for (int i = 0; i < MainMod.MaxExtraGuardianFollowers; i++)
                {
                    if (AssistGuardians[i].Active)
                    {
                        Guardian = AssistGuardians[i];
                        if (Guardian.AssistSlot == TitanGuardian)
                            TitanGuardian = 0;
                        Guardian.AssistSlot = 0;
                        SelectedGuardian = SelectedAssistGuardians[i];
                        AssistGuardians[i] = new TerraGuardian();
                        SelectedAssistGuardians[i] = -1;
                        return;
                    }
                }
            }
        }

        public bool AddNewGuardian(int Id, Mod mod, int FixedPosition = -1)
        {
            return AddNewGuardian(Id, mod.Name, FixedPosition);
        }

        public bool AddNewGuardian(int Id, string ModId = "", int FixedPosition = -1)
        {
            if (ModId == "")
                ModId = MainMod.mod.Name;
            int SpawnID = 0;
            bool AlreadySpawned = false;
            int[] Keys = MyGuardians.Keys.ToArray();
            foreach (int g in Keys)
            {
                //if (SpawnID == g)
                //    SpawnID++;
                if (MyGuardians[g].ID == Id && MyGuardians[g].ModID == ModId)
                    AlreadySpawned = true;
            }
            bool FirstGuardian = Keys.Length == 0;
            if (!AlreadySpawned)
            {
                while (MyGuardians.ContainsKey(SpawnID))
                    SpawnID++;
                MyGuardians.Add(SpawnID, new GuardianData(Id, ModId));
                if (MyGuardians[SpawnID].Base.CanChangeGender)
                    MyGuardians[SpawnID].Male = Main.rand.Next(2) == 0;
                if (FirstGuardian)
                {
                    if (player.whoAmI == Main.myPlayer && !TutorialCompanionIntroduction)
                    {
                        Main.NewText("Looks like you've got yourself a follower. You can check It by going in your inventory, on the lower part, and clicking the bulldog face.");
                        TutorialCompanionIntroduction = true;
                    }
                }
            }
            return AlreadySpawned;
        }

        public void DoRetroCompatibilityLoad(Terraria.ModLoader.IO.TagCompound tag, int ModVersion)
        {
            GuardianData d = new GuardianData();
            d.ID = tag.GetInt("GuardianID");
            if (ModVersion >= 7)
            {
                d.FriendshipLevel = tag.GetByte("FriendshipLevel");
                d.FriendshipProgression = tag.GetByte("FriendshipProgress");
                d.TravellingStacker = tag.GetFloat("TravellingStacker");
            }
            if (ModVersion >= 1)
            {
                float HealthPercentage = tag.GetFloat("HealthPercentage");
                d.HP = (int)(d.MHP * HealthPercentage);
            }
            d.LifeCrystalHealth = (byte)tag.GetInt("LifeCrystals");
            d.LifeFruitHealth = (byte)tag.GetInt("LifeFruits");
            if (ModVersion >= 5)
                d.tactic = (CombatTactic)tag.GetByte("CombatTactic");
            if (ModVersion >= 6)
                d.Tanker = tag.GetBool("TankingFlag");
            if (ModVersion >= 11)
                d.OverrideQuickMountToMountGuardianInstead = tag.GetBool("QuickMountOverride");
            if (ModVersion >= 12)
                d.UseHeavyMeleeAttackWhenMounted = tag.GetBool("MountMeleeUsageToggle");
            if (ModVersion >= 13)
            {
                d.AvoidCombat = tag.GetBool("AvoidCombat");
                d.ChargeAhead = tag.GetBool("ChargeAhead");
            }
            int ContainerSize = tag.GetInt("InventorySize");
            for (int i = 0; i < ContainerSize; i++)
            {
                Item j = new Item();
                if (ModVersion < 2)
                {
                    j.SetDefaults(tag.GetInt("Inventory_" + i + "_Type"));
                    j.stack = tag.GetInt("Inventory_" + i + "_Stack");
                    j.prefix = tag.GetByte("Inventory_" + i + "_Prefix");
                }
                else
                {
                    bool ItemExists = true;
                    if (ModVersion >= 3) ItemExists = tag.GetBool("Inventory_" + i + "_exists");
                    if (ItemExists)
                    {
                        j = tag.Get<Item>("Inventory_" + i);
                    }
                }
                if (i < d.Inventory.Length)
                    d.Inventory[i] = j;
            }
            ContainerSize = tag.GetInt("EquipmentsSize");
            for (int e = 0; e < ContainerSize; e++)
            {
                Item j = new Item();
                if (ModVersion < 4)
                {
                    j.SetDefaults(tag.GetInt("Equipment_" + e + "_Type"));
                    j.stack = tag.GetInt("Equipment_" + e + "_Stack");
                    j.prefix = tag.GetByte("Equipment_" + e + "_Prefix");
                }
                else
                {
                    if (tag.GetBool("Equipment_" + e + "_exists"))
                    {
                        j = tag.Get<Item>("Equipment_" + e);
                    }
                }
                if (e < d.Equipments.Length)
                    d.Equipments[e] = j;
            }
            //if (ModVersion >= 8)
            //    d.request.Load(tag, ModVersion, null);
            MyGuardians.Add(0, d);
            for (int c = 0; c < Main.PlayerList.Count; c++)
            {
                if (Main.PlayerList[c].Player == player)
                {
                    d.LifeTime = TimeSpan.FromMinutes(Main.PlayerList[c].GetPlayTime().TotalSeconds * 60);
                }
            }
            SelectedGuardian = 0;
        }

        public override void OnRespawn(Player player)
        {
            foreach (TerraGuardian tg in GetAllGuardianFollowers)
            {
                if (tg.KnockedOut)
                {
                    tg.TeleportToPlayer();
                }
            }
            KnockedOut = KnockedOutCold = false;
            player.fullRotation = 0;
            FriendlyDuelDefeat = false;
        }

        public override void UpdateVanityAccessories()
        {
            base.UpdateVanityAccessories();
        }

        public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
        {
            if (MountedOnGuardian)
            {
                player.legFrame.Y = player.legFrame.Height * 6;
                player.legFrameCounter = 0;
                if (player.itemAnimation == 0)
                {
                    player.bodyFrame.Y = player.bodyFrame.Height * 3;
                    player.bodyFrameCounter = 0;
                }
                player.headFrame.Y = player.bodyFrame.Y;
                player.headFrameCounter = 0;
            }
            if (KnockedOut && player.velocity.Y == 0)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 5;
            }
        }

        public void ChangeLeaderGuardian(byte GuardianPosition)
        {
            if (GuardianPosition == 0 && GuardianPosition < MainMod.MaxExtraGuardianFollowers + 1)
                return;
            TerraGuardian GuardianToChangePlace = AssistGuardians[GuardianPosition - 1];
            int GuardianToChangePlacePosition = SelectedAssistGuardians[GuardianPosition - 1];
            for (int s = GuardianPosition - 1; s > 0; s--)
            {
                AssistGuardians[s] = AssistGuardians[s - 1];
                SelectedAssistGuardians[s] = SelectedAssistGuardians[s - 1];
            }
            AssistGuardians[0] = Guardian;
            SelectedAssistGuardians[0] = SelectedGuardian;
            Guardian = GuardianToChangePlace;
            SelectedGuardian = GuardianToChangePlacePosition;
        }

        public static bool GiveHeightBoost(Player player)
        {
            int BodyFrame = player.bodyFrame.Y / player.bodyFrame.Height;
            return (BodyFrame == 7 || BodyFrame == 8 || BodyFrame == 9 || BodyFrame == 14 || BodyFrame == 15 || BodyFrame == 16);
        }

        public void GetDrawDatas(out List<Terraria.DataStructures.DrawData> Back, out List<Terraria.DataStructures.DrawData> Front)
        {
            Back = new List<Terraria.DataStructures.DrawData>();
            Front = new List<Terraria.DataStructures.DrawData>();

            foreach (TerraGuardian g in GetAllGuardianFollowers)
            {
                if (!g.Active)
                    continue;
                g.DrawDataCreation();
                int BackStack = 0, FurnitureStack = 0;
                if (g.PlayerControl)
                {
                    Front.InsertRange(0, TerraGuardian.GetDrawFrontData);
                    Front.InsertRange(0, TerraGuardian.GetDrawBehindData);
                }
                else if (g.KnockedOut)
                {
                    Back.InsertRange(0, TerraGuardian.GetDrawFrontData);
                    Back.InsertRange(0, TerraGuardian.GetDrawBehindData);
                }
                else if (g.PlayerMounted || g.SittingOnPlayerMount)
                {
                    Back.AddRange(TerraGuardian.GetDrawBehindData);
                    Front.AddRange(TerraGuardian.GetDrawFrontData);
                }
                else
                {
                    if (g.UsingFurniture)
                    {
                        Back.InsertRange(0, TerraGuardian.GetDrawFrontData);
                        Back.InsertRange(0, TerraGuardian.GetDrawBehindData);
                        FurnitureStack += TerraGuardian.DrawFront.Count + TerraGuardian.DrawBehind.Count - 1;
                    }
                    else if (g.ChargeAhead)
                    {
                        //Front.AddRange(TerraGuardian.DrawBehind);
                        //Front.AddRange(TerraGuardian.DrawFront);
                        Back.InsertRange(BackStack + FurnitureStack, TerraGuardian.GetDrawFrontData);
                        Back.InsertRange(BackStack + FurnitureStack, TerraGuardian.GetDrawBehindData);
                    }
                    else
                    {
                        Back.InsertRange(FurnitureStack, TerraGuardian.GetDrawFrontData);
                        Back.InsertRange(FurnitureStack, TerraGuardian.GetDrawBehindData);
                        BackStack += TerraGuardian.DrawBehind.Count + TerraGuardian.DrawFront.Count - 1;
                    }
                }
            }
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            if (ControllingGuardian)
                layers.Clear();
            PlayerLayer l;
            if (player.whoAmI == Main.myPlayer && HasGhostFoxHauntDebuff)
            {
                l = new PlayerLayer(mod.Name, "Ghost Fox Guardian Haunt", delegate(PlayerDrawInfo pdi)
                {
                    bool PlayerKOd = !pdi.drawPlayer.dead && pdi.drawPlayer.GetModPlayer<PlayerMod>().KnockedOut;
                    const int Frame = 10, ReviveFrame = 15;
                    GuardianBase gb = GuardianBase.GetGuardianBase(16);
                    float DirectionFace = pdi.drawPlayer.direction;
                    if (!gb.sprites.IsTextureLoaded)
                        gb.sprites.LoadTextures();
                    Microsoft.Xna.Framework.Graphics.SpriteEffects seffects = pdi.spriteEffects;
                    Vector2 FluffelPosition = gb.LeftHandPoints.GetPositionFromFrameVector((PlayerKOd ? ReviveFrame : Frame));
                    FluffelPosition.X = FluffelPosition.X - gb.SpriteWidth * 0.5f;
                    if (DirectionFace > 0)
                    {
                        FluffelPosition.X *= -1;
                    }
                    Vector2 HauntPosition = pdi.position;
                    HauntPosition.X += player.width * 0.5f - 6 * DirectionFace;
                    if (PlayerKOd)
                    {
                        HauntPosition.Y += player.height * 0.35f;
                        DirectionFace *= -1;
                        if (seffects == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                            seffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
                        else if (seffects == Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally)
                            seffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                        FluffelPosition.X *= -1f;
                    }
                    HauntPosition.Y += player.height + gb.SpriteHeight - FluffelPosition.Y - 30;
                    HauntPosition.X += FluffelPosition.X;
                    //HauntPosition.Y += ((float)Math.Sin(Main.GlobalTime * 2)) * 3;
                    Vector2 Origin = new Vector2(gb.SpriteWidth * 0.5f, gb.SpriteHeight);
                    Rectangle DrawFrame = new Rectangle((PlayerKOd ? ReviveFrame : Frame) * gb.SpriteWidth, 0, gb.SpriteWidth, gb.SpriteHeight);
                    float Opacity = MainMod.FlufflesHauntOpacity;
                    if (Opacity < 0)
                        Opacity = 0;
                    Color color = Creatures.FlufflesBase.GhostfyColor(Color.White, Opacity, Creatures.FlufflesBase.GetColorMod);
                    bool IgnoreRotation = PlayerKOd;
                    Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(gb.sprites.BodySprite, HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
                    dd.ignorePlayerRotation = IgnoreRotation;
                    Main.playerDrawData.Insert(0, dd);
                    dd = new Terraria.DataStructures.DrawData(gb.sprites.RightArmSprite, HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
                    dd.ignorePlayerRotation = IgnoreRotation;
                    Main.playerDrawData.Insert(0, dd);
                    dd = new Terraria.DataStructures.DrawData(gb.sprites.LeftArmSprite, HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
                    dd.ignorePlayerRotation = IgnoreRotation;
                    Main.playerDrawData.Add(dd);
                });
                layers.Add(l);
            }
            l = new PlayerLayer(mod.Name, "TerraGuardians: Player Buff Effects", delegate(PlayerDrawInfo pdi)
            {
                if (player.HasBuff(ModContent.BuffType<Buffs.Love>()) && Main.rand.Next(15) == 0)
                {
                    Vector2 Velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                    Velocity.Normalize();
                    Velocity.X *= 0.66f;
                    int gore = Gore.NewGore(player.position + new Vector2(Main.rand.Next(player.width + 1), Main.rand.Next(player.height + 1)), Velocity * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                    Main.gore[gore].sticky = false;
                    Main.gore[gore].velocity *= 0.4f;
                    Main.gore[gore].velocity.Y -= 0.6f;
                }
            });
            l = new PlayerLayer(mod.Name, "Terra Guardian Layer", delegate(PlayerDrawInfo pdi)
            {
                if (pdi.shadow != 0)
                    return;
                List<Terraria.DataStructures.DrawData> Back = new List<Terraria.DataStructures.DrawData>();
                List<Terraria.DataStructures.DrawData> Front = new List<Terraria.DataStructures.DrawData>();
                GetDrawDatas(out Back, out Front);
                Main.playerDrawData.InsertRange(0, Back);
                Main.playerDrawData.AddRange(Front);
                /*TerraGuardian controlledOne = null, MountedOne = null, ReverseMountedOne = null;
                foreach (TerraGuardian guardian in GetAllGuardianFollowers.Reverse())
                {
                    if (guardian.Active)
                    {
                        if (false && guardian.PlayerMounted)
                        {
                            if (!guardian.Base.ReverseMount)
                                MountedOne = guardian;
                            else
                                ReverseMountedOne = guardian;
                        }
                        else if (guardian.PlayerControl)
                            controlledOne = guardian;
                        else
                            guardian.Draw();
                    }
                }
                if (MountedOne != null)
                    MountedOne.Draw();
                if (ReverseMountedOne != null)
                    ReverseMountedOne.Draw();
                if (controlledOne != null)
                    controlledOne.Draw();*/
                foreach (GuardianDrawMoment gdm in MainMod.DrawMoment)
                {
                    if (gdm.DrawTargetType == TerraGuardian.TargetTypes.Player && gdm.DrawTargetID == player.whoAmI && MainMod.ActiveGuardians.ContainsKey(gdm.GuardianWhoAmID))
                    {
                        MainMod.ActiveGuardians[gdm.GuardianWhoAmID].DrawDataCreation();
                        Main.playerDrawData.InsertRange(0, TerraGuardian.GetDrawBehindData);
                        Main.playerDrawData.AddRange(TerraGuardian.GetDrawFrontData);
                    }
                }
            });
            layers.Add(l);
            if (AlexRecruitScripts.AlexNPCPosition > -1)
            {
                if (Main.npc[AlexRecruitScripts.AlexNPCPosition].target == player.whoAmI)
                {
                    l = new PlayerLayer(mod.Name, "Alex Front Part Layer", delegate(PlayerDrawInfo pdi)
                    {
                        if (!(Main.npc[AlexRecruitScripts.AlexNPCPosition].modNPC is Npcs.AlexNPC))
                            return;
                        Npcs.AlexNPC npc = (Main.npc[AlexRecruitScripts.AlexNPCPosition].modNPC as Npcs.AlexNPC);
                        Microsoft.Xna.Framework.Color c = Lighting.GetColor((int)npc.npc.Center.X / 16, (int)npc.npc.Center.Y / 16, Microsoft.Xna.Framework.Color.White);
                        Terraria.DataStructures.DrawData[] dds = npc.GetNpcDrawDatas(c, true);
                        for (int x = 0; x < dds.Length; x++)
                        {
                            dds[x].ignorePlayerRotation = true;
                            Main.playerDrawData.Add(dds[x]);
                        }
                        /*foreach (Terraria.DataStructures.DrawData dd in npc.GetNpcDrawDatas(c, true))
                        {
                            dd.ignorePlayerRotation = true;
                            Main.playerDrawData.Add(dd);
                        }*/
                    });
                    layers.Add(l);
                }
            }
            l = new PlayerLayer(mod.Name, "Guardian NPCs Front Body Parts", delegate(PlayerDrawInfo pdi)
            {
                for (int n = 0; n < 200; n++)
                {
                    if (Main.npc[n].active && Main.npc[n].modNPC is Npcs.GuardianActorNPC && ((Npcs.GuardianActorNPC)Main.npc[n].modNPC).DrawInFrontOfPlayers.Contains(player.whoAmI))
                    {
                        Npcs.GuardianActorNPC ganpc = ((Npcs.GuardianActorNPC)Main.npc[n].modNPC);
                        Color color = Lighting.GetColor((int)ganpc.npc.Center.X / 16, (int)ganpc.npc.Center.Y / 16);
                        List<GuardianDrawData> dds = ganpc.GetDrawDatas(color, true);
                        for (int x = 0; x < dds.Count; x++)
                        {
                            Terraria.DataStructures.DrawData dd = dds[x].GetDrawData();
                            dd.ignorePlayerRotation = true;
                            Main.playerDrawData.Add(dd);
                        }
                    }
                }
            });
            layers.Add(l);
            if (eye != EyeState.Open)
            {
                l = new PlayerLayer(mod.Name, "Player Eye", delegate(PlayerDrawInfo pdi)
                {
                    const int FrameYBonus = 336;
                    for (int t = 0; t < Main.playerDrawData.Count; t++)
                    {
                        if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.Eyes])
                        {
                            Vector2 Position = Main.playerDrawData[t].position;
                            if ((player.headFrame.Y + FrameYBonus >= 7 * player.headFrame.Height && player.headFrame.Y + FrameYBonus < 11 * player.headFrame.Height) ||
                                player.headFrame.Y + FrameYBonus >= 14 * player.headFrame.Height && player.headFrame.Y + FrameYBonus < 17 * player.headFrame.Height)
                            {
                                Position.Y -= 2;
                            }
                            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.EyeTexture, Position, new Rectangle(40 * ((int)eye), 0, 40, 56), (eye == EyeState.Closed ? pdi.bodyColor : Main.playerDrawData[t].color), Main.playerDrawData[t].rotation, Main.playerDrawData[t].origin, Main.playerDrawData[t].scale, Main.playerDrawData[t].effect, 0);
                            Main.playerDrawData[t] = dd;
                        }
                        if (Main.playerDrawData[t].texture == Main.playerTextures[player.skinVariant, Terraria.ID.PlayerTextureID.EyeWhites])
                        {
                            Vector2 Position = Main.playerDrawData[t].position;
                            if ((player.headFrame.Y + FrameYBonus >= 7 * player.headFrame.Height && player.headFrame.Y + FrameYBonus < 11 * player.headFrame.Height) ||
                                player.headFrame.Y + FrameYBonus >= 14 * player.headFrame.Height && player.headFrame.Y + FrameYBonus < 17 * player.headFrame.Height)
                            {
                                Position.Y -= 2;
                            }
                            Terraria.DataStructures.DrawData dd = new Terraria.DataStructures.DrawData(MainMod.EyeTexture, Position, new Rectangle(40 * ((int)eye), 56, 40, 56), (eye == EyeState.Closed ? pdi.bodyColor : Main.playerDrawData[t].color), Main.playerDrawData[t].rotation, Main.playerDrawData[t].origin, Main.playerDrawData[t].scale, Main.playerDrawData[t].effect, 0);
                            Main.playerDrawData[t] = dd;
                        }
                    }
                });
            }
            layers.Add(l);
        }

        public enum EyeState
        {
            Open,
            HalfOpen,
            Closed
        }

        public enum BehaviorChanges
        {
            FreeWill,
            ChargeOnTarget,
            AvoidContact,
            StayAwayFromTarget
        }
    }
}
