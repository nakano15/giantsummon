using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace giantsummon
{
    public class MainMod : Mod
	{
        public static Texture2D GuardianButtonSlots, GuardianHealthBar, FriendshipHeartTexture, EmotionTexture, ReportButtonTexture, GuardianMouseTexture, EditButtonTexture,
            GuardianInfoIcons, CrownTexture, GuardianStatusIconTexture, HideButtonTexture, GuideArrowTexture, GSI_ForegroundInterfaceTexture, GSI_BackgroundInterfaceTexture,
            TrustIconsTexture;
        public static Texture2D EyeTexture;
        public static Texture2D TacticsBarTexture, TacticsIconsTexture;
        public static Texture2D TrappedCatTexture;
        public static Texture2D NinjaTextureBackup;
        public static Texture2D TwoHandedSwordSprite;
        public static Texture2D LosangleOfUnnown;
        public static Texture2D ContributorIconTexture;
        public static Mod mod;
        public static ModPacket GetPacket { get { return mod.GetPacket(); } }
        public static GuardianItemSlotButtons SelectedGuardianInventorySlot = GuardianItemSlotButtons.Nothing;
        public static byte SkillListRow = 0;
        public static SoundData FemaleHitSound = new SoundData(Terraria.ID.SoundID.FemaleHit);
        public static byte SelectedGuardian = 0;
        public static int GuardianInventoryMenuSubTab = 0;
        public static bool CheckingQuestBrief = false;
        public static bool WarnAboutSaleableInventorySlotsLeft = false, MobHealthBoost = false, GuardiansIdleEasierOnTowns = true, ShowBackwardAnimations = true, UseCompanionsDefinedScaleChange = true;
        public static Compatibility.SubworldLibraryCompatibility.SubworldInfo CurrentSubworld = null;
        //Contest related
        public const string VoteLink = "https://forms.gle/2KNqNcabh3rqn5Mu8";
        public const string ContestResultLink = "https://steamcommunity.com/groups/Nakano15-Modding/discussions/1/3201493834345301732/";
        public static readonly DateTime ContestEndDate = new DateTime(2021, 12, 19);
        public static bool IsPopularityContestRunning { get { return DateTime.Now <= ContestEndDate; } }
        public static bool IsPopularityContestResultShowing { get { return DateTime.Now + TimeSpan.FromDays(14) <= ContestEndDate; } }
        /// <summary>
        /// Returns 1 if popularity contest is running.
        /// Returns 2 if it's result showing time.
        /// </summary>
        public static byte PopContestState
        {
            get
            {
                if (IsPopularityContestRunning) return 1;
                if (IsPopularityContestResultShowing) return 2;
                return 0;
            }
        }
        //End contest related
        public const int ModVersion = 100, LastModVersion = 97;
        public const int MaxExtraGuardianFollowers = 7;
        public static bool ShowDebugInfo = false;
        //Downed system configs
        public static bool PlayersGetKnockedOutUponDefeat = false, PlayersDontDiesAfterDownedDefeat = false, GuardiansGetKnockedOutUponDefeat = false, 
            GuardiansDontDiesAfterDownedDefeat = false;
        //
        public static bool PlayableOnMultiplayer = false, UseNewMonsterModifiersSystem = true, UsingGuardianNecessitiesSystem = false, TestNewOrderHud = true, SharedCrystalValues = false,
            SetGuardiansHealthAndManaToPlayerStandards = false, UseSkillsSystem = true, CompanionsSpeaksWhileReviving = true, TileCollisionIsSameAsHitCollision = false, NoEtherItems = false, StartRescueCountdownWhenKnockedOutCold = false,
            DoNotUseRescue = false, CompanionsCanVisitWorld = true, DisableDamageReductionByNumberOfCompanions = false;
        public static float DamageNerfPerExtraCompanion = 0.1f;
        public static List<Terraria.ModLoader.Config.ItemDefinition> DualwieldWhitelist = new List<Terraria.ModLoader.Config.ItemDefinition>();
        public static Dictionary<Terraria.ModLoader.Config.ItemDefinition, int> ItemAttackRange = new Dictionary<Terraria.ModLoader.Config.ItemDefinition, int>();
        public static bool ForceUpdateGuardiansStatus = false;
        public static bool ManagingGuardianEquipments = false;
        public const bool IndividualSkillLeveling = true;
        public static PlayerIndex controlPort = PlayerIndex.Two;
        public static GamePadState gamePadState = GamePad.GetState(controlPort),
            oldGamePadState = GamePad.GetState(controlPort);
        public static bool Gameplay2PMode = false;
        public static bool MoveLeftPress = false, MoveRightPress = false, MoveUpPress = false, MoveDownPress = false, UseItemPress = false, JumpPress = false;
        public static Mod NExperienceMod, KalciphozMod, SubworldLibrary, TerraClassesMod, ThoriumMod, FargoMutantMod;
        public static bool TestForceGuardianOnFront = false;
        public static Main MainHook { get { return Main.instance; } }
        public static bool LastWof = false;
        public static int LastTrashItemID = 0;
        public static int NemesisFadeEffect = -NemesisFadeCooldown;
        public const int NemesisFadeCooldown = 15 * 60, NemesisFadingTime = 3 * 60;
        public static readonly int[] HolyTileIDs = new int[] { 109, 110, 113, 117, 116, 164, 403, 402 },
            JungleTileIDs = new int[] { 60, 61, 62, 74, 226 },
            SnowTileIDs = new int[] { 147, 148, 161, 162, 164, 163, 200 },
            CrimsonTileIDs = new int[] { 199, 203, 200, 401, 399, 234 },
            DesertTileIDs = new int[] { 53, 112, 116, 234, 397, 398, 402, 399, 396, 400, 403, 401 },
            GlowshroomTileIDs = new int[] { 70, 71, 72 },
            DungeonTileIDs = new int[] { 41, 43, 44 };
        public static readonly int[] VendorNpcs = new int[]{
            NPCID.ArmsDealer,
            NPCID.Clothier,
            NPCID.Demolitionist,
            NPCID.Dryad,
            NPCID.DyeTrader,
            NPCID.GoblinTinkerer,
            NPCID.Mechanic,
            NPCID.Merchant,
            NPCID.Painter,
            NPCID.PartyGirl,
            NPCID.SkeletonMerchant,
            NPCID.Stylist,
            NPCID.DD2Bartender,
            NPCID.TravellingMerchant,
            NPCID.WitchDoctor,
            NPCID.Cyborg,
            NPCID.Pirate,
            NPCID.SantaClaus,
            NPCID.Steampunker,
            NPCID.Truffle,
            NPCID.Wizard
        };
        private static Dictionary<string, GuardianBase.ModGuardianDB> ModGuardianLoader = new Dictionary<string, GuardianBase.ModGuardianDB>();
        public static int GuardianNpcHousingCheckCooldown = 0;
        public const int MaxGuardianNpcHousingCheckCooldown = 60 * 60 * 3;
        public static bool CheckIfGuardianNpcsCanStayAtHomeCheck { get { return GuardianNpcHousingCheckCooldown == MaxGuardianNpcHousingCheckCooldown; } }
        public static List<GuardianID> InitialGuardians = new List<GuardianID>();
        public static bool NetplaySync = true;
        public static ModHotKey orderCallButton;
        public static bool OrderCallButtonPressed = false;//, MakeUseOfOrderHotkey = false;
        private static Dictionary<int, TerraGuardian> TempActiveGuardians = new Dictionary<int, TerraGuardian>();
        public static Dictionary<int, TerraGuardian> ActiveGuardians = new Dictionary<int,TerraGuardian>();
        public static float CarpetAnimationTime = 0;
        public static Vector2 FocusCameraPosition = Vector2.Zero;
        public static byte DButtonPress = 255;
        public static int ToReviveID = -1;
        public static bool ToReviveIsGuardian = false, IsReviving = false;
        public static int ReviveTalkDelay = 0;
        public static int LastEventWave = 0;
        private static Dictionary<string, Group> CompanionGroups = new Dictionary<string, Group>();
        public static float TimeTranslated = 0;
        public static List<GuardianDrawMoment> DrawMoment = new List<GuardianDrawMoment>();
        public static float FlufflesHauntOpacity = -1f;
        public static Color ScreenColor = Color.Black;
        public static float ScreenColorAlpha = 0f;
        public static Vector2 PlayerSoulPosition = Vector2.Zero;
        public static bool SoulSaved = false;
        public static int LastChatTime = 0;
        public const string CustomCompanionCallString = "loadcompanions", CustomStarterCallString = "loadstarters";
        public static bool NpcInCameraRange = false;
        public static List<TerraGuardian> CompanionsToShowArrowFor = new List<TerraGuardian>();
        public const float BleedingHealthDamage = 0.015f;
        public const int BleedingHealthDamageTime = 30;
        public static int GeneralIdleCommentCooldown = 0;
        public static List<GuardianID> CompanionBlacklist = new List<GuardianID>();
        private static Tile DefaultTile = new Tile();
        public static bool LastBossSpotted = false, LastInvasionSpotted = false, LastEventStarted = false, LastMoonLordKilled = false, LastHardMode = false;
        public static byte LastEvent = 0;
        public static sbyte LastInvasion = -1;
        public static byte CompanionFaceJS = 0;
        private const byte CompanionFaceJSMaxTime = 30;
        private static int CompanionFaceJSID = GuardianBase.Alex;
        private static string CompanionFaceJSModID = "";

        public static void TriggerAlexJS()
        {
            TriggerCompanionJS(GuardianBase.Alex);
        }

        public static void TriggerAnyPlayerCompanionJS()
        {
            List<GuardianData> validGds = new List<GuardianData>();
            {
                foreach(GuardianData gd in Main.LocalPlayer.GetModPlayer<PlayerMod>().GetGuardians())
                {
                    if (gd.Base.IsCustomSpriteCharacter)
                        validGds.Add(gd);
                }
            }
            if (validGds.Count == 0)
            {
                TriggerCompanionJS(Main.rand.Next(2));
            }
            else
            {
                GuardianData picked = validGds[Main.rand.Next(validGds.Count)];
                TriggerCompanionJS(picked.ID, picked.ModID);
            }
        }

        public static void TriggerCompanionJS(int CompanionID, string CompanionModID = "")
        {
            if (Main.gameMenu)
                return;
            if (CompanionModID == "")
                CompanionModID = mod.Name;
            CompanionFaceJS = CompanionFaceJSMaxTime;
            CompanionFaceJSID = CompanionID;
            CompanionFaceJSModID = CompanionModID;
            switch (Main.rand.Next(4))
            {
                case 0:
                    Main.PlaySound(4, Main.LocalPlayer.Center, 10);
                    break;
                case 1:
                    Main.PlaySound(29, Main.LocalPlayer.Center, Main.rand.Next(88, 92));
                    break;
                case 2:
                    Main.PlaySound(3, Main.LocalPlayer.Center, 1);
                    break;
                case 3:
                    Main.PlaySound(29, Main.LocalPlayer.Center, 20);
                    break;
            }
        }

        public static Vector2 GetScreenCenter { get { return new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f + Main.screenPosition; } }

        public static Tile GetTile(Point p)
        {
            return GetTile(p.X, p.Y);
        }

        public static Tile GetTile(int x, int y)
        {
            if(x >= 0 && x < Main.maxTilesX && 
                y >= 0 && y < Main.maxTilesY)
            {
                if (Main.tile[x, y] == null)
                    Main.tile[x, y] = new Tile();
                return Main.tile[x, y];
            }
            return DefaultTile;
        }

        public static void SetIdleCommentCooldown()
        {
            GeneralIdleCommentCooldown = (int)(Main.rand.Next(15 * 60, 30 * 60));
        }

        public static void EnterSubworld(Compatibility.SubworldLibraryCompatibility.SubworldInfo subWorld)
        {
            if(SubworldLibrary == null)
            {
                Main.NewText("You don't have Subworld Library installed.", Color.Red);
                return;
            }
            if(CurrentSubworld != null && CurrentSubworld.AnyActive())
            {
                Main.NewText("You're currently in a subworld.", Color.Red);
                return;
            }
            CurrentSubworld = subWorld;
            subWorld.Enter();
        }

        public static void ExitSubworld()
        {
            if (SubworldLibrary == null)
            {
                Main.NewText("You don't have Subworld Library installed.", Color.Red);
                return;
            }
            if (CurrentSubworld != null && !CurrentSubworld.AnyActive())
            {
                Main.NewText("You're not in a subworld.", Color.Red);
                return;
            }
            SubworldLibrary.Call(new object[] { "Exit" });
        }

        public static Group AddNewGroup(string ID, string Name, float AgingSpeed, bool CustomSprite = true, bool RecognizeAsTerraGuardian = false)
        {
            Group g = AddNewGroup(ID, Name, CustomSprite, RecognizeAsTerraGuardian);
            g.AgingSpeed = AgingSpeed;
            return g;
        }

        public static Group AddNewGroup(string ID, string Name, bool CustomSprite = true, bool RecognizeAsTerraGuardian = false)
        {
            Group g;
            if (CompanionGroups.ContainsKey(ID))
            {
                g = CompanionGroups[ID];
                if (!g.RecognizeAsTerraGuardian && RecognizeAsTerraGuardian)
                    g.RecognizeAsTerraGuardian = true;
                return g;
            }
            g = new Group();
            g.Name = Name;
            g.CustomSprite = CustomSprite;
            g.RecognizeAsTerraGuardian = RecognizeAsTerraGuardian;
            CompanionGroups.Add(ID, g);
            return g;
        }

        public static GuardianID[] GetPossibleStarterGuardians()
        {
            List<GuardianID> PossibleGuardians = new List<GuardianID>();
            PossibleGuardians.AddRange(InitialGuardians);
            foreach (Terraria.IO.PlayerFileData pfd in Main.PlayerList)
            {
                try
                {
                    PlayerMod pm = pfd.Player.GetModPlayer<PlayerMod>();
                    if (pm == null)
                    {
                        continue;
                    }
                    foreach (GuardianData gd in pm.MyGuardians.Values)
                    {
                        if (!PossibleGuardians.Any(x => x.ID == gd.ID && x.ModID == gd.ModID) && !gd.Base.InvalidGuardian)
                        {
                            PossibleGuardians.Add(new GuardianID(gd.ID, gd.ModID));
                        }
                    }
                }
                catch { }
            }
            if (PossibleGuardians.Count == 0)
            {
                PossibleGuardians.Add(new GuardianID(0, ""));
            }
            return PossibleGuardians.ToArray();
        }

        public override object Call(params object[] args)
        {
            if(args.Length > 1 && (string)args[0] == "getothermodtargets")
            {
                //return Compatibility.TerraClassesCompatibility.GetTargets((Player)args[1], (bool)args[2]);
            }
            return base.Call(args);
        }

        public static int CalculateMessageTime(string s)
        {
            int Time = 300;
            foreach (char c in s)
            {
                if (c == '.' || c == ':')
                {
                    Time += 7;
                }
                else if (c == ',' || c == ';')
                {
                    Time += 5;
                }
                else if (c != ' ' && c != '\n')
                {
                    Time++;
                }
            }
            return Time;
        }

        public static Group GetGroup(string ID)
        {
            if (CompanionGroups.ContainsKey(ID))
                return CompanionGroups[ID];
            return AddNewGroup(ID, ID);
        }

        public static void AddActiveGuardian(TerraGuardian Guardian, bool Force = false)
        {
            if (!TempActiveGuardians.ContainsKey(Guardian.WhoAmID))
            {
                TempActiveGuardians.Add(Guardian.WhoAmID, Guardian);
            }
            if (Force && !ActiveGuardians.ContainsKey(Guardian.WhoAmID))
            {
                ActiveGuardians.Add(Guardian.WhoAmID, Guardian);
            }
        }

        public static void GetInitialCompanionsList()
        {
            InitialGuardians.Clear();
            InitialGuardians.Add(new GuardianID(GuardianBase.Rococo));
            InitialGuardians.Add(new GuardianID(GuardianBase.Blue));
            InitialGuardians.Add(new GuardianID(GuardianBase.Cinnamon));
            InitialGuardians.Add(new GuardianID(GuardianBase.Luna));
        }

        public static List<Terraria.ModLoader.Config.ItemDefinition> GetDefaultDualwieldableItems()
        {
            List<Terraria.ModLoader.Config.ItemDefinition> items = new List<Terraria.ModLoader.Config.ItemDefinition>();
            //PHM melee weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.WoodenSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BorealWoodSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CopperBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PalmWoodSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RichMahoganySword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CactusSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.EbonwoodSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.IronBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ShadewoodSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.LeadBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BladedGlove));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TungstenBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ZombieArm));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.GoldBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AntlionClaw)); //This is actually the Mandible Blade
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StylistKilLaKillScissorsIWish));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PlatinumBroadsword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BoneSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Katana));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.IceBlade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Muramasa));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Arkhalis));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.DyeTradersScimitar));
            //Phaseblades and Phasesabers
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BluePhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BluePhasesaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.GreenPhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.GreenPhasesaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PurplePhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PurplePhasesaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RedPhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RedPhasesaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.WhitePhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.WhitePhasesaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.YellowPhaseblade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.YellowPhasesaber));
            //PHM melee weapons continue
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BloodButcherer));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Starfury));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.EnchantedSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BeeKeeper));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FalconBlade));
            //HM melee weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PearlwoodSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TaxCollectorsStickOfDoom));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SlapHand));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CobaltSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PalladiumSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(3823)); //Brand of Inferno
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.MythrilSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.OrichalcumSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Cutlass));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Frostbrand));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AdamantiteSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BeamSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TitaniumSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FetidBaghnakhs));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Bladetongue));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Excalibur));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ChlorophyteSaber));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PsychoKnife));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Keybrand));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TheHorsemansBlade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ChristmasTreeSword));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Seedler));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TerraBlade));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.InfluxWaver));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StarWrath));

            //HM Repeaters
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CobaltRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PalladiumRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.MythrilRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.OrichalcumRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AdamantiteRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TitaniumRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.HallowedRepeater));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ChlorophyteShotbow));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StakeLauncher));

            //PHM guns
            //items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RedRyder));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FlintlockPistol));
            //items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Musket));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TheUndertaker));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Revolver));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Handgun));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PhoenixBlaster));

            //HM guns
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Uzi));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.VenusMagnum));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CandyCornRifle));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BorealWood));

            //Other guns
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SnowballCannon));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PainterPaintballGun));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StarCannon));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Toxikarp));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.DartPistol));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Flamethrower));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.EldMelter));

            //PHM magic weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AmethystStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.TopazStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SapphireStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.EmeraldStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RubyStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.DiamondStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AmberStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Vilethorn));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.MagicMissile));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AquaScepter));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Flamelash));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FlowerofFire));

            //HM magic weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FlowerofFrost));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SkyFracture));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CrystalSerpent));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.CrystalVileShard));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.MeteorStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.UnholyTrident));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PoisonStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FrostStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RainbowRod));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.VenomStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.NettleBurst));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ShadowbeamStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.InfernoFork));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SpectreStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StaffofEarth));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BatScepter));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Razorpine));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BlizzardStaff));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(3870)); //Betsy's Wrath

            //PHM Magic guns
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.SpaceGun));
            //HM Magic guns
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.LaserRifle));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.LeafBlower));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.HeatRay));

            //HM other magic weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.MagicDagger));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ToxicFlask));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.NebulaBlaze));

            //PHM thrown weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Shuriken));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.ThrowingKnife));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.PoisonedKnife));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Snowball));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.AleThrowingGlove));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Bone));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BoneGlove));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.RottenEgg));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.StarAnise));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.FrostDaggerfish));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.Javelin));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BoneJavelin));
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(ItemID.BoneDagger));
            return items;
        }

        public static Dictionary<Terraria.ModLoader.Config.ItemDefinition, int> GetDefaultItemRanges()
        {
            Dictionary<Terraria.ModLoader.Config.ItemDefinition, int> items = new Dictionary<Terraria.ModLoader.Config.ItemDefinition, int>();
            //Sickle
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.IceSickle), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.DeathSickle), 160);
            //Boomerang
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.WoodenBoomerang), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.IceBoomerang), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.EnchantedBoomerang), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FruitcakeChakram), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BloodyMachete), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ThornChakram), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Wrench), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Flamarang), 240);

            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FlyingKnife), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.LightDisc), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Bananarang), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.PossessedHatchet), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.PaladinsHammer), 320);

            //Flails
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ChainKnife), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BallOHurt), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.TheMeatball), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BlueMoon), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Sunfury), 160);

            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Anchor), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.KOCannon), 17 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ChainGuillotines), 22 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.DaoofPow), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FlowerPow), 240);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.GolemFist), 20 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Flairon), 240);

            //Other(Melee)
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ShadowFlameKnife), 160);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.VampireKnives), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.DayBreak), 320);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.SolarEruption), 320);

            //Throwing Weapons
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Shuriken), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ThrowingKnife), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.PoisonedKnife), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Snowball), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.AleThrowingGlove), 10 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.SpikyBall), 3 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Bone), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.RottenEgg), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.StarAnise), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.MolotovCocktail), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FrostDaggerfish), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Javelin), 16 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BoneJavelin), 16 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BoneDagger), 12 * 16);

            //Grenades
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Grenade), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.StickyGrenade), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.BouncyGrenade), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Beenade), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.PartyGirlGrenade), 8 * 16);

            //Others(Ranged)
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.SnowballCannon), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Harpoon), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Blowpipe), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Blowgun), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Flamethrower), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.EldMelter), 12 * 16);

            //Wands
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.Vilethorn), 14 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.AquaScepter), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FlowerofFire), 8 * 16);

            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.FlowerofFrost), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.CrystalVileShard), 14 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.NettleBurst), 23 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.RainbowGun), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.GoldenShower), 20 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.MagicDagger), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.MedusaHead), 8 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ToxicFlask), 12 * 16);
            items.Add(new Terraria.ModLoader.Config.ItemDefinition(Terraria.ID.ItemID.ShadowFlameHexDoll), 12 * 16);
            return items;
        }

        /*public static double TimeToTerrariaTime(int Hours, int Minutes, out bool Daytime)
        {
            double ResultTime = Hours * 3600 + (double)Minutes / 60;
            if (Hours * 60 + Minutes < 270) // 4:30
            {
                ResultTime -= 27000;
            }

            return ResultTime;
        }*/

        public static void TerrariaTimeToTime(double Time, bool Daytime, out int Hours, out int Minutes)
        {
            Hours = Minutes = 0;
            if (!Daytime)
                Time += 54000;
            Time = Time / 86400 * 24;
            Time = Time - 7.5; //- 12
            if (Time < 0)
                Time += 24;
            Hours = (int)Time;
            Minutes = (int)((Time - (int)Time) * 60);
        }

        public static bool IsSolidTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            return tile.active() && Main.tileSolid[tile.type];
        }

        public static bool CanPassThroughThisTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            return !tile.active() || !Main.tileSolid[tile.type] || ((Main.tileSolid[tile.type] && tile.type == Terraria.ID.TileID.Platforms) || tile.type == Terraria.ID.TileID.ClosedDoor || tile.type == Terraria.ID.TileID.TallGateClosed);
        }

        public static bool IsDangerousTile(int x, int y, bool FireblockProtection)
        {
            Tile tile = GetTile(x, y);
            if ((!tile.active() || tile.active() && !Main.tileSolid[tile.type]) && tile.lava() && tile.liquid >= 16)
            {
                return true;
            }
            if (tile.active())
            {
                if (tile.type == Terraria.ID.TileID.Spikes || tile.type == Terraria.ID.TileID.WoodenSpikes)
                {
                    return true;
                }
                if (!FireblockProtection && (tile.type == Terraria.ID.TileID.Meteorite || tile.type == Terraria.ID.TileID.Hellstone || 
                    tile.type == Terraria.ID.TileID.HellstoneBrick))
                {
                    return true;
                }
            }
            return false;
        }

        public MainMod()
		{
            mod = this;
		}

        public static void AddInitialGuardian(GuardianID ID)
        {
            InitialGuardians.Add(ID);
        }

        public static void AddGuardianList(Mod ModName, GuardianBase.ModGuardianDB guardiandb)
        {
            if (ModGuardianLoader.ContainsKey(ModName.Name))
            {
                ModGuardianLoader.Remove(ModName.Name);
            }
            ModGuardianLoader.Add(ModName.Name, guardiandb);
        }

        public static bool TryGettingGuardianInfo(int ID, Mod mod, out GuardianBase guardian)
        {
            guardian = null;
            if (mod == null)
                return false;
            if (!ModGuardianLoader.ContainsKey(mod.Name))
            {
                return false;
            }
            ModGuardianLoader[mod.Name](ID, out guardian);
            return true;
        }

        public static bool CanGiveFreeNemesis()
        {
            if (Main.halloween)
                return true;
            DateTime dt = DateTime.Now;
            return dt.Year == 2019 && dt.Month == 10;
        }

        public static bool CanGiveFreeVladimir()
        {
            DateTime dt = DateTime.Now;
            return dt.Month == 5 && dt.Day >= 4 && dt.Day <= 19;//dt.Year == 2020 && (dt.Month < 5 || (dt.Month == 5 && dt.Day < 19));
        }

        public static bool CanGiveFreeLiebre()
        {
            DateTime dt = DateTime.Now;
            return dt.Year == 2021 && dt.Month == 10;
        }

        public static bool IsGuardianInTheWorld(int ID, string ModId = "")
        {
            if (ModId == "")
                ModId = mod.Name;
            foreach (TerraGuardian tg in ActiveGuardians.Values)
            {
                if (tg.ID == ID && tg.ModID == ModId && (Main.netMode == 0 || tg.OwnerPos == -1))
                    return true;
            }
            return false;
        }

        public static bool IsGuardianItem(Item item)
        {
            return item.modItem is Items.GuardianItemPrefab;
        }

        public static int LoadSound(string Directory)
        {
            return mod.GetSoundSlot(SoundType.Custom, Directory);
        }

        public override void MidUpdateGoreProjectile()
        {
            foreach (int p in ProjMod.GuardianProj.Keys)
            {
                if (Main.projectile[p].active) //Deduct num minions and their slots, and increase max minions
                {
                    Projectile proj = Main.projectile[p];
                    if (Main.projectile[p].minion)
                    {
                        //Main.player[proj.owner].numMinions--;
                        //Main.player[proj.owner].slotsMinions -= proj.minionSlots;
                        Main.player[proj.owner].maxMinions++;
                    }
                    else if (Main.projectile[p].sentry)
                    {
                        Main.player[proj.owner].maxTurrets++;
                    }
                }
            }
        }

        public override void MidUpdateProjectileItem()
        {

        }

        public override void MidUpdateItemDust()
        {
            ItemMod.RefreshItemsSpawnedLists();
        }

        public override void PreUpdateEntities()
        {
            Main.ignoreErrors = false;
            ActiveGuardians = TempActiveGuardians;
            TempActiveGuardians = new Dictionary<int, TerraGuardian>();
            DrawMoment.Clear();
            if (Gameplay2PMode)
            {
                byte ControllerCount = 0;
                for(byte i = 0; i < 4; i++)
                {
                    GamePadState gps = GamePad.GetState((PlayerIndex)i);
                    if (gps.IsConnected)
                        ControllerCount++;
                }
                if(ControllerCount == 1)
                {
                    Update2PKeyboardControls();
                }
            }
        }

        public static void Update2PKeyboardControls()
        {

        }

        public override void MidUpdateNPCGore()
        {
            NpcMod.RestorePlayersPosition();
            CheckIfThereIsNpcInCameraRange();
            if (Main.netMode < 2 && !Main.gameMenu)
            {
                UpdateReactions();
            }
        }

        public void UpdateReactions()
        {
            if (!LastMoonLordKilled && NPC.downedMoonlord)
            {
                GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.KilledMoonLordFirstTime, Main.player[Main.myPlayer].name, Main.worldName, 30, 45,
                    GuardianGlobalInfos.GetGuardiansInTheWorld());
            }
            LastMoonLordKilled = NPC.downedMoonlord;
            if (!LastHardMode && Main.hardMode)
            {
                GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.StartedHardMode, Main.player[Main.myPlayer].name, Main.worldName, 15, 45,
                    GuardianGlobalInfos.GetGuardiansInTheWorld());
            }
            LastHardMode = Main.hardMode;
            if (LastBossSpotted)
            {
                bool HasBossAlive = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && NpcMod.IsBoss(Main.npc[i]))
                    {
                        HasBossAlive = true;
                        break;
                    }
                }
                LastBossSpotted = HasBossAlive;
            }
            else
            {
                bool BossInRange = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].active && NpcMod.IsBoss(Main.npc[i]))
                    {
                        if (Math.Abs(Main.player[Main.myPlayer].Center.X - Main.npc[i].Center.X) < 450f &&
                            Math.Abs(Main.player[Main.myPlayer].Center.Y - Main.npc[i].Center.Y) < 380f && 
                            Collision.CanHitLine(Main.player[Main.myPlayer].position, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height,
                            Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
                        {
                            BossInRange = true;
                            break;
                        }
                    }
                }
                if (BossInRange)
                {
                    LastBossSpotted = true;
                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.SpottedABoss);
                }
            }
            if (Main.invasionType > InvasionID.None || Main.invasionSize > 0) //Invasion happening
            {
                if (!LastInvasionSpotted)
                {
                    Player player = Main.player[Main.myPlayer];
                    if (player.position.Y < Main.worldSurface * 16 + NPC.sHeight &&
                        Math.Abs(player.position.X - Main.invasionX * 16) < 3000)
                    {
                        LastInvasionSpotted = true;
                        LastInvasion = (sbyte)Main.invasionType;
                        player.GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.InvasionBegins);
                    }
                }
            }
            else
            {
                if (LastInvasionSpotted)
                {
                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.RepelledInvasion);
                    LastInvasionSpotted = false;
                    string InvasionName = "";
                    switch ((short)LastInvasion)
                    {
                        case InvasionID.GoblinArmy:
                            InvasionName = "Goblin Army";
                            break;
                        case InvasionID.SnowLegion:
                            InvasionName = "Frost Legion";
                            break;
                        case InvasionID.PirateInvasion:
                            InvasionName = "Pirate Invasion";
                            break;
                        case InvasionID.MartianMadness:
                            InvasionName = "Martian Madness";
                            break;
                    }
                    GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.EventFinished,
                        Main.player[Main.myPlayer].name, InvasionName, 12, 15,
                        GuardianGlobalInfos.GetGuardiansInTheWorld());
                }
            }
            bool AnEventIsHappening = Main.bloodMoon || Main.eclipse || Main.pumpkinMoon || Main.snowMoon;
            if (AnEventIsHappening)
            {
                if (!LastEventStarted)
                {
                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.EventBegins);
                    LastEventStarted = true;
                    if (Main.slimeRain)
                        LastEvent = 1;
                    if (Main.bloodMoon)
                        LastEvent = 2;
                    else if (Main.eclipse)
                        LastEvent = 3;
                    if (Main.pumpkinMoon)
                        LastEvent = 4;
                    if (Main.snowMoon)
                        LastEvent = 5;
                }
            }
            else
            {
                if (LastEventStarted)
                {
                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().CompanionReaction(GuardianBase.MessageIDs.EventEnds);
                    LastEventStarted = false;
                    string EventName = "";
                    switch (LastEvent)
                    {
                        case 1:
                            EventName = "Slime Rain";
                            break;
                        case 2:
                            EventName = "Blood Moon";
                            break;
                        case 3:
                            EventName = "Eclipse";
                            break;
                        case 4:
                            EventName = "Pumpkin Moon";
                            break;
                        case 5:
                            EventName = "Frost Moon";
                            break;
                    }
                    GuardianGlobalInfos.AddFeat(FeatMentioning.FeatType.EventFinished,
                        Main.player[Main.myPlayer].name, EventName, 12, LastEvent,
                        GuardianGlobalInfos.GetGuardiansInTheWorld());
                    LastEvent = 0;
                }
            }
        }

        public void CheckIfThereIsNpcInCameraRange()
        {
            Rectangle rect = new Rectangle((int)Main.screenPosition.X - 800, (int)Main.screenPosition.Y - 800, Main.screenWidth + 1600, Main.screenHeight + 1600);
            NpcInCameraRange = false;
            for (int n = 0; n < 200; n++)
            {
                if(Main.npc[n].active && rect.Intersects(Main.npc[n].getRect()))
                {
                    NpcInCameraRange = true;
                    return;
                }
            }
        }

        public override void MidUpdatePlayerNPC()
        {
            GuardianBountyQuest.Update();
            NemesisFadeEffect++;
            if (NemesisFadeEffect >= NemesisFadingTime)
                NemesisFadeEffect -= NemesisFadeCooldown + NemesisFadingTime;
            /*for (int Assists = 0; Assists < MaxExtraGuardianFollowers + 1; Assists++)
            {
                if (Main.netMode > 0)
                    break;
                List<PlayerDataBackup> DataBackup = new List<PlayerDataBackup>();
                bool[] PlayerWasActive = new bool[255];
                bool AtLeastOne = false;
                for (int p = 0; p < 255; p++)
                {
                    PlayerWasActive[p] = Main.player[p].active;
                    if (Main.player[p].active)
                    {
                        PlayerMod pm = Main.player[p].GetModPlayer<PlayerMod>();
                        if (pm != null)
                        {
                            TerraGuardian g = pm.GetAllGuardianFollowers[Assists];
                            if (g.Active && !g.Downed && !g.PlayerMounted && !g.SittingOnPlayerMount && (Math.Abs(Main.player[p].Center.X - g.Position.X) >= NPC.sWidth * 2 || Math.Abs(Main.player[p].Center.Y - g.CenterY) >= NPC.sHeight * 2))// && Main.player[p].Distance(Main.player[p].GetModPlayer<PlayerMod>().Guardian.CenterPosition) >= 1512f)
                            {
                                PlayerDataBackup db = new PlayerDataBackup(Main.player[p], Main.player[p].GetModPlayer<PlayerMod>().Guardian);
                                DataBackup.Add(db);
                                AtLeastOne = true;
                            }
                            else
                            {
                                Main.player[p].active = false;
                            }
                        }
                    }
                }
                if(AtLeastOne)
                    NPC.SpawnNPC();
                for (int p = 0; p < 255; p++)
                {
                    Main.player[p].active = PlayerWasActive[p];
                }
                foreach (PlayerDataBackup db in DataBackup)
                {
                    db.RestorePlayerStatus();
                }
            }*/
            if (LastWof && Main.wof == -1)
            {
                for (int p = 0; p < 255; p++)
                {
                    if (Main.player[p].active)
                    {
                        foreach (GuardianData gd in Main.player[p].GetModPlayer<PlayerMod>().GetGuardians())
                        {
                            gd.WofFood = false;
                        }
                    }
                }
            }
            AlexRecruitScripts.AlexNPCPosition = -1;
            Npcs.BrutusNPC.TrySpawningBrutus();
            Npcs.MabelNPC.TrySpawningMabel();
            GuardianSpawningScripts.TrySpawningMichelle();
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Netplay.GetMessage(reader, whoAmI);
        }

        public override void PostUpdateEverything()
        {
            if (!Main.gameMenu)
                TerraGuardian.UpdateAge = false;
            SoulSaved = false;
            GuardianNpcHousingCheckCooldown++;
            if (GuardianNpcHousingCheckCooldown > MaxGuardianNpcHousingCheckCooldown)
            {
                GuardianNpcHousingCheckCooldown -= MaxGuardianNpcHousingCheckCooldown;
            }
            CarpetAnimationTime += 0.167f;
            if (CarpetAnimationTime >= 6)
                CarpetAnimationTime -= 6;
            ForceUpdateGuardiansStatus = false;
            LastWof = Main.wof > -1;
            if (ReviveTalkDelay > 0)
                ReviveTalkDelay--;
            LastEventWave = Main.invasionProgressWave;
            for (int dm = 0; dm < MainMod.DrawMoment.Count; dm++)
            {
                if (!ActiveGuardians.ContainsKey(MainMod.DrawMoment[dm].GuardianWhoAmID))
                {
                    MainMod.DrawMoment.RemoveAt(dm);
                }
            }
            if (!Main.gameMenu && !Main.gamePaused)
                GuardianShopHandler.UpdateShops();
            if (GuardianMood.MoodUpdateDelay == 0)
                GuardianMood.MoodUpdateDelay += 9;
            else
                GuardianMood.MoodUpdateDelay--;
            if(Main.netMode < 2 && !Main.gameMenu)
            {
                if (Main.player[Main.myPlayer].chatOverhead.timeLeft > LastChatTime)
                {
                    if (Main.player[Main.myPlayer].chatOverhead.chatText.Trim().ToLower().Contains("daphne") && !NpcMod.HasMetGuardian(GuardianBase.Daphne) && !NPC.AnyNPCs(ModContent.NPCType<Npcs.DaphneNPC>()) && Main.rand.NextFloat() < 0.25f)
                    {
                        int NpcID = ModContent.NPCType<Npcs.DaphneNPC>();
                        NPC.SpawnOnPlayer(Main.myPlayer, NpcID);
                        if (NPC.AnyNPCs(NpcID))
                        {
                            NPC npc = Main.npc[NPC.FindFirstNPC(NpcID)];
                            Npcs.DaphneNPC danpc = (Npcs.DaphneNPC)npc.modNPC;
                            npc.target = Main.myPlayer;
                            danpc.MeetStep = 1;
                        }
                    }
                }
                LastChatTime = Main.player[Main.myPlayer].chatOverhead.timeLeft;
            }
            WorldMod.AnalyzeDrawMoment();
            if (GeneralIdleCommentCooldown > 0)
                GeneralIdleCommentCooldown--;
            GuardianGlobalInfos.UpdateFeatTime();
            GuardianGlobalInfos.UpdateGlobalInfos();
            if (CompanionFaceJS > 0)
                UpdateJumpscare();
            if (Main.halloween)
            {
                if(Main.LocalPlayer.chest != Main.LocalPlayer.lastChest && Main.LocalPlayer.lastChest == -1 && Main.rand.Next(256) == 0)
                {
                    TriggerAnyPlayerCompanionJS();
                }
            }
        }

        private static void UpdateJumpscare()
        {
            CompanionFaceJS--;
        }

        private static bool DrawAlexJumpscare()
        {
            float Percentage = 1f - (float)CompanionFaceJS / CompanionFaceJSMaxTime;
            Vector2 HeadPosition = new Vector2(Main.screenWidth, Main.screenHeight) * 0.1f;
            float Scale = 1f + Percentage * 3;
            HeadPosition *= Scale;
            GuardianBase guardianbase = GuardianBase.GetGuardianBase(CompanionFaceJSID, CompanionFaceJSModID);
            if (!guardianbase.IsCustomSpriteCharacter || guardianbase.sprites.HasErrorLoadingOccurred)
                return true;
            if (!guardianbase.sprites.IsTextureLoaded)
            {
                guardianbase.sprites.LoadTextures();
            }
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black);
            Texture2D texture = guardianbase.sprites.HeadSprite;
            if (texture == null)
                texture = MainMod.LosangleOfUnnown;
            Main.spriteBatch.Draw(texture, HeadPosition, null, Color.DarkRed, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
            return true;
        }

        private static LegacyGameInterfaceLayer gi, downedInterface, dgi, hsi, gsi, goi, gmi, dnagd, dgdi, dgmo, dghmi, dghmig, bmsi, dgrb, dcs, umos, dngh,dgqi,alexjslayer,dsi,d2pi;
        private static bool InterfacesSetup = false;

        public override void ModifyInterfaceLayers(List<Terraria.UI.GameInterfaceLayer> layers)
        {
            const int TownNpcHeadBannersLayer = 7, EntityHealthBarLayer = 14, NpcSignDialogueLayer = 23, HealthBarLayer = 25, InventoryLayer = 27, HotbatLayer = 30, 
                MouseTextLayer = 33, PlayerChatLayer = 34, PlayerDeathLayer = 35, CursorLayer = 36, MouseOverLayer = 39;
            try
            {
                if (!InterfacesSetup)
                {
                    gi = new LegacyGameInterfaceLayer("Terra Guardians: Debug Interface", DrawDebugInterface, Terraria.UI.InterfaceScaleType.UI);
                    downedInterface = new LegacyGameInterfaceLayer("Terra Guardians: Downed Interface", DrawDownedInterface, InterfaceScaleType.UI);
                    dgi = new LegacyGameInterfaceLayer("Terra Guardians: Inventory Interface", DrawGuardianInventoryInterface, InterfaceScaleType.UI);
                    hsi = new LegacyGameInterfaceLayer("Terra Guardians: Status Interface", DrawGuardianHealthStatusInterface, InterfaceScaleType.UI);
                    gsi = new LegacyGameInterfaceLayer("Terra Guardians: Selection Interface", DrawGuardianSelectionInterface, InterfaceScaleType.UI);
                    goi = new LegacyGameInterfaceLayer("Terra Guardians: Order Selection Interface", DrawGuardianOrderInterface, InterfaceScaleType.UI);
                    gmi = new LegacyGameInterfaceLayer("Terra Guardians: Mouse Interface", DrawGuardianMouse, InterfaceScaleType.Game);
                    dnagd = new LegacyGameInterfaceLayer("Terra Guardians: Chat Messages Interface", DrawNpcsAndGuardianChatMessages, InterfaceScaleType.Game);
                    dgrb = new LegacyGameInterfaceLayer("Terra Guardians: Revive Bar Interface", DrawGuardianReviveBar, InterfaceScaleType.Game);
                    dgdi = new LegacyGameInterfaceLayer("Terra Guardians: Dialogue Inteface", DrawGuardianDialogueInterface, InterfaceScaleType.UI);
                    dgmo = new LegacyGameInterfaceLayer("Terra Guardians: Mouse Over", DrawGuardianMouseOverInterface, InterfaceScaleType.Game);
                    dghmi = new LegacyGameInterfaceLayer("Terra Guardians: House Management", DrawGuardianHouseManagementInterface, InterfaceScaleType.UI);
                    dghmig = new LegacyGameInterfaceLayer("Terra Guardians: House Management Game UI", DrawGuardianHouseManagementInterfaceGame, InterfaceScaleType.Game);
                    dcs = new LegacyGameInterfaceLayer("Terra Guardians: Colored Screen", DrawColoredScreen, InterfaceScaleType.UI);
                    umos = new LegacyGameInterfaceLayer("Terra Guardians: Update Mouse Over Revive", UpdateMouseOverScript, InterfaceScaleType.Game);
                    dngh = new LegacyGameInterfaceLayer("Terra Guardians: Draw Nearby Guardian Head", DrawCompanionPointingArrow, InterfaceScaleType.UI);
                    alexjslayer = new LegacyGameInterfaceLayer("Terra Guardians: Alex Jump Scare", DrawAlexJumpscare, InterfaceScaleType.UI);
                    dsi = new LegacyGameInterfaceLayer("Terra Guardians: Season Info Interface", DrawSeasonInfos, InterfaceScaleType.UI);
                    d2pi = new LegacyGameInterfaceLayer("Terra Guardians: Draw 2P Interface", Draw2PInterface, InterfaceScaleType.UI);
                    bmsi = new LegacyGameInterfaceLayer("Terra Guardians: Buddy Mode Hud", delegate()
                    {
                        BuddyModeSetupInterface.Draw();
                        return true;
                    }, InterfaceScaleType.UI);
                    dgqi = new LegacyGameInterfaceLayer("Terra Guardians: Test Quest Interface", delegate ()
                    {
                        GuardianQuestInterface.Draw();
                        return true;
                    }, InterfaceScaleType.UI);
                    InterfacesSetup = true;
                }
                //Todo - This is only for testing purpose
                if (ShowDebugInfo)
                {
                    layers.Add(gi);
                }
                layers.Insert(CursorLayer, gsi);
                layers.Insert(CursorLayer, dgqi);
                if (BuddyModeSetupInterface.WindowActive)
                {
                    layers.Insert(CursorLayer, bmsi);
                }
                if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().KnockedOut)
                {
                    if (Main.playerInventory)
                        Main.playerInventory = false;
                    if (!Main.player[Main.myPlayer].dead)
                        layers.Clear();
                    layers.Add(hsi);
                    layers.Add(downedInterface);
                    //layers.Insert(PlayerDeathLayer, downedInterface);
                    //layers.Insert(HealthBarLayer - 1, hsi);
                    return;
                }
                layers.Insert(PlayerChatLayer, dnagd);
                if(Gameplay2PMode)
                    layers.Insert(HotbatLayer, d2pi);
                layers.Insert(HotbatLayer, goi);
                layers.Insert(MouseOverLayer, gmi);
                layers.Insert(InventoryLayer, dsi);
                layers.Insert(InventoryLayer, dgi);
                layers.Insert(NpcSignDialogueLayer, dgdi);
                layers.Insert(HealthBarLayer - 1, hsi);
                layers.Insert(EntityHealthBarLayer, dgrb);
                layers.Insert(TownNpcHeadBannersLayer, dghmi);
                layers.Insert(TownNpcHeadBannersLayer, dghmig);
                layers.Insert(TownNpcHeadBannersLayer, umos);
                layers.Insert(TownNpcHeadBannersLayer, dcs);
                layers.Insert(TownNpcHeadBannersLayer, dgmo);
                layers.Insert(0, dngh);
                bool RemoveHealthBarAndInventory = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.PlayerControl;
                for (int l = 0; l < layers.Count; l++)
                {
                    if (RemoveHealthBarAndInventory && layers[l].Name.Contains("Resource Bars"))
                    {
                        layers.RemoveAt(l);
                    }
                    if (RemoveHealthBarAndInventory && layers[l].Name.Contains("Inventory") && !layers[l].Name.Contains("Terra Guardians"))
                    {
                        layers.RemoveAt(l);
                    }
                }
                if (CompanionFaceJS > 0)
                    layers.Add(alexjslayer);
            }
            catch
            {

            }
        }

        public static bool DrawSeasonInfos()
        {
            if (!Main.playerInventory)
                return true;
            Vector2 Position = new Vector2(Main.screenWidth - 304, 88);
            Position.Y += Utils.DrawBorderStringBig(Main.spriteBatch, GuardianGlobalInfos.GetSeason.ToString(), Position, Color.White, 0.6f, 1f).Y;
            Position.Y += Utils.DrawBorderString(Main.spriteBatch, "Day: " + (GuardianGlobalInfos.GetDay + 1), Position, Color.White, 1f, 1f).Y;
            Position.Y += Utils.DrawBorderString(Main.spriteBatch, "Ether Realm Time: " + GuardianGlobalInfos.GetTime.ToString(@"h\:mm"), Position, Color.White, 1f, 1f).Y;
            {
                Vector2 WeekDaysPosition = new Vector2(Position.X, Position.Y);
                WeekDaysPosition.X -= 8;
                for(byte i = 6; i < 254; i--)
                {
                    char Letter = ' ';
                    switch (i)
                    {
                        case 0:
                        case 6:
                            Letter = 'S';
                            break;
                        case 5:
                            Letter = 'F';
                            break;
                        case 4:
                        case 2:
                            Letter = 'T';
                            break;
                        case 3:
                            Letter = 'W';
                            break;
                        case 1:
                            Letter = 'M';
                            break;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, Letter.ToString(), WeekDaysPosition, (i == (byte)GuardianGlobalInfos.GetDayOfWeek ? Color.Yellow : Color.White), 1f, 0.5f);
                    WeekDaysPosition.X -= 18;
                }
            }
            return true;
        }

        public static bool UpdateMouseOverScript()
        {
            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().UpdateMouseOverSystem();
            return true;
        }

        public static bool DrawColoredScreen()
        {
            if (ScreenColorAlpha > 0)
            {
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), ScreenColor * ScreenColorAlpha);
            }
            return true;
        }

        public static bool DrawGuardianHouseManagementInterface()
        {
            GuardianHouseManagementInterface.Draw();
            return true;
        }

        public static bool DrawGuardianHouseManagementInterfaceGame()
        {
            GuardianHouseManagementInterface.DrawBannerGame();
            return true;
        }

        public static bool DrawGuardianMouseOverInterface()
        {
            GuardianMouseOverAndDialogueInterface.DrawMouseOver();
            if (ToReviveID > -1)
            {
                string MouseOverText = "";
                Vector2 Position = Vector2.Zero;
                if (ToReviveIsGuardian)
                {
                    if (ActiveGuardians.ContainsKey(ToReviveID))
                    {
                        MouseOverText = (IsReviving ? "Reviving " : "Revive ") + ActiveGuardians[ToReviveID].ReferenceName;
                        Position = ActiveGuardians[ToReviveID].Position;
                        Position.Y += 22;
                    }
                }
                else
                {
                    MouseOverText = (IsReviving ? "Reviving " : "Revive ") + Main.player[ToReviveID].name;
                    Position = Main.player[ToReviveID].position;
                    Position.X += Main.player[ToReviveID].width * 0.5f;
                    Position.Y += Main.player[ToReviveID].height + 22;
                }
                if(MouseOverText != "")
                {
                    Utils.DrawBorderString(Main.spriteBatch, MouseOverText, Position - Main.screenPosition, Color.White, 1f, 0.5f, 0f);
                }
            }
            return true;
        }

        public static bool DrawGuardianDialogueInterface()
        {
            GuardianMouseOverAndDialogueInterface.DrawDialogue();
            return true;
        }

        public static bool RectangleIntersects(Vector2 p1, int p1width, int p1height, Vector2 p2, int p2width, int p2height)
        {
            return p1.X < p2.X + p2width && p2.X < p1.X + p1width && p1.Y > p2.Y + p2height && p2.Y > p1.Y + p1height;
        }

        public static bool DrawDownedInterface()
        {
            //Add a script for when the player is controlling a guardian, instead.
            const int BarWidth = 360, BarHeight = 14;
            PlayerMod playerMod = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            float BarSize = (float)Main.player[Main.myPlayer].statLife / Main.player[Main.myPlayer].statLifeMax2;
            if (playerMod.RescueTime > 0)
                BarSize = 0f;
            if (Main.player[Main.myPlayer].statLife < 1)
                BarSize = 0f;
            if (BarSize > 1f)
                BarSize = 1f;
            if (BarSize < 0)
                BarSize = 0f;
            //
            float MaxBarsize = Main.screenHeight * 0.25f;
            int BarDimY = (int)((1f - BarSize) * MaxBarsize);
            if (playerMod.RescueWakingUp)
                BarSize = 0;
            Color ClosingBarsColor = playerMod.KnockedOutCold || playerMod.RescueWakingUp ? new Color(0, 0, 0, (int)(255 * (1f - BarSize))) : new Color((byte)(128 * (1f - BarSize)), 0, 0, (int)(255 * (1f - BarSize)));
            float ClosingBar2EffectPercentage = 0.5f + ((float)playerMod.RescueTime / (PlayerMod.MaxRescueTime * 60)) * 1f;
            if (ClosingBar2EffectPercentage > 1f)
                ClosingBar2EffectPercentage = 1f;
            if (playerMod.RescueWakingUp)
                ClosingBar2EffectPercentage = 1f;
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, BarDimY), ClosingBarsColor);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, BarDimY, Main.screenWidth, BarDimY), ClosingBarsColor * ClosingBar2EffectPercentage);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, (int)(Main.screenHeight - BarDimY * 2), Main.screenWidth, BarDimY), ClosingBarsColor * ClosingBar2EffectPercentage);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, (int)(Main.screenHeight - BarDimY), Main.screenWidth, BarDimY), ClosingBarsColor);
            //
            if (!Main.player[Main.myPlayer].dead && (!playerMod.KnockedOutCold || Main.player[Main.myPlayer].statLife > 1))
            {
                Vector2 BarPosition = new Vector2(Main.screenWidth * 0.5f - BarWidth * 0.5f - 2, Main.screenHeight * 0.8f - BarHeight * 0.5f - 2);
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)BarPosition.X, (int)BarPosition.Y, BarWidth + 4, BarHeight + 4), Color.Black);
                BarPosition.X += 2;
                BarPosition.Y += 2;
                Color c = Color.Green;
                if (BarSize < 0.333f)
                    c = Color.Red;
                else if (BarSize < 0.667f)
                    c = Color.Yellow;
                Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)BarPosition.X, (int)BarPosition.Y, (int)(BarWidth * BarSize), BarHeight), c);
            }
            if (!Main.player[Main.myPlayer].dead)
            {
                string Text = "Knocked Out";
                if (playerMod.ReviveBoost > 0)
                {
                    if (playerMod.NegativeReviveBoost)
                        Text = "Dying";
                    else
                        Text = "Being Revived";
                }
                Utils.DrawBorderStringBig(Main.spriteBatch, Text, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f), Color.OrangeRed, 1f, 0.5f, 0.5f);
                if (playerMod.KnockedOutCold)
                {
                    Text = (MainMod.PlayersDontDiesAfterDownedDefeat && !DoNotUseRescue ? "Hold '" + Main.cHook + "' to call for help." : "Hold '" + Main.cHook + "' to give up.");
                    Utils.DrawBorderString(Main.spriteBatch, Text, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f + 28), Color.OrangeRed, 1f, 0.5f, 0.5f);
                    if (playerMod.RescueTime > 0)
                    {
                        Text = (DoNotUseRescue ? "Giving up in " : "Rescue in ") + Math.Round(PlayerMod.MaxRescueTime - (float)playerMod.RescueTime / 60, 1) + " seconds";
                        Utils.DrawBorderString(Main.spriteBatch, Text, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f + 28 + 26), Color.OrangeRed, 0.85f, 0.5f, 0.5f);
                    }
                }
                else
                {
                    if (playerMod.RescueWakingUp)
                    {
                        Text = "Waking up...";
                        Utils.DrawBorderString(Main.spriteBatch, Text, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f + 28), Color.OrangeRed, 1f, 0.5f, 0.5f);
                    }
                }
            }
            return true;
        }

        public static void OpenGuardianSelectionInterface()
        {
            GuardianSelectionInterface.OpenInterface();
        }

        public bool DrawGuardianMouse()
        {
            TerraGuardian guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            if (guardian.Active && !guardian.PlayerControl && (ShowDebugInfo || Gameplay2PMode))
            {
                Main.spriteBatch.Draw(GuardianMouseTexture, guardian.AimDirection - Main.screenPosition, Main.mouseColor);
            }
            return true;
        }

        public bool DrawDebugInterface()
        {
            if (Main.playerInventory || Main.mapFullscreen)
                return true;
            Vector2 DrawPos = new Vector2(0, 138f + 64 * 2);
            TerraGuardian guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            string[] TextsToDraw = new string[0];
            /*if (!guardian.Active) return true;
            TextsToDraw = new string[]{
                "Selected Weapon Slot: " + guardian.SelectedItem,
                "Selected Off-hand Slot: " + guardian.SelectedOffhand,
                "Held Projectile: " + guardian.HeldProj,
                "Item Use Time: " + guardian.ItemAnimationTime + "/" + guardian.ItemMaxAnimationTime,
                "Item Use Type: " + guardian.ItemUseType.ToString(),
                "Item Rotation: " + MathHelper.ToDegrees(guardian.ItemRotation) + "",
                "Speed: " + guardian.Velocity,
                "Max Summon Count: " + guardian.NumMinions + "/" + guardian.MaxMinions,
                "Move Up: " + guardian.MoveUp,
                "Move Down: " + guardian.MoveDown,
                "Move Left: " + guardian.MoveLeft,
                "Move Right: " + guardian.MoveRight,
                "Action: " + guardian.Action,
                "Jump: " + guardian.Jump,
                "Jump Time: " + guardian.JumpHeight,
                "Stuck Timer: " + guardian.StuckTimer,
                (AlexRecruitScripts.SpawnedTombstone ? "Tombstone position: " + GuardianBountyQuest.GetDirectionText(new Vector2(AlexRecruitScripts.TombstoneTileX * 16, AlexRecruitScripts.TombstoneTileY * 16) - Main.player[Main.myPlayer].Center) : "No Tombstone in this world."),
                "Distance Travelled: " + (guardian.TravellingStacker * 0.5f) + "ft",
                "Combat Damage Stacked: " + guardian.DamageStacker + "%",
                "Afk Time: " + Math.Round((float)guardian.AfkCounter / 60, 1) + "s"
            };*/
            if(true)
            {
                List<string> New = new List<string>();
                foreach (TerraGuardian g in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (!g.Active) continue;
                    New.Add(g.Name);
                    New.Add("Moving Down: " + g.MoveDown);
                    New.Add("Jump? " + g.Jump);
                    /*New.Add(" Sub attack in use? " + g.SubAttackInUse);
                    if (g.SubAttackInUse)
                    {
                        New.Add(" ID: " + g.SpecialAttack.Name + "{" + g.SpecialAttack.ID + "}");
                        New.Add(" Time: " + g.SpecialAttack.Time);
                        New.Add(" Cooldown: " + g.SpecialAttack.Cooldown);
                    }*/
                }
                /*New.Clear();
                PlayerMod pm = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
                New.Add("Current exercise: " + pm.CurrentExercise.ToString());
                New.Add("Exercise progress: " + pm.ExerciseCounter);*/
                /*foreach(FeatMentioning feat in GuardianGlobalInfos.Feats)
                {
                    New.Add(feat.type.ToString() + " " + feat.PlayerName + " T:" + feat.FeatDurationInGameDays + " " + feat.FeatSubject);
                }*/
                TextsToDraw = New.ToArray();
            }
            foreach (string s in TextsToDraw)
            {
                Utils.DrawBorderString(Main.spriteBatch, s, DrawPos, Color.White);
                DrawPos.Y += 22;
            }
            return true;
        }

        public bool DrawNpcsAndGuardianChatMessages()
        {
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active)
                {
                    /*if (Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab)
                    {
                        GuardianNPC.GuardianNPCPrefab npc = ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC);
                        if (!PlayerMod.PlayerHasGuardianSummoned(Main.player[Main.myPlayer], npc.GuardianID, npc.GuardianModID) && !npc.Guardian.WofFood)
                        {
                            npc.Guardian.DrawReviveBar();
                            npc.Guardian.DrawSocialMessages();
                        }
                    }*/
                    if (Main.npc[n].modNPC is Npcs.GuardianActorNPC)
                    {
                        Npcs.GuardianActorNPC npc = ((Npcs.GuardianActorNPC)Main.npc[n].modNPC);
                        npc.DrawMessage(Main.spriteBatch);
                    }
                }
            }
            /*for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    foreach (TerraGuardian g in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                    {
                        if (g.Active && !g.WofFood)
                        {
                            g.DrawSocialMessages();
                        }
                    }
                }
            }*/
            foreach(TerraGuardian g in MainMod.ActiveGuardians.Values)
            {
                if (g.Active && !g.WofFood)
                {
                    g.DrawSocialMessages();
                }
            }
            return true;
        }

        public bool DrawGuardianReviveBar()
        {
            foreach (int key in ActiveGuardians.Keys)
            {
                if (!ActiveGuardians[key].WofFood)
                {
                    ActiveGuardians[key].DrawReviveBar();
                }
            }
            return true;
        }

        public bool DrawGuardianOrderInterface()
        {
            if (TestNewOrderHud)
            {
                GuardianOrderHudNew.Draw();
            }
            else
            {
                GuardianOrderHud.UpdateAndDraw();
            }
            return true;
        }

        public bool DrawGuardianSelectionInterface()
        {
            GuardianManagement.UpdateAndDraw();
            GuardianSelectionInterface.DrawInterface();
            return true;
        }

        public override void PostUpdateInput()
        {

        }

        public bool Draw2PInterface()
        {
            TerraGuardian Guardian = PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer);
            if(!Main.playerInventory)
            {
                int BackupSelectedItem = Main.LocalPlayer.selectedItem;
                Main.LocalPlayer.selectedItem = Guardian.SelectedItem;
                int LastSlot = Guardian.SelectedItem - 2;
                if (LastSlot < -1)
                    LastSlot = -1;
                if (LastSlot > 5)
                    LastSlot = 5;
                Main.inventoryScale = 0.75f;
                Vector2 TextPosition = new Vector2(304 + 150, 108);
                Utils.DrawBorderString(Main.spriteBatch, (Guardian.SelectedItem == -1 ? "Unnarmed" : Guardian.Inventory[Guardian.SelectedItem].Name), TextPosition, Color.White, anchory: 1);
                for (int i = 0; i < 5; i++)
                {
                    Vector2 InventoryPosition = new Vector2(304 + 40 * i, 108);
                    int SlotID = LastSlot + i;
                    if (SlotID == -1)
                    {
                        InventoryPosition.X += 20;
                        InventoryPosition.Y += 20;
                        Utils.DrawBorderStringBig(Main.spriteBatch, "X", InventoryPosition, SlotID == Guardian.SelectedItem ? Color.Yellow : Color.Red, 0.5f, 0.5f, 0.4f);
                    }
                    else
                    {
                        ItemSlot.Draw(Main.spriteBatch, Guardian.Inventory, 0, SlotID, InventoryPosition, Color.White * 0.5f);
                    }
                }
                Main.LocalPlayer.selectedItem = BackupSelectedItem;
            }
            return true;
        }

        public bool DrawPosDeathInterface()
        {
            Player p = Main.player[Main.myPlayer];
            if (!p.dead || p.ghost)
                return true;
            TerraGuardian g = p.GetModPlayer<PlayerMod>().Guardian;
            Vector2 TimerPosition = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.8f);
            float TimeToRespawn = (float)p.respawnTimer / 60;
            if (TimeToRespawn < 1)
                TimeToRespawn = (float)Math.Round(TimeToRespawn, 1);
            else
                TimeToRespawn = (int)TimeToRespawn;
            Utils.DrawBorderString(Main.spriteBatch, "Respawn in " + TimeToRespawn + "s", TimerPosition, Color.White, 1f, 0.5f, 0.5f);
            if (g.Active && g.FriendshipLevel >= 3)
            {

            }
            return true;
        }

        public bool DrawGuardianHealthStatusInterface()
        {
            if (Main.playerInventory || Main.mapFullscreen || Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().IsTalkingToAGuardian)
                return true;
            const float StartY = 138;
            Vector2 HealthbarPosition = new Vector2(16f, StartY);
            string MouseOverText = "";
            bool IsMainGuardian = true;
            foreach (TerraGuardian Guardian in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
            {
                float XSum = 0f;
                if (!Guardian.Active)
                {
                    IsMainGuardian = false;
                    continue;
                }
                HealthbarPosition.X = 16f;
                if (!IsMainGuardian)
                {
                    HealthbarPosition.X += 12;
                }
                if (!Guardian.Base.InvalidGuardian)
                {
                    if (Guardian.Base.IsCustomSpriteCharacter)
                    {
                        Vector2 HeadDrawPosition = HealthbarPosition;
                        HeadDrawPosition.X += 16;
                        HeadDrawPosition.Y += 16;
                        Guardian.DrawHead(HeadDrawPosition);
                        //Main.spriteBatch.Draw(HeadTexture, HeadDrawPosition, null, Color.White, 0f, new Vector2(HeadTexture.Width * 0.5f, HeadTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                        //HeadDrawPosition.X += 8f;
                        HeadDrawPosition.Y += 8f;
                        Guardian.DrawFriendshipHeart(HeadDrawPosition);
                        XSum = 46f;
                    }
                    else
                    {
                        Vector2 HeadDrawPosition = HealthbarPosition;
                        HeadDrawPosition.X += 16;
                        HeadDrawPosition.Y -= 8;
                        Guardian.DrawTerrarianHeadData(HeadDrawPosition);
                        //HeadDrawPosition.X += 8f;
                        HeadDrawPosition.Y += 24f;
                        Guardian.DrawFriendshipHeart(HeadDrawPosition);
                        XSum = 46f;
                    }
                }
                byte LifeCrystals = Guardian.GetUsedLifeCrystal,
                    LifeFruits = Guardian.GetUsedLifeFruit,
                    ManaCrystals = Guardian.GetUsedManaCrystal;
                string HealthText = Guardian.Name + ": " + Guardian.HP + "/" + Guardian.MHP;
                if (IsMainGuardian && Gameplay2PMode)
                    HealthText = "2P> " + HealthText;
                HealthbarPosition.X += XSum;
                Vector2 HealthTextPosition = HealthbarPosition;
                HealthbarPosition.Y += Utils.DrawBorderString(Main.spriteBatch, HealthText, HealthbarPosition, Color.White, 1f).Y;
                bool Downed = Guardian.KnockedOut || Guardian.Downed;
                Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(Downed ? 122 * 4 : 0, 0, 122, 16), Color.White);
                float MaxHealthBarScale = 1f;
                if (Guardian.HP < Guardian.MHP) MaxHealthBarScale = (float)Guardian.HP / Guardian.MHP;
                HealthbarPosition.X += 22;
                HealthbarPosition.Y += 4;
                if (UsingGuardianNecessitiesSystem)
                {
                    Vector2 IconStartPosition = HealthbarPosition;
                    IconStartPosition.X += 98 + 8;
                    if (Guardian.Data.Fatigue >= GuardianData.LightFatigueCount)
                    {
                        bool VeryTired = Guardian.Data.Fatigue >= GuardianData.HeavyFatigueCount;
                        Color color = VeryTired ? Color.Red : Color.White;
                        Main.spriteBatch.Draw(GuardianStatusIconTexture, IconStartPosition, new Rectangle(0, 0, 16, 16), color);
                        if (Main.mouseX >= IconStartPosition.X && Main.mouseX < IconStartPosition.X + 16 && Main.mouseY >= IconStartPosition.Y && Main.mouseY < IconStartPosition.Y + 16)
                        {
                            MouseOverText = (VeryTired ? "Exausted" : "Tired");
                        }
                        IconStartPosition.X += 16 + 4;
                    }
                    if (Guardian.Data.Injury >= GuardianData.LightWoundCount)
                    {
                        bool VeryTired = Guardian.Data.Injury >= GuardianData.HeavyWoundCount;
                        Color color = VeryTired ? Color.Red : Color.White;
                        Main.spriteBatch.Draw(GuardianStatusIconTexture, IconStartPosition, new Rectangle(16, 0, 16, 16), color);
                        if (Main.mouseX >= IconStartPosition.X && Main.mouseX < IconStartPosition.X + 16 && Main.mouseY >= IconStartPosition.Y && Main.mouseY < IconStartPosition.Y + 16)
                        {
                            MouseOverText = (VeryTired ? "Crippled" : "Wounded");
                        }
                        IconStartPosition.X += 16 + 4;
                    }
                }
                if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + 98 &&
                    Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + 8)
                {
                    MouseOverText = "Life Crystals used: " + LifeCrystals + "/" + TerraGuardian.MaxLifeCrystals + "  Life Fruits used: " + LifeFruits + "/" + TerraGuardian.MaxLifeFruit;
                    if (Guardian.Data.Injury > 0)
                        MouseOverText += "  [Injury " + Guardian.Data.Injury + "%]";
                    if (MainMod.SharedCrystalValues)
                    {
                        MouseOverText += "\nShared Life Crystal Value: " + Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeCrystalsUsed + "  Shared Life Fruit Value: " + Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeFruitsUsed;
                    }
                }
                int MaxHealthBarSize = 98;
                if(UsingGuardianNecessitiesSystem && Guardian.Data.Injury > 0 && !Downed)
                    MaxHealthBarSize = (int)(MaxHealthBarSize * (1f - (float)Guardian.Data.Injury / 100));
                if (Downed)
                {
                    int BarWidth = (int)(MaxHealthBarScale * 98);
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 4 + 22, 16 + 4, BarWidth, 8), Color.White);
                }
                else
                {
                    for (int y = 0; y < 2; y++)
                    {
                        int LCValue = LifeCrystals,
                            LFValue = LifeFruits;
                        if (SharedCrystalValues && y == 1)
                        {
                            LCValue = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeCrystalsUsed;
                            LFValue = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().LifeFruitsUsed;
                        }
                        Vector2 ThisHealthBarPosition = HealthbarPosition;
                        if (y == 1)
                            ThisHealthBarPosition.Y += 4;
                        for (int i = 0; i < 3; i++)
                        {
                            float BarScale = 1f;
                            switch (i)
                            {
                                case 0: //Red health bar
                                    BarScale = MaxHealthBarScale;
                                    break;
                                case 1:
                                    BarScale = (float)LCValue / TerraGuardian.MaxLifeCrystals;
                                    break;
                                case 2:
                                    BarScale = (float)LFValue / TerraGuardian.MaxLifeFruit;
                                    break;
                            }
                            if (BarScale > MaxHealthBarScale)
                                BarScale = MaxHealthBarScale;
                            //Draw bar
                            int BarWidth = (int)(MaxHealthBarSize * BarScale);
                            Main.spriteBatch.Draw(GuardianHealthBar, ThisHealthBarPosition, new Rectangle(22, 16 * (i + 1) + 4 + y * 4, BarWidth, 4), Color.White);
                        }
                    }
                }
                if (UsingGuardianNecessitiesSystem && Guardian.Data.Injury > 0)
                {
                    Vector2 InjuryPosition = HealthbarPosition;
                    int BarSize = 98 - MaxHealthBarSize;
                    InjuryPosition.X += MaxHealthBarSize;
                    Main.spriteBatch.Draw(GuardianHealthBar, InjuryPosition, new Rectangle(22 + MaxHealthBarSize, 16 * 4 + 4, BarSize, 8), Color.White);
                }
                {
                    HealthbarPosition.X -= 22;
                    HealthbarPosition.Y += 16;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 3, 0, 122, 16), Color.White);
                    float BarValue = ((float)Guardian.MP / Guardian.MMP);
                    HealthbarPosition.X += 22;
                    HealthbarPosition.Y += 4;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 3 + 22, 20 + 16, (int)(98 * BarValue), 8), Color.White);
                    for (int y = 0; y < 2; y++)
                    {
                        int MCValue = ManaCrystals;
                        if (MainMod.SharedCrystalValues && y == 1)
                        {
                            MCValue = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ManaCrystalsUsed;
                        }
                        float ManaCrystalProgress = (float)MCValue / GuardianData.MaxManaCrystals;
                        if (ManaCrystalProgress > BarValue)
                            ManaCrystalProgress = BarValue;
                        Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition + new Vector2(0, 4 * y), new Rectangle(122 * 3 + 22, 20 + 4 * y, (int)(98 * ManaCrystalProgress), 4), Color.White);
                    }
                    if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + 98 &&
                        Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + 8)
                        MouseOverText = "Mana: " + Guardian.MP + "/" + Guardian.MMP + "  Mana Crystals: " + ManaCrystals + "/" + GuardianData.MaxManaCrystals;
                }
                if (Guardian.Breath < Guardian.BreathMax && Guardian.BreathMax > 0)
                {
                    HealthbarPosition.X = 16 + XSum;
                    HealthbarPosition.Y += 16;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122, 0, 122, 16), Color.White);
                    float BarValue = ((float)Guardian.Breath / Guardian.BreathMax);
                    HealthbarPosition.X += 22;
                    HealthbarPosition.Y += 4;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 + 22, 20, (int)(98 * BarValue), 8), Color.White);
                    if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + 98 &&
                        Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + 8)
                        MouseOverText = "Breath: " + Guardian.Breath + "/" + Guardian.BreathMax;
                }
                if (Guardian.HasCooldown(GuardianCooldownManager.CooldownType.LavaToleranceCooldown))
                {
                    HealthbarPosition.X = 16 + XSum;
                    HealthbarPosition.Y += 16;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 2, 0, 122, 16), Color.White);
                    float BarValue = 1f - ((float)Guardian.GetCooldownValue(GuardianCooldownManager.CooldownType.LavaToleranceCooldown) / (60 * 7));
                    HealthbarPosition.X += 22;
                    HealthbarPosition.Y += 4;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 2 + 22, 20, (int)(98 * BarValue), 8), Color.White);
                    if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + 98 &&
                        Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + 8)
                        MouseOverText = "Lava Tolerance: " + Guardian.GetCooldownValue(GuardianCooldownManager.CooldownType.LavaToleranceCooldown) + "/" + (60 * 7);
                }
                HealthbarPosition.X = 16;
                HealthbarPosition.Y += 16;
                if (HealthbarPosition.Y < StartY + 64f)
                    HealthbarPosition.Y = StartY + 64f;
                if (IsMainGuardian)
                {
                    for (int b = 0; b < Guardian.Buffs.Count; b++)
                    {
                        if (Guardian.Buffs[b].ID < 0 || Guardian.Buffs[b].ID >= Main.buffTexture.Length)
                            continue;
                        Main.spriteBatch.Draw(Main.buffTexture[Guardian.Buffs[b].ID], HealthbarPosition, Color.White * 0.5f);
                        int TextureWidth = Main.buffTexture[Guardian.Buffs[b].ID].Width, TextureHeight = Main.buffTexture[Guardian.Buffs[b].ID].Height;
                        string TimeText = "";
                        float Time = (float)Guardian.Buffs[b].Time / 60;
                        if (Guardian.Buffs[b].Time > 5)
                        {
                            if (Time < 5)
                            {
                                TimeText = Math.Round(Time, 1) + "s";
                            }
                            else
                            {
                                if (Time >= 3600)
                                {
                                    Time = Time / 3600;
                                    TimeText = (int)Time + "h";
                                }
                                else if (Time >= 60)
                                {
                                    Time = Time / 60;
                                    TimeText = (int)Time + "m";
                                }
                                else
                                {
                                    TimeText = (int)Time + "s";
                                }
                            }
                            Utils.DrawBorderString(Main.spriteBatch, TimeText, HealthbarPosition + new Vector2(TextureWidth * 0.5f, TextureHeight), Color.White * 0.5f, 0.8f, 0.5f, 1f);
                        }
                        if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + TextureWidth && Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + TextureHeight)
                        {
                            MouseOverText = Lang.GetBuffName(Guardian.Buffs[b].ID) + "\n" + Lang.GetBuffDescription(Guardian.Buffs[b].ID);
                        }
                        HealthbarPosition.X += TextureWidth * 1.10f;
                    }
                    HealthbarPosition.Y += 32;
                }
                HealthbarPosition.X = 16;
                IsMainGuardian = false;
            }
            if(!Gameplay2PMode)
                Utils.DrawBorderString(Main.spriteBatch, "2P Press Start", HealthbarPosition, Color.White, 0.95f);
            HealthbarPosition.Y += 22f;
            if (MouseOverText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseOverText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            return true;
        }

        public static float InventoryEndY
        {
            get
            {
                float StartY = 258;
                if (MainMod.KalciphozMod != null)
                {
                    float kscale = Math.Min(1f, (float)Main.screenWidth / 3840 + 0.4f);
                    StartY = 432 * kscale;
                }
                return StartY;
            }
        }

        private static bool HiddenInterface = false;
        private static uint LastCoinValue = 0;
        private static byte CopperCoins = 0, SilverCoins = 0, GoldCoins = 0;
        public static ushort PlatinumCoins = 0;

        public static void UpdateCoinCount(TerraGuardian g)
        {
            UpdateCoinCount(g.Data);
        }

        public static void UpdateCoinCount(GuardianData gd)
        {
            uint c = gd.Coins, s = 0, g = 0, p = 0;
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
            CopperCoins = (byte)c;
            SilverCoins = (byte)s;
            GoldCoins = (byte)g;
            PlatinumCoins = (ushort)p;
            LastCoinValue = gd.Coins;
        }

        public bool DrawGuardianInventoryInterface()
        {
            if (GuardianManagement.Active || GuardianShopInterface.ShopOpen)
                return true;
            string MouseOverText = "";
            const int InventoryMenuButtons = 8;
            PlayerMod player = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>();
            TerraGuardian Guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            if (SelectedGuardian > 0)
            {
                if (!player.AssistGuardians[SelectedGuardian - 1].Active)
                {
                    SelectedGuardian = 0;
                }
                else
                {
                    Guardian = player.AssistGuardians[SelectedGuardian - 1];
                }
            }
            if (Guardian.Active)
            {
                if (Guardian.Coins != LastCoinValue)
                    UpdateCoinCount(Guardian);
            }
            //
            float StartX = 0;
            float StartY = 258;
            if(MainMod.FargoMutantMod != null)
            {
                StartY += 28;
            }
            else if (MainMod.KalciphozMod != null)
            {
                float kscale = Math.Min(1f, (float)Main.screenWidth / 3840 + 0.4f);
                StartY = 432 * kscale;
                StartX = 52;
            }
            if (Main.playerInventory)
            {
                TerraGuardian[] guardians = player.GetAllGuardianFollowers;
                Vector2 SelectionPosition = new Vector2(StartX, StartY);
                Main.spriteBatch.Draw(HideButtonTexture, SelectionPosition, null, Color.White, 0f, Vector2.Zero, 1f, (HiddenInterface ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0f);
                if (Main.mouseX >= SelectionPosition.X && Main.mouseX < SelectionPosition.X + 24 && Main.mouseY >= SelectionPosition.Y && Main.mouseY < SelectionPosition.Y + 32)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        HiddenInterface = !HiddenInterface;
                    }
                }
                if (HiddenInterface)
                    return true;
                SelectionPosition.X += 24;
                for (byte g = 0; g < guardians.Length; g++)
                {
                    TerraGuardian guardian = guardians[g];
                    if (guardian.Active)
                    {
                        Vector2 ThisPosition = SelectionPosition;
                        if (Main.mouseX >= SelectionPosition.X && Main.mouseX < SelectionPosition.X + 32 && Main.mouseY >= SelectionPosition.Y && Main.mouseY < SelectionPosition.Y + 40f)
                        {
                            MouseOverText = guardian.Name;
                            Main.player[Main.myPlayer].mouseInterface = true;
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                SelectedGuardian = g;
                                CheckingQuestBrief = false;
                            }
                        }
                        if (g != SelectedGuardian)
                        {
                            ThisPosition.Y += 8f;
                        }
                        if (guardian.Base.IsCustomSpriteCharacter)
                        {
                            Texture2D texture;
                            if (guardian.Base.InvalidGuardian)
                                texture = LosangleOfUnnown;
                            else
                                texture = guardian.Base.sprites.HeadSprite;
                            float ScaleMod = (texture.Width > texture.Height ? 32f / texture.Width : 32f / texture.Height);
                            guardian.DrawHead(ThisPosition, ScaleMod, 0, 0);
                            //Main.spriteBatch.Draw(texture, ThisPosition, null, Color.White, 0f, Vector2.Zero, ScaleMod, SpriteEffects.None, 0f);
                            SelectionPosition.X += 32;
                        }
                        else
                        {
                            SelectionPosition.X += 32;
                            ThisPosition.X += 16;
                            ThisPosition.Y -= 16;
                            float ScaleMod = 1.25f;
                            guardian.DrawTerrarianHeadData(ThisPosition, ScaleMod);
                            //SelectionPosition.X += 16;
                        }
                    }
                }
            }
            else
            {
                if (CheckingQuestBrief)
                    CheckingQuestBrief = false;
            }
            List<GuardianItemSlotButtons> Buttons = new List<GuardianItemSlotButtons>();
            Buttons.Add(GuardianItemSlotButtons.GuardianSelection);
            Buttons.Add(GuardianItemSlotButtons.QuestBook);
            Buttons.Add(GuardianItemSlotButtons.Requests);
            if (Guardian.Active)
            {
                //Buttons.Clear();
                for (int b = 0; b < InventoryMenuButtons; b++)
                {
                    if (!Buttons.Contains((GuardianItemSlotButtons)b))
                    {
                        Buttons.Add((GuardianItemSlotButtons)b);
                    }
                }
            }
            Item HoverItem = null;
            if (Main.playerInventory && Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0 && !Main.InReforgeMenu && Main.player[Main.myPlayer].talkNPC == -1)
            {
                if ((int)SelectedGuardianInventorySlot > -1 && !Buttons.Contains((GuardianItemSlotButtons)SelectedGuardianInventorySlot))
                    SelectedGuardianInventorySlot = (GuardianItemSlotButtons)(-1);
                if (SelectedGuardianInventorySlot != GuardianItemSlotButtons.Nothing && (Main.player[Main.myPlayer].chest != -1 || Main.npcShop > 0 || Main.InReforgeMenu || Main.player[Main.myPlayer].talkNPC > -1))
                    SelectedGuardianInventorySlot = GuardianItemSlotButtons.Nothing;
                if (Main.player[Main.myPlayer].chest == -1 && !Main.InReforgeMenu)
                {
                    float ButtonSize = 0.7f;
                    int ButtonDimension = (int)(36 * ButtonSize);
                    Vector2 ButtonPosition = new Vector2(6f + StartX, StartY + 40);
                    foreach (int b in Buttons)
                    {
                        //if (!Guardian.Active && (GuardianItemSlotButtons)b != GuardianItemSlotButtons.GuardianSelection)
                        //    continue;
                        ButtonPosition.X = (int)ButtonPosition.X;
                        ButtonPosition.Y = (int)ButtonPosition.Y;
                        bool Selected = b == (int)SelectedGuardianInventorySlot;
                        if (Main.mouseX >= ButtonPosition.X && Main.mouseY >= ButtonPosition.Y && Main.mouseX < ButtonPosition.X + ButtonDimension && Main.mouseY < ButtonPosition.Y + ButtonDimension)
                        {
                            Main.player[Main.myPlayer].mouseInterface = true;
                            switch ((GuardianItemSlotButtons)b)
                            {
                                case GuardianItemSlotButtons.GuardianSelection:
                                    MouseOverText = "Guardian Selection Window";
                                    break;
                                case GuardianItemSlotButtons.Inventory:
                                    MouseOverText = "Guardian Inventory";
                                    break;
                                case GuardianItemSlotButtons.Equipment:
                                    MouseOverText = "Guardian Equipments";
                                    break;
                                case GuardianItemSlotButtons.ExtraStorage:
                                    MouseOverText = "Guardian Private Storage";
                                    break;
                                case GuardianItemSlotButtons.Requests:
                                    MouseOverText = "Guardian Requests";
                                    break;
                                case GuardianItemSlotButtons.Skills:
                                    MouseOverText = "Guardian Life Skills";
                                    break;
                                case GuardianItemSlotButtons.QuestBook:
                                    MouseOverText = "Quest List";
                                    break;
                                case GuardianItemSlotButtons.CombatTactics:
                                    MouseOverText = "Guardian Behavior Settings";
                                    break;
                            }
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                GuardianInventoryMenuSubTab = 0;
                                if (Selected)
                                {
                                    SelectedGuardianInventorySlot = GuardianItemSlotButtons.Nothing;
                                }
                                else
                                {
                                    SelectedGuardianInventorySlot = (GuardianItemSlotButtons)b;
                                    Main.InGuideCraftMenu = false;
                                }
                                LastTrashItemID = Main.player[Main.myPlayer].trashItem.type;
                            }
                        }
                        if (Selected)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                for (int y = -1; y < 2; y++)
                                {
                                    Main.spriteBatch.Draw(GuardianButtonSlots, ButtonPosition + new Vector2(x, y), new Rectangle(36 * b, 0, 36, 36), Color.White, 0f, Vector2.Zero, ButtonSize, SpriteEffects.None, 0f);
                                }
                            }
                        }
                        Main.spriteBatch.Draw(GuardianButtonSlots, ButtonPosition, new Rectangle(36 * b, 0, 36, 36), Color.White, 0f, Vector2.Zero, ButtonSize, SpriteEffects.None, 0f);
                        ButtonPosition.X += ButtonDimension + 6;
                    }
                }
                //if (Main.player[Main.myPlayer].chest != -1 || Main.npcShop > -1 || Main.InReforgeMenu)
                //    return true;
                foreach (TerraGuardian guardian in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (!guardian.Active || (SelectedGuardianInventorySlot == GuardianItemSlotButtons.Inventory && guardian != Guardian))
                        continue;
                    if ((SelectedGuardianInventorySlot == GuardianItemSlotButtons.Inventory || guardian.Data.GetItemsISendtoTrash) && (!guardian.DoAction.InUse || !guardian.DoAction.CantUseInventory) && Main.player[Main.myPlayer].trashItem.type > 0 && ItemSlot.ShiftInUse && LastTrashItemID != Main.player[Main.myPlayer].trashItem.type)
                    {
                        
                        guardian.MoveItemToInventory(Main.player[Main.myPlayer].trashItem);
                        if (Main.player[Main.myPlayer].trashItem.type > 0)
                        {
                            LastTrashItemID = Main.player[Main.myPlayer].trashItem.type;
                        }
                        else
                        {
                            LastTrashItemID = 0;
                        }
                    }
                }
                Vector2 SlotStartPosition = new Vector2(20 + StartX, StartY + 40);
                SlotStartPosition.X += 56;
                SlotStartPosition.Y += 22;
                Main.inventoryScale = 0.7f;
                if (SelectedGuardianInventorySlot != GuardianItemSlotButtons.Nothing)
                {
                    for(int i = 0; i < Main.availableRecipeY.Length; i++){
                        Main.availableRecipeY[i] = Main.screenHeight * 2;
                    }
                    Main.craftingHide = true;
                }
                bool BlockedInventory = Guardian.DoAction.InUse && Guardian.DoAction.CantUseInventory;
                switch (SelectedGuardianInventorySlot)
                {
                    case GuardianItemSlotButtons.Nothing:
                        return true;
                    case GuardianItemSlotButtons.GuardianSelection:
                        MainMod.OpenGuardianSelectionInterface();
                        break;
                    case GuardianItemSlotButtons.Inventory:
                        Utils.DrawBorderString(Main.spriteBatch, "Guardian Inventory", SlotStartPosition, Color.White);
                        Main.inventoryScale = 0.755f;
                        for (int y = 0; y < 5; y++)
                        {
                            for (int x = 0; x < 10; x++)
                            {
                                Vector2 SlotPosition = SlotStartPosition;
                                SlotPosition.X += x * 56 * Main.inventoryScale;
                                SlotPosition.Y += y * 56 * Main.inventoryScale + 20;
                                int i = x + y * 10;
                                if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + 56 * Main.inventoryScale && Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + 56 * Main.inventoryScale)
                                {
                                    Main.player[Main.myPlayer].mouseInterface = true;
                                    if (BlockedInventory)
                                    {
                                        MouseOverText = "You can't change the guardian inventory right now.";
                                    }
                                    else
                                    {
                                        ItemSlot.OverrideHover(Guardian.Inventory, 0, i);
                                        HoverItem = Guardian.Inventory[i];
                                        if (!(i == Guardian.SelectedItem && Guardian.ItemAnimationTime > 0))
                                        {
                                            int LastItemID = Main.mouseItem.type;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                if (Main.keyState.IsKeyDown(Main.FavoriteKey))
                                                {
                                                    Guardian.Inventory[i].favorited = !Guardian.Inventory[i].favorited;
                                                }
                                                else if (ItemSlot.ShiftInUse)
                                                {
                                                    if (!Guardian.Inventory[i].favorited && Guardian.Inventory[i].type != 0)
                                                    {
                                                        Item item = Main.player[Main.myPlayer].GetItem(Main.myPlayer, Guardian.Inventory[i]);
                                                        Guardian.Inventory[i] = item;
                                                    }
                                                }
                                                else
                                                {
                                                    ItemSlot.LeftClick(Guardian.Inventory, 0, i);
                                                    Guardian.ItemPlacedInInventory(i);
                                                    if (i < 10 && !Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialVanityIntroduction)
                                                    {
                                                        Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialVanityIntroduction = true;
                                                        Main.NewText("You can make the companion wear some vanity items by placing it on one of the 10 inventory slots. They will use it If they are able to, so get ready to experiement.");
                                                    }
                                                    if ((LastItemID == Terraria.ID.ItemID.LifeCrystal || LastItemID == Terraria.ID.ItemID.LifeFruit || LastItemID == Terraria.ID.ItemID.ManaCrystal ||
                                                        LastItemID == ModContent.ItemType<Items.Consumable.EtherHeart>() || LastItemID == ModContent.ItemType<Items.Consumable.EtherFruit>()) && 
                                                        !Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialStatusIncreaseItemIntroduction)
                                                    {
                                                        Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().TutorialStatusIncreaseItemIntroduction = true;
                                                        Main.NewText("You gave a status increase item to your companion, but they wont use it unless you tell them to. Give them the order to use the item so they know It's okay to use it.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ItemSlot.RightClick(Guardian.Inventory, 0, i);
                                            }
                                            if (LastItemID != Main.mouseItem.type)
                                            {
                                                if (i < 10)
                                                    Guardian.UpdateWeapons = true;
                                            }
                                        }
                                        ItemSlot.MouseHover(Guardian.Inventory, 0, i);
                                    }
                                }
                                ItemSlot.Draw(Main.spriteBatch, Guardian.Inventory, 0, i, SlotPosition);
                            }
                        }
                        SlotStartPosition.Y += 56 * Main.inventoryScale * 5 + 20;
                        Vector2 CoinTextPosition = SlotStartPosition;
                        if (Main.mouseX >= CoinTextPosition.X && Main.mouseX < CoinTextPosition.X + 560 * Main.inventoryScale &&
                            Main.mouseY >= CoinTextPosition.Y && Main.mouseY < CoinTextPosition.Y + 24)
                            Main.player[Main.myPlayer].mouseInterface = true;
                        CoinTextPosition.X += Utils.DrawBorderString(Main.spriteBatch, "Personal Savings: ", CoinTextPosition, Color.White).X;
                        for (byte c = 0; c < 4; c++)
                        {
                            byte CoinID = (byte)(Terraria.ID.ItemID.PlatinumCoin - c);
                            Texture2D CoinTexture = Main.itemTexture[CoinID];
                            float TextureScale = 1f;
                            ushort CoinValue = 0;
                            switch (c)
                            {
                                case 0:
                                    CoinValue = PlatinumCoins;
                                    break;
                                case 1:
                                    CoinValue = GoldCoins;
                                    break;
                                case 2:
                                    CoinValue = SilverCoins;
                                    break;
                                case 3:
                                    CoinValue = CopperCoins;
                                    break;
                            }
                            CoinTextPosition.X += Utils.DrawBorderString(Main.spriteBatch, CoinValue.ToString(), CoinTextPosition, Color.White, TextureScale).X;
                            Main.spriteBatch.Draw(CoinTexture, CoinTextPosition, null, Color.White, 0f, Vector2.Zero, TextureScale, SpriteEffects.None, 0);
                            CoinTextPosition.X += 20;
                        }
                        break;
                    case GuardianItemSlotButtons.Equipment:
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "Equipments", SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 20;
                            //if (Guardian.Base.IsCustomSpriteCharacter)
                            {
                                SlotStartPosition.X = 0;
                                string[] EquipmentTab = new string[] { "Equipment", "Skins", "Outfits" }; //Set the empty string to always be the last
                                {
                                    const float TabSize = 0.85f;
                                    SlotStartPosition.X = 2 + 56 + StartX;
                                    for (int s = 0; s < EquipmentTab.Length; s++)
                                    {
                                        if (EquipmentTab[s] == "")
                                            continue;
                                        Vector2 ButtonDim = Utils.DrawBorderString(Main.spriteBatch, EquipmentTab[s], SlotStartPosition, Color.Black, TabSize);
                                        Color buttonColor = (s == GuardianInventoryMenuSubTab ? Color.White : Color.Black);
                                        if (Main.mouseX >= SlotStartPosition.X - 2 && Main.mouseX < SlotStartPosition.X + ButtonDim.X + 2 && Main.mouseY >= SlotStartPosition.Y - 2 && Main.mouseY < SlotStartPosition.Y + ButtonDim.Y + 2)
                                        {
                                            buttonColor = Color.Cyan;
                                            Main.player[Main.myPlayer].mouseInterface = true;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                GuardianInventoryMenuSubTab = s;
                                            }
                                        }
                                        Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SlotStartPosition.X - 2, (int)SlotStartPosition.Y - 2, (int)ButtonDim.X + 3, (int)ButtonDim.Y + 3), buttonColor);
                                        Utils.DrawBorderString(Main.spriteBatch, EquipmentTab[s], SlotStartPosition, Color.White, TabSize);
                                        SlotStartPosition.X += ButtonDim.X + 4f;
                                    }
                                }
                                SlotStartPosition.X = 50;
                                SlotStartPosition.Y += 30;
                            }
                            switch (GuardianInventoryMenuSubTab)
                            {
                                case 0: //Equipments
                                    {
                                        ManagingGuardianEquipments = true;
                                        float EquipmentSlot6StartPos = 0f;
                                        for (int s = 0; s < 9; s++)
                                        {
                                            Vector2 SlotPosition = SlotStartPosition;
                                            SlotPosition.Y += s * 56 * Main.inventoryScale + 20;
                                            if (SlotPosition.Y + 56 * Main.inventoryScale >= Main.screenHeight)
                                            {
                                                SlotPosition.X += 56 * Main.inventoryScale + 20;
                                                SlotPosition.Y -= SlotPosition.Y - 20 - SlotStartPosition.Y;
                                            }
                                            int context = 8;
                                            if (s > 2)
                                            {
                                                context = 10;
                                                SlotPosition.Y += 4;
                                            }
                                            if (s == 3)
                                            {
                                                EquipmentSlot6StartPos = SlotPosition.Y;
                                            }
                                            if (s == 3 + 5)
                                            {
                                                SlotPosition.X += 56 * Main.inventoryScale + 20;
                                                SlotPosition.Y = EquipmentSlot6StartPos;
                                            }
                                            if (s == 8 && (!Guardian.ExtraAccessorySlot || (!Main.expertMode && Guardian.Equipments[8].type == 0)))
                                                continue;
                                            if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + 56 * Main.inventoryScale && Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + 56 * Main.inventoryScale)
                                            {
                                                Main.player[Main.myPlayer].mouseInterface = true;
                                                if (BlockedInventory)
                                                {
                                                    MouseOverText = "Can't change equipments right now.";
                                                }
                                                else
                                                {
                                                    ItemSlot.OverrideHover(Guardian.Equipments, context, s);
                                                    HoverItem = Guardian.Equipments[s];
                                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                                    {
                                                        /*if (Main.mouseItem.type > 0 && !giantsummon.IsGuardianItem(Main.mouseItem))
                                                        {
                                                            Main.NewText("This item doesn't fit my Guardian...");
                                                        }
                                                        else*/
                                                        {
                                                            int HeldItemID = Main.mouseItem.type;
                                                            bool Allow = true;//Main.mouseItem.type == 0 || (s < 3 && Main.mouseItem.modItem is Items.GuardianItemPrefab) || s >= 3;
                                                            //Main.mouseItem.modItem is Items.GuardianItemPrefab ||
                                                            if (Allow)
                                                            {
                                                                Allow = false;
                                                                switch (s)
                                                                {
                                                                    case 0:
                                                                        if (Main.mouseItem.type == 0 || Main.mouseItem.headSlot > 0)
                                                                            Allow = true;
                                                                        break;
                                                                    case 1:
                                                                        if (Main.mouseItem.type == 0 || Main.mouseItem.bodySlot > 0)
                                                                            Allow = true;
                                                                        break;
                                                                    case 2:
                                                                        if (Main.mouseItem.type == 0 || Main.mouseItem.legSlot > 0)
                                                                            Allow = true;
                                                                        break;
                                                                    default:
                                                                        if (Main.mouseItem.type == 0 || Main.mouseItem.accessory)
                                                                        {
                                                                            Allow = true;
                                                                            if (Main.mouseItem.type != 0)
                                                                            {
                                                                                for (int a = 3; a < 9; a++)
                                                                                {
                                                                                    if (Guardian.Equipments[a].type == Main.mouseItem.type)
                                                                                    {
                                                                                        Allow = false;
                                                                                        break;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        break;
                                                                }
                                                            }
                                                            if (Allow)
                                                            {
                                                                Main.mouseItem.favorited = false;
                                                                ItemSlot.LeftClick(Guardian.Equipments, 0, s);
                                                            }
                                                            else
                                                            {
                                                                Main.NewText("I can't do that...");
                                                            }
                                                            if (HeldItemID != Main.mouseItem.type)
                                                                Guardian.EquipmentChanged();
                                                        }
                                                    }
                                                    ItemSlot.MouseHover(Guardian.Equipments, context, s);
                                                }
                                            }
                                            ItemSlot.Draw(Main.spriteBatch, Guardian.Equipments, context, s, SlotPosition);
                                        }
                                        SlotStartPosition.X += 120;
                                        Vector2 BodyDyeSlot = SlotStartPosition;
                                        Utils.DrawBorderString(Main.spriteBatch, "Body Dye", BodyDyeSlot, Color.White);
                                        BodyDyeSlot.Y += 24f;
                                        if (Main.mouseX >= BodyDyeSlot.X && Main.mouseX < BodyDyeSlot.X + 56 * Main.inventoryScale && Main.mouseY >= BodyDyeSlot.Y && Main.mouseY < BodyDyeSlot.Y + 56 * Main.inventoryScale)
                                        {
                                            HoverItem = Guardian.BodyDye;
                                            Main.player[Main.myPlayer].mouseInterface = true;
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                ItemSlot.LeftClick(ref Guardian.Data.BodyDye, ItemSlot.Context.EquipDye);
                                            }
                                            ItemSlot.MouseHover(ref Guardian.Data.BodyDye, ItemSlot.Context.EquipDye);
                                        }
                                        ItemSlot.Draw(Main.spriteBatch, ref Guardian.Data.BodyDye, ItemSlot.Context.EquipDye, BodyDyeSlot);
                                        ManagingGuardianEquipments = true;
                                        SlotStartPosition.X += 96;
                                        string[] InfosToDraw = new string[]{
                                            "Melee Damage: " + Math.Round(Guardian.MeleeDamageMultiplier * 100, 2) + "%",
                                            "Ranged Damage: " + Math.Round(Guardian.RangedDamageMultiplier * 100, 2) + "%",
                                            "Magic Damage: " + Math.Round(Guardian.MagicDamageMultiplier * 100, 2) + "%",
                                            "Summon Damage: " + Math.Round(Guardian.SummonDamageMultiplier * 100, 2) + "%",
                                            "Attack Time: " + Math.Round(Guardian.MeleeSpeed * 100, 2) + "%",
                                            "Walk Speed: " + Math.Round(Guardian.MoveSpeed * 100, 2) + "%",
                                            "Defense: " + Guardian.Defense,
                                            "Defense Rate: " + Math.Round(Guardian.DefenseRate * 100, 2) + "%",
                                            "Dodge Rate: " + Math.Round(Guardian.DodgeRate, 2) + "%",
                                            "Block Rate: " + Math.Round(Guardian.BlockRate, 2) + "%",
							                "Cover Rate: " + Math.Round(Guardian.CoverRate, 2) + "%",
							                "Accuracy: " + Math.Round(Guardian.Accuracy * 100, 2) + "%"
                                        };
                                        SlotStartPosition.Y += 30f;
                                        foreach (string s in InfosToDraw)
                                        {
                                            Utils.DrawBorderString(Main.spriteBatch, s, SlotStartPosition, Color.White);
                                            SlotStartPosition.Y += 22f;
                                        }
                                    }
                                    break;
                                case 1: //Skins
                                    {
                                        bool HasSkin = false;
                                        foreach (SkinReqStruct skin in Guardian.Base.SkinList)
                                        {
                                            HasSkin = true;
                                            bool Active = Guardian.Data.SkinID == skin.SkinID;
                                            bool LastActive = Active;
                                            bool RequirementBeaten = ShowDebugInfo || skin.Requirement(Guardian.Data, player.player);
                                            AddOnOffButton(SlotStartPosition.X, SlotStartPosition.Y, skin.Name, ref Active, RequirementBeaten, !RequirementBeaten);
                                            if (Active != LastActive)
                                            {
                                                if (Active)
                                                    Guardian.Data.SkinID = skin.SkinID;
                                                else
                                                    Guardian.Data.SkinID = 0;
                                            }
                                            SlotStartPosition.Y += 20;
                                        }
                                        if (!HasSkin)
                                        {
                                            Utils.DrawBorderString(Main.spriteBatch, "This companion has no skin right now.", SlotStartPosition, Color.White);
                                        }
                                    }
                                    break;
                                case 2: //Outfits
                                    {
                                        bool HasOutfit = false;
                                        foreach (SkinReqStruct skin in Guardian.Base.OutfitList)
                                        {
                                            HasOutfit = true;
                                            bool Active = Guardian.Data.OutfitID == skin.SkinID;
                                            bool LastActive = Active;
                                            bool RequirementBeaten = ShowDebugInfo || skin.Requirement(Guardian.Data, player.player);
                                            AddOnOffButton(SlotStartPosition.X, SlotStartPosition.Y, skin.Name, ref Active, Active || RequirementBeaten, Active || !RequirementBeaten);
                                            if (Active != LastActive)
                                            {
                                                if (Active)
                                                    Guardian.Data.OutfitID = skin.SkinID;
                                                else
                                                    Guardian.Data.OutfitID = 0;
                                            }
                                            SlotStartPosition.Y += 20;
                                        }
                                        if (!HasOutfit)
                                        {
                                            Utils.DrawBorderString(Main.spriteBatch, "This companion has no outfits right now.", SlotStartPosition, Color.White);
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                    case GuardianItemSlotButtons.ExtraStorage:
                        break;
                    case GuardianItemSlotButtons.CombatTactics:
                        {
                            float ButtonPosX = 56;
                            Utils.DrawBorderString(Main.spriteBatch, "Guardian Tactics", SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 36;
                            string[] TacticsTabs = new string[] { "Behavior", "Combat", "Permissions", "" }; //Set the empty string to always be the last
                            {
                                if (ShowDebugInfo)
                                    TacticsTabs[TacticsTabs.Length - 1] = "Debug";
                                const float TabSize = 0.85f;
                                SlotStartPosition.X = 2 + 56 + StartX;
                                for (int s = 0; s < TacticsTabs.Length; s++)
                                {
                                    if (TacticsTabs[s] == "")
                                        continue;
                                    Vector2 ButtonDim = Utils.DrawBorderString(Main.spriteBatch, TacticsTabs[s], SlotStartPosition, Color.Black, TabSize);
                                    Color buttonColor = (s == GuardianInventoryMenuSubTab ? Color.White : Color.Black);
                                    if (Main.mouseX >= SlotStartPosition.X - 2 && Main.mouseX < SlotStartPosition.X + ButtonDim.X + 2 && Main.mouseY >= SlotStartPosition.Y - 2 && Main.mouseY < SlotStartPosition.Y + ButtonDim.Y + 2)
                                    {
                                        buttonColor = Color.Cyan;
                                        Main.player[Main.myPlayer].mouseInterface = true;
                                        if (Main.mouseLeft && Main.mouseLeftRelease)
                                        {
                                            GuardianInventoryMenuSubTab = s;
                                        }
                                    }
                                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)SlotStartPosition.X - 2, (int)SlotStartPosition.Y - 2, (int)ButtonDim.X + 3, (int)ButtonDim.Y + 3), buttonColor);
                                    Utils.DrawBorderString(Main.spriteBatch, TacticsTabs[s], SlotStartPosition, Color.White, TabSize);
                                    SlotStartPosition.X += ButtonDim.X + 4f;
                                }
                            }
                            SlotStartPosition.X = ButtonPosX + StartX + 16;
                            SlotStartPosition.Y += 26;
                            ButtonPosX = SlotStartPosition.X;
                            switch (GuardianInventoryMenuSubTab)
                            {
                                default: //Debug infos
                                    if (ShowDebugInfo)
                                    {
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Force draw guardian on front of the player? ", ref TestForceGuardianOnFront);
                                        SlotStartPosition.Y += 26;
                                        int[] TestGuardianIDs = new int[] { 19, 20, 22, 26, 27, 28, 29, 30 };
                                        bool b = false;
                                        foreach (int TestGuardianID in TestGuardianIDs)
                                        {
                                            if (!Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().HasGuardian(TestGuardianID))
                                            {
                                                AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Test " + GuardianBase.GetGuardianBase(TestGuardianID).Name + "? ", ref b);
                                                if (b)
                                                {
                                                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().AddNewGuardian(TestGuardianID);
                                                    Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetGuardian(TestGuardianID).FriendshipLevel = 15;
                                                }
                                                SlotStartPosition.Y += 26;
                                            }
                                            b = false;
                                        }
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Start Blood Moon ", ref b);
                                        if (b)
                                        {
                                            Main.dayTime = false;
                                            Main.time = 0;
                                            Main.bloodMoon = true;
                                        }
                                        SlotStartPosition.Y += 26;
                                        //b = (!Main.dayTime && Main.time >= 4.5 * 3600) || (Main.dayTime && Main.time < 3 * 3600);
                                        b = false;
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Start Late Night ", ref b);
                                        if (b)
                                        {
                                            Main.dayTime = false;
                                            Main.time = 4.5 * 3600;
                                            Main.bloodMoon = false;
                                        }
                                        SlotStartPosition.Y += 26;
                                        b = false;
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Set guardian to item selling level ", ref b);
                                        if (b)
                                        {
                                            Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.FriendshipLevel = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.Base.MaySellYourLoot;
                                        }
                                    }
                                    break;
                                case 0: //Behavior
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Avoid Combat?", ref Guardian.Data.AvoidCombat);
                                    SlotStartPosition.Y += 26;
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Be Passive?", ref Guardian.Data.Passive);
                                    SlotStartPosition.Y += 26;
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Attack what I attack?", ref Guardian.Data.AttackMyTarget);
                                    SlotStartPosition.Y += 26;
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Set Guardian to player size?", ref Guardian.Data.SetToPlayerSize);
                                    SlotStartPosition.Y += 26;
                                    if (Guardian.HasFlag(GuardianFlags.MayGoSellLoot))
                                    {
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Auto Sell when inventory is filled.", ref Guardian.Data.AutoSellWhenInvIsFull);
                                        SlotStartPosition.Y += 26;
                                    }
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Hide Werewolf/Merfolk form?", ref Guardian.Data.HideWereForm);
                                    SlotStartPosition.Y += 26;
                                    break;
                                case 1: //Combat
                                        Utils.DrawBorderString(Main.spriteBatch, "Combat Behavior", SlotStartPosition, Color.White);
                                        SlotStartPosition.Y += 26;
                                        int hoveritem = -1;
                                        int SelectedOption = DrawBar(SlotStartPosition, 3, out hoveritem);
                                        if (SelectedOption > -1)
                                        {
                                            Guardian.tactic = (CombatTactic)SelectedOption;
                                        }
                                        switch (hoveritem)
                                        {
                                            case 0:
                                                MouseOverText = (!Guardian.AvoidCombat ? "Attack far away from target." : "Void the target at all cost.");
                                                break;
                                            case 1:
                                                MouseOverText = (!Guardian.AvoidCombat ? "Attack near the target, but keep some distance." : "Keep a decent distance away from target.");
                                                break;
                                            case 2:
                                                MouseOverText = (!Guardian.AvoidCombat ? "Try melee combat with the target." : "Keep some distance from target.");
                                                break;
                                        }
                                        Main.spriteBatch.Draw(TacticsIconsTexture, SlotStartPosition + new Vector2((int)Guardian.tactic * 56 - 4, -8), new Rectangle(0, 0, 36, 36), Color.White);
                                        Main.spriteBatch.Draw(TacticsIconsTexture, SlotStartPosition + new Vector2(56 * 3 - 4, -4), new Rectangle(36, 0, 36, 36), Color.White);

                                    SlotStartPosition.Y += 26;
                                    if (!Guardian.AvoidCombat)
                                    {
                                        if (Guardian.HasFlag(GuardianFlags.Tanking))
                                        {
                                            AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Guardian lures foes attention?", ref Guardian.Data.Tanker);
                                            SlotStartPosition.Y += 26;
                                        }
                                    }
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Pick Weapons by Inventory Order", ref Guardian.Data.UseWeaponsByInventoryOrder);
                                    SlotStartPosition.Y += 26;
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Protection Mode", ref Guardian.Data.ProtectMode);
                                    SlotStartPosition.Y += 26;
                                    break;
                                case 2: //Permissions
                                    if (Guardian.HasFlag(GuardianFlags.AllowMount))
                                    {
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Quick Mount button mounts Guardian instead?", ref Guardian.Data.OverrideQuickMountToMountGuardianInstead);
                                        SlotStartPosition.Y += 26;
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "May use Heavy Melee attacks when mounted?", ref Guardian.Data.UseHeavyMeleeAttackWhenMounted);
                                        SlotStartPosition.Y += 26;
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Sit on player mount after being pulled to player?", ref Guardian.Data.SitOnTheMount);
                                        SlotStartPosition.Y += 26;
                                    }
                                    if (Guardian.HasFlag(GuardianFlags.MayLootItems))
                                    {
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Allow Looting Items?", ref Guardian.Data.MayLootItems);
                                        SlotStartPosition.Y += 26;
                                    }
                                    AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Get Items I send to trash?", ref Guardian.Data.GetItemsISendtoTrash);
                                    SlotStartPosition.Y += 26;
                                    break;
                            }
                        }
                        break;
                    case GuardianItemSlotButtons.Requests:
                        //Guardian.DrawFriendshipHeart(SlotStartPosition);
                        //SlotStartPosition.X += 24;
                        List<GuardianData> RequestCount = new List<GuardianData>();
                        foreach (GuardianData d in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().MyGuardians.Values)
                        {
                            if (d.request.requestState == RequestData.RequestState.RequestActive)
                            {
                                if (d.ID == Guardian.ID && d.ModID == Guardian.ModID)
                                    RequestCount.Insert(0, d);
                                else
                                    RequestCount.Add(d);
                            }
                            else if (d.request.requestState == RequestData.RequestState.HasExistingRequestReady)
                            {
                                if (d.ID == Guardian.ID && d.ModID == Guardian.ModID)
                                {
                                    RequestCount.Insert(0, d);
                                }
                                else if(PlayerMod.HasGuardianSummoned(player.player, d.ID, d.ModID))
                                {
                                    RequestCount.Add(d);
                                }
                            }
                        }
                        bool HasRequest = false;
                        bool HasUnsummonedGuardianRequest = false;
                        foreach (GuardianData d in RequestCount)
                        {
                            if (d.Base.InvalidGuardian)
                                continue;
                            if (!HasRequest)
                            {
                                Utils.DrawBorderString(Main.spriteBatch, "Guardian Requests", SlotStartPosition, Color.White);
                                SlotStartPosition.Y += 36;
                                //SlotStartPosition.X -= 24;
                                HasRequest = true;
                            }
                            if (Guardian.Active == false || d.ID != Guardian.ID) HasUnsummonedGuardianRequest = true;
                            string[] RequestDesc = d.request.GetRequestText(Main.player[Main.myPlayer], d);
                            foreach (string s in RequestDesc)
                            {
                                SlotStartPosition.Y += Utils.DrawBorderString(Main.spriteBatch, s, SlotStartPosition, (d.request.Failed ? Color.Red : Color.White)).Y;
                            }
                            Vector2 ButtonPosition = SlotStartPosition;
                            if (Guardian.Active && Guardian.ID == d.ID && Guardian.ModID == d.ModID)
                            {
                                if (false && d.request.IsTalkQuest && d.request.requestState >= RequestData.RequestState.HasExistingRequestReady)
                                {
                                    ButtonPosition.X += 48f;
                                    Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, "Talk", ButtonPosition, Color.White, 1f, 0.5f);
                                    if (Main.mouseX >= ButtonPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ButtonPosition.X + ButtonDimension.X * 0.5f &&
                                        Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + ButtonDimension.Y)
                                    {
                                        player.player.mouseInterface = true;
                                        Utils.DrawBorderString(Main.spriteBatch, "Talk", ButtonPosition, Color.Yellow, 1f, 0.5f);
                                        if (Main.mouseLeft && Main.mouseLeftRelease)
                                        {
                                            d.request.CompleteRequest(Guardian, d, player);
                                        }
                                    }
                                    SlotStartPosition.Y += 28f;
                                }
                                else if (d.request.requestState == RequestData.RequestState.HasExistingRequestReady)
                                {

                                }
                                else if (d.request.requestState == RequestData.RequestState.RequestActive)
                                {
                                    if (d.request.RequestCompleted || d.request.Failed)
                                    {

                                    }
                                    else
                                    {
                                        ButtonPosition.X += 48f;
                                        string ButtonText = "Ask for Clues";
                                        Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ButtonPosition, Color.White, 1f, 0.5f);
                                        if (Main.mouseX >= ButtonPosition.X - ButtonDimension.X * 0.5f && Main.mouseX < ButtonPosition.X + ButtonDimension.X * 0.5f &&
                                            Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + ButtonDimension.Y)
                                        {
                                            player.player.mouseInterface = true;
                                            Utils.DrawBorderString(Main.spriteBatch, ButtonText, ButtonPosition, Color.Yellow, 1f, 0.5f);
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                CheckingQuestBrief = false;
                                                Guardian.SaySomething(GuardianMouseOverAndDialogueInterface.MessageParser(d.request.GetRequestInfo(d), Guardian), true);
                                            }
                                        }
                                        ButtonPosition.X -= 48;
                                    }
                                    SlotStartPosition.Y += 28f;
                                }
                            }
                            if (d.request.requestState == RequestData.RequestState.RequestActive)
                            {
                                for (int o = 0; o < d.request.GetRequestBase(d).Objectives.Count; o++)
                                {
                                    if (d.request.GetRequestBase(d).Objectives[o].objectiveType == RequestBase.RequestObjective.ObjectiveTypes.KillBoss && d.request.GetIntegerValue(o) > 0)
                                    {
                                        RequestBase.KillBossRequest req = (RequestBase.KillBossRequest)d.request.GetRequestBase(d).Objectives[o];
                                        string ButtonText = "Spawn " + Lang.GetNPCName(req.BossID);
                                        Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, ButtonText, ButtonPosition, Color.White, 1f);
                                        if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + ButtonDimension.X &&
                                            Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + ButtonDimension.Y)
                                        {
                                            player.player.mouseInterface = true;
                                            Utils.DrawBorderString(Main.spriteBatch, ButtonText, ButtonPosition, Color.Yellow, 1f);
                                            if (Main.mouseLeft && Main.mouseLeftRelease)
                                            {
                                                CheckingQuestBrief = false;
                                                if (!d.request.TrySpawningBoss(player.player.whoAmI, req.BossID, req.DifficultyBonus))
                                                {
                                                    Main.NewText("Failed. Did you tried calling It when It can appear?");
                                                }
                                            }
                                        }
                                        SlotStartPosition.Y += 26f;
                                    }
                                }
                            }
                            SlotStartPosition.Y += 8f;
                        }
                        if (HasUnsummonedGuardianRequest)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "Report to the Quest Giver when done.", SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 28f;
                        }
                        if (!HasRequest)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "No requests right now.", SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 36;
                            //SlotStartPosition.X -= 24;
                            //Utils.DrawBorderString(Main.spriteBatch, "New request in " + Guardian.request.GetRequestDuration, SlotStartPosition, Color.White);
                        }
                        break;

                    case GuardianItemSlotButtons.Skills:
                        {
                            const int MaxSkillsDisplay = 8;
                            List<GuardianSkills> Skills = Guardian.SkillList;
                            int SkillRows = Skills.Count / MaxSkillsDisplay;
                            Utils.DrawBorderString(Main.spriteBatch, "Guardian Skills", SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 30;
                            {
                                float RowX = SlotStartPosition.X;
                                for (byte Row = 0; Row < SkillRows; Row++)
                                {
                                    Vector2 Position = new Vector2(RowX, SlotStartPosition.Y);
                                    Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle((int)Position.X, (int)Position.Y, 20, 20), SkillListRow == Row ? Color.White : Color.Black);
                                    bool MouseOver = Main.mouseX >= Position.X && Main.mouseX < Position.X + 20 && Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 20;
                                    if(MouseOver)
                                    {
                                        Main.LocalPlayer.mouseInterface = true;
                                        if (Main.mouseLeft && Main.mouseLeftRelease)
                                        {
                                            SkillListRow = Row;
                                        }
                                    }
                                    Position.X += 10;
                                    Position.Y += 12;
                                    Utils.DrawBorderString(Main.spriteBatch, (Row + 1).ToString(), Position, MouseOver ? Color.Yellow : Color.White, 0.8f, 0.5f, 0.5f);
                                    RowX += 22;
                                }
                            }
                            SlotStartPosition.Y += 20;
                            int LevelSum = 0;
                            int Discount = -SkillListRow * MaxSkillsDisplay;
                            foreach (GuardianSkills s in Guardian.Data.GetSkillList)
                            {
                                if (Discount >= 0 && Discount < MaxSkillsDisplay)
                                {
                                    Vector2 SkillInfoDimension = Utils.DrawBorderString(Main.spriteBatch, s.GetSkillInfo(Guardian), SlotStartPosition, Color.White, 0.85f);
                                    if (Main.mouseX >= SlotStartPosition.X && Main.mouseX < SlotStartPosition.X + SkillInfoDimension.X &&
                                        Main.mouseY >= SlotStartPosition.Y && Main.mouseY < SlotStartPosition.Y + SkillInfoDimension.Y)
                                    {
                                        MouseOverText = s.GetSkillDescription;
                                    }
                                    SlotStartPosition.Y += SkillInfoDimension.Y;
                                }
                                LevelSum += s.Level;
                            }
                            SlotStartPosition.Y += 36;
                            Utils.DrawBorderString(Main.spriteBatch, "Total Levels: " + LevelSum, SlotStartPosition, Color.White, 0.9f);
                            SlotStartPosition.Y += 22;
                            if (Guardian.Data.SkillLevelSum < Guardian.Data.LastTotalSkillLevel)
                            {
                                Utils.DrawBorderString(Main.spriteBatch, "Bonus Skill EXP until total level: " + Guardian.Data.LastTotalSkillLevel, SlotStartPosition, Color.White, 0.9f);
                                SlotStartPosition.Y += 22;
                            }
                        }
                        break;

                    case GuardianItemSlotButtons.QuestBook:
                        GuardianQuestInterface.OpenInterface();
                        break;
                }
                LastTrashItemID = Main.player[Main.myPlayer].trashItem.type;
            }
            else
            {
                if (Guardian.PlayerControl)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        Vector2 SlotPosition = new Vector2(8, 64 + 8);
                        SlotPosition.X += x * 56 * Main.inventoryScale;
                        //SlotPosition.Y += y * 56 * Main.inventoryScale + 20;
                        int i = x;
                        ItemSlot.Draw(Main.spriteBatch, Guardian.Inventory, 0, i, SlotPosition);
                    }
                }
                SelectedGuardianInventorySlot = GuardianItemSlotButtons.Nothing;
            }
            if (MouseOverText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseOverText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            if (Guardian.GrabbingPlayer && Guardian.PlayerCanEscapeGrab)
            {
                Vector2 TextPosition = new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f);
                Utils.DrawBorderString(Main.spriteBatch, "Press Jump button", TextPosition, Color.White, 1f, 0.5f);
            }
            if (HoverItem != null && HoverItem.type != 0)
            {

                //List<string> ItemInfo = new List<string>();
                //ItemInfo.Add(HoverItem.HoverName);
                //ItemInfo.AddRange(GetItemInfo(HoverItem, Guardian));
                /*Vector2 TextPos = new Vector2(Main.mouseX + 16, Main.mouseY + 16);
                foreach (string s in ItemInfo)
                {
                    Utils.DrawBorderString(Main.spriteBatch, s, TextPos, Color.White);
                    TextPos.Y += 26f;
                }*/
            }
            return true;
        }

        public static void AddOnOffButton(float PosX, float PosY, string Text, ref bool State, bool Activeable = true, bool Grayed = false)
        {
            const float DistanceFromButton = 38f;
            //
            PosX += DistanceFromButton;
            Utils.DrawBorderString(Main.spriteBatch, Text, new Vector2(PosX, PosY), (Grayed ? Color.Gray : Color.White));
            PosX -= DistanceFromButton * 1.5f;
            //
            Vector2 ButtonPosition = new Vector2((int)PosX, (int)PosY);
            Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, "[" + (State ? "ON" : "OFF") + "]", ButtonPosition, (State ? Color.Green : Color.Red));
            if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + ButtonDimension.X &&
                Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + ButtonDimension.Y)
            {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (Activeable && Main.mouseLeft && Main.mouseLeftRelease)
                    State = !State;
            }
        }

        public static bool DrawCompanionPointingArrow()
        {
            Vector2 DrawCenter = GetScreenCenter;
            foreach(TerraGuardian tg in CompanionsToShowArrowFor)
            {
                Vector2 PointingDirection = tg.CenterPosition - DrawCenter;
                float Rotation = (float)Math.Atan2(PointingDirection.Y, PointingDirection.X);
                PointingDirection.Normalize();
                PointingDirection.X *= Main.screenWidth * 0.4f;
                PointingDirection.Y *= Main.screenHeight * 0.4f;
                PointingDirection.X += Main.screenWidth * 0.5f;
                PointingDirection.Y += Main.screenHeight * 0.5f;
                Main.spriteBatch.Draw(GuideArrowTexture, PointingDirection, null, Color.White, Rotation + 1.570796f, new Vector2(16, 32), 1f, SpriteEffects.None, 0);
                tg.DrawHead(PointingDirection);
            }
            return true;
        }

        public static int DrawBar(Vector2 Position, int Length, out int hoveritem)
        {
            int ReturnedValue = -1;
            hoveritem = -1;
            for (int s = 0; s < Length; s++)
            {
                Vector2 BarPosition = Position;
                BarPosition.X += s * 56;
                Main.spriteBatch.Draw(TacticsBarTexture, BarPosition, new Rectangle(0, 0, 28, 28), Color.White);
                if (Main.mouseX >= BarPosition.X && Main.mouseX < BarPosition.X + 28 && Main.mouseY >= BarPosition.Y && Main.mouseY < BarPosition.Y + 28)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    hoveritem = s;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        ReturnedValue = s;
                    }
                }
                if (s < Length - 1)
                {
                    BarPosition.X += 28;
                    Main.spriteBatch.Draw(TacticsBarTexture, BarPosition, new Rectangle(28, 0, 28, 28), Color.White);
                }
            }
            return ReturnedValue;
        }
        
        public static void Update2PControl(TerraGuardian Guardian)
        {
            gamePadState = GamePad.GetState(controlPort);
            if (MainMod.Gameplay2PMode && !gamePadState.IsConnected)
            {
                Gameplay2PMode = false;
                Main.NewText("Controller disconnected: 2P mode now turned off.");
            }
            if (CheckForButtonPress(Buttons.Start))
            {
                Gameplay2PMode = !Gameplay2PMode;
                if(Gameplay2PMode && !PlayerMod.GetPlayerMainGuardian(Main.LocalPlayer).Active)
                {
                    Gameplay2PMode = false;
                    Main.NewText("You must have a Leader Guardian following you to start 2P mode.");
                }
                else
                    Main.NewText("2P gameplay is now " + (Gameplay2PMode ? "ON" : "OFF") + ".");
            }
            MoveUpPress = MoveDownPress = MoveLeftPress = MoveRightPress = UseItemPress = false;
            if (Gameplay2PMode && Guardian.Active)
            {
                Vector2 MovementThumbstick = -gamePadState.ThumbSticks.Left; //Negative values brings the intended result.
                Guardian.MoveUp = MovementThumbstick.Y < -0.2f;
                Guardian.MoveDown = MovementThumbstick.Y > 0.2f;
                Guardian.MoveLeft = MovementThumbstick.X > 0.2f;
                Guardian.MoveRight = MovementThumbstick.X < -0.2f;
                Guardian.Action = CheckForButtonPress(Buttons.RightTrigger);
                int SlotChange = Guardian.SelectedItem;
                if (CheckForButtonPress(Buttons.LeftShoulder)) SlotChange--;
                if (CheckForButtonPress(Buttons.RightShoulder)) SlotChange++;
                if (SlotChange < 1) SlotChange += 11;
                if (SlotChange >= 10) SlotChange -= 11;
                Guardian.SelectedItem = SlotChange;
                Guardian.Jump = CheckForButtonPress(Buttons.LeftTrigger);
                Vector2 RightThumbstick = gamePadState.ThumbSticks.Right * 128f;
                RightThumbstick.Y *= -1;
                if(RightThumbstick.Length() == 0)
                {
                    RightThumbstick.X = Guardian.Direction * Guardian.Width * 0.5f;
                }
                Vector2 AimDirection = Guardian.CenterPosition + new Vector2(RightThumbstick.X, RightThumbstick.Y);
                Guardian.AimDirection = AimDirection;
            }
            oldGamePadState = gamePadState;
        }

        public static bool CheckForButtonPress(Buttons button)
        {
            return gamePadState.IsButtonDown(button) && oldGamePadState.IsButtonUp(button);
        }

        public static string GetTitleText
        {
            get
            {
                string s = "";
                switch (Main.rand.Next(8))
                {
                    case 0: s = "With more friends for you to meet!"; break;
                    case 1: s = "Contains snouts, furs and tails."; break;
                    case 2: s = "One Man's Army."; break;
                    case 3: s = "Terrarian's Ark."; break;
                    case 4: s = "Tales from the Ether Realm."; break;
                    case 5: s = "Town management simulator."; break;
                    case 6: s = "It's hard to pick the best."; break;
                    case 7: s = "Gotta meet 'em all!"; break;
                }
                return "Terraria: " + s;
            }
        }
        
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            NpcMod.DrawnCompanionsInFrontOfNpcs = false;
        }

        public override void PostDrawFullscreenMap(ref string mouseText)
        {
            /*foreach (TerraGuardian tg in MainMod.ActiveGuardians.Values)
            {
                if (tg.OwnerPos == -1 || tg.GuardingPosition.HasValue)
                {
                    Vector2 DotPosition = tg.CenterPosition / 16f * Main.mapFullscreenScale;
                    DotPosition.Y -= tg.Height * 0.75f;
                    DotPosition *= Main.mapFullscreenScale;
                    float Scale = 1f;
                    if (Main.mouseX >= DotPosition.X - 8 && Main.mouseX < DotPosition.X + 8 &&
                        Main.mouseY >= DotPosition.Y - 8 && Main.mouseY < DotPosition.Y + 8)
                    {
                        mouseText = tg.ReferenceName;
                        Scale = 2f;
                    }
                    tg.DrawHead(DotPosition, Scale);
                    //
                }
            }*/
        }

        public override void Load()
        {
            orderCallButton = RegisterHotKey("Order Wheel Key", "Q");
            if (!Main.dedServ)
            {
                NinjaTextureBackup = Main.ninjaTexture;

                LosangleOfUnnown = GetTexture("Companions/LosangleOfUnnown");
                GuardianButtonSlots = GetTexture("Interface/GuardianEquipButtons");
                GuardianHealthBar = GetTexture("Interface/GuardianHealthBar");
                FriendshipHeartTexture = GetTexture("Interface/FriendshipHeart");
                TacticsBarTexture = GetTexture("Interface/TacticsBar");
                TacticsIconsTexture = GetTexture("Interface/TacticsIcons");
                EmotionTexture = GetTexture("Interface/Emotions");
                ReportButtonTexture = GetTexture("Interface/ReportButton");
                GuardianMouseTexture = GetTexture("Interface/GuardianMouse");
                EditButtonTexture = GetTexture("Interface/EditButton");
                TrappedCatTexture = GetTexture("Extra/TrappedCat");
                TwoHandedSwordSprite = GetTexture("Items/Weapons/TwoHandedSword");
                GuardianInfoIcons = GetTexture("Interface/GuardianInfoIcons");
                CrownTexture = GetTexture("Interface/Crown");
                GuardianStatusIconTexture = GetTexture("Interface/GuardianStatusIcon");
                HideButtonTexture = GetTexture("Interface/HideButton");
                GuideArrowTexture = GetTexture("Interface/GuideArrow");
                if (Main.rand.NextDouble() < 0.5)
                {
                    Main.instance.Window.Title = GetTitleText;
                }
                EyeTexture = GetTexture("ExtraTextures/Eyes");
                GSI_BackgroundInterfaceTexture = GetTexture("Interface/gsi_bg");
                GSI_ForegroundInterfaceTexture = GetTexture("Interface/gsi_fg");
                ContributorIconTexture = GetTexture("Interface/Contributor_Icon");
                TrustIconsTexture = GetTexture("Interface/Trust_Icons");
            }
            AddNewGroup(GuardianBase.TerraGuardianGroupID, "TerraGuardian", true, true);
            AddNewGroup(GuardianBase.TerrarianGroupID, "Terrarian", false);
            AddNewGroup(GuardianBase.TerraGuardianCaitSithGroupID, "Cait Sith Guardian", 1f / 0.272f, true, true);
            Group g = AddNewGroup(GuardianBase.GiantDogGuardianGroupID, "Giant Dog Guardian", 4.6667f, true, true);
            g.ReverseMountWhenUnderaged = false;
            GetInitialCompanionsList();
            CommonRequestsDB.PopulateCommonRequestsDB();
            AddQuestContainer(this, new Quests.TgQuestContainer());
            new Requests.RequestDB(); //This automatically adds the RequestDB to the request database, so... Nothing else needed to do.
            RequestReward.InitializeRequestRewards();
            //
            NExperienceMod = ModLoader.GetMod("NExperience");
            KalciphozMod = ModLoader.GetMod("kRPG");
            SubworldLibrary = ModLoader.GetMod("SubworldLibrary");
            TerraClassesMod = ModLoader.GetMod("TerraClasses");
            ThoriumMod = ModLoader.GetMod("ThoriumMod");
            FargoMutantMod = ModLoader.GetMod("Fargowiltas");
        }

        public override void Unload()
        {
            GuardianBase.ClearGuardianContainers();
            ModGuardianLoader.Clear();
            RequestContainer.DisposeRequests();
        }

        public static void AddQuestContainer(Mod mod, QuestContainer container)
        {
            QuestContainer.AddQuestContainer(mod, container);
        }

        public override void PostSetupContent()
        {
            W1kWeaponScalingSupportSetup();
        }

        private void W1kWeaponScalingSupportSetup()
        {
            Mod scaling = ModLoader.GetMod("WWeaponScaling");
            if (scaling == null)
                return;
            /*WEAPON TIERS
               1 = Wooden
               2 = Iron
               3 = Gold
               4 = Evil
               5 = Jungle
               6 = Dungeon
               7 = Molten
               8 = Adamantite
               9 = Hallowed
               10 = Chlorophyte
               11 = Post-Plantera
               12 = Post-Golem
               13 = Lunar
               14 = Moon Lord*/
            scaling.Call(ModContent.ItemType<Items.Weapons.WoodenGreathammer>(), 1, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.TwoHandedSword>(), 2, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.WoodenCrossbow>(), 1, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.UprootedTree>(), 11, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.TheStinger>(), 5, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.ScreamingSalamander>(), 7, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.RottenBoomer>(), 4, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.ReallyHeavyChaingun>(), 12, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.MeatMasher>(), 4, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.MahoganyCompositeBow>(), 1, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.JungleStar>(), 5, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.FleshCarver>(), 4, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.Flamberge>(), 7, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.DaylightBane>(), 4, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.ChlorophyteCrescentAxe>(), 10, 1f);
            scaling.Call(ModContent.ItemType<Items.Weapons.BladeOfLight>(), 9, 1f);
        }

        public static string[] GetItemInfo(Item i, TerraGuardian guardian)
        {
            List<string> Text = new List<string>();
            Text.Add(i.HoverName);
            Text.Add("Info based on " + guardian.Name + ".");
            if (i.damage > 0)
            {
                int DamageValue = i.damage;
                if (guardian.Active)
                {
                    float Multiplier = guardian.NeutralDamageMultiplier;
                    if (i.melee)
                        Multiplier = guardian.MeleeDamageMultiplier;
                    if (i.ranged)
                        Multiplier = guardian.RangedDamageMultiplier;
                    if (i.magic)
                        Multiplier = guardian.MagicDamageMultiplier;
                    if (i.summon)
                        Multiplier = guardian.SummonDamageMultiplier;
                    if (i.thrown)
                        Multiplier = guardian.RangedDamageMultiplier;
                    DamageValue = (int)(DamageValue * Multiplier);
                }
                string DamageType = " Neutral";
                if (i.melee)
                    DamageType = " Melee";
                if (i.ranged || i.thrown)
                    DamageType = " Ranged";
                if (i.magic)
                    DamageType = " Magic";
                if (i.summon)
                    DamageType = " Summon";
                Text.Add(DamageValue + DamageType + " Damage");
            }
            if (i.useAnimation > 0)
            {
                float UseTime = i.useAnimation;
                if (i.melee) UseTime *= guardian.MeleeSpeed;
                if (i.ranged) UseTime *= guardian.RangedSpeed;
                if (i.magic) UseTime *= guardian.MagicSpeed;
                UseTime = UseTime / 60;
                Text.Add("Attack Speed: " + Math.Round(UseTime, 1) + "s");
            }
            if (i.defense > 0)
            {
                int DefenseValue = i.defense;
                Text.Add(DefenseValue + " Defense");
            }
            /*for (int l = 0; l < i.ToolTip.Lines; l++)
            {
                Text.Add(i.ToolTip.GetLine(l));
            }*/
            int p = 0, g = 0, s = 0, c = i.value;
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
            string PriceText = "Price: ";
            if (p > 0)
                PriceText += p + "P ";
            if (g > 0)
                PriceText += g + "G ";
            if (s > 0)
                PriceText += s + "S ";
            if (c > 0)
                PriceText += c + "C";
            Text.Add(PriceText);
            return Text.ToArray();
        }

        public static string PluralifyWord(string Word)
        {
            if (Word.Length > 1 && Word[Word.Length - 1] == 'f')
            {
                return Word.Substring(0, Word.Length - 1) + "ves";
            }
            if (Word.Length > 2 && Word.Substring(Word.Length - 2) == "fe")
            {
                return Word.Substring(0, Word.Length - 2) + "ves";
            }
            if (Word.Length > 1 && Word[Word.Length - 1] == 's')
                return Word;
            return Word + 's';
        }

        public enum GuardianItemSlotButtons
        {
            Nothing = -1,
            GuardianSelection,
            Inventory,
            Equipment,
            ExtraStorage,
            CombatTactics,
            Requests,
            Skills,
            QuestBook
        }
	}
}
