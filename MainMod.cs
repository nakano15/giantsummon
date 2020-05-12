using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace giantsummon
{
	public class MainMod : Mod
	{
        public static Texture2D GuardianButtonSlots, GuardianHealthBar, FriendshipHeartTexture, EmotionTexture, ReportButtonTexture, GuardianMouseTexture, EditButtonTexture,
            GuardianInfoIcons, CrownTexture, GuardianStatusIconTexture, HideButtonTexture;
        public static Texture2D TacticsBarTexture, TacticsIconsTexture;
        public static Texture2D TrappedCatTexture;
        public static Texture2D NinjaTextureBackup;
        public static Texture2D TwoHandedSwordSprite;
        public static Texture2D LosangleOfUnnown;
        public static Mod mod;
        public static ModPacket GetPatcket { get { return mod.GetPacket(); } }
        public static GuardianItemSlotButtons SelectedGuardianInventorySlot = GuardianItemSlotButtons.Nothing;
        public static SoundData FemaleHitSound = new SoundData(Terraria.ID.SoundID.FemaleHit);
        public static byte SelectedGuardian = 0;
        public static bool WarnAboutSaleableInventorySlotsLeft = false, MobHealthBoost = false, GuardiansIdleEasierOnTowns = true;
        //Contest related
        public const string VoteLink = "https://forms.gle/CQpXjU2HrSvuFwXg9";
        public static bool HasPlayerAwareOfContestMonthChange = false;
        public const int LastContestModVersion = 56;
        public const string ContestResultLink = "https://forums.terraria.org/index.php?threads/terraguardians-terrarian-companions.81757/post-1946985";
        //End contest related
        public static int GuardianInventoryMenuSubTab = 0;
        public const int ModVersion = 58, LastModVersion = 56;
        public const int MaxExtraGuardianFollowers = 4;
        public static bool ShowDebugInfo = false;
        public static bool PlayableOnMultiplayer = false, TestNewCombatAI = true, UseNewMonsterModifiersSystem = true, UsingGuardianNecessitiesSystem = false, TestNewOrderHud = true, SharedCrystalValues = false,
            SetGuardiansHealthAndManaToPlayerStandards = false, UseSkillsSystem = true, UseNewDownedSystem = true;
        public static bool CharacterDoesntDiesAfterDownedDefeat = true;
        public static bool ForceUpdateGuardiansStatus = false;
        public static bool ManagingGuardianEquipments = false;
        public const bool IndividualSkillLeveling = false;
        public static PlayerIndex controlPort = PlayerIndex.Two;
        public static GamePadState gamePadState = GamePad.GetState(controlPort),
            oldGamePadState = GamePad.GetState(controlPort);
        public static bool Gameplay2PMode = false;
        public static bool MoveLeftPress = false, MoveRightPress = false, MoveUpPress = false, MoveDownPress = false, UseItemPress = false, JumpPress = false;
        public static Mod NExperienceMod, KalciphozMod;
        public static bool TestForceGuardianOnFront = false;
        public static Main MainHook { get { return Main.instance; } }
        public static bool LastWof = false;
        public static int LastTrashItemID = 0;
        public static int NemesisFadeEffect = -NemesisFadeCooldown;
        public const int NemesisFadeCooldown = 15 * 60, NemesisFadingTime = 3 * 60;
        public static bool PauseGameWhenUsingOrders = false;
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
        public static List<int> InitialGuardians = new List<int>();
        public static List<KeyValuePair<byte, int>> PlayerGuardianSync = new List<KeyValuePair<byte, int>>();
        public static bool NetplaySync = true;
        public static ModHotKey orderCallButton;
        public static bool OrderCallButtonPressed = false;//, MakeUseOfOrderHotkey = false;
        private static Dictionary<int, TerraGuardian> TempActiveGuardians = new Dictionary<int, TerraGuardian>();
        public static Dictionary<int, TerraGuardian> ActiveGuardians = new Dictionary<int,TerraGuardian>();
        public static float CarpetAnimationTime = 0;
        public static Vector2 FocusCameraPosition = Vector2.Zero;
        public static byte DButtonPress = 255;

        public static void AddActiveGuardian(TerraGuardian Guardian)
        {
            TempActiveGuardians.Add(Guardian.WhoAmID, Guardian);
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
        
        public static Tile GetTile(int x, int y)
        {
            if (x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY)
            {
                return Main.tile[x, y];
            }
            else
            {
                return Main.tile[0,0];
            }
        }

        public static bool IsSolidTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            return tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
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

        public static bool HasSyncFlag(int Player, int Guardian)
        {
            for (int s = 0; s < PlayerGuardianSync.Count; s++)
            {
                if (PlayerGuardianSync[s].Key == Player && PlayerGuardianSync[s].Value == Guardian)
                {
                    return true;
                }
            }
            PlayerGuardianSync.Add(new KeyValuePair<byte, int>((byte)Player, Guardian));
            return false;
        }

        public MainMod()
		{
            mod = this;
		}

        public static void AddInitialGuardian(int NpcID)
        {
            InitialGuardians.Add(NpcID);
        }

        public static void AddGuardianList(Mod ModName, GuardianBase.ModGuardianDB guardiandb)
        {
            if (!ModGuardianLoader.ContainsKey(ModName.Name))
            {
                ModGuardianLoader.Add(ModName.Name, guardiandb);
            }
        }

        /*public bool AddGuardianToPlayer(Player player, int ID, Mod mod)
        {
            return player.GetModPlayer<PlayerMod>().AddNewGuardian(ID, mod.Name);
        }*/

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
            return dt.Year == 2020 && (dt.Month < 5 || (dt.Month == 5 && dt.Day < 19));
        }

        public override void Unload()
        {
            GuardianBase.UnloadGuardians();
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

        }

        public override void MidUpdateProjectileItem()
        {

        }

        public override void PreUpdateEntities()
        {
            /*if (Main.netMode == 0 && PauseGameWhenUsingOrders && GuardianOrderHud.OrderSelection)
            {
                Main.gamePaused = true;
            }*/
            ActiveGuardians = TempActiveGuardians;
            TempActiveGuardians = new Dictionary<int, TerraGuardian>();
        }

        public override void MidUpdateNPCGore()
        {
            NpcMod.RestorePlayersPosition();
        }

        public override void MidUpdatePlayerNPC()
        {
            GuardianBountyQuest.Update();
            NemesisFadeEffect++;
            if (NemesisFadeEffect >= NemesisFadingTime)
                NemesisFadeEffect -= NemesisFadeCooldown + NemesisFadingTime;
            for (int Assists = 0; Assists < MaxExtraGuardianFollowers + 1; Assists++)
            {
                List<PlayerDataBackup> DataBackup = new List<PlayerDataBackup>();
                bool[] PlayerWasActive = new bool[255];
                for (int p = 0; p < 255; p++)
                {
                    PlayerWasActive[p] = Main.player[p].active;
                    if (Main.player[p].active)
                    {
                        TerraGuardian g = Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers[Assists];
                        if (g.Active && !g.Downed && !g.PlayerMounted && !g.SittingOnPlayerMount && (Math.Abs(Main.player[p].Center.X - g.CenterPosition.X) >= NPC.sWidth * 2 || Math.Abs(Main.player[p].Center.Y - g.CenterPosition.Y) >= NPC.sHeight * 2))// && Main.player[p].Distance(Main.player[p].GetModPlayer<PlayerMod>().Guardian.CenterPosition) >= 1512f)
                        {
                            PlayerDataBackup db = new PlayerDataBackup(Main.player[p], Main.player[p].GetModPlayer<PlayerMod>().Guardian);
                            DataBackup.Add(db);
                        }
                        else
                        {
                            Main.player[p].active = false;
                        }
                    }
                }
                NPC.SpawnNPC();
                for (int p = 0; p < 255; p++)
                {
                    Main.player[p].active = PlayerWasActive[p];
                }
                foreach (PlayerDataBackup db in DataBackup)
                {
                    db.RestorePlayerStatus();
                }
            }
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
        }

        public override void PostUpdateEverything()
        {
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
        }

        public override void ModifyInterfaceLayers(System.Collections.Generic.List<Terraria.UI.GameInterfaceLayer> layers)
        {
            if (ShowDebugInfo)
            {
                Terraria.UI.LegacyGameInterfaceLayer gi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Debug Interface", DrawDebugInterface, Terraria.UI.InterfaceScaleType.UI);
                layers.Add(gi);
            }
            if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().KnockedOut)
            {
                /*if (!Main.player[Main.myPlayer].dead)
                {
                    for (int l = 0; l < layers.Count; l++)
                    {
                        if (!layers[l].Name.Contains("Mouse"))
                        {
                            layers.RemoveAt(l);
                        }
                    }
                }*/
                if (Main.playerInventory)
                    Main.playerInventory = false;
                Terraria.UI.LegacyGameInterfaceLayer downedInterface = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Downed Interface", DrawDownedInterface, InterfaceScaleType.UI);
                layers.Insert(0, downedInterface);
                if (!Main.player[Main.myPlayer].dead)
                {
                    return;
                }
            }
            int MouseSlotPosition = -1, InventorySlotPosition = -1, ResourceBarPosition = -1, PlayerChatPos = -1;
            bool RemoveInventory = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian.PlayerControl;
            for (int l = 0; l < layers.Count; l++)
            {
                if (MouseSlotPosition == -1 && layers[l].Name.Contains("Mouse"))
                    MouseSlotPosition = l - 1;
                if(PlayerChatPos == -1 && layers[l].Name.Contains("Player Chat"))
                    PlayerChatPos = l - 1;
                if (InventorySlotPosition == -1 && layers[l].Name.Contains("Inventory"))
                {
                    InventorySlotPosition = l - 1;
                }
                if (ResourceBarPosition == -1 && layers[l].Name.Contains("Resource Bar"))
                {
                    ResourceBarPosition = l;
                    if (RemoveInventory)
                        layers.RemoveAt(l);
                }
            }
            Terraria.UI.LegacyGameInterfaceLayer dgi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Inventory Interface", DrawGuardianInventoryInterface, Terraria.UI.InterfaceScaleType.UI);
            Terraria.UI.LegacyGameInterfaceLayer hsi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Status Interface", DrawGuardianHealthStatusInterface, Terraria.UI.InterfaceScaleType.UI);
            Terraria.UI.LegacyGameInterfaceLayer gsi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Selection Interface", DrawGuardianSelectionInterface, Terraria.UI.InterfaceScaleType.UI);
            Terraria.UI.LegacyGameInterfaceLayer goi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Order Selection Interface", DrawGuardianOrderInterface, Terraria.UI.InterfaceScaleType.UI);
            Terraria.UI.LegacyGameInterfaceLayer gmi = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Mouse Interface", DrawGuardianMouse, Terraria.UI.InterfaceScaleType.UI);
            Terraria.UI.LegacyGameInterfaceLayer dnagd = new Terraria.UI.LegacyGameInterfaceLayer("Terra Guardians: Dialogue Interface", DrawNpcsAndGuardianDialogues, Terraria.UI.InterfaceScaleType.UI);
            if (PlayerChatPos > -1)
            {
                layers.Insert(PlayerChatPos, dnagd);
            }
            else if (MouseSlotPosition > -1)
            {
                layers.Insert(MouseSlotPosition, dnagd);
            }
            if (MouseSlotPosition > -1)
            {
                layers.Insert(MouseSlotPosition, goi);
                layers.Insert(MouseSlotPosition, gmi);
            }
            else
            {
                layers.Add(goi);
                layers.Add(gmi);
            }
            if (InventorySlotPosition > -1)
                layers.Insert(InventorySlotPosition, dgi);
            else if (MouseSlotPosition > -1)
                layers.Insert(MouseSlotPosition, dgi);
            else
                layers.Add(dgi);
            if (MouseSlotPosition > -1)
            {
                layers.Insert(MouseSlotPosition, hsi);
                layers.Insert(MouseSlotPosition, gsi);
            }
            else
                layers.Add(hsi);
        }

        public static bool RectangleIntersects(Vector2 p1, int p1width, int p1height, Vector2 p2, int p2width, int p2height)
        {
            return p1.X < p2.X + p2width && p2.X < p1.X + p1width && p1.Y > p2.Y + p2height && p2.Y > p1.Y + p1height;
        }

        public static bool DrawDownedInterface()
        {
            //Add a script for when the player is controlling a guardian, instead.
            const int BarWidth = 360, BarHeight = 14;
            float BarSize = (float)Main.player[Main.myPlayer].statLife / Main.player[Main.myPlayer].statLifeMax2;
            if (BarSize > 1f)
                BarSize = 1f;
            if (BarSize < 0)
                BarSize = 0f;
            //
            int BarDimY = (int)(Main.screenHeight * (1f - BarSize) * 0.25f);
            Color ClosingBarsColor = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().KnockedOutCold ? new Color(0, 0, 0, (int)(255 * (1f - BarSize))) : new Color((byte)(128 * (1f - BarSize)), 0, 0, (int)(255 * (1f - BarSize)));
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, 0, Main.screenWidth, BarDimY), ClosingBarsColor);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, BarDimY, Main.screenWidth, BarDimY), ClosingBarsColor * 0.5f);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, (int)(Main.screenHeight - BarDimY * 2), Main.screenWidth, BarDimY), ClosingBarsColor * 0.5f);
            Main.spriteBatch.Draw(Main.blackTileTexture, new Rectangle(0, (int)(Main.screenHeight - BarDimY), Main.screenWidth, BarDimY), ClosingBarsColor);
            //
            if (!Main.player[Main.myPlayer].dead)
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
                string Text = "Downed";
                if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().ReviveBoost > 0)
                    Text = "Being Revived";
                else if (Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().KnockedOutCold)
                {
                    Text = "Press '" + Main.cHook + "' to give up.";
                }
                Utils.DrawBorderStringBig(Main.spriteBatch, Text, new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.75f), Color.OrangeRed, 1f, 0.5f, 0.5f);
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
            if (guardian.Active && (ShowDebugInfo || Gameplay2PMode))
            {
                Main.spriteBatch.Draw(GuardianMouseTexture, new Vector2(guardian.AimDirection.X, guardian.AimDirection.Y) - Main.screenPosition, Main.mouseColor);
            }
            return true;
        }

        public bool DrawDebugInterface()
        {
            if (Main.playerInventory || Main.mapFullscreen)
                return true;
            Vector2 DrawPos = new Vector2(0, 138f + 64 * 2);
            TerraGuardian guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            if (!guardian.Active) return true;
            string[] TextsToDraw = new string[]{
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
            };
            if(true)
            {
                List<string> New = new List<string>();
                New.Add("Moving Up: " + guardian.LastMoveUp);
                New.Add("Moving Down: " + guardian.LastMoveDown);
                New.Add("Moving Left: " + guardian.LastMoveLeft);
                New.Add("Moving Right: " + guardian.LastMoveRight);
                New.Add("Action: " + guardian.LastAction);
                New.Add("Jump: " + guardian.Jump);
                New.Add("Target On Sight: " + guardian.TargetInAim);
                New.Add("Target Position: " + guardian.TargetID);
                /*foreach (TerraGuardian g in Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                {
                    if (!g.Active) continue;
                    New.Add(g.Name + ": " + g.Data.GetNecessityStatus);
                    New.Add("Fatigue: " + g.Data.Fatigue);
                    New.Add("Injury: " + g.Data.Injury);
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

        public bool DrawNpcsAndGuardianDialogues()
        {
            for (int n = 0; n < 200; n++)
            {
                if (Main.npc[n].active)
                {
                    if (Main.npc[n].modNPC is GuardianNPC.GuardianNPCPrefab)
                    {
                        GuardianNPC.GuardianNPCPrefab npc = ((GuardianNPC.GuardianNPCPrefab)Main.npc[n].modNPC);
                        npc.Guardian.DrawSocialMessages();
                    }
                    if (Main.npc[n].modNPC is Npcs.GuardianActorNPC)
                    {
                        Npcs.GuardianActorNPC npc = ((Npcs.GuardianActorNPC)Main.npc[n].modNPC);
                        npc.DrawMessage(Main.spriteBatch);
                    }
                }
            }
            for (int p = 0; p < 255; p++)
            {
                if (Main.player[p].active)
                {
                    foreach (TerraGuardian g in Main.player[p].GetModPlayer<PlayerMod>().GetAllGuardianFollowers)
                    {
                        if (g.Active)
                            g.DrawSocialMessages();
                    }
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
            if (Main.playerInventory || Main.mapFullscreen)
                return true;
            //TerraGuardian Guardian = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().Guardian;
            const float StartY = 138;
            Vector2 HealthbarPosition = new Vector2(16f, StartY);
            string MouseOverText = "";
            bool IsMainGuardian = true;
            //Utils.DrawBorderString(Main.spriteBatch, "2P Press Start", HealthbarPosition, Color.White, 0.95f);
            //HealthbarPosition.Y += 22f;
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
                    if (Guardian.Base.IsTerraGuardian)
                    {
                        Texture2D HeadTexture = Guardian.Base.sprites.HeadSprite;
                        Vector2 HeadDrawPosition = HealthbarPosition;
                        HeadDrawPosition.X += 16;
                        HeadDrawPosition.Y += 16;
                        Main.spriteBatch.Draw(HeadTexture, HeadDrawPosition, null, Color.White, 0f, new Vector2(HeadTexture.Width * 0.5f, HeadTexture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
                        //HeadDrawPosition.X += 8f;
                        HeadDrawPosition.Y += 8f;
                        Guardian.DrawFriendshipHeart(HeadDrawPosition);
                        XSum = 46f;
                    }
                    else if (Guardian.Base.IsTerrarian)
                    {
                        Texture2D HeadTexture = Guardian.Base.sprites.HeadSprite;
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
                string HealthText = Guardian.Name + ": " + Guardian.HP + "/" + Guardian.MHP;
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
                    MouseOverText = "Life Crystals: " + Guardian.LifeCrystalHealth + "/" + TerraGuardian.MaxLifeCrystals + "  Life Fruits: " + Guardian.LifeFruitHealth + "/" + TerraGuardian.MaxLifeFruit;
                    if (Guardian.Data.Injury > 0)
                        MouseOverText += "  [Injury " + Guardian.Data.Injury + "%]";
                }
                int MaxHealthBarSize = 98;
                if(UsingGuardianNecessitiesSystem && Guardian.Data.Injury > 0 && !Downed)
                    MaxHealthBarSize = (int)(MaxHealthBarSize * ((float)Guardian.Data.Injury / 100));
                if (Downed)
                {
                    int BarWidth = (int)(MaxHealthBarScale * 98);
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 4 + 22, 16 + 4, BarWidth, 8), Color.White);
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        float BarScale = 1f;
                        switch (i)
                        {
                            case 0: //Red health bar
                                BarScale = MaxHealthBarScale;
                                break;
                            case 1:
                                BarScale = (float)Guardian.LifeCrystalHealth / TerraGuardian.MaxLifeCrystals;
                                break;
                            case 2:
                                BarScale = (float)Guardian.LifeFruitHealth / TerraGuardian.MaxLifeFruit;
                                break;
                        }
                        if (BarScale > MaxHealthBarScale)
                            BarScale = MaxHealthBarScale;
                        //Draw bar
                        int BarWidth = (int)(MaxHealthBarSize * BarScale);
                        Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(22, 16 * (i + 1) + 4, BarWidth, 8), Color.White);
                    }
                }
                if (UsingGuardianNecessitiesSystem && Guardian.Data.Injury > 0)
                {
                    Vector2 InjuryPosition = HealthbarPosition;
                    InjuryPosition.X += 98 - MaxHealthBarSize;
                    Main.spriteBatch.Draw(GuardianHealthBar, InjuryPosition, new Rectangle(22 + 98 - MaxHealthBarSize, 16 * 4 + 4, MaxHealthBarSize, 8), Color.White);
                }
                {
                    HealthbarPosition.X -= 22;
                    HealthbarPosition.Y += 16;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 3, 0, 122, 16), Color.White);
                    float BarValue = ((float)Guardian.MP / Guardian.MMP);
                    HealthbarPosition.X += 22;
                    HealthbarPosition.Y += 4;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 3 + 22, 20 + 16, (int)(98 * BarValue), 8), Color.White);
                    float ManaCrystalProgress = (float)Guardian.Data.ManaCrystals / GuardianData.MaxManaCrystals;
                    if (ManaCrystalProgress > BarValue)
                        ManaCrystalProgress = BarValue;
                    Main.spriteBatch.Draw(GuardianHealthBar, HealthbarPosition, new Rectangle(122 * 3 + 22, 20, (int)(98 * ManaCrystalProgress), 8), Color.White);
                    if (Main.mouseX >= HealthbarPosition.X && Main.mouseX < HealthbarPosition.X + 98 &&
                        Main.mouseY >= HealthbarPosition.Y && Main.mouseY < HealthbarPosition.Y + 8)
                        MouseOverText = "Mana: " + Guardian.MP + "/" + Guardian.MMP + "  Mana Crystals: " + Guardian.ManaCrystals + "/" + GuardianData.MaxManaCrystals;
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
                            //Utils.DrawBorderString(Main.spriteBatch, Lang.GetBuffName(Guardian.Buffs[b].ID) + "\n" + Lang.GetBuffDescription(Guardian.Buffs[b].ID), new Vector2(Main.mouseX + 8, Main.mouseY + 8), Color.White);
                        }
                        HealthbarPosition.X += TextureWidth * 1.10f;
                    }
                    HealthbarPosition.Y += 32;
                }
                HealthbarPosition.X = 16;
                IsMainGuardian = false;
            }
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

        public bool DrawGuardianInventoryInterface()
        {
            if (GuardianManagement.Active)
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
            //
            float StartX = 0;
            float StartY = 258;
            if (MainMod.KalciphozMod != null)
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
                                SelectedGuardian = g;
                        }
                        if (g != SelectedGuardian)
                        {
                            ThisPosition.Y += 8f;
                        }
                        if (guardian.Base.IsTerraGuardian)
                        {
                            Texture2D texture;
                            if (guardian.Base.InvalidGuardian)
                                texture = LosangleOfUnnown;
                            else
                                texture = guardian.Base.sprites.HeadSprite;
                            float ScaleMod = (texture.Width > texture.Height ? 32f / texture.Width : 32f / texture.Height);
                            Main.spriteBatch.Draw(texture, ThisPosition, null, Color.White, 0f, Vector2.Zero, ScaleMod, SpriteEffects.None, 0f);
                            SelectionPosition.X += 32;
                        }
                        else if(guardian.Base.IsTerrarian)
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
            List<GuardianItemSlotButtons> Buttons = new List<GuardianItemSlotButtons>();
            Buttons.Add(GuardianItemSlotButtons.GuardianSelection);
            Buttons.Add(GuardianItemSlotButtons.Requests);
            if (Guardian.Active)
            {
                Buttons.Clear();
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
                                case GuardianItemSlotButtons.SpellBook:
                                    MouseOverText = "Guardian Spells";
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
                                    Main.spriteBatch.Draw(MainMod.GuardianButtonSlots, ButtonPosition + new Vector2(x, y), new Rectangle(36 * b, 0, 36, 36), Color.White, 0f, Vector2.Zero, ButtonSize, SpriteEffects.None, 0f);
                                }
                            }
                        }
                        Main.spriteBatch.Draw(MainMod.GuardianButtonSlots, ButtonPosition, new Rectangle(36 * b, 0, 36, 36), Color.White, 0f, Vector2.Zero, ButtonSize, SpriteEffects.None, 0f);
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
                        break;
                    case GuardianItemSlotButtons.Equipment:
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "Equipments", SlotStartPosition, Color.White);
                            ManagingGuardianEquipments = true;
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
                                        const int TestGuardianID = 13;
                                        bool b = false;
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
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Start Blood Moon ", ref b);
                                        if (b)
                                        {
                                            Main.dayTime = false;
                                            Main.time = 0;
                                            Main.bloodMoon = true;
                                        }
                                        SlotStartPosition.Y += 26;
                                        b = (!Main.dayTime && Main.time >= 4.5 * 3600) || (Main.dayTime && Main.time < 3 * 3600);
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Start Late Night ", ref b);
                                        if (b)
                                        {
                                            Main.dayTime = false;
                                            Main.time = 4.5 * 3600;
                                            Main.bloodMoon = false;
                                        }
                                        SlotStartPosition.Y += 26;
                                        b = false;
                                        AddOnOffButton(ButtonPosX, SlotStartPosition.Y, "Reset Request Timer", ref b);
                                        if (b)
                                        {
                                            GuardianData[] gds = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetGuardians();
                                            foreach (GuardianData gd in gds)
                                            {
                                                if (gd.request.Active)
                                                {
                                                    gd.request.Active = false;
                                                    gd.request.SetNewRequestTimer();
                                                }
                                                else
                                                {
                                                    gd.request.GenerateRequest(Main.player[Main.myPlayer], gd);
                                                }
                                            }
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
                                    break;
                                case 1: //Combat
                                    if (!Guardian.AvoidCombat)
                                    {
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
                                                MouseOverText = "Attack far away from target.";
                                                break;
                                            case 1:
                                                MouseOverText = "Attack near the target, but keep some distance.";
                                                break;
                                            case 2:
                                                MouseOverText = "Try melee combat with the target.";
                                                break;
                                        }
                                        Main.spriteBatch.Draw(TacticsIconsTexture, SlotStartPosition + new Vector2((int)Guardian.tactic * 56 - 4, -8), new Rectangle(0, 0, 36, 36), Color.White);
                                        Main.spriteBatch.Draw(TacticsIconsTexture, SlotStartPosition + new Vector2(56 * 3 - 4, -4), new Rectangle(36, 0, 36, 36), Color.White);

                                        SlotStartPosition.Y += 26;
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
                            if (d.request.Active)
                            {
                                if (d.ID == Guardian.ID)
                                    RequestCount.Insert(0, d);
                                else
                                    RequestCount.Add(d);
                            }
                        }
                        bool HasRequest = false;
                        bool HasUnsummonedGuardianRequest = false;
                        foreach (GuardianData d in RequestCount)
                        {
                            if (d.request.Active)
                            {
                                if (!HasRequest)
                                {
                                    Utils.DrawBorderString(Main.spriteBatch, "Guardian Requests", SlotStartPosition, Color.White);
                                    SlotStartPosition.Y += 36;
                                    //SlotStartPosition.X -= 24;
                                    HasRequest = true;
                                }
                                if (Guardian.Active == false || d.ID != Guardian.ID) HasUnsummonedGuardianRequest = true;
                                //Guardian.DrawFriendshipHeart(SlotStartPosition, d.FriendshipLevel, d.FriendshipProgression);
                                //SlotStartPosition.X += 24f;
                                //Utils.DrawBorderString(Main.spriteBatch, d.Name + "'s Request", SlotStartPosition, Color.White);
                                //SlotStartPosition.X -= 24f;
                                //SlotStartPosition.Y += 28f;
                                SlotStartPosition.Y += Utils.DrawBorderString(Main.spriteBatch, d.request.GetRequestInformation(d), SlotStartPosition, Color.White).Y;
                                Utils.DrawBorderString(Main.spriteBatch, "Duration: " + d.request.GetRequestDuration, SlotStartPosition, Color.White);
                                SlotStartPosition.Y += 28f;
                                if (Guardian.Active && Guardian.ID == d.ID)
                                {
                                    if (Main.mouseX >= SlotStartPosition.X && Main.mouseX < SlotStartPosition.X + 58 &&
                                        Main.mouseY >= SlotStartPosition.Y && Main.mouseY < SlotStartPosition.Y + 22)
                                    {
                                        Main.player[Main.myPlayer].mouseInterface = true;
                                        MouseOverText = "Click to report the quest current progress on the summoned guardian.";
                                        if (Main.mouseLeft && Main.mouseLeftRelease)
                                        {
                                            TerraGuardian[] guardians = Main.player[Main.myPlayer].GetModPlayer<PlayerMod>().GetAllGuardianFollowers;
                                            TerraGuardian pickedguardian = Guardian;
                                            foreach (TerraGuardian guardian in guardians)
                                            {
                                                if (guardian.Active && guardian.ID == d.ID && guardian.ModID == d.ModID)
                                                {
                                                    pickedguardian = guardian;
                                                    break;
                                                }
                                            }
                                            d.ReportRequest(pickedguardian);
                                        }
                                    }
                                    Main.spriteBatch.Draw(ReportButtonTexture, SlotStartPosition, Color.White);
                                    SlotStartPosition.Y += 22f;
                                }
                                else
                                {
                                }
                                SlotStartPosition.Y += 8f;
                            }
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
                        Utils.DrawBorderString(Main.spriteBatch, "Guardian Skills", SlotStartPosition, Color.White);
                        SlotStartPosition.Y += 36;
                        int LevelSum = 0;
                        foreach (GuardianSkills s in Guardian.Data.SkillList)
                        {
                            Vector2 SkillInfoDimension = Utils.DrawBorderString(Main.spriteBatch, s.GetSkillInfo(Guardian), SlotStartPosition, Color.White);
                            if (Main.mouseX >= SlotStartPosition.X && Main.mouseX < SlotStartPosition.X + SkillInfoDimension.X &&
                                Main.mouseY >= SlotStartPosition.Y && Main.mouseY < SlotStartPosition.Y + SkillInfoDimension.Y)
                            {
                                MouseOverText = s.GetSkillDescription;
                            }
                            SlotStartPosition.Y += SkillInfoDimension.Y;
                            LevelSum += s.Level;
                        }
                        SlotStartPosition.Y += 36;
                        Utils.DrawBorderString(Main.spriteBatch, "Total Levels: " + LevelSum, SlotStartPosition, Color.White);
                        SlotStartPosition.Y += 22;
                        if (Guardian.Data.SkillLevelSum < Guardian.Data.LastTotalSkillLevel)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "Bonus Skill EXP until total level: " + Guardian.Data.LastTotalSkillLevel, SlotStartPosition, Color.White);
                            SlotStartPosition.Y += 22;
                        }
                        break;

                    case GuardianItemSlotButtons.SpellBook:
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
                List<string> ItemInfo = new List<string>();
                //ItemInfo.Add(HoverItem.HoverName);
                ItemInfo.AddRange(GetItemInfo(HoverItem, Guardian));
                /*Vector2 TextPos = new Vector2(Main.mouseX + 16, Main.mouseY + 16);
                foreach (string s in ItemInfo)
                {
                    Utils.DrawBorderString(Main.spriteBatch, s, TextPos, Color.White);
                    TextPos.Y += 26f;
                }*/
            }
            return true;
        }

        public static void AddOnOffButton(float PosX, float PosY, string Text, ref bool State)
        {
            const float DistanceFromButton = 38f;
            //
            PosX += DistanceFromButton;
            Utils.DrawBorderString(Main.spriteBatch, Text, new Vector2(PosX, PosY), Color.White);
            PosX -= DistanceFromButton * 1.5f;
            //
            Vector2 ButtonPosition = new Vector2((int)PosX, (int)PosY);
            Vector2 ButtonDimension = Utils.DrawBorderString(Main.spriteBatch, "[" + (State ? "ON" : "OFF") + "]", ButtonPosition, (State ? Color.Green : Color.Red));
            if (Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + ButtonDimension.X &&
                Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + ButtonDimension.Y)
            {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                    State = !State;
            }
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
                Main.NewText("2P gameplay is now " + (Gameplay2PMode ? "ON" : "OFF") + ".");
            }
            MoveUpPress = MoveDownPress = MoveLeftPress = MoveRightPress = UseItemPress = false;
            if (Gameplay2PMode && Guardian.Active)
            {
                Vector2 Movement = gamePadState.ThumbSticks.Left;
                Guardian.MoveUp = Movement.Y < -0.2f;
                Guardian.MoveDown = Movement.Y > 0.2f;
                Guardian.MoveLeft = Movement.X < -0.2f;
                Guardian.MoveRight = Movement.X > 0.2f;
                Guardian.Action = CheckForButtonPress(Buttons.RightTrigger);
                int SlotChange = Guardian.SelectedItem;
                if (CheckForButtonPress(Buttons.LeftShoulder)) SlotChange--;
                if (CheckForButtonPress(Buttons.RightShoulder)) SlotChange++;
                if (SlotChange < 0) SlotChange += 10;
                if (SlotChange >= 10) SlotChange -= 10;
                Guardian.SelectedItem = SlotChange;
                Guardian.Jump = CheckForButtonPress(Buttons.B);
                Vector2 AimDirection = Guardian.CenterPosition + gamePadState.ThumbSticks.Right * 128f;
                Guardian.AimDirection = AimDirection.ToPoint();
            }
            oldGamePadState = gamePadState;
        }

        public static bool CheckForButtonPress(Buttons button)
        {
            return gamePadState.IsButtonDown(button) && oldGamePadState.IsButtonUp(button);
        }

        public override void Load()
        {
            orderCallButton = RegisterHotKey("Order Wheel Key", "Q");
            if (!Main.dedServ)
            {
                NinjaTextureBackup = Main.ninjaTexture;

                LosangleOfUnnown = GetTexture("Creatures/LosangleOfUnnown");
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
            }
            InitialGuardians.Add(ModContent.NPCType<GuardianNPC.List.RaccoonGuardian>());
            InitialGuardians.Add(ModContent.NPCType<GuardianNPC.List.WolfGuardian>());
            try
            {
                NExperienceMod = ModLoader.GetMod("NExperience");
                KalciphozMod = ModLoader.GetMod("kRPG");
            }
            catch
            {
                NExperienceMod = null;
                KalciphozMod = null;
            }
        }

        public static string[] GetItemInfo(Item i, TerraGuardian guardian)
        {
            List<string> Text = new List<string>();
            Text.Add(i.HoverName);
            if (i.damage > 0)
            {
                int DamageValue = i.damage;
                if (guardian.Active)
                {
                    DamageValue = (int)(DamageValue * guardian.MeleeDamageMultiplier);
                }
                string DamageType = " Neutral";
                if (i.melee)
                    DamageType = " Melee";
                if (i.ranged)
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

        public override void HandlePacket(System.IO.BinaryReader reader, int whoAmI)
        {
            NetMod.ReceiveMessage(reader, whoAmI);
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
            SpellBook
        }
	}
}
